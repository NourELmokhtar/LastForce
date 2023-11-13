using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Enums
{
    public enum ItemClass
    {
        [Description("A")]
        A = 1
            , [Description("B")] B = 2
            , [Description("C")] C = 3
    }
}
