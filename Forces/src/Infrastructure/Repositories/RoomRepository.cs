using Forces.Application.Interfaces.Repositories;
using Forces.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly IRepositoryAsync<Room, int> _repository;

        public RoomRepository(IRepositoryAsync<Room, int> repository)
        {
            _repository = repository;
        }

        public async Task<Room> Add(Room Room)
        {
            return await _repository.AddAsync(Room);
        }

        public async Task<Room> Delete(Room Room)
        {
            await _repository.DeleteAsync(Room);

            return Room;
        }

        public Task<List<Room>> GetAll()
        {
            return _repository.GetAllAsync();
        }

        public Task<Room> GetById(int Id)
        {
            return _repository.GetByIdAsync(Id);
        }

        public async Task<Room> Update(Room Room)
        {
            await _repository.UpdateAsync(Room);

            return Room;
        }
    }
}
