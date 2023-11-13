using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forces.Application.Enums;
namespace Forces.Application.Requests.Requests
{
    public class GetRequestsBySpecificationsRequest
    {
        public int? RequestId { get; set; }
        public string RequestRef { get; set; }
        public RequestSteps? RequestStep { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string UserId { get; set; }
        public string TargetUserId { get; set; }
        public int? Year { get; set; }
        public int? BaseId { get; set; }
        public int? ForceId { get; set; }
        public int? BaseSectionId { get; set; }
        public DepartType? TargetDeparmentType { get; set; }
        public int? TargetDepartmentId { get; set; }
        public UserType? userType { get; set; }
    }
}
