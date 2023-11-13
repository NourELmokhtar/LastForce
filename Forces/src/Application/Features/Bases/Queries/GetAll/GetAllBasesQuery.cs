using AutoMapper;
using Forces.Application.Enums;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services.Identity;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Bases.Queries.GetAll
{
    public class GetAllBasesQuery : IRequest<Result<List<GetAllBasesResponse>>>
    {
        public string CurrentUserID { get; set; }
        public GetAllBasesQuery(string UserID)
        {
            CurrentUserID = UserID;
        }
    }
    internal class GetAllBasesQueryHandler : IRequestHandler<GetAllBasesQuery, Result<List<GetAllBasesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public GetAllBasesQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Result<List<GetAllBasesResponse>>> Handle(GetAllBasesQuery request, CancellationToken cancellationToken)
        {
            var userTypeData = await _userService.GetCurrentUserTypeAsync(request.CurrentUserID);
            UserType userType = userTypeData.Data;
            List<Models.Bases> BasesList = new List<Models.Bases>();
            if (userType == UserType.SuperAdmin)
            {
                BasesList = await _unitOfWork.Repository<Models.Bases>().Entities.Include(x => x.Sections).Include(x => x.Force).IgnoreAutoIncludes().AsNoTracking().ToListAsync();
            }

            var ForceID = await _userService.GetForceID(request.CurrentUserID);
            var BaseID = await _userService.GetBaseID(request.CurrentUserID);
            if (ForceID.HasValue)
            {
                BasesList = await _unitOfWork.Repository<Models.Bases>().Entities.Where(b => b.ForceId == ForceID.Value).Include(x => x.Force).IgnoreAutoIncludes().AsNoTracking().ToListAsync();
            }
            if (BaseID.HasValue)
            {
                BasesList = await _unitOfWork.Repository<Models.Bases>().Entities.Where(b => b.Id == BaseID.Value).Include(x => x.Force).IgnoreAutoIncludes().AsNoTracking().ToListAsync();
            }


            var MappedBases = _mapper.Map<List<GetAllBasesResponse>>(BasesList);
            return await Result<List<GetAllBasesResponse>>.SuccessAsync(MappedBases);
        }
    }
}
