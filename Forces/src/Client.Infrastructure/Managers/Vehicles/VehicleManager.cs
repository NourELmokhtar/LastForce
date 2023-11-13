using Forces.Application.Features.Vehicle.Commands.AddEdit;
using Forces.Application.Features.Vehicle.Queries.GetAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Forces.Client.Infrastructure.Routes;
using Forces.Shared.Wrapper;
using Forces.Client.Infrastructure.Extensions;
using System.Net.Http.Json;
using Forces.Application.Features.Vehicle.Queries.Dashboard;

namespace Forces.Client.Infrastructure.Managers.Vehicles
{
    public class VehicleManager : IVehicleManager
    {
        private readonly HttpClient _httpClient;

        public VehicleManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var Response = await _httpClient.DeleteAsync(VehicleEndpoints.Delete(id));
            return await Response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllVehicleResponse>>> GetAllAsync()
        {
            var Response = await _httpClient.GetAsync(VehicleEndpoints.GetAll);
            return await Response.ToResult<List<GetAllVehicleResponse>>();
        }

        public async Task<IResult<VehicleDashboardResponse>> GetDashboardDataAsync()
        {
            var Response = await _httpClient.GetAsync(VehicleEndpoints.DashboardData);
            return await Response.ToResult<VehicleDashboardResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditVehicleCommand command)
        {
            var Response = await _httpClient.PostAsJsonAsync(VehicleEndpoints.Save, command);
            return await Response.ToResult<int>();
        }
    }
}
