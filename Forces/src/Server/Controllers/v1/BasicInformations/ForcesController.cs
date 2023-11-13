using Forces.Application.Features.Forces.Commands.AddEdit;
using Forces.Application.Features.Forces.Commands.Delete;
using Forces.Application.Features.Forces.Queries.GetAll;
using Forces.Application.Features.Forces.Queries.GetById;
using Forces.Application.Interfaces.Services;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.BasicInformations
{

    public class ForcesController : BaseApiController<ForcesController>
    {
        private readonly ICurrentUserService _currentUser;

        public ForcesController(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }

        /// <summary>
        /// Create/Update a Force With Permission Create Forces
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.BasicInformations.CreateForces)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditForceCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        /// <summary>
        /// Get All Forces With Permission View Forces
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.BasicInformations.ViewForces)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var forces = await _mediator.Send(new GetAllForcesQuery(_currentUser.UserId));
            return Ok(forces);
        }
        /// <summary>
        /// Get All Forces Without Permission Forces At All
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet("GetAllForces")]
        public async Task<IActionResult> GetAllForces()
        {
            var forces = await _mediator.Send(new GetAllForcesQuery(_currentUser.UserId));
            return Ok(forces);
        }
        /// <summary>
        /// Get a Force By Id With Permission View Forces
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.BasicInformations.ViewForces)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var force = await _mediator.Send(new GetForceByIdQuery() { Id = id });
            return Ok(force);
        }

        /// <summary>
        /// Delete a Force With Permission Delete Forces
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.BasicInformations.DeleteForces)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteForceCommand { Id = id }));
        }

    }
}
