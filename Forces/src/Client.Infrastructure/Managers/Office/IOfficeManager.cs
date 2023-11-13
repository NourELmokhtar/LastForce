using Forces.Application.Features.Office.Commands.AddEdit;
using Forces.Application.Features.Office.Queries.GetAll;
using Forces.Application.Features.Office.Queries.GetAllBySpecifics;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Office
{
    public interface IOfficeManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditOfficeCommand request);
        Task<IResult<List<GetAllOfficeResponse>>> GetAllAsync();
        Task<IResult<GetOfficeByResponse>> GetOfficeByAsync(int Id);
        Task<IResult<int>> DeleteAsync(int id);
    }
}
