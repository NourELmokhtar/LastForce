using Forces.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Repositories
{
    public interface IOfficeRepository
    {
        Task<List<Office>> GetAll();
        Task<Office> GetById(int Id);
        Task<Office> Add(Office Office);
        Task<Office> Update(Office Office);
        Task<Office> Delete(Office Office);
    }
}
