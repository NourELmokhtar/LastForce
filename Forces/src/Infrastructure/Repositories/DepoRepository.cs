using Forces.Application.Interfaces.Repositories;
using Forces.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Repositories
{
    public class DepoRepository : IDepoRepository
    {
        private readonly IRepositoryAsync<DepoDepartment, int> _DepoRepository;

        public DepoRepository(IRepositoryAsync<DepoDepartment, int> depoRepository)
        {
            _DepoRepository = depoRepository;
        }

        public async Task<bool> IsDepoInuse(int DepoId)
        {
            return await Task<bool>.FromResult(_DepoRepository.Entities.Any());
        }

        public async Task<bool> IsNameExist(string DepartmentName)
        {
            return await Task<bool>.FromResult(_DepoRepository.Entities.Any(d => d.Name == DepartmentName));
        }
    }
}
