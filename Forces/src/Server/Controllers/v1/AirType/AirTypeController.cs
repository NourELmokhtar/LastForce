using Forces.Application.Features.AirType.Commands.AddEdit;
using Forces.Application.Features.AirType.Commands.Delete;
using Forces.Application.Features.AirType.Queries.GetAll;
using Forces.Application.Features.AirType.Queries.GetById;
using Forces.Application.Interfaces.Services;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.AirType
{
    public class AirTypeController : BaseApiController<AirTypeController>
    {
        private readonly ICurrentUserService _currentUser;

        public AirTypeController(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }
        /// <summary>
        /// Create/Update a AirType With Permission Create AirType
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
       // [Authorize(Policy = Permissions.AirTypeOperations.CreateAirType)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditAirTypeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        /// <summary>
        /// Get All AirType With Permission View AirType
        /// </summary>
        /// <returns>Status 200 OK</returns>
       // [Authorize(Policy = Permissions.AirTypeOperations.ViewAirType)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var airkind = await _mediator.Send(new GetAllAirTypeQuery());
            return Ok(airkind);
        }
        /// <summary>
        /// Get All AirType Without Permission AirType At All
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet("GetAllAirType")]

        public async Task<IActionResult> GetAllAirTypeQuery()
        {
            var airtype = await _mediator.Send(new GetAllAirTypeQuery());
            return Ok(airtype);
        }
        /// <summary>
        /// Get a AirType By Id Without Any Permission
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Status 200 Ok</returns>

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var AirType = await _mediator.Send(new GetAirTypeByIdQuery() { Id = Id });
            return Ok(AirType);
        }

        /// <summary>
        /// Delete a AirType With Permission Delete AirType
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.AirTypeOperations.DeleteAirType)]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            return Ok(await _mediator.Send(new DeleteAirTypeCommand { Id = Id }));
        }

    }
}
