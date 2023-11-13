using Forces.Application.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Requests.VoteCodes
{
    public class VoteCodeLogSpecificationRequest
    {
        [Required]
        public int VoteCodeId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string ItemNSN { get; set; }
        public string UserName { get; set; }
        public string RequestRef { get; set; }
        public MathOperation? Operator { get; set; }
        public decimal? Value { get; set; }
        public TransactionType? TransactionType { get; set; }
    }
}
