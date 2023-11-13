using Forces.Application.Features.HQDepartment.Commands.AddEdit;
using Forces.Application.Features.HQDepartment.Queries.GetAll;
using Forces.Application.Features.HQDepartment.Queries.GetByForceId;
using Forces.Client.Infrastructure.Extensions;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Departments.HQ
{
    public class HQManager : IHQManager
    {
        private readonly HttpClient _httpClient;

        public HQManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(Routes.HQEndpoints.Delete(id));
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllHQDepartmentsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.HQEndpoints.GetAll);
            return await response.ToResult<List<GetAllHQDepartmentsResponse>>();
        }

        public async Task<IResult<List<GetAllHQDepartmentsResponse>>> GetAllHqAsync()
        {
            var response = await _httpClient.GetAsync(Routes.HQEndpoints.GetAllHQ);
            return await response.ToResult<List<GetAllHQDepartmentsResponse>>();
        }

        public async Task<IResult<List<GetAllHQbyForceIdResponse>>> GetByForceIdAsync(int ForceId)
        {
            var response = await _httpClient.GetAsync(Routes.HQEndpoints.GetByForceId(ForceId));
            return await response.ToResult<List<GetAllHQbyForceIdResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditHQCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.HQEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}
