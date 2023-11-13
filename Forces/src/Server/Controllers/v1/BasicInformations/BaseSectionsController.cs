using Forces.Application.Features.BaseSections.Commands.AddEdit;
using Forces.Application.Features.BaseSections.Commands.Delete;
using Forces.Application.Features.BaseSections.Queries.GetAll;
using Forces.Application.Features.BaseSections.Queries.GetByBaseId;
using Forces.Application.Features.BaseSections.Queries.GetById;
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

    public class BaseSectionsController : BaseApiController<BaseSectionsController>
    {
        private readonly ICurrentUserService _currentUser;

        public BaseSectionsController(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }
        /// <summary>
        /// Create/Update a Base Section With Permission Create Bases Section 
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.BasicInformations.CreateBasesSection)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditBaseSectionCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        /// <summary>
        /// Get All Bases Section With Permission View Bases Section
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.BasicInformations.ViewBasesSection)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var BasesSections = await _mediator.Send(new GetAllBasesSectionsQuery(_currentUser.UserId));
            return Ok(BasesSections);
        }
        /// <summary>
        /// Get All Bases Without Permission Bases Section At All
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet("GetAllBaseSections")]
        public async Task<IActionResult> GetAllSections()
        {
            var BasesSections = await _mediator.Send(new GetAllBasesSectionsQuery(_currentUser.UserId));
            return Ok(BasesSections);
        }
        /// <summary>
        /// Get a Base Section By Id With Permission View Bases Section
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.BasicInformations.ViewBasesSection)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var BasesSection = await _mediator.Send(new GetBaseSectionByIdQuery() { Id = id });
            return Ok(BasesSection);
        }

        /// <summary>
        /// Delete a Base Section With Permission Delete Bases
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.BasicInformations.DeleteBasesSection)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteBaseSectionCommand { Id = id }));
        }

        /// <summary>
        /// Get a Sections By Base Id Without Any Permission  
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>

        [HttpGet("Base/{id}")]
        public async Task<IActionResult> GetByBaseId(int id)
        {
            var BasesSections = await _mediator.Send(new GetAllSectionsByBaseIdQuery() { Id = id });
            return Ok(BasesSections);
        }
    }
}
