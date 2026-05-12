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
using Npgsql;
using NpgsqlTypes;

namespace Change_Point.Controllers
{
    public class LookupController : Controller
    {

        Home ARR = new Home();

        public string err_msg = "พบปัญหา \n กรุณาติดต่อแอดมิน";

        public class MOLD_NO
        {
            public string ITEM_ID { get; set; }
            public string ITEM_NAME { get; set; }
            public string CREATE_BY { get; set; }
            public string CREATE_DATE { get; set; }
            public string UPDATE_BY { get; set; }
            public string UPDATE_DATE { get; set; }
            public string IS_USE { get; set; }
            public string GNAME_ENG { get; set; }
            public string FNAME_ENG { get; set; }
            public string TOOL { get; set; }
        }
        public class MC
        {
            public string ITEM_ID { get; set; }
            public string ITEM_NAME { get; set; }
            public string ITEM_SIZETON { get; set; }
            public string CREATE_BY { get; set; }
            public string CREATE_DATE { get; set; }
            public string UPDATE_BY { get; set; }
            public string UPDATE_DATE { get; set; }
            public string IS_USE { get; set; }
            public string GNAME_ENG { get; set; }
            public string FNAME_ENG { get; set; }
            public string TOOL { get; set; }
        }
        public class EMP
        {
            public string EMP_NO { get; set; }
            public string GNAME_TH { get; set; }
            public string FNAME_TH { get; set; }
            public string GNAME_ENG { get; set; }
            public string FNAME_ENG { get; set; }
            public string FNAME { get; set; }
            public string WC_CODE { get; set; }
            public string WC_NAME { get; set; }
            public string NAME_UPDATE { get; set; }
            public string IS_USE { get; set; }
            public string CREATE_BY { get; set; }
            public string CREATE_DATE { get; set; }
            public string UPDATE_BY { get; set; }
            public string UPDATE_DATE { get; set; }
            public string TOOL { get; set; }
        }
        public class MEMBER
        {
            public string EMP_NO { get; set; }
            public string GNAME_TH { get; set; }
            public string FNAME_TH { get; set; }
            public string GNAME_ENG { get; set; }
            public string FNAME_ENG { get; set; }
            public string FNAME { get; set; }
            public string IS_USE { get; set; }
            public string CREATE_BY { get; set; }
            public string CREATE_DATE { get; set; }
            public string UPDATE_BY { get; set; }
            public string UPDATE_DATE { get; set; }
            public string NAME_UPDATE { get; set; }
            public string ROLE { get; set; }
            public string EMAIL { get; set; }
            public string WC_CODE { get; set; }
            public string TOOL { get; set; }
        }
        public class FILES
        {
            public string ITEM_ID { get; set; }
            public string ITEM_NAME { get; set; }
            public string ITEM_FILE_TYPE { get; set; }
            public string ITEM_FILE_SIZE { get; set; }
            public string ITEM_CALENDAR_ID { get; set; }
        }
        public class LASTID
        {
            public string ID { get; set; }
        }
        public class WC_CODE
        {
            public string WC { get; set; }
            public string NAME { get; set; }
        }
        public class GROUP
        {
            public string ITEM_ID { get; set; }
            public string ITEM_NAME { get; set; }
            public string ITEM_WC { get; set; }
            public string CREATE_BY { get; set; }
            public string CREATE_DATE { get; set; }
            public string UPDATE_BY { get; set; }
            public string UPDATE_DATE { get; set; }
            public string IS_USE { get; set; }
            public string GNAME_ENG { get; set; }
            public string FNAME_ENG { get; set; }
            public string TOOL { get; set; }
            public string MENU_1 { get; set; }
            public string MENU_2 { get; set; }
            public string MENU_3 { get; set; }
            public string MENU_4 { get; set; }
            public string MENU_5 { get; set; }
        }
        public class GROUP_OPTION
        {
            public string WC_CODE { get; set; }
            public string WC_NAME { get; set; }
            public string GROUP_ID { get; set; }
            public string GROUP_NAME { get; set; }
            public string MENU_1 { get; set; }
            public string MENU_2 { get; set; }
            public string MENU_3 { get; set; }
            public string MENU_4 { get; set; }
            public string MENU_5 { get; set; }
        }
        public class CHANGE_POINT
        {
            public string ID { get; set; }
            public string SHIFT { get; set; }
            public string SHIFT_TEAM { get; set; }
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
            public string GNAME_ENG_CREATE { get; set; }
            public string FNAME_ENG_CREATE { get; set; }
            public string GNAME_ENG_UPDATE { get; set; }
            public string FNAME_ENG_UPDATE { get; set; }

            public string REMARK { get; set; }
            public string INFORMED { get; set; }
            public string RECIPIENT_ID { get; set; }
            public List<APPROVE_MEMBER> RECIPIENT_LIST { get; set; }
        }
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
                Response.Redirect(Url.Action("index", "Login"));
            }

            ViewBag.Data = ARR;

