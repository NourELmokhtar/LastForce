using Forces.Application.Features.DepoDepartment.Commands.AddEdit;
using Forces.Application.Features.DepoDepartment.Commands.Delete;
using Forces.Application.Features.DepoDepartment.Queries.GetAll;
using Forces.Application.Features.DepoDepartment.Queries.GetByForceId;
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

    public class DepoController : BaseApiController<DepoController>
    {
        private readonly ICurrentUserService _currentUser;

        public DepoController(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }

        /// <summary>
        /// Create/Update a Depo Department With Permission Create Depo Department
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.DepoManagement.CreateDepartments)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditDepoCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        /// <summary>
        /// Get All Depo Departments With Permission View Depo Department
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.DepoManagement.ViewDepartments)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var depos = await _mediator.Send(new GetAllDepoDepartmentQuery() { UserId = _currentUser.UserId });
            return Ok(depos);
        }
        /// <summary>
        /// Get All Depo Department Without Permission Depo Department At All
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet("GetAllDepos")]
        public async Task<IActionResult> GetAllForces()
        {
            var depos = await _mediator.Send(new GetAllDepoDepartmentQuery() { UserId = _currentUser.UserId });
            return Ok(depos);
        }
        /// <summary>
        /// Get All Depo Departments By ForceID Without Permission Depo Department At All
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet("Force/{id}")]
        public async Task<IActionResult> GetAllDepos(int id)
        {
            var depos = await _mediator.Send(new GetAllDepoByForceIdQuery { ForceId = id });
            return Ok(depos);
        }
        /// <summary>
        /// Delete a Depo Department With Permission Delete Depo Department
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.DepoManagement.DeleteDepartments)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteDepoCommand { Id = id }));
        }

    }
}
