using FATWA_DOMAIN.Models.LegalPrinciple;
using FATWA_DOMAIN.Models.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.LegalLegislationEnum;
using static FATWA_DOMAIN.Enums.LegalPrinciple.LegalPrincipleEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;

namespace FATWA_WEB.Pages.LLSLegalPrinciple
{
    public partial class LLSLegalPrincipleDecision : ComponentBase
    {

        #region Parameter
        [Parameter]
        public dynamic PrincipleId { get; set; }
        [Parameter]
        public dynamic FromPage { get; set; }
        #endregion

        #region Variables
        protected LLSLegalPrinciplesReviewVM LLSLegalPrincipleDetailVM { get; set; } = new LLSLegalPrinciplesReviewVM();
        protected LLSLegalPrincipleDecisionVM legalPrincipleDecisionVM { get; set; } = new LLSLegalPrincipleDecisionVM();
        protected List<LegalPrincipleFlowStatus> excludedFlowStatus { get; set; } = new List<LegalPrincipleFlowStatus>();
        protected List<LegalPrincipleFlowStatus> legalPrincipleFlowStatus { get; set; } = new List<LegalPrincipleFlowStatus>();
        protected LLSLegalPrincipleSystem lLSLegalPrincipleSystem { get; set; } = new LLSLegalPrincipleSystem();
        protected RadzenDataGrid<LLSLegalPrinciplesContentVM> gridRelation { get; set; }
        public List<LLSLegalPrinciplesContentVM> lLSLegalPrinciplesContentVMs { get; set; } = new List<LLSLegalPrinciplesContentVM>();


        #endregion

        #region On Initialize
        protected override async Task OnInitializedAsync()
        {

            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }
        #endregion

        #region Load
        protected async Task Load()
        {
            await PopulatePrincipleFlowStatus();
            await GetLLSLegalPrincipleDetailById();
            await ChangeApprovalTypeForAddingDecision(legalPrincipleFlowStatus);
            await GetLLSLegalPrincipleContentListByPrincipleId();
            //await PopulateLLSLegalPrincipleReferences();
        }
        #endregion

        #region Functions

        private async Task GetLLSLegalPrincipleDetailById()
        {
            var response = await lLSLegalPrincipleService.GetLegalPrincipleDetailById(Guid.Parse(PrincipleId));

            if (response.IsSuccessStatusCode)
            {
                LLSLegalPrincipleDetailVM = (LLSLegalPrinciplesReviewVM)response.ResultData;
            }
            else
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        }


