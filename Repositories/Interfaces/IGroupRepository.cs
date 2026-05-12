using System.Collections.Generic;
using Change_Point.Models.DTOs;

namespace Change_Point.Repositories.Interfaces
{
    public interface IGroupRepository
    {
        IEnumerable<GroupDto>       ListAll();
        GroupDto                    GetById(int id);
        IEnumerable<GroupOptionDto> GetByWc(string wcCode, string role);
        int    Add(string empNo);
        void   Edit(int id, string name, string wcList, bool isUse,
                    string menu1, string menu2, string menu3, string menu4, string menu5,
                    string empNo);
        void   Delete(int id);
        void   DeleteOldHidden();
        void   SetColors(int id, string c1, string c2, string c3, string c4, string c5);
        ColorsDto GetColors(int id);
    }
}
