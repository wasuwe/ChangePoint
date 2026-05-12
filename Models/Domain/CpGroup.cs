using System;

namespace Change_Point.Models.Domain
{
    public class CpGroup
    {
        public int      Id          { get; set; }
        public string   Name        { get; set; }
        public string   WcList      { get; set; }
        public bool     IsUse       { get; set; }
        public string   Menu1       { get; set; }
        public string   Menu2       { get; set; }
        public string   Menu3       { get; set; }
        public string   Menu4       { get; set; }
        public string   Menu5       { get; set; }
        public string   Color1      { get; set; }
        public string   Color2      { get; set; }
        public string   Color3      { get; set; }
        public string   Color4      { get; set; }
        public string   Color5      { get; set; }
        public string   CreateBy    { get; set; }
        public DateTime CreateDate  { get; set; }
        public string   UpdateBy    { get; set; }
        public DateTime UpdateDate  { get; set; }
    }
}
