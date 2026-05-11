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

//using IronPdf;

namespace Change_Point.Controllers
{
    public class Home_ver1Controller : Controller
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

                Response.Redirect(Url.Action("index", "Login"));
            }

         

            //string FileName = "Sample";
            //string extFile = ".xlsx";
            //string path = Path.Combine(Server.MapPath("~/src/upload"), FileName + extFile);

            ViewBag.Data = ARR;

            return View();
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
                Response.Redirect(Url.Action("index", "Login"));
            }


         
            ViewBag.Data = ARR;

            return View();
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

            return View();
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

        //[HttpPost]
        //public ActionResult FileUpload(HttpPostedFileBase file)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (file == null)
        //        {
        //            ModelState.AddModelError("File", "Please Upload Your file");
        //        }
        //        else if (file.ContentLength > 0)
        //        {
        //            int MaxContentLength = 1024 * 1024 * 3; //3 MB
        //            string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf" };

        //            if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
        //            {
        //                ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
        //            }

        //            else if (file.ContentLength > MaxContentLength)
        //            {
        //                ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
        //            }
        //            else
        //            {
        //                //TO:DO
        //                var fileName = Path.GetFileName(file.FileName);
        //                var path = Path.Combine(Server.MapPath("~/Content/Upload"), fileName);
        //                file.SaveAs(path);
        //                ModelState.Clear();
        //                ViewBag.Message = "File uploaded successfully";
        //            }
        //        }
        //    }
        //    return View();
        //}

        //public ActionResult Upload()
        //{
        //    ARR.IsLayout = true;
        //    ARR.IsLogin = true;

        //    if (Session["EMP_NO"] == null)
        //    {
        //        Session["STATUS"] = "V";
        //        Session["ROLE"] = "V";
        //        Response.Redirect(Url.Action("index", "Login"));
        //    }

        //    ViewBag.Data = ARR;

        //    return View();
        //}

        //public ActionResult Upload1()
        //{
        //    ARR.IsLayout = true;
        //    ARR.IsLogin = true;

        //    if (Session["EMP_NO"] == null)
        //    {
        //        Session["STATUS"] = "V";
        //        Session["ROLE"] = "V";
        //        Response.Redirect(Url.Action("index", "Login"));
        //    }

        //    ViewBag.Data = ARR;

        //    return View();
        //}

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

        //public ActionResult exportCSV(List<LIST_PART> result)
        //{
        //    string wf = System.Web.HttpContext.Current.Server.MapPath("~/src/format/Plan_format.csv");
        //    StringBuilder sb = new StringBuilder();
        //    var gT = Session["GROUP_ID"].ToString();

        //    try
        //    {

        //        // Excel

        //        sb.AppendLine(string.Join(",", "PART_NO", "PART_NAME"));


        //        //List<List_Plan> result

        //        var record_csv = result.Where(s => s.IS_USE != "Y").Select(s => new
        //        {
        //            PART_NO = s.PART_NO,
        //            PART_NAME = s.PART_NAME,
        //        }).ToList();

        //        foreach (DataRow r in Util.ToDataTable(record_csv).Rows)
        //        {
        //            IEnumerable<string> fields = r.ItemArray.Select(field => field.ToString());
        //            sb.AppendLine(string.Join(",", fields));
        //        }

        //        return Json(new { success = true, responseText = "SUCCES", Message = "สร้างไฟล์สำเร็จ" }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, responseText = "FAILED", data = "", Message = "ERROR:" + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
        //    }
        //    finally
        //    {
        //        System.IO.File.WriteAllText(wf, sb.ToString(), Encoding.UTF8);
        //    }
        //}

        //public static void export(List<List_Plan> result, string search_dept, string search_date, string search_date_to, string search_mc, string search_status, string search_cell, string search_size, string search_zone, string search_shift)
        //{

        //    Document document = new Document(PageSize.A4, 40, 40, 10, 10);
        //    //document.SetPageSize(iTextSharp.text.PageSize.A4);
        //    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(HttpContext.Current.Server.MapPath("~/Format/_Plan_format.pdf"), FileMode.Create));
        //    document.Open();
        //    document.NewPage();

        //    try
        //    {

        //        Font FONT_TITLE = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
        //        Font Header_font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);
        //        Font Header_detail_font = new Font(Font.FontFamily.HELVETICA, 8, Font.UNDERLINE);
        //        Font Header_detail_font_date = new Font(Font.FontFamily.HELVETICA, 8);
        //        Font Column_font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 7);
        //        Font Remark_font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 4);
        //        Font Row_font = FontFactory.GetFont(FontFactory.HELVETICA, 8);
        //        Font Row_font_small = FontFactory.GetFont(FontFactory.HELVETICA, 7);
        //        Font Row_font_bold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8);
        //        BaseFont Angsana_New = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/fonts/") + "angsa.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        //        Font FONT_THAI = new Font(Angsana_New, 10);
        //        BaseFont Arial = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/fonts/") + "arialbd_3.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        //        Font FONT_ARIAL_SM = new Font(Arial, 9);
        //        Font FONT_ARIAL = new Font(Arial, 10);
        //        Font FONT_ARIAL_LL = new Font(Arial, 12);
        //        //Header



        //        var date = search_date.Substring(6, 2) + "/" + search_date.Substring(4, 2) + "/" + search_date.Substring(2, 2);
        //        var date_to = search_date_to.Substring(6, 2) + "/" + search_date_to.Substring(4, 2) + "/" + search_date_to.Substring(2, 2);

        //        string head_dept = "", head_mc = "", head_status = "", head_cell = "", head_size = "", head_zone = "", head_shift = "";

        //        int column_header = 4;

        //        if (!string.IsNullOrEmpty(search_dept))
        //        {
        //            head_dept = "Dept :";
        //            search_dept = "Mold " + search_dept + " ";
        //            column_header += 2;
        //        }

        //        if (!string.IsNullOrEmpty(search_mc))
        //        {
        //            head_mc = "M/C :";
        //            search_mc = search_mc + " ";
        //            column_header += 2;
        //        }

        //        if (!string.IsNullOrEmpty(search_status))
        //        {
        //            head_status = "Status :";
        //            search_status = search_status + " ";
        //            column_header += 2;
        //        }

        //        if (!string.IsNullOrEmpty(search_cell))
        //        {
        //            head_cell = search_cell.Substring(0, 4) + " :";
        //            search_cell = search_cell.Substring(5, search_cell.Length - 5) + " ";
        //            column_header += 2;
        //        }

        //        if (!string.IsNullOrEmpty(search_size))
        //        {
        //            head_size = "Size :";
        //            search_size = search_size + " ";
        //            column_header += 2;
        //        }

        //        if (!string.IsNullOrEmpty(search_zone))
        //        {
        //            head_zone = "Zone :";
        //            search_zone = search_zone + " ";
        //            column_header += 2;
        //        }

        //        if (!string.IsNullOrEmpty(search_shift))
        //        {
        //            head_shift = "Shift :";
        //            var shift_name = search_shift == "1" ? "Day" : "Night";
        //            search_shift = shift_name + " ";
        //            column_header += 2;
        //        }
        //        else
        //        {
        //            head_shift = "Shift :";
        //            var shift_name = "Day + Night";
        //            search_shift = shift_name + " ";
        //            column_header += 2;
        //        }

        //        //var str_header = "Production Record      " + search_dept + date + search_shift + search_mc + search_size + search_zone + search_cell + search_status;

        //        // PDF
        //        PdfPTable header = new PdfPTable(column_header);
        //        header.WidthPercentage = 115.0f;
        //        header.DefaultCell.VerticalAlignment = Element.ALIGN_LEFT;
        //        //header.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        header.DefaultCell.Border = Rectangle.NO_BORDER;
        //        var title = new PdfPCell(new Phrase("Production Record", FONT_TITLE));
        //        title.Colspan = 2;
        //        title.Border = Rectangle.NO_BORDER;
        //        header.AddCell(title);

        //        if (!string.IsNullOrEmpty(search_dept))
        //        {
        //            header.AddCell(new Phrase(head_dept, Header_font));
        //            header.AddCell(new Phrase(search_dept, Header_detail_font));
        //        }

        //        header.AddCell(new Phrase("Date :", Header_font));
        //        header.AddCell(new Phrase(date + " to " + date_to, Header_detail_font_date));

        //        if (!string.IsNullOrEmpty(search_shift))
        //        {
        //            header.AddCell(new Phrase(head_shift, Header_font));
        //            header.AddCell(new Phrase(search_shift, Header_detail_font));
        //        }

        //        if (!string.IsNullOrEmpty(search_mc))
        //        {
        //            header.AddCell(new Phrase(head_mc, Header_font));
        //            header.AddCell(new Phrase(search_mc, Header_detail_font));
        //        }

        //        if (!string.IsNullOrEmpty(search_size))
        //        {
        //            header.AddCell(new Phrase(head_size, Header_font));
        //            header.AddCell(new Phrase(search_size, Header_detail_font));
        //        }

        //        if (!string.IsNullOrEmpty(search_zone))
        //        {
        //            header.AddCell(new Phrase(head_zone, Header_font));
        //            header.AddCell(new Phrase(search_zone, Header_detail_font));
        //        }

        //        if (!string.IsNullOrEmpty(search_cell))
        //        {
        //            header.AddCell(new Phrase(head_cell, Header_font));
        //            header.AddCell(new Phrase(search_cell, Header_detail_font));
        //        }

        //        if (!string.IsNullOrEmpty(search_status))
        //        {
        //            header.AddCell(new Phrase(head_status, Header_font));
        //            header.AddCell(new Phrase(search_status, Header_detail_font));
        //        }

        //        PdfPTable table = new PdfPTable(16);

        //        table.WidthPercentage = 115.0f;
        //        table.HorizontalAlignment = Element.ALIGN_CENTER;
        //        table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        table.HeaderRows = 1;

        //        // Col_name

        //        table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        table.DefaultCell.BackgroundColor = BaseColor.LIGHT_GRAY;
        //        table.DefaultCell.FixedHeight = 21f;
        //        table.SetWidths(new float[] { 5f, 3f, 14f, 4f, 9f, 7f, 4f, 7f, 7f, 6f, 7f, 7f, 6f, 6f, 7f, 11f });
        //        table.AddCell(new Phrase("M/C", Column_font));
        //        table.AddCell(new Phrase("No", Column_font));
        //        table.AddCell(new Phrase("Part No.", Column_font));
        //        table.AddCell(new Phrase("CAV", Column_font));
        //        table.AddCell(new Phrase("Mat'l", Column_font));
        //        table.AddCell(new Phrase("CT Plan", Column_font));
        //        table.AddCell(new Phrase("CT Act.", Column_font));
        //        table.AddCell(new Phrase("Plan QT'Y", Column_font));
        //        table.AddCell(new Phrase("Act QT'Y", Column_font));
        //        table.AddCell(new Phrase("NG QT'Y", Column_font));
        //        table.AddCell(new Phrase("Plan Start", Column_font));
        //        table.AddCell(new Phrase("Plan Stop", Column_font));
        //        table.AddCell(new Phrase("Act. Start", Column_font));
        //        table.AddCell(new Phrase("Act. Stop", Column_font));
        //        table.AddCell(new Phrase("Loss", Column_font));
        //        table.AddCell(new Phrase("Remark", Column_font));



        //        // Row

        //        string temp_bg_mc = "";

        //        var record = result.Select(s => new
        //        {
        //            PLAN_DATE = s.PLAN_DATE,
        //            PLAN_SHIFT = s.PLAN_SHIFT,
        //            PLAN_MACHINE = s.PLAN_MC,
        //            PLAN_PRIORITY = s.PLAN_PRIOIRTY,
        //            PLAN_PART = s.PLAN_PART,
        //            PLAN_QTY = s.PLAN_QTY,
        //            PLAN_CT = s.PLAN_CT,
        //            PLAN_STATUS = s.PLAN_STATUS,
        //            PLAN_START = s.PLAN_START,
        //            PLAN_STOP = s.PLAN_STOP,
        //            CAV = s.CAV,
        //            ACTUAL_CT = s.ACTUAL_CT,
        //            ACTUAL_QTY = s.ACTUAL_QTY,
        //            ACTUAL_NG = s.ACTUAL_NG,
        //            ACTUAL_START = s.FLAG != "1" ? "" : s.ACTUAL_START,
        //            ACTUAL_STOP = s.FLAG != "1" ? "" : s.ACTUAL_STOP,
        //            UPDATE_BY = s.UPDATE_BY,
        //            LAST_UPDATE = s.LAST_UPDATE,
        //            PLAN_ID = s.PLAN_ID,
        //            REMARK = s.REMARK.Replace(",", " "),
        //            PART_LINK = s.PLAN_PART.Length >= 8 ? s.PLAN_PART.Substring(0, 8) + "-000" : s.PLAN_PART,
        //            //MAT_1 = s.MATERIAL_1,
        //            //MAT_2 = s.MATERIAL_2
        //            FAM = s.FM,
        //            MAT = s.MATERIAL_1,
        //            FLAG = s.FLAG,
        //            PLAN_COMMENT = s.PLAN_COMMENT
        //        }).ToList();


        //        foreach (DataRow r in Util.ToDataTable(record).Rows)
        //        {
        //            string machine = r[2].ToString();
        //            string priority = r[3].ToString();
        //            string plan_shift = r[1].ToString() == "1" ? "Day" : "Night";
        //            string part = r[4].ToString();
        //            string status = r[7].ToString();
        //            Decimal qty = Convert.ToDecimal(r[5]);
        //            Double ct = Convert.ToDouble(r[6]);
        //            string start = r[8].ToString();
        //            string stop = r[9].ToString();
        //            string cav = r[10].ToString();
        //            Double act_ct = Convert.ToDouble(r[11]);
        //            Decimal act_qty = Convert.ToDecimal(r[12]);
        //            string act_ng = r[13].ToString() == "0" ? "" : r[13].ToString();
        //            string act_start = r[14].ToString();
        //            string act_stop = r[15].ToString();
        //            string plan_id = r[18].ToString();
        //            string remark = r[19].ToString();
        //            string mat_1 = r[22].ToString();
        //            //string mat_2 = r[21].ToString();
        //            string type_fm = r[21].ToString();
        //            string flag = r[23].ToString();
        //            string plan_comment = r[24].ToString();

        //            if (type_fm != "1")
        //            {
        //                string family = "";
        //                if (part != "NOPLAN")
        //                {
        //                    family = Util.get_family(part.Substring(0, 8) + "-000");
        //                }
        //                string loss = Util.get_Loss(plan_id);

        //                table.DefaultCell.BackgroundColor = BaseColor.WHITE;

        //                if (machine != temp_bg_mc)
        //                {
        //                    table.DefaultCell.FixedHeight = 20f;
        //                    for (var x = 0; x < 16; x++)
        //                    {
        //                        table.AddCell(new Phrase("", Row_font));
        //                    }
        //                }

        //                table.DefaultCell.FixedHeight = 38f;
        //                table.AddCell(new Phrase(flag == "2" ? "Next shift" : machine != temp_bg_mc ? machine : "", Row_font));
        //                table.AddCell(new Phrase(flag == "2" ? "" : priority, Row_font));
        //                table.AddCell(new Phrase(flag == "2" && part == "NOPLAN" ? "" : part + "\n" + family, FONT_ARIAL));
        //                table.AddCell(new Phrase(cav, Row_font));
        //                table.AddCell(new Phrase(mat_1, Row_font_small));
        //                //table.AddCell(new Phrase(mat_2, Row_font_detail));
        //                table.AddCell(new Phrase(ct == 0.0 ? "" : string.Format("{0:n1}", ct), FONT_ARIAL_LL));
        //                table.AddCell(new Phrase(act_ct == 0.0 ? "" : string.Format("{0:n1}", act_ct), Row_font));
        //                table.AddCell(new Phrase(qty == 0 ? "" : status == "MT1" || status == "TT" ? "TEST \n" + qty + " Min" : string.Format("{0:n0}", qty), FONT_ARIAL_LL));
        //                table.AddCell(new Phrase(act_qty == 0 ? "" : string.Format("{0:n0}", act_qty), FONT_ARIAL_LL));
        //                table.AddCell(new Phrase(act_ng, FONT_ARIAL));
        //                table.AddCell(new Phrase(start, FONT_ARIAL_LL));
        //                table.AddCell(new Phrase(stop, FONT_ARIAL_LL));
        //                table.AddCell(new Phrase(act_start, FONT_ARIAL_SM));
        //                table.AddCell(new Phrase(act_stop, FONT_ARIAL_SM));

        //                var text_left = new PdfPCell(new Phrase(loss, Row_font_small));
        //                text_left.HorizontalAlignment = Element.ALIGN_LEFT;
        //                table.AddCell(text_left);


        //                var planner_comment = plan_comment.Length > 0 ? "P : " + plan_comment : "";
        //                var actual_comment = remark.Length > 0 ? "Act : " + remark : "";


        //                table.AddCell(new Phrase(planner_comment + "\n" + actual_comment, FONT_THAI));



        //            }




        //            temp_bg_mc = machine;
        //        }

        //        for (var x = 0; x < 16; x++)
        //        {
        //            table.DefaultCell.FixedHeight = 20f;
        //            table.AddCell(new Phrase("", Row_font));
        //        }

        //        PdfPTable footer = new PdfPTable(1);
        //        footer.WidthPercentage = 105.0f;
        //        footer.DefaultCell.VerticalAlignment = Element.ALIGN_LEFT;
        //        footer.DefaultCell.Border = Rectangle.NO_BORDER;
        //        footer.AddCell(new Phrase("\n Comment \n \n", Column_font));
        //        footer.AddCell(new Phrase("................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................. \n \n \n", Remark_font));
        //        footer.AddCell(new Phrase("................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................. \n \n \n", Remark_font));
        //        footer.AddCell(new Phrase("................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................. \n \n \n", Remark_font));
        //        //footer.AddCell(new Phrase("........................................................................................................................................................................................................................................................................................ \n \n", Column_font));

        //        var foot_right = new PdfPCell(new Phrase("Print Date : " + DateTime.Now.ToString("dd MMM yyyy HH:mm"), Column_font));
        //        foot_right.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        foot_right.Border = Rectangle.NO_BORDER;
        //        footer.AddCell(foot_right);
        //        footer.FooterRows = 1;

        //        document.Add(header);
        //        document.Add(new Paragraph("\n"));
        //        document.Add(table);
        //        document.Add(footer);
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Web.HttpContext.Current.Response.Write("<script language='javascript'>alert('Report : " + HttpContext.Current.Server.HtmlEncode(ex.Message) + "')</script>");
        //    }
        //    finally
        //    {
        //        document.Close();
        //    }
        //}


        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Export_Detail(string GridHtml)
        //{
        //    using (MemoryStream stream = new System.IO.MemoryStream())
        //    {
        //        StringReader sr = new StringReader(GridHtml);
        //        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 50f, 0f);

        //        // กำหนดเส้นทางของไฟล์ PDF ที่ต้องการบันทึก
        //        string filePath = Server.MapPath("~/src/format/ex.pdf");
        //        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));

        //        // กำหนดฟอนต์ที่รองรับภาษาไทย (เช่น "THSarabunNew")
        //        BaseFont bf = BaseFont.CreateFont(Server.MapPath("~/fonts/") + "angsa.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        //        Font thaiFont = new Font(bf, 12);

        //        pdfDoc.Open();

        //        // ใช้ XMLWorkerHelper พร้อมกับฟอนต์ที่รองรับภาษาไทย
        //        //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr, Encoding.UTF8, new UnicodeFontFactory(thaiFont));
        //        File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        //        pdfDoc.Close();


        //        return Json(new { success = true, responseText = "SUCCESS", Message = "อัพโหลดไฟล์สำเร็จ" }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Export_Detail(string GridHtml)
        //{
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        // สร้างไฟล์ PDF ใน MemoryStream
        //        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 50f, 0f);
        //        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
        //        pdfDoc.Open();

        //        // กำหนดฟอนต์ที่รองรับภาษาไทย (เช่น "THSarabunNew")
        //        BaseFont bf = BaseFont.CreateFont(Server.MapPath("~/fonts/") + "angsa.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        //        Font thaiFont = new Font(bf, 12);

        //        // แปลง HTML เป็นไฟล์ PDF
        //        using (TextReader sr = new StringReader(GridHtml))
        //        {
        //            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr, Encoding.UTF8, new UnicodeFontFactory(thaiFont));
        //        }

        //        pdfDoc.Close();

        //        // บันทึกไฟล์ PDF ลงบนเซิร์ฟเวอร์
        //        string filePath = Server.MapPath("~/src/format/ex.pdf");
        //        System.IO.File.WriteAllBytes(filePath, stream.ToArray());

        //        return Json(new { success = true, responseText = "SUCCESS", Message = "อัพโหลดไฟล์สำเร็จ" }, JsonRequestBehavior.AllowGet);
        //    }
        //}



        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Export_Detail(string GridHtml)
        //{

        //    try
        //    {
        //        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        //        string newFileName = "Change Point Detail_" + timestamp + ".pdf";
        //        string path = Path.Combine(Server.MapPath("~/src/format"), newFileName);

        //        var renderer = new ChromePdfRenderer();
        //        var pdf = renderer.RenderHtmlAsPdf(GridHtml);
        //        pdf.SaveAs(path);


        //        return Json(new { success = true, data = path, responseText = "SUCCESS", Message = "สร้าง PDF สำเร็จ" }, JsonRequestBehavior.AllowGet);

        //    }
        //    catch
        //    {
        //        return Json(new { success = false, data = "", responseText = "FAILD", Message = "สร้าง PDF ไม่สำเร็จ" }, JsonRequestBehavior.AllowGet);

        //    }
        //    finally
        //    {

        //    }

        //    }


    }
}


