using Forces.Application.Features.Color.Commands.AddEdit;
using Forces.Application.Features.Color.Queries.GetAll;
using Forces.Client.Infrastructure.Extensions;
using Forces.Client.Infrastructure.Routes;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Color
{
    public class ColorManager : IColorManager
    {
        private protected readonly HttpClient _httpClient;

        public ColorManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int Id)
        {
            var Response = await _httpClient.DeleteAsync(ColorEndpoints.Delete(Id));
            return await Response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllColorResponse>>> GetAllAsync()
        {
            var Response = await _httpClient.GetAsync(ColorEndpoints.GetAll);
            return await Response.ToResult<List<GetAllColorResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditColorCommand command)
        {
            var Response = await _httpClient.PostAsJsonAsync(ColorEndpoints.Save, command);
            return await Response.ToResult<int>();
        }
    }
}
