using Forces.Application.Features.Vehicle.Commands.AddEdit;
using Forces.Application.Features.Vehicle.Queries.Dashboard;
using Forces.Application.Features.Vehicle.Queries.GetAll;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Vehicles
{
    public interface IVehicleManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditVehicleCommand command);
        Task<IResult<int>> DeleteAsync(int id);
        Task<IResult<List<GetAllVehicleResponse>>> GetAllAsync();
        Task<IResult<VehicleDashboardResponse>> GetDashboardDataAsync();
    }
}
