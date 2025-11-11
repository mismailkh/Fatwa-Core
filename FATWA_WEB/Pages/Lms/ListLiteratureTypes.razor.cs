using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using FATWA_WEB.Services;
using FATWA_DOMAIN.Models;
using FATWA_WEB.Pages.Lms;
using FATWA_WEB.Data;


namespace FATWA_WEB.Pages.Lms
{
    public partial class ListLiteratureTypes : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public int DecisionId { get; set; } = 0;


        public bool allowRowSelectOnRowClick = true;
        protected int count { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {

        }

        

        protected RadzenDataGrid<LmsLiteratureType> grid0;

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

        IEnumerable<LmsLiteratureType> _getLmsLiteratureTypesResult;
        IEnumerable<LmsLiteratureType> FilteredGetLmsLiteratureTypesResult { get; set; } = new List<LmsLiteratureType>();
        protected IEnumerable<LmsLiteratureType> getLmsLiteratureTypesResult
        {
            get
            {
                return _getLmsLiteratureTypesResult;
            }
            set
            {
                if (!object.Equals(_getLmsLiteratureTypesResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLmsLiteratureTypesResult", NewValue = value, OldValue = _getLmsLiteratureTypesResult };
                    _getLmsLiteratureTypesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            await Load();
            translationState.TranslateGridFilterLabels(grid0);

            spinnerService.Hide();
        }
        protected async Task Load()
        {
            IQueryable<LmsLiteratureType> result;
                result = await lmsLiteratureTypeService.GetLmsLiteratureTypes();
            if (result != null && result.Any())
            {
                getLmsLiteratureTypesResult = result;
                FilteredGetLmsLiteratureTypesResult = result;
                count = getLmsLiteratureTypesResult.Count();
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
                    FilteredGetLmsLiteratureTypesResult = await gridSearchExtension.Filter(getLmsLiteratureTypesResult,new Query()
                    {
                        Filter = $@"i => (i.Name_En != null && i.Name_En.ToLower().Contains(@0) ) || (i.CreatedBy != null &&i.CreatedBy.ToLower().Contains(@2) )",
                        FilterParameters = new object[] { search, search, search }
                    });
                }
                else
                {
                    FilteredGetLmsLiteratureTypesResult = await gridSearchExtension.Filter(getLmsLiteratureTypesResult,new Query()
                    {
                        Filter = $@"i => (i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@0) ) || (i.CreatedBy != null &&i.CreatedBy.ToLower().Contains(@2) )",
                        FilterParameters = new object[] { search, search, search }
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
            protected async Task AddLiteratureType(MouseEventArgs args)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<AddLiteratureType>(
                    translationState.Translate("Add_Lms_Literature_Type"),
                    null,
                    new DialogOptions() { CloseDialogOnOverlayClick = true });
                    await Task.Delay(200);
                    await Load();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        protected async Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await lmsLiteratureTypeService.ExportLmsLiteratureTypesToCSV(new Query()
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Expand = "",
                    Select = "TypeId,Name,Name_Ar,CreatedBy,CreatedDate"
                }, $"Lms Literature Types");
            }

            if (args == null || args.Value == "xlsx")
            {
                await lmsLiteratureTypeService.ExportLmsLiteratureTypesToExcel(new Query()
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Expand = "",
                    Select = "TypeId,Name,Name_Ar,CreatedBy,CreatedDate"
                }, $"Lms Literature Types");
            }
        }
        protected async Task EditLiteratureType(MouseEventArgs args, dynamic data)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<EditLiteratureType>(
                    translationState.Translate("Edit_Literature_Type"),
                    new Dictionary<string, object>() { { "TypeId", data.TypeId } },
                    new DialogOptions() { CloseDialogOnOverlayClick = true });
                await Task.Delay(400);
                await Load();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }


        protected async Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            //    try
            //    {
            //        bool? dialogResponse = await dialogService.Confirm(
            //           translationState.Translate("Are_you_sure_you_want_to_delete_this_record"),
            //           translationState.Translate("Confirm"),
            //           new ConfirmOptions()
            //           {
            //               OkButtonText = translationState.Translate("OK"),
            //               CancelButtonText = translationState.Translate("Cancel")
            //           });

            //        if (dialogResponse == true)
            //        {
            //            SpinnerService.Show();
            //            var fatwaDbDeleteLmsLiteratureTypeResult = await lmsLiteratureTypeService.DeleteLmsLiteratureType(data.TypeId);
            //            if (fatwaDbDeleteLmsLiteratureTypeResult != null)
            //            {
            //                await Load();
            //                await grid0.Reload();
            //                SpinnerService.Hide();
            //            }
            //        }
            //    }
            //    catch (System.Exception ex)
            //    {
            //        notificationService.Notify(new NotificationMessage()
            //        {
            //            Severity = NotificationSeverity.Error,
            //            Detail = ex.Message,
            //            //Summary = $"Error",
            //            Style = "position: fixed !important; left: 0; margin: auto; "
            //        }
            //        );
            //    }
            //}
            //Delete functionality by Hassan Iftikhar According to Aqeel Abbasi's Guide line
            if (await dialogService.Confirm(translationState.Translate("Are_you_sure_you_want_to_delete_this_record"), translationState.Translate("delete"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                var response = await lmsLiteratureTypeService.DeleteLmsLiteratureType(data.TypeId);
                if (response.IsSuccessStatusCode)
                {
                    var result = (int)response.ResultData;
                    if (result > 0)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Info,
                            Detail = translationState.Translate("Literature_Type_number_is_associated_with") + " " + result + " " + translationState.Translate("Literature_Type_with_Literature"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                    }
                    await Load();
                    StateHasChanged();
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                }
            }
        }

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
