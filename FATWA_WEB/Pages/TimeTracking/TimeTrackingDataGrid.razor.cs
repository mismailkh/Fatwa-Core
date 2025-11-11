using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.ViewModel.TimeTrackingVM;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using System.Globalization;
namespace FATWA_WEB.Pages.TimeTracking
{
    public partial class TimeTrackingDataGrid : ComponentBase
    {
        #region Parameters
        [Parameter]
        public IEnumerable<TimeTrackingVM> filterRefGetTimeTracking { get; set; }
        [Parameter]
        public int? SubModuleId { get; set; }
        [Parameter]
        public Guid? ReferenceId { get; set; }
        #endregion

        #region Variables Declaration
        protected RadzenDataGrid<TimeTrackingVM>? grid = new RadzenDataGrid<TimeTrackingVM>();
        protected TimeTrackingAdvanceSearchVM advanceSearchTimeTrackingVM = new TimeTrackingAdvanceSearchVM();
        protected IEnumerable<TimeTrackingVM> getTimeTrackingList;
        public string roleId;
        protected string? search { get; set; }
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            translationState.TranslateGridFilterLabels(grid);
            advanceSearchTimeTrackingVM.SectortypeId = (int)loginState.UserDetail.SectorTypeId;
            roleId = loginState.UserDetail.RoleId;
            advanceSearchTimeTrackingVM.ReferenceId = ReferenceId ?? Guid.Empty;
            advanceSearchTimeTrackingVM.ModuleId = loginState.ModuleId;
            if (SystemRoles.HOS == roleId || SystemRoles.ComsHOS == roleId || SystemRoles.ViceHOS == roleId)
            {
                advanceSearchTimeTrackingVM.UserId = "";
                advanceSearchTimeTrackingVM.UserName = loginState.UserDetail.UserName;
            }
            else
            {
                advanceSearchTimeTrackingVM.UserId = loginState.UserDetail.UserId;
            }
            spinnerService.Hide();
        }
        #endregion

        #region On Load Grid Data 
        protected async Task OnLoadData(LoadDataArgs dataArgs)
        {
            if (string.IsNullOrEmpty(dataArgs.OrderBy) || CurrentPage != advanceSearchTimeTrackingVM.PageNumber)
            {
                try
                {
                    spinnerService.Show();
                    List<TimeTrackingVM> timeTrackingVMs = new List<TimeTrackingVM>();
                    advanceSearchTimeTrackingVM.PageSize = dataArgs.Top;
                    advanceSearchTimeTrackingVM.PageNumber = (dataArgs.Skip / dataArgs.Top) + 1;
                    var result = await timeTrackingService.GetTimeTrackingList(advanceSearchTimeTrackingVM);
                    spinnerService.Hide();
                    if (result.IsSuccessStatusCode)
                    {
                        getTimeTrackingList = (IEnumerable<TimeTrackingVM>)result.ResultData;
                        foreach (var item in getTimeTrackingList)
                        {
                            item.ActivityName = translationState.Translate(item.ActivityName);
                            timeTrackingVMs.Add(item);
                        }
                        getTimeTrackingList = timeTrackingVMs;
                        filterRefGetTimeTracking = (IEnumerable<TimeTrackingVM>)result.ResultData;
                        if (!(string.IsNullOrEmpty(dataArgs.OrderBy)))
                        {
                            filterRefGetTimeTracking = await gridSearchExtension.Sort(filterRefGetTimeTracking, ColumnName, SortOrder);
                        }
                        await InvokeAsync(StateHasChanged);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(result);
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
        }
        #endregion

        #region On Sort Grid Data
        private async Task OnSortData(DataGridColumnSortEventArgs<TimeTrackingVM> args)
        {
            if (args.SortOrder != null)
            {
                filterRefGetTimeTracking = await gridSearchExtension.Sort(filterRefGetTimeTracking, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
            }
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
                    string formattedSearch = search;
                    if (DateTime.TryParseExact(search, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                    {
                        formattedSearch = parsedDate.ToString("M/d/yyyy h:mm:ss").ToLower();
                    }
                    filterRefGetTimeTracking = await gridSearchExtension.Filter(getTimeTrackingList, new Query()
                    {
                        Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US"
                        ? $@"i => (i.ActivityName != null && i.ActivityName.ToLower().Contains(@0)) 
                        || (i.AssignedOn.HasValue && i.AssignedOn.Value.ToString(""M/d/yyyy h:mm:ss tt"").Contains(@1))
                        || (i.CompleteOn.HasValue && i.CompleteOn.Value.ToString(""M/d/yyyy h:mm:ss tt"").Contains(@2))
                        || (i.Duration != null && i.Duration.ToString().Contains(@3))
                        || (i.AssignedBy != null && i.AssignedBy.ToLower().Contains(@4))                         
                        || (i.AssignedByDepartmentNameEn != null && i.AssignedByDepartmentNameEn.ToLower().Contains(@5))                          
                        || (i.AssignedToDepartmentNameEn != null && i.AssignedToDepartmentNameEn.ToLower().Contains(@6))                          
                        || (i.AssignedToEn != null && i.AssignedToEn.ToLower().Contains(@7))                        
                        || (i.StatusEn != null && i.StatusEn.Replace(""  "","" "").ToLower().Contains(@8))"

                        : $@"i => ((i.ActivityName != null && i.ActivityName.ToLower().Contains(@0))
                        || (i.AssignedOn.HasValue && i.AssignedOn.Value.ToString(""M/d/yyyy h:mm:ss tt"").Contains(@1))
                        || (i.CompleteOn.HasValue && i.CompleteOn.Value.ToString(""M/d/yyyy h:mm:ss tt"").Contains(@2))
                        || (i.Duration != null && i.Duration.ToString().Contains(@3))
                        || (i.AssignedBy != null && i.AssignedBy.ToLower().Contains(@4))
                        || (i.AssignedByDepartmentNameAr != null && i.AssignedByDepartmentNameAr.ToLower().Contains(@5))
                        || (i.AssignedToDepartmentNameAr != null && i.AssignedToDepartmentNameAr.ToLower().Contains(@6))
                        || (i.AssignedToAr != null && i.AssignedToAr.ToLower().Contains(@7))
                        || (i.StatusAr != null && i.StatusAr.Replace(""  "","" "").ToLower().Contains(@8))",

                        FilterParameters = new object[] { search, formattedSearch, formattedSearch, search, search, search, search, search, search }
                    }); await InvokeAsync(StateHasChanged);
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
