using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;

namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    public partial class ApproveRejectCaseFileByLawyer : ComponentBase
    {
        #region Parameters

        [Parameter]
        public Guid ReferenceId { get; set; }
        [Parameter]
        public string TaskId { get; set; }
   
        #endregion

        #region Variables

        public CmsCaseFileDetailVM caseFile { get; set; } = new CmsCaseFileDetailVM();
        protected TaskDetailVM taskDetailVM = new TaskDetailVM();
        #endregion

        #region On Load

        protected override async Task OnInitializedAsync()
        {
            var result = await cmsCaseFileService.GetCaseFileDetailByIdVM(ReferenceId);
            if (result.IsSuccessStatusCode)
            {
                caseFile = (CmsCaseFileDetailVM)result.ResultData;
                await PopulateTaskDetails();
            }

        }

        protected async Task PopulateTaskDetails()
        {
            if (TaskId != null)
            {
                var getTaskDetail = await taskService.GetTaskDetailById(Guid.Parse(TaskId));
                if (getTaskDetail.IsSuccessStatusCode)
                {
                    taskDetailVM = (TaskDetailVM)getTaskDetail.ResultData;
                }
                else
                {
                    taskDetailVM = new TaskDetailVM();
                }
            }
        }
        #endregion

        #region Functions
        protected async Task ApproveCaseFile()
        {
            caseFile.LawyerId = loginState.UserDetail.UserId;
            caseFile.SectorTypeId = loginState.UserDetail.SectorTypeId;
            var response = await cmsSharedService.ApproveCaseFile(caseFile);
            if (response.IsSuccessStatusCode)
            {
                if (TaskId != null)
                {
                    if (taskDetailVM.Url.StartsWith("casefile-review-assignment"))
                    {
                        taskDetailVM.Url = taskDetailVM.Url.Replace("casefile-review-assignment", "casefile-view");
                    }
                    var taskResponse = await taskService.DecisionTask(taskDetailVM);
                    if (!taskResponse.IsSuccessStatusCode)
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                    }
                }
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Detail = translationState.Translate("Case_File_Approved"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
            StateHasChanged();
            await RedirectBack();
        }
        protected async Task RejectCaseFile(MouseEventArgs args)
        {
            var dialogResult = await dialogService.OpenAsync<AssignBackToHosCaseFile>
                (
                translationState.Translate("Remarks"),
                new Dictionary<string, object>() {
                    { "ReferenceId", ReferenceId},
                    { "TaskId", TaskId }
                },
                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
            if (dialogResult != null)
            {
                await Task.Delay(300);
                await RedirectBack();
            }
        }
        protected void ButtonCancelClick(MouseEventArgs args)
        {
            dialogService.Close(null);
        }

        #endregion

        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");

        }
        protected async Task RedirectToDetail()
        {
            navigationManager.NavigateTo("casefile-view/" + ReferenceId);
        }
    }

}
