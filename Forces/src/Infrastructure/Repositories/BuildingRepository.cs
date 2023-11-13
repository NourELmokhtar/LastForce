using Forces.Application.Interfaces.Repositories;
using Forces.Application.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Repositories
{
    public class BuildingRepository : IBuildingRepository
    {
        private readonly IRepositoryAsync<Building, int> _repository;

        public BuildingRepository(IRepositoryAsync<Building, int> repository)
        {
            _repository = repository;
        }

        public async Task<Building> Add(Building Building)
        {
            return await _repository.AddAsync(Building);
        }

        public async Task<Building> Delete(Building Building)
        {
            await _repository.DeleteAsync(Building);

            return Building;
        }

        public Task<List<Building>> GetAll()
        {
            return _repository.GetAllAsync();
        }

        public Task<Building> GetById(int Id)
        {
            return _repository.GetByIdAsync(Id);
        }

        public async Task<Building> Update(Building Building)
        {
            await _repository.UpdateAsync(Building);

            return Building;
        }
    }
}
