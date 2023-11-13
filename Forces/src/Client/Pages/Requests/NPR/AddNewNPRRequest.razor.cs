using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forces.Application.Requests.Requests.NPRRequest;
using Microsoft.AspNetCore.Components.Forms;
using Forces.Client.Infrastructure.Managers.Requests.NPR;
using Forces.Client.Infrastructure.Managers.Items;
using MudBlazor;
using Blazored.FluentValidation;
using Forces.Application.Features.Items.Queries.GetAll;
using Forces.Client.Infrastructure.Managers.VoteCodes;
using Forces.Application.Responses.VoteCodes;
using System.IO;
using Forces.Application.Validators.Requests.Requests.NPRRequest;
using Forces.Application.Extensions;
using Microsoft.AspNetCore.SignalR.Client;
using Forces.Client.Extensions;
using Forces.Shared.Constants.Application;

namespace Forces.Client.Pages.Requests.NPR
{
    public partial class AddNewNPRRequest
    {
        [Inject] public INprManager _NprManager { get; set; }
        [Inject] public IItemsManager _itemManager { get; set; }
        [Inject] private IVoteCodesManager _VoteCodeMnager { get; set; }
        private List<VoteCodeResponse> votecodeList = new();
        [Inject] IDialogService DialogService { get; set; }

        public AddEditNPRRequest Model { get; set; } = new();
        private IList<IBrowserFile> files = new List<IBrowserFile>();
        public GetAllItemsResponse _Item { get; set; } = new();
        private FluentValidationValidator _fluentValidationValidator;
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private bool _isSubmiting = false;
        private bool _isSubmited = false;
        private string RefNumber = "";
        private static System.Timers.Timer aTimer;
        private int counter = 10;
        protected override async Task OnInitializedAsync()
        {
            await GetVoteCodesAsync();
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
            await base.OnInitializedAsync();
        }

        private async Task Submit()
        {
            _isSubmiting = true;
            await UploadAttachmentAsync();
            var response = await _NprManager.SaveAsync(Model);
            if (response.Succeeded)
            {
                RefNumber = response.Data;
                _snackBar.Add(response.Messages[0], Severity.Success);
                _isSubmiting = false;
                _isSubmited = true;
                await HubConnection.SendAsync(ApplicationConstants.SignalR.NotifySteppers);
                StartTimer();
            }
            else
            {
                _isSubmiting = false;
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }

            }

        }
        private string VoteCodestr(int Id)
        {
            var code = votecodeList.FirstOrDefault(x => x.Id == Id);
            if (code == null)
            {
                return string.Empty;
            }
            return code.VoteCode;
        }
        private async Task GetVoteCodesAsync()
        {
            var response = await _VoteCodeMnager.GetAllAsync();
            if (response.Succeeded)
            {
                votecodeList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private void UploadFiles(InputFileChangeEventArgs e)
        {
            foreach (var file in e.GetMultipleFiles())
            {
                files.Add(file);
                Model.Attachments.Add(new Application.Requests.Requests.AttachmentRequest());
            }

        }


        private async Task UploadAttachmentAsync()
        {
            Model.Attachments = new List<Application.Requests.Requests.AttachmentRequest>();

            foreach (var _file in files)
            {
                Application.Requests.Requests.AttachmentRequest att = new Application.Requests.Requests.AttachmentRequest();
                var buffer = new byte[_file.Size];
                var extension = Path.GetExtension(_file.Name);
                var format = "application/octet-stream";

                await _file.OpenReadStream(_file.Size).ReadAsync(buffer);
                att = new Application.Requests.Requests.AttachmentRequest { Data = buffer, FileName = _file.Name, UploadType = Application.Enums.UploadType.Qoutaions, Extension = extension };

                Model.Attachments.Add(att);
            }
        }
        private void removeItem(IBrowserFile file)
        {
            files.Remove(file);
            Model.Attachments.Remove(Model.Attachments.First());
        }
        private void CleareAttachment()
        {
            files.Clear();
            Model.Attachments.Clear();
        }
        private async Task SelectItem()
        {
            DialogOptions options = new DialogOptions();
            options.CloseButton = true;
            options.CloseOnEscapeKey = true;
            options.DisableBackdropClick = true;
            options.FullWidth = true;
            options.MaxWidth = MaxWidth.ExtraLarge;
            var dialog = DialogService.Show<SelectItemModal>("Select Item", options);
            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                _Item = result.Data as GetAllItemsResponse;
                Model.ItemId = _Item.Id;
                Model.VoteCodeId = _Item.VoteCodesId.Value;
                Model.Unit = _Item.MeasureName;
                Model.ItemClass = _Item.ItemClass.ToDescriptionString();
            }
        }

        public void StartTimer()
        {
            aTimer = new System.Timers.Timer(1000);
            aTimer.Elapsed += CountDownTimer;
            aTimer.Enabled = true;
        }

        public void CountDownTimer(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (counter > 0)
            {
                counter -= 1;
                StateHasChanged();
            }
            else
            {
                aTimer.Enabled = false;
                _navigationManager.NavigateTo("/Requests/MPR");
            }
        }
    }
}
