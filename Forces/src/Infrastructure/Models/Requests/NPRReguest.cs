using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forces.Application.Models;
using Forces.Infrastructure.Models.Identity;

namespace Forces.Infrastructure.Models.Requests
{
    public class NPRReguest : Requests<VoteCodes, Appuser, RequestActions>
    {
        public string ItemName { get; set; }
        public string ItemArName { get; set; }
        public decimal ItemQty { get; set; }

        public string ItemNSN { get; set; }
        public string ItemCode { get; set; }
        public string Unit { get; set; }
        public string ItemClass { get; set; }
    }
    public class BODReguest : Requests<VoteCodes, Appuser, RequestActions>
    {

    }
    public class CashReguest : Requests<VoteCodes, Appuser, RequestActions>
    {

    }
}
