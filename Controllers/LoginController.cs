using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Common;
using Oracle.ManagedDataAccess.Client;
using System.Web.Services;
using System.Configuration;

using System.Globalization;
using System.Net.Mail;
using Change_Point.Models;
using System.Net;

namespace Chage_Point.Controllers
{
    public class LoginController : Controller
    {
        Home ARR = new Home();

        public class Member
        {
            public string EMP_NO { get; set; }
            public string GNAME_TH { get; set; }
            public string FNAME_TH { get; set; }
            public string GNAME_ENG { get; set; }
            public string FNAME_ENG { get; set; }
            public string WC_CODE { get; set; }
        }
        public class PM
        {
            public string PM_ID { get; set; }
            public string STATUS { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public ActionResult Index(Boolean IsLogin = false)
        {     

            if (Session["EMP_NO"] != null)
            {
                Response.Redirect(Url.Action("index", "home"));
            }

            Session["WC_CODE"] = "000";

            ARR.IsLayout = false;
            ARR.IsLogin = IsLogin;

            ViewBag.Data = ARR;
            return View();
        }
        public ActionResult get_text(Boolean IsLogin = false)
        {

            if (Session["EMP_NO"] != null)
            {
                Response.Redirect(Url.Action("index", "home"));
            }

            Session["WC_CODE"] = "000";

            ARR.IsLayout = false;
            ARR.IsLogin = IsLogin;

            ViewBag.Data = ARR;
            return View();
        }
        public ActionResult Login(string item_username, string item_password, string action)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                OracleCommand cmd = new OracleCommand();
                List<PM> result = new List<PM>();

                string command = "";
                var wc_code = "";

                if (action == "M")
                {
                    command = "SELECT m.*, e1.GNAME_ENG,e1.FNAME_ENG, e1.GNAME_THA,e1.FNAME_THA FROM CENTER_TM_MEMBER m " +
                   "LEFT JOIN CENTER_TM_EMPLOYEE e1 ON m.EMP_NO = e1.EMP_NO " +
                   " WHERE m.EMP_NO = '" + item_username + "' AND m.PASSWORD = '" + item_password + "' ";
                }
                else
                {
                    command = "SELECT * FROM CENTER_TM_EMPLOYEE WHERE EMP_NO = '" + item_username + "'";
                }

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
                            var fname_th = dtreader["FNAME_THA"].ToString();
                            var gname_th = dtreader["GNAME_THA"].ToString();
                            var fname_eng = dtreader["GNAME_ENG"].ToString();
                            var gname_eng = dtreader["FNAME_ENG"].ToString();
                            wc_code = dtreader["WC_CODE"].ToString();

                            if (action == "M")
                            {
                                var isuse = dtreader["IS_USE"].ToString();

                                if (isuse == "N")
                                {
                                    return Json(new { success = false, responseText = "คุณถูกระงับการใช้งาน  กรุณาติดต่อ Admin" }, JsonRequestBehavior.AllowGet);
                                }
                                Session["EMP_NO"] = emp_no;
                                Session["FNAME_TH"] = fname_th;
                                Session["GNAME_TH"] = gname_th;
                                Session["GNAME_ENG"] = fname_eng;
                                Session["FNAME_ENG"] = gname_eng;
                                Session["ROLE"] = dtreader["ROLE"].ToString();
                                Session["WC_CODE"] = wc_code;
                                Session["STATUS"] = "M";
                            }
                            else
                            {
                                Session["EMP_NO"] = emp_no;
                                Session["FNAME_TH"] = fname_th;
                                Session["GNAME_TH"] = gname_th;
                                Session["GNAME_ENG"] = fname_eng;
                                Session["FNAME_ENG"] = gname_eng;
                                Session["WC_CODE"] = wc_code;
                                Session["ROLE"] = "V";
                                Session["STATUS"] = "V";
                            }

                        }

                        Session["GROUP_ID"] = "";

                        return Json(new { success = true, responseText = "ล็อคอินสำเร็จ", wc_code = wc_code }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "Username หรือ Password ไม่ถูกต้อง", wc_code = wc_code }, JsonRequestBehavior.AllowGet);
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
        public ActionResult Guest()
        {

            Session["EMP_NO"] = "G00000";
            Session["FNAME_TH"] = "Canon";
            Session["GNAME_TH"] = "Visitor";
            Session["GNAME_ENG"] = "Canon";
            Session["FNAME_ENG"] = "Visitor";
            Session["ROLE"] = "G";
            Session["STATUS"] = "G";
            Session["WC_CODE"] = "";
            Session["GROUP_ID"] = "";

            return Json(new { success = true, responseText = "OK" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Logout()
        {

            Session["EMP_NO"] = null;
            Session["FNAME_TH"] = null;
            Session["GNAME_TH"] = null;
            Session["GNAME_ENG"] = null;
            Session["FNAME_ENG"] = null;
            Session["WC_CODE"] = null;
            Session["GROUP_ID"] = null;
            Session["ROLE"] = null;
            Session["STATUS"] = null;

            return RedirectToAction("index", "login");
        }

        public ActionResult Forgot_Password(string item_id)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["htmfg2t"].ConnectionString);

            try
            {
                
                OracleCommand cmd = new OracleCommand();

                string command = "";
                var result = "";

                command = "SELECT EMAIL, PASSWORD FROM CENTER_TM_MEMBER WHERE EMP_NO = '" + item_id + "'";

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
                            string password = dtreader["PASSWORD"].ToString();

                            if (email == "")
                            {
                                result = "ไม่พบ E-mail ของท่าน <br>กรุณาติกต่อ Admin";
                            } else
                            {

                                string subject = "[HT-MFG 2 Change Point] Your password";
                                string body = "*** Do not reply this E-Mail *** \n";
                                body += "===================== \n";
                                body += "Fogot Password : Change Point System \n";
                                body += "Your Password : " + password;

                                //var mail_user = "HTMFG2_ChagePoint@mail.canon";
                                var mail_user = "ChangePoint_System@mail.canon";
                                var mail_target = email;

                                //Util sendmail = new Util();
                                Util.SendEmail(subject, mail_user, mail_target, "", body);

                                result = "รหัสผ่านถูกส่งไปที่ E-mail : " + email;
                            }
                        }
                        return Json(new { success = true, responseText = "SUCCESS", data = result }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        result = "ไม่พบรหัสพนักงาน";

                        return Json(new { success = false, responseText = "ERROR", data = result }, JsonRequestBehavior.AllowGet);
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

        public ActionResult Lo(Boolean IsLogin = false)
        {

            if (Session["EMP_NO"] != null)
            {
                Response.Redirect(Url.Action("index", "home"));
            }

            ARR.IsLayout = false;
            ARR.IsLogin = IsLogin;

            ViewBag.Data = ARR;
            return View();
        }

    }
}