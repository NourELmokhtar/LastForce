using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forces.Application.Enums;
namespace Forces.Application.Requests.Requests
{
    public class ActionRequest
    {
        public int RequestActionId { get; set; }
        public int RequestId { get; set; }
        public ActionsType ActionsType { get; set; }
        public DepartType? DepartType { get; set; }
        public int? DeparmentId { get; set; }
        public string TargetUserId { get; set; }
        public string ActionNote { get; set; }
        public int? VoteCodeId { get; set; }
        public List<AttachmentRequest> Attachments { get; set; } = new();
        public decimal? AdditionalPrice { get; set; }
    }
}
