using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;

namespace FATWA_WEB.Pages.Consultation.ConsultationFiles
{



    public partial class ApproveRejectAssignmentConsultationFile : ComponentBase
    {
        #region Parameters

        [Parameter]
        public Guid ReferenceId { get; set; }
        [Parameter]
        public string TaskId { get; set; }
        #endregion

        #region Variables
        public ConsultationFileDetailVM consultationFile { get; set; } = new ConsultationFileDetailVM();
        protected TaskDetailVM taskDetailVM = new TaskDetailVM();
        public int sectorId = 0;
        #endregion

        #region On Load

        protected override async Task OnInitializedAsync()
        {
            var result = await consultationFileService.GetConsultationFileDetailById(ReferenceId);
             sectorId = (int)loginState.UserDetail.SectorTypeId;
            if (result.IsSuccessStatusCode)
            {
                consultationFile = (ConsultationFileDetailVM)result.ResultData;
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
        protected async Task ApproveConsultationFile()
        {
            consultationFile.LawyerId = loginState.UserDetail.UserId;
            var response = await comsSharedService.ApproveConsultationFile(consultationFile);
            if (response.IsSuccessStatusCode)
            {
                if (TaskId != null)
                {
                    if (taskDetailVM.Url.StartsWith("consultationfile-review-assignment"))
                    {
                        taskDetailVM.Url =  "consultationfile-view/" + ReferenceId + "/" + sectorId ;
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
        protected async Task RejectConsultationFile(MouseEventArgs args)
        {
            var dialogResult = await dialogService.OpenAsync<AssignBackToHosConsultationFile>
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
