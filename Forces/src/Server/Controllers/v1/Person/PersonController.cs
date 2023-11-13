using Forces.Application.Features.Person.Commands.AddEdit;
using Forces.Application.Features.Person.Commands.Delete;
using Forces.Application.Features.Person.Queries.GetAll;
using Forces.Application.Features.Person.Queries.GetBySpecifics;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polly;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.Person
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : BaseApiController<PersonController>
    {
        [Authorize(Policy = Permissions.Person.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditPersonCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet]

        public async Task<IActionResult> GetALl()
        {
            var Persons = await _mediator.Send(new GetAllPersonsQuery());
            return Ok(Persons);
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetByCondition(GetPersonByQuery command)
        {
            var Persons = await _mediator.Send(command);
            return Ok(Persons);
        }

        [Authorize(Policy = Permissions.Person.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(DeletePersonCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
