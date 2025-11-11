using FATWA_ADMIN.Services.UserManagement;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace FATWA_ADMIN.Pages.UserManagement.Users
{
    public partial class TransferUsers : ComponentBase
    {
        #region Constructor
        public TransferUsers()
        {
            UserDetailsList = new List<UserVM>();
            SelectedUserNameResult = new UserVM();
            TransferDeptDDShow = false;
            TransferToDeptList = new List<Department>();
            transferFieldsValidation = new TransferDetailValidationClass();
            transferUserModel = new TransferUser();
        }
        #endregion

        #region Service Injection

        [Inject]
        protected TransferUserService transferUserServices { get; set; }

        [Inject]
        protected UserService userServices { get; set; }
        #endregion

        #region Variable Declaration

        //public DateTime Max = new DateTime(DateTime.);
        public DateTime Min = new DateTime(DateTime.Now.Date.Ticks);

        protected IEnumerable<UserVM> UserDetailsList { get; set; }
        public UserVM? SelectedUserNameResult { get; private set; }
        public bool TransferDeptDDShow;
        protected List<Department> TransferToDeptList { get; set; }
        UserVM userVM = new UserVM();
        protected TransferDetailValidationClass transferFieldsValidation { get; set; }
        protected TransferUser transferUserModel { get; set; }
        #endregion

        #region Fields validation
        protected class TransferDetailValidationClass
        {
            public string ValidateTransferId { get; set; } = string.Empty;
            public string ValidateTransferStartDate { get; set; } = string.Empty;
            public string ValidateTransferEndDate { get; set; } = string.Empty;
        }
        #endregion

        #region On Component Load

        //<History Author = 'Umer Zaman' Date='2022-07-26' Version="1.0" Branch="master"> get user details operation</History>
        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async Task Load()
        {
            spinnerService.Show();

            UserDetailsList = await userServices.GetUserDetails();
            TransferToDeptList = await transferUserServices.GetAllDepartmentList();
            spinnerService.Hide();
        }
        #endregion

        #region OnChange UserName 
        protected async void ChangeUserName()
        {
            if (!string.IsNullOrEmpty(transferUserModel.Id))
            {
                SelectedUserNameResult = UserDetailsList.Where(x => x.Id == transferUserModel.Id).FirstOrDefault();
                // remove user current depatment from list
                var record = TransferToDeptList.Where(x => x.Id == SelectedUserNameResult.DepartmentId).FirstOrDefault();
                if (record != null)
                {
                    TransferToDeptList.Remove(record);
                }
                TransferDeptDDShow = true;
            }
            else
            {
                SelectedUserNameResult = new UserVM();
                TransferDeptDDShow = false;
                TransferToDeptList = new List<Department>();
            }
        }
        #endregion

        #region Transfer Button Click
        protected async Task SaveTransferButtonClick()
        {
            bool valid = await ValidateTransferFields();
            if (valid)
            {
                if (transferUserModel.TransferEndDate < transferUserModel.TransferStartDate)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Warning,
                        Detail = translationState.Translate("Transfer_Date_Check"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
                transferUserModel.TransferId = Guid.NewGuid();
                if (SelectedUserNameResult != null)
                {
                    transferUserModel.Previous_DepartmentId = SelectedUserNameResult.DepartmentId;
                }
                if (transferUserModel.TransferStartDate != null)
                {
                    transferUserModel.StartDate = (DateTime)transferUserModel.TransferStartDate;
                }
                if (transferUserModel.TransferEndDate != null)
                {
                    transferUserModel.EndDate = (DateTime)transferUserModel.TransferEndDate;
                }

                var response = await transferUserServices.SaveTransferUser(transferUserModel);
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Save_Transfer_Success"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await Task.Delay(300);
                    bool? dialogResponse = await dialogService.Confirm(
                    translationState.Translate("Transfer_Permission_Change_Message"),
                    translationState.Translate("Confirm"),
                    new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("Yes"),
                        CancelButtonText = translationState.Translate("No")
                    });
                    if (dialogResponse == true)
                    {
                        if (!string.IsNullOrEmpty(transferUserModel.Id))
                        {
                            navigationManager.NavigateTo("/save-user/" + transferUserModel.Id);
                        }
                    }
                    else
                    {
                        navigationManager.NavigateTo("/Users");
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Fill_Required_Fields"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        private Task<bool> ValidateTransferFields()
        {
            bool basicDetailsValid = true;
            if (transferUserModel.Current_DepartmentId <= 0)
            {
                transferFieldsValidation.ValidateTransferId = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                transferFieldsValidation.ValidateTransferId = "k-valid";
            }
            if (transferUserModel.TransferStartDate == null)
            {
                transferFieldsValidation.ValidateTransferStartDate = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                transferFieldsValidation.ValidateTransferStartDate = "k-valid";
            }
            if (transferUserModel.TransferEndDate == null)
            {
                transferFieldsValidation.ValidateTransferEndDate = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                transferFieldsValidation.ValidateTransferEndDate = "k-valid";
            }
            return Task.FromResult(basicDetailsValid);
        }
        #endregion
    }
}
