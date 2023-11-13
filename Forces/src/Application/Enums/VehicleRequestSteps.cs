using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Enums
{
    public enum VehicleRequestSteps
    {
        [Description("New Request")] Creation,
        [Description("OC")] OC,
        [Description("MT")] MT,
        [Description("Security")] Security,
        [Description("Submitted")] Submitted
    }
}
