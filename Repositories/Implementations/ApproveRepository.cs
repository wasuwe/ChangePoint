using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dapper;
using Change_Point.Infrastructure;
using Change_Point.Models.DTOs;
using Change_Point.Repositories.Interfaces;

namespace Change_Point.Repositories.Implementations
{
    public class ApproveRepository : IApproveRepository
    {
        private readonly string _cp   = GlobalConfig.CpSchema;
        private readonly string _main = GlobalConfig.MainSchema;

        // -----------------------------------------------------------------------
        // Approve Member
        // -----------------------------------------------------------------------
        public IEnumerable<ApproveDto> ListApproveMembers(int groupId)
        {
            try
            {
                var sql = $@"
                    SELECT m.id, m.title, m.member_list, m.is_use,
                           m.create_by, m.create_date, m.update_by, m.update_date,
                           e.gname_eng, e.fname_eng
                    FROM {_cp}.cp_approve_member m
                    LEFT JOIN {_main}.center_tm_employee e ON m.update_by = e.emp_no
                    WHERE m.group_id = @groupId
                    ORDER BY m.id";

                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql, new { groupId }).Select(r =>
                    {
                        var cb = (string)r.create_by ?? "";
                        var ub = (string)r.update_by ?? "";
                        var cd = (DateTime?)r.create_date;
                        var ud = (DateTime?)r.update_date;
                        return new ApproveDto
                        {
                            ITEM_ID     = ((int)r.id).ToString(),
                            ITEM_TITLE  = (string)r.title ?? "",
                            ITEM_MEMBER = (string)r.member_list ?? "",
                            IS_USE      = (bool)r.is_use ? "Y" : "N",
                            GNAME_ENG   = (string)r.gname_eng ?? "",
                            FNAME_ENG   = (string)r.fname_eng ?? "",
                            UPDATE_BY   = string.IsNullOrEmpty(ub) ? cb : ub,
                            UPDATE_DATE = string.IsNullOrEmpty(ub)
                                            ? cd?.ToString("dd/MM/yy HH:mm") ?? ""
                                            : ud?.ToString("dd/MM/yy HH:mm") ?? "",
                            CONFIRM_LIST = new List<ConfirmListDto>()
                        };
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ApproveRepository.ListApproveMembers] " + ex.Message);
                throw;
            }
        }

        public IEnumerable<ApproveMemberOptionDto> OptionApproveMembers(int groupId)
        {
            try
            {
                var sql = $@"
                    SELECT id, title, member_list
                    FROM {_cp}.cp_approve_member
                    WHERE is_use = TRUE AND group_id = @groupId
                    ORDER BY id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    return conn.Query(sql, new { groupId })
                               .Select(r => new ApproveMemberOptionDto
                               {
                                   ITEM_ID     = ((int)r.id).ToString(),
                                   ITEM_TITLE  = (string)r.title ?? "",
                                   ITEM_MEMBER = (string)r.member_list ?? ""
                               }).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ApproveRepository.OptionApproveMembers] " + ex.Message);
                throw;
            }
        }

        public ApproveDto GetApproveMember(int id)
        {
            try
            {
                var sql = $"SELECT * FROM {_cp}.cp_approve_member WHERE id = @id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    var r = conn.QueryFirstOrDefault(sql, new { id });
                    if (r == null) return null;
                    return new ApproveDto
                    {
                        ITEM_ID     = ((int)r.id).ToString(),
                        ITEM_TITLE  = (string)r.title ?? "",
                        ITEM_MEMBER = (string)r.member_list ?? "",
                        IS_USE      = (bool)r.is_use ? "Y" : "N",
                        CONFIRM_LIST = new List<ConfirmListDto>()
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ApproveRepository.GetApproveMember] " + ex.Message);
                throw;
            }
        }

