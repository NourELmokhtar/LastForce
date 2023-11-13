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

namespace Forces.Application.Features.House.Commands.Delete
{
    public class DeleteHouseCommand : IRequest<IResult<int>>
    {
        [Required]
        public int HouseId { get; set; }
    }

    internal class DeleteHouseCommandHandler : IRequestHandler<DeleteHouseCommand, IResult<int>>
    {
        private readonly IStringLocalizer<DeleteHouseCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteHouseCommandHandler(
            IStringLocalizer<DeleteHouseCommandHandler> localizer,
            IUnitOfWork<int> unitOfWork)
        {
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<int>> Handle(DeleteHouseCommand request, CancellationToken cancellationToken)
        {
            var House = await _unitOfWork.Repository<Models.House>().GetByIdAsync(request.HouseId);
            if (House != null)
            {
                await _unitOfWork.Repository<Models.House>().DeleteAsync(House);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(House.Id, _localizer["House Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["House Not Found!"]);
            }
        }
    }
}
