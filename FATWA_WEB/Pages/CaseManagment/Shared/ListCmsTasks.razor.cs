using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.TaskEnums;

namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    //<History Author = "Hassan Abbas" Date="2023-03-08" Version="1.0" Branch="master">List of Tasks related to Case Management filtered based on screen, task type and submodule</History>

    public partial class ListCmsTasks : ComponentBase
    {
		#region Parameter
		[Parameter]
		public dynamic? SubModule { get; set; }
        [Parameter]
        public dynamic? Screen { get; set; }
		public int SubModuleId { get { return Convert.ToInt32(SubModule); } set { SubModule = value; } }
		public int ScreenId { get { return Convert.ToInt32(Screen); } set { Screen = value; } }
		#endregion

		#region Variables Declarations
		private UserTask saveTask = new UserTask();
		protected List<UserTaskStatus> TaskStatuses = new List<UserTaskStatus>();

		protected RadzenDropDown<int?> ddlStatus;
		public bool StatusTrue { get; set; }
		public bool Statusfalse { get; set; }

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
					var args = new PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
					_search = value;
					OnPropertyChanged(args);
					Reload();
				}
			}
		}

		protected RadzenDataGrid<TaskVM> grid = new RadzenDataGrid<TaskVM>();
		protected IEnumerable<TaskVM> getTaskList { get; set; }
		protected IEnumerable<TaskVM> FilteredGetTaskList { get; set; }=new List<TaskVM>();
		public int count { get; set; } = 0;
		public bool isVisible { get; set; }
		protected AdvanceSearchTaskVM advanceSearchVM = new AdvanceSearchTaskVM { TaskStatusId = (int)TaskStatusEnum.Pending};
		protected bool Keywords = false;

		#endregion

		#region Task Status
		protected async Task PopulateTaskStatuses()
		{
			var response = await lookupService.GetTaskStatuses();

			if (response.IsSuccessStatusCode)
			{
				TaskStatuses = (List<UserTaskStatus>)response.ResultData;
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}
		}
		#endregion

		#region On Load

		protected override async Task OnInitializedAsync()
		{
			spinnerService.Show();

			await PopulateTaskStatuses();
			await Load();
			translationState.TranslateGridFilterLabels(grid);

			spinnerService.Hide();
		}

		protected async Task Load()
		{
			if (string.IsNullOrEmpty(search))
			{
				search = "";
			}
			else
			{
				search = search.ToLower();
			}
			advanceSearchVM.SubModuleId = SubModuleId;
			advanceSearchVM.ScreenId = ScreenId;
            var result = await taskService.GetCmsTasksList(advanceSearchVM);
			if (result.IsSuccessStatusCode)
			{
				getTaskList = (IEnumerable<TaskVM>)result;
				FilteredGetTaskList = (IEnumerable<TaskVM>)result;
				count = getTaskList.Count();
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(result);
			}
		}
		protected async Task OnSearchInput()
		{
			try
			{
                if (string.IsNullOrEmpty(search))
                {
                    search = "";
                }
                else
                {
                    search = search.ToLower();
                }
                FilteredGetTaskList = await gridSearchExtension.Filter(getTaskList, new Query()
                {
                    Filter = $@"i => ( (i.Name != null && i.Name.ToLower().Contains(@0)) || (i.Description != null && i.Description.ToLower().Contains(@1)) || (i.AssignedBy != null && i.AssignedBy.ToLower().Contains(@2)) )",
                    FilterParameters = new object[] { search, search, search }
                });
            }
			catch (Exception ex)
			{
				throw ex;
			}
		}
            #endregion

            #region Functions

            public void Reload()
		{
			InvokeAsync(StateHasChanged);
		}

		public void OnPropertyChanged(PropertyChangedEventArgs args)
		{
		}

		protected void DetailTask(TaskVM task)
		{
			navigationManager.NavigateTo("/usertask-detail/" + task.TaskId);
		}

		protected void TaskResponse(TaskVM task)
		{
			navigationManager.NavigateTo("/taskresponse-decision/" + task.TaskId);
		}

		protected void DetailTaskDetails(TaskVM task)
        {
            navigationState.ReturnUrl = "usertask-list";
            if (task.Url.StartsWith("caserequest-transfer-review") || task.Url.StartsWith("caserequest-copy-review") || task.Url.StartsWith("casefile-copy-review") || task.Url.StartsWith("casefile-transfer-review") ||
                 task.Url.StartsWith("mergerequest-view") || task.Url.StartsWith("draftdocument-detail") || task.Url.StartsWith("executionrequest-detail"))
            {
                navigationManager.NavigateTo(navigationManager.BaseUri + task.Url + "/" + task.TaskId);

            }
			else
            {
                navigationManager.NavigateTo(navigationManager.BaseUri + task.Url);
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

		#endregion

		#region Advance Search

		//<History Author = 'Zain Ul Islam' Date='2022-11-30' Version="1.0" Branch="master">Open advance search popup </History>
		protected void ToggleAdvanceSearch()
		{
			isVisible = !isVisible;
		}

		//<History Author = 'Zain Ul Islam' Date='2022-11-30' Version="1.0" Branch="master">Validate the Advance Search form</History> 
		protected async Task SubmitAdvanceSearch()
		{
			if (advanceSearchVM.FromDate > advanceSearchVM.ToDate)
			{
				notificationService.Notify(new NotificationMessage()
				{
					Severity = NotificationSeverity.Error,
					Detail = translationState.Translate("FromDate_NotGreater_ToDate")
				});
				return;
			}

			Keywords = true;
			await Load();
			StateHasChanged();
		}

		public async Task ResetForm()
		{
			advanceSearchVM = new AdvanceSearchTaskVM { TaskStatusId = (int)TaskStatusEnum.Pending};
			await Load();
			Keywords = false;
			StateHasChanged();
		}

		#endregion

	}
}
