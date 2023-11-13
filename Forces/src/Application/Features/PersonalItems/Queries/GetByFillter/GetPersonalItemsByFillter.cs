using AutoMapper;
using Forces.Application.Extensions;
using Forces.Application.Features.PersonalItems.Queries.DTO;
using Forces.Application.Interfaces.Repositories;
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

namespace Forces.Application.Features.PersonalItems.Queries.GetByFillter
{
    public class GetPersonalItemsByFillter : IRequest<IResult<List<PersonalItemDto>>>
    {

        public string ItemName { get; set; }
        public string ItemArName { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string ItemNsn { get; set; }
    }
    internal class GetPersonalItemsByFillterQueryHandler : IRequestHandler<GetPersonalItemsByFillter, IResult<List<PersonalItemDto>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetPersonalItemsByFillterQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult<List<PersonalItemDto>>> Handle(GetPersonalItemsByFillter request, CancellationToken cancellationToken)
        {
            Expression<Func<Models.PersonalItems, bool>> Condition = x => x.Id != 0;
            if (!string.IsNullOrEmpty(request.ItemArName))
            {
                Condition = Condition.And(x => x.ItemArName == request.ItemArName);
            }
            if (!string.IsNullOrEmpty(request.ItemCode))
            {
                Condition = Condition.And(x => x.ItemCode == request.ItemCode);
            }
            if (!string.IsNullOrEmpty(request.ItemDescription))
            {
                Condition = Condition.And(x => x.ItemDescription == request.ItemDescription);
            }
            if (!string.IsNullOrEmpty(request.ItemName))
            {
                Condition = Condition.And(x => x.ItemName == request.ItemName);
            }
            if (!string.IsNullOrEmpty(request.ItemNsn))
            {
                Condition = Condition.And(x => x.ItemNsn == request.ItemNsn);
            }
            var PersonalItems = await _unitOfWork.Repository<Models.PersonalItems>().Entities.Where(Condition).ToListAsync();
            var MappedPersonalItems = _mapper.Map<List<PersonalItemDto>>(PersonalItems);
            return await Result<List<PersonalItemDto>>.SuccessAsync(MappedPersonalItems);

        }
    }
}
