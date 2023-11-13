using Forces.Application.Features.Inventory.Commands.AddEdit;
using Forces.Application.Features.Inventory.Queries.GetAll;
using Forces.Application.Features.Inventory.Queries.GetBySpecifics;
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

namespace Forces.Client.Infrastructure.Managers.Inventory
{
    public class InventoryManager : IInventoryManager
    {
        private protected readonly HttpClient _httpClient;

        public InventoryManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int Id)
        {
            var Response = await _httpClient.DeleteAsync(InventoryEndPoints.Delete(Id));
            return await Response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllInventoriesResponse>>> GetAllAsync()
        {
            var Response = await _httpClient.GetAsync(InventoryEndPoints.GetAll);
            return await Response.ToResult<List<GetAllInventoriesResponse>>();
        }

        public async Task<IResult<GetInventoryByResponse>> GetInventoryByIdAsync(int Id)
        {
            var Response = await _httpClient.GetAsync(InventoryEndPoints.GetInventoryById(Id));
            return await Response.ToResult<GetInventoryByResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditInventoryCommand command)
        {
            var Response = await _httpClient.PostAsJsonAsync(InventoryEndPoints.Save, command);
            return await Response.ToResult<int>();
        }
    }
}
