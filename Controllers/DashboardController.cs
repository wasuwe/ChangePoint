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

using Change_Point.Models;

namespace Change_Point.Controllers
{
    public class DashboardController : Controller
    {
        Home ARR = new Home();
        // GET: Dashboard

        public class COUNT_DASHBOARD
        {
            public string TITLE { get; set; }
            public string RESULT_OK { get; set; }
            public string RESULT_NG { get; set; }
            public string RESULT_TOTAL { get; set; }
        }

        public class DASHBOARD_SETTING
        {
            public string PRIORITY { get; set; }
            public string TITLE { get; set; }
            public string TIME { get; set; }
            public string TYPE { get; set; }
        }

        [WebMethod(EnableSession = true)]

        public ActionResult Index(string Id, Boolean IsLogin = false )
        {

            ARR.IsLayout = false;
            ARR.IsLogin = IsLogin;
            ARR.param_id = Id;

            ViewBag.Data = ARR;
            return View();
        }

        public ActionResult Group(string Id, Boolean IsLogin = false)
        {

            ARR.IsLayout = false;
            ARR.IsLogin = IsLogin;
            ARR.param_id = Id;

            ViewBag.Data = ARR;

            if (Id == "21")
            {
                return View("~/Views/Dashboard/Group_ver1.cshtml");
            }
            else
            {
                return View();
            }

            //return View();
        }

        public ActionResult Get_Result_Dashboard_Today(string date_select1, string date_select2, string group_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<COUNT_DASHBOARD> result = new List<COUNT_DASHBOARD>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                command = "SELECT STATUS_TYPE, " +
                    "COUNT(CASE WHEN INFORMED = 'Y' THEN 1 ELSE NULL END) as RESULT_OK, " +
                    "COUNT(CASE WHEN INFORMED = 'N' THEN 1 ELSE NULL END) as RESULT_NG, " +
                    "COUNT(CASE WHEN INFORMED = 'N' OR INFORMED = 'Y' THEN 1 ELSE NULL END) as RESULT_TOTAL " +
                    "FROM CP_CALENDAR " +
                    "WHERE DATE_CHANGE >= to_timestamp('" + date_select1 + " 00:00:00', 'dd-mm-yyyy hh24:mi:ss') " +
                    "and DATE_CHANGE <= to_timestamp('" + date_select2 + " 23:59:59', 'dd-mm-yyyy hh24:mi:ss') " +
                    "AND GROUP_ID = '" + group_id + "' " +
                    "GROUP BY STATUS_TYPE ";

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

                            result.Add(new COUNT_DASHBOARD
                            {
                                TITLE = dtreader["STATUS_TYPE"].ToString(),
                                RESULT_OK = dtreader["RESULT_OK"].ToString(),
                                RESULT_NG = dtreader["RESULT_NG"].ToString(),
                                RESULT_TOTAL = dtreader["RESULT_TOTAL"].ToString(),
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

        public ActionResult ListItem_Dashboard(string group_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                List<DASHBOARD_SETTING> result = new List<DASHBOARD_SETTING>();

                OracleCommand cmd = new OracleCommand();

                string command = "";

                command = "SELECT m.* FROM CP_DASHBOARD m " +
                    "WHERE GROUP_ID = " + group_id +
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



    }
}