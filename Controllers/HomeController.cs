using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using Oracle.ManagedDataAccess.Client;
using System.Web.Services;
using System.Configuration;
//using Microsoft.Extensions.Logging;

using System.Globalization;

using Change_Point.Models;
using System.IO;

using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;
using System.Text;


using System.Net.Mail;

namespace Change_Point.Controllers
{
    public class HomeController : Controller
    {
        Home ARR = new Home();

        
        [WebMethod(EnableSession = true)]

        public ActionResult Index()
        {
            ARR.IsLayout = true;
            ARR.IsLogin = true;
            ARR.ROLE = Session["ROLE"];

            if (Session["EMP_NO"] == null)
            {
                Session["STATUS"] = "V";
                Session["ROLE"] = "V";
                Session["WC_CODE"] = "";
                Session["GROUP_ID"] = "";

                return RedirectToAction("index", "Login");
            }
                     
            ViewBag.Data = ARR;

            if (Session["WC_CODE"].ToString() == "3601" ||
                Session["WC_CODE"].ToString() == "7001" ||
                Session["WC_CODE"].ToString() == "7090" ||
                Session["WC_CODE"].ToString() == "7180" ||
                Session["WC_CODE"].ToString() == "7110"
                )
            {
                return View("~/Views/Home_ver1/Index.cshtml");
            }
            else
            {
                return View();
            }

            //return View();
        }

        public ActionResult  Detail(string Id)
        {
            ARR.IsLayout = true;
            ARR.IsLogin = true;
            ARR.param_date = Id;
            ARR.ROLE = Session["ROLE"];
            
            if (Session["EMP_NO"] == null)
            {
                Session["STATUS"] = "V";
                Session["ROLE"] = "V";
                Session["WC_CODE"] = "";
                Session["GROUP_ID"] = "";

                return RedirectToAction("index", "Login");
            }
         
            ViewBag.Data = ARR;

            if (Session["WC_CODE"].ToString() == "3601" ||
                    Session["WC_CODE"].ToString() == "7001" ||
                    Session["WC_CODE"].ToString() == "7090" ||
                    Session["WC_CODE"].ToString() == "7180" ||
                    Session["WC_CODE"].ToString() == "7110"
                    )
            {
                return View("~/Views/Home_ver1/Detail.cshtml");
            }
            else
            {
                return View();
            }

            //return View();
        }

