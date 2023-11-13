using Blazored.FluentValidation;
using FluentValidation;
using Forces.Application.Features.BaseSections.Queries.GetAll;
using Forces.Application.Features.Building.Queries.GetAll;
using Forces.Application.Features.Forces.Queries.GetAll;
using Forces.Application.Features.House.Queries.GetAll;
using Forces.Application.Features.Inventory.Commands.AddEdit;
using Forces.Application.Features.Room.Queries.GetAll;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.BasicInformation.BaseSections;
using Forces.Client.Infrastructure.Managers.BasicInformation.Forces;
using Forces.Client.Infrastructure.Managers.Building;
using Forces.Client.Infrastructure.Managers.House;
using Forces.Client.Infrastructure.Managers.Inventory;
using Forces.Client.Infrastructure.Managers.Room;
using Forces.Shared.Constants.Application;
using Forces.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Security.Claims;

namespace Forces.Client.Pages.Inventory
{
    public partial class AddEditInventoryModal
    {
        public AddEditInventoryModal()
        {

        }
        [Inject] private IInventoryManager InventoryManager { get; set; }
        [Inject] private IBaseSectionManager BaseSectionManager { get; set; }
        private List<GetAllBasesSectionsQueryResponse> _BaseSectionList = new();

        [Inject] private IHouseManager HouseManager { get; set; }
        private List<GetAllHousesResponse> _HouseList = new();
        
        [Inject] private IBuildingManager BuildingManager { get; set; }
        private List<GetAllBuildingsResponse> _BuildingList = new();

        [Inject] private IRoomManager RoomManager { get; set; }
        private List<GetAllRoomsResponse> _RoomList = new();
        private List<GetAllRoomsResponse> _UsedRoomList = new();


        private List<GetAllForcesResponse> _ForceList = new();
        [Parameter] public AddEditInventoryCommand AddEditInventoryModel { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [Inject] private IForceManager ForceManager { get; set; }
        private List<string> dropdownItems = new List<string> { "Building", "House", "BasesSections" };
        private string selectedDropdownItem;
        private string BaseSectionName;
        private string HouseName;
        private string BuildingName;
        private int RoomNumber = 0;
        private bool _canCreateBaseSection;
        private bool _canEditBaseSection;
        private bool _canDeleteBaseSection;
        private bool _canSearchBaseSection;
        private ClaimsPrincipal _currentUser;
        private IEnumerable<GetAllRoomsResponse> filteredRooms;

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public void Cancel()
        {
            MudDialog.Cancel();
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
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }
           
        }
        private async Task GetBasesAsync()
        {
            var response = await BaseSectionManager.GetAllAsync();
            if (response.Succeeded)
            {
                _BaseSectionList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }
        }

        private async Task GetBuildingsAsync()
        {
            var response = await BuildingManager.GetAllAsync();
            if (response.Succeeded)
            {
                _BuildingList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }
        }

        private async Task GetHousesAsync()
        {
            var response = await HouseManager.GetAllAsync();
            if (response.Succeeded)
            {
                _HouseList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }
        }
        private async Task GetRoomsAsync()
        {
            var response = await RoomManager.GetAllAsync();
            if (response.Succeeded)
            {
                _RoomList = response.Data.ToList();
                filteredRooms = _RoomList.Where(x => x.BuildingId == converterForBuildings(BuildingName));
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }
        }

        private async Task SaveAsync()
        {
            if(selectedDropdownItem == "Building" && RoomNumber != 0)
            {
                AddEditInventoryModel.RoomId = converterForRooms(RoomNumber);
            }
            else if(selectedDropdownItem == "House")
            {
                AddEditInventoryModel.HouseId = converterForHouses(HouseName);
            }
            else if(selectedDropdownItem == "BasesSections")
            {
                AddEditInventoryModel.BaseSectionId = converterForSections(BaseSectionName);
            }
            var response = await InventoryManager.SaveAsync(AddEditInventoryModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], MudBlazor.Severity.Success);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, MudBlazor.Severity.Error);
                }
            }
            await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
        }
        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateBaseSection = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BasicInformations.CreateBasesSection)).Succeeded;
            _canEditBaseSection = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BasicInformations.EditBasesSection)).Succeeded;
            _canDeleteBaseSection = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BasicInformations.DeleteBasesSection)).Succeeded;
            _canSearchBaseSection = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.BasicInformations.SearchBasesSection)).Succeeded;

            await LoadDataAsync();
            
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        Func<int, string> converter()
        {
            return p => $"{_ForceList.FirstOrDefault(x => x.Id == p).ForceName} | {_ForceList.FirstOrDefault(x => x.Id == p).ForceCode}";
        }
        private int? converterForSections(string ss)
        {
            return _BaseSectionList.FirstOrDefault(s => s.SectionName == ss).Id;
        }
        private int? converterForHouses(string ss)
        {
            return _HouseList.FirstOrDefault(s => s.HouseName == ss).Id;
        }
        private int? converterForBuildings(string ss)
        {
            return _BuildingList.FirstOrDefault(s => s.BuildingName == ss).Id;
        }
        private int? converterForRooms(int ss)
        {
            return _RoomList.FirstOrDefault(s => s.RoomNumber == RoomNumber).Id;
        }
        private async Task LoadDataAsync()
        {
            await GetBasesAsync();
            await GetBuildingsAsync();
            await GetHousesAsync();
            await GetRoomsAsync();

            
            await GetForcesAsync();
            
            await Task.CompletedTask;
        }
    }

}
