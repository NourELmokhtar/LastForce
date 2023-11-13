using Forces.Application.Enums;
using Forces.Application.Features.MPRDashboard.Query.GetMPRData;
using Forces.Application.Requests.Requests;
using Forces.Application.Requests.Requests.NPRRequest;
using Forces.Application.Responses.Requets.MPR;
using Forces.Client.Infrastructure.Extensions;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Requests.NPR
{
    public class NprManager : INprManager
    {
        private readonly HttpClient _httpClient;

        public NprManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<GetAllMPRResponse>> GetAllRequestsById(int requestId)
        {
            var response = await _httpClient.GetAsync(Routes.RequestsEndpoints.GetAllRequestsById(requestId));
            return await response.ToResult<GetAllMPRResponse>();
        }

        public async Task<IResult<GetAllMPRResponse>> GetAllRequestsByRefrance(string Refrance)
        {
            var response = await _httpClient.GetAsync(Routes.RequestsEndpoints.GetAllRequestsByRefrance(Refrance));
            return await response.ToResult<GetAllMPRResponse>();
        }

        public async Task<IResult<List<GetAllMPRResponse>>> GetAllRequestsBySpecifications(GetRequestsBySpecificationsRequest Specifications)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.RequestsEndpoints.GetAllRequestsBySpecifications, Specifications);
            return await response.ToResult<List<GetAllMPRResponse>>();
        }

        public async Task<IResult<List<GetAllMPRResponse>>> GetAllRequestsBySteps(RequestSteps Step)
        {
            var response = await _httpClient.GetAsync(Routes.RequestsEndpoints.GetAllRequestsBySteps(Step));
            return await response.ToResult<List<GetAllMPRResponse>>();
        }

        public async Task<IResult<List<GetAllMPRResponse>>> GetAllRequestsByUser()
        {
            var response = await _httpClient.GetAsync(Routes.RequestsEndpoints.GetAllRequestsByUser);
            return await response.ToResult<List<GetAllMPRResponse>>();
        }

        public async Task<IResult<int>> GetAllRequestsCount(GetRequestsBySpecificationsRequest Specifications)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.RequestsEndpoints.GetAllRequestsCount, Specifications);
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllMPRResponse>>> GetAllRequestsForTargetUser()
        {
            var response = await _httpClient.GetAsync(Routes.RequestsEndpoints.GetAllRequestsForTargetUser);
            return await response.ToResult<List<GetAllMPRResponse>>();
        }

        public async Task<IResult<List<GetAllMPRResponse>>> GetAllRequestsToLog()
        {
            var response = await _httpClient.GetAsync(Routes.RequestsEndpoints.GetAllRequestsToLog);
            return await response.ToResult<List<GetAllMPRResponse>>();
        }

        public async Task<IResult<List<ActionsType>>> GetAvilableActions()
        {
            var response = await _httpClient.GetAsync(Routes.RequestsEndpoints.GetAvilableActions);
            return await response.ToResult<List<ActionsType>>();
        }

        public async Task<IResult<GetMPRDashboardDataResponse>> GetDashboardData()
        {
            var response = await _httpClient.GetAsync(Routes.DashboardEndpoints.GetMPRData);
            return await response.ToResult<GetMPRDashboardDataResponse>();
        }

        public async Task<IResult<string>> SaveAsync(AddEditNPRRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.RequestsEndpoints.Save, request);
            return await response.ToResult<string>();
        }

        public async Task<IResult<bool>> SubmitAction(ActionRequest action)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.RequestsEndpoints.SubmitAction, action);
            return await response.ToResult<bool>();
        }
    }
}
