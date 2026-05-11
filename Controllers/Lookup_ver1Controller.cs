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
    public class Lookup_ver1Controller : Controller
    {

        Home ARR = new Home();

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
            public string WARNINGS { get; set; }
            public string FILE_NAME { get; set; }
            public string FILE_SIZE { get; set; }
            public string FILE_PATH { get; set; }
            public string FILE_EXTENSION { get; set; }
            public string STATUS_TYPE { get; set; }
            public string MAN_SPOT_NAME_ENG { get; set; }
            public string MAN_SPOT_NAME_THA { get; set; }
            public string MAN_INSTEAD_NAME_ENG { get; set; }
            public string MAN_INSTEAD_NAME_THA { get; set; }
            public string INFORMED { get; set; }
            public string CREATE_BY { get; set; }
            public string CREATE_DATE { get; set; }
            public string GNAME_ENG_CREATE { get; set; }
            public string FNAME_ENG_CREATE { get; set; }

        }
        public ActionResult Index()
        {
            ARR.IsLayout = true;
            ARR.IsLogin = true;

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

        // ------------------------------------------- EMP_NO ------------------------------------------- //
        public ActionResult Member()
        {
            ARR.IsLayout = true;
            ARR.IsLogin = true;

            if (Session["EMP_NO"] == null)
            {
                Session["STATUS"] = "V";
                Session["ROLE"] = "V";
                Session["WC_CODE"] = "";
                Session["GROUP_ID"] = "";
                Response.Redirect(Url.Action("index", "Login"));
            }

            if (Session["ROLE"].ToString() != "A")
            {
                Response.Redirect(Url.Action("index", "home"));
            }

            ViewBag.Data = ARR;

            return View();
        }

        public ActionResult SelectItem_EMP(string item_id)
        {
            try
            {
                List<EMP> result = new List<EMP>();

                using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["PostgreSqlDbConnectionActual"].ConnectionString))
                {
                    conn.Open();

                    string query = $@"SELECT * FROM CENTER_TM_EMPLOYEE WHERE EMP_NO = @empNO ";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.Add(new NpgsqlParameter("empNO", NpgsqlDbType.Varchar)).Value = item_id;

                        using (var dtreader = cmd.ExecuteReader())
                        {
                            if (dtreader.Read())
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

        //public ActionResult SelectItem_EMP(string item_id)
        //{
        //    OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

        //    try
        //    {
        //        List<EMP> result = new List<EMP>();

        //        OracleCommand cmd = new OracleCommand();

        //        string command = "";

        //        //item_id = "026739";


        //        command = "SELECT * FROM CENTER_TM_EMPLOYEE WHERE EMP_NO = '" + item_id + "' ";

        //        cmd.Connection = conn;
        //        cmd.CommandText = command;
        //        cmd.CommandType = CommandType.Text;

        //        conn.Open();

        //        using (OracleDataReader dtreader = cmd.ExecuteReader())
        //        {
        //            if (dtreader.HasRows)
        //            {
        //                if (dtreader.Read())
        //                {

        //                    var emp_no = dtreader["EMP_NO"].ToString();
        //                    var gname_th = dtreader["GNAME_THA"].ToString();
        //                    var fname_th = dtreader["FNAME_THA"].ToString();
        //                    var gname_eng = dtreader["GNAME_ENG"].ToString();
        //                    var fname_eng = dtreader["FNAME_ENG"].ToString();


        //                    result.Add(new EMP
        //                    {
        //                        EMP_NO = emp_no,
        //                        GNAME_TH = gname_th,
        //                        FNAME_TH = fname_th,
        //                        GNAME_ENG = gname_eng,
        //                        FNAME_ENG = fname_eng,
        //                    });
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

                command = "SELECT m.*, e1.GNAME_ENG,e1.FNAME_ENG, e1.GNAME_THA,e1.FNAME_THA FROM CENTER_TM_MEMBER m " +
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
                        cmd.Parameters.Add(new NpgsqlParameter("empNO", NpgsqlDbType.Varchar)).Value = item_id;

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
        //                    });
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
            try
            {
                List<WC_CODE> result = new List<WC_CODE>();

                using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["PostgreSqlDbConnectionActual"].ConnectionString))
                {
                    conn.Open();

                    string query = $@"SELECT DISTINCT WC_CODE, DEPT_ABB_NAME FROM CENTER_TM_EMPLOYEE ORDER BY WC_CODE ASC ";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.Add(new NpgsqlParameter("empNO", NpgsqlDbType.Varchar)).Value = item_id;

                        using (var dtreader = cmd.ExecuteReader())
                        {
                            if (dtreader.Read())
                            {
                                while (dtreader.Read())
                                {
                                    result.Add(new WC_CODE
                                    {
                                        WC = dtreader["WC_CODE"].ToString(),
                                        NAME = dtreader["DEPT_ABB_NAME"].ToString(),
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

        //public ActionResult Option_WC(string item_id)
        //{
        //    OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

        //    try
        //    {
        //        List<WC_CODE> result = new List<WC_CODE>();

        //        OracleCommand cmd = new OracleCommand();

        //        string command = "";

        //        command = "SELECT DISTINCT WC_CODE, DEPT_ABB_NAME FROM CENTER_TM_EMPLOYEE ORDER BY WC_CODE ASC";

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

        //                    result.Add(new WC_CODE
        //                    {
        //                        WC = dtreader["WC_CODE"].ToString(),
        //                        NAME = dtreader["DEPT_ABB_NAME"].ToString(),
        //                    });
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

        public ActionResult ListItem_EMP()
        {
            try
            {
                List<EMP> result = new List<EMP>();

                using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["PostgreSqlDbConnectionActual"].ConnectionString))
                {
                    conn.Open();

                    string query = $@"SELECT m.*, e1.GNAME_ENG,e1.FNAME_ENG, e2.GNAME_ENG as GNAME_UPDATE,e2.FNAME_ENG as FNAME_UPDATE FROM CENTER_TM_MEMBER m 
                    LEFT JOIN CENTER_TM_EMPLOYEE e1 ON m.EMP_NO = e1.EMP_NO 
                    LEFT JOIN CENTER_TM_EMPLOYEE e2 ON m.UPDATE_BY = e2.EMP_NO ";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        using (var dtreader = cmd.ExecuteReader())
                        {
                            if (dtreader.HasRows)
                            {
                                while (dtreader.Read())
                                {

                                    DateTime create_date = Convert.ToDateTime(dtreader["CREATE_DATE"]);
                                    var create_by = dtreader["CREATE_BY"].ToString();
                                    DateTime update_date = Convert.ToDateTime(dtreader["UPDATE_DATE"]);
                                    var update_by = dtreader["UPDATE_BY"].ToString();

                                    result.Add(new EMP
                                    {
                                        EMP_NO = dtreader["EMP_NO"].ToString(),
                                        IS_USE = dtreader["IS_USE"].ToString(),
                                        GNAME_ENG = dtreader["GNAME_ENG"].ToString(),
                                        FNAME_ENG = dtreader["FNAME_ENG"].ToString(),
                                        NAME_UPDATE = dtreader["GNAME_UPDATE"].ToString() + " " + dtreader["FNAME_UPDATE"].ToString(),
                                        FNAME = dtreader["GNAME_ENG"].ToString() + " " + dtreader["FNAME_ENG"].ToString(),
                                        WC_CODE = dtreader["WC_CODE"].ToString(),
                                        TOOL = "-",
                                        UPDATE_BY = string.IsNullOrEmpty(update_by) ? create_by : update_by,
                                        UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy") : update_date.ToString("dd/MM/yy"),

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

        //public ActionResult ListItem_EMP()
        //{
        //    OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

        //    try
        //    {
        //        List<EMP> result = new List<EMP>();

        //        OracleCommand cmd = new OracleCommand();

        //        string command = "";

        //        command = "SELECT m.*, e1.GNAME_ENG,e1.FNAME_ENG, e2.GNAME_ENG as GNAME_UPDATE,e2.FNAME_ENG as FNAME_UPDATE FROM CENTER_TM_MEMBER m " +
        //            "LEFT JOIN CENTER_TM_EMPLOYEE e1 ON m.EMP_NO = e1.EMP_NO " +
        //            "LEFT JOIN CENTER_TM_EMPLOYEE e2 ON m.UPDATE_BY = e2.EMP_NO";

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

        //                    DateTime create_date = Convert.ToDateTime(dtreader["CREATE_DATE"]);
        //                    var create_by = dtreader["CREATE_BY"].ToString();
        //                    DateTime update_date = Convert.ToDateTime(dtreader["UPDATE_DATE"]);
        //                    var update_by = dtreader["UPDATE_BY"].ToString();

        //                    result.Add(new EMP
        //                    {
        //                        EMP_NO = dtreader["EMP_NO"].ToString(),
        //                        IS_USE = dtreader["IS_USE"].ToString(),
        //                        GNAME_ENG = dtreader["GNAME_ENG"].ToString(),
        //                        FNAME_ENG = dtreader["FNAME_ENG"].ToString(),
        //                        NAME_UPDATE = dtreader["GNAME_UPDATE"].ToString() + " " + dtreader["FNAME_UPDATE"].ToString(),
        //                        FNAME = dtreader["GNAME_ENG"].ToString() + " " +dtreader["FNAME_ENG"].ToString(),
        //                        WC_CODE = dtreader["WC_CODE"].ToString() ,
        //                        TOOL = "-",
        //                        UPDATE_BY = string.IsNullOrEmpty(update_by) ? create_by : update_by,
        //                        UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy") : update_date.ToString("dd/MM/yy"),

        //                    });
        //                }
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

        public ActionResult AddItem_Member(string item_emp, string item_role, string item_email, string item_password, string item_wc)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {

                var emp_no = Session["EMP_NO"];

                OracleCommand cmd = new OracleCommand();

                string command = "";

                command = "SELECT * FROM CENTER_TM_MEMBER WHERE EMP_NO = '" + item_emp + "'";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;

                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("MFG2_MEMBER.ADD_MEMBER ", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_EMP", OracleDbType.Varchar2)).Value = item_emp;
                        cmd.Parameters.Add(new OracleParameter("pITEM_ROLE", OracleDbType.Varchar2)).Value = item_role;
                        cmd.Parameters.Add(new OracleParameter("pITEM_EMAIL", OracleDbType.Varchar2)).Value = item_email;
                        cmd.Parameters.Add(new OracleParameter("pITEM_PASSWORD", OracleDbType.Varchar2)).Value = item_password;
                        cmd.Parameters.Add(new OracleParameter("pITEM_WC", OracleDbType.Varchar2)).Value = item_wc;

                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = "Y";
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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
                conn.Open();
                OracleCommand cmd = new OracleCommand();
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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

            if (Session["EMP_NO"] == null)
            {
                Session["STATUS"] = "V";
                Session["ROLE"] = "V";
                Session["WC_CODE"] = "";
                Session["GROUP_ID"] = "";
                Response.Redirect(Url.Action("index", "Login"));
            }

            if (Session["ROLE"].ToString() != "A")
            {
                Response.Redirect(Url.Action("index", "home"));
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
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy") : update_date.ToString("dd/MM/yy"),
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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

                command = "SELECT * FROM CP_GROUP WHERE WC_LIST = '" + item_wc + "'AND ID <> " + item_id;
                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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
            string warning,
            string type,
            string spot_eng,
            string spot_tha,
            string instead_eng,
            string instead_tha
            )
        {

            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            //string dateString = "Thu Aug 24 2023 15:47:42 GMT+0700 (Indochina Time)";
            DateTime dateTime = DateTime.ParseExact(dates, "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz '(Indochina Time)'", System.Globalization.CultureInfo.InvariantCulture);

            List<MOLD_NO> result = new List<MOLD_NO>();
            List<MOLD_NO> result2 = new List<MOLD_NO>();

            try
            {

                var emp_no = Session["EMP_NO"];
                var gT = Session["GROUP_ID"].ToString();
                var return_id = "";

                conn.Open();
                OracleCommand cmd = new OracleCommand();

                //cmd = new OracleCommand("CP_SYSTEM.ADD_CALENDAR", conn);
                //cmd.Parameters.Add(new OracleParameter("pITEM_DATE_CHANGE", OracleDbType.TimeStamp)).Value = dateTime;
                //cmd.Parameters.Add(new OracleParameter("pITEM_MAN_SPOT", OracleDbType.Varchar2)).Value = spot;
                //cmd.Parameters.Add(new OracleParameter("pITEM_MAN_INSTEAD", OracleDbType.Varchar2)).Value = instead;
                //cmd.Parameters.Add(new OracleParameter("pITEM_MC_NO", OracleDbType.Varchar2)).Value = mc_no;
                //cmd.Parameters.Add(new OracleParameter("pITEM_EDIT_POINT", OracleDbType.Varchar2)).Value = edit;
                //cmd.Parameters.Add(new OracleParameter("pITEM_PART_NO", OracleDbType.Varchar2)).Value = part_no;
                //cmd.Parameters.Add(new OracleParameter("pITEM_MOLD_NO", OracleDbType.Varchar2)).Value = mold_no;
                //cmd.Parameters.Add(new OracleParameter("pITEM_CHANGE", OracleDbType.Varchar2)).Value = change;
                //cmd.Parameters.Add(new OracleParameter("pITEM_PROCESS", OracleDbType.Varchar2)).Value = process;
                //cmd.Parameters.Add(new OracleParameter("pITEM_DETAILS", OracleDbType.Varchar2)).Value = detail;
                //cmd.Parameters.Add(new OracleParameter("pITEM_WARNING", OracleDbType.Varchar2)).Value = warning;
                //cmd.Parameters.Add(new OracleParameter("pITEM_TYPE", OracleDbType.Varchar2)).Value = type;
                //cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;

                //cmd.Parameters.Add(new OracleParameter("pITEM_MAN_SPOT_NAME_ENG", OracleDbType.Varchar2)).Value = spot_eng;
                //cmd.Parameters.Add(new OracleParameter("pITEM_MAN_SPOT_NAME_THA", OracleDbType.Varchar2)).Value = spot_tha;
                //cmd.Parameters.Add(new OracleParameter("pITEM_MAN_INSTEAD_NAME_ENG", OracleDbType.Varchar2)).Value = instead_eng;
                //cmd.Parameters.Add(new OracleParameter("pITEM_MAN_INSTEAD_NAME_THA", OracleDbType.Varchar2)).Value = instead_tha;

                //cmd.Parameters.Add(new OracleParameter("pCREATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                //cmd.Parameters.Add(new OracleParameter("pCREATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                //cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                //cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.ExecuteNonQuery();


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
                cmd.Parameters.Add(new OracleParameter("pITEM_ACTION", OracleDbType.Varchar2)).Value = "";
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

                cmd.Parameters.Add(new OracleParameter("pITEM_SHIFT", OracleDbType.Varchar2)).Value = "";
                cmd.Parameters.Add(new OracleParameter("pITEM_SHIFT_TEAM", OracleDbType.Varchar2)).Value = "";
                cmd.Parameters.Add(new OracleParameter("pITEM_REMARK", OracleDbType.Varchar2)).Value = "";
                cmd.Parameters.Add(new OracleParameter("pITEM_RECIPIENT", OracleDbType.Varchar2)).Value = "";

                cmd.Parameters.Add(new OracleParameter("RETURN_ID", OracleDbType.Int32)).Direction = ParameterDirection.Output;

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();


                return_id = cmd.Parameters["RETURN_ID"].Value.ToString();

                Session["ID_FILES"] = return_id;

                //string command = "";

                //command = "select ID from cp_calendar where  id=(select max(ID) from cp_calendar)";

                //cmd.Connection = conn;
                //cmd.CommandText = command;
                //cmd.CommandType = CommandType.Text;

                //using (OracleDataReader dtreader = cmd.ExecuteReader())
                //{
                //    if (dtreader.HasRows)
                //    {
                //        if (dtreader.Read())
                //        {
                //            Session["ID_FILES"] = dtreader["ID"].ToString();

                //            result2.Add(new MOLD_NO
                //            {
                //                ITEM_ID = dtreader["ID"].ToString(),
                //            });
                //        }
                //    }
                //}

                return Json(new { success = true, responseText = "SUCCES", Message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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
            string warning,
            string type,
            string spot_eng,
            string spot_tha,
            string instead_eng,
            string instead_tha,
            string id
            )
        {

            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            //string dateString = "Thu Aug 24 2023 15:47:42 GMT+0700 (Indochina Time)";
            DateTime dateTime = DateTime.ParseExact(dates, "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz '(Indochina Time)'", System.Globalization.CultureInfo.InvariantCulture);

            try
            {
                //Session["ID_FILES"] = id;
                //var idTemp = id != "" ? id: Session["ID_FILES"];
                var emp_no = Session["EMP_NO"];

                if (id == "")
                {
                    id = Session["ID_FILES"].ToString();
                }


                conn.Open();
                OracleCommand cmd = new OracleCommand();

                //cmd = new OracleCommand("CP_SYSTEM.EDIT_CALENDAR", conn);
                //cmd.Parameters.Add(new OracleParameter("pITEM_DATE_CHANGE", OracleDbType.TimeStamp)).Value = dateTime;
                //cmd.Parameters.Add(new OracleParameter("pITEM_MAN_SPOT", OracleDbType.Varchar2)).Value = spot;
                //cmd.Parameters.Add(new OracleParameter("pITEM_MAN_INSTEAD", OracleDbType.Varchar2)).Value = instead;
                //cmd.Parameters.Add(new OracleParameter("pITEM_MC_NO", OracleDbType.Varchar2)).Value = mc_no;
                //cmd.Parameters.Add(new OracleParameter("pITEM_EDIT_POINT", OracleDbType.Varchar2)).Value = edit;
                //cmd.Parameters.Add(new OracleParameter("pITEM_PART_NO", OracleDbType.Varchar2)).Value = part_no;
                //cmd.Parameters.Add(new OracleParameter("pITEM_MOLD_NO", OracleDbType.Varchar2)).Value = mold_no;
                //cmd.Parameters.Add(new OracleParameter("pITEM_CHANGE", OracleDbType.Varchar2)).Value = change;
                //cmd.Parameters.Add(new OracleParameter("pITEM_PROCESS", OracleDbType.Varchar2)).Value = process;
                //cmd.Parameters.Add(new OracleParameter("pITEM_DETAILS", OracleDbType.Varchar2)).Value = detail;
                //cmd.Parameters.Add(new OracleParameter("pITEM_WARNING", OracleDbType.Varchar2)).Value = warning;
                //cmd.Parameters.Add(new OracleParameter("pITEM_TYPE", OracleDbType.Varchar2)).Value = type;
                //cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Varchar2)).Value = id;

                //cmd.Parameters.Add(new OracleParameter("pITEM_MAN_SPOT_NAME_ENG", OracleDbType.Varchar2)).Value = spot_eng;
                //cmd.Parameters.Add(new OracleParameter("pITEM_MAN_SPOT_NAME_THA", OracleDbType.Varchar2)).Value = spot_tha;
                //cmd.Parameters.Add(new OracleParameter("pITEM_MAN_INSTEAD_NAME_ENG", OracleDbType.Varchar2)).Value = instead_eng;
                //cmd.Parameters.Add(new OracleParameter("pITEM_MAN_INSTEAD_NAME_THA", OracleDbType.Varchar2)).Value = instead_tha;

                //cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                //cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.ExecuteNonQuery();

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
                cmd.Parameters.Add(new OracleParameter("pITEM_ACTION", OracleDbType.Varchar2)).Value = "";
                cmd.Parameters.Add(new OracleParameter("pITEM_WARNING", OracleDbType.Varchar2)).Value = warning;
                cmd.Parameters.Add(new OracleParameter("pITEM_TYPE", OracleDbType.Varchar2)).Value = type;
                cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Varchar2)).Value = id;

                cmd.Parameters.Add(new OracleParameter("pITEM_MAN_SPOT_NAME_ENG", OracleDbType.Varchar2)).Value = spot_eng;
                cmd.Parameters.Add(new OracleParameter("pITEM_MAN_SPOT_NAME_THA", OracleDbType.Varchar2)).Value = spot_tha;
                cmd.Parameters.Add(new OracleParameter("pITEM_MAN_INSTEAD_NAME_ENG", OracleDbType.Varchar2)).Value = instead_eng;
                cmd.Parameters.Add(new OracleParameter("pITEM_MAN_INSTEAD_NAME_THA", OracleDbType.Varchar2)).Value = instead_tha;

                cmd.Parameters.Add(new OracleParameter("pUPDATE_BY", OracleDbType.Varchar2)).Value = emp_no;
                cmd.Parameters.Add(new OracleParameter("pUPDATE_DATE", OracleDbType.TimeStamp)).Value = DateTime.Now;

                cmd.Parameters.Add(new OracleParameter("pITEM_SHIFT", OracleDbType.Varchar2)).Value = "";
                cmd.Parameters.Add(new OracleParameter("pITEM_SHIFT_TEAM", OracleDbType.Varchar2)).Value = "";
                cmd.Parameters.Add(new OracleParameter("pITEM_REMARK", OracleDbType.Varchar2)).Value = "";
                cmd.Parameters.Add(new OracleParameter("pITEM_RECIPIENT", OracleDbType.Varchar2)).Value = "";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                Session["ID_FILES"] = "";

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

        public ActionResult DeleteItem_ChangePoint(string item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {

                var idTemp = Session["ID_FILES"];
                var emp_no = Session["EMP_NO"];

                if (item_id == "")
                {
                    item_id = idTemp.ToString();
                }

                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd = new OracleCommand("CP_SYSTEM.DELETE_CALENDAR", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Varchar2)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                cmd = new OracleCommand("CP_SYSTEM.DELETE_FILES_CID", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_ID", OracleDbType.Varchar2)).Value = item_id;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                return Json(new { success = true, responseText = "SUCCES", Message = "ลบข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
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

        public ActionResult ListItem_ChangePoint_Day(string date_select)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<CHANGE_POINT> result = new List<CHANGE_POINT>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT CP_CALENDAR.* FROM CP_CALENDAR WHERE DATE_CHANGE >= to_timestamp('" + date_select + " 00:00:00', 'dd-mm-yyyy hh24:mi:ss') and DATE_CHANGE <= to_timestamp('" + date_select + " 23:59:59', 'dd-mm-yyyy hh24:mi:ss') AND STATUS_TYPE <> 'hide' AND GROUP_ID = '" + gT + "'";

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
                                FILE_NAME = dtreader["FILE_NAME"].ToString(),
                                FILE_SIZE = dtreader["FILE_SIZE"].ToString(),
                                FILE_PATH = dtreader["FILE_PATH"].ToString(),
                                FILE_EXTENSION = dtreader["FILE_EXTENSION"].ToString(),
                                STATUS_TYPE = dtreader["STATUS_TYPE"].ToString(),

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

        public ActionResult ListItem_ChangePoint_All()
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<CHANGE_POINT> result = new List<CHANGE_POINT>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                var gT = Session["GROUP_ID"].ToString();

                command = "SELECT CP_CALENDAR.* FROM CP_CALENDAR WHERE STATUS_TYPE <> 'hide' AND GROUP_ID = '" + gT + "'";

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
                                FILE_NAME = dtreader["FILE_NAME"].ToString(),
                                FILE_SIZE = dtreader["FILE_SIZE"].ToString(),
                                FILE_PATH = dtreader["FILE_PATH"].ToString(),
                                FILE_EXTENSION = dtreader["FILE_EXTENSION"].ToString(),
                                STATUS_TYPE = dtreader["STATUS_TYPE"].ToString(),

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

                var gT = Session["GROUP_ID"].ToString();
                var empNo = Session["EMP_NO"].ToString();

                string command =
                    "SELECT CP_CALENDAR.*, " +
                    "e1.GNAME_ENG as GNAME_ENG_CREATE, e1.FNAME_ENG as FNAME_ENG_CREATE " +
                    "FROM CP_CALENDAR " +
                    "LEFT JOIN CENTER_TM_EMPLOYEE e1 ON CP_CALENDAR.CREATE_BY = e1.EMP_NO " +
                    "WHERE CP_CALENDAR.STATUS_TYPE <> 'hide' " +
                    "AND CP_CALENDAR.GROUP_ID = '" + gT + "' " +
                    "AND CP_CALENDAR.CREATE_BY = '" + empNo + "' " +
                    "ORDER BY CP_CALENDAR.DATE_CHANGE DESC";

                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;

                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    while (dtreader.Read())
                    {
                        var cp = new CHANGE_POINT
                        {
                            ID = dtreader["ID"].ToString(),
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
                            MAN_SPOT_NAME_ENG = dtreader["MAN_SPOT_NAME_ENG"].ToString(),
                            MAN_SPOT_NAME_THA = dtreader["MAN_SPOT_NAME_THA"].ToString(),
                            MAN_INSTEAD_NAME_ENG = dtreader["MAN_INSTEAD_NAME_ENG"].ToString(),
                            MAN_INSTEAD_NAME_THA = dtreader["MAN_INSTEAD_NAME_THA"].ToString(),
                            INFORMED = dtreader["INFORMED"].ToString(),
                            CREATE_BY = dtreader["CREATE_BY"].ToString(),
                            CREATE_DATE = dtreader["CREATE_DATE"].ToString(),
                            GNAME_ENG_CREATE = dtreader["GNAME_ENG_CREATE"].ToString(),
                            FNAME_ENG_CREATE = dtreader["FNAME_ENG_CREATE"].ToString(),
                        };
                        result.Add(cp);
                    }
                    return Json(new { data = result, success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Error Code " + ex.Message }, JsonRequestBehavior.AllowGet);
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

                var idTemp = Session["ID_FILES"];
                var emp_no = Session["EMP_NO"];
                var gT = Session["GROUP_ID"].ToString();

                if (id == "")
                {
                    id = idTemp.ToString();
                }

                conn.Open();
                OracleCommand cmd = new OracleCommand();

                string originalFileName = Path.GetFileName(file.FileName);
                string[] FileName = originalFileName.Split('.');
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string newFileName = FileName[0] + "_" + timestamp + "." + FileName[1];
                string path = Path.Combine(Server.MapPath("~/src/upload"), newFileName);
                file.SaveAs(path);

                cmd = new OracleCommand("CP_SYSTEM.ADD_FILES", conn);
                cmd.Parameters.Add(new OracleParameter("pITEM_CALENDAR_ID", OracleDbType.Varchar2)).Value = id;
                cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = newFileName;
                cmd.Parameters.Add(new OracleParameter("pITEM_TYPE", OracleDbType.Varchar2)).Value = type;
                cmd.Parameters.Add(new OracleParameter("pITEM_SIZE", OracleDbType.Varchar2)).Value = size;
                cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                string command = "";

                command = "select ID,NAME from CP_FILES where  id=(select max(ID) from CP_FILES)";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (dtreader.HasRows)
                    {
                        if (dtreader.Read())
                        {
                            result2.Add(new MOLD_NO
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
                            });
                        }
                    }
                }

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

        public ActionResult SelectItem_Files(string item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {

                var idTemp = Session["ID_FILES"];
                var emp_no = Session["EMP_NO"];

                if (item_id == "")
                {
                    item_id = idTemp.ToString();
                }

                List<FILES> result = new List<FILES>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                command = "SELECT * FROM CP_FILES WHERE CALENDAR_ID = " + item_id;

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
                            result.Add(new FILES
                            {
                                ITEM_ID = dtreader["ID"].ToString(),
                                ITEM_NAME = dtreader["NAME"].ToString(),
                                ITEM_FILE_TYPE = dtreader["FILE_TYPE"].ToString(),
                                ITEM_FILE_SIZE = dtreader["FILE_SIZE"].ToString(),
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


        // ------------------------------------------- MOLD NO ------------------------------------------- //
        public ActionResult Mold_NO()
        {
            ARR.IsLayout = true;
            ARR.IsLogin = true;

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
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy") : update_date.ToString("dd/MM/yy"),
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

                command = "SELECT ID, NAME FROM CP_MOLD_NO WHERE GROUP_ID = '" + gT + "'";


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

        public ActionResult AddItem_Mold_NO(string item_name)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";

                command = "SELECT * FROM CP_MOLD_NO WHERE NAME = '" + item_name + "' AND GROUP_ID = '" + gT + "' ";
                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.ADD_MOLD_NO ", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = item_name;
                        cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = "Y";
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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

                command = "SELECT * FROM CP_MOLD_NO WHERE NAME = '" + item_name + "' AND ID <> " + item_id + " AND GROUP_ID = '" + gT + "' ";
                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy") : update_date.ToString("dd/MM/yy"),
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

                command = "SELECT ID, NAME FROM CP_PROCESS WHERE GROUP_ID = '" + gT + "'";

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

        public ActionResult AddItem_Process(string item_name)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";

                command = "SELECT * FROM CP_PROCESS WHERE NAME = '" + item_name + "' AND GROUP_ID = '" + gT + "' ";
                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.ADD_PROCESS ", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = item_name;
                        cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = "Y";
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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

                command = "SELECT * FROM CP_PROCESS WHERE NAME = '" + item_name + "' AND ID <> " + item_id + " AND GROUP_ID = '" + gT + "' ";
                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy") : update_date.ToString("dd/MM/yy"),
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

                command = "SELECT ID, NAME FROM CP_EDIT WHERE GROUP_ID = '" + gT + "'";

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

        public ActionResult AddItem_Edit(string item_name)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";

                command = "SELECT * FROM CP_EDIT WHERE NAME = '" + item_name + "' AND GROUP_ID = '" + gT + "' ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.ADD_EDIT", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = item_name;
                        cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = "Y";
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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

                command = "SELECT * FROM CP_EDIT WHERE NAME = '" + item_name + "' AND ID <> " + item_id + " AND GROUP_ID = '" + gT + "' ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy") : update_date.ToString("dd/MM/yy"),
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

                command = "SELECT ID, NAME FROM CP_CHANGE WHERE GROUP_ID = '" + gT + "'";

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

        public ActionResult AddItem_Change(string item_name)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";

                command = "SELECT * FROM CP_CHANGE WHERE NAME = '" + item_name + "' AND GROUP_ID = '" + gT + "' ";
                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.ADD_CHANGE", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = item_name;
                        cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = "Y";
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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

                command = "SELECT * FROM CP_CHANGE WHERE NAME = '" + item_name + "' AND ID <> " + item_id + " AND GROUP_ID = '" + gT + "' ";
                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy") : update_date.ToString("dd/MM/yy"),
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

        public ActionResult AddItem_Part(string item_part, string item_name)
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
                    command = "SELECT * FROM CP_PART WHERE PART_NO = '" + item_part + "' AND GROUP_ID = '" + gT + "' ";
                    cmd.Connection = conn;
                    cmd.CommandText = command;
                    cmd.CommandType = CommandType.Text;
                    conn.Open();

                    using (OracleDataReader dtreader = cmd.ExecuteReader())
                    {
                        if (!dtreader.HasRows)
                        {
                            cmd = new OracleCommand("CP_SYSTEM.ADD_PART", conn);
                            cmd.Parameters.Add(new OracleParameter("pITEM_PART", OracleDbType.Varchar2)).Value = item_part;
                            cmd.Parameters.Add(new OracleParameter("pITEM_NAME", OracleDbType.Varchar2)).Value = item_name;
                            cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                            cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = "Y";
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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

        public ActionResult Machince()
        {
            ARR.IsLayout = true;
            ARR.IsLogin = true;

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
                                UPDATE_DATE = string.IsNullOrEmpty(update_by) ? create_date.ToString("dd/MM/yy") : update_date.ToString("dd/MM/yy"),
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

        public ActionResult AddItem_Machine(string item_mc, string item_ton)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                var emp_no = Session["EMP_NO"];
                OracleCommand cmd = new OracleCommand();
                var gT = Session["GROUP_ID"].ToString();
                string command = "";

                command = "SELECT * FROM CP_MACHINE " +
                    "WHERE MACHINE_NO = '" + item_mc + "' AND SIZE_TON = '" + item_ton + "' AND GROUP_ID = '" + gT + "' ";

                cmd.Connection = conn;
                cmd.CommandText = command;
                cmd.CommandType = CommandType.Text;
                conn.Open();

                using (OracleDataReader dtreader = cmd.ExecuteReader())
                {
                    if (!dtreader.HasRows)
                    {
                        cmd = new OracleCommand("CP_SYSTEM.ADD_MACHINE", conn);
                        cmd.Parameters.Add(new OracleParameter("pITEM_MC", OracleDbType.Varchar2)).Value = item_mc;
                        cmd.Parameters.Add(new OracleParameter("pITEM_TON", OracleDbType.Varchar2)).Value = item_ton;
                        cmd.Parameters.Add(new OracleParameter("pITEM_GROUP_ID", OracleDbType.Varchar2)).Value = gT;
                        cmd.Parameters.Add(new OracleParameter("pIS_USE", OracleDbType.Varchar2)).Value = "Y";
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
                return Json(new { success = false, responseText = "Error Code " + ex.Message + " \n Please contact admin." }, JsonRequestBehavior.AllowGet);
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
                 
                conn.Open();
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