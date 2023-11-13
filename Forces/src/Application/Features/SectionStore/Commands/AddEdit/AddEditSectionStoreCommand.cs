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

namespace Forces.Application.Features.SectionStore.Commands.AddEdit
{
    public class AddEditSectionStoreCommand : IRequest<Result<int>>
    {
        public int StoreId { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public int SectionId { get; set; }
    }
    internal class AddEditSectionStoreCommandHandler : IRequestHandler<AddEditSectionStoreCommand, Result<int>>
    {

        private readonly IStringLocalizer<AddEditSectionStoreCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditSectionStoreCommandHandler(IStringLocalizer<AddEditSectionStoreCommandHandler> localizer, IUnitOfWork<int> unitOfWork)
        {

            _localizer = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(AddEditSectionStoreCommand request, CancellationToken cancellationToken)
        {
            if (request.StoreId == 0)
            {
                var ExistSectionStore = await _unitOfWork.Repository<Models.SectionStore>().Entities.FirstOrDefaultAsync(x => x.StoreName == request.StoreName);
                if (ExistSectionStore != null)
                {
                    return await Result<int>.FailAsync(_localizer["This Store Is Already Exist!"]);
                }
                else
                {
                    Models.SectionStore store = new Models.SectionStore()
                    {
                        StoreName = request.StoreName,
                        SectionId = request.SectionId,
                        StoreCode = request.StoreCode,

                    };
                    await _unitOfWork.Repository<Models.SectionStore>().AddAsync(store);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(store.Id, _localizer["Store Added Successfuly!"]);
                }
            }
            else
            {
                var ExistSectionStore = await _unitOfWork.Repository<Models.SectionStore>().Entities.FirstOrDefaultAsync(x => x.Id == request.StoreId);
                if (ExistSectionStore == null)
                {
                    return await Result<int>.FailAsync(_localizer["Store Not Found!!"]);
                }
                else
                {
                    var ExistnameStore = await _unitOfWork.Repository<Models.SectionStore>().Entities.FirstOrDefaultAsync(x => x.StoreName == request.StoreName && x.Id != request.StoreId);
                    if (ExistnameStore != null)
                    {
                        return await Result<int>.FailAsync(_localizer["This Store Is Already Exist!"]);
                    }
                    else
                    {
                        ExistSectionStore.StoreName = request.StoreName;
                        ExistSectionStore.SectionId = request.SectionId;
                        ExistSectionStore.StoreCode = request.StoreCode;
                        await _unitOfWork.Repository<Models.SectionStore>().UpdateAsync(ExistSectionStore);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(ExistSectionStore.Id, _localizer["Store Updated Successfuly!"]);
                    }
                }
            }
        }
    }
}
