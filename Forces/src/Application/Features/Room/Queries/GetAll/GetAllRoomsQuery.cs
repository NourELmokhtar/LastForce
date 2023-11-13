using Forces.Application.Features.Room.Queries.GetAll;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
using Forces.Application.Responses.Identity;
using Forces.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Room.Queries.GetAll
{
    public class GetAllRoomQuery : IRequest<IResult<List<GetAllRoomsResponse>>>
    {
        public GetAllRoomQuery()
        {
        }
    }

    internal class GetAllRoomQueryHandler : IRequestHandler<GetAllRoomQuery, IResult<List<GetAllRoomsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IVoteCodeService _voteCodeService;

        public GetAllRoomQueryHandler(
            IUnitOfWork<int> unitOfWork,
            IVoteCodeService voteCodeService)
        {
            _unitOfWork = unitOfWork;
            _voteCodeService = voteCodeService;
        }

        public async Task<IResult<List<GetAllRoomsResponse>>> Handle(GetAllRoomQuery request, CancellationToken cancellationToken)
        {
            var Rooms = await _unitOfWork.Repository<Models.Room>().GetAllAsync();
            var MappedRooms = Rooms.Select(x => new GetAllRoomsResponse()
            {
                RoomNumber = x.RoomNumber,
                Id = x.Id,
                BuildingName = _unitOfWork.Repository<Models.Building>().GetAllAsync().Result.Where(y => y.Id == x.BuildingId).FirstOrDefault().BuildingName,
                BuildingId = x.BuildingId,


            }).ToList();
            return await Result<List<GetAllRoomsResponse>>.SuccessAsync(MappedRooms);
        }
    }
}