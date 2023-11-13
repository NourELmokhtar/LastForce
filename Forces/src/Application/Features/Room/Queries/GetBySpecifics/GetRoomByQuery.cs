using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace Forces.Application.Features.Room.Queries.GetBySpecifics
{
    public class GetRoomByQuery : IRequest<IResult<List<GetRoomByResponse>>>
    {
        public int Id { get; set; }
    }

    internal class GetRoomByQueryHandler : IRequestHandler<GetRoomByQuery, IResult<List<GetRoomByResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetRoomByQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult<List<GetRoomByResponse>>> Handle(GetRoomByQuery request, CancellationToken cancellationToken)
        {
            var Room = await _unitOfWork.Repository<Models.Room>().GetByIdAsync(request.Id);
            var mappedRoom = _mapper.Map<GetRoomByResponse>(Room);
            return (IResult<List<GetRoomByResponse>>)await Result<GetRoomByResponse>.SuccessAsync(mappedRoom);
        }
    }
}