        public ApproveDto GetApproveMemberWithConfirms(int memberId, int calendarId)
        {
            try
            {
                var dto = GetApproveMember(memberId);
                if (dto == null) return null;

                var sql = $@"
                    SELECT cc.id, cc.calendar_id, cc.is_confirm, cc.confirm_date, cc.create_date,
                           c.status_type, c.date_change, c.shift,
                           e.gname_eng, e.fname_eng
                    FROM {_cp}.cp_calendar_confirm cc
                    LEFT JOIN {_cp}.cp_calendar c ON cc.calendar_id = c.id
                    LEFT JOIN {_main}.center_tm_employee e ON cc.emp_no = e.emp_no
                    WHERE cc.group_member_id = @memberId AND cc.calendar_id = @calendarId
                    ORDER BY cc.is_confirm DESC";

                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    dto.CONFIRM_LIST = conn.Query(sql, new { memberId, calendarId }).Select(r =>
                    {
                        var cd = (DateTime?)r.create_date;
                        var cpd = (DateTime?)r.date_change;
                        DateTime? cfDate = r.confirm_date == null ? (DateTime?)null : (DateTime?)r.confirm_date;
                        return new ConfirmListDto
                        {
                            ID                = ((int)r.id).ToString(),
                            CALENDAR_ID       = ((int)r.calendar_id).ToString(),
                            RESULT            = (string)r.is_confirm ?? "N",
                            GNAME_ENG         = (string)r.gname_eng ?? "",
                            FNAME_ENG         = (string)r.fname_eng ?? "",
                            SHIFT             = (string)r.shift ?? "",
                            TYPE              = (string)r.status_type ?? "",
                            CONFIRM_DATE      = cfDate?.ToString("dd/MM/yyyy HH:mm") ?? "",
                            CREATE_DATE       = cd?.ToString("dd/MM/yyyy HH:mm") ?? "",
                            CHANGE_POINT_DATE = cpd?.ToString("dd/MM/yyyy") ?? ""
                        };
                    }).ToList();
                }
                return dto;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ApproveRepository.GetApproveMemberWithConfirms] " + ex.Message);
                throw;
            }
        }

        public int AddApproveMember(string title, string memberList, int groupId, bool isUse, string empNo)
        {
            try
            {
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    conn.Open();
                    using (var tx = conn.BeginTransaction())
                    {
                        int newId = conn.ExecuteScalar<int>($@"
                            INSERT INTO {_cp}.cp_approve_member
                                (group_id, title, member_list, is_use, create_by, create_date, update_by, update_date)
                            VALUES (@groupId, @title, @memberList, @isUse, @empNo, NOW(), @empNo, NOW())
                            RETURNING id",
                            new { groupId, title, memberList, isUse, empNo }, tx);

                        if (!string.IsNullOrEmpty(memberList))
                        {
                            foreach (var emp in memberList.Split(','))
                            {
                                if (!string.IsNullOrWhiteSpace(emp))
                                    conn.Execute($@"
                                        INSERT INTO {_cp}.cp_approve_member_item
                                            (approve_member_id, emp_no, group_id)
                                        VALUES (@newId, @emp, @groupId)",
                                        new { newId, emp = emp.Trim(), groupId }, tx);
                            }
                        }
                        tx.Commit();
                        return newId;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ApproveRepository.AddApproveMember] " + ex.Message);
                throw;
            }
        }

        public void EditApproveMember(int id, string title, string memberList, bool isUse, string empNo)
        {
            try
            {
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    conn.Open();
                    using (var tx = conn.BeginTransaction())
                    {
                        conn.Execute($@"
                            UPDATE {_cp}.cp_approve_member
                            SET title = @title, member_list = @memberList, is_use = @isUse,
                                update_by = @empNo, update_date = NOW()
                            WHERE id = @id",
                            new { id, title, memberList, isUse, empNo }, tx);

                        conn.Execute($"DELETE FROM {_cp}.cp_approve_member_item WHERE approve_member_id = @id",
                            new { id }, tx);

                        if (!string.IsNullOrEmpty(memberList))
                        {
                            foreach (var emp in memberList.Split(','))
                            {
                                if (!string.IsNullOrWhiteSpace(emp))
                                    conn.Execute($@"
                                        INSERT INTO {_cp}.cp_approve_member_item
                                            (approve_member_id, emp_no, group_id)
                                        VALUES (@id, @emp, (SELECT group_id FROM {_cp}.cp_approve_member WHERE id = @id))",
                                        new { id, emp = emp.Trim() }, tx);
                            }
                        }
                        tx.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ApproveRepository.EditApproveMember] " + ex.Message);
                throw;
            }
        }

        public void DeleteApproveMember(int id)
        {
            try
            {
                // CASCADE handles items
                var sql = $"DELETE FROM {_cp}.cp_approve_member WHERE id = @id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { id });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ApproveRepository.DeleteApproveMember] " + ex.Message);
                throw;
            }
        }

        // -----------------------------------------------------------------------
        // Calendar Confirm
        // -----------------------------------------------------------------------
        public void AddCalendarConfirm(int calendarId, int groupMemberId, string empNo)
        {
            try
            {
                var sql = $@"
                    INSERT INTO {_cp}.cp_calendar_confirm
                        (calendar_id, group_member_id, emp_no, is_confirm, create_date)
                    VALUES (@calendarId, @groupMemberId, @empNo, 'N', NOW())";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { calendarId, groupMemberId, empNo });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ApproveRepository.AddCalendarConfirm] " + ex.Message);
                throw;
            }
        }

        public void DeleteCalendarConfirms(int calendarId)
        {
            try
            {
                var sql = $"DELETE FROM {_cp}.cp_calendar_confirm WHERE calendar_id = @calendarId";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { calendarId });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ApproveRepository.DeleteCalendarConfirms] " + ex.Message);
                throw;
            }
        }

        public void ConfirmChangePoint(int confirmId)
        {
            try
            {
                var sql = $@"
                    UPDATE {_cp}.cp_calendar_confirm
                    SET is_confirm = 'Y', confirm_date = NOW()
                    WHERE id = @confirmId";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { confirmId });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ApproveRepository.ConfirmChangePoint] " + ex.Message);
                throw;
            }
        }

        // -----------------------------------------------------------------------
        // Waiting List
        // -----------------------------------------------------------------------
        public IEnumerable<WaitingConfirmDto> ListWaiting(string empNo, string dateStart, string dateStop,
                                                          string filterType, string filterShift, string filterStatus)
        {
            try
            {
                var sql = $@"
                    SELECT cc.id, cc.calendar_id, cc.is_confirm, cc.confirm_date,
                           c.status_type, c.date_change, c.shift, c.create_date,
                           e.gname_eng, e.fname_eng,
                           g.name AS group_name
                    FROM {_cp}.cp_calendar_confirm cc
                    LEFT JOIN {_cp}.cp_calendar c ON cc.calendar_id = c.id
                    LEFT JOIN {_main}.center_tm_employee e ON c.update_by = e.emp_no
                    LEFT JOIN {_cp}.cp_group g ON c.group_id = g.id
                    WHERE cc.emp_no = @empNo";

                var p = new Dapper.DynamicParameters();
                p.Add("empNo", empNo);

                if (!string.IsNullOrEmpty(dateStart) && !string.IsNullOrEmpty(dateStop))
                {
                    if (DateTime.TryParseExact(dateStart, "dd-MM-yyyy",
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None, out DateTime ds) &&
                        DateTime.TryParseExact(dateStop, "dd-MM-yyyy",
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None, out DateTime de))
                    {
                        sql += " AND c.date_change >= @dateStart AND c.date_change <= @dateEnd";
                        p.Add("dateStart", ds.Date);
                        p.Add("dateEnd",   de.Date.AddDays(1).AddSeconds(-1));
                    }
                }

                // filterType / filterShift / filterStatus are CSV lists of quoted values from Vue
                // We pass them as is to avoid SQL injection risk; validate they contain only safe chars
                if (!string.IsNullOrEmpty(filterType)   && IsSafeFilter(filterType))
                    sql += $" AND c.status_type IN ({filterType})";
                if (!string.IsNullOrEmpty(filterShift)  && IsSafeFilter(filterShift))
                    sql += $" AND c.shift IN ({filterShift})";
                if (!string.IsNullOrEmpty(filterStatus) && IsSafeFilter(filterStatus))
                    sql += $" AND cc.is_confirm IN ({filterStatus})";

                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql, p).Select(r =>
                    {
                        var cd  = (DateTime?)r.create_date;
                        var cpd = (DateTime?)r.date_change;
                        DateTime? cfDate = r.confirm_date == null ? (DateTime?)null : (DateTime?)r.confirm_date;
                        return new WaitingConfirmDto
                        {
                            ID                = ((int)r.id).ToString(),
                            CALENDAR_ID       = ((int)r.calendar_id).ToString(),
                            RESULT            = (string)r.is_confirm ?? "N",
                            GNAME_ENG         = (string)r.gname_eng ?? "",
                            FNAME_ENG         = (string)r.fname_eng ?? "",
                            SHIFT             = (string)r.shift ?? "",
                            TYPE              = (string)r.status_type ?? "",
                            GROUP_NAME        = (string)r.group_name ?? "",
                            CONFIRM_DATE      = cfDate?.ToString("dd/MM/yyyy HH:mm") ?? "",
                            CREATE_DATE       = cd?.ToString("dd/MM/yyyy HH:mm") ?? "",
                            CHANGE_POINT_DATE = cpd?.ToString("dd/MM/yyyy") ?? ""
                        };
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ApproveRepository.ListWaiting] " + ex.Message);
                throw;
            }
        }

        public int CountWaiting(string empNo)
        {
            try
            {
                var sql = $@"
                    SELECT COUNT(*) FROM {_cp}.cp_calendar_confirm
                    WHERE emp_no = @empNo AND is_confirm = 'N'";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    return conn.ExecuteScalar<int>(sql, new { empNo });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ApproveRepository.CountWaiting] " + ex.Message);
                throw;
            }
        }

        // -----------------------------------------------------------------------
        // Check Sheet
        // -----------------------------------------------------------------------
        public IEnumerable<CheckSheetDto> ListCheckSheets(int groupId)
        {
            try
            {
                var sql = $@"
                    SELECT cs.id, cs.name, cs.is_use,
                           cs.create_by, cs.create_date, cs.update_by, cs.update_date,
                           e.gname_eng, e.fname_eng
                    FROM {_cp}.cp_check_sheet cs
                    LEFT JOIN {_main}.center_tm_employee e ON cs.update_by = e.emp_no
                    WHERE cs.group_id = @groupId
                    ORDER BY cs.id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql, new { groupId }).Select(r =>
                    {
                        var cb = (string)r.create_by ?? "";
                        var ub = (string)r.update_by ?? "";
                        var cd = (DateTime?)r.create_date;
                        var ud = (DateTime?)r.update_date;
                        return new CheckSheetDto
                        {
                            ITEM_ID     = ((int)r.id).ToString(),
                            ITEM_NAME   = (string)r.name ?? "",
                            IS_USE      = (bool)r.is_use ? "Y" : "N",
                            GNAME_ENG   = (string)r.gname_eng ?? "",
                            FNAME_ENG   = (string)r.fname_eng ?? "",
                            UPDATE_BY   = string.IsNullOrEmpty(ub) ? cb : ub,
                            UPDATE_DATE = string.IsNullOrEmpty(ub)
                                            ? cd?.ToString("dd/MM/yy HH:mm") ?? ""
                                            : ud?.ToString("dd/MM/yy HH:mm") ?? ""
                        };
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ApproveRepository.ListCheckSheets] " + ex.Message);
                throw;
            }
        }

        public CheckSheetDto GetCheckSheet(int id)
        {
            try
            {
                var sql = $"SELECT * FROM {_cp}.cp_check_sheet WHERE id = @id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    var r = conn.QueryFirstOrDefault(sql, new { id });
                    if (r == null) return null;
                    return new CheckSheetDto
                    {
                        ITEM_ID   = ((int)r.id).ToString(),
                        ITEM_NAME = (string)r.name ?? "",
                        IS_USE    = (bool)r.is_use ? "Y" : "N"
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ApproveRepository.GetCheckSheet] " + ex.Message);
                throw;
            }
        }

        public int AddCheckSheet(string name, int groupId, bool isUse, string empNo, string[] items)
        {
            try
            {
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    conn.Open();
                    using (var tx = conn.BeginTransaction())
                    {
                        int newId = conn.ExecuteScalar<int>($@"
                            INSERT INTO {_cp}.cp_check_sheet
                                (group_id, name, is_use, create_by, create_date, update_by, update_date)
                            VALUES (@groupId, @name, @isUse, @empNo, NOW(), @empNo, NOW())
                            RETURNING id",
                            new { groupId, name, isUse, empNo }, tx);

                        if (items != null)
                        {
                            foreach (var item in items)
                            {
                                if (!string.IsNullOrWhiteSpace(item))
                                    conn.Execute($@"
                                        INSERT INTO {_cp}.cp_check_sheet_item
                                            (check_sheet_id, emp_no, group_id)
                                        VALUES (@newId, @item, @groupId)",
                                        new { newId, item = item.Trim(), groupId }, tx);
                            }
                        }
                        tx.Commit();
                        return newId;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ApproveRepository.AddCheckSheet] " + ex.Message);
                throw;
            }
        }

        public void EditCheckSheet(int id, string name, bool isUse, string empNo, string[] items)
        {
            try
            {
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    conn.Open();
                    using (var tx = conn.BeginTransaction())
                    {
                        conn.Execute($@"
                            UPDATE {_cp}.cp_check_sheet
                            SET name = @name, is_use = @isUse, update_by = @empNo, update_date = NOW()
                            WHERE id = @id",
                            new { id, name, isUse, empNo }, tx);

                        conn.Execute($"DELETE FROM {_cp}.cp_check_sheet_item WHERE check_sheet_id = @id",
                            new { id }, tx);

                        int groupId = conn.ExecuteScalar<int>(
                            $"SELECT group_id FROM {_cp}.cp_check_sheet WHERE id = @id", new { id }, tx);

                        if (items != null)
                        {
                            foreach (var item in items)
                            {
                                if (!string.IsNullOrWhiteSpace(item))
                                    conn.Execute($@"
                                        INSERT INTO {_cp}.cp_check_sheet_item
                                            (check_sheet_id, emp_no, group_id)
                                        VALUES (@id, @item, @groupId)",
                                        new { id, item = item.Trim(), groupId }, tx);
                            }
                        }
                        tx.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ApproveRepository.EditCheckSheet] " + ex.Message);
                throw;
            }
        }

        public void DeleteCheckSheet(int id)
        {
            try
            {
                // CASCADE handles items
                var sql = $"DELETE FROM {_cp}.cp_check_sheet WHERE id = @id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { id });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ApproveRepository.DeleteCheckSheet] " + ex.Message);
                throw;
            }
        }

        // -----------------------------------------------------------------------
        // Safety helper: filter value should only contain digits, commas, quotes, spaces
        // -----------------------------------------------------------------------
        private static bool IsSafeFilter(string value)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"^[0-9,'\s]+$");
        }
    }
}
