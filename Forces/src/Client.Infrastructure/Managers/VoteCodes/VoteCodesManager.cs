using Forces.Application.Requests.VoteCodes;
using Forces.Application.Responses.VoteCodes;
using Forces.Client.Infrastructure.Extensions;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.VoteCodes
{
    public class VoteCodesManager : IVoteCodesManager
    {
        private readonly HttpClient _httpClient;

        public VoteCodesManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> AddEditTransaction(AddEditVcodeTransactionRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.VoteCodesEndpoints.AddEditTransaction, request);
            return await response.ToResult<int>();
        }

        public Task<IResult<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<string>> ExportToExcelAsync(List<VoteCodeLogResponse> Data)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.VoteCodesEndpoints.Export, Data);
            return await response.ToResult<string>();
        }

        public async Task<IResult<List<VoteCodeResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.VoteCodesEndpoints.GetAll);
            return await response.ToResult<List<VoteCodeResponse>>();
        }

        public async Task<IResult<List<VoteCodeResponse>>> GetAllByCurrentUser()
        {
            var response = await _httpClient.GetAsync(Routes.VoteCodesEndpoints.GetAllByCurrentUser);
            return await response.ToResult<List<VoteCodeResponse>>();
        }

        public async Task<IResult<VoteCodeResponse>> GetCodeBy(int Id)
        {
            var response = await _httpClient.GetAsync(Routes.VoteCodesEndpoints.GetvCodeById(Id));
            return await response.ToResult<VoteCodeResponse>();
        }

        public async Task<IResult<List<VoteCodeResponse>>> GetCodesByUserId(string UserId)
        {
            var response = await _httpClient.GetAsync(Routes.VoteCodesEndpoints.GetVoteCodeByUserId(UserId));
            return await response.ToResult<List<VoteCodeResponse>>();
        }

        public async Task<IResult<VoteCodeLogResponse>> GetLogDetails(int logId)
        {
            var response = await _httpClient.GetAsync(Routes.VoteCodesEndpoints.GetLogDetails(logId));
            return await response.ToResult<VoteCodeLogResponse>();
        }

        public async Task<IResult<List<VoteCodeLogResponse>>> GetLogsSpec(VoteCodeLogSpecificationRequest command)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.VoteCodesEndpoints.GetLogBySpecification, command);
            return await response.ToResult<List<VoteCodeLogResponse>>();
        }

        public async Task<IResult<List<VoteCodeLogResponse>>> GetvCodeLogs(int VoteCodeId)
        {
            var response = await _httpClient.GetAsync(Routes.VoteCodesEndpoints.GetvCodeLogs(VoteCodeId));
            return await response.ToResult<List<VoteCodeLogResponse>>();
        }

        public async Task<IResult<decimal>> GetVoteCodeCredit(int VoteCodeId)
        {
            var response = await _httpClient.GetAsync(Routes.VoteCodesEndpoints.GetVoteCodeCredit(VoteCodeId));
            return await response.ToResult<decimal>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditVoteCodeRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.VoteCodesEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}
