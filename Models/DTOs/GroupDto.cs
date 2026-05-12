namespace Change_Point.Models.DTOs
{
    public class GroupDto
    {
        public string ITEM_ID     { get; set; }
        public string ITEM_NAME   { get; set; }
        public string ITEM_WC     { get; set; }
        public string IS_USE      { get; set; }
        public string MENU_1      { get; set; }
        public string MENU_2      { get; set; }
        public string MENU_3      { get; set; }
        public string MENU_4      { get; set; }
        public string MENU_5      { get; set; }
        public string GNAME_ENG   { get; set; }
        public string FNAME_ENG   { get; set; }
        public string UPDATE_BY   { get; set; }
        public string UPDATE_DATE { get; set; }
        public string TOOL        { get; set; }
    }

    public class GroupOptionDto
    {
        public string WC_CODE    { get; set; }
        public string WC_NAME    { get; set; }
        public string GROUP_ID   { get; set; }
        public string GROUP_NAME { get; set; }
        public string MENU_1     { get; set; }
        public string MENU_2     { get; set; }
        public string MENU_3     { get; set; }
        public string MENU_4     { get; set; }
        public string MENU_5     { get; set; }
    }

    public class ColorsDto
    {
        public string ITEM_COLORS_1 { get; set; }
        public string ITEM_COLORS_2 { get; set; }
        public string ITEM_COLORS_3 { get; set; }
        public string ITEM_COLORS_4 { get; set; }
        public string ITEM_COLORS_5 { get; set; }
    }
}
