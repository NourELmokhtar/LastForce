using AutoMapper;
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

namespace Forces.Application.Features.Vehicle.Queries.GetAll
{
   public class GetAllVehicleQuery : IRequest<IResult<List<GetAllVehicleResponse>>>
    {
        public GetAllVehicleQuery()
        {
        }
    }
    internal class GetAllVehicleQueryHandler : IRequestHandler<GetAllVehicleQuery, IResult<List<GetAllVehicleResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private protected IMapper _mapper;
        public GetAllVehicleQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<IResult<List<GetAllVehicleResponse>>> Handle(GetAllVehicleQuery request, CancellationToken cancellationToken)
        {
            var Vehicle = await _unitOfWork.Repository<Models.Vehicle>().Entities.Include(x => x.VehicleColor).ToListAsync();
            var MappedVehicle = _mapper.Map<List<GetAllVehicleResponse>>(Vehicle);
            return await Result<List<GetAllVehicleResponse>>.SuccessAsync(MappedVehicle);
        }
    }
}
