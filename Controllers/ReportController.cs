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
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;
using System.Text;

using Change_Point.Models;
using iText.Html2pdf;

namespace Change_Point.Controllers
{
    public class ReportController : Controller
    {

        Home ARR = new Home();

        public string err_msg = "พบปัญหา \n กรุณาติดต่อแอดมิน";

        public class SUMMARY_DAY
        {
            public string TYPE { get; set; }
            public string COUNT { get; set; }
        }
        public class SUMMARY_MONTH
        {
            public string DATE { get; set; }
            public string TYPE { get; set; }
            public string COUNT { get; set; }
        }
        public class CHANGE_POINT
        {
            public string ID { get; set; }
            public string SHIFT { get; set; }
            public string DATE_CHANGE { get; set; }
            public string MAN_SPOT { get; set; }
            public string MAN_INSTEAD { get; set; }
            public string MC_NO { get; set; }
            public string EDIT_POINT { get; set; }
            public string PART_NO { get; set; }
            public string MOLD_NO { get; set; }
            public string CHANGE { get; set; }
            public string PROCESS_POINT { get; set; }
            public string DETAILS { get; set; }
            public string ACTION { get; set; }
            public string WARNINGS { get; set; }
            public string STATUS_TYPE { get; set; }
            public string MAN_SPOT_NAME_ENG { get; set; }
            public string MAN_SPOT_NAME_THA { get; set; }
            public string MAN_INSTEAD_NAME_ENG { get; set; }
            public string MAN_INSTEAD_NAME_THA { get; set; }
            public string CREATE_BY { get; set; }
            public string CREATE_DATE { get; set; }
            public string UPDATE_BY { get; set; }
            public string UPDATE_DATE { get; set; }
            public string GNAME_ENG { get; set; }
            public string FNAME_ENG { get; set; }
            public string REMARK { get; set; }
            public string INFORMED { get; set; }
            public string RECIPIENT_ID { get; set; }
        }
        // GET: Report
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

            return View();
        }
        public ActionResult Export()
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

