using Forces.Application.Features.Building.Commands.AddEdit;
using Forces.Application.Features.Building.Queries.GetAll;
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

namespace Forces.Client.Infrastructure.Managers.Building
{
    public class BuildingManager : IBuildingManager 
    {
        private protected readonly HttpClient _httpClient;
        public BuildingManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int Id)
        {
            var Response = await _httpClient.DeleteAsync(BuildingEndPoints.Delete(Id));
            return await Response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllBuildingsResponse>>> GetAllAsync()
        {
            var Response = await _httpClient.GetAsync(BuildingEndPoints.GetAll);
            return await Response.ToResult<List<GetAllBuildingsResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditBuildingCommand command)
        {
            var Response = await _httpClient.PostAsJsonAsync(BuildingEndPoints.Save, command); ;
            return await Response.ToResult<int>();
        }
    }
}
