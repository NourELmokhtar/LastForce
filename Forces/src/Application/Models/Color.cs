

using Forces.Domain.Contracts;
using System.Collections.Generic;

namespace Forces.Application.Models
{
    public class Color : AuditableEntity<int>

    {
        public string ColorName { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }

    }
}
