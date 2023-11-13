using AutoMapper;
using Forces.Application.Enums;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services.Identity;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.AirKind.Queries.GetAll
{
    public class GetAllAirKindQuery : IRequest<Result<List<GetAllAirKindResponse>>>
    {
       
        public GetAllAirKindQuery()
        {
             
        }
    }
    internal class GetAllAirKindQueryHandler : IRequestHandler<GetAllAirKindQuery, Result<List<GetAllAirKindResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllAirKindQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<Result<List<GetAllAirKindResponse>>> Handle(GetAllAirKindQuery request, CancellationToken cancellationToken)
        { 
          var  AirKindList = await _unitOfWork.Repository<Models.AirKind>().GetAllAsync();
            var MappedAirKind = _mapper.Map<List<GetAllAirKindResponse>>(AirKindList);
            return await Result<List<GetAllAirKindResponse>>.SuccessAsync(MappedAirKind);
        }
    }
}
