using AutoMapper;
using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.HQDepartment.Queries.GetByForceId
{
    public class GetAllHQbyForceIdQuery : IRequest<Result<List<GetAllHQbyForceIdResponse>>>
    {
        public int ForceId { get; set; }
    }
    internal class GetAllHQbyForceIdQueryHandler : IRequestHandler<GetAllHQbyForceIdQuery, Result<List<GetAllHQbyForceIdResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllHQbyForceIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllHQbyForceIdResponse>>> Handle(GetAllHQbyForceIdQuery request, CancellationToken cancellationToken)
        {
            var HQList = await _unitOfWork.Repository<Application.Models.HQDepartment>().Entities.Where(x => x.ForceID == request.ForceId).ToListAsync();
            var MappedHQ = _mapper.Map<List<GetAllHQbyForceIdResponse>>(HQList);
            return await Result<List<GetAllHQbyForceIdResponse>>.SuccessAsync(MappedHQ);
        }
    }
}
