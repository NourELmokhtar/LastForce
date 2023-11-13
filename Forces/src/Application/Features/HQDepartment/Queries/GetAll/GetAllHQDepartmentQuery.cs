using AutoMapper;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services.Identity;
using Forces.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.HQDepartment.Queries.GetAll
{
    public class GetAllHQDepartmentQuery : IRequest<Result<List<GetAllHQDepartmentsResponse>>>
    {
        public string UserId { get; set; }
        public GetAllHQDepartmentQuery()
        {
        }
    }
    internal class GetAllHQDepartmentQueryHandler : IRequestHandler<GetAllHQDepartmentQuery, Result<List<GetAllHQDepartmentsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public GetAllHQDepartmentQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Result<List<GetAllHQDepartmentsResponse>>> Handle(GetAllHQDepartmentQuery request, CancellationToken cancellationToken)
        {
            var ForceID = await _userService.GetForceID(request.UserId);
            var HQList = await _unitOfWork.Repository<Application.Models.HQDepartment>().GetAllAsync();
            if (ForceID.HasValue)
            {
                HQList = HQList.Where(x => x.ForceID == ForceID.Value).ToList();
            }
            var MappedHQ = _mapper.Map<List<GetAllHQDepartmentsResponse>>(HQList);
            return await Result<List<GetAllHQDepartmentsResponse>>.SuccessAsync(MappedHQ);
        }
    }
}
