using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using static FATWA_DOMAIN.Enums.TaskEnums;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Merge Request Detail</History>
    public partial class DetailMergeRequest : ComponentBase
    {
        #region Parameter

        [Parameter]
        public dynamic MergeRequestId { get; set; }

        [Parameter]
        public string? TaskId { get; set; }

        #endregion

        #region Variables
        protected TaskDetailVM taskDetailVM { get; set; }

        MergeRequestVM mergeRequest = new MergeRequestVM { Id = Guid.NewGuid() };
        List<CmsRegisteredCaseVM> mergedCases = new List<CmsRegisteredCaseVM>();
        protected CmsRegisteredCaseDetailVM primaryCase { get; set; }
        protected Guid PrimaryCaseId { get; set; }
        protected string Reason { get; set; }
        protected string MergedCANs { get; set; }

        protected string primaryValidationMsg = "";
        protected string reasonValidationMsg = "";
        protected List<ManagerTaskReminderVM> managerTaskReminderData { get; set; } = new List<ManagerTaskReminderVM>();

        #endregion

        #region Component Load

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                await PopulateMergedRequestDetail();
                await PopulatePrimaryCaseDetail();
                await PopulateMergedCases();
                await PopulateMergedCANs();
                if (TaskId != null)
                {
                    await PopulateTaskDetails();
                    await GetManagerTaskReminderData();
                }

                spinnerService.Hide();
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

        #region Data Population Events

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Populate Merged request details</History>
        protected async Task PopulateMergedRequestDetail()
        {
            try
            {
                var response = await cmsRegisteredCaseService.GetMergeRequestDetailById(Guid.Parse(MergeRequestId));
                if (response.IsSuccessStatusCode)
                {
                    mergeRequest = (MergeRequestVM)response.ResultData;
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

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Populate Primary Case Detail</History>
        protected async Task PopulatePrimaryCaseDetail()
        {
            try
            {
                var result = await cmsRegisteredCaseService.GetRegisteredCaseDetailByIdVM(mergeRequest.PrimaryId);

                if (result.IsSuccessStatusCode)
                {
                    primaryCase = (CmsRegisteredCaseDetailVM)result.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(result);
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

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Populate Merged Cases</History>
        protected async Task PopulateMergedCases()
        {
            try
            {
                var response = await cmsRegisteredCaseService.GetMergedCasesByMergeRequestId(Guid.Parse(MergeRequestId));
                if (response.IsSuccessStatusCode)
                {
                    mergedCases = (List<CmsRegisteredCaseVM>)response.ResultData;
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

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Concat Merged CANs</History>
        protected async Task PopulateMergedCANs()
        {
            try
            {
                var length = mergedCases?.Count();
                MergedCANs = mergeRequest.PrimaryCANNumber;
                for (int i = 0; i < length; i++)
                {
                    var seperator = i + 1 == length ? "" : ", ";
                    MergedCANs += mergedCases[i].CANNumber + seperator;
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
        //<History Author = 'Hassan Abbas' Date='2023-03-10' Version="1.0" Branch="master">Populate Task Detail to be Approved or Rejected</History>
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

        #endregion

        #region Button Events

        //< History Author = 'Hassan Abbas' Date = '2022-11-20' Version = "1.0" Branch = "master" >Submit Form</History>
        protected async Task Form0Submit(MergeRequestVM args)
        {
            try
            {
                if (mergeRequest.PrimaryId == Guid.Empty || String.IsNullOrEmpty(mergeRequest.Reason))
                {
                    primaryValidationMsg = mergeRequest.PrimaryId == Guid.Empty ? translationState.Translate("Required_Field") : "";
                    reasonValidationMsg = String.IsNullOrEmpty(mergeRequest.Reason) ? translationState.Translate("Required_Field") : "";
                    return;
                }
                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
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

        #region Redirect Function

        //<History Author = 'Hassan Abbas' Date='2022-11-25' Version="1.0" Branch="master">Redirect to respective page</History>
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        //<History Author = 'Hassan Abbas' Date='2022-11-29' Version="1.0" Branch="master"> Redirect to Case Detail page</History>
        protected async Task DetailRegisteredCase(CmsRegisteredCaseVM args)
        {
            navigationManager.NavigateTo("/case-view/" + args.CaseId);
        }
        //<History Author = 'Hassan Abbas' Date='2022-11-25' Version="1.0" Branch="master">Redirect to respective page</History>
        protected async Task RedirectCancel()
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Cancel"), translationState.Translate("Confirm_Cancel"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                await RedirectBack();
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-11-25' Version="1.0" Branch="master">Redirect to respective page</History>
        protected async Task Reject()
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Reject"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                var response = await cmsRegisteredCaseService.RejectMergeRequest(Guid.Parse(MergeRequestId));
                if (response.IsSuccessStatusCode)
                {
                    if (TaskId != null)
                    {
                        taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                        var taskResponse = await taskService.DecisionTask(taskDetailVM);
                        if (!taskResponse.IsSuccessStatusCode)
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                        }
                    }
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Merge_Request_Rejected"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await Task.Delay(1500);
                    await RedirectBack();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-11-25' Version="1.0" Branch="master">Redirect to respective page</History>
        protected async Task Approve()
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Approve"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                var response = await cmsRegisteredCaseService.ApproveMergeRequest(Guid.Parse(MergeRequestId));
                if (response.IsSuccessStatusCode)
                {
                    if (TaskId != null)
                    {
                        taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                        var taskResponse = await taskService.DecisionTask(taskDetailVM);
                        if (!taskResponse.IsSuccessStatusCode)
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                        }
                    }
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Merge_Request_Approved"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await Task.Delay(1500);
                    await RedirectBack();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
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
