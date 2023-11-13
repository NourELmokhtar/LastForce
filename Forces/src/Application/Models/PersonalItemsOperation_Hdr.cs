using Forces.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Models
{
    public class PersonalItemsOperation_Hdr : AuditableEntity<int>
    {
        public PersonalItemsOperation_Hdr()
        {
            Details = new HashSet<PersonalItemsOperation_Details>();
        }
        public string UserId { get; set; }
        public DateTime OperationDate { get; set; }
        public decimal Total { get; set; }
        public int ForceId { get; set; }
        public int? BaseId { get; set; }
        public int? BaseSectionId { get; set; }
        public virtual ICollection<PersonalItemsOperation_Details> Details { get; set; }
    }
}
