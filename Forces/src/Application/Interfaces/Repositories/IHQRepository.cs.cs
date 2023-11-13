using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Repositories
{
    public interface IHQRepository
    {
        Task<bool> IsHQInuse(int HQId);
        Task<bool> IsNameExist(string DepartmentName);
    }
}
