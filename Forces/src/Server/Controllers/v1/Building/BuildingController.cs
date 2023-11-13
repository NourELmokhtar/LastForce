using Forces.Application.Features.Building.Commands.AddEdit;
using Forces.Application.Features.Building.Commands.Delete;
using Forces.Application.Features.Building.Queries.GetAll;
using Forces.Application.Features.Building.Queries.GetBySpecifics;
using Forces.Application.Features.Building.Commands.AddEdit;
using Forces.Application.Features.Building.Commands.Delete;
using Forces.Application.Features.Building.Queries.GetAll;
using Forces.Application.Features.Building.Queries.GetBySpecifics;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polly;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.Building
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController : BaseApiController<BuildingController>
    {
        /// <summary>
        /// Add/Edit a Building
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Building.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditBuildingCommand command)
        {
            return Ok(await _mediator.Send(command));
        }


        // <summary>
        /// Delete an Building
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Building.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteBuildingCommand { BuildingId = id }));
        }
        /// <summary>
        /// Get Buildings By Specifics
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Building.View)]
        [HttpGet("Filter")]
        public async Task<IActionResult> GetBy(GetBuildingsByQuery command)
        {
            var Buildings = await _mediator.Send(command);
            return Ok(Buildings);
        }

        /// <summary>
        /// Get All Buildings
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Building.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Buildings = await _mediator.Send(new GetAllBuildingsQuery());
            return Ok(Buildings);
        }
    }
}

