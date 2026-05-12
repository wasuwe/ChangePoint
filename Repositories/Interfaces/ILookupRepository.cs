using System.Collections.Generic;
using Change_Point.Models.DTOs;

namespace Change_Point.Repositories.Interfaces
{
    public interface ILookupRepository
    {
        // ---- Employees ----
        IEnumerable<EmployeeDto> GetEmployees();

        // ---- Generic lookup tables (MoldNo, Process, Edit, Change, Machine, Part) ----
        IEnumerable<LookupItemDto>   ListLookup(string table, int groupId);
        IEnumerable<OptionDto>       OptionLookup(string table, int groupId);
        LookupItemDto                SelectLookup(string table, int id);
        int                          AddLookup(string table, string name, int groupId, bool isUse, string empNo);
        void                         EditLookup(string table, int id, string name, bool isUse, string empNo);
        void                         DeleteLookup(string table, int id);

        // ---- Machine (has extra sizeton field) ----
        IEnumerable<MachineListDto>  ListMachines(int groupId);
        IEnumerable<MachineOptionDto> OptionMachines(int groupId);
        MachineListDto               SelectMachine(int id);
        int                          AddMachine(string machineNo, string sizeTon, int groupId, bool isUse, string empNo);
        void                         EditMachine(int id, string machineNo, string sizeTon, bool isUse, string empNo);
        void                         DeleteMachine(int id);

        // ---- Part ----
        IEnumerable<LookupItemDto>   ListParts(int groupId);
        IEnumerable<PartOptionDto>   OptionParts(int groupId);
        LookupItemDto                SelectPart(string partNo);
        bool                         AddPart(string partNo, string partName, int groupId, bool isUse, string empNo);
        void                         EditPart(string partNo, string partName, bool isUse, string empNo);
        void                         DeletePart(string partNo, int groupId);

        // ---- Details & Categories ----
        IEnumerable<DetailsListDto>         ListDetails(int groupId, string filterType, string filterCategory);
        IEnumerable<DetailsOptionDto>       OptionDetails(int groupId, string filterType);
        IEnumerable<LookupItemDto>          ListDetailCategories(int groupId, string filterType);
        IEnumerable<DetailCategoryOptionDto> OptionDetailCategories(int groupId);
        void AddDetail(string detail, string risk, int categoryId, int groupId, bool isUse, string empNo);
        void EditDetail(int id, string detail, string risk, int categoryId, bool isUse, string empNo);
        void DeleteDetail(int id);
        void AddDetailCategory(string name, string type, int groupId, bool isUse, string empNo);
        void EditDetailCategory(int id, string name, string type, bool isUse, string empNo);
        void DeleteDetailCategory(int id);

        // ---- Dashboard ----
        IEnumerable<DashboardSettingDto> ListDashboard(int groupId);
        void SaveDashboard(int groupId, string type, IEnumerable<DashboardSettingDto> items);

        // ---- Report summaries ----
        IEnumerable<SummaryDayDto>   SummaryDay(string dateSelect, int groupId);
        IEnumerable<SummaryMonthDto> SummaryMonth(string dateStart, string dateStop, int groupId, string toCharFormat);
        IEnumerable<CalendarDto>     ReportList(int groupId, string dateStart, string dateStop,
                                                string item5m, string itemMan, string itemMc,
                                                string itemMold, string itemProcess, string itemChange, string itemEdit);
    }
}
