using System.Collections.Generic;

namespace Change_Point.Models.DTOs
{
    public class ApproveDto
    {
        public string ITEM_ID     { get; set; }
        public string ITEM_TITLE  { get; set; }
        public string ITEM_MEMBER { get; set; }
        public string IS_USE      { get; set; }
        public string GNAME_ENG   { get; set; }
        public string FNAME_ENG   { get; set; }
        public string UPDATE_BY   { get; set; }
        public string UPDATE_DATE { get; set; }
        public List<ConfirmListDto> CONFIRM_LIST { get; set; }
    }

    public class ApproveMemberOptionDto
    {
        public string ITEM_ID     { get; set; }
        public string ITEM_TITLE  { get; set; }
        public string ITEM_MEMBER { get; set; }
    }

    public class ConfirmListDto
    {
        public string ID                { get; set; }
        public string CALENDAR_ID       { get; set; }
        public string TYPE              { get; set; }
        public string SHIFT             { get; set; }
        public string RESULT            { get; set; }
        public string GNAME_ENG         { get; set; }
        public string FNAME_ENG         { get; set; }
        public string CREATE_DATE       { get; set; }
        public string CHANGE_POINT_DATE { get; set; }
        public string GROUP_NAME        { get; set; }
        public string CONFIRM_DATE      { get; set; }
    }

    public class RecipientListInput
    {
        public string ITEM_ID     { get; set; }
        public string ITEM_TITLE  { get; set; }
        public string ITEM_MEMBER { get; set; }
    }

    public class WaitingConfirmDto
    {
        public string ID                { get; set; }
        public string CALENDAR_ID       { get; set; }
        public string TYPE              { get; set; }
        public string SHIFT             { get; set; }
        public string RESULT            { get; set; }
        public string GNAME_ENG         { get; set; }
        public string FNAME_ENG         { get; set; }
        public string CREATE_DATE       { get; set; }
        public string CHANGE_POINT_DATE { get; set; }
        public string GROUP_NAME        { get; set; }
        public string CONFIRM_DATE      { get; set; }
    }

    public class CheckSheetDto
    {
        public string ITEM_ID     { get; set; }
        public string ITEM_NAME   { get; set; }
        public string IS_USE      { get; set; }
        public string GNAME_ENG   { get; set; }
        public string FNAME_ENG   { get; set; }
        public string UPDATE_BY   { get; set; }
        public string UPDATE_DATE { get; set; }
    }
}
