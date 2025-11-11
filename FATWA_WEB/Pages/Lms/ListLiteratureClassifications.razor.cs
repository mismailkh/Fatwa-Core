using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using FATWA_WEB.Services;
using Microsoft.EntityFrameworkCore;
using FATWA_DOMAIN.Models;
using FATWA_WEB.Data;
using FATWA_WEB.Pages.Lms;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Lms
{
    //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master">Literature Classification Component</History>
    //<History Author = 'Zain Ul Islam' Date='2022-08-01' Version="1.0" Branch="master">Literature Classification Component</History>

    public partial class ListLiteratureClassifications : ComponentBase
    {

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

      
        #region Variables Declaration

        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }
        protected RadzenDataGrid<LmsLiteratureClassification> grid0;
        public bool isLoading { get; set; }
        public int count { get; set; }

        string _search;
        public bool isVisible { get; set; }
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
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<LmsLiteratureClassification> _getLmsLiteratureClassificationsResult;
        IEnumerable<LmsLiteratureClassification> FilteredGetLmsLiteratureClassificationsResult { get; set; } = new List<LmsLiteratureClassification>();
        protected IEnumerable<LmsLiteratureClassification> getLmsLiteratureClassificationsResult
        {
            get
            {
                return _getLmsLiteratureClassificationsResult;
            }
            set
            {
                if (!object.Equals(_getLmsLiteratureClassificationsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLmsLiteratureClassificationsResult", NewValue = value, OldValue = _getLmsLiteratureClassificationsResult };
                    _getLmsLiteratureClassificationsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        #endregion

        #region On Component Load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            await Load();
            translationState.TranslateGridFilterLabels(grid0);

            spinnerService.Hide();
        }

        protected async Task Load()
        {
            
             var result = await lmsLiteratureClassificationService.GetLiteratureClassifications();
            if (result.IsSuccessStatusCode)
            {
                
                    getLmsLiteratureClassificationsResult =(List<LmsLiteratureClassification>)result.ResultData;
                    FilteredGetLmsLiteratureClassificationsResult = (List<LmsLiteratureClassification>)result.ResultData;
                    count = getLmsLiteratureClassificationsResult.Count();
                
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
                    FilteredGetLmsLiteratureClassificationsResult = await gridSearchExtension.Filter(getLmsLiteratureClassificationsResult,
                    new Query()
                    {
                        Filter = $@"i => (i.Name_En != null && i.Name_En.ToLower().Contains(@0)) || (i.CreatedBy != null && i.CreatedBy.ToLower().Contains(@1) )",
                        FilterParameters = new object[] { search, search }
                    });

                }
                else
                {
                    FilteredGetLmsLiteratureClassificationsResult = await gridSearchExtension.Filter(getLmsLiteratureClassificationsResult,
                    new Query()
                    {
                        Filter = $@"i => (i.Name_Ar != null &&  i.Name_Ar.ToLower().Contains(@0)) || (i.CreatedBy != null && i.CreatedBy.ToLower().Contains(@1) )",
                        FilterParameters = new object[] { search, search }
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
            #endregion

            #region Export File

            protected async Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                lmsLiteratureClassificationService.ExportLmsLiteratureClassificationsToCSV(new Query()
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Expand = "",
                    Select = "ClassificationId,Name,Name_Ar,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted"
                },
                    $"Lms Literature Classifications");

            }

            if (args == null || args.Value == "xlsx")
            {
                lmsLiteratureClassificationService.ExportLmsLiteratureClassificationsToExcel(new Query()
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Expand = "",
                    Select = "ClassificationId,Name,Name_Ar,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted"
                },
                    $"Lms Literature Classifications");

            }
        }

        #endregion

        #region Functions 

        protected async Task AddLiteratureClassification(MouseEventArgs args)
        {
            var dialogResult = await dialogService.OpenAsync<AddLiteratureClassification>(
                translationState.Translate("Add_category_literature"),
                null,
                new DialogOptions() { CloseDialogOnOverlayClick = true });
            await Task.Delay(400);
            await Load();
        }
        protected async Task EditLiteratureClassification(MouseEventArgs args, dynamic data)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<EditLiteratureClassification>(
                 translationState.Translate("Literature_classification_editing"),
                 new Dictionary<string, object>() { { "ClassificationId", data.ClassificationId } },
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
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                    translationState.Translate("Delete_Catalogue_Message"),
                    translationState.Translate("Confirm"),
                    new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    });
                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    var response = await lmsLiteratureClassificationService.DeleteLmsLiteratureClassification(data.ClassificationId);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = (int)response.ResultData;
                        if (result > 0)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Info,
                                Detail = translationState.Translate("Literature_Classification_number_is_associated_with") + " " + result + " " + translationState.Translate("Literature_Classification_with_Literature"),
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
                    spinnerService.Hide();

                    //if (result != null)
                    //{
                    //    if (result == translationState.Translate("Item_Unavailable"))
                    //    {
                    //        notificationService.Notify(new NotificationMessage()
                    //        {
                    //            Severity = NotificationSeverity.Error,
                    //            Detail = translationState.Translate("Item_Unavailable"),
                    //            Style = "position: fixed !important; left: 0; margin: auto; "
                    //        });
                    //    }
                    //    else if (result == translationState.Translate("Classification_Delete_Failed"))
                    //    {
                    //        notificationService.Notify(new NotificationMessage()
                    //        {
                    //            Severity = NotificationSeverity.Error,
                    //            Detail = translationState.Translate("Classification_Delete_Failed"),
                    //            Style = "position: fixed !important; left: 0; margin: auto; "
                    //        });
                    //    }
                    //    else
                    //    {
                    //        notificationService.Notify(new NotificationMessage()
                    //        {
                    //            Severity = NotificationSeverity.Success,
                    //            Detail = translationState.Translate("Deleted_Successfully"),
                    //            Style = "position: fixed !important; left: 0; margin: auto; "
                    //        });
                    //    }

                    //    await Load();
                    //    //await grid0.Reload();
                    //}

                }
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

        #endregion

        #region Advance Search



        //<History Author = 'Hassan Abbas' Date='2022-09-09' Version="1.0" Branch="master">Open Advance search Popup </History>
        protected async Task ToggleAdvanceSearch()
        {
            isVisible = !isVisible;
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
