using AutoMapper;
using Forces.Application.Enums;
using Forces.Application.Extensions;
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

namespace Forces.Application.Features.AirCraft.Queries.GetAll
{
    public class GetAllAirCraftQuery : IRequest<Result<List<GetAllAirCraftResponse>>>
    {
        public string CurrentUserID { get; set; }
        public GetAllAirCraftQuery(string UserID)
        {
            CurrentUserID = UserID;
        }
    }
    internal class GetAllAirCraftQueryHandler : IRequestHandler<GetAllAirCraftQuery, Result<List<GetAllAirCraftResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public GetAllAirCraftQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }
        //AirCraft
        public async Task<Result<List<GetAllAirCraftResponse>>> Handle(GetAllAirCraftQuery request, CancellationToken cancellationToken)
        {
            var userTypeData = await _userService.GetCurrentUserTypeAsync(request.CurrentUserID);
            UserType userType = userTypeData.Data;
            List<Models.AirCraft> AirCraftList = new List<Models.AirCraft>();
            Expression<Func<Models.AirCraft, bool>> Criteria = x => x.Id != 0;
            if (userType == UserType.SuperAdmin)
            {
                AirCraftList = await _unitOfWork.Repository<Models.AirCraft>().Entities.Include(x=>x.BaseSection).Include(x=>x.Bases).ThenInclude(x=>x.Force).Include(x => x.AirKind).AsNoTracking().ToListAsync();
            }
            if (userType == UserType.ForceAdmin || userType == UserType.BaseAdmin)
            {
                var ForceId = await _userService.GetForceID(request.CurrentUserID);
                var BaseId = await _userService.GetBaseID(request.CurrentUserID);
                if (ForceId.HasValue)
                {
                    Criteria = Criteria.And(x => x.Bases.ForceId == ForceId);
                }
                if (BaseId.HasValue)
                {
                    Criteria = Criteria.And(x => x.BaseId == BaseId);
                }
                AirCraftList = await _unitOfWork.Repository<Models.AirCraft>().Entities.Where(Criteria).Include(x => x.BaseSection).Include(x => x.Bases).Include(x => x.AirKind).AsNoTracking().ToListAsync();

            }
            var MappedAirCrafs = _mapper.Map<List<GetAllAirCraftResponse>>(AirCraftList);
            return await Result<List<GetAllAirCraftResponse>>.SuccessAsync(MappedAirCrafs);
        }
    }
}
