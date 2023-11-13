using Forces.Application.Features.MprRequest.Command;
using Forces.Application.Features.MprRequest.Command.Actions;
using Forces.Application.Features.MprRequest.Query.GetRequestById;
using Forces.Application.Features.MprRequest.Query.GetRequests;
using Forces.Application.Features.MprRequest.Query.GetRequestsByVoteCodeId;
using Forces.Application.Requests.Requests.NPRRequest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.Requests
{
 
    public class MPRController : BaseApiController<MPRController>
    {
        [HttpPost]
        public async Task<IActionResult> Post(AddMprRequestCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpGet]
        public async Task<IActionResult> GetRequests() 
        {
            return Ok (await _mediator.Send(new GetRequestsQuery()));
        }

        [HttpGet("GetRequetById/{Id}")]
        public async Task<IActionResult> GetRequetById(int Id)
        {
            return Ok(await _mediator.Send(new GetRequestByIdQuery() { Id = Id}));
        }
        [HttpPost("Action/reject")]
        public async Task<IActionResult> RejectAction(RejectActionCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpPost("Action/Cancel")]
        public async Task<IActionResult> CancelAction(CancelActionCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpPost("Action/Esclate")]
        public async Task<IActionResult> EsclateAction(SclateActionCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpPost("Action/redirect")]
        public async Task<IActionResult> RedirectAction(RedirectActionCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpPost("Action/submit")]
        public async Task<IActionResult> SubmitAction(SubmitActionCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpPost("Action/Edit")]
        public async Task<IActionResult> EditAction(EditActionCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpPost("Action/SelectQutaion")]
        public async Task<IActionResult> SelectQutaionAction(SelectQutationActionCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("Action/SubmitPay/{Id}")]
        public async Task<IActionResult> SubmitPayment(int Id)
        {
            return Ok(await _mediator.Send(new SubmitPaymentCommand() { RequestId = Id }));
        }

        [HttpGet("Action/ConfirmPay/{Id}")]
        public async Task<IActionResult> ConfirmPayment(int Id)
        {
            return Ok(await _mediator.Send(new ConfirmPaymentCommand() { RequestId = Id }));
        }

        [HttpGet("Requests/Votecode/{Id}")]
        public async Task<IActionResult> RequestsByVoteCode(int Id)
        {
            return Ok(await _mediator.Send(new GetRequestsByVoteCodeIdQuery() { Id = Id }));
        }
    }
}
