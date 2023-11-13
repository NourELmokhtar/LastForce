using AutoMapper;
using Forces.Application.Features.AirType.Queries.GetAll;
using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.AirType.Queries.GetById
{
    public class GetAirTypeByIdQuery : IRequest<Result<GetAllAirTypeResponse>>
    {
        public int Id { get; set; }
    }
    internal class GetAirTypeByIdQueryHandler : IRequestHandler<GetAirTypeByIdQuery, Result<GetAllAirTypeResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAirTypeByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        
            public async Task<Result<GetAllAirTypeResponse>> Handle(GetAirTypeByIdQuery request, CancellationToken cancellationToken)
            {
                var AirType = await _unitOfWork.Repository<Models.AirType>().GetByIdAsync(request.Id);
                var mappedAirType = _mapper.Map<GetAllAirTypeResponse>(AirType);
                return await Result<GetAllAirTypeResponse>.SuccessAsync(mappedAirType);
            }
        }
    }
