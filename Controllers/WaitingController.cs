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


using System.Globalization;

using Change_Point.Models;
using System.IO;

using System.Text.RegularExpressions;

using Microsoft.VisualBasic.FileIO;

using System.Text;

namespace Change_Point.Controllers
{
    public class WaitingController : Controller
    {

        Home ARR = new Home();

        public string err_msg = "พบปัญหา \n กรุณาติดต่อแอดมิน";

        public class WAITING_CONFIRM
        {
            public string ID { get; set; }
            public string CALENDAR_ID { get; set; }
            public string TYPE { get; set; }
            public string SHIFT { get; set; }
            public string RESULT { get; set; }
            public string GNAME_ENG { get; set; }
            public string FNAME_ENG { get; set; }
            public string CREATE_DATE { get; set; }
            public string CHANGE_POINT_DATE { get; set; }
            public string GROUP_NAME { get; set; }
            public string CONFIRM_DATE { get; set; }
        }
        public class COUNT_WAITING_CONFIRM
        {
            public string RESULT { get; set; }
        }

        // GET: Waiting
        public ActionResult Index()
        {
            ARR.IsLayout = true;
            ARR.IsLogin = true;

            if (Session["EMP_NO"] == null || Session["ROLE"] == null)
            {
                Session["STATUS"] = "V";
                Session["ROLE"] = "V";
                Session["WC_CODE"] = "";
                Session["GROUP_ID"] = "";
                return RedirectToAction("index", "Login");
            }

            if (Session["ROLE"].ToString() != "A" && Session["ROLE"].ToString() != "S")
            {
                return RedirectToAction("index", "home");
            }

            ViewBag.Data = ARR;

            return View();
        }

        public ActionResult List_Waiting_Confirm(string date_start, string date_stop, string filter_type, string filter_shift, string filter_status)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<WAITING_CONFIRM> result = new List<WAITING_CONFIRM>();

                OracleCommand cmd = new OracleCommand();

                var emp_no = Session["EMP_NO"];
                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT m.*, e.GNAME_ENG,e.FNAME_ENG, c.STATUS_TYPE, c.ID as CALENDAR_ID, c.CREATE_DATE, c.DATE_CHANGE, c.SHIFT, g.NAME AS GROUP_NAME " + 

                    "FROM CP_CALENDAR_CONFIRM m " +

                    "LEFT JOIN CP_CALENDAR c ON m.CALENDAR_ID = c.ID " +
                    "LEFT JOIN CENTER_TM_EMPLOYEE e ON c.UPDATE_BY = e.EMP_NO " +
                    "LEFT JOIN CP_GROUP g ON c.GROUP_ID = g.ID " +

                    "WHERE m.EMP_NO = '" + emp_no + "'";

                command += date_start != "" ? " AND c.DATE_CHANGE >= to_timestamp('" + date_start + " 00:00:00', 'dd-mm-yyyy hh24:mi:ss') AND " +
                                                    "c.DATE_CHANGE <= to_timestamp('" + date_stop + " 23:59:59', 'dd-mm-yyyy hh24:mi:ss')  " : "";

                command += filter_type != "" ? " AND c.STATUS_TYPE in (" + filter_type + ") " : "";
                command += filter_shift != "" ? " AND c.SHIFT in (" + filter_shift + ") " : "";
                command += filter_status != "" ? " AND m.IS_CONFIRM in (" + filter_status + ") " : "";


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

                            DateTime create_date = Convert.ToDateTime(dtreader["CREATE_DATE"]);
                            DateTime change_point_date = Convert.ToDateTime(dtreader["DATE_CHANGE"]);

                            DateTime? confirm_date = null;

                            if (dtreader["CONFIRM_DATE"] != DBNull.Value)
                            {
                                confirm_date = Convert.ToDateTime(dtreader["CONFIRM_DATE"]);
                            }

                            result.Add(new WAITING_CONFIRM
                            {
                                ID = dtreader["ID"].ToString(),
                                CALENDAR_ID = dtreader["CALENDAR_ID"].ToString(),
                                RESULT = dtreader["IS_CONFIRM"].ToString(),
                                GNAME_ENG = dtreader["GNAME_ENG"].ToString(),
                                FNAME_ENG = dtreader["FNAME_ENG"].ToString(),
                                SHIFT = dtreader["SHIFT"].ToString(),
                                TYPE = dtreader["STATUS_TYPE"].ToString(),
                                GROUP_NAME = dtreader["GROUP_NAME"].ToString(),
                                CONFIRM_DATE = confirm_date == null ? "" : confirm_date.Value.ToString("dd/MM/yyyy HH:mm") ,
                                CREATE_DATE = string.IsNullOrEmpty(create_date.ToString()) ? "" : create_date.ToString("dd/MM/yyyy HH:mm") ,
                                CHANGE_POINT_DATE = string.IsNullOrEmpty(change_point_date.ToString()) ? "" : change_point_date.ToString("dd/MM/yyyy") ,
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

        public ActionResult Confirm_Change_Point(string item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {

                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd = new OracleCommand("CP_SYSTEM.CONFIRM_CHANGE_POINT", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Int32)).Value = item_id;
                cmd.Parameters.Add(new OracleParameter("pCONFIRM_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                //cmd.Parameters.Add(new OracleParameter("rCALENDAR_ID", OracleDbType.Int32)).Direction = ParameterDirection.Output;
                //cmd.Parameters.Add(new OracleParameter("rGROUP_MEMBER_ID", OracleDbType.Int32)).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                //string cid = cmd.Parameters["rCALENDAR_ID"].Value.ToString();
                //string gid = cmd.Parameters["rGROUP_MEMBER_ID"].Value.ToString();

                return Json(new { success = true, responseText = "SUCCES", Message = "Confirm สำเร็จ"}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin.", Message = err_msg }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }

        }

        public ActionResult Count_Waiting_Confirm()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<COUNT_WAITING_CONFIRM> result = new List<COUNT_WAITING_CONFIRM>();

                OracleCommand cmd = new OracleCommand();

                var emp_no = Session["EMP_NO"];
                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT COUNT(m.ID) as RESULT " +
                    "FROM CP_CALENDAR_CONFIRM m " +
                    "WHERE m.EMP_NO = '" + emp_no + "' AND m.IS_CONFIRM = 'N'";

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
                            result.Add(new COUNT_WAITING_CONFIRM
                            {
                                RESULT = dtreader["RESULT"].ToString(),
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
    }
}