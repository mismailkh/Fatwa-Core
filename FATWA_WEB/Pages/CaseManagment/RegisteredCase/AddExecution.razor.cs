using FATWA_DOMAIN.Interfaces.CaseManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using Microsoft.AspNetCore.Components;
using Radzen;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    public partial class AddExecution : ComponentBase
    {
        #region Parameter

        [Parameter]
        public dynamic? ExecutionRequestId { get; set; } 
        [Parameter]
        public dynamic? CaseId { get; set; }
        [Parameter]
        public dynamic? DecisionId { get; set; }
        [Parameter]
        public dynamic? ExecutionId { get; set; }
        #endregion

        #region Variables
        private readonly ICmsCaseFile _ICmsCaseFile;

        protected MojExecutionRequest executionRequest { get; set; }

         protected CmsJudgmentExecution cmsJudgmentExecution { get; set; } = new CmsJudgmentExecution() { Id = Guid.NewGuid(),  FileOpeningDate = DateTime.Now, FileStatusId = 0 };
        protected List<CmsExecutionFileStatus> cmsExecutionFileStatuses = new List<CmsExecutionFileStatus>();
        protected List<CasePartyLinkExecutionVM> caseParties = new List<CasePartyLinkExecutionVM>();

        #endregion

        #region OnInitialized
        //<History Author = 'Danish' Date='2022-01-04' Version="1.0" Branch="master">OnInitialized</History>
        protected override async Task OnInitializedAsync()
        {
            await PopulateExecutionFileStatus();
            if(ExecutionId != null)
            {
                var response = await cmsRegisteredCaseService.GetExecutionById((Guid)ExecutionId);
                if (response.IsSuccessStatusCode)
                {
                    cmsJudgmentExecution = (CmsJudgmentExecution)response.ResultData;
                    if(cmsJudgmentExecution != null)
                    {
                        cmsJudgmentExecution.IsUpdated = true;
                        await LoadCaseParties((Guid)cmsJudgmentExecution.CaseId);
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            if (ExecutionRequestId != null && ExecutionRequestId != Guid.Empty)
            {
                var result = await cmsRegisteredCaseService.GetExecutionRequestById((Guid)ExecutionRequestId);

                if (result.IsSuccessStatusCode)
                {
                    executionRequest = (MojExecutionRequest)result.ResultData;

                    await LoadCaseParties((Guid)executionRequest.CaseId);

                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(result);
                }
            }
        }
        #endregion


        #region Form Submit
        //<History Author = 'Danish' Date='2022-01-04' Version="1.0" Branch="master"> Form Submit</History>
        protected async Task Form0Submit(CmsJudgmentExecution cmsJudgmentExecution)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    ApiCallResponse response = null;
                    if(ExecutionRequestId != null)
                    {
						executionRequest.ModifiedBy = loginState.Username;
						executionRequest.SectorTypeId = loginState.UserDetail.SectorTypeId != null ? (int)loginState.UserDetail.SectorTypeId : 0;

						cmsJudgmentExecution.ExecutionRequestId = (Guid)ExecutionRequestId;
                        cmsJudgmentExecution.ExecutionRequest = executionRequest;
                        cmsJudgmentExecution.CaseId = (Guid)executionRequest.CaseId;

                    }
                    else
                    {
                        if (DecisionId != null && CaseId !=null && !cmsJudgmentExecution.IsUpdated)
                        {
                            cmsJudgmentExecution.DecisionId = (Guid)DecisionId;
                            cmsJudgmentExecution.CaseId = (Guid)CaseId;
                        }
                    }
					response = await cmsRegisteredCaseService.AddJudgmentExecution(cmsJudgmentExecution);
					if (response.IsSuccessStatusCode)
                    {
                        await SaveTempAttachementToUploadedDocument();
                        await CopyAttachmentsFromSourceToDestination();
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Execution_Added_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        dialogService.Close(cmsJudgmentExecution);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected async Task SaveTempAttachementToUploadedDocument()
        {
            try
            {
                var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                {
                    RequestIds = new List<Guid> { cmsJudgmentExecution.Id },
                    CreatedBy = cmsJudgmentExecution.CreatedBy,
                    FilePath = _config.GetValue<string>("dms_file_path"),
                    DeletedAttachementIds = null
                });

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
            }
            catch (Exception)
            {
                return;
                throw;
            }
        }
        protected async Task CopyAttachmentsFromSourceToDestination()
        {
            try
            {
                var docResponse = await fileUploadService.CopyAttachmentsFromSourceToDestination(new List<CopyAttachmentVM> { new CopyAttachmentVM()
                    {
                        SourceId = cmsJudgmentExecution.Id,
                        DestinationId = (Guid)cmsJudgmentExecution.CaseId,
                        CreatedBy = cmsJudgmentExecution.CreatedBy
                    }});
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
            }
            catch (Exception)
            {
                return;
                throw;
            }
        }
        #endregion

        private async Task LoadCaseParties(Guid caseId)
        {
            try
            {
                var response  = await cmsRegisteredCaseService.GetCasePartiesForExecution(caseId);
                if (response.IsSuccessStatusCode)
                {
                    caseParties = (List<CasePartyLinkExecutionVM>)response.ResultData;
                   
                }
                else
                {
                   await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
           

        }


        protected async Task PopulateExecutionFileStatus()
        {
            var response = await cmsRegisteredCaseService.GetExecutionFileStatus();
            if (response.IsSuccessStatusCode)
            {
                cmsExecutionFileStatuses = (List<CmsExecutionFileStatus>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        void CalculateAmount()
        {
            if(cmsJudgmentExecution.Amount != null && cmsJudgmentExecution.PaidAmount != null)
            cmsJudgmentExecution.FileBalance = cmsJudgmentExecution.Amount - cmsJudgmentExecution.PaidAmount;
        }
    }
}
