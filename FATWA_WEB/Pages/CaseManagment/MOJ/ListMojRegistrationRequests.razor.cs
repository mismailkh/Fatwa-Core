using Append.Blazor.Printing;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_GENERAL.Helper;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.CaseManagment.MOJ
{
    //<History Author = 'Hassan Abbas' Date='2022-11-29' Version="1.0" Branch="master"> List of Moj Registration Requests</History>
    public partial class ListMojRegistrationRequests : ComponentBase
    {
        #region Variables Declaration
        protected RadzenDataGrid<MojRegistrationRequestVM>? grid = new RadzenDataGrid<MojRegistrationRequestVM>();
        protected bool Keywords = false; private Timer debouncer;
        private const int debouncerDelay = 500;
        public int selectedIndex { get; set; } = 0;
        public bool IsRegistered = false;
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int PageNumber { get; set; } = 1;
        private int? PageSize { get; set; }
        private int CurrentPageSize => grid.PageSize;
        private bool isGridLoaded { get; set; }
        private bool isPageSizeChangeOnFirstLastPage { get; set; }

        #endregion

        #region Properties
        IEnumerable<MojRegistrationRequestVM> FilteredMojRegistrationRequests { get; set; }
        protected IEnumerable<MojRegistrationRequestVM> mojRegistrationRequests = new List<MojRegistrationRequestVM>();

        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            PageSize = systemSettingState.Grid_Pagination;
            translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
        }
        #endregion        

        #region On Load Grid Data
        /*<History Author='Ammaar Naveed' Date='2025-02-04' Version="1.0" Branch="master">Load grid data based on pagination</History>*/
        protected async Task OnLoadData(LoadDataArgs args)
        {
            try
            {
                if (string.IsNullOrEmpty(args.OrderBy) || CurrentPage != PageNumber || CurrentPageSize != PageSize)
                {
                    if (isGridLoaded && PageSize == CurrentPageSize && !isPageSizeChangeOnFirstLastPage)
                    {
                        grid.CurrentPage = (int)PageNumber - 1;
                        isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(args);
                    if (selectedIndex == 0)
                    {
                        IsRegistered = false;
                    }
                    else
                    {
                        IsRegistered = true;
                    }
                    spinnerService.Show();
                    var response = await cmsCaseFileService.GetMojRegistrationRequests(loginState.UserDetail.SectorTypeId, IsRegistered, PageNumber, PageSize);
                    spinnerService.Hide();
                    if (response.IsSuccessStatusCode)
                    {
                        FilteredMojRegistrationRequests = mojRegistrationRequests = (IEnumerable<MojRegistrationRequestVM>)response.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput(search);
                        if (!string.IsNullOrEmpty(args.OrderBy) && string.IsNullOrEmpty(search))
                        {
                            FilteredMojRegistrationRequests = await gridSearchExtension.Sort(FilteredMojRegistrationRequests, ColumnName, SortOrder);
                        }
                        await InvokeAsync(StateHasChanged);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        private void SetPagingProperties(LoadDataArgs args)
        {
            if (PageSize != CurrentPageSize)
            {
                int oldPageCount = mojRegistrationRequests.Any() ? (mojRegistrationRequests.First().TotalCount) / ((int)PageSize) : 1;
                int oldPageNumber = (int)PageNumber - 1;
                isGridLoaded = true;
                PageNumber = CurrentPage;
                PageSize = args.Top;
                int TotalPages = mojRegistrationRequests.Any() ? (mojRegistrationRequests.First().TotalCount) / (grid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    PageNumber = TotalPages + 1;
                    PageSize = args.Top;
                    grid.CurrentPage = TotalPages;
                }
                if ((PageNumber == 1 || (PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                {
                    isPageSizeChangeOnFirstLastPage = true;
                }
                else
                {
                    isPageSizeChangeOnFirstLastPage = false;
                }
                return;
            }
            PageNumber = CurrentPage;
            PageSize = args.Top;
        }

        #endregion

        #region On Sort Grid Data
        /*<History Author='Ammaar Naveed' Date='2025-02-04' Version="1.0" Branch="master">Sort grid data</History>*/
        private async Task OnSortData(DataGridColumnSortEventArgs<MojRegistrationRequestVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredMojRegistrationRequests = await gridSearchExtension.Sort(FilteredMojRegistrationRequests, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        #endregion

        #region Grid Search
        protected string search { get; set; }

        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredMojRegistrationRequests = await gridSearchExtension.Filter(mojRegistrationRequests, new Query()
                {
                    Filter = $@"i => ( i.FileNumber != null && i.FileNumber.ToString().ToLower().Contains(@0)||
                    ( i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@0)))",
                    FilterParameters = new object[] { search }
                });
                if (!string.IsNullOrEmpty(ColumnName))
                    FilteredMojRegistrationRequests = await gridSearchExtension.Sort(FilteredMojRegistrationRequests, ColumnName, SortOrder);
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

        #region Tab Change Event
        public async Task OnTabChange(int index)
        {
            try
            {
                if (index == selectedIndex) { return;}
                search = ColumnName = string.Empty;
                spinnerService.Show();
                selectedIndex = index;
                await Task.Delay(100);
                grid.Reset();
                await grid.Reload();
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

        #region GRID Buttons
        //<History Author = 'Hassan Abbas' Date='2022-11-29' Version="1.0" Branch="master"> Redirect to View Detail page</History>
        protected async Task Detail(MojRegistrationRequestVM args)
        {
            navigationState.ReturnUrl = "moj-registration-requests";
            navigationManager.NavigateTo("register-to-moj/" + args.FileId + "/" + args.Id);
        }
        protected async void UploadAttachments(MojRegistrationRequestVM args)
        {
            spinnerService.Show();
            var uploadedAttachement = await fileUploadService.GetUploadedAttachementById(args.DocumentId);
            if (uploadedAttachement != null)
            {
                navigationState.ReturnUrl = "moj-registration-requests";
                if (uploadedAttachement.AttachmentTypeId == (int)AttachmentTypeEnum.StopExecutionOfJudgment)
                {
                    navigationManager.NavigateTo("/Add-Sub-Case/" + uploadedAttachement.ReferenceGuid + "/" + args.FileId);
                }
                else
                {
                    navigationManager.NavigateTo("/create-registered-case/" + args.FileId + "/" + args.Id + "/" + args.DocumentId + "/" + uploadedAttachement.AttachmentTypeId);
                }
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("File_Not_Found"),
                    Summary = translationState.Translate("Error"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

            spinnerService.Hide();
        }
        protected async Task PrintDocument(MojRegistrationRequestVM args)
        {
            try
            {
                var physicalPath = string.Empty;
                var document = await fileUploadService.GetUploadedAttachementById(args.DocumentId);

#if DEBUG
                {
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + document.StoragePath);
                }
#else
			{
			 physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + document.StoragePath);
		     physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
            }
#endif
                if (FileUtils.CheckFilePath(physicalPath))
                {
                    string base64String = await DocumentEncryptionService.GetDecryptedDocumentBase64(physicalPath, document.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                    await PrintingService.Print(new PrintOptions(base64String) { Base64 = true });
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("File_Not_Found"),
                        Summary = translationState.Translate("Error"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
            catch
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

        #region Redirect Function
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }

        protected void ViewDetail(MojRegistrationRequestVM arg)
        {
            navigationManager.NavigateTo("/case-detailmoj/" + arg.CaseId);
        }
        #endregion
    }
}
