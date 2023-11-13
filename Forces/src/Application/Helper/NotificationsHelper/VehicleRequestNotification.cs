using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forces.Application.Models;
using Forces.Domain.Contracts;

namespace Forces.Application.Helper.NotificationsHelper
{
    public class VehicleRequestNotification : INotifire<VehicleRequest>
    {
        public VehicleRequest Entity { get; set; }
        public Type StateEnum { get; set; }
        public string Message { get; set; }
        public string ReturnURL { get; set; }
    }
}
