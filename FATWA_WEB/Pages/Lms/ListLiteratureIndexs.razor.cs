using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.Lms
{
    public partial class ListLiteratureIndexs : ComponentBase
    {
        public ListLiteratureIndexs()
        {
            IndexNumberIndexesId = new List<LmsLiteratureIndex>();
            LmsLiteratureIndexCounter = 0;
        }

        #region Parameters
        [Parameter]
        public dynamic ParentIndexId { get; set; }
        [Parameter]
        public dynamic ParentIndexNumber { get; set; }
        #endregion

        #region Variables Declared
        private IEnumerable<LmsLiteratureIndex> IndexNumberIndexesId;
        private IEnumerable<LmsLiterature> lmsLiteraturesResult;
        private int LmsLiteratureIndexCounter;
        protected RadzenDataGrid<LmsLiteratureIndex> grid0;
        protected int totalRecords { get; set; }
        protected

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
        #endregion

        
        #region Full property declared 

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        IEnumerable<LmsLiteratureIndex> _getLmsLiteratureIndexResult;
        protected IEnumerable<LmsLiteratureIndex> FilteredGetLmsLiteratureIndexResult { get; set; } = new List<LmsLiteratureIndex>();
        protected IEnumerable<LmsLiteratureIndex> getLmsLiteratureIndexResult
        {
            get
            {
                return _getLmsLiteratureIndexResult;
            }
            set
            {
                if (!object.Equals(_getLmsLiteratureIndexResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLmsLiteratureIndexResult", NewValue = value, OldValue = _getLmsLiteratureIndexResult };
                    _getLmsLiteratureIndexResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        #endregion

        public LmsLiteratureParentIndexVM RegisteredIndexDetails { get; set; } = new LmsLiteratureParentIndexVM();

        #region initialized & load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            await Load();
            translationState.TranslateGridFilterLabels(grid0);

            spinnerService.Hide();
        }

        protected async Task Load()
        {
            var result = await lmsLiteratureIndexService.GetLiteratureIndexByIndexIdAndNumber(int.Parse(ParentIndexId), Convert.ToString(ParentIndexNumber));
            if (result.IsSuccessStatusCode)
            {
                RegisteredIndexDetails = (LmsLiteratureParentIndexVM)result.ResultData;
            }
            else
            {
                RegisteredIndexDetails = new LmsLiteratureParentIndexVM();
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }

            var response = await lmsLiteratureIndexService.GetLmsLiteratureIndexDetilsByUsingParentIdAndNumber(int.Parse(ParentIndexId), Convert.ToString(ParentIndexNumber));
            if(response.IsSuccessStatusCode)
            {
                getLmsLiteratureIndexResult = (List<LmsLiteratureIndex>)response.ResultData;
                FilteredGetLmsLiteratureIndexResult = (List<LmsLiteratureIndex>)response.ResultData;
                totalRecords = getLmsLiteratureIndexResult.Count();
            }
            else
            {
                //notificationService.Notify(new NotificationMessage()
                //{
                //    Severity = NotificationSeverity.Error,
                //    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                //    Style = "position: fixed !important; left: 0; margin: auto; "
                //});
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
                FilteredGetLmsLiteratureIndexResult = await gridSearchExtension.Filter(getLmsLiteratureIndexResult,
                new Query()
                {
                    Filter = $@"i => (i.Name_En != null && i.Name_En.ToLower().Contains(@0)) || (i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@1) )||(i.IndexNumber != null && i.IndexNumber.ToLower().Contains(@1))",
                    FilterParameters = new object[] { search, search, search }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region add book index
        protected async Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await dialogService.OpenAsync<AddLiteratureIndex>(
                translationState.Translate("Add_Lms_Literature_Index"),
                new Dictionary<string, object>() { { "ParentIndexId", ParentIndexId }, { "ParentIndexNumber", ParentIndexNumber } },
                new DialogOptions() { CloseDialogOnOverlayClick = true });
            await Task.Delay(300);
            await Load();

        }
        #endregion

        #region export to csv & xslv

        protected async System.Threading.Tasks.Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await lmsLiteratureIndexService.ExportLmsLiteratureIndexsToCSV(new Query()
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Select = "Name, Name_Ar, IndexNumber, IndexCreationDate"
                }, $"Lms Literature Indexs");

            }

            if (args == null || args.Value == "xlsx")
            {
                await lmsLiteratureIndexService.ExportLmsLiteratureIndexsToExcel(new Query()
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Select = "Name, Name_Ar, IndexNumber, IndexCreationDate"
                }, $"Lms Literature Indexs");
            }
        }
        #endregion

        #region rediect to book index division
        protected async Task Grid0RowSelect(LmsLiteratureIndex args)
        {
            navigationManager.NavigateTo("/lmsliteratureindexdivisionaisle-list/" + args.IndexId);
        }
        #endregion

        #region Index child call
        protected async Task ShowChildsByUsingParentIdAndNumber(LmsLiteratureIndex args)
        {
            if (!args.IndexNumber.Contains("."))
            {
                navigationManager.NavigateTo("/lmsliteratureindex-list/" + args.IndexId + "/" + args.IndexNumber, forceLoad: true);
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Info,
                    Detail = translationState.Translate("End_Level"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion

        #region edit book index
        protected async Task GridEditButtonClick(MouseEventArgs args, LmsLiteratureIndex data)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<EditLiteratureIndex>(
                    translationState.Translate("Edit_Lms_Literature_Index"),
                    new Dictionary<string, object>() { { "IndexId", data.IndexId } },
                    new DialogOptions() { CloseDialogOnOverlayClick = true });
                await Task.Delay(300);
                await Load();
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

        #region delete selected book index
        protected async Task GridDeleteButtonClick(MouseEventArgs args, LmsLiteratureIndex data)
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
                    //check if existinng index number(index id) associated with laterature's.
                    lmsLiteraturesResult = await lmsLiteratureIndexService.CheckLmsLiteratureIndexIdAssociatedWithLiteratures(data.IndexId);
                    if (lmsLiteraturesResult.Count() > 0)
                    {
                        LmsLiteratureIndexCounter = lmsLiteraturesResult.Count();
                    }
                    if (LmsLiteratureIndexCounter == 0)
                    {
                        spinnerService.Show();
                        var fatwaDbDeleteLmsLiteratureResult = await lmsLiteratureIndexService.DeleteLmsLiteratureIndex(data.IndexId);
                        spinnerService.Hide();
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = lmsLiteratureIndexService.ResultDetails,
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await Task.Delay(300);
                        await Load();
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Info,
                            Detail = translationState.Translate("This_index_number_is_associated_with") + " " + LmsLiteratureIndexCounter + " " + translationState.Translate("Authors_The_connected_index_number_cannot_be_edited"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        LmsLiteratureIndexCounter = 0;
                        lmsLiteraturesResult = null;
                    }
                }
            }
            catch (System.Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
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

		protected async Task RedirectBack()
		{
			await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
		}
		#endregion

    }
}
