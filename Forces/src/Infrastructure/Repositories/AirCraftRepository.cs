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
    public class AirCraftRepository : IAirCraftRepository
    {
        private protected readonly IRepositoryAsync<AirCraft,int> _repository;

        public AirCraftRepository(IRepositoryAsync<AirCraft, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsAirCraftInused(int AirKindId)
        {
           return await _repository.Entities.AnyAsync(x=>x.AirKindId == AirKindId);
        }

        public async Task<bool> IsCodeExist(int airCraftCode)
        {
           return await _repository.Entities.AnyAsync(x=>x.AirCraftCode == airCraftCode);
        }

        public async Task<bool> IsNameExist(string AirCraftName, int AirKindId)
        {
            return await _repository.Entities.AnyAsync(x=>x.AirCraftName == AirCraftName && x.AirKindId == AirKindId);
        }
    }
}
