using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Change_Point.Models.DTOs;
using Change_Point.Repositories.Implementations;
using Change_Point.Repositories.Interfaces;

namespace Change_Point.Controllers
{
    public class LookupController : Controller
    {
        // -----------------------------------------------------------------------
        // Repositories — instantiated directly (no IoC container required)
        // -----------------------------------------------------------------------
        private readonly ICalendarRepository  _calRepo   = new CalendarRepository();
        private readonly IGroupRepository     _groupRepo  = new GroupRepository();
        private readonly IFileRepository      _fileRepo   = new FileRepository();
        private readonly IApproveRepository   _approveRepo = new ApproveRepository();
        private readonly ILookupRepository    _lookupRepo = new LookupRepository();

        private const string ErrMsg = "พบปัญหา \n กรุณาติดต่อแอดมิน";

        // -----------------------------------------------------------------------
        // Input model for recipient list posted from the front-end
        // -----------------------------------------------------------------------
        public class RecipientItem
        {
            public string ITEM_ID     { get; set; }
            public string ITEM_TITLE  { get; set; }
            public string ITEM_MEMBER { get; set; }
        }

        // -----------------------------------------------------------------------
        // Auth helper
        // -----------------------------------------------------------------------
        private bool IsAuthenticated()
        {
            return Session["EMP_NO"] != null && Session["ROLE"] != null;
        }

        private string EmpNo   => Session["EMP_NO"]?.ToString()  ?? "";
        private string Role    => Session["ROLE"]?.ToString()     ?? "";
        private string WcCode  => Session["WC_CODE"]?.ToString()  ?? "";
        private int    GroupId
        {
            get
            {
                int.TryParse(Session["GROUP_ID"]?.ToString(), out int g);
                return g;
            }
        }

        // -----------------------------------------------------------------------
        // Index (landing page)
        // -----------------------------------------------------------------------
        public ActionResult Index()
        {
            if (!IsAuthenticated())
            {
                Session["ROLE"]     = "V";
                Session["WC_CODE"]  = "";
                Session["GROUP_ID"] = "";
                return RedirectToAction("index", "Login");
            }
            return View();
        }

        // -----------------------------------------------------------------------
        // Dashboard
        // -----------------------------------------------------------------------
        public ActionResult Dashboard()
        {
            if (!IsAuthenticated()) return RedirectToAction("index", "Login");
            return View();
        }

