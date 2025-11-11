using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

//< History Author = 'Ammaar Naveed' Date = '2024-06-04' Version = "1.0" Branch = "master" > UI fixations in pages and dialogs</History>*@
namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class COMSLookup : ComponentBase
    {

        #region Variable Declaration
        protected bool isLoading { get; set; }
        protected bool isCheckedRole = false;

        protected RadzenDataGrid<CourtDetailVM>? grid1 = new RadzenDataGrid<CourtDetailVM>();
        protected RadzenDataGrid<SubTypeVM>? gridSubtype = new RadzenDataGrid<SubTypeVM>();
        protected RadzenDataGrid<RequestTypeVM>? gridRequesttype = new RadzenDataGrid<RequestTypeVM>();
        protected RadzenDataGrid<LookupsHistory>? gridLookupHistoryVM = new RadzenDataGrid<LookupsHistory>();

        SubTypeVM subTypeVM = new SubTypeVM();

        protected string search;
        protected string searchSubType;
        protected string searchHistory;
        IEnumerable<SubTypeVM> SubTypeVM = new List<SubTypeVM>();
        protected IEnumerable<SubTypeVM> FilteredSubTypeVM = new List<SubTypeVM>();
        /*protected IEnumerable<LookupsHistory> LookupHistoryVM = new List<LookupsHistory>();*/
        IEnumerable<RequestTypeVM> RequestTypeVM = new List<RequestTypeVM>();
        protected IEnumerable<RequestTypeVM> FilteredRequestTypeVM = new List<RequestTypeVM>(); private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion

        #region Fuctions

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async Task Load()
        {
            spinnerService.Show();
            await GetRequestTypeList();
            StateHasChanged();
            spinnerService.Hide();
        }

        #endregion
        #region On Search Input

        protected async Task OnFileTypeSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    searchSubType = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredSubTypeVM = await gridSearchExtension.Filter(SubTypeVM, new Query()
                    {
                        Filter = $@"i => ((i.Name_En != null && i.Name_En.ToLower().Contains(@0))
                    ||(i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@1))
                    || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").ToLower().Contains(@2))
                    || (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@3)) 
                    || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@4)))",
                        FilterParameters = new object[] { searchSubType, searchSubType, searchSubType, searchSubType, searchSubType }
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

        #region Sub Type Lists



        protected async Task GetSubTypeList(int RequestTypeId)
        {
            var result = await lookupService.GetSubTypeList(RequestTypeId);
            SubTypeVM = (IEnumerable<SubTypeVM>)result.ResultData;
            FilteredSubTypeVM = SubTypeVM;
            //count = CmsCourtG2GLKP.Count();
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Request Type Lists

        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredRequestTypeVM = await gridSearchExtension.Filter(RequestTypeVM, new Query()
                    {
                        Filter = $@"i => ((i.Name_En != null && i.Name_En.ToLower().Contains(@0)) ||(i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@1))|| (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").ToLower().Contains(@2))|| (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@3)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@4)))",
                        FilterParameters = new object[] { search, search, search, search, search }
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
        protected async Task GetRequestTypeList()
        {
            var result = await lookupService.GetRequestTypeList();
            if (result.IsSuccessStatusCode)
            {
                RequestTypeVM = (IEnumerable<RequestTypeVM>)result.ResultData;
                FilteredRequestTypeVM = (IEnumerable<RequestTypeVM>)result.ResultData;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }
        #endregion

        #region HistoryFunctions
        protected async Task ExpandDraftVersions(RequestTypeVM requestTypeVM)
        {
            await GetSubTypeList(requestTypeVM.Id);
            selectedRequestType = requestTypeVM;
        }
        /*protected async Task GetLookupHistoryListByRefernceId(int Id, int LookupstableId)
        {
            var result = await lookupService.GetLookupHistoryListByRefernceId(Id, LookupstableId);

            LookupHistoryVM = (IEnumerable<LookupsHistory>)result.ResultData;

            await InvokeAsync(StateHasChanged);
        }*/
        #endregion

        #region Edit Request type 
        protected async Task EditRequestTypeItem(RequestTypeVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<RequestTypeDetailAdd>(
                translationState.Translate("Edit_Request_Detail"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false,
                    Width = "30%"
                }) == true)
                {
                    await Task.Delay(100);
                    await GetRequestTypeList();
                }

                StateHasChanged();
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

        #region Add Sub Types
        private RequestTypeVM selectedRequestType;
        protected async Task AddSubtype(RequestTypeVM selectedRequestType)
        {
            try
            {

                if (await dialogService.OpenAsync<SubTypesAdd>(
                    translationState.Translate("Add_Sub_Types"),
                     new Dictionary<string, object>()
                        {
                            { "SelectedRequestType", selectedRequestType }
                        },
                    new DialogOptions() { Width = "30%", CloseDialogOnOverlayClick = false }) == true)
                {
                    await Task.Delay(100);
                    await GetSubTypeList(selectedRequestType.Id);
                }
                StateHasChanged();
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

        #region Edit Request Sub type 
        protected async Task EditRequestSubTypeItem(SubTypeVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<RequestSubTypeDetailAdd>(
                translationState.Translate("Edit_Request_Detail"),
                new Dictionary<string, object>() { { "Id", args.Id } },
               new DialogOptions()
               {
                   CloseDialogOnOverlayClick = false,
                   CloseDialogOnEsc = false,
                   Width = "30%"
               }) == true)
                {
                    await Task.Delay(100);
                    await GetSubTypeList(selectedRequestType.Id);
                }
                StateHasChanged();
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

        #region Redirect Functions
        public void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        public void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

    }
}
