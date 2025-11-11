using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.Consultation.Shared
{
    //<History Author = "Muhammad Zaeem" Date="2023-03-08" Version="1.0" Branch="master">List of Tasks related to Consultation Management filtered based on screen, task type and submodule</History>

    public partial class ListComsTasks : ComponentBase
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
		public int count { get; set; } = 0;
		public bool isVisible { get; set; }
		protected AdvanceSearchTaskVM advanceSearchVM = new AdvanceSearchTaskVM();
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
            var result = await taskService.GetComsTasksList(advanceSearchVM, new Query()
			{
				Filter = $@"i => ( (i.Name != null && i.Name.ToLower().Contains(@0)) || (i.Description != null && i.Description.ToLower().Contains(@1)) || (i.AssignedBy != null && i.AssignedBy.ToLower().Contains(@2)) )",
				FilterParameters = new object[] { search, search, search }
			});

			getTaskList = result;
			count = getTaskList.Count();
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
			//To remove Web url from URL
			
			navigationState.ReturnUrl = "usertask-list";
			if (task.Url.StartsWith("consultationrequest-transfer-review") || task.Url.StartsWith("consultationfile-transfer-review") || task.Url.StartsWith("draftdocument-detail/"))
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

		//<History Author = 'Muhammad Zaeem' Date='2023-10-03' Version="1.0" Branch="master">Open advance search popup </History>
		protected void ToggleAdvanceSearch()
		{
			isVisible = !isVisible;
		}

		//<History Author = 'Muhammad Zaeem' Date='2023-10-03' Version="1.0" Branch="master">Validate the Advance Search form</History> 
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
			advanceSearchVM = new AdvanceSearchTaskVM();
			await Load();
			Keywords = false;
			//TaskStatuses = null;
			advanceSearchVM.TaskStatusId = null;
			StateHasChanged();
		}

		#endregion

	}
}
