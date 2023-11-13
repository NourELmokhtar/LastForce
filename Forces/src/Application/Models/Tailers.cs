using Forces.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Models
{
    public class Tailers : AuditableEntity<int>
    {
        public Tailers()
        {
            PersonalItems = new HashSet<PersonalItems>();
            Operations = new HashSet<PersonalItemsOperation_Details>();
        }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int BaseId { get; set; }
        public string TailerCode { get; set; }
        public virtual ICollection<PersonalItems> PersonalItems { get; set; }
        public virtual ICollection<PersonalItemsOperation_Details> Operations { get; set; }

    }
}
