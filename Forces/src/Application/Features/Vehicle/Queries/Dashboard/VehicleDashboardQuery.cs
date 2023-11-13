using Forces.Application.Interfaces.Repositories;
using Forces.Application.Models;
using Forces.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Vehicle.Queries.Dashboard
{
    public class VehicleDashboardQuery : IRequest<IResult<VehicleDashboardResponse>>
    {
        public VehicleDashboardQuery() { }
    }
    internal class VehicleDashboardQueryHandler : IRequestHandler<VehicleDashboardQuery, IResult<VehicleDashboardResponse>>
    {
        private protected readonly IUnitOfWork<int> _unitOfWork;

        public VehicleDashboardQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<VehicleDashboardResponse>> Handle(VehicleDashboardQuery request, CancellationToken cancellationToken)
        {
            VehicleDashboardResponse result = new VehicleDashboardResponse();
            var Vehicle = await _unitOfWork.Repository<Models.Vehicle>().GetAllAsync();
            result.AllVehicleCount = Vehicle.Count;
            result.AllVehicleCountP = Vehicle.Where(x=>x.VehicleCarryType == Enums.VehicleCarryType.Passenger).Count();
            result.AllVehicleCountS = Vehicle.Where(x=> x.VehicleCarryType == Enums.VehicleCarryType.Shipments).Count();
            result.AvilableCount = Vehicle.Where(x=>x.State == Enums.VehicleState.Avilable).Count();
            result.AvilableCountP = Vehicle.Where(x=>x.State == Enums.VehicleState.Avilable&& x.VehicleCarryType == Enums.VehicleCarryType.Passenger).Count();
            result.AvilableCountS = Vehicle.Where(x=>x.State == Enums.VehicleState.Avilable && x.VehicleCarryType == Enums.VehicleCarryType.Shipments).Count();
            result.OnServiceCount = Vehicle.Where(x=>x.State == Enums.VehicleState.OnService).Count();
            result.OnServiceP = Vehicle.Where(x=>x.State == Enums.VehicleState.OnService&& x.VehicleCarryType == Enums.VehicleCarryType.Passenger).Count();
            result.OnServiceS = Vehicle.Where(x=>x.State == Enums.VehicleState.OnService && x.VehicleCarryType == Enums.VehicleCarryType.Shipments).Count();
            result.NotAvilableCount = Vehicle.Where(x=>x.State == Enums.VehicleState.NotAvilable).Count();
            result.NotAvilableCountP = Vehicle.Where(x=>x.State == Enums.VehicleState.NotAvilable && x.VehicleCarryType == Enums.VehicleCarryType.Passenger).Count();
            result.NotAvilableCountS = Vehicle.Where(x=>x.State == Enums.VehicleState.NotAvilable && x.VehicleCarryType == Enums.VehicleCarryType.Shipments ).Count();
            result.PetrolCount = Vehicle.Where(x => x.FuleType == Enums.FuleType.Petrol).Count();
            result.DiselCount = Vehicle.Where(x => x.FuleType == Enums.FuleType.Disel).Count();
            var VehicleRequests = await _unitOfWork.Repository<Models.VehicleRequest>().GetAllAsync();
            result.AllRequestsCount= VehicleRequests.Count;
            result.AllRequestsCountP = VehicleRequests.Where(x => x.ShipmentType == Enums.ShipmentType.Passengers).Count();
            result.AllRequestsCountS = VehicleRequests.Where(x => x.ShipmentType == Enums.ShipmentType.Package).Count();
            result.RejectedRequests = VehicleRequests.Where(x => x.RequestState == Enums.RequestState.Rejected).Count();
            result.PendingRequests = VehicleRequests.Where(x => x.RequestState == Enums.RequestState.Pending).Count();
            result.CompletedRequests = VehicleRequests.Where(x => x.RequestState == Enums.RequestState.Completed).Count();
            result.CompletedRequestsS = VehicleRequests.Where(x => x.RequestState == Enums.RequestState.Completed).Count();
            result.CompletedRequestsP = VehicleRequests.Where(x => x.RequestState == Enums.RequestState.Completed).Count();
            result.CarrageRequestCount = VehicleRequests.Where(x => x.ShipmentType == Enums.ShipmentType.Package).Count();
            result.PassengersRequestCount = VehicleRequests.Where(x => x.ShipmentType == Enums.ShipmentType.Passengers).Count();
            result.BookedCount = VehicleRequests.Where(x => x.RequestState == Enums.RequestState.Canceldeld && !x.IsDone).Count();
            return await Result<VehicleDashboardResponse>.SuccessAsync(result);
        }
    }
}
