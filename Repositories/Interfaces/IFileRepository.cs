using System.Collections.Generic;
using Change_Point.Models.DTOs;

namespace Change_Point.Repositories.Interfaces
{
    public interface IFileRepository
    {
        int    Add(string name, string fileType, string fileSize, int groupId);
        void   Delete(int id);
        void   LinkToCalendar(int[] fileIds, int calendarId);
        IEnumerable<FileDto> GetByCalendarId(int calendarId);
        IEnumerable<FileDto> GetByIds(int[] ids);
        IEnumerable<FileDto> GetByCalendarIdRaw(int calendarId); // returns name for disk delete
    }
}
