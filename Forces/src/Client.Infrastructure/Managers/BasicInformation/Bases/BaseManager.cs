using Forces.Application.Features.Bases.Commands.AddEdit;
using Forces.Application.Features.Bases.Queries.GetAll;
using Forces.Application.Features.Bases.Queries.GetByForceId;
using Forces.Application.Features.Bases.Queries.GetById;
using Forces.Client.Infrastructure.Extensions;
using Forces.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.BasicInformation.Bases
{
    public class BaseManager : IBaseManager
    {
        private readonly HttpClient _httpClient;

        public BaseManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(Routes.BaseEndpoints.Delete(id));
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllBasesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.BaseEndpoints.GetAll);
            return await response.ToResult<List<GetAllBasesResponse>>();
        }

        public async Task<IResult<GetBaseByIdResponse>> GetBaseByIdAsync(int Id)
        {
            var response = await _httpClient.GetAsync(Routes.BaseEndpoints.GetBaseById(Id));
            return await response.ToResult<GetBaseByIdResponse>();
        }

        public async Task<IResult<List<GetAllBasesByForceIdResponse>>> GetBasesByForceIdAsync(int ForceId)
        {
            var response = await _httpClient.GetAsync(Routes.BaseEndpoints.GetBaseByForceId(ForceId));
            return await response.ToResult<List<GetAllBasesByForceIdResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditBaseCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.BaseEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}
