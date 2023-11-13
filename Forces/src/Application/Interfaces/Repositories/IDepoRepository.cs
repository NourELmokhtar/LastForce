using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Repositories
{
    public interface IDepoRepository
    {
        Task<bool> IsDepoInuse(int DepoId);
        Task<bool> IsNameExist(string DepartmentName);
    }
}
