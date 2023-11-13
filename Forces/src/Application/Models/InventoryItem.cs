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
    public class InventoryItem : AuditableEntity<int>
    {
        public string ItemName { get; set; }
        public string ItemArName { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string ItemNsn { get; set; }
        public int MeasureUnitId { get; set; }
        [ForeignKey("MeasureUnitId")]
        public virtual MeasureUnits MeasureUnit { get; set; }

        public ItemClass ItemClass { get; set; }
        public DateTime? DateOfEnter { get; set; }
        public DateTime? FirstUseDate { get; set; }
        public DateTime? EndOfServiceDate { get; set; }
        public string SerialNumber { get; set; }
        [ForeignKey("Inventory")]
        public int? InventoryId { get; set; }
        public virtual Inventory? Inventory { get; set; }

    }
}
