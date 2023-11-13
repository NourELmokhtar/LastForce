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

namespace Forces.Application.Features.Forces.Queries.GetAll
{
    public class GetAllForcesQuery : IRequest<Result<List<GetAllForcesResponse>>>
    {
        public string CurrentUserID { get; set; }
        public GetAllForcesQuery(string UserID)
        {
            CurrentUserID = UserID;
        }
    }
    internal class GetAllForcesQueryHandler : IRequestHandler<GetAllForcesQuery, Result<List<GetAllForcesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public GetAllForcesQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Result<List<GetAllForcesResponse>>> Handle(GetAllForcesQuery request, CancellationToken cancellationToken)
        {
            var userTypeResponse = await _userService.GetCurrentUserTypeAsync(request.CurrentUserID);
            var userType = userTypeResponse.Data;
            List<Models.Forces> ForceList = new List<Models.Forces>();

            if (userType == UserType.SuperAdmin)
            {
                ForceList = await _unitOfWork.Repository<Application.Models.Forces>().GetAllAsync();
                var MappedForcess = _mapper.Map<List<GetAllForcesResponse>>(ForceList);
                return await Result<List<GetAllForcesResponse>>.SuccessAsync(MappedForcess);
            }
            var ForceID = await _userService.GetForceID(request.CurrentUserID);
            Expression<Func<Models.Forces, bool>> ForceCreteria = x => x.Id != 0;
            if (ForceID.HasValue)
            {
                ForceCreteria = ForceCreteria.And(x => x.Id == ForceID.Value);
                ForceList = await _unitOfWork.Repository<Application.Models.Forces>().Entities.Where(ForceCreteria).ToListAsync();
            }
            ForceList = await _unitOfWork.Repository<Application.Models.Forces>().GetAllAsync();
            var MappedForces = _mapper.Map<List<GetAllForcesResponse>>(ForceList);
            return await Result<List<GetAllForcesResponse>>.SuccessAsync(MappedForces);
        }
    }
}
