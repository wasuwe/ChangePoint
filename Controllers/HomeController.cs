using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using Change_Point.Models;
using Change_Point.Repositories.Implementations;
using Change_Point.Repositories.Interfaces;

namespace Change_Point.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILookupRepository _lookupRepo = new LookupRepository();

        private string EmpNo  => Session["EMP_NO"]?.ToString() ?? "";
        private int    GroupId
        {
            get { int.TryParse(Session["GROUP_ID"]?.ToString(), out int g); return g; }
        }

        private const string ErrMsg = "พบปัญหา \n กรุณาติดต่อแอดมิน";

        // -----------------------------------------------------------------------
        [WebMethod(EnableSession = true)]
        public ActionResult Index()
        {
            if (Session["EMP_NO"] == null)
            {
                Session["STATUS"]    = "V";
                Session["ROLE"]      = "V";
                Session["WC_CODE"]   = "";
                Session["GROUP_ID"]  = "";
                return RedirectToAction("index", "Login");
            }
            return View();
        }

        public ActionResult Detail(string Id)
        {
            if (Session["EMP_NO"] == null)
            {
                Session["STATUS"]    = "V";
                Session["ROLE"]      = "V";
                Session["WC_CODE"]   = "";
                Session["GROUP_ID"]  = "";
                return RedirectToAction("index", "Login");
            }
            ViewBag.ParamDate = Id;
            return View();
        }

        public ActionResult closing(bool IsLogin = false)
        {
            return View();
        }

        public ActionResult Dashboard(string Id)
        {
            Session["STATUS"]    = "V";
            Session["ROLE"]      = "G";
            Session["WC_CODE"]   = "";
            Session["GROUP_ID"]  = Id;
            return View();
        }

        // -----------------------------------------------------------------------
        // File Upload (no DB involvement — just saves to disk)
        // -----------------------------------------------------------------------
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
                return Json(new { success = false, message = "You have not specified a file." }, JsonRequestBehavior.AllowGet);

            try
            {
                string originalFileName = Path.GetFileName(file.FileName);
                string timestamp        = DateTime.Now.ToString("yyyyMMddHHmmss");
                string newFileName      = timestamp + "_" + originalFileName;
                string path             = Path.Combine(Server.MapPath("~/src/upload"), newFileName);
                file.SaveAs(path);
                return Json(new { success = true, data = newFileName, message = "File Uploaded Successfully!!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // -----------------------------------------------------------------------
        // CSV check (validates rows before upload — no DB)
        // -----------------------------------------------------------------------
        public class READFILE
        {
            public string PART_NO   { get; set; }
            public string PART_NAME { get; set; }
            public string CHK       { get; set; }
        }

        public ActionResult checkCSV(HttpPostedFileBase file)
        {
            var result  = new List<READFILE>();
            string pattern = @"^[A-Za-z\d]{3}-[A-Za-z\d]{4}-[A-Za-z\d]{3}$";
            string isPass  = "Y";

            string originalFileName = Path.GetFileName(file.FileName);
            string timestamp        = DateTime.Now.ToString("yyyyMMddHHmmss");
            string newFileName      = timestamp + "_" + originalFileName;
            string path             = Path.Combine(Server.MapPath("~/src/file_temp"), newFileName);
            file.SaveAs(path);

            var csvData = new DataTable();
            try
            {
                var util = new Util();
                util.toDatatable(csvData, path);
                var col = csvData.Columns;

                if (!col.Contains("PART_NO") || !col.Contains("PART_NAME"))
                    return Json(new { success = false, message = "Format ที่ Upload ไม่ถูกต้อง" }, JsonRequestBehavior.AllowGet);

                using (var reader = csvData.CreateDataReader())
                {
                    while (reader.Read())
                    {
                        string partNo   = reader["PART_NO"].ToString();
                        string partName = reader["PART_NAME"].ToString();
                        bool   ok       = Regex.IsMatch(partNo, pattern) && partNo != "" && partName != "";
                        result.Add(new READFILE { PART_NO = partNo, PART_NAME = partName, CHK = ok ? "Y" : "N" });
                        if (!ok) isPass = "N";
                    }
                }
                return Json(new { is_pass = isPass, success = true, data = result, message = "อ่านไฟล์สำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }
        }

        // -----------------------------------------------------------------------
        // CSV import — replaces all parts for this group with rows from CSV
        // -----------------------------------------------------------------------
        public ActionResult uploadCSV(HttpPostedFileBase file)
        {
            string originalFileName = Path.GetFileName(file.FileName);
            string timestamp        = DateTime.Now.ToString("yyyyMMddHHmmss");
            string newFileName      = timestamp + "_" + originalFileName;
            string path             = Path.Combine(Server.MapPath("~/src/file_temp"), newFileName);
            file.SaveAs(path);

            var csvData = new DataTable();
            try
            {
                var util = new Util();
                util.toDatatable(csvData, path);
                var col = csvData.Columns;

                if (!col.Contains("PART_NO") || !col.Contains("PART_NAME"))
                    return Json(new { success = false, message = "Format ที่ Upload ไม่ถูกต้อง" }, JsonRequestBehavior.AllowGet);

                // Delete all existing parts for this group, then re-insert from CSV
                using (var reader = csvData.CreateDataReader())
                {
                    // First pass: collect all rows
                    var rows = new List<(string partNo, string partName)>();
                    while (reader.Read())
                    {
                        string partNo   = reader["PART_NO"].ToString();
                        string partName = reader["PART_NAME"].ToString();
                        if (!string.IsNullOrWhiteSpace(partNo))
                            rows.Add((partNo, partName));
                    }

                    // Delete existing and re-add
                    foreach (var (partNo, _) in rows)
                        _lookupRepo.DeletePart(partNo, GroupId);

                    foreach (var (partNo, partName) in rows)
                        _lookupRepo.AddPart(partNo, partName, GroupId, true, EmpNo);
                }

                return Json(new { success = true, message = "อัพโหลดไฟล์สำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }
        }
    }
}
