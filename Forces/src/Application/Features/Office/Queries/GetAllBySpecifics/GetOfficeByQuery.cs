using AutoMapper;
using Forces.Application.Features.Brands.Queries.GetById;
using Forces.Application.Features.Building.Queries.GetBySpecifics;
using Forces.Application.Interfaces.Repositories;
using Forces.Domain.Entities.Catalog;
using Forces.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Forces.Application.Features.Office.Queries.GetAllBySpecifics
{
    public class GetOfficeByQuery : IRequest<IResult<List<GetOfficeByResponse>>>
    {
        public int Id { get; set; }
    }
    internal class GetAllOfficeQuesryHandler : IRequestHandler<GetOfficeByQuery, IResult<List<GetOfficeByResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllOfficeQuesryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult<List<GetOfficeByResponse>>> Handle(GetOfficeByQuery request, CancellationToken cancellationToken)
        {
            var office = await _unitOfWork.Repository<Models.Office>().GetByIdAsync(request.Id);
            var mappedoffice = _mapper.Map<GetOfficeByResponse>(office);
            return (IResult<List<GetOfficeByResponse>>)await Result<GetOfficeByResponse>.SuccessAsync(mappedoffice);
        }
    }
}
