using FATWA_DOMAIN.Models;
using FATWA_WEB.Data;
using FATWA_WEB.Pages.Lms;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.Lms
{
    public partial class ListLiteratureParentIndexs : ComponentBase
    {
        public ListLiteratureParentIndexs()
        {
            IndexNumberParentIndexesResult = new List<LmsLiteratureParentIndex>();
            IndexNumberParentIndexeCounter = 0;
        }

        #region Variables Declared
        private IEnumerable<LmsLiteratureParentIndex> IndexNumberParentIndexesResult;
        private IEnumerable<LmsLiterature> lmsLiteraturesResult;
        private int IndexNumberParentIndexeCounter;
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }
        protected RadzenDataGrid<LmsLiteratureParentIndex> grid0;

        protected int count { get; set; }

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

        IEnumerable<LmsLiteratureParentIndex> _getLmsLiteratureParentIndexResult;
        IEnumerable<LmsLiteratureParentIndex> FilteredgetLmsLiteratureParentIndexResult { get; set; }=new List<LmsLiteratureParentIndex>();
        protected IEnumerable<LmsLiteratureParentIndex> getLmsLiteratureParentIndexResult
        {
            get
            {
                return _getLmsLiteratureParentIndexResult;
            }
            set
            {
                if (!object.Equals(_getLmsLiteratureParentIndexResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLmsLiteratureParentIndexResult", NewValue = value, OldValue = _getLmsLiteratureParentIndexResult };
                    _getLmsLiteratureParentIndexResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        #endregion

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

            if (string.IsNullOrEmpty(search))
            {
                search = "";
            }
            else
                search = search.ToLower();
                var result = await lmsliteratureParentIndexServices.GetLmsLiteratureParentIndexs();
            if (result != null && result.Count() > 0)
            {
                getLmsLiteratureParentIndexResult = result;
                FilteredgetLmsLiteratureParentIndexResult = result;
                count = getLmsLiteratureParentIndexResult.Count();
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
                if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                {
                    FilteredgetLmsLiteratureParentIndexResult = await gridSearchExtension.Filter(getLmsLiteratureParentIndexResult,new Query()
                    {
                        Filter = $@"i => (i.Name_En != null && i.Name_En.ToLower().Contains(@0) ) || (i.ParentIndexNumber != null && i.ParentIndexNumber.Contains(@1) )",
                        FilterParameters = new object[] { search, search }
                    });
                }
                else
                {
                    FilteredgetLmsLiteratureParentIndexResult = await gridSearchExtension.Filter(getLmsLiteratureParentIndexResult,new Query()
                    {
                        Filter = $@"i => (i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@0) ) || (i.ParentIndexNumber != null && i.ParentIndexNumber.Contains(@1) )",
                        FilterParameters = new object[] { search, search }
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
            #endregion


            #region add Parent book index
            protected async Task AddParentIndexButton0Click(MouseEventArgs args)
        {
            var dialogResult = await dialogService.OpenAsync<AddLiteratureParentIndex>(
                translationState.Translate("Add_Lms_Literature_Parent_Index"),
                null,
                new DialogOptions() { CloseDialogOnOverlayClick = true });
            await Task.Delay(300);
            await Load();
        }
        #endregion

        #region edit book parent index
        protected async Task GridEditButtonClick(MouseEventArgs args, LmsLiteratureParentIndex data)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<AddLiteratureParentIndex>(
                    translationState.Translate("Edit_Lms_Literature_Parent_Index"),
                    new Dictionary<string, object>() { { "ParentIndexId", data.ParentIndexId } },
                    new DialogOptions() { CloseDialogOnOverlayClick = true });
                await Task.Delay(300);
                await Load();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region export to csv & xslv

        protected async System.Threading.Tasks.Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await lmsliteratureParentIndexServices.ExportLmsLiteratureParentIndexsToCSV(new Query()
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Select = "Name, Name_Ar, ParentIndexNumber, CreatedDate"
                }, $"Lms Literature Parent Indexs");
            }
            if (args == null || args.Value == "xlsx")
            {
                await lmsliteratureParentIndexServices.ExportLmsLiteratureParentIndexsToExcel(new Query()
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Select = "Name, Name_Ar, ParentIndexNumber, CreatedDate"
                }, $"Lms Literature Parent Indexs");
            }
        }
        #endregion

        #region rediect to book index
        protected async Task Grid0RowSelect(LmsLiteratureParentIndex args)
        {
            navigationManager.NavigateTo("/lmsliteratureindex-list/" + args.ParentIndexId + "/" + args.ParentIndexNumber);
        }

        #endregion

        #region delete selected book index
        protected async Task GridDeleteButtonClick(MouseEventArgs args, LmsLiteratureParentIndex data)
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
                    var response = await lmsLiteratureIndexServices.GetLmsLiteratureIndexDetilsByUsingParentIdAndNumber(data.ParentIndexId, data.ParentIndexNumber);
                    // if record found against parent id & parent index number in index table then check either that index number's is associated with literature's or not.
                    if (response.IsSuccessStatusCode)
                    {
                        var IndexesDetailsResult = (List<LmsLiteratureIndex>)response.ResultData;
                        foreach (var item in IndexesDetailsResult)
                        {
                            //check if existinng index number(index id) associated with laterature's.
                            lmsLiteraturesResult = await lmsLiteratureIndexServices.CheckLmsLiteratureIndexIdAssociatedWithLiteratures(item.IndexId);
                            if (lmsLiteraturesResult.Count() > 0)
                            {
                                IndexNumberParentIndexeCounter += lmsLiteraturesResult.Count();
                            }
                        }
                        if (IndexNumberParentIndexeCounter == 0)
                        {
                            spinnerService.Show();
                            var fatwaDbDeleteLmsLiteratureResult = await lmsliteratureParentIndexServices.DeleteLmsLiteratureParentIndex(data.ParentIndexId);
                            spinnerService.Hide();
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = lmsliteratureParentIndexServices.ResultDetails,
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
                                Detail = translationState.Translate("Parent_index_number_is_associated") + " " + IndexNumberParentIndexeCounter + " " + translationState.Translate("Parent_index_number_is_associated_message"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            IndexNumberParentIndexeCounter = 0;
                            lmsLiteraturesResult = null;
                        }
                    }
                    else // if no record found against parent index id in index table then delete parent index record.
                    {
                        spinnerService.Show();
                        var fatwaDbDeleteLmsLiteratureResult = await lmsliteratureParentIndexServices.DeleteLmsLiteratureParentIndex(data.ParentIndexId);
                        spinnerService.Hide();
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = lmsliteratureParentIndexServices.ResultDetails,
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await Task.Delay(300);
                        await Load();
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
        #endregion

    }
}
