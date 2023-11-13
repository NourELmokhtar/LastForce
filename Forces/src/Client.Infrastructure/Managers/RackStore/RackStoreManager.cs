using Forces.Application.Features.RackStore.Commands.AddEdit;
using Forces.Application.Features.RackStore.Queries.GetAll;
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

namespace Forces.Client.Infrastructure.Managers.RackStore
{
    public class RackStoreManager : IRackStoreManager
    {
        private protected readonly HttpClient _httpClient;

        public RackStoreManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int Id)
        {
            var Response = await _httpClient.DeleteAsync(RackStoreEndpoints.Delete(Id));
            return await Response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllRackStoreResponse>>> GetAllAsync()
        {
            var Response = await _httpClient.GetAsync(RackStoreEndpoints.GetAll);
            return await Response.ToResult<List<GetAllRackStoreResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditRackStoreCommand command)
        {
            var Response = await _httpClient.PostAsJsonAsync(RackStoreEndpoints.Save, command);
            return await Response.ToResult<int>();
        }
    }
}
