using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Responses.VoteCodes
{
    public class VoteCodeResponse
    {
        public int Id { get; set; }
        public string VoteCode { get; set; }
        public string VoteShortcut { get; set; }
        public int ForceId { get; set; }
        public List<VoteCodeControllersResponse> Holders { get; set; } = new();
        public string DfaultHolderId { get; set; }
        public string UserName { get; set; }
        public List<VoteCodeLogResponse> Logs { get; set; }
        public bool IsPrimery { get; set; }
        public decimal Cridet { get; set; }
        public int AllRequestsCount { get; set; }
        public int PendingRequestsCount { get; set; }
        public int CompletedRequestsCount { get; set; }
    }
}
