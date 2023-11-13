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

namespace Forces.Application.Features.Building.Commands.Delete
{
    public class DeleteBuildingCommand : IRequest<IResult<int>>
    {
        [Required]
        public int BuildingId { get; set; }
    }
    internal class DeleteItemCommandHandler : IRequestHandler<DeleteBuildingCommand, IResult<int>>
    {
        private readonly IStringLocalizer<DeleteItemCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;


        public DeleteItemCommandHandler(IBuildingRepository buildingRepository, IStringLocalizer<DeleteItemCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<int>> Handle(DeleteBuildingCommand request, CancellationToken cancellationToken)
        {
            var Building = await _unitOfWork.Repository<Models.Building>().GetByIdAsync(request.BuildingId);
            if (Building != null)
            {
                await _unitOfWork.Repository<Models.Building>().DeleteAsync(Building);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(Building.Id, _localizer["Building Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Building Not Found!"]);
            }
        }
    }
}
