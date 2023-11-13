using AutoMapper;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services.Identity;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Tailers.Queries.GetAll
{
    public class GetAllTailersQuery : IRequest<IResult<List<TailerDto>>>
    {
        public string CurrentUserID { get; set; }
        public GetAllTailersQuery(string UserID)
        {
            CurrentUserID = UserID;
        }
    }
    internal class GetAllTailersQueryHandler : IRequestHandler<GetAllTailersQuery, IResult<List<TailerDto>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public GetAllTailersQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<IResult<List<TailerDto>>> Handle(GetAllTailersQuery request, CancellationToken cancellationToken)
        {
            var CurrentUserBaseID = await _userService.GetBaseID(request.CurrentUserID);
            if (CurrentUserBaseID.HasValue)
            {
                var TailerList = await _unitOfWork.Repository<Models.Tailers>().Entities.Where(x => x.BaseId == CurrentUserBaseID).ToListAsync();
                var MappedTailers = _mapper.Map<List<TailerDto>>(TailerList);
                return await Result<List<TailerDto>>.SuccessAsync(MappedTailers);
            }
            else
            {
                var TailerList = await _unitOfWork.Repository<Models.Tailers>().GetAllAsync();
                var MappedTailers = _mapper.Map<List<TailerDto>>(TailerList);
                return await Result<List<TailerDto>>.SuccessAsync(MappedTailers);
            }

        }
    }
}
