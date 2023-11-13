using AutoMapper;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Room.Commands.AddEdit
{
    public class AddEditRoomCommand : IRequest<IResult<int>>
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public int BuildingId { get; set; }
    }

    internal class AddEditRoomCommandHandler : IRequestHandler<AddEditRoomCommand, IResult<int>>
    {
        private protected IItemRepository _ItemsRepository;
        private protected IUnitOfWork<int> _unitOfWork;
        private protected IMapper _mapper;
        private readonly IStringLocalizer<AddEditRoomCommandHandler> _localizer;
        private readonly IVoteCodeService _voteCodeService;

        public AddEditRoomCommandHandler(
            IItemRepository itemsRepository,
            IUnitOfWork<int> unitOfWork,
            IMapper mapper,
            IStringLocalizer<AddEditRoomCommandHandler> localizer,
            IVoteCodeService voteCodeService)
        {
            _ItemsRepository = itemsRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _voteCodeService = voteCodeService;
        }

        public async Task<IResult<int>> Handle(AddEditRoomCommand request, CancellationToken cancellationToken)
        {
            if (request.Id == 0)
            {
                var ExistRoom = await _unitOfWork.Repository<Models.Room>().Entities.FirstOrDefaultAsync(x => x.RoomNumber == request.RoomNumber && x.BuildingId == request.BuildingId);
                if (ExistRoom != null)
                {
                    return await Result<int>.FailAsync(_localizer["This Room Name Is Already Exist!"]);
                }
                else
                {
                    Models.Room Room = new Models.Room()
                    {
                        RoomNumber = request.RoomNumber,
                        BuildingId = request.BuildingId,
                    };
                    await _unitOfWork.Repository<Models.Room>().AddAsync(Room);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(Room.Id, _localizer["Room Added Successfuly!"]);
                }
            }
            else
            {
                var ExistRoom = await _unitOfWork.Repository<Models.Room>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (ExistRoom == null)
                {
                    return await Result<int>.FailAsync(_localizer["Room Not Found!!"]);
                }
                else
                {
                    var ExistnameRoom = await _unitOfWork.Repository<Models.Room>().Entities.FirstOrDefaultAsync(x => x.RoomNumber == request.RoomNumber && x.Id != request.Id);
                    if (ExistnameRoom != null)
                    {
                        return await Result<int>.FailAsync(_localizer["This Room Is Already Exist!"]);
                    }
                    else
                    {
                        ExistRoom.RoomNumber = request.RoomNumber;
                        await _unitOfWork.Repository<Models.Room>().UpdateAsync(ExistRoom);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(ExistRoom.Id, _localizer["Room Updated Successfuly!"]);
                    }
                }
            }
        }
    }
}
