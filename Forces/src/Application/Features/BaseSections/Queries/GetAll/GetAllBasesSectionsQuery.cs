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

namespace Forces.Application.Features.BaseSections.Queries.GetAll
{
    public class GetAllBasesSectionsQuery : IRequest<Result<List<GetAllBasesSectionsQueryResponse>>>
    {
        public string CurrentUserID { get; set; }
        public GetAllBasesSectionsQuery(string UserID)
        {
            CurrentUserID = UserID;
        }
    }
    internal class GetAllBasesSectionsQueryHandler : IRequestHandler<GetAllBasesSectionsQuery, Result<List<GetAllBasesSectionsQueryResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public GetAllBasesSectionsQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Result<List<GetAllBasesSectionsQueryResponse>>> Handle(GetAllBasesSectionsQuery request, CancellationToken cancellationToken)
        {
            var userTypeData = await _userService.GetCurrentUserTypeAsync(request.CurrentUserID);
            UserType userType = userTypeData.Data;
            List<Models.BasesSections> BasesSectinList = new List<Models.BasesSections>();
            Expression<Func<Models.BasesSections, bool>> Criteria = x => x.Id != 0;
            if (userType == UserType.SuperAdmin)
            {
                BasesSectinList = await _unitOfWork.Repository<Models.BasesSections>().Entities.Include(x => x.Base).AsNoTracking().ToListAsync();
            }
            if (userType == UserType.ForceAdmin || userType == UserType.BaseAdmin)
            {
                var ForceID = await _userService.GetForceID(request.CurrentUserID);
                var BaseID = await _userService.GetBaseID(request.CurrentUserID);
                if (ForceID.HasValue)
                {
                    Criteria = Criteria.And(x => x.Base.ForceId == ForceID);
                }
                if (BaseID.HasValue)
                {
                    Criteria = Criteria.And(x => x.BaseId == BaseID);
                }
                BasesSectinList = await _unitOfWork.Repository<Models.BasesSections>().Entities.Where(Criteria).Include(x => x.Base).AsNoTracking().ToListAsync();

            }

            var MappedBasesSections = BasesSectinList.Select(x =>

                                new GetAllBasesSectionsQueryResponse()
                                {
                                    BaseId = x.BaseId,
                                    ForceId = x.Base.ForceId,
                                    Id = x.Id,
                                    SectionCode = x.SectionCode,
                                    SectionName = x.SectionName
                                }
            ).ToList();
            return await Result<List<GetAllBasesSectionsQueryResponse>>.SuccessAsync(MappedBasesSections);
        }
    }
}