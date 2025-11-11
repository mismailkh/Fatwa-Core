using ExcelDataReader;
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Interfaces.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.Contact;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.LegalPrinciple;
using FATWA_DOMAIN.Models.Lms;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.ServiceRequestModels;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.ContactManagmentVMs;
using FATWA_DOMAIN.Models.ViewModel.InventoryManagementVMs;
using FATWA_DOMAIN.Models.ViewModel.MojRollsVM;
using FATWA_DOMAIN.Models.ViewModel.OrganizingCommitteeVMs;
using FATWA_DOMAIN.Models.ViewModel.ServiceRequestVMs;
using FATWA_DOMAIN.Models.ViewModel.TimeIntervalVMs;
using FATWA_DOMAIN.Models.WorkerService;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using Group = FATWA_DOMAIN.Models.AdminModels.UserManagement.Group;

namespace FATWA_INFRASTRUCTURE.Repository.CommonRepos
{
    //<History Author = 'Hassan Abbas' Date='2022-09-29' Version="1.0" Branch="master"> Repository for sending lookups data to G2G Portal</History>
    public class LookupsRepository : ILookups
    {
        private readonly DatabaseContext _dbContext;
        private readonly DmsDbContext _dmsDbContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public LookupsRepository(DatabaseContext dbContext, DmsDbContext dmsDbContext, IServiceScopeFactory serviceScopeFactory)
        {
            _dbContext = dbContext;
            _dmsDbContext = dmsDbContext;
            _serviceScopeFactory = serviceScopeFactory;
        }
        private List<AssignLawyerToCourtVM> AssignLawyerToCourtVMs;
        private List<LegallegislationtypesVM> _legallegislationtypesVMS;
        private List<LmsLiteratureTagVM> _lmsLiteratureTagVM;
        private List<CmsComsNumPatternGroups> _cmsComsNumPatternGroups;
        private List<Group> _umsUserGroups;
        //private List<StoreInchargeVM> _storeInchargeVMs;
        private GovernmentEntitiesVM _governmentEntitiesVM;
        private List<AttachmentType> _ldsDocumenttypedetail;
        private List<CaseFileStatusVM> _casefilestatus;
        private List<CmsRegisteredCaseStatus> _casestatus;
        private List<CaseRequestStatus> _caserequeststatus;
        private List<LookupHistoryVM> _LookupHistoryVM;
        private List<TimeIntervalDetailVM> _TimeIntervalDetailVM;
        private CmsComsNumPattern _CmsComsNumGroups;
        private List<ChambersNumberDetailVM> _chamberNumberDetailVMs;
        private List<ChamberDetailVM> _chamberDetailVMs;
        private List<MOJRollsChamberCourtChamberNumberVM> _MOJRollsChamberCourtChamberNumberVMs;
        private List<SectorBuilding> buildingNames;
        private List<SectorFloor> floorNames;
        private List<ChamberNumberHearingDetailVM> _chamberNumberHearingDetailVMs;
        //<History Author = 'Hassan Abbas' Date='2022-09-29' Version="1.0" Branch="master"> Get Government Entities</History>
        public async Task<List<GovernmentEntity>> GetGovernmentEntities(string Culture)
        {
            try
            {

                return await _dbContext.GovernmentEntity.Where(u => u.IsActive == true && u.IsDeleted == false).OrderBy(u => Culture == "en-US" ? u.Name_En : u.Name_Ar).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<CmsGovtEntityNumPattern>> GetGovernmentEntity()
        {
            try
            {
                return await _dbContext.CmsGovtEntityNumPattern.OrderBy(u => u.GovtEntityId).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-02-27' Version="1.0" Branch="master"> Get GE Representatives</History>
        public async Task<List<GovernmentEntityRepresentative>> GetGeRepresentatives(int? govtEntityId)
        {
            try
            {
                return await _dbContext.GovernmentEntityRepresentative.Where(r => r.GovtEntityId == govtEntityId && r.IsActive == true && r.IsDeleted == false).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-29' Version="1.0" Branch="master"> Get Operating Sector Types</History>
        public async Task<List<OperatingSectorType>> GetOperatingSectorTypes()
        {
            try
            {
                return await _dbContext.OperatingSectorType.OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        //<History Author = 'Hassan Abbas' Date='2022-12-23' Version="1.0" Branch="master"> Get Request Types</History>
        public async Task<List<RequestType>> GetRequestTypes()
        {
            try
            {
                return await _dbContext.RequestTypes.OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-29' Version="1.0" Branch="master"> Get Case Request Statuses</History>
        public async Task<List<CaseRequestStatus>> GetCaseRequestStatuses()
        {
            try
            {
                return await _dbContext.CaseRequestStatus.OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;

            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-29' Version="1.0" Branch="master"> Get Case Request Statuses</History>
        public async Task<List<CaseFileStatus>> GetCaseFileStatuses()
        {
            try
            {
                return await _dbContext.CaseFileStatus.OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-29' Version="1.0" Branch="master"> Get Case Request Statuses</History>
        public async Task<List<CaseFile>> GetFileNumbers()
        {
            try
            {
                var result = _dbContext.CaseFiles.OrderBy(u => u.StatusId).ToList();
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }

        //<History Author = 'Nadia Gull' Date='2023-01-27' Version="1.0" Branch="master"> Get RequestNumber/FileNumbers</History>
        public async Task<string> GetReferenceNumber(Guid ReferenceId, int SubModulId)
        {
            try
            {
                if (SubModulId == (int)SubModuleEnum.CaseRequest)
                {
                    var caseRequest = await _dbContext.CaseRequests.FindAsync(ReferenceId);
                    if (caseRequest != null)
                    {
                        return caseRequest.RequestNumber.ToString();
                    }
                }
                else if (SubModulId == (int)SubModuleEnum.CaseFile)
                {
                    var caseFile = await _dbContext.CaseFiles.FindAsync(ReferenceId);
                    if (caseFile != null)
                    {
                        return caseFile.FileNumber;
                    }
                }
                else if (SubModulId == (int)SubModuleEnum.RegisteredCase)
                {
                    var registeredCase = await _dbContext.CmsRegisteredCases.FindAsync(ReferenceId);
                    if (registeredCase != null)
                    {
                        return registeredCase.CaseNumber;
                    }
                }
                else if (SubModulId == (int)SubModuleEnum.ConsultationRequest)
                {
                    var consultationRequest = await _dbContext.ConsultationRequests.FindAsync(ReferenceId);
                    if (consultationRequest != null)
                    {
                        return consultationRequest.RequestNumber.ToString();
                    }
                }
                else if (SubModulId == (int)SubModuleEnum.ConsultationFile)
                {
                    var consultationFile = await _dbContext.ConsultationFiles.FindAsync(ReferenceId);
                    if (consultationFile != null)
                    {
                        return consultationFile.FileNumber;
                    }
                }
                else if (SubModulId == (int)SubModuleEnum.OrganizingCommittee)
                {
                    CommitteeDetailsVm committee = new CommitteeDetailsVm();
                    var storeProc = $"exec pOC_GetCommitteeNumber @CommitteeId='{ReferenceId}'";
                    var committees = _dbContext.CommitteeDetailsVms.FromSqlRaw(storeProc).ToList();
                    committee = committees.FirstOrDefault();
                    if (committee != null)
                    {
                        return Convert.ToString(committee.CommitteeNumber);
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<string> GetConsultationReferenceNumber(Guid ReferenceId)
        {
            try
            {
                var consultationRequest = await _dbContext.ConsultationRequests.FindAsync(ReferenceId);
                if (consultationRequest != null)
                {
                    return consultationRequest.RequestNumber.ToString();
                }
                else
                {
                    var consultationFile = await _dbContext.ConsultationFiles.FindAsync(ReferenceId);
                    if (consultationFile != null)
                    {
                        return consultationFile.FileNumber;
                    }

                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<ConsultationFile>> GetConsultationFileNumber()
        {
            try
            {
                var result = _dbContext.ConsultationFiles.OrderBy(u => u.StatusId).ToList();
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }

        //<History Author = 'Zain Ul Islam' Date='2022-12-15' Version="1.0" Branch="master"> Get Request Numbers</History> 
        public async Task<List<CaseRequest>> GetRequestNumber()
        {
            try
            {
                var result = _dbContext.CaseRequests.OrderBy(u => u.RequestNumber).ToList();
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }


        //<History Author = 'Hassan Abbas' Date='2022-09-29' Version="1.0" Branch="master"> Get Case Priorities</History>
        public async Task<List<Priority>> GetCasePriorities()
        {
            try
            {
                return await _dbContext.CasePriority.OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        // Author Hassan Iftikhar
        public async Task<List<ResponseType>> GetResponseTypes()
        {
            try
            {
                return await _dbContext.ResponseTypes.OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<TaskType>> GetTaskType()
        {
            try
            {
                return await _dbContext.TaskTypes.OrderBy(u => u.TypeId).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<TaskSubType>> GetTaskSubType()
        {
            try
            {
                return await _dbContext.TaskSubTypes.OrderBy(u => u.SubTypeId).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<string>> GetFileNumber()
        {
            try
            {
                return await _dbContext.CaseFiles.Select(u => u.FileNumber).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<Frequency>> GetFrequency()
        {
            try
            {
                return await _dbContext.Frequencies.OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-09-29' Version="1.0" Branch="master"> Get Case Templates based on Attachment Type</History>
        //<History Author = 'Hassan Abbas' Date='2023-05-26' Version="1.0" Branch="master"> Modified where clause to not return the Blank Template as the it is not required anymore according to DPS</History>
        public async Task<List<CaseTemplate>> GetCaseTemplates(int attachmentTypeId)
        {
            try
            {
                return await _dbContext.CaseTemplate.Where(t => t.AttachmentTypeId == attachmentTypeId && t.IsActive).OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get Hearing Statuses</History>
        public async Task<List<HearingStatus>> GetCaseHearingStatuses()
        {
            try
            {
                return await _dbContext.HearingStatuses.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get Judgement Types</History>
        public async Task<List<JudgementType>> GetCaseJudgementTypes()
        {
            try
            {
                return await _dbContext.JudgementTypes.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-11-28' Version="1.0" Branch="master"> Get Judgement Statuses</History>
        public async Task<List<JudgementStatus>> GetCaseJudgementStatuses()
        {
            try
            {
                return await _dbContext.JudgementStatuses.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-11-28' Version="1.0" Branch="master"> Get Judgement Cateogries</History>
        public async Task<List<JudgementCategory>> GetCaseJudgementCategories()
        {
            try
            {
                return await _dbContext.JudgementCategories.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-11-28' Version="1.0" Branch="master"> Get Execution File Levels</History>
        public async Task<List<ExecutionFileLevel>> GetExecutionFileLevels()
        {
            try
            {
                return await _dbContext.ExecutionFileLevels.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region get court types
        //<History Author = 'Nabeel ur Rehman' Date='2023-09-1' Version="1.0" Branch="master"> Get court types</History>
        public async Task<List<CourtType>> GetCourtTypes()
        {
            try
            {
                return await _dbContext.CourtTypes.Where(u => u.IsDeleted == false && u.IsActive == true).OrderBy(u => u.Id).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region get courts
        public async Task<List<Court>> GetCourts()
        {
            try
            {
                return await _dbContext.Courts.OrderBy(u => u.Id).Where(u => u.IsActive == true && u.IsDeleted == false).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region get Chamber Numbers
        public async Task<List<ChamberNumber>> GetChamberNumber()
        {
            try
            {
                return await _dbContext.ChamberNumbers.OrderBy(u => u.Id).Where(u => u.IsActive && u.IsDeleted == false).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region get Chamber Numbers
        public async Task<List<HearingDay>> GetHearingDays()
        {
            try
            {
                return await _dbContext.HearingDays.OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region get chambers
        public async Task<List<Chamber>> GetChambers()
        {
            try
            {
                var test = await _dbContext.Chambers.OrderBy(u => u.Id).Where(u => u.IsActive == true && u.IsDeleted == false).ToListAsync();
                return test;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region get Shifts
        public async Task<List<ChamberShift>> GetShift()
        {
            try
            {
                var test = await _dbContext.ChamberShifts.OrderBy(u => u.Id).Where(u => u.IsActive == true && u.IsDeleted == false).ToListAsync();
                return test;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Get Chamber Number

        public async Task<List<ChamberNumber>> GetChamberNumbersbyChamberId(int chamberId)
        {
            try
            {
                var chamberChamberNumbers = await _dbContext.ChamberChamberNumbers.Where(ccn => ccn.ChamberId == chamberId).ToListAsync();
                if (chamberChamberNumbers.Any())
                {
                    var chamberNumberIds = chamberChamberNumbers.Select(ccn => ccn.ChamberNumberId).ToList();
                    if (chamberNumberIds.Any())
                    {
                        var chamberNumbers = await _dbContext.ChamberNumbers.Where(cn => chamberNumberIds.Contains(cn.Id) && cn.IsActive && !cn.IsDeleted).ToListAsync();
                        return chamberNumbers.Any() ? chamberNumbers : new List<ChamberNumber>();
                    }
                }
                return new List<ChamberNumber>();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Save  lawyer Assigment To Court
        public async Task SaveAssignLawyerToCourt(CmsAssignLawyerToCourt cmsAssignLawyerToCourt)
        {
            using (_dbContext)
            {
                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        foreach (var courtId in cmsAssignLawyerToCourt.SelectedCourts)
                        {
                            foreach (var chamberId in cmsAssignLawyerToCourt.SelectedChamber)
                            {
                                if (await _dbContext.CourtChambers.AnyAsync(cc => cc.CourtId == courtId && cc.ChamberId == chamberId)) // To Check the Relation of Court & Chamber
                                {
                                    foreach (var chamberNumberId in cmsAssignLawyerToCourt.SelectedChamberNumbers)
                                    {
                                        if (await _dbContext.ChamberChamberNumbers.AnyAsync(ccn => ccn.ChamberId == chamberId && ccn.ChamberNumberId == chamberNumberId)) // To Check the Relation of Chamber & ChamberNumber
                                        {
                                            foreach (var lawyerId in cmsAssignLawyerToCourt.SelectedUsers)
                                            {
                                                if (!await _dbContext.CmsAssignLawyerToCourts.AnyAsync(x => x.LawyerId == lawyerId && x.ChamberNumberId == chamberNumberId && x.CourtId == courtId && x.ChamberId == chamberId && !x.IsDeleted))
                                                {
                                                    CmsAssignLawyerToCourt lawyerObj = new CmsAssignLawyerToCourt
                                                    {
                                                        LawyerId = lawyerId,
                                                        ChamberNumberId = chamberNumberId,
                                                        CourtId = courtId,
                                                        ChamberId = chamberId,
                                                        CreatedBy = cmsAssignLawyerToCourt.CreatedBy,
                                                        CreatedDate = DateTime.Now
                                                    };
                                                    await _dbContext.CmsAssignLawyerToCourts.AddAsync(lawyerObj);
                                                    await _dbContext.SaveChangesAsync();
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        #endregion

        #region Get lawyer  Assigment to Court       
        //<History Author = 'Ijaz Ahmad' Date='2023-01-19' Version="1.0" Branch="master">Get lawyer  Assigment to Court </History>
        public async Task<List<AssignLawyerToCourtVM>> GetAssignLawyerToCourt(AdvanceSearchVMAssignLawyerToCourt advanceSearchVM)
        {
            try
            {
                if (AssignLawyerToCourtVMs == null)
                {
                    string StoredProc = $"exec pAssigncourtToLawyer @CourtId='{advanceSearchVM.CourtName}',@LawyerId='{advanceSearchVM.LawyerName}',@ChamberId='{advanceSearchVM.ChamberName}',@ChamberNumberId='{advanceSearchVM.ChamberNumber}',@SectorTypeId = '{advanceSearchVM.SectorTypeId}', @PageNumber = '{advanceSearchVM.PageNumber}', @PageSize = '{advanceSearchVM.PageSize}'";
                    AssignLawyerToCourtVMs = await _dbContext.AssignLawyerToCourtVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return AssignLawyerToCourtVMs;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region lookups by zain


        //<History Author = 'Hassan Abbas' Date='2022-09-29' Version="1.0" Branch="master"> Get Request Subtypes</History>
        public async Task<List<Subtype>> GetAllRequestSubtypes()
        {
            try
            {
                return await _dbContext.Subtypes.OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-29' Version="1.0" Branch="master"> Get Sector Subtypes</History>
        public async Task<List<Subtype>> GetRequestSubtypesByRequestId(int requestTypeId)
        {
            try
            {
                return await _dbContext.Subtypes.Where(t => t.RequestTypeId == requestTypeId).OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        //<History Author = 'Hassan Abbas' Date='2022-09-29' Version="1.0" Branch="master"> Get Departments</History>
        public async Task<List<Department>> GetDepartments()
        {
            try
            {
                return await _dbContext.Departments.Where(u => u.IsDeleted == false && u.IsActive == true).OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-29' Version="1.0" Branch="master"> Get Case Request Statuses</History>
        public async Task<List<MeetingType>> GetMeetingTypes()
        {
            try
            {
                return await _dbContext.MeetingTypes.OrderBy(u => u.MeetingTypeId).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Get Roles

        //<History Author = 'Zain Ul Islam' Date='2022-12-13' Version="1.0" Branch="master">Get Roles</History>
        public async Task<List<Role>> GetRoles()
        {
            try
            {
                return await _dbContext.Roles.OrderBy(u => u.Name).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Court Visit Types
        //<History Author = 'Nabeel Ur Rehman' Date='2022-12-13' Version="1.0" Branch="master">Get Court Visit Type</History>
        public async Task<List<CmsCaseVisitType>> GetCourtVisitTypes()
        {
            try
            {
                return await _dbContext.CmsCaseVisitType.OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Users By Sector
        private List<UserVM> _usersList;
        //<History Author = 'Hassan Abbas' Date='2022-12-28' Version="1.0" Branch="master">Get Users List By Sector</History>   
        public async Task<List<UserVM>> GetUsersBySector(int? sectorTypeId)
        {
            try
            {
                if (_usersList == null)
                {
                    string StoredProc = $"exec pUserListBySector @sectorTypeId='{sectorTypeId}'";
                    _usersList = await _dbContext.UserVM.FromSqlRaw(StoredProc).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return _usersList;
        }
        #endregion

        #region Users By Sector For Court Assignment

        //<History Author = 'Hassan Abbas' Date='2024-02-07' Version="1.0" Branch="master">Get Users by Sector For Court Assignment </History>
        public async Task<List<UserVM>> GetUsersBySectorForCourtAssignment(int? sectorTypeId)
        {
            try
            {
                string StoredProc = $"exec pUserListBySectorForCoutAssignment @sectorTypeId='{sectorTypeId}'";
                var result = await _dbContext.UserVM.FromSqlRaw(StoredProc).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Lawyer By Sector 

        //<History Author = 'Zain Ul Islam' Date='2022-01-16' Version="1.0" Branch="master">Get Lawyers List By Sector</History>   
        public async Task<List<LawyerVM>> GetLawyersBySector(int? sectorTypeId)
        {
            string storedProc;
            try
            {
                if (sectorTypeId <= (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases || sectorTypeId == (int)OperatingSectorTypeEnum.Execution || sectorTypeId == (int)OperatingSectorTypeEnum.PublicOperationalSector)
                {
                    storedProc = $"exec pLawyerListBySector @sectorTypeId='{sectorTypeId}'";
                }
                else
                {
                    storedProc = $"exec pConsultationLawyerListBySector @sectorTypeId='{sectorTypeId}'";
                }
                var result = await _dbContext.LawyerVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return new List<LawyerVM>();
                throw;
            }
        }
        //<History Author = 'ijaz Ahmad' Date='2024-06-24' Version="1.0" Branch="master">Get Lawyer Supervisor Assignment List By Sector</History>   
        public async Task<List<ListLawyerSupervisorAssignmentVM>> GetLawyerSupervisorAssignmentListBySector(int? sectorTypeId)
        {
            string storedProc;
            try
            {
                if (sectorTypeId <= (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases || sectorTypeId == (int)OperatingSectorTypeEnum.Execution || sectorTypeId == (int)OperatingSectorTypeEnum.PublicOperationalSector)
                {
                    storedProc = $"exec pCmsLawyerSupervisorAssignmentListBySector @sectorTypeId='{sectorTypeId}'";
                }
                else
                {
                    storedProc = $"exec pComsLawyerSupervisorAssignmentListBySector @sectorTypeId='{sectorTypeId}'";
                }
                var result = await _dbContext.AssignSupervisorLawyersVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return new List<ListLawyerSupervisorAssignmentVM>();
                throw;
            }
        }
        #endregion

        #region Lawyer By Sector And Chamber

        //<History Author = 'Hassan Abbas' Date='2023-12-04' Version="1.0" Branch="master">List of Lawyers based on Sector And Chamber</History>    
        public async Task<List<LawyerVM>> GetLawyersBySectorAndChamber(int? sectorTypeId, string? UserId, int chamberNumberId = 0)
        {
            string storedProc;
            try
            {
                if (sectorTypeId <= (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases || sectorTypeId == (int)OperatingSectorTypeEnum.Execution || sectorTypeId == (int)OperatingSectorTypeEnum.PublicOperationalSector)
                {
                    storedProc = $"exec pCmsLawyerListBySectorAndChamber @sectorTypeId='{sectorTypeId}', @chamberNumberId='{chamberNumberId}', @ManagerId='{UserId}'";
                }
                else
                {
                    storedProc = $"exec pConsultationLawyerListBySector @sectorTypeId='{sectorTypeId}'";
                }
                var result = await _dbContext.LawyerVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return new List<LawyerVM>();
                throw;
            }
        }

        #endregion

        #region Supervisor By Sector 
        //<History Author = 'Hassan Abbas' Date='2023-03-07' Version="1.0" Branch="master">Get Supervisors List By Sector</History>    
        public async Task<List<LawyerVM>> GetSupervisorsBySector(int? sectorTypeId)
        {
            string storedProc;
            try
            {
                if (sectorTypeId <= (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases
                    || sectorTypeId == (int)OperatingSectorTypeEnum.Execution
                    || sectorTypeId == (int)OperatingSectorTypeEnum.PublicOperationalSector)
                {
                    storedProc = $"exec pSupervisorListBySector @sectorTypeId='{sectorTypeId}'";
                }
                else
                {
                    storedProc = $"exec pConsultationSupervisorListBySector @sectorTypeId='{sectorTypeId}'";
                }

                return await _dbContext.LawyerVMs.FromSqlRaw(storedProc).ToListAsync();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }


        #endregion

        #region Get Assinged Lawyer To Court By Id
        public async Task<CmsAssignLawyerToCourt> GetAssignLawyertoCourtById(Guid Id)
        {
            try
            {
                CmsAssignLawyerToCourt assingedlawyertocourt = await _dbContext.CmsAssignLawyerToCourts.FirstOrDefaultAsync(x => x.Id == Id);
                if (assingedlawyertocourt != null)
                {
                    return assingedlawyertocourt;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        #region Edit Assign Lawyer To Court
        //< History Author = 'Danish' Date = '2023-01-04' Version = "1.0" Branch = "master" >Edit Judgment Execution</History>
        public async Task EditAssignLawyertoCourt(CmsAssignLawyerToCourt cmsAssignLawyerToCourt)
        {
            try
            {
                _dbContext.CmsAssignLawyerToCourts.Update(cmsAssignLawyerToCourt);
                await _dbContext.SaveChangesAsync();
            }

            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Soft delete
        public async Task DeleteAssignLawyerToCourt(AssignLawyerToCourtVM assignLawyerToCourtVM)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        CmsAssignLawyerToCourt? cmsAssignLawyerToCourt = await _dbContext.CmsAssignLawyerToCourts.FindAsync(assignLawyerToCourtVM.Id);
                        if (cmsAssignLawyerToCourt != null)
                        {
                            _dbContext.CmsAssignLawyerToCourts.Remove(cmsAssignLawyerToCourt);
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        #endregion

        #region Assign Supervisor And Manager To LAwyers
        //< History Author = 'Hassan Abbas' Date = '2023-03-01' Version = "1.0" Branch = "master" Assign Supervisor To Lawyers</History>
        public async Task AssignSupervisorAndManagerToLawyers(CmsLawyerSupervisorVM cmsAssignLawyerToCourt)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var user in cmsAssignLawyerToCourt.SelectedUsers)
                        {

                            var userDetails = await _dbContext.UserEmploymentInformation
                                .FirstOrDefaultAsync(u => u.UserId == user.Id);

                            if (userDetails != null)
                            {
                                userDetails.SupervisorId = cmsAssignLawyerToCourt.SupervisorId != null ? cmsAssignLawyerToCourt.SupervisorId : userDetails.SupervisorId;
                                userDetails.ManagerId = cmsAssignLawyerToCourt.ManagerId != null ? cmsAssignLawyerToCourt.ManagerId : userDetails.ManagerId;
                                _dbContext.UserEmploymentInformation.Update(userDetails);
                            }
                            await _dbContext.SaveChangesAsync();
                        }
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion

        #region Get Supervisor By LawyerId      
        //<History Author = 'Hassan Abbas' Date='2023-03-01' Version="1.0" Branch="master">Get Supervisor By LaywerId</History>
        public async Task<string> GetSupervisorByLawyerId(string lawyerId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var user = _DbContext.UserEmploymentInformation.Where(l => l.UserId == lawyerId).FirstOrDefault();
                return _DbContext.UserEmploymentInformation.Where(l => l.UserId == lawyerId).FirstOrDefault()?.SupervisorId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Get rejected lawyer Id By email      
        //<History Author = 'Hassan Abbas' Date='2023-03-01' Version="1.0" Branch="master">Get Supervisor By LaywerId</History>
        public async Task<string> GetUserIdByUserEmail(string email)
        {
            try
            {
                var user = _dbContext.Users.Where(l => l.Email == email).FirstOrDefault();
                return user.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Get Consultation Party Types
        public async Task<List<ConsultationPartyType>> GetConsultationPartyTypes()
        {
            try
            {
                return await _dbContext.ConsultationPartyTypes.OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Consultation Request Number
        //<History Author = 'Muhammad Zaeem' Date='2023-03-23' Version="1.0" Branch="master"> Get Consulatation Request Numbers</History>

        public async Task<List<ConsultationRequest>> GetConsultationRequestNumber()
        {
            try
            {
                var result = _dbContext.ConsultationRequests.OrderBy(u => u.RequestNumber).ToList();
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Get job Roles Of Contact
        //<History Author = 'Muhammad Zaeem' Date='2023-03-23' Version="1.0" Branch="master"> Get Consulatation Request Numbers</History>

        public async Task<List<CntContactJobRole>> GetContactJobRole()
        {
            try
            {
                return await _dbContext.ContactJobRoles.OrderBy(u => u.RoleId).ToListAsync();
            }


            catch (Exception)
            {

                throw;
            }

        }

        public async Task<List<CntContactType>> GetContactType()
        {
            try
            {
                return await _dbContext.ContactTypes.OrderBy(u => u.TypeId).ToListAsync();
            }


            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Get Meeting status
        public async Task<List<MeetingStatus>> GetMeetingStatus()
        {
            try
            {
                return await _dbContext.MeetingStatuses.OrderBy(u => u.MeetingStatusId).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Reference Number by SubmoduleID
        public async Task<List<ReferenceNumberVM>> GetReferenceNumberBySubmoduleId(int SubModulId, int SectorId)
        {
            try
            {
                List<ReferenceNumberVM> getList = new List<ReferenceNumberVM>();
                ReferenceNumberVM getObj = new ReferenceNumberVM();
                if (SubModulId == (int)SubModuleEnum.CaseRequest)
                {
                    var caseRequest = await _dbContext.CaseRequests.ToListAsync();


                    if (caseRequest != null)
                    {
                        foreach (var refno in caseRequest)
                        {
                            getObj.ReferenceId = refno.RequestId;
                            getObj.ReferenceNumber = Convert.ToString(refno.RequestNumber);
                            getList.Add(getObj);
                            getObj = new ReferenceNumberVM();
                        }
                    }
                }





                else if (SubModulId == (int)SubModuleEnum.CaseFile)
                {


                    var caseFiles = await _dbContext.CaseFiles
               .Where(file =>
                    file.IsDeleted != true)
               .Join(
                   _dbContext.CaseRequests.Where(request => request.SectorTypeId == SectorId),
                   file => file.RequestId,
                   request => request.RequestId,
                    (file, request) => new
                    {
                        FileId = file.FileId,
                        FileNumber = file.FileNumber,
                        SectorTypeId = request.SectorTypeId
                    }
                ).ToListAsync();


                    if (caseFiles != null)
                    {
                        foreach (var caseFile in caseFiles)
                        {
                            getObj.ReferenceId = caseFile.FileId;
                            getObj.ReferenceNumber = caseFile.FileNumber;
                            getList.Add(getObj);
                            getObj = new ReferenceNumberVM();

                        }
                    }
                }








                //else if (SubModulId == (int)SubModuleEnum.CaseFile)
                //{
                //    var caseFile = await _dbContext.CaseFiles.ToListAsync();
                //    if (caseFile != null)
                //    {
                //        foreach (var file in caseFile)
                //        {
                //            {
                //                getObj.ReferenceId = file.FileId;
                //                getObj.ReferenceNumber = file.FileNumber;
                //                getList.Add(getObj);
                //                getObj = new ReferenceNumberVM();
                //            }

                //        }
                //    }
                //}
                else if (SubModulId == (int)SubModuleEnum.RegisteredCase)
                {
                    var registeredCase = await _dbContext.CmsRegisteredCases.Where(x => x.SectorTypeId == SectorId).ToListAsync();
                    if (registeredCase != null)
                    {
                        foreach (var regcase in registeredCase)
                        {
                            getObj.ReferenceId = regcase.CaseId;
                            getObj.ReferenceNumber = regcase.CaseNumber;
                            getList.Add(getObj);
                            getObj = new ReferenceNumberVM();
                        }

                    }
                }
                else if (SubModulId == (int)SubModuleEnum.ConsultationRequest)
                {
                    var consultationRequest = await _dbContext.ConsultationRequests.ToListAsync();
                    if (consultationRequest != null)
                    {
                        foreach (var conReq in consultationRequest)
                        {
                            getObj.ReferenceId = conReq.ConsultationRequestId;
                            getObj.ReferenceNumber = conReq.RequestNumber;
                            getList.Add(getObj);
                            getObj = new ReferenceNumberVM();

                        }

                    }
                }
                else if (SubModulId == (int)SubModuleEnum.ConsultationFile)
                {


                    var consultationFile = await _dbContext.ConsultationFiles
               .Where(file =>
                   file.StatusId != (int)CaseFileStatusEnum.FileIsClosed && file.IsDeleted != true)
               .Join(
                   _dbContext.ConsultationRequests.Where(request => request.SectorTypeId == SectorId),
                   file => file.RequestId,
                   request => request.ConsultationRequestId,
                    (file, request) => new
                    {
                        FileId = file.FileId,
                        FileNumber = file.FileNumber,
                        SectorTypeId = request.SectorTypeId
                    }
                ).ToListAsync();


                    if (consultationFile != null)
                    {
                        foreach (var conFile in consultationFile)
                        {
                            getObj.ReferenceId = conFile.FileId;
                            getObj.ReferenceNumber = conFile.FileNumber;
                            getList.Add(getObj);
                            getObj = new ReferenceNumberVM();

                        }
                    }
                }
                else if (SubModulId == (int)SubModuleEnum.OrganizingCommittee)
                {
                    var storeProc = $"exec pOC_GetCommitteeNumber @CommitteeId='{Guid.Empty}'";
                    var committees = _dbContext.CommitteeDetailsVms.FromSqlRaw(storeProc).ToList();
                    if (committees != null)
                    {
                        foreach (var com in committees)
                        {
                            getObj.ReferenceId = com.Id;
                            getObj.ReferenceNumber = Convert.ToString(com.CommitteeNumber);
                            getList.Add(getObj);
                            getObj = new ReferenceNumberVM();
                        }
                    }
                }
                return getList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Modules 
        public async Task<List<Module>> GetModules()
        {
            try
            {
                return await _dbContext.Modules.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Events 
        public async Task<List<NotificationEvent>> GetNotificationEvents()
        {
            try
            {
                return await _dbContext.NotificationEvents.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Submodule 
        public async Task<List<SubModule>> GetSubmodule()
        {
            try
            {
                return await _dbContext.SubModules.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Floors 
        public async Task<List<UserFloor>> GetFloors()
        {
            try
            {
                return await _dbContext.UserFloors.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Store Keepers 
        public async Task<List<UserEmploymentInformation>> GetStoreKeepers(string userId, int userTypeId)
        {
            try
            {
                var filteredUsers = await _dbContext.UserEmploymentInformation
                  .Where(user => (user.EmployeeTypeId == userTypeId) && (user.UserId != userId)).ToListAsync();
                return filteredUsers;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        #region Get List Of Store Incharges by sector type id
        public async Task<List<StoreInchargeVM>> GetListofStoreInchargesbySectortypeId(int sectortypeId)
        {
            try
            {
                string StoredProc = $"exec pGetInvEmployeeListbySectorType @sectorTypeId='{sectortypeId}'";
                var _storeInchargeVMs = await _dbContext.StoreInchargeVMs.FromSqlRaw(StoredProc).ToListAsync();
                return _storeInchargeVMs;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion



        #region Get  Users By Role Id
        public async Task<List<StoreInchargeVM>> GetUserbyRoleId(Guid roleId)
        {
            try
            {
                string StoredProc = $"exec pGetUserByRoleId @roleId='{roleId}'";
                var _storeInchargeVMs = await _dbContext.StoreInchargeVMs.FromSqlRaw(StoredProc).ToListAsync();
                return _storeInchargeVMs;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Get User by Department
        public async Task<List<UserVM>> GetUsersByDepartment(int DepartmentId)
        {
            try
            {
                if (_usersList == null)
                {
                    string StoredProc = $"exec pUserListByDepartment @departmentId='{DepartmentId}'";
                    _usersList = await _dbContext.UserVM.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _usersList;

            }
            catch (Exception ex)
            {
                throw;
            }

            //return _usersList;
        }
        #endregion

        #region Get Company List

        public async Task<List<Company>> GetCompanyList()
        {
            try
            {
                return await _dbContext.Companies.OrderBy(u => u.NameEn).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get City List

        public async Task<List<City>> GetCityList()
        {
            try
            {
                return await _dbContext.Cities.OrderBy(u => u.NameEn).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Government Entites
        #region Get Government Entities 

        public async Task<List<GovernmentEntitiesVM>> GetGovernmentEntiteslist()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pGovernmentEntityG2GLKPList";

                var result = await _dbContext.GovernmentEntitiesVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }


        #endregion
        #region Get Government Entities by Id 
        public async Task<GovernmentEntity> GetGovernmentEntitysById(int EntityId)

        {
            try
            {
                var result = await _dbContext.GovernmentEntity.Where(x => x.EntityId == EntityId).FirstOrDefaultAsync();

                return result;


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #region Save Government Entity
        public async Task<GovernmentEntity> SaveGovernmentEntity(GovernmentEntity governmentEntity)
        {
            try
            {
                governmentEntity.EntityId = 0;
                await _dbContext.GovernmentEntity.AddAsync(governmentEntity);
                await _dbContext.SaveChangesAsync();
                var latestEntityId = governmentEntity.EntityId;

                foreach (var bankGovernmentEntity in governmentEntity.CmsBankGovernmentEntities.ToList())
                {
                    CmsBankGovernmentEntity BankGovernmentEntity = new CmsBankGovernmentEntity
                    {
                        BankId = bankGovernmentEntity.BankId,
                        IBAN = bankGovernmentEntity.IBAN,
                        GovtEntityId = latestEntityId,
                        CreatedBy = governmentEntity.CreatedBy,
                        CreatedDate = governmentEntity.CreatedDate,
                    };
                    await _dbContext.CmsBankGovernmentEntities.AddAsync(BankGovernmentEntity);
                }
                await _dbContext.SaveChangesAsync();
                LookupsHistory lookups = new LookupsHistory
                {
                    LookupsId = governmentEntity.EntityId,
                    NameEn = governmentEntity.Name_En,
                    NameAr = governmentEntity.Name_Ar,
                    LookupsTableId = (int)LookupsTablesEnum.CMS_GOVERNMENT_ENTITY_G2G_LKP,
                    CreatedDate = governmentEntity.CreatedDate,
                    CreatedBy = governmentEntity.CreatedBy,
                    StatusId = (int)LookupHistoryEnums.Added,
                };
                await SaveLookupsHistroy(lookups, null, _dbContext);

                CmsGovtEntityNumPattern cmsGovtEntityNumPattern = new CmsGovtEntityNumPattern
                {
                    GovtEntityId = governmentEntity.EntityId,
                };
                await SaveCmsGovtEntityNumPattern(cmsGovtEntityNumPattern, _dbContext);

                return governmentEntity;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        #region Update Government Enitity 
        public async Task<GovernmentEntity> UpdateGovernmentEntity(GovernmentEntity governmentEntity)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.GovernmentEntity.Where(x => x.EntityId == governmentEntity.EntityId).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.EntityId = governmentEntity.EntityId;
                            task.Name_En = governmentEntity.Name_En;
                            task.Name_Ar = governmentEntity.Name_Ar;
                            task.ModifiedBy = governmentEntity.ModifiedBy;
                            task.ModifiedDate = governmentEntity.ModifiedDate;
                            task.IsDeleted = governmentEntity.IsDeleted;
                            task.IsActive = governmentEntity.IsActive;
                            task.IsConfidential = governmentEntity.IsConfidential;
                            task.GECode = governmentEntity.GECode;
                            task.G2GSiteID = governmentEntity.G2GSiteID;
                            task.G2GSiteCode = governmentEntity.G2GSiteCode;
                            await _dbContext.GovernmentEntity.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();

                            // Update related CmsBankGovernmentEntities
                            var existingBankEntities = await _dbContext.CmsBankGovernmentEntities
                                .Where(b => b.GovtEntityId == governmentEntity.EntityId)
                                .ToListAsync();

                            foreach (var bankEntity in existingBankEntities)
                            {
                                _dbContext.CmsBankGovernmentEntities.Remove(bankEntity);
                            }

                            foreach (var bankGovernmentEntity in governmentEntity.CmsBankGovernmentEntities)
                            {
                                var newBankEntity = new CmsBankGovernmentEntity
                                {
                                    BankId = bankGovernmentEntity.BankId,
                                    IBAN = bankGovernmentEntity.IBAN,
                                    GovtEntityId = governmentEntity.EntityId,
                                    CreatedBy = governmentEntity.ModifiedBy,
                                    CreatedDate = (DateTime)governmentEntity.ModifiedDate,
                                };
                                _dbContext.CmsBankGovernmentEntities.Add(newBankEntity);
                            }

                            await _dbContext.SaveChangesAsync();

                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = governmentEntity.EntityId,
                                NameEn = governmentEntity.Name_En,
                                NameAr = governmentEntity.Name_Ar,
                                LookupsTableId = (int)LookupsTablesEnum.CMS_GOVERNMENT_ENTITY_G2G_LKP,
                                ModifiedBy = governmentEntity.ModifiedBy,
                                ModifiedDate = governmentEntity.ModifiedDate,
                                CreatedBy = governmentEntity.ModifiedBy,
                                CreatedDate = (DateTime)governmentEntity.ModifiedDate,
                                IsActive = (bool)governmentEntity.IsActive,
                                StatusId = (int)LookupHistoryEnums.Updated,
                            };

                            await SaveLookupsHistroy(lookups, governmentEntity.ModifiedDate);
                            transation.Commit();
                        }
                        return governmentEntity;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Delete Government Entity 
        public async Task<GovernmentEntitiesVM> DeleteGovernmentEntity(GovernmentEntitiesVM governmentEntity)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.GovernmentEntity.Where(x => x.EntityId == governmentEntity.EntityId).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.DeletedBy = governmentEntity.DeletedBy;
                            task.DeletedDate = governmentEntity.DeletedDate;
                            task.IsDeleted = true;
                            await _dbContext.GovernmentEntity.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return governmentEntity;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Active status of Government Entity 
        public async Task<GovernmentEntitiesVM> ActiveGovernmentEntities(GovernmentEntitiesVM governmentEntities)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.GovernmentEntity.Where(x => x.EntityId == governmentEntities.EntityId).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.EntityId = governmentEntities.EntityId;
                            task.Name_En = governmentEntities.Name_En;
                            task.Name_Ar = governmentEntities.Name_Ar;
                            task.ModifiedBy = governmentEntities.ModifiedBy;
                            task.ModifiedDate = governmentEntities.ModifiedDate;
                            task.IsDeleted = governmentEntities.IsDeleted;
                            task.IsActive = governmentEntities.IsActive;
                            await _dbContext.GovernmentEntity.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = governmentEntities.EntityId,
                                LookupsTableId = (int)LookupsTablesEnum.CMS_GOVERNMENT_ENTITY_G2G_LKP,
                                NameEn = governmentEntities.Name_En,
                                NameAr = governmentEntities.Name_Ar,
                                ModifiedBy = governmentEntities.ModifiedBy,
                                ModifiedDate = governmentEntities.ModifiedDate,
                                CreatedBy = governmentEntities.CreatedBy,
                                CreatedDate = (DateTime)governmentEntities.CreatedDate,
                                IsActive = (bool)governmentEntities.IsActive,
                                StatusId = (int)LookupHistoryEnums.Updated,
                            };
                            await ActiveLookupsHistroy(lookups, governmentEntities.ModifiedDate);
                            transation.Commit();

                        }
                        return governmentEntities;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Sync Government Entities And Departments

        //<History Author = 'Hassan Abbas' Date='2024-06-06' Version="1.0" Branch="master"> Update Government Entity and Departments Site Ids from Excel File for first run</History>
        public async Task UpdateExistingGEsAndDepartmentFromExcel()
        {
            try
            {
                DataTable excelDataGEs = ReadExcelFile(0);
                var ges = await _dbContext.GovernmentEntity.ToListAsync();
                foreach (DataRow excelRow in excelDataGEs.Rows)
                {
                    var matchedGE = await _dbContext.GovernmentEntity.Where(x => x.Name_Ar == excelRow["G2GSiteNameAr"].ToString()).FirstOrDefaultAsync();
                    if (matchedGE != null)
                    {
                        if (int.TryParse(excelRow["G2GSiteCode"].ToString(), out _))
                        {
                            matchedGE.G2GSiteCode = Convert.ToInt32(excelRow["G2GSiteCode"].ToString());
                        }
                        else
                        {

                        }
                        matchedGE.G2GSiteID = Convert.ToInt32(excelRow["G2GSiteID"].ToString());
                        await _dbContext.SaveChangesAsync();
                    }
                }
                DataTable excelDataDepartments = ReadExcelFile(1);
                foreach (DataRow excelRow in excelDataDepartments.Rows)
                {
                    var matchedGE = await _dbContext.GovernmentEntity.Where(x => x.G2GSiteID == Convert.ToInt32(excelRow["G2GSiteID"].ToString())).FirstOrDefaultAsync();
                    if (matchedGE != null)
                    {
                        var matchedDepartment = await _dbContext.GeDepartments.Where(x => x.Name_Ar == excelRow["G2GBrSiteNameAr"].ToString() && x.EntityId == matchedGE.EntityId).FirstOrDefaultAsync();
                        if (matchedDepartment != null)
                        {
                            matchedDepartment.G2GBRSiteID = Convert.ToInt32(excelRow["G2GBrSiteID"].ToString());
                            await _dbContext.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public DataTable ReadExcelFile(int sheet)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = File.Open(Path.Combine(Directory.GetCurrentDirectory() + $"\\GEsList\\GE and Departments.xlsx"), FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

                    return result.Tables[sheet];
                }
            }
        }

        //<History Author = 'Hassan Abbas' Date='2024-06-06' Version="1.0" Branch="master"> Sync Government Entity and Departments and maintain Log</History>
        public async Task SyncGEsAndDepartments(string username, DataSet sitesList, DataSet sitesBranchList)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        await SaveGovernmentEntities(username, sitesList, _dbContext);
                        await SaveGovernmentEntityDepartments(username, sitesBranchList, _dbContext);

                        GovermentEntityAndDepartmentSyncLog logDetail = new GovermentEntityAndDepartmentSyncLog
                        {
                            Message = "Ok",
                            CreatedBy = username,
                            CreatedDate = DateTime.Now
                        };
                        await _dbContext.GovermentEntityAndDepartmentSyncLogs.AddAsync(logDetail);
                        await _dbContext.SaveChangesAsync();
                        transation.Commit();
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw ex;
                    }
                }
            }
        }


        public async Task SaveGovernmentEntities(string username, DataSet sitesList, DatabaseContext dbContext)
        {
            try
            {
                foreach (DataTable table in sitesList.Tables)
                {
                    var gEList = (from rw in table.AsEnumerable()
                                  select new GovernmentEntity()
                                  {
                                      G2GSiteID = Convert.ToInt32(rw["G2GSiteID"]),
                                      Name_En = Convert.ToString(rw["G2GSiteNameAr"]),
                                      Name_Ar = Convert.ToString(rw["G2GSiteNameAr"]),
                                      CreatedBy = username,
                                      CreatedDate = DateTime.Now,
                                      IsActive = true
                                  }).ToList();
                    foreach (var governmentEntity in gEList)
                    {
                        var existingGE = await dbContext.GovernmentEntity.Where(x => x.G2GSiteID == governmentEntity.G2GSiteID).FirstOrDefaultAsync();
                        if (existingGE == null)
                        {
                            await dbContext.GovernmentEntity.AddAsync(governmentEntity);
                            await dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = governmentEntity.EntityId,
                                NameEn = governmentEntity.Name_En,
                                NameAr = governmentEntity.Name_Ar,
                                LookupsTableId = (int)LookupsTablesEnum.CMS_GOVERNMENT_ENTITY_G2G_LKP,
                                CreatedBy = username,
                                CreatedDate = DateTime.Now,
                                IsActive = (bool)governmentEntity.IsActive,
                                StatusId = (int)LookupHistoryEnums.Added,
                            };
                            await SaveLookupsHistroy(lookups, null, dbContext);

                            CmsGovtEntityNumPattern cmsGovtEntityNumPattern = new CmsGovtEntityNumPattern
                            {
                                GovtEntityId = governmentEntity.EntityId,
                            };
                            await SaveCmsGovtEntityNumPattern(cmsGovtEntityNumPattern, dbContext);
                        }
                        else
                        {
                            existingGE.Name_Ar = governmentEntity.Name_Ar;
                            existingGE.Name_En = governmentEntity.Name_En;
                            existingGE.ModifiedBy = username;
                            existingGE.ModifiedDate = DateTime.Now;
                            await dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = existingGE.EntityId,
                                NameEn = existingGE.Name_En,
                                NameAr = existingGE.Name_Ar,
                                LookupsTableId = (int)LookupsTablesEnum.CMS_GOVERNMENT_ENTITY_G2G_LKP,
                                ModifiedBy = username,
                                ModifiedDate = DateTime.Now,
                                CreatedBy = username,
                                CreatedDate = DateTime.Now,
                                IsActive = (bool)existingGE.IsActive,
                                StatusId = (int)LookupHistoryEnums.Updated,
                            };
                            await SaveLookupsHistroy(lookups, DateTime.Now, dbContext);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task SaveGovernmentEntityDepartments(string username, DataSet sitesBranchList, DatabaseContext dbContext)
        {
            try
            {
                foreach (DataTable table in sitesBranchList.Tables)
                {
                    var departmentList = (from rw in table.AsEnumerable()
                                          select new GEDepartments()
                                          {
                                              G2GBRSiteID = Convert.ToInt32(rw["G2GBrSiteID"]),
                                              G2GSiteID = Convert.ToInt32(rw["G2GSiteID"]),
                                              Name_En = Convert.ToString(rw["G2GBrSiteNameAr"]),
                                              Name_Ar = Convert.ToString(rw["G2GBrSiteNameAr"]),
                                              CreatedBy = username,
                                              CreatedDate = DateTime.Now,
                                              IsActive = true
                                          }).ToList();
                    foreach (var department in departmentList)
                    {
                        var existingGE = await dbContext.GovernmentEntity.Where(x => x.G2GSiteID == department.G2GSiteID).FirstOrDefaultAsync();
                        if (existingGE != null)
                        {
                            var existingDepartment = await dbContext.GEDepartments.Where(x => x.G2GBRSiteID == department.G2GBRSiteID && x.EntityId == existingGE.EntityId).FirstOrDefaultAsync();
                            if (existingDepartment == null)
                            {
                                department.EntityId = existingGE.EntityId;
                                await dbContext.GEDepartments.AddAsync(department);
                                await dbContext.SaveChangesAsync();
                                LookupsHistory lookups = new LookupsHistory
                                {
                                    LookupsId = department.Id,
                                    NameEn = department.Name_En,
                                    NameAr = department.Name_Ar,
                                    LookupsTableId = (int)LookupsTablesEnum.CMS_GOVERNMENT_ENTITY_DEPARTMENT_G2G_LKP,
                                    CreatedDate = DateTime.Now,
                                    CreatedBy = username,
                                    StatusId = (int)LookupHistoryEnums.Added,
                                };

                                await SaveLookupsHistroy(lookups, null, dbContext);
                            }
                            else
                            {
                                existingDepartment.Name_Ar = department.Name_Ar;
                                existingDepartment.Name_En = department.Name_En;
                                existingDepartment.ModifiedBy = username;
                                existingDepartment.ModifiedDate = DateTime.Now;
                                await dbContext.SaveChangesAsync();
                                LookupsHistory lookups = new LookupsHistory
                                {
                                    LookupsId = existingDepartment.Id,
                                    NameEn = existingDepartment.Name_En,
                                    NameAr = existingDepartment.Name_Ar,
                                    LookupsTableId = (int)LookupsTablesEnum.CMS_GOVERNMENT_ENTITY_DEPARTMENT_G2G_LKP,
                                    ModifiedBy = username,
                                    ModifiedDate = DateTime.Now,
                                    CreatedBy = username,
                                    CreatedDate = DateTime.Now,
                                    StatusId = (int)LookupHistoryEnums.Updated,
                                };
                                await SaveLookupsHistroy(lookups, DateTime.Now, dbContext);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2024-06-06' Version="1.0" Branch="master"> Get Sync Government Entity and Departments Log</History>
        public async Task<GovermentEntityAndDepartmentSyncLog> GetLatestGEsAndDepartmentsSyncLog()
        {
            try
            {
                var result = await _dbContext.GovermentEntityAndDepartmentSyncLogs.OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
                if (result == null && File.Exists(Path.Combine(Directory.GetCurrentDirectory() + $"\\GEsList\\GE and Departments.xlsx")))
                {
                    await UpdateExistingGEsAndDepartmentFromExcel();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion
        #region Court Types 
        #region Get  Court Types

        public async Task<List<CourtDetailVM>> GetCourtTypeList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pCmsCourtG2GLKPlist";

                var result = await _dbContext.CourtDetailVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }


        #endregion
        #region Get Court Type by Id 
        public async Task<Court> GetCourtTypesById(int Id)
        {
            try
            {
                var result = await _dbContext.Courts.Where(x => x.Id == Id).FirstOrDefaultAsync();

                return result;


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #region Save Court  Type
        public async Task<Court> SaveCourtType(Court courts)
        {
            try
            {
                await _dbContext.Courts.AddAsync(courts);
                await _dbContext.SaveChangesAsync();
                LookupsHistory lookups = new LookupsHistory
                {
                    LookupsId = courts.Id,
                    NameEn = courts.Name_En,
                    NameAr = courts.Name_Ar,
                    LookupsTableId = (int)LookupsTablesEnum.CMS_COURT_G2G_LKP,
                    CreatedDate = courts.CreatedDate,
                    CreatedBy = courts.CreatedBy,
                    StatusId = (int)LookupHistoryEnums.Added,
                };

                await SaveLookupsHistroy(lookups, null);
                return courts;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        #region Update Court Type
        public async Task<Court> UpdateCourtType(Court courts)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.Courts.Where(x => x.Id == courts.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = courts.Id;
                            task.Name_En = courts.Name_En;
                            task.Name_Ar = courts.Name_Ar;
                            task.ModifiedBy = courts.ModifiedBy;
                            task.ModifiedDate = courts.ModifiedDate;
                            task.IsDeleted = courts.IsDeleted;
                            task.IsActive = courts.IsActive;
                            task.TypeId = courts.TypeId;
                            task.Description = courts.Description;
                            task.CourtCode = courts.CourtCode;
                            task.Address = courts.Address;
                            task.District = courts.District;
                            task.Location = courts.Location;
                            await _dbContext.Courts.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = courts.Id,
                                NameEn = courts.Name_En,
                                NameAr = courts.Name_Ar,
                                LookupsTableId = (int)LookupsTablesEnum.CMS_COURT_G2G_LKP,
                                ModifiedBy = courts.ModifiedBy,
                                ModifiedDate = courts.ModifiedDate,
                                CreatedBy = courts.ModifiedBy,
                                CreatedDate = (DateTime)courts.ModifiedDate,
                                IsActive = courts.IsActive,
                                StatusId = (int)LookupHistoryEnums.Updated,


                            };

                            await SaveLookupsHistroy(lookups, courts.ModifiedDate);
                            transation.Commit();
                        }

                        return courts;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Delete Court Type 
        public async Task<CourtDetailVM> DeleteCourtType(CourtDetailVM courts)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.Courts.Where(x => x.Id == courts.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.DeletedBy = courts.DeletedBy;
                            task.DeletedDate = courts.DeletedDate;
                            task.IsDeleted = true;
                            await _dbContext.Courts.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return courts;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Active status of Court Type 
        public async Task<CourtDetailVM> ActiveCourtTypes(CourtDetailVM courts)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.Courts.Where(x => x.Id == courts.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = courts.Id;
                            task.Name_En = courts.Name_En;
                            task.Name_Ar = courts.Name_Ar;
                            task.ModifiedBy = courts.ModifiedBy;
                            task.ModifiedDate = courts.ModifiedDate;
                            task.IsDeleted = (bool)courts.IsDeleted;
                            task.IsActive = courts.IsActive;
                            await _dbContext.Courts.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = courts.Id,
                                LookupsTableId = (int)LookupsTablesEnum.CMS_COURT_G2G_LKP,
                                NameAr = courts.Name_Ar,
                                NameEn = courts.Name_En,
                                ModifiedBy = courts.ModifiedBy,
                                ModifiedDate = courts.ModifiedDate,
                                CreatedBy = courts.CreatedBy,
                                CreatedDate = (DateTime)courts.CreatedDate,
                                StatusId = (int)LookupHistoryEnums.Updated,
                                IsActive = courts.IsActive,
                            };
                            await ActiveLookupsHistroy(lookups, courts.ModifiedDate);
                            transation.Commit();
                        }
                        return courts;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #endregion
        #region Chamber Lookup 
        #region Get  Chamber List 

        public async Task<List<ChamberDetailVM>> GetChamberList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pCmsChamberG2GLkplist";

                var result = await _dbContext.ChamberDetailVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion
        #region Get Chamber by Id 
        public async Task<Chamber> GetChamberById(int Id)
        {
            try
            {
                var result = await (from c in _dbContext.Chambers
                                    join cc in _dbContext.CourtChambers on c.Id equals cc.ChamberId into courtChambersGroup
                                    from cc in courtChambersGroup.DefaultIfEmpty()
                                    where c.Id == Id
                                    select new
                                    {
                                        Chamber = c,
                                        CourtChamber = cc
                                    })
                    .ToListAsync();

                var chamberWithCourtChambers = result.GroupBy(
                    x => x.Chamber,
                    x => x.CourtChamber,
                    (chamber, courtChambers) => new Chamber
                    {
                        Id = chamber.Id,
                        Number = chamber.Number,
                        Name_En = chamber.Name_En,
                        Name_Ar = chamber.Name_Ar,
                        Address = chamber.Address,
                        IsActive = chamber.IsActive,
                        ChamberCode = chamber.ChamberCode,
                        Description = chamber.Description,
                        SelectedCourtIds = courtChambers
                                            .Where(cc => cc != null)
                                            .Select(cc => cc.CourtId)
                                            .ToList()
                    })
                    .FirstOrDefault();

                var finalResult = chamberWithCourtChambers;

                return finalResult;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Save Chamber
        public async Task<Chamber> SaveChamber(Chamber chamber)
        {
            try
            {
                await _dbContext.Chambers.AddAsync(chamber);
                await _dbContext.SaveChangesAsync();
                foreach (var courtId in chamber.SelectedCourtIds)
                {
                    CourtChamber courtChamber = new CourtChamber
                    {
                        CourtId = courtId,
                        ChamberId = chamber.Id,
                        CreatedBy = chamber.CreatedBy,
                        CreatedDate = chamber.CreatedDate,
                    };
                    await _dbContext.CourtChambers.AddAsync(courtChamber);
                }
                await _dbContext.SaveChangesAsync();
                LookupsHistory lookups = new LookupsHistory
                {
                    LookupsId = chamber.Id,
                    NameEn = chamber.Name_En,
                    NameAr = chamber.Name_Ar,
                    LookupsTableId = (int)LookupsTablesEnum.CMS_CHAMBER_G2G_LKP,
                    CreatedDate = chamber.CreatedDate,
                    CreatedBy = chamber.CreatedBy,
                    IsActive = chamber.IsActive,
                    StatusId = (int)LookupHistoryEnums.Added,
                };

                await SaveLookupsHistroy(lookups, null);
                return chamber;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        #region Update Chamber
        public async Task<Chamber> UpdateChamber(Chamber chamber)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var existingChamber = await dbContext.Chambers.FirstOrDefaultAsync(x => x.Id == chamber.Id);
                        if (existingChamber != null)
                        {
                            existingChamber.Name_En = chamber.Name_En;
                            existingChamber.Name_Ar = chamber.Name_Ar;
                            existingChamber.ModifiedBy = chamber.ModifiedBy;
                            existingChamber.ModifiedDate = chamber.ModifiedDate;
                            existingChamber.IsDeleted = chamber.IsDeleted;
                            existingChamber.IsActive = chamber.IsActive;
                            existingChamber.Description = chamber.Description;
                            existingChamber.Number = chamber.Number;
                            existingChamber.Address = chamber.Address;
                            existingChamber.ChamberCode = chamber.ChamberCode;
                            // Update court-chamber relationships based on selectedCourtIds
                            var selectedCourtIds = chamber.SelectedCourtIds ?? Enumerable.Empty<int>();
                            var existingCourtChamberIds = await dbContext.CourtChambers
                                .Where(cc => cc.ChamberId == chamber.Id)
                                .Select(cc => cc.CourtId)
                                .ToListAsync();
                            var courtChambersToRemove = existingCourtChamberIds.Except(selectedCourtIds).ToList();
                            var courtChambersToAdd = selectedCourtIds.Except(existingCourtChamberIds).ToList();
                            // Remove court-chamber relationships that are no longer selected
                            foreach (var courtIdToRemove in courtChambersToRemove)
                            {
                                var courtChamberToRemove = await dbContext.CourtChambers
                                    .FirstOrDefaultAsync(cc => cc.ChamberId == chamber.Id && cc.CourtId == courtIdToRemove);

                                if (courtChamberToRemove != null)
                                {
                                    dbContext.CourtChambers.Remove(courtChamberToRemove);
                                }
                            }
                            // Add new court-chamber relationships
                            foreach (var courtIdToAdd in courtChambersToAdd)
                            {
                                var courtChamberToAdd = new CourtChamber
                                {
                                    CourtId = courtIdToAdd,
                                    ChamberId = chamber.Id,
                                    CreatedDate = DateTime.Now,
                                    CreatedBy = chamber.ModifiedBy
                                };
                                dbContext.CourtChambers.Add(courtChamberToAdd);
                            }
                            await dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = chamber.Id,
                                NameEn = chamber.Name_En,
                                NameAr = chamber.Name_Ar,
                                LookupsTableId = (int)LookupsTablesEnum.CMS_CHAMBER_G2G_LKP,
                                ModifiedBy = chamber.ModifiedBy,
                                ModifiedDate = chamber.ModifiedDate,
                                CreatedBy = chamber.ModifiedBy,
                                CreatedDate = (DateTime)chamber.ModifiedDate,
                                IsActive = chamber.IsActive,
                                StatusId = (int)LookupHistoryEnums.Updated,
                            };

                            await SaveLookupsHistroy(lookups, chamber.ModifiedDate);
                            transation.Commit();
                        }
                        return chamber;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Delete Chamber
        public async Task<ChamberDetailVM> DeleteChamber(ChamberDetailVM chamber)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.Chambers.Where(x => x.Id == chamber.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.DeletedBy = chamber.DeletedBy;
                            task.DeletedDate = chamber.DeletedDate;
                            task.IsDeleted = true;
                            await _dbContext.Chambers.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return chamber;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Active status of Chamber
        public async Task<ChamberDetailVM> ActiveChamber(ChamberDetailVM chamber)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.Chambers.Where(x => x.Id == chamber.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = chamber.Id;
                            //task.Name_En = chamber.Name_En;
                            //task.Name_Ar = chamber.Name_Ar;
                            //task.ModifiedBy = chamber.ModifiedBy;
                            //task.ModifiedDate = chamber.ModifiedDate;
                            //task.IsDeleted = (bool)chamber.IsDeleted;
                            task.IsActive = chamber.IsActive;

                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();

                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = chamber.Id,
                                LookupsTableId = (int)LookupsTablesEnum.CMS_CHAMBER_G2G_LKP,
                                NameEn = chamber.Name_En,
                                NameAr = chamber.Name_Ar,
                                ModifiedBy = chamber.ModifiedBy,
                                ModifiedDate = chamber.ModifiedDate,
                                CreatedBy = chamber.CreatedBy,
                                CreatedDate = (DateTime)chamber.CreatedDate,
                                IsActive = chamber.IsActive,
                                StatusId = (int)LookupHistoryEnums.Updated,
                            };
                            await ActiveLookupsHistroy(lookups, chamber.ModifiedDate);
                            transation.Commit();
                        }
                        return chamber;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Get Chamber View Detail by ID 
        public async Task<List<ChamberDetailVM>> GetChamberDetailById(int Id)
        {
            try
            {
                if (_chamberDetailVMs == null)
                {
                    string StoredProc = $"exec pCmsChamberDetailById @Id ='{Id}'";

                    _chamberDetailVMs = await _dbContext.ChamberDetailVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _chamberDetailVMs;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #region Save Chamber Sector 
        public async Task SaveChamberOperatingSector(ChamberOperatingSector chamberOperatingSector)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var existingChamberOperatingSectors = await _dbContext.ChamberOperatingSectors
                            .Where(c => chamberOperatingSector.SelectedChamberIds.Contains(c.ChamberId))
                            .ToListAsync();

                        _dbContext.ChamberOperatingSectors.RemoveRange(existingChamberOperatingSectors);

                        foreach (var chamberId in chamberOperatingSector.SelectedChamberIds)
                        {
                            ChamberOperatingSector newChamberOperatingSector = new ChamberOperatingSector
                            {
                                ChamberId = chamberId,
                                SectorTypeId = chamberOperatingSector.SectorTypeId,
                                CreatedBy = chamberOperatingSector.CreatedBy,
                                CreatedDate = DateTime.Now
                            };
                            await _dbContext.ChamberOperatingSectors.AddAsync(newChamberOperatingSector);
                        }
                        await _dbContext.SaveChangesAsync();
                        await transation.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transation.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        #endregion
        #region Get Chambers  by Court  Id 
        public async Task<List<Chamber>> GetChamberByCourtId(int courtId)
        {
            try
            {
                var result = await (from c in _dbContext.Chambers
                                    join cc in _dbContext.CourtChambers on c.Id equals cc.ChamberId into courtChambersGroup
                                    from cc in courtChambersGroup.DefaultIfEmpty()
                                    where cc.CourtId == courtId && c.IsActive && !c.IsDeleted  // Filter by courtId instead of chamberId
                                    select new
                                    {
                                        Chamber = c,
                                        CourtChamber = cc
                                    })
                .ToListAsync();

                var chamberWithCourtChambers = result.GroupBy(
                    x => x.Chamber,
                    x => x.CourtChamber,
                    (chamber, courtChambers) => new Chamber
                    {
                        Id = chamber.Id,
                        Number = chamber.Number,
                        Name_En = chamber.Name_En,
                        Name_Ar = chamber.Name_Ar,
                        Address = chamber.Address,
                        IsActive = chamber.IsActive,
                        ChamberCode = chamber.ChamberCode,
                        Description = chamber.Description,
                        SelectedCourtIds = courtChambers
                                            .Where(cc => cc != null)
                                            .Select(cc => cc.CourtId)
                                            .ToList()
                    })
                    .ToList();

                var finalResult = chamberWithCourtChambers;

                return finalResult;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #endregion
        #region Department Lookup 
        #region Get Department List 

        public async Task<List<DepartmentDetailVM>> GetDepartmentList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pDepartmentlist";

                var result = await _dbContext.DepartmentDetailVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }


        #endregion
        #region Get Department by Id 
        public async Task<Department> GetDepartmentById(int Id)
        {
            try
            {
                var result = await _dbContext.Departments.Where(x => x.Id == Id).FirstOrDefaultAsync();

                return result;


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #region Save Department
        public async Task<Department> SaveDepartment(Department department)
        {
            try
            {
                var task = await _dbContext.Departments.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                if (task != null)
                {
                    int id = GetNextUniqueIddept(task.Id);
                    department.Id = id;
                    await _dbContext.Departments.AddAsync(department);
                    await _dbContext.SaveChangesAsync();
                    LookupsHistory lookups = new LookupsHistory
                    {
                        LookupsId = department.Id,
                        NameEn = department.Name_En,
                        NameAr = department.Name_Ar,
                        LookupsTableId = (int)LookupsTablesEnum.Department,
                        CreatedDate = department.CreatedDate,
                        CreatedBy = department.CreatedBy,
                        IsActive = department.IsActive,
                        StatusId = (int)LookupHistoryEnums.Added,
                    };

                    await SaveLookupsHistroy(lookups, null);
                    return department;
                }
                else
                {
                    int counter = 0;
                    ++counter;
                    department.Id = counter;
                    await _dbContext.Departments.AddAsync(department);
                    await _dbContext.SaveChangesAsync();
                    return department;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #region Get Unique id 
        private int GetNextUniqueIddept(int counter)
        {
            if (_dbContext.Departments == null)
            {
                counter = 0;
                return ++counter;
            }
            else
            {
                return ++counter;
            }

        }
        #endregion
        #endregion
        #region Update Department
        public async Task<Department> UpdateDepartment(Department department)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.Departments.Where(x => x.Id == department.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = department.Id;
                            task.Name_En = department.Name_En;
                            task.Name_Ar = department.Name_Ar;
                            task.ModifiedBy = department.ModifiedBy;
                            task.ModifiedDate = department.ModifiedDate;
                            task.IsDeleted = department.IsDeleted;
                            task.IsActive = department.IsActive;
                            task.Borrow_Return_Day_Duration = department.Borrow_Return_Day_Duration;
                            await _dbContext.Departments.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = department.Id,
                                NameEn = department.Name_En,
                                NameAr = department.Name_Ar,
                                LookupsTableId = (int)LookupsTablesEnum.Department,
                                ModifiedBy = department.ModifiedBy,
                                ModifiedDate = department.ModifiedDate,
                                CreatedBy = department.ModifiedBy,
                                CreatedDate = (DateTime)department.ModifiedDate,
                                IsActive = department.IsActive,
                                StatusId = (int)LookupHistoryEnums.Updated,
                            };

                            await SaveLookupsHistroy(lookups, department.ModifiedDate);
                            transation.Commit();
                        }
                        return department;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Delete Department
        public async Task<DepartmentDetailVM> DeleteDepartment(DepartmentDetailVM department)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.Departments.Where(x => x.Id == department.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.DeletedBy = department.DeletedBy;
                            task.DeletedDate = department.DeletedDate;
                            task.IsDeleted = true;
                            await _dbContext.Departments.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return department;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Active status of Department
        public async Task<DepartmentDetailVM> ActiveDepartment(DepartmentDetailVM department)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.Departments.Where(x => x.Id == department.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = department.Id;
                            task.Name_En = department.Name_En;
                            task.Name_Ar = department.Name_Ar;
                            task.ModifiedBy = department.ModifiedBy;
                            task.ModifiedDate = department.ModifiedDate;
                            task.IsDeleted = (bool)department.IsDeleted;
                            task.IsActive = department.IsActive;
                            await _dbContext.Departments.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = department.Id,
                                LookupsTableId = (int)LookupsTablesEnum.Department,
                                NameEn = department.Name_En,
                                NameAr = department.Name_Ar,
                                ModifiedBy = department.ModifiedBy,
                                ModifiedDate = department.ModifiedDate,
                                CreatedBy = department.CreatedBy,
                                CreatedDate = (DateTime)department.CreatedDate,
                                StatusId = (int)LookupHistoryEnums.Updated,
                                IsActive = department.IsActive,
                            };
                            await ActiveLookupsHistroy(lookups, department.ModifiedDate);
                            transation.Commit();
                        }
                        return department;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #endregion

        #region TaskList Loopup
        #region Get TaskList
        public async Task<List<TaskTypeVM>> GetTaskTypeList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pTaskTypeList";

                var result = await _dbContext.TaskTypeVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion
        #region Get TaskList by Id 
        public async Task<TaskType> GetTaskTypeById(int TypeId)
        {
            try
            {
                var result = await _dbContext.TaskTypes.Where(x => x.TypeId == TypeId).FirstOrDefaultAsync();

                return result;


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #region Update TaskType
        public async Task<TaskType> UpdateTaskType(TaskType taskType)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.TaskTypes.Where(x => x.TypeId == taskType.TypeId).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.TypeId = taskType.TypeId;
                            task.NameEn = taskType.NameEn;
                            task.NameAr = taskType.NameAr;
                            task.ModifiedBy = taskType.ModifiedBy;
                            task.ModifiedDate = taskType.ModifiedDate;
                            task.IsDeleted = taskType.IsDeleted;
                            task.IsActive = taskType.IsActive;
                            task.Description = taskType.Description;
                            await _dbContext.TaskTypes.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return taskType;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion
        #endregion

        #region Communication Type  
        #region Get Communication Type List 
        public async Task<List<CommunicationTypeVM>> GetCommunicationTypeList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pCommunicationTypelist";

                var result = await _dbContext.CommunicationTypeVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        #endregion
        #region Get Communication Type by Id 
        public async Task<CommunicationType> GetCommunicationByTypeId(int CommunicationTypeId)
        {
            try
            {
                var result = await _dbContext.CommunicationTypes.Where(x => x.CommunicationTypeId == CommunicationTypeId).FirstOrDefaultAsync();

                return result;


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #region Save Communication Type
        public async Task<CommunicationType> SaveCommunicationType(CommunicationType communication)
        {
            try
            {
                await _dbContext.CommunicationTypes.AddAsync(communication);
                await _dbContext.SaveChangesAsync();
                return communication;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        #region Update Communication
        public async Task<CommunicationType> UpdateCommunicationType(CommunicationType communication)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.CommunicationTypes.Where(x => x.CommunicationTypeId == communication.CommunicationTypeId).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.CommunicationTypeId = communication.CommunicationTypeId;
                            task.NameEn = communication.NameEn;
                            task.NameAr = communication.NameAr;
                            task.ModifiedBy = communication.ModifiedBy;
                            task.ModifiedDate = communication.ModifiedDate;
                            //task.IsDeleted = department.IsDeleted;
                            //task.IsActive = department.IsActive;
                            await _dbContext.CommunicationTypes.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return communication;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #endregion
        #region Document Type 
        #region  Get document type
        public async Task<List<AttachmentTypeVM>> GetDocumentTypeList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pAttachmentTypelist";

                var result = await _dmsDbContext.AttachmentTypeVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion
        #region Update Document Type
        public async Task UpdateDocumentType(AttachmentType ldsDocument)
        {
            using (var dbContext = _dmsDbContext)
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (ldsDocument.AttachmentTypeIds.Any())
                        {
                            foreach (var AttachmentTypeId in ldsDocument.AttachmentTypeIds)
                            {
                                var task = await dbContext.AttachmentType.Where(x => x.AttachmentTypeId == AttachmentTypeId).FirstOrDefaultAsync();
                                if (task != null)
                                {
                                    task.IsDigitallySign = true;
                                    dbContext.Update(task);
                                    var existingDesignationMappings = await dbContext.DsAttachmentTypeDesignationMapping
                                        .Where(x => x.AttachmentTypeId == AttachmentTypeId)
                                        .ToListAsync();

                                    var designationIdsList = ldsDocument.DesignationIds.ToList();
                                    foreach (var designationId in designationIdsList)
                                    {
                                        var existingMapping = existingDesignationMappings
                                            .FirstOrDefault(x => x.DesignationId == designationId);

                                        if (existingMapping == null)
                                        {
                                            var newMapping = new DsAttachmentTypeDesignationMapping
                                            {
                                                AttachmentTypeId = AttachmentTypeId,
                                                DesignationId = designationId,
                                                CreatedBy = ldsDocument.ModifiedBy,
                                                CreatedDate = (DateTime)ldsDocument.ModifiedDate
                                            };
                                            await dbContext.DsAttachmentTypeDesignationMapping.AddAsync(newMapping);
                                        }
                                        else
                                        {
                                            existingMapping.ModifiedBy = ldsDocument.ModifiedBy;
                                            existingMapping.ModifiedDate = (DateTime)ldsDocument.ModifiedDate;
                                            dbContext.Update(existingMapping);
                                        }
                                    }
                                    //var mappingsToRemove = existingDesignationMappings.Where(x => !designationIdsList.Contains(x.DesignationId)).ToList();
                                    //dbContext.DsAttachmentTypeDesignationMapping.RemoveRange(mappingsToRemove);

                                    var existingSigningMethodMappings = await dbContext.DSAttachmentTypeSigningMethods.Where(x => x.AttachmentTypeId == AttachmentTypeId).ToListAsync();

                                    var signingMethodIdsList = ldsDocument.SigningMethodIds.ToList();
                                    foreach (var signingMethodId in signingMethodIdsList)
                                    {
                                        var existingMapping = existingSigningMethodMappings
                                            .FirstOrDefault(x => x.MethodId == signingMethodId);

                                        if (existingMapping == null)
                                        {
                                            var newMapping = new DSAttachmentTypeSigningMethods
                                            {
                                                AttachmentTypeId = AttachmentTypeId,
                                                MethodId = signingMethodId,
                                                CreatedBy = ldsDocument.ModifiedBy,
                                                CreatedDate = (DateTime)ldsDocument.ModifiedDate
                                            };
                                            await dbContext.DSAttachmentTypeSigningMethods.AddAsync(newMapping);
                                        }
                                        else
                                        {
                                            existingMapping.ModifiedBy = ldsDocument.ModifiedBy;
                                            existingMapping.ModifiedDate = (DateTime)ldsDocument.ModifiedDate;
                                            dbContext.Update(existingMapping);
                                        }
                                    }
                                    //var signingMethodMappingsToRemove = existingSigningMethodMappings.Where(x => !signingMethodIdsList.Contains(x.MethodId)).ToList();
                                    //dbContext.DSAttachmentTypeSigningMethods.RemoveRange(signingMethodMappingsToRemove);
                                }
                            }
                        }
                        else
                        {
                            var task = await dbContext.AttachmentType.Where(x => x.AttachmentTypeId == ldsDocument.AttachmentTypeId).FirstOrDefaultAsync();
                            if (task != null)
                            {
                                task.Type_En = ldsDocument.Type_En;
                                task.Type_Ar = ldsDocument.Type_Ar;
                                task.ModifiedBy = ldsDocument.ModifiedBy;
                                task.ModifiedDate = ldsDocument.ModifiedDate;
                                task.IsMandatory = ldsDocument.IsMandatory;
                                task.IsActive = ldsDocument.IsActive;
                                task.IsGePortalType = ldsDocument.IsGePortalType;
                                task.IsOfficialLetter = ldsDocument.IsOfficialLetter;
                                task.Description = ldsDocument.Description;
                                task.IsDigitallySign = ldsDocument.IsDigitallySign;
                                dbContext.Update(task);

                                if (!ldsDocument.IsDigitallySign)
                                {
                                    ldsDocument.DesignationIds = new List<int>();
                                    ldsDocument.SigningMethodIds = new List<int>();
                                    var existingDesignationMappings = await dbContext.DsAttachmentTypeDesignationMapping.Where(x => x.AttachmentTypeId == ldsDocument.AttachmentTypeId).ToListAsync();
                                    dbContext.DsAttachmentTypeDesignationMapping.RemoveRange(existingDesignationMappings);

                                    var existingSigningMethodMappings = await dbContext.DSAttachmentTypeSigningMethods.Where(x => x.AttachmentTypeId == ldsDocument.AttachmentTypeId).ToListAsync();
                                    dbContext.DSAttachmentTypeSigningMethods.RemoveRange(existingSigningMethodMappings);
                                }

                                if (ldsDocument.DesignationIds.Any())
                                {
                                    var existingDesignationMappings = await dbContext.DsAttachmentTypeDesignationMapping
                                        .Where(x => x.AttachmentTypeId == ldsDocument.AttachmentTypeId)
                                        .ToListAsync();

                                    var designationIds = ldsDocument.DesignationIds.ToList();
                                    var mappingsToRemove = existingDesignationMappings.Where(x => !designationIds.Contains(x.DesignationId)).ToList();
                                    dbContext.DsAttachmentTypeDesignationMapping.RemoveRange(mappingsToRemove);
                                    foreach (var designationId in designationIds)
                                    {
                                        var existingMapping = existingDesignationMappings.FirstOrDefault(x => x.DesignationId == designationId);
                                        if (existingMapping != null)
                                        {
                                            existingMapping.ModifiedBy = ldsDocument.ModifiedBy;
                                            existingMapping.ModifiedDate = ldsDocument.ModifiedDate;
                                            dbContext.Update(existingMapping);
                                        }
                                        else
                                        {
                                            var newData = new DsAttachmentTypeDesignationMapping
                                            {
                                                AttachmentTypeId = ldsDocument.AttachmentTypeId,
                                                DesignationId = designationId,
                                                CreatedBy = ldsDocument.ModifiedBy,
                                                CreatedDate = DateTime.Now
                                            };
                                            await dbContext.DsAttachmentTypeDesignationMapping.AddAsync(newData);
                                        }
                                    }
                                }
                                else
                                {
                                    var existingDesignationMappings = await dbContext.DsAttachmentTypeDesignationMapping.Where(x => x.AttachmentTypeId == ldsDocument.AttachmentTypeId).ToListAsync();
                                    dbContext.DsAttachmentTypeDesignationMapping.RemoveRange(existingDesignationMappings);
                                }
                                if (ldsDocument.SigningMethodIds.Any())
                                {
                                    var existingSigningMethodMappings = await dbContext.DSAttachmentTypeSigningMethods
                                        .Where(x => x.AttachmentTypeId == ldsDocument.AttachmentTypeId)
                                        .ToListAsync();
                                    var signingMethodIds = ldsDocument.SigningMethodIds.ToList();
                                    var signingMethodMappingsToRemove = existingSigningMethodMappings.Where(x => !signingMethodIds.Contains(x.MethodId)).ToList();
                                    dbContext.DSAttachmentTypeSigningMethods.RemoveRange(signingMethodMappingsToRemove);
                                    foreach (var signingMethodId in signingMethodIds)
                                    {
                                        var existingMapping = existingSigningMethodMappings.FirstOrDefault(x => x.MethodId == signingMethodId);
                                        if (existingMapping != null)
                                        {
                                            existingMapping.ModifiedBy = ldsDocument.ModifiedBy;
                                            existingMapping.ModifiedDate = ldsDocument.ModifiedDate;
                                            dbContext.Update(existingMapping);
                                        }
                                        else
                                        {
                                            var newData = new DSAttachmentTypeSigningMethods
                                            {
                                                AttachmentTypeId = ldsDocument.AttachmentTypeId,
                                                MethodId = signingMethodId,
                                                CreatedBy = ldsDocument.ModifiedBy,
                                                CreatedDate = DateTime.Now
                                            };
                                            await dbContext.DSAttachmentTypeSigningMethods.AddAsync(newData);
                                        }
                                    }
                                }
                                else
                                {
                                    var existingSigningMethodMappings = await dbContext.DSAttachmentTypeSigningMethods.Where(x => x.AttachmentTypeId == ldsDocument.AttachmentTypeId).ToListAsync();
                                    dbContext.DSAttachmentTypeSigningMethods.RemoveRange(existingSigningMethodMappings);
                                }
                            }
                        }
                        await dbContext.SaveChangesAsync();
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion
        #region Save Document  Type
        public async Task SaveDocumentType(AttachmentType attachment)
        {
            try
            {
                using (var transaction = _dmsDbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dmsDbContext.AttachmentType.OrderByDescending(x => x.AttachmentTypeId).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            int AttachmentTypeId = GetNextUniqueId(task.AttachmentTypeId);
                            attachment.AttachmentTypeId = AttachmentTypeId;
                            await _dmsDbContext.AttachmentType.AddAsync(attachment);
                        }
                        else
                        {
                            int counter = 0;
                            ++counter;
                            attachment.AttachmentTypeId = counter;
                            await _dmsDbContext.AttachmentType.AddAsync(attachment);
                        }

                        foreach (var designationId in attachment.DesignationIds)
                        {
                            var designationMapping = new DsAttachmentTypeDesignationMapping
                            {
                                AttachmentTypeId = attachment.AttachmentTypeId,
                                DesignationId = designationId,
                                CreatedBy = attachment.CreatedBy,
                                CreatedDate = attachment.CreatedDate
                            };
                            await _dmsDbContext.DsAttachmentTypeDesignationMapping.AddAsync(designationMapping);
                        }

                        foreach (var signingMethodId in attachment.SigningMethodIds)
                        {
                            var signingMethodMapping = new DSAttachmentTypeSigningMethods
                            {
                                AttachmentTypeId = attachment.AttachmentTypeId,
                                MethodId = signingMethodId,
                                CreatedBy = attachment.CreatedBy,
                                CreatedDate = attachment.CreatedDate
                            };
                            await _dmsDbContext.DSAttachmentTypeSigningMethods.AddAsync(signingMethodMapping);
                        }

                        await _dmsDbContext.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private int GetNextUniqueId(int counter)
        {
            if (_dmsDbContext.AttachmentType == null)
            {
                counter = 0;
                return ++counter;
            }
            else
            {
                return ++counter;
            }

        }
        #endregion
        #region Get Document Type by Id 
        public async Task<AttachmentType> GetDocumentTypeById(int AttachmentTypeId)
        {
            try
            {
                var result = await _dmsDbContext.AttachmentType.Where(x => x.AttachmentTypeId == AttachmentTypeId).FirstOrDefaultAsync();
                if (result != null)
                {
                    result.DesignationIds = await _dmsDbContext.DsAttachmentTypeDesignationMapping
                        .Where(x => x.AttachmentTypeId == AttachmentTypeId).Select(x => x.DesignationId)
                        .ToListAsync();

                    result.SigningMethodIds = await _dmsDbContext.DSAttachmentTypeSigningMethods
                        .Where(x => x.AttachmentTypeId == AttachmentTypeId).Select(x => x.MethodId)
                        .ToListAsync();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        #region Get Module 
        public async Task<List<Module>> GetModule()
        {
            try
            {
                return await _dbContext.Modules.OrderBy(c => c.ModuleId).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Get Sub Type Id 
        public async Task<List<Subtype>> GetSubTypeId()
        {
            try
            {
                return await _dbContext.Subtypes.OrderBy(c => c.Id).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #endregion
        #region  Case File Status
        #region  Get Case File Status List
        public async Task<List<CaseFileStatusVM>> GetCaseFileStatusList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pCaseFileStatuslist";

                var result = await _dbContext.CaseFileStatusVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion
        #region  Update Case File Status
        public async Task<CaseFileStatus> UpdateCaseFileStatus(CaseFileStatus caseFileStatus)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.CaseFileStatus.Where(x => x.Id == caseFileStatus.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = caseFileStatus.Id;
                            task.Name_En = caseFileStatus.Name_En;
                            task.Name_Ar = caseFileStatus.Name_Ar;
                            task.ModifiedBy = caseFileStatus.ModifiedBy;
                            task.ModifiedDate = caseFileStatus.ModifiedDate;
                            task.IsDeleted = caseFileStatus.IsDeleted;
                            task.IsActive = caseFileStatus.IsActive;
                            await _dbContext.CaseFileStatus.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return caseFileStatus;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Get Case File Status By Id
        public async Task<CaseFileStatus> GetCaseFileStatusById(int Id)
        {
            try
            {
                var result = await _dbContext.CaseFileStatus.Where(x => x.Id == Id).FirstOrDefaultAsync();

                return result;


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #endregion
        #region  Case Request Status
        #region  Get Case Request Status List
        public async Task<List<CaseRequestStatusVM>> GetCaseRequestStatusList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pCaseRequestStatuslist";

                var result = await _dbContext.CaseRequestStatusVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion
        #region  Update Case Request Status
        public async Task<CaseRequestStatus> UpdateCaseRequestStatus(CaseRequestStatus caseRequestStatus)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.CaseRequestStatus.Where(x => x.Id == caseRequestStatus.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = caseRequestStatus.Id;
                            task.Name_En = caseRequestStatus.Name_En;
                            task.Name_Ar = caseRequestStatus.Name_Ar;
                            task.ModifiedBy = caseRequestStatus.ModifiedBy;
                            task.ModifiedDate = caseRequestStatus.ModifiedDate;
                            task.IsDeleted = caseRequestStatus.IsDeleted;
                            task.IsActive = caseRequestStatus.IsActive;
                            await _dbContext.CaseRequestStatus.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return caseRequestStatus;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Get Case Request Status By Id
        public async Task<CaseRequestStatus> GetCaseRequestStatusById(int Id)
        {
            try
            {
                var result = await _dbContext.CaseRequestStatus.Where(x => x.Id == Id).FirstOrDefaultAsync();

                return result;


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #endregion
        #region  Case  Status
        #region  Get Case  Status List
        public async Task<List<CmsRegisteredCaseStatusVM>> GetCaseStatusList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pCaseStatuslist";

                var result = await _dbContext.CmsRegisteredCaseStatusVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion
        #region  Update Case File Status
        public async Task UpdateCaseStatus(CmsRegisteredCaseStatus RegisteredCaseStatus)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.CmsRegisteredCasestatuss.Where(x => x.Id == RegisteredCaseStatus.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = RegisteredCaseStatus.Id;
                            task.Name_En = RegisteredCaseStatus.Name_En;
                            task.Name_Ar = RegisteredCaseStatus.Name_Ar;
                            task.ModifiedBy = RegisteredCaseStatus.ModifiedBy;
                            task.ModifiedDate = RegisteredCaseStatus.ModifiedDate;
                            task.IsDeleted = RegisteredCaseStatus.IsDeleted;
                            task.IsActive = RegisteredCaseStatus.IsActive;
                            await _dbContext.CmsRegisteredCasestatuss.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Get Case  Status By Id
        public async Task<CmsRegisteredCaseStatus> GetCaseStatusById(int Id)
        {
            try
            {
                var result = await _dbContext.CmsRegisteredCasestatuss.Where(x => x.Id == Id).FirstOrDefaultAsync();

                return result;


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion
        #endregion
        #region Save literature Tag
        public async Task SaveLiteratureTags(LiteratureTag literatureTag)
        {
            try
            {
                await _dbContext.LiteratureTags.AddAsync(literatureTag);
                await _dbContext.SaveChangesAsync();
                LookupsHistory lookups = new LookupsHistory
                {
                    LookupsId = literatureTag.Id,
                    TagNo = literatureTag.TagNo,
                    Description = literatureTag.Description,
                    DescriptionAr = literatureTag.Description_Ar,
                    LookupsTableId = (int)LookupsTablesEnum.LMS_LITERATURE_TAG,
                    CreatedDate = literatureTag.CreatedDate,
                    CreatedBy = literatureTag.CreatedBy,
                    StatusId = (int)LookupHistoryEnums.Added,
                };

                await SaveLookupsHistroy(lookups, null);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        #region Sector Type Enums
        #region Get Sector List
        public async Task<List<SectorTypeVM>> GetSectorTypeList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pSectorTypeList";

                var result = await _dbContext.SectorTypeVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion
        #region Get Sector Type by Id 
        public async Task<OperatingSectorType> GetSectorTypeById(int Id)
        {
            try
            {
                var result = await _dbContext.OperatingSectorType.Where(x => x.Id == Id).FirstOrDefaultAsync();

                return result;


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        //<History Author='Ammaar Naveed' Date='05/03/2024'>Fetch buildings</History>//
        public async Task<List<SectorBuilding>> GetSectorBuilding()
        {
            try
            {
                if (buildingNames == null)
                {
                    buildingNames = await _dbContext.SectorBuilding.ToListAsync();
                }
                return buildingNames;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //<History Author='Ammaar Naveed' Date='05/03/2024'>Fetch floors</History>//
        public async Task<List<SectorFloor>> GetSectorFloor()
        {
            try
            {
                if (floorNames == null)
                {
                    floorNames = await _dbContext.SectorFloor.ToListAsync();
                }
                return floorNames;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        #region Update Sector Type
        //<History Author = 'Ammaar Naveed' Date='2024-03-07' Version="1.0" Branch="master"> Provision of lookup history for sector type.</History>
        public async Task UpdateSectorType(OperatingSectorType SectorType)
        {

            using (var dbContext = _dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.OperatingSectorType.Where(x => x.Id == SectorType.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = SectorType.Id;
                            task.Name_En = SectorType.Name_En;
                            task.Name_Ar = SectorType.Name_Ar;
                            task.ModifiedBy = SectorType.ModifiedBy;
                            task.ModifiedDate = SectorType.ModifiedDate;
                            task.IsDeleted = SectorType.IsDeleted;
                            task.IsActive = SectorType.IsActive;
                            task.BuildingId = SectorType.BuildingId;
                            task.FloorId = SectorType.FloorId;
                            task.IsOnlyViceHosApprovalRequired = SectorType.IsOnlyViceHosApprovalRequired;
                            task.IsViceHosResponsibleForAllLawyers = SectorType.IsViceHosResponsibleForAllLawyers;
                            task.G2GBRSiteID = SectorType.G2GBRSiteID;
                            await _dbContext.OperatingSectorType.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();

                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = SectorType.Id,
                                NameEn = SectorType.Name_En,
                                NameAr = SectorType.Name_Ar,
                                LookupsTableId = (int)LookupsTablesEnum.CMS_OPERATING_SECTOR_TYPE_G2G_LKP,
                                ModifiedBy = SectorType.ModifiedBy,
                                ModifiedDate = SectorType.ModifiedDate,
                                CreatedBy = SectorType.ModifiedBy,
                                CreatedDate = (DateTime)SectorType.ModifiedDate,
                                StatusId = (int)LookupHistoryEnums.Updated,
                                IsActive = SectorType.IsActive
                            };
                            await SaveSectorRoles(new CmsOperatingSectorTypesRoles() { SectorId = SectorType.Id, RoleIds = SectorType.RoleIds });
                            await SaveLookupsHistroy(lookups, SectorType.ModifiedDate);
                            transaction.Commit();
                        }
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #endregion
        #region  Get Governament Entity User List
        public async Task<List<GovernmentEntity>> GetAllUserGroupsList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pGetAllUserGroupsGovEntitesList";

                var result = await _dbContext.GovernmentEntity.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion



        #region  Get Case Request Pattren List
        public async Task<List<CmsComsNumPatternVM>> GetAllCaseRequestNumber(int PatternTypeId)
        {
            string storedProc;
            try
            {
                storedProc = $"exec pCmsComsPattrenList @PatternTypeId = '{PatternTypeId}'";

                var result = await _dbContext.CmsComsNumPatternVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion
        #region Save Case File Number Pattren
        public async Task<CmsComsNumPattern> SaveCMSCOMSPattrenNumber(CmsComsNumPattern comsNumPattern)
        {
            using (var dbContext = _dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        await _dbContext.CmsComsNumPatterns.AddAsync(comsNumPattern);
                        await _dbContext.SaveChangesAsync();
                        await SyncPatterenNumberWithGE(comsNumPattern, dbContext);
                        await dbContext.SaveChangesAsync();
                        transaction.Commit();
                        return comsNumPattern;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        /* private async Task AddPatternNumberPattrenGroupsofCaseAndConsaltationfile(List<FATWA_DOMAIN.Models.AdminModels.UserManagement.Group> numPatternGroups, Guid Id, DatabaseContext dbContext)
         {
             try
             {
                 foreach (var item in numPatternGroups)
                 {
                     var res = await dbContext.Groups.Where(x => x.GroupId == item.GroupId).FirstOrDefaultAsync();
                     if (res != null)
                     {
                         CmsComsNumPatternGroups? Obj = new CmsComsNumPatternGroups();
                         Obj.GroupId = item.GroupId;
                         Obj.NumPattrenId = Id;
                         await dbContext.CmsComsNumPatternGroups.AddAsync(Obj);
                         await dbContext.SaveChangesAsync();
                     }
                 }

             }
             catch (Exception)
             {

                 throw;
             }
         }
 */
        /*     private async Task AddPatternNumberCOMSPattrenGroups(CmsComsNumPattern comsNumPattern, DatabaseContext dbContext)
             {
                 try
                 {
                     foreach (var item in comsNumPattern.GovernamentEntityIds)
                     {
                         var governmentEntity = await dbContext.GovernmentEntity.FirstOrDefaultAsync(x => x.EntityId == item);
                         var cmsGovtEntityNumPattern = await dbContext.CmsGovtEntityNumPattern.FirstOrDefaultAsync(x => x.GovtEntityId == item);

                         cmsGovtEntityNumPattern.GovtEntityId = governmentEntity.EntityId;
                         cmsGovtEntityNumPattern.COMSRequestPatternId = comsNumPattern.Id; // For Case Request
                         cmsGovtEntityNumPattern.CreatedDate = comsNumPattern.CreatedDate;
                         cmsGovtEntityNumPattern.CreatedBy = comsNumPattern.CreatedBy;

                         dbContext.Entry(cmsGovtEntityNumPattern).State = EntityState.Modified;
                         await dbContext.SaveChangesAsync(); // Save the changes to the existing governmentEntity
                     }
                 }
                 catch (Exception)
                 {

                     throw;
                 }
             }
             private async Task<CmsComsNumPattern> AddCOMSGovtEntityPatternNumber(CmsComsNumPattern comsNumPattern, DatabaseContext dbContext)
             {
                 try
                 {
                     foreach (var item in comsNumPattern.GovernamentEntityIds)
                     {
                         var governmentEntity = await dbContext.GovernmentEntity.FirstOrDefaultAsync(x => x.EntityId == item);

                         if (governmentEntity != null)
                         {
                             //comsNumPattern.GovtEntityNumPatternId = governmentEntity.EntityId;
                             dbContext.Entry(comsNumPattern).State = EntityState.Added;
                         }
                     }
                     return comsNumPattern;
                 }
                 catch (Exception)
                 {
                     throw;
                 }
             }*/
        private async Task SyncPatterenNumberWithGE(CmsComsNumPattern comsNumPattern, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in comsNumPattern.GovernamentEntityIds)
                {
                    var governmentEntity = await dbContext.GovernmentEntity.FirstOrDefaultAsync(x => x.EntityId == item);
                    var cmsGovtEntityNumPattern = await dbContext.CmsGovtEntityNumPattern.FirstOrDefaultAsync(x => x.GovtEntityId == item);

                    if (cmsGovtEntityNumPattern != null)
                    {
                        cmsGovtEntityNumPattern.GovtEntityId = governmentEntity.EntityId;
                        if (comsNumPattern.PatternTypId == (int)CmsComsNumPatternTypeEnum.CaseRequestNumber)
                        {
                            cmsGovtEntityNumPattern.CMSRequestPatternId = comsNumPattern.Id;
                        }
                        else
                        {
                            cmsGovtEntityNumPattern.COMSRequestPatternId = comsNumPattern.Id; // For COMS request Number
                        }
                        cmsGovtEntityNumPattern.CreatedDate = comsNumPattern.CreatedDate;
                        cmsGovtEntityNumPattern.CreatedBy = comsNumPattern.CreatedBy;
                        dbContext.Entry(cmsGovtEntityNumPattern).State = EntityState.Modified;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        /*
        private async Task<CmsComsNumPattern> AddCMSGovtEntityPatternNumber( CmsComsNumPattern comsNumPattern, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in comsNumPattern.GovernamentEntityIds)
                {
                    var governmentEntity = await dbContext.GovernmentEntity.FirstOrDefaultAsync(x => x.EntityId == item);

                    if (governmentEntity != null)
                    {
                        //comsNumPattern.GovtEntityNumPatternId = governmentEntity.EntityId;
                        dbContext.Entry(comsNumPattern).State = EntityState.Added;
                    }
                }
                return comsNumPattern;
            }
            catch (Exception)
            {

                throw;
            }
        }*/
        #endregion

        #region Update Case File Number Pattren
        /* public async Task<CmsComsNumPattern> UpdateCaseFileNumberPattren(CmsComsNumPattern comsNumPattern)
         {
             string storedProc;
             using (var dbContext = _dbContext)
             {
                 using (var transation = _dbContext.Database.BeginTransaction())
                 {
                     try
                     {
                         if (comsNumPattern != null)
                         {
                             var task = await _dbContext.CmsComsNumPatterns.Where(x => x.Id == comsNumPattern.Id).FirstOrDefaultAsync();

                             if (comsNumPattern.PatternTypId == (int)CmsComsNumPatternTypeEnum.CaseFileNumber || comsNumPattern.PatternTypId == (int)CmsComsNumPatternTypeEnum.ConsultationFileNumber)
                             {
                                 storedProc = $"exec pGetAllUserGroupsList";

                                 var result = await _dbContext.Groups.FromSqlRaw(storedProc).ToListAsync();
                                 comsNumPattern.usersGroup = result;
                                 var groupIds = result.Select(group => group.GroupId);
                                 comsNumPattern.GroupIds = groupIds;
                                 await AddPatternNumberPattrenGroupsofCaseAndConsaltationfile(comsNumPattern.usersGroup, comsNumPattern.Id, _dbContext);
                             }
                             else if (comsNumPattern.PatternTypId == (int)CmsComsNumPatternTypeEnum.CaseRequestNumber)
                             {
                                 await RemovePatternNumberCMSPattrenGovEntity(comsNumPattern.GroupIds, comsNumPattern.Id, _dbContext);
                                 await UpdatePatternNumberCMSPattrenGroups(comsNumPattern.GroupIds, comsNumPattern.Id, _dbContext);

                             }
                             else if (comsNumPattern.PatternTypId == (int)CmsComsNumPatternTypeEnum.ConsultationRequestNumber)
                             {
                                 await RemovePatternNumberCOMSPattrenGovEntity(comsNumPattern.GroupIds, comsNumPattern.Id, _dbContext);
                                 await UpdatePatternNumberCOMSPattrenGroups(comsNumPattern.GroupIds, comsNumPattern.Id, _dbContext);

                             }


                             //   await UpdateAuthorByLiterature(comsNumPattern, _dbContext);

                             if (task != null)
                             {
                                 task.Id = comsNumPattern.Id;
                                 task.Year = comsNumPattern.Year;
                                 task.Day = comsNumPattern.Day;
                                 task.Month = comsNumPattern.Month;
                                 task.SequanceNumber = comsNumPattern.SequanceNumber;
                                 task.StaticTextPattern = comsNumPattern.StaticTextPattern;
                                 task.SequanceResult = comsNumPattern.SequanceResult;
                                 task.ModifiedDate = comsNumPattern.ModifiedDate;
                                 task.IsDeleted = comsNumPattern.IsDeleted;
                                 task.IsActive = comsNumPattern.IsActive;
                                 //task.UserId = comsNumPattern.UserId;

                                 await _dbContext.CmsComsNumPatterns.AddAsync(task);
                                 _dbContext.Entry(task).State = EntityState.Modified;
                                 await _dbContext.SaveChangesAsync();
                                 transation.Commit();
                             }
                         }
                         return comsNumPattern;
                     }

                     catch (Exception ex)
                     {
                         transation.Rollback();
                         throw;
                     }
                 }
             }

         }
 */
        /* private async Task UpdatePatternNumberCMSPattrenGroups(IEnumerable<Guid> usersGroupIds, Guid id, DatabaseContext dbContext)
         {
             try
             {
                 foreach (var item in usersGroupIds)
                 {
                     var res = await dbContext.CmsGovtEntityNumPattern.Where(x => x.CMSRequestPatternId == item).FirstOrDefaultAsync();
                     if (res != null)
                     {
                         CmsGovtEntityNumPattern? Obj = new CmsGovtEntityNumPattern();
                         Obj.CMSRequestPatternId = item;

                         await _dbContext.CmsGovtEntityNumPattern.AddAsync(Obj);
                         await _dbContext.SaveChangesAsync();
                     }
                 }

             }
             catch (Exception)
             {

                 throw;
             }
         }
 */
        /* private async Task UpdatePatternNumberCOMSPattrenGroups(IEnumerable<Guid> usersGroupIds, Guid id, DatabaseContext dbContext)
         {
             try
             {
                 foreach (var item in usersGroupIds)
                 {
                     var res = await dbContext.CmsGovtEntityNumPattern.Where(x => x.COMSRequestPatternId == item).FirstOrDefaultAsync();
                     if (res != null)
                     {
                         CmsGovtEntityNumPattern? Obj = new CmsGovtEntityNumPattern();
                         Obj.CMSRequestPatternId = item;

                         await _dbContext.CmsGovtEntityNumPattern.AddAsync(Obj);
                         await _dbContext.SaveChangesAsync();
                     }
                 }

             }
             catch (Exception)
             {

                 throw;
             }
         }*/

        /*private async Task RemovePatternNumberCMSPattrenGovEntity(IEnumerable<Guid> usersGroupIds, Guid id, DatabaseContext dbContext)
        {
            try
            {
                dbContext.CmsGovtEntityNumPattern.RemoveRange(await dbContext.CmsGovtEntityNumPattern.Where(x => x.CMSRequestPatternId == id).ToListAsync());
                //}
                await dbContext.SaveChangesAsync();
                //}

            }
            catch (Exception)
            {

                throw;
            }
        }
       */
        /*private async Task RemovePatternNumberCOMSPattrenGovEntity(IEnumerable<Guid> usersGroupIds, Guid id, DatabaseContext dbContext)
        {
            try
            {
                dbContext.CmsGovtEntityNumPattern.RemoveRange(await dbContext.CmsGovtEntityNumPattern.Where(x => x.COMSRequestPatternId == id).ToListAsync());
                //}
                await dbContext.SaveChangesAsync();
                //}

            }
            catch (Exception)
            {

                throw;
            }
        }*/
        /*        private async Task DeletePatternNumberPattrenGroups(Guid id, DatabaseContext dbContext)
                {
                    try
                    {


                        //Foreach (var item in res)
                        //{
                        //await _dbContext.CmsComsNumPatternGroups.AddAsync(Obj);

                        //dbContext.CmsComsNumPatternGroups.Remove(Obj);
                        //await dbContext.SaveChangesAsync();
                        //await dbContext.SaveChangesAsync();
                        dbContext.CmsComsNumPatternGroups.RemoveRange(await dbContext.CmsComsNumPatternGroups.Where(x => x.NumPattrenId == id).ToListAsync());
                        //}
                        await dbContext.SaveChangesAsync();
                        //}

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
        */
        #endregion

        #region  Get Governament Entity User List

        #endregion
        //<History Author = 'Nabeel ur Rehman' Date='2022-12-13' Version="1.0" Branch="master">Get Number Pattern type</History>
        public async Task<List<CmsComsNumPatternType>> GetCmsComsNumberPatterntype()
        {
            try
            {
                return await _dbContext.CmsComsNumPatternTypes.OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Get Cms Pattern By Id
        public async Task<CmsComsNumPattern> GetCmsPatternById(Guid Id)
        {
            try
            {
                string storedProc;
                try
                {
                    storedProc = $"exec pCmsComsPattrenbyId @Id = '{Id}'";

                    List<CmsComsNumPattern>? result = await _dbContext.CmsComsNumPatterns.FromSqlRaw(storedProc).ToListAsync();

                    foreach (var result2 in result)
                    {
                        if (result2 != null)
                        {
                            if (result2.PatternTypId == (int)CmsComsNumPatternTypeEnum.CaseFileNumber || result2.PatternTypId == (int)CmsComsNumPatternTypeEnum.ConsultationFileNumber)
                            {
                                string storedProc2 = $"exec pCmsComsGovEntityGroupsbyCMSPatternId @PatternId = '{Id}'";

                                var groups = await _dbContext.CmsGovtEntityNumPattern.FromSqlRaw(storedProc2).ToListAsync();
                                result2.CmsGovtEntityNumPatternGroup = groups;
                                var groupIds = groups.Select(group => group.CMSRequestPatternId);
                                result2.GroupIds = groupIds;
                            }
                            else if (result2.PatternTypId == (int)CmsComsNumPatternTypeEnum.CaseRequestNumber)
                            {
                                string storedProc2 = $"exec pCmsComsGovEntityGroupsbyCMSPatternId @PatternId = '{Id}'";

                                var groups = await _dbContext.CmsGovtEntityNumPattern.FromSqlRaw(storedProc2).ToListAsync();
                                result2.CmsGovtEntityNumPatternGroup = groups;
                                var groupIds = groups.Select(group => group.CMSRequestPatternId);
                                result2.GroupIds = groupIds;
                            }
                            else if (result2.PatternTypId == (int)CmsComsNumPatternTypeEnum.ConsultationRequestNumber)
                            {
                                string storedProc2 = $"exec pCmsComsGovEntityGroupsbyCOMSPatternId @PatternId = '{Id}'";

                                var groups = await _dbContext.CmsGovtEntityNumPattern.FromSqlRaw(storedProc2).ToListAsync();
                                result2.CmsGovtEntityNumPatternGroup = groups;
                                var groupIds = groups.Select(group => group.COMSRequestPatternId);
                                result2.GroupIds = groupIds;
                            }
                        }
                    }
                    return result.FirstOrDefault();
                }
                catch (Exception)
                {
                    return null;
                    throw;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public async Task<List<Group>> GetCmsComsNumberPatternGroupById(Guid Id)
        {
            try
            {
                List<CmsComsNumPatternGroups> result = await _dbContext.CmsComsNumPatternGroups.Where(x => x.NumPattrenId == Id).ToListAsync();
                if (result.Count > 0)
                {
                    foreach (var group in result)
                    {
                        var result2 = await _dbContext.Groups.Where(x => x.GroupId == group.GroupId).ToListAsync();
                        if (result2 != null)
                        {
                            _CmsComsNumGroups.usersGroup = result2;
                        }

                    }
                    return _umsUserGroups;
                }
                return null;

            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        public async Task<CmsComsNumPatternVM> DeleteCmComsPattern(CmsComsNumPatternVM cmsComsNumPatternVM)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var NumberPattrn = await _dbContext.CmsComsNumPatterns.Where(x => x.Id == cmsComsNumPatternVM.Id).FirstOrDefaultAsync();
                        if (NumberPattrn != null)
                        {
                            NumberPattrn.DeletedBy = cmsComsNumPatternVM.DeletedBy;
                            NumberPattrn.DeletedDate = cmsComsNumPatternVM.DeletedDate;
                            NumberPattrn.IsDeleted = true;
                            await _dbContext.CmsComsNumPatterns.AddAsync(NumberPattrn);
                            _dbContext.Entry(NumberPattrn).State = EntityState.Modified;
                            var deletedCMSCOMSReqPatteren = await _dbContext.CmsGovtEntityNumPattern.Where(x => x.CMSRequestPatternId == NumberPattrn.Id || x.COMSRequestPatternId == NumberPattrn.Id).ToListAsync();
                            if (deletedCMSCOMSReqPatteren != null)
                            {
                                foreach (var pattern in deletedCMSCOMSReqPatteren)
                                {
                                    if (cmsComsNumPatternVM.PatternTypeId == (int)CmsComsNumPatternTypeEnum.CaseRequestNumber)
                                        pattern.CMSRequestPatternId = Guid.Empty;
                                    else
                                        pattern.COMSRequestPatternId = Guid.Empty;
                                    _dbContext.Entry(pattern).State = EntityState.Modified;
                                }
                            }
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return cmsComsNumPatternVM;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }

        public async Task<CmsComsNumPatternHistory> GetCmsPatternHistoryById(Guid Id)
        {
            try
            {
                string storedProc;
                try
                {
                    storedProc = $"exec pCmsComsPattrenHistorybyId @PatternId = '{Id}'";

                    List<CmsComsNumPatternHistory>? result = await _dbContext.CmsComsNumPatternHistories.FromSqlRaw(storedProc).ToListAsync();

                    foreach (var result2 in result)
                    {
                        if (result2 != null)
                        {
                            if (result2.PatternTypId == (int)CmsComsNumPatternTypeEnum.CaseFileNumber || result2.PatternTypId == (int)CmsComsNumPatternTypeEnum.ConsultationFileNumber)
                            {
                                string storedProc2 = $"exec pCmsComsGovEntityGroupsbyCMSPatternId @PatternId = '{Id}'";

                                var groups = await _dbContext.CmsGovtEntityNumPattern.FromSqlRaw(storedProc2).ToListAsync();
                                result2.CmsGovtEntityNumPatternGroup = groups;
                                var groupIds = groups.Select(group => group.CMSRequestPatternId);
                                result2.GroupIds = groupIds;
                            }
                            else if (result2.PatternTypId == (int)CmsComsNumPatternTypeEnum.CaseRequestNumber)
                            {
                                string storedProc2 = $"exec pCmsComsGovEntityGroupsbyCMSPatternId @PatternId = '{Id}'";

                                var groups = await _dbContext.CmsGovtEntityNumPattern.FromSqlRaw(storedProc2).ToListAsync();
                                result2.CmsGovtEntityNumPatternGroup = groups;
                                var groupIds = groups.Select(group => group.CMSRequestPatternId);
                                result2.GroupIds = groupIds;
                            }
                            else if (result2.PatternTypId == (int)CmsComsNumPatternTypeEnum.ConsultationRequestNumber)
                            {
                                string storedProc2 = $"exec pCmsComsGovEntityGroupsbyCOMSPatternId @PatternId = '{Id}'";

                                var groups = await _dbContext.CmsGovtEntityNumPattern.FromSqlRaw(storedProc2).ToListAsync();
                                result2.CmsGovtEntityNumPatternGroup = groups;
                                var groupIds = groups.Select(group => group.COMSRequestPatternId);
                                result2.GroupIds = groupIds;
                            }
                        }
                    }
                    return result.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    return null;
                    throw;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #region Legislation type 
        public async Task<List<LegallegislationtypesVM>> Getlegallegislationtypes()
        {
            try
            {
                string StoredProc = $"exec pLegallegislationtypes";
                var result = await _dbContext.legallegislationtypesVMs.FromSqlRaw(StoredProc).ToListAsync();
                if (result != null)
                {
                    _legallegislationtypesVMS = result;
                }
                return _legallegislationtypesVMS;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<LegalLegislationType> GetLegalLegislationtypeById(int Id)
        {
            try
            {
                LegalLegislationType legallegislationtypes = await _dbContext.legalLegislationTypes.FirstOrDefaultAsync(x => x.Id == Id);

                if (legallegislationtypes != null)
                {
                    return legallegislationtypes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get legal pricipal types 

        public async Task<List<LegalPrincipleTypeVM>> GetLegalPrincipleTypes()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pLegalprincipaltype";

                return await _dbContext.LegalPrincipleTypeVMs.FromSqlRaw(storedProc).ToListAsync();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }


        #endregion

        #region Get Literature Tag
        public async Task<List<LmsLiteratureTagVM>> GetLmsLiteratureTags()
        {
            try
            {
                string StoredProc = $"exec pLmsLiteratureTag";
                var result = await _dbContext.LmsLiteratureTagVMs.FromSqlRaw(StoredProc).ToListAsync();
                if (result != null)
                {
                    _lmsLiteratureTagVM = result;
                }
                return _lmsLiteratureTagVM;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<LiteratureTag> GetLmsLiteratureTagsById(int Id)
        {
            try
            {
                LiteratureTag literaturetag = await _dbContext.LiteratureTags.FirstOrDefaultAsync(x => x.Id == Id);

                if (literaturetag != null)
                {
                    return literaturetag;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region Update Literture Tags
        public async Task UpdateLiteratureTags(LiteratureTag LmsLiteratureTagVM)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.LiteratureTags.Where(x => x.Id == LmsLiteratureTagVM.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = LmsLiteratureTagVM.Id;
                            task.TagNo = LmsLiteratureTagVM.TagNo;
                            task.Description = LmsLiteratureTagVM.Description;
                            task.Description_Ar = LmsLiteratureTagVM.Description_Ar;
                            task.ModifiedBy = LmsLiteratureTagVM.ModifiedBy;
                            task.ModifiedDate = LmsLiteratureTagVM.ModifiedDate;
                            task.IsDeleted = (bool)LmsLiteratureTagVM.IsDeleted;
                            await _dbContext.LiteratureTags.AddAsync(task);
                            task.IsActive = LmsLiteratureTagVM.IsActive;
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = LmsLiteratureTagVM.Id,
                                TagNo = LmsLiteratureTagVM.TagNo,
                                Description = LmsLiteratureTagVM.Description,
                                DescriptionAr = LmsLiteratureTagVM.Description_Ar,
                                LookupsTableId = (int)LookupsTablesEnum.LMS_LITERATURE_TAG,
                                ModifiedBy = LmsLiteratureTagVM.ModifiedBy,
                                ModifiedDate = LmsLiteratureTagVM.ModifiedDate,
                                CreatedBy = LmsLiteratureTagVM.ModifiedBy,
                                CreatedDate = (DateTime)LmsLiteratureTagVM.ModifiedDate,
                                IsActive = LmsLiteratureTagVM.IsActive,
                                StatusId = (int)LookupHistoryEnums.Updated,


                            };

                            await SaveLookupsHistroy(lookups, LmsLiteratureTagVM.ModifiedDate);
                            transation.Commit();
                        }
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Delete Department
        public async Task DeleteLiteratureTags(LmsLiteratureTagVM LmsLiteratureTagVM)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.LiteratureTags.Where(x => x.Id == LmsLiteratureTagVM.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            //task.DeletedBy = LmsLiteratureTagVM.DeletedBy;
                            //task.DeletedDate = LmsLiteratureTagVM.DeletedDate;
                            //task.IsDeleted = true;
                            _dbContext.LiteratureTags.Remove(task);
                            //_dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Active  Literature Tags
        public async Task ActiveLiteratureTags(LmsLiteratureTagVM LmsLiteratureTagVM)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.LiteratureTags.Where(x => x.Id == LmsLiteratureTagVM.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = LmsLiteratureTagVM.Id;
                            task.TagNo = LmsLiteratureTagVM.TagNo;
                            task.Description = LmsLiteratureTagVM.Description;
                            task.Description_Ar = LmsLiteratureTagVM.Description_Ar;
                            task.ModifiedBy = LmsLiteratureTagVM.ModifiedBy;
                            task.ModifiedDate = LmsLiteratureTagVM.ModifiedDate;
                            task.IsDeleted = (bool)LmsLiteratureTagVM.IsDeleted;
                            task.IsActive = LmsLiteratureTagVM.IsActive;
                            await _dbContext.LiteratureTags.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = LmsLiteratureTagVM.Id,
                                LookupsTableId = (int)LookupsTablesEnum.LMS_LITERATURE_TAG,
                                TagNo = LmsLiteratureTagVM.TagNo,
                                Description = LmsLiteratureTagVM.Description,
                                DescriptionAr = LmsLiteratureTagVM.Description_Ar,
                                ModifiedBy = LmsLiteratureTagVM.ModifiedBy,
                                ModifiedDate = LmsLiteratureTagVM.ModifiedDate,
                                CreatedBy = LmsLiteratureTagVM.CreatedBy,
                                CreatedDate = LmsLiteratureTagVM.CreatedDate,
                                IsActive = LmsLiteratureTagVM.IsActive,
                                StatusId = (int)LookupHistoryEnums.Updated,
                            };
                            await ActiveLookupsHistroy(lookups, LmsLiteratureTagVM.ModifiedDate);
                            transation.Commit();
                        }
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        public async Task AddLookupHistory(LmsLiteratureTagVM LmsLiteratureTagVM)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        //var task = new LookupHistory(
                        //     task.NameEn = LmsLiteratureTagVM.Description;
                        //     task.ModifiedBy = LmsLiteratureTagVM.ModifiedBy;
                        //     task.ModifiedDate = LmsLiteratureTagVM.ModifiedDate;
                        //     task.IsDeleted = (bool)LmsLiteratureTagVM.IsDeleted;
                        //     task.IsActive = LmsLiteratureTagVM.IsActive;
                        //     await _dbContext.LiteratureTags.AddAsync(task);
                        //     _dbContext.Entry(task).State = EntityState.Modified;
                        //     await _dbContext.SaveChangesAsync();
                        //     transation.Commit();
                        // );
                        // }
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }

        #endregion

        #region Save Legallegislation Type 
        public async Task SavelegislationType(LegalLegislationType legislationType)
        {


            try
            {
                var task = await _dbContext.legalLegislationTypes.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                if (task != null)
                {
                    int Id = GetNextUniqueIdfLegalLegislationType(task.Id);
                    legislationType.Id = Id;
                    await _dbContext.legalLegislationTypes.AddAsync(legislationType);
                    await _dbContext.SaveChangesAsync();
                    LookupsHistory lookups = new LookupsHistory
                    {
                        LookupsId = legislationType.Id,
                        NameEn = legislationType.Name_En,
                        NameAr = legislationType.Name_Ar,
                        LookupsTableId = (int)LookupsTablesEnum.LEGAL_LEGISLATION_TYPE,
                        CreatedDate = legislationType.CreatedDate,
                        CreatedBy = legislationType.CreatedBy,
                        StatusId = (int)LookupHistoryEnums.Added,
                        IsActive = legislationType.IsActive,
                    };

                    await SaveLookupsHistroy(lookups, null);
                }
                else
                {
                    int counter = 0;
                    ++counter;
                    legislationType.Id = counter;
                    await _dbContext.legalLegislationTypes.AddAsync(legislationType);
                    await _dbContext.SaveChangesAsync();
                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private int GetNextUniqueIdfLegalLegislationType(int counter)
        {
            if (_dbContext.legalLegislationTypes == null)
            {
                counter = 0;
                return ++counter;
            }
            else
            {
                return ++counter;
            }

        }
        #endregion


        #region Update Legallegislation Type 
        public async Task UpdatelegislationType(LegalLegislationType legislationType)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.legalLegislationTypes.Where(x => x.Id == legislationType.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = legislationType.Id;
                            task.Name_En = legislationType.Name_En;
                            task.Name_Ar = legislationType.Name_Ar;
                            task.ModifiedBy = legislationType.ModifiedBy;
                            task.ModifiedDate = legislationType.ModifiedDate;
                            task.IsDeleted = legislationType.IsDeleted;
                            task.IsActive = legislationType.IsActive;
                            await _dbContext.legalLegislationTypes.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = legislationType.Id,
                                NameEn = legislationType.Name_En,
                                NameAr = legislationType.Name_Ar,
                                LookupsTableId = (int)LookupsTablesEnum.LEGAL_LEGISLATION_TYPE,
                                ModifiedBy = legislationType.ModifiedBy,
                                ModifiedDate = legislationType.ModifiedDate,
                                CreatedBy = legislationType.ModifiedBy,
                                CreatedDate = (DateTime)legislationType.ModifiedDate,
                                IsActive = legislationType.IsActive,
                                StatusId = (int)LookupHistoryEnums.Updated,
                            };

                            await SaveLookupsHistroy(lookups, legislationType.ModifiedDate);
                            transation.Commit();
                        }
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Active Legallegislation Type 
        public async Task ActivelegislationType(LegallegislationtypesVM legislationType)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.legalLegislationTypes.Where(x => x.Id == legislationType.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = legislationType.Id;
                            task.ModifiedBy = legislationType.ModifiedBy;
                            task.ModifiedDate = legislationType.ModifiedDate;
                            task.IsActive = legislationType.IsActive;
                            await _dbContext.legalLegislationTypes.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = legislationType.Id,
                                LookupsTableId = (int)LookupsTablesEnum.LEGAL_LEGISLATION_TYPE,
                                NameEn = legislationType.Name_En,
                                NameAr = legislationType.Name_Ar,
                                ModifiedBy = legislationType.ModifiedBy,
                                ModifiedDate = legislationType.ModifiedDate,
                                CreatedBy = legislationType.CreatedBy,
                                CreatedDate = legislationType.CreatedDate,
                                StatusId = (int)LookupHistoryEnums.Updated,
                                IsActive = legislationType.IsActive,
                            };
                            await ActiveLookupsHistroy(lookups, legislationType.ModifiedDate);
                            transation.Commit();
                        }
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Delete Legallegislation Type 
        public async Task DeletelegislationType(LegallegislationtypesVM legislationType)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.legalLegislationTypes.Where(x => x.Id == legislationType.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.IsDeleted = legislationType.IsDeleted;
                            task.DeletedBy = legislationType.DeletedBy;
                            task.DeletedDate = legislationType.DeletedDate;
                            await _dbContext.legalLegislationTypes.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        /////

        #region Save legalPrinciple Type 
        public async Task SavelegalPrincipleTypes(LegalPrincipleType legalPrincipleTypes)
        {
            try
            {
                var task = await _dbContext.legalPrincipleTypes.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                if (task != null)
                {
                    int id = GetNextUniqueIdlegalprincipletype(task.Id);
                    legalPrincipleTypes.Id = id;
                    await _dbContext.legalPrincipleTypes.AddAsync(legalPrincipleTypes);
                    await _dbContext.SaveChangesAsync();
                    LookupsHistory lookups = new LookupsHistory
                    {
                        LookupsId = legalPrincipleTypes.Id,
                        NameEn = legalPrincipleTypes.Name_En,
                        NameAr = legalPrincipleTypes.Name_Ar,
                        LookupsTableId = (int)LookupsTablesEnum.LEGAL_PRINCIPLE_TYPE,
                        CreatedDate = legalPrincipleTypes.CreatedDate,
                        CreatedBy = legalPrincipleTypes.CreatedBy,
                        StatusId = (int)LookupHistoryEnums.Added,
                        IsActive = false,
                    };

                    await SaveLookupsHistroy(lookups, null);
                }
                else
                {
                    int counter = 0;
                    ++counter;
                    legalPrincipleTypes.Id = counter;
                    await _dbContext.legalPrincipleTypes.AddAsync(legalPrincipleTypes);
                    await _dbContext.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #region Get Unique id 
        private int GetNextUniqueIdlegalprincipletype(int counter)
        {
            if (_dbContext.legalPrincipleTypes == null)
            {
                counter = 0;
                return ++counter;
            }
            else
            {
                return ++counter;
            }

        }
        #endregion
        #endregion

        #region Update legalPrinciple Type 
        public async Task UpdatelegalPrincipleTypes(LegalPrincipleType legalPrincipleTypes)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.legalPrincipleTypes.Where(x => x.Id == legalPrincipleTypes.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = legalPrincipleTypes.Id;
                            task.Name_En = legalPrincipleTypes.Name_En;
                            task.Name_Ar = legalPrincipleTypes.Name_Ar;
                            task.ModifiedBy = legalPrincipleTypes.ModifiedBy;
                            task.ModifiedDate = legalPrincipleTypes.ModifiedDate;
                            task.IsDeleted = legalPrincipleTypes.IsDeleted;
                            task.IsActive = legalPrincipleTypes.IsActive;
                            await _dbContext.legalPrincipleTypes.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = legalPrincipleTypes.Id,
                                NameEn = legalPrincipleTypes.Name_En,
                                NameAr = legalPrincipleTypes.Name_Ar,
                                LookupsTableId = (int)LookupsTablesEnum.LEGAL_PRINCIPLE_TYPE,
                                ModifiedBy = legalPrincipleTypes.ModifiedBy,
                                ModifiedDate = legalPrincipleTypes.ModifiedDate,
                                CreatedBy = legalPrincipleTypes.ModifiedBy,
                                CreatedDate = (DateTime)legalPrincipleTypes.ModifiedDate,
                                StatusId = (int)LookupHistoryEnums.Updated,
                                IsActive = legalPrincipleTypes.IsActive,

                            };

                            await SaveLookupsHistroy(lookups, legalPrincipleTypes.ModifiedDate);
                            transation.Commit();
                        }
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Delete legalPrinciple Type 
        public async Task DeletelegalPrincipleTypes(LegalPrincipleTypeVM legalPrincipleTypes)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.legalPrincipleTypes.Where(x => x.Id == legalPrincipleTypes.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.DeletedBy = legalPrincipleTypes.DeletedBy;
                            task.DeletedDate = legalPrincipleTypes.DeletedDate;
                            task.IsDeleted = true;
                            await _dbContext.legalPrincipleTypes.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        public async Task ActivelegalPrincipleTypes(LegallegislationtypesVM legalPrincipleTypes)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.legalPrincipleTypes.Where(x => x.Id == legalPrincipleTypes.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = legalPrincipleTypes.Id;
                            task.ModifiedBy = legalPrincipleTypes.ModifiedBy;
                            task.ModifiedDate = legalPrincipleTypes.ModifiedDate;
                            task.Name_Ar = legalPrincipleTypes.Name_Ar;
                            task.Name_En = legalPrincipleTypes.Name_En;
                            task.IsActive = legalPrincipleTypes.IsActive;
                            await _dbContext.legalPrincipleTypes.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = legalPrincipleTypes.Id,
                                LookupsTableId = (int)LookupsTablesEnum.LEGAL_PRINCIPLE_TYPE,
                                ModifiedBy = legalPrincipleTypes.ModifiedBy,
                                ModifiedDate = legalPrincipleTypes.ModifiedDate,
                                NameEn = legalPrincipleTypes.Name_En,
                                NameAr = legalPrincipleTypes.Name_Ar,
                                CreatedBy = legalPrincipleTypes.CreatedBy,
                                CreatedDate = legalPrincipleTypes.CreatedDate,
                                IsActive = legalPrincipleTypes.IsActive,
                                StatusId = (int)LookupHistoryEnums.Updated,
                            };
                            await ActiveLookupsHistroy(lookups, legalPrincipleTypes.ModifiedDate);
                            transation.Commit();


                        }
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        public async Task ActiveDocumentTypes(AttachmentTypeVM attachmentTypeVM)
        {
            try
            {
                var task = await _dmsDbContext.AttachmentType.Where(x => x.AttachmentTypeId == attachmentTypeVM.AttachmentTypeId).FirstOrDefaultAsync();
                if (task != null)
                {
                    task.ModifiedBy = attachmentTypeVM.ModifiedBy;
                    task.ModifiedDate = attachmentTypeVM.ModifiedDate;
                    task.IsActive = attachmentTypeVM.IsActive;

                    _dmsDbContext.AttachmentType.Update(task);
                    await _dmsDbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #region Save legalPublication Source Names 
        public async Task SavelegalPublicationSourceNames(LegalPublicationSourceName legalPublicationSourceNames)
        {


            try
            {
                await _dbContext.legalPublicationSourceNames.AddAsync(legalPublicationSourceNames);
                await _dbContext.SaveChangesAsync();
                LookupsHistory lookups = new LookupsHistory
                {
                    LookupsId = legalPublicationSourceNames.PublicationNameId,
                    NameEn = legalPublicationSourceNames.Name_En,
                    NameAr = legalPublicationSourceNames.Name_Ar,
                    LookupsTableId = (int)LookupsTablesEnum.LEGAL_PUBLICATION_SOURCE_NAME,
                    CreatedDate = legalPublicationSourceNames.CreatedDate,
                    CreatedBy = legalPublicationSourceNames.CreatedBy,
                    StatusId = (int)LookupHistoryEnums.Added,
                };

                await SaveLookupsHistroy(lookups, null);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Add New Company
        public async Task<bool> AddNewCompany(Company args)
        {
            try
            {
                await _dbContext.AddAsync(args);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Add New Designation
        public async Task<bool> AddNewDesignation(Designation args)
        {
            try
            {
                await _dbContext.AddAsync(args);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #region Update legalPublication Source Names
        public async Task UpdatelegalPublicationSourceName(LegalPublicationSourceName legalPublicationSourceNames)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.legalPublicationSourceNames.Where(x => x.PublicationNameId == legalPublicationSourceNames.PublicationNameId).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.PublicationNameId = legalPublicationSourceNames.PublicationNameId;
                            task.Name_En = legalPublicationSourceNames.Name_En;
                            task.Name_Ar = legalPublicationSourceNames.Name_Ar;
                            task.ModifiedBy = legalPublicationSourceNames.ModifiedBy;
                            task.ModifiedDate = legalPublicationSourceNames.ModifiedDate;
                            task.IsDeleted = legalPublicationSourceNames.IsDeleted;
                            task.IsActive = legalPublicationSourceNames.IsActive;
                            await _dbContext.legalPublicationSourceNames.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = legalPublicationSourceNames.PublicationNameId,
                                NameEn = legalPublicationSourceNames.Name_En,
                                NameAr = legalPublicationSourceNames.Name_Ar,
                                LookupsTableId = (int)LookupsTablesEnum.LEGAL_PUBLICATION_SOURCE_NAME,
                                ModifiedBy = legalPublicationSourceNames.ModifiedBy,
                                ModifiedDate = legalPublicationSourceNames.ModifiedDate,
                                CreatedBy = legalPublicationSourceNames.ModifiedBy,
                                CreatedDate = (DateTime)legalPublicationSourceNames.ModifiedDate,
                                StatusId = (int)LookupHistoryEnums.Updated,
                                IsActive = legalPublicationSourceNames.IsActive,
                            };
                            await SaveLookupsHistroy(lookups, legalPublicationSourceNames.ModifiedDate);
                            transation.Commit();
                        }
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Delete legal Publication Source Names
        public async Task DeletelegalPublicationSourceNames(LegalPublicationSourceNameVM legalPublicationSourceName)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.legalPublicationSourceNames.Where(x => x.PublicationNameId == legalPublicationSourceName.PublicationNameId).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.DeletedBy = legalPublicationSourceName.DeletedBy;
                            task.ModifiedDate = legalPublicationSourceName.DeletedDate;
                            task.IsDeleted = legalPublicationSourceName.IsDeleted;
                            task.IsActive = legalPublicationSourceName.IsActive;
                            await _dbContext.legalPublicationSourceNames.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Get Legal principle types by Id 
        public async Task<LegalPrincipleType> GetLegalPrincipleTypesById(int Id)
        {
            try
            {
                var result = await _dbContext.legalPrincipleTypes.Where(x => x.Id == Id).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Get Legal Publication Source Name 
        public async Task<List<LegalPublicationSourceNameVM>> GetLegalPublicationSourceName()
        {
            try
            {
                string StoredProc = $"exec pLegalPublicationSourceName";
                var result = await _dbContext.LegalPublicationSourceNameVMs.FromSqlRaw(StoredProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Legal Publication Source Name By ID
        public async Task<LegalPublicationSourceName> GetLegalPublicationSourceNameById(int PublicationNameId)
        {
            try
            {
                var result = await _dbContext.LegalPublicationSourceNames.Where(x => x.PublicationNameId == PublicationNameId).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task ActivePublicationSourceNames(LegalPublicationSourceNameVM legalPublicationSourceNames)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.legalPublicationSourceNames.Where(x => x.PublicationNameId == legalPublicationSourceNames.PublicationNameId).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.PublicationNameId = legalPublicationSourceNames.PublicationNameId;
                            task.Name_En = legalPublicationSourceNames.Name_En;
                            task.Name_Ar = legalPublicationSourceNames.Name_Ar;
                            task.ModifiedBy = legalPublicationSourceNames.ModifiedBy;
                            task.ModifiedDate = legalPublicationSourceNames.ModifiedDate;
                            task.IsDeleted = legalPublicationSourceNames.IsDeleted;
                            task.IsActive = legalPublicationSourceNames.IsActive;
                            await _dbContext.legalPublicationSourceNames.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = legalPublicationSourceNames.PublicationNameId,
                                LookupsTableId = (int)LookupsTablesEnum.LEGAL_PUBLICATION_SOURCE_NAME,
                                NameEn = legalPublicationSourceNames.Name_En,
                                NameAr = legalPublicationSourceNames.Name_Ar,
                                ModifiedBy = legalPublicationSourceNames.ModifiedBy,
                                ModifiedDate = legalPublicationSourceNames.ModifiedDate,
                                CreatedBy = legalPublicationSourceNames.CreatedBy,
                                CreatedDate = legalPublicationSourceNames.CreatedDate,
                                IsActive = legalPublicationSourceNames.IsActive,
                                StatusId = (int)LookupHistoryEnums.Updated,
                            };
                            await ActiveLookupsHistroy(lookups, legalPublicationSourceNames.ModifiedDate);
                            transation.Commit();
                        }
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }


        #endregion

        #region Save Case File Number Pattren

        private async Task AddPatternNumberPattrenGroups(IEnumerable<Guid> usersGroupIds, Guid id, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in usersGroupIds)
                {
                    var res = await dbContext.Groups.Where(x => x.GroupId == item).FirstOrDefaultAsync();
                    if (res != null)
                    {
                        CmsComsNumPatternGroups? Obj = new CmsComsNumPatternGroups();
                        Obj.GroupId = item;
                        Obj.NumPattrenId = id;
                        await dbContext.CmsComsNumPatternGroups.AddAsync(Obj);
                        await dbContext.SaveChangesAsync();
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion
        #region Update Case File Number Pattren
        private async Task UpdatePatternNumberPattrensGroups(IEnumerable<Guid> usersGroupIds, Guid id, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in usersGroupIds)
                {
                    var res = await dbContext.Groups.Where(x => x.GroupId == item).FirstOrDefaultAsync();
                    if (res != null)
                    {
                        CmsComsNumPatternGroups? Obj = new CmsComsNumPatternGroups();
                        Obj.GroupId = item;
                        Obj.NumPattrenId = id;
                        await _dbContext.CmsComsNumPatternGroups.AddAsync(Obj);
                        _dbContext.Entry(Obj).State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task UpdatePatternNumberPattrenGroups(IEnumerable<Guid> usersGroupIds, Guid id, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in usersGroupIds)
                {
                    var res = await dbContext.Groups.Where(x => x.GroupId == item).FirstOrDefaultAsync();
                    if (res != null)
                    {
                        CmsComsNumPatternGroups? Obj = new CmsComsNumPatternGroups();
                        Obj.GroupId = item;
                        Obj.NumPattrenId = id;
                        //await _dbContext.CmsComsNumPatternGroups.AddAsync(Obj);

                        //dbContext.CmsComsNumPatternGroups.Remove(Obj);
                        //await dbContext.SaveChangesAsync();
                        await _dbContext.CmsComsNumPatternGroups.AddAsync(Obj);
                        await _dbContext.SaveChangesAsync();
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task RemovePatternNumberPattrenGroups(IEnumerable<Guid> usersGroupIds, Guid id, DatabaseContext dbContext)
        {
            try
            {


                //Foreach (var item in res)
                //{
                //await _dbContext.CmsComsNumPatternGroups.AddAsync(Obj);

                //dbContext.CmsComsNumPatternGroups.Remove(Obj);
                //await dbContext.SaveChangesAsync();
                //await dbContext.SaveChangesAsync();
                dbContext.CmsComsNumPatternGroups.RemoveRange(await dbContext.CmsComsNumPatternGroups.Where(x => x.NumPattrenId == id).ToListAsync());
                //}
                await dbContext.SaveChangesAsync();
                //}

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #region  Get Communication Response Detail By Id

        public async Task<TimeIntervalDetailVM> GetCommunicationResponseDetailByid(int Id)
        {
            try
            {
                if (_TimeIntervalDetailVM == null)
                {

                    string StoredProc = $"exec pGetCommunicationResponseDetailByid @ReminderId = '{Id}'";
                    _TimeIntervalDetailVM = await _dbContext.TimeIntervalDetailVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _TimeIntervalDetailVM.FirstOrDefault();

            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion


        #region get courts
        public async Task<List<WSCommCommunicationTypes>> GetCommunicationType()
        {
            try
            {
                return await _dbContext.WsCommunicationTypes.OrderBy(u => u.CommunicationTypeId).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region SubType Lookup 
        #region Get SubType List 

        public async Task<List<SubTypeVM>> GetSubTypeList(int RequestTypeId)
        {
            string storedProc;
            try
            {
                storedProc = $"exec pCmssubtypeG2Glkplist  @Requesttypeid={RequestTypeId}";

                var result = await _dbContext.SubTypeVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        #endregion
        #region
        public async Task<List<RequestTypeVM>> GetRequestTypeList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pCmsRequesttypeG2Glkplist";

                var result = await _dbContext.RequestTypeVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        #endregion

        #region Get Request by Id 
        public async Task<RequestType> GetRequestTypeById(int Id)
        {
            try
            {
                var result = await _dbContext.RequestTypes.Where(x => x.Id == Id).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion


        #region Update subtype
        public async Task<RequestType> UpdateRequestType(RequestType subtype)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.RequestTypes.Where(x => x.Id == subtype.Id).FirstOrDefaultAsync();
                        var existingRequestType = await _dbContext.Subtype.FindAsync(subtype.Id);

                        if (task != null)
                        {
                            task.Id = subtype.Id;
                            task.Name_En = subtype.Name_En;
                            task.Name_Ar = subtype.Name_Ar;
                            task.ModifiedBy = subtype.ModifiedBy;
                            task.ModifiedDate = subtype.ModifiedDate;
                            task.IsDeleted = subtype.IsDeleted;
                            task.IsActive = subtype.IsActive;

                            await _dbContext.RequestTypes.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = subtype.Id,
                                NameEn = subtype.Name_En,
                                NameAr = subtype.Name_Ar,
                                LookupsTableId = (int)LookupsTablesEnum.CMS_REQUEST_TYPE_G2G_LKP,
                                ModifiedBy = subtype.ModifiedBy,
                                ModifiedDate = subtype.ModifiedDate,
                                CreatedBy = subtype.ModifiedBy,
                                CreatedDate = (DateTime)subtype.ModifiedDate,
                                StatusId = (int)LookupHistoryEnums.Updated,
                            };
                            await SaveLookupsHistroy(lookups, subtype.ModifiedDate);
                            transation.Commit();
                        }
                        else if (existingRequestType != null)
                        {
                            existingRequestType.Id = subtype.Id;
                            existingRequestType.Name_En = subtype.Name_En;
                            existingRequestType.Name_Ar = subtype.Name_Ar;
                            existingRequestType.ModifiedBy = subtype.ModifiedBy;
                            existingRequestType.ModifiedDate = subtype.ModifiedDate;
                            existingRequestType.IsDeleted = subtype.IsDeleted;
                            existingRequestType.IsActive = subtype.IsActive;

                            await _dbContext.Subtype.AddAsync(existingRequestType);
                            _dbContext.Entry(existingRequestType).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return subtype;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Get SubType by Id 
        public async Task<Subtype> GetSubTypeById(int Id)
        {
            try
            {
                var result = await _dbContext.Subtype.Where(x => x.Id == Id).FirstOrDefaultAsync();

                return result;


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #region Save SubType

        public async Task<Subtype> SaveSubType(Subtype subtype)
        {
            try
            {
                await _dbContext.Subtype.AddAsync(subtype);
                await _dbContext.SaveChangesAsync();
                LookupsHistory lookups = new LookupsHistory
                {
                    LookupsId = subtype.Id,
                    NameEn = subtype.Name_En,
                    NameAr = subtype.Name_Ar,
                    LookupsTableId = (int)LookupsTablesEnum.CMS_SUBTYPE_G2G_LKP,
                    ModifiedBy = subtype.ModifiedBy,
                    ModifiedDate = subtype.ModifiedDate,
                    CreatedBy = subtype.CreatedBy,
                    CreatedDate = subtype.CreatedDate,
                };
                await SaveLookupsHistroy(lookups, null);
                return subtype;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        #region Update subtype
        public async Task<Subtype> UpdateSubType(Subtype subtype)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.Subtype.Where(x => x.Id == subtype.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = subtype.Id;
                            task.Name_En = subtype.Name_En;
                            task.Name_Ar = subtype.Name_Ar;
                            task.ModifiedBy = subtype.ModifiedBy;
                            task.ModifiedDate = subtype.ModifiedDate;
                            task.IsDeleted = subtype.IsDeleted;
                            task.IsActive = subtype.IsActive;

                            await _dbContext.Subtype.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = subtype.Id,
                                NameEn = subtype.Name_En,
                                NameAr = subtype.Name_Ar,
                                LookupsTableId = (int)LookupsTablesEnum.CMS_SUBTYPE_G2G_LKP,
                                ModifiedBy = subtype.ModifiedBy,
                                ModifiedDate = subtype.ModifiedDate,
                                CreatedBy = subtype.CreatedBy,
                                CreatedDate = subtype.CreatedDate,
                                StatusId = (int)LookupHistoryEnums.Updated,
                            };
                            await SaveLookupsHistroy(lookups, subtype.ModifiedDate);
                            transation.Commit();
                        }
                        return subtype;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Delete SubType
        public async Task<SubTypeVM> DeleteSubtype(SubTypeVM subTypeVM)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.Subtype.Where(x => x.Id == subTypeVM.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.DeletedBy = subTypeVM.DeletedBy;
                            task.DeletedDate = subTypeVM.DeletedDate;
                            task.IsDeleted = true;
                            await _dbContext.Subtype.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return subTypeVM;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Active status of Subtype
        public async Task<SubTypeVM> ActiveSubType(SubTypeVM SubTypeVM)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.Subtype.Where(x => x.Id == SubTypeVM.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = SubTypeVM.Id;
                            task.Name_En = SubTypeVM.Name_En;
                            task.Name_Ar = SubTypeVM.Name_Ar;
                            task.ModifiedBy = SubTypeVM.ModifiedBy;
                            task.ModifiedDate = SubTypeVM.ModifiedDate;
                            task.IsDeleted = (bool)SubTypeVM.IsDeleted;
                            task.IsActive = SubTypeVM.IsActive;
                            await _dbContext.Subtype.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return SubTypeVM;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region ConsultationLegislationFileType Lookup 
        #region Get ConsultationLegislationFileType List 

        public async Task<List<ConsultationLegislationFileTypeVM>> GetConsultationLegislationFileTypeList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pComsconsultationlegislationfilEtypelist";

                var result = await _dbContext.ConsultationLegislationFileTypeVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }


        #endregion
        #region Get ConsultationLegislationFileType by Id 
        public async Task<ConsultationLegislationFileType> GetConsultationLegislationFileTypeById(int Id)
        {
            try
            {
                var result = await _dbContext.ConsultationLegislationFileTypes.Where(x => x.Id == Id).FirstOrDefaultAsync();

                return result;


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #region Save ConsultationLegislation FileType

        public async Task<ConsultationLegislationFileType> SaveConsultationLegislationFileType(ConsultationLegislationFileType ConsultationLegislationFileType)
        {
            try
            {
                await _dbContext.ConsultationLegislationFileTypes.AddAsync(ConsultationLegislationFileType);
                await _dbContext.SaveChangesAsync();
                LookupsHistory lookups = new LookupsHistory
                {
                    LookupsId = ConsultationLegislationFileType.Id,
                    NameEn = ConsultationLegislationFileType.Name_En,
                    NameAr = ConsultationLegislationFileType.Name_Ar,
                    LookupsTableId = (int)LookupsTablesEnum.COMS_CONSULTATION_Legislation_FILE_TYPE_G2G_LKP,
                    CreatedDate = ConsultationLegislationFileType.CreatedDate,
                    CreatedBy = ConsultationLegislationFileType.CreatedBy,
                    StatusId = (int)LookupHistoryEnums.Added,
                };

                await SaveLookupsHistroy(lookups, null);
                return ConsultationLegislationFileType;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        #region Update ConsultationLegislationFileType
        public async Task<ConsultationLegislationFileType> UpdateConsultationLegislationFileType(ConsultationLegislationFileType ConsultationLegislationFileType)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.ConsultationLegislationFileTypes.Where(x => x.Id == ConsultationLegislationFileType.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = ConsultationLegislationFileType.Id;
                            task.Name_En = ConsultationLegislationFileType.Name_En;
                            task.Name_Ar = ConsultationLegislationFileType.Name_Ar;
                            task.ModifiedBy = ConsultationLegislationFileType.ModifiedBy;
                            task.ModifiedDate = ConsultationLegislationFileType.ModifiedDate;
                            task.IsDeleted = ConsultationLegislationFileType.IsDeleted;
                            task.IsActive = ConsultationLegislationFileType.IsActive;

                            await _dbContext.ConsultationLegislationFileTypes.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = ConsultationLegislationFileType.Id,
                                NameEn = ConsultationLegislationFileType.Name_En,
                                NameAr = ConsultationLegislationFileType.Name_Ar,
                                LookupsTableId = (int)LookupsTablesEnum.COMS_CONSULTATION_Legislation_FILE_TYPE_G2G_LKP,
                                ModifiedBy = ConsultationLegislationFileType.ModifiedBy,
                                ModifiedDate = ConsultationLegislationFileType.ModifiedDate,
                                CreatedBy = ConsultationLegislationFileType.ModifiedBy,
                                CreatedDate = (DateTime)ConsultationLegislationFileType.ModifiedDate,
                                StatusId = (int)LookupHistoryEnums.Updated,
                            };

                            await SaveLookupsHistroy(lookups, ConsultationLegislationFileType.ModifiedDate);
                            transation.Commit();
                        }
                        return ConsultationLegislationFileType;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Delete ConsultationLegislationFileType
        public async Task<ConsultationLegislationFileTypeVM> DeleteConsultationLegislationFileType(ConsultationLegislationFileTypeVM ConsultationLegislationFileTypeVM)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.ConsultationLegislationFileTypes.Where(x => x.Id == ConsultationLegislationFileTypeVM.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.DeletedBy = ConsultationLegislationFileTypeVM.DeletedBy;
                            task.DeletedDate = ConsultationLegislationFileTypeVM.DeletedDate;
                            task.IsDeleted = true;
                            await _dbContext.ConsultationLegislationFileTypes.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return ConsultationLegislationFileTypeVM;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Active status of ConsultationLegislationFileType
        public async Task<ConsultationLegislationFileTypeVM> ActiveConsultationLegislationFileType(ConsultationLegislationFileTypeVM ConsultationLegislationFileTypeVM)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.ConsultationLegislationFileTypes.Where(x => x.Id == ConsultationLegislationFileTypeVM.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = ConsultationLegislationFileTypeVM.Id;
                            task.Name_En = ConsultationLegislationFileTypeVM.Name_En;
                            task.Name_Ar = ConsultationLegislationFileTypeVM.Name_Ar;
                            task.ModifiedBy = ConsultationLegislationFileTypeVM.ModifiedBy;
                            task.ModifiedDate = ConsultationLegislationFileTypeVM.ModifiedDate;
                            task.IsDeleted = (bool)ConsultationLegislationFileTypeVM.IsDeleted;
                            task.IsActive = ConsultationLegislationFileTypeVM.IsActive;
                            await _dbContext.ConsultationLegislationFileTypes.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = ConsultationLegislationFileTypeVM.Id,
                                LookupsTableId = (int)LookupsTablesEnum.COMS_CONSULTATION_Legislation_FILE_TYPE_G2G_LKP,
                                NameEn = ConsultationLegislationFileTypeVM.Name_En,
                                NameAr = ConsultationLegislationFileTypeVM.Name_Ar,
                                ModifiedBy = ConsultationLegislationFileTypeVM.ModifiedBy,
                                ModifiedDate = ConsultationLegislationFileTypeVM.ModifiedDate,
                                CreatedBy = ConsultationLegislationFileTypeVM.CreatedBy,
                                CreatedDate = (DateTime)ConsultationLegislationFileTypeVM.CreatedDate,
                                IsActive = ConsultationLegislationFileTypeVM.IsActive,
                                StatusId = (int)LookupHistoryEnums.Updated,
                            };
                            await ActiveLookupsHistroy(lookups, ConsultationLegislationFileTypeVM.ModifiedDate);
                            transation.Commit();
                        }
                        return ConsultationLegislationFileTypeVM;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #endregion

        #region Delete ComsInternationalArbitrationType
        public async Task<ComsInternationalArbitrationTypeVM> DeleteComsInternationalArbitrationType(ComsInternationalArbitrationTypeVM ConsultationLegislationFileTypeVM)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.ComsInternationalArbitrationTypes.Where(x => x.Id == ConsultationLegislationFileTypeVM.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.DeletedBy = ConsultationLegislationFileTypeVM.DeletedBy;
                            task.DeletedDate = ConsultationLegislationFileTypeVM.DeletedDate;
                            task.IsDeleted = true;
                            await _dbContext.ComsInternationalArbitrationTypes.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return ConsultationLegislationFileTypeVM;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Active status of ComsInternationalArbitrationType
        public async Task<ComsInternationalArbitrationTypeVM> ActiveComsInternationalArbitrationType(ComsInternationalArbitrationTypeVM ConsultationLegislationFileTypeVM)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.ComsInternationalArbitrationTypes.Where(x => x.Id == ConsultationLegislationFileTypeVM.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = ConsultationLegislationFileTypeVM.Id;
                            task.NameEn = ConsultationLegislationFileTypeVM.NameEn;
                            task.NameAr = ConsultationLegislationFileTypeVM.NameAr;
                            task.ModifiedBy = ConsultationLegislationFileTypeVM.ModifiedBy;
                            task.ModifiedDate = ConsultationLegislationFileTypeVM.ModifiedDate;
                            task.IsDeleted = (bool)ConsultationLegislationFileTypeVM.IsDeleted;
                            task.IsActive = ConsultationLegislationFileTypeVM.IsActive;
                            await _dbContext.ComsInternationalArbitrationTypes.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = ConsultationLegislationFileTypeVM.Id,
                                LookupsTableId = (int)LookupsTablesEnum.COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_G2G_LKP,
                                NameAr = ConsultationLegislationFileTypeVM.NameAr,
                                NameEn = ConsultationLegislationFileTypeVM.NameEn,
                                ModifiedBy = ConsultationLegislationFileTypeVM.ModifiedBy,
                                ModifiedDate = ConsultationLegislationFileTypeVM.ModifiedDate,
                                CreatedBy = ConsultationLegislationFileTypeVM.CreatedBy,
                                CreatedDate = (DateTime)ConsultationLegislationFileTypeVM.CreatedDate,
                                IsActive = ConsultationLegislationFileTypeVM.IsActive,
                                StatusId = (int)LookupHistoryEnums.Updated,
                            };
                            await ActiveLookupsHistroy(lookups, ConsultationLegislationFileTypeVM.ModifiedDate);
                            transation.Commit();
                        }
                        return ConsultationLegislationFileTypeVM;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion


        #region ConsultationLegislationFileType Lookup 
        #region Get ComsInternationalArbitrationType List 

        public async Task<List<ComsInternationalArbitrationTypeVM>> GetComsInternationalArbitrationTypeList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pCOMSCONSULTATIONINTERNATIONALARBITRATIONTYPEFTWlist";

                var result = await _dbContext.ComsInternationalArbitrationTypeVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }


        #endregion
        #region Get ComsInternationalArbitrationType by Id 
        public async Task<ComsInternationalArbitrationType> GetComsInternationalArbitrationTypeById(int Id)
        {
            try
            {
                var result = await _dbContext.ComsInternationalArbitrationTypes.Where(x => x.Id == Id).FirstOrDefaultAsync();

                return result;


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #region Save ConsultationLegislation File Type

        public async Task<ComsInternationalArbitrationType> SaveComsInternationalArbitrationType(ComsInternationalArbitrationType ComsInternationalArbitrationType)
        {
            try
            {
                //int id = 0;
                //var LatestID = await _dbContext.ComsInternationalArbitrationTypes.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                //id = LatestID.Id;
                //id++;

                //ComsInternationalArbitrationType.Id = id;
                await _dbContext.ComsInternationalArbitrationTypes.AddAsync(ComsInternationalArbitrationType);
                await _dbContext.SaveChangesAsync();
                LookupsHistory lookups = new LookupsHistory
                {
                    LookupsId = ComsInternationalArbitrationType.Id,
                    NameEn = ComsInternationalArbitrationType.NameEn,
                    NameAr = ComsInternationalArbitrationType.NameAr,
                    LookupsTableId = (int)LookupsTablesEnum.COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_G2G_LKP,
                    CreatedDate = ComsInternationalArbitrationType.CreatedDate,
                    CreatedBy = ComsInternationalArbitrationType.CreatedBy,
                    StatusId = (int)LookupHistoryEnums.Added,
                };

                await SaveLookupsHistroy(lookups, null);
                return ComsInternationalArbitrationType;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        #endregion
        #region Update ComsInternationalArbitrationType
        public async Task<ComsInternationalArbitrationType> UpdateComsInternationalArbitrationType(ComsInternationalArbitrationType ComsInternationalArbitrationType)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.ComsInternationalArbitrationTypes.Where(x => x.Id == ComsInternationalArbitrationType.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = ComsInternationalArbitrationType.Id;
                            task.NameEn = ComsInternationalArbitrationType.NameEn;
                            task.NameAr = ComsInternationalArbitrationType.NameAr;
                            task.ModifiedBy = ComsInternationalArbitrationType.ModifiedBy;
                            task.ModifiedDate = ComsInternationalArbitrationType.ModifiedDate;
                            task.IsDeleted = ComsInternationalArbitrationType.IsDeleted;
                            task.IsActive = ComsInternationalArbitrationType.IsActive;

                            await _dbContext.ComsInternationalArbitrationTypes.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = ComsInternationalArbitrationType.Id,
                                NameEn = ComsInternationalArbitrationType.NameEn,
                                NameAr = ComsInternationalArbitrationType.NameAr,
                                LookupsTableId = (int)LookupsTablesEnum.COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_G2G_LKP,
                                ModifiedBy = ComsInternationalArbitrationType.ModifiedBy,
                                ModifiedDate = ComsInternationalArbitrationType.ModifiedDate,
                                CreatedBy = ComsInternationalArbitrationType.ModifiedBy,
                                CreatedDate = (DateTime)ComsInternationalArbitrationType.ModifiedDate,
                                StatusId = (int)LookupHistoryEnums.Updated,
                            };

                            await SaveLookupsHistroy(lookups, ComsInternationalArbitrationType.ModifiedDate);
                            transation.Commit();
                        }
                        return ComsInternationalArbitrationType;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Save Sub Types

        public async Task<Subtype> SaveSubTypes(Subtype subtype)
        {
            try
            {
                await _dbContext.subtype.AddAsync(subtype);
                await _dbContext.SaveChangesAsync();

                return subtype;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        #region Update Case File Number Pattren History

        //public async Task UpdateCaseFileNumberPattrenHistory(CmsComsNumPatternHistory PatternHistory)
        //{
        //    string storedProc;
        //    using (var dbContext = _dbContext)
        //    {
        //        using (var transation = _dbContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                if (PatternHistory != null)
        //                {
        //                    var task = await _dbContext.CmsComsNumPatternHistories.Where(x => x.Id == PatternHistory.Id).FirstOrDefaultAsync();




        //                    if (task != null)
        //                    {
        //                        task.PatternId = PatternHistory.PatternId;
        //                        task.Year = PatternHistory.Year;
        //                        task.Day = PatternHistory.Day;
        //                        task.Month = PatternHistory.Month;
        //                        task.SequanceNumber = PatternHistory.SequanceNumber;
        //                        task.CharaterString = PatternHistory.CharaterString;
        //                        task.SequanceResult = PatternHistory.SequanceResult;
        //                        task.ModifiedDate = PatternHistory.ModifiedDate;
        //                        task.IsDeleted = PatternHistory.IsDeleted;
        //                        //task.IsActive = PatternHistory.IsActive;
        //                        task.UserId = PatternHistory.UserId;

        //                        await _dbContext.CmsComsNumPatternHistories.AddAsync(task);
        //                        _dbContext.Entry(task).State = EntityState.Modified;
        //                        await _dbContext.SaveChangesAsync();
        //                        transation.Commit();
        //                    }
        //                    else
        //                    {
        //                        task.PatternId = PatternHistory.PatternId;
        //                        task.Year = PatternHistory.Year;
        //                        task.Day = PatternHistory.Day;
        //                        task.Month = PatternHistory.Month;
        //                        task.SequanceNumber = PatternHistory.SequanceNumber;
        //                        task.CharaterString = PatternHistory.CharaterString;
        //                        task.SequanceResult = PatternHistory.SequanceResult;
        //                        task.ModifiedDate = PatternHistory.ModifiedDate;
        //                        task.IsDeleted = PatternHistory.IsDeleted;
        //                        //task.IsActive = PatternHistory.IsActive;
        //                        task.UserId = PatternHistory.UserId;
        //                        await _dbContext.CmsComsNumPatternHistories.AddAsync(task);
        //                        await _dbContext.SaveChangesAsync();
        //                        transation.Commit();
        //                    }
        //                }
        //            }

        //            catch (Exception ex)
        //            {
        //                transation.Rollback();
        //                throw;
        //            }
        //        }
        //    }

        //}
        public async Task<CmsComsNumPatternHistory> UpdateCaseFileNumberPattrenHistory(CmsComsNumPatternHistory PatternHistory)
        {
            using (var dbContext = _dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (PatternHistory != null)
                        {
                            await _dbContext.CmsComsNumPatternHistories.AddAsync(PatternHistory);
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();
                        }
                        return PatternHistory;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }

                }
            }
        }

        #endregion
        #region  Get Case Request Pattren List
        public async Task<List<CmsComsNumPatternHistoryVM>> GetCmsComNumPatternHistoryDetail(Guid Id)
        {
            string storedProc;
            try
            {
                storedProc = $"exec pCmsComsPattrenHistory1 @Id = '{Id}'";

                var result = await _dbContext.CmsComsNumPatternHistoryVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion
        #region Get History  by Id 
        public async Task<List<LookupsHistory>> GetLookupHistoryListByRefernceId(int Id, int lookupsTableId)
        {
            try
            {
                var result = await _dbContext.LookupsHistories
        .Where(x => x.LookupsId == Id && x.LookupsTableId == lookupsTableId)
        .OrderByDescending(y => y.StartDate)
        .Select(history => new
        {
            History = history,
            UserFullNameEn = _dbContext.Users
            .Where(ums => ums.Email == history.CreatedBy)
            .Join(_dbContext.UserPersonalInformation, ums => ums.Id, epi => epi.UserId, (ums, epi) =>
                string.Concat(epi.FirstName_En ?? "", " ", epi.SecondName_En ?? "", " ", epi.LastName_En ?? "")
            )
            .FirstOrDefault(),
            UserFullNameAr = _dbContext.Users
            .Where(ums => ums.Email == history.CreatedBy)
            .Join(_dbContext.UserPersonalInformation, ums => ums.Id, epi => epi.UserId, (ums, epi) =>
                string.Concat(epi.FirstName_Ar ?? "", " ", epi.SecondName_Ar ?? "", " ", epi.LastName_Ar ?? "")
            )
            .FirstOrDefault()
        })
    .ToListAsync();

                return result.Select(r => new LookupsHistory
                {
                    LookupsHistroyId = r.History.LookupsHistroyId,
                    LookupsId = r.History.LookupsId,
                    NameEn = r.History.NameEn,
                    NameAr = r.History.NameAr,
                    LookupsTableId = r.History.LookupsTableId,
                    TagNo = r.History.TagNo,
                    Description = r.History.Description,
                    StartDate = r.History.StartDate,
                    EndDate = r.History.EndDate,
                    StatusId = r.History.StatusId,
                    IsActive = r.History.IsActive,
                    UserFullNameEn = r.UserFullNameEn,
                    UserFullNameAr = r.UserFullNameAr,
                    CreatedBy = r.History.CreatedBy,
                    CreatedDate = r.History.CreatedDate,
                    ModifiedBy = r.History.ModifiedBy,
                    ModifiedDate = r.History.ModifiedDate,
                    DeletedBy = r.History.DeletedBy,
                    DeletedDate = r.History.DeletedDate,
                    IsDeleted = r.History.IsDeleted
                }).ToList();

            }

            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #endregion

        #region Lookups Histroy 
        public async Task<LookupsHistory> SaveLookupsHistroy(LookupsHistory lookupHistory, DateTime? endDate, DatabaseContext dbContext = null)
        {
            try
            {
                DatabaseContext databaseContext = dbContext != null ? dbContext : _dbContext;

                var existingHistory = await databaseContext.LookupsHistories.FirstOrDefaultAsync(x => x.LookupsId == lookupHistory.LookupsId && x.LookupsTableId == lookupHistory.LookupsTableId && x.EndDate == null);

                if (existingHistory != null)
                {
                    existingHistory.EndDate = endDate;

                    databaseContext.Entry(existingHistory).State = EntityState.Modified;
                    await databaseContext.SaveChangesAsync();
                }

                lookupHistory.StartDate = DateTime.Now;
                lookupHistory.EndDate = null;

                lookupHistory.ModifiedDate = null;
                lookupHistory.ModifiedBy = null;
                await databaseContext.LookupsHistories.AddAsync(lookupHistory);
                await databaseContext.SaveChangesAsync();
                return lookupHistory;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<CmsGovtEntityNumPattern> SaveCmsGovtEntityNumPattern(CmsGovtEntityNumPattern cmsGovtEntityNumPattern, DatabaseContext dbContext)
        {
            try
            {
                var existingGovtEntityNumPattern = await dbContext.CmsGovtEntityNumPattern.FirstOrDefaultAsync(x => x.GovtEntityId == cmsGovtEntityNumPattern.GovtEntityId);

                if (existingGovtEntityNumPattern != null)
                {

                    dbContext.Entry(existingGovtEntityNumPattern).State = EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                }

                await dbContext.CmsGovtEntityNumPattern.AddAsync(cmsGovtEntityNumPattern);
                await dbContext.SaveChangesAsync();



                return existingGovtEntityNumPattern;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<LookupsHistory> ActiveLookupsHistroy(LookupsHistory lookupHistory, DateTime? endDate)
        {
            try
            {
                var existingHistory = await _dbContext.LookupsHistories.FirstOrDefaultAsync(x => x.LookupsId == lookupHistory.LookupsId && x.EndDate == null);

                if (existingHistory != null)
                {
                    existingHistory.EndDate = endDate;
                    _dbContext.Entry(existingHistory).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                }
                lookupHistory.StartDate = DateTime.Now;
                lookupHistory.EndDate = null;
                lookupHistory.ModifiedDate = null;
                lookupHistory.ModifiedBy = null;

                await _dbContext.LookupsHistories.AddAsync(lookupHistory);
                await _dbContext.SaveChangesAsync();
                return lookupHistory;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        #endregion
        public async Task<List<GovernmentEntitiesPatternVM>> GetAllAGEUserListPatternAttached(Guid Id, int SelectedPatternTypeId, bool isDefault)
        {
            try
            {
                var res = new List<int?>();
                var govtEntity = new List<GovernmentEntity>();
                // fetch the GovtEntityIDs which have null CMSRequestPatternId OR CMSRequestPatternId
                if (isDefault)
                {
                    res = await _dbContext.CmsGovtEntityNumPattern
                                         .Where(x => (SelectedPatternTypeId == (int)CmsComsNumPatternTypeEnum.CaseRequestNumber && x.CMSRequestPatternId != Guid.Empty) ||
                                                     (SelectedPatternTypeId == (int)CmsComsNumPatternTypeEnum.ConsultationRequestNumber && x.COMSRequestPatternId != Guid.Empty))
                                         .Select(x => x.GovtEntityId)
                                         .ToListAsync();
                    govtEntity = await _dbContext.GovernmentEntity.Where(x => !res.Contains(x.EntityId) && (bool)!x.IsDeleted && (bool)x.IsActive).ToListAsync();
                }
                else
                {
                    res = await _dbContext.CmsGovtEntityNumPattern
                                               .Where(SelectedPatternTypeId == (int)CmsComsNumPatternTypeEnum.CaseRequestNumber ? x => x.CMSRequestPatternId == Id : x => x.COMSRequestPatternId == Id)
                                               .Select(x => x.GovtEntityId)
                                               .ToListAsync();
                    govtEntity = await _dbContext.GovernmentEntity.Where(x => res.Contains(x.EntityId) && (bool)!x.IsDeleted && (bool)x.IsActive).ToListAsync();
                }
                // fetch all those govtEntity which have no pettren assgin
                var result = govtEntity.Select(entity => new GovernmentEntitiesPatternVM
                {
                    EntityId = entity.EntityId,
                    SelectedPatternTypeId = SelectedPatternTypeId,
                    Name_Ar = entity.Name_Ar,
                    Name_En = entity.Name_En
                }).ToList();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CheckPatternAlreadyAttachedGovtid(List<int> EntityIds, int SelectedPatternTypeId)
        {
            try
            {
                if (EntityIds.Any())
                {


                    /*  var isGovtEntityIdExist = await _dbContext.CmsGovtEntityNumPattern
                          .AnyAsync((x => x.GovtEntityId.HasValue && EntityIds
                          .Contains(x.GovtEntityId.Value) && (x.CMSRequestPatternId != Guid.Empty || x.COMSRequestPatternId != Guid.Empty)));*/

                    var govtEntityIds = await _dbContext.CmsGovtEntityNumPattern
                        .Where(x => x.GovtEntityId.HasValue && EntityIds.Contains(x.GovtEntityId.Value) && (x.CMSRequestPatternId != Guid.Empty || x.COMSRequestPatternId != Guid.Empty))
                        .Select(x => SelectedPatternTypeId == (int)CmsComsNumPatternTypeEnum.CaseRequestNumber ? x.CMSRequestPatternId : x.COMSRequestPatternId)
                        .ToListAsync();
                    if (govtEntityIds.Any())
                        return await _dbContext.CmsComsNumPatterns.AnyAsync(x => govtEntityIds.Contains(x.Id) && !x.IsDefault);
                }
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region Get Contact Details For File       
        //<History Author = 'Ijaz Ahmad' Date='2023-01-19' Version="1.0" Branch="master">Get lawyer  Assigment to Court </History>
        public async Task<List<ContactFileLinkVM>> GetContactDetailsForFile(Guid fileId)
        {
            try
            {
                string StoredProc = $"exec pContactDetailsForFileLink @FileId = '{fileId}'";
                var resultContactDetails = await _dbContext.ContactForFileVMs.FromSqlRaw(StoredProc).ToListAsync();
                if (resultContactDetails.Count() != 0)
                {
                    return resultContactDetails;
                }
                return new List<ContactFileLinkVM>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Get Meeting status
        public async Task<List<MeetingAttendeeStatus>> GetAttendeeMeetingStatus()
        {
            try
            {
                return await _dbContext.MeetingAttendeeStatuses.OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region Remove selected contact from file
        public async Task<bool> RemoveContact(ContactFileLinkVM args)
        {
            try
            {
                var cntContact = await _dbContext.CntContactFileLinks.Where(x => x.ContactId == (Guid)args.ContactId && x.ReferenceId == (Guid)args.ReferenceId).FirstOrDefaultAsync();
                if (cntContact != null)
                {
                    _dbContext.CntContactFileLinks.Remove(cntContact);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Add Selected Contact to file
        public async Task<bool> AddSelectedContactToFile(IList<ContactListVM> selectedContact, Guid? fileId, int? fileModule, string CurrentUser)
        {
            try
            {
                if (selectedContact.Count() != 0 && fileId != Guid.Empty && fileModule != 0 && CurrentUser != null)
                {
                    foreach (var item in selectedContact)
                    {
                        CntContactFileLink ObjFile = new CntContactFileLink();
                        ObjFile.ContactId = (Guid)item.ContactId;
                        ObjFile.ReferenceId = (Guid)fileId;
                        ObjFile.ContactLinkId = 1;
                        ObjFile.ModuleId = (int)fileModule;
                        ObjFile.CreatedBy = CurrentUser;
                        ObjFile.CreatedDate = DateTime.Now;
                        await _dbContext.CntContactFileLinks.AddAsync(ObjFile);
                    }
                    await _dbContext.SaveChangesAsync();
                }
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Chamber Number CRUD 
        #region  Get Chambers Number  List
        public async Task<List<ChambersNumberDetailVM>> GetChamberNumberList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pCmsChamberNumberG2GLkpList";

                var result = await _dbContext.ChambersNumberDetailVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion
        #region Delete Chambers Number
        public async Task<ChambersNumberDetailVM> DeleteChambersNumber(ChambersNumberDetailVM chambersNumber)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.ChamberNumbers.Where(x => x.Id == chambersNumber.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.DeletedBy = chambersNumber.DeletedBy;
                            task.DeletedDate = chambersNumber.DeletedDate;
                            task.IsDeleted = true;
                            await _dbContext.ChamberNumbers.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return chambersNumber;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Active status of Chamber 
        public async Task<ChambersNumberDetailVM> ActiveChambersNumber(ChambersNumberDetailVM chambersNumber)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.ChamberNumbers.Where(x => x.Id == chambersNumber.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = chambersNumber.Id;
                            // task.nam = chambersNumber.Name_En;
                            // task.Name_Ar = chambersNumber.Name_Ar;
                            task.ModifiedBy = chambersNumber.ModifiedBy;
                            task.ModifiedDate = chambersNumber.ModifiedDate;
                            task.IsDeleted = (bool)chambersNumber.IsDeleted;
                            task.IsActive = chambersNumber.IsActive;
                            await _dbContext.ChamberNumbers.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return chambersNumber;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Save Chamber Number 
        public async Task<ChamberNumber> SaveChamberNumber(ChamberNumber chambersNumber)
        {
            try
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    await _dbContext.ChamberNumbers.AddAsync(chambersNumber);
                    await _dbContext.SaveChangesAsync();

                    foreach (var chamberId in chambersNumber.ChamberIds)
                    {
                        var chamberChamberNumber = new ChamberChamberNumber
                        {
                            ChamberId = chamberId,
                            ChamberNumberId = chambersNumber.Id,
                            CreatedBy = chambersNumber.CreatedBy,
                            CreatedDate = chambersNumber.CreatedDate,
                        };
                        await _dbContext.ChamberChamberNumbers.AddAsync(chamberChamberNumber);
                    }

                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                    return chambersNumber;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        #region Update Chamber number 
        public async Task<ChamberNumber> UpdateChamberNumber(ChamberNumber chambersNumber)
        {
            using (var dbContext = _dbContext)
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var existingChamberNumber = await dbContext.ChamberNumbers.FirstOrDefaultAsync(x => x.Id == chambersNumber.Id);

                        if (existingChamberNumber != null)
                        {
                            existingChamberNumber.Number = chambersNumber.Number;
                            existingChamberNumber.Code = chambersNumber.Code;
                            existingChamberNumber.ModifiedBy = chambersNumber.ModifiedBy;
                            existingChamberNumber.ModifiedDate = DateTime.Now;
                            existingChamberNumber.IsDeleted = chambersNumber.IsDeleted;
                            existingChamberNumber.ShiftId = chambersNumber.ShiftId;
                            // Save changes to existing ChamberNumber
                            await dbContext.SaveChangesAsync();
                            // Remove existing ChamberChamberNumber entries not present in the updated ChamberNumber
                            var existingChamberIds = await dbContext.ChamberChamberNumbers
                                .Where(ccn => ccn.ChamberNumberId == existingChamberNumber.Id)
                                .Select(ccn => ccn.ChamberId)
                                .ToListAsync();
                            var chamberIdsToRemove = existingChamberIds.Except(chambersNumber.ChamberIds).ToList();
                            foreach (var chamberIdToRemove in chamberIdsToRemove)
                            {
                                var chamberChamberNumberToRemove = await dbContext.ChamberChamberNumbers
                                    .FirstOrDefaultAsync(ccn => ccn.ChamberId == chamberIdToRemove && ccn.ChamberNumberId == existingChamberNumber.Id);

                                if (chamberChamberNumberToRemove != null)
                                {
                                    dbContext.ChamberChamberNumbers.Remove(chamberChamberNumberToRemove);
                                }
                            }
                            // Add new ChamberChamberNumber entries for the updated ChamberNumber
                            foreach (var chamberId in chambersNumber.ChamberIds)
                            {
                                // Check if the ChamberChamberNumber entry already exists
                                var existingChamberChamberNumber = await dbContext.ChamberChamberNumbers.FirstOrDefaultAsync(ccn => ccn.ChamberId == chamberId && ccn.ChamberNumberId == existingChamberNumber.Id);

                                if (existingChamberChamberNumber == null)
                                {
                                    var chamberChamberNumber = new ChamberChamberNumber
                                    {
                                        ChamberId = chamberId,
                                        ChamberNumberId = existingChamberNumber.Id,
                                        CreatedDate = DateTime.Now,
                                        CreatedBy = chambersNumber.ModifiedBy

                                    };
                                    dbContext.ChamberChamberNumbers.Add(chamberChamberNumber);
                                }
                            }
                            // Save changes to ChamberChamberNumbers
                            await dbContext.SaveChangesAsync();
                            // Commit transaction
                            await transaction.CommitAsync();
                            // Return updated ChamberNumber
                            return existingChamberNumber;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction in case of exception
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Get Chamber Number by Id 
        public async Task<ChamberNumber> GetChamberNumberById(int Id)
        {
            try
            {
                //var result = await _dbContext.ChamberNumbers.Where(x => x.Id == Id).FirstOrDefaultAsync();

                //return result;
                var result = await (from cn in _dbContext.ChamberNumbers
                                    join cc in _dbContext.ChamberChamberNumbers on cn.Id equals cc.ChamberNumberId
                                    join c in _dbContext.Chambers on cc.ChamberId equals c.Id
                                    where cn.Id == Id  // Add filter for the provided ID
                                    select new ChamberNumber
                                    {
                                        Id = cn.Id,
                                        Number = cn.Number,
                                        Code = cn.Code,
                                        IsActive = cn.IsActive,
                                        ShiftId = cn.ShiftId,
                                        ChamberIds = _dbContext.ChamberChamberNumbers
                                                        .Where(ccn => ccn.ChamberNumberId == cn.Id)
                                                        .Select(ccn => ccn.ChamberId)
                                                        .ToList(),
                                        // Assuming ChamberNamesEn and ChamberNamesAr are properties in ChamberNumber model
                                        // ChamberNamesEn = c.Name_En,
                                        // ChamberNamesAr = c.Name_Ar
                                    }).FirstOrDefaultAsync();
                if (result == null)
                {
                    result = await _dbContext.ChamberNumbers.Where(x => x.Id == Id).FirstOrDefaultAsync();
                }
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion


        #region Get Chamber Number Detail by ID 
        public async Task<List<ChambersNumberDetailVM>> GetChamberNumberDetailById(int Id)
        {
            try
            {
                if (_chamberNumberDetailVMs == null)
                {
                    string StoredProc = $"exec pCmsChamberNumberDetailById @Id ='{Id}'";

                    _chamberNumberDetailVMs = await _dbContext.ChambersNumberDetailVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _chamberNumberDetailVMs;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #endregion

        #region Chamber Number Hearing CRUD
        #region  Get Chambers Number Hearing  List
        public async Task<List<ChamberNumberHearingDetailVM>> GetChamberNumberHearingList()

        {
            string storedProc;
            try
            {
                storedProc = $"exec pCmsChamberNumberHearingList";

                var result = await _dbContext.ChamberNumberHearingDetailVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion

        #region Delete Chambers Number Hearing
        public async Task<ChamberNumberHearingDetailVM> DeleteChambersNumberHearing(ChamberNumberHearingDetailVM chambersNumberHearing)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.ChamberNumberHearings.Where(x => x.Id == chambersNumberHearing.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.DeletedBy = chambersNumberHearing.DeletedBy;
                            task.DeletedDate = chambersNumberHearing.DeletedDate;
                            task.IsDeleted = true;
                            await _dbContext.ChamberNumberHearings.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return chambersNumberHearing;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Save Chamber Number Hearing 
        public async Task<ChamberNumberHearing> SaveChamberNumberHearing(ChamberNumberHearing chambersNumberHearing)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var courtId in chambersNumberHearing.SelectedCourts)
                    {
                        foreach (var chamberId in chambersNumberHearing.SelectedChambers)
                        {
                            if (await _dbContext.CourtChambers.AnyAsync(cc => cc.CourtId == courtId && cc.ChamberId == chamberId)) // Check the Relation of Court & Chamber
                            {
                                foreach (var chamberNumberId in chambersNumberHearing.SelectedChamberNumbers)
                                {
                                    if (await _dbContext.ChamberChamberNumbers.AnyAsync(ccn => ccn.ChamberId == chamberId && ccn.ChamberNumberId == chamberNumberId)) // Check the Relation of Chamber & ChamberNumber
                                    {
                                        foreach (var hearingDayId in chambersNumberHearing.SelectedHearingDayIds)
                                        {
                                            if (!await _dbContext.ChamberNumberHearings.AnyAsync(x => x.ChamberNumberId == chamberNumberId && x.HearingDayId == hearingDayId && x.CourtId == courtId && x.ChamberId == chamberId))
                                            {
                                                var chamberNumberHearing = new ChamberNumberHearing
                                                {
                                                    ChamberNumberId = chamberNumberId,
                                                    HearingDayId = hearingDayId,
                                                    ChamberId = chamberId,
                                                    CourtId = courtId,
                                                    HearingsRollDays = chambersNumberHearing.HearingsRollDays,
                                                    JudgmentsRollDays = chambersNumberHearing.JudgmentsRollDays,
                                                    CreatedBy = chambersNumberHearing.CreatedBy,
                                                    CreatedDate = chambersNumberHearing.CreatedDate,
                                                };
                                                await _dbContext.ChamberNumberHearings.AddAsync(chamberNumberHearing);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return chambersNumberHearing;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        #endregion

        #region Update Chamber Number Hearing
        public async Task<ChamberNumberHearing> UpdateChamberNumberHearing(ChamberNumberHearing chambersNumberHearing)
        {
            try
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        // Retrieve the existing entries that are not in the newly selected hearing days
                        var existingEntries = await _dbContext.ChamberNumberHearings
                                              .Where(cn => cn.ChamberNumberId == chambersNumberHearing.SelectedChamberNumbers.First()
                                                           && cn.ChamberId == chambersNumberHearing.SelectedChambers.First()
                                                           && cn.CourtId == chambersNumberHearing.SelectedCourts.First()
                                                           && !chambersNumberHearing.SelectedHearingDayIds.Contains(cn.HearingDayId))
                                              .ToListAsync();

                        // Remove the existing entries
                        _dbContext.ChamberNumberHearings.RemoveRange(existingEntries);
                        await _dbContext.SaveChangesAsync();

                        // Add the newly selected hearing days
                        foreach (var hearingDayId in chambersNumberHearing.SelectedHearingDayIds)
                        {
                            // Check if the hearing day already exists to avoid duplicates
                            if (!_dbContext.ChamberNumberHearings.Any(cn => cn.ChamberNumberId == chambersNumberHearing.SelectedChamberNumbers.First()
                                                                        && cn.ChamberId == chambersNumberHearing.SelectedChambers.First()
                                                                        && cn.CourtId == chambersNumberHearing.SelectedCourts.First()
                                                                        && cn.HearingDayId == hearingDayId))
                            {
                                var chamberNumberHearing = new ChamberNumberHearing
                                {
                                    ChamberNumberId = chambersNumberHearing.SelectedChamberNumbers.First(),
                                    ChamberId = chambersNumberHearing.SelectedChambers.First(),
                                    CourtId = chambersNumberHearing.SelectedCourts.First(),
                                    HearingDayId = hearingDayId,
                                    HearingsRollDays = chambersNumberHearing.HearingsRollDays,
                                    JudgmentsRollDays = chambersNumberHearing.JudgmentsRollDays,
                                    CreatedBy = chambersNumberHearing.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                };
                                await _dbContext.ChamberNumberHearings.AddAsync(chamberNumberHearing);
                            }
                        }
                        await _dbContext.SaveChangesAsync();
                        transaction.Commit();
                        return chambersNumberHearing;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //   public async Task<ChamberNumberHearing> UpdateChamberNumberHearing(ChamberNumberHearing chambersNumberHearing)
        //   {
        //       try
        //       {
        //           using (var transaction = _dbContext.Database.BeginTransaction())
        //           {
        //               try
        //               {
        //	var existingEntries = await _dbContext.ChamberNumberHearings
        //                            .Where(cn => cn.ChamberNumberId == chambersNumberHearing.SelectedChamberNumbers.First()
        //		                  && cn.ChamberId == chambersNumberHearing.SelectedChambers.First()
        //		                  && cn.CourtId == chambersNumberHearing.SelectedCourts.First())
        //						 .ToListAsync();
        //	_dbContext.ChamberNumberHearings.RemoveRange(existingEntries);
        //	await _dbContext.SaveChangesAsync();

        //	foreach (var hearingDayId in chambersNumberHearing.SelectedHearingDayIds)
        //                   {
        //                       var chamberNumberHearing = new ChamberNumberHearing
        //                       {
        //                           ChamberNumberId = chambersNumberHearing.SelectedChamberNumbers.First(),   
        //                           ChamberId = chambersNumberHearing.SelectedChambers.First(),
        //                           CourtId = chambersNumberHearing.SelectedCourts.First(),

        //			HearingDayId = hearingDayId,

        //			HearingsRollDays = chambersNumberHearing.HearingsRollDays,
        //                           JudgmentsRollDays = chambersNumberHearing.JudgmentsRollDays,
        //                           CreatedBy = chambersNumberHearing.CreatedBy,
        //                           CreatedDate = DateTime.Now,
        //                       };
        //                       await _dbContext.ChamberNumberHearings.AddAsync(chamberNumberHearing);
        //                   }
        //                   await _dbContext.SaveChangesAsync();
        //                   transaction.Commit();
        //                   return chambersNumberHearing;
        //               }
        //catch (Exception)
        //               {
        //                   transaction.Rollback();
        //                   throw;
        //               }
        //           }
        //       }
        //       catch (Exception ex)
        //       {
        //           throw;
        //       }
        //   }
        #endregion

        #region Get Chamber Number Hearing  by ID 
        public async Task<ChamberNumberHearingDetailVM> GetChamberNumberHearingById(int Id)
        {
            try
            {
                if (_chamberNumberHearingDetailVMs == null)
                {
                    string StoredProc = $"exec pGetCmsChamberNumberHearingById @Id ='{Id}'";
                    _chamberNumberHearingDetailVMs = await _dbContext.ChamberNumberHearingDetailVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _chamberNumberHearingDetailVMs.FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #endregion

        #region Government Entity Sector  CRUD 
        #region  Get Government Entity Department List
        public async Task<List<GovernmentEntitiesSectorsVM>> GetGovernmentEntityDepartmentList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pCmsGovernmentEntitiesSectorList";

                var result = await _dbContext.GovernmentEntitiesSectorsVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion
        #region Delete Government Entity Department 
        public async Task<GovernmentEntitiesSectorsVM> DeleteGovernmentEntityDepartment(GovernmentEntitiesSectorsVM GESector)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.GEDepartments.Where(x => x.Id == GESector.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.DeletedBy = GESector.DeletedBy;
                            task.DeletedDate = GESector.DeletedDate;
                            task.IsDeleted = true;
                            await _dbContext.GEDepartments.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return GESector;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Active status of Government Entity Department
        public async Task<GovernmentEntitiesSectorsVM> ActiveGovernmentEntityDepartment(GovernmentEntitiesSectorsVM GeDepartment)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.GEDepartments.Where(x => x.Id == GeDepartment.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = GeDepartment.Id;
                            task.Name_En = GeDepartment.Name_En;
                            task.Name_Ar = GeDepartment.Name_Ar;
                            task.EntityId = GeDepartment.EntityId;
                            task.ModifiedBy = GeDepartment.ModifiedBy;
                            task.ModifiedDate = GeDepartment.ModifiedDate;
                            task.IsDeleted = (bool)GeDepartment.IsDeleted;
                            task.IsActive = GeDepartment.IsActive;
                            task.DeptProfession = GeDepartment.DeptProfession;
                            await _dbContext.GEDepartments.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = GeDepartment.Id,
                                LookupsTableId = (int)LookupsTablesEnum.CMS_GOVERNMENT_ENTITY_DEPARTMENT_G2G_LKP,
                                NameEn = GeDepartment.Name_En,
                                NameAr = GeDepartment.Name_Ar,
                                ModifiedBy = GeDepartment.ModifiedBy,
                                ModifiedDate = GeDepartment.ModifiedDate,
                                CreatedBy = GeDepartment.CreatedBy,
                                CreatedDate = (DateTime)GeDepartment.CreatedDate,
                                IsActive = (bool)GeDepartment.IsActive,
                                StatusId = (int)LookupHistoryEnums.Updated,
                            };
                            await ActiveLookupsHistroy(lookups, GeDepartment.ModifiedDate);
                            transation.Commit();
                        }
                        return GeDepartment;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Save Government Entity Department 
        public async Task<GEDepartments> SaveGovernmentEntityDepartment(GEDepartments GESector)
        {
            try
            {
                await _dbContext.GEDepartments.AddAsync(GESector);
                await _dbContext.SaveChangesAsync();
                LookupsHistory lookups = new LookupsHistory
                {
                    LookupsId = GESector.Id,
                    NameEn = GESector.Name_En,
                    NameAr = GESector.Name_Ar,
                    LookupsTableId = (int)LookupsTablesEnum.CMS_GOVERNMENT_ENTITY_DEPARTMENT_G2G_LKP,
                    CreatedDate = GESector.CreatedDate,
                    CreatedBy = GESector.CreatedBy,
                    StatusId = (int)LookupHistoryEnums.Added,
                    IsActive = false,
                };

                await SaveLookupsHistroy(lookups, null);
                return GESector;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
        #region Update Government Entity Sector 
        public async Task<GEDepartments> UpdateGovernmentEntityDepartment(GEDepartments GESector)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.GEDepartments.Where(x => x.Id == GESector.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = GESector.Id;
                            task.Name_En = GESector.Name_En;
                            task.Name_Ar = GESector.Name_Ar;
                            task.EntityId = GESector.EntityId;
                            task.ModifiedBy = GESector.ModifiedBy;
                            task.ModifiedDate = GESector.ModifiedDate;
                            task.IsDeleted = GESector.IsDeleted;
                            task.IsActive = GESector.IsActive;
                            task.G2GBRSiteID = GESector.G2GBRSiteID;
                            task.DefaultReceiver = GESector.DefaultReceiver;
                            task.DeptProfession = GESector.DeptProfession;
                            await _dbContext.GEDepartments.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            LookupsHistory lookups = new LookupsHistory
                            {
                                LookupsId = GESector.Id,
                                NameEn = GESector.Name_En,
                                NameAr = GESector.Name_Ar,
                                LookupsTableId = (int)LookupsTablesEnum.CMS_GOVERNMENT_ENTITY_DEPARTMENT_G2G_LKP,
                                ModifiedBy = GESector.ModifiedBy,
                                ModifiedDate = GESector.ModifiedDate,
                                CreatedBy = GESector.ModifiedBy,
                                CreatedDate = (DateTime)GESector.ModifiedDate,
                                StatusId = (int)LookupHistoryEnums.Updated,
                                IsActive = (bool)GESector.IsActive,
                            };

                            await SaveLookupsHistroy(lookups, GESector.ModifiedDate);
                            transation.Commit();
                        }
                        return GESector;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Get Government Entity Department  by Id 
        public async Task<GEDepartments> GetGovtEntityDepartmentById(int Id)
        {
            try
            {
                var result = await _dbContext.GEDepartments.Where(x => x.Id == Id).FirstOrDefaultAsync();

                return result;


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #endregion

        #region Government Entity Representative CRUD 
        #region  Get Government Entity Representative List
        public async Task<List<GovernmentEntitiesRepresentativeVM>> GetGovernmentEntiteRepresentativesList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pCmsGovernmentRepresentativeList";

                var result = await _dbContext.GovernmentEntitiesRepresentativeVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion
        #region Delete Government Entity Representative  
        public async Task<GovernmentEntitiesRepresentativeVM> DeleteGovernmentEntityRepresentative(GovernmentEntitiesRepresentativeVM GERepresentative)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.GovernmentEntityRepresentative.Where(x => x.Id == GERepresentative.id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.DeletedBy = GERepresentative.DeletedBy;
                            task.DeletedDate = GERepresentative.DeletedDate;
                            task.IsDeleted = true;
                            await _dbContext.GovernmentEntityRepresentative.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return GERepresentative;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Active status of Government Entity Representative
        public async Task<GovernmentEntitiesRepresentativeVM> ActiveGovernmentEntityRepresentative(GovernmentEntitiesRepresentativeVM GERepresentative)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.GovernmentEntityRepresentative.Where(x => x.Id == GERepresentative.id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = GERepresentative.id;
                            task.NameEn = GERepresentative.NameEn;
                            task.NameAr = GERepresentative.NameAr;
                            task.RepresentativeCode = GERepresentative.RepresentativeCode;
                            task.Representative_Designation_EN = GERepresentative.Representative_Designation_EN;
                            task.Representative_Designation_AR = GERepresentative.Representative_Designation_AR;
                            task.GovtEntityId = GERepresentative.GovtEntityId;
                            task.ModifiedBy = GERepresentative.ModifiedBy;
                            task.ModifiedDate = GERepresentative.ModifiedDate;
                            task.IsDeleted = GERepresentative.IsDeleted;
                            task.IsActive = GERepresentative.IsActive;
                            await _dbContext.GovernmentEntityRepresentative.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return GERepresentative;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Save Government Entity Representative  
        public async Task<GovernmentEntityRepresentative> SaveGovernmentEntityRepresentative(GovernmentEntityRepresentative GERepresentative)
        {
            try
            {
                await _dbContext.GovernmentEntityRepresentative.AddAsync(GERepresentative);
                await _dbContext.SaveChangesAsync();
                return GERepresentative;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
        #region Update Government Entity Representative  
        public async Task<GovernmentEntityRepresentative> UpdateGovernmentEntityRepresentative(GovernmentEntityRepresentative GERepresentative)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.GovernmentEntityRepresentative.Where(x => x.Id == GERepresentative.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = GERepresentative.Id;
                            task.NameEn = GERepresentative.NameEn;
                            task.NameAr = GERepresentative.NameAr;
                            task.RepresentativeCode = GERepresentative.RepresentativeCode;
                            task.Representative_Designation_EN = GERepresentative.Representative_Designation_EN;
                            task.Representative_Designation_AR = GERepresentative.Representative_Designation_AR;
                            task.GovtEntityId = GERepresentative.GovtEntityId;
                            task.ModifiedBy = GERepresentative.ModifiedBy;
                            task.ModifiedDate = GERepresentative.ModifiedDate;
                            task.IsDeleted = GERepresentative.IsDeleted;
                            task.IsActive = GERepresentative.IsActive;
                            await _dbContext.GovernmentEntityRepresentative.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return GERepresentative;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #region Get Government Entity Representative  by Id 
        public async Task<GovernmentEntityRepresentative> GetGovernmentRepresentativeById(Guid Id)
        {
            try
            {
                var result = await _dbContext.GovernmentEntityRepresentative.Where(x => x.Id == Id).FirstOrDefaultAsync();

                return result;


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #endregion

        #region Get the record from number pattern history table when user editing 
        public async Task<List<CmsComsNumPatternHistory>> GetCmsComsNumberPatternHistoryForEditing(Guid Id)
        {
            try
            {
                List<CmsComsNumPatternHistory> previousHistoryEntry = await _dbContext.CmsComsNumPatternHistories
                                                        .Where(x => x.PatternId == Id)
                                                        .OrderByDescending(x => x.CreatedDate)
                                                        .ThenByDescending(x => x.CreatedDate)
                                                        .Take(1)
                                                        .ToListAsync();
                if (previousHistoryEntry != null)
                {
                    return previousHistoryEntry;
                }

                return null;

            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Moj Roll 
        public async Task<List<RMSChamberVM>> GetChamberByUserId(string UserId)
        {
            try
            {
                string StoredProc = $"exec pGetChamberByUserId @UserId='{UserId}'";
                var result = await _dbContext.RMSChamberVMs.FromSqlRaw(StoredProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        public async Task<List<RMSCourtsVM>> GetCourtByUserId(string UserId)
        {
            try
            {
                string StoredProc = $"exec pGetCourtByUserId @UserId='{UserId}'";
                var result = await _dbContext.RMSCourtsVMs.FromSqlRaw(StoredProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public async Task<List<FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups.MOJRollsChamberNumberVM>> GetChamberNumberByUserId(string UserId)
        {
            try
            {
                string StoredProc = $"exec pGetChamberNumberByUserId @UserId='{UserId}'";
                var result = await _dbContext.MOJRollsChamberNumberVMs.FromSqlRaw(StoredProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public async Task<List<MOJRollsChamberCourtChamberNumberVM>> GetAllChamberCourtChamberNumberForMojRollsByUserId(string UserId)
        {
            try
            {
                string StoredProc = $"exec pGetMojRollsAllChamberCourtChamberNumberByUserId @UserId='{UserId}'";
                var result = await _dbContext.MOJRollsChamberCourtChamberNumberVMs.FromSqlRaw(StoredProc).ToListAsync();
                if (result != null)
                {
                    _MOJRollsChamberCourtChamberNumberVMs = result;
                }
                return _MOJRollsChamberCourtChamberNumberVMs;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<List<MOJRollsChamberCourtChamberNumberVM>> GetAllChamberCourtChamberNumberForMojRolls()
        {
            try
            {
                string StoredProc = $"exec pGetAllChamberCourtChamberNumberForMojRolls";
                var result = await _dbContext.MOJRollsChamberCourtChamberNumberVMs.FromSqlRaw(StoredProc).ToListAsync();
                if (result != null)
                {
                    _MOJRollsChamberCourtChamberNumberVMs = result;
                }
                return _MOJRollsChamberCourtChamberNumberVMs;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Get Document Type by Id 
        public async Task<AttachmentType> FindAndSaveAttachmentType(string attchmenttype)
        {
            try
            {
                int newId = await _dmsDbContext.AttachmentType.MaxAsync(x => x.AttachmentTypeId) + 1;
                var existingtAttachmentType = await _dmsDbContext.AttachmentType.Where(x => x.Type_Ar == attchmenttype).FirstOrDefaultAsync();
                if (existingtAttachmentType != null)
                {
                    return existingtAttachmentType;
                }
                else
                {
                    AttachmentType newAttachmentType = new AttachmentType
                    {
                        AttachmentTypeId = newId,
                        Type_Ar = attchmenttype,
                        Type_En = attchmenttype,
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        CreatedBy = "MOJ RPA",
                        CreatedDate = DateTime.Now,
                        IsMandatory = false,
                        IsOpinion = false,
                        IsOfficialLetter = false,
                        IsGePortalType = false,
                        IsSystemDefine = false,
                        IsActive = true,
                        IsMojExtracted = true
                    };
                    await _dmsDbContext.AttachmentType.AddAsync(newAttachmentType);
                    await _dmsDbContext.SaveChangesAsync();
                    return newAttachmentType;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<AttachmentType> GetDocumentTypeByName(string attachmentTypeName)
        {
            try
            {
                var result = await _dmsDbContext.AttachmentType.Where(x => EF.Functions.Like(x.Type_Ar, "%" + attachmentTypeName + "%")).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Bank Details 
        #region Get Bank Names
        public async Task<List<CmsBank>> GetBankNames()
        {
            try
            {
                return await _dbContext.CmsBanks.OrderBy(u => u.Id).Where(u => u.IsDeleted == false).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Get Bank Name By Id 
        public async Task<CmsBank> GetBankNameById(int BankId)
        {
            try
            {
                var bankDetails = await _dbContext.CmsBanks
                                .Where(u => u.Id == BankId && !u.IsDeleted)
                                .FirstOrDefaultAsync();

                return bankDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Get Bank Name By  Entity Id
        public async Task<List<CmsBankGovernmentEntity>> GetBankDetailByEntityId(int EntityId)
        {
            try
            {
                var bankDetails = await (from bge in _dbContext.CmsBankGovernmentEntities
                                         join bank in _dbContext.CmsBanks on bge.BankId equals bank.Id
                                         where bge.GovtEntityId == EntityId && bge.IsDeleted == false
                                         select new CmsBankGovernmentEntity
                                         {
                                             Id = bge.Id,
                                             BankId = bge.BankId,
                                             GovtEntityId = bge.GovtEntityId,
                                             IBAN = bge.IBAN,
                                             BankNameEn = bank.Name_En,
                                             BankNameAr = bank.Name_Ar
                                         }).ToListAsync();
                if (bankDetails.Count == 0)
                {
                    //return null; 
                    return new List<CmsBankGovernmentEntity>();
                }
                else
                {
                    return bankDetails;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Delete Bank Details
        public async Task<CmsBankGovernmentEntity> DeleteBankDetail(CmsBankGovernmentEntity cmsBankGovernmentEntity)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.CmsBankGovernmentEntities.Where(x => x.Id == cmsBankGovernmentEntity.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.DeletedBy = cmsBankGovernmentEntity.DeletedBy;
                            task.DeletedDate = cmsBankGovernmentEntity.DeletedDate;
                            task.IsDeleted = true;
                            await _dbContext.CmsBankGovernmentEntities.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return cmsBankGovernmentEntity;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #endregion

        #region Ep Nationality CRUD 
        #region  Get Ep Nationality List
        public async Task<List<EpNationalityVM>> GetEpNationalityList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pEpNationalitylist";

                var result = await _dbContext.EpNationalityVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion

        #region Delete Ep Nationality
        public async Task<EpNationalityVM> DeleteEpNationality(EpNationalityVM EPNationality)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.Nationalities.Where(x => x.Id == EPNationality.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.DeletedBy = EPNationality.DeletedBy;
                            task.DeletedDate = EPNationality.DeletedDate;
                            task.IsDeleted = true;
                            await _dbContext.Nationalities.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return EPNationality;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Active status of Ep Nationality
        public async Task<EpNationalityVM> ActiveEpNationality(EpNationalityVM EPNationality)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.Nationalities.Where(x => x.Id == EPNationality.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = EPNationality.Id;
                            task.Name_En = EPNationality.Name_En;
                            task.Name_Ar = EPNationality.Name_Ar;
                            task.ModifiedBy = EPNationality.ModifiedBy;
                            task.ModifiedDate = EPNationality.ModifiedDate;
                            task.IsDeleted = (bool)EPNationality.IsDeleted;
                            task.IsActive = EPNationality.IsActive;
                            await _dbContext.Nationalities.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return EPNationality;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Save Ep Nationality 
        public async Task<Nationality> SaveNationality(Nationality EPNationality)
        {
            try
            {
                var task = await _dbContext.Nationalities.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                if (task != null)
                {
                    int id = GetNextUniqueIdNationality(task.Id);
                    EPNationality.Id = id;
                    await _dbContext.Nationalities.AddAsync(EPNationality);
                    await _dbContext.SaveChangesAsync();
                    return EPNationality;
                }
                else
                {
                    int counter = 0;
                    ++counter;
                    EPNationality.Id = counter;
                    await _dbContext.Nationalities.AddAsync(EPNationality);
                    await _dbContext.SaveChangesAsync();
                    return EPNationality;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #region Get Unique id 
        private int GetNextUniqueIdNationality(int counter)
        {
            if (_dbContext.Nationalities == null)
            {
                counter = 0;
                return ++counter;
            }
            else
            {
                return ++counter;
            }

        }
        #endregion
        #endregion

        #region Update Ep Nationality  
        public async Task<Nationality> UpdateNationality(Nationality EPNationality)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.Nationalities.Where(x => x.Id == EPNationality.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = EPNationality.Id;
                            task.Name_En = EPNationality.Name_En;
                            task.Name_Ar = EPNationality.Name_Ar;
                            task.ModifiedBy = EPNationality.ModifiedBy;
                            task.ModifiedDate = EPNationality.ModifiedDate;
                            task.IsDeleted = EPNationality.IsDeleted;
                            task.IsActive = EPNationality.IsActive;
                            await _dbContext.Nationalities.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return EPNationality;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Get Nationality by Id 
        public async Task<Nationality> GetEpNationalityById(int Id)
        {
            try
            {
                var result = await _dbContext.Nationalities.Where(x => x.Id == Id).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #endregion

        #region Ep Grade CRUD 
        #region  Get Ep Grade List
        public async Task<List<EpGradeVM>> GetEpGradeList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pEpGradelist";

                var result = await _dbContext.EpGradeVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion

        #region Delete Ep Grade
        public async Task<EpGradeVM> DeleteEpGrade(EpGradeVM EPGrade)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.UserGrades.Where(x => x.Id == EPGrade.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.DeletedBy = EPGrade.DeletedBy;
                            task.DeletedDate = EPGrade.DeletedDate;
                            task.IsDeleted = true;
                            await _dbContext.UserGrades.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return EPGrade;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Active status of Ep Grade
        public async Task<EpGradeVM> ActiveEpGrade(EpGradeVM EPGrade)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.UserGrades.Where(x => x.Id == EPGrade.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = EPGrade.Id;
                            task.Name_En = EPGrade.Name_En;
                            task.Name_Ar = EPGrade.Name_Ar;
                            task.ModifiedBy = EPGrade.ModifiedBy;
                            task.ModifiedDate = EPGrade.ModifiedDate;
                            task.IsDeleted = (bool)EPGrade.IsDeleted;
                            task.IsActive = EPGrade.IsActive;
                            await _dbContext.UserGrades.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return EPGrade;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Save Ep Grade 
        public async Task<Grade> SaveGrade(Grade EPGrade)
        {
            try
            {
                var task = await _dbContext.UserGrades.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                if (task != null)
                {
                    int id = GetNextUniqueIdGrade(task.Id);
                    EPGrade.Id = id;
                    await _dbContext.UserGrades.AddAsync(EPGrade);
                    await _dbContext.SaveChangesAsync();
                    return EPGrade;
                }
                else
                {
                    int counter = 0;
                    ++counter;
                    EPGrade.Id = counter;
                    await _dbContext.UserGrades.AddAsync(EPGrade);
                    await _dbContext.SaveChangesAsync();
                    return EPGrade;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #region Get Unique id 
        private int GetNextUniqueIdGrade(int counter)
        {
            if (_dbContext.UserGrades == null)
            {
                counter = 0;
                return ++counter;
            }
            else
            {
                return ++counter;
            }

        }
        #endregion
        #endregion

        #region Update Ep Grade  
        public async Task<Grade> UpdateGrade(Grade EPGrade)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.UserGrades.Where(x => x.Id == EPGrade.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = EPGrade.Id;
                            task.Name_En = EPGrade.Name_En;
                            task.Name_Ar = EPGrade.Name_Ar;
                            task.ModifiedBy = EPGrade.ModifiedBy;
                            task.ModifiedDate = EPGrade.ModifiedDate;
                            task.IsDeleted = EPGrade.IsDeleted;
                            task.IsActive = EPGrade.IsActive;
                            await _dbContext.UserGrades.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return EPGrade;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Get Grade by Id 
        public async Task<Grade> GetEpGradeById(int Id)
        {
            try
            {
                var result = await _dbContext.UserGrades.Where(x => x.Id == Id).FirstOrDefaultAsync();

                return result;


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #endregion

        #region Ep Grade Type CRUD 
        #region  Get Ep Grade Type List
        public async Task<List<EpGradeTypeVM>> GetEpGradeTypeList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pEpGradeTypelist";

                var result = await _dbContext.EpGradeTypeVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion

        #region Delete Ep Grade Type
        public async Task<EpGradeTypeVM> DeleteEpGradeType(EpGradeTypeVM EPGradeType)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        //var task = await _dbContext.UserGrades.Where(x => x.Id == EPGradeType.Id).FirstOrDefaultAsync();
                        var task = await _dbContext.GradeTypes.Where(x => x.Id == EPGradeType.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.DeletedBy = EPGradeType.DeletedBy;
                            task.DeletedDate = EPGradeType.DeletedDate;
                            task.IsDeleted = true;
                            //await _dbContext.UserGrades.AddAsync(task);
                            await _dbContext.GradeTypes.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return EPGradeType;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Save Ep Grade Type
        public async Task<GradeType> SaveGradeType(GradeType EPGradeType)
        {
            try
            {
                await _dbContext.GradeTypes.AddAsync(EPGradeType);
                await _dbContext.SaveChangesAsync();
                return EPGradeType;
                //var task = await _dbContext.UserGrades.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                //var task = await _dbContext.GradeTypes.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                //if (task != null)
                //{
                //    //int id = GetNextUniqueIdGradeType(task.Id);
                //    //EPGradeType.Id = id;
                //    //await _dbContext.UserGrades.AddAsync(EPGrade);

                //}
                //else
                //{
                //    int counter = 0;
                //    ++counter;
                //    EPGradeType.Id = counter;
                //    //await _dbContext.UserGrades.AddAsync(EPGradeType);
                //    await _dbContext.GradeTypes.AddAsync(EPGradeType);
                //    await _dbContext.SaveChangesAsync();
                //    return EPGradeType;
                //}

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #region Get Unique id 
        //private int GetNextUniqueIdGradeType(int counter)
        //{
        //    if (_dbContext.GradeTypes == null)
        //    {
        //        counter = 0;
        //        return ++counter;
        //    }
        //    else
        //    {
        //        return ++counter;
        //    }

        //}
        #endregion

        #endregion

        #region Update Ep Grade  
        public async Task<GradeType> UpdateGradeType(GradeType EPGradeType)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        //var task = await _dbContext.UserGrades.Where(x => x.Id == EPGrade.Id).FirstOrDefaultAsync();
                        var task = await _dbContext.GradeTypes.Where(x => x.Id == EPGradeType.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = EPGradeType.Id;
                            task.Name_En = EPGradeType.Name_En;
                            task.Name_Ar = EPGradeType.Name_Ar;
                            task.DepartmentId = EPGradeType.DepartmentId;
                            task.ModifiedBy = EPGradeType.ModifiedBy;
                            task.ModifiedDate = EPGradeType.ModifiedDate;
                            task.IsDeleted = EPGradeType.IsDeleted;
                            //await _dbContext.UserGrades.AddAsync(task);
                            await _dbContext.GradeTypes.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return EPGradeType;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Get Grade by Id 
        public async Task<GradeType> GetEpGradeTypeById(int Id)
        {
            try
            {
                //var result = await _dbContext.UserGrades.Where(x => x.Id == Id).FirstOrDefaultAsync();
                var result = await _dbContext.GradeTypes.Where(x => x.Id == Id).FirstOrDefaultAsync();

                return result;


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #endregion

        #region Gender Enum 
        #region Get Gender List 
        public async Task<List<EpGenderVM>> GetGenderList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pEpGenderlist";

                var result = await _dbContext.EpGenderVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        #endregion

        #region Get Gender  by Id 
        public async Task<Gender> GetGenderById(int Id)
        {
            try
            {
                var result = await _dbContext.Genders.Where(x => x.Id == Id).FirstOrDefaultAsync();

                return result;


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Update Gender
        public async Task<Gender> UpdateGender(Gender gender)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.Genders.Where(x => x.Id == gender.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = gender.Id;
                            task.Name_En = gender.Name_En;
                            task.Name_Ar = gender.Name_Ar;
                            task.ModifiedBy = gender.ModifiedBy;
                            task.ModifiedDate = gender.ModifiedDate;
                            //task.IsDeleted = department.IsDeleted;
                            //task.IsActive = department.IsActive;
                            await _dbContext.Genders.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return gender;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion
        #endregion

        #region Ep Designation CRUD 
        #region  Get Ep Designation List
        public async Task<List<EpDesignationVM>> GetEpDesignationList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pEpDesignationlist";

                var result = await _dbContext.EpDesignationVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion

        #region Delete Ep Designation
        public async Task<EpDesignationVM> DeleteEpDesignation(EpDesignationVM EPDesignation)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.Designations.Where(x => x.Id == EPDesignation.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.DeletedBy = EPDesignation.DeletedBy;
                            task.DeletedDate = EPDesignation.DeletedDate;
                            task.IsDeleted = true;
                            await _dbContext.Designations.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return EPDesignation;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Save Ep Designation 
        public async Task<Designation> SaveDesignation(Designation EPDesignation)
        {
            try
            {
                await _dbContext.Designations.AddAsync(EPDesignation);
                await _dbContext.SaveChangesAsync();
                return EPDesignation;


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Update Ep Designation  
        public async Task<Designation> UpdateDesignation(Designation EPDesignation)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.Designations.Where(x => x.Id == EPDesignation.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = EPDesignation.Id;
                            task.Name_En = EPDesignation.Name_En;
                            task.Name_Ar = EPDesignation.Name_Ar;
                            task.ModifiedBy = EPDesignation.ModifiedBy;
                            task.ModifiedDate = EPDesignation.ModifiedDate;
                            task.IsDeleted = EPDesignation.IsDeleted;
                            await _dbContext.Designations.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return EPDesignation;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Get EP Designation by Id 
        public async Task<Designation> GetEpDesignationById(int Id)
        {
            try
            {
                var result = await _dbContext.Designations.Where(x => x.Id == Id).FirstOrDefaultAsync();

                return result;


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #endregion
        public async Task<List<Court>> GetCourtById(string UserId)
        {
            try
            {
                string StoredProc = $"exec pGetCourtByUserId @UserId='{UserId}'";
                var result = await _dbContext.Courts.FromSqlRaw(StoredProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        #region Ep Contract Type  CRUD 
        #region  Get Ep Contract List
        public async Task<List<EpContractTypeVM>> GetEpContractTypeList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pEpContractTypelist";

                var result = await _dbContext.EpContractTypeVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion

        #region Delete Ep Contract Type 
        public async Task<EpContractTypeVM> DeleteEpContractType(EpContractTypeVM EPContractType)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.ContractTypes.Where(x => x.Id == EPContractType.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.DeletedBy = EPContractType.DeletedBy;
                            task.DeletedDate = EPContractType.DeletedDate;
                            task.IsDeleted = true;
                            await _dbContext.ContractTypes.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return EPContractType;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Save Ep Contract Type
        public async Task<ContractType> SaveContractType(ContractType EPContractType)
        {
            try
            {
                await _dbContext.ContractTypes.AddAsync(EPContractType);
                await _dbContext.SaveChangesAsync();
                return EPContractType;


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Update Ep Contract Type    
        public async Task<ContractType> UpdateContractType(ContractType EPContractType)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.ContractTypes.Where(x => x.Id == EPContractType.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = EPContractType.Id;
                            task.Name_En = EPContractType.Name_En;
                            task.Name_Ar = EPContractType.Name_Ar;
                            task.ModifiedBy = EPContractType.ModifiedBy;
                            task.ModifiedDate = EPContractType.ModifiedDate;
                            task.IsDeleted = EPContractType.IsDeleted;
                            await _dbContext.ContractTypes.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return EPContractType;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Get EP Contract Type  by Id 
        public async Task<ContractType> GetEpContractTypeById(int Id)
        {
            try
            {
                var result = await _dbContext.ContractTypes.Where(x => x.Id == Id).FirstOrDefaultAsync();

                return result;


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #endregion

        #region Book Author CRUD 
        #region  Get Book Author List
        public async Task<List<BookAuthorVM>> GetBookAuthorList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pGetBookAuthorlist";

                var result = await _dbContext.BookAuthorVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion

        #region Delete Book Author 
        public async Task<BookAuthorVM> DeleteBookAuthor(BookAuthorVM bookAuthorVM)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.LmsLiteratureAuthors.Where(x => x.AuthorId == bookAuthorVM.AuthorId).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.DeletedBy = bookAuthorVM.DeletedBy;
                            task.DeletedDate = bookAuthorVM.DeletedDate;
                            task.IsDeleted = true;
                            await _dbContext.LmsLiteratureAuthors.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return bookAuthorVM;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Save Book Author
        public async Task<LmsLiteratureAuthor> SaveBookAuthor(LmsLiteratureAuthor lmsLiteratureAuthor)
        {
            try
            {
                await _dbContext.LmsLiteratureAuthors.AddAsync(lmsLiteratureAuthor);
                await _dbContext.SaveChangesAsync();
                return lmsLiteratureAuthor;


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Update Book Author   
        public async Task<LmsLiteratureAuthor> UpdateBookAuthor(LmsLiteratureAuthor lmsLiteratureAuthor)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.LmsLiteratureAuthors.Where(x => x.AuthorId == lmsLiteratureAuthor.AuthorId).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.AuthorId = lmsLiteratureAuthor.AuthorId;
                            task.FullName_En = lmsLiteratureAuthor.FullName_En;
                            task.FullName_Ar = lmsLiteratureAuthor.FullName_Ar;
                            task.FirstName_En = lmsLiteratureAuthor.FirstName_En;
                            task.FirstName_Ar = lmsLiteratureAuthor.FirstName_Ar;
                            task.SecondName_En = lmsLiteratureAuthor.SecondName_En;
                            task.SecondName_Ar = lmsLiteratureAuthor.SecondName_Ar;
                            task.ThirdName_En = lmsLiteratureAuthor.ThirdName_En;
                            task.ThirdName_Ar = lmsLiteratureAuthor.ThirdName_Ar;
                            task.Address_En = lmsLiteratureAuthor.Address_En;
                            task.Address_Ar = lmsLiteratureAuthor.Address_Ar;
                            task.ModifiedBy = lmsLiteratureAuthor.ModifiedBy;
                            task.ModifiedDate = lmsLiteratureAuthor.ModifiedDate;
                            task.IsDeleted = lmsLiteratureAuthor.IsDeleted;
                            await _dbContext.LmsLiteratureAuthors.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return lmsLiteratureAuthor;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Get Book Author  by Id 
        public async Task<LmsLiteratureAuthor> GetBookAuthorById(int AuthorId)
        {
            try
            {
                var result = await _dbContext.LmsLiteratureAuthors.Where(x => x.AuthorId == AuthorId).FirstOrDefaultAsync();

                return result;


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #endregion

        #region G2G Correspondences Receiver CRUD 
        #region  Get G2G Correspondences Receiver List
        public async Task<List<CmsSectorTypeGEDepartmentVM>> GetG2GCorrespondencesReceiverList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pGetG2GCorrespondencesReceiverlist";

                var result = await _dbContext.CmsSectorTypeGEDepartmentVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion

        #region Delete G2G Correspondences Receiver
        public async Task<CmsSectorTypeGEDepartmentVM> DeleteG2GCorrespondencesReceiver(CmsSectorTypeGEDepartmentVM cmsSectorTypeGEDepartmentVM)
        {
            using (var dbContext = _dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var departmentToDelete = await _dbContext.CmsSectorTypeGEDepartments
                            .FirstOrDefaultAsync(x => x.Id == cmsSectorTypeGEDepartmentVM.Id);

                        if (departmentToDelete != null)
                        {
                            _dbContext.CmsSectorTypeGEDepartments.Remove(departmentToDelete);
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();
                        }

                        return cmsSectorTypeGEDepartmentVM;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        #endregion

        #region Save G2G Correspondences Receiver  
        public async Task<CmsSectorTypeGEDepartment> SaveG2GCorrespondencesReceiver(CmsSectorTypeGEDepartment cmsSectorTypeGEDepartment)
        {
            try
            {
                foreach (var departmentId in cmsSectorTypeGEDepartment.SelectedDepartments)
                {
                    var existingDepartment = await _dbContext.CmsSectorTypeGEDepartments
                        .FirstOrDefaultAsync(d => d.SectorTypeId == (int)cmsSectorTypeGEDepartment.SectorTypeId &&
                                                  d.DepartmentId == departmentId);

                    if (existingDepartment == null)
                    {
                        var newDepartment = new CmsSectorTypeGEDepartment
                        {
                            SectorTypeId = (int)cmsSectorTypeGEDepartment.SectorTypeId,
                            DepartmentId = departmentId,
                            CreatedDate = DateTime.Now,
                            CreatedBy = cmsSectorTypeGEDepartment.CreatedBy
                        };

                        await _dbContext.CmsSectorTypeGEDepartments.AddAsync(newDepartment);
                    }
                }

                await _dbContext.SaveChangesAsync();
                return cmsSectorTypeGEDepartment;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Get G2G Correspondences Receiver  by Id 
        public async Task<List<GEDepartments>> GetDepartmentByGEEntityId(List<int> EntityIds)
        {
            try
            {
                var departments = await (from entity in _dbContext.GovernmentEntity
                                         join department in _dbContext.GeDepartments
                                         on entity.EntityId equals department.EntityId
                                         where EntityIds.Contains(entity.EntityId)
                                         select department)
                                      .ToListAsync();

                return departments;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
        #endregion

        #region Check Default Receiver Already Attached
        public async Task<bool> CheckDefaultReceiverAlreadyAttached(int EntityId, int DepartmentId)
        {
            try
            {
                var result = await _dbContext.GEDepartments
                    .AnyAsync(x => x.EntityId == EntityId && x.Id != DepartmentId && x.DefaultReceiver == true);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Literature Dewey Number Pattern CRUD 
        #region  Get Literature Dewey Number Pattern List
        public async Task<List<LiteratureDeweyNumberPatternVM>> GetLiteratureDeweyNumberPatternsList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pGetliteratureDeweyNumberPatternslist";

                var result = await _dbContext.LiteratureDeweyNumberPatternVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion

        #region Delete Literature Dewey Number Pattern 
        public async Task<LiteratureDeweyNumberPatternVM> DeleteLiteratureDeweyNumberPattern(LiteratureDeweyNumberPatternVM literatureDeweyNumberPatternVM)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.LiteratureDeweyNumberPatterns.Where(x => x.Id == literatureDeweyNumberPatternVM.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.DeletedBy = literatureDeweyNumberPatternVM.DeletedBy;
                            task.DeletedDate = literatureDeweyNumberPatternVM.DeletedDate;
                            task.IsDeleted = true;
                            await _dbContext.LiteratureDeweyNumberPatterns.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return literatureDeweyNumberPatternVM;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Save Literature Dewey Number Patterns
        public async Task<LiteratureDeweyNumberPattern> SaveLiteratureDeweyNumberPattern(LiteratureDeweyNumberPattern literatureDeweyNumberPatterns)
        {
            try
            {
                await _dbContext.LiteratureDeweyNumberPatterns.AddAsync(literatureDeweyNumberPatterns);
                await _dbContext.SaveChangesAsync();
                return literatureDeweyNumberPatterns;


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Update Literature Dewey Number Patterns
        public async Task<LiteratureDeweyNumberPattern> UpdateLiteratureDeweyNumberPattern(LiteratureDeweyNumberPattern literatureDeweyNumberPatterns)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.LiteratureDeweyNumberPatterns.Where(x => x.Id == literatureDeweyNumberPatterns.Id).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Id = literatureDeweyNumberPatterns.Id;
                            task.SeriesNumber = literatureDeweyNumberPatterns.SeriesNumber;
                            task.DigitSequenceNumber = literatureDeweyNumberPatterns.DigitSequenceNumber;
                            task.SequenceFormatResult = literatureDeweyNumberPatterns.SequenceFormatResult;
                            task.SequenceResult = literatureDeweyNumberPatterns.SequenceResult;
                            task.SeriesSequenceNumber = literatureDeweyNumberPatterns.SeriesSequenceNumber;
                            task.SeperatorPattern = literatureDeweyNumberPatterns.SeperatorPattern;
                            task.CheracterSeriesOrder = literatureDeweyNumberPatterns.CheracterSeriesOrder;
                            task.DigitSequnceOrder = literatureDeweyNumberPatterns.DigitSequnceOrder;
                            await _dbContext.LiteratureDeweyNumberPatterns.AddAsync(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        return literatureDeweyNumberPatterns;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw;
                    }
                }
            }

        }
        #endregion

        #region Get Literature Dewey Number Pattern By Id  
        public async Task<LiteratureDeweyNumberPattern> GetLiteratureDeweyNumberPatternById(Guid Id)
        {
            try
            {
                var result = await _dbContext.LiteratureDeweyNumberPatterns.Where(x => x.Id == Id).FirstOrDefaultAsync();

                return result;


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
        #endregion

        #region Check Name En and Name Ar Already Exists
        public async Task<bool> CheckNameEnExists(string NameEn, int requestTypeId, int subTypeId)
        {
            try
            {
                //return await _dbContext.Subtypes.Where(x=>x.RequestTypeId >= start && x.RequestTypeId <= end).AnyAsync(x => x.Name_En == NameEn);
                return await _dbContext.Subtypes.AnyAsync(x => x.RequestTypeId == requestTypeId && x.Name_En == NameEn && subTypeId != x.Id);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> CheckNameArExists(string NameAr, int requestTypeId, int subTypeId)
        {
            try
            {
                //return await _dbContext.Subtypes.Where(x => x.RequestTypeId >= start && x.RequestTypeId <= end).AnyAsync(x => x.Name_Ar == NameAr);
                return await _dbContext.Subtypes.AnyAsync(x => x.RequestTypeId == requestTypeId && x.Name_Ar == NameAr && subTypeId != x.Id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Get Sector Types By Department Id 
        //<History Author = 'Muhammad Zaeem' Date='2024-07-10' Version="1.0" Branch="master"> Get Operating Sector Types By DepartmentId</History>
        public async Task<List<OperatingSectorType>> GetOperatingSectorsByDepartmentId(int DepartmentId)
        {
            try
            {
                return await _dbContext.OperatingSectorType.Where(x => x.DepartmentId == DepartmentId).OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Weekdays Settings
        /// <summary>
        /// To get the weekdays settings
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<WeekdaysSetting>> GetWeekdaysSettings()
        {
            try
            {
                return await _dbContext.WeekdaysSettings.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Get Users List By RoleId and Sectoid
        public async Task<List<SectorUsersVM>> GetUsersListBySectorIdAndRoleId(string RoleId, int SectorTypeId)
        {
            try
            {
                var storeProc = $"exec pGetUsersListBySectorIdAndRoleId @RoleId= '{RoleId}' , @SectorTypeId = '{SectorTypeId}'";
                var result = await _dbContext.SectorUsersVM.FromSqlRaw(storeProc).ToListAsync();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new List<SectorUsersVM>();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Get Government Entity Representatives List
        //<History Author = 'Ammaar Naveed' Date='2024-09-03' Version="1.0" Branch="master">Get Government Entity Representatives By Govt Entity EntityId</History>
        public async Task<GovtEntityRepresentativeNamesResponseVM> GetGovernmentEntityRepresentatives(GovtEntityIdsPayload request)
        {
            try
            {
                GovtEntityRepresentativeNamesResponseVM responseVM = new GovtEntityRepresentativeNamesResponseVM();

                var representatives = await _dbContext.GovernmentEntityRepresentative
                    .Where(rep => request.EntityIds.Contains(rep.GovtEntityId))
                    .GroupBy(rep => rep.GovtEntityId)
                    .Select(g => g.OrderByDescending(rep => rep.CreatedDate).FirstOrDefault())
                    .ToListAsync();

                representatives = representatives.OrderBy(rep => request.EntityIds.IndexOf(rep.GovtEntityId)).ToList();

                responseVM.RepresentativeNameEn = string.Join(", ", representatives.Select(rep => rep.NameEn));
                responseVM.RepresentativeNameAr = string.Join("، ", representatives.Select(rep => rep.NameAr));


                var govtEntities = await _dbContext.GovernmentEntity
                    .Where(rep => request.EntityIds.Contains(rep.EntityId))
                    .GroupBy(rep => rep.EntityId)
                    .Select(g => g.OrderByDescending(rep => rep.CreatedDate).FirstOrDefault())
                    .ToListAsync();
                responseVM.GovtEntityNameEn = string.Join(", ", govtEntities.Select(rep => rep.Name_En));
                responseVM.GovtEntityNameAr = string.Join("، ", govtEntities.Select(rep => rep.Name_Ar));

                return responseVM;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Sector Role ( Save Sector Roles, Get Sector Roles )
        public async Task<List<SectorRolesVM>> GetRolesBySectorIds(List<int> sectorIds)
        {
            try
            {
                string sectortypeids = string.Join(",", sectorIds);
                var storeProc = $"exec pGetRolesBysectorTypeIds @sectorIds= '{sectortypeids}'";
                var result = await _dbContext.SectorRolesVms.FromSqlRaw(storeProc).ToListAsync();
                if (result != null)
                {
                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CmsOperatingSectorTypesRoles>> GetRolesOfSectorBySectorId(int sectorId)
        {
            try
            {
                var result = await _dbContext.CmsOperatingSectorTypesRoles.Where(x => x.SectorId == sectorId).ToListAsync();
                return result != null ? result : null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        protected async Task<bool> SaveSectorRoles(CmsOperatingSectorTypesRoles sectorRoles)
        {
            try
            {
                var existanceRec = _dbContext.CmsOperatingSectorTypesRoles.Where(x => x.SectorId == sectorRoles.SectorId);
                _dbContext.CmsOperatingSectorTypesRoles.RemoveRange(existanceRec);
                var isRemoved = await _dbContext.SaveChangesAsync();
                if (isRemoved > 0)
                {
                    foreach (var role in sectorRoles.RoleIds)
                    {
                        CmsOperatingSectorTypesRoles sectorRole = new CmsOperatingSectorTypesRoles();
                        sectorRole.SectorId = sectorRoles.SectorId;
                        sectorRole.RoleId = role;
                        _dbContext.CmsOperatingSectorTypesRoles.Add(sectorRole);
                    }
                    var result = await _dbContext.SaveChangesAsync();
                    return result > 0 ? true : false;
                }
                else
                {
                    foreach (var role in sectorRoles.RoleIds)
                    {
                        CmsOperatingSectorTypesRoles sectorRole = new CmsOperatingSectorTypesRoles();
                        sectorRole.SectorId = sectorRoles.SectorId;
                        sectorRole.RoleId = role;
                        _dbContext.CmsOperatingSectorTypesRoles.Add(sectorRole);
                    }
                    var result = await _dbContext.SaveChangesAsync();
                    return result > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region Service Request Approval ( CRUD )

        public async Task<bool> AddServiceRequestApproval(ServiceRequestFinalApprovalVM srFinalApprovalVm)
        {
            try
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    int approvaleSequenceNo = 0;
                    var finalApprovals = new List<ServiceRequestFinalApproval>();
                    foreach (var reqTypeId in srFinalApprovalVm.ServiceRequestTypesId)
                    {
                        var finalApproval = new ServiceRequestFinalApproval();
                        finalApproval.ServiceRequestTypeId = reqTypeId;
                        finalApproval.NoOfApprovals = srFinalApprovalVm.NoOfApproval;
                        finalApproval.CreatedBy = srFinalApprovalVm.CreatedBy;
                        finalApproval.CreatedDate = srFinalApprovalVm.CreatedDate;
                        finalApproval.IsActive = true;
                        _dbContext.ServiceRequestFinalApprovals.Add(finalApproval);
                        await _dbContext.SaveChangesAsync();

                        var IsFinalActivitesSave = await AddServieFinalApprovalActivites(srFinalApprovalVm, finalApproval.Id, 1);
                        if (!IsFinalActivitesSave)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                    transaction.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> AddServiceRequestApproval1(ServiceRequestFinalApprovalVM srFinalApprovalVm)
        {
            try
            {
                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    int approvalSequenceNo = 0;
                    var finalApprovals = new List<ServiceRequestFinalApproval>();
                    var finalApprovalActivities = new List<ServiceRequestFinalApprovalActivities>();

                    foreach (var reqTypeId in srFinalApprovalVm.ServiceRequestTypesId)
                    {
                        var finalApproval = new ServiceRequestFinalApproval
                        {
                            ServiceRequestTypeId = reqTypeId,
                            NoOfApprovals = srFinalApprovalVm.NoOfApproval,
                            CreatedBy = srFinalApprovalVm.CreatedBy,
                            CreatedDate = srFinalApprovalVm.CreatedDate,
                            IsActive = true
                        };

                        // Iterate over the selected sector and roles
                        foreach (var sectorRoleId in srFinalApprovalVm.SelectedSectorAndRoles)
                        {
                            var idSepa = sectorRoleId.Split(',');
                            int sectId = int.Parse(idSepa[0]);

                            // Check if approval flow exists
                            bool isFlowExist = await IsApprovalFlowExistForServiceReq(reqTypeId, srFinalApprovalVm.DepartmentId, sectId);
                            if (!isFlowExist)
                            {
                                // Add the final approval only if the flow doesn't exist
                                _dbContext.ServiceRequestFinalApprovals.Add(finalApproval);
                                await _dbContext.SaveChangesAsync();  // Save the final approval

                                // Add related approval activities
                                foreach (var srId in srFinalApprovalVm.SelectedSectorAndRoles)
                                {
                                    approvalSequenceNo++;

                                    var idSeparations = srId.Split(',');
                                    var finalApprovalActivity = new ServiceRequestFinalApprovalActivities
                                    {
                                        SectorTypeId = int.Parse(idSeparations[0]),
                                        // RoleId = Guid.Parse(idSeparations[1]),
                                        DepartmentId = srFinalApprovalVm.DepartmentId,
                                        ApprovalSequenceNo = approvalSequenceNo,
                                        FinalApprovalId = finalApproval.Id
                                    };

                                    finalApprovalActivities.Add(finalApprovalActivity);
                                }

                                // Batch insert all final approval activities after the inner loop
                                _dbContext.ServiceRequestFinalApprovalActivities.AddRange(finalApprovalActivities);
                                await _dbContext.SaveChangesAsync();  // Save activities in batch
                            }
                        }
                    }

                    await transaction.CommitAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                await _dbContext.Database.RollbackTransactionAsync();  // Rollback in case of an exception
                throw;
            }
        }

        public async Task<bool> UpdateServiceRequestApproval(ServiceRequestFinalApprovalVM serviceRequestApproval)
        {
            try
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    var finalApproval = await _dbContext.ServiceRequestFinalApprovals.Where(x => x.Id == serviceRequestApproval.ApprovalId).FirstOrDefaultAsync();
                    if (finalApproval is not null)
                    {
                        finalApproval.NoOfApprovals = serviceRequestApproval.NoOfApproval;
                        finalApproval.ModifiedBy = serviceRequestApproval.ModifiedBy;
                        finalApproval.ModifiedDate = serviceRequestApproval.ModifiedDate;
                        await _dbContext.ServiceRequestFinalApprovals.AddAsync(finalApproval);
                        _dbContext.Entry(finalApproval).State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();
                    }
                    bool isStatusUpdated = await UpdateIsActiveStatusAsync(finalApproval.Id);
                    if (!isStatusUpdated)
                    {
                        transaction.Rollback();
                        return false;
                    }
                    int versionId = await _dbContext.ServiceRequestFinalApprovalActivities.Where(x => x.FinalApprovalId == finalApproval.Id).Select(x => x.VersionId).MaxAsync();

                    var IsFinalActivitesSave = await AddServieFinalApprovalActivites(serviceRequestApproval, finalApproval.Id, versionId + 1);
                    if (!IsFinalActivitesSave)
                    {
                        transaction.Rollback();
                        return false;
                    }
                    transaction.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<ServiceRequestApprovalDetailVm> GetServiceRequestApprovalDetail(int Id)
        {
            try
            {
                var storproce = $"exec pGetServiceRequestApprovalFlowList @approvalId = {Id}";
                var result = await _dbContext.ServiceRequestApprovalDetailVms.FromSqlRaw(storproce).ToListAsync();
                if (result.Any())
                {
                    return result.FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<ServiceRequestApprovalDetailVm>> GetAllServiceRequestApprovalList()
        {
            try
            {
                var storproce = $"exec pGetServiceRequestApprovalFlowList @approvalId = {0}";
                var result = await _dbContext.ServiceRequestApprovalDetailVms.FromSqlRaw(storproce).ToListAsync();
                return result != null ? result : null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<ServiceRequestApprovalHistoryVm>> GetServiceRequestApprovalHistory(int approvalId)
        {
            try
            {
                string storeproc = $"exec pGetServiceRequestApprovalHistory @approvalId = {approvalId}";
                var result = await _dbContext.ServiceRequestApprovalHistoryVms.FromSqlRaw(storeproc).AsNoTracking().ToListAsync();
                return result.Any() ? result : new();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected async Task<bool> AddServieFinalApprovalActivites(ServiceRequestFinalApprovalVM srFinalApprovalVm, int finalApprovalId, int versionId)
        {
            int approvaleSequenceNo = 0;
            List<ServiceRequestFinalApprovalActivities> finalApprovalActivityList = new List<ServiceRequestFinalApprovalActivities>();
            foreach (var srId in srFinalApprovalVm.SelectedSectorAndRoles)
            {
                approvaleSequenceNo++;
                var finalApprovalActivity = new ServiceRequestFinalApprovalActivities();
                var idSeparations = srId.Split(',');
                finalApprovalActivity.SectorTypeId = int.Parse(idSeparations[0]);
                finalApprovalActivity.RoleId = idSeparations[1];
                finalApprovalActivity.ApprovalSequenceNo = approvaleSequenceNo;
                finalApprovalActivity.FinalApprovalId = finalApprovalId;
                finalApprovalActivity.DepartmentId = srFinalApprovalVm.DepartmentId;
                finalApprovalActivity.VersionId = versionId;
                finalApprovalActivity.IsActive = true;
                finalApprovalActivity.CreatedBy = srFinalApprovalVm.CreatedBy;
                finalApprovalActivity.CreatedDate = srFinalApprovalVm.CreatedDate;
                finalApprovalActivity.FinalApprovalId = finalApprovalId;
                finalApprovalActivityList.Add(finalApprovalActivity);
            }
            _dbContext.ServiceRequestFinalApprovalActivities.AddRange(finalApprovalActivityList);
            int IsApprovalActivitiesSave = await _dbContext.SaveChangesAsync();
            return IsApprovalActivitiesSave > 0 ? true : false;
        }

        protected async Task<bool> UpdateIsActiveStatusAsync(int finalApprovalId)
        {
            List<ServiceRequestFinalApprovalActivities> serviceRequestFinalApprovalActivities = new();
            var result = await _dbContext.ServiceRequestFinalApprovalActivities.Where(x => x.FinalApprovalId == finalApprovalId).ToListAsync();
            foreach (var approval in result)
            {
                approval.IsActive = false;
                serviceRequestFinalApprovalActivities.Add(approval);
            }
            _dbContext.ServiceRequestFinalApprovalActivities.UpdateRange(serviceRequestFinalApprovalActivities);
            var IsUpdated = await _dbContext.SaveChangesAsync();
            return IsUpdated > 0 ? true : false;
        }
        protected async Task<bool> IsApprovalFlowExistForServiceReq(int ServiceReqId, int deptId, int sectortypeId)
        {
            try
            {
                var isApprovalFlowExist = await (from sa in _dbContext.ServiceRequestFinalApprovals
                                                 join sra in _dbContext.ServiceRequestFinalApprovalActivities
                                                 on sa.Id equals sra.FinalApprovalId
                                                 where sa.ServiceRequestTypeId == ServiceReqId
                                                 && sra.SectorTypeId == sectortypeId
                                                 && sra.DepartmentId == deptId
                                                 select sa).AnyAsync();

                return isApprovalFlowExist;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //protected async Task<bool> IsApprovalFlowExistForServiceReq(int ServiceReqId, int deptId, int sectortypeId)
        //{
        //    try
        //    {
        //        string proc = $"exec pCheckApprovalFlowExistForServiceReq @serviceReqId = {ServiceReqId},  @sectorId = {sectortypeId} @departmentId = {deptId}";
        //        //   bool isExist = await   
        //        //var srId = await _dbContext.ServiceRequestFinalApprovals.Where(x => x.ServiceRequestTypeId == ServiceReqId).Select(x =>  new { x.ServiceRequestTypeId, x.Id }).FirstOrDefaultAsync();
        //        var approvalId = await _dbContext.ServiceRequestFinalApprovals.Where(x => x.ServiceRequestTypeId == ServiceReqId).Select(x => x.Id).FirstOrDefaultAsync();
        //        var sectorId = await _dbContext.ServiceRequestFinalApprovalActivities.Where(x => x.FinalApprovalId == approvalId && x.DepartmentId == deptId && x.SectorTypeId == sectortypeId).Select(x => x.SectorTypeId).FirstOrDefaultAsync();
        //        return sectorId > 0 ? true : false;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        #endregion

        #region

        public async Task<List<MobileAppCourtVM>> GetCourtByUserIdForMobileApp(string userId)
        {
            try
            {
                string StoredProc = $"exec pMobileAppGetCourtByUserId @userId='{userId}'";
                var result = await _dbContext.MobileAppCourtVM.FromSqlRaw(StoredProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MobileAppChamberVM>> GetChambersByUserIdForMobileApp(int courtId, string userId)
        {
            try
            {
                string StoredProc = $"exec pMobileAppGetChamberByUserId @courtId='{courtId}', @userId='{userId}'";
                var result = await _dbContext.MobileAppChamberVM.FromSqlRaw(StoredProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MobileAppChamberNumberVM>> GetChamberNumberByUserIdForMobileApp(int courtId, int chamberId, string userId)
        {
            try
            {
                string StoredProc = $"exec pMobileAppGetChamberNumberByUserId @courtId='{courtId}', @chamberId='{chamberId}', @userId='{userId}'";
                return await _dbContext.MobileAppChamberNumberVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region GetStockTakingStatus
        public async Task<List<LmsStockTakingStatus>> GetStockTakingStatus()
        {
            try
            {
                var result = await _dbContext.StockTakingStatuses.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        public async Task<List<PreCourtType>> GetPreCourtTypes()
        {
            try
            {
                return await _dbContext.PreCourtTypes.OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CourtType>> GetCourtTypesByRequestType(int requestTypeId)
        {
            try
            {
                List<CourtType> courtTypes = new List<CourtType>();
                courtTypes = await _dbContext.CourtTypes.Where(u => u.IsDeleted == false && u.IsActive == true).OrderBy(u => u.Id).ToListAsync();
                return courtTypes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region 
        //<History Author = 'Muhammad Zaeem' Date='2024-10-10' Version="1.0" Branch="master"> Get consultation legislation file type Details</History>
        public async Task<List<ConsultationLegislationFileType>> GetConsultationLegislationFileTypes()
        {
            try
            {
                return await _dbContext.ConsultationLegislationFileTypes.Where(u => u.IsDeleted == false && u.IsActive == true).OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        public async Task<List<ComsInternationalArbitrationType>> GetConsultationInternationalArbitrationTypes()
        {
            try
            {
                return await _dbContext.ComsInternationalArbitrationTypes.Where(u => u.IsDeleted == false && u.IsActive == true).OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Get Manager Task Reminder Data
        public async Task<List<ManagerTaskReminderVM>> GetManagerTaskReminderData(Guid TaskId)
        {
            try
            {
                string storProc = $"pGetManagerTaskReminderData @TaskId ='{TaskId}'";
                var result = await _dbContext.ManagerTaskReminderVMs.FromSqlRaw(storProc).ToListAsync();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new List<ManagerTaskReminderVM>();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion
    }
}