        public ActionResult AddItem_Dashboard(List<DashboardSettingDto> data_upload, string item_type)
        {
            try
            {
                _lookupRepo.SaveDashboard(GroupId, item_type ?? "", data_upload ?? new List<DashboardSettingDto>());
                return Json(new { success = true, message = "บันทึกสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListItem_Dashboard()
        {
            try
            {
                var data = _lookupRepo.ListDashboard(GroupId);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // -----------------------------------------------------------------------
        // Employee / Member options
        // -----------------------------------------------------------------------
        public ActionResult Option_EMP(string item_id)
        {
            try
            {
                var data = _lookupRepo.GetEmployees();
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // -----------------------------------------------------------------------
        // Group
        // -----------------------------------------------------------------------
        public ActionResult Group()
        {
            if (!IsAuthenticated()) return RedirectToAction("index", "Login");
            return View();
        }

        public ActionResult ListItem_Group()
        {
            try
            {
                var data = _groupRepo.ListAll();
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SelectItem_GROUP(string item_id)
        {
            try
            {
                if (!int.TryParse(item_id, out int id))
                    return Json(new { success = false, message = "Invalid id" }, JsonRequestBehavior.AllowGet);

                var data = _groupRepo.GetById(id);
                if (data == null) return Json(new { success = false, message = "Not found" }, JsonRequestBehavior.AllowGet);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SelectItem_GROUP_WC_ByWC()
        {
            try
            {
                var data = _groupRepo.GetByWc(WcCode, Role);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddItem_Group(string item_name)
        {
            try
            {
                _groupRepo.DeleteOldHidden();
                int newId = _groupRepo.Add(EmpNo);
                return Json(new { success = true, data = newId.ToString(), message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditItem_Group(int item_id, string item_name, string item_wc,
            string item_isUse, string item_menu1, string item_menu2, string item_menu3,
            string item_menu4, string item_menu5)
        {
            try
            {
                bool isUse = item_isUse == "Y" || item_isUse == "true" || item_isUse == "1";
                _groupRepo.Edit(item_id, item_name ?? "", item_wc ?? "", isUse,
                    item_menu1 ?? "Y", item_menu2 ?? "Y", item_menu3 ?? "Y",
                    item_menu4 ?? "Y", item_menu5 ?? "Y", EmpNo);
                return Json(new { success = true, message = "แก้ไขสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteItem_Group(int item_id)
        {
            try
            {
                _groupRepo.Delete(item_id);
                return Json(new { success = true, message = "ลบข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete_Group_Over()
        {
            try
            {
                _groupRepo.DeleteOldHidden();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Select_GROUP(string item_group)
        {
            try
            {
                if (!int.TryParse(item_group, out int gid))
                    return Json(new { success = false, message = "Invalid group id" }, JsonRequestBehavior.AllowGet);
                var data = _groupRepo.GetById(gid);
                if (data == null) return Json(new { success = false, message = "Not found" }, JsonRequestBehavior.AllowGet);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // -----------------------------------------------------------------------
        // Change Point (Calendar)
        // -----------------------------------------------------------------------
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
            List<RecipientItem> recipient_list,
            string file_ids)
        {
            try
            {
                // Parse date — front-end sends: "Thu Aug 24 2023 15:47:42 GMT+0700 (Indochina Time)"
                DateTime dateTime;
                if (!DateTime.TryParseExact(dates,
                    "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz '(Indochina Time)'",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out dateTime))
                {
                    // fallback — try ISO
                    dateTime = DateTime.Parse(dates);
                }

                // Convert comma-separated IDs → arrays
                int[] detailIds    = ParseIntArray(detail);
                string[] actions   = ParseStringArray(action);
                string[] warnings  = ParseStringArray(warning);
                int[] recipientIds = ParseIntArray(recipient);

                var p = new AddCalendarParams
                {
                    GroupId      = GroupId,
                    DateChange   = dateTime,
                    Shift        = shift   ?? "",
                    Team         = team    ?? "",
                    Spot         = spot    ?? "",
                    Instead      = instead ?? "",
                    SpotEng      = spot_eng    ?? "",
                    SpotTha      = spot_tha    ?? "",
                    InsteadEng   = instead_eng ?? "",
                    InsteadTha   = instead_tha ?? "",
                    McNo         = mc_no    ?? "",
                    Edit         = edit     ?? "",
                    PartNo       = part_no  ?? "",
                    MoldNo       = mold_no  ?? "",
                    Change       = change   ?? "",
                    Process      = process  ?? "",
                    DetailIds    = detailIds,
                    Actions      = actions,
                    Warnings     = warnings,
                    Remark       = remark   ?? "",
                    RecipientIds = recipientIds,
                    Type         = type     ?? "",
                    EmpNo        = EmpNo
                };

                int newCalId = _calRepo.Add(p);

                // Link uploaded files to new calendar record
                if (!string.IsNullOrEmpty(file_ids))
                {
                    int[] fids = ParseIntArray(file_ids);
                    if (fids.Length > 0)
                        _fileRepo.LinkToCalendar(fids, newCalId);
                }

                // Add confirm entries per recipient list member
                if (recipient_list != null)
                {
                    foreach (var r in recipient_list)
                    {
                        if (string.IsNullOrEmpty(r.ITEM_MEMBER)) continue;
                        if (!int.TryParse(r.ITEM_ID, out int groupMemberId)) continue;
                        foreach (var empNo in r.ITEM_MEMBER.Split(','))
                        {
                            var e = empNo.Trim();
                            if (!string.IsNullOrEmpty(e))
                                _approveRepo.AddCalendarConfirm(newCalId, groupMemberId, e);
                        }
                    }
                }

                return Json(new { success = true, message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
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
            List<RecipientItem> recipient_list)
        {
            try
            {
                DateTime dateTime;
                if (!DateTime.TryParseExact(dates,
                    "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz '(Indochina Time)'",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out dateTime))
                {
                    dateTime = DateTime.Parse(dates);
                }

                if (!int.TryParse(id, out int calId))
                    return Json(new { success = false, message = "Invalid id" }, JsonRequestBehavior.AllowGet);

                int[] detailIds    = ParseIntArray(detail);
                string[] actions   = ParseStringArray(action);
                string[] warnings  = ParseStringArray(warning);
                int[] recipientIds = ParseIntArray(recipient);

                var p = new EditCalendarParams
                {
                    Id           = calId,
                    DateChange   = dateTime,
                    Shift        = shift   ?? "",
                    Team         = team    ?? "",
                    Spot         = spot    ?? "",
                    Instead      = instead ?? "",
                    SpotEng      = spot_eng    ?? "",
                    SpotTha      = spot_tha    ?? "",
                    InsteadEng   = instead_eng ?? "",
                    InsteadTha   = instead_tha ?? "",
                    McNo         = mc_no    ?? "",
                    Edit         = edit     ?? "",
                    PartNo       = part_no  ?? "",
                    MoldNo       = mold_no  ?? "",
                    Change       = change   ?? "",
                    Process      = process  ?? "",
                    DetailIds    = detailIds,
                    Actions      = actions,
                    Warnings     = warnings,
                    Remark       = remark   ?? "",
                    RecipientIds = recipientIds,
                    Type         = type     ?? "",
                    EmpNo        = EmpNo
                };

                _calRepo.Edit(p);

                // Replace confirm entries
                _approveRepo.DeleteCalendarConfirms(calId);
                if (recipient_list != null)
                {
                    foreach (var r in recipient_list)
                    {
                        if (string.IsNullOrEmpty(r.ITEM_MEMBER)) continue;
                        if (!int.TryParse(r.ITEM_ID, out int groupMemberId)) continue;
                        foreach (var empNo in r.ITEM_MEMBER.Split(','))
                        {
                            var e = empNo.Trim();
                            if (!string.IsNullOrEmpty(e))
                                _approveRepo.AddCalendarConfirm(calId, groupMemberId, e);
                        }
                    }
                }

                return Json(new { success = true, message = "แก้ไขข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteItem_ChangePoint(string item_id)
        {
            try
            {
                if (!int.TryParse(item_id, out int calId))
                    return Json(new { success = false, message = "Invalid id" }, JsonRequestBehavior.AllowGet);

                // Delete associated files from disk then DB
                var files = _fileRepo.GetByCalendarId(calId);
                foreach (var f in files)
                {
                    string fullPath = Path.Combine(Server.MapPath("~/src/upload"), f.ITEM_NAME);
                    if (System.IO.File.Exists(fullPath))
                        System.IO.File.Delete(fullPath);
                }

                _approveRepo.DeleteCalendarConfirms(calId);
                _calRepo.SoftDelete(calId, EmpNo);

                return Json(new { success = true, message = "ลบข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListItem_ChangePoint_Day(string date_select)
        {
            try
            {
                var items = _calRepo.ListByDay(date_select, GroupId).ToList();
                EnrichWithApproveMembers(items);
                return Json(new { success = true, data = items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListItem_ChangePoint_All()
        {
            try
            {
                var items = _calRepo.ListAll(GroupId).ToList();
                EnrichWithApproveMembers(items);
                return Json(new { success = true, data = items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListItem_ChangePoint_My()
        {
            try
            {
                var items = _calRepo.ListMy(GroupId, EmpNo).ToList();
                EnrichWithApproveMembers(items);
                return Json(new { success = true, data = items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListItem_ChangePoint_Item(string item_id)
        {
            try
            {
                if (!int.TryParse(item_id, out int calId))
                    return Json(new { success = false, message = "Invalid id" }, JsonRequestBehavior.AllowGet);

                var item = _calRepo.GetItem(calId);
                if (item == null)
                    return Json(new { success = false, message = "Not found" }, JsonRequestBehavior.AllowGet);

                EnrichWithApproveMembers(new List<CalendarDto> { item });
                return Json(new { success = true, data = new[] { item } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListItem_ChangePoint_Over()
        {
            try
            {
                var items = _calRepo.ListOver().ToList();
                foreach (var item in items)
                {
                    if (int.TryParse(item.ID, out int calId))
                    {
                        var files = _fileRepo.GetByCalendarId(calId);
                        foreach (var f in files)
                        {
                            string fullPath = Path.Combine(Server.MapPath("~/src/upload"), f.ITEM_NAME);
                            if (System.IO.File.Exists(fullPath))
                                System.IO.File.Delete(fullPath);
                        }
                        _calRepo.DeleteOver(calId);
                    }
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditItem_ChangePoint_Informed(string item_informed, int item_id)
        {
            try
            {
                _calRepo.SetInformed(item_id, item_informed, EmpNo);
                return Json(new { success = true, message = "อัปเดตสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // -----------------------------------------------------------------------
        // File upload / delete
        // -----------------------------------------------------------------------
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, string type, string size, string id)
        {
            try
            {
                if (file == null || file.ContentLength == 0)
                    return Json(new { success = false, message = "No file selected" }, JsonRequestBehavior.AllowGet);

                string originalFileName = Path.GetFileName(file.FileName);
                string[] parts         = originalFileName.Split('.');
                string ext             = parts.Length > 1 ? parts[parts.Length - 1] : "bin";
                string baseName        = string.Join(".", parts.Take(parts.Length - 1));
                string timestamp       = DateTime.Now.ToString("yyyyMMddHHmmss");
                string newFileName     = baseName + "_" + timestamp + "." + ext;
                string path            = Path.Combine(Server.MapPath("~/src/upload"), newFileName);

                file.SaveAs(path);

                int newId = _fileRepo.Add(newFileName, type ?? "", size ?? "", GroupId);

                var result = new[] { new { ITEM_ID = newId.ToString(), ITEM_NAME = newFileName } };
                return Json(new { success = true, data = result, message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult deleteFile(string item_id, string item_name)
        {
            try
            {
                if (!int.TryParse(item_id, out int fileId))
                    return Json(new { success = false, message = "Invalid id" }, JsonRequestBehavior.AllowGet);

                _fileRepo.Delete(fileId);

                string fullPath = Path.Combine(Server.MapPath("~/src/upload"), item_name ?? "");
                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);

                return Json(new { success = true, message = "ลบสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SelectItem_Files_by_changepoint_id(string item_id)
        {
            try
            {
                if (!int.TryParse(item_id, out int calId))
                    return Json(new { success = false, message = "Invalid id" }, JsonRequestBehavior.AllowGet);

                var data = _fileRepo.GetByCalendarId(calId);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SelectItem_Files_by_in_file_id(string item_id)
        {
            try
            {
                int[] ids = ParseIntArray(item_id);
                if (ids.Length == 0)
                    return Json(new { success = true, data = new object[0] }, JsonRequestBehavior.AllowGet);

                var data = _fileRepo.GetByIds(ids);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // -----------------------------------------------------------------------
        // Colors
        // -----------------------------------------------------------------------
        public ActionResult Colors()
        {
            if (!IsAuthenticated()) return RedirectToAction("index", "Login");
            return View();
        }

        public ActionResult SelectItem_Colors()
        {
            try
            {
                var data = _groupRepo.GetColors(GroupId);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit_Colors(string item_color_1, string item_color_2, string item_color_3,
            string item_color_4, string item_color_5)
        {
            try
            {
                _groupRepo.SetColors(GroupId,
                    item_color_1 ?? "#ffffff", item_color_2 ?? "#ffffff", item_color_3 ?? "#ffffff",
                    item_color_4 ?? "#ffffff", item_color_5 ?? "#ffffff");
                return Json(new { success = true, message = "บันทึกสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // -----------------------------------------------------------------------
        // Mold No
        // -----------------------------------------------------------------------
        public ActionResult Mold_NO()
        {
            if (!IsAuthenticated()) return RedirectToAction("index", "Login");
            return View();
        }

        public ActionResult ListItem_Mold_NO()
        {
            try
            {
                var data = _lookupRepo.ListLookup("cp_mold_no", GroupId);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Option_Mold_NO()
        {
            try
            {
                var data = _lookupRepo.OptionLookup("cp_mold_no", GroupId);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SelectItem_Mold_NO(string item_id)
        {
            try
            {
                if (!int.TryParse(item_id, out int id))
                    return Json(new { success = false, message = "Invalid id" }, JsonRequestBehavior.AllowGet);
                var data = _lookupRepo.SelectLookup("cp_mold_no", id);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddItem_Mold_NO(string item_name, string item_isUse)
        {
            try
            {
                bool isUse = item_isUse == "Y" || item_isUse == "true";
                int newId  = _lookupRepo.AddLookup("cp_mold_no", item_name ?? "", GroupId, isUse, EmpNo);
                return Json(new { success = true, data = newId.ToString(), message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditItem_Mold_NO(string item_name, int item_id, string item_isUse)
        {
            try
            {
                bool isUse = item_isUse == "Y" || item_isUse == "true";
                _lookupRepo.EditLookup("cp_mold_no", item_id, item_name ?? "", isUse, EmpNo);
                return Json(new { success = true, message = "แก้ไขสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteItem_Mold_NO(int item_id)
        {
            try
            {
                _lookupRepo.DeleteLookup("cp_mold_no", item_id);
                return Json(new { success = true, message = "ลบสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // -----------------------------------------------------------------------
        // Process
        // -----------------------------------------------------------------------
        public ActionResult Process()
        {
            if (!IsAuthenticated()) return RedirectToAction("index", "Login");
            return View();
        }

        public ActionResult ListItem_Process()
        {
            try
            {
                var data = _lookupRepo.ListLookup("cp_process", GroupId);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Option_Process()
        {
            try
            {
                var data = _lookupRepo.OptionLookup("cp_process", GroupId);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SelectItem_Process(string item_id)
        {
            try
            {
                if (!int.TryParse(item_id, out int id))
                    return Json(new { success = false, message = "Invalid id" }, JsonRequestBehavior.AllowGet);
                var data = _lookupRepo.SelectLookup("cp_process", id);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddItem_Process(string item_name, string item_isUse)
        {
            try
            {
                bool isUse = item_isUse == "Y" || item_isUse == "true";
                int newId  = _lookupRepo.AddLookup("cp_process", item_name ?? "", GroupId, isUse, EmpNo);
                return Json(new { success = true, data = newId.ToString(), message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditItem_Process(string item_name, int item_id, string item_isUse)
        {
            try
            {
                bool isUse = item_isUse == "Y" || item_isUse == "true";
                _lookupRepo.EditLookup("cp_process", item_id, item_name ?? "", isUse, EmpNo);
                return Json(new { success = true, message = "แก้ไขสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteItem_Process(int item_id)
        {
            try
            {
                _lookupRepo.DeleteLookup("cp_process", item_id);
                return Json(new { success = true, message = "ลบสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // -----------------------------------------------------------------------
        // Edit (Edit Point)
        // -----------------------------------------------------------------------
        public ActionResult Edit()
        {
            if (!IsAuthenticated()) return RedirectToAction("index", "Login");
            return View();
        }

        public ActionResult ListItem_Edit()
        {
            try
            {
                var data = _lookupRepo.ListLookup("cp_edit", GroupId);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Option_Edit()
        {
            try
            {
                var data = _lookupRepo.OptionLookup("cp_edit", GroupId);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SelectItem_Edit(string item_id)
        {
            try
            {
                if (!int.TryParse(item_id, out int id))
                    return Json(new { success = false, message = "Invalid id" }, JsonRequestBehavior.AllowGet);
                var data = _lookupRepo.SelectLookup("cp_edit", id);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddItem_Edit(string item_name, string item_isUse)
        {
            try
            {
                bool isUse = item_isUse == "Y" || item_isUse == "true";
                int newId  = _lookupRepo.AddLookup("cp_edit", item_name ?? "", GroupId, isUse, EmpNo);
                return Json(new { success = true, data = newId.ToString(), message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditItem_Edit(string item_name, int item_id, string item_isUse)
        {
            try
            {
                bool isUse = item_isUse == "Y" || item_isUse == "true";
                _lookupRepo.EditLookup("cp_edit", item_id, item_name ?? "", isUse, EmpNo);
                return Json(new { success = true, message = "แก้ไขสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteItem_Edit(int item_id)
        {
            try
            {
                _lookupRepo.DeleteLookup("cp_edit", item_id);
                return Json(new { success = true, message = "ลบสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // -----------------------------------------------------------------------
        // Change
        // -----------------------------------------------------------------------
        public ActionResult Change()
        {
            if (!IsAuthenticated()) return RedirectToAction("index", "Login");
            return View();
        }

        public ActionResult ListItem_Change()
        {
            try
            {
                var data = _lookupRepo.ListLookup("cp_change", GroupId);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Option_Change()
        {
            try
            {
                var data = _lookupRepo.OptionLookup("cp_change", GroupId);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SelectItem_Change(string item_id)
        {
            try
            {
                if (!int.TryParse(item_id, out int id))
                    return Json(new { success = false, message = "Invalid id" }, JsonRequestBehavior.AllowGet);
                var data = _lookupRepo.SelectLookup("cp_change", id);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddItem_Change(string item_name, string item_isUse)
        {
            try
            {
                bool isUse = item_isUse == "Y" || item_isUse == "true";
                int newId  = _lookupRepo.AddLookup("cp_change", item_name ?? "", GroupId, isUse, EmpNo);
                return Json(new { success = true, data = newId.ToString(), message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditItem_Change(string item_name, int item_id, string item_isUse)
        {
            try
            {
                bool isUse = item_isUse == "Y" || item_isUse == "true";
                _lookupRepo.EditLookup("cp_change", item_id, item_name ?? "", isUse, EmpNo);
                return Json(new { success = true, message = "แก้ไขสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteItem_Change(int item_id)
        {
            try
            {
                _lookupRepo.DeleteLookup("cp_change", item_id);
                return Json(new { success = true, message = "ลบสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // -----------------------------------------------------------------------
        // Part
        // -----------------------------------------------------------------------
        public ActionResult Part()
        {
            if (!IsAuthenticated()) return RedirectToAction("index", "Login");
            return View();
        }

        public ActionResult ListItem_Part()
        {
            try
            {
                var data = _lookupRepo.ListParts(GroupId);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Option_Part()
        {
            try
            {
                var data = _lookupRepo.OptionParts(GroupId);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SelectItem_Part(string item_part)
        {
            try
            {
                var data = _lookupRepo.SelectPart(item_part ?? "");
                if (data == null) return Json(new { success = false, message = "Not found" }, JsonRequestBehavior.AllowGet);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddItem_Part(string item_part, string item_name, string item_isuse)
        {
            try
            {
                bool isUse = item_isuse == "Y" || item_isuse == "true";
                bool added = _lookupRepo.AddPart(item_part ?? "", item_name ?? "", GroupId, isUse, EmpNo);
                if (!added)
                    return Json(new { success = false, message = "Part No นี้มีอยู่แล้ว" }, JsonRequestBehavior.AllowGet);
                return Json(new { success = true, message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditItem_Part(string item_part, string item_name, string item_isuse)
        {
            try
            {
                bool isUse = item_isuse == "Y" || item_isuse == "true";
                _lookupRepo.EditPart(item_part ?? "", item_name ?? "", isUse, EmpNo);
                return Json(new { success = true, message = "แก้ไขสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteItem_Part(string item_part)
        {
            try
            {
                _lookupRepo.DeletePart(item_part ?? "", GroupId);
                return Json(new { success = true, message = "ลบสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult export_Part()
        {
            // Returns CSV of all parts for this group
            try
            {
                var parts = _lookupRepo.ListParts(GroupId);
                var sb    = new System.Text.StringBuilder();
                sb.AppendLine("PART_NO,PART_NAME,IS_USE");
                foreach (var p in parts)
                    sb.AppendLine($"{CsvEscape(p.ITEM_ID)},{CsvEscape(p.ITEM_NAME)},{p.IS_USE}");

                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                return File(bytes, "text/csv", $"parts_{GroupId}_{DateTime.Now:yyyyMMdd}.csv");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // -----------------------------------------------------------------------
        // Machine
        // -----------------------------------------------------------------------
        public ActionResult Machine()
        {
            if (!IsAuthenticated()) return RedirectToAction("index", "Login");
            return View();
        }

        public ActionResult ListItem_MC()
        {
            try
            {
                var data = _lookupRepo.ListMachines(GroupId);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Option_MC()
        {
            try
            {
                var data = _lookupRepo.OptionMachines(GroupId);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SelectItem_Machine(string item_id)
        {
            try
            {
                if (!int.TryParse(item_id, out int id))
                    return Json(new { success = false, message = "Invalid id" }, JsonRequestBehavior.AllowGet);
                var data = _lookupRepo.SelectMachine(id);
                if (data == null) return Json(new { success = false, message = "Not found" }, JsonRequestBehavior.AllowGet);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddItem_Machine(string item_mc, string item_ton, string item_isUse)
        {
            try
            {
                bool isUse = item_isUse == "Y" || item_isUse == "true";
                int newId  = _lookupRepo.AddMachine(item_mc ?? "", item_ton ?? "", GroupId, isUse, EmpNo);
                return Json(new { success = true, data = newId.ToString(), message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditItem_Machine(string item_id, string item_mc, string item_ton, string item_isuse)
        {
            try
            {
                if (!int.TryParse(item_id, out int id))
                    return Json(new { success = false, message = "Invalid id" }, JsonRequestBehavior.AllowGet);
                bool isUse = item_isuse == "Y" || item_isuse == "true";
                _lookupRepo.EditMachine(id, item_mc ?? "", item_ton ?? "", isUse, EmpNo);
                return Json(new { success = true, message = "แก้ไขสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteItem_Machine(int item_id)
        {
            try
            {
                _lookupRepo.DeleteMachine(item_id);
                return Json(new { success = true, message = "ลบสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // -----------------------------------------------------------------------
        // Details
        // -----------------------------------------------------------------------
        public ActionResult Detail()
        {
            if (!IsAuthenticated()) return RedirectToAction("index", "Login");
            return View();
        }

        public ActionResult ListItem_Details(string filter_type, string filter_category)
        {
            try
            {
                var data = _lookupRepo.ListDetails(GroupId, filter_type, filter_category);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Option_Details(string filter_type)
        {
            try
            {
                var data = _lookupRepo.OptionDetails(GroupId, filter_type);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SelectItem_Details(string item_id)
        {
            try
            {
                // Returns all detail options (same as Option_Details but legacy route)
                var data = _lookupRepo.OptionDetails(GroupId, null);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddItem_Details(string item_detail, string item_risk, string item_category, string item_isUse)
        {
            try
            {
                int.TryParse(item_category, out int catId);
                bool isUse = item_isUse == "Y" || item_isUse == "true";
                _lookupRepo.AddDetail(item_detail ?? "", item_risk ?? "", catId, GroupId, isUse, EmpNo);
                return Json(new { success = true, message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditItem_Details(string item_detail, string item_risk, string item_category, string item_id, string item_isUse)
        {
            try
            {
                if (!int.TryParse(item_id, out int id))
                    return Json(new { success = false, message = "Invalid id" }, JsonRequestBehavior.AllowGet);
                int.TryParse(item_category, out int catId);
                bool isUse = item_isUse == "Y" || item_isUse == "true";
                _lookupRepo.EditDetail(id, item_detail ?? "", item_risk ?? "", catId, isUse, EmpNo);
                return Json(new { success = true, message = "แก้ไขสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteItem_Details(int item_id)
        {
            try
            {
                _lookupRepo.DeleteDetail(item_id);
                return Json(new { success = true, message = "ลบสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // -----------------------------------------------------------------------
        // Detail Category
        // -----------------------------------------------------------------------
        public ActionResult Detail_Category()
        {
            if (!IsAuthenticated()) return RedirectToAction("index", "Login");
            return View();
        }

        public ActionResult ListItem_Detail_Category(string filter_type)
        {
            try
            {
                var data = _lookupRepo.ListDetailCategories(GroupId, filter_type);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Option_Detail_Category()
        {
            try
            {
                var data = _lookupRepo.OptionDetailCategories(GroupId);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SelectItem_Detail_Category(string item_id)
        {
            try
            {
                // Returns full category list (legacy — item_id not used for filtering here)
                var data = _lookupRepo.OptionDetailCategories(GroupId);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddItem_Detail_Category(string item_name, string item_type, string item_isUse)
        {
            try
            {
                bool isUse = item_isUse == "Y" || item_isUse == "true";
                _lookupRepo.AddDetailCategory(item_name ?? "", item_type ?? "", GroupId, isUse, EmpNo);
                return Json(new { success = true, message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditItem_Detail_Category(string item_name, string item_type, string item_id, string item_isUse)
        {
            try
            {
                if (!int.TryParse(item_id, out int id))
                    return Json(new { success = false, message = "Invalid id" }, JsonRequestBehavior.AllowGet);
                bool isUse = item_isUse == "Y" || item_isUse == "true";
                _lookupRepo.EditDetailCategory(id, item_name ?? "", item_type ?? "", isUse, EmpNo);
                return Json(new { success = true, message = "แก้ไขสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteItem_Detail_Category(int item_id)
        {
            try
            {
                _lookupRepo.DeleteDetailCategory(item_id);
                return Json(new { success = true, message = "ลบสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // -----------------------------------------------------------------------
        // Approve Member
        // -----------------------------------------------------------------------
        public ActionResult Approve_Member()
        {
            if (!IsAuthenticated()) return RedirectToAction("index", "Login");
            return View();
        }

        public ActionResult ListItem_Approve_Member()
        {
            try
            {
                var data = _approveRepo.ListApproveMembers(GroupId);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Option_Approve_Member()
        {
            try
            {
                var data = _approveRepo.OptionApproveMembers(GroupId);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SelectItem_Approve_Member(string item_id)
        {
            try
            {
                if (!int.TryParse(item_id, out int id))
                    return Json(new { success = false, message = "Invalid id" }, JsonRequestBehavior.AllowGet);
                var data = _approveRepo.GetApproveMember(id);
                if (data == null) return Json(new { success = false, message = "Not found" }, JsonRequestBehavior.AllowGet);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SelectItem_Approve_Member_by_in(string item_id, string item_cid)
        {
            try
            {
                if (!int.TryParse(item_id, out int id))
                    return Json(new { success = false, message = "Invalid id" }, JsonRequestBehavior.AllowGet);
                int.TryParse(item_cid, out int calId);
                var data = _approveRepo.GetApproveMemberWithConfirms(id, calId);
                if (data == null) return Json(new { success = false, message = "Not found" }, JsonRequestBehavior.AllowGet);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddItem_Approve_Member(string item_title, string item_member, string item_isUse)
        {
            try
            {
                bool isUse = item_isUse == "Y" || item_isUse == "true";
                int newId  = _approveRepo.AddApproveMember(item_title ?? "", item_member ?? "", GroupId, isUse, EmpNo);
                return Json(new { success = true, data = newId.ToString(), message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditItem_Approve_Member(string item_title, string item_member, string item_id, string item_isUse)
        {
            try
            {
                if (!int.TryParse(item_id, out int id))
                    return Json(new { success = false, message = "Invalid id" }, JsonRequestBehavior.AllowGet);
                bool isUse = item_isUse == "Y" || item_isUse == "true";
                _approveRepo.EditApproveMember(id, item_title ?? "", item_member ?? "", isUse, EmpNo);
                return Json(new { success = true, message = "แก้ไขสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteItem_Approve_Member(int item_id)
        {
            try
            {
                _approveRepo.DeleteApproveMember(item_id);
                return Json(new { success = true, message = "ลบสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // -----------------------------------------------------------------------
        // Check Sheet
        // -----------------------------------------------------------------------
        public ActionResult Check_Sheet()
        {
            if (!IsAuthenticated()) return RedirectToAction("index", "Login");
            return View();
        }

        public ActionResult ListItem_Check_Sheet()
        {
            try
            {
                var data = _approveRepo.ListCheckSheets(GroupId);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Option_Check_Sheet()
        {
            try
            {
                var data = _approveRepo.ListCheckSheets(GroupId)
                                       .Where(x => x.IS_USE == "Y")
                                       .Select(x => new { x.ITEM_ID, x.ITEM_NAME });
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SelectItem_Check_Sheet(string item_id)
        {
            try
            {
                if (!int.TryParse(item_id, out int id))
                    return Json(new { success = false, message = "Invalid id" }, JsonRequestBehavior.AllowGet);
                var data = _approveRepo.GetCheckSheet(id);
                if (data == null) return Json(new { success = false, message = "Not found" }, JsonRequestBehavior.AllowGet);
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddItem_Check_Sheet(string item_name, string items, string item_isUse)
        {
            try
            {
                bool isUse      = item_isUse == "Y" || item_isUse == "true";
                string[] itemArr = ParseStringArray(items);
                int newId = _approveRepo.AddCheckSheet(item_name ?? "", GroupId, isUse, EmpNo, itemArr);
                return Json(new { success = true, data = newId.ToString(), message = "เพิ่มข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditItem_Check_Sheet(string item_name, string items, string item_id, string item_isUse)
        {
            try
            {
                if (!int.TryParse(item_id, out int id))
                    return Json(new { success = false, message = "Invalid id" }, JsonRequestBehavior.AllowGet);
                bool isUse      = item_isUse == "Y" || item_isUse == "true";
                string[] itemArr = ParseStringArray(items);
                _approveRepo.EditCheckSheet(id, item_name ?? "", isUse, EmpNo, itemArr);
                return Json(new { success = true, message = "แก้ไขสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteItem_Check_Sheet(int item_id)
        {
            try
            {
                _approveRepo.DeleteCheckSheet(item_id);
                return Json(new { success = true, message = "ลบสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ErrMsg, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // -----------------------------------------------------------------------
        // Member management page (view-only shell — data served via API actions)
        // -----------------------------------------------------------------------
        public ActionResult Member()
        {
            if (!IsAuthenticated()) return RedirectToAction("index", "Login");
            return View();
        }

        // -----------------------------------------------------------------------
        // Private helpers
        // -----------------------------------------------------------------------

        /// <summary>
        /// Splits a comma-separated string into int[]. Silently skips non-integer tokens.
        /// </summary>
        private static int[] ParseIntArray(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return new int[0];
            return s.Split(',')
                    .Select(t => t.Trim())
                    .Where(t => t.Length > 0)
                    .Select(t => int.TryParse(t, out int v) ? (int?)v : null)
                    .Where(v => v.HasValue)
                    .Select(v => v.Value)
                    .ToArray();
        }

        /// <summary>
        /// Splits a comma-separated string into string[]. Skips empty tokens.
        /// </summary>
        private static string[] ParseStringArray(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return new string[0];
            return s.Split(',')
                    .Select(t => t.Trim())
                    .Where(t => t.Length > 0)
                    .ToArray();
        }

        /// <summary>
        /// For each CalendarDto, loads the ApproveDto list from the comma-separated RECIPIENT_ID field.
        /// </summary>
        private void EnrichWithApproveMembers(IList<CalendarDto> items)
        {
            foreach (var item in items)
            {
                item.RECIPIENT_LIST = new List<ApproveDto>();
                int[] recipIds = ParseIntArray(item.RECIPIENT_ID);
                if (recipIds.Length == 0) continue;

                foreach (var rid in recipIds)
                {
                    var am = _approveRepo.GetApproveMember(rid);
                    if (am != null)
                        item.RECIPIENT_LIST.Add(am);
                }
            }
        }

        private static string CsvEscape(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            if (s.Contains(',') || s.Contains('"') || s.Contains('\n'))
                return "\"" + s.Replace("\"", "\"\"") + "\"";
            return s;
        }
    }
}
