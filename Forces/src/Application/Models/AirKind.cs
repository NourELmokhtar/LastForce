using Forces.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Models
{
    public class AirKind : AuditableEntity<int>
    {
        public AirKind()
        {
            AirCrafts = new HashSet<AirCraft>();
        }
        public string AirKindName { get; set; }
        public string AirKindCode { get; set; }
        public int AirTypeId { get; set; }
        [ForeignKey("AirTypeId")]
        public virtual AirType AirType { get; set; }
        public virtual ICollection<AirCraft> AirCrafts { get; set; }
    }
}
