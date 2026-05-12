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
    public class FileRepository : IFileRepository
    {
        private readonly string _cp = GlobalConfig.CpSchema;

        public int Add(string name, string fileType, string fileSize, int groupId)
        {
            try
            {
                var sql = $@"
                    INSERT INTO {_cp}.cp_files (calendar_id, group_id, name, file_type, file_size, create_date)
                    VALUES (0, @groupId, @name, @fileType, @fileSize, NOW())
                    RETURNING id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    return conn.ExecuteScalar<int>(sql, new { groupId, name, fileType, fileSize });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[FileRepository.Add] " + ex.Message);
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                var sql = $"DELETE FROM {_cp}.cp_files WHERE id = @id";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { id });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[FileRepository.Delete] " + ex.Message);
                throw;
            }
        }

        public void LinkToCalendar(int[] fileIds, int calendarId)
        {
            if (fileIds == null || fileIds.Length == 0) return;
            try
            {
                var sql = $@"
                    UPDATE {_cp}.cp_files
                    SET calendar_id = @calendarId
                    WHERE id = ANY(@fileIds)";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    conn.Execute(sql, new { calendarId, fileIds });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[FileRepository.LinkToCalendar] " + ex.Message);
                throw;
            }
        }

        public IEnumerable<FileDto> GetByCalendarId(int calendarId)
        {
            try
            {
                var sql = $"SELECT * FROM {_cp}.cp_files WHERE calendar_id = @calendarId";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    return conn.Query(sql, new { calendarId })
                               .Select(r => MapFile(r)).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[FileRepository.GetByCalendarId] " + ex.Message);
                throw;
            }
        }

        public IEnumerable<FileDto> GetByIds(int[] ids)
        {
            if (ids == null || ids.Length == 0) return Enumerable.Empty<FileDto>();
            try
            {
                var sql = $"SELECT * FROM {_cp}.cp_files WHERE id = ANY(@ids)";
                using (var conn = PostgreSqlDbConnection.GetConnection())
                    return conn.Query(sql, new { ids }).Select(r => MapFile(r)).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[FileRepository.GetByIds] " + ex.Message);
                throw;
            }
        }

        public IEnumerable<FileDto> GetByCalendarIdRaw(int calendarId)
        {
            return GetByCalendarId(calendarId);
        }

        private static FileDto MapFile(dynamic r) => new FileDto
        {
            ITEM_ID          = ((int)r.id).ToString(),
            ITEM_NAME        = (string)r.name ?? "",
            ITEM_FILE_TYPE   = (string)r.file_type ?? "",
            ITEM_FILE_SIZE   = (string)r.file_size ?? "",
            ITEM_CALENDAR_ID = ((int)r.calendar_id).ToString()
        };
    }
}
