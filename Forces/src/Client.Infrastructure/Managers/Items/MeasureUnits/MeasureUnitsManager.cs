using Forces.Application.Features.MeasureUnits.Commands.AddEdit;
using Forces.Application.Features.MeasureUnits.Queries.GetAll;
using Forces.Client.Infrastructure.Extensions;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Items.MeasureUnits
{
    public class MeasureUnitsManager : IMeasureUnitsManager
    {
        private readonly HttpClient _httpClient;

        public MeasureUnitsManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(Routes.MeasureUnitsEndpoints.Delete(id));
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllMeasureUnitsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.MeasureUnitsEndpoints.GetAll);
            return await response.ToResult<List<GetAllMeasureUnitsResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditMeasureUnitsCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.MeasureUnitsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}
