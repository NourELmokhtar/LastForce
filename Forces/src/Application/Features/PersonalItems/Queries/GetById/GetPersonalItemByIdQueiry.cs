using AutoMapper;
using Forces.Application.Features.PersonalItems.Queries.DTO;
using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.PersonalItems.Queries.GetById
{
    public class GetPersonalItemByIdQueiry : IRequest<IResult<PersonalItemDto>>
    {
        [Required]
        public int PersonalItemId { get; set; }
    }
    internal class GetPersonalItemByIdQueiryHandler : IRequestHandler<GetPersonalItemByIdQueiry, IResult<PersonalItemDto>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetPersonalItemByIdQueiryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult<PersonalItemDto>> Handle(GetPersonalItemByIdQueiry request, CancellationToken cancellationToken)
        {
            var PersonalItem = await _unitOfWork.Repository<Models.PersonalItems>().GetByIdAsync(request.PersonalItemId);
            var MappedPersonalItem = _mapper.Map<PersonalItemDto>(PersonalItem);
            return await Result<PersonalItemDto>.SuccessAsync(MappedPersonalItem);
        }
    }
}
