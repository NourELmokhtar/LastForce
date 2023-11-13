using Forces.Application.Features.SectionStore.Commands.Delete;
using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.SectionStore.Commands.Delete
{
    public class DeleteSectionStoreCommand : IRequest<Result<int>>
    {
        public int StoreCode { get; set; }
    }
    internal class DeleteSectionStoreCommandHandler : IRequestHandler<DeleteSectionStoreCommand, Result<int>>
    {

        private readonly IStringLocalizer<DeleteSectionStoreCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteSectionStoreCommandHandler(IStringLocalizer<DeleteSectionStoreCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {

            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(DeleteSectionStoreCommand command, CancellationToken cancellationToken)
        {
            var isSectionStoreUsed = await _unitOfWork.Repository<Models.RackStore>().Entities.AnyAsync(x => x.StoreId == command.StoreCode);
            if (!isSectionStoreUsed)
            {
                var store = await _unitOfWork.Repository<Models.SectionStore>().GetByIdAsync(command.StoreCode);
                if (store != null)
                {
                    await _unitOfWork.Repository<Models.SectionStore>().DeleteAsync(store);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(store.Id, _localizer["Store Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["StoreNot Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["This Store Inuse,Deletion Not Allowed"]);
            }
        }
    }
}
