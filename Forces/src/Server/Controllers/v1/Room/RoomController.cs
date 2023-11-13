using Forces.Application.Features.Room.Commands.AddEdit;
using Forces.Application.Features.Room.Commands.Delete;
using Forces.Application.Features.Room.Queries.GetAll;
using Forces.Application.Features.Room.Queries.GetBySpecifics;
using Forces.Server.Controllers.v1.Room;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.Room
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : BaseApiController<RoomController>
    {
        /// <summary>
        /// Add/Edit a Room
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Rooms.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditRoomCommand command)
        {
            return Ok(await _mediator.Send(command));
        }


        // <summary>
        /// Delete an Room
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
       [Authorize(Policy = Permissions.Rooms.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteRoomCommand { RoomId = id }));
        }
        /// <summary>
        /// Get Rooms By Specifics
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Rooms.View)]
        [HttpGet("Filter")]
        public async Task<IActionResult> GetBy(GetRoomByQuery command)
        {
            var Rooms = await _mediator.Send(command);
            return Ok(Rooms);
        }

        /// <summary>
        /// Get All Rooms
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Rooms.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Rooms = await _mediator.Send(new GetAllRoomQuery());
            return Ok(Rooms);
        }
    }
}
