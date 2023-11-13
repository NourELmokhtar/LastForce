using Forces.Application.Interfaces.Repositories;
using Forces.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Repositories
{
    public class BaseSectionRepository : IBaseSectionRepository
    {
        private readonly IRepositoryAsync<BasesSections, int> _Sectionsrepository;
        private readonly IRepositoryAsync<Bases, int> _Baserepository;

        public BaseSectionRepository(IRepositoryAsync<BasesSections, int> sectionsrepository, IRepositoryAsync<Bases, int> baserepository)
        {
            _Sectionsrepository = sectionsrepository;
            _Baserepository = baserepository;
        }

        public Task<bool> IsBaseSectionInused(int BaseId)
        {
            return Task<bool>.FromResult(false);
        }

        public Task<bool> IsCodeExist(string BaseSectionCode)
        {
            return Task<bool>.FromResult(_Sectionsrepository.Entities.Any(s => s.SectionCode == BaseSectionCode));
        }

        public Task<bool> IsNameExist(string BaseSectionName, int BaseId)
        {
            return Task<bool>.FromResult(_Sectionsrepository.Entities.Any(s => s.SectionName == BaseSectionName && s.BaseId == BaseId));
        }
    }
}
