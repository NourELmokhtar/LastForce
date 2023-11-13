using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.MeasureUnits.Queries.GetAll
{
    public class GetAllMeasureUnitsQuery : IRequest<IResult<List<GetAllMeasureUnitsResponse>>>
    {
        public GetAllMeasureUnitsQuery()
        {

        }
    }
    internal class GetAllMeasureUnitsQueryHandeler : IRequestHandler<GetAllMeasureUnitsQuery, IResult<List<GetAllMeasureUnitsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllMeasureUnitsQueryHandeler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<List<GetAllMeasureUnitsResponse>>> Handle(GetAllMeasureUnitsQuery request, CancellationToken cancellationToken)
        {
            var UnitsList = await _unitOfWork.Repository<Models.MeasureUnits>().GetAllAsync();
            var MappedUnitList = UnitsList.Select(x => new GetAllMeasureUnitsResponse() { Id = x.Id, Name = x.Name }).ToList();

            return await Result<List<GetAllMeasureUnitsResponse>>.SuccessAsync(MappedUnitList);
        }
    }
}
