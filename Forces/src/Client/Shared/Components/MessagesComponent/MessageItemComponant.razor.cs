using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Forces.Client.Shared.Components.MessagesComponent
{
    public partial class MessageItemComponant
    {
        [Parameter] public string UserId { get; set; }
        [Parameter] public string Name { get; set; }
        [Parameter] public string LastMessage { get; set; }
        [Parameter] public DateTime MesageDate { get; set; }
        [Parameter] public string ProfilePic { get; set; }
        [Parameter] public bool Seen { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
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
