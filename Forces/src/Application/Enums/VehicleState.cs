using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Enums
{
    public enum VehicleState
    {
        [Description("Avilable")]
        Avilable,
        [Description("Not Avilable")]
        NotAvilable,
        [Description("On Service")]
        OnService,
    }
}
