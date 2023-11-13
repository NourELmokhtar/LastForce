using Forces.Application.Features.Office.Commands.AddEdit;
using Forces.Application.Features.Office.Commands.Delete;
using Forces.Application.Features.Office.Queries.GetAll;
using Forces.Application.Features.Office.Queries.GetAllBySpecifics;
using Forces.Server.Controllers.v1.Office;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.Office
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeController : BaseApiController<OfficeController>
    {
        /// <summary>
        /// Add/Edit a Office
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Office.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditOfficeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }


        // <summary>
        /// Delete an Office
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Office.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteOfficeCommand { OfficeId = id }));
        }
        /// <summary>
        /// Get Offices By Specifics
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Office.View)]
        [HttpGet("Filter")]
        public async Task<IActionResult> GetBy(GetOfficeByQuery command)
        {
            var Offices = await _mediator.Send(command);
            return Ok(Offices);
        }

        /// <summary>
        /// Get All Offices
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Office.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Offices = await _mediator.Send(new GetAllOfficeQuery());
            return Ok(Offices);
        }
    }
}
