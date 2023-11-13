using AutoMapper;
using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Bases.Queries.GetById
{
    public class GetBaseByIdQuery : IRequest<Result<GetBaseByIdResponse>>
    {
        public int Id { get; set; }
    }
    internal class GetBaseByIdQueryHandler : IRequestHandler<GetBaseByIdQuery, Result<GetBaseByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetBaseByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetBaseByIdResponse>> Handle(GetBaseByIdQuery request, CancellationToken cancellationToken)
        {
            var Base = await _unitOfWork.Repository<Models.Bases>().GetByIdAsync(request.Id);
            var mappedBase = _mapper.Map<GetBaseByIdResponse>(Base);
            return await Result<GetBaseByIdResponse>.SuccessAsync(mappedBase);
        }
    }
}
