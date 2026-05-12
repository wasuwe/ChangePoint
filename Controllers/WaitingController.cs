using System;
using System.Web.Mvc;
using Change_Point.Repositories.Implementations;
using Change_Point.Repositories.Interfaces;

namespace Change_Point.Controllers
{
    public class WaitingController : Controller
    {
        private readonly IApproveRepository _approveRepo = new ApproveRepository();

        private string EmpNo => Session["EMP_NO"]?.ToString() ?? "";

        private const string ErrMsg = "พบปัญหา \n กรุณาติดต่อแอดมิน";

        // GET: Waiting
        public ActionResult Index()
        {
            if (Session["EMP_NO"] == null || Session["ROLE"] == null)
            {
                Session["ROLE"]     = "V";
                Session["WC_CODE"]  = "";
                Session["GROUP_ID"] = "";
                return RedirectToAction("index", "Login");
            }
            return View();
        }

        public ActionResult List_Waiting_Confirm(
            string date_start, string date_stop,
            string filter_type, string filter_shift, string filter_status)
        {
            try
            {
                var data = _approveRepo.ListWaiting(
                    EmpNo,
                    date_start   ?? "",
                    date_stop    ?? "",
                    filter_type  ?? "",
                    filter_shift ?? "",
                    filter_status ?? "");

                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Confirm_Change_Point(string item_id)
        {
            try
            {
                if (!int.TryParse(item_id, out int confirmId))
                    return Json(new { success = false, message = "Invalid id" }, JsonRequestBehavior.AllowGet);

                _approveRepo.ConfirmChangePoint(confirmId);
                return Json(new { success = true, message = "Confirm สำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Count_Waiting_Confirm()
        {
            try
            {
                int count = _approveRepo.CountWaiting(EmpNo);
                var result = new[] { new { RESULT = count.ToString() } };
                return Json(new { success = true, data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
