using Forces.Domain.Contracts;
using Forces.Application.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forces.Application.Models
{
    public class PersonalItems : AuditableEntity<int>, IBaseScope
    {
        public PersonalItems()
        {
            OperationDetails = new HashSet<PersonalItemsOperation_Details>();
        }
        public string ItemName { get; set; }
        public string ItemArName { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string ItemNsn { get; set; }
        public UsagePeriodUnit UsagePeriodUnit { get; set; }
        public int? UsagePeriod { get; set; }
        public bool StorageableItem { get; set; }
        public decimal? ItemPrice { get; set; }
        public int? MaxQtyOnPeriod { get; set; }
        public int? TailerId { get; set; }
        [ForeignKey("TailerId")]
        public virtual Tailers Tailer { get; set; }
        public virtual ICollection<PersonalItemsOperation_Details> OperationDetails { get; set; }
        public int BaseId { get; set; }
    }
}
