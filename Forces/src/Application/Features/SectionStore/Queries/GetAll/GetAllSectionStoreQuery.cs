using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.SectionStore.Queries.GetAll
{
    public class GetAllSectionStoreQuery : IRequest<IResult<List<GetAllSectionStoreResponse>>>
    {
        public GetAllSectionStoreQuery()
        {

        }
    }
    internal class GetAllSectionStoreQueryHandler : IRequestHandler<GetAllSectionStoreQuery, IResult<List<GetAllSectionStoreResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        public GetAllSectionStoreQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<List<GetAllSectionStoreResponse>>> Handle(GetAllSectionStoreQuery request, CancellationToken cancellationToken)
        {
            var SectionStore = await _unitOfWork.Repository<Models.SectionStore>().GetAllAsync();
            var MappedSectionStore = SectionStore.Select(x => new GetAllSectionStoreResponse()
            {
                StoreName = x.StoreName,
                StoreCode = x.StoreCode,
                Id = x.Id,
                SectionId = x.SectionId
            }).ToList();
            return await Result<List<GetAllSectionStoreResponse>>.SuccessAsync(MappedSectionStore);
        }
    }
}
