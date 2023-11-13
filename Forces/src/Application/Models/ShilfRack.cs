

using Forces.Domain.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forces.Application.Models
{
    public class ShilfRack : AuditableEntity<int>

    {
        public ShilfRack()
        { Bins = new HashSet<BinRack>();
        }
        public string ShilfCode { get; set; }
        public string ShilfName { get; set; }
        [ForeignKey("RackId")]
        public virtual RackStore Store { get; set; }
        public int RackId { get; set; }
        public virtual ICollection<BinRack> Bins { get; set; }

    }
}
