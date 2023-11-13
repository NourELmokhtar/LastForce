using Forces.Application.Features.Tailers.Commands.AddEdit;
using Forces.Application.Features.Tailers.Commands.Delete;
using Forces.Application.Features.Tailers.Queries.GetAll;
using Forces.Application.Interfaces.Services;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.Tailer
{

    public class TailerController : BaseApiController<TailerController>
    {
        private readonly ICurrentUserService _currentUser;
        public TailerController(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }
        /// <summary>
        /// Create/Update a Tailer With Permission Create Tailers
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Tailer.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditTailerCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        /// <summary>
        /// Get All Tailers Without Any Permission  
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Tailers = await _mediator.Send(new GetAllTailersQuery(_currentUser.UserId));
            return Ok(Tailers);
        }

        /// <summary>
        /// Delete a Tailer With Permission Delete Tailer
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Tailer.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteTailerCommand { Id = id }));
        }
    }
}
