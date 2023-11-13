using Forces.Application.Features.InventoryInventoryItem.Commands.AddEdit;
using Forces.Application.Features.InventoryItemBridge.Commands.AddEdit;
using Forces.Application.Features.InventoryItemBridge.Queries.GetAll;
using Forces.Application.Features.Items.Queries.GetAll;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.InventoryItemBridge
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemBridgeController : BaseApiController<InventoryItemBridgeController>
    {
        [HttpPost]
        [Authorize(Policy = Permissions.InventoryItemsBridge.Create)]
        public async Task<IActionResult> Post(AddEditInventoryItemBridgeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet]
        [Authorize(Policy = Permissions.InventoryItemsBridge.View)]

        public async Task<IActionResult> GetAll()
        {
            var Items = await _mediator.Send(new GetAllInventoryItemBridgeQuery());
            return Ok(Items);
        }
    }
}
