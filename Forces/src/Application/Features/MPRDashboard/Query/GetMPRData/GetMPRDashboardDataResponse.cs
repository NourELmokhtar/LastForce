using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Features.MPRDashboard.Query.GetMPRData
{
    public class GetMPRDashboardDataResponse
    {
        public int TotalRequests { get; set; }
        public int Completed { get; set; }
        public int Pending { get; set; }
        public int Rejected { get; set; }
        public int PaiedOff { get; set; }
        public int Canceld { get; set; }
    }
}
