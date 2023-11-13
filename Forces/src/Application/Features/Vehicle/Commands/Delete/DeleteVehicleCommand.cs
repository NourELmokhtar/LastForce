using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Vehicle.Commands.Delete
{
    public class DeleteVehicleCommand : IRequest<Result<int>>
    {
        public int VehicleID { get; set; }

    }
    internal class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, Result<int>>
    {

        private readonly IStringLocalizer<DeleteVehicleCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteVehicleCommandHandler(IStringLocalizer<DeleteVehicleCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {

            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteVehicleCommand command, CancellationToken cancellationToken)
        {

            var vehicle = await _unitOfWork.Repository<Models.Vehicle>().GetByIdAsync(command.VehicleID);
            if (vehicle != null)
            {
                await _unitOfWork.Repository<Models.Vehicle>().DeleteAsync(vehicle);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(vehicle.Id, _localizer["Vehicle Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Vehicle Not Found!"]);
            }

        }
    }
}
