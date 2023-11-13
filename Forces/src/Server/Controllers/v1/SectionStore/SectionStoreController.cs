using Forces.Application.Features.SectionStore.Commands.AddEdit;
using Forces.Application.Features.SectionStore.Commands.Delete;
using Forces.Application.Features.SectionStore.Queries.GetAll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.SectionStore
{

    public class SectionStoreController : BaseApiController<SectionStoreController>
    {
        /// <summary>
        /// Add/Edit a Store
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Products.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditSectionStoreCommand command)
        {
            return Ok(await _mediator.Send(command));
        }


        // <summary>
        /// Delete a Store
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
       // [Authorize(Policy = Permissions.Products.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteSectionStoreCommand { StoreCode = id }));
        }

        /// <summary>
        /// Get All Store
        /// </summary>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Products.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var SectionStore = await _mediator.Send(new GetAllSectionStoreQuery());
            return Ok(SectionStore);
        }
    }
}
