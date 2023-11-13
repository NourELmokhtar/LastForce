using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.HQDepartment.Commands.Delete
{
    public class DeleteHQCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

    }
    internal class DeleteHQCommandHandler : IRequestHandler<DeleteHQCommand, Result<int>>
    {
        private readonly IHQRepository _hqRepository;
        private readonly IStringLocalizer<DeleteHQCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteHQCommandHandler(IHQRepository hqRepository, IStringLocalizer<DeleteHQCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {
            _hqRepository = hqRepository;
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteHQCommand command, CancellationToken cancellationToken)
        {
            var isHQUsed = await _hqRepository.IsHQInuse(command.Id);
            if (!isHQUsed)
            {
                var HQ = await _unitOfWork.Repository<Models.HQDepartment>().GetByIdAsync(command.Id);
                if (HQ != null)
                {
                    await _unitOfWork.Repository<Models.HQDepartment>().DeleteAsync(HQ);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(HQ.Id, _localizer["Department Deleted"]);
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
