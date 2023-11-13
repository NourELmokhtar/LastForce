using Forces.Application.Enums;
using Forces.Application.Features.Items.Queries.GetAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.Items.Queries.GetBySpecifics
{
    public class GetItemsByResponse
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string ItemArName { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string ItemNsn { get; set; }
        public int MeasureUnitId { get; set; }
        public string MeasureName { get; set; }
        public ItemClass ItemClass { get; set; }
        public int VoteCodesId { get; set; }
        public string VoteCode { get; set; }
        public string MadeIn { get; set; }

        public DateTime? DateOfEnter { get; set; }
        public DateTime? FirstUseDate { get; set; }
        public DateTime? EndOfServiceDate { get; set; }
        public string SerialNumber { get; set; }
    }

}
