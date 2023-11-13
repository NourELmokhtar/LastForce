using Forces.Application.Interfaces.Repositories;
using Forces.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Repositories
{
    public class HQRepository : IHQRepository
    {
        private readonly IRepositoryAsync<HQDepartment, int> _HQRepository;

        public HQRepository(IRepositoryAsync<HQDepartment, int> hQRepository)
        {
            _HQRepository = hQRepository;
        }

        public async Task<bool> IsHQInuse(int HQId)
        {
            return await Task<bool>.FromResult(false);
        }

        public async Task<bool> IsNameExist(string DepartmentName)
        {
            return await Task<bool>.FromResult(_HQRepository.Entities.Any(h => h.Name == DepartmentName));
        }
    }
}
