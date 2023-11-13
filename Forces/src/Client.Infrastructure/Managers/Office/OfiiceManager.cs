using Forces.Application.Features.Office.Commands.AddEdit;
using Forces.Application.Features.Office.Queries.GetAll;
using Forces.Application.Features.Office.Queries.GetAllBySpecifics;
using Forces.Client.Infrastructure.Extensions;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Office
{
    public class OfficeManager : IOfficeManager
    {
        private protected readonly HttpClient _httpClient;

        public OfficeManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(Routes.OfficeEndPoints.Delete(id));
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllOfficeResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.OfficeEndPoints.GetAll);
            return await response.ToResult<List<GetAllOfficeResponse>>();
        }

        public async Task<IResult<GetOfficeByResponse>> GetOfficeByAsync(int Id)
        {
            var response = await _httpClient.GetAsync(Routes.OfficeEndPoints.GetOfficeById(Id));
            return await response.ToResult<GetOfficeByResponse>();
        }



        public async Task<IResult<int>> SaveAsync(AddEditOfficeCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.OfficeEndPoints.Save, request);
            return await response.ToResult<int>();
        }

    }
}
