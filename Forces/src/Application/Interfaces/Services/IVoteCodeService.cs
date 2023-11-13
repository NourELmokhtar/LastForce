using Forces.Application.Interfaces.Common;
using Forces.Application.Requests.VoteCodes;
using Forces.Application.Responses.VoteCodes;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Services
{
    public interface IVoteCodeService : IService
    {
        Task<IResult> AddEditVoteCode(AddEditVoteCodeRequest Request);
        Task<IResult<List<VoteCodeResponse>>> GetAllCodes();
        Task<IResult<List<VoteCodeLogResponse>>> GetvCodeLogs(int VoteCodeId);
        Task<IResult<VoteCodeLogResponse>> GetLogDetails(int logId);
        Task<IResult<List<VoteCodeLogResponse>>> GetLogBySpecification(VoteCodeLogSpecificationRequest reuest);
        Task<IResult<List<VoteCodeResponse>>> GetCodesByUserId(string userId);
        Task<VoteCodeResponse> GetCodeBy(string Code);
        Task<IResult<VoteCodeResponse>> RGetCodeBy(int Id);
        Task<VoteCodeResponse> GetCodeBy(int Id);
        Task<int> GetVoteCodeCountAsync();
        Task<int> GetVoteCodeUsersCountAsync();
        Task<IResult<int>> AddEditTransaction(AddEditVcodeTransactionRequest request);
        Task<IResult<decimal>> GetVoteCodeCredit(int VoteCodeId);
        Task<IResult<string>> ExportLog(List<VoteCodeLogResponse> Data);
    }
}
