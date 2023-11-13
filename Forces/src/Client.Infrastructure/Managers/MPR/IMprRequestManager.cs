using Forces.Application.Features.MprRequest.Command;
using Forces.Application.Features.MprRequest.Command.Actions;
using Forces.Application.Features.MprRequest.Dto.Response;
using Forces.Application.Responses.Requets.MPR;
using Forces.Client.Infrastructure.Extensions;
using Forces.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.MPR
{
    public interface IMprRequestManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddMprRequestCommand request);
        Task<IResult<List<GetMprResponse>>> GetRequestsAsync();
        public Task<IResult<GetMprResponse>> GetAllRequestsById(int requestId);

        Task<IResult<int>> RejectActionAsync(RejectActionCommand request);
        Task<IResult<int>> CancelActionAsync(CancelActionCommand request);
        Task<IResult<int>> EsclateActionAsync(SclateActionCommand request);
        Task<IResult<int>> RedirectActionAsync(RedirectActionCommand request);
        Task<IResult<int>> SubmitActionAsync(SubmitActionCommand request);
        Task<IResult<int>> EditActionAsync(EditActionCommand request);
        Task<IResult<int>> SelectQutationActionAsync(SelectQutationActionCommand request);
        Task<IResult<int>> SubmitPaymentAsync(int requestId);
        Task<IResult<int>> ConfirmPaymentAsync(int requestId);
        Task<IResult<List<GetMprResponse>>> GetRequestsByVoteCodeAsync(int voteCode);

    }
    public class MprRequestManager : IMprRequestManager
    {
        private readonly HttpClient _httpClient;

        public MprRequestManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> CancelActionAsync(CancelActionCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.RequestsEndpoints.MPR.CancelAction, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> ConfirmPaymentAsync(int requestId)
        {
            var response = await _httpClient.GetAsync(Routes.RequestsEndpoints.MPR.ConfirmPayment(requestId));
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> EditActionAsync(EditActionCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.RequestsEndpoints.MPR.EditAction, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> EsclateActionAsync(SclateActionCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.RequestsEndpoints.MPR.EsclateAction, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<GetMprResponse>> GetAllRequestsById(int requestId)
        {
            var response = await _httpClient.GetAsync(Routes.RequestsEndpoints.MPR.GetAllRequestsById(requestId));
            return await response.ToResult<GetMprResponse>();
        }

        public async Task<IResult<List<GetMprResponse>>> GetRequestsAsync()
        {
            var response = await _httpClient.GetAsync(Routes.RequestsEndpoints.MPR.GetAll);
            return await response.ToResult<List<GetMprResponse>>();
        }

        public async Task<IResult<List<GetMprResponse>>> GetRequestsByVoteCodeAsync(int voteCode)
        {
            var response = await _httpClient.GetAsync(Routes.RequestsEndpoints.MPR.RequestsByVoteCode(voteCode));
            return await response.ToResult<List<GetMprResponse>>();
        }

        public async Task<IResult<int>> RedirectActionAsync(RedirectActionCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.RequestsEndpoints.MPR.RedirectAction, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> RejectActionAsync(RejectActionCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.RequestsEndpoints.MPR.RejectAction, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> SaveAsync(AddMprRequestCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.RequestsEndpoints.MPR.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> SelectQutationActionAsync(SelectQutationActionCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.RequestsEndpoints.MPR.SelectQutaionAction, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> SubmitActionAsync(SubmitActionCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.RequestsEndpoints.MPR.SubmitAction, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> SubmitPaymentAsync(int requestId)
        {
            var response = await _httpClient.GetAsync(Routes.RequestsEndpoints.MPR.SubmitPayment(requestId));
            return await response.ToResult<int>();
        }
    }
}
