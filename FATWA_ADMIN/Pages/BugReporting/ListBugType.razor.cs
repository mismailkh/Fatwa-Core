using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.BugReporting;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.BugReporting.BugReportingEnum;

namespace FATWA_ADMIN.Pages.BugReporting
{
    public partial class ListBugType : ComponentBase
    {
        #region Variables
        protected RadzenDataGrid<BugIssueTypeListVM>? bugIssueGrid { get; set; } = new RadzenDataGrid<BugIssueTypeListVM>();
        protected List<BugIssueTypeListVM> FilteredBugIssueListVM = new List<BugIssueTypeListVM>();
        public bool isVisible { get; set; }
        protected bool Keywords = false;

        IEnumerable<BugIssueTypeListVM> _getBugIssues;
        string _search;
        protected IEnumerable<BugIssueTypeListVM> GetBugIssues
        {
            get
            {
                return _getBugIssues;
            }
            set
            {
                if (!Equals(_getBugIssues, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "GetBugIssues", NewValue = value, OldValue = _getBugIssues };
                    _getBugIssues = value;

                    Reload();
                }

            }
        }
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion

        #region OnInitializedAsync
        protected async override Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            translationState.TranslateGridFilterLabels(bugIssueGrid);
            spinnerService.Hide();
        }
        #endregion

        #region Load 
        protected async Task Load()
        {
            try
            {
                var response = await bugReportingService.GetBugIssueList();
                if (response.IsSuccessStatusCode)
                {
                    GetBugIssues = (IEnumerable<BugIssueTypeListVM>)response.ResultData;
                    FilteredBugIssueListVM = (List<BugIssueTypeListVM>)GetBugIssues;
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Remove Bug Type
        protected async Task RemoveBugType(BugIssueTypeListVM args)
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                    translationState.Translate("Sure_Remove_Bug_Type"),
                    translationState.Translate("Submit"),
                    new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    });
                if (dialogResponse != null)
                {
                    if ((bool)dialogResponse)
                    {
                        spinnerService.Show();

                        var response = await bugReportingService.RemoveBugType(args);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Bug_Type_Removed_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            await Load();
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                        spinnerService.Hide();

                    }
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

        #region Assign Bug type to user
        protected async Task AssignBugTypeToUser(BugIssueTypeListVM args)
        {
            try
            {
                await dialogService.OpenAsync<BugTypeUserAssignment>(
            translationState.Translate("User_Configuration_Type"),
            new Dictionary<string, object>() { { "TypeId", args.Id }, { "AssignmentType", (int)IssueTypeAssignmentEnum.Assign } },
            new DialogOptions() { Width = "30%", CloseDialogOnOverlayClick = false });
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


        #region Assign Bug Type to Module
        protected async Task AssignBugTypeToModule(BugIssueTypeListVM args)
        {
            try
            {
                await dialogService.OpenAsync<BugTypeModuleAssignment>(
            translationState.Translate("Properties_Configuration_Type"),
            new Dictionary<string, object>() { { "TypeId", args.Id }, { "Assignment", (int)IssueTypeAssignmentEnum.Assign } },
            new DialogOptions() { Width = "40%", CloseDialogOnOverlayClick = false });
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



        #region Add Issue Type Button
        protected async Task ButtonAddClick()
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<AddBugType>(
                                    translationState.Translate("Add_Issue_Type"),
                                    null,
                                    new DialogOptions() { Width = "30%", CloseDialogOnOverlayClick = false });
                await Task.Delay(100);
                await Load();
                StateHasChanged();
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

        #region Button click Events

        private void GoBackHomeScreen()
        {

            navigationManager.NavigateTo("/index");
        }

        #endregion

        #region OnSearch
        protected string search
        {
            get
            {
                return _search;
            }
            set
            {
                if (!Equals(_search, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;

                    Reload();
                }
            }
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredBugIssueListVM = await gridSearchExtension.Filter(GetBugIssues, new Query()
                    {
                        Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? $@"i => (i.Type_En != null && i.Type_En.ToString().ToLower().Contains(@0)) || (i.CreatedByEn != null && i.CreatedByEn.ToString().ToLower().Contains(@1)) || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").ToLower().Contains(@2))" : $@"i => (i.Type_Ar != null && i.Type_Ar.ToString().ToLower().Contains(@0)) || (i.CreatedByAr != null && i.CreatedByAr.ToString().ToLower().Contains(@1)) || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").ToLower().Contains(@2))",

                        FilterParameters = new object[] { search, search, search }
                    });  await InvokeAsync(StateHasChanged);
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
