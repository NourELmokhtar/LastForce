using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.PersonalItemOperations.Queries.Eligibility
{
    public class EligibilityModel
    {
        public int ItemId { get; set; }
        public string UserId { get; set; }
        public bool Eligibile { get; set; }
        public int EligibilityQty { get; set; }
        public DateTime? LastOrderDate { get; set; }
        public EligibilityModel(int itemId, string userId, bool eligibile, int eligibilityQty, DateTime? lastOrderDate = null)
        {
            ItemId = itemId;
            UserId = userId;
            Eligibile = eligibile;
            EligibilityQty = eligibilityQty;
            LastOrderDate = lastOrderDate;
        }
        public EligibilityModel()
        {

        }
    }
}
