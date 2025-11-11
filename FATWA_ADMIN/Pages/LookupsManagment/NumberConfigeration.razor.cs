using FATWA_ADMIN.Services.General;
using FATWA_ADMIN.Services.UserManagement;
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Group = FATWA_DOMAIN.Models.AdminModels.UserManagement.Group;

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class NumberConfigeration : ComponentBase
    {

        #region Parameter
        [Parameter]
        public int PatternTypeId { get; set; }
        #endregion

        #region Service Injection

        [Inject]
        protected RoleService roleService { get; set; }


        #endregion

        #region Variable Declaration

        protected RadzenDataGrid<ClaimVM>? grid = new RadzenDataGrid<ClaimVM>();
        protected RadzenDataGrid<LegallegislationtypesVM>? grid0 = new RadzenDataGrid<LegallegislationtypesVM>();

        protected RadzenDataGrid<CmsComsNumPatternVM>? grid1 = new RadzenDataGrid<CmsComsNumPatternVM>();
        protected RadzenDataGrid<LmsLiteratureTagVM>? gridLmsLiteratureTag = new RadzenDataGrid<LmsLiteratureTagVM>();
        public Group Group = new Group();
        protected List<CmsComsNumPatternVM> cmsComsNumPattern { get; set; } = new List<CmsComsNumPatternVM>();
        public string TransKeyHeader = string.Empty;
        public string TransKeyBreadCrumbItem = string.Empty;
        public string TransKeyTitle = string.Empty;



        //  public LegalLegislationType legislationType = new LegalLegislationType();
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        protected string search { get; set; }


        IEnumerable<CmsComsNumPatternVM> CmsComsNumPattern;
        IEnumerable<CmsComsNumPatternVM> _FilteredCmsComsNumPattern;
        protected IEnumerable<CmsComsNumPatternVM> FilteredCmsComsNumPattern
        {
            get
            {
                return _FilteredCmsComsNumPattern;
            }
            set
            {
                if (!object.Equals(_FilteredCmsComsNumPattern, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredCmsComsNumPattern", NewValue = value, OldValue = _FilteredCmsComsNumPattern };
                    _FilteredCmsComsNumPattern = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }


        protected bool isLoading { get; set; }
        protected int count { get; set; }
        protected bool isCheckedRole = false;
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion

        #region Fuctions
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected override async Task OnInitializedAsync()
        {
            if (PatternTypeId == (int)CmsComsNumPatternTypeEnum.CaseRequestNumber)
            {
                TransKeyTitle = "CaseRequesNumberPattern";
                TransKeyBreadCrumbItem = "Case Request Number Pattern";
                TransKeyHeader = "CaseRequesNumberPattern";
            }
            else if (PatternTypeId == (int)CmsComsNumPatternTypeEnum.CaseFileNumber)
            {
                TransKeyTitle = "CaseFileNumberPattern";
                TransKeyBreadCrumbItem = "Case File Number Pattern";
                TransKeyHeader = "CaseFileNumberPattern";

            }
            else if (PatternTypeId == (int)CmsComsNumPatternTypeEnum.ConsultationRequestNumber)
            {
                TransKeyTitle = "ConsultationRequestNumberPattern";
                TransKeyBreadCrumbItem = "Consultation Request Number Pattern";
                TransKeyHeader = "ConsultationRequestNumberPattern";

            }
            else if (PatternTypeId == (int)CmsComsNumPatternTypeEnum.ConsultationFileNumber)
            {
                TransKeyTitle = "ConsultationFileNumberPattern";
                //TransKeyBreadCrumbItem = "List_Legislations_File";
                TransKeyHeader = "ConsultationFileNumberPattern";

            }
            else if (PatternTypeId == (int)CmsComsNumPatternTypeEnum.InboxNumber)
            {
                TransKeyTitle = "InboxNumberPattern";
                //TransKeyBreadCrumbItem = "List_Administrative_Complaints_File";
                TransKeyHeader = "InboxNumberPattern";

            }
            else if (PatternTypeId == (int)CmsComsNumPatternTypeEnum.OutboxNumber)
            {
                TransKeyTitle = "OutboxNumberPattern";
                //TransKeyBreadCrumbItem = "OutboxNumberPattern";
                TransKeyHeader = "OutboxNumberPattern";

            }
            await Load();

        }
        protected async Task Load()
        {
            spinnerService.Show();
            await GetAllCaseRequestNumber(PatternTypeId);
            spinnerService.Hide();
        }
        #endregion

        #region Get All CASE Request Number Lists 
        protected async Task GetAllCaseRequestNumber(int PatternTypeId)
        {
            var result = await lookupService.GetAllCaseRequestNumber(PatternTypeId);
            if (result.IsSuccessStatusCode)
            {
                CmsComsNumPattern = (IEnumerable<CmsComsNumPatternVM>)result.ResultData;
                FilteredCmsComsNumPattern = (IEnumerable<CmsComsNumPatternVM>)result.ResultData;
                count = CmsComsNumPattern.Count();

                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
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
                    FilteredCmsComsNumPattern = await gridSearchExtension.Filter(CmsComsNumPattern, new Query()
                    {
                        Filter = $@"i => ((i.Name_En != null && i.Name_En.ToLower().Contains(@0)) ||(i.SequanceResult != null && i.SequanceResult.ToLower().Contains(@1))|| (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").ToLower().Contains(@2))|| (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@3)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@4)))",
                        FilterParameters = new object[] { search, search, search, search, search }
                    });  await InvokeAsync(StateHasChanged);
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

        #region Number Pattern Action Button Detail
        #region Add Number Pattern
        protected async Task Button1Click(MouseEventArgs args)
        {

            try
            {
                int value = 1;
                if (await dialogService.OpenAsync<CaseFileNumberAdd>(
                    @translationState.Translate("Add_Number_Patterns"),
                    new Dictionary<string, object>
                    {
                    { "Selectedvalue", value },
                    { "SelectedPatternTypeId", PatternTypeId }
                    },
                    new DialogOptions { CloseDialogOnOverlayClick = false, Width = "75%" }
                ) == true)
                {
                    await Load();
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
        #region Edit Number Pattern
        protected async Task EditItem(CmsComsNumPatternVM args)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<CaseFileNumberAdd>(
                translationState.Translate("Edit_Number_Patterns"),
                new Dictionary<string, object>() {
                    { "Id", args.Id },
                    { "SelectedPatternTypeId", PatternTypeId },
                    { "IsDefault", args.IsDefault }
                },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false,
                    Width = "75%"
                });
                StateHasChanged();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
        #region Delete Number Pattern
        protected async Task DeleteItem(CmsComsNumPatternVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Delete_Pattern"),
                translationState.Translate("Delete"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();

                    args.PatternTypeId = PatternTypeId;
                    var response = await lookupService.DeleteCmComsPattern(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await Load();
                        StateHasChanged();
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("An_unexpected_error_occurred") + $": {response.StatusCode}",
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }
                    spinnerService.Hide();
                }
            }
            catch (Exception ex)
            {
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("An_unexpected_error_occurred") + ": " + ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
        #region Active Number Pattern
        protected async Task GridActiveButtonClick1(LegalPublicationSourceNameVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_you_want_to_update_Status"),
                    translationState.Translate("Status"), new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    }) == true)
                {
                    args.IsActive = args.IsActive ? false : true;
                    spinnerService.Show();

                    var response = await lookupService.ActivePublicationSourceNames(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await Load();
                        StateHasChanged();
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("An_unexpected_error_occurred") + $": {response.StatusCode}",
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }
                    spinnerService.Hide();
                }
            }
            catch (Exception ex)
            {

                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("An_unexpected_error_occurred") + ": " + ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }
        #endregion
        #endregion

        #region Row Cell Render for Default pattern color coding
        protected void RowCellRender(RowRenderEventArgs<CmsComsNumPatternVM> CmsComsNumPattern)
        {
            if (CmsComsNumPattern.Data.IsDefault == true)
            {
                CmsComsNumPattern.Attributes.Add("style", $"background-color: #90EE90;");
            }
        }
        #endregion

    }
}
