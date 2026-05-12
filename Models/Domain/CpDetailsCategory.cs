using System;

namespace Change_Point.Models.Domain
{
    public class CpDetailsCategory
    {
        public int      Id          { get; set; }
        public int      GroupId     { get; set; }
        public string   Name        { get; set; }
        public string   Type        { get; set; }
        public bool     IsUse       { get; set; }
        public string   CreateBy    { get; set; }
        public DateTime CreateDate  { get; set; }
        public string   UpdateBy    { get; set; }
        public DateTime UpdateDate  { get; set; }
    }
}
