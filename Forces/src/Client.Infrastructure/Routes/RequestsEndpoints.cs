using Forces.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Routes
{
    public class RequestsEndpoints
    {
        public static string Save = "api/v1/NPR";
        public static string GetAllRequestsCount = "api/v1/NPR/GetAllRequestsCount";
        public static string GetAllRequestsByUser = "api/v1/NPR/GetAllRequestsForUser";
        public static string GetAllRequestsForTargetUser = $"api/v1/NPR/GetAllRequestsForTargetUser";
        public static string GetAllRequestsToLog = $"api/v1/NPR/GetAllRequestsToLog";
        public static string GetAllRequestsBySpecifications = $"api/v1/NPR/GetAllRequestsBySpecifications";
        public static string GetAllRequestsById(int Id) => $"api/v1/NPR/GetRequetById/{Id}";
        public static string GetAllRequestsByRefrance(string Ref) => $"api/v1/NPR/GetRequetByRef/{Ref}";
        public static string GetAllRequestsBySteps(RequestSteps Step) => $"api/v1/NPR/GetRequetByStep/{(int)Step}";
        public static string GetAvilableActions = $"api/v1/NPR/GetAvilableActions";
        public static string SubmitAction = $"api/v1/NPR/SubmitAction";


        ///////////////////////MPR New ///////////////////////////////
       public class MPR
        {
            public static string Save = "api/v1/MPR";
            public static string GetAll = "api/v1/MPR";
            public static string GetAllRequestsById(int Id) => $"api/v1/MPR/GetRequetById/{Id}";
            public static string RequestsByVoteCode(int Id) => $"api/v1/MPR/Requests/Votecode/{Id}";
            public static string SubmitPayment(int Id) => $"api/v1/MPR/Action/SubmitPay/{Id}";
            public static string ConfirmPayment(int Id) => $"api/v1/MPR/Action/ConfirmPay/{Id}";

            public static string RejectAction = "api/v1/MPR/Action/reject";
            public static string CancelAction = "api/v1/MPR/Action/Cancel";
            public static string EsclateAction = "api/v1/MPR/Action/Esclate";
            public static string RedirectAction = "api/v1/MPR/Action/redirect";
            public static string SubmitAction = "api/v1/MPR/Action/submit";
            public static string EditAction = "api/v1/MPR/Action/Edit";
            public static string SelectQutaionAction = "api/v1/MPR/Action/SelectQutaion";
             
        }
    }
}
