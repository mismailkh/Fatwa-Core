using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.CaseManagment;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Lms;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using FATWA_DOMAIN.Models.ViewModel.Shared;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using FATWA_DOMAIN.Models.WorkflowModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using FATWA_DOMAIN.Models.LegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel.LegalPrinciple;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel.ContactManagmentVMs;
using FATWA_DOMAIN.Models.Contact;
using FATWA_DOMAIN.Models.InventoryManagement;
using FATWA_DOMAIN.Models.ViewModel.InventoryManagementVMs;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.TimeTrackingVM;
using FATWA_DOMAIN.Models.WorkerService;
using FATWA_DOMAIN.Models.TimeInterval;
using FATWA_DOMAIN.Models.ViewModel.TimeIntervalVMs;
using FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs;
using System.Collections.Generic;
using FATWA_DOMAIN.Models.ViewModel.MojRollsVM;
using FATWA_DOMAIN.Models.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.Email;
using FATWA_DOMAIN.Models.ViewModel.TranslationMobileAppVMs;
using FATWA_DOMAIN.Models.MobileApp;
using FATWA_DOMAIN.Models.ViewModel.MobileAppVMs;
using FATWA_DOMAIN.Models.ServiceRequestModels;
using FATWA_DOMAIN.Models.RabbitMQ;
using FATWA_DOMAIN.Models.ViewModel.OrganizingCommitteeVMs;
using FATWA_DOMAIN.Models.ViewModel.RolesVM;
using FATWA_DOMAIN.Models.BugReporting;
using FATWA_DOMAIN.Models.ViewModel.BugReportingVMs;


//< History Author = 'Umer Zaman' Date = '2022-03-18' Version = "1.0" Branch = "master" >Add Literature Type, Classification & Index DbSet and model builders</History>

namespace FATWA_INFRASTRUCTURE.Database
{
    public partial class DatabaseContext : IdentityDbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        #region Model/Table
        
        public virtual DbSet<RMQ_UnpublishMessage> RMQ_UnpublishMessages { get; set; } = null;
        public virtual DbSet<EmailConfiguration> EmailConfigurations { get; set; } = null;

