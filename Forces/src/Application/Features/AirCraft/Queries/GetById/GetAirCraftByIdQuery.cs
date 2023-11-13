using AutoMapper;
using Forces.Application.Features.AirCraft.Queries.GetAll;
using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.AirCraft.Queries.GetById
{
   public class GetAirCraftByIdQuery :IRequest<Result<GetAllAirCraftResponse>>
    {
        [Required]
        public int Id { get; set; }
    }
    internal class GetAirCraftByIdQueryHandler : IRequestHandler<GetAirCraftByIdQuery, Result<GetAllAirCraftResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAirCraftByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetAllAirCraftResponse>> Handle(GetAirCraftByIdQuery request, CancellationToken cancellationToken)
        {
            var Aircraft = await _unitOfWork.Repository<Models.AirCraft>().GetByIdAsync(request.Id);
            if (Aircraft != null)
            {
                var Mapped = _mapper.Map<GetAllAirCraftResponse>(Aircraft);
                return await Result<GetAllAirCraftResponse>.SuccessAsync(Mapped);
            }
            return await Result<GetAllAirCraftResponse>.FailAsync("No Air Craft Found With this Id!");
        }
    }
}
