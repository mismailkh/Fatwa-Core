using DMS_WEB.Services;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using static FATWA_DOMAIN.Enums.Consultation.ConsultationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_GENERAL.Helper.Response;

namespace DMS_WEB.Pages.DocumentManagement
{
    //<History Author = 'Muhammad Zaeem' Date='2023-06-06' Version="1.0" Branch="master">Document List Page</History>

    public partial class ListDocument : ComponentBase
    {

        #region Varriable

        protected DocumentListAdvanceSearchVM advanceSearchVM { get; set; } = new DocumentListAdvanceSearchVM();
        protected RadzenDataGrid<DMSDocumentListVM>? grid = new RadzenDataGrid<DMSDocumentListVM>();
        protected bool Keywords = false;
        public bool isVisible { get; set; }
        protected List<AttachmentType> attachmentTypes { get; set; } = new List<AttachmentType>();
        #endregion

        #region Full property declaration

        IEnumerable<DMSDocumentListVM> _documentList;
        protected IEnumerable<DMSDocumentListVM> FilteredDocumentList { get; set; } = new List<DMSDocumentListVM>();
        protected IEnumerable<DMSDocumentListVM> documentList
        {
            get
            {
                return _documentList;
            }
            set
            {
                if (!object.Equals(_documentList, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "documentList", NewValue = value, OldValue = _documentList };
                    _documentList = value;

                    Reload();
                }
            }

        }
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
                    Reload();
                }
            }
        }
        #endregion

        #region On Initialize
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateAttachmentTypes();
            await Load();
            spinnerService.Hide();
        }
        #endregion

        #region On Load
       
        protected async Task Load()
        {
            try
            {
                var response = await fileUploadService.GetDocumentsList(advanceSearchVM);
                if (response.IsSuccessStatusCode)
                {
                    _documentList = (IEnumerable<DMSDocumentListVM>)response.ResultData;
                    FilteredDocumentList = (IEnumerable<DMSDocumentListVM>)response.ResultData;
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    await ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                {
                    FilteredDocumentList = await gridSearchExtension.Filter(_documentList, new Query()
                    {
                        Filter = $@"i => (i.FileName != null && i.FileName.ToString().Contains(@0)) || (i.AttachmentTypeEn != null && i.AttachmentTypeEn.ToString().ToLower().Contains(@1))",
                        FilterParameters = new object[] { search, search }
                    });
                }
                else
                {
                    FilteredDocumentList = await gridSearchExtension.Filter(_documentList, new Query()
                    {
                        Filter = $@"i => (i.FileName != null && i.FileName.ToString().Contains(@0)) || (i.AttachmentTypeAr != null && i.AttachmentTypeAr.ToString().ToLower().Contains(@1))",
                        FilterParameters = new object[] { search, search }

                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region GRid Buttons
        protected async Task DocumentDetail(DMSDocumentListVM args)
        {
            navigationManager.NavigateTo("document-detail/" + Convert.ToInt32(args.UploadedDocumentId));


        }
        #endregion

        #region Badrequest Notification
        protected async Task ReturnBadRequestNotification(ApiCallResponse response)
        {
            try
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Token_Expired"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await Task.Delay(5000);
                    await BrowserStorage.RemoveItemAsync("User");
                    await BrowserStorage.RemoveItemAsync("Token");
                    await BrowserStorage.RemoveItemAsync("RefreshToken");
                    await BrowserStorage.RemoveItemAsync("UserDetail");
                    await BrowserStorage.RemoveItemAsync("SecurityStamp");
                    loginState.IsLoggedIn = false;
                    loginState.IsStateChecked = true;
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

        #region Advance Search
        public async void ResetForm()
        {

            advanceSearchVM = new DocumentListAdvanceSearchVM();

            await Load();
            Keywords = false;
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

        protected async Task SubmitAdvanceSearch()
        {
            if (advanceSearchVM.CreatedFrom > advanceSearchVM.CreatedTo)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
                    //Summary = $"خطأ!",
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                Keywords = true;
                return;
            }
            if ( advanceSearchVM.AttachmentTypeId == 0 && string.IsNullOrWhiteSpace(advanceSearchVM.Filename)
                 && !advanceSearchVM.CreatedFrom.HasValue && !advanceSearchVM.CreatedTo.HasValue)
            {

            }
            else
            {
                Keywords = true;

                await Load();
                //await grid.Reload();
                StateHasChanged();
            }
        }

        #endregion

        #region Redirect Function
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

        #region populate grids
        //<History Author = 'Muhammad Zaeem' Date='2023-03-14' Version="1.0" Branch="master">Populate Count data</History>
        protected async Task PopulateAttachmentTypes()
        {
            var response = await fileUploadService.GetAllAttachmentTypes();
            if (response.IsSuccessStatusCode)
            {
                attachmentTypes = (List<AttachmentType>)response.ResultData;
            }
            else
            {
                await ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion

    }
}
