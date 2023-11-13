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

namespace Forces.Application.Features.AirType.Commands.Delete
{
    public class DeleteAirTypeCommand : IRequest<Result<int>>
    {
        [Required]
        public int Id { get; set; }
    }
    internal class DeleteAirTypeCommandHandler : IRequestHandler<DeleteAirTypeCommand, Result<int>>
    {

        private readonly IStringLocalizer<DeleteAirTypeCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteAirTypeCommandHandler(IStringLocalizer<DeleteAirTypeCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteAirTypeCommand request, CancellationToken cancellationToken)
        {
            var AirType = await _unitOfWork.Repository<Models.AirType>().GetByIdAsync(request.Id);
            if (AirType == null)
            {
                return await Result<int>.FailAsync(_localizer["AirType Not Found!"]);
            }
            var isAirTypeUsed = AirType.Kinds.Any();
            if (!isAirTypeUsed)
            {
                await _unitOfWork.Repository<Models.AirType>().DeleteAsync(AirType);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(AirType.Id, _localizer["AirType Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}
