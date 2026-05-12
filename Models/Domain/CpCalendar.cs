using System;

namespace Change_Point.Models.Domain
{
    public class CpCalendar
    {
        public int      Id                 { get; set; }
        public int      GroupId            { get; set; }
        public DateTime? DateChange        { get; set; }
        public string   Shift              { get; set; }
        public string   ShiftTeam          { get; set; }
        public string   ManSpot            { get; set; }
        public string   ManInstead         { get; set; }
        public string   ManSpotNameEng     { get; set; }
        public string   ManSpotNameTha     { get; set; }
        public string   ManInsteadNameEng  { get; set; }
        public string   ManInsteadNameTha  { get; set; }
        public string   McNo               { get; set; }
        public string   EditPoint          { get; set; }
        public string   PartNo             { get; set; }
        public string   MoldNo             { get; set; }
        public string   Change             { get; set; }
        public string   ProcessPoint       { get; set; }
        public int[]    Details            { get; set; }
        public string[] Action             { get; set; }
        public string[] Warnings           { get; set; }
        public string   StatusType         { get; set; }
        public string   Remark             { get; set; }
        public bool     Informed           { get; set; }
        public int[]    Recipient          { get; set; }
        public string   Type               { get; set; }
        public string   CreateBy           { get; set; }
        public DateTime CreateDate         { get; set; }
        public string   UpdateBy           { get; set; }
        public DateTime UpdateDate         { get; set; }
    }
}
