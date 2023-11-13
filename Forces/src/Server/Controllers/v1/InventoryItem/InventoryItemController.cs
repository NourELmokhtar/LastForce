using Forces.Application.Features.InventoryInventoryItem.Commands.AddEdit;
using Forces.Application.Features.InventoryItem.Commands.Delete;
using Forces.Application.Features.InventoryItem.Queries.GetAll;
using Forces.Application.Features.InventoryItem.Queries.GetBySpecifics;
using Forces.Server.Controllers.v1.InventoryItem;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.InventoryItem
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemController : BaseApiController<InventoryItemController>
    {
        /// <summary>
        /// Add/Edit a InventoryItem
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.InventoryItems.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditInventoryItemCommand command)
        {
            return Ok(await _mediator.Send(command));
        }


        // <summary>
        /// Delete an InventoryItem
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.InventoryItems.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteInventoryItemCommand { InventoryItemId = id }));
        }
        /// <summary>
        /// Get Inventory Items By Specifics
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.InventoryItems.View)]
        [HttpGet("Filter")]
        public async Task<IActionResult> GetBy(GetInventoryItemByQuery command)
        {
            var Inventories = await _mediator.Send(command);
            return Ok(Inventories);
        }

        /// <summary>
        /// Get All Inventories
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.InventoryItems.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Inventories = await _mediator.Send(new GetAllInventoryItemQuery());
            return Ok(Inventories);
        }
    }
}
