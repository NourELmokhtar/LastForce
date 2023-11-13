using Forces.Application.Features.Room.Commands.AddEdit;
using Forces.Application.Features.Room.Queries.GetAll;
using Forces.Application.Features.Room.Queries.GetBySpecifics;
using Forces.Client.Infrastructure.Extensions;
using Forces.Client.Infrastructure.Routes;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Room
{
    public class RoomManager :IRoomManager
    {
        private protected readonly HttpClient _httpClient;

        public RoomManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int Id)
        {
            var Response = await _httpClient.DeleteAsync(RoomEndPoints.Delete(Id));
            return await Response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllRoomsResponse>>> GetAllAsync()
        {
            var Response = await _httpClient.GetAsync(RoomEndPoints.GetAll);
            return await Response.ToResult<List<GetAllRoomsResponse>>();
        }

        public async Task<IResult<GetRoomByResponse>> GetRoomByIdAsync(int Id)
        {
            var Response = await _httpClient.GetAsync(RoomEndPoints.GetRoomById(Id));
            return await Response.ToResult<GetRoomByResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditRoomCommand command)
        {
            var Response = await _httpClient.PostAsJsonAsync(RoomEndPoints.Save, command);
            return await Response.ToResult<int>();
        }
    }
}
