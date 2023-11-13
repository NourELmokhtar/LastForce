using Forces.Application.Enums;
using Forces.Application.Features.MPRDashboard.Query.GetMPRData;
using Forces.Application.Requests.Requests;
using Forces.Application.Requests.Requests.NPRRequest;
using Forces.Application.Responses.Requets.MPR;
using Forces.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Requests.NPR
{
    public interface INprManager : IManager
    {
        Task<IResult<string>> SaveAsync(AddEditNPRRequest request);
        public Task<IResult<int>> GetAllRequestsCount(GetRequestsBySpecificationsRequest Specifications);

        public Task<IResult<List<GetAllMPRResponse>>> GetAllRequestsByUser();
        public Task<IResult<List<GetAllMPRResponse>>> GetAllRequestsForTargetUser();
        public Task<IResult<List<GetAllMPRResponse>>> GetAllRequestsToLog();
        public Task<IResult<List<GetAllMPRResponse>>> GetAllRequestsBySpecifications(GetRequestsBySpecificationsRequest Specifications);
        public Task<IResult<GetAllMPRResponse>> GetAllRequestsById(int requestId);
        public Task<IResult<GetAllMPRResponse>> GetAllRequestsByRefrance(string Refrance);
        public Task<IResult<List<ActionsType>>> GetAvilableActions();
        public Task<IResult<List<GetAllMPRResponse>>> GetAllRequestsBySteps(RequestSteps Step);
        public Task<IResult<bool>> SubmitAction(ActionRequest action);
        public Task<IResult<GetMPRDashboardDataResponse>> GetDashboardData();
    }
}
