using Forces.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Repositories
{
    public interface IInventoryRepository
    {
        Task<List<Inventory>> GetAll();
        Task<Inventory> GetById(int Id);
        Task<Inventory> Add(Inventory Inventory);
        Task<Inventory> Update(Inventory Inventory);
        Task<Inventory> Delete(Inventory Inventory);
    }
}
