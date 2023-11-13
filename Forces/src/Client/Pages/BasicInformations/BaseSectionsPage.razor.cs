using Forces.Application.Features.Bases.Commands.AddEdit;
using Forces.Application.Features.Bases.Queries.GetAll;
using Forces.Application.Features.Forces.Queries.GetAll;
using Forces.Application.Features.BaseSections.Queries.GetAll;
using Forces.Application.Features.BaseSections.Commands.AddEdit;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.BasicInformation.Bases;
using Forces.Client.Infrastructure.Managers.BasicInformation.Forces;
using Forces.Client.Infrastructure.Managers.BasicInformation.BaseSections;
using Forces.Shared.Constants.Application;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Forces.Client.Pages.BasicInformations
{
    public partial class BaseSectionsPage
    {
        [Inject] private IBaseManager baseManager { get; set; }
        [Inject] private IForceManager ForceManager { get; set; }
        [Inject] private IBaseSectionManager BaseSectionManager { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private List<GetAllBasesResponse> _BasesList = new();
        private GetAllBasesResponse _base = new();
        private List<GetAllBasesSectionsQueryResponse> _SectionsList = new();
        private GetAllBasesSectionsQueryResponse _Section = new();
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = false;
        private ClaimsPrincipal _currentUser;
        private bool _canCreateBaseSection;
        private bool _canEditBaseSection;
        private bool _canDeleteBaseSection;
        private bool _canSearchBaseSection;
        private bool _loaded;
        private List<GetAllForcesResponse> _ForceList = new();
        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateBaseSection = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BasicInformations.CreateBasesSection)).Succeeded;
            _canEditBaseSection = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BasicInformations.EditBasesSection)).Succeeded;
            _canDeleteBaseSection = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BasicInformations.DeleteBasesSection)).Succeeded;
            _canSearchBaseSection = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BasicInformations.SearchBasesSection)).Succeeded;


            await GetBasesAsync();
            await GetForcesAsync();
            await GetSectionsAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        private async Task GetForcesAsync()
        {
            var response = await ForceManager.GetAllAsync();
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
        private async Task GetBasesAsync()
        {
            var response = await baseManager.GetAllAsync();
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
        private async Task GetSectionsAsync()
        {
            var response = await BaseSectionManager.GetAllAsync();
            if (response.Succeeded)
            {
                _SectionsList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private TableGroupDefinition<GetAllBasesSectionsQueryResponse> _groupDefinition = new()
        {
            GroupName = "Force",
            Indentation = true,
            Expandable = true,
            Selector = (e) => e.ForceId,
            InnerGroup = new TableGroupDefinition<GetAllBasesSectionsQueryResponse>()
            {
                GroupName = "Base",
                Indentation = true,
                Expandable = true,
                Selector = (e) => e.BaseId
            }
        };

        private string BaseNameAndCode(int id, string GroupName)
        {
            if (GroupName == "Force")
            {
                var force = _ForceList.FirstOrDefault(x => x.Id == id);
                return $"{force.ForceName} | {force.ForceCode}";
            }
            var Base = _BasesList.FirstOrDefault(x => x.Id == id);
            return $"{Base.BaseName} | {Base.BaseCode}";
        }
        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _Section = _SectionsList.FirstOrDefault(c => c.Id == id);
                if (_Section != null)
                {
                    parameters.Add(nameof(AddEditSectionModal.AddEditBaseSectionModel), new AddEditBaseSectionCommand
                    {
                        Id = _Section.Id,
                        SectionName = _Section.SectionName,
                        SectionCode = _Section.SectionCode,
                        BaseId = _Section.BaseId
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditSectionModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }
        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Are You Sure To Delete This Section?"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await BaseSectionManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    await Reset();
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }
        private async Task Reset()
        {
            _Section = new GetAllBasesSectionsQueryResponse();
            await GetSectionsAsync();
        }
        private bool Search(GetAllBasesSectionsQueryResponse section)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (section.SectionName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (section.SectionCode?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
