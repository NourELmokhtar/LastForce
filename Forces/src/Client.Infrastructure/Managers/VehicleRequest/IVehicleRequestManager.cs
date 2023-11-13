using Forces.Application.Features.VehicleRequest.AddEditRequest;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.VehicleRequest
{
    public interface IVehicleRequestManager :IManager
    {
        public Task<IResult<int>> SaveAsync(AddEditVehicleRequest command);
    }
}
