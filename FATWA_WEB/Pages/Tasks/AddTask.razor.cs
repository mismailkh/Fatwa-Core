using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_DOMAIN.Models.WorkflowModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Tasks
{
    public partial class AddTask : ComponentBase
	{
		#region Parameters

		[Parameter]
		public dynamic? TaskId { get; set; }

		#endregion

		#region Variables

		protected bool isEdit = false;

		SaveTaskVM saveTask = new SaveTaskVM()
		{
			Task = new UserTask()
			{
				DueDate = DateTime.Now.Date,
				Date = DateTime.Now.Date
			},
			TaskActions = new List<TaskAction>(),
			DeletedTaskActionIds = new List<Guid>(),
		};

		//DDL
		protected List<Module> Modules = new List<Module>();
		protected List<Role> Roles = new List<Role>();
		protected List<Department> Departments = new List<Department>();
		protected List<TaskSubType> TaskSubTypes = new List<TaskSubType>();
		protected List<TaskType> TaskTypes = new List<TaskType>();
		protected List<Priority> Priorities = new List<Priority>();
		protected List<CaseFile> FileNumbers = new List<CaseFile>();
		protected List<ConsultationFile> ConsultationFileNumbers = new List<ConsultationFile>();
		protected List<CaseRequest> RequestNumbers = new List<CaseRequest>();
		protected List<ConsultationRequest> ConsultationRequestNumbers = new List<ConsultationRequest>();


		protected List<OperatingSectorType> Sectors = new List<OperatingSectorType>();
		protected List<UserVM> Assignees = new List<UserVM>();

		//Element Reference
		protected RadzenDropDown<int> ddlModule;
		protected RadzenDropDown<string> ddlRole;
		protected RadzenDropDown<int?> ddlDepartment;
		protected RadzenDropDown<int> ddlTaskType;
		protected RadzenDropDown<int?> ddlTaskSubType;
		protected RadzenDropDown<int?> ddlPriority;
		protected RadzenDropDown<Guid?> ddlFileRequestNumber;

		protected RadzenDropDown<int> ddlSector;
		protected RadzenDropDown<string> ddlAssignTo;

		protected RadzenDataGrid<TaskAction> ActionItemGrid;

		#endregion

		#region Load

		protected override async Task OnInitializedAsync()
		{
			spinnerService.Show();
			if (TaskId != null)
			{
				var response = await taskService.GetTaskById(Guid.Parse(TaskId));
				if (response.IsSuccessStatusCode)
				{
					saveTask = (SaveTaskVM)response.ResultData;

					GetActionItemList = saveTask.TaskActions;

					Reload();
				}
				isEdit = true;
			}
			else
			{
				saveTask.Task.TaskId = Guid.NewGuid();
				await GetMaxTaskNumber();
				saveTask.Task.TaskStatusId = (int)TaskStatusEnum.Pending;
				saveTask.Task.TypeId = (int)TaskTypeEnum.Task;
				saveTask.Task.PriorityId = (int)PriorityEnum.Medium;

				isEdit = false;
			}

			await PopulateDropdowns(); 

			spinnerService.Hide();
		}

		protected async Task GetMaxTaskNumber()
		{
			var response = await taskService.GetMaxTaskNumber();
			if (response.IsSuccessStatusCode)
			{
				saveTask.Task.TaskNumber = (int)response.ResultData;
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}
		}

		#endregion

		#region Remote Dropdown Data and Dropdown Change Events

		//<History Author = 'Zain Ul Islam' Date='2022-12-01' Version="1.0" Branch="master">Populate DropdownLists</History>
		protected async Task PopulateDropdowns()
		{
			await PopulateModules();
			await PopulateSectors();
			await PopulateDepartments();
			await PopulateAssignees();
			await PopulateTaskType();
			await PopulateTaskSubType();
			await PopulatePrioritities();
			await PopulateFileRequestNumber(false);
			await PopulateRoles();
		}

		protected async Task PopulateModules()
		{
			var response = await lookupService.GetModules();
			if (response.IsSuccessStatusCode)
			{
				Modules = (List<Module>)response.ResultData;
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}
		}

		protected async Task PopulateSectors()
		{
			var response = await lookupService.GetOperatingSectorTypes();
			if (response.IsSuccessStatusCode)
			{
				Sectors = (List<OperatingSectorType>)response.ResultData;
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}
		}

		protected async Task PopulateDepartments()
		{
			var response = await lookupService.GetDepartments();
			if (response.IsSuccessStatusCode)
			{
				Departments = (List<Department>)response.ResultData;
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}
		}

		protected async Task PopulateAssignees()
		{
			var response = await lookupService.GetUsers();
			if (response.IsSuccessStatusCode)
			{
				Assignees = (List<UserVM>)response.ResultData;
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}
		}

		protected async Task PopulateTaskType()
		{
			var response = await lookupService.GetTaskType();
			if (response.IsSuccessStatusCode)
			{
				TaskTypes = (List<TaskType>)response.ResultData; 
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}
		}

		protected async Task PopulateTaskSubType()
		{
			var response = await lookupService.GetTaskSubType();
			if (response.IsSuccessStatusCode)
			{
				TaskSubTypes = (List<TaskSubType>)response.ResultData;
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}
		}

		protected async Task PopulatePrioritities()
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
		}

		protected async Task PopulateFileRequestNumber(bool isChange)
		{
			if(isChange)
				saveTask.Task.ReferenceId = null;
			
				if (saveTask.Task.TypeId == (int)TaskTypeEnum.Task)
				{
					var response = await lookupService.GetFileNumber();
					if (response.IsSuccessStatusCode)
					{
						FileNumbers = (List<CaseFile>)response.ResultData;
					}
					else
					{
						await invalidRequestHandlerService.ReturnBadRequestNotification(response);
					}
				}
				else
				{
					var response = await lookupService.GetRequestNumber();
					if (response.IsSuccessStatusCode)
					{
						RequestNumbers = (List<CaseRequest>)response.ResultData;
					}
					else
					{
						await invalidRequestHandlerService.ReturnBadRequestNotification(response);
					}

				}
			
			

			}

		protected async Task PopulateRoles()
		{
			var response = await lookupService.GetRoles();
			if (response.IsSuccessStatusCode)
			{
				Roles = (List<Role>)response.ResultData;
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}
		}

		#endregion  

		#region Action Item GRID

		protected List<TaskAction> GetActionItemList = new List<TaskAction>();
		protected TaskAction ActionItem = new TaskAction()
		{
			DueDate = DateTime.Now.Date
		};

		protected async Task AddActionItem()
		{
			if (ActionItem.ActionName != null)
			{
				ActionItem.ActionId = Guid.NewGuid();
				ActionItem.CreatedDate = DateTime.Now;
				ActionItem.IsDeleted = false;

				GetActionItemList.Add(ActionItem);

				ActionItemGrid.Reset();
				await ActionItemGrid.Reload();

				ActionItem = new TaskAction()
				{
					DueDate = DateTime.Now.Date
				};
			}
			else
			{

                if (GetActionItemList.Count == 0)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Please_Select_Atleast_One_ActionItem"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
                    return;
                }
            }
			Reload();
		}

		protected async Task DeleteActionItem(TaskAction actionItem)
		{
			saveTask.DeletedTaskActionIds.Add(actionItem.ActionId);

			GetActionItemList.Remove(actionItem);
			ActionItemGrid.Reset();
			await ActionItemGrid.Reload();

			Reload();
		}

		#endregion

		#region Functions

		public void Reload()
		{
			InvokeAsync(StateHasChanged);
		}

		protected async Task FormSubmit()
		{
			try
			{
				//Action Item
				//Check if the Atleast one ActionItem is added
				if (GetActionItemList.Count == 0)
				{
					notificationService.Notify(new NotificationMessage()
					{
						Severity = NotificationSeverity.Error,
						Detail = translationState.Translate("Please_Select_Atleast_One_ActionItem")
					});
					return;
				}
				else
				{
					saveTask.TaskActions = GetActionItemList;
				}

				bool? dialogResponse = await dialogService.Confirm(
				  translationState.Translate("Sure_Save_Task"),
				  translationState.Translate("Confirm"),
				  new ConfirmOptions()
				  {
					  OkButtonText = @translationState.Translate("OK"),
					  CancelButtonText = @translationState.Translate("Cancel")
				  });

				if (dialogResponse == true)
				{ 
					ApiCallResponse response = null;

					if (isEdit == false)
					{
                        response = await taskService.AddTask(saveTask);
                        // Save Attachement To Uploaded Documents
                        await SaveTempAttachementToUploadedDocument();
                    }
					else
					{
                        response = await taskService.EditTask(saveTask);
                        // Save Attachement To Uploaded Documents
                        await SaveTempAttachementToUploadedDocument();
                    }	
					if (response.IsSuccessStatusCode)
					{
						notificationService.Notify(new NotificationMessage()
						{
							Severity = NotificationSeverity.Success,
							Detail = translationState.Translate("Task_Saved"),
							Style = "position: fixed !important; left: 0; margin: auto; "
						});
						//dialogService.Close(saveTask);
						await RedirectBack();
					}
				}
			}
			catch (Exception)
			{
				notificationService.Notify(new NotificationMessage()
				{
					Severity = NotificationSeverity.Error,
					Detail = translationState.Translate("Something_Went_Wrong")
				});
			}
		}

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
		protected async Task SaveTempAttachementToUploadedDocument()
        {
            try
            {
                List<Guid> requestIds = new List<Guid>
                {
                    saveTask.Task.TaskId
                };

                var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                {
                    RequestIds = requestIds,
                    CreatedBy = saveTask.Task.CreatedBy,
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
		#endregion
	}
}
