using Forces.Application.Features.PersonalItems.Commands.AddEdit;
using Forces.Application.Features.PersonalItems.Commands.Delete;
using Forces.Application.Features.PersonalItems.Queries.GetAll;
using Forces.Application.Features.PersonalItems.Queries.GetByFillter;
using Forces.Application.Features.PersonalItems.Queries.GetById;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.PersonalItems
{

    public class PersonalItemsController : BaseApiController<PersonalItemsController>
    {
        /// <summary>
        /// Create/Update a Personal Item With Permission Create Personal Service Items
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.PersonalServicesItems.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditPersonalItemCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Get All Personal Items Without Any Permission 
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Items = await _mediator.Send(new GetAllPersonalItemQuery());
            return Ok(Items);
        }

        /// <summary>
        /// Get All Personal Items By Id ,Name , ArName , Code , NSN  Without Any Permission
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 Ok</returns>
        [HttpPost("Get")]
        public async Task<IActionResult> GetByConditions(GetPersonalItemsByFillter command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Personal Item With Permission Delete Items
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.PersonalServicesItems.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeletePersonalItemCommand { PersonalItemId = id }));
        }

        /// <summary>
        /// Get a Personal Item By Id Without Any Permission 
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet("Get/{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var Item = await _mediator.Send(new GetPersonalItemByIdQueiry() { PersonalItemId = Id });
            return Ok(Item);
        }
    }
}
