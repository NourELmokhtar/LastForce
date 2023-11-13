using Forces.Application.Interfaces.Services;
using Forces.Application.Requests.VoteCodes;
using Forces.Application.Responses.VoteCodes;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.v1.VoteCode
{

    public class VoteCodesController : BaseApiController<VoteCodesController>
    {
        private readonly IVoteCodeService _voteCodeService;
        private readonly ICurrentUserService _currentUser;

        public VoteCodesController(IVoteCodeService voteCodeService, ICurrentUserService currentUser)
        {
            _voteCodeService = voteCodeService;
            _currentUser = currentUser;
        }


        /// <summary>
        /// Create/Update a Vote Code With Permission Create VoteCodes
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.VoteCodes.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditVoteCodeRequest command)
        {
            return Ok(await _voteCodeService.AddEditVoteCode(command));
        }

        /// <summary>
        /// Search For Vote Code Logs By Specifications
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [HttpPost("GetLogsSpec")]
        public async Task<IActionResult> GetLogsSpec(VoteCodeLogSpecificationRequest command)
        {
            return Ok(await _voteCodeService.GetLogBySpecification(command));
        }
        /// <summary>
        /// Get All Vote Codes Without Any Permission 
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _voteCodeService.GetAllCodes());
        }
        /// <summary>
        /// Get All Vote Codes By UserId Without Any Permission 
        /// </summary>
        /// <param name="userID"/>
        /// <returns>Status 200 OK</returns>
        // [Authorize(Policy = Permissions.BasicInformations.CreateBases)]
        [HttpGet("{userID}")]
        public async Task<IActionResult> GetAllByUserId(string userID)
        {
            return Ok(await _voteCodeService.GetCodesByUserId(userID));
        }
        /// <summary>
        /// Get All Vote Code Logs By Vote Code's Id Without Any Permission 
        /// </summary>
        /// <param name="Id"/>
        /// <returns>Status 200 OK</returns>
        [HttpGet("vCodeLogs/{Id}")]
        public async Task<IActionResult> GetvCodeLogs(int Id)
        {
            return Ok(await _voteCodeService.GetvCodeLogs(Id));
        }

        /// <summary>
        /// Get a Vote Code Log By Vote Code Log's Id Without Any Permission 
        /// </summary>
        /// <param name="Id"/>
        /// <returns>Status 200 OK</returns>
        [HttpGet("LogDetails/{Id}")]
        public async Task<IActionResult> GetLogDetails(int Id)
        {
            return Ok(await _voteCodeService.GetLogDetails(Id));
        }

        /// <summary>
        /// Add Or Edit Vote Code Transaction Log Without Any Permission 
        /// </summary>
        /// <param name="command"/>
        /// <returns>Status 200 OK</returns>
        [HttpPost("AddEditTransaction")]
        public async Task<IActionResult> AddEditTransaction(AddEditVcodeTransactionRequest command)
        {
            return Ok(await _voteCodeService.AddEditTransaction(command));
        }

        /// <summary>
        /// Get a Vote Code Cridet By Vote Code Id Without Any Permission 
        /// </summary>
        /// <param name="Id"/>
        /// <returns>Status 200 OK</returns>
        [HttpGet("VoteCodeCredit/{Id}")]
        public async Task<IActionResult> GetVoteCodeCredit(int Id)
        {
            return Ok(await _voteCodeService.GetVoteCodeCredit(Id));
        }

        /// <summary>
        /// Get All Vote Codes By UserId Without Any Permission 
        /// </summary>
        /// <returns>Status 200 OK</returns>
        // [Authorize(Policy = Permissions.BasicInformations.CreateBases)]
        [HttpGet("GetAllByCurrentUser")]
        public async Task<IActionResult> GetAllByCurrentUser()
        {
            return Ok(await _voteCodeService.GetCodesByUserId(_currentUser.UserId));
        }

        /// <summary>
        /// Get All Vote Codes By UserId Without Any Permission 
        /// </summary>
        /// <returns>Status 200 OK</returns>
        // [Authorize(Policy = Permissions.BasicInformations.CreateBases)]
        [HttpGet("Get/{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            return Ok(await _voteCodeService.RGetCodeBy(Id));
        }

        /// <summary>
        /// Export Transaction Logs to Excel
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        [HttpPost("export")]
        public async Task<IActionResult> Export(List<VoteCodeLogResponse> Data)
        {
            return Ok(await _voteCodeService.ExportLog(Data));
        }
    }
}
