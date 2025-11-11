using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Server.IISIntegration;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;

namespace FATWA_WEB.Pages.Tasks
{
    public partial class ListTask : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic? TaskStatus { get; set; }
        #endregion

        #region Variables Declarations
        private UserTask saveTask = new UserTask();
        protected List<UserTaskStatus> TaskStatuses = new List<UserTaskStatus>();

        protected RadzenDropDown<int?> ddlStatus;
        public bool StatusTrue { get; set; }
        public bool Statusfalse { get; set; }
        public int selectedIndex { get; set; } = 0;
        private int CurrentPage { get; set; }
        private int CurrentPageSize { get; set; }
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        protected string search { get; set; }
        protected string searchComs { get; set; }
        protected string searchCmsComs { get; set; }
        protected string searchCms { get; set; }
        protected RadzenDataGrid<TaskVM> grid = new RadzenDataGrid<TaskVM>();
        //protected RadzenDataGrid<TaskVM> gridModule = new RadzenDataGrid<TaskVM>();
        protected RadzenDataGrid<TaskVM> gridCms = new RadzenDataGrid<TaskVM>();
        protected RadzenDataGrid<TaskVM> gridComs = new RadzenDataGrid<TaskVM>();
        protected RadzenDataGrid<TaskVM> gridCMSComs = new RadzenDataGrid<TaskVM>();
        protected IEnumerable<TaskVM> getTaskList { get; set; } = new List<TaskVM>();
        protected IEnumerable<TaskVM> FilteredTaskList { get; set; }
        // protected IEnumerable<TaskVM> moduleTaskList { get; set; }
        protected IEnumerable<TaskVM> cmsTaskList { get; set; } = new List<TaskVM>();
        protected IEnumerable<TaskVM> cmsTaskList1 { get; set; }
        protected IEnumerable<TaskVM> cmsFilteredTaskList { get; set; }
        protected IEnumerable<TaskVM> comsTaskList { get; set; } = new List<TaskVM>();
        protected IEnumerable<TaskVM> comsFilteredTaskList { get; set; }
        protected IEnumerable<TaskVM> CMScomsTaskList { get; set; } = new List<TaskVM>();
        protected IEnumerable<TaskVM> filteredCmsComsTaskList { get; set; }
        protected List<GovernmentEntity> GovermentEntities { get; set; } = new List<GovernmentEntity>();

        public bool isVisible { get; set; }
        protected AdvanceSearchTaskVM advanceSearchVM = new AdvanceSearchTaskVM();
        public TaskDashboardVM dashboardVM = new TaskDashboardVM();
        protected bool Keywords = false;
        protected bool isAdvanceSearchForTask { get; set; }
        private Timer debouncer;
        private const int debouncerDelay = 500;

        #endregion

