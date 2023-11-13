using Forces.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Models
{
    public class Office : AuditableEntity<int>
    {
        [ForeignKey("BasesSections")]
        public int BasesSectionsId { get; set; }
        public string Name { get; set; }
        public virtual BasesSections BasesSections { get; set; }
    }
}
