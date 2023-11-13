using Forces.Application.Features.Inventory.Commands.AddEdit;
using Forces.Application.Features.Inventory.Queries.GetAll;
using Forces.Application.Features.Inventory.Queries.GetBySpecifics;
using Forces.Application.Features.Room.Commands.AddEdit;
using Forces.Application.Features.Room.Queries.GetAll;
using Forces.Application.Features.Room.Queries.GetBySpecifics;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Room
{
    public interface IRoomManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditRoomCommand request);
        Task<IResult<List<GetAllRoomsResponse>>> GetAllAsync();
        Task<IResult<GetRoomByResponse>> GetRoomByIdAsync(int Id);
        Task<IResult<int>> DeleteAsync(int id);
    }
}
