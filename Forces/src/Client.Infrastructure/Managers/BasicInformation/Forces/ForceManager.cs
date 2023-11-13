using Forces.Application.Features.Forces.Commands.AddEdit;
using Forces.Client.Infrastructure.Extensions;
using Forces.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Forces.Application.Features.Forces.Queries.GetAll;
using Forces.Application.Features.Forces.Queries.GetById;

namespace Forces.Client.Infrastructure.Managers.BasicInformation.Forces
{
    public class ForceManager : IForceManager
    {
        private readonly HttpClient _httpClient;

        public ForceManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(Routes.ForcesEndpoints.Delete(id));
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllForcesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ForcesEndpoints.GetAll);
            return await response.ToResult<List<GetAllForcesResponse>>();
        }

        public async Task<IResult<List<GetAllForcesResponse>>> GetAllForcesAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ForcesEndpoints.GetAllForces);
            return await response.ToResult<List<GetAllForcesResponse>>();
        }

        public async Task<IResult<GetForceByIdResponse>> GetForceByIdAsync(int Id)
        {
            var response = await _httpClient.GetAsync(Routes.ForcesEndpoints.GetForceById(Id));
            return await response.ToResult<GetForceByIdResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditForceCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ForcesEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}
