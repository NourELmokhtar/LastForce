using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forces.Application.Enums;
namespace Forces.Application.Responses.Requets.MPR
{
    public class GetAllMPRResponse
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastActionDate { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string ItemName { get; set; }
        public decimal ItemQTY { get; set; }
        public string ItemCode { get; set; }
        public string ItemNSN { get; set; }
        public string ItemDescription { get; set; }
        public decimal ItemPrice { get; set; }
        public Priority Priority { get; set; }
        public RequestState RequestState { get; set; }
        public string VoteCode { get; set; }
        public int? ForceId { get; set; }
        public int? BaseId { get; set; }
        public int? BaseSectionId { get; set; }
        public RequestSteps CurrentStep { get; set; }
        public string ItemUnit { get; set; }
        public string ItemNameAR { get; set; }
        public List<string> Attachments { get; set; }
        public string RefrenceId { get; set; }
        public List<RequestActions> Actions { get; set; }
        public string CreatorId { get; set; }
        public bool isDone { get; set; }

    }

    public class RequestActions
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        public DateTime? ActionDate { get; set; }
        public RequestSteps ActionStep { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public int? Rank { get; set; }
        public string JobTitle { get; set; }
        public string TakenAction { get; set; }
        public List<string> ActionAttachment { get; set; }
        public string ActionNote { get; set; }
        public ActionState ActionState { get; set; }
        public DepartType? Department { get; set; }
        public int? departId { get; set; }
    }
}
