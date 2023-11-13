using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    public static class VoteCodesEndpoints
    {
        public static string Save = "api/v1/VoteCodes";
        public static string GetLogBySpecification = "api/v1/VoteCodes/GetLogsSpec";
        public static string AddEditTransaction = "api/v1/VoteCodes/AddEditTransaction";
        public static string GetAll = "api/v1/VoteCodes";
        public static string Export = "api/v1/VoteCodes/export";
        public static string GetVoteCodeByUserId(string Id) => $"api/v1/VoteCodes/{Id}";
        public static string GetAllByCurrentUser = "api/v1/VoteCodes/GetAllByCurrentUser";
        public static string GetvCodeLogs(int Id) => $"api/v1/VoteCodes/vCodeLogs/{Id}";
        public static string GetvCodeById(int Id) => $"api/v1/VoteCodes/Get/{Id}";
        public static string GetLogDetails(int Id) => $"api/v1/VoteCodes/LogDetails/{Id}";
        public static string GetVoteCodeCredit(int Id) => $"api/v1/VoteCodes/VoteCodeCredit/{Id}";
    }
}
