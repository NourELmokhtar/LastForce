using Forces.Application.Features.HQDepartment.Commands.AddEdit;
using Forces.Application.Features.HQDepartment.Commands.Delete;
using Forces.Application.Features.HQDepartment.Queries.GetAll;
using Forces.Application.Features.HQDepartment.Queries.GetByForceId;
using Forces.Application.Interfaces.Services;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.BasicInformations
{

    public class HQController : BaseApiController<HQController>
    {
        private readonly ICurrentUserService _currentUser;

        public HQController(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }

        /// <summary>
        /// Create/Update a HQ Department With Permission Create HQ Department
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.HQManagement.CreateDepartments)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditHQCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        /// <summary>
        /// Get All HQ Departments With Permission View HQ Department
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.HQManagement.ViewDepartments)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var HQs = await _mediator.Send(new GetAllHQDepartmentQuery() { UserId = _currentUser.UserId });
            return Ok(HQs);
        }
        /// <summary>
        /// Get All HQ Department Without Permission HQ Department At All
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet("GetAllHQs")]
        public async Task<IActionResult> GetAllForces()
        {
            var HQs = await _mediator.Send(new GetAllHQDepartmentQuery() { UserId = _currentUser.UserId });
            return Ok(HQs);
        }

        /// <summary>
        /// Get All HQ Departments By ForceID Without Permission HQ Department At All
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet("Force/{id}")]
        public async Task<IActionResult> GetAllHQs(int id)
        {
            var HQs = await _mediator.Send(new GetAllHQbyForceIdQuery { ForceId = id });
            return Ok(HQs);
        }

        /// <summary>
        /// Delete a HQ Department With Permission Delete HQ Department
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.HQManagement.DeleteDepartments)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteHQCommand { Id = id }));
        }

    }
}
