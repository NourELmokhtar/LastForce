using Forces.Application.Interfaces.Services;
using Forces.Application.Interfaces.Services.Identity;
using Forces.Application.Requests.Identity;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Forces.Server.Controllers.Identity
{
    [Authorize]
    [Route("api/identity/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUser;
        public UserController(IUserService userService, ICurrentUserService currentUser)
        {
            _userService = userService;
            _currentUser = currentUser;
        }

        /// <summary>
        /// Get User Details
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Users.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }
        /// <summary>
        /// Get Vote Code Users Details
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet("Holders/{Id}")]
        public async Task<IActionResult> GetVoteCodeHoldersAll(int Id)
        {
            var users = await _userService.GetAllVoteCodeControllersAsync(Id);
            return Ok(users);
        }

        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Users.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userService.GetAsync(id);
            return Ok(user);
        }

        /// <summary>
        /// Get User Roles By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Users.View)]
        [HttpGet("roles/{id}")]
        public async Task<IActionResult> GetRolesAsync(string id)
        {
            var userRoles = await _userService.GetRolesAsync(id);
            return Ok(userRoles);
        }

        /// <summary>
        /// Update Roles for User
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Users.Edit)]
        [HttpPut("roles/{id}")]
        public async Task<IActionResult> UpdateRolesAsync(UpdateUserRolesRequest request)
        {
            return Ok(await _userService.UpdateRolesAsync(request));
        }

        /// <summary>
        /// Register a User
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _userService.RegisterAsync(request, origin));
        }
        /// <summary>
        /// Edit a User With Edit User Permission
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Users.Edit)]
        [HttpPost("edit-user")]
        public async Task<IActionResult> EditAsync(EditUserRequest request)
        {

            return Ok(await _userService.EditAsync(request));
        }
        /// <summary>
        /// Confirm Email
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns>Status 200 OK</returns>
        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        {
            return Ok(await _userService.ConfirmEmailAsync(userId, code));
        }

        /// <summary>
        /// Toggle User Status (Activate and Deactivate)
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Status 200 OK</returns>
        [HttpPost("toggle-status")]
        public async Task<IActionResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
        {
            return Ok(await _userService.ToggleUserStatusAsync(request));
        }

        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Status 200 OK</returns>
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _userService.ForgotPasswordAsync(request, origin));
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Status 200 OK</returns>
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            return Ok(await _userService.ResetPasswordAsync(request));
        }

        /// <summary>
        /// Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Users.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            var data = await _userService.ExportToExcelAsync(searchString);
            return Ok(data);
        }
        /// <summary>
        /// Return User Type
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet("GetUserType")]
        public async Task<IActionResult> GetUserType()
        {
            var userRoles = await _userService.GetCurrentUserTypeAsync(_currentUser.UserId);
            return Ok(userRoles);
        }

        /// <summary>
        /// Return Current User Notifications
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet("Notifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var userRoles = await _userService.GetNotificationByUserId();
            return Ok(userRoles);
        }

        /// <summary>
        /// Return Current User Notifications
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet("MarkNotificationAsSeen/{Id}")]
        public async Task<IActionResult> MarkNotificationAsSeen(int Id)
        {
            await _userService.MarkNotificationAsSeen(Id);
            return Ok();
        }

        /// <summary>
        /// Return Current User Notifications
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet("MarkNotificationAsRead/{Id}")]
        public async Task<IActionResult> MarkNotificationAsRead(int Id)
        {
            await _userService.MarkNotificationAsRead(Id);
            return Ok();
        }

        /// <summary>
        /// Return Current User Notifications
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet("MarkAllNotificationAsSeen")]
        public async Task<IActionResult> MarkAllNotificationAsSeen()
        {
            await _userService.MarkAllNotificationAsSeen();
            return Ok();
        }

        /// <summary>
        /// Return Current User Notifications
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet("MarkAllNotificationAsRead")]
        public async Task<IActionResult> MarkAllNotificationAsRead()
        {
            await _userService.MarkAllNotificationAsRead();
            return Ok();
        }

    }
}