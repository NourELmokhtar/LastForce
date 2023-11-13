using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Repositories
{
    public interface IForceRepository
    {
        Task<bool> IsForceInused(int ForceId);
    }
}
