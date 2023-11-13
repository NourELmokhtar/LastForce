using AutoMapper;
using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.BaseSections.Queries.GetById
{
    public class GetBaseSectionByIdQuery : IRequest<Result<GetBaseSectionByIdQueryResponse>>
    {
        public int Id { get; set; }
    }
    internal class GetBaseSectionByIdQueryHandler : IRequestHandler<GetBaseSectionByIdQuery, Result<GetBaseSectionByIdQueryResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetBaseSectionByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetBaseSectionByIdQueryResponse>> Handle(GetBaseSectionByIdQuery request, CancellationToken cancellationToken)
        {
            var BaseSection = await _unitOfWork.Repository<Models.Bases>().GetByIdAsync(request.Id);
            var mappedBaseSection = _mapper.Map<GetBaseSectionByIdQueryResponse>(BaseSection);
            return await Result<GetBaseSectionByIdQueryResponse>.SuccessAsync(mappedBaseSection);
        }
    }
}
