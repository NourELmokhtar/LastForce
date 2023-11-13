using Forces.Application.Features.PersonalItems.Commands.AddEdit;
using Forces.Application.Features.PersonalItems.Queries.DTO;
using Forces.Application.Features.PersonalItems.Queries.GetByFillter;
using Forces.Client.Infrastructure.Extensions;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.PersonalItems
{
    public class PersonalItemsManager : IPersonalItemsManager
    {
        private readonly HttpClient _httpClient;

        public PersonalItemsManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(Routes.PersonalItemsEndpoints.Delete(id));
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<PersonalItemDto>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.PersonalItemsEndpoints.GetAll);
            return await response.ToResult<List<PersonalItemDto>>();
        }

        public async Task<IResult<List<PersonalItemDto>>> GetAllByConditionsAsync(GetPersonalItemsByFillter ConditionsModel)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.PersonalItemsEndpoints.GetByCondition, ConditionsModel);
            return await response.ToResult<List<PersonalItemDto>>();
        }

        public async Task<IResult<PersonalItemDto>> GetByIdAsync(int Id)
        {
            var response = await _httpClient.GetAsync(Routes.PersonalItemsEndpoints.GetById(Id));
            return await response.ToResult<PersonalItemDto>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditPersonalItemCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.PersonalItemsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}
