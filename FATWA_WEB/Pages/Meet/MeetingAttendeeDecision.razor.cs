using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using SelectPdf;
using Syncfusion.Blazor.PdfViewerServer;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.MeetingEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;

namespace FATWA_WEB.Pages.Meet
{
    public partial class MeetingAttendeeDecision : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic meetingId { get; set; }
        [Parameter]
        public dynamic TaskId { get; set; }
        [Parameter]
        public dynamic CommunicationId { get; set; }
        #endregion

        #region Variables Declaration
        protected List<MeetingAttendeeStatus> attendeeDecisionStatus = new List<MeetingAttendeeStatus>();
        protected AttendeeDecisionVM meetingDecisionVM = new AttendeeDecisionVM();
        protected string govtEntityId;
        protected RadzenHtmlEditor editor = new RadzenHtmlEditor();
        protected bool isMomAttendeeDecision = false;
        protected bool isRequestForMeetingAttendeeDecision = false;


        public class MeetingStatuses
        {
            public int MeetingStatusId { get; set; }
            public string NameEn { get; set; }
        }
        protected TaskDetailVM taskDetailVM { get; set; } = new TaskDetailVM();
        protected List<CaseTemplate> HeaderFooterTemplates { get; set; }
        public TelerikPdfViewer PdfViewerRef { get; set; }
        public byte[]? FileData { get; set; }
        protected List<ManagerTaskReminderVM> managerTaskReminderData { get; set; } = new List<ManagerTaskReminderVM>();
        public SfPdfViewerServer pdfViewer;
        public string DocumentPath { get; set; } = string.Empty;
        #endregion

