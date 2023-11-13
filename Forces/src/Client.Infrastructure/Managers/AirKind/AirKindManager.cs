using Forces.Application.Features.AirKind.Commands.AddEdit;
using Forces.Application.Features.AirKind.Queries.GetAll;
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

namespace Forces.Client.Infrastructure.Managers.AirKind
{
    public class AirKindManager : IAirKindManager
    {
        private protected readonly HttpClient _httpClient;

        public AirKindManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var Response = await _httpClient.DeleteAsync(AirKindEndpoints.Delete(id));
            return await Response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllAirKindResponse>>> GetAllAsync()
        {
            var Response = await _httpClient.GetAsync(AirKindEndpoints.GetAll);
            return await Response.ToResult<List<GetAllAirKindResponse>>();
        }

        public async Task<IResult<List<GetAllAirKindResponse>>> GetAllByTypeIdAsync(int id)
        {
            var Response = await _httpClient.GetAsync(AirKindEndpoints.GetByTypeId(id));
            return await Response.ToResult<List<GetAllAirKindResponse>>();
        }

        public async Task<IResult<GetAllAirKindResponse>> GetByIdAsync(int id)
        {
            var Response = await _httpClient.GetAsync(AirKindEndpoints.GetById(id));
            return await Response.ToResult<GetAllAirKindResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditAirKindCommand command)
        {
            var Response = await _httpClient.PostAsJsonAsync(AirKindEndpoints.Save, command);
            return await Response.ToResult<int>();
        }
    }
}
