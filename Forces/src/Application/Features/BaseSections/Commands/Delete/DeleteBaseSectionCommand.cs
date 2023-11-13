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

namespace Forces.Application.Features.BaseSections.Commands.Delete
{
    public class DeleteBaseSectionCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }
    internal class DeleteBaseSectionCommandHandler : IRequestHandler<DeleteBaseSectionCommand, Result<int>>
    {
        private readonly IBaseSectionRepository _baseSectionRepository;
        private readonly IStringLocalizer<DeleteBaseSectionCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteBaseSectionCommandHandler(IBaseSectionRepository baseSectionRepository, IStringLocalizer<DeleteBaseSectionCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {
            _baseSectionRepository = baseSectionRepository;
            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteBaseSectionCommand command, CancellationToken cancellationToken)
        {
            var isBaseSectionUsed = await _baseSectionRepository.IsBaseSectionInused(command.Id);
            if (!isBaseSectionUsed)
            {
                var BaseSection = await _unitOfWork.Repository<Models.BasesSections>().GetByIdAsync(command.Id);
                if (BaseSection != null)
                {
                    await _unitOfWork.Repository<Models.BasesSections>().DeleteAsync(BaseSection);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(BaseSection.Id, _localizer["Base Section Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Base Section Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}
