using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    public partial class AssignToLawyer : ComponentBase

    {
        #region Parameter
        [Parameter]
        public Guid? RequestId { get; set; }
        [Parameter]
        public dynamic AssignCaseLawyerType { get; set; }
        [Parameter]
        public Guid ReferenceId { get; set; }
        [Parameter]
        public string? TaskId { get; set; }
        #endregion

        #region Variable declaration

        //public string PrimaryLaywerId { get; set; } 
        protected IEnumerable<LawyerVM> lawyers { get; set; }
        protected IEnumerable<LawyerVM> supervisors { get; set; }
        protected CmsAssignCaseFileBackToHos assignBackToHosCaseFiles { get; set; } = new CmsAssignCaseFileBackToHos();
        protected RadzenDataGrid<LawyerVM> grid = new RadzenDataGrid<LawyerVM>();
        protected bool IsVisible { get; set; }
        protected bool showadvancegrid { get; set; }
        protected bool allowRowSelectOnRowClick = true;
        protected CaseAssignment caseRequestLawyerAssignment = new CaseAssignment();
        protected List<CopyAttachmentVM> copyAttachments = new List<CopyAttachmentVM>();
        public string rejectedBy { get; set; }
        public string rejectedUserId { get; set; }
        protected TaskDetailVM taskDetailVM = new TaskDetailVM();


        #endregion 

        #region  OnInitialized

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            caseRequestLawyerAssignment = new CaseAssignment() { Id = Guid.NewGuid() };
            await PopulateReferenceEntityDetail();
            await PopulateSupervisorsList();
            if (TaskId != null)
            {
                await PopulateTaskDetails();
            }

            spinnerService.Hide();
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
                await invalidRequestHandlerService.ReturnBadRequestNotification(getTaskDetail);
            }
        }
        protected async Task PopulateReferenceEntityDetail()
        {
            if (AssignCaseLawyerType == (int)AssignCaseToLawyerTypeEnum.RegisteredCase)
            {
                var result = await cmsRegisteredCaseService.GetRegisteredCaseDetailByIdVM(ReferenceId);
                if (result.IsSuccessStatusCode)
                {
                    var registeredCase = (CmsRegisteredCaseDetailVM)result.ResultData;
                    await PopulateLawyersList(loginState.UserDetail.SectorTypeId != null ? (int)loginState.UserDetail.SectorTypeId : 0, registeredCase.ChamberNumberId);
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(result);
                }
            }
            else
            {
                await PopulateLawyersList(loginState.UserDetail.SectorTypeId != null ? (int)loginState.UserDetail.SectorTypeId : 0);
            }
        }
        protected async Task PopulateLawyersList(int sectorTypeId, int chamberNumberId = 0)
        {
            var userresponse = await lookupService.GetLawyersBySectorAndChamber(sectorTypeId, chamberNumberId);
            if (userresponse.IsSuccessStatusCode)
            {
                lawyers = (IEnumerable<LawyerVM>)userresponse.ResultData;
                lawyers = lawyers.OrderBy(x => x.TotalTasks);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(userresponse);
            }
        }
        protected async Task PopulateSupervisorsList()
        {
            var userresponse = await lookupService.GetSupervisorsBySector(loginState.UserDetail.SectorTypeId);
            if (userresponse.IsSuccessStatusCode)
            {
                supervisors = (IEnumerable<LawyerVM>)userresponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(userresponse);
            }
        }

        #endregion

        #region change event dropdown

        LawyerVM? selectedLawyer = null;
        protected async Task OnTypeChange(object args)
        {
            try
            {
                selectedLawyer = lawyers.FirstOrDefault(x => x.Id == (string)args);
                if (args != null)
                {
                    IsVisible = false;
                    caseRequestLawyerAssignment.SelectedUsers = new List<LawyerVM>();
                    caseRequestLawyerAssignment.PrimaryLawyerId = null;
                    await PopulateSupervisorId(caseRequestLawyerAssignment.LawyerId);
                }
                else
                {
                    caseRequestLawyerAssignment.SupervisorId = null;
                }
                StateHasChanged();
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected async Task OnPrimaryLawyerChange(object args)
        {
            try
            {
                if (args != null)
                {
                    await PopulateSupervisorId(caseRequestLawyerAssignment.PrimaryLawyerId);
                }
                else
                {
                    caseRequestLawyerAssignment.SupervisorId = null;
                }
                StateHasChanged();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected async Task PopulateSupervisorId(string lawyerId)
        {
            try
            {
                var response = await lookupService.GetSupervisorByLawyerId(lawyerId);
                if (response.IsSuccessStatusCode)
                {
                    caseRequestLawyerAssignment.SupervisorId = (string)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Advance selection grid view List

        protected async Task ShowAdvanceSelectionGrid()
        {
            caseRequestLawyerAssignment.LawyerId = null;
            caseRequestLawyerAssignment.SupervisorId = null;
            caseRequestLawyerAssignment.PrimaryLawyerId = null;
            caseRequestLawyerAssignment.SelectedUsers = new List<LawyerVM>();
            selectedLawyer = null;
            if (!IsVisible)
                IsVisible = true;

            else
                IsVisible = false;
            StateHasChanged();

        }

        #endregion

        #region Submit button click


        protected async Task FormSubmit(CaseAssignment args)
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                   translationState.Translate("Sure_Assign_Request"),
                   translationState.Translate("Confirm"),
                   new ConfirmOptions()
                   {
                       OkButtonText = @translationState.Translate("OK"),
                       CancelButtonText = @translationState.Translate("Cancel")
                   });
                if (dialogResponse == true)
                {
                    if (AssignCaseLawyerType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
                    {
                        spinnerService.Show();

                        if (caseRequestLawyerAssignment.Remarks == null)
                        {
                            caseRequestLawyerAssignment.Remarks = string.Empty;
                        }
                        caseRequestLawyerAssignment.RequestId = RequestId;
                        caseRequestLawyerAssignment.AssignCaseToLawyerType = AssignCaseLawyerType;
                        caseRequestLawyerAssignment.ReferenceId = ReferenceId;
                        caseRequestLawyerAssignment.SectorTypeId = loginState.UserDetail.SectorTypeId;
                        var DepartmentandGovtEntityUser = await BrowserStorage.GetItemAsync<UserDetailVM>("UserDetail");

                        var response = await cmsSharedService.AssignCaseRequestToLawyer(caseRequestLawyerAssignment, DepartmentandGovtEntityUser.UserName);
                        if (response.IsSuccessStatusCode)
                        {

                            taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                            if (loginState.UserRoles.Any(ur => ur.RoleId == SystemRoles.HOS || ur.RoleId == SystemRoles.ViceHOS))
                            {
                                taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                                taskDetailVM.IsMultipleTaskUpdateForSameEntity = true;
                                taskDetailVM.SectorId = loginState.UserDetail.SectorTypeId;
                                taskDetailVM.SystemGenTypeIdsToComplete.Add((int)TaskSystemGenTypeEnum.CreateCaseRequest);
                            }
                            var taskResponse = await taskService.DecisionTask(taskDetailVM);
                            if (!taskResponse.IsSuccessStatusCode)
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                            }

                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Request_Assigned"),
                                Style = "position: fixed !important; left: 0; margin: auto;"
                            });

                            caseRequestLawyerAssignment = (CaseAssignment)response.ResultData;

                            var docResponse = await fileUploadService.CopyAttachmentsFromSourceToDestination(caseRequestLawyerAssignment.CopyAttachmentVMs);
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

                            if ((int)AssignCaseToLawyerTypeEnum.CaseRequest == AssignCaseLawyerType)
                            {
                                navigationManager.NavigateTo("/usertask-list");
                            }
                            else
                            {
                                dialogService.Close(caseRequestLawyerAssignment);
                            }

                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }

                        spinnerService.Hide();

                    }

                    if (AssignCaseLawyerType == (int)AssignCaseToLawyerTypeEnum.CaseFile
                        || AssignCaseLawyerType == (int)AssignCaseToLawyerTypeEnum.RegisteredCase)
                    {
                        ApiCallResponse userresponse = null;
                        if (!IsVisible)
                        {
                            userresponse = await cmsSharedService.GetSendBackToHosByReferenceId(ReferenceId, caseRequestLawyerAssignment.LawyerId);
                            if (userresponse.IsSuccessStatusCode)
                            {
                                assignBackToHosCaseFiles = (CmsAssignCaseFileBackToHos)userresponse.ResultData;
                                if (assignBackToHosCaseFiles.Id == Guid.Empty)
                                {
                                    spinnerService.Show();

                                    if (caseRequestLawyerAssignment.Remarks == null)
                                    {
                                        caseRequestLawyerAssignment.Remarks = string.Empty;
                                    }
                                    caseRequestLawyerAssignment.RequestId = RequestId;
                                    caseRequestLawyerAssignment.AssignCaseToLawyerType = AssignCaseLawyerType;
                                    caseRequestLawyerAssignment.ReferenceId = ReferenceId;
                                    caseRequestLawyerAssignment.SectorTypeId = loginState.UserDetail.SectorTypeId;
                                    var DepartmentandGovtEntityUser = await BrowserStorage.GetItemAsync<UserDetailVM>("UserDetail");

                                    var response = await cmsSharedService.AssignCaseRequestToLawyer(caseRequestLawyerAssignment, DepartmentandGovtEntityUser.UserName);
                                    if (response.IsSuccessStatusCode)
                                    {
                                        notificationService.Notify(new NotificationMessage()
                                        {
                                            Severity = NotificationSeverity.Success,
                                            Detail = translationState.Translate("Request_Assigned"),
                                            Style = "position: fixed !important; left: 0; margin: auto;"
                                        });

                                        caseRequestLawyerAssignment = (CaseAssignment)response.ResultData;

                                        if ((int)AssignCaseToLawyerTypeEnum.CaseRequest == AssignCaseLawyerType)
                                        {
                                            navigationManager.NavigateTo("/usertask-list");
                                        }
                                        else
                                        {
                                            dialogService.Close(caseRequestLawyerAssignment);
                                        }
                                    }
                                    else
                                    {
                                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                                    }
                                    spinnerService.Hide();
                                }
                                else
                                {
                                    notificationService.Notify(new NotificationMessage()
                                    {
                                        Severity = NotificationSeverity.Error,
                                        Detail = translationState.Translate("Cannot_assign_to_this_laywer_Beacuse_already_rejected"),
                                        Style = "position: fixed !important; left: 0; margin: auto; "

                                    });
                                }
                            }
                            else
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(userresponse);
                            }
                        }
                        else
                        {
                            spinnerService.Show();
                            foreach (var userlist in caseRequestLawyerAssignment.SelectedUsers)
                            {
                                userresponse = await cmsSharedService.GetSendBackToHosByReferenceId(ReferenceId, userlist.Id);
                                if (userresponse.IsSuccessStatusCode)
                                {
                                    assignBackToHosCaseFiles = (CmsAssignCaseFileBackToHos)userresponse.ResultData;
                                    if (assignBackToHosCaseFiles.Id != Guid.Empty)
                                    {
                                        spinnerService.Hide();
                                        notificationService.Notify(new NotificationMessage()
                                        {
                                            Severity = NotificationSeverity.Error,
                                            Detail = translationState.Translate("Cannot_assign_to_this_laywer_Beacuse_already_rejected"),
                                            Style = "position: fixed !important; left: 0; margin: auto; "

                                        });
                                        return;
                                    }
                                }
                                else
                                {
                                    spinnerService.Hide();
                                    await invalidRequestHandlerService.ReturnBadRequestNotification(userresponse);
                                    return;
                                }
                            }

                            if (caseRequestLawyerAssignment.Remarks == null)
                            {
                                caseRequestLawyerAssignment.Remarks = string.Empty;
                            }
                            caseRequestLawyerAssignment.RequestId = RequestId;
                            caseRequestLawyerAssignment.AssignCaseToLawyerType = AssignCaseLawyerType;
                            caseRequestLawyerAssignment.ReferenceId = ReferenceId;
                            caseRequestLawyerAssignment.SectorTypeId = loginState.UserDetail.SectorTypeId;
                            var DepartmentandGovtEntityUser = await BrowserStorage.GetItemAsync<UserDetailVM>("UserDetail");

                            var response = await cmsSharedService.AssignCaseRequestToLawyer(caseRequestLawyerAssignment, DepartmentandGovtEntityUser.UserName);
                            if (response.IsSuccessStatusCode)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("Request_Assigned"),
                                    Style = "position: fixed !important; left: 0; margin: auto;"
                                });

                                caseRequestLawyerAssignment = (CaseAssignment)response.ResultData;

                                if ((int)AssignCaseToLawyerTypeEnum.CaseRequest == AssignCaseLawyerType)
                                {
                                    navigationManager.NavigateTo("/casefile-view/" + caseRequestLawyerAssignment.ReferenceId);
                                }
                                else
                                {
                                    dialogService.Close(caseRequestLawyerAssignment);
                                }
                            }
                            else
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                            }

                            spinnerService.Hide();
                        }

                    }

                }
            }
            catch
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong")
                });
            }
        }

        protected void AddCopyAttachment(CopyAttachmentVM copyAttachment)
        {
            try
            {
                copyAttachments.Add(
                    new CopyAttachmentVM()
                    {
                        SourceId = copyAttachment.SourceId,
                        DestinationId = copyAttachment.DestinationId,
                        CreatedBy = copyAttachment.CreatedBy
                    });
            }
            catch (Exception)
            {
                return;
                throw;
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
