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

namespace Forces.Application.Features.DepoDepartment.Queries.GetAll
{
    public class GetAllDepoDepartmentQuery : IRequest<Result<List<GetAllDepoDepartmentsResponse>>>
    {
        public string UserId { get; set; }
        public GetAllDepoDepartmentQuery()
        {

        }
    }
    internal class GetAllDepoDepartmentQueryHandler : IRequestHandler<GetAllDepoDepartmentQuery, Result<List<GetAllDepoDepartmentsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public GetAllDepoDepartmentQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Result<List<GetAllDepoDepartmentsResponse>>> Handle(GetAllDepoDepartmentQuery request, CancellationToken cancellationToken)
        {
            var ForceID = await _userService.GetForceID(request.UserId);
            var DepoList = await _unitOfWork.Repository<Application.Models.DepoDepartment>().GetAllAsync();
            if (ForceID.HasValue)
            {
                DepoList = DepoList.Where(x => x.ForceID == ForceID.Value).ToList();
            }

            var MappedDepos = _mapper.Map<List<GetAllDepoDepartmentsResponse>>(DepoList);
            return await Result<List<GetAllDepoDepartmentsResponse>>.SuccessAsync(MappedDepos);
        }
    }
}
