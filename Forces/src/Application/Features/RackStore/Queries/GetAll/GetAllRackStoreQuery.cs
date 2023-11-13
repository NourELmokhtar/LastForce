using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.RackStore.Queries.GetAll
{
    public class GetAllRackStoreQuery : IRequest<IResult<List<GetAllRackStoreResponse>>>
    {
        public GetAllRackStoreQuery()
        {

        }
    }
    internal class GetAllRackStoreQueryHandler : IRequestHandler<GetAllRackStoreQuery, IResult<List<GetAllRackStoreResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        public GetAllRackStoreQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<List<GetAllRackStoreResponse>>> Handle(GetAllRackStoreQuery request, CancellationToken cancellationToken)
        {
            var RackStore = await _unitOfWork.Repository<Models.RackStore>().GetAllAsync();
            var MappedRackStores = RackStore.Select(x => new GetAllRackStoreResponse()
            {
                RackName = x.RackName,
                RackCode = x.RackCode

            }).ToList();
            return await Result<List<GetAllRackStoreResponse>>.SuccessAsync(MappedRackStores);
        }
    }
}
