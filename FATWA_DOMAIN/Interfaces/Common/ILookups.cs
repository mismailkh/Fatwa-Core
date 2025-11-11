using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.Contact;
using FATWA_DOMAIN.Models.LegalPrinciple;
using FATWA_DOMAIN.Models.Lms;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.ContactManagmentVMs;
using FATWA_DOMAIN.Models.ViewModel.InventoryManagementVMs;
using FATWA_DOMAIN.Models.ViewModel.MojRollsVM;
using FATWA_DOMAIN.Models.ViewModel.RolesVM;
using FATWA_DOMAIN.Models.ViewModel.ServiceRequestVMs;
using FATWA_DOMAIN.Models.ViewModel.TimeIntervalVMs;
using FATWA_DOMAIN.Models.WorkerService;
using FATWA_DOMAIN.Models.WorkflowModels;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Interfaces.Common
{
    public interface ILookups
    {
        Task<List<CmsGovtEntityNumPattern>> GetGovernmentEntity();
        Task<List<GovernmentEntity>> GetGovernmentEntities(string Culture);
        Task<List<GovernmentEntityRepresentative>> GetGeRepresentatives(int? govtEntitiyId);
        Task<List<OperatingSectorType>> GetOperatingSectorTypes();
        Task<List<OperatingSectorType>> GetOperatingSectorsByDepartmentId(int DepartmentId);
        Task<List<RequestType>> GetRequestTypes();
        Task<List<CaseRequestStatus>> GetCaseRequestStatuses();
        Task<List<CaseFileStatus>> GetCaseFileStatuses();
        Task<List<CaseFile>> GetFileNumbers();
        Task<string> GetReferenceNumber(Guid ReferenceId, int SubModulId);
        Task<string> GetConsultationReferenceNumber(Guid ReferenceId);
        Task<List<ConsultationFile>> GetConsultationFileNumber();
        Task<List<CaseRequest>> GetRequestNumber();
        Task<List<Priority>> GetCasePriorities();
        Task<List<Frequency>> GetFrequency();
        Task<List<CmsCaseVisitType>> GetCourtVisitTypes();
        Task<List<CourtType>> GetCourtTypes();
        Task<List<Court>> GetCourts();
        Task<List<Chamber>> GetChambers();
        Task<List<ChamberShift>> GetShift();
        Task<List<ChamberNumber>> GetChamberNumbersbyChamberId(int chamberId);
        Task SaveAssignLawyerToCourt(CmsAssignLawyerToCourt cmsAssignLawyerToCourt);
        Task<List<ResponseType>> GetResponseTypes();
        Task<List<CaseTemplate>> GetCaseTemplates(int attachmentTypeId);

        #region Tasks

        Task<List<TaskType>> GetTaskType();
        Task<List<TaskSubType>> GetTaskSubType();
        Task<List<string>> GetFileNumber();

        #endregion
        Task<List<HearingStatus>> GetCaseHearingStatuses();
        Task<List<JudgementType>> GetCaseJudgementTypes();
        Task<List<JudgementStatus>> GetCaseJudgementStatuses();
        Task<List<JudgementCategory>> GetCaseJudgementCategories();
        Task<List<ExecutionFileLevel>> GetExecutionFileLevels();
        Task<List<UserVM>> GetUsersBySector(int? sectorTypeId);
        Task<List<UserVM>> GetUsersBySectorForCourtAssignment(int? sectorTypeId);
        Task<List<UserVM>> GetUsersByDepartment(int DepartmentId);
        Task<List<AssignLawyerToCourtVM>> GetAssignLawyerToCourt(AdvanceSearchVMAssignLawyerToCourt advanceSearchVM);
        Task<string> GetUserIdByUserEmail(string email);
        Task<string> GetSupervisorByLawyerId(string lawyerId);
        Task<List<Subtype>> GetRequestSubtypesByRequestId(int requestTypeId);
        Task<List<LawyerVM>> GetLawyersBySector(int? sectorTypeId);
        Task<List<ListLawyerSupervisorAssignmentVM>> GetLawyerSupervisorAssignmentListBySector(int? sectorTypeId);
        Task<List<LawyerVM>> GetLawyersBySectorAndChamber(int? sectorTypeId, string? UserId, int chamberNumberId = 0);
        Task<List<LawyerVM>> GetSupervisorsBySector(int? sectorTypeId);
        Task<List<Subtype>> GetAllRequestSubtypes();
        Task<List<Department>> GetDepartments();
        Task<List<MeetingType>> GetMeetingTypes();

        Task<List<Role>> GetRoles();
        Task<CmsAssignLawyerToCourt> GetAssignLawyertoCourtById(Guid id);
        Task EditAssignLawyertoCourt(CmsAssignLawyerToCourt cmsAssignLawyerToCourt);
        Task AssignSupervisorAndManagerToLawyers(CmsLawyerSupervisorVM item);
        Task DeleteAssignLawyerToCourt(AssignLawyerToCourtVM assignLawyerToCourtVM);
        Task<List<ConsultationPartyType>> GetConsultationPartyTypes();
        Task<List<ConsultationRequest>> GetConsultationRequestNumber();
        //Task<List<CntContactJobRole>> GetJobRoles();
        Task<List<MeetingStatus>> GetMeetingStatus();
        Task<List<CntContactJobRole>> GetContactJobRole();
        Task<List<CntContactType>> GetContactType();
        Task<List<ReferenceNumberVM>> GetReferenceNumberBySubmoduleId(int SubModulId, int SectorId);
        Task<List<Module>> GetModules();
        Task<List<NotificationEvent>> GetNotificationEvents();
        Task<List<SubModule>> GetSubmodule();
        Task<List<UserFloor>> GetFloors();
        Task<List<UserEmploymentInformation>> GetStoreKeepers(string userId, int userTypeId);
        Task<List<StoreInchargeVM>> GetListofStoreInchargesbySectortypeId(int sectortypeId);
        Task<List<StoreInchargeVM>> GetUserbyRoleId(Guid roleId);
        Task<List<Company>> GetCompanyList();
        Task<List<City>> GetCityList();
        Task<bool> AddNewCompany(Company args);
        Task<bool> AddNewDesignation(Designation args);
        Task<List<ContactFileLinkVM>> GetContactDetailsForFile(Guid fileId);
        Task<List<MeetingAttendeeStatus>> GetAttendeeMeetingStatus();
        Task<List<LegallegislationtypesVM>> Getlegallegislationtypes();
        Task<List<LegalPrincipleTypeVM>> GetLegalPrincipleTypes();
        Task<List<LmsLiteratureTagVM>> GetLmsLiteratureTags();
        Task SaveLiteratureTags(LiteratureTag communication);
        Task<LegalLegislationType> GetLegalLegislationtypeById(int Id);
        Task<LiteratureTag> GetLmsLiteratureTagsById(int Id);


        Task<bool> RemoveContact(ContactFileLinkVM args);
        Task<bool> AddSelectedContactToFile(IList<ContactListVM> selectedContact, Guid? fileId, int? fileModule, string CurrentUser);
        Task DeleteLiteratureTags(LmsLiteratureTagVM literatureTagVM);
        Task ActiveLiteratureTags(LmsLiteratureTagVM literatureTagVM);
        Task UpdateLiteratureTags(LiteratureTag literatureTagVM);

        #region Legal legislation Type
        Task SavelegislationType(LegalLegislationType legislationType);
        Task UpdatelegislationType(LegalLegislationType legislationType);
        Task ActivelegislationType(LegallegislationtypesVM legislationType);
        Task DeletelegislationType(LegallegislationtypesVM legislationType);
        #endregion

        #region Legal legalPrinciple Type 
        Task SavelegalPrincipleTypes(LegalPrincipleType legalPrincipleTypes);
        Task UpdatelegalPrincipleTypes(LegalPrincipleType legalPrincipleTypes);
        Task DeletelegalPrincipleTypes(LegalPrincipleTypeVM legalPrincipleTypes);
        Task ActivelegalPrincipleTypes(LegallegislationtypesVM legislationType);
        Task ActiveDocumentTypes(AttachmentTypeVM attachmentType);
        #endregion

        #region Legal legal Publication Source Names
        Task SavelegalPublicationSourceNames(LegalPublicationSourceName legalPublicationSourceNames);
        Task UpdatelegalPublicationSourceName(LegalPublicationSourceName legalPublicationSourceNames);
        Task DeletelegalPublicationSourceNames(LegalPublicationSourceNameVM legalPublicationSourceName);
        Task ActivePublicationSourceNames(LegalPublicationSourceNameVM legalPublicationSourceName);
        Task<List<LegalPublicationSourceNameVM>> GetLegalPublicationSourceName();
        Task<LegalPublicationSourceName> GetLegalPublicationSourceNameById(int PublicationNameId);
        #endregion

        Task<LegalPrincipleType> GetLegalPrincipleTypesById(int Id);
        #region Government Entites
        Task<List<GovernmentEntitiesVM>> GetGovernmentEntiteslist();
        Task<GovernmentEntity> GetGovernmentEntitysById(int EntityId);
        Task<GovernmentEntity> SaveGovernmentEntity(GovernmentEntity governmentEntity);
        Task<GovernmentEntity> UpdateGovernmentEntity(GovernmentEntity governmentEntity);
        Task<GovernmentEntitiesVM> DeleteGovernmentEntity(GovernmentEntitiesVM governmentEntity);
        Task<GovernmentEntitiesVM> ActiveGovernmentEntities(GovernmentEntitiesVM governmentEntities);
        Task SyncGEsAndDepartments(string username, DataSet sitesList, DataSet sitesBranchList);
        Task<GovermentEntityAndDepartmentSyncLog> GetLatestGEsAndDepartmentsSyncLog();

        #endregion
        #region Court Types
        Task<List<CourtDetailVM>> GetCourtTypeList();
        Task<Court> GetCourtTypesById(int Id);
        Task<Court> SaveCourtType(Court courts);
        Task<Court> UpdateCourtType(Court courts);
        Task<CourtDetailVM> DeleteCourtType(CourtDetailVM courts);
        Task<CourtDetailVM> ActiveCourtTypes(CourtDetailVM courts);
        #endregion
        #region Chamber lookup
        Task<List<ChamberDetailVM>> GetChamberList();
        Task<Chamber> GetChamberById(int Id);
        Task<Chamber> SaveChamber(Chamber chamber);
        Task<Chamber> UpdateChamber(Chamber chamber);
        Task<ChamberDetailVM> DeleteChamber(ChamberDetailVM chamber);
        Task<ChamberDetailVM> ActiveChamber(ChamberDetailVM chamber);
        Task<List<ChamberDetailVM>> GetChamberDetailById(int Id);
        Task SaveChamberOperatingSector(ChamberOperatingSector chamberOperatingSector);
        Task<List<Chamber>> GetChamberByCourtId(int courtId);
        #endregion
        #region Department lookup
        Task<List<DepartmentDetailVM>> GetDepartmentList();
        Task<Department> GetDepartmentById(int Id);
        Task<Department> SaveDepartment(Department department);
        Task<Department> UpdateDepartment(Department department);
        Task<DepartmentDetailVM> DeleteDepartment(DepartmentDetailVM department);
        Task<DepartmentDetailVM> ActiveDepartment(DepartmentDetailVM department);
        #endregion

        #region TaskType Lookup
        Task<List<TaskTypeVM>> GetTaskTypeList();
        Task<TaskType> GetTaskTypeById(int TypeId);
        Task<TaskType> UpdateTaskType(TaskType taskType);
        #endregion
        #region Communication Type
        Task<List<CommunicationTypeVM>> GetCommunicationTypeList();
        Task<CommunicationType> GetCommunicationByTypeId(int CommunicationTypeId);
        Task<CommunicationType> SaveCommunicationType(CommunicationType communication);
        Task<CommunicationType> UpdateCommunicationType(CommunicationType communicationType);
        #endregion
        #region Document Type 
        Task<List<AttachmentTypeVM>> GetDocumentTypeList();
        Task UpdateDocumentType(AttachmentType ldsDocument);
        Task SaveDocumentType(AttachmentType attachment);
        Task<AttachmentType> FindAndSaveAttachmentType(string attachment);
        Task<AttachmentType> GetDocumentTypeById(int AttachmentTypeId);
        Task<AttachmentType> GetDocumentTypeByName(string AttachmentType);
        Task<List<Module>> GetModule();
        Task<List<Subtype>> GetSubTypeId();
        #endregion
        #region case File Status  
        Task<List<CaseFileStatusVM>> GetCaseFileStatusList();
        Task<CaseFileStatus> UpdateCaseFileStatus(CaseFileStatus caseFileStatus);
        Task<CaseFileStatus> GetCaseFileStatusById(int Id);
        #endregion
        #region case Request Status  
        Task<List<CaseRequestStatusVM>> GetCaseRequestStatusList();
        Task<CaseRequestStatus> UpdateCaseRequestStatus(CaseRequestStatus caseRequestStatus);
        Task<CaseRequestStatus> GetCaseRequestStatusById(int Id);
        #endregion
        #region case  Status  
        Task<List<CmsRegisteredCaseStatusVM>> GetCaseStatusList();
        Task UpdateCaseStatus(CmsRegisteredCaseStatus RegisteredCaseStatus);
        Task<CmsRegisteredCaseStatus> GetCaseStatusById(int Id);
        #endregion


        Task<List<GovernmentEntity>> GetAllUserGroupsList();
        Task<List<CmsComsNumPatternVM>> GetAllCaseRequestNumber(int PatternTypeId);
        Task<CmsComsNumPattern> GetCmsPatternById(Guid Id);
        Task<List<Group>> GetCmsComsNumberPatternGroupById(Guid Id);

        Task<CmsComsNumPattern> SaveCMSCOMSPattrenNumber(CmsComsNumPattern chamber);
        //Task<CmsComsNumPattern> UpdateCaseFileNumberPattren(CmsComsNumPattern chamber);
        Task<CmsComsNumPatternHistory> UpdateCaseFileNumberPattrenHistory(CmsComsNumPatternHistory PatternHistory);
        Task<List<CmsComsNumPatternType>> GetCmsComsNumberPatterntype();

        Task<CmsComsNumPatternVM> DeleteCmComsPattern(CmsComsNumPatternVM cmsComsNumPatternVM);
        #region Sector Type Lookup
        Task<List<SectorTypeVM>> GetSectorTypeList();
        Task<OperatingSectorType> GetSectorTypeById(int Id);
        Task<List<SectorBuilding>> GetSectorBuilding();
        Task<List<SectorFloor>> GetSectorFloor();

        Task UpdateSectorType(OperatingSectorType SectorType);
        #endregion
        //Task<List<GovernmentEntitiesUsersVM>> GetAllGEUserList();


        Task<TimeIntervalDetailVM> GetCommunicationResponseDetailByid(int Id);

        Task<List<WSCommCommunicationTypes>> GetCommunicationType();




        #region Request Type 
        Task<List<RequestTypeVM>> GetRequestTypeList();

        Task<RequestType> GetRequestTypeById(int Id);

        Task<RequestType> UpdateRequestType(RequestType subType);
        #endregion

        #region SubType lookup
        Task<List<SubTypeVM>> GetSubTypeList(int RequestTypeId);
        Task<Subtype> GetSubTypeById(int Id);
        Task<Subtype> SaveSubType(Subtype subType);
        Task<Subtype> UpdateSubType(Subtype subType);
        Task<SubTypeVM> DeleteSubtype(SubTypeVM subTypeVM);
        Task<SubTypeVM> ActiveSubType(SubTypeVM SubTypeVM);
        #endregion

        #region Consultation Legislation File Type
        Task<List<ConsultationLegislationFileTypeVM>> GetConsultationLegislationFileTypeList();
        Task<ConsultationLegislationFileType> GetConsultationLegislationFileTypeById(int Id);
        Task<ConsultationLegislationFileType> SaveConsultationLegislationFileType(ConsultationLegislationFileType ConsultationLegislationFileType);
        Task<ConsultationLegislationFileType> UpdateConsultationLegislationFileType(ConsultationLegislationFileType ConsultationLegislationFileType);
        Task<ConsultationLegislationFileTypeVM> DeleteConsultationLegislationFileType(ConsultationLegislationFileTypeVM ConsultationLegislationFileTypeVM);
        Task<ConsultationLegislationFileTypeVM> ActiveConsultationLegislationFileType(ConsultationLegislationFileTypeVM ConsultationLegislationFileTypeVM);
        Task<List<ConsultationLegislationFileType>> GetConsultationLegislationFileTypes();

        #endregion

        #region Consultation Legislation File Type
        Task<List<ComsInternationalArbitrationTypeVM>> GetComsInternationalArbitrationTypeList();
        Task<ComsInternationalArbitrationType> GetComsInternationalArbitrationTypeById(int Id);
        Task<ComsInternationalArbitrationType> SaveComsInternationalArbitrationType(ComsInternationalArbitrationType ComsInternationalArbitrationType);
        Task<ComsInternationalArbitrationType> UpdateComsInternationalArbitrationType(ComsInternationalArbitrationType ComsInternationalArbitrationType);
        Task<ComsInternationalArbitrationTypeVM> DeleteComsInternationalArbitrationType(ComsInternationalArbitrationTypeVM ComsInternationalArbitrationTypeVM);
        Task<ComsInternationalArbitrationTypeVM> ActiveComsInternationalArbitrationType(ComsInternationalArbitrationTypeVM ComsInternationalArbitrationTypeVM);
        #endregion

        Task<Subtype> SaveSubTypes(Subtype SaveSubTypes);

        Task<CmsComsNumPatternHistory> GetCmsPatternHistoryById(Guid Id);
        Task<List<LookupsHistory>> GetLookupHistoryListByRefernceId(int Id, int LookupstableId);

        Task<List<CmsComsNumPatternHistoryVM>> GetCmsComNumPatternHistoryDetail(Guid Id);
        Task<List<GovernmentEntitiesPatternVM>> GetAllAGEUserListPatternAttached(Guid Id, int SelectedPatternTypeId, bool IsDefault);
        Task<bool> CheckPatternAlreadyAttachedGovtid(List<int> EntityId, int SelectedPatternTypeId);


        #region Chamber Number CRUD
        Task<List<ChambersNumberDetailVM>> GetChamberNumberList();
        Task<ChambersNumberDetailVM> DeleteChambersNumber(ChambersNumberDetailVM chambersNumber);
        Task<ChambersNumberDetailVM> ActiveChambersNumber(ChambersNumberDetailVM chambersNumber);
        Task<ChamberNumber> GetChamberNumberById(int Id);
        Task<ChamberNumber> SaveChamberNumber(ChamberNumber chambersNumber);
        Task<ChamberNumber> UpdateChamberNumber(ChamberNumber chambersNumber);
        Task<List<ChambersNumberDetailVM>> GetChamberNumberDetailById(int Id);
        #endregion

        #region Chamber Number Hearing CRUD
        Task<List<ChamberNumber>> GetChamberNumber();
        Task<List<HearingDay>> GetHearingDays();
        Task<List<ChamberNumberHearingDetailVM>> GetChamberNumberHearingList();
        Task<ChamberNumberHearing> SaveChamberNumberHearing(ChamberNumberHearing chamberNumberHearing);
        Task<ChamberNumberHearing> UpdateChamberNumberHearing(ChamberNumberHearing chamberNumberHearing);
        Task<ChamberNumberHearingDetailVM> GetChamberNumberHearingById(int Id);
        Task<ChamberNumberHearingDetailVM> DeleteChambersNumberHearing(ChamberNumberHearingDetailVM chamberNumberHearing);
        #endregion

        #region  Government Entity Department CRUD
        Task<List<GovernmentEntitiesSectorsVM>> GetGovernmentEntityDepartmentList();
        Task<GovernmentEntitiesSectorsVM> DeleteGovernmentEntityDepartment(GovernmentEntitiesSectorsVM GESector);
        Task<GovernmentEntitiesSectorsVM> ActiveGovernmentEntityDepartment(GovernmentEntitiesSectorsVM GESector);
        Task<GEDepartments> GetGovtEntityDepartmentById(int Id);
        Task<GEDepartments> SaveGovernmentEntityDepartment(GEDepartments GESector);
        Task<GEDepartments> UpdateGovernmentEntityDepartment(GEDepartments GESector);
        Task<bool> CheckDefaultReceiverAlreadyAttached(int EntityId, int DepartmentId);
        #endregion

        #region  Government Entity Representative CRUD
        Task<List<GovernmentEntitiesRepresentativeVM>> GetGovernmentEntiteRepresentativesList();
        Task<GovernmentEntitiesRepresentativeVM> DeleteGovernmentEntityRepresentative(GovernmentEntitiesRepresentativeVM GERepresentative);
        Task<GovernmentEntitiesRepresentativeVM> ActiveGovernmentEntityRepresentative(GovernmentEntitiesRepresentativeVM GERepresentative);
        Task<GovernmentEntityRepresentative> GetGovernmentRepresentativeById(Guid Id);
        Task<GovernmentEntityRepresentative> SaveGovernmentEntityRepresentative(GovernmentEntityRepresentative GERepresentative);
        Task<GovernmentEntityRepresentative> UpdateGovernmentEntityRepresentative(GovernmentEntityRepresentative GERepresentative);
        #endregion
        Task<List<CmsComsNumPatternHistory>> GetCmsComsNumberPatternHistoryForEditing(Guid Id);
        #region Moj Roll
        Task<List<RMSChamberVM>> GetChamberByUserId(string UserId);
        Task<List<RMSCourtsVM>> GetCourtByUserId(string UserId);
        Task<List<Models.ViewModel.AdminVM.Lookups.MOJRollsChamberNumberVM>> GetChamberNumberByUserId(string UserId);
        Task<List<MOJRollsChamberCourtChamberNumberVM>> GetAllChamberCourtChamberNumberForMojRollsByUserId(string UserId);
        Task<List<MOJRollsChamberCourtChamberNumberVM>> GetAllChamberCourtChamberNumberForMojRolls();
        #endregion

        #region   Ep Nationality CRUD
        Task<List<EpNationalityVM>> GetEpNationalityList();
        Task<EpNationalityVM> DeleteEpNationality(EpNationalityVM EPNationality);
        Task<EpNationalityVM> ActiveEpNationality(EpNationalityVM EPNationality);
        Task<Nationality> GetEpNationalityById(int Id);
        Task<Nationality> SaveNationality(Nationality EPNationality);
        Task<Nationality> UpdateNationality(Nationality EPNationality);
        #endregion

        #region   Ep Grade CRUD
        Task<List<EpGradeVM>> GetEpGradeList();
        Task<EpGradeVM> DeleteEpGrade(EpGradeVM EPGrade);
        Task<EpGradeVM> ActiveEpGrade(EpGradeVM EPGrade);
        Task<Grade> GetEpGradeById(int Id);
        Task<Grade> SaveGrade(Grade EPGrade);
        Task<Grade> UpdateGrade(Grade EPGrade);
        #endregion

        #region   Ep Grade Type CRUD
        Task<List<EpGradeTypeVM>> GetEpGradeTypeList();
        Task<EpGradeTypeVM> DeleteEpGradeType(EpGradeTypeVM EPGradeType);
        Task<GradeType> GetEpGradeTypeById(int Id);
        Task<GradeType> SaveGradeType(GradeType EPGradeType);
        Task<GradeType> UpdateGradeType(GradeType EPGradeType);
        #endregion

        #region Gender Enum
        Task<List<EpGenderVM>> GetGenderList();
        Task<Gender> GetGenderById(int Id);
        Task<Gender> UpdateGender(Gender gender);
        #endregion

        #region   Ep Designation CRUD
        Task<List<EpDesignationVM>> GetEpDesignationList();
        Task<EpDesignationVM> DeleteEpDesignation(EpDesignationVM EPDesignation);
        Task<Designation> GetEpDesignationById(int Id);
        Task<Designation> SaveDesignation(Designation EPDesignation);
        Task<Designation> UpdateDesignation(Designation EPDesignation);
        #endregion

        #region Bank Details 
        Task<List<CmsBank>> GetBankNames();
        Task<CmsBank> GetBankNameById(int bankId);
        Task<List<CmsBankGovernmentEntity>> GetBankDetailByEntityId(int EntityId);
        Task<CmsBankGovernmentEntity> DeleteBankDetail(CmsBankGovernmentEntity cmsBankGovernmentEntity);
        #endregion
        Task<List<Court>> GetCourtById(string UserId);

        #region   Ep Contract Type CRUD
        Task<List<EpContractTypeVM>> GetEpContractTypeList();
        Task<EpContractTypeVM> DeleteEpContractType(EpContractTypeVM epContractTypeVM);
        Task<ContractType> GetEpContractTypeById(int Id);
        Task<ContractType> SaveContractType(ContractType contractType);
        Task<ContractType> UpdateContractType(ContractType contractType);
        #endregion

        #region   Book Author CRUD
        Task<List<BookAuthorVM>> GetBookAuthorList();
        Task<BookAuthorVM> DeleteBookAuthor(BookAuthorVM bookAuthorVM);
        Task<LmsLiteratureAuthor> GetBookAuthorById(int AuthorId);
        Task<LmsLiteratureAuthor> SaveBookAuthor(LmsLiteratureAuthor lmsLiteratureAuthor);
        Task<LmsLiteratureAuthor> UpdateBookAuthor(LmsLiteratureAuthor lmsLiteratureAuthor);
        #endregion

        #region   G2G Correspondences Receiver 
        Task<List<CmsSectorTypeGEDepartmentVM>> GetG2GCorrespondencesReceiverList();
        Task<CmsSectorTypeGEDepartmentVM> DeleteG2GCorrespondencesReceiver(CmsSectorTypeGEDepartmentVM cmsSectorTypeGEDepartmentVM);
        Task<List<GEDepartments>> GetDepartmentByGEEntityId(List<int> EntityIds);
        Task<CmsSectorTypeGEDepartment> SaveG2GCorrespondencesReceiver(CmsSectorTypeGEDepartment cmsSectorTypeGEDepartment);
        #endregion

        #region  Literature Dewey Number Pattern CRUD
        Task<List<LiteratureDeweyNumberPatternVM>> GetLiteratureDeweyNumberPatternsList();
        Task<LiteratureDeweyNumberPatternVM> DeleteLiteratureDeweyNumberPattern(LiteratureDeweyNumberPatternVM literatureDeweyNumberPatternVM);
        Task<LiteratureDeweyNumberPattern> GetLiteratureDeweyNumberPatternById(Guid Id);
        Task<LiteratureDeweyNumberPattern> SaveLiteratureDeweyNumberPattern(LiteratureDeweyNumberPattern literatureDeweyNumberPattern);
        Task<LiteratureDeweyNumberPattern> UpdateLiteratureDeweyNumberPattern(LiteratureDeweyNumberPattern literatureDeweyNumberPattern);
        #endregion

        #region Check Name En and Name Ar Already Exists
        Task<bool> CheckNameEnExists(string NameEn, int requestTypeId, int subTypeId);
        Task<bool> CheckNameArExists(string NameAr, int requestTypeId, int subTypeId);
        #endregion

        #region Weekdays Settings
        Task<IEnumerable<WeekdaysSetting>> GetWeekdaysSettings();
        #endregion

        #region Get Users List By SectorId And RoleId
        Task<List<SectorUsersVM>> GetUsersListBySectorIdAndRoleId(string RoleId, int SectorTypeId);
        Task<GovtEntityRepresentativeNamesResponseVM> GetGovernmentEntityRepresentatives(GovtEntityIdsPayload govtEntityIds);
        #endregion

        #region Get Courts, Chambers, Chamber Numbers for Mobile App
        Task<List<MobileAppCourtVM>> GetCourtByUserIdForMobileApp(string userId);
        Task<List<MobileAppChamberVM>> GetChambersByUserIdForMobileApp(int courtId, string userId);
        Task<List<MobileAppChamberNumberVM>> GetChamberNumberByUserIdForMobileApp(int courtId, int chamberId, string userId);
        #endregion

        #region Get Stock Taking Status
        Task<List<LmsStockTakingStatus>> GetStockTakingStatus();
        #endregion

        #region Sector Role ( Save Sector Roles, Get Sector Roles )
        Task<List<SectorRolesVM>> GetRolesBySectorIds(List<int> sectorIds);
        Task<List<CmsOperatingSectorTypesRoles>> GetRolesOfSectorBySectorId(int sectorId);
        #endregion

        #region Service Request Approval ( CRUD )
        Task<bool> AddServiceRequestApproval(ServiceRequestFinalApprovalVM serviceRequestFinalApproval);
        Task<bool> UpdateServiceRequestApproval(ServiceRequestFinalApprovalVM serviceRequestFinalApproval);
        Task<List<ServiceRequestApprovalDetailVm>> GetAllServiceRequestApprovalList();
        Task<ServiceRequestApprovalDetailVm> GetServiceRequestApprovalDetail(int Id);
        Task<List<ServiceRequestApprovalHistoryVm>> GetServiceRequestApprovalHistory(int approvalId);
        #endregion
        Task<List<PreCourtType>> GetPreCourtTypes();
        Task<List<CourtType>> GetCourtTypesByRequestType(int requestTypeId);
        Task<List<ComsInternationalArbitrationType>> GetConsultationInternationalArbitrationTypes();
        Task<List<ManagerTaskReminderVM>> GetManagerTaskReminderData(Guid TaskId);
    }
}
