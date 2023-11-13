using Forces.Application.Extensions;
using Forces.Application.Interfaces.Repositories;
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

namespace Forces.Application.Features.Person.Queries.GetBySpecifics
{
    public class GetPersonByQuery : IRequest<IResult<List<GetPersonByResponse>>>
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? NationalNumber { get; set; }

    }
    internal class GetPersonByQuesryHandler : IRequestHandler<GetPersonByQuery, IResult<List<GetPersonByResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetPersonByQuesryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<List<GetPersonByResponse>>> Handle(GetPersonByQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Models.Person, bool>> Condition = x => x.Id != 0;
            if (!string.IsNullOrEmpty(request.NationalNumber))
            {
                Condition = Condition.And(x => x.NationalNumber == request.NationalNumber);
            }
            var PersonList = await _unitOfWork.Repository<Models.Person>().Entities.Where(Condition).ToListAsync();
            var PersonListResponse = PersonList.Select(x => new GetPersonByResponse()
            {
                Id = x.Id,
                Name = x.Name,
                NationalNumber = x.NationalNumber,
                RoomId = x.RoomId,
                OfficePhone = x.OfficePhone,
                Phone = x.Phone,
                Section = x.Section,
            }).ToList();
            return await Result<List<GetPersonByResponse>>.SuccessAsync(PersonListResponse);
        }
    }
}
