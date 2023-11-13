using AutoMapper;
using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Forces.Queries.GetById
{
    public class GetForceByIdQuery : IRequest<Result<GetForceByIdResponse>>
    {
        public int Id { get; set; }
    }
    internal class GetForceByIdQueryHandler : IRequestHandler<GetForceByIdQuery, Result<GetForceByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetForceByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetForceByIdResponse>> Handle(GetForceByIdQuery request, CancellationToken cancellationToken)
        {
            var Force = await _unitOfWork.Repository<Models.Forces>().GetByIdAsync(request.Id);
            var mappedForce = _mapper.Map<GetForceByIdResponse>(Force);
            return await Result<GetForceByIdResponse>.SuccessAsync(mappedForce);
        }
    }
}
