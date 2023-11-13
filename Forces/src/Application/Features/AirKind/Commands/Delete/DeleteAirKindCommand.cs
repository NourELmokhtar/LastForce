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

namespace Forces.Application.Features.AirKind.Commands.Delete
{
    public class DeleteAirKindCommand : IRequest<IResult<int>>
    {
        [Required]
        public int AirKindId { get; set; }
    }
    internal class DeleteAirKindCommandHandler : IRequestHandler<DeleteAirKindCommand, IResult<int>>
    {
        private readonly IAirKindRepository _airkindRepository;
        private readonly IStringLocalizer<DeleteAirKindCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteAirKindCommandHandler(IAirKindRepository airkindRepository, IStringLocalizer<DeleteAirKindCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {
            _airkindRepository = airkindRepository;
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<int>> Handle(DeleteAirKindCommand command, CancellationToken cancellationToken)
        {
            var isAirKindUsed = await _airkindRepository.IsAirKindInused(command.AirKindId);
            if (!isAirKindUsed)
            {
                var AirKind = await _unitOfWork.Repository<Models.AirKind>().GetByIdAsync(command.AirKindId);
                if (AirKind != null)
                {
                    await _unitOfWork.Repository<Models.AirKind>().DeleteAsync(AirKind);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(AirKind.Id, _localizer["AirKind Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["AirKind Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}
