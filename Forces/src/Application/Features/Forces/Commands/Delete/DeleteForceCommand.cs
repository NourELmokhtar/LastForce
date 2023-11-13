using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Forces.Commands.Delete
{
    public class DeleteForceCommand : IRequest<Result<int>>
    {
        [Required]
        public int Id { get; set; }

    }
    internal class DeleteForceCommandHandler : IRequestHandler<DeleteForceCommand, Result<int>>
    {
        private readonly IForceRepository _forceRepository;
        private readonly IStringLocalizer<DeleteForceCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteForceCommandHandler(IForceRepository forceRepository, IStringLocalizer<DeleteForceCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {
            _forceRepository = forceRepository;
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteForceCommand command, CancellationToken cancellationToken)
        {
            var isForceUsed = await _forceRepository.IsForceInused(command.Id);
            if (!isForceUsed)
            {
                var force = await _unitOfWork.Repository<Models.Forces>().GetByIdAsync(command.Id);
                if (force != null)
                {
                    await _unitOfWork.Repository<Models.Forces>().DeleteAsync(force);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(force.Id, _localizer["Force Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Force Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}
