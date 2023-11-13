using Forces.Application.Features.AirKind.Commands.AddEdit;
using Forces.Application.Features.AirKind.Commands.Delete;
using Forces.Application.Features.AirKind.Queries.GetAll;
using Forces.Application.Features.AirKind.Queries.GetByAirTypeld;
using Forces.Application.Features.AirKind.Queries.GetById;
using Forces.Application.Interfaces.Services;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.AirKind
{
    public class AirKindController : BaseApiController<AirKindController>
    {
        private readonly ICurrentUserService _currentUser;

        public AirKindController(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }
        /// <summary>
        /// Create/Update a AirKnid With Permission Create AirKnids
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
       // [Authorize(Policy = Permissions.AirKindOperations.CreateAirKind)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditAirKindCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        /// <summary>
        /// Get All AirKnid With Permission View AirKnids
        /// </summary>
        /// <returns>Status 200 OK</returns>
       // [Authorize(Policy = Permissions.AirKindOperations.ViewAirKind)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var airkind = await _mediator.Send(new GetAllAirKindQuery());
            return Ok(airkind);
        }
        /// <summary>
        /// Get All AirKind Without Permission AirKind At All
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet("GetAllAirKind")]

        public async Task<IActionResult> GetAllAirKind()
        {
            var airkind = await _mediator.Send(new GetAllAirKindQuery());
            return Ok(airkind);
        }
        /// <summary>
        /// Get a AirKind By Id Without Any Permission
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Status 200 Ok</returns>

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var AirKind = await _mediator.Send(new GetAirKindByIdQuery() { Id = Id });
            return Ok(AirKind);
        }

        /// <summary>
        /// Delete a AirKind With Permission Delete AirKind
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.AirKindOperations.DeleteAirKind)]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            return Ok(await _mediator.Send(new DeleteAirKindCommand { AirKindId = Id }));
        }
        /// <summary>
        /// Get a AirKind By AirType Id Without Any Permission 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>

        [HttpGet("AirType/{id}")]
        public async Task<IActionResult> GetByAirTypeId(int id)
        {
            var AirKinds = await _mediator.Send(new GetAllAirKindByAirTypeIdQuery() {TypeID = id});
            return Ok(AirKinds);

        }
    }
}
