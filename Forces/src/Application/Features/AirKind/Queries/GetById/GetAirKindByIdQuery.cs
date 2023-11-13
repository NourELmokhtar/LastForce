using AutoMapper;
using Forces.Application.Features.AirKind.Queries.GetAll;
using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.AirKind.Queries.GetById
{
    public class GetAirKindByIdQuery : IRequest<Result<GetAllAirKindResponse>>
    {
        public int Id { get; set; }
    }
    internal class GetAirKindByIdQueryHandler : IRequestHandler<GetAirKindByIdQuery, Result<GetAllAirKindResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAirKindByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetAllAirKindResponse>> Handle(GetAirKindByIdQuery request, CancellationToken cancellationToken)
        {
            var AirKind = await _unitOfWork.Repository<Models.AirKind>().GetByIdAsync(request.Id);
            if (AirKind == null)
            {
                return await Result<GetAllAirKindResponse>.FailAsync("Air Kind Not Found!");
            }
            var mappedAirKind = _mapper.Map<GetAllAirKindResponse>(AirKind);
            return await Result<GetAllAirKindResponse>.SuccessAsync(mappedAirKind);
        }
    }
}
