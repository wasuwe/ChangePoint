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
    public class CalendarRepository : ICalendarRepository
    {
        private readonly string _cp   = GlobalConfig.CpSchema;
        private readonly string _main = GlobalConfig.MainSchema;

        // -----------------------------------------------------------------------
        // Helper: map a raw Dapper dynamic row → CalendarDto
        // -----------------------------------------------------------------------
        private CalendarDto Map(dynamic r)
        {
            var createDate = (DateTime?)r.create_date;
            var updateDate = (DateTime?)r.update_date;
            var createBy   = (string)r.create_by ?? "";
            var updateBy   = (string)r.update_by ?? "";

            // PostgreSQL arrays come through as arrays; join for legacy display
            string DetailsStr  = ArrayToString(r.details);
            string ActionStr   = ArrayToString(r.action);
            string WarningsStr = ArrayToString(r.warnings);
            string RecipStr    = ArrayToString(r.recipient);

            return new CalendarDto
            {
                ID                   = ((int)r.id).ToString(),
                SHIFT                = (string)r.shift ?? "",
                SHIFT_TEAM           = (string)r.shift_team ?? "",
                DATE_CHANGE          = r.date_change?.ToString() ?? "",
                MAN_SPOT             = (string)r.man_spot ?? "",
                MAN_INSTEAD          = (string)r.man_instead ?? "",
                MC_NO                = (string)r.mc_no ?? "",
                EDIT_POINT           = (string)r.edit_point ?? "",
                PART_NO              = (string)r.part_no ?? "",
                MOLD_NO              = (string)r.mold_no ?? "",
                CHANGE               = (string)r.change ?? "",
                PROCESS_POINT        = (string)r.process_point ?? "",
                DETAILS              = DetailsStr,
                ACTION               = ActionStr,
                WARNINGS             = WarningsStr,
                STATUS_TYPE          = (string)r.status_type ?? "",
                REMARK               = (string)r.remark ?? "",
                INFORMED             = ((bool?)r.informed ?? false) ? "Y" : "N",
                RECIPIENT_ID         = RecipStr,
                MAN_SPOT_NAME_ENG    = (string)r.man_spot_name_eng ?? "",
                MAN_SPOT_NAME_THA    = (string)r.man_spot_name_tha ?? "",
                MAN_INSTEAD_NAME_ENG = (string)r.man_instead_name_eng ?? "",
                MAN_INSTEAD_NAME_THA = (string)r.man_instead_name_tha ?? "",
                GNAME_ENG_CREATE     = HasProp(r, "gname_eng_create") ? ((string)r.gname_eng_create ?? "") : "",
                FNAME_ENG_CREATE     = HasProp(r, "fname_eng_create") ? ((string)r.fname_eng_create ?? "") : "",
                GNAME_ENG_UPDATE     = HasProp(r, "gname_eng_update") ? ((string)r.gname_eng_update ?? "") : "",
                FNAME_ENG_UPDATE     = HasProp(r, "fname_eng_update") ? ((string)r.fname_eng_update ?? "") : "",
                CREATE_BY            = createBy,
                CREATE_DATE          = createDate?.ToString("dd/MM/yy HH:mm") ?? "",
                UPDATE_BY            = string.IsNullOrEmpty(updateBy) ? createBy : updateBy,
                UPDATE_DATE          = string.IsNullOrEmpty(updateBy)
                                            ? createDate?.ToString("dd/MM/yy HH:mm") ?? ""
                                            : updateDate?.ToString("dd/MM/yy HH:mm") ?? "",
                RECIPIENT_LIST       = new List<ApproveDto>()
            };
        }

        private static string ArrayToString(object arr)
        {
            if (arr == null) return "";
            if (arr is int[]    ia) return string.Join(",", ia);
            if (arr is string[] sa) return string.Join(",", sa);
            return arr.ToString();
        }

        private static bool HasProp(dynamic d, string name)
        {
            try { var _ = ((IDictionary<string, object>)d)[name]; return true; }
            catch { return false; }
        }

        // -----------------------------------------------------------------------
        public IEnumerable<CalendarDto> ListAll(int groupId)
        {
            try
            {
                var sql = $@"
                    SELECT c.*,
                           e1.gname_eng AS gname_eng_create, e1.fname_eng AS fname_eng_create,
                           e2.gname_eng AS gname_eng_update, e2.fname_eng AS fname_eng_update
                    FROM {_cp}.cp_calendar c
                    LEFT JOIN {_main}.center_tm_employee e1 ON c.create_by = e1.emp_no
                    LEFT JOIN {_main}.center_tm_employee e2 ON c.update_by = e2.emp_no
                    WHERE c.status_type <> 'hide'
                      AND c.group_id = @groupId
                    ORDER BY c.date_change DESC";

                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql, new { groupId })
                               .Select(r => Map(r))
                               .ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[CalendarRepository.ListAll] " + ex.Message);
                throw;
            }
        }

        public IEnumerable<CalendarDto> ListMy(int groupId, string empNo)
        {
            try
            {
                var sql = $@"
                    SELECT c.*,
                           e1.gname_eng AS gname_eng_create, e1.fname_eng AS fname_eng_create,
                           e2.gname_eng AS gname_eng_update, e2.fname_eng AS fname_eng_update
                    FROM {_cp}.cp_calendar c
                    LEFT JOIN {_main}.center_tm_employee e1 ON c.create_by = e1.emp_no
                    LEFT JOIN {_main}.center_tm_employee e2 ON c.update_by = e2.emp_no
                    WHERE c.status_type <> 'hide'
                      AND c.group_id   = @groupId
                      AND c.create_by  = @empNo
                    ORDER BY c.date_change DESC";

                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql, new { groupId, empNo })
                               .Select(r => Map(r))
                               .ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[CalendarRepository.ListMy] " + ex.Message);
                throw;
            }
        }

        public CalendarDto GetItem(int id)
        {
            try
            {
                var sql = $@"
                    SELECT c.*,
                           e1.gname_eng AS gname_eng_create, e1.fname_eng AS fname_eng_create,
                           e2.gname_eng AS gname_eng_update, e2.fname_eng AS fname_eng_update
                    FROM {_cp}.cp_calendar c
                    LEFT JOIN {_main}.center_tm_employee e1 ON c.create_by = e1.emp_no
                    LEFT JOIN {_main}.center_tm_employee e2 ON c.update_by = e2.emp_no
                    WHERE c.id = @id";

                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    var row = conn.QueryFirstOrDefault(sql, new { id });
                    return row == null ? null : Map(row);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[CalendarRepository.GetItem] " + ex.Message);
                throw;
            }
        }

        public IEnumerable<CalendarDto> ListByDay(string dateSelect, int groupId)
        {
            try
            {
                var sql = $@"
                    SELECT c.*,
                           e1.gname_eng AS gname_eng_create, e1.fname_eng AS fname_eng_create,
                           e2.gname_eng AS gname_eng_update, e2.fname_eng AS fname_eng_update
                    FROM {_cp}.cp_calendar c
                    LEFT JOIN {_main}.center_tm_employee e1 ON c.create_by = e1.emp_no
                    LEFT JOIN {_main}.center_tm_employee e2 ON c.update_by = e2.emp_no
                    WHERE c.date_change >= @dateStart
                      AND c.date_change <= @dateEnd
                      AND c.status_type <> 'hide'
                      AND c.group_id = @groupId
                    ORDER BY c.shift ASC";

                // dateSelect is dd-MM-yyyy
                DateTime.TryParseExact(dateSelect, "dd-MM-yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime d);

                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql, new
                    {
                        dateStart = d.Date,
                        dateEnd   = d.Date.AddDays(1).AddSeconds(-1),
                        groupId
                    }).Select(r => Map(r)).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[CalendarRepository.ListByDay] " + ex.Message);
                throw;
            }
        }

        public IEnumerable<CalendarDto> ListOver()
        {
            try
            {
                var cutoff = DateTime.Now.AddMinutes(-10);
                var sql = $@"
                    SELECT id FROM {_cp}.cp_calendar
                    WHERE create_date < @cutoff
                      AND status_type = 'hide'";

                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql, new { cutoff })
                               .Select(r => new CalendarDto { ID = ((int)r.id).ToString() })
                               .ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[CalendarRepository.ListOver] " + ex.Message);
                throw;
            }
        }

        public int Add(AddCalendarParams p)
        {
            try
            {
                var sql = $@"
                    INSERT INTO {_cp}.cp_calendar
                        (group_id, date_change, shift, shift_team,
                         man_spot, man_instead, man_spot_name_eng, man_spot_name_tha,
                         man_instead_name_eng, man_instead_name_tha,
                         mc_no, edit_point, part_no, mold_no, change, process_point,
                         details, action, warnings, status_type, remark,
                         informed, recipient, type,
                         create_by, create_date, update_by, update_date)
                    VALUES
                        (@groupId, @dateChange, @shift, @team,
                         @spot, @instead, @spotEng, @spotTha,
                         @insteadEng, @insteadTha,
                         @mcNo, @edit, @partNo, @moldNo, @change, @process,
                         @detailIds, @actions, @warnings, 'show', @remark,
                         FALSE, @recipientIds, @type,
                         @empNo, NOW(), @empNo, NOW())
                    RETURNING id";

                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.ExecuteScalar<int>(sql, new
                    {
                        p.GroupId,
                        p.DateChange,
                        p.Shift,
                        p.Team,
                        p.Spot,
                        p.Instead,
                        p.SpotEng,
                        p.SpotTha,
                        p.InsteadEng,
                        p.InsteadTha,
                        p.McNo,
                        p.Edit,
                        p.PartNo,
                        p.MoldNo,
                        p.Change,
                        p.Process,
                        detailIds   = p.DetailIds   ?? new int[0],
                        actions     = p.Actions     ?? new string[0],
                        warnings    = p.Warnings    ?? new string[0],
                        p.Remark,
                        recipientIds = p.RecipientIds ?? new int[0],
                        p.Type,
                        p.EmpNo
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[CalendarRepository.Add] " + ex.Message);
                throw;
            }
        }

        public void Edit(EditCalendarParams p)
        {
            try
            {
                var sql = $@"
                    UPDATE {_cp}.cp_calendar SET
                        date_change          = @dateChange,
                        shift                = @shift,
                        shift_team           = @team,
                        man_spot             = @spot,
                        man_instead          = @instead,
                        man_spot_name_eng    = @spotEng,
                        man_spot_name_tha    = @spotTha,
                        man_instead_name_eng = @insteadEng,
                        man_instead_name_tha = @insteadTha,
                        mc_no                = @mcNo,
                        edit_point           = @edit,
                        part_no              = @partNo,
                        mold_no              = @moldNo,
                        change               = @change,
                        process_point        = @process,
                        details              = @detailIds,
                        action               = @actions,
                        warnings             = @warnings,
                        status_type          = @type,
                        remark               = @remark,
                        recipient            = @recipientIds,
                        update_by            = @empNo,
                        update_date          = NOW()
                    WHERE id = @id";

                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    conn.Execute(sql, new
                    {
                        p.Id,
                        p.DateChange,
                        p.Shift,
                        p.Team,
                        p.Spot,
                        p.Instead,
                        p.SpotEng,
                        p.SpotTha,
                        p.InsteadEng,
                        p.InsteadTha,
                        p.McNo,
                        p.Edit,
                        p.PartNo,
                        p.MoldNo,
                        p.Change,
                        p.Process,
                        detailIds    = p.DetailIds   ?? new int[0],
                        actions      = p.Actions     ?? new string[0],
                        warnings     = p.Warnings    ?? new string[0],
                        p.Remark,
                        recipientIds = p.RecipientIds ?? new int[0],
                        p.Type,
                        p.EmpNo
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[CalendarRepository.Edit] " + ex.Message);
                throw;
            }
        }

        public void SoftDelete(int id, string empNo)
        {
            try
            {
                var sql = $@"
                    UPDATE {_cp}.cp_calendar
                    SET status_type = 'hide', update_by = @empNo, update_date = NOW()
                    WHERE id = @id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { id, empNo });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[CalendarRepository.SoftDelete] " + ex.Message);
                throw;
            }
        }

        public void SetInformed(int id, string informedValue, string empNo)
        {
            try
            {
                bool informed = informedValue == "Y";
                var sql = $@"
                    UPDATE {_cp}.cp_calendar
                    SET informed = @informed, update_by = @empNo, update_date = NOW()
                    WHERE id = @id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { id, informed, empNo });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[CalendarRepository.SetInformed] " + ex.Message);
                throw;
            }
        }

        public void DeleteOver(int id)
        {
            try
            {
                var sql = $"DELETE FROM {_cp}.cp_calendar WHERE id = @id AND status_type = 'hide'";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { id });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[CalendarRepository.DeleteOver] " + ex.Message);
                throw;
            }
        }
    }
}
