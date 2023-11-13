using AutoMapper;
using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Color.Queries.GetAll
{
    public class GetAllColorQuery : IRequest<IResult<List<GetAllColorResponse>>>
    {

        public GetAllColorQuery()
        {

        }

    }
    internal class GetAllColorQueryHandler : IRequestHandler<GetAllColorQuery, IResult<List<GetAllColorResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        public GetAllColorQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public async Task<IResult<List<GetAllColorResponse>>> Handle(GetAllColorQuery request, CancellationToken cancellationToken)
        {
            var Colorss = await _unitOfWork.Repository<Models.Color>().GetAllAsync();
            var MappedColors = Colorss.Select(x => new GetAllColorResponse()
            {
                ColorName = x.ColorName,
                Id = x.Id
            }).ToList();
            return await Result<List<GetAllColorResponse>>.SuccessAsync(MappedColors);
        }
    }
}
