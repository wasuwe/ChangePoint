using System.Collections.Generic;
using Change_Point.Models.DTOs;

namespace Change_Point.Repositories.Interfaces
{
    public interface IApproveRepository
    {
        // Approve Member CRUD
        IEnumerable<ApproveDto>           ListApproveMembers(int groupId);
        IEnumerable<ApproveMemberOptionDto> OptionApproveMembers(int groupId);
        ApproveDto                        GetApproveMember(int id);
        ApproveDto                        GetApproveMemberWithConfirms(int id, int calendarId);
        int  AddApproveMember(string title, string memberList, int groupId, bool isUse, string empNo);
        void EditApproveMember(int id, string title, string memberList, bool isUse, string empNo);
        void DeleteApproveMember(int id);

        // Calendar Confirm
        void AddCalendarConfirm(int calendarId, int groupMemberId, string empNo);
        void DeleteCalendarConfirms(int calendarId);
        void ConfirmChangePoint(int confirmId);

        // Waiting list
        IEnumerable<WaitingConfirmDto> ListWaiting(string empNo, string dateStart, string dateStop,
                                                    string filterType, string filterShift, string filterStatus);
        int CountWaiting(string empNo);

        // Check Sheet CRUD
        IEnumerable<CheckSheetDto> ListCheckSheets(int groupId);
        CheckSheetDto              GetCheckSheet(int id);
        int  AddCheckSheet(string name, int groupId, bool isUse, string empNo, string[] items);
        void EditCheckSheet(int id, string name, bool isUse, string empNo, string[] items);
        void DeleteCheckSheet(int id);
    }
}
