using Forces.Application.Features.Items.Commands.AddEdit;
using Forces.Application.Features.Items.Queries.GetAll;
using Forces.Application.Features.MeasureUnits.Queries.GetAll;
using Forces.Application.Responses.VoteCodes;
using Forces.Client.Infrastructure.Managers.Items;
using Forces.Client.Infrastructure.Managers.VoteCodes;
using Forces.Client.Pages.Items;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Forces.Client.Pages.Requests
{
    public partial class SelectItemModal
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [Inject] private IItemsManager ItemManager { get; set; }
        [Inject] private IVoteCodesManager voteCodeManager { get; set; }
        private List<GetAllItemsResponse> _ItemsList = new();
        private GetAllItemsResponse _item = new();
        private string _searchString = "";
        private List<GetAllMeasureUnitsResponse> _UnitList = new();
        private List<VoteCodeResponse> votecodeList = new();

        private bool _loaded = true;
        [Parameter]
        public int? VoteCodeId { get; set; }
        [Parameter]
        public List<int> Exist { get; set; }
        private string VoteCodeString(int Id)
        {
            return votecodeList.FirstOrDefault(x => x.Id == Id).VoteCode;
        }
        private async Task GetVoteCodesAsync()
        {
            var response = await voteCodeManager.GetAllAsync();
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
        private async Task InvokeAddNewModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _item = _ItemsList.FirstOrDefault(c => c.Id == id);
                if (_item != null)
                {
                    parameters.Add(nameof(AddEditItemModal.AddEditModel), new AddEditItemCommand
                    {
                        Id = _item.Id,
                        ItemArName = _item.ItemArName,
                        ItemCode = _item.ItemCode,
                        ItemNsn = _item.ItemNsn,
                        ItemDescription = _item.ItemDescription,
                        ItemName = _item.ItemName,
                        MeasureUnitId = _item.MeasureUnitId,
                        VoteCodesId = _item.VoteCodesId,
                        VoteCode = _item.VoteCode,
                        ItemClass = _item.ItemClass,
                        SerialNumber = _item.SerialNumber,
                        DateOfEnter = _item.DateOfEnter,
                        EndOfServiceDate = _item.EndOfServiceDate,
                        FirstUseDate = _item.FirstUseDate,
                        MadeIn = _item.MadeIn

                    });
                }
            }
            else if (VoteCodeId.HasValue)
            {
                parameters.Add(nameof(AddEditItemModal.AddEditModel), new AddEditItemCommand
                {
                 
                    VoteCodesId = VoteCodeId.Value,
                    VoteCode = VoteCodeString(VoteCodeId.Value), 

                });
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditItemModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await GetItemsAsync();
            }
        }
        private async Task GetItemsAsync()
        {
            var response = await ItemManager.GetAllAsync();
            if (response.Succeeded)
            {
                if (VoteCodeId.HasValue) 
                {
                    _ItemsList = response.Data.ToList().Where(x=>x.VoteCodesId == VoteCodeId.Value).ToList();
                    _ItemsList.RemoveAll(x=> Exist.Contains(x.Id));
                }
                else
                {
                    _ItemsList = response.Data.ToList();
                }
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
            await GetVoteCodesAsync();
            await GetItemsAsync();

            _loaded = false;
        }
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        private async Task Submit(GetAllItemsResponse Item)
        {
            MudDialog.Close(DialogResult.Ok(Item));
        }
        private bool Search(GetAllItemsResponse item)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (item.ItemArName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (item.ItemName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (item.ItemCode?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (item.ItemNsn?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (item.MeasureName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (item.ItemDescription?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            if (item.VoteCode?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            return false;
        }
    }
}
