using Forces.Application.Features.AirType.Commands.AddEdit;
using Forces.Application.Features.AirType.Queries.GetAll;
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

namespace Forces.Client.Infrastructure.Managers.AirType
{
    public class AirTypeManager : IAirTypeManager
    {
        private protected readonly HttpClient _httpClient;

        public AirTypeManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var Response = await _httpClient.DeleteAsync(AirTypeEndpoints.Delete(id));
            return await Response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllAirTypeResponse>>> GetAllAsync()
        {
            var Response = await _httpClient.GetAsync(AirTypeEndpoints.GetAll);
            return await Response.ToResult<List<GetAllAirTypeResponse>>();
        }

        public async Task<IResult<GetAllAirTypeResponse>> GetByIdAsync(int id)
        {
            var Response = await _httpClient.GetAsync(AirTypeEndpoints.GetById(id));
            return await Response.ToResult<GetAllAirTypeResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditAirTypeCommand command)
        {
            var Response = await _httpClient.PostAsJsonAsync(AirTypeEndpoints.Save, command);
            return await Response.ToResult<int>();
        }
    }
}
