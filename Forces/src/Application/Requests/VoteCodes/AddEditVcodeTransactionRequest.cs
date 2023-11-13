using Forces.Application.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Requests.VoteCodes
{
    public class AddEditVcodeTransactionRequest
    {
        public int LogId { get; set; }
        [Required]
        public int VoteCodeId { get; set; }
        [Required]
        public decimal TransactionAmount { get; set; }
        public string RequestType { get; set; }
        public int? RequestId { get; set; }
        public string RequestRefrance { get; set; }
        [Required]
        public TransactionType Transactiontype { get; set; }
        public string Reason { get; set; }
        public string Note { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; } = "";
        public string ItemCode { get; set; } = "";
        public string ItemNSN { get; set; } = "";
        public decimal Credit { get; set; } = decimal.Zero;
    }
}
