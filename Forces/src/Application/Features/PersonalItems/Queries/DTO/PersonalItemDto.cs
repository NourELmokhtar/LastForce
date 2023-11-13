using Forces.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.PersonalItems.Queries.DTO
{
    public class PersonalItemDto
    {
        public int Id { get; set; }
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
        public int BaseId { get; set; }
    }
}
