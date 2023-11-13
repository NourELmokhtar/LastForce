using Forces.Application.Extensions;
using Forces.Application.Features.Items.Queries.GetBySpecifics;
using Forces.Application.Interfaces.Repositories;
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

namespace Forces.Application.Features.Building.Queries.GetBySpecifics
{
    public class GetBuildingsByQuery : IRequest<IResult<List<GetBuildingsByResponse>>>
    {
        public int? Id { get; set; }
        public string? BuildingName { get; set; }
        public string? BuildingCode { get; set; }

    }
    internal class GetAllBuildingsQuesryHandler : IRequestHandler<GetBuildingsByQuery, IResult<List<GetBuildingsByResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllBuildingsQuesryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<List<GetBuildingsByResponse>>> Handle(GetBuildingsByQuery request, CancellationToken cancellationToken)
        {

            Expression<Func<Models.Building, bool>> Condition = x => x.Id != 0;
            if (!string.IsNullOrEmpty(request.BuildingName))
            {
                Condition = Condition.And(x => x.BuildingName == request.BuildingName);
            }
            if (!string.IsNullOrEmpty(request.BuildingCode))
            {
                Condition = Condition.And(x => x.BuildingCode == request.BuildingCode);
            }
            var BuildingList = await _unitOfWork.Repository<Models.Building>().Entities.Where(Condition).ToListAsync();
            var MappedBuildingList = BuildingList.Select(x => new GetBuildingsByResponse()
            {
                BaseId = x.BaseId,
                Id = x.Id,
                BuildingName = x.BuildingName,
                BuildingCode = x.BuildingCode,
            }).ToList();
            return await Result<List<GetBuildingsByResponse>>.SuccessAsync(MappedBuildingList);
        }
    }
}
