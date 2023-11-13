using AutoMapper;
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

namespace Forces.Application.Features.Bases.Queries.GetByForceId
{
    public class GetAllBasesByForceIdQuery : IRequest<Result<List<GetAllBasesByForceIdResponse>>>
    {
        public int Id { get; set; }
        public string CurrentUserID { get; set; }
    }
    internal class GetAllBasesByForceIdQueryHandler : IRequestHandler<GetAllBasesByForceIdQuery, Result<List<GetAllBasesByForceIdResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public GetAllBasesByForceIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Result<List<GetAllBasesByForceIdResponse>>> Handle(GetAllBasesByForceIdQuery request, CancellationToken cancellationToken)
        {
            var ForceID = await _userService.GetForceID(request.CurrentUserID);
            var BaseID = await _userService.GetBaseID(request.CurrentUserID);
            List<Models.Bases> BasesList = new List<Models.Bases>();
            Expression<Func<Models.Bases, bool>> Creteria = x => x.Id != 0;
            if (ForceID.HasValue)
            {
                Creteria = Creteria.And(x => x.ForceId == ForceID.Value);
            }
            if (BaseID.HasValue)
            {
                Creteria = Creteria.And(x => x.Id == BaseID.Value);
            }
            BasesList = await _unitOfWork.Repository<Models.Bases>().Entities.Where(x => x.ForceId == request.Id).Where(Creteria).ToListAsync();
            var MappedBases = _mapper.Map<List<GetAllBasesByForceIdResponse>>(BasesList);
            return await Result<List<GetAllBasesByForceIdResponse>>.SuccessAsync(MappedBases);
        }
    }
}
