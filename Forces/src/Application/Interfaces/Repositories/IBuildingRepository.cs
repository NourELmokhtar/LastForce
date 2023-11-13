using Forces.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Repositories
{
    public interface IBuildingRepository
    {
        Task<List<Building>> GetAll();
        Task<Building> GetById(int Id);
        Task<Building> Add(Building Building);
        Task<Building> Update(Building Building);
        Task<Building> Delete(Building Building);
    }
}