        public ActionResult closing(Boolean IsLogin = false)
        {
            ARR.IsLayout = false;
            ARR.IsLogin = true;

            ViewBag.Data = ARR;
            return View();
        }

       
        public ActionResult Dashboard(string Id)
        {
            ARR.IsLayout = false;
            ARR.IsLogin = true;

            ARR.ROLE = "V";
            Session["STATUS"] = "V";
            Session["ROLE"] = "G";
            Session["WC_CODE"] = "";
            Session["GROUP_ID"] = Id;

            ViewBag.Data = ARR;

            if (Id == "21"
                )
            {
                return View("~/Views/Home_ver1/Dashboard.cshtml");
            }
            else
            {
                return View();
            }

            //return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string originalFileName = Path.GetFileName(file.FileName);
                    string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string newFileName = timestamp + "_" + originalFileName;
                    string path = Path.Combine(Server.MapPath("~/src/upload"), newFileName);
                    file.SaveAs(path);

                    return Json(new { success = true, responseText = "SUCCES", data = newFileName,  Message = "File Uploaded Successfully!!" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, responseText = "FAILED", data = "", Message = "ERROR:" + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                }
            else
            {
                return Json(new { success = false, responseText = "FAILED", data = "", Message = "You have not specified a file." }, JsonRequestBehavior.AllowGet);
            }
        }
        
        public class READFILE
        {
            public string PART_NO { get; set; }
            public string PART_NAME { get; set; }
            public string CHK { get; set; }
        }

        public ActionResult checkCSV(HttpPostedFileBase file)
        {
            List<READFILE> result = new List<READFILE>();

            string pattern = @"^[A-Za-z\d]{3}-[A-Za-z\d]{4}-[A-Za-z\d]{3}$"; // รูปแบบของข้อมูล: xxx-xxxx-xxx
            string isPass = "Y";

            // Upload File
            string originalFileName = Path.GetFileName(file.FileName);
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string newFileName = timestamp + "_" + originalFileName;
            //string newFileName = "Format_UploadPlan.csv";
            string path = Path.Combine(Server.MapPath("~/src/file_temp"), newFileName);
            file.SaveAs(path);

            DataTable csvData = new DataTable();

            try
            {
                Util csvManage = new Util();
                csvManage.toDatatable(csvData, path);
                DataColumnCollection col = csvData.Columns;

                //Step 1 Check column error
                if (!col.Contains("PART_NO") || !col.Contains("PART_NAME") )
                {
                    return Json(new { success = false, responseText = "FAILED", data = "", Message = "Format ที่ Upload ไม่ถูกต้อง" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //Step 2 Check input error
                    using (DataTableReader dtreader = csvData.CreateDataReader())
                    {
                        if (dtreader.HasRows)
                        {

                            while (dtreader.Read())
                            {

                                string part_no = dtreader["PART_NO"].ToString();
                                string part_nanme = dtreader["PART_NAME"].ToString();

                                result.Add(new READFILE
                                {
                                    PART_NO = dtreader["PART_NO"].ToString(),
                                    PART_NAME = dtreader["PART_NAME"].ToString(),
                                    CHK = Regex.IsMatch(part_no, pattern) && part_no != "" && part_nanme != "" ? "Y" : "N",

                                });

                                if (!Regex.IsMatch(part_no, pattern) || part_no == "" || part_nanme == "")
                                {
                                    isPass = "N";
                                }

                            }
                        }
                    }
                    return Json(new { is_pass = isPass, success = true, responseText = "SUCCES", data = result, Message = "อัพโหลดไฟล์สำเร็จ" }, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "FAILED", data = "", Message = "ERROR:" + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }

            finally
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
        }

        public ActionResult uploadCSV(HttpPostedFileBase file)
        {
            // Upload File
            string originalFileName = Path.GetFileName(file.FileName);
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string newFileName = timestamp + "_" + originalFileName;
            //string newFileName = "Format_UploadPlan.csv";
            string path = Path.Combine(Server.MapPath("~/src/file_temp"), newFileName);
            file.SaveAs(path);

            var gT = Session["GROUP_ID"].ToString();
            var emp_no = Session["EMP_NO"];

            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);
            OracleDataAdapter adp = new OracleDataAdapter();
            DataTable csvData = new DataTable();

            conn.Open();

            try
            {
                Util csvManage = new Util();
                csvManage.toDatatable(csvData, path);
                DataColumnCollection col = csvData.Columns;

                //Step 1 Check column error
                if (!col.Contains("PART_NO") || !col.Contains("PART_NAME") )
                {
                    return Json(new { success = false, responseText = "FAILED", data = "", Message = "Format ที่ Upload ไม่ถูกต้อง" }, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    OracleCommand cmd = new OracleCommand();
                    cmd = new OracleCommand("CP_SYSTEM.DELETE_PART_BY_GID", conn);
                    cmd.Parameters.Add(new OracleParameter("pGROUP_ID", OracleDbType.Int32)).Value = gT;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                    //Step 2 Check input error
                    using (DataTableReader dtreader = csvData.CreateDataReader())
                    {
                        if (dtreader.HasRows)
                        {

                            while (dtreader.Read())
                            {

                                string PART_NO = dtreader["PART_NO"].ToString();
                                string PART_NAME = dtreader["PART_NAME"].ToString();

                                cmd = new OracleCommand("CP_SYSTEM.ADD_PART", conn);
                                cmd.Parameters.Add(new OracleParameter("pITEM_PART", OracleDbType.Varchar2)).Value = PART_NO;
                                cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = PART_NAME;
                                cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                                cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = "Y";
                                cmd.Parameters.Add(new OracleParameter("pCREATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                                cmd.Parameters.Add(new OracleParameter("pCREATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                                cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                                cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.ExecuteNonQuery();

                            }
                        }
                    }
                    return Json(new { success = true, responseText = "SUCCES", Message = "อัพโหลดไฟล์สำเร็จ" }, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "FAILED", data = "", Message = "ERROR:" + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }

            finally
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                conn.Dispose();
                conn.Close();
            }
        }

        public class LIST_PART
        {
            public string PART_NO { get; set; }
            public string PART_NAME { get; set; }
            public string IS_USE { get; set; }
        }


    }
}


