using Forces.Application.Features.VehicleRequest.AddEditRequest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.VehicleRequests
{
    public class VehicleRequestController : BaseApiController<VehicleRequestController>
    {
        /// <summary>
        /// Add/Edit a Vehicle Request
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [HttpPost()]
        public async Task<IActionResult> AddEditRequest(AddEditVehicleRequest command) 
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
