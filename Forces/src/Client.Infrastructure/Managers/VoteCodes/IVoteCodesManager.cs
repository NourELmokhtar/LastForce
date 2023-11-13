using Forces.Application.Requests.VoteCodes;
using Forces.Application.Responses.VoteCodes;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.VoteCodes
{
    public interface IVoteCodesManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditVoteCodeRequest request);
        Task<IResult<List<VoteCodeResponse>>> GetAllAsync();
        Task<IResult<List<VoteCodeResponse>>> GetCodesByUserId(string UserId);
        Task<IResult<List<VoteCodeResponse>>> GetAllByCurrentUser();
        Task<IResult<VoteCodeResponse>> GetCodeBy(int Id);
        Task<IResult<int>> DeleteAsync(int id);
        Task<IResult<List<VoteCodeLogResponse>>> GetvCodeLogs(int VoteCodeId);
        Task<IResult<List<VoteCodeLogResponse>>> GetLogsSpec(VoteCodeLogSpecificationRequest command);
        Task<IResult<VoteCodeLogResponse>> GetLogDetails(int logId);
        Task<IResult<int>> AddEditTransaction(AddEditVcodeTransactionRequest request);
        Task<IResult<decimal>> GetVoteCodeCredit(int VoteCodeId);
        Task<IResult<string>> ExportToExcelAsync(List<VoteCodeLogResponse> Data);
    }
}
