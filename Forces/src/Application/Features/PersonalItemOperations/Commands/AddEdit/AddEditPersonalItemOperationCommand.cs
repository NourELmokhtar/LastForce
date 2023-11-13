using AutoMapper;
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

namespace Forces.Application.Features.PersonalItemOperations.Commands.AddEdit
{
    public class AddEditPersonalItemOperationCommand : IRequest<IResult<int>>
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OperationDate { get; set; }
        public decimal Total { get; set; }
        public int ForceId { get; set; }
        public int? BaseId { get; set; }
        public int? BaseSectionId { get; set; }
        public List<OperationDetailsDto> Details { get; set; }
    }
    internal class AddEditPersonalItemOperationCommandHandler : IRequestHandler<AddEditPersonalItemOperationCommand, IResult<int>>
    {
        private protected IUnitOfWork<int> _unitOfWork;
        private protected IMapper _mapper;
        private readonly IStringLocalizer<AddEditPersonalItemOperationCommandHandler> _localizer;

        public AddEditPersonalItemOperationCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditPersonalItemOperationCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IResult<int>> Handle(AddEditPersonalItemOperationCommand command, CancellationToken cancellationToken)
        {
            var PersonalItemOhdr = _mapper.Map<Models.PersonalItemsOperation_Hdr>(command);
            var Items = await _unitOfWork.Repository<Models.PersonalItems>().GetAllAsync();
            foreach (var operation in PersonalItemOhdr.Details)
            {
                var personalItem = Items.FirstOrDefault(x => x.Id == operation.PersonalItemId);
                operation.TailerId = personalItem.TailerId;
            }
            if (command.Id == 0)
            {
                if (command.Details.Count == 0)
                {
                    return await Result<int>.FailAsync(_localizer["Items List Can not Be Empty!"]);
                }

                await _unitOfWork.Repository<Application.Models.PersonalItemsOperation_Hdr>().AddAsync(PersonalItemOhdr);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(PersonalItemOhdr.Id, _localizer["Operation Submitted Successfully!"]);
            }
            else
            {
                var dbPersonalItemOhdr = await _unitOfWork.Repository<Models.PersonalItemsOperation_Hdr>().GetByIdAsync(command.Id);
                if (dbPersonalItemOhdr != null)
                {
                    await _unitOfWork.Repository<Models.PersonalItemsOperation_Hdr>().UpdateAsync(PersonalItemOhdr);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(PersonalItemOhdr.Id, _localizer["Operation Updated!"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Operation Not Found!"]);
                }
            }

        }
    }
}
