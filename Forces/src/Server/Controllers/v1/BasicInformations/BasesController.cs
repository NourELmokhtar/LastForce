using Forces.Application.Features.Bases.Commands.AddEdit;
using Forces.Application.Features.Bases.Commands.Delete;
using Forces.Application.Features.Bases.Queries.GetAll;
using Forces.Application.Features.Bases.Queries.GetByForceId;
using Forces.Application.Features.Bases.Queries.GetById;
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

    public class BasesController : BaseApiController<BasesController>
    {
        private readonly ICurrentUserService _currentUser;

        public BasesController(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }

        /// <summary>
        /// Create/Update a Base With Permission Create Bases
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.BasicInformations.CreateBases)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditBaseCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        /// <summary>
        /// Get All Bases With Permission View Bases
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.BasicInformations.ViewBases)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bases = await _mediator.Send(new GetAllBasesQuery(_currentUser.UserId));
            return Ok(bases);
        }
        /// <summary>
        /// Get All Bases Without Permission Bases At All
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet("GetAllBases")]
        public async Task<IActionResult> GetAllBases()
        {
            var bases = await _mediator.Send(new GetAllBasesQuery(_currentUser.UserId));
            return Ok(bases);
        }
        /// <summary>
        /// Get a Base By Id Without Any Permission
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Base = await _mediator.Send(new GetBaseByIdQuery() { Id = id });
            return Ok(Base);
        }

        /// <summary>
        /// Delete a Base With Permission Delete Bases
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.BasicInformations.DeleteBases)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteBaseCommand { Id = id }));
        }

        /// <summary>
        /// Get a Base By Force Id Without Any Permission 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>

        [HttpGet("force/{id}")]
        public async Task<IActionResult> GetByForceId(int id)
        {
            var Bases = await _mediator.Send(new GetAllBasesByForceIdQuery() { Id = id, CurrentUserID = _currentUser.UserId });
            return Ok(Bases);
        }

    }
}
