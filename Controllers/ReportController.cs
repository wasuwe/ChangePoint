using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Change_Point.Models;
using Change_Point.Models.DTOs;
using Change_Point.Repositories.Implementations;
using Change_Point.Repositories.Interfaces;
using iText.Html2pdf;

namespace Change_Point.Controllers
{
    public class ReportController : Controller
    {
        private readonly ILookupRepository _lookupRepo = new LookupRepository();

        private string Role    => Session["ROLE"]?.ToString()     ?? "";
        private int    GroupId
        {
            get { int.TryParse(Session["GROUP_ID"]?.ToString(), out int g); return g; }
        }

        private const string ErrMsg = "พบปัญหา \n กรุณาติดต่อแอดมิน";

        // -----------------------------------------------------------------------
        // Views
        // -----------------------------------------------------------------------
        public ActionResult Index()
        {
            if (Session["EMP_NO"] == null)
            {
                Session["ROLE"]     = "V";
                Session["WC_CODE"]  = "";
                Session["GROUP_ID"] = "";
                return RedirectToAction("index", "Login");
            }
            return View();
        }

        public ActionResult Export()
        {
            if (Session["EMP_NO"] == null)
            {
                Session["ROLE"]     = "V";
                Session["WC_CODE"]  = "";
                Session["GROUP_ID"] = "";
                return RedirectToAction("index", "Login");
            }
            return View();
        }

        public ActionResult Summary()
        {
            if (Session["EMP_NO"] == null)
            {
                Session["ROLE"]     = "V";
                Session["WC_CODE"]  = "";
                Session["GROUP_ID"] = "";
                return RedirectToAction("index", "Login");
            }
            return View();
        }

        // -----------------------------------------------------------------------
        // Summary endpoints
        // -----------------------------------------------------------------------

        /// <summary>Count by type for a single day. date_select = dd-MM-yyyy</summary>
        public ActionResult Summary_day(string date_select)
        {
            try
            {
                var data = _lookupRepo.SummaryDay(date_select, GroupId);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>Count grouped by day (DD) within a date range. Dates in dd-MM-yyyy.</summary>
        public ActionResult Summary_month(string date_start, string date_stop)
        {
            try
            {
                var data = _lookupRepo.SummaryMonth(date_start, date_stop, GroupId, "DD");
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>Count grouped by month (MM) within a date range. Dates in dd-MM-yyyy.</summary>
        public ActionResult Summary_year(string date_start, string date_stop)
        {
            try
            {
                var data = _lookupRepo.SummaryMonth(date_start, date_stop, GroupId, "MM");
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>Count grouped by year (YYYY) within a date range. Dates in dd-MM-yyyy.</summary>
        public ActionResult Summary_years(string date_start, string date_stop)
        {
            try
            {
                var data = _lookupRepo.SummaryMonth(date_start, date_stop, GroupId, "YYYY");
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // -----------------------------------------------------------------------
        // Full report list with optional filters
        // -----------------------------------------------------------------------
        public ActionResult ListItem_ChangePoint_All(
            string date_start, string date_stop,
            string item_5m,
            string item_man,
            string item_mc,
            string item_mold,
            string item_process,
            string item_change,
            string item_edit)
        {
            try
            {
                var data = _lookupRepo.ReportList(
                    GroupId,
                    date_start   ?? "",
                    date_stop    ?? "",
                    item_5m      ?? "",
                    item_man     ?? "",
                    item_mc      ?? "",
                    item_mold    ?? "",
                    item_process ?? "",
                    item_change  ?? "",
                    item_edit    ?? "");

                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // -----------------------------------------------------------------------
        // CSV export
        // -----------------------------------------------------------------------
        public ActionResult export_data_CSV(
            string date_start, string date_stop,
            string item_5m, string item_man, string item_mc,
            string item_mold, string item_process, string item_change, string item_edit)
        {
            try
            {
                var records = _lookupRepo.ReportList(
                    GroupId,
                    date_start   ?? "",
                    date_stop    ?? "",
                    item_5m      ?? "",
                    item_man     ?? "",
                    item_mc      ?? "",
                    item_mold    ?? "",
                    item_process ?? "",
                    item_change  ?? "",
                    item_edit    ?? "");

                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string fileName  = date_start == date_stop
                    ? $"Report_Data_CP_{date_start.Replace("/", "-")}_{timestamp}.csv"
                    : $"Report_Data_CP_{date_start.Replace("/", "-")}_to_{date_stop.Replace("/", "-")}_{timestamp}.csv";

                string virtualPath = $"~/src/format/{fileName}";
                string physPath    = Server.MapPath(virtualPath);

                var sb = new StringBuilder();
                sb.AppendLine("DATE_CHANGE,STATUS_TYPE,MAN_SPOT_NAME_THA,MAN_INSTEAD_NAME_THA,PART_NO,MC_NO,MOLD_NO,PROCESS_POINT,EDIT_POINT,CHANGE,WARNINGS,REMARK");

                foreach (var r in records)
                {
                    sb.AppendLine(string.Join(",",
                        CsvEscape(r.DATE_CHANGE),
                        CsvEscape(r.STATUS_TYPE),
                        CsvEscape(r.MAN_SPOT_NAME_THA),
                        CsvEscape(r.MAN_INSTEAD_NAME_THA),
                        CsvEscape(r.PART_NO),
                        CsvEscape(r.MC_NO),
                        CsvEscape(r.MOLD_NO),
                        CsvEscape(r.PROCESS_POINT),
                        CsvEscape(r.EDIT_POINT),
                        CsvEscape(r.CHANGE),
                        CsvEscape(r.WARNINGS),
                        CsvEscape(r.REMARK)));
                }

                System.IO.File.WriteAllText(physPath, sb.ToString(), Encoding.UTF8);
                return Json(new { success = true, data = virtualPath, message = "สร้างไฟล์สำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult delete_export_data_file(string path_file)
        {
            try
            {
                string fullPath = Server.MapPath(path_file);
                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);
                return Json(new { success = true, message = "ลบไฟล์สำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // -----------------------------------------------------------------------
        private static string CsvEscape(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            if (s.Contains(',') || s.Contains('"') || s.Contains('\n'))
                return "\"" + s.Replace("\"", "\"\"") + "\"";
            return s;
        }
    }
}
