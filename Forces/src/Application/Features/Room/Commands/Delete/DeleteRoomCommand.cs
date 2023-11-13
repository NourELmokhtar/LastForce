using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Room.Commands.Delete
{
    public class DeleteRoomCommand : IRequest<IResult<int>>
    {
        [Required]
        public int RoomId { get; set; }
    }

    internal class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, IResult<int>>
    {
        private readonly IStringLocalizer<DeleteRoomCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteRoomCommandHandler(
            IStringLocalizer<DeleteRoomCommandHandler> localizer,
            IUnitOfWork<int> unitOfWork)
        {
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<int>> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
        {
            var Room = await _unitOfWork.Repository<Models.Room>().GetByIdAsync(request.RoomId);
            if (Room != null)
            {
                await _unitOfWork.Repository<Models.Room>().DeleteAsync(Room);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(Room.Id, _localizer["Room Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Room Not Found!"]);
            }
        }
    }
}
