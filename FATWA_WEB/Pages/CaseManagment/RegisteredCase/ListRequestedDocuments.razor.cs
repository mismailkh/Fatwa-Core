using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    public partial class ListRequestedDocuments : ComponentBase
    {
        #region Variables
        protected RadzenDataGrid<CmsRequestDocumentsVM> grid;
        protected bool Keywords = false;
        public bool isVisible { get; set; }

        public IList<CmsRequestDocumentsVM> selectedFiles;

        #endregion

        IEnumerable<CmsRequestDocumentsVM> _getRequestedDocuments;
        IEnumerable<CmsRequestDocumentsVM>  FilteredGetRequestedDocuments;
        protected IEnumerable<CmsRequestDocumentsVM> getRequestedDocuments
        {
            get
            {
                return _getRequestedDocuments;
            }
            set
            {
                if (!Equals(_getRequestedDocuments, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getRequestedDocuments", NewValue = value, OldValue = getRequestedDocuments };
                    _getRequestedDocuments = value;

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
                if (!Equals(_search, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;

                    Reload();
                }
            }
        }

        protected async Task Load()
        {
            try
            {
                var response = await cmsRegisteredCaseService.GetRequestedDocuments();
                if (response.IsSuccessStatusCode)
                {
                    getRequestedDocuments = (IEnumerable<CmsRequestDocumentsVM>)response.ResultData;
                    FilteredGetRequestedDocuments = (IEnumerable<CmsRequestDocumentsVM>)response.ResultData;
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
        protected async Task OnSearchInput()
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                {
                    search = "";
                }
                else
                    search = search.ToLower();
                string Filter;
                if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                {
                    Filter = $@"i => ( i.CANNumber != null && i.CANNumber.ToString().ToLower().Contains(@0) ) || ( i.DocumentTypeEn != null && i.DocumentTypeEn.ToLower().Contains(@1))";
                }
                else
                {
                    Filter = $@"i => ( i.CANNumber != null && i.CANNumber.ToString().ToLower().Contains(@0) ) || ( i.DocumentTypeAr != null && i.DocumentTypeAr.ToLower().Contains(@1))";
                }
                FilteredGetRequestedDocuments=await gridSearchExtension.Filter(getRequestedDocuments,new Query()
                {
                    Filter = Filter,
                    FilterParameters = new object[] { search, search }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Redirect Function
        //<History Author = 'Ijaz Ahmad' Date='2022-12-13' Version="1.0" Branch="master"> Redirect Function </History>

        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        protected async Task RequestedDocumentDetails(CmsRequestDocumentsVM args)
        {
            navigationManager.NavigateTo("requested-documents-list-detail/" + args.CaseId);
        }
        #endregion

    }
}