        #region OnInitialized

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }

        protected async Task Load()
        {
            try
            {
                if (TaskId != null)
                {
                    await PopulateTaskDetails();
                    await GetManagerTaskReminderData();
                }
                await PopulateMeetingAttendeeStatus();
                await PopulateHeaderFooter();
                if (taskDetailVM.TaskId != Guid.Empty && taskDetailVM.Url != null)
                {
                    if (taskDetailVM.Url.StartsWith("mom-attendeedecision"))
                    {
                        isRequestForMeetingAttendeeDecision = false;
                        isMomAttendeeDecision = true;
                    }
                    else if (taskDetailVM.Url.StartsWith("requestformeeting-attendeedecision"))
                    {
                        isMomAttendeeDecision = false;
                        isRequestForMeetingAttendeeDecision = true;
                    }
                }
                var UserId = loginState.UserDetail.UserId;
                var getDecision = await meetingService.GetMeetingAttendeeDecisionById(Guid.Parse(meetingId), UserId, isMomAttendeeDecision);
                if (getDecision.IsSuccessStatusCode)
                {
                    meetingDecisionVM = (AttendeeDecisionVM)getDecision.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(getDecision);
                }
                await PopulatePdfFromHtml();

            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

        #region Remote Dropdown Data and Dropdown Change Events
        protected async Task PopulateMeetingAttendeeStatus()
        {
            var response = await lookupService.GetAttendeeMeetingStatus();
            if (response.IsSuccessStatusCode)
            {
                var attendeeStatus = (List<MeetingAttendeeStatus>)response.ResultData;
                foreach (MeetingAttendeeStatus item in attendeeStatus)
                {
                    if (item.Id == (int)MeetingAttendeeStatusEnum.Accept || item.Id == (int)MeetingAttendeeStatusEnum.Reject)
                    {
                        attendeeDecisionStatus.Add(new MeetingAttendeeStatus
                        {
                            Id = item.Id,
                            NameEn = item.NameEn,
                            NameAr = item.NameAr,
                        });
                    }
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        protected async Task PopulateTaskDetails()
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

        protected async Task PopulateHeaderFooter()
        {
            var response = await cmsCaseTemplateService.GetHeaderFooter();
            if (response.IsSuccessStatusCode)
            {
                HeaderFooterTemplates = (List<CaseTemplate>)response.ResultData;
            }
        }
        #endregion

        #region Get Govt Entity Id By ReferencId
        protected async Task GetGovtEnityByReferencId(Guid ReferenceId, int SubModuleId)
        {
            var response = await cmsSharedService.GetGovtEnityByReferencId(ReferenceId, SubModuleId);
            if (response.IsSuccessStatusCode)
            {
                govtEntityId = (string)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Pdf Viewer
        protected async Task PopulatePdfFromHtml()
        {
            try
            {
                HtmlToPdf converter = new HtmlToPdf();
                MemoryStream stream = new MemoryStream();

                converter.Options.DisplayHeader = true;
                converter.Options.DisplayFooter = true;
                converter.Header.DisplayOnFirstPage = true;
                converter.Header.DisplayOnOddPages = true;
                converter.Header.DisplayOnEvenPages = true;
                converter.Header.Height = 100;
                converter.Footer.Height = 50;
                converter.Footer.DisplayOnFirstPage = true;
                converter.Footer.DisplayOnOddPages = true;
                converter.Footer.DisplayOnEvenPages = true;
                string headerHtmlContent = HeaderFooterTemplates.Where(x => x.Id == (Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? (int)CaseTemplateEnum.HeaderEn : (int)CaseTemplateEnum.HeaderAr)).Select(x => x.Content).FirstOrDefault();
                string footerHtmlContent = HeaderFooterTemplates.Where(x => x.Id == (int)CaseTemplateEnum.Footer).Select(x => x.Content).FirstOrDefault();
                PdfHtmlSection headerHtml = new PdfHtmlSection(headerHtmlContent, "");
                PdfHtmlSection footerHtml = new PdfHtmlSection(footerHtmlContent, "");
                headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                converter.Header.Add(headerHtml);
                converter.Footer.Add(footerHtml);
                // set converter options
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                converter.Options.WebPageWidth = 1024;
                converter.Options.WebPageHeight = 1024;
                converter.Options.MarginRight = 30;
                converter.Options.MarginLeft = 30;
                converter.Options.MarginRight = 30;
                converter.Options.MarginLeft = 30;
                converter.Options.EmbedFonts = true;
                meetingDecisionVM.MOMContent = string.Concat($"<style>@font-face {{font-family: 'Sultan';src: url('{Directory.GetCurrentDirectory().Replace("\\", "/") + "/wwwroot/fonts/Sultan/arfonts-sultan-normal.ttf"}') format('truetype');font-weight: normal;font-style: normal;font-display: swap;}} @font-face {{font-family: 'Sultan Medium';src: url('{Directory.GetCurrentDirectory().Replace("\\", "/") + "/wwwroot/fonts/Sultan/arfonts-sultan-mudaim.ttf"}') format('truetype');font-weight: normal;font-style: normal;font-display: swap;}}</style>", meetingDecisionVM.MOMContent);
                SelectPdf.PdfDocument pdfDocument = converter.ConvertHtmlString(meetingDecisionVM.MOMContent != null ? meetingDecisionVM.MOMContent : string.Empty);

                pdfDocument.Save(stream);
                pdfDocument.Close();
                stream.Close();
                //PreviewFileData = stream.ToArray();
                FileData = stream.ToArray();
                string base64String = Convert.ToBase64String(FileData);
                DocumentPath = "data:application/pdf;base64," + base64String;
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
        #endregion

        #region Submit Form

        protected async Task FormSubmit(AttendeeDecisionVM args)
        {
            try
            {
                var UserId = loginState.UserDetail.UserId;
                args.LoggedInUser = loginState.UserDetail.Email;
                bool? dialogResponse = await dialogService.Confirm(
                   translationState.Translate("Are_you_sure_you_want_to_save_this_change"),
                   translationState.Translate("Confirm"),
                   new ConfirmOptions()
                   {
                       OkButtonText = @translationState.Translate("OK"),
                       CancelButtonText = @translationState.Translate("Cancel")
                   });
                if (dialogResponse == true)
                {
                    //if (meetingDecisionVM.MeetingStatusId == (int)MeetingStatusEnum.ApprovedByHOS)
                    //{
                    //	meetingDecisionVM.IsApproved = true;
                    //}
                    meetingDecisionVM.SectorTypeId = loginState.UserDetail.SectorTypeId;
                    //meetingDecisionVM.SentBy = loginState.Username;
                    //meetingDecisionVM.MeetingId = Guid.Parse(meetingId);
                    //meetingDecisionVM.GovtEntityId = Convert.ToInt32(govtEntityId);
                    if (isMomAttendeeDecision && meetingDecisionVM.AttendeeStatusId == (int)MeetingAttendeeStatusEnum.Reject && string.IsNullOrEmpty(meetingDecisionVM.MOMAttendeeRejectReason))
                    {
                        return;
                    }
                    var response = await meetingService.UpdateMeetingAttendeeDecision(meetingDecisionVM, UserId, isMomAttendeeDecision);
                    if (response.IsSuccessStatusCode)
                    {
                        if (TaskId != null)
                        {
                            if (taskDetailVM.Url.StartsWith("meeting-attendeedecision"))
                            {
                                taskDetailVM.Url = $"meeting-view/{meetingId}/{true}";
                            }
                            else if (taskDetailVM.Url.StartsWith("requestformeeting-attendeedecision"))
                            {
                                taskDetailVM.Url = $"meeting-view/{meetingId}/{true}";
                            }
                            taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                            taskDetailVM.Name = "Save_Meeting_Task";
                            var taskResponse = await taskService.DecisionTask(taskDetailVM);
                            if (!taskResponse.IsSuccessStatusCode)
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                            }
                        }

                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Decicion_Save_Success"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
                        //await RedirectBack();
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

        protected async Task ButtonCancelClick()
        {
            await JSRuntime.InvokeVoidAsync("history.back");
            //navigationManager.NavigateTo("/meeting-list");
        }
        private async Task PreviousPageUrl()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        protected void ButtonDetail()
        {
            if (meetingDecisionVM.SubModulId == (int)SubModuleEnum.CaseRequest)
            {
                navigationManager.NavigateTo("/caserequest-view/" + meetingDecisionVM.ReferenceGuid);
            }
            if (meetingDecisionVM.SubModulId == (int)SubModuleEnum.CaseFile)
            {
                navigationManager.NavigateTo("/casefile-view/" + meetingDecisionVM.ReferenceGuid);
            }
            if (meetingDecisionVM.SubModulId == (int)SubModuleEnum.RegisteredCase)
            {
                navigationManager.NavigateTo("/case-view/" + meetingDecisionVM.ReferenceGuid);
            }
            if (meetingDecisionVM.SubModulId == (int)SubModuleEnum.ConsultationRequest)
            {
                meetingDecisionVM.SectorTypeId = loginState.UserDetail.SectorTypeId;
                navigationManager.NavigateTo("/consultationrequest-detail/" + meetingDecisionVM.ReferenceGuid + "/" + meetingDecisionVM.SectorTypeId);
            }
            if (meetingDecisionVM.SubModulId == (int)SubModuleEnum.ConsultationFile)
            {
                meetingDecisionVM.SectorTypeId = loginState.UserDetail.SectorTypeId;
                navigationManager.NavigateTo("/consultationfile-view/" + meetingDecisionVM.ReferenceGuid + "/" + meetingDecisionVM.SectorTypeId);
            }
        }

        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion
        #region Get Manager Task Reminder Data
        protected async Task GetManagerTaskReminderData()
        {
            try
            {
                var response = await lookupService.GetManagerTaskReminderData(Guid.Parse(TaskId));
                if (response.IsSuccessStatusCode)
                {
                    managerTaskReminderData = (List<ManagerTaskReminderVM>)response.ResultData;
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
        #endregion
    }
}
