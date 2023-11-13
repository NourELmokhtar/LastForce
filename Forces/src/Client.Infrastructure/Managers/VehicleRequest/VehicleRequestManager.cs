using Forces.Application.Features.VehicleRequest.AddEditRequest;
using Forces.Client.Infrastructure.Extensions;
using Forces.Client.Infrastructure.Routes;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.VehicleRequest
{
    public class VehicleRequestManager : IVehicleRequestManager
    {
        private readonly HttpClient _httpClient;

        public VehicleRequestManager(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<IResult<int>> SaveAsync(AddEditVehicleRequest command)
        {
            var response = await _httpClient.PostAsJsonAsync(VehicleRequestEndpoints.Save, command);
            return await response.ToResult<int>();
        }
    }
}
