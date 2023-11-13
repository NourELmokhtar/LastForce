using Forces.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Repositories
{
    public interface IItemRepository
    {
        Task<bool> IsCodeExist(string ItemCode);
        Task<bool> IsNsnExist(string ItemNsn);
        Task<bool> IsNameExist(string ItemName, int MeasureUnitID);
        Task<bool> IsArNameExist(string ItemName, int MeasureUnitID);
        Task<List<Items>> GetByName(string Name);
        Task<List<Items>> GetByArName(string ArName);
        Task<Items> GetByCode(string Code);
        Task<Items> GetByNSN(string NSNCode);
        Task<List<MeasureUnits>> GetMeasureUnitsByName(string ItemName);
        Task<List<MeasureUnits>> GetMeasureUnitsByArName(string ItemName);

    }
}
