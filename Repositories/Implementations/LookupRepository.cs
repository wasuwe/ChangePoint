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
    public class LookupRepository : ILookupRepository
    {
        private readonly string _cp   = GlobalConfig.CpSchema;
        private readonly string _main = GlobalConfig.MainSchema;

        // Whitelist of allowed table names to prevent SQL injection
        private static readonly HashSet<string> _allowedTables = new HashSet<string>
        {
            "cp_mold_no", "cp_process", "cp_edit", "cp_change"
        };

        private static string ValidateTable(string table)
        {
            if (!_allowedTables.Contains(table))
                throw new ArgumentException($"Table '{table}' is not a valid lookup table.");
            return table;
        }

        // -----------------------------------------------------------------------
        // Employees
        // -----------------------------------------------------------------------
        public IEnumerable<EmployeeDto> GetEmployees()
        {
            try
            {
                var sql = $@"
                    SELECT emp_no, gname_th, fname_th, gname_eng, fname_eng, wc_code, wc_name
                    FROM {_main}.center_tm_employee
                    ORDER BY gname_eng, fname_eng";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql).Select(r => new EmployeeDto
                    {
                        EMP_NO    = (string)r.emp_no   ?? "",
                        GNAME_TH  = (string)r.gname_th  ?? "",
                        FNAME_TH  = (string)r.fname_th  ?? "",
                        GNAME_ENG = (string)r.gname_eng ?? "",
                        FNAME_ENG = (string)r.fname_eng ?? "",
                        WC_CODE   = (string)r.wc_code   ?? "",
                        WC_NAME   = (string)r.wc_name   ?? ""
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.GetEmployees] " + ex.Message);
                throw;
            }
        }

        // -----------------------------------------------------------------------
        // Generic lookup (cp_mold_no, cp_process, cp_edit, cp_change)
        // -----------------------------------------------------------------------
        public IEnumerable<LookupItemDto> ListLookup(string table, int groupId)
        {
            ValidateTable(table);
            try
            {
                var sql = $@"
                    SELECT t.id, t.name, t.is_use,
                           t.create_by, t.create_date, t.update_by, t.update_date,
                           e.gname_eng, e.fname_eng
                    FROM {_cp}.{table} t
                    LEFT JOIN {_main}.center_tm_employee e ON t.update_by = e.emp_no
                    WHERE t.group_id = @groupId
                    ORDER BY t.id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql, new { groupId }).Select(r => MapLookupItem(r)).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[LookupRepository.ListLookup:{table}] " + ex.Message);
                throw;
            }
        }

        public IEnumerable<OptionDto> OptionLookup(string table, int groupId)
        {
            ValidateTable(table);
            try
            {
                var sql = $@"
                    SELECT id, name
                    FROM {_cp}.{table}
                    WHERE group_id = @groupId AND is_use = TRUE
                    ORDER BY name";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql, new { groupId }).Select(r => new OptionDto
                    {
                        ITEM_ID   = ((int)r.id).ToString(),
                        ITEM_NAME = (string)r.name ?? ""
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[LookupRepository.OptionLookup:{table}] " + ex.Message);
                throw;
            }
        }

        public LookupItemDto SelectLookup(string table, int id)
        {
            ValidateTable(table);
            try
            {
                var sql = $@"
                    SELECT t.id, t.name, t.is_use,
                           t.create_by, t.create_date, t.update_by, t.update_date,
                           e.gname_eng, e.fname_eng
                    FROM {_cp}.{table} t
                    LEFT JOIN {_main}.center_tm_employee e ON t.update_by = e.emp_no
                    WHERE t.id = @id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    var row = conn.QueryFirstOrDefault(sql, new { id });
                    return row == null ? null : MapLookupItem(row);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[LookupRepository.SelectLookup:{table}] " + ex.Message);
                throw;
            }
        }

        public int AddLookup(string table, string name, int groupId, bool isUse, string empNo)
        {
            ValidateTable(table);
            try
            {
                var sql = $@"
                    INSERT INTO {_cp}.{table} (group_id, name, is_use, create_by, create_date, update_by, update_date)
                    VALUES (@groupId, @name, @isUse, @empNo, NOW(), @empNo, NOW())
                    RETURNING id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    return conn.ExecuteScalar<int>(sql, new { groupId, name, isUse, empNo });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[LookupRepository.AddLookup:{table}] " + ex.Message);
                throw;
            }
        }

        public void EditLookup(string table, int id, string name, bool isUse, string empNo)
        {
            ValidateTable(table);
            try
            {
                var sql = $@"
                    UPDATE {_cp}.{table}
                    SET name = @name, is_use = @isUse, update_by = @empNo, update_date = NOW()
                    WHERE id = @id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { id, name, isUse, empNo });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[LookupRepository.EditLookup:{table}] " + ex.Message);
                throw;
            }
        }

        public void DeleteLookup(string table, int id)
        {
            ValidateTable(table);
            try
            {
                var sql = $"DELETE FROM {_cp}.{table} WHERE id = @id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { id });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[LookupRepository.DeleteLookup:{table}] " + ex.Message);
                throw;
            }
        }

        // -----------------------------------------------------------------------
        // Machine
        // -----------------------------------------------------------------------
        public IEnumerable<MachineListDto> ListMachines(int groupId)
        {
            try
            {
                var sql = $@"
                    SELECT m.id, m.machine_no, m.size_ton, m.is_use,
                           m.create_by, m.create_date, m.update_by, m.update_date,
                           e.gname_eng, e.fname_eng
                    FROM {_cp}.cp_machine m
                    LEFT JOIN {_main}.center_tm_employee e ON m.update_by = e.emp_no
                    WHERE m.group_id = @groupId
                    ORDER BY m.id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql, new { groupId }).Select(r =>
                    {
                        var createBy  = (string)r.create_by  ?? "";
                        var updateBy  = (string)r.update_by  ?? "";
                        var cd        = (DateTime?)r.create_date;
                        var ud        = (DateTime?)r.update_date;
                        return new MachineListDto
                        {
                            ITEM_ID      = ((int)r.id).ToString(),
                            ITEM_NAME    = (string)r.machine_no ?? "",
                            ITEM_SIZETON = (string)r.size_ton   ?? "",
                            IS_USE       = (bool)r.is_use ? "Y" : "N",
                            GNAME_ENG    = (string)r.gname_eng  ?? "",
                            FNAME_ENG    = (string)r.fname_eng  ?? "",
                            UPDATE_BY    = string.IsNullOrEmpty(updateBy) ? createBy : updateBy,
                            UPDATE_DATE  = string.IsNullOrEmpty(updateBy)
                                            ? cd?.ToString("dd/MM/yy HH:mm") ?? ""
                                            : ud?.ToString("dd/MM/yy HH:mm") ?? "",
                            TOOL         = "-"
                        };
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.ListMachines] " + ex.Message);
                throw;
            }
        }

        public IEnumerable<MachineOptionDto> OptionMachines(int groupId)
        {
            try
            {
                var sql = $@"
                    SELECT id, machine_no, size_ton
                    FROM {_cp}.cp_machine
                    WHERE group_id = @groupId AND is_use = TRUE
                    ORDER BY machine_no";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql, new { groupId }).Select(r => new MachineOptionDto
                    {
                        ITEM_ID      = ((int)r.id).ToString(),
                        ITEM_NAME    = (string)r.machine_no ?? "",
                        ITEM_SIZETON = (string)r.size_ton   ?? ""
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.OptionMachines] " + ex.Message);
                throw;
            }
        }

        public MachineListDto SelectMachine(int id)
        {
            try
            {
                var sql = $@"
                    SELECT m.id, m.machine_no, m.size_ton, m.is_use,
                           m.create_by, m.create_date, m.update_by, m.update_date,
                           e.gname_eng, e.fname_eng
                    FROM {_cp}.cp_machine m
                    LEFT JOIN {_main}.center_tm_employee e ON m.update_by = e.emp_no
                    WHERE m.id = @id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    var r = conn.QueryFirstOrDefault(sql, new { id });
                    if (r == null) return null;
                    var createBy = (string)r.create_by ?? "";
                    var updateBy = (string)r.update_by ?? "";
                    var cd       = (DateTime?)r.create_date;
                    var ud       = (DateTime?)r.update_date;
                    return new MachineListDto
                    {
                        ITEM_ID      = ((int)r.id).ToString(),
                        ITEM_NAME    = (string)r.machine_no ?? "",
                        ITEM_SIZETON = (string)r.size_ton   ?? "",
                        IS_USE       = (bool)r.is_use ? "Y" : "N",
                        GNAME_ENG    = (string)r.gname_eng  ?? "",
                        FNAME_ENG    = (string)r.fname_eng  ?? "",
                        UPDATE_BY    = string.IsNullOrEmpty(updateBy) ? createBy : updateBy,
                        UPDATE_DATE  = string.IsNullOrEmpty(updateBy)
                                        ? cd?.ToString("dd/MM/yy HH:mm") ?? ""
                                        : ud?.ToString("dd/MM/yy HH:mm") ?? ""
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.SelectMachine] " + ex.Message);
                throw;
            }
        }

        public int AddMachine(string machineNo, string sizeTon, int groupId, bool isUse, string empNo)
        {
            try
            {
                var sql = $@"
                    INSERT INTO {_cp}.cp_machine (group_id, machine_no, size_ton, is_use, create_by, create_date, update_by, update_date)
                    VALUES (@groupId, @machineNo, @sizeTon, @isUse, @empNo, NOW(), @empNo, NOW())
                    RETURNING id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    return conn.ExecuteScalar<int>(sql, new { groupId, machineNo, sizeTon, isUse, empNo });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.AddMachine] " + ex.Message);
                throw;
            }
        }

        public void EditMachine(int id, string machineNo, string sizeTon, bool isUse, string empNo)
        {
            try
            {
                var sql = $@"
                    UPDATE {_cp}.cp_machine
                    SET machine_no = @machineNo, size_ton = @sizeTon, is_use = @isUse,
                        update_by = @empNo, update_date = NOW()
                    WHERE id = @id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { id, machineNo, sizeTon, isUse, empNo });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.EditMachine] " + ex.Message);
                throw;
            }
        }

        public void DeleteMachine(int id)
        {
            try
            {
                var sql = $"DELETE FROM {_cp}.cp_machine WHERE id = @id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { id });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.DeleteMachine] " + ex.Message);
                throw;
            }
        }

        // -----------------------------------------------------------------------
        // Part
        // -----------------------------------------------------------------------
        public IEnumerable<LookupItemDto> ListParts(int groupId)
        {
            try
            {
                var sql = $@"
                    SELECT p.part_no, p.part_name, p.is_use,
                           p.create_by, p.create_date, p.update_by, p.update_date,
                           e.gname_eng, e.fname_eng
                    FROM {_cp}.cp_part p
                    LEFT JOIN {_main}.center_tm_employee e ON p.update_by = e.emp_no
                    WHERE p.group_id = @groupId
                    ORDER BY p.part_no";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql, new { groupId }).Select(r =>
                    {
                        var createBy = (string)r.create_by ?? "";
                        var updateBy = (string)r.update_by ?? "";
                        var cd       = (DateTime?)r.create_date;
                        var ud       = (DateTime?)r.update_date;
                        return new LookupItemDto
                        {
                            ITEM_ID     = (string)r.part_no   ?? "",
                            ITEM_NAME   = (string)r.part_name ?? "",
                            IS_USE      = (bool)r.is_use ? "Y" : "N",
                            GNAME_ENG   = (string)r.gname_eng ?? "",
                            FNAME_ENG   = (string)r.fname_eng ?? "",
                            UPDATE_BY   = string.IsNullOrEmpty(updateBy) ? createBy : updateBy,
                            UPDATE_DATE = string.IsNullOrEmpty(updateBy)
                                            ? cd?.ToString("dd/MM/yy HH:mm") ?? ""
                                            : ud?.ToString("dd/MM/yy HH:mm") ?? "",
                            TOOL        = "-"
                        };
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.ListParts] " + ex.Message);
                throw;
            }
        }

        public IEnumerable<PartOptionDto> OptionParts(int groupId)
        {
            try
            {
                var sql = $@"
                    SELECT part_no
                    FROM {_cp}.cp_part
                    WHERE group_id = @groupId AND is_use = TRUE
                    ORDER BY part_no";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql, new { groupId }).Select(r => new PartOptionDto
                    {
                        ITEM_ID = (string)r.part_no ?? ""
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.OptionParts] " + ex.Message);
                throw;
            }
        }

        public LookupItemDto SelectPart(string partNo)
        {
            try
            {
                var sql = $@"
                    SELECT p.part_no, p.part_name, p.is_use,
                           p.create_by, p.create_date, p.update_by, p.update_date,
                           e.gname_eng, e.fname_eng
                    FROM {_cp}.cp_part p
                    LEFT JOIN {_main}.center_tm_employee e ON p.update_by = e.emp_no
                    WHERE p.part_no = @partNo";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    var r = conn.QueryFirstOrDefault(sql, new { partNo });
                    if (r == null) return null;
                    var createBy = (string)r.create_by ?? "";
                    var updateBy = (string)r.update_by ?? "";
                    var cd       = (DateTime?)r.create_date;
                    var ud       = (DateTime?)r.update_date;
                    return new LookupItemDto
                    {
                        ITEM_ID     = (string)r.part_no   ?? "",
                        ITEM_NAME   = (string)r.part_name ?? "",
                        IS_USE      = (bool)r.is_use ? "Y" : "N",
                        GNAME_ENG   = (string)r.gname_eng ?? "",
                        FNAME_ENG   = (string)r.fname_eng ?? "",
                        UPDATE_BY   = string.IsNullOrEmpty(updateBy) ? createBy : updateBy,
                        UPDATE_DATE = string.IsNullOrEmpty(updateBy)
                                        ? cd?.ToString("dd/MM/yy HH:mm") ?? ""
                                        : ud?.ToString("dd/MM/yy HH:mm") ?? ""
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.SelectPart] " + ex.Message);
                throw;
            }
        }

        public bool AddPart(string partNo, string partName, int groupId, bool isUse, string empNo)
        {
            try
            {
                var sql = $@"
                    INSERT INTO {_cp}.cp_part (part_no, group_id, part_name, is_use, create_by, create_date, update_by, update_date)
                    VALUES (@partNo, @groupId, @partName, @isUse, @empNo, NOW(), @empNo, NOW())
                    ON CONFLICT DO NOTHING";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    int rows = conn.Execute(sql, new { partNo, groupId, partName, isUse, empNo });
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.AddPart] " + ex.Message);
                throw;
            }
        }

        public void EditPart(string partNo, string partName, bool isUse, string empNo)
        {
            try
            {
                var sql = $@"
                    UPDATE {_cp}.cp_part
                    SET part_name = @partName, is_use = @isUse, update_by = @empNo, update_date = NOW()
                    WHERE part_no = @partNo";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { partNo, partName, isUse, empNo });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.EditPart] " + ex.Message);
                throw;
            }
        }

        public void DeletePart(string partNo, int groupId)
        {
            try
            {
                var sql = $"DELETE FROM {_cp}.cp_part WHERE part_no = @partNo AND group_id = @groupId";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { partNo, groupId });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.DeletePart] " + ex.Message);
                throw;
            }
        }

        // -----------------------------------------------------------------------
        // Details & Categories
        // -----------------------------------------------------------------------
        public IEnumerable<DetailsListDto> ListDetails(int groupId, string filterType, string filterCategory)
        {
            try
            {
                var sql = $@"
                    SELECT d.id, d.detail, d.risk, d.category_id, c.name AS category_name,
                           d.is_use, d.create_by, d.create_date, d.update_by, d.update_date,
                           e.gname_eng, e.fname_eng
                    FROM {_cp}.cp_details d
                    LEFT JOIN {_cp}.cp_details_category c ON d.category_id = c.id
                    LEFT JOIN {_main}.center_tm_employee e ON d.update_by = e.emp_no
                    WHERE d.group_id = @groupId";

                var param = new DynamicParameters();
                param.Add("groupId", groupId);

                if (!string.IsNullOrEmpty(filterType))
                {
                    sql += " AND c.type = @filterType";
                    param.Add("filterType", filterType);
                }
                if (!string.IsNullOrEmpty(filterCategory) && int.TryParse(filterCategory, out int catId))
                {
                    sql += " AND d.category_id = @catId";
                    param.Add("catId", catId);
                }
                sql += " ORDER BY d.id";

                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql, param).Select(r =>
                    {
                        var createBy = (string)r.create_by ?? "";
                        var updateBy = (string)r.update_by ?? "";
                        var cd       = (DateTime?)r.create_date;
                        var ud       = (DateTime?)r.update_date;
                        return new DetailsListDto
                        {
                            ITEM_ID            = ((int)r.id).ToString(),
                            ITEM_DETAIL        = (string)r.detail        ?? "",
                            ITEM_RISK          = (string)r.risk          ?? "",
                            ITEM_CATEGORY_ID   = r.category_id == null ? "" : ((int)r.category_id).ToString(),
                            ITEM_CATEGORY_NAME = (string)r.category_name ?? "",
                            IS_USE             = (bool)r.is_use ? "Y" : "N",
                            GNAME_ENG          = (string)r.gname_eng ?? "",
                            FNAME_ENG          = (string)r.fname_eng ?? "",
                            UPDATE_BY          = string.IsNullOrEmpty(updateBy) ? createBy : updateBy,
                            UPDATE_DATE        = string.IsNullOrEmpty(updateBy)
                                                    ? cd?.ToString("dd/MM/yy HH:mm") ?? ""
                                                    : ud?.ToString("dd/MM/yy HH:mm") ?? ""
                        };
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.ListDetails] " + ex.Message);
                throw;
            }
        }

        public IEnumerable<DetailsOptionDto> OptionDetails(int groupId, string filterType)
        {
            try
            {
                var sql = $@"
                    SELECT d.id, d.detail, d.risk, c.name AS category_name
                    FROM {_cp}.cp_details d
                    LEFT JOIN {_cp}.cp_details_category c ON d.category_id = c.id
                    WHERE d.group_id = @groupId AND d.is_use = TRUE";

                var param = new DynamicParameters();
                param.Add("groupId", groupId);

                if (!string.IsNullOrEmpty(filterType))
                {
                    sql += " AND c.type = @filterType";
                    param.Add("filterType", filterType);
                }
                sql += " ORDER BY d.id";

                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql, param).Select(r => new DetailsOptionDto
                    {
                        ITEM_ID            = ((int)r.id).ToString(),
                        ITEM_DETAIL        = (string)r.detail        ?? "",
                        ITEM_RISK          = (string)r.risk          ?? "",
                        ITEM_CATEGORY_NAME = (string)r.category_name ?? ""
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.OptionDetails] " + ex.Message);
                throw;
            }
        }

        public IEnumerable<LookupItemDto> ListDetailCategories(int groupId, string filterType)
        {
            try
            {
                var sql = $@"
                    SELECT c.id, c.name, c.type, c.is_use,
                           c.create_by, c.create_date, c.update_by, c.update_date,
                           e.gname_eng, e.fname_eng
                    FROM {_cp}.cp_details_category c
                    LEFT JOIN {_main}.center_tm_employee e ON c.update_by = e.emp_no
                    WHERE c.group_id = @groupId";

                var param = new DynamicParameters();
                param.Add("groupId", groupId);

                if (!string.IsNullOrEmpty(filterType))
                {
                    sql += " AND c.type = @filterType";
                    param.Add("filterType", filterType);
                }
                sql += " ORDER BY c.id";

                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql, param).Select(r => MapLookupItem(r)).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.ListDetailCategories] " + ex.Message);
                throw;
            }
        }

        public IEnumerable<DetailCategoryOptionDto> OptionDetailCategories(int groupId)
        {
            try
            {
                var sql = $@"
                    SELECT id, name, type
                    FROM {_cp}.cp_details_category
                    WHERE group_id = @groupId AND is_use = TRUE
                    ORDER BY id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql, new { groupId }).Select(r => new DetailCategoryOptionDto
                    {
                        ITEM_ID   = ((int)r.id).ToString(),
                        ITEM_NAME = (string)r.name ?? "",
                        ITEM_TYPE = (string)r.type ?? ""
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.OptionDetailCategories] " + ex.Message);
                throw;
            }
        }

        public void AddDetail(string detail, string risk, int categoryId, int groupId, bool isUse, string empNo)
        {
            try
            {
                var sql = $@"
                    INSERT INTO {_cp}.cp_details (group_id, category_id, detail, risk, is_use, create_by, create_date, update_by, update_date)
                    VALUES (@groupId, @categoryId, @detail, @risk, @isUse, @empNo, NOW(), @empNo, NOW())";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { groupId, categoryId, detail, risk, isUse, empNo });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.AddDetail] " + ex.Message);
                throw;
            }
        }

        public void EditDetail(int id, string detail, string risk, int categoryId, bool isUse, string empNo)
        {
            try
            {
                var sql = $@"
                    UPDATE {_cp}.cp_details
                    SET detail = @detail, risk = @risk, category_id = @categoryId,
                        is_use = @isUse, update_by = @empNo, update_date = NOW()
                    WHERE id = @id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { id, detail, risk, categoryId, isUse, empNo });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.EditDetail] " + ex.Message);
                throw;
            }
        }

        public void DeleteDetail(int id)
        {
            try
            {
                var sql = $"DELETE FROM {_cp}.cp_details WHERE id = @id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { id });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.DeleteDetail] " + ex.Message);
                throw;
            }
        }

        public void AddDetailCategory(string name, string type, int groupId, bool isUse, string empNo)
        {
            try
            {
                var sql = $@"
                    INSERT INTO {_cp}.cp_details_category (group_id, name, type, is_use, create_by, create_date, update_by, update_date)
                    VALUES (@groupId, @name, @type, @isUse, @empNo, NOW(), @empNo, NOW())";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { groupId, name, type, isUse, empNo });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.AddDetailCategory] " + ex.Message);
                throw;
            }
        }

        public void EditDetailCategory(int id, string name, string type, bool isUse, string empNo)
        {
            try
            {
                var sql = $@"
                    UPDATE {_cp}.cp_details_category
                    SET name = @name, type = @type, is_use = @isUse, update_by = @empNo, update_date = NOW()
                    WHERE id = @id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { id, name, type, isUse, empNo });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.EditDetailCategory] " + ex.Message);
                throw;
            }
        }

        public void DeleteDetailCategory(int id)
        {
            try
            {
                var sql = $"DELETE FROM {_cp}.cp_details_category WHERE id = @id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { id });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.DeleteDetailCategory] " + ex.Message);
                throw;
            }
        }

        // -----------------------------------------------------------------------
        // Dashboard
        // -----------------------------------------------------------------------
        public IEnumerable<DashboardSettingDto> ListDashboard(int groupId)
        {
            try
            {
                var sql = $@"
                    SELECT priority, title, time, type
                    FROM {_cp}.cp_dashboard
                    WHERE group_id = @groupId
                    ORDER BY priority";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql, new { groupId }).Select(r => new DashboardSettingDto
                    {
                        PRIORITY = ((int)r.priority).ToString(),
                        TITLE    = (string)r.title ?? "",
                        TIME     = (string)r.time  ?? "",
                        TYPE     = (string)r.type  ?? ""
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.ListDashboard] " + ex.Message);
                throw;
            }
        }

        public void SaveDashboard(int groupId, string type, IEnumerable<DashboardSettingDto> items)
        {
            try
            {
                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    conn.Open();
                    using (var tx = conn.BeginTransaction())
                    {
                        conn.Execute(
                            $"DELETE FROM {_cp}.cp_dashboard WHERE group_id = @groupId AND type = @type",
                            new { groupId, type }, tx);

                        int priority = 1;
                        foreach (var item in items)
                        {
                            conn.Execute($@"
                                INSERT INTO {_cp}.cp_dashboard (group_id, priority, title, time, type)
                                VALUES (@groupId, @priority, @title, @time, @type)",
                                new { groupId, priority, title = item.TITLE, time = item.TIME, type },
                                tx);
                            priority++;
                        }
                        tx.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.SaveDashboard] " + ex.Message);
                throw;
            }
        }

        // -----------------------------------------------------------------------
        // Report Summaries
        // -----------------------------------------------------------------------
        public IEnumerable<SummaryDayDto> SummaryDay(string dateSelect, int groupId)
        {
            try
            {
                // dateSelect is dd-MM-yyyy
                DateTime.TryParseExact(dateSelect, "dd-MM-yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime d);

                var sql = $@"
                    SELECT type, COUNT(*) AS count
                    FROM {_cp}.cp_calendar
                    WHERE group_id = @groupId
                      AND status_type <> 'hide'
                      AND date_change >= @dateStart
                      AND date_change <= @dateEnd
                    GROUP BY type
                    ORDER BY type";

                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql, new
                    {
                        groupId,
                        dateStart = d.Date,
                        dateEnd   = d.Date.AddDays(1).AddSeconds(-1)
                    }).Select(r => new SummaryDayDto
                    {
                        TYPE  = (string)r.type ?? "",
                        COUNT = ((long)r.count).ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.SummaryDay] " + ex.Message);
                throw;
            }
        }

        public IEnumerable<SummaryMonthDto> SummaryMonth(string dateStart, string dateStop, int groupId, string toCharFormat)
        {
            try
            {
                // Parse dates as dd-MM-yyyy
                DateTime.TryParseExact(dateStart, "dd-MM-yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime dStart);
                DateTime.TryParseExact(dateStop, "dd-MM-yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime dStop);

                // toCharFormat maps Oracle TO_CHAR formats → PostgreSQL to_char formats
                // e.g. "MM/YYYY" or "DD/MM/YYYY"
                // Sanitise: allow only alphanumeric, '/', '-', '_'
                var safeFormat = System.Text.RegularExpressions.Regex.IsMatch(toCharFormat ?? "MM/YYYY", @"^[A-Za-z0-9/_\-]+$")
                    ? toCharFormat
                    : "MM/YYYY";

                var sql = $@"
                    SELECT TO_CHAR(date_change, '{safeFormat}') AS date,
                           type,
                           COUNT(*) AS count
                    FROM {_cp}.cp_calendar
                    WHERE group_id = @groupId
                      AND status_type <> 'hide'
                      AND date_change >= @dStart
                      AND date_change <= @dStop
                    GROUP BY TO_CHAR(date_change, '{safeFormat}'), type
                    ORDER BY MIN(date_change), type";

                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql, new
                    {
                        groupId,
                        dStart = dStart.Date,
                        dStop  = dStop.Date.AddDays(1).AddSeconds(-1)
                    }).Select(r => new SummaryMonthDto
                    {
                        DATE  = (string)r.date ?? "",
                        TYPE  = (string)r.type ?? "",
                        COUNT = ((long)r.count).ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.SummaryMonth] " + ex.Message);
                throw;
            }
        }

        public IEnumerable<CalendarDto> ReportList(int groupId, string dateStart, string dateStop,
            string item5m, string itemMan, string itemMc,
            string itemMold, string itemProcess, string itemChange, string itemEdit)
        {
            try
            {
                DateTime.TryParseExact(dateStart, "dd-MM-yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime dStart);
                DateTime.TryParseExact(dateStop, "dd-MM-yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime dStop);

                var sql = $@"
                    SELECT c.*,
                           e1.gname_eng AS gname_eng_create, e1.fname_eng AS fname_eng_create,
                           e2.gname_eng AS gname_eng_update, e2.fname_eng AS fname_eng_update
                    FROM {_cp}.cp_calendar c
                    LEFT JOIN {_main}.center_tm_employee e1 ON c.create_by = e1.emp_no
                    LEFT JOIN {_main}.center_tm_employee e2 ON c.update_by = e2.emp_no
                    WHERE c.group_id = @groupId
                      AND c.status_type <> 'hide'
                      AND c.date_change >= @dStart
                      AND c.date_change <= @dStop";

                var param = new DynamicParameters();
                param.Add("groupId",  groupId);
                param.Add("dStart",   dStart.Date);
                param.Add("dStop",    dStop.Date.AddDays(1).AddSeconds(-1));

                if (!string.IsNullOrEmpty(item5m))    { sql += " AND c.type         = @item5m";    param.Add("item5m",    item5m);    }
                if (!string.IsNullOrEmpty(itemMan))   { sql += " AND c.man_spot      = @itemMan";   param.Add("itemMan",   itemMan);   }
                if (!string.IsNullOrEmpty(itemMc))    { sql += " AND c.mc_no         = @itemMc";    param.Add("itemMc",    itemMc);    }
                if (!string.IsNullOrEmpty(itemMold))  { sql += " AND c.mold_no       = @itemMold";  param.Add("itemMold",  itemMold);  }
                if (!string.IsNullOrEmpty(itemProcess)){ sql += " AND c.process_point = @itemProcess"; param.Add("itemProcess", itemProcess); }
                if (!string.IsNullOrEmpty(itemChange)){ sql += " AND c.change        = @itemChange"; param.Add("itemChange", itemChange); }
                if (!string.IsNullOrEmpty(itemEdit))  { sql += " AND c.edit_point    = @itemEdit";  param.Add("itemEdit",  itemEdit);  }

                sql += " ORDER BY c.date_change DESC";

                using (var conn = PostgreSqlDbConnection.GetConnection())
                {
                    return conn.Query(sql, param)
                               .Select(r => MapCalendar(r))
                               .ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[LookupRepository.ReportList] " + ex.Message);
                throw;
            }
        }

        // -----------------------------------------------------------------------
        // Private helpers
        // -----------------------------------------------------------------------
        private LookupItemDto MapLookupItem(dynamic r)
        {
            var createBy = (string)r.create_by ?? "";
            var updateBy = (string)r.update_by ?? "";
            var cd       = (DateTime?)r.create_date;
            var ud       = (DateTime?)r.update_date;
            return new LookupItemDto
            {
                ITEM_ID     = ((int)r.id).ToString(),
                ITEM_NAME   = (string)r.name ?? "",
                IS_USE      = (bool)r.is_use ? "Y" : "N",
                GNAME_ENG   = (string)r.gname_eng ?? "",
                FNAME_ENG   = (string)r.fname_eng ?? "",
                UPDATE_BY   = string.IsNullOrEmpty(updateBy) ? createBy : updateBy,
                UPDATE_DATE = string.IsNullOrEmpty(updateBy)
                                ? cd?.ToString("dd/MM/yy HH:mm") ?? ""
                                : ud?.ToString("dd/MM/yy HH:mm") ?? "",
                TOOL        = "-"
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

        private static CalendarDto MapCalendar(dynamic r)
        {
            var createDate = (DateTime?)r.create_date;
            var updateDate = (DateTime?)r.update_date;
            var createBy   = (string)r.create_by ?? "";
            var updateBy   = (string)r.update_by ?? "";

            return new CalendarDto
            {
                ID                   = ((int)r.id).ToString(),
                SHIFT                = (string)r.shift      ?? "",
                SHIFT_TEAM           = (string)r.shift_team ?? "",
                DATE_CHANGE          = r.date_change?.ToString() ?? "",
                MAN_SPOT             = (string)r.man_spot    ?? "",
                MAN_INSTEAD          = (string)r.man_instead ?? "",
                MC_NO                = (string)r.mc_no       ?? "",
                EDIT_POINT           = (string)r.edit_point  ?? "",
                PART_NO              = (string)r.part_no     ?? "",
                MOLD_NO              = (string)r.mold_no     ?? "",
                CHANGE               = (string)r.change      ?? "",
                PROCESS_POINT        = (string)r.process_point ?? "",
                DETAILS              = ArrayToString(r.details),
                ACTION               = ArrayToString(r.action),
                WARNINGS             = ArrayToString(r.warnings),
                STATUS_TYPE          = (string)r.status_type ?? "",
                REMARK               = (string)r.remark      ?? "",
                INFORMED             = ((bool?)r.informed ?? false) ? "Y" : "N",
                RECIPIENT_ID         = ArrayToString(r.recipient),
                MAN_SPOT_NAME_ENG    = (string)r.man_spot_name_eng    ?? "",
                MAN_SPOT_NAME_THA    = (string)r.man_spot_name_tha    ?? "",
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
    }
}
