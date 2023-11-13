using Forces.Application.Features.AirCraft.Commands.AddEdit;
using Forces.Application.Features.AirCraft.Commands.Delete;
using Forces.Application.Features.AirCraft.Queries.GetAll;
using Forces.Application.Features.AirCraft.Queries.GetByAirKindId;
using Forces.Application.Features.AirCraft.Queries.GetById;
using Forces.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.AirCraft
{
    public class AirCraftController : BaseApiController<AirCraftController>
    {
        private protected readonly ICurrentUserService _currentUserService;
        public AirCraftController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }
        /// <summary>
        /// Add/Edit a AirCraft
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Products.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditAirCraftCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        // <summary>
        /// Delete a AirCraft
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        // [Authorize(Policy = Permissions.Products.Delete)]
        [HttpDelete("{AirCraftid}")]
        public async Task<IActionResult> Delete(int AirCraftid)
        {
            return Ok(await _mediator.Send(new DeleteAirCraftCommand { Id = AirCraftid }));
        }
        /// <summary>
        /// Get All AirCraft
        /// </summary>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Products.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var AirCrafts = await _mediator.Send(new GetAllAirCraftQuery(_currentUserService.UserId));
            return Ok(AirCrafts);
        }

        /// <summary>
        /// Get All AirCraft By Kind Id
        /// </summary>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Products.View)]
        [HttpGet("Kind/{Id}")]
        public async Task<IActionResult> GetAll(int Id)
        {
            var AirCrafts = await _mediator.Send(new GetAllAirCraftByAirKindIdQuery() { AirCraftId=Id});
            return Ok(AirCrafts);
        }

        /// <summary>
        /// Get All AirCraft By Id
        /// </summary>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Products.View)]
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(int Id)
        {
            var AirCrafts = await _mediator.Send(new GetAirCraftByIdQuery() { Id = Id });
            return Ok(AirCrafts);
        }

    }
}
