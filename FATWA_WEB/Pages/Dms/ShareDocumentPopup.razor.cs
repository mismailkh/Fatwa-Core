using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_WEB.Pages.Dms
{
    public partial class ShareDocumentPopup : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic DocumentId { get; set; }
        [Parameter]
        public bool IsConfidential { get; set; }
        #endregion

        #region variable declaration
        public DmsSharedDocument dmsSharedDocument { get; set; } = new DmsSharedDocument();
        protected IEnumerable<UserVM> users { get; set; }
        protected IEnumerable<Department> department { get; set; }
        public UploadedDocument copydocumnet { get; set; } = new UploadedDocument();
        public int DepartmentId { get; set; }
        #endregion

        #region ON INITIALIZED
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            if (loginState.UserDetail.SectorTypeId > 0)
            {
                await PopulateUsersListBySector();
            }
            await PopulateDepartmentList();
            spinnerService.Hide();
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

        #region Populate Lists
        protected async Task PopulateUsersListBySector()
        {
            var userresponse = await lookupService.GetUsersBySector(loginState.UserDetail.SectorTypeId);
            if (userresponse.IsSuccessStatusCode)
            {
                users = (IEnumerable<UserVM>)userresponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(userresponse);
            }
        }
        protected async Task PopulateDepartmentList()
        {
            var userresponse = await lookupService.GetDepartments();
            if (userresponse.IsSuccessStatusCode)
            {
                department = (IEnumerable<Department>)userresponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(userresponse);
            }
        }
        protected async Task PopulateUsersListByDepartment()
        {
            var userresponse = await lookupService.GetUsersByDepartment(DepartmentId);
            if (userresponse.IsSuccessStatusCode)
            {
                users = (IEnumerable<UserVM>)userresponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(userresponse);
            }
        }
        #endregion

        #region On Change

        protected async Task OnChangeDepId(object args)
        {
            if (args != null)
            {
                await PopulateUsersListByDepartment();
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("No_User_Of_Department"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }


        }
        #endregion

        #region SUBMIT BUTTON
        protected async Task FormSubmit(DmsSharedDocument args)
        {
            bool? dialogResponse;
            if (IsConfidential == false)
            {
                dialogResponse = await dialogService.Confirm(
                translationState.Translate("Sure_Share_Document"),
                translationState.Translate("Confirm"),
                new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                });
            }
            else
            {
                dialogResponse = await dialogService.Confirm(
                translationState.Translate("Sure_Share_Confidential_Document"),
                translationState.Translate("Confirm"),
                new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                });
            }

            if (dialogResponse == true)
            {
                var docResponse = await fileUploadService.AddCopyAttachments(Convert.ToInt32(DocumentId));
                var copyDocId = docResponse.ResultData;
                if (!docResponse.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Attachment_Save_Failed"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }

                spinnerService.Show();
                args.DocumentId = copyDocId;
                args.Id = Guid.NewGuid();
                args.SenderId = loginState.UserDetail.UserId;
                args.RecieverId = dmsSharedDocument.RecieverId;
                args.CreatedBy = loginState.UserDetail.Email;
                args.CreatedDate = DateTime.Now;
                args.Notes = dmsSharedDocument.Notes;


                var response = await fileUploadService.ShareDocument(args);
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Document_Shared_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    spinnerService.Hide();
                    dialogService.Close();
                    return;

                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }

            }




        }
        #endregion

        #region cancel button

        protected async Task ButtonCloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }

        #endregion

    }
}
