using System;

namespace Change_Point.Models.Domain
{
    /// <summary>
    /// Shared domain model for simple lookup tables:
    /// CpEdit, CpChange, CpMoldNo, CpProcess
    /// </summary>
    public class CpLookupItem
    {
        public int      Id          { get; set; }
        public int      GroupId     { get; set; }
        public string   Name        { get; set; }
        public bool     IsUse       { get; set; }
        public string   CreateBy    { get; set; }
        public DateTime CreateDate  { get; set; }
        public string   UpdateBy    { get; set; }
        public DateTime UpdateDate  { get; set; }
    }

    public class CpEdit    : CpLookupItem { }
    public class CpChange  : CpLookupItem { }
    public class CpMoldNo  : CpLookupItem { }
    public class CpProcess : CpLookupItem { }
}
