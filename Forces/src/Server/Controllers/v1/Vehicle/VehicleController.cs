using Forces.Application.Features.Vehicle.Commands.AddEdit;
using Forces.Application.Features.Vehicle.Commands.Delete;
using Forces.Application.Features.Vehicle.Queries.Dashboard;
using Forces.Application.Features.Vehicle.Queries.GetAll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.Vehicle
{

    public class VehicleController : BaseApiController<VehicleController>
    {
        /// <summary>
        /// Add/Edit a Vehicle
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Products.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditVehicleCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        // <summary>
        /// Delete a Vehicle
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        // [Authorize(Policy = Permissions.Products.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteVehicleCommand { VehicleID = id }));
        }
        /// <summary>
        /// Get All Vehicles
        /// </summary>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Products.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Vehicles = await _mediator.Send(new GetAllVehicleQuery());
            return Ok(Vehicles);
        }

        /// <summary>
        /// Get All Vehicles Dashboard Data
        /// </summary>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Products.View)]
        [HttpGet("Dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var Data = await _mediator.Send(new VehicleDashboardQuery());
            return Ok(Data);
        }
    }
}
