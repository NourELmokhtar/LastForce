using Forces.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Repositories
{
    public interface IHouseRepository
    {
        Task<List<House>> GetAll();
        Task<House> GetById(int Id);
        Task<House> Add(House House);
        Task<House> Update(House House);
        Task<House> Delete(House House);
    }
}
