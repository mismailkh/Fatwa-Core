using FATWA_DOMAIN.Models;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.Lms
{
    public partial class ListLiteratureIndexDivisions : ComponentBase
    {
        public ListLiteratureIndexDivisions()
        {
            IndexNumberIndexesId = new List<LmsLiteratureIndexDivisionAisle>();
            indexIdAssociatedLiteratureResult = new List<LmsLiterature>();
            LmsLiteratureDivisionCounter = 0;
        }
        #region Variables declaration
        [Parameter]
        public dynamic IndexId { get; set; }
        private IEnumerable<LmsLiteratureIndexDivisionAisle> IndexNumberIndexesId;
        private IEnumerable<LmsLiterature> indexIdAssociatedLiteratureResult;
        private IEnumerable<LmsLiterature> lmsLiteraturesResult { get; set; }
        private int LmsLiteratureDivisionCounter;
        protected int count { get; set; }
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }
        protected RadzenDataGrid<LmsLiteratureIndexDivisionAisle> grid0;
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
                    var args = new PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        IEnumerable<LmsLiteratureIndexDivisionAisle> _getLmsLiteratureIndexDivisionResult;
        IEnumerable<LmsLiteratureIndexDivisionAisle> FilteredGetLmsLiteratureIndexDivisionResult;
        protected IEnumerable<LmsLiteratureIndexDivisionAisle> getLmsLiteratureIndexDivisionResult
        {
            get
            {
                return _getLmsLiteratureIndexDivisionResult;
            }
            set
            {
                if (!object.Equals(_getLmsLiteratureIndexDivisionResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLmsLiteratureIndexDivisionResult", NewValue = value, OldValue = _getLmsLiteratureIndexDivisionResult };
                    _getLmsLiteratureIndexDivisionResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        #endregion

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        #region Service Injections
         
          
        

        #endregion

        #region Initialization

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            await Load();
            translationState.TranslateGridFilterLabels(grid0);

            spinnerService.Hide();
        }

        protected async Task Load()
        {
            var response = await lmsLiteratureIndexDivisionServices.GetLmsLiteratureDivisionDetailsByUsingIndexIdForViewPage(Convert.ToInt32(IndexId));
            if(response != null)
            {
                getLmsLiteratureIndexDivisionResult = response;
                FilteredGetLmsLiteratureIndexDivisionResult = response;
                count = getLmsLiteratureIndexDivisionResult.Count();
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
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
                FilteredGetLmsLiteratureIndexDivisionResult = await gridSearchExtension.Filter(getLmsLiteratureIndexDivisionResult,
                new Query()
                {
                    Filter = $@"i => (i.DivisionNumber != null && i.DivisionNumber.ToLower().Contains(@0)) || (i.AisleNumber != null && i.AisleNumber.ToLower().Contains(@1) )",
                    FilterParameters = new object[] { search, search }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Create

        protected async Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await dialogService.OpenAsync<AddLiteratureIndexDivision>(
                translationState.Translate("Add_Lms_Literature_Index_Division"),
                new Dictionary<string, object>() { { "IndexId", IndexId } },
                new DialogOptions() { CloseDialogOnOverlayClick = true });
            await Task.Delay(200);
            await Load();
        }
        #endregion

        #region Export to csv * xlsv

        protected async Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await lmsLiteratureIndexDivisionServices.ExportLmsLiteratureIndexDivisionsToCSV(
                IndexId, new Query()
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Expand = "",
                    Select = "DivisionNumber, AisleNumber, DivisionCreationDate"
                }, $"Lms Literature Index Divisions");
            }

            if (args == null || args.Value == "xlsx")
            {
                await lmsLiteratureIndexDivisionServices.ExportLmsLiteratureIndexDivisionsToExcel(
                IndexId, new Query()
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Expand = "",
                    Select = "DivisionNumber, AisleNumber, DivisionCreationDate"
                }, $"Lms Literature Index Divisions");
            }
        }
        #endregion

        #region Update
        protected async Task Grid0RowSelect(LmsLiteratureIndexDivisionAisle args)
        {
            var dialogResult = await dialogService.OpenAsync<EditLiteratureIndexDivision>(
                translationState.Translate("Edit_Lms_Literature_Index_Division"),
                new Dictionary<string, object>() { { "DivisionAisleId", args.DivisionAisleId } },
                new DialogOptions() { CloseDialogOnOverlayClick = true });
            await Task.Delay(200);
            await Load();
        }
        #endregion

        #region delete

        protected async Task GridDeleteButtonClick(MouseEventArgs args, LmsLiteratureIndexDivisionAisle data)
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                    translationState.Translate("Sure_Delete_The_Record"),
                    translationState.Translate("Confirm"),
                    new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    });

                if (dialogResponse == true)
                {
                    // first check if division id associated with literature's
                    indexIdAssociatedLiteratureResult = await lmsLiteratureIndexDivisionServices.GetLmsLiteratureDivisionDetailByUsingDivisionId(data.DivisionAisleId);
                    if (indexIdAssociatedLiteratureResult.Count() == 0)
                    {
                        var response = await lmsLiteratureIndexDivisionServices.DeleteLmsLiteratureIndexDivision(data.DivisionAisleId);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = lmsLiteratureIndexDivisionServices.ResultDetails,
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            StateHasChanged();
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                        await Task.Delay(200);
                        await Load();
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Info,
                            Detail = translationState.Translate("This_division_index_number_is_associated_with") + " " + indexIdAssociatedLiteratureResult.Count() + " " + translationState.Translate("This_division_index_number_is_associated_with_second"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        indexIdAssociatedLiteratureResult = null;
                    }
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Literatue_Index_Division_Delete_Failed"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
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
        #endregion
    }
}
