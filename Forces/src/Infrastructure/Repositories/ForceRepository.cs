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
    public class ForceRepository : IForceRepository
    {
        private readonly IRepositoryAsync<Bases, int> _repository;

        public ForceRepository(IRepositoryAsync<Bases, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsForceInused(int ForceId)
        {
            return await _repository.Entities.AnyAsync(b => b.ForceId == ForceId);
        }
    }
}
