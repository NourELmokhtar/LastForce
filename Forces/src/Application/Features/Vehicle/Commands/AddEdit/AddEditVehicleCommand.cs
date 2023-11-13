using AutoMapper;
using Forces.Application.Enums;
using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Vehicle.Commands.AddEdit
{
    public class AddEditVehicleCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleName { get; set; }
        public int ColorID { get; set; }
        public string Year { get; set; } // true , False
        public string MadeIn { get; set; }
        public BattryType BattryType { get; set; }
        public string EngineNo { get; set; }
        public DateTime? DateOfEnter { get; set; }
        public DateTime? EndOfServiceDate { get; set; }
        public int WheelsCount { get; set; }
        public string WheelsSize { get; set; }
        public FuleType FuleType { get; set; }
        public string AdditionalNumber { get; set; }
        public string WheelsYear { get; set; }
        public string VIN { get; set; }
        public VehicleState State { get; set; }
        public VehicleCarryType VehicleCarryType { get; set; }

    }

    internal class AddEditVehicleCommandHandler : IRequestHandler<AddEditVehicleCommand, Result<int>>
    {

        private readonly IStringLocalizer<AddEditVehicleCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;
        private protected IMapper _mapper;
        public AddEditVehicleCommandHandler(IStringLocalizer<AddEditVehicleCommandHandler> localizer, IUnitOfWork<int> unitOfWork, IMapper mapper)
        {

            _localizer = localizer;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(AddEditVehicleCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
            {
                var ExistVehicle = await _unitOfWork.Repository<Models.Vehicle>().Entities.FirstOrDefaultAsync(x => x.VehicleName == request.VehicleName || x.VehicleNumber == request.VehicleNumber);
                if (ExistVehicle != null)
                {
                    return await Result<int>.FailAsync(_localizer["This Vehicle Is Already Exist!"]);
                }
                else
                {
                    Models.Vehicle vehicle = _mapper.Map<Models.Vehicle>(request);
                    await _unitOfWork.Repository<Models.Vehicle>().AddAsync(vehicle);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(vehicle.Id, _localizer["Vehicle Added Successfuly!"]);
                }
            }
            else
            {
                var ExistVehicle = await _unitOfWork.Repository<Models.Vehicle>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (ExistVehicle == null)
                {
                    return await Result<int>.FailAsync(_localizer["Vehicle Not Found!!"]);
                }
                else
                {
                    var ExistnameRack = await _unitOfWork.Repository<Models.Vehicle>().Entities.FirstOrDefaultAsync(x => (x.VehicleName == request.VehicleName || x.VehicleNumber == request.VehicleNumber) && x.Id != request.Id);
                    if (ExistnameRack != null)
                    {
                        return await Result<int>.FailAsync(_localizer["This Vehicle Is Already Exist!"]);
                    }
                    else
                    {
                        ExistVehicle = _mapper.Map<Models.Vehicle>(request);
                        await _unitOfWork.Repository<Models.Vehicle>().UpdateAsync(ExistVehicle);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(ExistVehicle.Id, _localizer["Vehicle Updated Successfuly!"]);
                    }
                }
            }
        }
    }
}