        #region VM
        public virtual DbSet<Group> Groups { get; set; } = null;
        public virtual DbSet<GroupType> GroupType { get; set; } = null!;
        public virtual DbSet<GroupAccessType> GroupAccessType { get; set; } = null!;
        public virtual DbSet<UserGroup> UserGroups { get; set; } = null;
        public virtual DbSet<UserPasswordHistory> UserPasswordHistory { get; set; } = null;
        public virtual DbSet<GroupClaims> UmsGroupClaims { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<RoleBasicDetailVM> RoleBasicDetailVMs { get; set; } = null!;
        public virtual DbSet<CmsOperatingSectorTypesRoles> CmsOperatingSectorTypesRoles { get; set; } = null!;
        public virtual DbSet<UserClaims> UmsUserClaims { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;
        public virtual DbSet<ClaimUms> Claims { get; set; } = null!;
        public virtual DbSet<Nationality> Nationalities { get; set; } = null!;
        public virtual DbSet<Gender> Genders { get; set; } = null!;
        public virtual DbSet<EmployeeType> EmployeeTypes { get; set; } = null!;
        public virtual DbSet<EmployeeWorkingTime> WorkingTimes { get; set; } = null!;
        public virtual DbSet<Grade> UserGrades { get; set; } = null!;
        public virtual DbSet<GradeType> GradeTypes { get; set; } = null!;
        public virtual DbSet<ContractType> ContractTypes { get; set; } = null!;
        public virtual DbSet<EmployeeStatus> EmployeeStatuses { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Designation> Designations { get; set; } = null!;
        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<UserFloor> UserFloors { get; set; } = null!;
        public virtual DbSet<UserEmploymentInformation> UserEmploymentInformation { get; set; } = null!;
        public virtual DbSet<UserPersonalInformation> UserPersonalInformation { get; set; } = null!;
        public virtual DbSet<UserContactInformation> UserContactInformation { get; set; } = null!;
        public virtual DbSet<UserWorkExperience> UserWorkExperience { get; set; } = null!;
        public virtual DbSet<UserTrainingAttended> UserTrainingAttended { get; set; } = null!;
        public virtual DbSet<UserEducationalInformation> UserEducationalInformation { get; set; } = null!;
        public virtual DbSet<UserAdress> UserAdress { get; set; } = null!;
        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Governorate> Governorates { get; set; } = null!;
        public virtual DbSet<WebSystem> WebSystems { get; set; } = null!;
        public virtual DbSet<GEDepartments> GEDepartments { get; set; } = null!;
        public virtual DbSet<UserActivity> UserActivities { get; set; } = null!;
        public virtual DbSet<EmployeeLeaveDelegationInformation> EmployeeLeaveDelegation { get; set; }
        public virtual DbSet<WorkingHour> WorkingHours { get; set; } = null!;
        public virtual DbSet<ServiceRequestFinalApproval> ServiceRequestFinalApprovals { get; set; } = null!;
        public virtual DbSet<ServiceRequestFinalApprovalActivities> ServiceRequestFinalApprovalActivities { get; set; } = null!;
        #endregion

        public virtual DbSet<CaseRequest> CaseRequests { get; set; } = null;
        public virtual DbSet<CmsWithdrawRequest> WithdrawRequests { get; set; } = null;
        public virtual DbSet<CmsCaseRequestLinkedRequest> CaseRequestLinkedRequests { get; set; } = null;
        public virtual DbSet<CmsCaseFileLinkedFile> CmsCaseFileLinkedFiles { get; set; } = null;
        public virtual DbSet<CaseFile> CaseFiles { get; set; } = null;
        public virtual DbSet<ConsultationFile> ConsultationFiles { get; set; } = null;
        public virtual DbSet<CmsAssignCaseFileBackToHos> CmsAssignCaseFileBackToHos { get; set; } = null;
        public virtual DbSet<CourtType> CourtTypes { get; set; } = null;
        public virtual DbSet<PreCourtType> PreCourtTypes { get; set; } = null!;
        public virtual DbSet<CmsRegisteredCase> CmsRegisteredCases { get; set; } = null;
        public virtual DbSet<CmsRegisteredCaseStatus> CmsRegisteredCasestatuss { get; set; } = null;
        public virtual DbSet<MergeRequest> MergeRequests { get; set; } = null;
        public virtual DbSet<MergeRequestSecondaries> MergeRequestSecondaries { get; set; } = null;
        public virtual DbSet<CmsRegisteredCaseMergedCase> CmsRegisteredCaseMergedCases { get; set; } = null;
        public virtual DbSet<CmsRegisteredCaseSubCase> CmsRegisteredCaseSubCases { get; set; } = null;
        public virtual DbSet<Court> Courts { get; set; } = null;
        public virtual DbSet<Chamber> Chambers { get; set; } = null;
        public virtual DbSet<ChamberOperatingSector> ChamberOperatingSectors { get; set; } = null;
        public virtual DbSet<ChamberNumber> ChamberNumbers { get; set; } = null;
        public virtual DbSet<ChamberShift> ChamberShifts { get; set; } = null;
        public virtual DbSet<HearingDay> HearingDays { get; set; } = null;
        public virtual DbSet<CaseAssignment> CaseFileAssignment { get; set; } = null;
        public virtual DbSet<CmsLawyerSupervisor> CmsLawyerSupervisor { get; set; } = null;
        public virtual DbSet<CmsCaseFileSectorAssignment> CmsCaseFileSectorAssignment { get; set; } = null;
        public virtual DbSet<ComsConsultationFileSectorAssignment> ComsConsultationFileSectorAssignments { get; set; } = null;
        public virtual DbSet<CmsApprovalTracking> CmsApprovalTracking { get; set; } = null;
        public virtual DbSet<ComsApprovalTracking> ComsApprovalTracking { get; set; } = null;

        public virtual DbSet<TempCaseAssignment> TempCaseAssignments { get; set; } = null;
        public virtual DbSet<TempConsultationAssignment> TempConsultationAssignments { get; set; } = null;
        public virtual DbSet<ConsultationAssignment> ConsultationAssignments { get; set; } = null;
        public virtual DbSet<ConsultationAssignmentHistory> ConsultationAssignmentHistorys { get; set; } = null;
        public virtual DbSet<Hearing> Hearings { get; set; } = null;
        public virtual DbSet<Judgement> Judgements { get; set; } = null;
        public virtual DbSet<PostponeHearing> PostponeHearings { get; set; } = null;
        public virtual DbSet<OutcomeHearing> OutcomeHearings { get; set; } = null;
        public virtual DbSet<HearingStatus> HearingStatuses { get; set; } = null;
        public virtual DbSet<JudgementType> JudgementTypes { get; set; } = null;
        public virtual DbSet<JudgementStatus> JudgementStatuses { get; set; } = null;
        public virtual DbSet<JudgementCategory> JudgementCategories { get; set; } = null;
        public virtual DbSet<ExecutionFileLevel> ExecutionFileLevels { get; set; } = null;
        public virtual DbSet<CmsExecutionFileStatus> CmsExecutionFileStatuses { get; set; } = null;

        public virtual DbSet<CaseAssignmentHistory> CaseFileAssignmentHistory { get; set; } = null;
        public virtual DbSet<CmsAssignLawyerToCourt> CmsAssignLawyerToCourts { get; set; } = null;
        public virtual DbSet<AssignLawyerToCourtVM> AssignLawyerToCourtVMs { get; set; } = null;
        public virtual DbSet<CasePartyLink> CasePartyLink { get; set; } = null;
        public virtual DbSet<ConsultationParty> ConsultationParties { get; set; } = null;


        public virtual DbSet<MojRegistrationRequest> MojRegistrationRequests { get; set; } = null;

        public virtual DbSet<CmsSaveCloseCaseFile> cmsSaveCloseCaseFiles { get; set; } = null;

        public virtual DbSet<Module> Modules { get; set; } = null!;
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; } = null!;
        public virtual DbSet<ProcessLog> ProcessLogs { get; set; } = null!;
        public virtual DbSet<LmsLiterature> LmsLiteratures { get; set; } = null!;
        public virtual DbSet<LiteratureTag> LiteratureTags { get; set; } = null!;
        public virtual DbSet<LiteratureDetailLiteratureTag> LiteratureDetailLiteratureTags { get; set; } = null!;
        public virtual DbSet<LdsDocumentComment> LdsDocumentComment { get; set; } = null!;
        public virtual DbSet<LpsPrincipleComment> LpsPrincipleComments { get; set; } = null!;
        public virtual DbSet<GovermentEntityAndDepartmentSyncLog> GovermentEntityAndDepartmentSyncLogs { get; set; } = null!;
        public virtual DbSet<GovernmentEntity> GovernmentEntity { get; set; } = null!;
        public virtual DbSet<CmsGovtEntityNumPattern> CmsGovtEntityNumPattern { get; set; } = null!;
        public virtual DbSet<GovernmentEntitiesPatternVM> governmentEntitiesPatternVMs { get; set; } = null!;
        public virtual DbSet<GovernmentEntityRepresentative> GovernmentEntityRepresentative { get; set; } = null!;
        public virtual DbSet<LmsLiteratureIndex> LmsLiteraturesIndex { get; set; } = null!;
        public virtual DbSet<LmsLiteratureParentIndex> LmsLiteratureParentIndexs { get; set; } = null!;
        public virtual DbSet<LmsLiteratureIndexDivisionAisle> LmsLiteratureIndexDivisionAisles { get; set; } = null!;
        public virtual DbSet<LmsLiteratureClassification> LmsLiteratureClassification { get; set; } = null!;
        public virtual DbSet<LmsLiteratureBorrowDetail> LmsLiteratureBorrowDetails { get; set; } = null!;
        public virtual DbSet<LmsLiteratureBorrowHistory> LmsLiteratureBorrowHistory { get; set; } = null!;
        public virtual DbSet<Workflow> Workflow { get; set; } = null!;
        public virtual DbSet<Module> WorkflowModule { get; set; } = null!;
        public virtual DbSet<WorkflowTrigger> WorkflowTrigger { get; set; } = null!;
        public virtual DbSet<WorkflowAttachmentType> WorkflowAttachmentType { get; set; } = null!;
        public virtual DbSet<WorkflowActivity> WorkflowActivity { get; set; } = null!;
        public virtual DbSet<ModuleTrigger> ModuleTrigger { get; set; } = null!;
        public virtual DbSet<WorkflowSubModule> SubModuleTrigger { get; set; } = null!;
        public virtual DbSet<ModuleActivity> ModuleActivity { get; set; } = null!;
        public virtual DbSet<ModuleCondition> ModuleCondition { get; set; } = null!;
        public virtual DbSet<Parameter> Parameter { get; set; } = null!;
        public virtual DbSet<WorkflowActivityParameters> WorkflowActivityParameters { get; set; } = null!;
        public virtual DbSet<SLAActionParameters> SLAActionParameters { get; set; } = null!;
        public virtual DbSet<WorkflowCondition> WorkflowCondition { get; set; } = null!;
        public virtual DbSet<WorkflowTriggerCondition> WorkflowTriggerCondition { get; set; } = null!;
        public virtual DbSet<WorkflowTriggerSectorOptions> WorkflowTriggerSectorOptions { get; set; } = null!;
        public virtual DbSet<WorkflowTriggerSectorTransferOptions> WorkflowTriggerSectorTransferOptions { get; set; } = null!;
        public virtual DbSet<SLA> SLA { get; set; } = null!;
        public virtual DbSet<ModuleConditionOptions> ConditionOptions { get; set; } = null!;
        public virtual DbSet<WorkflowConditionOptions> WorkflowConditionOptions { get; set; } = null!;
        public virtual DbSet<WorkflowTriggerConditionOption> WorkflowTriggerConditionOption { get; set; } = null!;
        public virtual DbSet<WorkflowOption> WorkflowActivityOptions { get; set; } = null!;
        public virtual DbSet<WorkflowStatus> WorkflowStatus { get; set; } = null!;
        public virtual DbSet<WorkflowInstance> WorkflowInstance { get; set; } = null!;
        public virtual DbSet<LmsLiteraturePurchase> LmsLiteraturePurchases { get; set; } = null!;
        public virtual DbSet<UploadedDocument> UploadedDocuments { get; set; } = null!;
        public virtual DbSet<TempAttachement> TempAttachements { get; set; } = null!;
        public virtual DbSet<LmsLiteratureBarcode> LmsLiteratureBarcodes { get; set; } = null!;
        public virtual DbSet<LmsLiteratureAuthor> LmsLiteratureAuthors { get; set; } = null!;
        public virtual DbSet<LmsLiteratureDetailsLmsLiteratureAuthor> LmsLiteratureDetailsLmsLiteratureAuthors { get; set; } = null!;
        public virtual DbSet<LmsLiteratureType> LmsLiteratureTypes { get; set; } = null!;
        public virtual DbSet<LiteratureBorrowApprovalType> LiteratureBorrowApprovalTypes { get; set; } = null!;
        public virtual DbSet<Translation> Translations { get; set; } = null!;
        public virtual DbSet<CmsComsReminder> CmsComsReminders { get; set; } = null!;
        public virtual DbSet<CmsComsReminderHistory> CmsComsReminderHistory { get; set; } = null!;
        public virtual DbSet<TokenModel> TokenModels { get; set; } = null!;


        public virtual DbSet<TransferUser> TransferUsers { get; set; } = null!;
        public virtual DbSet<Priority> CasePriority { get; set; } = null!;
        public virtual DbSet<ResponseType> ResponseTypes { get; set; } = null!;
        public virtual DbSet<Frequency> Frequencies { get; set; } = null!;

        public virtual DbSet<CmsCaseRequestHistory> CmsCaseRequestHistories { get; set; } = null!;
        public virtual DbSet<CmsTransferHistory> CmsTransferHistories { get; set; } = null!;
        public virtual DbSet<CmsCopyHistory> CmsCopyHistories { get; set; } = null!;
        public virtual DbSet<ComsConsultationRequestHistory> ComsConsultationRequestHistories { get; set; } = null!;
        public virtual DbSet<CmsCaseFileStatusHistory> CmsCaseFileStatusHistory { get; set; } = null!;
        public virtual DbSet<ConsultationFileHistory> ConsultationFileHistory { get; set; } = null!;
        public virtual DbSet<CmsRegisteredCaseStatusHistory> CmsRegisteredCaseStatusHistory { get; set; } = null!;
        public virtual DbSet<CMSRegisteredCaseTransferHistory> CmsRegisteredCaseTransferHistory { get; set; } = null!;
        public virtual DbSet<Subtype> Subtypes { get; set; } = null!;
        public virtual DbSet<LookupsHistory> LookupsHistories { get; set; } = null!;
        public virtual DbSet<StopExecutionRejectionReason> StopExecutionRejectionReason { get; set; } = null!;

        //Communications Models
        public virtual DbSet<CommunicationAttendee> CommunicationAttendees { get; set; } = null!;
        public virtual DbSet<CommunicationMeeting> CommunicationMeetings { get; set; } = null!;
        public virtual DbSet<CommunicationTargetLink> CommunicationTargetLinks { get; set; } = null!;

        public virtual DbSet<LinkTarget> LinkTargets { get; set; } = null!;
        public virtual DbSet<Communication> Communications { get; set; } = null!;
        public virtual DbSet<CommunicationResponse> CommunicationResponses { get; set; } = null!;
        public virtual DbSet<CommunicationResponseGovtEntity> CommunicationResponseGovtEntities { get; set; } = null!;
        public virtual DbSet<CommunicationType> CommunicationTypes { get; set; } = null!;
        public virtual DbSet<CommunicationTarasolRpaPayload> CommunicationTarasolRpaPayloads { get; set; } = null!;
        public virtual DbSet<CommunicationHistory> CommunicationHistories { get; set; } = null!;
        public virtual DbSet<CommunicationRecipient> CommunicationRecipients { get; set; } = null!;

        //end Communication Db 
        //Contact start
        public virtual DbSet<CntContact> Contacts { get; set; } = null!;
        public virtual DbSet<CntContactJobRole> ContactJobRoles { get; set; } = null!;
        public virtual DbSet<CntContactType> ContactTypes { get; set; } = null!;
        public virtual DbSet<LegalPublicationSourceName> LegalPublicationSourceNames { get; set; } = null!;
        public virtual DbSet<CasePartyType> CasePartyTypes { get; set; } = null!;
        public virtual DbSet<CmsOutcomePartyHistory> CmsOutcomePartyHistories { get; set; } = null!;
        public virtual DbSet<ChamberChamberNumber> ChamberChamberNumbers { get; set; } = null;
        public virtual DbSet<ChamberNumberHearing> ChamberNumberHearings { get; set; } = null;
        public virtual DbSet<CourtChamber> CourtChambers { get; set; } = null;
        public virtual DbSet<ContactType> EmployeeContactTypes { get; set; } = null!;
        public virtual DbSet<CmsBank> CmsBanks { get; set; } = null!;
        public virtual DbSet<CmsBankGovernmentEntity>  CmsBankGovernmentEntities {get; set;} = null!;
        public virtual DbSet<LLSLegalPrincipleComment> LLSLegalPrincipleComments {get; set;} = null!;

        //contact end
        public virtual DbSet<UmsUserDeviceToken> UmsUserDeviceToken { get; set; } = null!;
        public virtual DbSet<CmsRegisteredCasesAssigneeVM> CmsRegisteredCasesAssigneeVM { get; set; } = null!;
        public virtual DbSet<CmsCaseFileTranferRequest> CmsCaseFileTranferRequest { get; set; } = null!;
        public virtual DbSet<CmsRegisteredCaseTransferRequest> CmsRegisteredCaseTransferRequest { get; set; } = null!;
        #endregion
        #region Mobile Application
        public virtual DbSet<MobileAppVersions> MobileAppVersions { get; set; } = null!;
        #endregion
        #region Notification
        public virtual DbSet<Notification> NotifNotifications { get; set; } = null!;
        public virtual DbSet<NotificationTemplate> NotificationTemplates { get; set; } = null!;
        public virtual DbSet<NotificationEventPlaceholders> NotificationEventPlaceholders { get; set; } = null!;
        public virtual DbSet<NotificationEvent> NotificationEvents { get; set; } = null!;
        public virtual DbSet<NotificationChannel> NotificationChannels { get; set; } = null!;
        public virtual DbSet<NotificationReceiverType> NotificationReceiverTypes { get; set; } = null!;

        #region VM

        [NotMapped]
        public virtual DbSet<NotificationVM> NotifNotificationVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<BellNotificationVM> BellNotificationVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<NotificationDetailVM> NotificationDetailVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<NotificationEventListVM> NotificationEventListVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<NotificationTemplateListVM> NotificationTemplateListVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<DeactivateEmployeesVM> DeactivateEmployeesVM { get; set; } = null!;

        #endregion
        #endregion

        #region Contact Managment VM
        [NotMapped]
        public virtual DbSet<ContactListVM> ContactListVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ContactDetailVM> ContactDetailVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ContactCaseConsultationListVM> ContactCaseConsultationListVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ContactCaseConsultationRequestListVM> ContactCaseConsultationRequestListVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ContactFileLinkVM> ContactForFileVMs { get; set; } = null!;
        #endregion

        #region System Configuration
        public virtual DbSet<SystemConfiguration> SystemConfigurations { get; set; } = null!;
        public virtual DbSet<SystemPasswordPolicy> SystemPasswordPolicys { get; set; } = null!;
        public virtual DbSet<SystemConfigurationSystemPasswordPolicy> SystemConfigurationSystemPasswordPolicys { get; set; } = null!;
        public virtual DbSet<SystemPasswordRule> SystemPasswordRules { get; set; } = null!;
        public virtual DbSet<SystemOption> SystemOptions { get; set; } = null!;
        public virtual DbSet<SystemPasswordDataType> SystemPasswordDataTypes { get; set; } = null!;
        #endregion

        #region System setting
        public virtual DbSet<SystemSetting> SystemSettings { get; set; } = null!;
        #endregion

        #region VM
        [NotMapped]
        public virtual DbSet<BorrowDetailVM> LmsLiteratureBorrowDetailsVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ReturnDetailVM> LmsLiteratureReturnDetailsVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsJudgmentExecutionVM> CmsJudgmentExecutionVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsJudgmentExecutionDetailVM> CmsJudgmentExecutionDetailVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsCaseAssigneesHistoryVM> CmsCaseAssigneesHistoryVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsCaseAssigneeVM> CmsCaseAssigneeVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<UserBorrowLiteratureVM> UserBorrowLiteratureVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LiteratureAllDetailsVM> LiteratureAllDetailsVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LiteratureAllAuthorsVM> LiteratureAllAuthorsVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsCaseRequestHistoryVM> CmsCaseRequestHistoryVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsTransferHistoryVM> CmsTransferHistoryVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ComsConsultationRequestHistoryVM> ComsConsultationRequestHistoryVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CaseTemplateParametersVM> CaseTemplateParametersVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CaseTemplateSectionsVM> CaseTemplateSectionsVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsDraftedDocumentDetailVM> CmsDraftedDocumentDetailVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<UserGroupVM> UserGroupVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CaseRequestDetailVM> CaseRequestDetailVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<WithdrawRequestDetailVM> cmsWithdrawCaseRequestDetailVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CasePartyLinkVM> CasePartyVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<HearingVM> HearingVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsPrintHearingRollDetailVM> CmsPrintHearingRollDetailVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<HearingDetailVM> HearingDetailVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<OutcomeHearingVM> OutcomeHearingVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<OutcomeAndHearingVM> OutcomeAndHearingVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<OutcomeHearingDetailVM> OutcomeHearingDetailVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<JudgementVM> JudgementVMs { get; set; } = null!;
        public virtual DbSet<TransferHistoryVM> TransferHistoryVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsJudgementDetailVM> JudgementDetailVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsCaseRequestVM> CmsCaseRequestVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsCaseRequestDmsVM> CmsCaseRequestDmsVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsWithDrawCaseRequestVM> CmsWithDrawCaseRequestVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsDraftedDocumentVM> CmsDraftedDocumentVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsDraftTemplateVersionLogVM> CmsDraftTemplateVersionLogVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsDraftedTemplateVersionVM> CmsDraftedTemplateVersionVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsDraftedDocumentReasonVM> CmsDraftedDocumentReasonVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsDraftedDocumentOpioninVM> CmsDraftedDocumentOpioninVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ComsDraftedDocumentReasonVM> ComsDraftedDocumentReasonVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LegislationVM> LegislationVMs { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<CmsCaseFileVM> RegisterdRequestVMs { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<MojUnassignedCaseFileVM> MojUnassignedCaseFileVMs { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<CmsCaseFileDmsVM> CmsCaseFileDmsVMs { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<MojExecutionRequestVM> MojExecutionRequestVMs { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<SchedulingCourtVisitVM> SchedulingCourtVisitVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<RegisteredCaseFileVM> RegisteredCaseFileVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsRegisteredCaseVM> CmsRegisteredCaseVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsRegisteredCaseFileDetailVM> CmsRegisteredCaseFileDetailVMs{ get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsRegisteredCaseDmsVM> CmsRegisteredCaseDmsVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsJugdmentDecisionVM> CmsJugdmentDecisionVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsRequestDocumentsVM> CmsRequestDocumentsVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsCaseRequestResponseVM> CmsCaseRequestResponseVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<DetailSubCaseVM> DetailSubCaseVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsCaseFileDetailVM> CmsCaseFileDetailVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsRegisteredCaseDetailVM> CmsRegisteredCaseDetailVM { get; set; } = null!;
        public virtual DbSet<CaseDetailMOJVM> CaseDetailMOJVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<MergeRequestVM> CmsMergeRequestVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<MojRegistrationRequestVM> MojRegistrationRequestVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<MojDocumentPortfolioRequestVM> MojDocumentPortfolioRequestVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<UserVM> UserVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<UserBasicDetailVM> UserBasicDetailVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<UserListGroupVM> UserListGroupVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<UserGroupListVM> UserGroupListVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<UserTransferVM> UserTransferVMs { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<LiteratureDetailVM> LmsLiteratureDetailsVM { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<LiteratureListMobileAppVM> LiteratureListMobileAppVM { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<LiteratureDetailLiteratureTagVM> LmsLiteratureDetailLiteratureTagVM { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<LegalLibraryVM> LmsLegalLibraryVM { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<LmsViewableLiteratureVM> ViewableLiteratureVMs { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<LegalSectionArticalVM> LegalSectionArticalVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LegalClausesSectionVM> LegalClausesSectionVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LegalArticalSectionVM> LegalArticalSectionVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LegalPublicationSourceVM> LegalPublicationSourceVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LegalLegislationDetailVM> LegalLegislationDetailVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LegalLegislationCommentVM> LegalLegislationCommentVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ConsultationRequestDmsVM> ConsultationRequestDmsVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<WorkflowVM> WorkflowVM { get; set; } = null!;
        public virtual DbSet<WorkflowListVM> WorkflowListVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<WorkflowCountVM> WorkflowCountVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<WorkflowInstanceCountVM> WorkflowInstanceCountVM { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<WorkflowInstanceDocumentVM> WorkflowInstanceDocumentVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<WorkflowConditionsOptionVM> WorkflowConditionsOptionVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<WorkflowActivityOptionVM> WorkflowActivityOptionVMs { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<WorkflowActivityVM> WorkflowActivityVM { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<WorkflowConditionsVM> WorkflowConditionsVM { get; set; } = null!;
        public virtual DbSet<WorkflowOptionsVM> WorkflowOptionsVM { get; set; } = null!;
        public virtual DbSet<WorkflowConditionsOptionsListVM> WorkflowConditionsOptionsListVM { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<WorkflowTriggerConditionsVM> WorkflowTriggerConditionsVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<WorkflowTriggerConditionOptionsVM> WorkflowTriggerConditionOptionsVM { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<WorkflowActivityParametersVM> WorkflowActivityParametersVM { get; set; } = null!;

        //[NotMapped]
        //public virtual DbSet<WorkflowSlaActionParametersVM> WorkflowSlaActionParametersVM { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<ParameterVM> ParameterVM { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<ModuleTriggerVM> ModuleTriggerVM { get; set; } = null!;
        public virtual DbSet<AttachmentTypeListVM> AttachmentTypeVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<TempAttachementVM> TempAttachementVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LegalPrincipleTempAttachmentVM> LegalPrincipleTempAttachmentVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<UploadedDocumentVM> UploadedDocumentVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ClaimVM> ClaimVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<UserVM> UserVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<FatwaAttendeeVM> FatwaAttendeeVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<EmployeeVM> EmployeeVM { get; set; } = null!;
        public virtual DbSet<EmployeesListVM> EmployeesListVM { get; set; } = null!;
        public virtual DbSet<EmployeesListDropdownVM> UserGroupEmployeesListVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<UserDataVM> UserDataVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CommitteeUserDataVM> CommitteeUserDataVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<SectorUsersVM> SectorUsersVM { get; set; } = null!;
        [NotMapped]        
        public virtual DbSet<ManagersListVM> ManagersListVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<UserClaimsVM> UserClaimsVM { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<EmployeeDelegationVM> EmployeeDelegationVM { get; set; } = null!;
        public virtual DbSet<EmployeeDelegationHistoryVM> EmployeeDelegationHistoryVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ErrorLogVM> ErrorLogVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ProcessLogVM> ProcessLogVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<IdentityUserVM> IdentityUserVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LegalLegislationDecisionVM> LegalLegislationDecisionVMs { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<UserDetailViewListVM> UserDetailViewListVMs { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<LawyerVM> LawyerVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ListLawyerSupervisorAssignmentVM> AssignSupervisorLawyersVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ConsultationFileListDmsVM> ConsultationFileListDmsVMs { get; set; } = null!;
        #region Communication VM

        [NotMapped]
        public virtual DbSet<CommunicationVM> CommunicationVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CommunicationSendResponseVM> communicationSendResponseVMs { get; set; } = null!;
        public virtual DbSet<CommunicationDetailVM> communicationDetailVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CommunicationSendMessageVM> communicationSendMessageVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CommunicationResponseVM> CommunicationResponseVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CommunicationMeetingDetailVM> CommunicationMeetingDetailVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CommunicationListCaseRequestVM> CommunicationListCaseRequestVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CommunicationInboxOutboxVM> CommunicationInboxOutboxVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CommunicationListConsultationRequestVM> CommunicationListConsultationRequestVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CommunicationListVM> CommunicationListVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CommunicationListCaseFileVM> CommunicationListCaseFileVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CorrespondenceHistoryVM> CorrespondenceHistoryVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CommunicationTarasolMarkedCorrespondencesVM> CommunicationTarasolMarkedCorrespondencesVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CommunicationConsultationFileVM> CommunicationConsultationFileVMs { get; set; } = null!;
        #endregion
        [NotMapped]
        public virtual DbSet<CmsCaseFileStatusHistoryVM> CmsCaseFileHistoryVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsRegisteredCaseStatusHistoryVM> CmsRegisteredCaseStatusHistoryVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LegalPublicationSourceNameVM> LegalPublicationSourceNameVMs { get; set; } = null!;


        [NotMapped]
        public virtual DbSet<LiteratureDetailsForBorrowRequestVM> LiteratureDetailsForBorrowRequestVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<TimeIntervalVM> TimeIntervalVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<TimeIntervalDetailVM> TimeIntervalDetailVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ComsInternationalArbitrationTypeVM> ComsInternationalArbitrationTypeVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CaseOutcomePartyLinkHistoryVM> CaseOutcomePartyLinkHistoryVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CasePartyLinkExecutionVM> CasePartyLinkExecutionVMs { get; set; } = null!;

        public virtual DbSet<ComsInternationalArbitrationType> ComsInternationalArbitrationTypes { get; set; } = null;
        [NotMapped]
        public virtual DbSet<MOJRollsChamberCourtChamberNumberVM> MOJRollsChamberCourtChamberNumberVMs { get; set; } = null;
        [NotMapped]
        public virtual DbSet<CmsAnnouncementVM> CmsAnnouncementsVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsCaseFileTransferRequestVM> CmsCaseFileTransferRequestVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsCaseFileTransferRequestDetailVM> CmsCaseFileTransferRequestDetailVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsRegisteredCaseTransferRequestVM> CmsRegisteredCaseTransferRequestVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsDraftedRequestListVM> CmsDraftedRequestListVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<UserListMentionVM> UserListMentionVMs { get; set; } = null!;
        #endregion
        #region Translation VM
        [NotMapped]
        public virtual DbSet<TranslationMobileAppVM> TranslationMobileAppVMs { get; set; } = null!;
        #endregion
        #region Dashboard VM

        [NotMapped]
        public virtual DbSet<DashboardVM> DashboardVMs { get; set; } = null!;

        #endregion

        #region Legal Legislations VM
        [NotMapped]
        public virtual DbSet<LegalLegislationsVM> LegalLegislationsVMs { get; set; } = null!;
        public virtual DbSet<LegalLegislationsDmsVM> LegalLegislationsDmsVMs { get; set; } = null!;
        public virtual DbSet<LegallegislationtypesVM> legallegislationtypesVMs { get; set; } = null!;

        #endregion

        #region Legal Principle VM
        public virtual DbSet<LegalPrinciplesDmsVM> LegalPrinciplesDmsVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LegalPrincipleTypeVM> LegalPrincipleTypeVMs { get; set; } = null!;
        public virtual DbSet<LmsLiteratureTagVM> LmsLiteratureTagVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LLSLegalPrinciplesReviewVM> LLSLegalPrinciplesReviewVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<AllLmsUserDetailVM> AllLmsUserDetailVMs { get; set; } = null!;

        #endregion

        #region Govenment Entities VM
        public virtual DbSet<GovernmentEntitiesVM> GovernmentEntitiesVMs { get; set; } = null!;

        #endregion

        #region Court Type  VM
        public virtual DbSet<CourtDetailVM> CourtDetailVMs { get; set; } = null!;
        #endregion

        #region Chamber VM
        [NotMapped]
        public virtual DbSet<MobileAppCourtVM> MobileAppCourtVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<MobileAppChamberVM> MobileAppChamberVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<MobileAppChamberNumberVM> MobileAppChamberNumberVM { get; set; } = null!;
        public virtual DbSet<ChamberDetailVM> ChamberDetailVMs { get; set; } = null!;
        public virtual DbSet<RMSChamberVM> RMSChamberVMs { get; set; } = null!;
        public virtual DbSet<RMSCourtsVM> RMSCourtsVMs { get; set; } = null!;
        public virtual DbSet<FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups.MOJRollsChamberNumberVM> MOJRollsChamberNumberVMs { get; set; } = null!;
        #endregion

        #region Department lookup VM
        public virtual DbSet<DepartmentDetailVM> DepartmentDetailVMs { get; set; } = null!;
        #endregion
        #region Ep Nationality  lookup VM
        public virtual DbSet<EpNationalityVM> EpNationalityVMs { get; set; } = null!;
        #endregion
        #region Ep Grade  lookup VM
        public virtual DbSet<EpGradeVM> EpGradeVMs { get; set; } = null!;
        #endregion
        #region Ep Grade Type  lookup VM
        public virtual DbSet<EpGradeTypeVM> EpGradeTypeVMs { get; set; } = null!;
        #endregion
        #region Ep Gender lookup VM
        public virtual DbSet<EpGenderVM> EpGenderVMs { get; set; } = null!;
        #endregion
        #region Ep Designation lookup VM
        public virtual DbSet<EpDesignationVM> EpDesignationVMs { get; set; } = null!;
        #endregion
        #region TaskType Lookup VM

        public virtual DbSet<TaskTypeVM> TaskTypeVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<RequestTypeVM> RequestTypeVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<SubTypeVM> SubTypeVMs { get; set; } = null!;
        #endregion
        #region Ep Contract Type lookup VM
        public virtual DbSet<EpContractTypeVM> EpContractTypeVMs { get; set; } = null!;
        #endregion

        #region  Book Author lookup VM
        public virtual DbSet<BookAuthorVM> BookAuthorVMs { get; set; } = null!;
        #endregion

        #region Sector Type Enums VM

        public virtual DbSet<SectorTypeVM> SectorTypeVMs { get; set; } = null!;
        #endregion

        #region Communication Type VM
        public virtual DbSet<CommunicationTypeVM> CommunicationTypeVMs { get; set; } = null!;
        #endregion

        #region Case File Status  VM
        public virtual DbSet<CaseFileStatusVM> CaseFileStatusVMs { get; set; } = null!;
        #endregion

        #region Case Request Status  VM
        public virtual DbSet<CaseRequestStatusVM> CaseRequestStatusVMs { get; set; } = null!;
        #endregion

        #region Cms Registered Case Status VM 
        public virtual DbSet<CmsRegisteredCaseStatusVM> CmsRegisteredCaseStatusVMs { get; set; } = null!;
        #endregion

        #region Attachment Type VM 
        public virtual DbSet<AttachmentTypeVM> AttachmentTypeVMs { get; set; } = null!;
        #endregion

        #region Meetings VM

        [NotMapped]
        public virtual DbSet<MeetingVM> MeetingVMs { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<MeetingAttendeeVM> MeetingAttendeeVMs { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<MeetingDecisionVM> MeetingDecisionVMs { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<AttendeeDecisionVM> AttendeeDecisionVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CommunicationAttendeeVM> CommunicationAttendeeVMs { get; set; } = null!;
        #endregion

        #region Governament Entity User
        [NotMapped]
        public virtual DbSet<GovernmentEntitiesUsersVM> GovernmentEntitiesUsersVMs { get; set; } = null!;
        #endregion

        #region Case Management

        public virtual DbSet<CmsRegisteredCase> cmsRegisteredCases { get; set; } = null!;
        public virtual DbSet<CaseResponseReason> CaseResponseReasons { get; set; } = null!;
        public virtual DbSet<CaseResponseType> CaseResponseTypes { get; set; } = null!;
        public virtual DbSet<CaseRequestResponse> CaseRequestResponses { get; set; } = null!;
        public virtual DbSet<OperatingSectorType> OperatingSectorType { get; set; } = null!;
        public virtual DbSet<RequestType> RequestTypes { get; set; } = null!;
        public virtual DbSet<Subtype> Subtype { get; set; } = null!;
        public virtual DbSet<ConsultationLegislationFileType> ConsultationLegislationFileTypes { get; set; } = null!;
        public virtual DbSet<Subtype> subtype { get; set; } = null!;
        public virtual DbSet<CaseRequestStatus> CaseRequestStatus { get; set; } = null!;
        public virtual DbSet<CmsCaseVisitType> CmsCaseVisitType { get; set; } = null!;
        public virtual DbSet<CaseFileStatus> CaseFileStatus { get; set; } = null!;
        public virtual DbSet<CaseTemplate> CaseTemplate { get; set; } = null!;
        public virtual DbSet<CmsDraftStamp> SaveDraftStamp { get; set; } = null!;
        public virtual DbSet<CaseTemplateParameter> CaseTemplateParameters { get; set; } = null!;
        public virtual DbSet<CaseTemplateSection> CaseTemplateSections { get; set; } = null!;
        public virtual DbSet<CaseTemplateSectionParameter> CaseTemplateSectionParameters { get; set; } = null!;
        public virtual DbSet<CmsDraftedTemplate> CmsDraftedTemplate { get; set; } = null!;
        public virtual DbSet<CmsDraftedTemplateVersionLogs> CmsDraftedTemplateVersionLogs { get; set; } = null!;
        public virtual DbSet<CmsDraftedTemplateVersions> CmsDraftedTemplateVersions { get; set; } = null!;
        public virtual DbSet<CmsDraftedTemplateVersionVicehos> CmsDraftedTemplateVersionVicehos { get; set; } = null!;
        public virtual DbSet<CmsDraftedTemplateSection> CmsDraftedTemplateSection { get; set; } = null!;
        public virtual DbSet<CmsDraftedTemplateSectionParameter> CmsDraftedTemplateSectionParameter { get; set; } = null!;
        public virtual DbSet<MojRequestForDocument> MojRequestForDocument { get; set; } = null!;
        public virtual DbSet<CmsDocumentPortfolio> CmsDocumentPortfolio { get; set; } = null!;

        public virtual DbSet<SchedulingCourtVisits> SchedulingCourtVisits { get; set; } = null!;
        public virtual DbSet<CmsJudgmentExecution> CmsJudgmentExecutions { get; set; } = null!;
        public virtual DbSet<ExecutionPartyLink> ExecutionPartyLinks { get; set; } = null!;
        public virtual DbSet<CmsMojRpaPayload> CmsMojRpaPayloads { get; set; } = null!;
        public virtual DbSet<CmsHearingRollOutcomeDraftPayload> CmsHearingRollOutcomeDraftPayloads { get; set; } = null!;
        public virtual DbSet<ExecutionAnouncement> ExecutionAnouncements { get; set; } = null!;
        public virtual DbSet<CaseAnnouncement> CaseAnnouncements { get; set; } = null!;
        public virtual DbSet<MojExecutionRequest> MojExecutionRequest { get; set; } = null!;
        public virtual DbSet<MojExecutionRequestAssignee> MojExecutionRequestAssignees { get; set; } = null!;
        public virtual DbSet<CmsDraftedTemplateReason> CmsDraftedTemplateReasons { get; set; } = null!;
        public virtual DbSet<DraftExpertOpinion> DraftExpertOpinions { get; set; } = null!;
        public virtual DbSet<CmsCaseDecision> CmsCaseDecisions { get; set; } = null!;
        public virtual DbSet<CmsCaseDecisionAssignee> CmsCaseDecisionAssignees { get; set; } = null;
        public virtual DbSet<CaseUserImportant> CaseUserImportants { get; set; } = null;

        #endregion

        #region Legal Principle
        public virtual DbSet<LegalPrincipleType> legalPrincipleTypes { get; set; } = null!;
        public virtual DbSet<LegalPrincipleFlowStatus> LegalPrincipleFlowStatuses { get; set; } = null!;

        #endregion

        #region LLSLegal Principle VM
        [NotMapped]
        public virtual DbSet<LLSLegalPrinciplesVM> LLSLegalPrinciplesVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LLSLegalPrincipleDetailVM> LLSLegalPrincipleDetailVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LLSLegalPrinciplReferenceVM> LLSLegalPrinciplReferenceVM { get; set; } = null!;  
        [NotMapped]
        public virtual DbSet<LLSLegalPrincipleCategoriesVM> LLSLegalPrincipleCategoriesVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LLSLegalPrincipleContentCategoriesVM> LLSLegalPrincipleContentCategoriesVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<MobileAppLegalPrincipleContentDetailVM> MobileAppLegalPrincipleContentDetailVM { get; set; } = null!;

        #endregion

        #region Legal Legislation
        public virtual DbSet<LegalLegislation> legalLegislations { get; set; } = null!;
        public virtual DbSet<LegalLegislationLegalTag> legalLegislationLegalTags { get; set; } = null!;
        public virtual DbSet<LegalLegislationSignature> legalLegislationSignatures { get; set; } = null!;
        public virtual DbSet<LegalPublicationSourceName> legalPublicationSourceNames { get; set; } = null!;
        public virtual DbSet<LegalPublicationSource> legalPublicationSources { get; set; } = null!;
        public virtual DbSet<LegalArticle> legalArticles { get; set; } = null!;
        public virtual DbSet<LegalClause> legalClauses { get; set; } = null!;
        public virtual DbSet<LegalArticleChild> legalArticleChilds { get; set; } = null!;
        public virtual DbSet<LegalSection> legalSections { get; set; } = null!;
        public virtual DbSet<LegalExplanatoryNote> legalExplanatoryNotes { get; set; } = null!;
        public virtual DbSet<LegalNote> legalNotes { get; set; } = null!;
        public virtual DbSet<LegalLegislationStatus> legalLegislationStatuss { get; set; } = null!;
        public virtual DbSet<LegalLegislationFlowStatus> legalLegislationFlowStatuss { get; set; } = null!;
        public virtual DbSet<LegalLegislationType> legalLegislationTypes { get; set; } = null!;
        public virtual DbSet<LegalArticleStatus> legalArticleStatuss { get; set; } = null!;
        public virtual DbSet<LegalArticleSource> legalArticleSources { get; set; } = null!;
        public virtual DbSet<LegalLegislationTag> legalLegislationTags { get; set; } = null!;
        public virtual DbSet<LegalLegislationReference> legalLegislationReferences { get; set; } = null!;
        public virtual DbSet<LegalLegislationVM> legalLegislationVMs { get; set; } = null!;
        public virtual DbSet<LegalTemplate> legalTemplates { get; set; } = null!;
        public virtual DbSet<LegalTemplateSetting> legalTemplateSettings { get; set; } = null!;
        public virtual DbSet<LegalLegislationLegalTemplate> legalLegislationLegalTemplates { get; set; } = null!;
        public virtual DbSet<LegalLegislationArticleEffectHistory> legalLegislationArticleEffectHistorys { get; set; } = null!;

        #endregion

        #region Meetings

        public virtual DbSet<Meeting> Meetings { get; set; } = null!;
        public virtual DbSet<MeetingAttendee> MeetingAttendees { get; set; } = null!;
        public virtual DbSet<MeetingMom> MeetingMoms { get; set; } = null!;
        public virtual DbSet<MeetingType> MeetingTypes { get; set; } = null!;
        public virtual DbSet<MeetingStatus> MeetingStatuses { get; set; } = null!;
        public virtual DbSet<MeetingAttendeeStatus> MeetingAttendeeStatuses { get; set; } = null!;
        public virtual DbSet<MomAttendeeDecision> MomAttendeeDecisions { get; set; } = null!;
        public virtual DbSet<GEDepartments> GeDepartments { get; set; } = null!;
        #endregion

        #region Reject Reason

        public virtual DbSet<RejectReason> RejectReasons { get; set; } = null!;
        #endregion

        #region Task

        public virtual DbSet<UserTask> Tasks { get; set; } = null!;
        public virtual DbSet<UserTodoList> UserTodoLists { get; set; } = null!;
        //public virtual DbSet<DraftTask> DraftTasks { get; set; } = null!;
        public virtual DbSet<TaskAction> TaskActions { get; set; } = null!;
        public virtual DbSet<TaskResponse> TaskResponses { get; set; } = null!;
        public virtual DbSet<UserTaskView> UserTaskViews { get; set; } = null!;

        #region Task Drop Downs
        public virtual DbSet<TaskType> TaskTypes { get; set; } = null!;
        public virtual DbSet<TaskSubType> TaskSubTypes { get; set; } = null!;
        public virtual DbSet<StatementType> StatementTypes { get; set; } = null!;
        public virtual DbSet<UserTaskStatus> TaskStatuses { get; set; } = null!;
        public virtual DbSet<TaskResponseStatus> TaskResponseStatuses { get; set; } = null!;
        #endregion

        #endregion

        #region Task VM

        [NotMapped]
        public virtual DbSet<TaskListMobileAppVM> TaskListMobileAppVM { get; set; } = null!; 

        [NotMapped]
        public virtual DbSet<TaskVM> TaskVMs { get; set; } = null!; 
        [NotMapped]
        public virtual DbSet<TaskCountVM> TaskCountVM { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<TaskDashboardVM> TaskDashboardVMs { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<TaskDetailVM> TaskDetailVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<FatwaOssTaskDetailVM> FatwaOssTaskDetailVMs { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<TaskEntityHistoryVM> TaskEntityHistoryVMs { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<DraftListVM> DraftListVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LookupHistoryVM> LookupHistoryVMs { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<WSReminderToCompleteDraftModificationVMS> WSReminderToCompleteDraftModificationVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<FatwaOssTaskVM> FatwaOssTaskVMs { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<TaskResponseStatusVM> TaskResponseStatusVMs { get; set; } = null!;
        #endregion

        #region Consultation
        public virtual DbSet<ConsultationRequest> ConsultationRequests { get; set; } = null!;
        public virtual DbSet<CmsComsNumPattern> CmsComsNumPatterns { get; set; } = null!;
        public virtual DbSet<CmsComsNumPatternHistory> CmsComsNumPatternHistories { get; set; } = null!;
        public virtual DbSet<CmsComsNumPatternGroups> CmsComsNumPatternGroups { get; set; } = null!;
        public virtual DbSet<CmsComsNumPatternType> CmsComsNumPatternTypes { get; set; } = null!;
        public virtual DbSet<CmsComsReminderType> CmsComsReminderTypes { get; set; } = null!;
        public virtual DbSet<ConsultationArticle> ConsultationArticles { get; set; } = null!;
        public virtual DbSet<ConsultationSection> ConsultationSections { get; set; } = null!;

        public virtual DbSet<ConsultationLegalAdvice> ConsultationLegalAdvices { get; set; } = null!;
        public virtual DbSet<ConsultationInternationalArbitration> ConsultationInternationalArbitrations { get; set; } = null!;
        public virtual DbSet<ConsultationArticleStatus> ConsultationArticleStatuses { get; set; } = null!;
        public virtual DbSet<ComsDraftedTemplate> ComsDraftedTemplate { get; set; } = null!;
        public virtual DbSet<ComsDraftedTemplateSection> ComsDraftedTemplateSection { get; set; } = null!;
        public virtual DbSet<ComsDraftedTemplateSectionParameter> ComsDraftedTemplateSectionParameter { get; set; } = null!;
        public virtual DbSet<ComsWithdrawRequest> ComsWithdrawRequests { get; set; } = null!;
        public virtual DbSet<ConsultationTemplate> ConsultationTemplates { get; set; } = null!;
        public virtual DbSet<ConsultationTemplateSection> ConsultationTemplateSections { get; set; } = null!;
        public virtual DbSet<ConsultationTemplateTemplateSection> ConsultationTemplateTemplateSections { get; set; } = null!;
        public virtual DbSet<ConsultationTemplateSectionHead> ConsultationTemplateSectionHeads { get; set; } = null!;

        public virtual DbSet<ComsDraftedTemplateReason> ComsDraftedTemplateReasons { get; set; } = null!;
        public virtual DbSet<ComsWithdrawRequestReason> ComsWithdrawRequestReasons { get; set; } = null!;
        public virtual DbSet<ConsultationPartyType> ConsultationPartyTypes { get; set; } = null!;

        #endregion

        #region Consultation VMs 
        [NotMapped]
        public virtual DbSet<RequestListVM> RequestListVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ConsultationRequestVM> ConsultationRequestVMs { get; set; } = null!;
       [NotMapped]
        public virtual DbSet<ConsultationDraftedRequestListVM> ConsultationDraftedRequestListVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CmsComsNumPatternVM> CmsComsNumPatternVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ViewConsultationVM> ViewConsultationVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ConsultationLegislationFileTypeVM> ConsultationLegislationFileTypeVMs { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<ConsultationPartyListVM> ConsultationPartyVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ConsultationArticleByConsultationIdListVM> ConsultationArticleByConsultationIdListVMs { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<ConsultationFileListVM> ConsultationFileListVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ConsultationFileDetailVM> ConsultationFileDetailVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ConsultationFileHistoryVM> ConsultationFileHistoryVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ConsultationFileAssignmentVM> ConsultationFileAssignmentVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ConsultationFileAssignmentHistoryVM> ConsultationFileAssignmentHistoryVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<AdvanceSearchConsultationCaseFile> AdvanceSearchConsultationCaseFiles { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ConsultationPartyVM> ConsultationPartyDetailVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ComsWithDrawConsultationRequestVM> ComsWithDrawConsultationRequestVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ComsDraftedDocumentDetailVM> ComsDraftedDocumentDetailVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ComsDraftedDocumentVM> ComsDraftedDocumentVM { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<ComsConsultationRequestResponseVM> ComsConsultationRequestResponseVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<WSCmsDraftTemplateIntervalVM> WSCmsDraftTemplateIntervalVMs { get; set; } = null!;
        [NotMapped]//remove the below later
        public virtual DbSet<WSCMSCaseFileHOSReminderVM> WSCMSCaseFileHOSRemindersVMs { get; set; } = null!;
		[NotMapped]
		public virtual DbSet<WSCMSCaseFileHOSReminderRegionalORAppealORSupremeVM> WSCMSCaseFileHOSRemindersRegionalORAppealORSupremeVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<WSReminderCommunicationResponseVM> WSReminderCommunicationResponseVM { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<CmsComsNumPatternHistoryVM> CmsComsNumPatternHistoryVMs { get; set; } = null!;
        #endregion

        #region Contact
        public virtual DbSet<CntContact> CntContacts { get; set; } = null!;
        public virtual DbSet<CntContactJobRole> CntContactJobRoles { get; set; } = null!;
        public virtual DbSet<CntContactFileLink> CntContactFileLinks { get; set; } = null!;
        public virtual DbSet<CntContactType> CntContactTypes { get; set; } = null!;
        #endregion

        #region Inventory Management
        public virtual DbSet<InvItemCategory> invItemCategory { get; set; } = null!;
        public virtual DbSet<InvItemName> invItemName { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<StoreInchargeVM> StoreInchargeVMs { get; set; } = null!;
        public virtual DbSet<SectorBuilding> SectorBuilding { get; set; } = null;
        public virtual DbSet<SectorFloor> SectorFloor { get; set; } = null!;
        #endregion

        #region Document Management
        [NotMapped]
        public virtual DbSet<DMSDocumentListVM> DMSDocumentListVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<DMSDocumentDetailVM> DMSDocumentDetailVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<DmsAddedDocumentReasonVM> DmsAddedDocumentReasonVMs { get; set; } = null!;
        #endregion

        #region Time Tracking VM
        [NotMapped]
        public virtual DbSet<TimeTrackingVM> TimeTrackingVMs { get; set; }
        #endregion
        #region Organizing Committee Vm
        [NotMapped]
        public virtual DbSet<CommitteeDetailsVm> CommitteeDetailsVms { get; set; }
        #endregion

        #region Stock Taking 
       
        public virtual DbSet<LmsStockTakingStatus> StockTakingStatuses { get; set; } = null!;
        #endregion


        public virtual DbSet<SubModule> SubModules { get; set; } = null!;
        public virtual DbSet<DmsFileTypes> FileTypes { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<DMSTemplateListVM> DMSTemplateListVMs { get; set; } = null!;
        #region Inventory Management VM
        public virtual DbSet<ServiceRequestDetailVM> ServiceRequestDetailVMs { get; set; } = null!;
        public virtual DbSet<ServiceRequestVM> ItemRequestListVMs { get; set; } = null!;
        public virtual DbSet<StoreDetailVM> StoreDetailVMs { get; set; } = null!;
        public virtual DbSet<ServiceRequestItemsDetailVM> RequestDetailVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<RejectReasonVM> RejectReasonVM { get; set; } = null!;
        #endregion
        #region Chamber Number  VM
        public virtual DbSet<ChambersNumberDetailVM> ChambersNumberDetailVMs { get; set; } = null!;
        #endregion
        #region Chamber Number Hearing VM
        public virtual DbSet<ChamberNumberHearingDetailVM> ChamberNumberHearingDetailVMs { get; set; } = null!;
        #endregion
        #region Government Entity Sector  VM
        public virtual DbSet<GovernmentEntitiesSectorsVM> GovernmentEntitiesSectorsVMs { get; set; } = null!;
        #endregion
        #region Government Entity Representative  VM
        public virtual DbSet<GovernmentEntitiesRepresentativeVM> GovernmentEntitiesRepresentativeVMs { get; set; } = null!;
        #endregion

        #region Stock Taking VM 
        public virtual DbSet<LmsStockTakingListVM> StockTakingVM { get; set;} = null!;
        public virtual DbSet<LmsStockTakingDetailVM> LmsStockTakingDetailVM { get; set; } = null!;
        public virtual DbSet<LmsStockTakingBooksReportListVm> LmsStockTakingBooksReportListVM { get; set; }= null!;
        #endregion

        #region Worker Service 
        public virtual DbSet<WSCmsComsReminderProcessLog> CmsComsReminderProcessLogs { get; set; } = null!;
        public virtual DbSet<WSCmsComsReminderErrorLog> CmsComsReminderErrorLogs { get; set; } = null!;
        public virtual DbSet<WSCommCommunicationTypes> WsCommunicationTypes { get; set; } = null!;
        public virtual DbSet<WSExecutionStatus> WSExecutionStatus { get; set; } = null!;
        public virtual DbSet<WSWorkerServices> WSWorkerServices { get; set; } = null!;
        public virtual DbSet<PublicHoliday> PublicHolidays { get; set; } = null!;
        public virtual DbSet<TaskDecisionReminder> TaskDecisionReminders { get; set; } = null!;
        #endregion

        #region Worker Service VMs
        [NotMapped]
        public virtual DbSet<WSDataMigrationVM> WSDataMigrationVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<WSCmsCaseFileForHOSIntervalVM> WSCmsCaseFileForHOSIntervalVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<WSCmsComsIntervalVM> WSCmsComsIntervalVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<WSCmsFinalJudgmentIntervalVM> WSCmsFinalJudgmentIntervalVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<WSCmsMOJMessangerIntervalVM> CmsMOJMessangerIntervalVMs { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<WSReminderToCompleteClaimStatementVM> WSReminderToCompleteClaimStatement { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<WSDefenseLetterReminderServiceVM> WSDefenseLetterReminderServiceVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<WSRequestForAdditionalInfoVM> WSRequestForAdditionalInfoVMs { get; set; } = null!;
        public virtual DbSet<WSRequestForAdditionalInfoReminderVM> WSRequestForAdditionalInfoReminderVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<WSWorkerServiceExecution> WSWorkerServiceExecution { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<WSExecutionDetailVM> WSExecutionDetailVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<WSAdditionalInformationCommunication> WSAdditionalInformationCommunication { get; set; } = null!;

        [NotMapped]
        public virtual DbSet<TimeIntervalHistoryVM> TimeIntervalHistoryVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<PublicHolidaysVM> PublicHolidaysVMs { get; set; } = null!;  
        [NotMapped]
        public virtual DbSet<WSReminderForPendingTaskDecisionVM> WSReminderForPendingTaskDecisionVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ManagerTaskReminderVM> ManagerTaskReminderVMs { get; set; } = null!;

        #endregion

        #region LLS Legal Principle
        public virtual DbSet<LLSLegalPrincipleSystem> LLSLegalPrinciples { get; set; } = null!;
        public virtual DbSet<LLSLegalPrincipleContentSourceDocumentReference> LLSLegalPrincipleContentSourceDocumentReferences { get; set; } = null!;
        public virtual DbSet<LLSLegalPrincipleCategory> LLSLegalPrincipleCategorys { get; set; } = null!;
        public virtual DbSet<LLSLegalPrincipleContentCategory> LLSLegalPrincipleLLSLegalPrincipleCategorys { get; set; } = null!;
        public virtual DbSet<LLSLegalPrinciplesRelationVM> LLSLegalPrinciplesRelationVMs { get; set; } = null!;
        public virtual DbSet<LLSLegalPrincipleDocumentVM> LLSLegalPrincipleDocumentVMs { get; set; } = null!;
        public virtual DbSet<LLSLegalPrincipleContent> LLSLegalPrincipleContents { get; set; } = null!;
        public virtual DbSet<LLSLegalPrinciplesContentVM> LLSLegalPrinciplesContentVMs { get; set; } = null!;


        #endregion

        #region Cms Sector Type Ge Department
        public virtual DbSet<CmsSectorTypeGEDepartment> CmsSectorTypeGEDepartments { get; set; } = null!;
        public virtual DbSet<CmsSectorTypeGEDepartmentVM> CmsSectorTypeGEDepartmentVMs { get; set; } = null!;
        #endregion

        #region Bug Reporting Models/Tables
        //Bug Reporting
        public virtual DbSet<BugApplication> BugApplicatoins { get; set; } = null!;
        public virtual DbSet<BugModule> BugModules { get; set; } = null!;
        public virtual DbSet<BugIssueType> BugIssueTypes { get; set; } = null!;
        public virtual DbSet<BugStatus> BugStatuses { get; set; } = null!;
        public virtual DbSet<BugSeverity> BugSeverities { get; set; } = null!;
        public virtual DbSet<BugTicket> BugTickets { get; set; } = null!;
        public virtual DbSet<BugTicketStatusHistory> BugTicketStatusHistories { get; set; } = null!;
        public virtual DbSet<ReportedBug> ReportedBugs { get; set; } = null!;
        public virtual DbSet<BugCommentFeedBack> BugTicketComments { get; set; } = null!;
        public virtual DbSet<BugUserTypeAssignment> AssigningTypeUsers { get; set; } = null!;
        public virtual DbSet<BugModuleTypeAssignment> AssigningTypeModule { get; set; } = null!;
        public virtual DbSet<TicketAssignmentHistory> TicketAssignmentHistories { get; set; } = null!;
        // Bug Reporting End
        #endregion

        #region Bug Reporting VM
        public virtual DbSet<TicketListVM> TicketListVM { get; set; } = null!;
        public virtual DbSet<TicketDetailVM> TicketDetailVM { get; set; } = null!;
        public virtual DbSet<TicketStatusHistoryVM> BugTicketStatusHistoriesVM { get; set; } = null!;
        public virtual DbSet<BugTicketCommentVM> BugTicketCommentsVM { get; set; } = null!;
        public virtual DbSet<ReportedBugListVM> ReportedBugListVMs { get; set; } = null!;
        public virtual DbSet<ReportedBugDetailVM> ReportedBugDetailVMs { get; set; } = null!;
        public virtual DbSet<CrashReportListVM> CrashReportListVMs { get; set; } = null!;
        public virtual DbSet<BugIssueTypeListVM> BugIssueTypeListVMs  { get; set; } = null!;
        #endregion
        #region Literature Dewey Number Pattern
        public virtual DbSet<LiteratureDeweyNumberPatternType> LiteratureDeweyNumberPatternTypes { get; set; } = null!;
        public virtual DbSet<LiteratureDeweyNumberPattern> LiteratureDeweyNumberPatterns { get; set; } = null!;
        #endregion

        #region Literature Dewey Number Pattern VM 
        public virtual DbSet<LiteratureDeweyNumberPatternVM> LiteratureDeweyNumberPatternVMs { get; set; } = null!;
        #endregion 
        #region Mobile App VMs
        [NotMapped]
        public virtual DbSet<CMSCOMSRequestDetailVM> CMSCOMSRequestDetailVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CMSCOMSFileDetailVM> CMSCOMSFileDetailVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<MOJRegisteredCaseDetailVM> MOJRegisteredCaseDetailVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<MobileAppDraftDocumentDetailVM> MobileAppDraftDocumentDetailVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<MobileAppMeetingDetailVM> MobileAppMeetingDetailVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<WithdrawCMSCOMSRequestDetailVM> WithdrawCMSCOMSRequestDetailVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<CMSCOMSCommunicationDetailVM> CMSCOMSCommunicationDetailVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<MobileAppMergeCaseRequestDetailVM> MobileAppMergeCaseRequestDetailVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<MobileAppExecutionDetailVM> MobileAppExecutionDetailVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<MobileAppHearingDetailVM> MobileAppHearingDetailVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<MobileAppHearingListVM> MobileAppHearingListVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<MobileAppCaseFileTransferRequestVM> MobileAppCaseFileTransferRequestVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<MobileAppCaseTransferRequestVM> MobileAppCaseTransferRequestVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<MobileAppLegalLegislationDetailVM> MobileAppLegalLegislationDetailVM { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<UserDetailsVM> UserDetailsVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<BorrowedLiteratureVM> BorrowedLiteratureVMs { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<UserBorrowedHistoryVM> UserBorrowedHistoryVMs { get; set; } = null!;

        #endregion

        #region WeekdaysSettings
        public virtual DbSet<WeekdaysSetting> WeekdaysSettings { get; set; } = null!;

        #endregion

        #region Get Roles by Sector type id
        [NotMapped]
        public virtual DbSet<SectorRolesVM> SectorRolesVms { get; set; } = null!;
        #endregion

        #region Stock taking Models and Vms
        public virtual DbSet<LmsStockTakingStatus> LmsStockTakingStatuses { get; set; } = null!;
        public virtual DbSet<LmsStockTaking> LmsStockTakings { get; set; } = null!;
        public virtual DbSet<LmsStockTakingReport> LmsStockTakingReports { get; set; } = null!;
        public virtual DbSet<LmsBarcodeStockTakingRemarks> LmsBarcodeStockTakingRemarks { get; set; } = null!;
        public virtual DbSet<LmsStockTakingPerformer> LmsStockTakingPerformers { get; set; } = null!;
        public virtual DbSet<LmsStockTakingHistory> LmsStockTakingHistories { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<LmsStockTakingBooksReportListVm> StockTakingBooksReportListVms { get; set; } = null!;
         [NotMapped]
        public virtual DbSet<LmsStockTakingHistoryVm> GetLmsStockTakingHistoryVms { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<StockTakingPerformerVm> StockTakingPerformerVms { get; set; } = null!;
        #endregion

        #region Get Roles by Sector type id
        [NotMapped]
        public virtual DbSet<ServiceRequestApprovalDetailVm> ServiceRequestApprovalDetailVms { get; set; } = null!;
        [NotMapped]
        public virtual DbSet<ServiceRequestApprovalHistoryVm> ServiceRequestApprovalHistoryVms { get; set; } = null!;

        #endregion

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>().Ignore(u => u.PhoneNumber);
            modelBuilder.Entity<IdentityUser>().Ignore(u => u.PhoneNumberConfirmed);
            modelBuilder.Entity<UserPersonalInformation>()
                .HasOne(e => e.User)
                .WithOne()
                .HasForeignKey<User>(u => u.Id);

            modelBuilder.Entity<UserAdress>()
                .HasOne(t => t.UserPersonalInformation)
                .WithMany(t => t.UserAdresses)
                .HasForeignKey(t => t.UserId)
                .HasPrincipalKey(t => t.UserId);

            modelBuilder.Entity<UserContactInformation>()
                .HasOne(t => t.UserPersonalInformation)
                .WithMany(t => t.UserContacts)
                .HasForeignKey(t => t.UserId)
                .HasPrincipalKey(t => t.UserId);

            modelBuilder.Entity<UserEducationalInformation>()
                .HasOne(u => u.UserPersonalInformation)
                .WithMany(u => u.UserEducationalInformation)
                .HasForeignKey(u => u.UserId)
                .HasPrincipalKey(u => u.UserId);

            modelBuilder.Entity<UserWorkExperience>()
                .HasOne(w => w.UserPersonalInformation)
                .WithMany(w => w.UserWorkExperiences)
                .HasForeignKey(w => w.UserId)
                .HasPrincipalKey(w => w.UserId);

            modelBuilder.Entity<UserTrainingAttended>()
                .HasOne(t => t.UserPersonalInformation)
                .WithMany(t => t.UserTrainingAttendeds)
                .HasForeignKey(t => t.UserId)
                .HasPrincipalKey(t => t.UserId);

            modelBuilder.Entity<UserEmploymentInformation>()
                .HasOne(e => e.User)
                .WithOne()
                .HasForeignKey<User>(u => u.Id);

            modelBuilder.Entity<LmsLiteratureBorrowDetail>()
                    .HasOne(i => i.LmsLiterature)
                    .WithMany(i => i.LmsLiteratureBorrowDetails)
                    .HasForeignKey(i => i.LiteratureId)
                    .HasPrincipalKey(i => i.LiteratureId);

            modelBuilder.Entity<LmsLiterature>()
                .HasOne(i => i.LmsLiteratureType)
                .WithMany(i => i.LmsLiterature)
                .HasForeignKey(i => i.TypeId)
                .HasPrincipalKey(i => i.TypeId);

            modelBuilder.Entity<LmsLiterature>()
                .HasOne(i => i.LmsLiteratureClassification)
                .WithMany(i => i.LmsLiterature)
                .HasForeignKey(i => i.ClassificationId)
                .HasPrincipalKey(i => i.ClassificationId);

            modelBuilder.Entity<LmsLiteraturePurchase>()
                .HasOne(i => i.LmsLiterature)
                .WithMany(i => i.LmsLiteraturePurchases)
                .HasForeignKey(i => i.LiteratureId)
                .HasPrincipalKey(i => i.LiteratureId);
            modelBuilder.Entity<LmsLiterature>()
                .HasOne(i => i.LmsLiteratureIndex)
                .WithMany(i => i.LmsLiterature)
                .HasForeignKey(i => i.IndexId)
                .HasPrincipalKey(i => i.IndexId);

            modelBuilder.Entity<LmsLiteratureBorrowDetail>()
                .HasOne(i => i.LmsLiterature)
                .WithMany(i => i.LmsLiteratureBorrowDetails)
                .HasForeignKey(i => i.LiteratureId)
                .HasPrincipalKey(i => i.LiteratureId);

            modelBuilder.Entity<LmsLiteratureDetailsLmsLiteratureAuthor>().HasKey(table => new
            {
                table.LiteratureId,
                table.AuthorId
            });
            modelBuilder.Entity<UserRole>().HasKey(table => new
            {
                table.UserId,
                table.RoleId
            });
            modelBuilder.Entity<UserGroup>().HasKey(table => new
            {
                table.GroupId,
                table.UserId
            });
            modelBuilder.Entity<LmsLiterature>()
                .Property(p => p.LiteratureId)
                .HasPrecision(10, 0);

            modelBuilder.Entity<LmsLiterature>()
                .Property(p => p.ISBN)
                .HasPrecision(10, 0);

            modelBuilder.Entity<LmsLiterature>()
                .Property(p => p.CopyCount)
                .HasPrecision(10, 0);

            modelBuilder.Entity<LmsLiterature>()
                .Property(p => p.NumberOfPages)
                .HasPrecision(10, 0);

            modelBuilder.Entity<LmsLiterature>()
                .Property(p => p.SeriesNumber)
                .HasPrecision(10, 0);

            modelBuilder.Entity<LmsLiterature>()
                .Property(p => p.TypeId)
                .HasPrecision(10, 0);

            modelBuilder.Entity<LmsLiterature>()
                .Property(p => p.ClassificationId)
                .HasPrecision(10, 0);

            modelBuilder.Entity<LmsLiterature>()
                .Property(p => p.IndexId)
                .HasPrecision(10, 0);

            modelBuilder.Entity<LmsLiteratureType>()
                .Property(p => p.TypeId)
                .HasPrecision(10, 0);

            modelBuilder.Entity<LmsLiteratureClassification>()
                  .Property(p => p.ClassificationId)
                  .HasPrecision(10, 0);

            modelBuilder.Entity<LmsLiteratureIndex>()
                .Property(p => p.IndexId)
                .HasPrecision(10, 0);

            modelBuilder.Entity<LmsLiteratureParentIndex>()
                  .Property(p => p.ParentIndexId)
                  .HasPrecision(10, 0);

            modelBuilder.Entity<LmsLiteratureParentIndex>()
                  .Property(p => p.ParentIndexNumber)
                  .HasPrecision(10, 0);

            modelBuilder.Entity<LmsLiteratureIndexDivisionAisle>()
                  .Property(p => p.DivisionAisleId)
                  .HasPrecision(10, 0);

            modelBuilder.Entity<LmsLiteratureIndexDivisionAisle>()
                 .Property(p => p.DivisionNumber)
                 .HasPrecision(10, 0);

            modelBuilder.Entity<LmsLiteratureIndexDivisionAisle>()
                 .Property(p => p.AisleNumber)
                 .HasPrecision(10, 0);

            modelBuilder.Entity<LmsLiteratureIndex>()
                 .Property(p => p.IndexNumber)
                 .HasPrecision(10, 0);

            modelBuilder.Entity<WorkflowActivityParameters>().HasKey(table => new
            {
                table.WorkflowActivityId,
                table.ParameterId
            });

            modelBuilder.Entity<WorkflowActivityParametersVM>().HasKey(table => new
            {
                table.WorkflowActivityId,
                table.ParameterId
            });

            modelBuilder.Entity<SLAActionParameters>().HasKey(table => new
            {
                table.WorkflowSLAId,
                table.ParameterId
            });
            modelBuilder.Entity<ServiceRequestApprovalDetailVm>().HasKey(table => new
            {
                table.Id
            });
            modelBuilder.Entity<ServiceRequestApprovalHistoryVm>().HasKey(table => new
            {
                table.Id
            });


            modelBuilder.Entity<WSReminderToCompleteClaimStatementVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<UserBorrowedHistoryVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<WSReminderCommunicationResponseVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<WSReminderToCompleteDraftModificationVMS>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<WSRequestForAdditionalInfoVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<WSRequestForAdditionalInfoReminderVM>(builder => { builder.HasNoKey(); });

            modelBuilder.Entity<DashboardVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<BellNotificationVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<AdvanceSearchConsultationCaseFile>(builder => { builder.HasNoKey(); });

            modelBuilder.Entity<CommunicationResponseVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<CommunicationListCaseRequestVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<CommunicationListCaseFileVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<CommunicationListVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<CommunicationVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<CommunicationListConsultationRequestVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<CommunicationConsultationFileVM>(builder => { builder.HasNoKey(); });

            modelBuilder.Entity<ContactListVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ContactDetailVM>(builder => { builder.HasNoKey(); });

            modelBuilder.Entity<MeetingVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<MeetingAttendeeVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<MeetingDecisionVM>(builder => { builder.HasNoKey(); });

            modelBuilder.Entity<TaskListMobileAppVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<TaskVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<FatwaOssTaskVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<TaskDetailVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<FatwaOssTaskDetailVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<TaskDashboardVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<DraftListVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<TaskResponseVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ViewConsultationVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ComsConsultationRequestResponseVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ConsultationPartyListVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ConsultationFileListVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ConsultationFileListDmsVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ConsultationArticleByConsultationIdListVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ConsultationFileDetailVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ConsultationFileHistoryVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ConsultationFileAssignmentVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ConsultationFileAssignmentHistoryVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<LawyerVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<CmsWithDrawCaseRequestVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ComsWithDrawConsultationRequestVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ContactCaseConsultationListVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ContactCaseConsultationRequestListVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ReferenceNumberVM>(builder => { builder.HasNoKey(); });

            modelBuilder.Entity<ServiceRequestDetailVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ServiceRequestVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<StoreDetailVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ServiceRequestItemsDetailVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<DMSDocumentListVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<DMSDocumentDetailVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<DMSTemplateListVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<EmployeeVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<AttendeeDecisionVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<WorkflowCountVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<WorkflowInstanceCountVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<NotificationEventListVM>(builder => { builder.HasNoKey(); });

            modelBuilder.Entity<IdentityUser>().ToTable("UMS_USER", "dbo");
            modelBuilder.Entity<IdentityRole>().ToTable("UMS_ROLE", "dbo");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("UMS_ROLE_CLAIMS", "dbo");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UMS_USER_CLAIMS", "dbo");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UMS_USER_LOGINS", "dbo");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UMS_USER_ROLES", "dbo");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UMS_USER_TOKENS", "dbo");
            modelBuilder.Entity<UserDataVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<CommitteeUserDataVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ManagersListVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<UserClaimsVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<SectorUsersVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<DeactivateEmployeesVM>(builder => { builder.HasNoKey(); });


            modelBuilder.Entity<WSCmsMOJMessangerIntervalVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<WSReminderToCompleteClaimStatementVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<WSAdditionalInformationCommunication>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<WSDefenseLetterReminderServiceVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<TaskEntityHistoryVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<OutcomeAndHearingVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<NotificationTemplateListVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<TranslationMobileAppVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<CMSCOMSRequestDetailVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<CMSCOMSFileDetailVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<MOJRegisteredCaseDetailVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<MobileAppDraftDocumentDetailVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<MobileAppMeetingDetailVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<WithdrawCMSCOMSRequestDetailVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<CMSCOMSCommunicationDetailVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<MobileAppMergeCaseRequestDetailVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<MobileAppExecutionDetailVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<MobileAppHearingDetailVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<MobileAppDMSDocumentDetailVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<MobileAppHearingListVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<UserDetailsVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<BorrowedLiteratureVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<StockTakingPerformerVm>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<SectorRolesVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<WSReminderForPendingTaskDecisionVM>(builder => { builder.HasNoKey(); });



            modelBuilder.Entity<CommunicationDetailVM>(entity =>
            {
                entity.HasNoKey();
                entity.Property(e => e.DueDate).IsRequired(false);
                entity.Property(e => e.InboxDate).IsRequired(false);
                entity.Property(e => e.G2GReferenceDate).IsRequired(false);
                entity.Property(e => e.OutboxDate).IsRequired(false);
                entity.Property(e => e.RequestDate).IsRequired(false);
                entity.Property(e => e.RequestOrFileDate).IsRequired(false);
                entity.Property(e => e.ResponseDate).IsRequired(false);
                entity.Property(e => e.ResponseTypeId).IsRequired(false);
                entity.Property(e => e.ResponseTypeEn).IsRequired(false);
                entity.Property(e => e.ResponseTypeAr).IsRequired(false);
                entity.Property(e => e.Description).IsRequired(false);
                entity.Property(e => e.PriorityNameEN).IsRequired(false);
                entity.Property(e => e.PriorityNameAr).IsRequired(false);
                entity.Property(e => e.FrequencyNameEn).IsRequired(false);
                entity.Property(e => e.FrequencyNameAr).IsRequired(false);
                entity.Property(e => e.AdditionalGEUserEn).IsRequired(false);
                entity.Property(e => e.AdditionalGEUserAr).IsRequired(false);
                entity.Property(e => e.IsUrgent).IsRequired(false);
                entity.Property(e => e.InboxNumber).IsRequired(false);
                entity.Property(e => e.OutboxNumber).IsRequired(false);
                entity.Property(e => e.G2GReferenceNumber).IsRequired(false);
                entity.Property(e => e.Reason).IsRequired(false);
                entity.Property(e => e.Other).IsRequired(false);
            });

            modelBuilder.Entity<CmsDraftedDocumentVM>(entity =>
            {
                entity.HasNoKey();
                entity.Property(e => e.ReviewerRoleId).IsRequired(false);
                entity.Property(e => e.ReviewerUserId).IsRequired(false);

            });
            modelBuilder.Entity<CmsDraftedTemplateVersionVM>(entity =>
            {
                entity.HasNoKey();
                entity.Property(e => e.ReviewerRoleId).IsRequired(false);
                entity.Property(e => e.ReviewerUserId).IsRequired(false);

            });
            modelBuilder.Entity<TimeTrackingVM>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<NotificationParameter>(entity =>
            {
                entity.HasNoKey();
            });         
            modelBuilder.Entity<LLSLegalPrincipleContentCategoriesVM>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<EmployeeDelegationHistoryVM>()
            .HasKey(e => new { e.UserId });

            #region Worker Service VMs HasNoKey
            modelBuilder.Entity<WSDataMigrationVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ManagerTaskReminderVM>(builder => { builder.HasNoKey(); });
            #endregion

            #region BugReporing HasNoKey
            modelBuilder.Entity<TicketStatusHistoryVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ReportedBugListVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<ReportedBugDetailVM>(builder => { builder.HasNoKey(); });
            modelBuilder.Entity<CrashReportListVM>(builder => { builder.HasNoKey(); });
            #endregion
        }

    }
}
