using AutoMapper;
using Forces.Application.Enums;
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

namespace Forces.Application.Features.PersonalItems.Commands.AddEdit
{
    public class AddEditPersonalItemCommand : IRequest<IResult<int>>
    {
        public int Id { get; set; }
        [Required]
        public string ItemName { get; set; }
        public string ItemArName { get; set; }
        [Required]
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string ItemNsn { get; set; }
        [Required]
        public UsagePeriodUnit UsagePeriodUnit { get; set; }
        public int? UsagePeriod { get; set; }
        [Required]
        public bool StorageableItem { get; set; }
        public decimal? ItemPrice { get; set; }
        public int? MaxQtyOnPeriod { get; set; }
        public int? TailerId { get; set; }
        public int BaseId { get; set; }

    }
    internal class AddEditPersonalItemCommandHandler : IRequestHandler<AddEditPersonalItemCommand, IResult<int>>
    {
        private protected IUnitOfWork<int> _unitOfWork;
        private protected IMapper _mapper;
        private readonly IStringLocalizer<AddEditPersonalItemCommandHandler> _localizer;

        public AddEditPersonalItemCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditPersonalItemCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<IResult<int>> Handle(AddEditPersonalItemCommand command, CancellationToken cancellationToken)
        {
            var PersonalItem = _mapper.Map<Models.PersonalItems>(command);
            if (command.Id == 0)
            {
                if (!command.StorageableItem && command.TailerId == null)
                {
                    return await Result<int>.FailAsync(_localizer["Please Select A Tailer For This Item!"]);
                }
                await _unitOfWork.Repository<Application.Models.PersonalItems>().AddAsync(PersonalItem);
                await _unitOfWork.Commit(cancellationToken);

                return await Result<int>.SuccessAsync(PersonalItem.Id, _localizer["Personal Item Added!"]);
            }
            else
            {
                var dbPersonalItem = await _unitOfWork.Repository<Models.PersonalItems>().GetByIdAsync(PersonalItem.Id);
                if (dbPersonalItem != null)
                {
                    await _unitOfWork.Repository<Models.PersonalItems>().UpdateAsync(PersonalItem);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(PersonalItem.Id, _localizer["Personal Item Updated!"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Personal Item Not Found!"]);
                }
            }

        }
    }
}
