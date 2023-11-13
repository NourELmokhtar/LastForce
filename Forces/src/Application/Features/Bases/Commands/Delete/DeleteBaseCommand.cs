using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Bases.Commands.Delete
{
    public class DeleteBaseCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

    }
    internal class DeleteBaseCommandHandler : IRequestHandler<DeleteBaseCommand, Result<int>>
    {
        private readonly IBaseRepository _baseRepository;
        private readonly IStringLocalizer<DeleteBaseCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteBaseCommandHandler(IBaseRepository baseRepository, IStringLocalizer<DeleteBaseCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {
            _baseRepository = baseRepository;
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteBaseCommand command, CancellationToken cancellationToken)
        {
            var isBaseUsed = await _baseRepository.IsBaseInused(command.Id);
            if (!isBaseUsed)
            {
                var Base = await _unitOfWork.Repository<Models.Bases>().GetByIdAsync(command.Id);
                if (Base != null)
                {
                    await _unitOfWork.Repository<Models.Bases>().DeleteAsync(Base);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(Base.Id, _localizer["Base Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Base Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}
