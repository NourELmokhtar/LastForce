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

namespace Forces.Application.Features.DepoDepartment.Queries.GetByForceId
{
    public class GetAllDepoByForceIdQuery : IRequest<Result<List<GetAllDepoByForceIdResponse>>>
    {
        public int ForceId { get; set; }
    }
    internal class GetAllDepoByForceIdQueryHandler : IRequestHandler<GetAllDepoByForceIdQuery, Result<List<GetAllDepoByForceIdResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllDepoByForceIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllDepoByForceIdResponse>>> Handle(GetAllDepoByForceIdQuery request, CancellationToken cancellationToken)
        {
            var DepoList = await _unitOfWork.Repository<Application.Models.DepoDepartment>().Entities.Where(x => x.ForceID == request.ForceId).ToListAsync();
            var MappedDepo = _mapper.Map<List<GetAllDepoByForceIdResponse>>(DepoList);
            return await Result<List<GetAllDepoByForceIdResponse>>.SuccessAsync(MappedDepo);
        }
    }
}
