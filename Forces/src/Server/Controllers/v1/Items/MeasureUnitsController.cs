using Forces.Application.Features.MeasureUnits.Commands.AddEdit;
using Forces.Application.Features.MeasureUnits.Commands.Delete;
using Forces.Application.Features.MeasureUnits.Queries.GetAll;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.Items
{

    public class MeasureUnitsController : BaseApiController<MeasureUnitsController>
    {
        /// <summary>
        /// Create/Update a Measure unit With Permission Create MeasureUnit
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.MeasureUnits.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditMeasureUnitsCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        /// <summary>
        /// Get All Measure Units Without Any Permission 
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Units = await _mediator.Send(new GetAllMeasureUnitsQuery());
            return Ok(Units);
        }
        /// <summary>
        /// Delete a Measure Unit With Permission Delete Measure Units
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.MeasureUnits.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteBinRackCmmand { Id = id }));
        }

    }
}
