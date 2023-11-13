using Forces.Application.Interfaces.Common;
using Forces.Application.Requests.Requests.NPRRequest;
using Forces.Application.Responses.Requets.MPR;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forces.Application.Enums;
using Forces.Application.Requests.Requests;
using Forces.Application.Features.MPRDashboard.Query.GetMPRData;

namespace Forces.Application.Interfaces.Services
{
    public interface IRequestService<TType> : IService
        where TType : class
    {
        public Task<Result<string>> AddRequest(AddEditNPRRequest request);
        public Task<Result<int>> GetAllRequestsCount(GetRequestsBySpecificationsRequest Specifications);
        public Task<Result<List<ActionsType>>> GetAvilableActions();
        public Task<Result<List<GetAllMPRResponse>>> GetAllRequestsByUser(string UserId);
        public Task<Result<List<GetAllMPRResponse>>> GetAllRequestsForTargetUser(string UserId);
        public Task<Result<List<GetAllMPRResponse>>> GetAllRequestsToLog();
        public Task<Result<List<GetAllMPRResponse>>> GetAllRequestsBySpecifications(GetRequestsBySpecificationsRequest Specifications);
        public Task<Result<GetAllMPRResponse>> GetAllRequestsById(int requestId);
        public Task<Result<GetAllMPRResponse>> GetAllRequestsByRefrance(string Refrance);
        public Task<Result<bool>> SubmitAction(ActionRequest action);
        public Task<Result<List<GetAllMPRResponse>>> GetAllRequestsBySteps(RequestSteps Step);
        public Task<Result<GetMPRDashboardDataResponse>> GetMPRDashboardData();
    }
}
