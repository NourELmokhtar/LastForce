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
    public class AirKindRepository : IAirKindRepository
    {
        private protected readonly IRepositoryAsync<AirKind, int> _repository;

        public AirKindRepository(IRepositoryAsync<AirKind, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsAirKindInused(int Id)
        {
            var AirKind = await _repository.GetByIdAsync(Id);

            return AirKind.AirCrafts.Count >0;
        }

        public async Task<bool> IsCodeExist(string AirKindCode, int? Id = null)
        {
            if (Id.HasValue)
            {
                return await _repository.Entities.AnyAsync(x => x.AirKindCode == AirKindCode && x.Id != Id);
            }
            return await _repository.Entities.AnyAsync(x => x.AirKindCode == AirKindCode);

        }

        public async Task<bool> IsNameExist(string AirKindName, int? Id = null)
        {
            if (Id.HasValue)
            {
                return await _repository.Entities.AnyAsync(x => x.AirKindName == AirKindName && x.Id != Id);

            }
            return await _repository.Entities.AnyAsync(x => x.AirKindName == AirKindName);

        }
    }
}
