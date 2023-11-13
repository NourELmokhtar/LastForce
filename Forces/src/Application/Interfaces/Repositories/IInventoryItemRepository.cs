using Forces.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Repositories
{
    public interface IInventoryItemRepository
    {
        Task<List<InventoryItem>> GetAll();
        Task<InventoryItem> GetById(int Id);
        Task<InventoryItem> Add(InventoryItem InventoryItem);
        Task<InventoryItem> Update(InventoryItem InventoryItem);
        Task<InventoryItem> Delete(InventoryItem InventoryItem);
        Task<bool> IsCodeExist(string ItemCode);
        Task<bool> IsNsnExist(string ItemNsn);
        Task<bool> IsNameExist(string ItemName, int MeasureUnitID);
        Task<bool> IsArNameExist(string ItemName, int MeasureUnitID);
        Task<List<InventoryItem>> GetByName(string Name);
        Task<List<InventoryItem>> GetByArName(string ArName);
        Task<InventoryItem> GetByCode(string Code);
        Task<InventoryItem> GetByNSN(string NSNCode);
        Task<List<MeasureUnits>> GetMeasureUnitsByName(string ItemName);
        Task<List<MeasureUnits>> GetMeasureUnitsByArName(string ItemName);
    }
}
