using Blazored.FluentValidation;
using Forces.Application.Enums;
using Forces.Application.Features.Bases.Queries.GetByForceId;
using Forces.Application.Features.BaseSections.Queries.GetByBaseId;
using Forces.Application.Features.DepoDepartment.Queries.GetByForceId;
using Forces.Application.Features.Forces.Queries.GetAll;
using Forces.Application.Features.HQDepartment.Queries.GetByForceId;
using Forces.Application.Requests.Identity;
using Forces.Client.Infrastructure.Managers.BasicInformation.Bases;
using Forces.Client.Infrastructure.Managers.BasicInformation.BaseSections;
using Forces.Client.Infrastructure.Managers.BasicInformation.Forces;
using Forces.Client.Infrastructure.Managers.Departments.Depo;
using Forces.Client.Infrastructure.Managers.Departments.HQ;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Forces.Client.Pages.Identity
{
    public partial class EditUserModal
    {
        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        [Inject] private MprStepTypes mprStep { get; set; }
        [Parameter] public EditUserRequest _registerUserModel { get; set; } = new();
        [Inject] private IBaseManager baseManager { get; set; }
        [Inject] private IForceManager ForceManager { get; set; }
        [Inject] private IBaseSectionManager BaseSectionManager { get; set; }
        [Inject] private IDepoManager DepoManager { get; set; }
        [Inject] private IHQManager HQManager { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        string lang = "";


        private List<GetAllForcesResponse> _ForceList = new();
        private List<GetAllBasesByForceIdResponse> _BasesList = new();
        private List<GetAllSectionsByBaseIdQueryResponse> _baseSectionList = new();
        private List<GetAllHQbyForceIdResponse> _HQDepartmentList = new();
        private List<GetAllDepoByForceIdResponse> _DepoDepartmentList = new();
        private List<UserType> TypeList = new();
        private void Cancel()
        {
            MudDialog.Cancel();

        }
        private int selectedVal = 0;
        private int? activeVal;
        private void HandleHoveredValueChanged(int? val) => activeVal = val;
        private async Task SubmitAsync()
        {
            BeforeSubmit();
            var response = await _userManager.EditUserAsync(_registerUserModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private string LabelText => (activeVal ?? selectedVal) switch
        {
            1 => _localizer["Second Lieutenant"],//ملازم
            2 => _localizer["First Lieutenant"],// ملازم اول
            3 => _localizer["Captain"],// نقيب
            4 => _localizer["Major"],//رائد
            5 => _localizer["Lieutenant Colonel"],// مقدم
            6 => _localizer["Colonel"],// عقيد
            7 => _localizer["Brigadier General"],// عميد
            8 => _localizer["Major General"],// لواء
            9 => _localizer["Lieutenant General"],// فريق
            10 => _localizer["General"],// فريق أول
            _ => _localizer["Rank"]// الرتبه
        };
        private async Task GetUsersType()
        {
            var CurrentUserTypeResponse = await _userManager.GetUserType();
            var CurrentUserType = CurrentUserTypeResponse.Data;
            if (CurrentUserType == UserType.SuperAdmin)
            {
                TypeList.AddRange(new List<UserType> {UserType.BaseAdmin,UserType.SuperAdmin,UserType.ForceAdmin, UserType.BaseSectionAdmin, UserType.Commander, UserType.Department,
                                                      UserType.DepartmentAdmin,UserType.DFINANCE,
                                                      UserType.OCLog,UserType.OCLogAdmin,UserType.Regular,UserType.RegularAdmin,UserType.VoteHolder});
            }
            if (CurrentUserType == UserType.ForceAdmin)
            {
                TypeList.AddRange(new List<UserType> {UserType.BaseAdmin, UserType.BaseSectionAdmin, UserType.Commander, UserType.Department,
                                                      UserType.DepartmentAdmin,UserType.DFINANCE,
                                                      UserType.OCLog,UserType.OCLogAdmin,UserType.Regular,UserType.RegularAdmin,UserType.VoteHolder});
            }
            if (CurrentUserType == UserType.BaseAdmin)
            {
                TypeList.AddRange(new List<UserType> { UserType.BaseSectionAdmin,
                                                      UserType.OCLog,UserType.OCLogAdmin,UserType.Regular,UserType.RegularAdmin});
            }
            if (CurrentUserType == UserType.BaseSectionAdmin)
            {
                TypeList.AddRange(new List<UserType> { UserType.OCLog, UserType.OCLogAdmin, UserType.Regular, UserType.RegularAdmin });
            }
            if (CurrentUserType == UserType.DepartmentAdmin)
            {
                TypeList.AddRange(new List<UserType> { UserType.Department });
            }
            if (CurrentUserType == UserType.OCLogAdmin)
            {
                TypeList.AddRange(new List<UserType> { UserType.OCLog });
            }
            if (CurrentUserType == UserType.RegularAdmin)
            {
                TypeList.AddRange(new List<UserType> { UserType.Regular });
            }

        }
        private void BeforeSubmit()
        {
            _registerUserModel.Rank = selectedVal;
            if (_registerUserModel.UserType == UserType.SuperAdmin)
            {
                SubmitSuperAdmin();
            }
            if (_registerUserModel.UserType == UserType.ForceAdmin)
            {
                SubmitForceAdmin();
            }
            if (_registerUserModel.UserType == UserType.BaseAdmin)
            {
                SubmitForceBaseAdmin();
            }
            if (_registerUserModel.UserType == UserType.BaseSectionAdmin)
            {
                SubmitForceBaseSectionAdmin();
            }
            if (_registerUserModel.UserType == UserType.Department)
            {
                if (_registerUserModel.DepartmentType == DepartType.Depot)
                {
                    SubmitDepoDepartment();
                }
                else
                {
                    SubmitHQDepartment();
                }
            }
            if (_registerUserModel.UserType == UserType.DepartmentAdmin)
            {
                SubmitDepartmentAdmin();
            }
            if (_registerUserModel.UserType == UserType.Depot)
            {
                SubmitDepartment();
            }
            if (_registerUserModel.UserType == UserType.HQ)
            {
                SubmitDepartment();
            }
            if (_registerUserModel.UserType == UserType.VoteHolder)
            {
                SubmitVoteCodeController();
            }
            if (_registerUserModel.UserType == UserType.OCLogAdmin)
            {
                SubmitForceBaseSectionAdmin();
            }
            if (_registerUserModel.UserType == UserType.OCLogAdmin)
            {
                SubmitForceBaseSectionAdmin();
            }
            if (_registerUserModel.UserType == UserType.Commander)
            {

            }
            if (_registerUserModel.UserType == UserType.DFINANCE)
            {

            }
            if (_registerUserModel.UserType == UserType.Regular)
            {
                SubmitForceBaseSectionAdmin();
            }
            if (_registerUserModel.UserType == UserType.RegularAdmin)
            {
                SubmitForceBaseSectionAdmin();
            }
        }
        private void SubmitSuperAdmin()
        {
            _registerUserModel.ForceID = null;
            SubmitForceAdmin();
            _registerUserModel.InternalId = null;
        }
        private void SubmitForceAdmin()
        {
            _registerUserModel.BaseID = null;
            SubmitForceBaseAdmin();
        }
        private void SubmitForceBaseAdmin()
        {
            _registerUserModel.BaseSectionID = null;
            SubmitForceBaseSectionAdmin();
        }
        private void SubmitForceBaseSectionAdmin()
        {
            _registerUserModel.DefaultVoteCodeID = null;
            _registerUserModel.DepartmentType = null;
            _registerUserModel.DepoDepartmentID = null;
            _registerUserModel.HQDepartmentID = null;
        }
        private void SubmitDepartmentAdmin()
        {
            _registerUserModel.BaseID = null;
            _registerUserModel.BaseSectionID = null;
            _registerUserModel.DefaultVoteCodeID = null;
            _registerUserModel.DepoDepartmentID = null;
            _registerUserModel.HQDepartmentID = null;
        }
        private void SubmitDepoDepartment()
        {
            _registerUserModel.BaseID = null;
            _registerUserModel.BaseSectionID = null;
            _registerUserModel.DefaultVoteCodeID = null;
            _registerUserModel.HQDepartmentID = null;

        }
        private void SubmitDepartment()
        {
            _registerUserModel.BaseID = null;
            _registerUserModel.BaseSectionID = null;
            _registerUserModel.HQDepartmentID = null;
            _registerUserModel.DefaultVoteCodeID = null;

        }

        private void SubmitHQDepartment()
        {
            _registerUserModel.BaseID = null;
            _registerUserModel.BaseSectionID = null;
            _registerUserModel.DefaultVoteCodeID = null;
            _registerUserModel.DepoDepartmentID = null;
            _registerUserModel.DefaultVoteCodeID = null;

        }
        private void SubmitVoteCodeController()
        {
            _registerUserModel.BaseID = null;
            _registerUserModel.BaseSectionID = null;
            _registerUserModel.DepartmentType = null;
            _registerUserModel.DepoDepartmentID = null;
            _registerUserModel.HQDepartmentID = null;
            _registerUserModel.DefaultVoteCodeID = null;
        }

        private async Task GetDepoDeparments()
        {
            if (_registerUserModel.ForceID != null)
            {
                var response = await DepoManager.GetByForceIdAsync(_registerUserModel.ForceID.Value);
                if (response.Succeeded)
                {
                    _DepoDepartmentList = response.Data.ToList();
                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }
        private async Task GetHQDeparments()
        {
            if (_registerUserModel.ForceID != null)
            {
                var response = await HQManager.GetByForceIdAsync(_registerUserModel.ForceID.Value);
                if (response.Succeeded)
                {
                    _HQDepartmentList = response.Data.ToList();
                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }
        private async Task GetForcesAsync()
        {
            var response = await ForceManager.GetAllForcesAsync();
            if (response.Succeeded)
            {
                _ForceList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        protected override async Task OnInitializedAsync()
        {
            lang = await _clientPreferenceManager.GetCurrentLanguage();
            await GetForcesAsync();
            await DepartTypeChanged();
            await GetBasesAsync();
            await GetBaseSections();
            await GetUsersType();
        }
        Func<int?, string> ForceStringconverter()
        {
            return p => $"{_ForceList.FirstOrDefault(x => x.Id == p).ForceName} | {_ForceList.FirstOrDefault(x => x.Id == p).ForceCode}";
        }
        Func<int?, string> BaseStringconverter()
        {
            return p => $"{_BasesList.FirstOrDefault(x => x.Id == p).BaseName} | {_BasesList.FirstOrDefault(x => x.Id == p).BaseCode}";
        }
        Func<int?, string> BaseSectionStringconverter()
        {
            return p => $"{_baseSectionList.FirstOrDefault(x => x.Id == p).SectionName} | {_baseSectionList.FirstOrDefault(x => x.Id == p).SectionCode}";
        }
        Func<int?, string> DepoStringconverter()
        {
            return p => $"{_DepoDepartmentList.FirstOrDefault(x => x.Id == p).Name}";
        }
        Func<int?, string> HQStringconverter()
        {
            return p => $"{_HQDepartmentList.FirstOrDefault(x => x.Id == p).Name}";
        }

        private async Task FoceChanged()
        {
            if (mprStep.userTypeScope[_registerUserModel.UserType] >= Scope.Force)
            {
                await GetBasesAsync();
                if (_registerUserModel.DepartmentType != null)
                {
                    await DepartTypeChanged();
                }
            }
        }
        private async Task BaseChanged()
        {
            if (mprStep.userTypeScope[_registerUserModel.UserType] >= Scope.Base)
            {
                await GetBaseSections();
                StateHasChanged();
            }
        }
        private async Task DepartTypeChanged()
        {

            if (_registerUserModel.DepartmentType == DepartType.Depot)
            {
                await GetDepoDeparments();
            }
            else
            {
                await GetHQDeparments();
            }
        }

        private async Task GetBasesAsync()
        {
            if (_registerUserModel.ForceID.HasValue)
            {
                var response = await baseManager.GetBasesByForceIdAsync(_registerUserModel.ForceID.Value);
                if (response.Succeeded)
                {
                    _BasesList = response.Data.ToList();
                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }
        private async Task GetBaseSections()
        {
            if (_registerUserModel.BaseID.HasValue)
            {
                var response = await BaseSectionManager.GetSectionsByBaseIdAsync(_registerUserModel.BaseID.Value);
                if (response.Succeeded)
                {
                    _baseSectionList = response.Data.ToList();
                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }
        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    }
}
