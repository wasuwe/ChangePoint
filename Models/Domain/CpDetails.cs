using System;

namespace Change_Point.Models.Domain
{
    public class CpDetails
    {
        public int      Id          { get; set; }
        public int      GroupId     { get; set; }
        public int?     CategoryId  { get; set; }
        public string   Detail      { get; set; }
        public string   Risk        { get; set; }
        public bool     IsUse       { get; set; }
        public string   CreateBy    { get; set; }
        public DateTime CreateDate  { get; set; }
        public string   UpdateBy    { get; set; }
        public DateTime UpdateDate  { get; set; }
    }
}
