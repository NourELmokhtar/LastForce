using Forces.Application.Features.PersonalItemOperations.Commands.AddEdit;
using Forces.Application.Features.PersonalItemOperations.Queries.Eligibility;
using Forces.Application.Features.PersonalItemOperations.Queries.GetByFillter;
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

    public class PersonalItemsOperationsController : BaseApiController<PersonalItemsController>
    {
        /// <summary>
        /// Create/Update a Personal Item Operation With Permission Create Personal Items Operation
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.PersonalItemsOperations.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditPersonalItemOperationCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Get All Personal Items Operations With Permission View PersonalItem Operation 
        /// </summary>
        /// <returns>Status 200 OK</returns>
        /// 
        [Authorize(Policy = Permissions.PersonalItemsOperations.View)]
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(GetPersonalItemsOperationsByFillter command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Get All Personal Items Operations With Permission Search PersonalItem Operation 
        /// </summary>
        /// <returns>Status 200 OK</returns>
        /// 
        [Authorize(Policy = Permissions.PersonalItemsOperations.Search)]
        [HttpPost("Get")]
        public async Task<IActionResult> GetbyCondition(GetPersonalItemsOperationsByFillter command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Check User Aligbility For An Item Without Any Permissions
        /// </summary>
        /// <returns>Status 200 OK</returns>
        /// 

        [HttpPost("Check")]
        public async Task<IActionResult> Check(ItemEligibilityQuery command)
        {
            return Ok(await _mediator.Send(command));
        }

    }
}
