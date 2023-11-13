using Forces.Application.Extensions;
using Forces.Application.Features.House.Queries.GetAll;
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

namespace Forces.Application.Features.House.Queries.GetBySpecifics
{
    public class GetHouseByQuery : IRequest<IResult<List<GetHouseByResponse>>>
    {
        public int? Id { get; set; }
        public string? HouseName { get; set; }
        public string? HouseCode { get; set; }
    }

    internal class GetHouseByQueryHandler : IRequestHandler<GetHouseByQuery, IResult<List<GetHouseByResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetHouseByQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<List<GetHouseByResponse>>> Handle(GetHouseByQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Models.House, bool>> Condition = x => x.Id != 0;
            if (!string.IsNullOrEmpty(request.HouseName))
            {
                Condition = Condition.And(x => x.HouseName == request.HouseName);
            }
            if (!string.IsNullOrEmpty(request.HouseCode))
            {
                Condition = Condition.And(x => x.HouseCode == request.HouseCode);
            }
            var HouseList = await _unitOfWork.Repository<Models.House>().Entities.Where(Condition).ToListAsync();
            var MappedHouseList = HouseList.Select(x => new GetHouseByResponse()
            {
                Id = x.Id,
                BaseId = x.BaseId,
                HouseName = x.HouseName,
                HouseCode = x.HouseCode,
            }).ToList();
            return await Result<List<GetHouseByResponse>>.SuccessAsync(MappedHouseList);
        }
    }
}
