using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_WEB.Services;
using FATWA_WEB.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.Lds
{
    public partial class LegalLegislationList : ComponentBase
    {
        #region Variables Declaration
        protected RadzenDataGrid<LegalLegislationsVM> grid = new RadzenDataGrid<LegalLegislationsVM>();
        protected AdvanceSearchLegalLegislationsVM advanceSearchVM = new AdvanceSearchLegalLegislationsVM();
        protected List<LegalLegislationStatus> statuses { get; set; } = new List<LegalLegislationStatus>();
        protected List<LegalLegislationType> legislationTypes { get; set; } = new List<LegalLegislationType>();
        protected List<LegalLegislationReference> legislationReferences { get; set; } = new List<LegalLegislationReference>();
        protected List<LegalLegislation> legalLegislations { get; set; } = new List<LegalLegislation>();
        IEnumerable<LegalLegislationsVM> LegalLegislation { get; set; } = new List<LegalLegislationsVM>();
        public bool isVisible { get; set; }
        protected bool Keywords = false;
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int CurrentPageSize => grid.PageSize;
        protected string search { get; set; }
        IEnumerable<LegalLegislationsVM> _getLegalLegislations;
        protected IEnumerable<LegalLegislationsVM> getLegalLegislations { get; set; }
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion

        #region Initialized
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateLegislationTypes();
            await PopulateLegislationStatuses();
            translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
        } 
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        #endregion

        #region On Load Grid Data 
        protected async Task OnLoadData(LoadDataArgs dataArgs)
        {
            try
            {
                if (string.IsNullOrEmpty(dataArgs.OrderBy) || CurrentPage != advanceSearchVM.PageNumber || CurrentPageSize != advanceSearchVM.PageSize || (Keywords && advanceSearchVM.isDataSorted))
                {
                    if (advanceSearchVM.isGridLoaded && advanceSearchVM.PageSize == CurrentPageSize && !advanceSearchVM.isPageSizeChangeOnFirstLastPage)
                    {
                        grid.CurrentPage = (int)advanceSearchVM.PageNumber - 1;
                        advanceSearchVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(dataArgs);
                    spinnerService.Show();
                    var response = await legalLegislationService.GetLegalLegislations(advanceSearchVM);
                    if (response.IsSuccessStatusCode)
                    {
                        LegalLegislation = (IEnumerable<LegalLegislationsVM>)response.ResultData;
                        getLegalLegislations = (IEnumerable<LegalLegislationsVM>)response.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput(search);
                        if (!(string.IsNullOrEmpty(dataArgs.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            getLegalLegislations = await gridSearchExtension.Sort(getLegalLegislations, ColumnName, SortOrder);
                        }
                        await InvokeAsync(StateHasChanged);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion

        #region Grid Pagination Calculation
        private void SetPagingProperties(LoadDataArgs args)
        {
            if (advanceSearchVM.PageSize != null && advanceSearchVM.PageSize != CurrentPageSize)
            {
                int oldPageCount = LegalLegislation.Any() ? (LegalLegislation.First().TotalCount) / ((int)advanceSearchVM.PageSize) : 1;
                int oldPageNumber = (int)advanceSearchVM.PageNumber - 1;
                advanceSearchVM.isGridLoaded = true;
                advanceSearchVM.PageNumber = CurrentPage;
                advanceSearchVM.PageSize = args.Top;
                int TotalPages = LegalLegislation.Any() ? (LegalLegislation.First().TotalCount) / (grid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    advanceSearchVM.PageNumber = TotalPages + 1;
                    advanceSearchVM.PageSize = args.Top;
                    grid.CurrentPage = TotalPages;
                }
                if ((advanceSearchVM.PageNumber == 1 || (advanceSearchVM.PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                {
                    advanceSearchVM.isPageSizeChangeOnFirstLastPage = true;
                }
                else
                {
                    advanceSearchVM.isPageSizeChangeOnFirstLastPage = false;
                }
                return;
            }
            advanceSearchVM.PageNumber = CurrentPage;
            advanceSearchVM.PageSize = args.Top;
        }
        #endregion

        #region On Sort Grid Data
        private async Task OnSortData(DataGridColumnSortEventArgs<LegalLegislationsVM> args)
        {
            if (args.SortOrder != null)
            {
                getLegalLegislations = await gridSearchExtension.Sort(getLegalLegislations, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        #endregion

        #region Advance Search
        protected async Task SubmitAdvanceSearch()
        {
            if (advanceSearchVM.Start_From > advanceSearchVM.End_To)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                Keywords = advanceSearchVM.isDataSorted = true;
                return;
            }
            if (string.IsNullOrWhiteSpace(advanceSearchVM.Legislation_Number)
                && advanceSearchVM.Legislation_Type == 0
                && string.IsNullOrWhiteSpace(advanceSearchVM.Introduction)
                && !advanceSearchVM.Start_From.HasValue
                && !advanceSearchVM.End_To.HasValue
                && advanceSearchVM.Legislation_Status == 0
                && string.IsNullOrWhiteSpace(advanceSearchVM.LegislationTitle)) { }
            else
            {
                Keywords = advanceSearchVM.isDataSorted = true;
                if (grid.CurrentPage > 0)
                    await grid.FirstPage();
                else
                {
                    advanceSearchVM.isGridLoaded = false;
                    await grid.Reload();
                }
                StateHasChanged();
            }
        }
        public async void ResetForm()
        {
            advanceSearchVM = new AdvanceSearchLegalLegislationsVM { PageSize = grid.PageSize };
            Keywords = advanceSearchVM.isDataSorted = false;
            grid.Reset();
            await grid.Reload();
            StateHasChanged();
        }
        protected async Task ToggleAdvanceSearch()
        {
            isVisible = !isVisible;
            if (!isVisible)
            {
                ResetForm();
            }
        }

        #endregion

        #region GRID Buttons
        //<History Author = 'Nabeel ur Rehman' Date='2022-06-06' Version="1.0" Branch="master"> Redirect to View Detail page</History>

        protected async Task LegislationDescision(LegalLegislationsVM args)
        {
            navigationManager.NavigateTo("legislation-decision/" + args.LegislationId);
        }
        protected async Task AddLegislation(MouseEventArgs args)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<AddLegalLegislationDialog>(
                    translationState.Translate("Add_Legislation_Source_Docuemnt"),
                    new Dictionary<string, object>()
                    {
                        {"LegislationIdForEditAttachment", Guid.Empty }
                    },
                    new DialogOptions() { CloseDialogOnOverlayClick = true, Width = "70%" });
                await Task.Delay(200);
                grid.Reset();
                await grid.Reload();
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
        protected async Task ViewLegislationDetail(LegalLegislationsVM args)
        {
            navigationManager.NavigateTo("legallegislation-detailview/" + args.LegislationId);
        }
        protected async Task EditLegalLegalLegislation(LegalLegislationsVM args)
        {
            var result = await dialogService.Confirm(translationState.Translate("Edit_Legislation_Attachment_Popup_Confirm_Message"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("Yes"),
                CancelButtonText = translationState.Translate("No")
            });
            if (result == true)
            {
                var dialogResult = await dialogService.OpenAsync<AddLegalLegislationDialog>(
                    translationState.Translate("Add_Legislation_Source_Docuemnt"),
                    new Dictionary<string, object>()
                    {
                        {"LegislationIdForEditAttachment", args.LegislationId }
                    },
                    new DialogOptions() { CloseDialogOnOverlayClick = true, Width = "70%" });
                await Task.Delay(200);
            }
            else if (result == false)
            {
                navigationManager.NavigateTo("add-legislation/" + args.LegislationId + "/" + true);
            }
        }
        protected async Task DeleteLegislation(LegalLegislationsVM args)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Delete_Lesgislation"), translationState.Translate("delete"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                var response = await legalLegislationService.GetLegalLegislationReferenceByLegislationId(args.LegislationId);
                if (response.IsSuccessStatusCode)
                {
                    legislationReferences = (List<LegalLegislationReference>)response.ResultData;
                    var result = legislationReferences.Count();


                    if (result > 0)
                    {

                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Warning,
                            Detail = translationState.Translate("Unable_Delete_Legislation") + "  " + result + " " + translationState.Translate("Reference_With_Legal_Legislation"),
                            Style = "position: fixed !important; left: 0; margin: auto;"

                        });

                    }
                    else
                    {
                        await legalLegislationService.DeleteLegalLegislation(args);
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Legislation_Deleted_Success_Message"),
                            Style = "position:fixed !important;left: 0; margin: auto;"
                        });
                        grid.Reset();
                        await grid.Reload();
                        StateHasChanged();
                    }
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
        #endregion

        #region Remote Dropdown Data and Dropdown Change Events
        protected async Task PopulateLegislationTypes()
        {
            var response = await legalLegislationService.GetLegislationTypeDetails();
            if (response.IsSuccessStatusCode)
            {
                legislationTypes = (List<LegalLegislationType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulateLegislationStatuses()
        {
            var response = await legalLegislationService.GetLegislationStatusDetails();
            if (response.IsSuccessStatusCode)
            {
                statuses = (List<LegalLegislationStatus>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
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

        #region Grid Search
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    getLegalLegislations = await gridSearchExtension.Filter(LegalLegislation, new Query()
                {
                    Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US" 
                    ? $@"i => (i.Legislation_Number != null && i.Legislation_Number.ToString().ToLower().Contains(@0)) || (i.Legislation_Type_En != null && i.Legislation_Type_En.ToString().ToLower().Contains(@1)) || (i.LegislationTitle != null && i.LegislationTitle.ToString().ToLower().Contains(@2)) || (i.Legislation_Status_En != null && i.Legislation_Status_En.ToString().ToLower().Contains(@3)) || (i.Legislation_Flow_Status_En != null && i.Legislation_Flow_Status_En.ToString().ToLower().Contains(@4))" 
                    : $@"i => (i.Legislation_Number != null && i.Legislation_Number.ToString().ToLower().Contains(@0)) || (i.Legislation_Type_Ar != null && i.Legislation_Type_En.ToString().ToLower().Contains(@1)) || (i.LegislationTitle != null && i.LegislationTitle.ToString().ToLower().Contains(@2)) || (i.Legislation_Status_Ar != null && i.Legislation_Status_Ar.ToString().ToLower().Contains(@3)) || (i.Legislation_Flow_Status_Ar != null && i.Legislation_Flow_Status_En.ToString().ToLower().Contains(@4))"
                    ,
                    FilterParameters = new object[] { search, search, search, search, search }
                });
                if (!string.IsNullOrEmpty(ColumnName))
                {
                    getLegalLegislations = await gridSearchExtension.Sort(getLegalLegislations, ColumnName, SortOrder);
                }
                    await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
    }
}
