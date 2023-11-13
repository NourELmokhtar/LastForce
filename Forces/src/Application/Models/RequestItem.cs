using Forces.Application.Enums;
using Forces.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Models
{
    public class RequestItem : AuditableEntity<int>
    {
        public int RequestId { get; set; }
        public int ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Items Items { get; set; }
        public decimal ItemPrice { get; set; }
        public Priority Priority { get; set; }
        public string ItemName { get; set; }
        public string ItemArName { get; set; }
        public decimal ItemQty { get; set; }

        public string ItemNSN { get; set; }
        public string ItemCode { get; set; }
        public string Unit { get; set; }
        public string ItemClass { get; set; }
    }
}
