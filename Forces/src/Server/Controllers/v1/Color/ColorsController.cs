using Forces.Application.Features.Color.Commands.AddEdit;
using Forces.Application.Features.Color.Commands.Delete;
using Forces.Application.Features.Color.Queries.GetAll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.Color
{

    public class ColorsController : BaseApiController<ColorsController>
    {

        /// <summary>
        /// Add/Edit a Color
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Products.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditColorCommand command)
        {
            return Ok(await _mediator.Send(command));
        }


        // <summary>
        /// Delete a Color
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
       // [Authorize(Policy = Permissions.Products.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteColorCommand { ColorID = id }));
        }

        /// <summary>
        /// Get All Colors
        /// </summary>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Products.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Colors = await _mediator.Send(new GetAllColorQuery());
            return Ok(Colors);
        }
    }
}
