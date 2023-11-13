using Forces.Application.Features.Color.Queries.GetAll;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
using Forces.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Office.Queries.GetAll
{
    public class GetAllOfficeQuery : IRequest<IResult<List<GetAllOfficeResponse>>>
    {
        public GetAllOfficeQuery()
        {

        }
    }
    internal class GetAllOfficeQueryHandler : IRequestHandler<GetAllOfficeQuery, IResult<List<GetAllOfficeResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IVoteCodeService _voteCodeService;

        public GetAllOfficeQueryHandler(IUnitOfWork<int> unitOfWork, IVoteCodeService voteCodeService)
        {
            _unitOfWork = unitOfWork;
            _voteCodeService = voteCodeService;
        }

        public async Task<IResult<List<GetAllOfficeResponse>>> Handle(GetAllOfficeQuery request, CancellationToken cancellationToken)
        {
            var Offices = await _unitOfWork.Repository<Models.Office>().GetAllAsync();
            var MappedOffices = Offices.Select(x => new GetAllOfficeResponse()
            {
                OfficeName = x.Name,
                Id = x.Id
            }).ToList();
            return await Result<List<GetAllOfficeResponse>>.SuccessAsync(MappedOffices);
        }
    }
}
