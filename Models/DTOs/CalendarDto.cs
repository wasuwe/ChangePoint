using System.Collections.Generic;

namespace Change_Point.Models.DTOs
{
    /// <summary>Full calendar record returned to the Vue frontend.</summary>
    public class CalendarDto
    {
        public string ID                   { get; set; }
        public string SHIFT                { get; set; }
        public string SHIFT_TEAM           { get; set; }
        public string DATE_CHANGE          { get; set; }
        public string MAN_SPOT             { get; set; }
        public string MAN_INSTEAD          { get; set; }
        public string MC_NO                { get; set; }
        public string EDIT_POINT           { get; set; }
        public string PART_NO              { get; set; }
        public string MOLD_NO              { get; set; }
        public string CHANGE               { get; set; }
        public string PROCESS_POINT        { get; set; }
        public string DETAILS              { get; set; }   // joined string for display
        public string ACTION               { get; set; }   // joined string for display
        public string WARNINGS             { get; set; }   // joined string for display
        public string STATUS_TYPE          { get; set; }
        public string MAN_SPOT_NAME_ENG    { get; set; }
        public string MAN_SPOT_NAME_THA    { get; set; }
        public string MAN_INSTEAD_NAME_ENG { get; set; }
        public string MAN_INSTEAD_NAME_THA { get; set; }
        public string CREATE_BY            { get; set; }
        public string CREATE_DATE          { get; set; }
        public string UPDATE_BY            { get; set; }
        public string UPDATE_DATE          { get; set; }
        public string GNAME_ENG_CREATE     { get; set; }
        public string FNAME_ENG_CREATE     { get; set; }
        public string GNAME_ENG_UPDATE     { get; set; }
        public string FNAME_ENG_UPDATE     { get; set; }
        public string REMARK               { get; set; }
        public string INFORMED             { get; set; }
        public string RECIPIENT_ID         { get; set; }
        public List<ApproveDto> RECIPIENT_LIST { get; set; }
    }
}
