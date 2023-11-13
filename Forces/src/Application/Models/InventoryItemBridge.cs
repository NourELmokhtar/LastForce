using Forces.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Models
{
    public class InventoryItemBridge : AuditableEntity<int>
    {

        [ForeignKey("Inventory")]
        public int InventoryId { get; set; }

        [ForeignKey("InventoryItem")]
        public int InventoryItemId { get; set; }
        
        public string? SerialNumber { get; set; }
        public DateTime DateOfEnter { get; set; }
        public virtual InventoryItem InventoryItem { get; set; }
        public virtual Inventory Inventory { get; set; }
    }

}
