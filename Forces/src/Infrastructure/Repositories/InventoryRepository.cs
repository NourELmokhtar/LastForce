using Forces.Application.Interfaces.Repositories;
using Forces.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IRepositoryAsync<Inventory, int> _repository;

        public InventoryRepository(IRepositoryAsync<Inventory, int> repository)
        {
            _repository = repository;
        }

        public async Task<Inventory> Add(Inventory Inventory)
        {
            return await _repository.AddAsync(Inventory);
        }

        public async Task<Inventory> Delete(Inventory Inventory)
        {
            await _repository.DeleteAsync(Inventory);

            return Inventory;
        }

        public Task<List<Inventory>> GetAll()
        {
            return _repository.GetAllAsync();
        }

        public Task<Inventory> GetById(int Id)
        {
            return _repository.GetByIdAsync(Id);
        }

        public async Task<Inventory> Update(Inventory Inventory)
        {
            await _repository.UpdateAsync(Inventory);

            return Inventory;
        }
    }
}
