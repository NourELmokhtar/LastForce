using Forces.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Models
{
    public class MeasureUnits : AuditableEntity<int>
    {
        public MeasureUnits()
        {
            Items = new HashSet<Items>();
        }
        public string Name { get; set; }
        public virtual ICollection<Items> Items { get; set; }
    }
}
