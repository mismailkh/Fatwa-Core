using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CMSCOMSRequestNumberVMs;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_GENERAL.Helper;
using FATWA_WEB.Data;
using FATWA_WEB.Pages.Consultation.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.PdfViewer;
using Syncfusion.Blazor.PdfViewerServer;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Consultation.Request
{
    public partial class AddConsultationRequest : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic ConsultationRequestId { get; set; }

        #endregion

        #region Constructor
        public AddConsultationRequest()
        {
            RequestTypes = new List<RequestType>();
            tabPosition = TabPosition.Top;
            TemplateDetails = new CaseTemplate();
            SelectedRequestSubtypes = new List<Subtype>();
            Priorities = new List<Priority>();
            GovtEntities = new List<GovernmentEntity>();
            _templateList = new List<DMSTemplateListVM>();
            advanceSearchVM = new TemplateListAdvanceSearchVM();
            LegislationFileTypesDetails = new List<ConsultationLegislationFileType>();
            InternationalArbitrationList = new List<ComsInternationalArbitrationType>();
            HeaderFooterTemplates = new List<CaseTemplate>();
            Designations = new List<Designation>();
            Departments = new List<Department>();
            RequestStatuses = new List<CaseRequestStatus>();
            Subtypes = new List<Subtype>();
            ConsultationTemplates = new List<ConsultationTemplate>();
        }
        #endregion


        #region Varaiable Declaraton
        protected List<RequestType> RequestTypes { get; set; }
        TabPosition tabPosition;
        public bool ShowDocumentViewer { get; set; }
        public TelerikPdfViewer? PdfViewerRef { get; set; }
        public byte[] FileData { get; set; }
        protected CaseTemplate TemplateDetails { get; set; }
        protected RadzenHtmlEditor? editor;
        protected List<Subtype> SelectedRequestSubtypes { get; set; }
        protected bool ShowConsultationPriority { get; set; }
        protected List<Priority> Priorities { get; set; }
        protected IEnumerable<DMSTemplateListVM> _templateList { get; set; }
        protected IEnumerable<DMSTemplateListVM> _templateFilteredList { get; set; }
        protected TemplateListAdvanceSearchVM advanceSearchVM { get; set; }
        protected IEnumerable<GovernmentEntity> GovtEntities { get; set; }
        public DateTime Min = new DateTime(1950, 1, 1);
        protected List<ConsultationLegislationFileType> LegislationFileTypesDetails { get; set; }
        protected List<ComsInternationalArbitrationType> InternationalArbitrationList { get; set; }
        public bool busyPreviewBtn { get; set; }
        public bool isVisible { get; set; }
        public byte[] PreviewFileData { get; set; }
        protected bool PledgeErrorShow { get; set; } = false;
        protected List<CaseTemplate> HeaderFooterTemplates { get; set; }
        protected List<Designation> Designations { get; set; }
        protected List<Department> Departments { get; set; }
        protected List<CaseRequestStatus> RequestStatuses { get; set; }
        public bool IsConfidentialDisabled;
        protected List<Subtype> Subtypes { get; set; }
        protected List<ConsultationTemplate> ConsultationTemplates { get; set; }
        public string TransKeyHeader = string.Empty;
        protected ConsultationAssignment consultationRequestLawyerAssignment = new ConsultationAssignment();
        public SfPdfViewerServer PdfViewerPreview;

        public PdfViewerToolbarSettings ToolbarSettings = new PdfViewerToolbarSettings()
        {
            ToolbarItems = new List<ToolbarItem>()
            {
                ToolbarItem.MagnificationTool,
                ToolbarItem.SelectionTool,
                ToolbarItem.UndoRedoTool,
                ToolbarItem.SearchOption,
                ToolbarItem.PanTool
            },
            AnnotationToolbarItems = new List<AnnotationToolbarItem>()
            {
                AnnotationToolbarItem.ShapeTool,
                AnnotationToolbarItem.ColorEditTool
            }
        };
        public string DocumentPathPreview { get; set; } = string.Empty;
        public string DocumentPathPreviewPdf { get; set; } = string.Empty;
        private DotNetObjectReference<AddConsultationRequest>? dotNetRef;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            if (dotNetRef is null)
            {
                dotNetRef ??= DotNetObjectReference.Create(this);
                await JSRuntime.InvokeVoidAsync("attachUnloadSaveListenerForCOMS", dotNetRef);
            }
            draftHandlerService.SaveDraftForConsultation = SaveAsDraftForConsultation;
            PopulateTransationKey();
            await PopulateDropdowns();
            await GetAllTemplateList();
            if (ConsultationRequestId == null)
            {
                //check if user is redirecting from List View
                if (dataCommunicationService?.consultationRequest != null)
                {
                    dataCommunicationService.consultationRequest.SectorTypeId = loginState.UserDetail.SectorTypeId;
                    dataCommunicationService.consultationRequest.RequestStatusId = (int)CaseRequestStatusEnum.Draft;
                    //by Default Consultation Request Priorities is Low
                    dataCommunicationService.consultationRequest.PriorityId = (int)PriorityEnum.Low;
                    //populate new request number

                    var requestType = (Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? RequestTypes.Where(x => x.Id == dataCommunicationService.consultationRequest.RequestTypeId).FirstOrDefault()?.Name_En : RequestTypes.Where(x => x.Id == dataCommunicationService.consultationRequest.RequestTypeId).FirstOrDefault()?.Name_Ar);
                    dataCommunicationService.consultationRequest.RequestTitle = $"{dataCommunicationService.consultationRequest.RequestDate.ToString("dd-MM-yyyy")} {requestType} {dataCommunicationService.consultationRequest.RequestNumber}";
                    if (dataCommunicationService.consultationRequest.RequestTypeId == (int)RequestTypeEnum.LegalAdvice || dataCommunicationService.consultationRequest.RequestTypeId == (int)RequestTypeEnum.InternationalArbitration)
                    {
                        dataCommunicationService.consultationRequest.IsConfidential = true;

                    }
                }
                else
                {
                    navigationManager.NavigateTo("/consultationfile-list/");
                }
                await OnChangeGovernmentEntity();

            }
            else
            {
                var response = await consultationRequestService.GetConsultationById(Guid.Parse(ConsultationRequestId));
                if (response.IsSuccessStatusCode)
                {
                    dataCommunicationService.consultationRequest = (ConsultationRequest)response.ResultData;
                    if (dataCommunicationService.consultationRequest.RequestTypeId == (int)RequestTypeEnum.Contracts)
                    {
                        TemplateDetails.Content = dataCommunicationService.consultationRequest.TemplateContent;
                        await PopulateSectorSubtypes();
                        await OnContractTypeChange(dataCommunicationService.consultationRequest.RequestSubTypeId);
                    }
                    if (dataCommunicationService.consultationRequest.RequestTypeId == (int)RequestTypeEnum.LegalAdvice)
                    {
                        await PopulateSectorSubtypes();
                    }
                    if (dataCommunicationService.consultationRequest.PriorityId == (int)PriorityEnum.High)
                    {
                        ShowConsultationPriority = true;
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            await LoadAuthorityLetter();
            if (dataCommunicationService?.consultationRequest != null)
            {
                await GetNewConsultationFileNumber();
            }
            StateHasChanged();
            spinnerService.Hide();
        }
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

        #region Remote Dropdown Data and Dropdown Change Events
        //<History Author = 'Muhammad Zaeem' Date='2024-10-10' Version="1.0" Branch="master">Populate Govt, Department, Priority, Request Type and Sub Type data</History>
        protected async Task PopulateDropdowns()
        {

            await PopulateConsultationRequestStatuses();
            await PopulateGovtEntities();
            await PopulateDepartments();
            await PopulateDesignations();
            await PopulatePriorities();
            await PopulateRequestTypes();
            await PopulateSectorSubtypes();
            await GetConsultationTemplate();
            await GetConsultationInternationalArbitrationTypes();
            await PopulateHeaderFooter();
            await PopulateSupervisorId();

            if (dataCommunicationService.consultationRequest?.RequestTypeId == (int)RequestTypeEnum.Legislations)
            {
                await GetConsultationLegislationFileTypes();
            }
        }

        private async Task GetConsultationInternationalArbitrationTypes()
        {
            var response = await lookupService.GetConsultationInternationalArbitrationTypes();
            if (response.IsSuccessStatusCode)
            {
                InternationalArbitrationList = (List<ComsInternationalArbitrationType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        //<History Author = 'Muhammad Zaeem' Date='2024-10-10' Version="1.0" Branch="master">Get Consultation Legislation File Types details</History>
        protected async Task GetConsultationLegislationFileTypes()
        {
            var response = await lookupService.GetConsultationLegislationFileTypes();
            if (response.IsSuccessStatusCode)
            {
                LegislationFileTypesDetails = (List<ConsultationLegislationFileType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        //<History Author = 'Muhammad Zaeem' Date='2024-10-10' Version="1.0" Branch="master">Get template details</History>
        protected async Task GetConsultationTemplate()
        {
            var response = await consultationRequestService.GetConsultationTemplate();
            if (response.IsSuccessStatusCode)
            {
                ConsultationTemplates = (List<ConsultationTemplate>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }


        //<History Author = 'Muhammad Zaeem' Date='2024-10-10' Version="1.0" Branch="master">Populate  request types data</History>
        protected async Task PopulateRequestTypes()
        {
            var response = await lookupService.GetRequestTypes();
            if (response.IsSuccessStatusCode)
            {
                RequestTypes = (List<RequestType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        //<History Author = 'Muhammad Zaeem' Date='2024-10-10' Version="1.0" Branch="master">Populate Request Subtypes data</History>
        protected async Task PopulateSectorSubtypes()
        {
            var response = await lookupService.GetAllRequestSubtypes();
            if (response.IsSuccessStatusCode)
            {
                Subtypes = (List<Subtype>)response.ResultData;
                SelectedRequestSubtypes = Subtypes.Where(x => x.RequestTypeId == dataCommunicationService?.consultationRequest?.RequestTypeId).ToList();
                if (dataCommunicationService?.consultationRequest?.RequestSubTypeId != null)
                {
                    dataCommunicationService.consultationRequest.RequestSubTypeId = Subtypes.FirstOrDefault(x => x.Id == dataCommunicationService?.consultationRequest?.RequestSubTypeId && x.RequestTypeId == dataCommunicationService?.consultationRequest?.RequestTypeId)?.Id;
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }


        //<History Author = 'Muhammad Zaeem' Date='2024-10-10' Version="1.0" Branch="master">Populate Case Priorities</History>
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

        //<History Author = 'Muhammad Zaeem' Date='2024-10-10' Version="1.0" Branch="master">Populate Govt Entities</History>
        protected async Task PopulateGovtEntities()
        {
            var response = await lookupService.GetGovernmentEntities();
            if (response.IsSuccessStatusCode)
            {
                GovtEntities = (List<GovernmentEntity>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        //<History Author = 'Muhammad Zaeem' Date='2024-10-10' Version="1.0" Branch="master">Populate Consultation statuses</History>
        protected async Task PopulateConsultationRequestStatuses()
        {
            var response = await lookupService.GetCaseRequestStatuses();
            if (response.IsSuccessStatusCode)
            {
                RequestStatuses = (List<CaseRequestStatus>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        //<History Author = 'Muhammad Zaeem' Date='2024-10-10' Version="1.0" Branch="master">Populate Departments</History>
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
            StateHasChanged();
        }
        //<History Author = 'Muhammad Zaeem' Date='2024-10-10' Version="1.0" Branch="master">Populate Designations</History>
        protected async Task PopulateDesignations()
        {
            var response = await lookupService.GetDesignationList();
            if (response.IsSuccessStatusCode)
            {
                Designations = (List<Designation>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        protected async Task PopulateHeaderFooter()
        {
            var response = await fileUploadService.GetHeaderFooter();
            if (response.IsSuccessStatusCode)
            {
                HeaderFooterTemplates = (List<CaseTemplate>)response.ResultData;
            }
        }
        protected void PopulateTransationKey()
        {
            if (dataCommunicationService.consultationRequest?.RequestTypeId == (int)OperatingSectorTypeEnum.Contracts)
            {
                TransKeyHeader = "Contracts_File";


            }
            else if (dataCommunicationService.consultationRequest?.RequestTypeId == (int)OperatingSectorTypeEnum.LegalAdvice)
            {
                TransKeyHeader = "Legal_Advice_File";


            }
            else if (dataCommunicationService.consultationRequest?.RequestTypeId == (int)OperatingSectorTypeEnum.InternationalArbitration)
            {
                TransKeyHeader = "International_Arbitration_File";

            }
            else if (dataCommunicationService.consultationRequest?.RequestTypeId == (int)OperatingSectorTypeEnum.AdministrativeComplaints)
            {
                TransKeyHeader = "Administrative_Complaints_File";

            }
            else if (dataCommunicationService.consultationRequest?.RequestTypeId == (int)OperatingSectorTypeEnum.Legislations)
            {
                TransKeyHeader = "List_Legislations_File";


            }
            else
            {
                if (dataCommunicationService.consultationRequest?.RequestTypeId == (int)RequestTypeEnum.Legislations)
                {
                    TransKeyHeader = "List_Legislations_File";

                }
                if (dataCommunicationService.consultationRequest?.RequestTypeId == (int)RequestTypeEnum.Contracts)
                {
                    TransKeyHeader = "Contracts_File";

                }
                if (dataCommunicationService.consultationRequest?.RequestTypeId == (int)RequestTypeEnum.InternationalArbitration)
                {
                    TransKeyHeader = "International_Arbitration_File";

                }
                if (dataCommunicationService.consultationRequest?.RequestTypeId == (int)RequestTypeEnum.AdministrativeComplaints)
                {
                    TransKeyHeader = "Administrative_Complaints_File";

                }
                if (dataCommunicationService.consultationRequest?.RequestTypeId == (int)RequestTypeEnum.LegalAdvice)
                {
                    TransKeyHeader = "Legal_Advice_File";

                }
            }
        }

        #endregion

        #region On Change Government Entity
        protected async Task OnChangeGovernmentEntity()
        {
            try
            {
                if (dataCommunicationService.consultationRequest != null)
                {
                    if (!(dataCommunicationService.consultationRequest.RequestTypeId == (int)RequestTypeEnum.InternationalArbitration || dataCommunicationService.consultationRequest.RequestTypeId == (int)RequestTypeEnum.LegalAdvice))
                    {
                        dataCommunicationService.consultationRequest.IsConfidential = GovtEntities.Where(x => x.EntityId == dataCommunicationService.consultationRequest.GovtEntityId).Select(y => y.IsConfidential).FirstOrDefault();
                        IsConfidentialDisabled = dataCommunicationService.consultationRequest.IsConfidential == true;
                    }
                }
            }
            catch (Exception)
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
        #region Get New Consultation request NUmber
        protected async Task GetNewConsultationFileNumber()
        {
            var response = await consultationRequestService.GetConsultationRequestFileNumber(0, (int)CmsComsNumPatternTypeEnum.ConsultationFileNumber);
            if (response.IsSuccessStatusCode)
            {
                var result = (NumberPatternResult)response.ResultData;
                dataCommunicationService.consultationRequest.ConsultationFile.FileNumber = result.GenerateRequestNumber;
                dataCommunicationService.consultationRequest.ConsultationFile.ComsFileNumberFormat = result.FormatRequestNumber;
                dataCommunicationService.consultationRequest.ConsultationFile.PatternSequenceResult = result.PatternSequenceResult;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion

        #region Consultation Priority on change
        protected async Task OnChangeConsultationPriority()
        {
            if (dataCommunicationService.consultationRequest.PriorityId != null)
            {
                if (dataCommunicationService.consultationRequest.PriorityId == (int)PriorityEnum.High)
                {
                    ShowConsultationPriority = true;
                }
                else
                {
                    ShowConsultationPriority = false;
                }
            }
            else
            {
                ShowConsultationPriority = false;
            }
        }
        #endregion

        #region Contract Type Change

        protected async Task OnContractTypeChange(object requestSubTypeId)
        {
            try
            {
                if (requestSubTypeId != null && (int)requestSubTypeId > 0)
                {
                    if (_templateList.Count() != 0)
                    {
                        _templateFilteredList = _templateList.Where(x => x.ModuleId == (int)WorkflowModuleEnum.COMSConsultationManagement && x.SubTypeId == (int)requestSubTypeId).ToList();
                        foreach (var item in _templateFilteredList)
                        {
                            if (item.TemplateId == dataCommunicationService.consultationRequest.TemplateId)
                            {
                                dataCommunicationService.consultationRequest.AttachmentTypeId = item.AttachmentTypeId;
                                dataCommunicationService.consultationRequest.PreviousAttachmentTypeId = dataCommunicationService.consultationRequest.AttachmentTypeId;
                            }
                        }
                    }
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    _templateFilteredList = new List<DMSTemplateListVM>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Get All Template List 
        protected async Task GetAllTemplateList()
        {
            try
            {
                var response = await fileUploadService.GetAllTemplates(advanceSearchVM, new Query());
                if (response.IsSuccessStatusCode)
                {
                    _templateList = (IEnumerable<DMSTemplateListVM>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
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

        #region Consultation template on change
        protected async Task OnChangeConsultationTemplate(object selectedTemplateId)
        {
            if (selectedTemplateId != null && (int)selectedTemplateId > 0)
            {
                await PopulateTemplateContent((int)selectedTemplateId);


            }

            StateHasChanged();
        }

        protected async Task PopulateTemplateContent(int selectedTemplateId)
        {
            try
            {
                dataCommunicationService.consultationRequest.AttachmentTypeId = _templateList.Where(x => x.TemplateId == selectedTemplateId).Select(y => y.AttachmentTypeId).FirstOrDefault();
                bool lang_En = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? true : false;
                var response = await cmsCaseTemplateService.GetCaseTemplateContent(selectedTemplateId, null, null);
                if (response.IsSuccessStatusCode)
                {
                    TemplateDetails = (CaseTemplate)response.ResultData;
                    if (TemplateDetails != null)
                    {
                        TemplateDetails.Content = TemplateDetails.Content.Replace("[----", "<div class=\"temp-read-only-content\" contenteditable=\"false\">");
                        TemplateDetails.Content = TemplateDetails.Content.Replace("----]", "</div>");
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                StateHasChanged();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Authority Letter

        protected async void AuthorityLetterUpdated(bool isSelected)
        {
            ShowDocumentViewer = false;
            FileData = new byte[0];
            StateHasChanged();
            await LoadAuthorityLetter(true);
        }

        //<History Author = 'Muhammad Zaeem' Date='2024-10-10' Version="1.0" Branch="master"> Load Authority Letter </History>
        protected async Task LoadAuthorityLetter(bool isTemp = false)
        {
            try
            {
                var response = new ObservableCollection<TempAttachementVM>();
                if (ConsultationRequestId == null)
                {
                    response = await fileUploadService.GetTempAttachements(dataCommunicationService.consultationRequest?.ConsultationRequestId);
                }
                else
                {
                    if (isTemp)
                        response = await fileUploadService.GetTempAttachements(dataCommunicationService.consultationRequest?.ConsultationRequestId);
                    else
                        response = await fileUploadService.GetUploadedAttachements(false, 0, Guid.Parse(ConsultationRequestId));
                }
                var authorityLetter = response?.FirstOrDefault(t => t.AttachmentTypeId == (int)AttachmentTypeEnum.OfficialLetter);
                if (authorityLetter != null)
                {
                    var physicalPath = string.Empty;
#if DEBUG
                    {
                        physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + authorityLetter.StoragePath).Replace(@"\\", @"\");

                    }
#else
{

                         // Construct the physical path of the file on the server
                        physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + authorityLetter.StoragePath).Replace(@"\\", @"\");
                        // Remove the wwwroot/Attachments part of the path to get the actual file path
                        physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
}
#endif
                    if (!string.IsNullOrEmpty(physicalPath))
                    {
                        string base64String = await DocumentEncryptionService.GetDecryptedDocumentBase64(physicalPath, authorityLetter.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                        DocumentPathPreview = "data:application/pdf;base64," + base64String;
                        ShowDocumentViewer = true;
                        StateHasChanged();
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Authority_Letter_Not_Loaded"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }

                }

            }
            catch (Exception)
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

        #region Submit Click 
        protected async Task SubmitClicked()
        {
            if (dataCommunicationService.consultationRequest?.RequestTypeId == (int)RequestTypeEnum.Contracts)
            {
                if (dataCommunicationService.consultationRequest?.PriorityId <= 0 || string.IsNullOrEmpty(dataCommunicationService.consultationRequest?.Subject) || dataCommunicationService.consultationRequest?.Subject.Length > 50 || (dataCommunicationService.consultationRequest?.Subject is not null
              && !Regex.IsMatch(dataCommunicationService.consultationRequest?.Subject, RegexPatterns.NoLeadingSpacesPattern)) || dataCommunicationService.consultationRequest?.GEOpinion?.Length > 300 || (dataCommunicationService.consultationRequest?.GEOpinion is not null
              && !Regex.IsMatch(dataCommunicationService.consultationRequest?.GEOpinion, RegexPatterns.NoLeadingSpacesTextAreaPattern)) || dataCommunicationService.consultationRequest?.Remarks?.Length > 300 || (dataCommunicationService.consultationRequest?.Remarks is not null
              && !Regex.IsMatch(dataCommunicationService.consultationRequest?.Remarks, RegexPatterns.NoLeadingSpacesTextAreaPattern)) || dataCommunicationService.consultationRequest?.HighPriorityReason?.Length > 1000 || (dataCommunicationService.consultationRequest?.HighPriorityReason is not null
              && !Regex.IsMatch(dataCommunicationService.consultationRequest?.HighPriorityReason, RegexPatterns.NoLeadingSpacesTextAreaPattern)) || dataCommunicationService.consultationRequest?.RequestSubTypeId <= 0 || dataCommunicationService.consultationRequest?.RequestSubTypeId == null || string.IsNullOrEmpty(dataCommunicationService.consultationRequest?.RequestTitle) || dataCommunicationService.consultationRequest?.TemplateId <= 0)
                {
                    await JSRuntime.InvokeVoidAsync("ScrollCaseRequestToTop");
                }
            }
            else if (dataCommunicationService.consultationRequest?.RequestTypeId == (int)RequestTypeEnum.InternationalArbitration)
            {
                if (dataCommunicationService.consultationRequest?.PriorityId <= 0 || dataCommunicationService.consultationRequest?.InternationalArbitrationTypeId <= 0 || dataCommunicationService.consultationRequest?.InternationalArbitrationTypeId == null || string.IsNullOrEmpty(dataCommunicationService.consultationRequest?.Subject) || (dataCommunicationService.consultationRequest?.Subject is not null
                && !Regex.IsMatch(dataCommunicationService.consultationRequest?.Subject, RegexPatterns.NoLeadingSpacesPattern)) || dataCommunicationService.consultationRequest?.Subject.Length > 50 || dataCommunicationService.consultationRequest?.Remarks?.Length > 300 || (dataCommunicationService.consultationRequest?.Remarks is not null
                && !Regex.IsMatch(dataCommunicationService.consultationRequest?.Remarks, RegexPatterns.NoLeadingSpacesTextAreaPattern)) || dataCommunicationService.consultationRequest?.HighPriorityReason?.Length > 1000 || (dataCommunicationService.consultationRequest?.HighPriorityReason is not null
                && !Regex.IsMatch(dataCommunicationService.consultationRequest?.HighPriorityReason, RegexPatterns.NoLeadingSpacesTextAreaPattern)) || dataCommunicationService.consultationRequest?.GEOpinion?.Length > 300 || (dataCommunicationService.consultationRequest?.GEOpinion is not null
                && !Regex.IsMatch(dataCommunicationService.consultationRequest?.GEOpinion, RegexPatterns.NoLeadingSpacesTextAreaPattern)) || string.IsNullOrEmpty(dataCommunicationService.consultationRequest?.RequestTitle))
                {
                    await JSRuntime.InvokeVoidAsync("ScrollCaseRequestToTop");
                }
            }
            else if (dataCommunicationService.consultationRequest?.RequestTypeId == (int)RequestTypeEnum.Legislations)
            {
                if (dataCommunicationService.consultationRequest?.LegislationFileTypeId <= 0 || (dataCommunicationService.consultationRequest?.LegislationFileTypeId == null || dataCommunicationService.consultationRequest?.CompetentAuthorityId == null || dataCommunicationService.consultationRequest?.CompetentAuthorityId <= 0 || string.IsNullOrEmpty(dataCommunicationService.consultationRequest?.Subject) || dataCommunicationService.consultationRequest?.Remarks?.Length > 300 || dataCommunicationService.consultationRequest?.Subject.Length > 50 || dataCommunicationService.consultationRequest?.CompetentAuthorityOpinionWithNote.Length > 300 || dataCommunicationService.consultationRequest?.HighPriorityReason?.Length > 1000)
                     || (dataCommunicationService.consultationRequest?.Subject is not null
               && !Regex.IsMatch(dataCommunicationService.consultationRequest?.Subject, RegexPatterns.NoLeadingSpacesPattern)) || (dataCommunicationService.consultationRequest?.Remarks is not null
               && !Regex.IsMatch(dataCommunicationService.consultationRequest?.Remarks, RegexPatterns.NoLeadingSpacesTextAreaPattern)) || (dataCommunicationService.consultationRequest?.CompetentAuthorityOpinionWithNote is not null
               && !Regex.IsMatch(dataCommunicationService.consultationRequest?.CompetentAuthorityOpinionWithNote, RegexPatterns.NoLeadingSpacesTextAreaPattern)) || (dataCommunicationService.consultationRequest?.HighPriorityReason is not null
               && !Regex.IsMatch(dataCommunicationService.consultationRequest?.HighPriorityReason, RegexPatterns.NoLeadingSpacesTextAreaPattern)))
                {
                    await JSRuntime.InvokeVoidAsync("ScrollCaseRequestToTop");
                }
            }
            else if (dataCommunicationService.consultationRequest?.RequestTypeId == (int)RequestTypeEnum.AdministrativeComplaints)
            {
                if (string.IsNullOrEmpty(dataCommunicationService.consultationRequest?.Subject) || dataCommunicationService.consultationRequest?.Subject.Length > 50 || (dataCommunicationService.consultationRequest?.Subject is not null
              && !Regex.IsMatch(dataCommunicationService.consultationRequest?.Subject, RegexPatterns.NoLeadingSpacesPattern)) || dataCommunicationService.consultationRequest?.Remarks?.Length > 300 || (dataCommunicationService.consultationRequest?.Remarks is not null
              && !Regex.IsMatch(dataCommunicationService.consultationRequest?.Remarks, RegexPatterns.NoLeadingSpacesTextAreaPattern)) || dataCommunicationService.consultationRequest?.HighPriorityReason?.Length > 1000 || (dataCommunicationService.consultationRequest?.HighPriorityReason is not null
              && !Regex.IsMatch(dataCommunicationService.consultationRequest?.HighPriorityReason, RegexPatterns.NoLeadingSpacesTextAreaPattern)) || string.IsNullOrEmpty(dataCommunicationService.consultationRequest?.ComplainantName) || string.IsNullOrEmpty(dataCommunicationService.consultationRequest?.ComplaintAgainst) || dataCommunicationService.consultationRequest?.CSCSubmissionDate == null || dataCommunicationService.consultationRequest?.CivilId == null || string.IsNullOrEmpty(dataCommunicationService.consultationRequest?.ComplainantDecisionNumber) || dataCommunicationService.consultationRequest?.ComplaintAgainst?.Length > 90 || (dataCommunicationService.consultationRequest?.ComplaintAgainst is not null
              && !Regex.IsMatch(dataCommunicationService.consultationRequest?.ComplaintAgainst, RegexPatterns.NoLeadingSpacesPattern)) || dataCommunicationService.consultationRequest?.ComplainantName?.Length > 90 || (dataCommunicationService.consultationRequest?.ComplainantName is not null
              && !Regex.IsMatch(dataCommunicationService.consultationRequest?.ComplainantName, RegexPatterns.NoLeadingSpacesPattern))
              || dataCommunicationService.consultationRequest?.ComplainantDecisionNumber?.Length > 90 || (dataCommunicationService.consultationRequest?.ComplainantDecisionNumber is not null
              && !Regex.IsMatch(dataCommunicationService.consultationRequest?.ComplainantDecisionNumber, RegexPatterns.NoLeadingSpacesPattern))
               )
                {
                    await JSRuntime.InvokeVoidAsync("ScrollCaseRequestToTop");
                }
            }
            else if (dataCommunicationService.consultationRequest?.RequestTypeId == (int)RequestTypeEnum.LegalAdvice)
            {
                if (dataCommunicationService.consultationRequest?.PriorityId <= 0 || string.IsNullOrEmpty(dataCommunicationService.consultationRequest?.Subject) || dataCommunicationService.consultationRequest?.Subject.Length > 50 || (dataCommunicationService.consultationRequest?.Subject is not null
               && !Regex.IsMatch(dataCommunicationService.consultationRequest?.Subject, RegexPatterns.NoLeadingSpacesPattern)) || dataCommunicationService.consultationRequest?.Remarks?.Length > 300 || (dataCommunicationService.consultationRequest?.Remarks is not null
               && !Regex.IsMatch(dataCommunicationService.consultationRequest?.Remarks, RegexPatterns.NoLeadingSpacesTextAreaPattern)) || dataCommunicationService.consultationRequest?.GEOpinion?.Length > 300 || (dataCommunicationService.consultationRequest?.GEOpinion is not null
               && !Regex.IsMatch(dataCommunicationService.consultationRequest?.GEOpinion, RegexPatterns.NoLeadingSpacesTextAreaPattern)) || dataCommunicationService.consultationRequest?.HighPriorityReason?.Length > 1000 || (dataCommunicationService.consultationRequest?.HighPriorityReason is not null
               && !Regex.IsMatch(dataCommunicationService.consultationRequest?.HighPriorityReason, RegexPatterns.NoLeadingSpacesTextAreaPattern)) || string.IsNullOrEmpty(dataCommunicationService.consultationRequest?.RequestTitle))
                {
                    await JSRuntime.InvokeVoidAsync("ScrollCaseRequestToTop");
                }
            }
            if (dataCommunicationService.consultationRequest?.GovtEntityId <= 0)
            {
                await JSRuntime.InvokeVoidAsync("ScrollCaseRequestToTop");
            }

        }

        #endregion

        #region Form Submit
        protected async Task Form0Submit(ConsultationRequest args)
        {
            try
            {
                if (dataCommunicationService.consultationRequest?.RequestTypeId == (int)RequestTypeEnum.InternationalArbitration
                    || dataCommunicationService.consultationRequest?.RequestTypeId == (int)RequestTypeEnum.Legislations
                    || dataCommunicationService.consultationRequest?.RequestTypeId == (int)RequestTypeEnum.AdministrativeComplaints
                    || dataCommunicationService.consultationRequest?.RequestTypeId == (int)RequestTypeEnum.LegalAdvice)
                {
                    if (dataCommunicationService.consultationRequest.MandatoryTempFiles.Count() <= 0 || dataCommunicationService.consultationRequest.MandatoryTempFiles.Where(f => string.IsNullOrEmpty(f.FileName)).Count() > 0)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Must_Attach_Mandatory_Documents"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        return;
                    }
                    if (dataCommunicationService.consultationRequest?.Pledge == false)
                    {
                        PledgeErrorShow = true;
                        return;
                    }
                    else
                    {
                        PledgeErrorShow = false;
                    }
                    if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    }) == true)
                    {
                        spinnerService.Show();
                        dataCommunicationService.consultationRequest.RequestStatusId = (int)CaseRequestStatusEnum.Submitted;
                        if (ConsultationRequestId == null)
                        {
                            dataCommunicationService.consultationRequest.ComplainantName = $"{dataCommunicationService.consultationRequest.ComplainantName}-{dataCommunicationService.consultationRequest.CivilId}";

                        }
                        else
                        {
                            dataCommunicationService.consultationRequest.IsEdit = true;
                        }
                        var response = await consultationRequestService.CreateConsultationRequest(dataCommunicationService.consultationRequest);
                        // Save Attachment of Consultation Request
                        List<Guid> requestIds = new List<Guid>();

                        requestIds.Add(dataCommunicationService.consultationRequest.ConsultationRequestId);


                        var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                        {
                            RequestIds = requestIds,
                            CreatedBy = dataCommunicationService.consultationRequest.CreatedBy,
                            FilePath = _config.GetValue<string>("dms_file_path"),
                            DeletedAttachementIds = dataCommunicationService.consultationRequest.DeletedAttachementIds
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
                        spinnerService.Hide();
                        if (response.IsSuccessStatusCode)
                        {
                            var caseRequestCommunicationVM = (CaseRequestCommunicationVM)response.ResultData;
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Changes_saved_successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            if (caseRequestCommunicationVM.ConsultationFile != null && caseRequestCommunicationVM.ConsultationFile.FileId != Guid.Empty)
                            {
                                caseRequestCommunicationVM.CopyAttachmentVMs.Add(new CopyAttachmentVM()
                                {
                                    SourceId = (Guid)caseRequestCommunicationVM.ConsultationRequests.ConsultationRequestId,
                                    DestinationId = (Guid)caseRequestCommunicationVM.ConsultationFile.FileId,
                                    CreatedBy = caseRequestCommunicationVM.ConsultationRequests.CreatedBy
                                });
                                var attachmnetResponse = await fileUploadService.CopyAttachmentsFromSourceToDestination(caseRequestCommunicationVM.CopyAttachmentVMs);
                                if (!attachmnetResponse.IsSuccessStatusCode)
                                {
                                    notificationService.Notify(new NotificationMessage()
                                    {
                                        Severity = NotificationSeverity.Error,
                                        Detail = translationState.Translate("Attachment_Save_Failed"),
                                        Style = "position: fixed !important; left: 0; margin: auto; "
                                    });
                                    return;
                                }
                                await AssignFileToLawyer(caseRequestCommunicationVM);

                            }
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }

                    }
                }
                else if (dataCommunicationService.consultationRequest?.RequestTypeId == (int)RequestTypeEnum.Contracts)
                {
                    if (TemplateDetails.Content == null)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Fill_Contract_Template"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        return;
                    }
                    if (dataCommunicationService.consultationRequest?.Pledge == false)
                    {
                        PledgeErrorShow = true;
                        return;
                    }
                    else
                    {
                        PledgeErrorShow = false;
                    }
                    if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    }) == true)
                    {
                        spinnerService.Show();
                        dataCommunicationService.consultationRequest.RequestStatusId = (int)CaseRequestStatusEnum.ConvertedToFile;
                        dataCommunicationService.consultationRequest.TemplateContent = TemplateDetails.Content;
                        //send create/update request
                        if (ConsultationRequestId != null)
                        {
                            dataCommunicationService.consultationRequest.IsEdit = true;
                        }
                        var response = await consultationRequestService.CreateConsultationRequest(dataCommunicationService.consultationRequest);

                        // Save Attachment of Consultation Request
                        List<Guid> requestIds = new List<Guid>();
                        requestIds.Add(dataCommunicationService.consultationRequest.ConsultationRequestId);
                        var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                        {
                            RequestIds = requestIds,
                            CreatedBy = dataCommunicationService.consultationRequest.CreatedBy,
                            FilePath = _config.GetValue<string>("dms_file_path"),
                            DeletedAttachementIds = dataCommunicationService.consultationRequest.DeletedAttachementIds
                        });

                        if (!docResponse.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Attachment_Save_Failed"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        await PopulatePdfFromHtml();
                        //Save Draft Template To Document
                        var contractDocResponse = await fileUploadService.SaveContractTemplateToDocument(dataCommunicationService.consultationRequest);
                        if (!contractDocResponse.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Attachment_Save_Failed"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            return;
                        }

                        spinnerService.Hide();
                        if (response.IsSuccessStatusCode)
                        {
                            var caseRequestCommunicationVM = (CaseRequestCommunicationVM)response.ResultData;
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Changes_saved_successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            if (caseRequestCommunicationVM.ConsultationFile != null && caseRequestCommunicationVM.ConsultationFile.FileId != Guid.Empty)
                            {
                                caseRequestCommunicationVM.CopyAttachmentVMs.Add(new CopyAttachmentVM()
                                {
                                    SourceId = (Guid)caseRequestCommunicationVM.ConsultationRequests.ConsultationRequestId,
                                    DestinationId = (Guid)caseRequestCommunicationVM.ConsultationFile.FileId,
                                    CreatedBy = caseRequestCommunicationVM.ConsultationRequests.CreatedBy
                                });
                                var attachmnetResponse = await fileUploadService.CopyAttachmentsFromSourceToDestination(caseRequestCommunicationVM.CopyAttachmentVMs);
                                if (!attachmnetResponse.IsSuccessStatusCode)
                                {
                                    notificationService.Notify(new NotificationMessage()
                                    {
                                        Severity = NotificationSeverity.Error,
                                        Detail = translationState.Translate("Attachment_Save_Failed"),
                                        Style = "position: fixed !important; left: 0; margin: auto; "
                                    });
                                    return;
                                }
                                await AssignFileToLawyer(caseRequestCommunicationVM);

                            }
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                    }
                }
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
        #region Assign File To Lawyer
        protected async Task AssignFileToLawyer(CaseRequestCommunicationVM caseRequestCommunicationVM)
        {
            try
            {
                if (loginState.UserRoles.Any(x => x.RoleId == SystemRoles.ComsLawyer))
                {
                    consultationRequestLawyerAssignment.Id = Guid.NewGuid();
                    consultationRequestLawyerAssignment.LawyerId = loginState.UserDetail.UserId;
                    consultationRequestLawyerAssignment.FatwaPriorityId = dataCommunicationService.consultationRequest.PriorityId;
                    if (consultationRequestLawyerAssignment.Remarks == null)
                    {
                        if (dataCommunicationService.consultationRequest.SectorTypeId == (int)OperatingSectorTypeEnum.InternationalArbitration)
                        {
                            consultationRequestLawyerAssignment.Remarks = "Assign_For_International_Arbitration";
                        }
                        else if (dataCommunicationService.consultationRequest.SectorTypeId == (int)OperatingSectorTypeEnum.Contracts)
                        {
                            consultationRequestLawyerAssignment.Remarks = "Assign_For_Contracts";
                        }
                        else if (dataCommunicationService.consultationRequest.SectorTypeId == (int)OperatingSectorTypeEnum.LegalAdvice)
                        {
                            consultationRequestLawyerAssignment.Remarks = "Assign_For_Legal_Advice";
                        }
                        else if (dataCommunicationService.consultationRequest.SectorTypeId == (int)OperatingSectorTypeEnum.Legislations)
                        {
                            consultationRequestLawyerAssignment.Remarks = "Assign_For_Legislations";
                        }
                        else if (dataCommunicationService.consultationRequest.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeComplaints)
                        {
                            consultationRequestLawyerAssignment.Remarks = "Assign_For_Administrative_Complaints";
                        }
                    }
                    consultationRequestLawyerAssignment.ConsultationRequestId = dataCommunicationService.consultationRequest.ConsultationRequestId;
                    consultationRequestLawyerAssignment.AssignConsultationLawyerType = (int)AssignCaseToLawyerTypeEnum.ConsultationFile;
                    consultationRequestLawyerAssignment.ReferenceId = caseRequestCommunicationVM.ConsultationFile.FileId;
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

                        navigationManager.NavigateTo("/consultationfile-list/");
                        dataCommunicationService.consultationRequest = null;
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
                else
                {
                    var result = await dialogService.OpenAsync<AssignConsultationToLawyer>(translationState.Translate("Assign_To_Lawyer"),
                         new Dictionary<string, object>()
                         {
                                  { "ReferenceId", caseRequestCommunicationVM.ConsultationFile.FileId },
                                  { "AssignConsultationLawyerType", (int)AssignCaseToLawyerTypeEnum.ConsultationFile },
                         }
                         ,
                        new DialogOptions() { Width = "45% !important", CloseDialogOnOverlayClick = true });
                    navigationManager.NavigateTo("/consultationfile-list/");
                    dataCommunicationService.consultationRequest = null;

                }
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
        protected async Task PopulateSupervisorId()
        {
            try
            {
                if (loginState.UserRoles.Any(x => x.RoleId == SystemRoles.ComsLawyer))
                {
                    var response = await lookupService.GetSupervisorByLawyerId(loginState.UserDetail.UserId);
                    if (response.IsSuccessStatusCode)
                    {
                        consultationRequestLawyerAssignment.SupervisorId = (string)response.ResultData;
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
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

        #region Save As Draft
        public async Task<bool> SaveAsDraftForConsultation(bool isCultureTriggered = false)
        {
            if (dataCommunicationService.consultationRequest?.Subject?.Length > 50 || (dataCommunicationService.consultationRequest?.Subject is not null
                && !Regex.IsMatch(dataCommunicationService.consultationRequest?.Subject, RegexPatterns.NoLeadingSpacesPattern)) || dataCommunicationService.consultationRequest?.GEOpinion?.Length > 300 || (dataCommunicationService.consultationRequest?.GEOpinion is not null
                && !Regex.IsMatch(dataCommunicationService.consultationRequest?.GEOpinion, RegexPatterns.NoLeadingSpacesTextAreaPattern)) || dataCommunicationService.consultationRequest?.Remarks?.Length > 300 || (dataCommunicationService.consultationRequest?.Remarks is not null
                && !Regex.IsMatch(dataCommunicationService.consultationRequest?.Remarks, RegexPatterns.NoLeadingSpacesTextAreaPattern)) || dataCommunicationService.consultationRequest?.CompetentAuthorityOpinionWithNote?.Length > 300 || (dataCommunicationService.consultationRequest?.CompetentAuthorityOpinionWithNote is not null
                && !Regex.IsMatch(dataCommunicationService.consultationRequest?.CompetentAuthorityOpinionWithNote, RegexPatterns.NoLeadingSpacesTextAreaPattern)) || dataCommunicationService.consultationRequest?.ComplainantDecisionNumber?.Length > 30 || (dataCommunicationService.consultationRequest?.ComplainantDecisionNumber is not null
                && !Regex.IsMatch(dataCommunicationService.consultationRequest?.ComplainantDecisionNumber, RegexPatterns.NoLeadingSpacesPattern)) || dataCommunicationService.consultationRequest?.ComplainantName?.Length > 90 || (dataCommunicationService.consultationRequest?.ComplainantName is not null
                && !Regex.IsMatch(dataCommunicationService.consultationRequest?.ComplainantName, RegexPatterns.NoLeadingSpacesPattern)) || dataCommunicationService.consultationRequest?.ComplaintAgainst?.Length > 90 || (dataCommunicationService.consultationRequest?.ComplaintAgainst is not null
                && !Regex.IsMatch(dataCommunicationService.consultationRequest?.ComplaintAgainst, RegexPatterns.NoLeadingSpacesPattern)) || dataCommunicationService.consultationRequest?.HighPriorityReason?.Length > 1000 || (dataCommunicationService.consultationRequest?.HighPriorityReason is not null
                && !Regex.IsMatch(dataCommunicationService.consultationRequest?.HighPriorityReason, RegexPatterns.NoLeadingSpacesTextAreaPattern)))
            {
                await JSRuntime.InvokeVoidAsync("ScrollCaseRequestToTop");
                return false;
            }
            else
            {
                if (isCultureTriggered || await dialogService.Confirm(translationState.Translate("Save_Draft"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    dataCommunicationService.consultationRequest.RequestStatusId = (int)CaseRequestStatusEnum.Draft;
                    if (TemplateDetails.Content != null)
                    {
                        dataCommunicationService.consultationRequest.TemplateContent = TemplateDetails.Content;
                    }
                    ApiCallResponse response;
                    if (ConsultationRequestId != null)
                    {
                        dataCommunicationService.consultationRequest.IsEdit = true;
                    }
                    response = await consultationRequestService.CreateConsultationRequest(dataCommunicationService.consultationRequest);

                    spinnerService.Hide();
                    if (response.IsSuccessStatusCode)
                    {
                        // Save Attachment of Consultation Request
                        List<Guid> requestIds = new List<Guid>();

                        requestIds.Add(dataCommunicationService.consultationRequest.ConsultationRequestId);
                        var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                        {
                            RequestIds = requestIds,
                            CreatedBy = dataCommunicationService.consultationRequest.CreatedBy,
                            FilePath = _config.GetValue<string>("dms_file_path"),
                            DeletedAttachementIds = dataCommunicationService.consultationRequest.DeletedAttachementIds
                        });

                        if (!docResponse.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Attachment_Save_Failed"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            return false ;
                        }
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Draft_Saved"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        if (!isCultureTriggered)
                        {

                            navigationManager.NavigateTo("/consultationfile-list/");
                            dataCommunicationService.consultationRequest = null;
                        }
                        return true;
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion

        #region Preview Template
        //<History Author = 'Muhammad Zaeem' Date='2024-10-10' Version="1.0" Branch="master">Preview Template </History>
        protected async Task PreviewTemplate()
        {
            try
            {
                busyPreviewBtn = true;
                StateHasChanged();

                isVisible = true;
                await Task.Run(() => PopulatePdfFromHtml());
                busyPreviewBtn = false;
            }
            catch (Exception)
            {
                busyPreviewBtn = false;
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion

        #region populate pdf from Html
        protected async Task PopulatePdfFromHtml()
        {
            try
            {
                SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
                MemoryStream stream = new MemoryStream();

                // set converter options
                converter.Options.PdfPageSize = SelectPdf.PdfPageSize.A4;
                converter.Options.PdfPageOrientation = SelectPdf.PdfPageOrientation.Portrait;
                converter.Options.WebPageWidth = 1024;
                converter.Options.WebPageHeight = 1024;
                //TemplateDetails.Content = HeaderFooterTemplates.Where(x => x.Id == (Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? (int)CaseTemplateEnum.HeaderEn : (int)CaseTemplateEnum.HeaderAr)).Select(x => x.Content).FirstOrDefault() + TemplateDetails.Content + HeaderFooterTemplates.Where(x => x.Id == (int)CaseTemplateEnum.Footer).Select(x => x.Content).FirstOrDefault();

                SelectPdf.PdfDocument pdfDocument = converter.ConvertHtmlString(TemplateDetails.Content);

                pdfDocument.Save(stream);
                pdfDocument.Close();
                stream.Close();
                PreviewFileData = stream.ToArray();
                string base64String = Convert.ToBase64String(PreviewFileData);
                DocumentPathPreviewPdf = "data:application/pdf;base64," + base64String;
                dataCommunicationService.consultationRequest.FileData = PreviewFileData;
            }
            catch (Exception)
            {
                busyPreviewBtn = false;
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion

        #region Cancel Request
        protected async Task CancelRequest()
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Cancel"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                await RedirectBack();
            }
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

        #region For Hide Validation Message
        protected async Task OnChangePledge(object args)
        {
            if (args != null && (bool)args && PledgeErrorShow)
            {
                PledgeErrorShow = false;
            }
        }
        #endregion

        #region Dispose 
        public void Dispose()
        {
            dotNetRef?.Dispose();
            dotNetRef = null;
            dataCommunicationService.consultationRequest = null;
            draftHandlerService.SaveDraftForConsultation = null;
            JSRuntime.InvokeVoidAsync("window.removeUnloadListenerCOMS");
        }
        #endregion
    }
}
