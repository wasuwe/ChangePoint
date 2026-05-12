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
    public class GroupRepository : IGroupRepository
    {
        private readonly string _cp   = GlobalConfig.CpSchema;
        private readonly string _main = GlobalConfig.MainSchema;

        public IEnumerable<GroupDto> ListAll()
        {
            try
            {
                var sql = $@"
                    SELECT g.id, g.name, g.wc_list, g.is_use,
                           g.menu_1, g.menu_2, g.menu_3, g.menu_4, g.menu_5,
                           g.create_by, g.create_date, g.update_by, g.update_date,
                           e.gname_eng, e.fname_eng
                    FROM {_cp}.cp_group g
                    LEFT JOIN {_main}.center_tm_employee e ON g.update_by = e.emp_no
                    WHERE g.name <> 'hide'
                    ORDER BY g.id";

                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql).Select(r =>
                    {
                        var createBy  = (string)r.create_by ?? "";
                        var updateBy  = (string)r.update_by ?? "";
                        var cd        = (DateTime?)r.create_date;
                        var ud        = (DateTime?)r.update_date;
                        return new GroupDto
                        {
                            ITEM_ID     = ((int)r.id).ToString(),
                            ITEM_NAME   = (string)r.name ?? "",
                            ITEM_WC     = (string)r.wc_list ?? "",
                            IS_USE      = (bool)r.is_use ? "Y" : "N",
                            MENU_1      = (string)r.menu_1 ?? "",
                            MENU_2      = (string)r.menu_2 ?? "",
                            MENU_3      = (string)r.menu_3 ?? "",
                            MENU_4      = (string)r.menu_4 ?? "",
                            MENU_5      = (string)r.menu_5 ?? "",
                            GNAME_ENG   = (string)r.gname_eng ?? "",
                            FNAME_ENG   = (string)r.fname_eng ?? "",
                            UPDATE_BY   = string.IsNullOrEmpty(updateBy) ? createBy : updateBy,
                            UPDATE_DATE = string.IsNullOrEmpty(updateBy)
                                            ? cd?.ToString("dd/MM/yy HH:mm") ?? ""
                                            : ud?.ToString("dd/MM/yy HH:mm") ?? "",
                            TOOL = "-"
                        };
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[GroupRepository.ListAll] " + ex.Message);
                throw;
            }
        }

        public GroupDto GetById(int id)
        {
            try
            {
                var sql = $"SELECT * FROM {_cp}.cp_group WHERE id = @id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    var r = conn.QueryFirstOrDefault(sql, new { id });
                    if (r == null) return null;
                    var createBy = (string)r.create_by ?? "";
                    var updateBy = (string)r.update_by ?? "";
                    var cd       = (DateTime?)r.create_date;
                    var ud       = (DateTime?)r.update_date;
                    return new GroupDto
                    {
                        ITEM_ID     = ((int)r.id).ToString(),
                        ITEM_NAME   = (string)r.name ?? "",
                        ITEM_WC     = (string)r.wc_list ?? "",
                        IS_USE      = (bool)r.is_use ? "Y" : "N",
                        MENU_1      = (string)r.menu_1 ?? "",
                        MENU_2      = (string)r.menu_2 ?? "",
                        MENU_3      = (string)r.menu_3 ?? "",
                        MENU_4      = (string)r.menu_4 ?? "",
                        MENU_5      = (string)r.menu_5 ?? "",
                        UPDATE_BY   = string.IsNullOrEmpty(updateBy) ? createBy : updateBy,
                        UPDATE_DATE = string.IsNullOrEmpty(updateBy)
                                        ? cd?.ToString("dd/MM/yy") ?? ""
                                        : ud?.ToString("dd/MM/yy") ?? ""
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[GroupRepository.GetById] " + ex.Message);
                throw;
            }
        }

        public IEnumerable<GroupOptionDto> GetByWc(string wcCode, string role)
        {
            try
            {
                string sql;
                object param;

                if (role == "A" || role == "G")
                {
                    sql = $@"
                        SELECT id AS group_id, name, menu_1, menu_2, menu_3, menu_4, menu_5
                        FROM {_cp}.cp_group
                        WHERE is_use = TRUE
                        ORDER BY id";
                    param = new { };
                }
                else
                {
                    sql = $@"
                        SELECT DISTINCT w.group_id, g.name,
                               e.wc_name, w.wc_code,
                               g.menu_1, g.menu_2, g.menu_3, g.menu_4, g.menu_5
                        FROM {_cp}.cp_group_wc w
                        JOIN  {_cp}.cp_group g ON w.group_id = g.id
                        LEFT JOIN (
                            SELECT DISTINCT wc_code, wc_name FROM {_main}.center_tm_employee
                        ) e ON w.wc_code = e.wc_code
                        WHERE w.wc_code = @wcCode AND g.is_use = TRUE
                        ORDER BY w.group_id";
                    param = new { wcCode };
                }

                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql, param).Select(r =>
                    {
                        var dto = new GroupOptionDto
                        {
                            GROUP_ID   = ((int)r.group_id).ToString(),
                            GROUP_NAME = (string)r.name ?? "",
                            MENU_1     = (string)r.menu_1 ?? "",
                            MENU_2     = (string)r.menu_2 ?? "",
                            MENU_3     = (string)r.menu_3 ?? "",
                            MENU_4     = (string)r.menu_4 ?? "",
                            MENU_5     = (string)r.menu_5 ?? ""
                        };
                        if (role != "A" && role != "G")
                        {
                            dto.WC_CODE = (string)r.wc_code ?? "";
                            dto.WC_NAME = (string)r.wc_name ?? "";
                        }
                        return dto;
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[GroupRepository.GetByWc] " + ex.Message);
                throw;
            }
        }

        public int Add(string empNo)
        {
            try
            {
                var sql = $@"
                    INSERT INTO {_cp}.cp_group (name, is_use, create_by, create_date, update_by, update_date)
                    VALUES ('hide', TRUE, @empNo, NOW(), @empNo, NOW())
                    RETURNING id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    return conn.ExecuteScalar<int>(sql, new { empNo });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[GroupRepository.Add] " + ex.Message);
                throw;
            }
        }

        public void Edit(int id, string name, string wcList, bool isUse,
                         string menu1, string menu2, string menu3, string menu4, string menu5,
                         string empNo)
        {
            try
            {
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    conn.Open();
                    using (var tx = conn.BeginTransaction())
                    {
                        conn.Execute($@"
                            UPDATE {_cp}.cp_group
                            SET name = @name, wc_list = @wcList, is_use = @isUse,
                                menu_1 = @menu1, menu_2 = @menu2, menu_3 = @menu3, menu_4 = @menu4, menu_5 = @menu5,
                                update_by = @empNo, update_date = NOW()
                            WHERE id = @id",
                            new { id, name, wcList, isUse, menu1, menu2, menu3, menu4, menu5, empNo },
                            tx);

                        conn.Execute($"DELETE FROM {_cp}.cp_group_wc WHERE group_id = @id", new { id }, tx);

                        if (!string.IsNullOrEmpty(wcList))
                        {
                            foreach (var wc in wcList.Split(','))
                            {
                                if (!string.IsNullOrWhiteSpace(wc))
                                    conn.Execute(
                                        $"INSERT INTO {_cp}.cp_group_wc (group_id, wc_code) VALUES (@id, @wc)",
                                        new { id, wc = wc.Trim() }, tx);
                            }
                        }
                        tx.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[GroupRepository.Edit] " + ex.Message);
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                // CASCADE on FK handles related rows
                var sql = $"DELETE FROM {_cp}.cp_group WHERE id = @id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { id });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[GroupRepository.Delete] " + ex.Message);
                throw;
            }
        }

        public void DeleteOldHidden()
        {
            try
            {
                var cutoff = DateTime.Now.AddMinutes(-10);
                var sql = $"DELETE FROM {_cp}.cp_group WHERE name = 'hide' AND create_date < @cutoff";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { cutoff });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[GroupRepository.DeleteOldHidden] " + ex.Message);
                throw;
            }
        }

        public void SetColors(int id, string c1, string c2, string c3, string c4, string c5)
        {
            try
            {
                var sql = $@"
                    UPDATE {_cp}.cp_group
                    SET color_1 = @c1, color_2 = @c2, color_3 = @c3, color_4 = @c4, color_5 = @c5
                    WHERE id = @id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { id, c1, c2, c3, c4, c5 });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[GroupRepository.SetColors] " + ex.Message);
                throw;
            }
        }

        public ColorsDto GetColors(int id)
        {
            try
            {
                var sql = $"SELECT color_1, color_2, color_3, color_4, color_5 FROM {_cp}.cp_group WHERE id = @id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    var r = conn.QueryFirstOrDefault(sql, new { id });
                    if (r == null) return null;
                    return new ColorsDto
                    {
                        ITEM_COLORS_1 = (string)r.color_1 ?? "",
                        ITEM_COLORS_2 = (string)r.color_2 ?? "",
                        ITEM_COLORS_3 = (string)r.color_3 ?? "",
                        ITEM_COLORS_4 = (string)r.color_4 ?? "",
                        ITEM_COLORS_5 = (string)r.color_5 ?? ""
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[GroupRepository.GetColors] " + ex.Message);
                throw;
            }
        }
    }
}
