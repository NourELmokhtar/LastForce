using AutoMapper;
using Forces.Application.Features.AirCraft.Queries.GetAll;
using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.AirCraft.Queries.GetByAirKindId
{
    public class GetAllAirCraftByAirKindIdQuery : IRequest<Result<List<GetAllAirCraftResponse>>>
    {
        public int AirCraftId { get; set; }
    }
    internal class GetAllGetByAirKindIdQueryQueryHandler : IRequestHandler<GetAllAirCraftByAirKindIdQuery, Result<List<GetAllAirCraftResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllGetByAirKindIdQueryQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllAirCraftResponse>>> Handle(GetAllAirCraftByAirKindIdQuery request, CancellationToken cancellationToken)
        {
            var AirCraftList = await _unitOfWork.Repository<Models.AirCraft>().Entities.Where(x => x.AirKindId == request.AirCraftId).Include(x => x.AirKind).ToListAsync();
            var Mapped = _mapper.Map<List<GetAllAirCraftResponse>>(AirCraftList);
            return await Result<List<GetAllAirCraftResponse>>.SuccessAsync(Mapped);

        }

    }
}
