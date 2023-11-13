using Forces.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Enums
{
    public enum Priority
    {
        [ArDescription("إعتيادي")] [EnDescription("Normal  Priority")] Normal = 1,
        [ArDescription("هام")] [EnDescription("High Priority.")] High = 2,
        [ArDescription("هام جداً")] [EnDescription("Highest Priority")] Highest = 3,
        [ArDescription("غير هام")] [EnDescription("Low Priority")] Low = 4,

    }
}
