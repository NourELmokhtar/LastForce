using Forces.Application.Features.Vehicle.Queries.GetAll;
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

namespace Forces.Application.Features.Demand.Queries.GetAll
{
    class GetAllDemandQuery : IRequest<IResult<List<GetAllDemandResponse>>>
    {
        public GetAllDemandQuery()
        {

        }

    }
    internal class GetAllDemandQueryHandler : IRequestHandler<GetAllDemandQuery, IResult<List<GetAllDemandResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        public GetAllDemandQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<List<GetAllDemandResponse>>> Handle(GetAllDemandQuery request, CancellationToken cancellationToken)
        {

            var Demand = await _unitOfWork.Repository<Models.Demand>().Entities.Include(x => x.Id).ToListAsync();
            var MappedDemand = Demand.Select(x => new GetAllDemandResponse()
            {
                Id = x.Id,
                DemandSequence = x.DemandSequence,
                DemandNo = x.DemandNo,
                PartNo = x.PartNo,
                DemandedQty = x.DemandedQty,
                Priority = x.Priority,
                Category = x.Category,
                Consignee = x.Consignee,
                SpecialInstructions = x.SpecialInstructions,
                Description = x.Description,
                RAFOReference = x.RAFOReference,
                DofQ = x.DofQ,
                PartName = x.PartName,
                StationIV = x.StationIV,
                VoteCode = x.VoteCode,
                PhysicalStockBalance = x.PhysicalStockBalance,
                DuesIn = x.DuesIn,
                DuesOut = x.DuesOut,
                ClassofStore = x.ClassofStore,
                IssuedQty = x.IssuedQty,
                BaseId = x.BaseId,
                SectionId = x.SectionId

            }).ToList();
            return await Result<List<GetAllDemandResponse>>.SuccessAsync(MappedDemand);
        }
    }






}

