using Forces.Application.Features.InventoryItemBridge.Commands.AddEdit;
using Forces.Application.Features.InventoryItemBridge.Queries.GetAll;
using Forces.Application.Features.Items.Commands.AddEdit;
using Forces.Application.Features.Items.Queries.GetAll;
using Forces.Application.Features.Items.Queries.GetBySpecifics;
using Forces.Client.Infrastructure.Extensions;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.InventoryItemBridge
{
    public class InventoryItemBridgeManager: IInventoryItemBridgeManager
    {
        private readonly HttpClient _httpClient;

        public InventoryItemBridgeManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(Routes.InventoryItemBridgeEndPoints.Delete(id));
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllInventoryItemBridgeResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.InventoryItemBridgeEndPoints.GetAll);
            return await response.ToResult<List<GetAllInventoryItemBridgeResponse>>();
        }

       

        public Task<IResult<GetAllInventoryItemBridgeResponse>> GetInventoryItemByAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<int>> SaveAsync(AddEditInventoryItemBridgeCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.InventoryItemBridgeEndPoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}
