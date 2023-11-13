using Forces.Application.Interfaces.Repositories;
using Forces.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Repositories
{
    public class HouseRepository : IHouseRepository
    {
        private readonly IRepositoryAsync<House, int> _repository;

        public HouseRepository(IRepositoryAsync<House, int> repository)
        {
            _repository = repository;
        }

        public async Task<House> Add(House House)
        {
            return await _repository.AddAsync(House);
        }

        public async Task<House> Delete(House House)
        {
            await _repository.DeleteAsync(House);

            return House;
        }

        public Task<List<House>> GetAll()
        {
            return _repository.GetAllAsync();
        }

        public Task<House> GetById(int Id)
        {
            return _repository.GetByIdAsync(Id);
        }

        public async Task<House> Update(House House)
        {
            await _repository.UpdateAsync(House);

            return House;
        }
    }
}
