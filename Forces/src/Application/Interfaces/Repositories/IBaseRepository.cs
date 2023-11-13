using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Repositories
{
    public interface IBaseRepository
    {
        Task<bool> IsBaseInused(int BaseId);
        Task<bool> IsNameExist(string BaseName, int ForceId);
        Task<bool> IsCodeExist(string BaseCode);
    }
}
