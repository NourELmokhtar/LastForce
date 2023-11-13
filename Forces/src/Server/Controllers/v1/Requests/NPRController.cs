using Forces.Application.Enums;
using Forces.Application.Interfaces.Services;
using Forces.Application.Requests.Requests;
using Forces.Application.Requests.Requests.NPRRequest;
using Forces.Infrastructure.Models.Requests;
using Forces.Infrastructure.Services.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.Requests
{

    public class NPRController : BaseApiController<NPRController>
    {
        private readonly IRequestService<RequestService> _requestService;
        private readonly ICurrentUserService _currentUserService;

        public NPRController(IRequestService<RequestService> requestService, ICurrentUserService currentUserService)
        {
            _requestService = requestService;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddEditNPRRequest command)
        {
            return Ok(await _requestService.AddRequest(command));
        }
        [HttpGet("GetAllRequestsForUser")]
        public async Task<IActionResult> GetAll()
        {

            return Ok(await _requestService.GetAllRequestsByUser(_currentUserService.UserId));
        }
        [HttpPost("GetAllRequestsCount")]
        public async Task<IActionResult> GetCount(GetRequestsBySpecificationsRequest Specifications)
        {

            return Ok(await _requestService.GetAllRequestsCount(Specifications));
        }
        [HttpGet("GetAllRequestsForTargetUser")]
        public async Task<IActionResult> GetAllRequestsForTargetUser()
        {
            return Ok(await _requestService.GetAllRequestsForTargetUser(_currentUserService.UserId));
        }

        [HttpGet("GetAllRequestsToLog")]
        public async Task<IActionResult> GetAllRequestsToLog()
        {
            return Ok(await _requestService.GetAllRequestsToLog());
        }

        [HttpPost("GetAllRequestsBySpecifications")]
        public async Task<IActionResult> GetAllRequestsBySpecifications(GetRequestsBySpecificationsRequest Specifications)
        {
            return Ok(await _requestService.GetAllRequestsBySpecifications(Specifications));
        }

        [HttpGet("GetRequetById/{Id}")]
        public async Task<IActionResult> GetRequetById(int Id)
        {
            return Ok(await _requestService.GetAllRequestsById(Id));
        }

        [HttpGet("GetRequetByRef/{Ref}")]
        public async Task<IActionResult> GetRequetByRefrance(string Ref)
        {
            return Ok(await _requestService.GetAllRequestsByRefrance(Ref));
        }

        [HttpGet("GetRequetByStep/{Step}")]
        public async Task<IActionResult> GetRequetByRefrance(RequestSteps Step)
        {
            return Ok(await _requestService.GetAllRequestsBySteps(Step));
        }
        [HttpGet("GetAvilableActions")]
        public async Task<IActionResult> GetAvilableActions()
        {
            return Ok(await _requestService.GetAvilableActions());
        }

        [HttpPost("SubmitAction")]
        public async Task<IActionResult> SubmitAction(ActionRequest request)
        {
            return Ok(await _requestService.SubmitAction(request));
        }

        [HttpGet("DashboardData")]
        public async Task<IActionResult> DashboardData()
        {
            return Ok(await _requestService.GetMPRDashboardData());
        }

    }
}
