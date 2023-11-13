using Forces.Application.Features.House.Commands.AddEdit;
using Forces.Application.Features.House.Queries.GetAll;
using Forces.Application.Features.House.Queries.GetBySpecifics;
using Forces.Client.Infrastructure.Extensions;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.House
{
    public class HouseManager : IHouseManager
    {
        private protected readonly HttpClient _httpClient;

        public HouseManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(Routes.HouseEndPoints.Delete(id));
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllHousesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.HouseEndPoints.GetAll);
            return await response.ToResult<List<GetAllHousesResponse>>();
        }

        public async Task<IResult<GetHouseByResponse>> GetHouseByAsync(int Id)
        {
            var response = await _httpClient.GetAsync(Routes.HouseEndPoints.GetHouseById(Id));
            return await response.ToResult<GetHouseByResponse>();
        }



        public async Task<IResult<int>> SaveAsync(AddEditHouseCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.HouseEndPoints.Save, request);
            return await response.ToResult<int>();
        }

    }
}
