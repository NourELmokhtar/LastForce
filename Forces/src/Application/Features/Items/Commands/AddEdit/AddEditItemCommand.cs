using AutoMapper;
using Forces.Application.Enums;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
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

namespace Forces.Application.Features.Items.Commands.AddEdit
{
    public class AddEditItemCommand : IRequest<IResult<int>>
    {
        public int Id { get; set; }
        [Required]
        public string ItemName { get; set; }
        public string ItemArName { get; set; }
        [Required]
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        [Required]
        public string ItemNsn { get; set; }
        [Required]
        public int MeasureUnitId { get; set; }
        [Required]
        public int? VoteCodesId { get; set; }
        public string VoteCode { get; set; }
        public ItemClass ItemClass { get; set; } = ItemClass.A;
        public string MadeIn { get; set; }

        public DateTime? DateOfEnter { get; set; }
        public DateTime? FirstUseDate { get; set; }
        public DateTime? EndOfServiceDate { get; set; }
        public string SerialNumber { get; set; }
    }
    internal class AddEditItemCommandHandler : IRequestHandler<AddEditItemCommand, IResult<int>>
    {
        private protected IItemRepository _ItemsRepository;
        private protected IUnitOfWork<int> _unitOfWork;
        private protected IMapper _mapper;
        private readonly IStringLocalizer<AddEditItemCommandHandler> _localizer;
        private readonly IVoteCodeService _voteCodeService;
        public AddEditItemCommandHandler(IItemRepository itemsRepository, IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditItemCommandHandler> localizer, IVoteCodeService voteCodeService)
        {
            _ItemsRepository = itemsRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _voteCodeService = voteCodeService;
        }

        public async Task<IResult<int>> Handle(AddEditItemCommand command, CancellationToken cancellationToken)
        {
            var ForceOfVoteCode = await _voteCodeService.GetCodeBy(command.VoteCodesId.Value);
            if (string.IsNullOrEmpty(command.SerialNumber) && command.ItemClass != ItemClass.C)
            {
                return await Result<int>.FailAsync(_localizer["Item Serail Number Is Required"]);
            }
            if (command.Id == 0)
            {

                if (await _ItemsRepository.IsCodeExist(command.ItemCode))
                {
                    var item = await _ItemsRepository.GetByCode(command.ItemCode);
                    var vCode = await _voteCodeService.GetCodeBy(item.VoteCodesId);
                    if (vCode.ForceId == ForceOfVoteCode.ForceId)
                    {
                        return await Result<int>.FailAsync(_localizer["Item Code: {0} is Already Exist!", command.ItemCode]);
                    }
                }
                if (await _ItemsRepository.IsNsnExist(command.ItemNsn))
                {
                    var item = await _ItemsRepository.GetByNSN(command.ItemNsn);
                    var vCode = await _voteCodeService.GetCodeBy(item.VoteCodesId);
                    if (vCode.ForceId == ForceOfVoteCode.ForceId)
                    {
                        return await Result<int>.FailAsync(_localizer["Item NSN: {0} is Already Exist!", command.ItemNsn]);
                    }

                }
                var Item = _mapper.Map<Models.Items>(command);
                await _unitOfWork.Repository<Application.Models.Items>().AddAsync(Item);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(Item.Id, _localizer["Item Added!"]);
            }
            else
            {
                var Item = _mapper.Map<Models.Items>(command);
                var dbItem = await _unitOfWork.Repository<Models.Items>().GetByIdAsync(Item.Id);
                if (dbItem != null)
                {

                    if (Item.ItemCode != dbItem.ItemCode)
                    {
                        if (await _ItemsRepository.IsCodeExist(Item.ItemCode))
                        {
                            var item = await _ItemsRepository.GetByCode(command.ItemCode);
                            var vCode = await _voteCodeService.GetCodeBy(item.VoteCodesId);
                            if (vCode.ForceId == ForceOfVoteCode.ForceId)
                            {
                                return await Result<int>.FailAsync(_localizer["Item Code: {0} is Already Exist!", command.ItemCode]);
                            }
                        }
                    }
                    if (Item.ItemNsn != dbItem.ItemNsn)
                    {
                        if (await _ItemsRepository.IsNsnExist(Item.ItemNsn))
                        {
                            var item = await _ItemsRepository.GetByNSN(command.ItemNsn);
                            var vCode = await _voteCodeService.GetCodeBy(item.VoteCodesId);
                            if (vCode.ForceId == ForceOfVoteCode.ForceId)
                            {
                                return await Result<int>.FailAsync(_localizer["Item NSN: {0} is Already Exist!", command.ItemNsn]);
                            }

                        }
                    }
                    await _unitOfWork.Repository<Models.Items>().UpdateAsync(Item);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(Item.Id, _localizer["Item Updated!"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Item Not Found!"]);
                }
            }
        }
    }
}
