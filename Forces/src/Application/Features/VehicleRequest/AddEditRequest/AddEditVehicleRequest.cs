using AutoMapper;
using Forces.Application.Features.VehicleRequest.Dto;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
using Forces.Application.Models;
using Forces.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.VehicleRequest.AddEditRequest
{
    public class AddEditVehicleRequest : IRequest<IResult<int>>
    {
        public AddRequestDto Dto { get; set; }
    }
    internal class AddEditVehicleRequestHandler : IRequestHandler<AddEditVehicleRequest, IResult<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IVehicleRequestRepository _repository;
        public AddEditVehicleRequestHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IVehicleRequestRepository repository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IResult<int>> Handle(AddEditVehicleRequest request, CancellationToken cancellationToken)
        {

            
            if (request.Dto.Id == 0)
            {
                var MappedRequest = _mapper.Map<Models.VehicleRequest>(request.Dto);
                MappedRequest.RequestCode = await _repository.GenerateRequestCodeAsync();
                MappedRequest.IsDone = false;
                MappedRequest.RequestStep = Enums.VehicleRequestSteps.OC;
               await _unitOfWork.Repository<Models.VehicleRequest>().AddAsync(MappedRequest);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(MappedRequest.Id, "Request Added!");
            }
            else
            {
                var Request = await _unitOfWork.Repository<Models.VehicleRequest>().GetByIdAsync(request.Dto.Id);
                if (Request != null )
                {
                    var MappedRequest = _mapper.Map<Models.VehicleRequest>(request.Dto);
                    await _unitOfWork.Repository<Models.VehicleRequest>().UpdateAsync(MappedRequest);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(MappedRequest.Id, "Request Updated!");
                }
                return await Result<int>.FailAsync("Request Not Found!");
            }
        }
    }
}
