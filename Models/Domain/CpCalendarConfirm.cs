using System;

namespace Change_Point.Models.Domain
{
    public class CpCalendarConfirm
    {
        public int       Id             { get; set; }
        public int       CalendarId     { get; set; }
        public int?      GroupMemberId  { get; set; }
        public string    EmpNo          { get; set; }
        public string    IsConfirm      { get; set; }
        public DateTime? ConfirmDate    { get; set; }
        public DateTime  CreateDate     { get; set; }
    }
}
