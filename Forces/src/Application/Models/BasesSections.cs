using Forces.Application.Interfaces.Common;
using Forces.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Models
{
    public class BasesSections : AuditableEntity<int>
    {
        public BasesSections()
        {
            Stores = new HashSet<SectionStore>();
        }
        public string SectionName { get; set; }
        public string SectionCode { get; set; }
        public int BaseId { get; set; }
        [ForeignKey("BaseId")]
        public virtual Bases Base { get; set; }

        public virtual ICollection<SectionStore> Stores { get; set; }
        public virtual ICollection<Office> Offices { get; set; }

    }
}
