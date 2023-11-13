using Forces.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Models
{
    public class PersonalItemsOperation_Details : AuditableEntity<int>
    {
        public int PersonalItemsOperation_HdrId { get; set; }
        [ForeignKey("PersonalItemsOperation_HdrId")]
        public virtual PersonalItemsOperation_Hdr PersonalItemsOperation_Hdr { get; set; }
        public int PersonalItemId { get; set; }
        [ForeignKey("PersonalItemId")]
        public virtual PersonalItems PersonalItem { get; set; }
        public DateTime OperationDate { get; set; }
        public string UserId { get; set; }
        public int Qty { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal TotalLinePrice { get; set; }

        public int? TailerId { get; set; }
        [ForeignKey("TailerId")]
        public virtual Tailers Tailer { get; set; }

    }
}
