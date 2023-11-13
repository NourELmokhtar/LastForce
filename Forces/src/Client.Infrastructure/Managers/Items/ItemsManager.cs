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

namespace Forces.Client.Infrastructure.Managers.Items
{
    public class ItemsManager : IItemsManager
    {
        private readonly HttpClient _httpClient;

        public ItemsManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(Routes.ItemsEndpoints.Delete(id));
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllItemsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ItemsEndpoints.GetAll);
            return await response.ToResult<List<GetAllItemsResponse>>();
        }

        public async Task<IResult<List<GetItemsByResponse>>> GetAllByConditionsAsync(GetAllItemsBy ConditionsModel)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ItemsEndpoints.GetAllByConditions, ConditionsModel);
            return await response.ToResult<List<GetItemsByResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditItemCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ItemsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}
