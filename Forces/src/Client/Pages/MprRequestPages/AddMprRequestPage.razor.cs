using Blazored.FluentValidation;
using Forces.Application.Extensions;
using Forces.Application.Features.Items.Queries.GetAll;
using Forces.Application.Features.MprRequest.Command;
using Forces.Application.Features.MprRequest.Dto.Request;
using Forces.Application.Responses.VoteCodes;
using Forces.Client.Extensions;
using Forces.Client.Infrastructure.Managers.Items;
using Forces.Client.Infrastructure.Managers.MPR;
using Forces.Client.Infrastructure.Managers.Requests.NPR;
using Forces.Client.Infrastructure.Managers.VoteCodes;
using Forces.Client.Pages.Requests;
using Forces.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace Forces.Client.Pages.MprRequestPages
{
    public partial class AddMprRequestPage
    {
    
        //[Inject] public Microsoft.Extensions.Localization.IStringLocalizer<ِAddMprRequestPage> _localizer { get; set; }
        [Inject] public IItemsManager _itemManager { get; set; }
        [Inject] public IMprRequestManager _MprManager { get; set; }
        [Inject] private IVoteCodesManager _VoteCodeMnager { get; set; }
        private List<VoteCodeResponse> votecodeList = new();
        [Inject] IDialogService DialogService { get; set; }
        public AddMprRequestCommand Model { get; set; } = new();
        private IList<IBrowserFile> files = new List<IBrowserFile>();
        public GetAllItemsResponse _Item { get; set; } = new();
        private FluentValidationValidator _fluentValidationValidator;
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private bool _isSubmiting = false;
        private bool _isSubmited = false;
        private int RefNumber = 0;
        private static System.Timers.Timer aTimer;
        private int counter = 10;
        protected override async Task OnInitializedAsync()
        {
            Model.RequestItems = new();
            await GetVoteCodesAsync();
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
            await base.OnInitializedAsync();
        }
        private void RemoveItem(ItemDto item)
        {
            Model.RequestItems.Remove(item);
        }
        private async Task Submit()
        {
            if (Model.RequestItems.Count == 0)
            {
                _snackBar.Add("You must Select 1 Item at least", Severity.Error);
                return;
            }
            _isSubmiting = true;
            Model.VoteCodeId = Model.RequestItems.FirstOrDefault().VotecodeId;
            await UploadAttachmentAsync();
            var response = await _MprManager.SaveAsync(Model);
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

        private string VoteCodestr(int Id)
        {
            var code = votecodeList.FirstOrDefault(x => x.Id == Id);
            if (code == null)
            {
                return string.Empty;
            }
            return code.VoteCode;
        }
        private void UploadFiles(InputFileChangeEventArgs e)
        {
            Model.Attachments = new();
            foreach (var file in e.GetMultipleFiles())
            {
                files.Add(file);
                Model.Attachments.Add(new Application.Requests.UploadRequest());
            }

        }

        private async Task UploadAttachmentAsync()
        {
            Model.Attachments = new List<Application.Requests.UploadRequest>();

            foreach (var _file in files)
            {
                Application.Requests.UploadRequest att = new Application.Requests.UploadRequest();
                var buffer = new byte[_file.Size];
                var extension = Path.GetExtension(_file.Name);
                var format = "application/octet-stream";

                await _file.OpenReadStream(_file.Size).ReadAsync(buffer);
                att = new Application.Requests.UploadRequest() { Data = buffer, FileName = _file.Name, UploadType = Application.Enums.UploadType.Qoutaions, Extension = extension };

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
            DialogParameters parameters = new DialogParameters();
            if (Model.RequestItems.Count>0)
            {
                int vc = Model.RequestItems.FirstOrDefault().VotecodeId;
                 parameters.Add(nameof(SelectItemModal.VoteCodeId), vc);
                parameters.Add(nameof(SelectItemModal.Exist), Model.RequestItems.Select(x => x.ItemId).ToList());
            }
        
            var dialog = DialogService.Show<SelectItemModal>("Select Item", parameters, options);
            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                _Item = result.Data as GetAllItemsResponse;
                Model.RequestItems.Add(new Application.Features.MprRequest.Dto.Request.ItemDto() 
                {
                    ItemId = _Item.Id,
                    Unit = _Item.MeasureName,
                    ItemClass = _Item.ItemClass.ToDescriptionString(),
                    ItemArName = _Item.ItemArName,
                    ItemCode = _Item.ItemCode,
                    ItemName = _Item.ItemName,
                    ItemNSN = _Item.ItemNsn,
                    VotecodeId = _Item.VoteCodesId.Value
                  
                });
                //Model.ItemId = _Item.Id;
                //Model.VoteCodeId = _Item.VoteCodesId.Value;
                //Model.Unit = _Item.MeasureName;
                //Model.ItemClass = _Item.ItemClass.ToDescriptionString();
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
