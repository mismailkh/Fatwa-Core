using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using FATWA_WEB.Pages.LLSLegalPrinciple;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Linq.Dynamic.Core;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.DmsEnums;

namespace FATWA_WEB.Pages.Dms
{
    //<History Author = 'Hassan Abbas' Date='2023-06-23' Version="1.0" Branch="master"> Link Document with an Entity</History>
    //<History Author = 'Umer Zaman' Date='2025-03-13' Version="1.0" Branch="master"> Modified lnk document implementation according to latest Legal Principle system</History>
    public partial class LinkDocument : ComponentBase
    {
        #region Parameter

        [Parameter]
        public dynamic? DocumentVersionId { get; set; }
        #endregion

        #region Variables

        //Module and Submodules
        public int CourtTypeId { get; set; }
        public int ModuleId { get; set; }
        public int SubModuleId { get; set; }
        public int SelectedModuleId { get; set; }
        public int SelectedSubModuleId { get; set; }
        public int SectorTypeId { get; set; }
        public List<ModuleEnumTemp> Modules { get; set; } = new List<ModuleEnumTemp>();
        public List<ModuleEnumTemp> SubModules { get; set; } = new List<ModuleEnumTemp>();

        public class ModuleEnumTemp
        {
            public int ModuleEnumValue { get; set; }
            public string ModuleEnumName { get; set; }
        }
        Type TItemm = typeof(CmsCaseFileDmsVM);

        //Case Requests
        protected AdvanceSearchCmsCaseRequestVM advanceSearchCaseRequestVM = new AdvanceSearchCmsCaseRequestVM();
        protected RadzenDataGrid<CmsCaseRequestDmsVM> caseRequestGrid;

        IEnumerable<CmsCaseRequestDmsVM> CaseRequests { get; set; }

