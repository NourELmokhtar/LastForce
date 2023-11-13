using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Forces.Client.Shared.Components
{
    public partial class NotificationItemComponent
    {
        [Parameter] public string Title { get; set; }
        [Parameter] public string Description { get; set; }
        [Parameter] public string Url { get; set; }
        [Parameter] public bool Seen { get; set; }
        [Parameter] public DateTime Date { get; set; }
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }
        public string itemClass()
        {
            if (Seen)
            {
                return "msg-item";
            }
            else
            {
                return "msg-item un-read";
            }
        }


    }
}
