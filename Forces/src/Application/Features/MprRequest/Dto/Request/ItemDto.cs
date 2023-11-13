using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.MprRequest.Dto.Request
{
    public class ItemDto
    {
        public int ItemId { get; set; }
        public decimal ItemPrice { get; set; }
        public string ItemName { get; set; }
        public string ItemArName { get; set; }
        public decimal ItemQty { get; set; }

        public string ItemNSN { get; set; }
        public string ItemCode { get; set; }
        public string Unit { get; set; }
        public string ItemClass { get; set; }
        public int VotecodeId { get; set; }
 
    }
}
