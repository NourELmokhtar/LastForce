using Forces.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Models
{
    public class Forces : AuditableEntity<int>
    {
        public string ForceName { get; set; }
        public string ForceCode { get; set; }
        public virtual ICollection<Bases> Bases { get; set; }
    }
}
