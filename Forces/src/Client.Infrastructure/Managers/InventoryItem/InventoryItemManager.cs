using Forces.Application.Features.InventoryInventoryItem.Commands.AddEdit;
using Forces.Application.Features.InventoryItem.Queries.GetAll;
using Forces.Application.Features.InventoryItem.Queries.GetBySpecifics;
using Forces.Client.Infrastructure.Extensions;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.InventoryItem
{
    public class InventoryItemManager : IInventoryItemManager
    {
        private protected readonly HttpClient _httpClient;

        public InventoryItemManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(Routes.InventoryItemEndPoints.Delete(id));
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllInventoryItemsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.InventoryItemEndPoints.GetAll);
            return await response.ToResult<List<GetAllInventoryItemsResponse>>();
        }

        public async Task<IResult<GetInventoryItemByResponse>> GetInventoryItemByAsync(int Id)
        {
            var response = await _httpClient.GetAsync(Routes.InventoryItemEndPoints.GetInventoryItemById(Id));
            return await response.ToResult<GetInventoryItemByResponse>();
        }



        public async Task<IResult<int>> SaveAsync(AddEditInventoryItemCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.InventoryItemEndPoints.Save, request);
            return await response.ToResult<int>();
        }

    }
}
