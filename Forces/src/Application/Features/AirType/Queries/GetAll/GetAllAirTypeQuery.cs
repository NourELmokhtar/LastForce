using AutoMapper;
using Forces.Application.Enums;
using Forces.Application.Extensions;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services.Identity;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.AirType.Queries.GetAll
{
    public class GetAllAirTypeQuery : IRequest<Result<List<GetAllAirTypeResponse>>>
    {

        public GetAllAirTypeQuery()
        {

        }
    }
    internal class GetAllAirTypeQueryHandler : IRequestHandler<GetAllAirTypeQuery, Result<List<GetAllAirTypeResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        

        public GetAllAirTypeQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllAirTypeResponse>>> Handle(GetAllAirTypeQuery request, CancellationToken cancellationToken)
        {
             
           var AirTypeList = await _unitOfWork.Repository<Models.AirType>().GetAllAsync();
            var MappedAirType = _mapper.Map<List<GetAllAirTypeResponse>>(AirTypeList);
            return await Result<List<GetAllAirTypeResponse>>.SuccessAsync(MappedAirType);
        }
    }
}
