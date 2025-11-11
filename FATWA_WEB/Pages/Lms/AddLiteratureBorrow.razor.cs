using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_GENERAL.Helper;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Text.RegularExpressions;
using static FATWA_DOMAIN.Enums.LiteratureEnum;
using static FATWA_DOMAIN.Enums.UserEnum;
using static FATWA_DOMAIN.Models.ViewModel.LiteratureAdvancedSearchVM;
using Enum = System.Enum;

namespace FATWA_WEB.Pages.Lms
{
    public partial class AddLiteratureBorrow : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }


        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        #region Service Injections



        #endregion

        #region Variables
        protected bool DisableSaveButton = false;
        public bool Visible { get; set; }
        protected bool Keywords = false;
        protected bool AdvanceSearchGridSHow = false;
        protected List<object> AdvancedSearchOptions { get; set; } = new List<object>();
        protected LiteratureAdvancedSearchVM advancedSearchVM = new LiteratureAdvancedSearchVM();
        protected RadzenDataGrid<LiteratureDetailVM>? AdvanceSearchGrid;
        protected IEnumerable<LmsLiteratureIndex> LiteratureIndexeDetails { get; set; } = new List<LmsLiteratureIndex>();
        public IEnumerable<LmsLiteratureAuthor> LmsLiteratureAuthor { get; set; } = new List<LmsLiteratureAuthor>();

        public class AdvancedSearchEnumTypes
        {
            public LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum advancedSearchEnumValue { get; set; }
            public string advancedSearchEnumName { get; set; }
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
                    var args = new FATWA_WEB.Services.PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;
                    Reload();
                }
            }
        }

        protected RadzenDataGrid<UserBorrowLiteratureVM> userBorrowLiteratureGrid;
        public List<UserBorrowLiteratureVM> userBorrowLiteratureDetails { get; set; }

        LmsLiteratureBorrowDetail _lmsliteratureborrowdetail;
        protected LmsLiteratureBorrowDetail lmsliteratureborrowdetail
        {
            get
            {
                return _lmsliteratureborrowdetail;
            }
            set
            {
                if (!object.Equals(_lmsliteratureborrowdetail, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "lmsliteratureborrowdetail", NewValue = value, OldValue = _lmsliteratureborrowdetail };
                    _lmsliteratureborrowdetail = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<LmsLiterature> _getLmsLiteratureDetailsForLiteratureIdResult;
        protected IEnumerable<LmsLiterature> getLmsLiteratureDetailsForLiteratureIdResult
        {
            get
            {
                return _getLmsLiteratureDetailsForLiteratureIdResult;
            }
            set
            {
                if (!object.Equals(_getLmsLiteratureDetailsForLiteratureIdResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLmsLiteratureDetailsForLiteratureIdResult", NewValue = value, OldValue = _getLmsLiteratureDetailsForLiteratureIdResult };
                    _getLmsLiteratureDetailsForLiteratureIdResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        protected RadzenDataGrid<LiteratureDetailsForBorrowRequestVM> BorrowLiteratureGrid;

        protected UserDetailVM userDetails { get; set; } = new UserDetailVM();
        #endregion

        #region For LiteratureAutocomplete

        public IEnumerable<LiteratureDetailVM> getBookDetails = null;
        public IEnumerable<LiteratureDetailVM> getAdvanceSearchDetails { get; set; } = new List<LiteratureDetailVM>();
        Regex rgx = new Regex("[^a-zA-Z?-?]");

        string appCulture = Thread.CurrentThread.CurrentUICulture.Name;


        public Response.ApiCallResponse response { get; private set; }
        public bool ShowBarcode { get; private set; }

        //private bool isLiteratureChanged = false;
        protected LiteratureDetailVM selectedLiterature = null;
        //<History Author = 'Umer Zaman' Date='2022-08-31' Version="1.0" Branch="master">get isactive & isborrowable barcode number detail's by using literature id</History>
        public async void OnLiteratureChange(object value, string name)
        {
            try
            {
                //if (isLiteratureChanged != false && getBookDetails != null)
                if (getBookDetails != null)
                {
                    selectedLiterature = getBookDetails.Where(c => c.LiteratureName == (string)value).FirstOrDefault();

                    if (selectedLiterature != null)
                    {
                        if (selectedLiterature.BookStatus.Contains(BookStatus.Borrowable.ToString() + " : 0"))
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Info,
                                Detail = translationState.Translate("Literature_With_Zero_Borrowable"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            lmsliteratureborrowdetail.literatureDetailsForBorrowRequestModel = new List<LiteratureDetailsForBorrowRequestVM>();
                            return;
                        }
                        else
                        {
                            // Get active & isborrowable barcode number detail by using literature id.
                            response = await lmsLiteratureBorrowDetailService.GetBarcodeNumberDetailByusingLiteratureId(selectedLiterature.LiteratureId);
                            if (response.IsSuccessStatusCode)
                            {
                                var barcodeResult = (LiteratureDetailsForBorrowRequestVM)response.ResultData;
                                if (barcodeResult.LiteratureId != 0)
                                {
                                    lmsliteratureborrowdetail.literatureDetailsForBorrowRequestModel = new List<LiteratureDetailsForBorrowRequestVM>();

                                    lmsliteratureborrowdetail.literatureDetailsForBorrowRequestModel.Add(barcodeResult);
                                    lmsliteratureborrowdetail.literatureDetailsForBorrowRequestModel.FirstOrDefault().BorrowDate = lmsliteratureborrowdetail.IssueDate;
                                    lmsliteratureborrowdetail.literatureDetailsForBorrowRequestModel.FirstOrDefault().ReturnDate = lmsliteratureborrowdetail.DueDate;
                                    //lmsliteratureborrowdetail.BarCodeNumber = barcodeResult.BarCodeNumber;
                                    //lmsliteratureborrowdetail.BarcodeId = barcodeResult.BarcodeId;
                                    ShowBarcode = true;
                                }
                            }
                            lmsliteratureborrowdetail.LiteratureId = selectedLiterature.LiteratureId;
                            lmsliteratureborrowdetail.ISBN = selectedLiterature.ISBN;
                        }
                    }
                    else
                    {
                        lmsliteratureborrowdetail.LiteratureName = string.Empty;
                        lmsliteratureborrowdetail.LiteratureId = 0;
                        lmsliteratureborrowdetail.ISBN = string.Empty;
                        lmsliteratureborrowdetail.BarCodeNumber = string.Empty;
                        lmsliteratureborrowdetail.BarcodeId = 0;
                        ShowBarcode = false;
                        lmsliteratureborrowdetail.literatureDetailsForBorrowRequestModel = new List<LiteratureDetailsForBorrowRequestVM>();
                    }
                    await InvokeAsync(StateHasChanged);
                }
                //else
                //    isLiteratureChanged = true; 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        bool IsAdminUser;
        #endregion

        #region For UserAutocomplete

        public IEnumerable<UserVM> getUserDetails { get; set; }
        public async void OnLoadUserData(LoadDataArgs args)
        {
            try
            {
                string str = args.Filter;
                bool hasNumSpecialChars = rgx.IsMatch(str);

                if (!hasNumSpecialChars)
                {
                    var getUserDetail = await userService.UserListBySearchTerm(str);
                    if (getUserDetail.IsSuccessStatusCode)
                    {
                        getUserDetails = (List<UserVM>)getUserDetail.ResultData;
                    }
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected bool isAllowToSaveDisabled = false;
        protected UserVM? selectedUser = null;
        protected async Task OnUserChange(object userId, string name)
        {
            if (getUserDetails != null)
            {
                //getUserDetails = new List<UserVM>();
                var getUserDetail = await userService.UserListBySearchTerm(string.Empty);
                if (getUserDetail.IsSuccessStatusCode)
                {
                    getUserDetails = (List<UserVM>)getUserDetail.ResultData;
                }
                selectedUser = getUserDetails.FirstOrDefault(c => c.Id == (string)userId);
                if (selectedUser != null)
                {
                    lmsliteratureborrowdetail.UserId = selectedUser.Id;
                    lmsliteratureborrowdetail.PhoneNumber = selectedUser.PhoneNumber == null ? "---" : selectedUser.PhoneNumber;
                    lmsliteratureborrowdetail.EligibleCount = selectedUser.EligibleCount;
                    allowToSaveDisabled(lmsliteratureborrowdetail.EligibleCount);
                    await GetUserLiteratures(selectedUser.Id);
                    await InvokeAsync(StateHasChanged);
                }


            }
        }

        protected async Task GetUserLiteratures(string userId)
        {
            try
            {
                var BorrowLiteratureDetails = await userService.UserBorrowLiteratures(userId);
                if (BorrowLiteratureDetails.IsSuccessStatusCode)
                {
                    userBorrowLiteratureDetails = (List<UserBorrowLiteratureVM>)BorrowLiteratureDetails.ResultData;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

        #region On Component Load

        protected override async Task OnInitializedAsync()
        {
            userDetails = await BrowserStorage.GetItemAsync<UserDetailVM>("UserDetail");
            await Load();
        }

        protected async Task Load()
        {
            spinnerService.Show();
            try
            {
                if (userDetails != null)
                {
                    if (userDetails.RoleId == SystemRoles.FatwaAdmin || userDetails.RoleId == SystemRoles.LMSAdmin)
                    {
                        IsAdminUser = true;
                        DisableSaveButton = false;
                        lmsliteratureborrowdetail = new LmsLiteratureBorrowDetail() { };
                        await PopulateAdvancedSearchOptions();
                        OnLoadData();
                        await OnChangeIssueDate(null);
                    }
                    else
                    {
                        IsAdminUser = false;
                        DisableSaveButton = false;
                        lmsliteratureborrowdetail = new LmsLiteratureBorrowDetail() { };
                        var getUserDetail = await userService.UserListBySearchTerm(string.Empty);
                        if (getUserDetail.IsSuccessStatusCode)
                        {
                            getUserDetails = (List<UserVM>)getUserDetail.ResultData;
                            selectedUser = getUserDetails.FirstOrDefault(c => c.Id == loginState.UserDetail.UserId);
                            if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                            {
                                lmsliteratureborrowdetail.UserName = selectedUser.FirstNameEnglish;
                            }
                            else
                            {
                                lmsliteratureborrowdetail.UserName = selectedUser.FirstNameArabic;
                            }

                            lmsliteratureborrowdetail.UserId = loginState.UserDetail.UserId;
                            lmsliteratureborrowdetail.PhoneNumber = selectedUser.PhoneNumber == null ? "---" : selectedUser.PhoneNumber;
                            lmsliteratureborrowdetail.EligibleCount = selectedUser.EligibleCount;
                            if (selectedUser.EligibleCount >= 3)
                            {
                                isAllowToSaveDisabled = true;
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Info,
                                    Detail = translationState.Translate("Borrow_Book_Limit"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                            }
                            else
                                isAllowToSaveDisabled = false;
                            await GetUserLiteratures(selectedUser.Id);
                        }
                        DisableSaveButton = false;
                        await PopulateAdvancedSearchOptions();
                        OnLoadData();
                        await OnChangeIssueDate(null);

                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    await JSRuntime.InvokeVoidAsync("history.back");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            spinnerService.Hide();
        }

        #endregion

        #region Save Form 

        protected async Task FormSubmit(LmsLiteratureBorrowDetail args)
        {
            try
            {
                DisableSaveButton = true;
                if (lmsliteratureborrowdetail.literatureDetailsForBorrowRequestModel.Count() == 0)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Warning,
                        Detail = translationState.Translate("Please_select_literature"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    DisableSaveButton = false;
                    return;
                }
                else
                {
                    #region add Borrow Detail scenario
                    bool? dialogResponse = await dialogService.Confirm(
                        @translationState.Translate("Are_you_sure_you_want_to_add_this_borrower"),
                        @translationState.Translate("Confirm"),
                        new ConfirmOptions()
                        {
                            OkButtonText = @translationState.Translate("OK"),
                            CancelButtonText = @translationState.Translate("Cancel")
                        });

                    LmsLiteratureBorrowDetail lmsLiteratureBorrowDetail;
                    if (dialogResponse == true)
                    {
                        spinnerService.Show();
                        var result = lmsliteratureborrowdetail.literatureDetailsForBorrowRequestModel.FirstOrDefault();
                        if (result != null)
                        {
                            lmsliteratureborrowdetail.BarCodeNumber = result.BarcodeNumber;
                            lmsliteratureborrowdetail.BarcodeId = (int)result.BarcodeId;
                            lmsliteratureborrowdetail.IssueDate = (DateTime)result.BorrowDate;
                            lmsliteratureborrowdetail.DueDate = (DateTime)result.ReturnDate;
                            lmsliteratureborrowdetail.BorrowReturnApprovalStatus = (int)BorrowReturnApprovalStatus.Default;
                        }

                        if (loginState.UserRoles.Any(u => u.RoleId == SystemRoles.LMSAdmin)) // role id getting to check the admin
                        {
                            lmsliteratureborrowdetail.RoleId = SystemRoles.LMSAdmin;
                        }
                        else if (loginState.UserRoles.Any(u => u.RoleId == SystemRoles.FatwaAdmin)) // role id getting to check the admin
                        {
                            lmsliteratureborrowdetail.RoleId = SystemRoles.FatwaAdmin;
                        }
                        else // if there is no admin role except LMSAdmin and LMSAdmin then role id will be null
                             // and LmsLiteraturesController in (CreateLmsLiterature method) using there this role Id.
                        {
                            lmsliteratureborrowdetail.RoleId = null;
                        }

                        var response = await lmsLiteratureBorrowDetailService.CreateLmsLiteratureBorrowDetail(lmsliteratureborrowdetail);
                        spinnerService.Hide();
                        if (response.IsSuccessStatusCode)
                        {
                            lmsLiteratureBorrowDetail = (LmsLiteratureBorrowDetail)response.ResultData;
                            if (lmsLiteratureBorrowDetail != null)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("Borrower_Add_Success"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                                //  navigationManager.NavigateTo("/lms-literature-borrow-details");
                            }
                            StateHasChanged();
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }

                        lmsliteratureborrowdetail.EligibleCount++;
                        var boorowCount = lmsliteratureborrowdetail.EligibleCount;
                        if (selectedUser.DepartmentId == (int)DepartmentEnum.Operational)
                        {
                            if (boorowCount < 4)
                            {
                                #region boorow another book

                                dialogResponse = await dialogService.Confirm(
                                translationState.Translate("Borrow_Another_Book"),
                                translationState.Translate("Borrow_Another_Book_Confirmation"),
                                new ConfirmOptions()
                                {
                                    OkButtonText = @translationState.Translate("Yes"),
                                    CancelButtonText = @translationState.Translate("No")
                                });
                                if (dialogResponse == true)
                                {
                                    DisableSaveButton = false;
                                    //await Load();
                                    lmsliteratureborrowdetail.UserId = selectedUser.Id;

                                    if (userDetails.RoleId == SystemRoles.FatwaAdmin || userDetails.RoleId == SystemRoles.LMSAdmin)
                                    {
                                        lmsliteratureborrowdetail.UserName = selectedUser.Id;
                                    }
                                    else
                                    {
                                        if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                                        {
                                            lmsliteratureborrowdetail.UserName = selectedUser.FirstNameEnglish;
                                        }
                                        else
                                        {
                                            lmsliteratureborrowdetail.UserName = selectedUser.FirstNameArabic;
                                        }
                                    }
                                    if (selectedUser.PhoneNumber == null)
                                    {
                                        lmsliteratureborrowdetail.PhoneNumber = "---";
                                    }
                                    else
                                    {
                                        lmsliteratureborrowdetail.PhoneNumber = selectedUser.PhoneNumber;
                                    }
                                    lmsliteratureborrowdetail.EligibleCount = boorowCount;

                                    allowToSaveDisabled(lmsliteratureborrowdetail.EligibleCount);

                                    StateHasChanged();

                                    var responsedata = await lmsLiteratureBorrowDetailService.GetLmsLiteratureBySearchTerm(null, appCulture);
                                    if (responsedata.IsSuccessStatusCode)
                                    {
                                        getBookDetails = (IEnumerable<LiteratureDetailVM>)responsedata.ResultData;
                                    }
                                    else
                                    {
                                        await invalidRequestHandlerService.ReturnBadRequestNotification(responsedata);
                                    }
                                }

                                #endregion

                                else
                                {
                                    dialogService.Close(lmsliteratureborrowdetail);
                                    navigationManager.NavigateTo("/lmsliteratureborrowdetail-list");
                                    //await Load();
                                }
                            }
                        }
                        else if (selectedUser.DepartmentId == (int)DepartmentEnum.Administrative)
                        {
                            if (boorowCount < 3)
                            {
                                #region boorow another book

                                dialogResponse = await dialogService.Confirm(
                                translationState.Translate("Borrow_Another_Book"),
                                translationState.Translate("Confirm"),
                                new ConfirmOptions()
                                {
                                    OkButtonText = @translationState.Translate("OK"),
                                    CancelButtonText = @translationState.Translate("Cancel")
                                });
                                if (dialogResponse == true)
                                {
                                    DisableSaveButton = false;
                                    //await Load();

                                    lmsliteratureborrowdetail.UserId = selectedUser.Id;
                                    lmsliteratureborrowdetail.UserId = selectedUser.Id;
                                    if (userDetails.RoleId == SystemRoles.FatwaAdmin || userDetails.RoleId == SystemRoles.LMSAdmin)
                                    {
                                        lmsliteratureborrowdetail.UserName = selectedUser.Id;
                                    }
                                    else
                                    {
                                        if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                                        {
                                            lmsliteratureborrowdetail.UserName = selectedUser.FirstNameEnglish;
                                        }
                                        else
                                        {
                                            lmsliteratureborrowdetail.UserName = selectedUser.FirstNameArabic;
                                        }
                                    }
                                    if (selectedUser.PhoneNumber == null)
                                    {
                                        lmsliteratureborrowdetail.PhoneNumber = "---";
                                    }
                                    else
                                    {
                                        lmsliteratureborrowdetail.PhoneNumber = selectedUser.PhoneNumber;
                                    }
                                    lmsliteratureborrowdetail.EligibleCount = boorowCount;

                                    allowToSaveDisabled(lmsliteratureborrowdetail.EligibleCount);

                                    StateHasChanged();

                                    var responsedata = await lmsLiteratureBorrowDetailService.GetLmsLiteratureBySearchTerm(null, appCulture);
                                    if (responsedata.IsSuccessStatusCode)
                                    {
                                        getBookDetails = (IEnumerable<LiteratureDetailVM>)responsedata.ResultData;
                                    }
                                    else
                                    {
                                        await invalidRequestHandlerService.ReturnBadRequestNotification(responsedata);
                                    }
                                }

                                #endregion

                                else
                                {
                                    dialogService.Close(lmsliteratureborrowdetail);
                                    navigationManager.NavigateTo("/lmsliteratureborrowdetail-list");
                                    //await Load();
                                }
                            }
                        }

                        await GetUserLiteratures(selectedUser.Id);
                    }
                    #endregion

                    #region cancel Borrow Detail scenario

                    else
                    {
                        dialogResponse = await dialogService.Confirm(
                            translationState.Translate("Sure_Cancel"),
                            translationState.Translate("Confirm_Cancel"),
                            new ConfirmOptions()
                            {
                                CloseDialogOnOverlayClick = true,
                                OkButtonText = @translationState.Translate("OK"),
                                CancelButtonText = @translationState.Translate("Cancel")
                            });

                        if (dialogResponse == true)
                            await Load();
                    }

                    #endregion
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Unable_to_add_borrower"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        #endregion

        #region Cancel Form
        protected void ButtonCancelClick(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/lmsliteratureborrowdetail-list");
        }

        #endregion

        #region FUNCTIONS

        //Function to disable Save button based on Eligible Count
        protected void allowToSaveDisabled(int eligible)
        {
            if (selectedUser.DepartmentId == 0)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Info,
                    Detail = translationState.Translate("No_Sector_and_department_against_this_user"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                isAllowToSaveDisabled = true;
            }
            else
            {
                if (selectedUser.DepartmentId == (int)DepartmentEnum.Operational)
                {
                    if (eligible >= 3)
                    {
                        isAllowToSaveDisabled = true;
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Info,
                            Detail = translationState.Translate("Borrow_Book_Limit"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                    else
                        isAllowToSaveDisabled = false;
                }
                else if (selectedUser.DepartmentId == (int)DepartmentEnum.Administrative)
                {
                    if (eligible >= 2)
                    {
                        isAllowToSaveDisabled = true;
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Info,
                            Detail = translationState.Translate("Borrow_Book_Limit"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                    else
                        isAllowToSaveDisabled = false;
                }
            }
        }

        //Function for IssueDate and DueDate based on selection 
        public async Task OnChangeIssueDate(DateTime? date)
        {
            DateTime nowdate;
            if (date == null)
                nowdate = DateTime.Now;
            else
                nowdate = (DateTime)date;

            lmsliteratureborrowdetail.IssueDate = nowdate;
            lmsliteratureborrowdetail.DueDate = nowdate.AddDays(14);

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
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

        #region AdvanceSearch
        protected async Task SubmitAdvanceSearch()
        {
            if (advancedSearchVM.FromDate > advancedSearchVM.ToDate)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return;
            }
            else
            {
                Keywords = true;
                if (string.IsNullOrEmpty(search))
                {
                    search = "";
                }
                else
                    search = search.ToLower();

                var response = await lmsLiteratureService.GetLmsLiteratures(advancedSearchVM, new Query()
                {
                    Filter = $@"i => (i.LiteratureName != null && i.LiteratureName.ToLower().Contains(@0)) || (i.ISBN != null && i.ISBN.ToLower().Contains(@3)) || (i.IndexNumber != null && i.IndexNumber.ToLower().Contains(@4))",
                    FilterParameters = new object[] { search, search, search, search, search }
                });
                if (response.IsSuccessStatusCode)
                {
                    var resultDetails = (IEnumerable<LiteratureDetailVM>)response.ResultData;
                    getAdvanceSearchDetails = resultDetails.Where(x => x.BookStatus != BookStatus.Draft.ToString() && x.BookStatus != BookStatus.UnAvailable.ToString()).ToList();
                    await InvokeAsync(StateHasChanged);
                    AdvanceSearchGridSHow = true;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                StateHasChanged();
            }
        }
        public async void ResetForm()
        {
            advancedSearchVM = new LiteratureAdvancedSearchVM();
            //OnLoadData();
            //         lmsliteratureborrowdetail.LiteratureName = "";
            //lmsliteratureborrowdetail.ISBN = "";
            //         ShowBarcode = false;
            Keywords = false;
            AdvanceSearchGridSHow = false;
            getAdvanceSearchDetails = new List<LiteratureDetailVM>();
            StateHasChanged();
        }
        //<History Author = 'Hassan Abbas' Date='2022-09-09' Version="1.0" Branch="master">Open Advance search Popup </History>
        protected async Task ToggleAdvanceSearch()
        {
            Visible = !Visible;
            if (!Visible)
            {
                ResetForm();
            }
        }
        #endregion

        protected async Task PopulateAdvancedSearchOptions()
        {
            foreach (LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum item in Enum.GetValues(typeof(LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum)))
            {
                AdvancedSearchOptions.Add(new AdvancedSearchEnumTypes { advancedSearchEnumName = translationState.Translate(item.ToString()), advancedSearchEnumValue = item });
            }
            StateHasChanged();
        }
        public async void OnLoadData()
        {
            try
            {
                var str = "";
                var response = await lmsLiteratureBorrowDetailService.GetLmsLiteratureBySearchTerm(str, appCulture);
                if (response.IsSuccessStatusCode)
                {
                    getBookDetails = (IEnumerable<LiteratureDetailVM>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        protected async Task OnChangeSearchValue()
        {
            spinnerService.Show();
            AdvanceSearchGridSHow = false;
            advancedSearchVM.KeywordsType = null;
            advancedSearchVM.GenericsIntergerKeyword = 0;
            switch ((int)advancedSearchVM.EnumSearchValue)
            {
                case (int)AdvancedSearchDropDownEnum.Barcode:
                    break;

                case (int)AdvancedSearchDropDownEnum.Author_Name:
                    await GetAuthors();
                    break;

                case (int)AdvancedSearchDropDownEnum.Book_Index:
                    await GetLiteratureIndexDetails();
                    break;
                default:
                    break;
            }
            spinnerService.Hide();
            StateHasChanged();
        }

        private async Task GetLiteratureIndexDetails()
        {
            LiteratureIndexeDetails = await lmsLiteratureIndexService.GetLiteratureIndexDetails();
        }

        private async Task GetAuthors()
        {
            try
            {
                var response = await lmsLiteratureService.GetAuthorItems();
                if (response.IsSuccessStatusCode)
                {
                    LmsLiteratureAuthor = (IEnumerable<LmsLiteratureAuthor>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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

        #region Addnace Search Grid Data add in borrow 
        protected async Task AddAdvanceSearchItemAsBorrowDetails(LiteratureDetailVM args)
        {
            try
            {
                if (args != null)
                {
                    if (args.BookStatus.Contains(BookStatus.Borrowable.ToString() + " : 0"))
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Info,
                            Detail = translationState.Translate("Literature_With_Zero_Borrowable"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        return;
                    }
                    else
                    {
                        lmsliteratureborrowdetail.LiteratureId = args.LiteratureId;
                        lmsliteratureborrowdetail.LiteratureName = args.LiteratureName;
                        lmsliteratureborrowdetail.ISBN = args.ISBN;
                        var response = await lmsLiteratureBorrowDetailService.GetBarcodeNumberDetailByusingLiteratureId(args.LiteratureId);
                        if (response.IsSuccessStatusCode)
                        {
                            var barcodeResult = (LiteratureDetailsForBorrowRequestVM)response.ResultData;
                            if (barcodeResult.LiteratureId != 0)
                            {
                                lmsliteratureborrowdetail.literatureDetailsForBorrowRequestModel = new List<LiteratureDetailsForBorrowRequestVM>();

                                lmsliteratureborrowdetail.literatureDetailsForBorrowRequestModel.Add(barcodeResult);
                                lmsliteratureborrowdetail.literatureDetailsForBorrowRequestModel.FirstOrDefault().BorrowDate = lmsliteratureborrowdetail.IssueDate;
                                lmsliteratureborrowdetail.literatureDetailsForBorrowRequestModel.FirstOrDefault().ReturnDate = lmsliteratureborrowdetail.DueDate;
                                //lmsliteratureborrowdetail.BarCodeNumber = barcodeResult.BarCodeNumber;
                                //lmsliteratureborrowdetail.BarcodeId = barcodeResult.BarcodeId;
                                ShowBarcode = true;
                            }
                        }
                        Keywords = false;
                        AdvanceSearchGridSHow = false;
                        Visible = false;
                        await InvokeAsync(StateHasChanged);
                    }
                }
                AdvanceSearchGridSHow = false;
                getAdvanceSearchDetails = new List<LiteratureDetailVM>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
