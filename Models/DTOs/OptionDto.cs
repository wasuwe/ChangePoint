namespace Change_Point.Models.DTOs
{
    /// <summary>Generic ID/Name option used for all lookup dropdowns.</summary>
    public class OptionDto
    {
        public string ITEM_ID   { get; set; }
        public string ITEM_NAME { get; set; }
    }

    public class MachineOptionDto
    {
        public string ITEM_ID       { get; set; }
        public string ITEM_NAME     { get; set; }
        public string ITEM_SIZETON  { get; set; }
    }

    public class PartOptionDto
    {
        public string ITEM_ID { get; set; }  // part_no
    }

    public class LookupItemDto
    {
        public string ITEM_ID     { get; set; }
        public string ITEM_NAME   { get; set; }
        public string IS_USE      { get; set; }
        public string GNAME_ENG   { get; set; }
        public string FNAME_ENG   { get; set; }
        public string UPDATE_BY   { get; set; }
        public string UPDATE_DATE { get; set; }
        public string TOOL        { get; set; }
    }

    public class MachineListDto
    {
        public string ITEM_ID       { get; set; }
        public string ITEM_NAME     { get; set; }
        public string ITEM_SIZETON  { get; set; }
        public string IS_USE        { get; set; }
        public string GNAME_ENG     { get; set; }
        public string FNAME_ENG     { get; set; }
        public string UPDATE_BY     { get; set; }
        public string UPDATE_DATE   { get; set; }
        public string TOOL          { get; set; }
    }

    public class DetailCategoryOptionDto
    {
        public string ITEM_ID   { get; set; }
        public string ITEM_NAME { get; set; }
        public string ITEM_TYPE { get; set; }
    }

    public class DetailsOptionDto
    {
        public string ITEM_ID            { get; set; }
        public string ITEM_DETAIL        { get; set; }
        public string ITEM_RISK          { get; set; }
        public string ITEM_CATEGORY_NAME { get; set; }
    }

    public class DetailsListDto
    {
        public string ITEM_ID            { get; set; }
        public string ITEM_DETAIL        { get; set; }
        public string ITEM_RISK          { get; set; }
        public string ITEM_CATEGORY_ID   { get; set; }
        public string ITEM_CATEGORY_NAME { get; set; }
        public string IS_USE             { get; set; }
        public string GNAME_ENG          { get; set; }
        public string FNAME_ENG          { get; set; }
        public string UPDATE_BY          { get; set; }
        public string UPDATE_DATE        { get; set; }
    }

    public class DashboardSettingDto
    {
        public string PRIORITY { get; set; }
        public string TITLE    { get; set; }
        public string TIME     { get; set; }
        public string TYPE     { get; set; }
    }

    public class CountDashboardDto
    {
        public string TITLE        { get; set; }
        public string RESULT_OK    { get; set; }
        public string RESULT_NG    { get; set; }
        public string RESULT_TOTAL { get; set; }
    }

    public class SummaryDayDto
    {
        public string TYPE  { get; set; }
        public string COUNT { get; set; }
    }

    public class SummaryMonthDto
    {
        public string DATE  { get; set; }
        public string TYPE  { get; set; }
        public string COUNT { get; set; }
    }
}
