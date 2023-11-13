using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Tailers.Commands.Delete
{
    public class DeleteTailerCommand : IRequest<IResult<int>>
    {
        [Required]
        public int Id { get; set; }
    }
    internal class DeleteTilerCommandHandler : IRequestHandler<DeleteTailerCommand, IResult<int>>
    {
        private readonly IStringLocalizer<DeleteTilerCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteTilerCommandHandler(IStringLocalizer<DeleteTilerCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<int>> Handle(DeleteTailerCommand command, CancellationToken cancellationToken)
        {
            var Tailer = await _unitOfWork.Repository<Models.Tailers>().GetByIdAsync(command.Id);
            if (Tailer != null)
            {
                await _unitOfWork.Repository<Models.Tailers>().DeleteAsync(Tailer);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(Tailer.Id, _localizer["Tailer Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Tailer Not Found!"]);
            }
        }
    }
}