        private async Task GetLLSLegalPrincipleContentListByPrincipleId()
        {
            try
            {
                var response = await lLSLegalPrincipleService.GetLLSLegalPrincipleContentDetailsByUsingPrincipleId(Guid.Parse(PrincipleId));
                if (response.IsSuccessStatusCode)
                {
                    lLSLegalPrinciplesContentVMs = (List<LLSLegalPrinciplesContentVM>)response.ResultData;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected async Task PopulatePrincipleFlowStatus()
        {
            var response = await lLSLegalPrincipleService.GetPrincipleFlowStatusDetails();
            if (response.IsSuccessStatusCode)
            {
                legalPrincipleFlowStatus = (List<LegalPrincipleFlowStatus>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
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
     

        private async Task RedirectUrl()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }


        protected async Task ViewDetail(LLSLegalPrinciplesContentVM args)
        {
            var result = await dialogService.OpenAsync<DetailLLSLegalPrincipleContent>(translationState.Translate("Legal_Principle_Detail"),
            new Dictionary<string, object>()
            {
                { "PrincipleContentId", args.PrincipleContentId},
                { "IsPrincipleContent", true },
            },
            new DialogOptions() { Width = "100% !important", CloseDialogOnOverlayClick = true }
        );
            await Task.Delay(200);
            await Load();
            StateHasChanged();
        }
        #endregion

        #region Submit Form
        protected async Task FormSubmit(LLSLegalPrincipleDecisionVM args)
        {
            try
            {
                args.PrincipleId = Guid.Parse(PrincipleId);
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
                    var response1 = await lLSLegalPrincipleService.GetLegalPrincipleDetailsByUsingPrincipleId(args.PrincipleId);
                    if (response1.IsSuccessStatusCode)
                    {
                        lLSLegalPrincipleSystem = (LLSLegalPrincipleSystem)response1.ResultData;
                    }

                    lLSLegalPrincipleSystem.FlowStatus = (int)args.FlowStatusId;
                    args.ReceiverEmail = lLSLegalPrincipleSystem.CreatedBy;
                    args.PrincipleNumber = lLSLegalPrincipleSystem.PrincipleNumber;
                    var response = await lLSLegalPrincipleService.UpdateLegalPrincipleDecision(args);
                    if (args.FlowStatusId != (int)PrincipleFlowStatusEnum.Unpublished)
                    {
                        lLSLegalPrincipleSystem.SenderEmail = loginState.UserDetail.UserName;
                        await workflowService.ProcessWorkflowActvivities(lLSLegalPrincipleSystem, (int)WorkflowModuleEnum.LPSPrinciple, (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple);
                    }
                    if (response.IsSuccessStatusCode)
                    {
                        if (FromPage == "1") // approve, need to modify
                        {
                            if (args.FlowStatusId == (int)PrincipleFlowStatusEnum.NeedToModify)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("legislation_NeedToModify_Success_Message"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                            }
                            else if (args.FlowStatusId == (int)PrincipleFlowStatusEnum.Approve)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("legislation_Approved_Success_Message"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                            }
                            await RedirectBack();
                        }
                        else if (FromPage == "2") // publish, need to modify
                        {
                            if (args.FlowStatusId == (int)LegislationFlowStatusEnum.NeedToModify)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("legislation_NeedToModify_Success_Message"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                            }
                            else if (args.FlowStatusId == (int)LegislationFlowStatusEnum.Published)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("legislation_Published_Success_Message"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                            }
                            await RedirectBack();
                        }
                        else if (FromPage == "3") // unpublish
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("legislation_UnPublished_Success_Message"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            await RedirectBack();
                        }

                        StateHasChanged();
                    }

                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
                else
                {
                    await Load();
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Decision_could_not_be_updated"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }
        #endregion

        #region Validation
        protected bool isCommentRequired { get; set; } = false;
        protected async void OnChangeAddComment()
        {
            if (legalPrincipleDecisionVM.FlowStatusId == (int)PrincipleFlowStatusEnum.SendAComment ||
                legalPrincipleDecisionVM.FlowStatusId == (int)PrincipleFlowStatusEnum.NeedModification || 
                legalPrincipleDecisionVM.FlowStatusId == (int)PrincipleFlowStatusEnum.Approve ||  
                legalPrincipleDecisionVM.FlowStatusId == (int)PrincipleFlowStatusEnum.Publish || 
                legalPrincipleDecisionVM.FlowStatusId == (int)PrincipleFlowStatusEnum.Unpublished)
            {
                isCommentRequired = true;
            }
            else
            {
                isCommentRequired = false;
            }
            StateHasChanged();
        }

        #endregion

        #region Populate Function 
        private async Task ChangeApprovalTypeForAddingDecision(IEnumerable<LegalPrincipleFlowStatus> legalPrincipleFlowStatus)
        {
            try
            {
                if (LLSLegalPrincipleDetailVM.FlowStatusId == (int)PrincipleFlowStatusEnum.InReview)
                {
                    excludedFlowStatus = legalPrincipleFlowStatus.Where(x => x.Id == (int)PrincipleFlowStatusEnum.Approve || x.Id == (int)PrincipleFlowStatusEnum.NeedToModify).ToList();
                    await ChangeFlowStatusName(excludedFlowStatus);
                }
                else if (LLSLegalPrincipleDetailVM.FlowStatusId == (int)PrincipleFlowStatusEnum.Approve)
                {
                    excludedFlowStatus = legalPrincipleFlowStatus.Where(x => x.Id == (int)PrincipleFlowStatusEnum.Publish || x.Id == (int)PrincipleFlowStatusEnum.NeedToModify).ToList();
                    await ChangeFlowStatusName(excludedFlowStatus);
                }
                else if (LLSLegalPrincipleDetailVM.FlowStatusId == (int)PrincipleFlowStatusEnum.Publish)
                {
                    excludedFlowStatus = legalPrincipleFlowStatus.Where(x => x.Id == (int)PrincipleFlowStatusEnum.Unpublished).ToList();
                    await ChangeFlowStatusName(excludedFlowStatus);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task ChangeFlowStatusName(List<LegalPrincipleFlowStatus> excludedFlowStatus)
        {
            try
            {
                List<LegalPrincipleFlowStatus> flowStatus = new List<LegalPrincipleFlowStatus>();
                foreach (var item in excludedFlowStatus)
                {
                    if (item.Id == (int)PrincipleFlowStatusEnum.InReview)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            item.Name_En = translationState.Translate("In Review");
                            flowStatus.Add(item);
                        }
                        else
                        {
                            item.Name_Ar = translationState.Translate("In Review");
                            flowStatus.Add(item);
                        }
                    }
                    if (item.Id == (int)PrincipleFlowStatusEnum.Approve)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            item.Name_En = translationState.Translate("Approve");
                            flowStatus.Add(item);
                        }
                        else
                        {
                            item.Name_Ar = translationState.Translate("Approve");
                            flowStatus.Add(item);
                        }
                    }
                    if (item.Id == (int)PrincipleFlowStatusEnum.Reject)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            item.Name_En = translationState.Translate("Reject");
                            flowStatus.Add(item);
                        }
                        else
                        {
                            item.Name_Ar = translationState.Translate("Reject");
                            flowStatus.Add(item);
                        }
                    }
                    if (item.Id == (int)PrincipleFlowStatusEnum.NeedToModify)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            item.Name_En = translationState.Translate("Need to modify");
                            flowStatus.Add(item);
                        }
                        else
                        {
                            item.Name_Ar = translationState.Translate("Need_To_Modify");
                            flowStatus.Add(item);
                        }
                    }
                    if (item.Id == (int)PrincipleFlowStatusEnum.SendAComment)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            item.Name_En = translationState.Translate("Send a comment");
                            flowStatus.Add(item);
                        }
                        else
                        {
                            item.Name_Ar = translationState.Translate("Send a comment");
                            flowStatus.Add(item);
                        }
                    }
                    if (item.Id == (int)PrincipleFlowStatusEnum.Publish)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            item.Name_En = translationState.Translate("Publish");
                            flowStatus.Add(item);
                        }
                        else
                        {
                            item.Name_Ar = translationState.Translate("Publish");
                            flowStatus.Add(item);
                        }
                    }
                    if (item.Id == (int)PrincipleFlowStatusEnum.Unpublished)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            item.Name_En = translationState.Translate("Unpublish");
                            flowStatus.Add(item);
                        }
                        else
                        {
                            item.Name_Ar = translationState.Translate("UnPublish");
                            flowStatus.Add(item);
                        }
                    }

                }
                excludedFlowStatus = flowStatus;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

    }
}
