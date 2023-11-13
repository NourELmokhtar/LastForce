using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Forces.Client.Shared.Components
{
    public partial class EnumSelectorComponent<E>
        where E : Enum
    {
        [Parameter] public E TargetEnum { get; set; }

    }
}
