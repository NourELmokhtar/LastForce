using Forces.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Models
{
    public class AirType : AuditableEntity<int>
    {
        public AirType()
        {
            Kinds = new HashSet<AirKind>();
        }
        public string AirTypeName { get; set; }
        public string AirTypeCode { get; set; }
        public virtual ICollection<AirKind> Kinds { get; set; }
    }
}
