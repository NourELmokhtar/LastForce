using Forces.Application.Interfaces.Repositories;
using Forces.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        private readonly IRepositoryAsync<BasesSections, int> _Sectionsrepository;
        private readonly IRepositoryAsync<Bases, int> _Baserepository;

        public BaseRepository(IRepositoryAsync<BasesSections, int> sectionsrepository, IRepositoryAsync<Bases, int> baserepository)
        {
            _Sectionsrepository = sectionsrepository;
            _Baserepository = baserepository;
        }

        public async Task<bool> IsBaseInused(int BaseId)
        {
            return await Task.FromResult<bool>(_Sectionsrepository.Entities.Any(x => x.BaseId == BaseId));
        }

        public async Task<bool> IsCodeExist(string BaseCode)
        {
            return await Task.FromResult<bool>(_Baserepository.Entities.Any(x => x.BaseCode == BaseCode));
        }

        public async Task<bool> IsNameExist(string BaseName, int ForceId)
        {
            return await Task.FromResult<bool>(_Baserepository.Entities.Any(x => x.BaseName == BaseName && x.ForceId == ForceId));
        }
    }
}