            return View();
        }
        public ActionResult Summary()
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

            return View();
        }
        public ActionResult Summary_day(string date_select)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<SUMMARY_DAY> result = new List<SUMMARY_DAY>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT  STATUS_TYPE, COUNT(ID) AS SUMMARY " +
                    "FROM CP_CALENDAR " +
                    "WHERE DATE_CHANGE >= to_timestamp('" + date_select + " 00:00:00', 'dd-mm-yyyy hh24:mi:ss') and " +
                    "DATE_CHANGE <= to_timestamp('" + date_select + " 23:59:59', 'dd-mm-yyyy hh24:mi:ss') " +
                    "AND GROUP_ID = '" + gT + "'" +
                    "GROUP BY STATUS_TYPE";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;

                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        while (dtreader.Read())
                        {
                            result.Add(new SUMMARY_DAY
                            {
                                TYPE = dtreader["STATUS_TYPE"].ToString(),
                                COUNT = dtreader["SUMMARY"].ToString(),

                            });
                        }
                        return Json(new { data = result, success = true, responseText = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }

        }
        public ActionResult Summary_month(string date_start, string date_stop)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<SUMMARY_MONTH> result = new List<SUMMARY_MONTH>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT STATUS_TYPE, count(ID) as SUMMARY, to_char(DATE_CHANGE, 'DD') as DATES " +
                    "FROM CP_CALENDAR " +
                    "WHERE DATE_CHANGE >= to_timestamp('" + date_start + " 00:00:00', 'dd-mm-yyyy hh24:mi:ss') and " +
                    "DATE_CHANGE <= to_timestamp('" + date_stop + " 23:59:59', 'dd-mm-yyyy hh24:mi:ss') " +
                    "AND GROUP_ID = '" + gT + "'" +
                    "GROUP BY STATUS_TYPE, to_char(DATE_CHANGE, 'DD') " +
                    "ORDER BY to_char(DATE_CHANGE, 'DD') ASC ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;

                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        while (dtreader.Read())
                        {
                            result.Add(new SUMMARY_MONTH
                            {
                                DATE = dtreader["DATES"].ToString(),
                                TYPE = dtreader["STATUS_TYPE"].ToString(),
                                COUNT = dtreader["SUMMARY"].ToString(),
                            });
                        }
                        return Json(new { data = result, success = true, responseText = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }

        }
        public ActionResult Summary_year(string date_start, string date_stop)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<SUMMARY_MONTH> result = new List<SUMMARY_MONTH>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT STATUS_TYPE, count(ID) as SUMMARY, to_char(DATE_CHANGE, 'MM') as DATES " +
                    "FROM CP_CALENDAR " +
                    "WHERE DATE_CHANGE >= to_timestamp('" + date_start + " 00:00:00', 'dd-mm-yyyy hh24:mi:ss') and " +
                    "DATE_CHANGE <= to_timestamp('" + date_stop + " 23:59:59', 'dd-mm-yyyy hh24:mi:ss') " +
                    "AND GROUP_ID = '" + gT + "'" +
                    "GROUP BY STATUS_TYPE, to_char(DATE_CHANGE, 'MM') " +
                    "ORDER BY to_char(DATE_CHANGE, 'MM') ASC ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;

                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        while (dtreader.Read())
                        {
                            result.Add(new SUMMARY_MONTH
                            {
                                DATE = dtreader["DATES"].ToString(),
                                TYPE = dtreader["STATUS_TYPE"].ToString(),
                                COUNT = dtreader["SUMMARY"].ToString(),
                            });
                        }
                        return Json(new { data = result, success = true, responseText = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }

        }
        public ActionResult Summary_years(string date_start, string date_stop)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<SUMMARY_MONTH> result = new List<SUMMARY_MONTH>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT STATUS_TYPE, count(ID) as SUMMARY, to_char(DATE_CHANGE, 'yyyy') as DATES " +
                    "FROM CP_CALENDAR " +
                    "WHERE DATE_CHANGE >= to_timestamp('" + date_start + " 00:00:00', 'dd-mm-yyyy hh24:mi:ss') and " +
                    "DATE_CHANGE <= to_timestamp('" + date_stop + " 23:59:59', 'dd-mm-yyyy hh24:mi:ss') " +
                    "AND GROUP_ID = '" + gT + "'" +
                    "GROUP BY STATUS_TYPE, to_char(DATE_CHANGE, 'yyyy') " +
                    "ORDER BY to_char(DATE_CHANGE, 'yyyy') ASC ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;

                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        while (dtreader.Read())
                        {
                            result.Add(new SUMMARY_MONTH
                            {
                                DATE = dtreader["DATES"].ToString(),
                                TYPE = dtreader["STATUS_TYPE"].ToString(),
                                COUNT = dtreader["SUMMARY"].ToString(),
                            });
                        }
                        return Json(new { data = result, success = true, responseText = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }

        }
        public ActionResult ListItem_ChangePoint_All(
            string date_start, string date_stop,
            string item_5m,
            string item_man,
            string item_mc,
            string item_mold,
            string item_process,
            string item_change,
            string item_edit
            )
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<CHANGE_POINT> result = new List<CHANGE_POINT>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT CP_CALENDAR.* FROM CP_CALENDAR " +
                    "WHERE STATUS_TYPE <> 'hide' AND GROUP_ID = '" + gT + "' AND " +
                    "DATE_CHANGE >= to_timestamp('" + date_start + " 00:00:00', 'dd-mm-yyyy hh24:mi:ss') AND " +
                    "DATE_CHANGE <= to_timestamp('" + date_stop + " 23:59:59', 'dd-mm-yyyy hh24:mi:ss') ";

                command += item_5m != "" ? " AND STATUS_TYPE in (" + item_5m + ") " : "";
                command += item_mc != "" ? " AND MC_NO in (" + item_mc + ") " : "";
                command += item_mold != "" ? " AND MOLD_NO in (" + item_mold + ") " : "";
                command += item_process != "" ? " AND PROCESS_POINT in (" + item_process + ") " : "";
                command += item_change != "" ? " AND CHANGE in (" + item_change + ") " : "";
                command += item_edit != "" ? " AND EDIT_POINT in (" + item_edit + ") " : "";
                command += item_man != "" ? " AND (MAN_SPOT in (" + item_man + ") OR MAN_INSTEAD in (" + item_man + ")) " : "";
                command += "ORDER BY DATE_CHANGE ASC";


               cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;

                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        while (dtreader.Read())
                        {

                            DateTime date_change = Convert.ToDateTime(dtreader["DATE_CHANGE"]);

                            result.Add(new CHANGE_POINT
                            {
                                ID = dtreader["ID"].ToString(),
                                SHIFT = dtreader["SHIFT"].ToString(),
                                DATE_CHANGE = date_change.ToString("dd/MM/yyyy"),
                                MAN_SPOT = dtreader["MAN_SPOT"].ToString(),
                                MAN_INSTEAD = dtreader["MAN_INSTEAD"].ToString(),
                                MC_NO = dtreader["MC_NO"].ToString(),
                                EDIT_POINT = dtreader["EDIT_POINT"].ToString(),
                                PART_NO = dtreader["PART_NO"].ToString(),
                                MOLD_NO = dtreader["MOLD_NO"].ToString(),
                                CHANGE = dtreader["CHANGE"].ToString(),
                                PROCESS_POINT = dtreader["PROCESS_POINT"].ToString(),
                                DETAILS = dtreader["ACTION"].ToString(),
                                WARNINGS = dtreader["WARNINGS"].ToString(),
                                STATUS_TYPE = dtreader["STATUS_TYPE"].ToString(),
                                REMARK = dtreader["REMARK"].ToString(),

                                MAN_SPOT_NAME_ENG = dtreader["MAN_SPOT_NAME_ENG"].ToString(),
                                MAN_SPOT_NAME_THA = dtreader["MAN_SPOT_NAME_THA"].ToString(),
                                MAN_INSTEAD_NAME_ENG = dtreader["MAN_INSTEAD_NAME_ENG"].ToString(),
                                MAN_INSTEAD_NAME_THA = dtreader["MAN_INSTEAD_NAME_THA"].ToString(),

                            });
                        }
                        return Json(new { data = result, success = true, responseText = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }

        }
        public ActionResult export_data_CSV(List<CHANGE_POINT> result, string date_start, string date_stop)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var path_file = "~/src/format/" + (date_start == date_stop ? "Report Data Change Point " + date_start.Replace("/", "-") + " " + timestamp + ".csv" : "Report Data Change Point " + date_start.Replace("/", "-") + " to " + date_stop.Replace("/", "-") + " " + timestamp + ".csv");

            string path = System.Web.HttpContext.Current.Server.MapPath(path_file);
            StringBuilder sb = new StringBuilder();
            var gT = Session["GROUP_ID"].ToString();
            string[] arr_type = { "", "Man", "Machine", "Mold/DIE", "Method", "Part/Mat'l"};
            try
            {
                // Excel


                sb.AppendLine(string.Join(","
                    , "DATE_CHANGE"
                    , "STATUS_TYPE"
                    , "MAN_SPOT_NAME_THA"
                    , "MAN_INSTEAD_NAME_THA"
                    , "PART_NO"
                    , "MC_NO"
                    , "MOLD_NO"
                    , "PROCESS_POINT"
                    , "EDIT_POINT"
                    , "CHANGE"
                    , "DETAILS"
                    , "WARNINGS"
                    , "REMARK"
                    ));
                
                var record_csv = result.Where(s => s.DATE_CHANGE != "").Select(s => new
                {
                    DATE_CHANGE = s.DATE_CHANGE,
                    STATUS_TYPE = arr_type[Convert.ToInt64(s.STATUS_TYPE)],
                    MAN_SPOT_NAME_THA = s.MAN_SPOT_NAME_THA,
                    MAN_INSTEAD_NAME_THA = s.MAN_INSTEAD_NAME_THA,
                    PART_NO = s.PART_NO,
                    MC_NO = s.MC_NO,
                    MOLD_NO = s.MOLD_NO,
                    PROCESS_POINT = s.PROCESS_POINT,
                    EDIT_POINT = s.EDIT_POINT,
                    CHANGE = s.CHANGE,
                    //DETAILS = s.DETAILS.Replace(",", " | "),
                    //WARNINGS = s.WARNINGS.Replace(",", " | "),
                    REMARK = s.REMARK
                }).ToList();

                foreach (DataRow r in Util.ToDataTable(record_csv).Rows)
                {
                    IEnumerable<string> fields = r.ItemArray.Select(field => field.ToString().Replace("\n", ""));

                    sb.AppendLine(string.Join(",", fields));
                }

                return Json(new { data = path_file, success = true, responseText = "SUCCES", Message = "สร้างไฟล์สำเร็จ" }, JsonRequestBehavior.AllowGet);
            }   
            catch (Exception ex)
            {
                return Json(new { data = "", success = false, responseText = "ERROR:" + ex.Message.ToString(), Message = err_msg }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                System.IO.File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            }
        }

        public ActionResult delete_export_data_file(string path_file)
        {

            string fullPath = System.Web.HttpContext.Current.Server.MapPath(path_file);

            try
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                return Json(new { success = true, responseText = "SUCCES", Message = "ลบไฟล์สำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "FAILED", Message = "ERROR:" + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }

        }


    }
}