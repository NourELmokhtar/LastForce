

using Forces.Domain.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forces.Application.Models
{
    public class SectionStore : AuditableEntity<int>
    {
        public SectionStore()
        {
            Racks = new HashSet<RackStore>();
        }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        [ForeignKey("SectionId")]
        public virtual BasesSections Section { get; set; }

        public int SectionId { get; set; }
        public virtual ICollection<RackStore> Racks { get; set; }
    }
}

