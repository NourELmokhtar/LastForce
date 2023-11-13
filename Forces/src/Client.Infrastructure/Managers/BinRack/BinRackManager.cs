using Forces.Application.Features.BinRack.Commands.AddEdit;
using Forces.Application.Features.BinRack.Queries.GetAll;
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

namespace Forces.Client.Infrastructure.Managers.BinRack
{
    public class BinRackManager : IBinRackManager
    {
        private protected readonly HttpClient _httpClient;

        public BinRackManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IResult<int>> DeleteAsync(int Id)
        {
            var Response = await _httpClient.DeleteAsync(BinRackEndpoints.Delete(Id));
            return await Response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllBinRackResponse>>> GetAllAsync()
        {

            var Response = await _httpClient.GetAsync(BinRackEndpoints.GetAll);
            return await Response.ToResult<List<GetAllBinRackResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditBinRackCommand command)
        {
            var Response = await _httpClient.PostAsJsonAsync(BinRackEndpoints.Save, command);
            return await Response.ToResult<int>();
        }
    }
}
