using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using Microsoft.AspNetCore.Components;
using Radzen;
using static FATWA_DOMAIN.Enums.DmsEnums;

namespace FATWA_WEB.Pages.DS
{
    public partial class SendForSigning : ComponentBase
    {
        #region Parameter
        [Parameter]
        public TempAttachementVM Document { get; set; }
        [Parameter]
        public int SubModuleId { get; set; }
        #endregion

        #region Variables
        protected IEnumerable<EmployeeVMForDropDown> employeeList = new List<EmployeeVMForDropDown>();
        protected DsSigningRequestTaskLog taskForDS = new DsSigningRequestTaskLog();
        protected AttachmentType attachmentType = new AttachmentType();
        protected DsSigningRequestTaskLogVM task = new DsSigningRequestTaskLogVM();
        protected List<DsSigningRequestTaskLogVM> taskList = new List<DsSigningRequestTaskLogVM>();
        protected string userValidationMsg = "";
        protected bool notValidUser = false;
        protected bool HasEmpValue = false;
        protected bool HasRemarksValue = false;
        #endregion


        #region Component Load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await GetAllTasksForSignature();
            if (Document.StatusId == (int)SigningTaskStatusEnum.SendForSigning)
            {
                task = taskList.OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            }
            else
            {
                await Load();
                await GetAttachmentTypeById();
            }
            spinnerService.Hide();
        }
        #endregion
        protected async Task Load()
        {
            var response = await userService.GetEmployeeList((int)loginState.UserDetail.SectorTypeId, (int)Document.AttachmentTypeId, (int)Document.UploadedDocumentId);
            if (response.IsSuccessStatusCode)
            {
                employeeList = (IEnumerable<EmployeeVMForDropDown>)response.ResultData;
                employeeList = employeeList.Where(x => x.UserId != loginState.UserDetail.UserId).ToList();
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        protected async Task GetAllTasksForSignature()
        {
            var response = await fileUploadService.GetAllTasksForSignature((int)Document.UploadedDocumentId);
            if (response.IsSuccessStatusCode)
            {
                taskList = (List<DsSigningRequestTaskLogVM>)response.ResultData;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        protected async Task GetAttachmentTypeById()
        {
            var response = await lookupService.GetDocumentTypeById((int)Document.AttachmentTypeId);
            if (response.IsSuccessStatusCode)
            {
                attachmentType = (AttachmentType)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        public void OnUserChange(object value)
        {
            notValidUser = taskList.Where(x => x.ReceiverId == (string)value && x.StatusId == (int)SigningTaskStatusEnum.Signed).Any();
            userValidationMsg = !notValidUser ? "" : translationState.Translate("User_Already_signed");
        }

        public async Task Submit()
        {
            HasRemarksValue = string.IsNullOrEmpty(taskForDS.Remarks);
            HasEmpValue = string.IsNullOrEmpty(taskForDS.ReceiverId);
            if (HasEmpValue || HasRemarksValue) return;

            if (await dialogService.Confirm(
                translationState.Translate("Sure_Submit"),
                translationState.Translate("Confirm"),
                new ConfirmOptions()
                {
                    OkButtonText = @translationState.Translate("Yes"),
                    CancelButtonText = @translationState.Translate("No")
                }) == true)
            {
                taskForDS.SigningTaskId = Guid.NewGuid();
                taskForDS.ReferenceId = Document.ReferenceGuid;
                taskForDS.DocumentId = (int)Document.UploadedDocumentId;
                taskForDS.SenderId = loginState.UserDetail.UserId;
                taskForDS.SectorTypeId = (int)loginState.UserDetail.SectorTypeId;
                taskForDS.ModuleId = (int)attachmentType.ModuleId;
                taskForDS.StatusId = (int)SigningTaskStatusEnum.SendForSigning;
                taskForDS.SubModuleId = SubModuleId;
                taskForDS.CreatedBy = loginState.Username;
                taskForDS.CreatedDate = DateTime.Now;
                var response = await fileUploadService.CreateTaskForSignature(taskForDS);
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Send_Task_For_Signing"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    dialogService.Close(true);
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
        }
        public async Task Cancel()
        {
            dialogService.Close(true);
        }

    }
}
