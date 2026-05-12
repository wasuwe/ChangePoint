using System;

namespace Change_Point.Models.Domain
{
    public class CpFile
    {
        public int      Id          { get; set; }
        public int      CalendarId  { get; set; }
        public int?     GroupId     { get; set; }
        public string   Name        { get; set; }
        public string   FileType    { get; set; }
        public string   FileSize    { get; set; }
        public DateTime CreateDate  { get; set; }
    }
}
