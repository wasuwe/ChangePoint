namespace Change_Point.Models.Domain
{
    public class CpApproveMemberItem
    {
        public int    Id               { get; set; }
        public int    ApproveMemberId  { get; set; }
        public string EmpNo            { get; set; }
        public int?   GroupId          { get; set; }
    }
}
