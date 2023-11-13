using Forces.Application.Features.BaseSections.Commands.AddEdit;
using Forces.Application.Features.BaseSections.Queries.GetAll;
using Forces.Application.Features.BaseSections.Queries.GetByBaseId;
using Forces.Application.Features.BaseSections.Queries.GetById;
using Forces.Client.Infrastructure.Extensions;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.BasicInformation.BaseSections
{
    public class BaseSectionManager : IBaseSectionManager
    {
        private readonly HttpClient _httpClient;

        public BaseSectionManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(Routes.BaseSectionEndpoints.Delete(id));
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllBasesSectionsQueryResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.BaseSectionEndpoints.GetAllSections);
            return await response.ToResult<List<GetAllBasesSectionsQueryResponse>>();
        }

        public async Task<IResult<GetBaseSectionByIdQueryResponse>> GetBaseSectionByIdAsync(int Id)
        {
            var response = await _httpClient.GetAsync(Routes.BaseSectionEndpoints.GetBaseSectionById(Id));
            return await response.ToResult<GetBaseSectionByIdQueryResponse>();
        }

        public async Task<IResult<List<GetAllSectionsByBaseIdQueryResponse>>> GetSectionsByBaseIdAsync(int BaseId)
        {
            var response = await _httpClient.GetAsync(Routes.BaseSectionEndpoints.GetSectionByBaseId(BaseId));
            return await response.ToResult<List<GetAllSectionsByBaseIdQueryResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditBaseSectionCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.BaseSectionEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}
