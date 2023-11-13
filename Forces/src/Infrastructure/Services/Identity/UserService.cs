using AutoMapper;
using Forces.Application.Enums;
using Forces.Application.Exceptions;
using Forces.Application.Extensions;
using Forces.Application.Interfaces.Repositories;
using Forces.Application.Interfaces.Services;
using Forces.Application.Interfaces.Services.Identity;
using Forces.Application.Requests.Identity;
using Forces.Application.Requests.Mail;
using Forces.Application.Responses.Identity;
using Forces.Infrastructure.Models;
using Forces.Infrastructure.Models.Identity;
using Forces.Infrastructure.Specifications;
using Forces.Shared.Constants.Role;
using Forces.Shared.Wrapper;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Forces.Infrastructure.Services.Identity
{
    public class UserService : IUserService
    {
        private readonly UserManager<Appuser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMailService _mailService;
        private readonly IStringLocalizer<UserService> _localizer;
        private readonly IExcelService _excelService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IVoteCodeService _voteCodeService;
        private readonly IUnitOfWork<int> _unitOfWork;

        public UserService(
            UserManager<Appuser> userManager,
            IMapper mapper,
            RoleManager<AppRole> roleManager,
            IMailService mailService,
            IStringLocalizer<UserService> localizer,
            IExcelService excelService,
            ICurrentUserService currentUserService,
            IUnitOfWork<int> unitOfWork,
            IVoteCodeService voteCodeService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _mailService = mailService;
            _localizer = localizer;
            _excelService = excelService;
            _currentUserService = currentUserService;
            _voteCodeService = voteCodeService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<UserResponse>>> GetAllAsync()
        {
            var CurrentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
            Expression<Func<Appuser, bool>> condition = x => true;
            if (CurrentUser.ForceID.HasValue && CurrentUser.UserType == UserType.ForceAdmin)
            {
                condition = condition.And(x => x.ForceID == CurrentUser.ForceID.Value && x.UserType != UserType.SuperAdmin);
            }
            if (CurrentUser.BaseID.HasValue && CurrentUser.UserType == UserType.BaseAdmin)
            {
                condition = condition.And(x => x.BaseID == CurrentUser.BaseID.Value && (x.UserType != UserType.SuperAdmin || x.UserType != UserType.ForceAdmin));
            }
            if (CurrentUser.BaseSectionID.HasValue && CurrentUser.UserType == UserType.BaseSectionAdmin)
            {
                condition = condition.And(x => x.BaseSectionID == CurrentUser.BaseSectionID.Value && (x.UserType != UserType.SuperAdmin || x.UserType != UserType.ForceAdmin || x.UserType != UserType.BaseAdmin));
            }
            if (CurrentUser.DepartmentType.HasValue && CurrentUser.UserType == UserType.DepartmentAdmin)
            {
                if (CurrentUser.DepartmentType.Value == DepartType.Depot)
                {
                    condition = condition.And(x => x.DepartmentType == DepartType.Depot);
                }
                if (CurrentUser.DepartmentType.Value == DepartType.HQ)
                {
                    condition = condition.And(x => x.DepartmentType == DepartType.HQ);
                }
            }
            if (CurrentUser.UserType == UserType.OCLogAdmin || CurrentUser.UserType == UserType.RegularAdmin)
            {
                condition = condition.And(x => x.CreatedBy == CurrentUser.Id);
            }
            var users = await _userManager.Users.Where(condition).ToListAsync();
            var result = _mapper.Map<List<UserResponse>>(users);
            return await Result<List<UserResponse>>.SuccessAsync(result);
        }

        public async Task<IResult> RegisterAsync(RegisterRequest request, string origin)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                return await Result.FailAsync(string.Format(_localizer["Username {0} is already taken."], request.UserName));
            }

            var user = new Appuser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                IsActive = request.ActivateUser,
                EmailConfirmed = request.AutoConfirmEmail,
                ForceID = request.ForceID,
                DepartmentType = request.DepartmentType,
                BaseID = request.BaseID,
                BaseSectionID = request.BaseSectionID,
                DefaultVoteCodeID = request.DefaultVoteCodeID,
                DepoDepartmentID = request.DepoDepartmentID,
                HQDepartmentID = request.HQDepartmentID,
                InternalId = request.InternalId,
                JobTitle = request.JobTitle,
                Rank = request.Rank,
                UserType = request.UserType,

            };

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                var userWithSamePhoneNumber = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
                if (userWithSamePhoneNumber != null)
                {
                    return await Result.FailAsync(string.Format(_localizer["Phone number {0} is already registered."], request.PhoneNumber));
                }
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, RoleConstants.BasicRole);
                    if (!request.AutoConfirmEmail)
                    {
                        var verificationUri = await SendVerificationEmail(user, origin);
                        var mailRequest = new MailRequest
                        {
                            From = "mail@codewithmukesh.com",
                            To = user.Email,
                            Body = string.Format(_localizer["Please confirm your account by <a href='{0}'>clicking here</a>."], verificationUri),
                            Subject = _localizer["Confirm Registration"]
                        };
                        BackgroundJob.Enqueue(() => _mailService.SendAsync(mailRequest));
                        return await Result<string>.SuccessAsync(user.Id, string.Format(_localizer["User {0} Registered. Please check your Mailbox to verify!"], user.UserName));
                    }
                    return await Result<string>.SuccessAsync(user.Id, string.Format(_localizer["User {0} Registered."], user.UserName));
                }
                else
                {
                    return await Result.FailAsync(result.Errors.Select(a => _localizer[a.Description].ToString()).ToList());
                }
            }
            else
            {
                return await Result.FailAsync(string.Format(_localizer["Email {0} is already registered."], request.Email));
            }
        }

        private async Task<string> SendVerificationEmail(Appuser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/identity/user/confirm-email/";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            return verificationUri;
        }

        public async Task<IResult<UserResponse>> GetAsync(string userId)
        {
            var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            var result = _mapper.Map<UserResponse>(user);
            result.DepartType = user.DepartmentType;
            result.DepartId = user.DepoDepartmentID ?? user.HQDepartmentID;
            result.ForceId = user.ForceID;
            result.BaseId = user.BaseID;
            result.BaseSectionId = user.BaseSectionID;
            result.UserType = user.UserType;
            return await Result<UserResponse>.SuccessAsync(result);
        }

        public async Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
        {
            var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync();
            var isAdmin = await _userManager.IsInRoleAsync(user, RoleConstants.AdministratorRole);
            if (isAdmin)
            {
                return await Result.FailAsync(_localizer["Administrators Profile's Status cannot be toggled"]);
            }
            if (user != null)
            {
                user.IsActive = request.ActivateUser;
                var identityResult = await _userManager.UpdateAsync(user);
            }
            return await Result.SuccessAsync();
        }

        public async Task<IResult<UserRolesResponse>> GetRolesAsync(string userId)
        {
            var CurrentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
            Expression<Func<AppRole, bool>> condition = x => true;
            if (CurrentUser.ForceID.HasValue)
            {
                condition = condition.And(x => x.ForceID == CurrentUser.ForceID.Value);
            }
            if (CurrentUser.BaseID.HasValue && CurrentUser.UserType == UserType.BaseAdmin)
            {
                condition = condition.And(x => x.BaseID == CurrentUser.BaseID.Value);
            }
            if (CurrentUser.BaseSectionID.HasValue && CurrentUser.UserType == UserType.BaseSectionAdmin)
            {
                condition = condition.And(x => x.BaseSectionID == CurrentUser.BaseSectionID.Value);
            }
            var viewModel = new List<UserRoleModel>();
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _roleManager.Roles.Where(condition).ToListAsync();

            foreach (var role in roles)
            {
                var userRolesViewModel = new UserRoleModel
                {
                    RoleName = role.Name,
                    RoleDescription = role.Description
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                viewModel.Add(userRolesViewModel);
            }
            var result = new UserRolesResponse { UserRoles = viewModel };
            return await Result<UserRolesResponse>.SuccessAsync(result);
        }

        public async Task<IResult> UpdateRolesAsync(UpdateUserRolesRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user.Email == "mukesh@blazorhero.com")
            {
                return await Result.FailAsync(_localizer["Not Allowed."]);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var selectedRoles = request.UserRoles.Where(x => x.Selected).ToList();

            var currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
            if (!await _userManager.IsInRoleAsync(currentUser, RoleConstants.AdministratorRole))
            {
                var tryToAddAdministratorRole = selectedRoles
                    .Any(x => x.RoleName == RoleConstants.AdministratorRole);
                var userHasAdministratorRole = roles.Any(x => x == RoleConstants.AdministratorRole);
                if (tryToAddAdministratorRole && !userHasAdministratorRole || !tryToAddAdministratorRole && userHasAdministratorRole)
                {
                    return await Result.FailAsync(_localizer["Not Allowed to add or delete Administrator AppRole if you have not this role."]);
                }
            }

            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            result = await _userManager.AddToRolesAsync(user, selectedRoles.Select(y => y.RoleName));
            return await Result.SuccessAsync(_localizer["Roles Updated"]);
        }

        public async Task<IResult<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return await Result<string>.SuccessAsync(user.Id, string.Format(_localizer["Account Confirmed for {0}. You can now use the /api/identity/token endpoint to generate JWT."], user.Email));
            }
            else
            {
                throw new ApiException(string.Format(_localizer["An error occurred while confirming {0}"], user.Email));
            }
        }

        public async Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return await Result.FailAsync(_localizer["An Error has occurred!"]);
            }
            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "account/reset-password";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            var passwordResetURL = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", code);
            var mailRequest = new MailRequest
            {
                Body = string.Format(_localizer["Please reset your password by <a href='{0}>clicking here</a>."], HtmlEncoder.Default.Encode(passwordResetURL)),
                Subject = _localizer["Reset Password"],
                To = request.Email
            };
            BackgroundJob.Enqueue(() => _mailService.SendAsync(mailRequest));
            return await Result.SuccessAsync(_localizer["Password Reset Mail has been sent to your authorized Email."]);
        }

        public async Task<IResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return await Result.FailAsync(_localizer["An Error has occured!"]);
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (result.Succeeded)
            {
                return await Result.SuccessAsync(_localizer["Password Reset Successful!"]);
            }
            else
            {
                return await Result.FailAsync(_localizer["An Error has occured!"]);
            }
        }

        public async Task<int> GetCountAsync()
        {
            var CurrentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
            Expression<Func<Appuser, bool>> condition = x => true;
            if (CurrentUser.ForceID.HasValue && CurrentUser.UserType == UserType.ForceAdmin)
            {
                condition = condition.And(x => x.ForceID == CurrentUser.ForceID.Value && x.UserType != UserType.SuperAdmin);
            }
            if (CurrentUser.BaseID.HasValue && CurrentUser.UserType == UserType.BaseAdmin)
            {
                condition = condition.And(x => x.BaseID == CurrentUser.BaseID.Value && (x.UserType != UserType.SuperAdmin || x.UserType != UserType.ForceAdmin));
            }
            if (CurrentUser.BaseSectionID.HasValue && CurrentUser.UserType == UserType.BaseSectionAdmin)
            {
                condition = condition.And(x => x.BaseSectionID == CurrentUser.BaseSectionID.Value && (x.UserType != UserType.SuperAdmin || x.UserType != UserType.ForceAdmin || x.UserType != UserType.BaseAdmin));
            }
            if (CurrentUser.DepartmentType.HasValue)
            {
                if (CurrentUser.DepartmentType.Value == DepartType.Depot)
                {
                    condition = condition.And(x => x.DepartmentType == DepartType.Depot && x.ForceID == CurrentUser.ForceID.Value);
                }
                if (CurrentUser.DepartmentType.Value == DepartType.HQ)
                {
                    condition = condition.And(x => x.DepartmentType == DepartType.HQ && x.ForceID == CurrentUser.ForceID.Value);
                }
            }
            var count = await _userManager.Users.Where(condition).CountAsync();
            return count;
        }

        public async Task<string> ExportToExcelAsync(string searchString = "")
        {
            var userSpec = new UserFilterSpecification(searchString);
            var users = await _userManager.Users
                .Specify(userSpec)
                .OrderByDescending(a => a.CreatedOn)
                .ToListAsync();
            var result = await _excelService.ExportAsync(users, sheetName: _localizer["Users"],
                mappers: new Dictionary<string, Func<Appuser, object>>
                {
                    { _localizer["Id"], item => item.Id },
                    { _localizer["FirstName"], item => item.FirstName },
                    { _localizer["LastName"], item => item.LastName },
                    { _localizer["UserName"], item => item.UserName },
                    { _localizer["Email"], item => item.Email },
                    { _localizer["EmailConfirmed"], item => item.EmailConfirmed },
                    { _localizer["PhoneNumber"], item => item.PhoneNumber },
                    { _localizer["PhoneNumberConfirmed"], item => item.PhoneNumberConfirmed },
                    { _localizer["IsActive"], item => item.IsActive },
                    { _localizer["CreatedOn (Local)"], item => DateTime.SpecifyKind(item.CreatedOn, DateTimeKind.Utc).ToLocalTime().ToString("G", CultureInfo.CurrentCulture) },
                    { _localizer["CreatedOn (UTC)"], item => item.CreatedOn.ToString("G", CultureInfo.CurrentCulture) },
                    { _localizer["ProfilePictureDataUrl"], item => item.ProfilePictureDataUrl },
                });

            return result;
        }

        public async Task<IResult<UserType>> GetCurrentUserTypeAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            /*if (user.UserType != null || user.UserType.HasValue)
            {
                return await Result<UserType>.SuccessAsync(user.UserType.Value);
            }*/
            return await Result<UserType>.SuccessAsync(UserType.SuperAdmin);
        }

        public async Task<Result<List<UserTypeResponse>>> GetAllUserTypesAsync()
        {
            var TypeList = Enum.GetValues(typeof(UserType)).Cast<UserType>().Select(x => new UserTypeResponse
            {
                TypeId = (int)x,
                TypeName = x.ToDescriptionString()
            }).ToList();
            return await Result<List<UserTypeResponse>>.SuccessAsync(TypeList);
        }

        public async Task<IResult<DepartType>> GetCurrentUserDepartTypeAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user.DepartmentType != null || user.DepartmentType.HasValue)
            {
                return await Result<DepartType>.SuccessAsync(user.DepartmentType.Value);
            }
            return await Result<DepartType>.SuccessAsync(DepartType.Depot);
        }

        public async Task<int?> GetForceID(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user.ForceID;
        }

        public async Task<int?> GetBaseID(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user.BaseID;
        }

        public async Task<int?> GetBaseSectionID(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user.BaseSectionID;
        }

        public async Task<int?> GetDepartID(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user.DepartmentType == DepartType.Depot)
            {
                return user.DepoDepartmentID;
            }
            if (user.DepartmentType == DepartType.HQ)
            {
                return user.HQDepartmentID;
            }
            return null;
        }

        public async Task<int?> GetDefaultVoteCodeId(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user.DefaultVoteCodeID;
        }

        public async Task<Result<List<UserResponse>>> GetAllVoteCodeControllersAsync(int voteCodeId)
        {

            var users = await _userManager.Users.ToListAsync();
            if (voteCodeId != -1)
            {
                var vCode = await _voteCodeService.GetCodeBy(voteCodeId);
                if (vCode != null)
                {
                    if (!string.IsNullOrEmpty(vCode.UserName))
                    {
                        var defaultHolder = users.FirstOrDefault(x => x.UserName == vCode?.UserName);
                        if (defaultHolder != null)
                        {
                            users.Remove(defaultHolder);
                        }
                    }

                }
            }


            var result = _mapper.Map<List<UserResponse>>(users);
            return await Result<List<UserResponse>>.SuccessAsync(result);
        }

        public async Task<RequestSteps> GetStepByUserId(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user.UserType.HasValue)
            {
                switch (user.UserType)
                {
                    case UserType.Regular:
                        return RequestSteps.CreationStep;
                    case UserType.RegularAdmin:
                        return RequestSteps.CreationStep;
                    case UserType.OCLog:
                        return RequestSteps.OCLogStep;
                    case UserType.OCLogAdmin:
                        return RequestSteps.OCLogStep;
                    case UserType.Department:
                        return RequestSteps.DepartmentStep;
                    case UserType.DepartmentAdmin:
                        return RequestSteps.OCDepartment;
                    case UserType.HQ:
                        return RequestSteps.DepartmentStep;
                    case UserType.Depot:
                        return RequestSteps.DepartmentStep;
                    case UserType.VoteHolder:
                        return RequestSteps.VoteCodeContreoller;
                    case UserType.DFINANCE:
                        return RequestSteps.DFinanceStep;
                    case UserType.Commander:
                        return RequestSteps.CommanderStep;
                    case UserType.BaseSectionAdmin:
                        return RequestSteps.OCLogStep;
                    case UserType.BaseAdmin:
                        return RequestSteps.OCLogStep;
                    case UserType.ForceAdmin:
                        return RequestSteps.OCLogStep;
                    case UserType.SuperAdmin:
                        return RequestSteps.CommanderStep;
                    default:
                        return RequestSteps.CreationStep;
                }
            }
            else
            {
                return RequestSteps.CommanderStep;
            }

        }

        public Task<RequestSteps> GetStepByUserType(UserType userType)
        {
            switch (userType)
            {
                case UserType.Regular:
                    return Task.FromResult(RequestSteps.CreationStep);
                case UserType.RegularAdmin:
                    return Task.FromResult(RequestSteps.CreationStep);
                case UserType.OCLog:
                    return Task.FromResult(RequestSteps.OCLogStep);
                case UserType.OCLogAdmin:
                    return Task.FromResult(RequestSteps.OCLogStep);
                case UserType.Department:
                    return Task.FromResult(RequestSteps.DepartmentStep);
                case UserType.DepartmentAdmin:
                    return Task.FromResult(RequestSteps.OCDepartment);
                case UserType.HQ:
                    return Task.FromResult(RequestSteps.DepartmentStep);
                case UserType.Depot:
                    return Task.FromResult(RequestSteps.DepartmentStep);
                case UserType.VoteHolder:
                    return Task.FromResult(RequestSteps.VoteCodeContreoller);
                case UserType.DFINANCE:
                    return Task.FromResult(RequestSteps.DFinanceStep);
                case UserType.Commander:
                    return Task.FromResult(RequestSteps.CommanderStep);
                case UserType.BaseSectionAdmin:
                    return Task.FromResult(RequestSteps.OCLogStep);
                case UserType.BaseAdmin:
                    return Task.FromResult(RequestSteps.OCLogStep);
                case UserType.ForceAdmin:
                    return Task.FromResult(RequestSteps.OCLogStep);
                case UserType.SuperAdmin:
                    return Task.FromResult(RequestSteps.CommanderStep);
                default:
                    return Task.FromResult(RequestSteps.CreationStep);
            }
        }

        public Task<RequestSteps> GetCurrentUserStep()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<List<NotificationResponse>>> GetNotificationByUserId()
        {
            var user = await _userManager.Users.Include(x => x.Notifications).FirstOrDefaultAsync(x => x.Id == _currentUserService.UserId);
            var mapped = _mapper.Map<List<NotificationResponse>>(user.Notifications.OrderByDescending(x => x.CreatedOn).ToList());
            return await Result<List<NotificationResponse>>.SuccessAsync(mapped);
        }

        public async Task MarkNotificationAsSeen(int notificationId)
        {
            var notification = await _unitOfWork.Repository<NotificationsUsers>().GetByIdAsync(notificationId);
            notification.Seen = true;
            await _unitOfWork.Repository<NotificationsUsers>().UpdateAsync(notification);
            await _unitOfWork.Commit(new System.Threading.CancellationToken());
        }

        public async Task MarkNotificationAsRead(int notificationId)
        {
            var notification = await _unitOfWork.Repository<NotificationsUsers>().GetByIdAsync(notificationId);
            notification.Readed = true;
            await _unitOfWork.Repository<NotificationsUsers>().UpdateAsync(notification);
            await _unitOfWork.Commit(new System.Threading.CancellationToken());
        }

        public async Task MarkAllNotificationAsSeen()
        {
            var notification = await _unitOfWork.Repository<NotificationsUsers>().Entities.Where(x => x.TargetUserId == _currentUserService.UserId).ToListAsync();
            foreach (var noti in notification)
            {
                noti.Seen = true;
                await _unitOfWork.Repository<NotificationsUsers>().UpdateAsync(noti);
            }
            await _unitOfWork.Commit(new System.Threading.CancellationToken());
        }

        public async Task MarkAllNotificationAsRead()
        {
            var notification = await _unitOfWork.Repository<NotificationsUsers>().Entities.Where(x => x.TargetUserId == _currentUserService.UserId).ToListAsync();
            foreach (var noti in notification)
            {
                noti.Readed = true;
                await _unitOfWork.Repository<NotificationsUsers>().UpdateAsync(noti);
            }
            await _unitOfWork.Commit(new System.Threading.CancellationToken());
        }

        public async Task<IResult> EditAsync(EditUserRequest request)
        {
            var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync();
            var isAdmin = await _userManager.IsInRoleAsync(user, RoleConstants.AdministratorRole);
            if (user != null)
            {
                user.BaseID = request.BaseID;
                user.BaseSectionID = request.BaseSectionID;
                user.DepartmentType = request.DepartmentType;
                user.DepoDepartmentID = request.DepoDepartmentID;
                user.Email = request.Email;
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.ForceID = request.ForceID;
                user.HQDepartmentID = request.HQDepartmentID;
                user.JobTitle = request.JobTitle;
                user.PhoneNumber = request.PhoneNumber;
                user.Rank = request.Rank;
                user.UserType = request.UserType;
                user.NormalizedEmail = request.Email.Normalize();
                var identityResult = await _userManager.UpdateAsync(user);
            }
            return await Result.SuccessAsync("User Updated");
        }

        public async Task<IResult<UserResponse>> GetByUsaerNameAsync(string UserName)
        {
            var user = await _userManager.Users.Where(u => u.UserName == UserName).FirstOrDefaultAsync();
            var result = _mapper.Map<UserResponse>(user);
            result.DepartType = user.DepartmentType;
            result.DepartId = user.DepoDepartmentID ?? user.HQDepartmentID;
            result.ForceId = user.ForceID;
            result.BaseId = user.BaseID;
            result.BaseSectionId = user.BaseSectionID;
            result.UserType = user.UserType;
            return await Result<UserResponse>.SuccessAsync(result);
        }

        public Task GetVehicleID(string userId)
        {
            throw new NotImplementedException();
        }
    }
}