using Forces.Application.Features.SectionStore.Commands.AddEdit;
using Forces.Application.Features.SectionStore.Queries.GetAll;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Forces.Client.Infrastructure.Routes;
using Forces.Client.Infrastructure.Extensions;
using System.Net.Http.Json;

namespace Forces.Client.Infrastructure.Managers.StoreSection
{
    public class StoreSectionManager : IStoreSectionManager
    {
        private protected readonly HttpClient _httpClient;

        public StoreSectionManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int Id)
        {
            var Response = await _httpClient.DeleteAsync(SectionStoreEndpoints.Delete(Id));
            return await Response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllSectionStoreResponse>>> GetAllAsync()
        {
            var Response = await _httpClient.GetAsync(SectionStoreEndpoints.GetAll);
            return await Response.ToResult<List<GetAllSectionStoreResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditSectionStoreCommand command)
        {
            var Response = await _httpClient.PostAsJsonAsync(SectionStoreEndpoints.Save, command);
            return await Response.ToResult<int>();
        }
    }
}
