using Forces.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Repositories
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAll();
        Task<Room> GetById(int Id);
        Task<Room> Add(Room Room);
        Task<Room> Update(Room Room);
        Task<Room> Delete(Room Room);
    }
}
