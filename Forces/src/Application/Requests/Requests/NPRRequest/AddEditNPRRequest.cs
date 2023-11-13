using Forces.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Requests.Requests.NPRRequest
{
    public class AddEditNPRRequest
    {

        public Priority Priority { get; set; } = Priority.Normal;
        public int VoteCodeId { get; set; }
        public int ItemId { get; set; }
        public decimal QTY { get; set; }
        public decimal Price { get; set; }
        public string Note { get; set; }
        public string Unit { get; set; }
        public string ItemClass { get; set; }
        public List<AttachmentRequest> Attachments { get; set; } = new();



    }
}
