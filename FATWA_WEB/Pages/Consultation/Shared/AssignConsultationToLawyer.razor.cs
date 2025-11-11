using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Consultation.Shared
{
    public partial class AssignConsultationToLawyer : ComponentBase

	{
		#region Parameter
		[Parameter]
		public Guid? ConsultationRequestId { get; set; }
		[Parameter]
		public dynamic AssignConsultationLawyerType { get; set; }
		[Parameter]
		public Guid ReferenceId { get; set; }
		[Parameter]
		public int? SectorTypeId { get; set; }
        [Parameter]
        public string? TaskId { get; set; }
        #endregion

        #region Variable declaration

        //public string PrimaryLaywerId { get; set; } 
        protected IEnumerable<LawyerVM> users { get; set; } = new List<LawyerVM>();
		protected IEnumerable<LawyerVM> supervisors { get; set; }
		protected bool IsVisible { get; set; }
		protected bool showadvancegrid { get; set; }
		protected RadzenDataGrid<LawyerVM> grid = new RadzenDataGrid<LawyerVM>();
		protected bool allowRowSelectOnRowClick = true;
		protected ConsultationAssignment consultationRequestLawyerAssignment = new ConsultationAssignment();
		protected TaskDetailVM taskDetailVM { get; set; } = new TaskDetailVM();
		public ConsultationAssignment dmsCopyAttachmentFromSourceToDestination { get; set; } = new ConsultationAssignment();
		protected List<Priority> Priorities { get; set; } = new List<Priority>();
		protected CmsAssignCaseFileBackToHos assignBackToHosCaseFiles { get; set; } = new CmsAssignCaseFileBackToHos();

		//public CmsCaseRequestLawyerVM cmsCaseRequestLawyerVM = new CmsCaseRequestLawyerVM();

		#endregion

		#region  OnInitialized
		public void Reload()
		{
			InvokeAsync(StateHasChanged);
		}
		protected override async Task OnInitializedAsync()
		{
			spinnerService.Show();
			consultationRequestLawyerAssignment = new ConsultationAssignment() { Id = Guid.NewGuid() };
			await PopulateUserdropdownlist();
			await PopulateSupervisorsList();
			await PopulatePriorities();
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
        protected async Task PopulatePriorities()
		{
			var response = await lookupService.GetCasePriorities();
			if (response.IsSuccessStatusCode)
			{
				Priorities = (List<Priority>)response.ResultData;
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}
			StateHasChanged();
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
		protected async Task PopulateUserdropdownlist()
		{
			var userresponse = await lookupService.GetLawyersBySector(loginState.UserDetail.SectorTypeId);
			if (userresponse.IsSuccessStatusCode)
			{
				users = (IEnumerable<LawyerVM>)userresponse.ResultData;

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

				var singleUser = users.Where(x => x.Id == (string)args).FirstOrDefault();
				if (args != null)
				{
					IsVisible = false;
					consultationRequestLawyerAssignment.SelectedUsers = new List<LawyerVM>();
				}



				StateHasChanged();
			}
			catch (Exception ex)
			{

			}
		}
		#endregion

		#region Advance selection grid view List

		protected async Task ShowAdvanceSelectionGrid()
		{
			consultationRequestLawyerAssignment.LawyerId = null;
			selectedLawyer = null;
			if (!IsVisible)
				IsVisible = true;

			else
				IsVisible = false;
			consultationRequestLawyerAssignment.SelectedUsers = new List<LawyerVM>();
			StateHasChanged();

		}

		#endregion

		#region Submit button click
		//< History Author = 'Zaeem' Date = '2023-12-27' Version = "1.0" Branch = "master" >Fix the url navigation to file view,  change the pararmter to File Id </History>

		protected async Task FormSubmit(ConsultationAssignment args)
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
					if (AssignConsultationLawyerType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
					{
						spinnerService.Show();
						if (consultationRequestLawyerAssignment.Remarks == null)
						{
							if (SectorTypeId == (int)OperatingSectorTypeEnum.InternationalArbitration)
							{
								consultationRequestLawyerAssignment.Remarks = "Assign_For_International_Arbitration";
							}
							else if (SectorTypeId == (int)OperatingSectorTypeEnum.Contracts)
							{
								consultationRequestLawyerAssignment.Remarks = "Assign_For_Contracts";
							}
							else if (SectorTypeId == (int)OperatingSectorTypeEnum.LegalAdvice)
							{
								consultationRequestLawyerAssignment.Remarks = "Assign_For_Legal_Advice";
							}
							else if (SectorTypeId == (int)OperatingSectorTypeEnum.Legislations)
							{
								consultationRequestLawyerAssignment.Remarks = "Assign_For_Legislations";
							}
							else if (SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeComplaints)
							{
								consultationRequestLawyerAssignment.Remarks = "Assign_For_Administrative_Complaints";
							}
						}
						consultationRequestLawyerAssignment.ConsultationRequestId = ConsultationRequestId;
						consultationRequestLawyerAssignment.AssignConsultationLawyerType = AssignConsultationLawyerType;
						consultationRequestLawyerAssignment.ReferenceId = (Guid)ConsultationRequestId;
						consultationRequestLawyerAssignment.SectorTypeId = loginState.UserDetail.SectorTypeId;
						var DepartmentandGovtEntityUser = await BrowserStorage.GetItemAsync<UserDetailVM>("UserDetail");
						var response = await comsSharedService.AssignConsultationRequestToLawyer(consultationRequestLawyerAssignment, DepartmentandGovtEntityUser.UserName);

						if (response.IsSuccessStatusCode)
						{
                            taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                            if (loginState.UserRoles.Any(ur => ur.RoleId == SystemRoles.ComsHOS || ur.RoleId == SystemRoles.ViceHOS))
                            {
                                taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                                taskDetailVM.IsMultipleTaskUpdateForSameEntity = true;
                                taskDetailVM.SectorId = loginState.UserDetail.SectorTypeId;
                                taskDetailVM.SystemGenTypeIdsToComplete.Add((int)TaskSystemGenTypeEnum.CreateConsultationRequest);
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

							dmsCopyAttachmentFromSourceToDestination = (ConsultationAssignment)response.ResultData;

							var docResponse = await fileUploadService.CopyAttachmentsFromSourceToDestination(dmsCopyAttachmentFromSourceToDestination.CopyAttachmentVMs);
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
							if ((int)AssignCaseToLawyerTypeEnum.ConsultationRequest == AssignConsultationLawyerType)
							{
								navigationManager.NavigateTo("/consultationfile-view" + "/" + dmsCopyAttachmentFromSourceToDestination.FileId + "/" + consultationRequestLawyerAssignment.SectorTypeId);
							}
							else
							{
								dialogService.Close(consultationRequestLawyerAssignment);
							}
						}
						else
						{
							await invalidRequestHandlerService.ReturnBadRequestNotification(response);
						}

						spinnerService.Hide();
					}
					// start here
					if (AssignConsultationLawyerType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile)
					{
						ApiCallResponse userresponse = null;
						if (!IsVisible)
						{
							userresponse = await comsSharedService.GetSendBackToHosByReferenceId(ReferenceId, consultationRequestLawyerAssignment.LawyerId);
							if (userresponse.IsSuccessStatusCode)
							{
								assignBackToHosCaseFiles = (CmsAssignCaseFileBackToHos)userresponse.ResultData;
								if (assignBackToHosCaseFiles.Id == Guid.Empty)
								{
									spinnerService.Show();

									if (consultationRequestLawyerAssignment.Remarks == null)
									{
										if (SectorTypeId == (int)OperatingSectorTypeEnum.InternationalArbitration)
										{
											consultationRequestLawyerAssignment.Remarks = "Assign_For_International_Arbitration";
										}
										else if (SectorTypeId == (int)OperatingSectorTypeEnum.Contracts)
										{
											consultationRequestLawyerAssignment.Remarks = "Assign_For_Contracts";
										}
										else if (SectorTypeId == (int)OperatingSectorTypeEnum.LegalAdvice)
										{
											consultationRequestLawyerAssignment.Remarks = "Assign_For_Legal_Advice";
										}
										else if (SectorTypeId == (int)OperatingSectorTypeEnum.Legislations)
										{
											consultationRequestLawyerAssignment.Remarks = "Assign_For_Legislations";
										}
										else if (SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeComplaints)
										{
											consultationRequestLawyerAssignment.Remarks = "Assign_For_Administrative_Complaints";
										}
									}
									consultationRequestLawyerAssignment.ConsultationRequestId = ConsultationRequestId;
									consultationRequestLawyerAssignment.AssignConsultationLawyerType = AssignConsultationLawyerType;
									consultationRequestLawyerAssignment.ReferenceId = ReferenceId;
									consultationRequestLawyerAssignment.SectorTypeId = loginState.UserDetail.SectorTypeId;
									var DepartmentandGovtEntityUser = await BrowserStorage.GetItemAsync<UserDetailVM>("UserDetail");
									var response = await comsSharedService.AssignConsultationRequestToLawyer(consultationRequestLawyerAssignment, DepartmentandGovtEntityUser.UserName);

									if (response.IsSuccessStatusCode)
									{
										notificationService.Notify(new NotificationMessage()
										{
											Severity = NotificationSeverity.Success,
											Detail = translationState.Translate("Assign_Request_Initiated"),
											Style = "position: fixed !important; left: 0; margin: auto;"
										});

										dmsCopyAttachmentFromSourceToDestination = (ConsultationAssignment)response.ResultData;

										var docResponse = await fileUploadService.CopyAttachmentsFromSourceToDestination(dmsCopyAttachmentFromSourceToDestination.CopyAttachmentVMs);
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

										if ((int)AssignCaseToLawyerTypeEnum.ConsultationRequest == AssignConsultationLawyerType)
										{
											navigationManager.NavigateTo("/consultationfile-view" + "/" + consultationRequestLawyerAssignment.ReferenceId + "/" + consultationRequestLawyerAssignment.SectorTypeId);
										}
										else
										{
											dialogService.Close(consultationRequestLawyerAssignment);
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
						}
						else
						{
							foreach (var userlist in consultationRequestLawyerAssignment.SelectedUsers)
							{
								userresponse = await comsSharedService.GetSendBackToHosByReferenceId(ReferenceId, userlist.Id);
								if (userresponse.IsSuccessStatusCode)
								{
									assignBackToHosCaseFiles = (CmsAssignCaseFileBackToHos)userresponse.ResultData;

								}

							}
							if (assignBackToHosCaseFiles == null)
							{

								{
									spinnerService.Show();

									if (consultationRequestLawyerAssignment.Remarks == null)
									{
										if (SectorTypeId == (int)OperatingSectorTypeEnum.InternationalArbitration)
										{
											consultationRequestLawyerAssignment.Remarks = "Assign_For_International_Arbitration";
										}
										else if (SectorTypeId == (int)OperatingSectorTypeEnum.Contracts)
										{
											consultationRequestLawyerAssignment.Remarks = "Assign_For_Contracts";
										}
										else if (SectorTypeId == (int)OperatingSectorTypeEnum.LegalAdvice)
										{
											consultationRequestLawyerAssignment.Remarks = "Assign_For_Legal_Advice";
										}
										else if (SectorTypeId == (int)OperatingSectorTypeEnum.Legislations)
										{
											consultationRequestLawyerAssignment.Remarks = "Assign_For_Legislations";
										}
										else if (SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeComplaints)
										{
											consultationRequestLawyerAssignment.Remarks = "Assign_For_Administrative_Complaints";
										}
									}
									consultationRequestLawyerAssignment.ConsultationRequestId = ConsultationRequestId;
									consultationRequestLawyerAssignment.AssignConsultationLawyerType = AssignConsultationLawyerType;
									consultationRequestLawyerAssignment.ReferenceId = ReferenceId;
									consultationRequestLawyerAssignment.SectorTypeId = loginState.UserDetail.SectorTypeId;
									var DepartmentandGovtEntityUser = await BrowserStorage.GetItemAsync<UserDetailVM>("UserDetail");
									var response = await comsSharedService.AssignConsultationRequestToLawyer(consultationRequestLawyerAssignment, DepartmentandGovtEntityUser.UserName);

									if (response.IsSuccessStatusCode)
									{
										notificationService.Notify(new NotificationMessage()
										{
											Severity = NotificationSeverity.Success,
											Detail = translationState.Translate("Assign_Request_Initiated"),
											Style = "position: fixed !important; left: 0; margin: auto;"
										});

										dmsCopyAttachmentFromSourceToDestination = (ConsultationAssignment)response.ResultData;

										var docResponse = await fileUploadService.CopyAttachmentsFromSourceToDestination(dmsCopyAttachmentFromSourceToDestination.CopyAttachmentVMs);
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

										if ((int)AssignCaseToLawyerTypeEnum.ConsultationRequest == AssignConsultationLawyerType)
										{
											navigationManager.NavigateTo("/consultationfile-view" + "/" + consultationRequestLawyerAssignment.ReferenceId + "/" + consultationRequestLawyerAssignment.SectorTypeId);
										}
										else
										{
											dialogService.Close(consultationRequestLawyerAssignment);
										}
									}
									else
									{
										await invalidRequestHandlerService.ReturnBadRequestNotification(response);
									}


									spinnerService.Hide();

								}
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
		#endregion

		#region cancel button

		protected async Task ButtonCloseDialog(MouseEventArgs args)
		{
			dialogService.Close(null);
		}

		#endregion

	}
}
