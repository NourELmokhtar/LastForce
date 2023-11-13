using Forces.Application.Enums;
using Forces.Application.Features.MprRequest.Dto.Request;
using Forces.Application.Models;
using Forces.Application.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Application.Interfaces.Repositories
{
    public interface IMprRequestRepository
    {
        Task<int> SubmitPayment(int RequestId);
        Task<int> ConfirmPaied(int RequestId);
        Task<int> AddRequestAsync(MprRequest mpr);
        Task<int> AddActionAsync(MprRequestAction action,int RequestId, RequestState requestState); 
        Task<int> AddAttachmentAsync(MprRequestAttachments attachment, int RequestId);
        Task<int> AddActionAttachmentAsync(MprRequestAttachments attachment, int ActionId);
        Task<int> AddAttachmentsAsync(List<MprRequestAttachments> attachments, int RequestId);
        Task<int> AddActionAttachmentAsync(List<MprRequestAttachments> attachments, int ActionId);
        Task<MprRequest> GetRequestByIdAsync(int id);
        Task<MprRequest> GetRequestByRefrenceIdAsync(string RefId);
        Task<MprSteps> GerRequestStepAsync(int RequestId);
        Task<int> SelectAttachmentAsync(int ActionId , string Note , List<string> SelectedAttachments);
        Task<int> CancelRequestAsync(int RequestId,string Note);
        Task<List<MprRequest>> GetRequestsAsync();
        Task<List<MprRequest>> GetRequestsByVoteCodeAsync(int voteCodeId);
        Task<List<MprRequest>> GetAllRequestsAsync();
        Task<List<MprRequest>> GetAllRequestsBySectionIdAsync();
        Task<List<MprRequest>> GetAllRequestsByBaseIdAsync();
        Task<List<MprRequest>> GetAllRequestsByForceIdAsync();
        Task<List<MprRequest>> GetAllRequestsByUserIdAsync(string UserID);
        Task<List<StepActions>> GetAvilableActionsAsync(int RequestId);
        Task<int> NotifyOwner(MprRequest request);
        Task<int> NotifySteppers(MprRequest request);
        Task<int> NotifyStepperUser(MprRequest request,string TargetUser);
        MprSteps GetNextStep(MprRequest request);

       
        Task<int> SubmitRequestAsync(int ActionId, string Note,int? voteCodeId);
        Task<int> RejectRequestAsync(int ActionId, string Note);
        Task<int> EsclateRequestAsync(int ActionId,string Note);
        Task<int> EditRequestAsync(List<ItemDto> Items,int ActionId , string Note, List<UploadRequest> Attachments);
        Task<int> RedirectRequestAsync(int ActionId, RedirectAction To , string refId,string Note,int? refIdint);
        Task<(string Force, string Base, string Section)> GerRequestBasecInfoAsync(MprRequest request);





    }
}
