using Forces.Application.Features.PersonalItemOperations.Commands.AddEdit;
using Forces.Application.Features.PersonalItemOperations.Queries.Dto;
using Forces.Application.Features.PersonalItemOperations.Queries.Eligibility;
using Forces.Application.Features.PersonalItemOperations.Queries.GetByFillter;
using Forces.Client.Infrastructure.Extensions;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.PersonalItemOperations
{
    public class PersonalItemOperationManager : IPersonalItemOperationManager
    {
        private readonly HttpClient _httpClient;

        public PersonalItemOperationManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<EligibilityModel>> Check(ItemEligibilityQuery Model)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.PersonalItemsOperationsEndpoints.Check, Model);
            return await response.ToResult<EligibilityModel>();
        }

        public async Task<IResult<List<PersonalItemOperationDto>>> GetAllAsync(GetPersonalItemsOperationsByFillter request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.PersonalItemsOperationsEndpoints.GetAll, request);
            return await response.ToResult<List<PersonalItemOperationDto>>();
        }

        public async Task<IResult<List<PersonalItemOperationDto>>> GetAllFillterAsync(GetPersonalItemsOperationsByFillter request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.PersonalItemsOperationsEndpoints.GetByCondition, request);
            return await response.ToResult<List<PersonalItemOperationDto>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditPersonalItemOperationCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.PersonalItemsOperationsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}
