using AutoMapper;
using Forces.Application.Extensions;
using Forces.Application.Features.AirKind.Queries.GetAll;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services.Identity;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.AirKind.Queries.GetByAirTypeld
{
    public class GetAllAirKindByAirTypeIdQuery : IRequest<Result<List<GetAllAirKindResponse>>>
    {
        [Required]
        public int TypeID { get; set; }
    }
    internal class GetAllAirKindByAirTypeIdQueryHandler : IRequestHandler<GetAllAirKindByAirTypeIdQuery, Result<List<GetAllAirKindResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllAirKindByAirTypeIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<Result<List<GetAllAirKindResponse>>> Handle(GetAllAirKindByAirTypeIdQuery request, CancellationToken cancellationToken)
        {
            var AirKindList = await _unitOfWork.Repository<Models.AirKind>().Entities.Where(x => x.AirTypeId == request.TypeID).ToListAsync();
            var MappedAirKind = _mapper.Map<List<GetAllAirKindResponse>>(AirKindList);
            return await Result<List<GetAllAirKindResponse>>.SuccessAsync(MappedAirKind);
        }
    }
}