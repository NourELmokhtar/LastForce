using Forces.Application.Enums;
using Forces.Application.Features.MprRequest.Dto.Response;
using Forces.Application.Requests.VoteCodes;
using Forces.Application.Responses.Identity;
using Forces.Application.Responses.VoteCodes;
using Forces.Client.Infrastructure.Managers.VoteCodes;
using Forces.Client.Pages.VoteCodes;
using Forces.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Forces.Client.Pages.Content
{
    public partial class VoteCodeDashboardDetail
    {
        [Inject] private IVoteCodesManager _voteCodesmanager { get; set; }
        public VoteCodeResponse DetailedvoteCode { get; set; } = new();
        [Parameter] public string voteCode { get; set; }
        public VoteCodeLogSpecificationRequest Specifcation { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        public List<GetMprResponse> Requests { get; set; } = new();
        public List<GetMprResponse> PendingRequests { get; set; } = new();
        private UserType CurrentUserType { get; set; }
        private UserResponse CurrentUser { get; set; }
        private ClaimsPrincipal _authenticationStateProviderUser;
        private bool _Searching = false;
        private bool _loaded = false;
        private bool _Authurized = false;
        DateRange _dateRange = new DateRange(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - 1).Date);
        private string _searchString = "";
        protected override async Task OnInitializedAsync()
        {
            await GetVoteCode();
            var OwnedVoteCodes = await _voteCodesmanager.GetAllByCurrentUser();
            _Authurized = OwnedVoteCodes.Data.Any(x => x.Id == DetailedvoteCode.Id);
            HubConnection = new HubConnectionBuilder()
                            .WithUrl(_navigationManager.ToAbsoluteUri(ApplicationConstants.SignalR.HubUrl))
                            .Build();
            HubConnection.On(ApplicationConstants.SignalR.ReceiveUpdateDashboard, async () =>
            {
                await GetVoteCode();
                StateHasChanged();
            });
            await HubConnection.StartAsync();
            _loaded = true;
        }
        private async Task LoadUserData()
        {
            var uType = await _userManager.GetUserType();
            CurrentUserType = uType.Data;
            var authenticatedUser = await _authenticationManager.CurrentUser();
            var CurrentUserResponse = await _userManager.GetAsync(authenticatedUser.FindFirst(ClaimTypes.NameIdentifier).Value);
            CurrentUser = CurrentUserResponse.Data;
            _authenticationStateProviderUser = await _stateProvider.GetAuthenticationStateProviderUserAsync();
        }
        private async Task GetRequests()
        {
            var Response = await _mprManager.GetRequestsByVoteCodeAsync(DetailedvoteCode.Id);
            if (Response.Succeeded)
            {
                Requests = Response.Data.Where(x=>x.RequestState == RequestState.Completed).ToList();
                PendingRequests = Response.Data.Where(x => x.RequestState == RequestState.Pending).ToList();

            }
            else
            {
                foreach (var msg in Response.Messages)
                {
                    _snackBar.Add(msg, Severity.Error);
                }
            }
        }
        private async Task GetVoteCode()
        {
            if (int.TryParse(voteCode, out int num))
            {
                var response = await _voteCodesmanager.GetCodeBy(int.Parse(voteCode));
                if (response.Succeeded)
                {
                    DetailedvoteCode = response.Data;
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
        Func<TransactionType?, string> TransactionTypeToString = p => p.HasValue ? p.Value.ToString() : "All";
        private async Task Search()
        {
            _Searching = true;
            Specifcation.VoteCodeId = DetailedvoteCode.Id;
            Specifcation.DateFrom = _dateRange.Start;
            Specifcation.DateTo = _dateRange.End;

            var response = await _voteCodesmanager.GetLogsSpec(Specifcation);
            if (response.Succeeded)
            {
                DetailedvoteCode.Logs = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
            _Searching = false;
        }
        private string ArrowIcon(TransactionType type)
        {
            if (type == TransactionType.Credit)
            {
                return Icons.Filled.ArrowDropUp;
            }
            return Icons.Filled.ArrowDropDown;
        }
        private Color ArrowColor(TransactionType type)
        {
            if (type == TransactionType.Credit)
            {
                return Color.Tertiary;
            }
            return Color.Error;
        }
        private void ShowBtnPress(int nr)
        {
            DetailedvoteCode.Logs.Where(f => f.LogId != nr).ToList().ForEach(x => { x.ShowDetails = false; });
            var tmpLog = DetailedvoteCode.Logs.First(f => f.LogId == nr);
            tmpLog.ShowDetails = !tmpLog.ShowDetails;
        }
        private async Task InvokeTransactionModal(int id = 0)
        {
            var parameters = new DialogParameters();


            parameters.Add(nameof(AddTransactionModal.AddModel), new AddEditVcodeTransactionRequest
            {
                VoteCodeId = DetailedvoteCode.Id,
                Credit = DetailedvoteCode.Cridet,


            });

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Large, CloseOnEscapeKey = true, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddTransactionModal>(id == 0 ? "Add" : "Add", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await GetVoteCode();
            }
        }
        private async Task ExportToExcel()
        {
            var response = await _voteCodesmanager.ExportToExcelAsync(DetailedvoteCode.Logs);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{DetailedvoteCode.VoteShortcut}_Transactions_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add("Transactions Exported", Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private bool Filter(VoteCodeLogResponse force)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (force.ItemName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (force.ItemCode?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (force.ItemNSN?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (force.TransactionBy?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (force.RequestType?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (force.RequestRefrance?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            return false;
        }
    }
}
