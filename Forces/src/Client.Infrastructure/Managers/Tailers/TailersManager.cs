using Forces.Application.Features.Tailers.Commands.AddEdit;
using Forces.Application.Features.Tailers.Queries;
using Forces.Client.Infrastructure.Extensions;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Tailers
{
    public class TailersManager : ITailersManager
    {
        private readonly HttpClient _httpClient;

        public TailersManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(Routes.TailersEndpoints.Delete(id));
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<TailerDto>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.TailersEndpoints.GetAll);
            return await response.ToResult<List<TailerDto>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditTailerCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.TailersEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}
