using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using Dapper;
using Change_Point.Infrastructure;
using Change_Point.Models;

namespace Change_Point.Controllers
{
    public class LoginController : Controller
    {
        private readonly string _main = GlobalConfig.MainSchema;

        private const string ErrMsg = "พบปัญหา \n กรุณาติดต่อแอดมิน";

        // -----------------------------------------------------------------------
        // Index / landing
        // -----------------------------------------------------------------------
        [WebMethod(EnableSession = true)]
        public ActionResult Index(bool IsLogin = false)
        {
            if (Session["EMP_NO"] != null)
                return RedirectToAction("index", "home");

            Session["WC_CODE"] = "000";
            ViewBag.IsLogin    = IsLogin;
            return View();
        }

        public ActionResult get_text(bool IsLogin = false)
        {
            if (Session["EMP_NO"] != null)
                return RedirectToAction("index", "home");

            Session["WC_CODE"] = "000";
            ViewBag.IsLogin    = IsLogin;
            return View();
        }

        public ActionResult Lo(bool IsLogin = false)
        {
            if (Session["EMP_NO"] != null)
                return RedirectToAction("index", "home");

            ViewBag.IsLogin = IsLogin;
            return View();
        }

        // -----------------------------------------------------------------------
        // Login
        // action = "M"  → member login (center_tm_member with password check)
        // action != "M" → visitor / employee login (center_tm_employee lookup only)
        // -----------------------------------------------------------------------
        public ActionResult Login(string item_username, string item_password, string action)
        {
            try
            {
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    if (action == "M")
                    {
                        // Member login: validate against center_tm_member joined to center_tm_employee
                        var sql = $@"
                            SELECT m.emp_no, m.role, m.is_use,
                                   e.gname_tha, e.fname_tha, e.gname_eng, e.fname_eng, e.wc_code
                            FROM {_main}.center_tm_member m
                            LEFT JOIN {_main}.center_tm_employee e ON m.emp_no = e.emp_no
                            WHERE m.emp_no   = @username
                              AND m.password = @password";

                        var row = conn.QueryFirstOrDefault(sql, new
                        {
                            username = item_username ?? "",
                            password = item_password ?? ""
                        });

                        if (row == null)
                            return Json(new { success = false, responseText = "Username หรือ Password ไม่ถูกต้อง", wc_code = "" }, JsonRequestBehavior.AllowGet);

                        string isUse = (string)row.is_use ?? "Y";
                        if (isUse == "N")
                            return Json(new { success = false, responseText = "คุณถูกระงับการใช้งาน  กรุณาติดต่อ Admin" }, JsonRequestBehavior.AllowGet);

                        string wcCode = (string)row.wc_code ?? "";
                        Session["EMP_NO"]    = (string)row.emp_no   ?? "";
                        Session["GNAME_TH"]  = (string)row.gname_tha ?? "";
                        Session["FNAME_TH"]  = (string)row.fname_tha ?? "";
                        Session["GNAME_ENG"] = (string)row.gname_eng ?? "";
                        Session["FNAME_ENG"] = (string)row.fname_eng ?? "";
                        Session["ROLE"]      = (string)row.role      ?? "M";
                        Session["WC_CODE"]   = wcCode;
                        Session["STATUS"]    = "M";
                        Session["GROUP_ID"]  = "";

                        return Json(new { success = true, responseText = "ล็อคอินสำเร็จ", wc_code = wcCode }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        // Employee / visitor login — no password required, just look up by emp_no
                        var sql = $@"
                            SELECT emp_no, gname_tha, fname_tha, gname_eng, fname_eng, wc_code
                            FROM {_main}.center_tm_employee
                            WHERE emp_no = @username";

                        var row = conn.QueryFirstOrDefault(sql, new { username = item_username ?? "" });

                        if (row == null)
                            return Json(new { success = false, responseText = "ไม่พบรหัสพนักงาน", wc_code = "" }, JsonRequestBehavior.AllowGet);

                        string wcCode = (string)row.wc_code ?? "";
                        Session["EMP_NO"]    = (string)row.emp_no   ?? "";
                        Session["GNAME_TH"]  = (string)row.gname_tha ?? "";
                        Session["FNAME_TH"]  = (string)row.fname_tha ?? "";
                        Session["GNAME_ENG"] = (string)row.gname_eng ?? "";
                        Session["FNAME_ENG"] = (string)row.fname_eng ?? "";
                        Session["WC_CODE"]   = wcCode;
                        Session["ROLE"]      = "V";
                        Session["STATUS"]    = "V";
                        Session["GROUP_ID"]  = "";

                        return Json(new { success = true, responseText = "ล็อคอินสำเร็จ", wc_code = wcCode }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // -----------------------------------------------------------------------
        // Guest login (no DB call)
        // -----------------------------------------------------------------------
        public ActionResult Guest()
        {
            Session["EMP_NO"]    = "G00000";
            Session["FNAME_TH"]  = "Canon";
            Session["GNAME_TH"]  = "Visitor";
            Session["GNAME_ENG"] = "Canon";
            Session["FNAME_ENG"] = "Visitor";
            Session["ROLE"]      = "G";
            Session["STATUS"]    = "G";
            Session["WC_CODE"]   = "";
            Session["GROUP_ID"]  = "";
            return Json(new { success = true, responseText = "OK" }, JsonRequestBehavior.AllowGet);
        }

        // -----------------------------------------------------------------------
        // Logout
        // -----------------------------------------------------------------------
        public ActionResult Logout()
        {
            Session["EMP_NO"]    = null;
            Session["FNAME_TH"]  = null;
            Session["GNAME_TH"]  = null;
            Session["GNAME_ENG"] = null;
            Session["FNAME_ENG"] = null;
            Session["WC_CODE"]   = null;
            Session["GROUP_ID"]  = null;
            Session["ROLE"]      = null;
            Session["STATUS"]    = null;
            return RedirectToAction("index", "login");
        }

        // -----------------------------------------------------------------------
        // Forgot Password — looks up email from center_tm_member then sends it
        // -----------------------------------------------------------------------
        public ActionResult Forgot_Password(string item_id)
        {
            try
            {
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    var sql = $"SELECT email, password FROM {_main}.center_tm_member WHERE emp_no = @empNo";
                    var row = conn.QueryFirstOrDefault(sql, new { empNo = item_id ?? "" });

                    if (row == null)
                        return Json(new { success = false, responseText = "ERROR", data = "ไม่พบรหัสพนักงาน" }, JsonRequestBehavior.AllowGet);

                    string email    = (string)row.email    ?? "";
                    string password = (string)row.password ?? "";

                    if (string.IsNullOrEmpty(email))
                        return Json(new { success = true, responseText = "SUCCESS", data = "ไม่พบ E-mail ของท่าน <br>กรุณาติดต่อ Admin" }, JsonRequestBehavior.AllowGet);

                    string subject = "[HT-MFG 2 Change Point] Your password";
                    string body    = "*** Do not reply this E-Mail *** \n" +
                                     "===================== \n" +
                                     "Fogot Password : Change Point System \n" +
                                     "Your Password : " + password;

                    Util.SendEmail(subject, "ChangePoint_System@mail.canon", email, "", body);

                    return Json(new { success = true, responseText = "SUCCESS", data = "รหัสผ่านถูกส่งไปที่ E-mail : " + email }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Error", error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
