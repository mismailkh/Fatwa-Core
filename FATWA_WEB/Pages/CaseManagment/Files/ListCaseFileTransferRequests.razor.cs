using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_WEB.Pages.CaseManagment.Shared;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Globalization;
using System.Text;

namespace FATWA_WEB.Pages.CaseManagment.Files
{
    public partial class ListCaseFileTransferRequests : ComponentBase
    {

        #region Variables
        protected RadzenDataGrid<CmsCaseFileTransferRequestVM>? grid;
        public int sectorTypeId = 0;
        #endregion
        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
        }
        #endregion
        #region Grid Events
        IEnumerable<CmsCaseFileTransferRequestVM> _getCaseFileTransferRequests;
        IEnumerable<CmsCaseFileTransferRequestVM> FilteredCaseFileTransferRequests { get; set; } = new List<CmsCaseFileTransferRequestVM>();
        protected IEnumerable<CmsCaseFileTransferRequestVM> getCaseFileTransferRequests
        {
            get
            {
                return _getCaseFileTransferRequests;
            }
            set
            {
                if (!object.Equals(_getCaseFileTransferRequests, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getTransferRequests", NewValue = value, OldValue = _getCaseFileTransferRequests };
                    _getCaseFileTransferRequests = value;

                    Reload();
                }

            }
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        string _search;
        protected string search
        {
            get
            {
                return _search;
            }
            set
            {
                if (!object.Equals(_search, value))
                {
                    var args = new FATWA_WEB.Services.PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;

                    Reload();
                }
            }
        }
        protected async Task Load()
        {
            try
            {
                sectorTypeId = (int)loginState.UserDetail.SectorTypeId;
                var response = await cmsSharedService.GetCaseFileTransferRequestList(sectorTypeId);
                if (response.IsSuccessStatusCode)
                {
                    _getCaseFileTransferRequests = (IEnumerable<CmsCaseFileTransferRequestVM>)response.ResultData;
                    FilteredCaseFileTransferRequests = (IEnumerable<CmsCaseFileTransferRequestVM>)response.ResultData;
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                await grid.Reload();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async Task OnSearchInput()
        {
            try
            {
				if (string.IsNullOrEmpty(search))
				{
					search = "";
				}
				else
					search = search.ToLower();
				if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
				{
                    FilteredCaseFileTransferRequests = await gridSearchExtension.Filter(_getCaseFileTransferRequests, new Query()
					{
						Filter = $@"i => (i.SectorToNameEn != null && i.SectorToNameEn.ToString().ToLower().Contains(@0)) || (i.StatusNameEn != null && i.StatusNameEn.ToString().ToLower().Contains(@1)) || ( i.CreatedDate != null && i.CreatedDate.ToString().ToLower().Contains(@2))",
						FilterParameters = new object[] { RemoveDiacritics(search), RemoveDiacritics(search), RemoveDiacritics(search), search }
					});
				}
				else
				{
                    FilteredCaseFileTransferRequests = await gridSearchExtension.Filter(_getCaseFileTransferRequests, new Query()
					{
						Filter = $@"i => (i.SectorToNameAr != null && i.SectorToNameAr.ToString().ToLower().Contains(@0)) ||(i.StatusNameAr != null && i.StatusNameAr.ToString().ToLower().Contains(@1) ) || ( i.CreatedDate != null && i.CreatedDate.ToString().ToLower().Contains(@2))",
						FilterParameters = new object[] { search, search }
					});
				}
			}
            catch (Exception ex)
            {
                throw;
            }
        }
			#endregion

        #region Redirect Function
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion
        protected string RemoveDiacritics(string text)
        {
            // Use CultureInfo.InvariantCulture for consistent comparison
            CultureInfo culture = CultureInfo.InvariantCulture;

            // Remove diacritic marks from the text
            return new string(
                text.Normalize(NormalizationForm.FormD)
                    .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    .ToArray()
            ).Normalize(NormalizationForm.FormC);
        }
        protected async Task SelectSectorForTransfer(MouseEventArgs args)
        {
            try
            {
                var result = await dialogService.OpenAsync<CreateCaseFileTransferRequest>(translationState.Translate("Request_For_Transfer"),
                    new Dictionary<string, object>()
                    {
                          { "SectorTypeId", loginState.UserDetail.SectorTypeId }
                    },
                    new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true }
                );
                if (result != null)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Transfer_Requests_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await Load();
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
    }
}
