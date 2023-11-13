using Forces.Application.Features.BinRack.Commands.AddEdit;
using Forces.Application.Features.BinRack.Commands.Delete;
using Forces.Application.Features.BinRack.Queries.GetAll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.BinRack
{

    public class BinRackController : BaseApiController<BinRackController>
    {
        /// <summary>
        /// Add/Edit a Bin
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Products.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditBinRackCommand command)
        {
            return Ok(await _mediator.Send(command));
        }


        // <summary>
        /// Delete a Bin
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
       // [Authorize(Policy = Permissions.Products.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int BinCode)
        {
            return Ok(await _mediator.Send(new DeleteBinRackCommand { BinCode = BinCode }));

        }

        /// <summary>
        /// Get All BinRack
        /// </summary>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Products.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var BinRack = await _mediator.Send(new GetAllBinRackQuery());
            return Ok(BinRack);
        }
    }
}