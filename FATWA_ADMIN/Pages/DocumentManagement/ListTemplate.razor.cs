using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace FATWA_ADMIN.Pages.DocumentManagement
{
    public partial class ListTemplate : ComponentBase
    {

        #region varaiable Declaration
        protected RadzenDataGrid<DMSTemplateListVM>? grid;
        protected bool Keywords = false;
        public bool isVisible { get; set; }
        protected TemplateListAdvanceSearchVM advanceSearchVM { get; set; } = new TemplateListAdvanceSearchVM();
        protected List<AttachmentType> attachmentTypes { get; set; } = new List<AttachmentType>();
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion

        #region Full property declaration

        IEnumerable<DMSTemplateListVM> _templateList;
        protected IEnumerable<DMSTemplateListVM> FilteredTemplateList { get; set; } = new List<DMSTemplateListVM>();
        protected IEnumerable<DMSTemplateListVM> templateList
        {
            get
            {
                return _templateList;
            }
            set
            {
                if (!object.Equals(_templateList, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "templateList", NewValue = value, OldValue = _templateList };
                    _templateList = value;

                    Reload();
                }
            }

        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
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
                    var args = new PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;
                    Reload();
                }
            }
        }
        #endregion

        #region On Initialize
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            await PopulateAttachmentTypes();
            translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
        }
        #endregion

        #region On Load

        protected async Task Load()
        {
            try
            {
                var response = await fileUploadService.GetTemplatesList(advanceSearchVM);
                if (response.IsSuccessStatusCode)
                {
                    templateList = (IEnumerable<DMSTemplateListVM>)response.ResultData;
                    FilteredTemplateList = (IEnumerable<DMSTemplateListVM>)response.ResultData;
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
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                    {
                        FilteredTemplateList = await gridSearchExtension.Filter(templateList, new Query()
                        {
                            Filter = $@"i => (i.NameEn != null && i.NameEn.ToString().Contains(@0)) || (i.AttachmentTypeEn != null && i.AttachmentTypeEn.ToString().ToLower().Contains(@1))",
                            FilterParameters = new object[] { search, search }

                        });
                    }
                    else
                    {
                        FilteredTemplateList = await gridSearchExtension.Filter(templateList, new Query()
                        {
                            Filter = $@"i => (i.NameAr != null && i.NameAr.ToString().Contains(@0)) || (i.AttachmentTypeAr != null && i.AttachmentTypeAr.ToString().ToLower().Contains(@1))",
                            FilterParameters = new object[] { search, search }

                        });
                    }
                     await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
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
        public async void ResetForm()
        {

            advanceSearchVM = new TemplateListAdvanceSearchVM();

            await Load();
            Keywords = false;
            StateHasChanged();
        }
        protected async Task ToggleAdvanceSearch()
        {
            isVisible = !isVisible;
            if (!isVisible)
            {
                ResetForm();
            }
        }

        protected async Task SubmitAdvanceSearch()
        {
            if (advanceSearchVM.CreatedFrom > advanceSearchVM.CreatedTo)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                Keywords = true;
                return;
            }
            if (advanceSearchVM.AttachmentTypeId == 0 && string.IsNullOrWhiteSpace(advanceSearchVM.Templatename)
                 && !advanceSearchVM.CreatedFrom.HasValue && !advanceSearchVM.CreatedTo.HasValue)
            {

            }
            else
            {
                Keywords = true;

                await Load();
                StateHasChanged();
            }
        }

        #endregion

        #region populate Attachment type data
        //<History Author = 'Muhammad Zaeem' Date='2023-03-14' Version="1.0" Branch="master">Populate Count data</History>
        protected async Task PopulateAttachmentTypes()
        {
            var response = await fileUploadService.GetAllAttachmentTypes();
            if (response.IsSuccessStatusCode)
            {
                attachmentTypes = (List<AttachmentType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion

        #region Add/Edit Template Button
        protected async Task AddTemplate()
        {
            navigationManager.NavigateTo("/template-add");
        }
        protected async Task EditTemplate(DMSTemplateListVM args)
        {
            navigationManager.NavigateTo("/template-add/" + args.TemplateId);
        }
        #endregion

        #region Template Status
        protected async Task UpdateTemplateStatus(DMSTemplateListVM template)
        {
            if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_you_want_to_update_Status"), translationState.Translate("Confirm")) == true)
            {
                spinnerService.Show();
                var response = await fileUploadService.UpdateTemplateStatus(template.isActive, template.TemplateId);
                if (response == true)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Updated_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
                    StateHasChanged();
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });

                }
                spinnerService.Hide();
            }

            else
            {
                if (template.isActive == false)
                    template.isActive = true;
                else
                    template.isActive = false;
            }

        }
        #endregion

    }
}
