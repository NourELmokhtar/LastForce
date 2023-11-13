using Forces.Application.Features.Inventory.Commands.AddEdit;
using Forces.Application.Features.Inventory.Commands.Delete;
using Forces.Application.Features.Inventory.Queries.GetAll;
using Forces.Application.Features.Inventory.Queries.GetBySpecifics;
using Forces.Server.Controllers.v1.Color;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.Inventory
{
 
    public class InventoryController : BaseApiController<InventoryController>
    {
        /// <summary>
        /// Add/Edit a Inventory
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Inventory.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditInventoryCommand command)
        {
            return Ok(await _mediator.Send(command));
        }


        // <summary>
        /// Delete an Inventory
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
       [Authorize(Policy = Permissions.Inventory.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteInventoryCommand { InventoryId = id }));
        }
        /// <summary>
        /// Get Inventories By Specifics
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Inventory.View)]
        [HttpGet("Filter")]
        public async Task<IActionResult> GetBy(GetInventoryByQuery command)
        {
            var Inventories = await _mediator.Send(command);
            return Ok(Inventories);
        }

        /// <summary>
        /// Get All Inventories
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Inventory.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Inventories = await _mediator.Send(new GetAllInventoryQuery());
            return Ok(Inventories);
        }
    }
}
