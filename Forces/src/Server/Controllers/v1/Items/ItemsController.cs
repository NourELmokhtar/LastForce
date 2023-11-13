using Forces.Application.Features.Items.Commands.AddEdit;
using Forces.Application.Features.Items.Commands.Delete;
using Forces.Application.Features.Items.Queries.GetAll;
using Forces.Application.Features.Items.Queries.GetBySpecifics;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.Items
{

    public class ItemsController : BaseApiController<ItemsController>
    {
        /// <summary>
        /// Create/Update a Item With Permission Create Items
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Items.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditItemCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        /// <summary>
        /// Get All Items Without Any Permission 
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Items = await _mediator.Send(new GetAllItemsQuery());
            return Ok(Items);
        }
        /// <summary>
        /// Get a Items By Id ,Name , ArName , Code , NSN Or Measure Unit Without Any Permission
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 Ok</returns>
        [HttpPost("Get")]
        public async Task<IActionResult> GetByConditions(GetAllItemsBy command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete an Item With Permission Delete Items
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Items.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteItemCommand { ItemId = id }));
        }
    }
}
