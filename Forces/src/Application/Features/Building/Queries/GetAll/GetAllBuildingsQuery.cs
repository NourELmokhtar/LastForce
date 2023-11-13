using Forces.Application.Features.Items.Queries.GetAll;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
using Forces.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Building.Queries.GetAll
{
    public class GetAllBuildingsQuery : IRequest<IResult<List<GetAllBuildingsResponse>>>
    {
        public GetAllBuildingsQuery()
        {

        }
    }
    internal class GetAllBuildingsQueryHandler : IRequestHandler<GetAllBuildingsQuery, IResult<List<GetAllBuildingsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IVoteCodeService _voteCodeService;

        public GetAllBuildingsQueryHandler(IUnitOfWork<int> unitOfWork, IVoteCodeService voteCodeService)
        {
            _unitOfWork = unitOfWork;
            _voteCodeService = voteCodeService;
        }

        public async Task<IResult<List<GetAllBuildingsResponse>>> Handle(GetAllBuildingsQuery request, CancellationToken cancellationToken)
        {
            var BuildingList = await _unitOfWork.Repository<Models.Building>().GetAllAsync();
            var MappedBuildingList = BuildingList.Select(x => new GetAllBuildingsResponse()
            {
                Id = x.Id,
                BuildingCode = x.BuildingCode,
                BuildingName = x.BuildingName,
                BaseName = _unitOfWork.Repository<Models.Bases>().GetAllAsync().Result.Where(y => y.Id == x.BaseId).FirstOrDefault().BaseName,

            }).ToList();
            return await Result<List<GetAllBuildingsResponse>>.SuccessAsync(MappedBuildingList);
        }
    }
}