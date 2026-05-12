using System.Collections.Generic;
using Change_Point.Models.DTOs;

namespace Change_Point.Repositories.Interfaces
{
    public interface ICalendarRepository
    {
        IEnumerable<CalendarDto> ListAll(int groupId);
        IEnumerable<CalendarDto> ListMy(int groupId, string empNo);
        CalendarDto              GetItem(int id);
        IEnumerable<CalendarDto> ListByDay(string dateSelect, int groupId);
        IEnumerable<CalendarDto> ListOver();
        int    Add(AddCalendarParams p);
        void   Edit(EditCalendarParams p);
        void   SoftDelete(int id, string empNo);
        void   SetInformed(int id, string informedValue, string empNo);
        void   DeleteOver(int id);
    }

    public class AddCalendarParams
    {
        public System.DateTime DateChange          { get; set; }
        public string Spot                         { get; set; }
        public string Instead                      { get; set; }
        public string McNo                         { get; set; }
        public string Edit                         { get; set; }
        public string PartNo                       { get; set; }
        public string MoldNo                       { get; set; }
        public string Change                       { get; set; }
        public string Process                      { get; set; }
        public int[]  DetailIds                    { get; set; }
        public string[] Actions                    { get; set; }
        public string[] Warnings                   { get; set; }
        public string Type                         { get; set; }
        public string SpotEng                      { get; set; }
        public string SpotTha                      { get; set; }
        public string InsteadEng                   { get; set; }
        public string InsteadTha                   { get; set; }
        public string Shift                        { get; set; }
        public string Team                         { get; set; }
        public string Remark                       { get; set; }
        public int[]  RecipientIds                 { get; set; }
        public int    GroupId                      { get; set; }
        public string EmpNo                        { get; set; }
    }

    public class EditCalendarParams : AddCalendarParams
    {
        public int Id { get; set; }
    }
}