            return View();
        }
        public class DASHBOARD_SETTING
        {
            public string PRIORITY { get; set; }
            public string TITLE { get; set; }
            public string TIME { get; set; }
            public string TYPE { get; set; }
        }
        public class DETAIL_CATEGORY
        {
            public string ITEM_ID { get; set; }
            public string ITEM_NAME { get; set; }
            public string ITEM_TYPE { get; set; }
            public string CREATE_BY { get; set; }
            public string CREATE_DATE { get; set; }
            public string UPDATE_BY { get; set; }
            public string UPDATE_DATE { get; set; }
            public string IS_USE { get; set; }
            public string GNAME_ENG { get; set; }
            public string FNAME_ENG { get; set; }
        }
        public class DETAILS
        {
            public string ITEM_ID { get; set; }
            public string ITEM_DETAIL { get; set; }
            public string ITEM_RISK { get; set; }
            public string ITEM_CATEGORY_ID { get; set; }
            public string ITEM_CATEGORY_NAME { get; set; }
            public string CREATE_BY { get; set; }
            public string CREATE_DATE { get; set; }
            public string UPDATE_BY { get; set; }
            public string UPDATE_DATE { get; set; }
            public string IS_USE { get; set; }
            public string GNAME_ENG { get; set; }
            public string FNAME_ENG { get; set; }
        }
        public class DETAILS_OPTION
        {
            public string ITEM_ID { get; set; }
            public string ITEM_DETAIL { get; set; }
            public string ITEM_RISK { get; set; }
            public string ITEM_CATEGORY_NAME { get; set; }
        }
        public class DETAIL_CATEGORY_OPTION
        {
            public string ITEM_ID { get; set; }
            public string ITEM_NAME { get; set; }
            public string ITEM_TYPE { get; set; }
        }

        public class APPROVE_MEMBER
        {
            public string ITEM_ID { get; set; }
            public string ITEM_TITLE { get; set; }
            public string ITEM_MEMBER { get; set; }
            public string CREATE_BY { get; set; }
            public string CREATE_DATE { get; set; }
            public string UPDATE_BY { get; set; }
            public string UPDATE_DATE { get; set; }
            public string IS_USE { get; set; }
            public string GNAME_ENG { get; set; }
            public string FNAME_ENG { get; set; }
            public List<CONFIRM_LIST> CONFIRM_LIST  { get; set; }
        }
        public class APPROVE_MEMBER_OPTION
        {
            public string ITEM_ID { get; set; }
            public string ITEM_TITLE { get; set; }
            public string ITEM_MEMBER { get; set; }
        }

        public class RECIPIENT_LIST
        {
            public string ITEM_ID { get; set; }
            public string ITEM_TITLE { get; set; }
            public string ITEM_MEMBER { get; set; }
        }


        public ActionResult textaa(List<RECIPIENT_LIST> arr)
        {
            try
            {


                return Json(new { data = arr, success = true, responseText = "" }, JsonRequestBehavior.AllowGet);
                   
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }

        }

        public class PART_OPTION
        {
            public string ITEM_ID { get; set; }
        }

        public class CONFIRM_LIST
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


        // ------------------------------------------- DASHBOARD SETTING ------------------------------------------- //

        public ActionResult Dashboard()
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
                return RedirectToAction("index", "Home");
            }

            ARR.param_id = Session["GROUP_ID"].ToString();

            ViewBag.Data = ARR;

            return View();
        }
        public ActionResult AddItem_Dashboard(List<DASHBOARD_SETTING> data_upload, string item_type)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                var count = 1;

                conn.Open();

                cmd = new OracleCommand("CP_SYSTEM.DELETE_DASHBOARD_SETTING", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                cmd.Parameters.Add(new OracleParameter("pITEM_TYPE", OracleDbType.Varchar2)).Value = item_type;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                if (data_upload.Count > 0)
                {
                    foreach (var item in data_upload)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.DASHBOARD_SETTING", conn);

                        cmd.Parameters.Add(new OracleParameter("pITEM_PRIORITY", OracleDbType.Varchar2)).Value = count.ToString();
                        cmd.Parameters.Add(new OracleParameter("pITEM_TITLE", OracleDbType.Varchar2)).Value = item.TITLE.ToString();
                        cmd.Parameters.Add(new OracleParameter("pITEM_TIME", OracleDbType.Varchar2)).Value = item.TIME.ToString();
                        cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                        cmd.Parameters.Add(new OracleParameter("pITEM_TYPE", OracleDbType.Varchar2)).Value = item_type;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        count++;
                    }
                }

                return Json(new { success = true, responseText = "SUCCES", Message = "บันทึกข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult ListItem_Dashboard()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<DASHBOARD_SETTING> result = new List<DASHBOARD_SETTING>();

                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();

                string command = "";

                command = "SELECT m.* FROM CP_DASHBOARD m " +
                    "WHERE GROUP_ID = " + gT +
                    " ORDER BY PRIORITY ASC";

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
                            result.Add(new DASHBOARD_SETTING
                            {
                                PRIORITY = dtreader["PRIORITY"].ToString(),
                                TITLE = dtreader["TITLE"].ToString(),
                                TIME = dtreader["TIME"].ToString(),
                                TYPE = dtreader["TYPE"].ToString(),
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

        // ------------------------------------------- EMP_NO ------------------------------------------- //
        public ActionResult Member()
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

            if (Session["ROLE"].ToString() != "A")
            {
                return RedirectToAction("index", "Home");
            }

            ViewBag.Data = ARR;

            return View();
        }
        public ActionResult SelectItem_EMP(string item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<EMP> result = new List<EMP>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                //item_id = "026739";


                command = "SELECT * FROM CENTER_TM_EMPLOYEE WHERE EMP_NO = '" + item_id + "' ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;

                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        if (dtreader.Read())
                        {

                            var emp_no = dtreader["EMP_NO"].ToString();
                            var gname_th = dtreader["GNAME_THA"].ToString();
                            var fname_th = dtreader["FNAME_THA"].ToString();
                            var gname_eng = dtreader["GNAME_ENG"].ToString();
                            var fname_eng = dtreader["FNAME_ENG"].ToString();


                            result.Add(new EMP
                            {
                                EMP_NO = emp_no,
                                GNAME_TH = gname_th,
                                FNAME_TH = fname_th,
                                GNAME_ENG = gname_eng,
                                FNAME_ENG = fname_eng,
                            });
                        }
                        //return Json(new { success = true, responseText = "OK", data = result }, JsonRequestBehavior.AllowGet);
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
        public ActionResult SelectItem_Member(string item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<MEMBER> result = new List<MEMBER>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                //item_id = "026739";


                //command = "SELECT * FROM CENTER_TM_EMPLOYEE WHERE EMP_NO = '" + item_id + "'";

                command = "SELECT m.*, e1.GNAME_ENG,e1.FNAME_ENG, e1.GNAME_THA, e1.FNAME_THA FROM CENTER_TM_MEMBER m " +
                    "LEFT JOIN CENTER_TM_EMPLOYEE e1 ON m.EMP_NO = e1.EMP_NO " +
                    " WHERE m.EMP_NO = '" + item_id + "'";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;

                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        if (dtreader.Read())
                        {
                            result.Add(new MEMBER
                            {
                                EMP_NO = dtreader["EMP_NO"].ToString(),
                                GNAME_TH = dtreader["GNAME_THA"].ToString(),
                                FNAME_TH = dtreader["FNAME_THA"].ToString(),
                                GNAME_ENG = dtreader["GNAME_ENG"].ToString(),
                                FNAME_ENG = dtreader["FNAME_ENG"].ToString(),
                                ROLE = dtreader["ROLE"].ToString(),
                                EMAIL = dtreader["EMAIL"].ToString(),
                                WC_CODE = dtreader["WC_CODE"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                            });
                        }
                        //return Json(new { success = true, responseText = "OK", data = result }, JsonRequestBehavior.AllowGet);
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
        public ActionResult Option_EMP(string item_id)
        {
            try
            {
                List<EMP> result = new List<EMP>();

                using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["PostgreSqlDbConnectionActual"].ConnectionString))
                {
                    conn.Open();

                    string query = $@"SELECT EMP_NO, WC_CODE, GNAME_ENG, FNAME_ENG, GNAME_THA, FNAME_THA, WC_ABB_NAME FROM CENTER_TM_EMPLOYEE ORDER BY EMP_NO ASC ";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        //cmd.Parameters.Add(new NpgsqlParameter("empNO", NpgsqlDbType.Varchar)).Value = item_id;

                        using (var dtreader = cmd.ExecuteReader())
                        {
                            if (dtreader.Read())
                            {
                                while (dtreader.Read())
                                {
                                    result.Add(new EMP
                                    {
                                        EMP_NO = dtreader["EMP_NO"].ToString(),
                                        GNAME_TH = dtreader["GNAME_THA"].ToString(),
                                        FNAME_TH = dtreader["FNAME_THA"].ToString(),
                                        GNAME_ENG = dtreader["GNAME_ENG"].ToString(),
                                        FNAME_ENG = dtreader["FNAME_ENG"].ToString(),
                                        WC_CODE = dtreader["WC_CODE"].ToString(),
                                        WC_NAME = dtreader["WC_ABB_NAME"].ToString(),
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
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
            }
        }
        //public ActionResult Option_EMP(string item_id)
        //{
        //    OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

        //    try
        //    {
        //        List<EMP> result = new List<EMP>();

        //        OracleCommand cmd = new OracleCommand();

        //        string command = "";

        //        //item_id = "026739";


        //        command = "SELECT EMP_NO, WC_CODE, GNAME_ENG, FNAME_ENG, GNAME_THA, FNAME_THA, WC_ABB_NAME FROM CENTER_TM_EMPLOYEE ORDER BY EMP_NO ASC";

        //        cmd.Connection = conn;
        //        cmd.CommandText = command;
        //        cmd.CommandType = CommandType.Text;

        //        conn.Open();

        //        using (OracleDataReader dtreader = cmd.ExecuteReader())
        //        {
        //            if (dtreader.HasRows)
        //            {
        //                while (dtreader.Read())
        //                {

        //                    result.Add(new EMP
        //                    {
        //                        EMP_NO = dtreader["EMP_NO"].ToString(),
        //                        GNAME_TH = dtreader["GNAME_THA"].ToString(),
        //                        FNAME_TH = dtreader["FNAME_THA"].ToString(),
        //                        GNAME_ENG = dtreader["GNAME_ENG"].ToString(),
        //                        FNAME_ENG = dtreader["FNAME_ENG"].ToString(),
        //                        WC_CODE = dtreader["WC_CODE"].ToString(),
        //                        WC_NAME = dtreader["WC_ABB_NAME"].ToString(),
        //                });
        //                }
        //                //return Json(new { success = true, responseText = "OK", data = result }, JsonRequestBehavior.AllowGet);
        //                return Json(new { data = result, success = true, responseText = "" }, JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                return Json(new { success = false, responseText = "" }, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
        //    }
        //    finally
        //    {
        //        conn.Dispose();
        //        conn.Close();
        //    }

        //}
        public ActionResult Option_WC(string item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<WC_CODE> result = new List<WC_CODE>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                command = "SELECT DISTINCT WC_CODE, DEPT_ABB_NAME FROM CENTER_TM_EMPLOYEE ORDER BY WC_CODE ASC";

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

                            result.Add(new WC_CODE
                            {
                                WC = dtreader["WC_CODE"].ToString(),
                                NAME = dtreader["DEPT_ABB_NAME"].ToString(),
                            });
                        }
                        //return Json(new { success = true, responseText = "OK", data = result }, JsonRequestBehavior.AllowGet);
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
        public ActionResult ListItem_EMP()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<MEMBER> result = new List<MEMBER>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                command = "SELECT m.*, e1.GNAME_ENG,e1.FNAME_ENG, e2.GNAME_ENG as GNAME_UPDATE,e2.FNAME_ENG as FNAME_UPDATE FROM CENTER_TM_MEMBER m " +
                    "LEFT JOIN CENTER_TM_EMPLOYEE e1 ON m.EMP_NO = e1.EMP_NO " +
                    "LEFT JOIN CENTER_TM_EMPLOYEE e2 ON m.UPDATE_BY = e2.EMP_NO";

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
                            var create_by = dtreader["CREATE_BY"].ToString();
                            DateTime update_date = Convert.ToDateTime(dtreader["UPDATE_DATE"]);
                            var update_by = dtreader["UPDATE_BY"].ToString();

                            result.Add(new MEMBER
                            {
                                EMP_NO = dtreader["EMP_NO"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                                GNAME_ENG = dtreader["GNAME_ENG"].ToString(),
                                FNAME_ENG = dtreader["FNAME_ENG"].ToString(),
                                NAME_UPDATE = dtreader["GNAME_UPDATE"].ToString() + " " + dtreader["FNAME_UPDATE"].ToString(),
                                FNAME = dtreader["GNAME_ENG"].ToString() + " " +dtreader["FNAME_ENG"].ToString(),
                                WC_CODE = dtreader["WC_CODE"].ToString() ,
                                EMAIL = dtreader["EMAIL"].ToString() ,
                                TOOL = "-",
                                UPDATE_BY = string.IsNullOrEmpty(update_by) ? create_by : update_by,
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy HH:mm") : update_date.ToString("dd/MM/yy HH:mm"),

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
        public ActionResult AddItem_Member(string item_emp, string item_role, string item_email, string item_password, string item_wc, string item_isUse)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {

                var emp_no = Session["EMP_NO"];

                OracleCommand cmd = new OracleCommand();

                string command = "";

                command = "SELECT * FROM CENTER_TM_MEMBER WHERE EMP_NO = ':item_emp' ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":item_emp", item_emp));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("MFG2_MEMBER.ADD_MEMBER", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_EMP", OracleDbType.Varchar2)).Value = item_emp;
                        cmd.Parameters.Add(new OracleParameter("pITEM_ROLE", OracleDbType.Varchar2)).Value = item_role;
                        cmd.Parameters.Add(new OracleParameter("pITEM_EMAIL", OracleDbType.Varchar2)).Value = item_email;
                        cmd.Parameters.Add(new OracleParameter("pITEM_PASSWORD", OracleDbType.Varchar2)).Value = item_password;
                        cmd.Parameters.Add(new OracleParameter("pITEM_WC", OracleDbType.Varchar2)).Value = item_wc;

                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isUse;
                        cmd.Parameters.Add(new OracleParameter("pCREATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pCREATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        return Json(new { success = true, responseText = "SUCCES", Message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR", Message = "ข้อมูลนี้มีอยู่แล้ว ไม่สามารถเพิ่มซ้ำได้" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin.", Message  = err_msg }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }

        }
        public ActionResult EditItem_Member(string item_emp, string item_role, string item_email, string item_password, string item_isUse, string item_wc)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {

                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                conn.Open();
                cmd = new OracleCommand("MFG2_MEMBER.EDIT_MEMBER", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_EMP", OracleDbType.Varchar2)).Value = item_emp;
                cmd.Parameters.Add(new OracleParameter("pITEM_ROLE", OracleDbType.Varchar2)).Value = item_role;
                cmd.Parameters.Add(new OracleParameter("pITEM_EMAIL", OracleDbType.Varchar2)).Value = item_email;
                cmd.Parameters.Add(new OracleParameter("pITEM_PASSWORD", OracleDbType.Varchar2)).Value = item_password;
                cmd.Parameters.Add(new OracleParameter("pITEM_WC", OracleDbType.Varchar2)).Value = item_wc;

                cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isUse;
                cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                return Json(new { success = true, responseText = "SUCCES", Message = "แก้ไขข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult DeleteItem_Member(string item_emp)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {

                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd = new OracleCommand("MFG2_MEMBER.DELETE_MEMBER", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_EMP", OracleDbType.Varchar2)).Value = item_emp;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                return Json(new { success = true, responseText = "SUCCES", Message = "ลบข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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

        // ------------------------------------------- GROUP ------------------------------------------- //
        public ActionResult Group()
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

            if (Session["ROLE"].ToString() != "A")
            {
                return RedirectToAction("index", "Home");
            }

            ViewBag.Data = ARR;

            return View();
        }
        public ActionResult ListItem_Group()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<GROUP> result = new List<GROUP>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                command = "SELECT m.*, e.GNAME_ENG,e.FNAME_ENG FROM CP_GROUP m " +
                    "LEFT JOIN CENTER_TM_EMPLOYEE e ON m.UPDATE_BY = e.EMP_NO " +
                    "WHERE m.NAME != 'hide'";

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
                            var create_by = dtreader["CREATE_BY"].ToString();
                            DateTime update_date = Convert.ToDateTime(dtreader["UPDATE_DATE"]);
                            var update_by = dtreader["UPDATE_BY"].ToString();
                            var id = dtreader["ID"].ToString();

                            result.Add(new GROUP
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                                ITEM_WC = dtreader["WC_LIST"].ToString(),

                                GNAME_ENG = dtreader["GNAME_ENG"].ToString(),
                                FNAME_ENG = dtreader["FNAME_ENG"].ToString(),
                                UPDATE_BY = string.IsNullOrEmpty(update_by) ? create_by : update_by,
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy HH:mm") : update_date.ToString("dd/MM/yy HH:mm"),
                                TOOL = "-",

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
        public ActionResult SelectItem_GROUP(string item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<GROUP> result = new List<GROUP>();

                OracleCommand cmd = new OracleCommand();

                string command = "";


                command = "SELECT * FROM CP_GROUP WHERE ID = " + item_id ;

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;

                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        if (dtreader.Read())
                        {
                            DateTime create_date = Convert.ToDateTime(dtreader["CREATE_DATE"]);
                            var create_by = dtreader["CREATE_BY"].ToString();
                            DateTime update_date = Convert.ToDateTime(dtreader["UPDATE_DATE"]);
                            var update_by = dtreader["UPDATE_BY"].ToString();

                            result.Add(new GROUP
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
                                ITEM_WC = dtreader["WC_LIST"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                                UPDATE_BY = string.IsNullOrEmpty(update_by) ? create_by : update_by,
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy") : update_date.ToString("dd/MM/yy"),
                                MENU_1 = dtreader["MENU_1"].ToString(),
                                MENU_2 = dtreader["MENU_2"].ToString(),
                                MENU_3 = dtreader["MENU_3"].ToString(),
                                MENU_4 = dtreader["MENU_4"].ToString(),
                                MENU_5 = dtreader["MENU_5"].ToString()
                            });
                        }
                        return Json(new { success = true, responseText = "SUCCESS", data = result }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult SelectItem_GROUP_WC_ByWC()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<GROUP_OPTION> result = new List<GROUP_OPTION>();

                OracleCommand cmd = new OracleCommand();

                string command = "";
                int index = 0;
                string wc_code = Session["WC_CODE"].ToString();

                if (Session["ROLE"].ToString() == "A" || Session["ROLE"].ToString() == "G")
                {
                    command = "SELECT ID as GROUP_ID, NAME, MENU_1, MENU_2, MENU_3, MENU_4, MENU_5  FROM CP_GROUP m " +
                        "WHERE m.IS_USE = 'Y' ";
                } else
                {
                    command = "SELECT DISTINCT m.*, g.NAME, w.DEPT_ABB_NAME, g.MENU_1, g.MENU_2, g.MENU_3, g.MENU_4, g.MENU_5 FROM CP_GROUP_WC m " +
                    "LEFT JOIN CP_GROUP g ON m.GROUP_ID = g.ID " +
                    "LEFT JOIN CENTER_TM_EMPLOYEE w ON m.WC_CODE = w.WC_CODE " +
                    "WHERE m.WC_CODE = '" + wc_code + "' AND g.IS_USE = 'Y' ";
                }

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

                            var gID = dtreader["GROUP_ID"].ToString();
                            var gT = Session["GROUP_ID"].ToString();
                            if (gT == "")
                            {
                                Session["GROUP_ID"] = gID;
                                index++;
                            }

                            if (Session["ROLE"].ToString() == "A" || Session["ROLE"].ToString() == "G")
                            {
                                result.Add(new GROUP_OPTION
                                {
                                    GROUP_ID = dtreader["GROUP_ID"].ToString(),
                                    GROUP_NAME = dtreader["NAME"].ToString(),
                                    MENU_1 = dtreader["MENU_1"].ToString(),                                   
                                    MENU_2 = dtreader["MENU_2"].ToString(),                                   
                                    MENU_3 = dtreader["MENU_3"].ToString(),                                   
                                    MENU_4 = dtreader["MENU_4"].ToString(),                                   
                                    MENU_5 = dtreader["MENU_5"].ToString()                                  
                                });
                            }
                            else
                            {
                                result.Add(new GROUP_OPTION
                                {
                                    WC_NAME = dtreader["DEPT_ABB_NAME"].ToString(),
                                    WC_CODE = dtreader["WC_CODE"].ToString(),
                                    GROUP_ID = dtreader["GROUP_ID"].ToString(),
                                    GROUP_NAME = dtreader["NAME"].ToString(),
                                    MENU_1 = dtreader["MENU_1"].ToString(),
                                    MENU_2 = dtreader["MENU_2"].ToString(),
                                    MENU_3 = dtreader["MENU_3"].ToString(),
                                    MENU_4 = dtreader["MENU_4"].ToString(),
                                    MENU_5 = dtreader["MENU_5"].ToString()
                                });
                            }

                        }
                        return Json(new { success = true, responseText = "SUCCESS", data = result }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult AddItem_Group(string item_name)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            List<LASTID> result = new List<LASTID>();

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                string command = "";

                conn.Open();

                cmd = new OracleCommand("CP_SYSTEM.ADD_GROUP", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = "hide";
                cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = "Y";
                cmd.Parameters.Add(new OracleParameter("pCREATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                cmd.Parameters.Add(new OracleParameter("pCREATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                command = "select ID from CP_GROUP where  id=(select max(ID) from CP_GROUP)";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;

                using (OracleDataReader dtreader2 = cmd.ExecuteReader())
                {
                    if (dtreader2.HasRows)
                    {
                        if (dtreader2.Read())
                        {
                            result.Add(new LASTID
                            {
                                ID = dtreader2["ID"].ToString(),
                            });
                        }
                    }
                }

                return Json(new { data = result, success = true, responseText = "SUCCES", Message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
                  
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
        public ActionResult EditItem_Group(int item_id, string item_name, string item_wc, string item_isUse, string item_menu1, string item_menu2, string item_menu3, string item_menu4, string item_menu5)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();

                string[] arrs = item_wc.Split(',');

                string command = "";

                command = "SELECT * FROM CP_GROUP WHERE WC_LIST = ':item_wc'AND ID <> ':item_id' ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":item_wc", item_wc));
                cmd.Parameters.Add(new OracleParameter(":item_id", item_id));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.EDIT_GROUP", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Int32)).Value = item_id;
                        cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = item_name;
                        cmd.Parameters.Add(new OracleParameter("pITEM_WC", OracleDbType.Varchar2)).Value = item_wc;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isUse;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.Parameters.Add(new OracleParameter("pMENU_1", OracleDbType.Varchar2)).Value = item_menu1;
                        cmd.Parameters.Add(new OracleParameter("pMENU_2", OracleDbType.Varchar2)).Value = item_menu2;
                        cmd.Parameters.Add(new OracleParameter("pMENU_3", OracleDbType.Varchar2)).Value = item_menu3;
                        cmd.Parameters.Add(new OracleParameter("pMENU_4", OracleDbType.Varchar2)).Value = item_menu4;
                        cmd.Parameters.Add(new OracleParameter("pMENU_5", OracleDbType.Varchar2)).Value = item_menu5;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        cmd = new OracleCommand("CP_SYSTEM.DELETE_GROUP_WC", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Int32)).Value = item_id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        foreach (var item in arrs)
                        {
                            cmd = new OracleCommand("CP_SYSTEM.ADD_GROUP_WC", conn);
                            cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Int32)).Value = item_id;
                            cmd.Parameters.Add(new OracleParameter("pITEM_WC", OracleDbType.Varchar2)).Value = item;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.ExecuteNonQuery();
                        }

                        return Json(new { success = true, responseText = "SUCCES", Message = "แก้ไขข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR", Message = "ข้อมูลนี้มีอยู่แล้ว ไม่สามารถเพิ่มซ้ำได้" }, JsonRequestBehavior.AllowGet);
                    }
                }
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
        public ActionResult DeleteItem_Group(int item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {

                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd = new OracleCommand("CP_SYSTEM.DELETE_GROUP", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Int32)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                cmd = new OracleCommand("CP_SYSTEM.DELETE_GROUP_WC", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Int32)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                cmd = new OracleCommand("CP_SYSTEM.DELETE_MOLD_BY_GID", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Int32)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                cmd = new OracleCommand("CP_SYSTEM.DELETE_PROCESS_BY_GID", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Int32)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                cmd = new OracleCommand("CP_SYSTEM.DELETE_MACHINE_BY_GID", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Int32)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                cmd = new OracleCommand("CP_SYSTEM.DELETE_EDIT_BY_GID", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Int32)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                cmd = new OracleCommand("CP_SYSTEM.DELETE_CHANGE_BY_GID", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Int32)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                cmd = new OracleCommand("CP_SYSTEM.DELETE_PART_BY_GID", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Int32)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                cmd = new OracleCommand("CP_SYSTEM.DELETE_CALEDAR_BY_GID", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Int32)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                cmd = new OracleCommand("CP_SYSTEM.DELETE_FILES_BY_GID", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Int32)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                return Json(new { success = true, responseText = "SUCCES", Message = "ลบข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult Delete_Group_Over()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {

                DateTime date_now = DateTime.Now;
                var date_set = date_now.AddMinutes(-10);

                List<LASTID> result = new List<LASTID>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                command = "select * from CP_GROUP " +
                    "where CREATE_DATE < TO_TIMESTAMP('" + date_set.ToString("yyyy-MM-dd HH:mm") + "', 'YYYY-MM-DD HH24:MI') AND NAME = 'hide'";

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

                            cmd = new OracleCommand("CP_SYSTEM.DELETE_GROUP", conn);
                            cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Int32)).Value = dtreader["ID"].ToString();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.ExecuteNonQuery();

                            cmd = new OracleCommand("CP_SYSTEM.DELETE_GROUP_WC", conn);
                            cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Int32)).Value = dtreader["ID"].ToString();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.ExecuteNonQuery();

                            //result.Add(new LASTID
                            //{
                            //    ID = dtreader["ID"].ToString(),

                            //});
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin.", Message = err_msg }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }

        }
        public ActionResult Select_GROUP(string item_group)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                Session["GROUP_ID"] = item_group;


                return Json(new { success = true, responseText = "SUCCESS" }, JsonRequestBehavior.AllowGet);
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

        // ------------------------------------------- ChangePoint ------------------------------------------- //
        public ActionResult AddItem_ChangePoint(
            string dates,
            string spot,
            string instead,
            string mc_no,
            string edit,
            string part_no,
            string mold_no,
            string change,
            string process,
            string detail,
            string action,
            string warning,
            string type,
            string spot_eng,
            string spot_tha,
            string instead_eng,
            string instead_tha,
            string shift,
            string team,
            string remark,
            string recipient,
            List<RECIPIENT_LIST> recipient_list,
            string file_ids
            )
        {

            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            //string dateString = "Thu Aug 24 2023 15:47:42 GMT+0700 (Indochina Time)";
            DateTime dateTime = DateTime.ParseExact(dates, "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz '(Indochina Time)'", System.Globalization.CultureInfo.InvariantCulture);

            List<MOLD_NO> result = new List<MOLD_NO>();
            List<MOLD_NO> result2 = new List<MOLD_NO>();

            try
            {
                string[] recipient_arr = recipient.Split(',');
                var emp_no = Session["EMP_NO"];
                var gT = Session["GROUP_ID"].ToString();
                var return_id = ""; 

                conn.Open();
                OracleCommand cmd = new OracleCommand();

                cmd = new OracleCommand("CP_SYSTEM.ADD_CALENDAR", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_DATE_CHANGE", OracleDbType.TimeStamp)).Value = dateTime;
                cmd.Parameters.Add(new OracleParameter("pITEM_MAN_SPOT", OracleDbType.Varchar2)).Value = spot;
                cmd.Parameters.Add(new OracleParameter("pITEM_MAN_INSTEAD", OracleDbType.Varchar2)).Value = instead;
                cmd.Parameters.Add(new OracleParameter("pITEM_MC_NO", OracleDbType.Varchar2)).Value = mc_no;
                cmd.Parameters.Add(new OracleParameter("pITEM_EDIT_POINT", OracleDbType.Varchar2)).Value = edit;
                cmd.Parameters.Add(new OracleParameter("pITEM_PART_NO", OracleDbType.Varchar2)).Value = part_no;
                cmd.Parameters.Add(new OracleParameter("pITEM_MOLD_NO", OracleDbType.Varchar2)).Value = mold_no;
                cmd.Parameters.Add(new OracleParameter("pITEM_CHANGE", OracleDbType.Varchar2)).Value = change;
                cmd.Parameters.Add(new OracleParameter("pITEM_PROCESS", OracleDbType.Varchar2)).Value = process;
                cmd.Parameters.Add(new OracleParameter("pITEM_DETAILS", OracleDbType.Varchar2)).Value = detail;
                cmd.Parameters.Add(new OracleParameter("pITEM_ACTION", OracleDbType.Varchar2)).Value = action;
                cmd.Parameters.Add(new OracleParameter("pITEM_WARNING", OracleDbType.Varchar2)).Value = warning;
                cmd.Parameters.Add(new OracleParameter("pITEM_TYPE", OracleDbType.Varchar2)).Value = type;
                cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;

                cmd.Parameters.Add(new OracleParameter("pITEM_MAN_SPOT_NAME_ENG", OracleDbType.Varchar2)).Value = spot_eng;
                cmd.Parameters.Add(new OracleParameter("pITEM_MAN_SPOT_NAME_THA", OracleDbType.Varchar2)).Value = spot_tha;
                cmd.Parameters.Add(new OracleParameter("pITEM_MAN_INSTEAD_NAME_ENG", OracleDbType.Varchar2)).Value = instead_eng;
                cmd.Parameters.Add(new OracleParameter("pITEM_MAN_INSTEAD_NAME_THA", OracleDbType.Varchar2)).Value = instead_tha;
                
                cmd.Parameters.Add(new OracleParameter("pCREATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                cmd.Parameters.Add(new OracleParameter("pCREATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;

                cmd.Parameters.Add(new OracleParameter("pITEM_SHIFT", OracleDbType.Varchar2)).Value = shift;
                cmd.Parameters.Add(new OracleParameter("pITEM_SHIFT_TEAM", OracleDbType.Varchar2)).Value = team;
                cmd.Parameters.Add(new OracleParameter("pITEM_REMARK", OracleDbType.Varchar2)).Value = remark;
                cmd.Parameters.Add(new OracleParameter("pITEM_RECIPIENT", OracleDbType.Varchar2)).Value = recipient;

                cmd.Parameters.Add(new OracleParameter("RETURN_ID", OracleDbType.Int32)).Direction = ParameterDirection.Output;

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                return_id = cmd.Parameters["RETURN_ID"].Value.ToString();

                if (file_ids != "")
                {
                    string command = "";

                    command = "UPDATE CP_FILES SET CALENDAR_ID = " + return_id + " WHERE ID in (" + file_ids + ") ";

                    cmd.Connection = conn;
                    cmd.CommandText = command;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteReader();
                }

                string[] recipient_arr2;

                foreach (var v in recipient_list)
                {
                    recipient_arr2 = v.ITEM_MEMBER.Split(',');

                    foreach (var v2 in recipient_arr2)
                    {

                        //Send_Mail_Recipient(v2);
                        cmd = new OracleCommand("CP_SYSTEM.ADD_CALENDAR_CONFIRM", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_CALENDAR_ID", OracleDbType.Varchar2)).Value = return_id;
                        cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_MEMBER_ID", OracleDbType.Varchar2)).Value = v.ITEM_ID;
                        cmd.Parameters.Add(new OracleParameter("pITEM_EMP_NO", OracleDbType.Varchar2)).Value = v2;
                        cmd.Parameters.Add(new OracleParameter("pCREATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }

                }

                return Json(new { success = true, responseText = "SUCCES", Message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult editItem_ChangePoint(
            string dates,
            string spot,
            string instead,
            string mc_no,
            string edit,
            string part_no,
            string mold_no,
            string change,
            string process,
            string detail,
            string action,
            string warning,
            string type,
            string spot_eng,
            string spot_tha,
            string instead_eng,
            string instead_tha,
            string id,
            string shift,
            string team,
            string remark,
            string recipient,
            List<RECIPIENT_LIST> recipient_list
            )
        {

            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            //string dateString = "Thu Aug 24 2023 15:47:42 GMT+0700 (Indochina Time)";
            DateTime dateTime = DateTime.ParseExact(dates, "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz '(Indochina Time)'", System.Globalization.CultureInfo.InvariantCulture);

            try
            {
                string[] recipient_arr = recipient.Split(',');

                var emp_no = Session["EMP_NO"];
                var gT = Session["GROUP_ID"].ToString();

          
                conn.Open();
                OracleCommand cmd = new OracleCommand();

                cmd = new OracleCommand("CP_SYSTEM.EDIT_CALENDAR", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_DATE_CHANGE", OracleDbType.TimeStamp)).Value = dateTime;
                cmd.Parameters.Add(new OracleParameter("pITEM_MAN_SPOT", OracleDbType.Varchar2)).Value = spot;
                cmd.Parameters.Add(new OracleParameter("pITEM_MAN_INSTEAD", OracleDbType.Varchar2)).Value = instead;
                cmd.Parameters.Add(new OracleParameter("pITEM_MC_NO", OracleDbType.Varchar2)).Value = mc_no;
                cmd.Parameters.Add(new OracleParameter("pITEM_EDIT_POINT", OracleDbType.Varchar2)).Value = edit;
                cmd.Parameters.Add(new OracleParameter("pITEM_PART_NO", OracleDbType.Varchar2)).Value = part_no;
                cmd.Parameters.Add(new OracleParameter("pITEM_MOLD_NO", OracleDbType.Varchar2)).Value = mold_no;
                cmd.Parameters.Add(new OracleParameter("pITEM_CHANGE", OracleDbType.Varchar2)).Value = change;
                cmd.Parameters.Add(new OracleParameter("pITEM_PROCESS", OracleDbType.Varchar2)).Value = process;
                cmd.Parameters.Add(new OracleParameter("pITEM_DETAILS", OracleDbType.Varchar2)).Value = detail;
                cmd.Parameters.Add(new OracleParameter("pITEM_ACTION", OracleDbType.Varchar2)).Value = action;
                cmd.Parameters.Add(new OracleParameter("pITEM_WARNING", OracleDbType.Varchar2)).Value = warning;
                cmd.Parameters.Add(new OracleParameter("pITEM_TYPE", OracleDbType.Varchar2)).Value = type;
                cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Varchar2)).Value = id;

                cmd.Parameters.Add(new OracleParameter("pITEM_MAN_SPOT_NAME_ENG", OracleDbType.Varchar2)).Value = spot_eng;
                cmd.Parameters.Add(new OracleParameter("pITEM_MAN_SPOT_NAME_THA", OracleDbType.Varchar2)).Value = spot_tha;
                cmd.Parameters.Add(new OracleParameter("pITEM_MAN_INSTEAD_NAME_ENG", OracleDbType.Varchar2)).Value = instead_eng;
                cmd.Parameters.Add(new OracleParameter("pITEM_MAN_INSTEAD_NAME_THA", OracleDbType.Varchar2)).Value = instead_tha;

                cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;

                cmd.Parameters.Add(new OracleParameter("pITEM_SHIFT", OracleDbType.Varchar2)).Value = shift;
                cmd.Parameters.Add(new OracleParameter("pITEM_SHIFT_TEAM", OracleDbType.Varchar2)).Value = team;
                cmd.Parameters.Add(new OracleParameter("pITEM_REMARK", OracleDbType.Varchar2)).Value = remark;
                cmd.Parameters.Add(new OracleParameter("pITEM_RECIPIENT", OracleDbType.Varchar2)).Value = recipient;

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                cmd = new OracleCommand("CP_SYSTEM.DELETE_CALENDAR_CONFIRM", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_CALENDAR_ID", OracleDbType.Varchar2)).Value = id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                string[] recipient_arr2;

                foreach (var v in recipient_list)
                {
                    recipient_arr2 = v.ITEM_MEMBER.Split(',');

                    foreach (var v2 in recipient_arr2)
                    {

                        //Send_Mail_Recipient(v2);

                        cmd = new OracleCommand("CP_SYSTEM.ADD_CALENDAR_CONFIRM", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_CALENDAR_ID", OracleDbType.Varchar2)).Value = id;
                        cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_MEMBER_ID", OracleDbType.Varchar2)).Value = v.ITEM_ID;
                        cmd.Parameters.Add(new OracleParameter("pITEM_EMP_NO", OracleDbType.Varchar2)).Value = v2;
                        cmd.Parameters.Add(new OracleParameter("pCREATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }

                }

                return Json(new { success = true, responseText = "SUCCES", Message = "แก้ไขข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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

        public ActionResult DeleteItem_ChangePoint(string item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];

                conn.Open();

                OracleCommand cmd = new OracleCommand();

                string command = "";
                command = "SELECT * FROM CP_FILES WHERE CALENDAR_ID = :item_id ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":item_id", item_id));

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        while (dtreader.Read())
                        {
                            var file_name = dtreader["NAME"].ToString();

                            string fullPath = Path.Combine(Server.MapPath("~/src/upload"), file_name);

                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                            }
                        }
                    }
                }

                cmd = new OracleCommand("CP_SYSTEM.DELETE_FILES_CID", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Varchar2)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                
                cmd = new OracleCommand("CP_SYSTEM.DELETE_CALENDAR", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Varchar2)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                cmd = new OracleCommand("CP_SYSTEM.DELETE_CALENDAR_CONFIRM", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_CALENDAR_ID", OracleDbType.Varchar2)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                

                return Json(new { success = true, responseText = "SUCCES", Message = "ลบข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult ListItem_ChangePoint_Day(string date_select)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<CHANGE_POINT> result = new List<CHANGE_POINT>();
                List<APPROVE_MEMBER> result2 = new List<APPROVE_MEMBER>();

                OracleCommand cmd = new OracleCommand();
                OracleCommand cmd2 = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT CP_CALENDAR.*, e1.GNAME_ENG as GNAME_ENG_CREATE ,e1.FNAME_ENG as FNAME_ENG_CREATE, e2.GNAME_ENG as GNAME_ENG_UPDATE ,e2.FNAME_ENG as FNAME_ENG_UPDATE " +
                    "FROM CP_CALENDAR " +
                    "LEFT JOIN CENTER_TM_EMPLOYEE e1 ON CP_CALENDAR.CREATE_BY = e1.EMP_NO " +
                    "LEFT JOIN CENTER_TM_EMPLOYEE e2 ON CP_CALENDAR.UPDATE_BY = e2.EMP_NO " +

                    "WHERE DATE_CHANGE >= to_timestamp('" + date_select + " 00:00:00', 'dd-mm-yyyy hh24:mi:ss') and " +
                    "DATE_CHANGE <= to_timestamp('" + date_select + " 23:59:59', 'dd-mm-yyyy hh24:mi:ss') AND STATUS_TYPE <> 'hide' AND " +
                    "GROUP_ID = '" + gT + "' " +
                    "ORDER BY SHIFT ASC";


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
                            var create_by = dtreader["CREATE_BY"].ToString();
                            DateTime update_date = Convert.ToDateTime(dtreader["UPDATE_DATE"]);
                            var update_by = dtreader["UPDATE_BY"].ToString();
                            var group_member_ids = dtreader["RECIPIENT"].ToString();

                            if (group_member_ids != "N" && group_member_ids != "")
                            {
                                command = "SELECT * FROM CP_APPROVE_MEMBER WHERE ID IN (" + group_member_ids + ") ";

                                cmd2.Connection = conn;
                                cmd2.CommandText = command;
                                cmd2.CommandType = CommandType.Text;

                                using (OracleDataReader dtreader2 = cmd2.ExecuteReader())
                                {
                                    if (dtreader2.HasRows)
                                    {
                                        while (dtreader2.Read())
                                        {
                                            result2.Add(new APPROVE_MEMBER
                                            {
                                                ITEM_ID = dtreader2["ID"].ToString(),
                                                ITEM_TITLE = dtreader2["TITLE"].ToString(),
                                                ITEM_MEMBER = dtreader2["MEMBER_LIST"].ToString(),
                                            });
                                        }
                                    }
                                }
                            }

                            result.Add(new CHANGE_POINT
                            {
                                ID = dtreader["ID"].ToString(),
                                SHIFT = dtreader["SHIFT"].ToString(),
                                SHIFT_TEAM = dtreader["SHIFT_TEAM"].ToString(),
                                DATE_CHANGE = dtreader["DATE_CHANGE"].ToString(),
                                MAN_SPOT = dtreader["MAN_SPOT"].ToString(),
                                MAN_INSTEAD = dtreader["MAN_INSTEAD"].ToString(),
                                MC_NO = dtreader["MC_NO"].ToString(),
                                EDIT_POINT = dtreader["EDIT_POINT"].ToString(),
                                PART_NO = dtreader["PART_NO"].ToString(),
                                MOLD_NO = dtreader["MOLD_NO"].ToString(),
                                CHANGE = dtreader["CHANGE"].ToString(),
                                PROCESS_POINT = dtreader["PROCESS_POINT"].ToString(),
                                DETAILS = dtreader["DETAILS"].ToString(),
                                WARNINGS = dtreader["WARNINGS"].ToString(),
                                STATUS_TYPE = dtreader["STATUS_TYPE"].ToString(),
                                REMARK = dtreader["REMARK"].ToString(),
                                INFORMED = dtreader["INFORMED"].ToString(),
                                ACTION = dtreader["ACTION"].ToString(),
                                RECIPIENT_ID = dtreader["RECIPIENT"].ToString(),
                                RECIPIENT_LIST = result2,

                                MAN_SPOT_NAME_ENG = dtreader["MAN_SPOT_NAME_ENG"].ToString(),
                                MAN_SPOT_NAME_THA = dtreader["MAN_SPOT_NAME_THA"].ToString(),
                                MAN_INSTEAD_NAME_ENG = dtreader["MAN_INSTEAD_NAME_ENG"].ToString(),
                                MAN_INSTEAD_NAME_THA = dtreader["MAN_INSTEAD_NAME_THA"].ToString(),

                                GNAME_ENG_CREATE = dtreader["GNAME_ENG_CREATE"].ToString(),
                                FNAME_ENG_CREATE = dtreader["FNAME_ENG_CREATE"].ToString(),
                                GNAME_ENG_UPDATE = dtreader["GNAME_ENG_UPDATE"].ToString(),
                                FNAME_ENG_UPDATE = dtreader["FNAME_ENG_UPDATE"].ToString(),

                                CREATE_BY = create_by,
                                CREATE_DATE = create_date.ToString("dd/MM/yy HH:mm"),
                                UPDATE_BY = string.IsNullOrEmpty(update_by) ? create_by : update_by,
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy HH:mm") : update_date.ToString("dd/MM/yy HH:mm"),

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
        public ActionResult ListItem_ChangePoint_All()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<CHANGE_POINT> result = new List<CHANGE_POINT>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT CP_CALENDAR.* FROM CP_CALENDAR " +
                    "WHERE STATUS_TYPE <> 'hide' AND GROUP_ID = '" + gT + "'";

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

                            result.Add(new CHANGE_POINT
                            {
                                ID = dtreader["ID"].ToString(),
                                SHIFT = dtreader["SHIFT"].ToString(),
                                SHIFT_TEAM = dtreader["SHIFT_TEAM"].ToString(),
                                DATE_CHANGE = dtreader["DATE_CHANGE"].ToString(),
                                MAN_SPOT = dtreader["MAN_SPOT"].ToString(),
                                MAN_INSTEAD = dtreader["MAN_INSTEAD"].ToString(),
                                MC_NO = dtreader["MC_NO"].ToString(),
                                EDIT_POINT = dtreader["EDIT_POINT"].ToString(),
                                PART_NO = dtreader["PART_NO"].ToString(),
                                MOLD_NO = dtreader["MOLD_NO"].ToString(),
                                CHANGE = dtreader["CHANGE"].ToString(),
                                PROCESS_POINT = dtreader["PROCESS_POINT"].ToString(),
                                DETAILS = dtreader["DETAILS"].ToString(),
                                WARNINGS = dtreader["WARNINGS"].ToString(),
                                STATUS_TYPE = dtreader["STATUS_TYPE"].ToString(),
                                REMARK = dtreader["REMARK"].ToString(),
                                ACTION = dtreader["ACTION"].ToString(),

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
        public ActionResult ListItem_ChangePoint_My()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<CHANGE_POINT> result = new List<CHANGE_POINT>();

                OracleCommand cmd = new OracleCommand();

                var gT = Session["GROUP_ID"].ToString();
                var empNo = Session["EMP_NO"].ToString();

                string command =
                    "SELECT CP_CALENDAR.*, " +
                    "e1.GNAME_ENG as GNAME_ENG_CREATE, e1.FNAME_ENG as FNAME_ENG_CREATE, " +
                    "e2.GNAME_ENG as GNAME_ENG_UPDATE, e2.FNAME_ENG as FNAME_ENG_UPDATE " +
                    "FROM CP_CALENDAR " +
                    "LEFT JOIN CENTER_TM_EMPLOYEE e1 ON CP_CALENDAR.CREATE_BY = e1.EMP_NO " +
                    "LEFT JOIN CENTER_TM_EMPLOYEE e2 ON CP_CALENDAR.UPDATE_BY = e2.EMP_NO " +
                    "WHERE CP_CALENDAR.STATUS_TYPE <> 'hide' " +
                    "AND CP_CALENDAR.GROUP_ID = '" + gT + "' " +
                    "AND CP_CALENDAR.CREATE_BY = '" + empNo + "' " +
                    "ORDER BY CP_CALENDAR.DATE_CHANGE DESC";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;

                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    while (dtreader.Read())
                    {
                        DateTime create_date = Convert.ToDateTime(dtreader["CREATE_DATE"]);
                        var create_by = dtreader["CREATE_BY"].ToString();
                        var update_by = dtreader["UPDATE_BY"].ToString();
                        DateTime update_date = string.IsNullOrEmpty(update_by) ? create_date : Convert.ToDateTime(dtreader["UPDATE_DATE"]);

                        result.Add(new CHANGE_POINT
                        {
                            ID = dtreader["ID"].ToString(),
                            SHIFT = dtreader["SHIFT"].ToString(),
                            SHIFT_TEAM = dtreader["SHIFT_TEAM"].ToString(),
                            DATE_CHANGE = dtreader["DATE_CHANGE"].ToString(),
                            MAN_SPOT = dtreader["MAN_SPOT"].ToString(),
                            MAN_INSTEAD = dtreader["MAN_INSTEAD"].ToString(),
                            MC_NO = dtreader["MC_NO"].ToString(),
                            EDIT_POINT = dtreader["EDIT_POINT"].ToString(),
                            PART_NO = dtreader["PART_NO"].ToString(),
                            MOLD_NO = dtreader["MOLD_NO"].ToString(),
                            CHANGE = dtreader["CHANGE"].ToString(),
                            PROCESS_POINT = dtreader["PROCESS_POINT"].ToString(),
                            DETAILS = dtreader["DETAILS"].ToString(),
                            WARNINGS = dtreader["WARNINGS"].ToString(),
                            STATUS_TYPE = dtreader["STATUS_TYPE"].ToString(),
                            REMARK = dtreader["REMARK"].ToString(),
                            ACTION = dtreader["ACTION"].ToString(),
                            INFORMED = dtreader["INFORMED"].ToString(),
                            RECIPIENT_ID = dtreader["RECIPIENT"].ToString(),
                            MAN_SPOT_NAME_THA = dtreader["MAN_SPOT_NAME_THA"].ToString(),
                            MAN_INSTEAD_NAME_THA = dtreader["MAN_INSTEAD_NAME_THA"].ToString(),
                            CREATE_BY = create_by,
                            CREATE_DATE = create_date.ToString("dd/MM/yy HH:mm"),
                            UPDATE_BY = string.IsNullOrEmpty(update_by) ? create_by : update_by,
                            UPDATE_DATE = update_date.ToString("dd/MM/yy HH:mm"),
                            GNAME_ENG_CREATE = dtreader["GNAME_ENG_CREATE"].ToString(),
                            FNAME_ENG_CREATE = dtreader["FNAME_ENG_CREATE"].ToString(),
                            GNAME_ENG_UPDATE = dtreader["GNAME_ENG_UPDATE"].ToString(),
                            FNAME_ENG_UPDATE = dtreader["FNAME_ENG_UPDATE"].ToString(),
                        });
                    }
                    return Json(new { data = result, success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }
        }
        public ActionResult ListItem_ChangePoint_Item(string item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<CHANGE_POINT> result = new List<CHANGE_POINT>();
                List<APPROVE_MEMBER> result2 = new List<APPROVE_MEMBER>();

                OracleCommand cmd = new OracleCommand();
                OracleCommand cmd2 = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT CP_CALENDAR.*, e1.GNAME_ENG as GNAME_ENG_CREATE ,e1.FNAME_ENG as FNAME_ENG_CREATE, e2.GNAME_ENG as GNAME_ENG_UPDATE ,e2.FNAME_ENG as FNAME_ENG_UPDATE " +
                          "FROM CP_CALENDAR " +
                            "LEFT JOIN CENTER_TM_EMPLOYEE e1 ON CP_CALENDAR.CREATE_BY = e1.EMP_NO " +
                            "LEFT JOIN CENTER_TM_EMPLOYEE e2 ON CP_CALENDAR.UPDATE_BY = e2.EMP_NO " +
                            "WHERE ID = " + item_id;
              

        cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;

                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        if (dtreader.Read())
                        {

                            DateTime create_date = Convert.ToDateTime(dtreader["CREATE_DATE"]);
                            var create_by = dtreader["CREATE_BY"].ToString();
                            DateTime update_date = Convert.ToDateTime(dtreader["UPDATE_DATE"]);
                            var update_by = dtreader["UPDATE_BY"].ToString();
                            var group_member_ids = dtreader["RECIPIENT"].ToString();

                            if (group_member_ids != "N" && group_member_ids != "")
                            {
                                command = "SELECT * FROM CP_APPROVE_MEMBER WHERE ID IN (" + group_member_ids + ") ";

                                cmd2.Connection = conn;
                                cmd2.CommandText = command;
                                cmd2.CommandType = CommandType.Text;

                                using (OracleDataReader dtreader2 = cmd2.ExecuteReader())
                                {
                                    if (dtreader2.HasRows)
                                    {
                                        while (dtreader2.Read())
                                        {
                                            result2.Add(new APPROVE_MEMBER
                                            {
                                                ITEM_ID = dtreader2["ID"].ToString(),
                                                ITEM_TITLE = dtreader2["TITLE"].ToString(),
                                                ITEM_MEMBER = dtreader2["MEMBER_LIST"].ToString(),
                                            });
                                        }
                                    }
                                }
                            }

                            result.Add(new CHANGE_POINT
                            {
                                ID = dtreader["ID"].ToString(),
                                SHIFT = dtreader["SHIFT"].ToString(),
                                SHIFT_TEAM = dtreader["SHIFT_TEAM"].ToString(),
                                DATE_CHANGE = dtreader["DATE_CHANGE"].ToString(),
                                MAN_SPOT = dtreader["MAN_SPOT"].ToString(),
                                MAN_INSTEAD = dtreader["MAN_INSTEAD"].ToString(),
                                MC_NO = dtreader["MC_NO"].ToString(),
                                EDIT_POINT = dtreader["EDIT_POINT"].ToString(),
                                PART_NO = dtreader["PART_NO"].ToString(),
                                MOLD_NO = dtreader["MOLD_NO"].ToString(),
                                CHANGE = dtreader["CHANGE"].ToString(),
                                PROCESS_POINT = dtreader["PROCESS_POINT"].ToString(),
                                DETAILS = dtreader["DETAILS"].ToString(),
                                WARNINGS = dtreader["WARNINGS"].ToString(),
                                STATUS_TYPE = dtreader["STATUS_TYPE"].ToString(),
                                REMARK = dtreader["REMARK"].ToString(),
                                INFORMED = dtreader["INFORMED"].ToString(),
                                ACTION = dtreader["ACTION"].ToString(),
                                RECIPIENT_ID = dtreader["RECIPIENT"].ToString(),
                                RECIPIENT_LIST = result2,

                                MAN_SPOT_NAME_ENG = dtreader["MAN_SPOT_NAME_ENG"].ToString(),
                                MAN_SPOT_NAME_THA = dtreader["MAN_SPOT_NAME_THA"].ToString(),
                                MAN_INSTEAD_NAME_ENG = dtreader["MAN_INSTEAD_NAME_ENG"].ToString(),
                                MAN_INSTEAD_NAME_THA = dtreader["MAN_INSTEAD_NAME_THA"].ToString(),

                                GNAME_ENG_CREATE = dtreader["GNAME_ENG_CREATE"].ToString(),
                                FNAME_ENG_CREATE = dtreader["FNAME_ENG_CREATE"].ToString(),
                                GNAME_ENG_UPDATE = dtreader["GNAME_ENG_UPDATE"].ToString(),
                                FNAME_ENG_UPDATE = dtreader["FNAME_ENG_UPDATE"].ToString(),

                                UPDATE_BY = string.IsNullOrEmpty(update_by) ? create_by : update_by,
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy HH:mm") : update_date.ToString("dd/MM/yy HH:mm"),

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
        public ActionResult ListItem_ChangePoint_Over()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {

                DateTime date_now = DateTime.Now;
                var date_set = date_now.AddMinutes(-10);

                List<CHANGE_POINT> result = new List<CHANGE_POINT>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                command = "select * from cp_calendar " +
                    "where CREATE_DATE < TO_TIMESTAMP('" + date_set.ToString("yyyy-MM-dd HH:mm") + "', 'YYYY-MM-DD HH24:MI') AND STATUS_TYPE = 'hide'";

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

                            result.Add(new CHANGE_POINT
                            {
                                ID = dtreader["ID"].ToString(),

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
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, string type,string size, string id)
        {

            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);
            List<MOLD_NO> result2 = new List<MOLD_NO>();

            try
            {

                var emp_no = Session["EMP_NO"];
                var gT = Session["GROUP_ID"].ToString();
                var return_id = "";

                conn.Open();
                OracleCommand cmd = new OracleCommand();

                string originalFileName = Path.GetFileName(file.FileName);
                string[] FileName = originalFileName.Split('.');
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string newFileName = FileName[0] + "_" + timestamp + "." + FileName[1];
                string path = Path.Combine(Server.MapPath("~/src/upload"), newFileName);
                file.SaveAs(path);

                cmd = new OracleCommand("CP_SYSTEM.ADD_FILES", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_CALENDAR_ID", OracleDbType.Varchar2)).Value = "";
                cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = newFileName;
                cmd.Parameters.Add(new OracleParameter("pITEM_TYPE", OracleDbType.Varchar2)).Value = type;
                cmd.Parameters.Add(new OracleParameter("pITEM_SIZE", OracleDbType.Varchar2)).Value = size;
                cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                cmd.Parameters.Add(new OracleParameter("RETURN_ID", OracleDbType.Int32)).Direction = ParameterDirection.Output;

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                return_id = cmd.Parameters["RETURN_ID"].Value.ToString();

                result2.Add(new MOLD_NO
                {
                    ITEM_ID = return_id,
                    ITEM_NAME = newFileName,
                });

                //string command = "";

                //command = "select ID,NAME from CP_FILES where  id=(select max(ID) from CP_FILES)";

                //cmd.Connection = conn;
                //cmd.CommandText = command;
                //cmd.CommandType = CommandType.Text;

                //using (OracleDataReader dtreader = cmd.ExecuteReader())
                //{
                //    if (dtreader.HasRows)
                //    {
                //        if (dtreader.Read())
                //        {
                //            result2.Add(new MOLD_NO
                //            {
                //                ITEM_ID = dtreader["ID"].ToString(),
                //                ITEM_NAME = dtreader["NAME"].ToString(),
                //            });
                //        }
                //    }
                //}

                return Json(new { success = true, responseText = "SUCCES",data = result2, Message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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


            //string path = Server.MapPath("~/uploads");
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}

            //try
            //    {
            //        //string originalFileName = Path.GetFileName(file.FileName);
            //        //string uniqueFileName = Guid.NewGuid().ToString() + "_" + originalFileName;
            //        //string path = Path.Combine(Server.MapPath("~/src/upload"), uniqueFileName);
            //        //file.SaveAs(path);

            //        string originalFileName = Path.GetFileName(file.FileName);
            //        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            //        string newFileName = timestamp + "_" + originalFileName;
            //        string path = Path.Combine(Server.MapPath("~/src/upload"), newFileName);
            //        file.SaveAs(path);

            //        //string path = Path.Combine(Server.MapPath("~/src/upload"), Path.GetFileName(file.FileName));
            //        //file.SaveAs(path);
            //        return Json(new { success = true, responseText = "SUCCES", data = newFileName,  Message = "File Uploaded Successfully!!" }, JsonRequestBehavior.AllowGet);
            //    }
            //    catch (Exception ex)
            //    {
            //        return Json(new { success = true, responseText = "FAILED", data = "", Message = "ERROR:" + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            //    }
            //else
            //{
            //    return Json(new { success = true, responseText = "FAILED", data = "", Message = "You have not specified a file." }, JsonRequestBehavior.AllowGet);
            //}
        }
        public ActionResult deleteFile(string item_id, string item_name)
        {

            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);
            List<MOLD_NO> result2 = new List<MOLD_NO>();

            try
            {

                conn.Open();
                OracleCommand cmd = new OracleCommand();


                cmd = new OracleCommand("CP_SYSTEM.DELETE_FILES", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Varchar2)).Value = item_id;

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                string fullPath = Path.Combine(Server.MapPath("~/src/upload"), item_name);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                return Json(new { success = true, responseText = "SUCCES", data = result2, Message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult SelectItem_Files_by_changepoint_id(string item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {               
                var emp_no = Session["EMP_NO"];

                List<FILES> result = new List<FILES>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                command = "SELECT * FROM CP_FILES WHERE CALENDAR_ID = :item_id ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":item_id", item_id));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        while (dtreader.Read())
                        {
                            result.Add(new FILES
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
                                ITEM_FILE_TYPE = dtreader["FILE_TYPE"].ToString(),
                                ITEM_FILE_SIZE = dtreader["FILE_SIZE"].ToString(),
                                ITEM_CALENDAR_ID = dtreader["CALENDAR_ID"].ToString(),
                            });
                        }
                        return Json(new { success = true, responseText = "SUCCESS", data = result }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult SelectItem_Files_by_in_file_id(string item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];

                List<FILES> result = new List<FILES>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                command = "SELECT * FROM CP_FILES WHERE ID in (" + item_id + ") ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                //cmd.Parameters.Add(new OracleParameter(":item_id", item_id));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        while (dtreader.Read())
                        {
                            result.Add(new FILES
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
                                ITEM_FILE_TYPE = dtreader["FILE_TYPE"].ToString(),
                                ITEM_FILE_SIZE = dtreader["FILE_SIZE"].ToString(),
                                ITEM_CALENDAR_ID = dtreader["CALENDAR_ID"].ToString(),
                            });
                        }
                        return Json(new { success = true, responseText = "SUCCESS", data = result }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult EditItem_ChangePoint_Informed(string item_informed, int item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                conn.Open();
                cmd = new OracleCommand("CP_SYSTEM.EDIT_CALENDAR_INFROMED", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Int32)).Value = item_id;
                cmd.Parameters.Add(new OracleParameter("pITEM_INFORMED", OracleDbType.Varchar2)).Value = item_informed;
                cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                return Json(new { success = true, responseText = "SUCCES", Message = "แก้ไขข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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
        public static float Send_Mail_Recipient(string item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {

                OracleCommand cmd = new OracleCommand();

                string command = "";

                command = "SELECT EMAIL FROM CENTER_TM_MEMBER WHERE EMP_NO = '" + item_id + "'";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;

                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        if (dtreader.Read())
                        {
                            string email = dtreader["EMAIL"].ToString();
                            string link_address = System.Web.Hosting.HostingEnvironment.MapPath("~");

                            if (email != "")
                            {
                                string subject = "[HT-MFG 2 Change Point] You have Confirm List";
                                string body = "*** Do not reply this E-Mail *** \n";
                                body += "===================== \n";
                                body += "You have Confirm List \n";
                                body += "Link : " + link_address;

                                var mail_user = "ChangePoint_System@mail.canon";
                                var mail_target = email;

                                //Util sendmail = new Util();
                                Util.SendEmail(subject, mail_user, mail_target, "", body);

                            }
                        }
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }

        }


        // ------------------------------------------- MOLD NO ------------------------------------------- //
        public ActionResult Mold_NO()
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
                return RedirectToAction("index", "Home");
            }

            ViewBag.Data = ARR;

            return View();
        }
        public ActionResult ListItem_Mold_NO()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<MOLD_NO> result = new List<MOLD_NO>();

                OracleCommand cmd = new OracleCommand();

                string command = "";
                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT CP_MOLD_NO.*, CENTER_TM_EMPLOYEE.GNAME_ENG,CENTER_TM_EMPLOYEE.FNAME_ENG FROM CP_MOLD_NO LEFT JOIN CENTER_TM_EMPLOYEE ON CP_MOLD_NO.UPDATE_BY = CENTER_TM_EMPLOYEE.EMP_NO " +
                    "WHERE GROUP_ID = '" + gT + "'";

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
                            var create_by = dtreader["CREATE_BY"].ToString();
                            DateTime update_date = Convert.ToDateTime(dtreader["UPDATE_DATE"]);
                            var update_by = dtreader["UPDATE_BY"].ToString();

                            result.Add(new MOLD_NO
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                                GNAME_ENG = dtreader["GNAME_ENG"].ToString(),
                                FNAME_ENG = dtreader["FNAME_ENG"].ToString(),
                                UPDATE_BY = string.IsNullOrEmpty(update_by) ? create_by : update_by,
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy HH:mm") : update_date.ToString("dd/MM/yy HH:mm"),
                                TOOL = "-",

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
        public ActionResult Option_Mold_NO()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<MOLD_NO> result = new List<MOLD_NO>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT ID, NAME FROM CP_MOLD_NO WHERE IS_USE = 'Y' and GROUP_ID = '" + gT + "'";


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

                            result.Add(new MOLD_NO
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
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
        public ActionResult SelectItem_Mold_NO(string item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<MOLD_NO> result = new List<MOLD_NO>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT * FROM CP_MOLD_NO WHERE ID = " + item_id;

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;

                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        if (dtreader.Read())
                        {
                            DateTime create_date = Convert.ToDateTime(dtreader["CREATE_DATE"]);
                            var create_by = dtreader["CREATE_BY"].ToString();
                            DateTime update_date = Convert.ToDateTime(dtreader["UPDATE_DATE"]);
                            var update_by = dtreader["UPDATE_BY"].ToString();

                            result.Add(new MOLD_NO
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                                UPDATE_BY = string.IsNullOrEmpty(update_by) ? create_by : update_by,
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy") : update_date.ToString("dd/MM/yy"),

                            });
                        }
                        return Json(new { success = true, responseText = "SUCCESS", data = result }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult AddItem_Mold_NO(string item_name, string item_isUse)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";

                command = "SELECT * FROM CP_MOLD_NO WHERE NAME = ':item_name' AND GROUP_ID = 'gT' ";
                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":item_name", item_name));
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.ADD_MOLD_NO ", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = item_name;
                        cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isUse;
                        cmd.Parameters.Add(new OracleParameter("pCREATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pCREATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        return Json(new { success = true, responseText = "SUCCES", Message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR", Message = "ข้อมูลนี้มีอยู่แล้ว ไม่สามารถเพิ่มซ้ำได้" }, JsonRequestBehavior.AllowGet);
                    }
                }
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
        public ActionResult EditItem_Mold_NO(string item_name, int item_id, string item_isUse)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";

                command = "SELECT * FROM CP_MOLD_NO WHERE NAME = ':item_name' AND ID <> ':item_id' AND GROUP_ID = ':gT' ";
                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":item_id", item_id));
                cmd.Parameters.Add(new OracleParameter(":item_name", item_name));
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.EDIT_MOLD_NO", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Int32)).Value = item_id;
                        cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = item_name;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isUse;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        return Json(new { success = true, responseText = "SUCCES", Message = "แก้ไขข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR", Message = "ข้อมูลนี้มีอยู่แล้ว ไม่สามารถเพิ่มซ้ำได้" }, JsonRequestBehavior.AllowGet);
                    }
                }
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
        public ActionResult DeleteItem_Mold_NO(int item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {

                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd = new OracleCommand("CP_SYSTEM.DELETE_MOLD_NO", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Int32)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                return Json(new { success = true, responseText = "SUCCES", Message = "ลบข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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
                       
        // ------------------------------------------- PROCESS ------------------------------------------- //
        public ActionResult Process()
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
                return RedirectToAction("index", "Home");
            }

            ViewBag.Data = ARR;

            return View();
        }
        public ActionResult ListItem_Process()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<MOLD_NO> result = new List<MOLD_NO>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT CP_PROCESS.*, CENTER_TM_EMPLOYEE.GNAME_ENG,CENTER_TM_EMPLOYEE.FNAME_ENG FROM CP_PROCESS LEFT JOIN CENTER_TM_EMPLOYEE ON CP_PROCESS.UPDATE_BY = CENTER_TM_EMPLOYEE.EMP_NO " +
                    "WHERE GROUP_ID = '" + gT + "'";

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
                            var create_by = dtreader["CREATE_BY"].ToString();
                            DateTime update_date = Convert.ToDateTime(dtreader["UPDATE_DATE"]);
                            var update_by = dtreader["UPDATE_BY"].ToString();

                            result.Add(new MOLD_NO
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                                GNAME_ENG = dtreader["GNAME_ENG"].ToString(),
                                FNAME_ENG = dtreader["FNAME_ENG"].ToString(),
                                UPDATE_BY = string.IsNullOrEmpty(update_by) ? create_by : update_by,
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy HH:mm") : update_date.ToString("dd/MM/yy HH:mm"),
                                TOOL = "-",
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
        public ActionResult Option_Process()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<MOLD_NO> result = new List<MOLD_NO>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT ID, NAME FROM CP_PROCESS WHERE IS_USE = 'Y' and GROUP_ID = '" + gT + "'";

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
                            result.Add(new MOLD_NO
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
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
        public ActionResult SelectItem_Process(string item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<MOLD_NO> result = new List<MOLD_NO>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT * FROM CP_PROCESS WHERE ID = " + item_id;

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;

                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        if (dtreader.Read())
                        {
                            DateTime create_date = Convert.ToDateTime(dtreader["CREATE_DATE"]);
                            var create_by = dtreader["CREATE_BY"].ToString();
                            DateTime update_date = Convert.ToDateTime(dtreader["UPDATE_DATE"]);
                            var update_by = dtreader["UPDATE_BY"].ToString();

                            result.Add(new MOLD_NO
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                                UPDATE_BY = string.IsNullOrEmpty(update_by) ? create_by : update_by,
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy") : update_date.ToString("dd/MM/yy"),

                            });
                        }
                        return Json(new { success = true, responseText = "SUCCESS", data = result }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult AddItem_Process(string item_name, string item_isUse)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";

                command = "SELECT * FROM CP_PROCESS WHERE NAME = ':item_name' AND GROUP_ID = ':gT' ";
                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":item_name", item_name));
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.ADD_PROCESS ", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = item_name;
                        cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isUse;
                        cmd.Parameters.Add(new OracleParameter("pCREATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pCREATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        return Json(new { success = true, responseText = "SUCCES", Message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR", Message = "ข้อมูลนี้มีอยู่แล้ว ไม่สามารถเพิ่มซ้ำได้" }, JsonRequestBehavior.AllowGet);
                    }
                }
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
        public ActionResult EditItem_Process(string item_name, int item_id, string item_isUse)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";

                command = "SELECT * FROM CP_PROCESS WHERE NAME = ':item_name' AND ID <> ':item_id' AND GROUP_ID = ':gT' ";
                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":item_id", item_id));
                cmd.Parameters.Add(new OracleParameter(":item_name", item_name));
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.EDIT_PROCESS", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Int32)).Value = item_id;
                        cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = item_name;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isUse;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        return Json(new { success = true, responseText = "SUCCES", Message = "แก้ไขข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR", Message = "ข้อมูลนี้มีอยู่แล้ว ไม่สามารถเพิ่มซ้ำได้" }, JsonRequestBehavior.AllowGet);
                    }
                }
                
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
        public ActionResult DeleteItem_Process(int item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {

                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd = new OracleCommand("CP_SYSTEM.DELETE_PROCESS", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Int32)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                return Json(new { success = true, responseText = "SUCCES", Message = "ลบข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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

        // ------------------------------------------- EDIT ------------------------------------------- //
        public ActionResult Edit()
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
                return RedirectToAction("index", "Home");
            }

            ViewBag.Data = ARR;

            return View();
        }
        public ActionResult ListItem_Edit()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<MOLD_NO> result = new List<MOLD_NO>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT CP_EDIT.*, CENTER_TM_EMPLOYEE.GNAME_ENG,CENTER_TM_EMPLOYEE.FNAME_ENG FROM CP_EDIT LEFT JOIN CENTER_TM_EMPLOYEE ON CP_EDIT.UPDATE_BY = CENTER_TM_EMPLOYEE.EMP_NO " +
                    "WHERE GROUP_ID = '" + gT + "'";

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
                            var create_by = dtreader["CREATE_BY"].ToString();
                            DateTime update_date = Convert.ToDateTime(dtreader["UPDATE_DATE"]);
                            var update_by = dtreader["UPDATE_BY"].ToString();

                            result.Add(new MOLD_NO
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                                GNAME_ENG = dtreader["GNAME_ENG"].ToString(),
                                FNAME_ENG = dtreader["FNAME_ENG"].ToString(),
                                UPDATE_BY = string.IsNullOrEmpty(update_by) ? create_by : update_by,
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy HH:mm") : update_date.ToString("dd/MM/yy HH:mm"),
                                TOOL = "-",
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
        public ActionResult Option_Edit()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<MOLD_NO> result = new List<MOLD_NO>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT ID, NAME FROM CP_EDIT WHERE IS_USE = 'Y' and  GROUP_ID = '" + gT + "'";

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
                            result.Add(new MOLD_NO
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
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
        public ActionResult SelectItem_Edit(string item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<MOLD_NO> result = new List<MOLD_NO>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT * FROM CP_EDIT WHERE ID = " + item_id;

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;

                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        if (dtreader.Read())
                        {
                            DateTime create_date = Convert.ToDateTime(dtreader["CREATE_DATE"]);
                            var create_by = dtreader["CREATE_BY"].ToString();
                            DateTime update_date = Convert.ToDateTime(dtreader["UPDATE_DATE"]);
                            var update_by = dtreader["UPDATE_BY"].ToString();

                            result.Add(new MOLD_NO
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                                UPDATE_BY = string.IsNullOrEmpty(update_by) ? create_by : update_by,
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy") : update_date.ToString("dd/MM/yy"),

                            });
                        }
                        return Json(new { success = true, responseText = "SUCCESS", data = result }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult AddItem_Edit(string item_name, string item_isUse)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";

                command = "SELECT * FROM CP_EDIT WHERE NAME = ':item_name' AND GROUP_ID = ':gT' ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":item_name", item_name));
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.ADD_EDIT", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = item_name;
                        cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isUse;
                        cmd.Parameters.Add(new OracleParameter("pCREATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pCREATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        return Json(new { success = true, responseText = "SUCCES", Message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR", Message = "ข้อมูลนี้มีอยู่แล้ว ไม่สามารถเพิ่มซ้ำได้" }, JsonRequestBehavior.AllowGet);
                    }
                }
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
        public ActionResult EditItem_Edit(string item_name, int item_id, string item_isUse)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";

                command = "SELECT * FROM CP_EDIT WHERE NAME = ':item_name' AND ID <> ':item_id' AND GROUP_ID = ':gT' ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":item_id", item_id));
                cmd.Parameters.Add(new OracleParameter(":item_name", item_name));
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.EDIT_EDIT", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Int32)).Value = item_id;
                        cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = item_name;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isUse;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        return Json(new { success = true, responseText = "SUCCES", Message = "แก้ไขข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR", Message = "ข้อมูลนี้มีอยู่แล้ว ไม่สามารถเพิ่มซ้ำได้" }, JsonRequestBehavior.AllowGet);
                    }
                }                
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
        public ActionResult DeleteItem_Edit(int item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {

                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd = new OracleCommand("CP_SYSTEM.DELETE_EDIT", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Int32)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                return Json(new { success = true, responseText = "SUCCES", Message = "ลบข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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

        // ------------------------------------------- CHANGE ------------------------------------------- //
        public ActionResult Change()
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
                return RedirectToAction("index", "Home");
            }

            ViewBag.Data = ARR;

            return View();
        }
        public ActionResult ListItem_Change()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<MOLD_NO> result = new List<MOLD_NO>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT CP_CHANGE.*, CENTER_TM_EMPLOYEE.GNAME_ENG,CENTER_TM_EMPLOYEE.FNAME_ENG FROM CP_CHANGE LEFT JOIN CENTER_TM_EMPLOYEE ON CP_CHANGE.UPDATE_BY = CENTER_TM_EMPLOYEE.EMP_NO " +
                    "WHERE GROUP_ID = '" + gT + "'";

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
                            var create_by = dtreader["CREATE_BY"].ToString();
                            DateTime update_date = Convert.ToDateTime(dtreader["UPDATE_DATE"]);
                            var update_by = dtreader["UPDATE_BY"].ToString();

                            result.Add(new MOLD_NO
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                                GNAME_ENG = dtreader["GNAME_ENG"].ToString(),
                                FNAME_ENG = dtreader["FNAME_ENG"].ToString(),
                                UPDATE_BY = string.IsNullOrEmpty(update_by) ? create_by : update_by,
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy HH:mm") : update_date.ToString("dd/MM/yy HH:mm"),
                                TOOL = "-",
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
        public ActionResult Option_Change()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<MOLD_NO> result = new List<MOLD_NO>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT ID, NAME FROM CP_CHANGE WHERE IS_USE = 'Y' and GROUP_ID = '" + gT + "'";

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
                            result.Add(new MOLD_NO
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
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
        public ActionResult SelectItem_Change(string item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<MOLD_NO> result = new List<MOLD_NO>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT * FROM CP_CHANGE WHERE ID = " + item_id;

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;

                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        if (dtreader.Read())
                        {
                            DateTime create_date = Convert.ToDateTime(dtreader["CREATE_DATE"]);
                            var create_by = dtreader["CREATE_BY"].ToString();
                            DateTime update_date = Convert.ToDateTime(dtreader["UPDATE_DATE"]);
                            var update_by = dtreader["UPDATE_BY"].ToString();

                            result.Add(new MOLD_NO
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                                UPDATE_BY = string.IsNullOrEmpty(update_by) ? create_by : update_by,
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy") : update_date.ToString("dd/MM/yy"),

                            });
                        }
                        return Json(new { success = true, responseText = "SUCCESS", data = result }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult AddItem_Change(string item_name, string item_isUse)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";

                command = "SELECT * FROM CP_CHANGE WHERE NAME = ':item_name' AND GROUP_ID = ':gT' ";
                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":item_name", item_name));
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.ADD_CHANGE", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = item_name;
                        cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isUse;
                        cmd.Parameters.Add(new OracleParameter("pCREATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pCREATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        return Json(new { success = true, responseText = "SUCCES", Message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR", Message = "ข้อมูลนี้มีอยู่แล้ว ไม่สามารถเพิ่มซ้ำได้" }, JsonRequestBehavior.AllowGet);
                    }
                }
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
        public ActionResult EditItem_Change(string item_name, int item_id, string item_isUse)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";

                command = "SELECT * FROM CP_CHANGE WHERE NAME = ':item_name' AND ID <> ':item_id' AND GROUP_ID = ':gT' ";
                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":item_id", item_id));
                cmd.Parameters.Add(new OracleParameter(":item_name", item_name));
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.EDIT_CHANGE", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Int32)).Value = item_id;
                        cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = item_name;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isUse;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        return Json(new { success = true, responseText = "SUCCES", Message = "แก้ไขข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR", Message = "ข้อมูลนี้มีอยู่แล้ว ไม่สามารถเพิ่มซ้ำได้" }, JsonRequestBehavior.AllowGet);
                    }
                }                
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
        public ActionResult DeleteItem_Change(int item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {

                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd = new OracleCommand("CP_SYSTEM.DELETE_CHANGE", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Int32)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                return Json(new { success = true, responseText = "SUCCES", Message = "ลบข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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

        // ------------------------------------------- PART ------------------------------------------- //

        public ActionResult Part()
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
                return RedirectToAction("index", "Home");
            }

            ViewBag.Data = ARR;

            return View();
        }
        public ActionResult ListItem_Part()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<MOLD_NO> result = new List<MOLD_NO>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT m.*, e.GNAME_ENG,e.FNAME_ENG FROM CP_PART m " +
                    "LEFT JOIN CENTER_TM_EMPLOYEE e ON m.UPDATE_BY = e.EMP_NO " +
                    "WHERE GROUP_ID = '" + gT + "'";


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
                            var create_by = dtreader["CREATE_BY"].ToString();
                            DateTime update_date = Convert.ToDateTime(dtreader["UPDATE_DATE"]);
                            var update_by = dtreader["UPDATE_BY"].ToString(); 

                            result.Add(new MOLD_NO
                            {
                                ITEM_ID = dtreader["PART_NO"].ToString(),
                                ITEM_NAME = dtreader["PART_NAME"].ToString(),
                                GNAME_ENG = dtreader["GNAME_ENG"].ToString(),
                                FNAME_ENG = dtreader["FNAME_ENG"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                                UPDATE_BY = string.IsNullOrEmpty(update_by) ? create_by : update_by,
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy HH:mm") : update_date.ToString("dd/MM/yy HH:mm"),
                                TOOL = "-",

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
        public ActionResult Option_Part()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<PART_OPTION> result = new List<PART_OPTION>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT m.* FROM CP_PART m " +
                    "WHERE IS_USE = 'Y' and GROUP_ID = '" + gT + "'";

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
                            result.Add(new PART_OPTION
                            {
                                ITEM_ID = dtreader["PART_NO"].ToString(),
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
        public ActionResult SelectItem_Part(string item_part)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<MOLD_NO> result = new List<MOLD_NO>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT PART_NO, PART_NAME, IS_USE FROM CP_PART WHERE PART_NO = '" + item_part + "'";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;

                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        if (dtreader.Read())
                        {
                            result.Add(new MOLD_NO
                            {
                                ITEM_ID = dtreader["PART_NO"].ToString(),
                                ITEM_NAME = dtreader["PART_NAME"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                            });
                        }
                        return Json(new { success = true, responseText = "SUCCESS", data = result }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult AddItem_Part(string item_part, string item_name, string item_isuse)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";
                string pattern = @"^[A-Za-z\d]{3}-[A-Za-z\d]{4}-[A-Za-z\d]{3}$"; // รูปแบบของข้อมูล: xxx-xxxx-xxx

                if (Regex.IsMatch(item_part, pattern))
                {
                    command = "SELECT * FROM CP_PART WHERE PART_NO = ':item_part' AND GROUP_ID = ':gT' ";
                    cmd.Connection = conn;
                    cmd.CommandText = command;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new OracleParameter(":item_part", item_part));
                    cmd.Parameters.Add(new OracleParameter(":gT", gT));
                    conn.Open();

                    using (OracleDataReader dtreader = cmd.ExecuteReader())
                    {
                        if (!dtreader.HasRows)
                        {
                            cmd = new OracleCommand("CP_SYSTEM.ADD_PART", conn);
                            cmd.Parameters.Add(new OracleParameter("pITEM_PART", OracleDbType.Varchar2)).Value = item_part;
                            cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = item_name;
                            cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                            cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isuse;
                            cmd.Parameters.Add(new OracleParameter("pCREATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                            cmd.Parameters.Add(new OracleParameter("pCREATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                            cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                            cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.ExecuteNonQuery();

                            return Json(new { success = true, responseText = "SUCCES", Message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = false, responseText = "ERROR", Message = "ข้อมูลนี้มีอยู่แล้ว ไม่สามารถเพิ่มซ้ำได้" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                } else
                {
                    return Json(new { success = false, responseText = "ERROR", Message = "รูปแบบของ Part ไม่ถูกต้อง" }, JsonRequestBehavior.AllowGet);
                }
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
        public ActionResult EditItem_Part(string item_part, string item_name, string item_isuse)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();

                conn.Open();
                cmd = new OracleCommand("CP_SYSTEM.EDIT_PART", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_PART", OracleDbType.Varchar2)).Value = item_part;
                cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = item_name;
                cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isuse;
                cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                return Json(new { success = true, responseText = "SUCCES", Message = "แก้ไขข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult DeleteItem_Part(string item_part)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);
            var gT = Session["GROUP_ID"].ToString();

            try
            {

                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd = new OracleCommand("CP_SYSTEM.DELETE_PART", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_PART", OracleDbType.Varchar2)).Value = item_part;
                cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                return Json(new { success = true, responseText = "SUCCES", Message = "ลบข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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
        public class LIST_PART
        {
            public string PART_NO { get; set; }
            public string PART_NAME { get; set; }
            public string IS_USE { get; set; }
        }
        public ActionResult export_Part()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);
            string wf = System.Web.HttpContext.Current.Server.MapPath("~/src/format/Plan_format.csv");
            StringBuilder sb = new StringBuilder();
            var gT = Session["GROUP_ID"].ToString();

            try
            {

                List<LIST_PART> result = new List<LIST_PART>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                command = "SELECT m.*, e.GNAME_ENG,e.FNAME_ENG FROM CP_PART m " +
                    "LEFT JOIN CENTER_TM_EMPLOYEE e ON m.UPDATE_BY = e.EMP_NO " +
                    "WHERE GROUP_ID = '" + gT + "'";


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
                            var create_by = dtreader["CREATE_BY"].ToString();
                            DateTime update_date = Convert.ToDateTime(dtreader["UPDATE_DATE"]);
                            var update_by = dtreader["UPDATE_BY"].ToString();

                            result.Add(new LIST_PART
                            {
                                PART_NO = dtreader["PART_NO"].ToString(),
                                PART_NAME = dtreader["PART_NAME"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),

                            });
                        }
                        //return Json(new { data = result, success = true, responseText = "" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //return Json(new { success = false, responseText = "" }, JsonRequestBehavior.AllowGet);
                    }
                }

                // Excel

                sb.AppendLine(string.Join(",", "PART_NO", "PART_NAME"));


                //List<List_Plan> result

                var record_csv = result.Where(s => s.IS_USE != "N").Select(s => new
                {
                    PART_NO = s.PART_NO,
                    PART_NAME = s.PART_NAME,
                }).ToList();

                foreach (DataRow r in Util.ToDataTable(record_csv).Rows)
                {
                    IEnumerable<string> fields = r.ItemArray.Select(field => field.ToString());
                    sb.AppendLine(string.Join(",", fields));
                }

                return Json(new { success = true, responseText = "SUCCES", Message = "สร้างไฟล์สำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "FAILED", data = "", Message = "ERROR:" + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                System.IO.File.WriteAllText(wf, sb.ToString(), Encoding.UTF8);
            }
        }

        // ------------------------------------------- M/C ------------------------------------------- //

        public ActionResult Machine()
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
                return RedirectToAction("index", "Home");
            }

            ViewBag.Data = ARR;

            return View();
        }
        public ActionResult ListItem_MC()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<MC> result = new List<MC>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT m.* , e.GNAME_ENG,e.FNAME_ENG FROM CP_MACHINE m " +
                    "LEFT JOIN CENTER_TM_EMPLOYEE e ON m.UPDATE_BY = e.EMP_NO " +
                    "WHERE m.GROUP_ID = '" + gT + "'";

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
                            var create_by = dtreader["CREATE_BY"].ToString();
                            DateTime update_date = Convert.ToDateTime(dtreader["UPDATE_DATE"]);
                            var update_by = dtreader["UPDATE_BY"].ToString();

                            result.Add(new MC
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["MACHINE_NO"].ToString(),
                                ITEM_SIZETON = dtreader["SIZE_TON"].ToString(),
                                GNAME_ENG = dtreader["GNAME_ENG"].ToString(),
                                FNAME_ENG = dtreader["FNAME_ENG"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                                UPDATE_BY = string.IsNullOrEmpty(update_by) ? create_by : update_by,
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy HH:mm") : update_date.ToString("dd/MM/yy HH:mm"),
                                TOOL = "-",
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
        public ActionResult Option_MC()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<MC> result = new List<MC>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT m.* FROM CP_MACHINE m " +
                    "WHERE IS_USE = 'Y' and GROUP_ID = '" + gT + "'";

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
                            result.Add(new MC
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["MACHINE_NO"].ToString(),
                                ITEM_SIZETON = dtreader["SIZE_TON"].ToString(),
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
        public ActionResult SelectItem_Machine(string item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<MC> result = new List<MC>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT * FROM CP_MACHINE WHERE ID = '" + item_id + "'";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;

                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        if (dtreader.Read())
                        {
                            result.Add(new MC
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["MACHINE_NO"].ToString(),
                                ITEM_SIZETON = dtreader["SIZE_TON"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                            });
                        }
                        return Json(new { success = true, responseText = "SUCCESS", data = result }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult AddItem_Machine(string item_mc, string item_ton, string item_isUse)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";

                command = "SELECT * FROM CP_MACHINE " +
                    "WHERE MACHINE_NO = ':item_mc' AND SIZE_TON = ':item_ton' AND GROUP_ID = 'gT' ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":item_mc", item_mc));
                cmd.Parameters.Add(new OracleParameter(":item_ton", item_ton));
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.ADD_MACHINE", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_MC", OracleDbType.Varchar2)).Value = item_mc;
                        cmd.Parameters.Add(new OracleParameter("pITEM_TON", OracleDbType.Varchar2)).Value = item_ton;
                        cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isUse;
                        cmd.Parameters.Add(new OracleParameter("pCREATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pCREATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        return Json(new { success = true, responseText = "SUCCES", Message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR", Message = "ข้อมูลนี้มีอยู่แล้ว ไม่สามารถเพิ่มซ้ำได้" }, JsonRequestBehavior.AllowGet);
                    }
                }
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
        public ActionResult EditItem_Machine(string item_id, string item_mc, string item_ton, string item_isuse)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";

                command = "SELECT * FROM CP_MACHINE " +
                    "WHERE MACHINE_NO = ':item_mc' AND SIZE_TON = ':item_ton' AND GROUP_ID = 'gT' AND ID <> ':item_id' ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":item_id", item_id));
                cmd.Parameters.Add(new OracleParameter(":item_mc", item_mc));
                cmd.Parameters.Add(new OracleParameter(":item_ton", item_ton));
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.EDIT_MACHINE", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Varchar2)).Value = item_id;
                        cmd.Parameters.Add(new OracleParameter("pITEM_MC", OracleDbType.Varchar2)).Value = item_mc;
                        cmd.Parameters.Add(new OracleParameter("pITEM_TON", OracleDbType.Varchar2)).Value = item_ton;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isuse;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        return Json(new { success = true, responseText = "SUCCES", Message = "แก้ไขข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR", Message = "ข้อมูลนี้มีอยู่แล้ว ไม่สามารถเพิ่มซ้ำได้" }, JsonRequestBehavior.AllowGet);
                    }
                }
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
        public ActionResult DeleteItem_Machine(int item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {

                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd = new OracleCommand("CP_SYSTEM.DELETE_MACHINE", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Int32)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                return Json(new { success = true, responseText = "SUCCES", Message = "ลบข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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

        // ------------------------------------------- COLORS ------------------------------------------- //
        public ActionResult Colors()
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
                return RedirectToAction("index", "Home");
            }

            ViewBag.Data = ARR;

            return View();
        }
        public class COLORS
        {
            public string ITEM_COLORS_1 { get; set; }
            public string ITEM_COLORS_2 { get; set; }
            public string ITEM_COLORS_3 { get; set; }
            public string ITEM_COLORS_4 { get; set; }
            public string ITEM_COLORS_5 { get; set; }
        }
        public ActionResult SelectItem_Colors()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<COLORS> result = new List<COLORS>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT COLOR_1, COLOR_2, COLOR_3, COLOR_4, COLOR_5  FROM CP_GROUP WHERE ID = :gT ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        if (dtreader.Read())
                        {
                            result.Add(new COLORS
                            {
                                ITEM_COLORS_1 = dtreader["COLOR_1"].ToString(),
                                ITEM_COLORS_2 = dtreader["COLOR_2"].ToString(),
                                ITEM_COLORS_3 = dtreader["COLOR_3"].ToString(),
                                ITEM_COLORS_4 = dtreader["COLOR_4"].ToString(),
                                ITEM_COLORS_5 = dtreader["COLOR_5"].ToString(),
                            });
                        }
                        return Json(new { success = true, responseText = "SUCCESS", data = result }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult Edit_Colors(string item_color_1, string item_color_2, string item_color_3, string item_color_4, string item_color_5)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();

                conn.Open();
                cmd = new OracleCommand("CP_SYSTEM.EDIT_COLORS", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                cmd.Parameters.Add(new OracleParameter("pITEM_COLOR_1", OracleDbType.Varchar2)).Value = item_color_1;
                cmd.Parameters.Add(new OracleParameter("pITEM_COLOR_2", OracleDbType.Varchar2)).Value = item_color_2;
                cmd.Parameters.Add(new OracleParameter("pITEM_COLOR_3", OracleDbType.Varchar2)).Value = item_color_3;
                cmd.Parameters.Add(new OracleParameter("pITEM_COLOR_4", OracleDbType.Varchar2)).Value = item_color_4;
                cmd.Parameters.Add(new OracleParameter("pITEM_COLOR_5", OracleDbType.Varchar2)).Value = item_color_5;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                return Json(new { success = true, responseText = "SUCCES", Message = "แก้ไขข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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

        // ------------------------------------------- Detail ------------------------------------------- //

        public ActionResult Detail()
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
                return RedirectToAction("index", "Home");
            }
            ViewBag.Data = ARR;

            return View();
        }
        public ActionResult ListItem_Details(string filter_type, string filter_category)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<DETAILS> result = new List<DETAILS>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT m.*, e.GNAME_ENG,e.FNAME_ENG, c.NAME as CATEGORY_NAME " +
                    "FROM CP_DETAILS m " +
                    "LEFT JOIN CP_DETAILS_CATEGORY c ON m.CATEGORY_ID = c.ID " +
                    "LEFT JOIN CENTER_TM_EMPLOYEE e ON m.UPDATE_BY = e.EMP_NO " +
                    "WHERE m.GROUP_ID = '" + gT + "'";

                command += filter_type != "" ? " AND c.TYPE in (" + filter_type + ") " : "";
                command += filter_category != "" ? " AND m.CATEGORY_ID in (" + filter_category + ") " : "";


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
                            var create_by = dtreader["CREATE_BY"].ToString();
                            DateTime update_date = Convert.ToDateTime(dtreader["UPDATE_DATE"]);
                            var update_by = dtreader["UPDATE_BY"].ToString();

                            result.Add(new DETAILS
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_DETAIL = dtreader["DETAIL"].ToString(),
                                ITEM_RISK = dtreader["RISK"].ToString(),
                                ITEM_CATEGORY_ID = dtreader["CATEGORY_ID"].ToString(),
                                ITEM_CATEGORY_NAME = dtreader["CATEGORY_NAME"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                                GNAME_ENG = dtreader["GNAME_ENG"].ToString(),
                                FNAME_ENG = dtreader["FNAME_ENG"].ToString(),
                                UPDATE_BY = string.IsNullOrEmpty(update_by) ? create_by : update_by,
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy HH:mm") : update_date.ToString("dd/MM/yy HH:mm"),
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
        public ActionResult Option_Details(string filter_type)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<DETAILS_OPTION> result = new List<DETAILS_OPTION>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT m.*, c.NAME as CATEGORY_NAME FROM CP_DETAILS m " +
                        "LEFT JOIN CP_DETAILS_CATEGORY c ON m.CATEGORY_ID = c.ID " +
                        "WHERE m.IS_USE = 'Y' AND m.GROUP_ID = '" + gT + "' AND c.GROUP_ID = '" + gT + "' " +
                        "and c.TYPE = '" + filter_type + "' ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                //cmd.Parameters.Add(new OracleParameter(":gT", gT));
                //cmd.Parameters.Add(new OracleParameter(":filter_type", filter_type));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        while (dtreader.Read())
                        {
                            result.Add(new DETAILS_OPTION
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_DETAIL = dtreader["DETAIL"].ToString(),
                                ITEM_RISK = dtreader["RISK"].ToString(),
                                ITEM_CATEGORY_NAME = dtreader["CATEGORY_NAME"].ToString(),
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
        public ActionResult SelectItem_Details(string item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<DETAILS> result = new List<DETAILS>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT * FROM CP_DETAILS WHERE ID = :item_id ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":item_id", item_id));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        if (dtreader.Read())
                        {
                            result.Add(new DETAILS
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_DETAIL = dtreader["DETAIL"].ToString(),
                                ITEM_RISK = dtreader["RISK"].ToString(),
                                ITEM_CATEGORY_ID = dtreader["CATEGORY_ID"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                            });
                        }
                        return Json(new { success = true, responseText = "SUCCESS", data = result }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult AddItem_Details(string item_detail, string item_risk, string item_category, string item_isUse)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";

                command = "SELECT * FROM CP_DETAILS " +
                    "WHERE DETAIL = ':item_detail' AND RISK = ':item_risk' AND CATEGORY_ID = ':item_risk' AND GROUP_ID = ':gT' ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":item_detail", item_detail));
                cmd.Parameters.Add(new OracleParameter(":item_risk", item_risk));
                cmd.Parameters.Add(new OracleParameter(":item_category", item_category));
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.ADD_DETAILS", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_DETAIL", OracleDbType.Varchar2)).Value = item_detail;
                        cmd.Parameters.Add(new OracleParameter("pITEM_RISK", OracleDbType.Varchar2)).Value = item_risk;
                        cmd.Parameters.Add(new OracleParameter("pITEM_CATEGORY", OracleDbType.Varchar2)).Value = item_category;
                        cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isUse;
                        cmd.Parameters.Add(new OracleParameter("pCREATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pCREATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        return Json(new { success = true, responseText = "SUCCES", Message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR", Message = "ข้อมูลนี้มีอยู่แล้ว ไม่สามารถเพิ่มซ้ำได้" }, JsonRequestBehavior.AllowGet);
                    }
                }
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
        public ActionResult EditItem_Details(string item_detail, string item_risk, string item_category, string item_id, string item_isUse)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";

                command = "SELECT * FROM CP_DETAILS " +
                   "WHERE DETAIL = ':item_detail' AND RISK = ':item_risk' AND CATEGORY_ID = ':item_risk' AND GROUP_ID = ':gT' AND ID <> ':item_id'";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":item_id", item_id));
                cmd.Parameters.Add(new OracleParameter(":item_detail", item_detail));
                cmd.Parameters.Add(new OracleParameter(":item_risk", item_risk));
                cmd.Parameters.Add(new OracleParameter(":item_category", item_category));
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.EDIT_DETAILS", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Int32)).Value = item_id;
                        cmd.Parameters.Add(new OracleParameter("pITEM_DETAIL", OracleDbType.Varchar2)).Value = item_detail;
                        cmd.Parameters.Add(new OracleParameter("pITEM_RISK", OracleDbType.Varchar2)).Value = item_risk;
                        cmd.Parameters.Add(new OracleParameter("pITEM_CATEGORY", OracleDbType.Varchar2)).Value = item_category;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isUse;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        return Json(new { success = true, responseText = "SUCCES", Message = "แก้ไขข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR", Message = "ข้อมูลนี้มีอยู่แล้ว ไม่สามารถเพิ่มซ้ำได้" }, JsonRequestBehavior.AllowGet);
                    }
                }

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
        public ActionResult DeleteItem_Details(int item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {

                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd = new OracleCommand("CP_SYSTEM.DELETE_DETAILS", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Int32)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                return Json(new { success = true, responseText = "SUCCES", Message = "ลบข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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

        // ------------------------------------------- DETAIL CATEGORY ------------------------------------------- //
        public ActionResult Detail_Category()
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
                return RedirectToAction("index", "Home");
            }

            ViewBag.Data = ARR;

            return View();
        }
        public ActionResult ListItem_Detail_Category(string filter_type)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<DETAIL_CATEGORY> result = new List<DETAIL_CATEGORY>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT m.*, e.GNAME_ENG,e.FNAME_ENG " +
                    "FROM CP_DETAILS_CATEGORY m " +
                    "LEFT JOIN CENTER_TM_EMPLOYEE e ON m.UPDATE_BY = e.EMP_NO " +
                    "WHERE m.GROUP_ID = '" + gT + "'";

                command += filter_type != "" ? " AND m.TYPE in (" + filter_type + ") " : "";

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
                            var create_by = dtreader["CREATE_BY"].ToString();
                            DateTime update_date = Convert.ToDateTime(dtreader["UPDATE_DATE"]);
                            var update_by = dtreader["UPDATE_BY"].ToString();

                            result.Add(new DETAIL_CATEGORY
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
                                ITEM_TYPE = dtreader["TYPE"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                                GNAME_ENG = dtreader["GNAME_ENG"].ToString(),
                                FNAME_ENG = dtreader["FNAME_ENG"].ToString(),
                                UPDATE_BY = string.IsNullOrEmpty(update_by) ? create_by : update_by,
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy HH:mm") : update_date.ToString("dd/MM/yy HH:mm"),
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
        public ActionResult Option_Detail_Category()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<DETAIL_CATEGORY_OPTION> result = new List<DETAIL_CATEGORY_OPTION>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT * FROM CP_DETAILS_CATEGORY WHERE IS_USE = 'Y' AND GROUP_ID = :gT  ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        while (dtreader.Read())
                        {
                            result.Add(new DETAIL_CATEGORY_OPTION
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
                                ITEM_TYPE = dtreader["TYPE"].ToString(),
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
        public ActionResult SelectItem_Detail_Category(string item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<DETAIL_CATEGORY> result = new List<DETAIL_CATEGORY>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT * FROM CP_DETAILS_CATEGORY WHERE ID = '" + item_id + "'";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;

                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        if (dtreader.Read())
                        {
                            result.Add(new DETAIL_CATEGORY
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
                                ITEM_TYPE = dtreader["TYPE"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                            });
                        }
                        return Json(new { success = true, responseText = "SUCCESS", data = result }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult AddItem_Detail_Category(string item_name, string item_type, string item_isUse)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";

                command = "SELECT * FROM CP_DETAILS_CATEGORY " +
                    "WHERE NAME = ':item_name' AND TYPE = ':item_type' AND GROUP_ID = ':gT' ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":item_name", item_name));
                cmd.Parameters.Add(new OracleParameter(":item_type", item_type));
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.ADD_DETAIL_CATEGORY", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = item_name;
                        cmd.Parameters.Add(new OracleParameter("pITEM_TYPE", OracleDbType.Varchar2)).Value = item_type;
                        cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isUse;
                        cmd.Parameters.Add(new OracleParameter("pCREATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pCREATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        return Json(new { success = true, responseText = "SUCCES", Message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR", Message = "ข้อมูลนี้มีอยู่แล้ว ไม่สามารถเพิ่มซ้ำได้" }, JsonRequestBehavior.AllowGet);
                    }
                }
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
        public ActionResult EditItem_Detail_Category(string item_name, string item_type, string item_id, string item_isUse)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";

                command = "SELECT * FROM CP_DETAILS_CATEGORY " +
                    "WHERE NAME = ':item_name' AND TYPE = ':item_type' AND GROUP_ID = ':gT'  AND ID <> ':item_id'";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":item_id", item_id));
                cmd.Parameters.Add(new OracleParameter(":item_name", item_name));
                cmd.Parameters.Add(new OracleParameter(":item_type", item_type));
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.EDIT_DETAIL_CATEGORY", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Int32)).Value = item_id;
                        cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = item_name;
                        cmd.Parameters.Add(new OracleParameter("pITEM_TYPE", OracleDbType.Varchar2)).Value = item_type;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isUse;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        return Json(new { success = true, responseText = "SUCCES", Message = "แก้ไขข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR", Message = "ข้อมูลนี้มีอยู่แล้ว ไม่สามารถเพิ่มซ้ำได้" }, JsonRequestBehavior.AllowGet);
                    }
                }

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
        public ActionResult DeleteItem_Detail_Category(int item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {

                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd = new OracleCommand("CP_SYSTEM.DELETE_DETAIL_CATEGORY", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Int32)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                return Json(new { success = true, responseText = "SUCCES", Message = "ลบข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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

        // ------------------------------------------- APPROVE MEMBER ------------------------------------------- //

        public ActionResult Approve_Member()
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
                return RedirectToAction("index", "Home");
            }

            ViewBag.Data = ARR;

            return View();
        }
        public ActionResult ListItem_Approve_Member()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<APPROVE_MEMBER> result = new List<APPROVE_MEMBER>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT m.*, e.GNAME_ENG,e.FNAME_ENG " +
                    "FROM CP_APPROVE_MEMBER m " +
                    "LEFT JOIN CENTER_TM_EMPLOYEE e ON m.UPDATE_BY = e.EMP_NO " +
                    "WHERE m.GROUP_ID = :gT ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        while (dtreader.Read())
                        {

                            DateTime create_date = Convert.ToDateTime(dtreader["CREATE_DATE"]);
                            var create_by = dtreader["CREATE_BY"].ToString();
                            DateTime update_date = Convert.ToDateTime(dtreader["UPDATE_DATE"]);
                            var update_by = dtreader["UPDATE_BY"].ToString();

                            result.Add(new APPROVE_MEMBER
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_TITLE = dtreader["TITLE"].ToString(),
                                ITEM_MEMBER = dtreader["MEMBER_LIST"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                                GNAME_ENG = dtreader["GNAME_ENG"].ToString(),
                                FNAME_ENG = dtreader["FNAME_ENG"].ToString(),
                                UPDATE_BY = string.IsNullOrEmpty(update_by) ? create_by : update_by,
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy HH:mm") : update_date.ToString("dd/MM/yy HH:mm"),
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
        public ActionResult Option_Approve_Member()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<APPROVE_MEMBER_OPTION> result = new List<APPROVE_MEMBER_OPTION>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT * FROM CP_APPROVE_MEMBER WHERE IS_USE = 'Y' AND GROUP_ID = :gT ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        while (dtreader.Read())
                        {
                            result.Add(new APPROVE_MEMBER_OPTION
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_TITLE = dtreader["TITLE"].ToString(),
                                ITEM_MEMBER = dtreader["MEMBER_LIST"].ToString(),
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
        public ActionResult SelectItem_Approve_Member(string item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<APPROVE_MEMBER> result = new List<APPROVE_MEMBER>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT * FROM CP_APPROVE_MEMBER WHERE ID = :item_id ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.Parameters.Add(new OracleParameter(":item_id", item_id));
                cmd.CommandType = CommandType.Text;

                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        if (dtreader.Read())
                        {
                            result.Add(new APPROVE_MEMBER
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_TITLE = dtreader["TITLE"].ToString(),
                                ITEM_MEMBER = dtreader["MEMBER_LIST"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                            });
                        }
                        return Json(new { success = true, responseText = "SUCCESS", data = result }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult SelectItem_Approve_Member_by_in(string item_id, string item_cid)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<APPROVE_MEMBER> result = new List<APPROVE_MEMBER>();
                

                OracleCommand cmd = new OracleCommand();
                OracleCommand cmd2 = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT * FROM CP_APPROVE_MEMBER WHERE ID IN (" + item_id + ") ";

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
                            var group_member_id = dtreader["ID"];

                            command = "SELECT m.*, e.GNAME_ENG,e.FNAME_ENG, c.STATUS_TYPE, c.ID as CALENDAR_ID, c.CREATE_DATE, c.DATE_CHANGE, c.SHIFT " +
                                        "FROM CP_CALENDAR_CONFIRM m " +
                                        "LEFT JOIN CP_CALENDAR c ON m.CALENDAR_ID = c.ID " +
                                        "LEFT JOIN CENTER_TM_EMPLOYEE e ON m.EMP_NO = e.EMP_NO " +
                                        "WHERE m.GROUP_MEMBER_ID = " + group_member_id + " AND m.CALENDAR_ID = " + item_cid +
                                        " ORDER BY m.IS_CONFIRM DESC";

                            cmd2.Connection = conn;
                            cmd2.CommandText = command;
                            cmd2.CommandType = CommandType.Text;

                            List<CONFIRM_LIST> result2 = new List<CONFIRM_LIST>();

                            using (OracleDataReader dtreader2 = cmd2.ExecuteReader())
                            {
                                if (dtreader2.HasRows)
                                {
                                    while (dtreader2.Read())
                                    {

                                        DateTime create_date = Convert.ToDateTime(dtreader2["CREATE_DATE"]);
                                        DateTime change_point_date = Convert.ToDateTime(dtreader2["DATE_CHANGE"]);

                                        DateTime? confirm_date = null;

                                        if (dtreader2["CONFIRM_DATE"] != DBNull.Value)
                                        {
                                            confirm_date = Convert.ToDateTime(dtreader2["CONFIRM_DATE"]);
                                        }

                                        result2.Add(new CONFIRM_LIST
                                        {
                                            ID = dtreader2["ID"].ToString(),
                                            CALENDAR_ID = dtreader2["CALENDAR_ID"].ToString(),
                                            RESULT = dtreader2["IS_CONFIRM"].ToString(),
                                            GNAME_ENG = dtreader2["GNAME_ENG"].ToString(),
                                            FNAME_ENG = dtreader2["FNAME_ENG"].ToString(),
                                            SHIFT = dtreader2["SHIFT"].ToString(),
                                            TYPE = dtreader2["STATUS_TYPE"].ToString(),
                                            CONFIRM_DATE = confirm_date == null ? "" : confirm_date.Value.ToString("dd/MM/yyyy HH:mm"),
                                            CREATE_DATE = string.IsNullOrEmpty(create_date.ToString()) ? "" : create_date.ToString("dd/MM/yyyy HH:mm"),
                                            CHANGE_POINT_DATE = string.IsNullOrEmpty(change_point_date.ToString()) ? "" : change_point_date.ToString("dd/MM/yyyy"),
                                        });
                                    }
                                }
                            }

                            result.Add(new APPROVE_MEMBER
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_TITLE = dtreader["TITLE"].ToString(),
                                ITEM_MEMBER = dtreader["MEMBER_LIST"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                                CONFIRM_LIST = result2,
                            });
                        }
                        return Json(new { success = true, responseText = "SUCCESS", data = result }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult AddItem_Approve_Member(string item_title, string item_member, string item_isUse)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                string[] member_list = item_member.Split(',');
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";
                string return_id = "";

                command = "SELECT * FROM CP_APPROVE_MEMBER " +
                    "WHERE TITLE = ':item_title' AND GROUP_ID = ':gT' ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.Parameters.Add(new OracleParameter(":item_title", item_title));
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                cmd.CommandType = CommandType.Text;
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.ADD_APPROVE_MEMBER", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_TITLE", OracleDbType.Varchar2)).Value = item_title;
                        cmd.Parameters.Add(new OracleParameter("pITEM_MEMBER", OracleDbType.Varchar2)).Value = item_member;
                        cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isUse;
                        cmd.Parameters.Add(new OracleParameter("pCREATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pCREATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.Parameters.Add(new OracleParameter("RETURN_ID", OracleDbType.Int32)).Direction = ParameterDirection.Output;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        return_id = cmd.Parameters["RETURN_ID"].Value.ToString();

                        foreach (var emp in member_list)
                        {
                            cmd = new OracleCommand("CP_SYSTEM.ADD_APPROVE_MEMBER_ITEM", conn);
                            cmd.Parameters.Add(new OracleParameter("pITEM_APPROVE_MEMBER_ID", OracleDbType.Varchar2)).Value = return_id;
                            cmd.Parameters.Add(new OracleParameter("pITEM_EMP_NO", OracleDbType.Varchar2)).Value = emp;
                            cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.ExecuteNonQuery();
                        }

                        return Json(new { success = true, responseText = "SUCCES", Message = "เพิ่มข้อมูลสำเร็จ " }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR", Message = "ข้อมูลนี้มีอยู่แล้ว ไม่สามารถเพิ่มซ้ำได้" }, JsonRequestBehavior.AllowGet);
                    }
                }
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
        public ActionResult EditItem_Approve_Member(string item_title, string item_member, string item_id, string item_isUse)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                string[] member_list = item_member.Split(',');
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";

                command = "SELECT * FROM CP_APPROVE_MEMBER " +
                    "WHERE TITLE = :item_title AND GROUP_ID = :gT AND ID <> :item_id ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.Parameters.Add(new OracleParameter(":item_title", item_title));
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                cmd.Parameters.Add(new OracleParameter(":item_id", item_id));
                cmd.CommandType = CommandType.Text;
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.EDIT_APPROVE_MEMBER", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Int32)).Value = item_id;
                        cmd.Parameters.Add(new OracleParameter("pITEM_TITLE", OracleDbType.Varchar2)).Value = item_title;
                        cmd.Parameters.Add(new OracleParameter("pITEM_MEMBER", OracleDbType.Varchar2)).Value = item_member;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isUse;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        cmd = new OracleCommand("CP_SYSTEM.DELETE_APPROVE_MEMBER_ITEM", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_APPROVE_MEMBER_ID", OracleDbType.Varchar2)).Value = item_id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        foreach (var emp in member_list)
                        {
                            cmd = new OracleCommand("CP_SYSTEM.ADD_APPROVE_MEMBER_ITEM", conn);
                            cmd.Parameters.Add(new OracleParameter("pITEM_APPROVE_MEMBER_ID", OracleDbType.Varchar2)).Value = item_id;
                            cmd.Parameters.Add(new OracleParameter("pITEM_EMP_NO", OracleDbType.Varchar2)).Value = emp;
                            cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.ExecuteNonQuery();
                        }

                        return Json(new { success = true, responseText = "SUCCES", Message = "แก้ไขข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR", Message = "ข้อมูลนี้มีอยู่แล้ว ไม่สามารถเพิ่มซ้ำได้" }, JsonRequestBehavior.AllowGet);
                    }
                }

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
        public ActionResult DeleteItem_Approve_Member(int item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {

                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd = new OracleCommand("CP_SYSTEM.DELETE_APPROVE_MEMBER", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Int32)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                cmd = new OracleCommand("CP_SYSTEM.DELETE_APPROVE_MEMBER_ITEM", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_APPROVE_MEMBER_ID", OracleDbType.Varchar2)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                return Json(new { success = true, responseText = "SUCCES", Message = "ลบข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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

        // ------------------------------------------- CHECK SHEET ------------------------------------------- //

        public class CHECK_SHEET
        {
            public string ITEM_ID { get; set; }
            public string ITEM_NAME { get; set; }
            public string CREATE_BY { get; set; }
            public string CREATE_DATE { get; set; }
            public string UPDATE_BY { get; set; }
            public string UPDATE_DATE { get; set; }
            public string IS_USE { get; set; }
            public string GNAME_ENG { get; set; }
            public string FNAME_ENG { get; set; }
        }

        public class CHECK_SHEET_ITEM
        {
            public string ITEM_ID { get; set; }
            public string ITEM_TITLE { get; set; }
            public string ITEM_INDEX { get; set; }
            public string ITEM_TEXT_TYPE { get; set; }
            public string ITEM_STATUS { get; set; }
        }

        public ActionResult Check_Sheet()
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
                return RedirectToAction("index", "Home");
            }

            ViewBag.Data = ARR;

            return View();
        }

        public ActionResult ListItem_Check_Sheet()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<CHECK_SHEET> result = new List<CHECK_SHEET>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT m.*, e.GNAME_ENG,e.FNAME_ENG " +
                    "FROM CP_CHECK_SHEET m " +
                    "LEFT JOIN CENTER_TM_EMPLOYEE e ON m.UPDATE_BY = e.EMP_NO " +
                    "WHERE m.GROUP_ID = :gT ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        while (dtreader.Read())
                        {

                            DateTime create_date = Convert.ToDateTime(dtreader["CREATE_DATE"]);
                            var create_by = dtreader["CREATE_BY"].ToString();
                            DateTime update_date = Convert.ToDateTime(dtreader["UPDATE_DATE"]);
                            var update_by = dtreader["UPDATE_BY"].ToString();

                            result.Add(new CHECK_SHEET
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                                GNAME_ENG = dtreader["GNAME_ENG"].ToString(),
                                FNAME_ENG = dtreader["FNAME_ENG"].ToString(),
                                UPDATE_BY = string.IsNullOrEmpty(update_by) ? create_by : update_by,
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy HH:mm") : update_date.ToString("dd/MM/yy HH:mm"),
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
        public ActionResult Option_Check_Sheet()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<CHECK_SHEET> result = new List<CHECK_SHEET>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT * FROM CP_CHECK_SHEET WHERE IS_USE = 'Y' AND GROUP_ID = :gT ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        while (dtreader.Read())
                        {
                            result.Add(new CHECK_SHEET
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
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
        public ActionResult SelectItem_Check_Sheet(string item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<CHECK_SHEET> result = new List<CHECK_SHEET>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT * FROM CP_CHECK_SHEET WHERE ID = :item_id ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.Parameters.Add(new OracleParameter(":item_id", item_id));
                cmd.CommandType = CommandType.Text;

                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        if (dtreader.Read())
                        {
                            result.Add(new CHECK_SHEET
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
                                IS_USE = dtreader["IS_USE"].ToString(),
                            });
                        }
                        return Json(new { success = true, responseText = "SUCCESS", data = result }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR" }, JsonRequestBehavior.AllowGet);
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
        public ActionResult AddItem_Check_Sheet(string item_name, string items, string item_isUse)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);
            //CHECK_SHEET_ITEM
            try
            {
                string[] item_list = items.Split(',');
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";
                string return_id = "";

                command = "SELECT * FROM CP_CHECK_SHEET " +
                    "WHERE NAME = ':item_name' AND GROUP_ID = ':gT' ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":item_name", item_name));
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.ADD_CHECK_SHEET", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = item_name;
                        cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isUse;
                        cmd.Parameters.Add(new OracleParameter("pCREATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pCREATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.Parameters.Add(new OracleParameter("RETURN_ID", OracleDbType.Int32)).Direction = ParameterDirection.Output;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                        
                        return_id = cmd.Parameters["RETURN_ID"].Value.ToString();

                        foreach (var item in item_list)
                        {
                            cmd = new OracleCommand("CP_SYSTEM.ADD_CHECK_SHEET_ITEM", conn);
                            cmd.Parameters.Add(new OracleParameter("pITEM_APPROVE_MEMBER_ID", OracleDbType.Varchar2)).Value = return_id;
                            cmd.Parameters.Add(new OracleParameter("pITEM_EMP_NO", OracleDbType.Varchar2)).Value = item;
                            cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.ExecuteNonQuery();
                        }

                        return Json(new { success = true, responseText = "SUCCES", Message = "เพิ่มข้อมูลสำเร็จ " }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR", Message = "ข้อมูลนี้มีอยู่แล้ว ไม่สามารถเพิ่มซ้ำได้" }, JsonRequestBehavior.AllowGet);
                    }
                }
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
        public ActionResult EditItem_Check_Sheet(string item_name, string items, string item_id, string item_isUse)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                string[] item_list = items.Split(',');
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";

                command = "SELECT * FROM CP_CHECK_SHEET " +
                    "WHERE NAME = ':item_name' AND GROUP_ID = ':gT'  AND ID <> ':item_id'";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new OracleParameter(":item_id", item_id));
                cmd.Parameters.Add(new OracleParameter(":item_name", item_name));
                cmd.Parameters.Add(new OracleParameter(":gT", gT));
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.EDIT_CHECK_SHEET", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Int32)).Value = item_id;
                        cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = item_name;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = item_isUse;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                        cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        cmd = new OracleCommand("CP_SYSTEM.DELETE_CHECK_SHEET_ITEM", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_CHECK_SHEET_ID", OracleDbType.Varchar2)).Value = item_id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        foreach (var item in item_list)
                        {
                            cmd = new OracleCommand("CP_SYSTEM.ADD_CHECK_SHEET_ITEM", conn);
                            cmd.Parameters.Add(new OracleParameter("pITEM_CHECK_SHEET_ID", OracleDbType.Varchar2)).Value = item_id;
                            cmd.Parameters.Add(new OracleParameter("pITEM_EMP_NO", OracleDbType.Varchar2)).Value = item;
                            cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.ExecuteNonQuery();
                        }

                        return Json(new { success = true, responseText = "SUCCES", Message = "แก้ไขข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "ERROR", Message = "ข้อมูลนี้มีอยู่แล้ว ไม่สามารถเพิ่มซ้ำได้" }, JsonRequestBehavior.AllowGet);
                    }
                }

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
        public ActionResult DeleteItem_Check_Sheet(int item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {

                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd = new OracleCommand("CP_SYSTEM.DELETE_APPROVE_MEMBER", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Int32)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                cmd = new OracleCommand("CP_SYSTEM.DELETE_APPROVE_MEMBER_ITEM", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_APPROVE_MEMBER_ID", OracleDbType.Varchar2)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                return Json(new { success = true, responseText = "SUCCES", Message = "ลบข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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


    }
}