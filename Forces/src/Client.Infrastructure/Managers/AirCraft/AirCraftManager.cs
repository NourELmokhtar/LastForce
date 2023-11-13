using Forces.Application.Features.AirCraft.Commands.AddEdit;
using Forces.Application.Features.AirCraft.Queries.GetAll;
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

namespace Forces.Client.Infrastructure.Managers.AirCraft
{
    public class AirCraftManager : IAirCraftManager
    {
        private protected readonly HttpClient _httpClient;

        public AirCraftManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var Response = await _httpClient.DeleteAsync(AirCraftEndpoints.Delete(id));
            return await Response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllAirCraftResponse>>> GetAllAsync()
        {
            var Response = await _httpClient.GetAsync(AirCraftEndpoints.GetAll);
            return await Response.ToResult<List<GetAllAirCraftResponse>>();
        }

        public async Task<IResult<List<GetAllAirCraftResponse>>> GetAllByKindIdAsync(int KindId)
        {
            var Response = await _httpClient.GetAsync(AirCraftEndpoints.GetByKindId(KindId));
            return await Response.ToResult<List<GetAllAirCraftResponse>>();
        }

        public async Task<IResult<GetAllAirCraftResponse>> GetByIdAsync(int id)
        {
            var Response = await _httpClient.GetAsync(AirCraftEndpoints.GetById(id));
            return await Response.ToResult<GetAllAirCraftResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditAirCraftCommand command)
        {
            var Response = await _httpClient.PostAsJsonAsync(AirCraftEndpoints.Save, command);
            return await Response.ToResult<int>();
        }
    }
}
