using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Services;
using Dapper;
using Change_Point.Infrastructure;
using Change_Point.Repositories.Implementations;
using Change_Point.Repositories.Interfaces;

namespace Change_Point.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILookupRepository _lookupRepo = new LookupRepository();

        private readonly string _cp = GlobalConfig.CpSchema;

        private const string ErrMsg = "พบปัญหา \n กรุณาติดต่อแอดมิน";

        [WebMethod(EnableSession = true)]
        public ActionResult Index(string Id, bool IsLogin = false)
        {
            ViewBag.ParamId  = Id;
            ViewBag.IsLogin  = IsLogin;
            return View();
        }

        public ActionResult Group(string Id, bool IsLogin = false)
        {
            ViewBag.ParamId  = Id;
            ViewBag.IsLogin  = IsLogin;
            return View();
        }

        /// <summary>
        /// Returns OK/NG/Total counts grouped by status_type for a given group and date range.
        /// date_select1 and date_select2 are in dd-MM-yyyy format.
        /// group_id is passed as a query parameter (supports dashboard screens that specify a group).
        /// </summary>
        public ActionResult Get_Result_Dashboard_Today(string date_select1, string date_select2, string group_id)
        {
            try
            {
                if (!int.TryParse(group_id, out int gid))
                    return Json(new { success = false, message = "Invalid group_id" }, JsonRequestBehavior.AllowGet);

                if (!DateTime.TryParseExact(date_select1, "dd-MM-yyyy",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out DateTime dStart))
                    dStart = DateTime.Today;

                if (!DateTime.TryParseExact(date_select2, "dd-MM-yyyy",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out DateTime dEnd))
                    dEnd = DateTime.Today;

                var sql = $@"
                    SELECT status_type AS title,
                           COUNT(CASE WHEN informed = TRUE  THEN 1 END) AS result_ok,
                           COUNT(CASE WHEN informed = FALSE THEN 1 END) AS result_ng,
                           COUNT(*) AS result_total
                    FROM {_cp}.cp_calendar
                    WHERE group_id    = @gid
                      AND status_type <> 'hide'
                      AND date_change >= @dStart
                      AND date_change <= @dEnd
                    GROUP BY status_type";

                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    var rows = conn.Query(sql, new
                    {
                        gid,
                        dStart = dStart.Date,
                        dEnd   = dEnd.Date.AddDays(1).AddSeconds(-1)
                    });

                    var data = System.Linq.Enumerable.Select(rows, r => new
                    {
                        TITLE        = (string)r.title        ?? "",
                        RESULT_OK    = ((long)r.result_ok).ToString(),
                        RESULT_NG    = ((long)r.result_ng).ToString(),
                        RESULT_TOTAL = ((long)r.result_total).ToString()
                    });

                    return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[DashboardController.Get_Result_Dashboard_Today] " + ex.Message);
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListItem_Dashboard(string group_id)
        {
            try
            {
                if (!int.TryParse(group_id, out int gid))
                    return Json(new { success = false, message = "Invalid group_id" }, JsonRequestBehavior.AllowGet);

                var data = _lookupRepo.ListDashboard(gid);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
