using AutoMapper;
using Forces.Application.Features.Office.Queries.GetAllBySpecifics;
using Forces.Application.Interfaces.Repositories;
using Forces.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forces.Application.Features.Inventory.Queries.GetBySpecifics
{
    public class GetInventoryByQuery : IRequest<IResult<List<GetInventoryByResponse>>>
    {
        public int Id { get; set; }
    }

    internal class GetInventoryByQueryHandler : IRequestHandler<GetInventoryByQuery, IResult<List<GetInventoryByResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetInventoryByQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IResult<List<GetInventoryByResponse>>> Handle(GetInventoryByQuery request, CancellationToken cancellationToken)
        {
            var Inventory = await _unitOfWork.Repository<Models.Inventory>().GetByIdAsync(request.Id);
            var mappedInventory = _mapper.Map<GetInventoryByResponse>(Inventory);
            return (IResult<List<GetInventoryByResponse>>)await Result<GetInventoryByResponse>.SuccessAsync(mappedInventory);
        }
    }
}
