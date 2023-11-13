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

namespace Forces.Application.Features.BaseSections.Queries.GetByBaseId
{
    public class GetAllSectionsByBaseIdQuery : IRequest<Result<List<GetAllSectionsByBaseIdQueryResponse>>>
    {
        public int Id { get; set; }
    }
    internal class GetAllSectionsByBaseIdQueryQueryHandler : IRequestHandler<GetAllSectionsByBaseIdQuery, Result<List<GetAllSectionsByBaseIdQueryResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllSectionsByBaseIdQueryQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllSectionsByBaseIdQueryResponse>>> Handle(GetAllSectionsByBaseIdQuery request, CancellationToken cancellationToken)
        {
            var BasesSectionList = await _unitOfWork.Repository<Models.BasesSections>().Entities.Where(x => x.BaseId == request.Id).Include(x => x.Base).Select
                (x =>
                    new GetAllSectionsByBaseIdQueryResponse()
                    {
                        BaseId = x.BaseId,
                        ForceId = x.Base.ForceId,
                        Id = x.Id,
                        SectionCode = x.SectionCode,
                        SectionName = x.SectionName
                    }
                )

                .ToListAsync();
            //var MappedBasesSections = _mapper.Map<List<GetAllSectionsByBaseIdQueryResponse>>(BasesSectionList);
            return await Result<List<GetAllSectionsByBaseIdQueryResponse>>.SuccessAsync(BasesSectionList);
        }
    }
}