        #region Task Status
        protected async Task PopulateTaskStatuses()
        {
            var response = await lookupService.GetTaskStatuses();

            if (response.IsSuccessStatusCode)
            {
                TaskStatuses = (List<UserTaskStatus>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region On Initialized

        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                await PopulateTaskStatuses();

                if (TaskStatus != null)
                {
                    if (int.Parse(TaskStatus) == (int)TaskStatusEnum.Done)
                    {
                        selectedIndex = 1;
                        advanceSearchVM.TaskStatusId = (int)TaskStatusEnum.Done;
                    }
                    else
                    {
                        selectedIndex = 0;
                        advanceSearchVM.TaskStatusId = (int)TaskStatusEnum.Pending;
                    }
                }
                else
                {
                    selectedIndex = 0;
                    advanceSearchVM.TaskStatusId = (int)TaskStatusEnum.Pending;
                }

                await LoadStats();
                await PopulateGovernmentEntities();
                translationState.TranslateGridFilterLabels(grid);
                translationState.TranslateGridFilterLabels(gridCms);
                translationState.TranslateGridFilterLabels(gridComs);
                translationState.TranslateGridFilterLabels(gridCMSComs);
                spinnerService.Hide();
            }
            catch (Exception ex)
            {
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        protected async Task LoadStats()
        {
            try
            {
                var result = await taskService.GetTaskDashBoard();
                if (result.IsSuccessStatusCode)
                {
                    dashboardVM = (TaskDashboardVM)result.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(result);
                }
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

        protected async Task LoadTasks(LoadDataArgs dataArgs)
        {
            try
            {
                CurrentPage = grid.CurrentPage + 1;
                CurrentPageSize = grid.PageSize;
                if (string.IsNullOrEmpty(dataArgs.OrderBy) || CurrentPage != advanceSearchVM.PageNumber || CurrentPageSize != advanceSearchVM.PageSize || (Keywords && advanceSearchVM.isDataSorted))
                {
                    if (advanceSearchVM.isGridLoaded && advanceSearchVM.PageSize == CurrentPageSize && !advanceSearchVM.isPageSizeChangeOnFirstLastPage)
                    {
                        grid.CurrentPage = (int)advanceSearchVM.PageNumber - 1;
                        advanceSearchVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(dataArgs, getTaskList.Any() ? (getTaskList.First().TotalCount) : 0, grid);
                    spinnerService.Show();
                    advanceSearchVM.SelectedIndex = selectedIndex;
                    if (!isAdvanceSearchForTask)
                    {
                        advanceSearchVM.FromDate = null;
                        advanceSearchVM.ToDate = null;
                        advanceSearchVM.FromHearingDate = null;
                        advanceSearchVM.ToHearingDate = null;
                        advanceSearchVM.GovermentEntityId = null;
                        advanceSearchVM.ReferenceNumber = string.Empty;
                        advanceSearchVM.IsImpportant = false;
                    }
                    var result = await taskService.GetTasksList(advanceSearchVM);
                    if (result.IsSuccessStatusCode)
                    {
                        getTaskList = JsonConvert.DeserializeObject<IEnumerable<TaskVM>>(result.ResultData.ToString());
                        FilteredTaskList = JsonConvert.DeserializeObject<IEnumerable<TaskVM>>(result.ResultData.ToString());
                        if (!(string.IsNullOrEmpty(search))) await OnTasksSearchInput(search);
                        if (!(string.IsNullOrEmpty(dataArgs.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            FilteredTaskList = await gridSearchExtension.Sort(FilteredTaskList, ColumnName, SortOrder);
                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(result);
                    }
                    StateHasChanged();
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
            spinnerService.Hide();
        }
        protected async Task OnTasksSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredTaskList = await gridSearchExtension.Filter(getTaskList, new Query()
                    {
                        Filter = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? $@"i => ( (i.Name != null && i.Name.ToLower().Contains(@0)) 
                                                                                            || (i.Description != null && i.Description.ToLower().Contains(@0))
                                                                                            || (i.AssignedByEn != null && i.AssignedByEn.ToLower().Contains(@0)) 
                                                                                            || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@0))
                                                                                            || (i.ReferenceNumber != null && i.ReferenceNumber.ToLower().Contains(@0))
                                                                                            || (i.TaskNo != null && i.TaskNo.ToString().ToLower().Contains(@0))
                                                                                            || (i.StatusNameEn != null && i.StatusNameEn.ToLower().Contains(@0))
                                                                                            || (i.Subject != null && i.Subject.ToLower().Contains(@0)) )" :
                                                                                         $@"i => ( (i.Name != null && i.Name.ToLower().Contains(@0))
                                                                                            || (i.Description != null && i.Description.ToLower().Contains(@0))  
                                                                                            || (i.AssignedByAr != null && i.AssignedByAr.ToLower().Contains(@0))
                                                                                            || (i.ReferenceNumber != null && i.ReferenceNumber.ToLower().Contains(@0))
                                                                                            || (i.StatusNameAr != null && i.StatusNameAr.ToLower().Contains(@0)) 
                                                                                            c
                                                                                            || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@0))
                                                                                            || (i.Subject != null && i.Subject.ToLower().Contains(@0)) )",
                        FilterParameters = new object[] { search }
                    });
                    if (!string.IsNullOrEmpty(ColumnName))
                        FilteredTaskList = await gridSearchExtension.Sort(FilteredTaskList, ColumnName, SortOrder);
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

        protected async Task LoadCmsTasks(LoadDataArgs dataArgs)
        {
            try
            {
                CurrentPage = gridCms.CurrentPage + 1;
                CurrentPageSize = gridCms.PageSize;
                if (string.IsNullOrEmpty(dataArgs.OrderBy) || CurrentPage != advanceSearchVM.PageNumber || CurrentPageSize != advanceSearchVM.PageSize || (Keywords && advanceSearchVM.isDataSorted))
                {
                    if (advanceSearchVM.isGridLoaded && advanceSearchVM.PageSize == CurrentPageSize && !advanceSearchVM.isPageSizeChangeOnFirstLastPage)
                    {
                        gridCms.CurrentPage = (int)advanceSearchVM.PageNumber - 1;
                        advanceSearchVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(dataArgs, cmsTaskList.Any() ? (cmsTaskList.First().TotalCount) : 0, gridCms);
                    spinnerService.Show();
                    if (TaskStatus != null)
                    {
                        advanceSearchVM.TaskStatusId = int.Parse(TaskStatus);
                        Keywords = true;
                    }
                    else
                    {
                        if (selectedIndex == 0)
                        {
                            advanceSearchVM.TaskStatusId = (int)TaskStatusEnum.Pending;
                        }
                        else
                        {
                            advanceSearchVM.TaskStatusId = (int)TaskStatusEnum.Done;
                        }
                    }
                    if (isAdvanceSearchForTask)
                    {
                        advanceSearchVM.FromDate = null;
                        advanceSearchVM.ToDate = null;
                        advanceSearchVM.FromHearingDate = null;
                        advanceSearchVM.ToHearingDate = null;
                        advanceSearchVM.GovermentEntityId = null;
                        advanceSearchVM.ReferenceNumber = string.Empty;
                        advanceSearchVM.IsImpportant = false;
                    }
                    var result = await taskService.GetAllCmsTasks(advanceSearchVM);
                    if (result.IsSuccessStatusCode)
                    {
                        cmsTaskList = JsonConvert.DeserializeObject<IEnumerable<TaskVM>>(result.ResultData.ToString());
                        cmsFilteredTaskList = JsonConvert.DeserializeObject<IEnumerable<TaskVM>>(result.ResultData.ToString());
                        if (!(string.IsNullOrEmpty(searchCms))) await OnCmsTasksSearchInput(searchCms);
                        if (!(string.IsNullOrEmpty(dataArgs.OrderBy)) && (string.IsNullOrEmpty(searchCms)))
                        {
                            cmsFilteredTaskList = await gridSearchExtension.Sort(cmsFilteredTaskList, ColumnName, SortOrder);
                        }
                        await InvokeAsync(StateHasChanged);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(result);
                    }
                    StateHasChanged();
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
            spinnerService.Hide();
        }
        protected async Task OnCmsTasksSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    searchCms = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    cmsFilteredTaskList = await gridSearchExtension.Filter(cmsTaskList, new Query()
                    {
                        Filter = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? $@"i => ( (i.Name != null && i.Name.ToLower().Contains(@0)) 
                                                                                            || (i.Description != null && i.Description.ToLower().Contains(@0)) 
                                                                                            || (i.AssignedByEn != null && i.AssignedByEn.ToLower().Contains(@0))
                                                                                            || (i.ReferenceNumber != null && i.ReferenceNumber.ToLower().Contains(@0)) 
                                                                                            || (i.GovtEntityNameEn != null && i.GovtEntityNameEn.ToLower().Contains(@0)) 
                                                                                            || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@0))
                                                                                            || (i.StatusNameEn != null && i.StatusNameEn.ToLower().Contains(@0)) 
                                                                                            || (i.RequestGovtEntityNameEn != null && i.RequestGovtEntityNameEn.ToLower().Contains(@0))
                                                                                            || (i.Subject != null && i.Subject.ToLower().Contains(@0)) )" :
                                                                                                                    $@"i => ( (i.Name != null && i.Name.ToLower().Contains(@0))
                                                                                            || (i.Description != null && i.Description.ToLower().Contains(@0))
                                                                                            || (i.AssignedByAr != null && i.AssignedByAr.ToLower().Contains(@0))
                                                                                            || (i.ReferenceNumber != null && i.ReferenceNumber.ToLower().Contains(@0))
                                                                                            || (i.StatusNameAr != null && i.StatusNameAr.ToLower().Contains(@0)) 
                                                                                            || (i.RequestGovtEntityNameAr != null && i.RequestGovtEntityNameAr.ToLower().Contains(@0))
                                                                                            || (i.GovtEntityNameAr != null && i.GovtEntityNameAr.ToLower().Contains(@0)) 
                                                                                            || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@0))
                                                                                            ||(i.Subject != null && i.Subject.ToLower().Contains(@0)))",
                        FilterParameters = new object[] { searchCms }
                    });
                    if (!string.IsNullOrEmpty(ColumnName))
                        cmsFilteredTaskList = await gridSearchExtension.Sort(cmsFilteredTaskList, ColumnName, SortOrder);
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
        protected async Task LoadComsTasks(LoadDataArgs dataArgs)
        {
            try
            {
                CurrentPage = gridComs.CurrentPage + 1;
                CurrentPageSize = gridComs.PageSize;
                if (string.IsNullOrEmpty(dataArgs.OrderBy) || CurrentPage != advanceSearchVM.PageNumber || CurrentPageSize != advanceSearchVM.PageSize || (Keywords && advanceSearchVM.isDataSorted))
                {
                    if (advanceSearchVM.isGridLoaded && advanceSearchVM.PageSize == CurrentPageSize && !advanceSearchVM.isPageSizeChangeOnFirstLastPage)
                    {
                        gridComs.CurrentPage = (int)advanceSearchVM.PageNumber - 1;
                        advanceSearchVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(dataArgs, comsTaskList.Any() ? (comsTaskList.First().TotalCount) : 0, gridComs);
                    spinnerService.Show();

                    if (TaskStatus != null)
                    {
                        advanceSearchVM.TaskStatusId = int.Parse(TaskStatus);
                        Keywords = true;
                    }
                    else
                    {
                        if (selectedIndex == 0)
                        {
                            advanceSearchVM.TaskStatusId = (int)TaskStatusEnum.Pending;
                        }
                        else
                        {
                            advanceSearchVM.TaskStatusId = (int)TaskStatusEnum.Done;
                        }
                    }
                    if (isAdvanceSearchForTask)
                    {
                        advanceSearchVM.FromDate = null;
                        advanceSearchVM.ToDate = null;
                        advanceSearchVM.FromHearingDate = null;
                        advanceSearchVM.ToHearingDate = null;
                        advanceSearchVM.GovermentEntityId = null;
                        advanceSearchVM.ReferenceNumber = string.Empty;
                        advanceSearchVM.IsImpportant = false;
                    }
                    var result = await taskService.GetAllComsTasks(advanceSearchVM);
                    if (result.IsSuccessStatusCode)
                    {
                        comsTaskList = JsonConvert.DeserializeObject<IEnumerable<TaskVM>>(result.ResultData.ToString());
                        comsFilteredTaskList = JsonConvert.DeserializeObject<IEnumerable<TaskVM>>(result.ResultData.ToString());
                        if (!(string.IsNullOrEmpty(searchComs))) await OnComsTasksSearchInput(searchComs);
                        if (!(string.IsNullOrEmpty(dataArgs.OrderBy)) && (string.IsNullOrEmpty(searchComs)))
                        {
                            comsFilteredTaskList = await gridSearchExtension.Sort(comsFilteredTaskList, ColumnName, SortOrder);
                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(result);
                    }
                    StateHasChanged();
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
            spinnerService.Hide();
        }
        protected async Task OnComsTasksSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    searchComs = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    comsFilteredTaskList = await gridSearchExtension.Filter(comsTaskList, new Query()
                    {
                        Filter = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? $@"i => ( (i.Name != null && i.Name.ToLower().Contains(@0)) 
                                                    || (i.Description != null && i.Description.ToLower().Contains(@0)) 
                                                    || (i.AssignedByEn != null && i.AssignedByEn.ToLower().Contains(@0))
                                                    || (i.ReferenceNumber != null && i.ReferenceNumber.ToLower().Contains(@0)) 
                                                    || (i.StatusNameEn != null && i.StatusNameEn.ToLower().Contains(@0))
                                                    || (i.GovtEntityNameEn != null && i.GovtEntityNameEn.ToLower().Contains(@0)) 
                                                    || (i.RequestGovtEntityNameEn != null && i.RequestGovtEntityNameEn.ToLower().Contains(@0)) 
                                                    || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@0))
                                                    || (i.Subject != null && i.Subject.ToLower().Contains(@0)) )" :
                                                                            $@"i => ( (i.Name != null && i.Name.ToLower().Contains(@0)) 
                                                    || (i.Description != null && i.Description.ToLower().Contains(@0))
                                                    || (i.AssignedByAr != null && i.AssignedByAr.ToLower().Contains(@0))
                                                    || (i.ReferenceNumber != null && i.ReferenceNumber.ToLower().Contains(@0))
                                                    || (i.GovtEntityNameAr != null && i.GovtEntityNameAr.ToLower().Contains(@0)) 
                                                    || (i.RequestGovtEntityNameAr != null && i.RequestGovtEntityNameAr.ToLower().Contains(@0)) 
                                                    || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@0))
                                                    || (i.StatusNameAr != null && i.StatusNameAr.ToLower().Contains(@0))
                                                    || (i.Subject != null && i.Subject.ToLower().Contains(@0)) )",
                        FilterParameters = new object[] { searchComs }
                    });
                    if (!string.IsNullOrEmpty(ColumnName))
                        comsFilteredTaskList = await gridSearchExtension.Sort(comsFilteredTaskList, ColumnName, SortOrder);
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
        protected async Task LoadCMSCOMSTasks(LoadDataArgs dataArgs)
        {
            try
            {
                CurrentPage = gridCMSComs.CurrentPage + 1;
                CurrentPageSize = gridCMSComs.PageSize;
                if (string.IsNullOrEmpty(dataArgs.OrderBy) || CurrentPage != advanceSearchVM.PageNumber || CurrentPageSize != advanceSearchVM.PageSize || (Keywords && advanceSearchVM.isDataSorted))
                {
                    if (advanceSearchVM.isGridLoaded && advanceSearchVM.PageSize == CurrentPageSize && !advanceSearchVM.isPageSizeChangeOnFirstLastPage)
                    {
                        gridCMSComs.CurrentPage = (int)advanceSearchVM.PageNumber - 1;
                        advanceSearchVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(dataArgs, CMScomsTaskList.Any() ? (CMScomsTaskList.First().TotalCount) : 0, gridCMSComs);
                    spinnerService.Show();
                    if (TaskStatus != null)
                    {
                        advanceSearchVM.TaskStatusId = int.Parse(TaskStatus);
                        Keywords = true;
                    }
                    else
                    {
                        if (selectedIndex == 0)
                        {
                            advanceSearchVM.TaskStatusId = (int)TaskStatusEnum.Pending;
                        }
                        else
                        {
                            advanceSearchVM.TaskStatusId = (int)TaskStatusEnum.Done;
                        }
                    }
                    if (isAdvanceSearchForTask)
                    {
                        advanceSearchVM.FromDate = null;
                        advanceSearchVM.ToDate = null;
                        advanceSearchVM.FromHearingDate = null;
                        advanceSearchVM.ToHearingDate = null;
                        advanceSearchVM.GovermentEntityId = null;
                        advanceSearchVM.ReferenceNumber = string.Empty;
                        advanceSearchVM.IsImpportant = false;
                    }
                    var result = await taskService.GetAllCMSComsTasks(advanceSearchVM);
                    if (result.IsSuccessStatusCode)
                    {
                        CMScomsTaskList = JsonConvert.DeserializeObject<IEnumerable<TaskVM>>(result.ResultData.ToString());
                        filteredCmsComsTaskList = JsonConvert.DeserializeObject<IEnumerable<TaskVM>>(result.ResultData.ToString());
                        if (!(string.IsNullOrEmpty(searchCmsComs))) await OnCmsComsTasksSearchInput(searchCmsComs);
                        if (!(string.IsNullOrEmpty(dataArgs.OrderBy)) && (string.IsNullOrEmpty(searchCmsComs)))
                        {
                            filteredCmsComsTaskList = await gridSearchExtension.Sort(filteredCmsComsTaskList, ColumnName, SortOrder);
                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(result);
                    }
                    StateHasChanged();
                }
                spinnerService.Hide();
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
            spinnerService.Hide();
        }

        protected async Task OnCmsComsTasksSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    searchCmsComs = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    filteredCmsComsTaskList = await gridSearchExtension.Filter(comsTaskList, new Query()
                    {
                        Filter = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? $@"i => ( (i.Name != null && i.Name.ToLower().Contains(@0))
                                                || (i.Description != null && i.Description.ToLower().Contains(@0)) 
                                                || (i.AssignedByEn != null && i.AssignedByEn.ToLower().Contains(@0)) 
                                                || (i.ReferenceNumber != null && i.ReferenceNumber.ToLower().Contains(@0))
                                                || (i.StatusNameEn != null && i.StatusNameEn.ToLower().Contains(@0)) 
                                                || (i.GovtEntityNameEn != null && i.GovtEntityNameEn.ToLower().Contains(@0)) 
                                                || (i.RequestGovtEntityNameEn != null && i.RequestGovtEntityNameEn.ToLower().Contains(@0)) 
                                                || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@0))
                                                || (i.Subject != null && i.Subject.ToLower().Contains(@0)))" :
                                                                        $@"i => ( (i.Name != null && i.Name.ToLower().Contains(@0)) 
                                                || (i.Description != null && i.Description.ToLower().Contains(@0)) 
                                                || (i.AssignedByAr != null && i.AssignedByAr.ToLower().Contains(@0))
                                                || (i.ReferenceNumber != null && i.ReferenceNumber.ToLower().Contains(@0))
                                                || (i.GovtEntityNameAr != null && i.GovtEntityNameAr.ToLower().Contains(@0)) 
                                                || (i.RequestGovtEntityNameAr != null && i.RequestGovtEntityNameAr.ToLower().Contains(@0)) 
                                                || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@0))
                                                || (i.StatusNameAr != null && i.StatusNameAr.ToLower().Contains(@0))
                                                || (i.Subject != null && i.Subject.ToLower().Contains(@0)) )",
                        FilterParameters = new object[] { searchCmsComs }
                    });
                    if (!string.IsNullOrEmpty(ColumnName))
                        filteredCmsComsTaskList = await gridSearchExtension.Sort(filteredCmsComsTaskList, ColumnName, SortOrder);
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
        public async Task OnTabChange(int index)
        {
            try
            {
                if (index == selectedIndex) { return; }
                spinnerService.Show();
                selectedIndex = index;
                TaskStatus = null;
                search = searchComs = searchCmsComs = searchCms = ColumnName = string.Empty;
                advanceSearchVM = new AdvanceSearchTaskVM { PageSize = grid.PageSize };
                isVisible = Keywords = false;
                await Task.Delay(100);
                if (index == 0)
                {
                    advanceSearchVM.TaskStatusId = (int)TaskStatusEnum.Pending;
                    grid.Reset();
                    await grid.Reload();
                    spinnerService.Show();
                    if (loginState.UserDetail.SectorTypeId != null && loginState.UserDetail.SectorTypeId > 0 && (loginState.UserDetail.SectorTypeId <= (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases
                                 || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.Execution || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.PublicOperationalSector
                                 || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeGeneralSupervisor || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialGeneralSupervisor))
                    {
                        gridCms.Reset();
                        await gridCms.Reload();
                    }
                    if (loginState.UserDetail.SectorTypeId != null && loginState.UserDetail.SectorTypeId > 0 && (loginState.UserDetail.SectorTypeId > (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases && loginState.UserDetail.SectorTypeId <= (int)OperatingSectorTypeEnum.InternationalArbitration
                               || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.PublicOperationalSector))
                    {
                        gridComs.Reset();
                        await gridComs.Reload();
                    }
                    if (loginState.UserDetail.SectorTypeId != null && loginState.UserDetail.SectorTypeId > 0 && (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.PrivateOperationalSector || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.FatwaPresidentOffice))
                    {
                        gridCMSComs.Reset();
                        await gridCMSComs.Reload();
                    }
                }
                else
                {
                    advanceSearchVM.TaskStatusId = (int)TaskStatusEnum.Done;
                    grid.Reset();
                    await grid.Reload();
                    spinnerService.Show();
                    if (loginState.UserDetail.SectorTypeId != null && loginState.UserDetail.SectorTypeId > 0 && (loginState.UserDetail.SectorTypeId <= (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases
                               || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.Execution || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.PublicOperationalSector
                               || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeGeneralSupervisor || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialGeneralSupervisor))
                    {
                        gridCms.Reset();
                        await gridCms.Reload();
                    }
                    if (loginState.UserDetail.SectorTypeId != null && loginState.UserDetail.SectorTypeId > 0 && (loginState.UserDetail.SectorTypeId > (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases && loginState.UserDetail.SectorTypeId <= (int)OperatingSectorTypeEnum.InternationalArbitration
                               || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.PublicOperationalSector))
                    {
                        gridComs.Reset();
                        await gridComs.Reload();
                    }
                    if (loginState.UserDetail.SectorTypeId != null && loginState.UserDetail.SectorTypeId > 0 && (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.PrivateOperationalSector || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.FatwaPresidentOffice))
                    {
                        gridCMSComs.Reset();
                        await gridCMSComs.Reload();
                    }
                }
                spinnerService.Hide();
            }
            catch (Exception ex)
            {
                spinnerService.Hide();
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
        private void SetPagingProperties(LoadDataArgs args, int ListTotalCount, RadzenDataGrid<TaskVM> grid)
        {
            if (advanceSearchVM.PageSize != null && advanceSearchVM.PageSize != CurrentPageSize)
            {
                int oldPageCount = ListTotalCount > 0 ? (ListTotalCount) / ((int)advanceSearchVM.PageSize) : 1;
                int oldPageNumber = (int)advanceSearchVM.PageNumber - 1;
                advanceSearchVM.isGridLoaded = true;
                advanceSearchVM.PageNumber = CurrentPage;
                advanceSearchVM.PageSize = args.Top;
                int TotalPages = ListTotalCount > 0 ? (ListTotalCount) / (grid.PageSize) : 1;
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

        #region Functions

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        protected async Task AddTask(MouseEventArgs args)
        {
            try
            {
                navigationManager.NavigateTo("/usertask-add/" + "");
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

        protected async Task EditTask(TaskVM item)
        {
            try
            {
                navigationManager.NavigateTo("/usertask-add/" + item.TaskId);
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

        protected void DetailTask(TaskVM task)
        {
            navigationManager.NavigateTo("/usertask-detail/" + task.TaskId);
        }

        protected void TaskResponse(TaskVM task)
        {
            navigationManager.NavigateTo("/taskresponse-decision/" + task.TaskId);
        }

        protected async void DetailTaskDetails(TaskVM task)
        {
            //To remove Web url from URL
            navigationState.ReturnUrl = "usertask-list";
            if (task.Url.StartsWith("caserequest-view") || task.Url.StartsWith("caserequest-transfer-review") || task.Url.StartsWith("caserequest-copy-review") || task.Url.StartsWith("casefile-view") || task.Url.StartsWith("casefile-copy-review") || task.Url.StartsWith("casefile-transfer-review") ||
             task.Url.StartsWith("mergerequest-view") || task.Url.StartsWith("draftdocument-detail") || task.Url.StartsWith("executionrequest-detail") || task.Url.StartsWith("document-view") || task.Url.StartsWith("request-need-more-detail") || task.Url.StartsWith("casefile-review-assignment")
             || task.Url.StartsWith("casefile-view") || task.Url.StartsWith("detail-withdraw-request") || task.Url.StartsWith("meeting-decision") || task.Url.StartsWith("request-meeting-detail") || task.Url.StartsWith("consultationrequest-detail") || task.Url.StartsWith("consultationfile-view") ||
             task.Url.StartsWith("case-view") || task.Url.StartsWith("transferrequest-view") || task.Url.StartsWith("casetransferrequest-view"))
            {
                await AddOrUpdateUserTaskViewTime(task.ReferenceId);
                navigationManager.NavigateTo(navigationManager.BaseUri + task.Url + "/" + task.TaskId);
            }
            else if (task.Url.StartsWith("consultationrequest-transfer-review") || task.Url.StartsWith("consultationfile-review-assignment") || task.Url.StartsWith("consultationfile-transfer-review") || task.Url.StartsWith("draftdocument-detail/") || task.Url.StartsWith("detail-withdraw-consultationrequest") || task.Url.StartsWith("meeting-attendeedecision") || task.Url.StartsWith("mom-attendeedecision") || task.Url.StartsWith("requestformeeting-attendeedecision"))
            {
                await AddOrUpdateUserTaskViewTime(task.ReferenceId);
                navigationManager.NavigateTo(navigationManager.BaseUri + task.Url + "/" + task.TaskId);

            }
            else if (task.Url.StartsWith("review-documentforsigning"))
            {
                navigationManager.NavigateTo(navigationManager.BaseUri + task.Url + "/" + task.TaskId);
            }
            else
            {
                await AddOrUpdateUserTaskViewTime(task.ReferenceId);
                navigationManager.NavigateTo(navigationManager.BaseUri + task.Url);

            }
        }
        public async Task AddOrUpdateUserTaskViewTime(Guid? ReferenceId)
        {

            var response = await taskService.AddOrUpdateUserTaskViewTime(loginState.UserDetail.UserId, ReferenceId);
            if (!response.IsSuccessStatusCode)
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();

        }
        protected async Task ViewItemHistory(TaskVM task)
        {
            var labelReferenceTranslation = translationState.Translate(task.SubModuleId == (int)SubModuleEnum.CaseRequest ? "Request_Number" :
                                                                          task.SubModuleId == (int)SubModuleEnum.CaseFile ? "File_Number" :
                                                                          task.SubModuleId == (int)SubModuleEnum.RegisteredCase ? "Case_Number" :
                                                                          "");
            string translationKey;
            string canNumberTranslation = translationState.Translate("CAN");
            switch (task.SubModuleId)
            {
                case (int)SubModuleEnum.CaseRequest:
                    translationKey = $"{translationState.Translate("Case_Request_History")}({labelReferenceTranslation} {task.ReferenceNumber})";
                    break;
                case (int)SubModuleEnum.CaseFile:
                    translationKey = $"{translationState.Translate("Case_File_History")}({labelReferenceTranslation} {task.ReferenceNumber})";
                    break;
                case (int)SubModuleEnum.RegisteredCase:
                    translationKey = $"{translationState.Translate("Registered_Case_History")} ({labelReferenceTranslation} {task.ReferenceNumber}, {canNumberTranslation} {task.CANNumber})";
                    break;
                case (int)SubModuleEnum.ConsultationFile:
                    translationKey = $"{translationState.Translate("Coms_File_History")}";
                    break;
                case (int)SubModuleEnum.ConsultationRequest:
                    translationKey = $"{translationState.Translate("Coms_Request_History")}";
                    break;
                default:
                    translationKey = translationState.Translate("Item_History");
                    break;
            }

            var dialogResult = await dialogService.OpenAsync<ItemHistoryPopup>
            (
                translationKey,
                new Dictionary<string, object>()
                {
            { "Task", task }
                },
                new DialogOptions() { Width = "80% !important", CloseDialogOnOverlayClick = true, ShowClose = true }
            );
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

        #region Advance Search

        //<History Author = 'Zain Ul Islam' Date='2022-11-30' Version="1.0" Branch="master">Open advance search popup </History>
        protected void ToggleAdvanceSearch(bool isAdvanceSearchForTask)
        {
            this.isAdvanceSearchForTask = isAdvanceSearchForTask;
            isVisible = !isVisible;
            if (!isVisible)
            {
                ResetForm();
            }
        }

        //<History Author = 'Zain Ul Islam' Date='2022-11-30' Version="1.0" Branch="master">Validate the Advance Search form</History> 
        protected async Task SubmitAdvanceSearch()
        {

            if (advanceSearchVM.TaskStatusId == null)
            {
                navigationManager.NavigateTo("/usertask-list");
            }

            if (advanceSearchVM.FromDate > advanceSearchVM.ToDate)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return;
            }
            if (!advanceSearchVM.FromDate.HasValue && !advanceSearchVM.ToDate.HasValue
                && !advanceSearchVM.FromHearingDate.HasValue && !advanceSearchVM.ToHearingDate.HasValue
                && !advanceSearchVM.GovermentEntityId.HasValue && string.IsNullOrEmpty(advanceSearchVM.ReferenceNumber))
            {

            }
            else
            {
                Keywords = advanceSearchVM.isDataSorted = true;
                if (isAdvanceSearchForTask)
                {
                    if (grid.CurrentPage > 0)
                        await grid.FirstPage();
                    else
                    {
                        advanceSearchVM.isGridLoaded = false;
                        await grid.Reload();
                        return;
                    }
                }
                if (loginState.UserDetail.SectorTypeId > 0 && (loginState.UserDetail.SectorTypeId <= (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases
                               || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.Execution || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.PublicOperationalSector
                               || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeGeneralSupervisor || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialGeneralSupervisor))
                {
                    if (gridCms.CurrentPage > 0)
                        await gridCms.FirstPage();
                    else
                    {
                        advanceSearchVM.isGridLoaded = false;
                        await gridCms.Reload();
                    }
                }
                if (loginState.UserDetail.SectorTypeId != null && loginState.UserDetail.SectorTypeId > 0 && (loginState.UserDetail.SectorTypeId > (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases && loginState.UserDetail.SectorTypeId <= (int)OperatingSectorTypeEnum.InternationalArbitration
                              || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.PublicOperationalSector))
                {
                    if (gridComs.CurrentPage > 0)
                        await gridComs.FirstPage();
                    else
                    {
                        advanceSearchVM.isGridLoaded = false;
                        await gridComs.Reload();
                    }
                }
                if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.PrivateOperationalSector || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.FatwaPresidentOffice)
                {
                    if (gridCMSComs.CurrentPage > 0)
                        await gridCMSComs.FirstPage();
                    else
                    {
                        advanceSearchVM.isGridLoaded = false;
                        await gridCMSComs.Reload();
                    }
                }
            }

            StateHasChanged();
        }

        public async void ResetForm()
        {
            advanceSearchVM = new AdvanceSearchTaskVM { PageSize = grid.PageSize, TaskStatusId = selectedIndex == 0 ? (int)TaskStatusEnum.Pending : (int)TaskStatusEnum.Done };
            Keywords = advanceSearchVM.isDataSorted = isVisible = false;
            grid.Reset();
            await grid.Reload();
            if (loginState.UserDetail.SectorTypeId > 0 && (loginState.UserDetail.SectorTypeId <= (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases
                               || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.Execution || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.PublicOperationalSector
                               || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeGeneralSupervisor || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialGeneralSupervisor))
            {
                gridCms.Reset();
                await gridCms.Reload();
            }
            if (loginState.UserDetail.SectorTypeId != null && loginState.UserDetail.SectorTypeId > 0 && (loginState.UserDetail.SectorTypeId > (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases && loginState.UserDetail.SectorTypeId <= (int)OperatingSectorTypeEnum.InternationalArbitration
                               || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.PublicOperationalSector))
            {
                gridComs.Reset();
                await gridComs.Reload();
            }
            if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.PrivateOperationalSector || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.FatwaPresidentOffice)
            {
                gridCMSComs.Reset();
                await gridCMSComs.Reload();
            }
            StateHasChanged();
        }

        #endregion

        #region On Sort Grid Data
        private async Task OnSortDataCmsTasks(DataGridColumnSortEventArgs<TaskVM> args)
        {
            if (args.SortOrder != null)
            {
                cmsFilteredTaskList = await gridSearchExtension.Sort(cmsFilteredTaskList, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                advanceSearchVM.isDataSorted = false;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        private async Task OnSortDataComsTasks(DataGridColumnSortEventArgs<TaskVM> args)
        {
            if (args.SortOrder != null)
            {
                comsFilteredTaskList = await gridSearchExtension.Sort(comsFilteredTaskList, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                advanceSearchVM.isDataSorted = false;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        private async Task OnSortDataCmsComsTasks(DataGridColumnSortEventArgs<TaskVM> args)
        {
            if (args.SortOrder != null)
            {
                filteredCmsComsTaskList = await gridSearchExtension.Sort(filteredCmsComsTaskList, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                advanceSearchVM.isDataSorted = false;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        private async Task OnSortTasks(DataGridColumnSortEventArgs<TaskVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredTaskList = await gridSearchExtension.Sort(FilteredTaskList, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                advanceSearchVM.isDataSorted = false;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        #endregion

        #region Grid Funtions

        protected void RowRenderHandler(RowRenderEventArgs<TaskVM> task)
        {
            if (task.Data.IsImportant == true)
            {
                task.Attributes.Add("style", $"background-color: #e3d4c9;");

            }
        }

        protected async Task PopulateGovernmentEntities()
        {
            var response = await lookupService.GetGovernmentEntities();
            if (response.IsSuccessStatusCode)
            {
                GovermentEntities = (List<GovernmentEntity>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion
    }
}
