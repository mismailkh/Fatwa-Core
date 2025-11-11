using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Tasks
{
    public partial class TaskResponseDecision : ComponentBase
	{
		#region Parameters

		[Parameter]
		public dynamic taskId { get; set; }

		#endregion

		#region Variables

		TaskResponseVM saveTaskResponse = new TaskResponseVM()
		{
			TaskResponse = new TaskResponse
			{
				TaskResponseId = Guid.NewGuid() 
			},
			TaskActions = new List<TaskAction>()
		};

		string _search;
		protected string search
		{
			get
			{
				return _search;
			}
			set
			{
				if (!object.Equals(_search, value))
				{
					//var args = new PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
					_search = value;
					//OnPropertyChanged(args);
					Reload();
				}
			}
		}

		public List<UserTaskStatus> GetTaskStatus = new List<UserTaskStatus>();

		protected RadzenDataGrid<TaskAction> ActionItemGrid;
		protected List<TaskAction> GetActionItemList = new List<TaskAction>();
		protected TaskDetailVM taskDetailVM { get; set; }

		protected bool isDisabled = true;

		#endregion

		#region Load

		protected override async Task OnInitializedAsync()
		{
			spinnerService.Show();

			await PopulateDropdowns();
			await Load();

			spinnerService.Hide();
		}

		protected async Task Load()
		{
			try
			{
				saveTaskResponse.TaskResponse.TaskId =  Guid.Parse(taskId);

				var getTaskDetail = await taskService.GetTaskDetailById(Guid.Parse(taskId));
				if (getTaskDetail.IsSuccessStatusCode)
				{
					taskDetailVM = (TaskDetailVM)getTaskDetail.ResultData;
				}

				var getActionItem = await taskService.GetTaskActionsByTaskId(Guid.Parse(taskId));
				if (getActionItem.IsSuccessStatusCode)
				{
					GetActionItemList = (List<TaskAction>)getActionItem.ResultData;
				}
			}
			catch (Exception ex)
			{
				notificationService.Notify(new NotificationMessage()
				{
					Severity = NotificationSeverity.Error,
					Detail = ex.Message
				});
			}
		}

		public void Reload()
		{
			InvokeAsync(StateHasChanged);
		}

		#endregion

		#region Functions

		protected async Task EditRow(TaskAction action)
		{
			await ActionItemGrid.EditRow(action);
		}

		protected async Task SaveRow(TaskAction action)
		{
			await ActionItemGrid.UpdateRow(action);
		}

		protected void OnUpdateRow(TaskAction action)
		{
			var actionItem = GetActionItemList.FirstOrDefault(x => x.ActionId == action.ActionId);
			if (actionItem != null)
			{
				actionItem.CompleteDate = action.CompleteDate;
			}
		}

		protected async Task FormSubmit(TaskResponseVM args)
		{
			try
			{
				saveTaskResponse.TaskActions = GetActionItemList;

				bool? dialogResponse = await dialogService.Confirm(
				   translationState.Translate("Sure_Save_TaskResponse"),
				   translationState.Translate("Confirm"),
				   new ConfirmOptions()
				   {
					   OkButtonText = translationState.Translate("Yes"),
					   CancelButtonText = translationState.Translate("No")
				   });
				if (dialogResponse == true)
				{
					ApiCallResponse response = null;

					response = await taskService.AddTaskResponseDecision(saveTaskResponse);
					await SaveTempAttachementToUploadedDocument();

                    if (response.IsSuccessStatusCode)
					{
						if(saveTaskResponse.TaskResponse.TaskResponeStatusId==(int)TaskResponseStatusEnum.Rejected)
						{

						}
						notificationService.Notify(new NotificationMessage()
						{
							Severity = NotificationSeverity.Success,
							Detail = translationState.Translate("TaskResponse_Save_Success")
						});

						dialogService.Close(saveTaskResponse);
						navigationManager.NavigateTo("/usertask-list");
					}
					else
					{
						notificationService.Notify(new NotificationMessage()
						{
							Severity = NotificationSeverity.Error,
							Detail = translationState.Translate("Something_Went_Wrong")
						});
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
        protected async Task SaveTempAttachementToUploadedDocument()
        {
            try
            {
                List<Guid> requestIds = new List<Guid>
                {
                    saveTaskResponse.TaskResponse.TaskResponseId
                };

                var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                {
                    RequestIds = requestIds,
                    CreatedBy = saveTaskResponse.TaskResponse.CreatedBy,
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
        protected void ButtonCancel()
		{
			navigationManager.NavigateTo("/usertask-list");
		}

		#endregion

		#region Remote Dropdown Data and Dropdown Change Events

		//<History Author = 'Ijaz Ahmad' Date='2022-12-12' Version="1.0" Branch="master">Populate DropdownLists</History>
		protected async Task PopulateDropdowns()
		{
			await PopulateTaskStatuses();
		}

		protected async Task PopulateTaskStatuses()
		{
			var response = await lookupService.GetTaskStatuses();
			if (response.IsSuccessStatusCode)
			{
                GetTaskStatus = (List<UserTaskStatus>)response.ResultData;
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}
		}

		protected void ResponeStatusChange()
		{
			if (saveTaskResponse.TaskResponse.TaskResponeStatusId == (int)TaskStatusEnum.Rejected || saveTaskResponse.TaskResponse.TaskResponeStatusId == (int)TaskStatusEnum.Done)
			{
				isDisabled = false;
			}
			else
			{
				isDisabled = true;
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
