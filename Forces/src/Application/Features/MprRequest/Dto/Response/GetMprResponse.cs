using Forces.Application.Enums; 
using System;
using System.Collections.Generic;
 

namespace Forces.Application.Features.MprRequest.Dto.Response
{
    public class GetMprResponse
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastActionDate { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public Priority Priority { get; set; }
        public RequestState RequestState { get; set; }
        public string VoteCode { get; set; }
        public int? ForceId { get; set; }
        public int? BaseId { get; set; }
        public int? BaseSectionId { get; set; }
        public MprSteps CurrentStep { get; set; }
        public List<string> Attachments { get; set; }
        public string RefrenceId { get; set; }
        public List<RequestActions> Actions { get; set; }
        public List<ItemDto> Items { get; set; }
        public string CreatorId { get; set; }
        public bool isDone { get; set; }
        public string SelectedAttachment { get; set; }
        public string SelectedAttachmentBy { get; set; }
        public bool? PaiedOff { get; set; }
        public bool? ConfirmPaied { get; set; }
        public int? Rank { get; set; }
        public string strRank { get; set; }
        public string FullName { get; set; }
        public string Force { get; set; }
        public string Base { get; set; }
        public string Section { get; set; }
    }
    public class RequestActions
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        public DateTime? ActionDate { get; set; }
        public MprSteps ActionStep { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public int? Rank { get; set; }
        public string strRank { get; set; }
        public string JobTitle { get; set; }
        public string TakenAction { get; set; }
        public List<string> ActionAttachment { get; set; }
        public string ActionNote { get; set; }
        public ActionState ActionState { get; set; }
        public DepartType? Department { get; set; }
        public int? departId { get; set; }
        public string ActionResult { get; set; }
    }
    public class ItemDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemQTY { get; set; }
        public string ItemCode { get; set; }
        public string ItemNSN { get; set; }
        public string ItemDescription { get; set; }
        public decimal ItemPrice { get; set; }
        public string ItemUnit { get; set; }
        public string ItemNameAR { get; set; }
        public int VotecodeId { get; set; }

    }
}
