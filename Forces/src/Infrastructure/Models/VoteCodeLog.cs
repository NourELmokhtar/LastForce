using Forces.Application.Enums;
using Forces.Application.Models;
using Forces.Domain.Contracts;
using Forces.Infrastructure.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Models
{
    public class VoteCodeLog : AuditableEntity<int>
    {
        public int VoteCodeId { get; set; }
        [ForeignKey("VoteCodeId")]
        public virtual VoteCodes VoteCode { get; set; }
        public decimal AmountBefore { get; set; }
        public decimal AmountAfter { get; set; }
        public decimal TransactionAmount { get; set; }
        public int? RequestId { get; set; }
        [ForeignKey("RequestId")]
        public virtual Requests<VoteCodes, Appuser, RequestActions> Requests { get; set; }
        public string RequestRefrance { get; set; }
        public string Requesttype { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual Appuser User { get; set; }
        public TransactionType TransactionType { get; set; }
        public string Reason { get; set; }
        public string Note { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string ItemNSN { get; set; }

    }
}
