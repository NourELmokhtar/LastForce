using Forces.Application.Features.House.Commands.AddEdit;
using Forces.Application.Features.House.Commands.Delete;
using Forces.Application.Features.House.Queries.GetAll;
using Forces.Application.Features.House.Queries.GetBySpecifics;
using Forces.Application.Features.House.Commands.AddEdit;
using Forces.Application.Features.House.Commands.Delete;
using Forces.Application.Features.House.Queries.GetAll;
using Forces.Application.Features.House.Queries.GetBySpecifics;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polly;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.House
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseController : BaseApiController<HouseController>
    {
        /// <summary>
        /// Add/Edit a House
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.House.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditHouseCommand command)
        {
            return Ok(await _mediator.Send(command));
        }


        // <summary>
        /// Delete an House
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.House.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteHouseCommand { HouseId = id }));
        }
        /// <summary>
        /// Get Houses By Specifics
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.House.View)]
        [HttpGet("Filter")]
        public async Task<IActionResult> GetBy(GetHouseByQuery command)
        {
            var Houses = await _mediator.Send(command);
            return Ok(Houses);
        }

        /// <summary>
        /// Get All Houses
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.House.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Houses = await _mediator.Send(new GetAllHouseQuery());
            return Ok(Houses);
        }
    }

}
