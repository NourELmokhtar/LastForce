using Forces.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Enums
{
    public enum RequestState
    {
        [ArDescription("قيد الإنتظار")] [EnDescription("Pending")] Pending = 1,
        [ArDescription("مكتمل")] [EnDescription("Completed")] Completed = 2,
        [ArDescription("مرفوض")] [EnDescription("Rejected")] Rejected = 3,
        [ArDescription("ملغي")] [EnDescription("Canceldeld")] Canceldeld = 4,

    }
}
