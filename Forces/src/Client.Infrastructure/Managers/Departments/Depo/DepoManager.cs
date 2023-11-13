using Forces.Application.Features.DepoDepartment.Commands.AddEdit;
using Forces.Application.Features.DepoDepartment.Queries.GetAll;
using Forces.Application.Features.DepoDepartment.Queries.GetByForceId;
using Forces.Client.Infrastructure.Extensions;
using Forces.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Departments.Depo
{
    public class DepoManager : IDepoManager
    {
        private readonly HttpClient _httpClient;

        public DepoManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(Routes.DepoEndpoints.Delete(id));
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllDepoDepartmentsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.DepoEndpoints.GetAll);
            return await response.ToResult<List<GetAllDepoDepartmentsResponse>>();
        }

        public async Task<IResult<List<GetAllDepoDepartmentsResponse>>> GetAllDepoAsync()
        {
            var response = await _httpClient.GetAsync(Routes.DepoEndpoints.GetAllDepos);
            return await response.ToResult<List<GetAllDepoDepartmentsResponse>>();
        }

        public async Task<IResult<List<GetAllDepoByForceIdResponse>>> GetByForceIdAsync(int ForceId)
        {
            var response = await _httpClient.GetAsync(Routes.DepoEndpoints.GetByForceId(ForceId));
            return await response.ToResult<List<GetAllDepoByForceIdResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditDepoCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.DepoEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}
