using AutoMapper;
using Forces.Application.Extensions;
using Forces.Application.Features.PersonalItemOperations.Queries.Dto;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services.Identity;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.PersonalItemOperations.Queries.GetByFillter
{
    public class GetPersonalItemsOperationsByFillter : IRequest<IResult<List<PersonalItemOperationDto>>>
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public int? ItemID { get; set; }
        public bool? StorageItem { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string ItemSN { get; set; }
        public int? ForceId { get; set; }
        public int? BaseId { get; set; }
        public int? BaseSectionId { get; set; }
        public int? TailerId { get; set; }

    }
    internal class GetPersonalItemsOperationsByFillterHandler : IRequestHandler<GetPersonalItemsOperationsByFillter, IResult<List<PersonalItemOperationDto>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public GetPersonalItemsOperationsByFillterHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<IResult<List<PersonalItemOperationDto>>> Handle(GetPersonalItemsOperationsByFillter request, CancellationToken cancellationToken)
        {
            Expression<Func<Models.PersonalItemsOperation_Details, bool>> condition = x => x.Id != 0;
            if (request.DateFrom.HasValue)
            {
                condition = condition.And(x => x.OperationDate >= request.DateFrom.Value);
            }
            if (request.DateTo.HasValue)
            {
                condition = condition.And(x => x.OperationDate <= request.DateTo.Value);
            }
            if (!string.IsNullOrEmpty(request.UserName))
            {
                var user = await _userService.GetByUsaerNameAsync(request.UserName);
                condition = condition.And(x => x.UserId == user.Data.Id);
            }
            if (!string.IsNullOrEmpty(request.UserId))
            {
                condition = condition.And(x => x.UserId == request.UserId);
            }
            if (request.ItemID.HasValue)
            {
                condition = condition.And(x => x.PersonalItemId == request.ItemID.Value);
            }
            if (request.StorageItem.HasValue)
            {
                condition = condition.And(x => x.PersonalItem.StorageableItem == request.StorageItem);
            }
            if (!string.IsNullOrEmpty(request.ItemName))
            {
                condition = condition.And(x => x.PersonalItem.ItemName == request.ItemName);
            }
            if (!string.IsNullOrEmpty(request.ItemCode))
            {
                condition = condition.And(x => x.PersonalItem.ItemCode == request.ItemCode);
            }
            if (!string.IsNullOrEmpty(request.ItemSN))
            {
                condition = condition.And(x => x.PersonalItem.ItemNsn == request.ItemSN);
            }
            if (request.ForceId.HasValue)
            {
                condition = condition.And(x => x.PersonalItemsOperation_Hdr.ForceId == request.ForceId.Value);
            }
            if (request.BaseId.HasValue)
            {
                condition = condition.And(x => x.PersonalItemsOperation_Hdr.BaseId == request.BaseId.Value);
            }
            if (request.BaseSectionId.HasValue)
            {
                condition = condition.And(x => x.PersonalItemsOperation_Hdr.BaseSectionId == request.BaseSectionId.Value);
            }
            if (request.TailerId.HasValue)
            {
                condition = condition.And(x => x.TailerId == request.TailerId);
            }
            var OperationList = await _unitOfWork.Repository<Models.PersonalItemsOperation_Details>().Entities.Include(x => x.PersonalItemsOperation_Hdr).Include(x => x.PersonalItem).Where(condition).ToListAsync();
            var MappedOperationList = _mapper.Map<List<PersonalItemOperationDto>>(OperationList.Select(x => x.PersonalItemsOperation_Hdr));
            return await Result<List<PersonalItemOperationDto>>.SuccessAsync(MappedOperationList);
        }
    }
}
