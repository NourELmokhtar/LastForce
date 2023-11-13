using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forces.Application.Enums;
namespace Forces.Application.Responses.VoteCodes
{
    public class VoteCodeLogResponse
    {
        public int LogId { get; set; }
        public int VoteCodeId { get; set; }
        public decimal AmountBefore { get; set; }
        public decimal AmountAfter { get; set; }
        public decimal TransactionAmount { get; set; }
        public string RequestType { get; set; }
        public int? RequestId { get; set; }
        public string RequestRefrance { get; set; }
        public TransactionType Transactiontype { get; set; }
        public string Reason { get; set; }
        public string Note { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionBy { get; set; }
        public string UserId { get; set; }

        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string ItemNSN { get; set; }
        public bool ShowDetails { get; set; } = false;
    }
}
