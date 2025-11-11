using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.GeneralEnums;
namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class COMSEnums : ComponentBase
    {


        #region Variable Declaration  
        protected RadzenDataGrid<SubTypeVM>? gridSubtype = new RadzenDataGrid<SubTypeVM>();
        protected RadzenDataGrid<RequestTypeVM>? gridRequesttype = new RadzenDataGrid<RequestTypeVM>();
        protected RadzenDataGrid<LookupsHistory>? gridLookupHistoryVM = new RadzenDataGrid<LookupsHistory>();

        IEnumerable<LookupsHistory> _LookupHistoryVM;
        protected IEnumerable<LookupsHistory> LookupHistoryVM
        {
            get
            {
                return _LookupHistoryVM;
            }
            set
            {
                if (!Equals(_LookupHistoryVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "SubTypeVM", NewValue = value, OldValue = _LookupHistoryVM };
                    _LookupHistoryVM = value;
                    Reload();
                }
            }
        }

        public int count { get; set; }
        public int lookup { get; set; }
        public bool lookup1 { get; set; } = true;
        public bool lookup2 { get; set; } = false;
        public bool lookup3 { get; set; } = false;
        public bool lookup4 { get; set; } = false;
        protected string search { get; set; }
        private Timer debouncer;
        private const int debouncerDelay = 500;

        IEnumerable<SubTypeVM> _SubTypeVM;
        protected IEnumerable<SubTypeVM> SubTypeVM
        {
            get
            {
                return _SubTypeVM;
            }
            set
            {
                if (!Equals(_SubTypeVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "SubTypeVM", NewValue = value, OldValue = _SubTypeVM };
                    _SubTypeVM = value;
                    Reload();
                }
            }
        }


        IEnumerable<RequestTypeVM> RequestTypeVM;
        IEnumerable<RequestTypeVM> _FilteredRequestTypeVM;
        protected IEnumerable<RequestTypeVM> FilteredRequestTypeVM
        {
            get
            {
                return _FilteredRequestTypeVM;
            }
            set
            {
                if (!Equals(_FilteredRequestTypeVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredRequestTypeVM", NewValue = value, OldValue = _FilteredRequestTypeVM };
                    _FilteredRequestTypeVM = value;
                    Reload();
                }
            }
        }
        protected async Task GetRequestTypeList()
        {
            var result = await lookupService.GetRequestTypeList();
            if (result.IsSuccessStatusCode)
            {
                RequestTypeVM = (IEnumerable<RequestTypeVM>)result.ResultData;
                FilteredRequestTypeVM = (IEnumerable<RequestTypeVM>)result.ResultData;
                count = FilteredRequestTypeVM.Count();
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }
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
                        Filter = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? $@"i => (i.Name_En != null && i.Name_En.ToLower().Contains(@0))" : $@"i => (i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@0))",
                        FilterParameters = new object[] { search.ToLower() }
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

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        protected bool isLoading { get; set; }
        protected bool isCheckedRole = false;

        #region Fuctions
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected override async Task OnInitializedAsync()
        {
            await Load();

        }
        protected async Task Load()
        {
            await GetRequestTypeList();
            StateHasChanged();
        }

        #endregion

        protected async Task ExpandDraftVersions(RequestTypeVM requestTypeVM)
        {
            await GetSubTypeList(Convert.ToInt32(requestTypeVM.Id));
            await GetLookupHistoryListByRefernceId(requestTypeVM.Id, (int)LookupsTablesEnum.CMS_REQUEST_TYPE_G2G_LKP);
        }

        #region HistoryFunctions
        protected async Task ExpandConsultationLegislationFileTypeHistory(ConsultationLegislationFileTypeVM consultationLegislationFileTypeVM)
        {
            await GetLookupHistoryListByRefernceId(consultationLegislationFileTypeVM.Id, (int)LookupsTablesEnum.COMS_CONSULTATION_Legislation_FILE_TYPE_G2G_LKP);
        }
        protected async Task ExpandComsInternationalArbitrationTypeHistory(ComsInternationalArbitrationTypeVM comsInternationalArbitrationTypeVM)
        {
            await GetLookupHistoryListByRefernceId(comsInternationalArbitrationTypeVM.Id, (int)LookupsTablesEnum.COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_G2G_LKP);
        }
        protected async Task ExpandRequesttypeHistory(SubTypeVM subTypeVM)
        {
            await GetLookupHistoryListByRefernceId(subTypeVM.RequestTypeId, (int)LookupsTablesEnum.CMS_REQUEST_TYPE_G2G_LKP);
        }
        protected async Task ExpandSubTypeRequestHistory(SubTypeVM subTypeVM)
        {
            await GetLookupHistoryListByRefernceId(subTypeVM.Id, (int)LookupsTablesEnum.CMS_SUBTYPE_G2G_LKP);
        }

        protected async Task GetLookupHistoryListByRefernceId(int Id, int LookupstableId)
        {
            var result = await lookupService.GetLookupHistoryListByRefernceId(Id, LookupstableId);

            LookupHistoryVM = (IEnumerable<LookupsHistory>)result.ResultData;

            await InvokeAsync(StateHasChanged);
        }
        #endregion
        protected async Task GetSubTypeList(int RequestTypeId)
        {
            var result = await lookupService.GetSubTypeList(RequestTypeId);


            //if (string.IsNullOrEmpty(search))
            //    search = "";
            //else
            //    search = search.ToLower();
            //var result = await lookupService.GetSubTypeLists(new Query()
            //{
            //    Filter = $@"i => ((i.Name_En != null && i.Name_En.ToLower().Contains(@0)) ||(i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@1)))",
            //    FilterParameters = new object[] { search, search }
            //});
            SubTypeVM = (IEnumerable<SubTypeVM>)result.ResultData;
            await InvokeAsync(StateHasChanged);
        }


        #region Request Sub type 
        protected async Task EditRequestSubTypeItem(SubTypeVM args)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<RequestTypeDetailAdd>(
                translationState.Translate("Edit_Request_Detail"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions() { Width = "27%", CloseDialogOnOverlayClick = false });
                await Task.Delay(400);
                await Load();
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

        #region Update User Group

        #endregion

        #region button Event
        protected async Task Cencel(MouseEventArgs args)
        {
            bool? dialogResponse = await dialogService.Confirm(
            translationState.Translate("Sure_Cancel"),
            translationState.Translate("Confirm"),
            new ConfirmOptions()
            {
                OkButtonText = @translationState.Translate("OK"),
                CancelButtonText = @translationState.Translate("Cancel")
            });

            if (dialogResponse == true)
            {
                navigationManager.NavigateTo("/groups");
            }
        }
        protected async Task Submitform()
        {
            //grid0.Reset();
            //await grid0.Reload();

            StateHasChanged();

        }
        #endregion

        #region Request  type 
        protected async Task EditRequestTypeItem(RequestTypeVM args)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<RequestTypeDetailAdd>(
                translationState.Translate("Edit_Request_Detail"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions() { Width = "27%", CloseDialogOnOverlayClick = false });
                await Task.Delay(400);
                await Load();
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

    }
}
