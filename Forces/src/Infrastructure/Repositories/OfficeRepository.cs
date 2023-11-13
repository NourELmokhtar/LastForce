using Forces.Application.Interfaces.Repositories;
using Forces.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Repositories
{
    public class OfficeRepository : IOfficeRepository
    {
        private readonly IRepositoryAsync<Office, int> _repository;

        public OfficeRepository(IRepositoryAsync<Office, int> repository)
        {
            _repository = repository;
        }

        public async Task<Office> Add(Office Office)
        {
            return await _repository.AddAsync(Office);
        }

        public async Task<Office> Delete(Office Office)
        {
            await _repository.DeleteAsync(Office);

            return Office;
        }

        public Task<List<Office>> GetAll()
        {
            return _repository.GetAllAsync();
        }

        public Task<Office> GetById(int Id)
        {
            return _repository.GetByIdAsync(Id);
        }

        public async Task<Office> Update(Office Office)
        {
            await _repository.UpdateAsync(Office);

            return Office;
        }
    }
}
