using AutoMapper;
using Forces.Application.Features.PersonalItems.Queries.DTO;
using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.PersonalItems.Queries.GetAll
{
    public class GetAllPersonalItemQuery : IRequest<IResult<List<PersonalItemDto>>>
    {
        public GetAllPersonalItemQuery()
        {

        }
    }
    internal class GetAllPersonalItemQueryHandler : IRequestHandler<GetAllPersonalItemQuery, IResult<List<PersonalItemDto>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllPersonalItemQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult<List<PersonalItemDto>>> Handle(GetAllPersonalItemQuery request, CancellationToken cancellationToken)
        {
            var PersonalItems = await _unitOfWork.Repository<Models.PersonalItems>().GetAllAsync();
            var MappedPersonalItems = _mapper.Map<List<PersonalItemDto>>(PersonalItems);
            return await Result<List<PersonalItemDto>>.SuccessAsync(MappedPersonalItems);
        }
    }
}
