using Forces.Application.Features.RackStore.Commands.AddEdit;
using Forces.Application.Features.RackStore.Commands.Delete;
using Forces.Application.Features.RackStore.Queries.GetAll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.RackStore
{

    public class RackStoreController : BaseApiController<RackStoreController>
    {
        /// <summary>
        /// Add/Edit a Rack
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Products.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditRackStoreCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        // <summary>
        /// Delete a Rack
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        // [Authorize(Policy = Permissions.Products.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int RackCode)
        {
            return Ok(await _mediator.Send(new DeleteRackStoreCommand { RackCode = RackCode }));
        }

        /// <summary>
        /// Get All RackStore
        /// </summary>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Products.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var RackStore = await _mediator.Send(new GetAllRackStoreQuery());
            return Ok(RackStore);
        }
    }
}