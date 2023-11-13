using Forces.Application.Features.House.Commands.AddEdit;
using Forces.Application.Features.House.Queries.GetAll;
using Forces.Application.Features.House.Queries.GetBySpecifics;
using Forces.Application.Features.Person.Commands.AddEdit;
using Forces.Application.Features.Person.Queries.GetAll;
using Forces.Application.Features.Person.Queries.GetBySpecifics;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Person
{
    public interface IPersonManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditPersonCommand request);
        Task<IResult<List<GetAllPersonsResponse>>> GetAllAsync();
        Task<IResult<GetPersonByResponse>> GetPersonByAsync(int Id);
        Task<IResult<int>> DeleteAsync(int id);
    }
}