        List<CmsCaseRequestDmsVM> _FilteredCaseRequests;
        protected List<CmsCaseRequestDmsVM> FilteredCaseRequests
        {
            get
            {
                return _FilteredCaseRequests;
            }
            set
            {
                if (!object.Equals(_FilteredCaseRequests, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredCaseRequests", NewValue = value, OldValue = _FilteredCaseRequests };
                    _FilteredCaseRequests = value;

                    Reload();
                }

            }
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        IList<CmsCaseRequestDmsVM> selectedCaseRequests;

        protected string search { get; set; }
        protected bool allowRowSelectOnRowClick = true;

        //Case Files

        protected RadzenDataGrid<CmsCaseFileDmsVM>? caseFileGrid;
        IEnumerable<CmsCaseFileDmsVM> CaseFiles { get; set; }

        List<CmsCaseFileDmsVM> _FilteredCaseFiles;
        protected List<CmsCaseFileDmsVM> FilteredCaseFiles
        {
            get
            {
                return _FilteredCaseFiles;
            }
            set
            {
                if (!object.Equals(_FilteredCaseFiles, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredCaseFiles", NewValue = value, OldValue = _FilteredCaseFiles };
                    _FilteredCaseFiles = value;

                    Reload();
                }

            }
        }

        IList<CmsCaseFileDmsVM>? selectedCaseFiles;

        //Registered Cases

        protected RadzenDataGrid<CmsRegisteredCaseDmsVM>? registeredCasesGrid;
        IEnumerable<CmsRegisteredCaseDmsVM> RegisteredCases { get; set; }

        List<CmsRegisteredCaseDmsVM> _FilteredRegisteredCases;
        protected List<CmsRegisteredCaseDmsVM> FilteredRegisteredCases
        {
            get
            {
                return _FilteredRegisteredCases;
            }
            set
            {
                if (!object.Equals(_FilteredRegisteredCases, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredRegisteredCases", NewValue = value, OldValue = _FilteredRegisteredCases };
                    _FilteredRegisteredCases = value;

                    Reload();
                }

            }
        }

        IList<CmsRegisteredCaseDmsVM> selectedRegisteredCases;

        //Consultation Requests

        protected RadzenDataGrid<ConsultationRequestDmsVM>? consultationRequestsGrid;
        IEnumerable<ConsultationRequestDmsVM> ConsultationRequests { get; set; }

        List<ConsultationRequestDmsVM> _FilteredConsultationRequests;
        protected List<ConsultationRequestDmsVM> FilteredConsultationRequests
        {
            get
            {
                return _FilteredConsultationRequests;
            }
            set
            {
                if (!object.Equals(_FilteredConsultationRequests, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredConsultationRequests", NewValue = value, OldValue = _FilteredConsultationRequests };
                    _FilteredConsultationRequests = value;

                    Reload();
                }

            }
        }

        IList<ConsultationRequestDmsVM> selectedConsultationRequests;
        // Consultation Files
        protected RadzenDataGrid<ConsultationFileListDmsVM>? consultationFilesGrid;
        IEnumerable<ConsultationFileListDmsVM> ConsultationFiles { get; set; }
        List<ConsultationFileListDmsVM> _FilteredConsultationFiles;
        List<ConsultationFileListDmsVM> FilteredConsultationFiles
        {
            get
            {
                return _FilteredConsultationFiles;
            }
            set
            {
                if (!object.Equals(_FilteredConsultationFiles, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredConsultationFiles", NewValue = value, OldValue = _FilteredConsultationFiles };
                    _FilteredConsultationFiles = value;

                    Reload();
                }

            }

        }
        IList<ConsultationFileListDmsVM> selectedConsultationFiles;
        //Legal Legislation
        protected RadzenDataGrid<LegalLegislationsDmsVM>? LegallegislationGrid;
        IEnumerable<LegalLegislationsDmsVM> Legallegislation { get; set; }
        List<LegalLegislationsDmsVM> _FilteredLegallegislation;
        List<LegalLegislationsDmsVM> FilteredLegallegislation
        {
            get
            {
                return _FilteredLegallegislation;
            }
            set
            {
                if (!object.Equals(_FilteredLegallegislation, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredLegallegislation", NewValue = value, OldValue = _FilteredLegallegislation };
                    _FilteredLegallegislation = value;
                    Reload();
                }
            }
        }
        IList<LegalLegislationsDmsVM> selectedlegallegislation;
        IList<LLSLegalPrinciplesRelationVM> selectedlegalprinciple;
        //Shared
        protected bool DisableLinkDocumentButton = true;
        LinkDocumentsVM linkDocumentDetails { get; set; }
        protected LLSLegalPrinciplesRelationVM PrincipleContentSearch { get; set; } = new LLSLegalPrinciplesRelationVM();
        protected List<LLSLegalPrinciplesRelationVM> PrincipleContents { get; set; } = new List<LLSLegalPrinciplesRelationVM>();
        protected RadzenDataGrid<LLSLegalPrinciplesRelationVM>? PrincipleContentGrid = new RadzenDataGrid<LLSLegalPrinciplesRelationVM>();
        protected IList<LLSLegalPrinciplesRelationVM> selectedPrincipleContents { get; set; } = new List<LLSLegalPrinciplesRelationVM>();
        List<LLSLegalPrincipleContentCategoriesVM> PrincipleCategories { get; set; } = new List<LLSLegalPrincipleContentCategoriesVM>();
        private bool isAll = false;
        #endregion

        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            SectorTypeId= (int)loginState.UserDetail.SectorTypeId;
            await PopulateModules();
            await PopulateCourtType();
            linkDocumentDetails = new LinkDocumentsVM { SourceDocumentVersionId = DocumentVersionId, CreatedBy = loginState.Username };
            if (Modules.Count == 1 && SectorTypeId == 0)
            {
                await SubmitModuleSelection();
            }
            spinnerService.Hide();
        }
        #endregion

        #region Dropdown Events

        //< History Author = 'Hassan Abbas' Date = '2023-07-11' Version = "1.0" Branch = "master" >Populate Modules</History>
        protected async Task PopulateModules()
        {
            try
            {
                if (loginState.UserRoles.Any(r => SystemRoles.CaseRoles.Contains(r.RoleId)))
                {
                    Modules.Add(new ModuleEnumTemp { ModuleEnumName = translationState.Translate(Enum.GetName(typeof(DocumentLinkModuleEnum), (int)DocumentLinkModuleEnum.CaseManagement)), ModuleEnumValue = (int)DocumentLinkModuleEnum.CaseManagement });
                    ModuleId = (int)DocumentLinkModuleEnum.CaseManagement;
                    await OnModuleChange();
                }
                if (loginState.UserRoles.Any(r => SystemRoles.ConsultationRoles.Contains(r.RoleId)))
                {
                    Modules.Add(new ModuleEnumTemp { ModuleEnumName = translationState.Translate(Enum.GetName(typeof(DocumentLinkModuleEnum), (int)DocumentLinkModuleEnum.ConsultationManagement)), ModuleEnumValue = (int)DocumentLinkModuleEnum.ConsultationManagement });
                    ModuleId = (int)DocumentLinkModuleEnum.ConsultationManagement;
                    await OnModuleChange();
                }
                if (loginState.UserRoles.Any(r => SystemRoles.LiteratureRoles.Contains(r.RoleId)))
                {
                    Modules.Add(new ModuleEnumTemp { ModuleEnumName = translationState.Translate(Enum.GetName(typeof(DocumentLinkModuleEnum), (int)DocumentLinkModuleEnum.Literature)), ModuleEnumValue = (int)DocumentLinkModuleEnum.Literature });
                    ModuleId = (int)DocumentLinkModuleEnum.Literature;
                    await OnModuleChange();
                }
                if (loginState.UserRoles.Any(r => SystemRoles.LegislationRoles.Contains(r.RoleId)))
                {
                    Modules.Add(new ModuleEnumTemp { ModuleEnumName = translationState.Translate(Enum.GetName(typeof(DocumentLinkModuleEnum), (int)DocumentLinkModuleEnum.LegalDocument)), ModuleEnumValue = (int)DocumentLinkModuleEnum.LegalDocument });
                    ModuleId = (int)DocumentLinkModuleEnum.LegalDocument;
                    await OnModuleChange();
                }
                if (loginState.UserRoles.Any(r => SystemRoles.PrincipleRoles.Contains(r.RoleId)))
                {
                    Modules.Add(new ModuleEnumTemp { ModuleEnumName = translationState.Translate(Enum.GetName(typeof(DocumentLinkModuleEnum), (int)DocumentLinkModuleEnum.LegalPrinciple)), ModuleEnumValue = (int)DocumentLinkModuleEnum.LegalPrinciple });
                    ModuleId = (int)DocumentLinkModuleEnum.LegalPrinciple;
                    await OnModuleChange();
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

        //< History Author = 'Hassan Abbas' Date = '2023-07-11' Version = "1.0" Branch = "master" >Populate SubModules</History>
        protected async Task OnModuleChange()
        {
            try
            {
                if (ModuleId == (int)DocumentLinkModuleEnum.CaseManagement)
                {
                    if (loginState.UserRoles.Any(r => r.RoleId == SystemRoles.HOS))
                    {
                        SubModules.Add(new ModuleEnumTemp { ModuleEnumName = translationState.Translate(Enum.GetName(typeof(DocumentLinkSubModuleEnum), (int)DocumentLinkSubModuleEnum.CaseRequest)), ModuleEnumValue = (int)DocumentLinkSubModuleEnum.CaseRequest });
                    }
                    SubModules.Add(new ModuleEnumTemp { ModuleEnumName = translationState.Translate(Enum.GetName(typeof(DocumentLinkSubModuleEnum), (int)DocumentLinkSubModuleEnum.CaseFile)), ModuleEnumValue = (int)DocumentLinkSubModuleEnum.CaseFile });
                    SubModules.Add(new ModuleEnumTemp { ModuleEnumName = translationState.Translate(Enum.GetName(typeof(DocumentLinkSubModuleEnum), (int)DocumentLinkSubModuleEnum.Case)), ModuleEnumValue = (int)DocumentLinkSubModuleEnum.Case });
                }
                else if (ModuleId == (int)DocumentLinkModuleEnum.ConsultationManagement)
                {
                    if (loginState.UserRoles.Any(r => r.RoleId == SystemRoles.ComsHOS))
                    {
                        SubModules.Add(new ModuleEnumTemp { ModuleEnumName = translationState.Translate(Enum.GetName(typeof(DocumentLinkSubModuleEnum), (int)DocumentLinkSubModuleEnum.ConsultationRequest)), ModuleEnumValue = (int)DocumentLinkSubModuleEnum.ConsultationRequest });
                    }
                    SubModules.Add(new ModuleEnumTemp { ModuleEnumName = translationState.Translate(Enum.GetName(typeof(DocumentLinkSubModuleEnum), (int)DocumentLinkSubModuleEnum.ConsultationRequest)), ModuleEnumValue = (int)DocumentLinkSubModuleEnum.ConsultationRequest });
                    SubModules.Add(new ModuleEnumTemp { ModuleEnumName = translationState.Translate(Enum.GetName(typeof(DocumentLinkSubModuleEnum), (int)DocumentLinkSubModuleEnum.ConsultationFile)), ModuleEnumValue = (int)DocumentLinkSubModuleEnum.ConsultationFile });
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

        #region Submit Module Selection

        //< History Author = 'Hassan Abbas' Date = '2023-07-12' Version = "1.0" Branch = "master" >Populate Entities</History>
        protected async Task SubmitModuleSelection()
        {
            try
            {
                search = String.Empty;
                SelectedModuleId = ModuleId;
                SelectedSubModuleId = SubModuleId;
                linkDocumentDetails.DestinationIds = new List<Guid>();
                linkDocumentDetails.LiteratureIds = new List<int>();
                linkDocumentDetails.UploadFrom = "";
                TItemm = typeof(CmsCaseFileDmsVM);

                if (ModuleId == (int)DocumentLinkModuleEnum.CaseManagement)
                {
                    if (SubModuleId == (int)DocumentLinkSubModuleEnum.CaseRequest)
                    {
                       
                        await PopulateCaseRequests();
                    }
                    else if (SubModuleId == (int)DocumentLinkSubModuleEnum.CaseFile)
                    {
                        await PopulateCaseFiles();
                    }
                    else if (SubModuleId == (int)DocumentLinkSubModuleEnum.Case)
                    {
                        await PopulateCases();
                  
                    }
                }
                else if (ModuleId == (int)DocumentLinkModuleEnum.ConsultationManagement)
                {
                    if (SubModuleId == (int)DocumentLinkSubModuleEnum.ConsultationRequest)
                    {
                        await PopulateConsultationRequests();
                    }
                    else if (SubModuleId == (int)DocumentLinkSubModuleEnum.ConsultationFile)
                    {
                        await PopulateConsultationFiles();
                    }
                }
                else if (ModuleId == (int)DocumentLinkModuleEnum.Literature)
                {
                    //await PopulateLiteratures();
                }
                else if (ModuleId == (int)DocumentLinkModuleEnum.LegalDocument)
                {
                    await PopulateLegalDocuments();
                }
                else if (ModuleId == (int)DocumentLinkModuleEnum.LegalPrinciple)
                {
                    await GetLLSLegalPrincipleContent(PrincipleContentSearch);
                    await GetLLSLegalPrincipleCategories();
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region Grids Common Search

        protected async Task OnSearchInput()
       {
            try
            {
                if (string.IsNullOrEmpty(search))
                    search = "";
                else
                    search = search.ToLower();

                if (SelectedModuleId == (int)DocumentLinkModuleEnum.CaseManagement)
                {
                    if (SelectedSubModuleId == (int)DocumentLinkSubModuleEnum.CaseRequest)
                    {
                        FilteredCaseRequests = await gridSearchExtension.Filter(CaseRequests, new Query()
                        {
                            Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? $@"i => ( i.RequestNumber != null && i.RequestNumber.ToString().ToLower().Contains(@0) ) || (i.SectorType_Name_En != null && i.SectorType_Name_En.ToString().Contains(@1)) || (i.Subject != null && i.Subject.ToString().ToLower().Contains(@2))" :
                            $@"i => ( i.RequestNumber != null && i.RequestNumber.ToString().ToLower().Contains(@0) ) || (i.SectorType_Name_Ar != null && i.SectorType_Name_Ar.ToString().Contains(@1)) || (i.Subject != null && i.Subject.ToString().ToLower().Contains(@2))",
                            FilterParameters = new object[] { search, search, search }
                        });
                        if (selectedCaseRequests != null)
                        {
                            linkDocumentDetails.DestinationIds.RemoveAll(e => selectedCaseRequests.Except(FilteredCaseRequests).Select(f => f.RequestId).ToList().Contains(e));
                            selectedCaseRequests = FilteredCaseRequests.Intersect(selectedCaseRequests).ToList();
                        }
                    }
                    else if (SelectedSubModuleId == (int)DocumentLinkSubModuleEnum.CaseFile)
                    {
                        FilteredCaseFiles = await gridSearchExtension.Filter(CaseFiles, new Query()
                        {
                            Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? $@"i => ( i.FileNumber != null && i.FileNumber.ToString().ToLower().Contains(@0) ) || (i.CreatedDate != null && i.CreatedDate.ToString().Contains(@1)) || (i.Subject != null && i.Subject.ToString().ToLower().Contains(@2))" :
                            $@"i => ( i.FileNumber != null && i.FileNumber.ToString().ToLower().Contains(@0) ) || (i.CreatedDate != null && i.CreatedDate.ToString().Contains(@1)) || (i.Subject != null && i.Subject.ToString().ToLower().Contains(@2))",
                            FilterParameters = new object[] { search, search, search }
                        });
                        if (selectedCaseFiles != null)
                        {
                            linkDocumentDetails.DestinationIds.RemoveAll(e => selectedCaseFiles.Except(FilteredCaseFiles).Select(f => f.FileId).ToList().Contains(e));
                            selectedCaseFiles = FilteredCaseFiles.Intersect(selectedCaseFiles).ToList();
                        }  

                    }
                    else if (SelectedSubModuleId == (int)DocumentLinkSubModuleEnum.Case)
                    {
                        FilteredRegisteredCases = await gridSearchExtension.Filter(RegisteredCases, new Query()
                        {
                            Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? $@"i => ( i.CaseNumber != null && i.CaseNumber.ToString().ToLower().Contains(@0) ) || (i.CANNumber != null && i.CANNumber.ToLower().Contains(@1)) || (i.CourtNameEn != null && i.CourtNameEn.ToLower().Contains(@2))" :
                           $@"i => ( i.CaseNumber != null && i.v.ToString().ToLower().Contains(@0) ) || (i.CANNumber != null && i.CANNumber.ToLower().Contains(@1)) || (i.CourtNameAr != null && i.CourtNameAr.ToLower().Contains(@2))",
                            FilterParameters = new object[] { search, search, search }
                        });        
                        if (selectedRegisteredCases != null)
                        {
                            linkDocumentDetails.DestinationIds.RemoveAll(e => selectedRegisteredCases.Except(FilteredRegisteredCases).Select(f => f.CaseId).ToList().Contains(e));
                            selectedRegisteredCases = FilteredRegisteredCases.Intersect(selectedRegisteredCases).ToList();

                        }
                           
                    }
                }


                else if (SelectedModuleId == (int)DocumentLinkModuleEnum.ConsultationManagement)
                {
                    if (SelectedSubModuleId == (int)DocumentLinkSubModuleEnum.ConsultationRequest)
                    {
                        FilteredConsultationRequests = await gridSearchExtension.Filter(ConsultationRequests, new Query()
                        {
                            Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? $@"i => ( i.RequestNumber != null && i.RequestNumber.ToString().ToLower().Contains(@0) ) || (i.Status_Name_En != null && i.Status_Name_En.ToString().Contains(@1)) || (i.Subject != null && i.Subject.ToString().ToLower().Contains(@2))" :
                           $@"i => ( i.RequestNumber != null && i.RequestNumber.ToString().ToLower().Contains(@0) ) || (i.Status_Name_Ar != null && i.Status_Name_Ar.ToString().Contains(@1)) || (i.Subject != null && i.Subject.ToString().ToLower().Contains(@2))",
                            FilterParameters = new object[] { search, search, search }

                           
                        });

                      
                        if (selectedConsultationRequests != null )
                        {
                            linkDocumentDetails.DestinationIds.RemoveAll(e => selectedConsultationRequests.Except(FilteredConsultationRequests).Select(f => f.ConsultationRequestId).ToList().Contains(e));
                            selectedConsultationRequests = FilteredConsultationRequests.Intersect(selectedConsultationRequests).ToList();
                        }
                    }

                   
                    else if (SelectedSubModuleId == (int)DocumentLinkSubModuleEnum.ConsultationFile)
                    {
                        FilteredConsultationFiles = await gridSearchExtension.Filter(ConsultationFiles, new Query()
                        {
          
                            Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? $@"i => ( i.FileNumber != null && i.FileNumber.ToString().ToLower().Contains(@0) ) || (i.StatusEn != null && i.StatusEn.ToString().Contains(@1)) || (i.LawyerNameEn != null && i.LawyerNameEn.ToString().ToLower().Contains(@2))" :
                            $@"i => ( i.FileNumber != null && i.FileNumber.ToString().ToLower().Contains(@0) ) || (i.StatusAr != null && i.StatusAr.ToString().Contains(@1)) || (i.LawyerNameAr != null && i.LawyerNameAr.ToString().ToLower().Contains(@2))",
                            FilterParameters = new object[] { search, search, search }
                        });
                        if (selectedConsultationFiles != null)
                        {

                            linkDocumentDetails.DestinationIds.RemoveAll(e => selectedConsultationFiles.Except(FilteredConsultationFiles).Select(f => f.FileId).ToList().Contains(e));
                            selectedConsultationFiles = FilteredConsultationFiles.Intersect(selectedConsultationFiles).ToList();
                        }
                           
                    }
                }
                else if (SelectedSubModuleId == (int)DocumentLinkModuleEnum.Literature)
                {
                    //await PopulateLiteratures();
                }
                else if (SelectedModuleId == (int)DocumentLinkModuleEnum.LegalDocument)
                {
                    FilteredLegallegislation = await gridSearchExtension.Filter(Legallegislation, new Query()
                    {
                        Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? $@"i => (i.Legislation_Number != null && i.Legislation_Number.ToString().Contains(@0)) || (i.Legislation_Type_En != null && i.Legislation_Type_En.ToString().ToLower().Contains(@1))" :
                            $@"i => (i.Legislation_Number != null && i.Legislation_Number.ToString().Contains(@0)) || (i.Legislation_Type_Ar != null && i.Legislation_Type_Ar.ToString().ToLower().Contains(@1))",
                        FilterParameters = new object[] { search, search, search }
                    });
                    if (selectedlegallegislation != null)
                    {
                        linkDocumentDetails.DestinationIds.RemoveAll(e => selectedlegallegislation.Except(FilteredLegallegislation).Select(f => f.LegislationId).ToList().Contains(e));
                        selectedlegallegislation = FilteredLegallegislation.Intersect(selectedlegallegislation).ToList();
                    }
                }
                else if (SelectedModuleId == (int)DocumentLinkModuleEnum.LegalPrinciple)
                {
                    PrincipleContents = await gridSearchExtension.Filter(PrincipleContents, new Query()
                    {
                        Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? $@"i => (i.PrincipleContent != null && i.PrincipleContent.ToString().ToLower().Contains(@0)) || (i.FlowStatusName_En != null && i.FlowStatusName_En.ToString().ToLower().Contains(@1))" :
                        $@"i => (i.PrincipleContent != null && i.PrincipleContent.ToString().ToLower().Contains(@0)) || (i.FlowStatusName_Ar != null && i.FlowStatusName_Ar.ToString().ToLower().Contains(@1))",
                        FilterParameters = new object[] { search, search }
                    });
                    if (selectedlegalprinciple != null)
                    {
                        linkDocumentDetails.DestinationIds.RemoveAll(e => selectedlegalprinciple.Except(PrincipleContents).Select(f => f.PrincipleContentId).ToList().Contains(e));
                        selectedlegalprinciple = PrincipleContents.Intersect(selectedlegalprinciple).ToList();
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

        #region Populate Grids
        protected async Task PopulateCaseRequests()
        {
            try
            {
                var response = await caseRequestService.GetAllCaseRequestsBySectorTypeId(loginState.UserDetail.SectorTypeId != null ? (int)loginState.UserDetail.SectorTypeId : 0);
                if (response.IsSuccessStatusCode)
                {
                    CaseRequests = (IEnumerable<CmsCaseRequestDmsVM>)response.ResultData;
                    FilteredCaseRequests = (List<CmsCaseRequestDmsVM>)response.ResultData;
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
        protected async Task PopulateCaseFiles()
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                {
                    search = "";
                }
                else
                    search = search.ToLower();

                var response = await cmsCaseFileService.GetAllCaseFilesBySector(loginState.UserDetail?.SectorTypeId != null ? (int)loginState.UserDetail?.SectorTypeId : 0, loginState.UserDetail?.UserId);
                if (response.IsSuccessStatusCode)
                {
                    CaseFiles = (IEnumerable<CmsCaseFileDmsVM>)response.ResultData;
                    FilteredCaseFiles = (List<CmsCaseFileDmsVM>)response.ResultData;
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
        protected async Task PopulateCases()
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                {
                    search = "";
                }
                else
                    search = search.ToLower();

                var response = await cmsRegisteredCaseService.GetAllRegisteredCasesByCourtTypeId(CourtTypeId, loginState.UserDetail?.UserId);
                if (response.IsSuccessStatusCode)
                {
                    RegisteredCases = (IEnumerable<CmsRegisteredCaseDmsVM>)response.ResultData;
                    FilteredRegisteredCases = (List<CmsRegisteredCaseDmsVM>)response.ResultData;
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
        protected async Task PopulateConsultationRequests()
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                {
                    search = "";
                }
                else
                    search = search.ToLower();

                var response = await consultationRequestService.GetAllConsultationBySectorTypeId(loginState.UserDetail.SectorTypeId != null ? (int)loginState.UserDetail.SectorTypeId : 0);
                if (response.IsSuccessStatusCode)
                {
                    ConsultationRequests = (IEnumerable<ConsultationRequestDmsVM>)response.ResultData;
                    FilteredConsultationRequests = (List<ConsultationRequestDmsVM>)response.ResultData;
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
        protected async Task PopulateConsultationFiles()
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                {
                    search = "";
                }
                else
                    search = search.ToLower();

                var response = await consultationFileService.GetAllConsultationFileListBySectorTypeId(loginState.UserDetail?.SectorTypeId != null ? (int)loginState.UserDetail?.SectorTypeId : 0, loginState.UserDetail?.UserId);
                if (response.IsSuccessStatusCode)
                {
                    ConsultationFiles = (IEnumerable<ConsultationFileListDmsVM>)response.ResultData;
                    FilteredConsultationFiles = (List<ConsultationFileListDmsVM>)response.ResultData;
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
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Subtypes data</History>
        protected async Task PopulateCourtType()
        {
            if (new List<int> { (int)OperatingSectorTypeEnum.AdministrativeRegionalCases, (int)OperatingSectorTypeEnum.CivilCommercialRegionalCases }.Contains(loginState.UserDetail.SectorTypeId != null ? (int)loginState.UserDetail.SectorTypeId : 0))
            {
                CourtTypeId = (int)CourtTypeEnum.Regional;
            }
            else if (new List<int> { (int)OperatingSectorTypeEnum.AdministrativeAppealCases, (int)OperatingSectorTypeEnum.AdministrativeAppealCases }.Contains(loginState.UserDetail.SectorTypeId != null ? (int)loginState.UserDetail.SectorTypeId : 0))
            {
                CourtTypeId = (int)CourtTypeEnum.Appeal;
            }
            else if (new List<int> { (int)OperatingSectorTypeEnum.AdministrativeSupremeCases, (int)OperatingSectorTypeEnum.AdministrativeSupremeCases }.Contains(loginState.UserDetail.SectorTypeId != null ? (int)loginState.UserDetail.SectorTypeId : 0))
            {
                CourtTypeId = (int)CourtTypeEnum.Supreme;
            }
            else if (new List<int> { (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases }.Contains(loginState.UserDetail.SectorTypeId != null ? (int)loginState.UserDetail.SectorTypeId : 0))
            {
                //CourtTypeId = (int)CourtTypeEnum.PartialUrgent;
            }
        }
        protected async Task PopulateLegalDocuments()
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                {
                    search = "";
                }
                else
                    search = search.ToLower();
                var response = await legalLegislationService.GetLegalLegislationsDms();
                if (response.IsSuccessStatusCode)
                {
                    Legallegislation = (IEnumerable<LegalLegislationsDmsVM>)response.ResultData;
                    FilteredLegallegislation = (List<LegalLegislationsDmsVM>)response.ResultData;
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
        private async Task GetLLSLegalPrincipleContent(LLSLegalPrinciplesRelationVM item)
        {
            PrincipleContentSearch.FromPage = 2;
            var response = await lLSLegalPrincipleService.AdvanceSearchPrincipleRelation(PrincipleContentSearch);
            if (response.IsSuccessStatusCode)
            {
                PrincipleContents = (List<LLSLegalPrinciplesRelationVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        private async Task GetLLSLegalPrincipleCategories()
        {
            var response = await lLSLegalPrincipleService.GetLLSLegalPrincipleContentCategories();
            if (response.IsSuccessStatusCode)
            {
                PrincipleCategories = (List<LLSLegalPrincipleContentCategoriesVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Grid Events
        //History Author = 'Hassan Abbas' Date='2023-07-15' Version="1.0" Branch="master"> Redirect to Case Request Detail in new Window</History>
        protected async Task DetailCaseRequest(CmsCaseRequestDmsVM args)
        {
            await JSRuntime.InvokeAsync<object>("open", navigationManager.BaseUri + "/caserequest-view/" + args.RequestId, "_blank");
        }
        //History Author = 'Hassan Abbas' Date='2023-07-15' Version="1.0" Branch="master"> Redirect to Case File Detail in new Window</History>
        protected async Task DetailCaseFile(CmsCaseFileDmsVM args)
        {
            await JSRuntime.InvokeAsync<object>("open", navigationManager.BaseUri + "/casefile-view/" + args.FileId, "_blank");
        }
        //History Author = 'Hassan Abbas' Date='2023-07-15' Version="1.0" Branch="master"> Redirect to Case Detail in new Window</History>
        protected async Task DetailRegisteredCase(CmsRegisteredCaseDmsVM args)
        {
            await JSRuntime.InvokeAsync<object>("open", navigationManager.BaseUri + "/case-view/" + args.CaseId, "_blank");
        }
        //History Author = 'ijaz Ahmad' Date='2023-07-15' Version="1.0" Branch="master"> Redirect to Case Detail in new Window</History>
        protected async Task DetailConsultationRequest(ConsultationRequestDmsVM args)
        {
            await JSRuntime.InvokeAsync<object>("open", navigationManager.BaseUri + "/consultationrequest-detail/" + args.ConsultationRequestId + "/" + args.SectorTypeId, "_blank");
        }
        
        //History Author = 'ijaz Ahmad' Date='2023-07-15' Version="1.0" Branch="master"> Redirect to Case Detail in new Window</History>
        protected async Task DetailConsultationFile(ConsultationFileListDmsVM args)
        {
            await JSRuntime.InvokeAsync<object>("open", navigationManager.BaseUri + "/consultationfile-view/" + args.FileId + "/" + args.SectorTypeId, "_blank");
        }
        //History Author = 'Hassan Abbas' Date='2023-08-07' Version="1.0" Branch="master"> Handle Select All Rows Checkbox for all the grids</History>
        private async Task HandleSelectAllRows<T>(bool isChecked, IEnumerable<T> sourceList, string keyPropertyName, RadzenDataGrid<T> grid, string uploadFrom)
        {
            var keyProperty = typeof(T).GetProperty(keyPropertyName);
            if (keyProperty == null)
            {
                return;
            }

            var existingkeyValues = linkDocumentDetails.DestinationIds.ToList();
            var sourceValues = sourceList.Select(e => keyProperty.GetValue(e)).ToList();
            linkDocumentDetails.UploadFrom = uploadFrom;

            if (isChecked)
            {
                foreach (var item in sourceList)
                {
                    Guid keyValue = (Guid)keyProperty.GetValue(item);
                    if (!existingkeyValues.Contains(keyValue))
                    {
                        linkDocumentDetails.DestinationIds.Add(keyValue);
                        grid.SelectRow(item);
                    }
                }
            }
            else
            {
                linkDocumentDetails.DestinationIds.RemoveAll(e => sourceValues.Contains(e));
            }
        }

        //History Author = 'Hassan Abbas' Date='2023-08-07' Version="1.0" Branch="master"> Handle Individual Row Selecttion Checkbox for all the grids</History>
        private async Task HandleSelectRow<T>(bool isChecked, T item, string keyPropertyName, RadzenDataGrid<T> grid, string uploadFrom)
        {
            var keyProperty = typeof(T).GetProperty(keyPropertyName);
            if (keyProperty == null)
            {
                return;
            }

            var existingkeyValues = linkDocumentDetails.DestinationIds.ToList();
            Guid keyValue = (Guid)keyProperty.GetValue(item);
            linkDocumentDetails.UploadFrom = uploadFrom;

            if (isChecked)
            {
                if (!existingkeyValues.Contains(keyValue))
                {
                    linkDocumentDetails.DestinationIds.Add(keyValue);
                    grid.SelectRow(item);
                }
            }
            else
            {
                linkDocumentDetails.DestinationIds.Remove(keyValue);
            }
        }

        //History Author = 'Hassan Abbas' Date='2023-08-07' Version="1.0" Branch="master"> Grids Shared Selection</History>
        protected async Task SelectRow<T>(RadzenDataGrid<T> grid, T data)
            where T : class
        {
            try
            {
                if (!allowRowSelectOnRowClick)
                {
                    grid.SelectRow(data);
                };
                await OnSelectionChange<CmsCaseFileDmsVM>(selectedCaseFiles.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //History Author = 'Hassan Abbas' Date='2023-08-07' Version="1.0" Branch="master"> Grids Shared Selection</History>
        protected async Task OnSelectionChange<T>(List<T> selectedItems)
            where T : class
        {
            try
            {
                if (selectedItems != null && selectedItems.Any())
                {
                    DisableLinkDocumentButton = false;
                }
                else
                {
                    DisableLinkDocumentButton = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected async Task ViewLegislationDetail(LegalLegislationsDmsVM args)
        {
            navigationManager.NavigateTo("legallegislation-detailview/" + args.LegislationId);
        }
        #endregion

        #region Dialog Events

        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        protected async Task LinkDocumentsToEntity()
        {
            try
            {
                if (ModuleId == (int)DocumentLinkModuleEnum.LegalPrinciple)
                {
                    if (selectedPrincipleContents.Any(x => x.PageNumber == null || x.PageNumber == 0))
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Page_Number_is_required"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        return;
                    }
                }
                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    if (ModuleId == (int)DocumentLinkModuleEnum.LegalPrinciple)
                    {
                        linkDocumentDetails.DestinationIds = selectedPrincipleContents.Select(x => x.PrincipleContentId).ToList();
                        linkDocumentDetails.PrincipleContentsDetails = selectedPrincipleContents;
                        linkDocumentDetails.UploadFrom = "LLSLegalPrincipleSystem";
                        linkDocumentDetails.ModuleId = ModuleId;
                    }
                    var response = await fileUploadService.LinkDocumentToDestinationEntities(linkDocumentDetails);
                    if (response.IsSuccessStatusCode)
                    {
                        // now link with legal principle tables code here


                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Document_Linked_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        dialogService.Close();
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();
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

        #region LLS Legal Principle Grid Buttin Functions

        //on change check box
        private void OnChange(LLSLegalPrinciplesRelationVM data)
        {
            if (data.IsChecked)
            {
                selectedPrincipleContents.Add(data);
                //linkDocumentDetails
            }
            else
            {
                selectedPrincipleContents.Remove(data);
                data.PageNumber = null;
            }
        }

        //On Change Header Check Box
        private void OnChangeHeader()
        {
            if (isAll)
            {
                PrincipleContents.ForEach(x => x.IsChecked = true);
                selectedPrincipleContents = PrincipleContents;
            }
            else
            {
                PrincipleContents.ForEach(x => x.IsChecked = false);
                selectedPrincipleContents = new List<LLSLegalPrinciplesRelationVM>();
            }
        }
        protected async Task ViewLinkedDocments(Guid principleContentId)
        {
            var result = await dialogService.OpenAsync<PrincipleContentLinkedDocumentsDialog>(translationState.Translate("Principle_Content_Linked_Documents"),
                            new Dictionary<string, object>()
                            {
                                { "PrincipleContentId", principleContentId }
                            },
                            new DialogOptions() { Width = "80% !important", CloseDialogOnOverlayClick = false });
        }
        #endregion
    }
}
