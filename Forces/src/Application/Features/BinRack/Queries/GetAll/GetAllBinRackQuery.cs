using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.BinRack.Queries.GetAll
{
    public class GetAllBinRackQuery : IRequest<IResult<List<GetAllBinRackResponse>>>
    {
        public GetAllBinRackQuery()
        {

        }

    }
    internal class GetAllBinRackQueryHandler : IRequestHandler<GetAllBinRackQuery, IResult<List<GetAllBinRackResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        public GetAllBinRackQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<List<GetAllBinRackResponse>>> Handle(GetAllBinRackQuery request, CancellationToken cancellationToken)
        {
            var BinRacks = await _unitOfWork.Repository<Models.BinRack>().GetAllAsync();
            var MappedBinRacks = BinRacks.Select(x => new GetAllBinRackResponse()
            {
                BinName = x.BinName,
                BinCode = x.BinCode
            }).ToList();
            return await Result<List<GetAllBinRackResponse>>.SuccessAsync(MappedBinRacks);
        }
    }
}
