using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    //<History Author = 'Hassan Abbas' Date='2022-11-29' Version="1.0" Branch="master"> List of Merge Requests For Cases</History>
    public partial class ListMergeRequests : ComponentBase
    {

        #region Variables
        protected RadzenDataGrid<MergeRequestVM>? grid;
        protected bool Keywords = false;
        public int selectedIndex { get; set; } = 0;
        #endregion


        IEnumerable<MergeRequestVM> _getMergeRequests;
        protected IEnumerable<MergeRequestVM> getMergeRequests
        {
            get
            {
                return _getMergeRequests;
            }
            set
            {
                if (!object.Equals(_getMergeRequests, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getMergeRequests", NewValue = value, OldValue = _getMergeRequests };
                    _getMergeRequests = value;

                    Reload();
                }
            }
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
        }
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
                    var args = new FATWA_WEB.Services.PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;

                    Reload();
                }
            }
        }
        protected async Task Load()
        {
            try
            {
                var response = await cmsRegisteredCaseService.GetMergeRequestsForApproval();

                if (response.IsSuccessStatusCode)
                {
                    getMergeRequests = (IEnumerable<MergeRequestVM>)response.ResultData;
                    await InvokeAsync(StateHasChanged);
                }

                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region GRID Buttons
        //<History Author = 'Hassan Abbas' Date='2022-11-29' Version="1.0" Branch="master"> Redirect to View Detail page</History>
        protected async Task Detail(MergeRequestVM args)
        {
            navigationState.ReturnUrl = "merge-requests";
            navigationManager.NavigateTo("merge-request-detail/" + args.Id);
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
    }
}
