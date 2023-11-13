using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.DepoDepartment.Commands.Delete
{
    public class DeleteDepoCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

    }
    internal class DeleteDepoCommandHandler : IRequestHandler<DeleteDepoCommand, Result<int>>
    {
        private readonly IDepoRepository _depoRepository;
        private readonly IStringLocalizer<DeleteDepoCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteDepoCommandHandler(IDepoRepository forceRepository, IStringLocalizer<DeleteDepoCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {
            _depoRepository = forceRepository;
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteDepoCommand command, CancellationToken cancellationToken)
        {
            var isDepoUsed = await _depoRepository.IsDepoInuse(command.Id);
            if (!isDepoUsed)
            {
                var depo = await _unitOfWork.Repository<Models.DepoDepartment>().GetByIdAsync(command.Id);
                if (depo != null)
                {
                    await _unitOfWork.Repository<Models.DepoDepartment>().DeleteAsync(depo);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(depo.Id, _localizer["Department Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Department Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}
