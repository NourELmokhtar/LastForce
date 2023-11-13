using Forces.Domain.Contracts;
using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Models
{
    public class Inventory : AuditableEntity<int>
    {
        public int? RoomId { get; set; }
        public int? HouseId { get; set; }
        public int? BaseSectionId { get; set; }
        public string Name { get; set; }
        public virtual Room? Room { get; set; }
        public virtual House? House { get; set; }
        public virtual BasesSections? BaseSection { get; set; }
        public virtual List<InventoryItem>? InventoryItems { get; set; }
    }
}
