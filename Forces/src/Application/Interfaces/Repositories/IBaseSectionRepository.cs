using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Repositories
{
    public interface IBaseSectionRepository
    {
        Task<bool> IsBaseSectionInused(int BaseId);
        Task<bool> IsNameExist(string BaseSectionName, int BaseId);
        Task<bool> IsCodeExist(string BaseSectionCode);
    }
}
