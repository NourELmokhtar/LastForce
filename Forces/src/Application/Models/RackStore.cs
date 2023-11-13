

using Forces.Domain.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forces.Application.Models
{

    public class RackStore : AuditableEntity<int>
    {
        public RackStore()
        {
            Shilfs = new HashSet<ShilfRack>();
        }
        public int RackCode { get; set; }
        public string RackName { get; set; }
        [ForeignKey("StoreId")]
        public virtual SectionStore Store { get; set; }
        public int StoreId { get; set; }
        public virtual ICollection<ShilfRack> Shilfs { get; set; }
    }
}
