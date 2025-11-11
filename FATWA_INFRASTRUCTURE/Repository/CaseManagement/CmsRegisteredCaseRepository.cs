using AutoMapper;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.CaseManagement;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.MobileAppVMs;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;

namespace FATWA_INFRASTRUCTURE.Repository.CaseManagement
{
    //<!-- <History Author = 'Hassan Abbas' Date='2022-11-08' Version="1.0" Branch="master">Repo for Registered Case operations</History> -->
    public class CmsRegisteredCaseRepository : ICmsRegisteredCase
    {
        #region Variables declaration
        private readonly DatabaseContext _dbContext;
        private readonly DmsDbContext _dmsDbContext;
        private List<CmsRegisteredCaseDetailVM> CmsRegisteredCaseDetailVMs;
        private List<CaseDetailMOJVM> caseDetailMOJVMs;
        private List<CmsRegisteredCaseVM> _CmsRegisteredCaseVMs;
        private List<CmsJugdmentDecisionVM> _CmsJugdmentDecisionVMs;
        private MojRequestForDocument _MojRequestForDocument;
        private List<MergeRequestVM> CmsMergeRequestVMs;
        private List<CmsJudgmentExecutionVM> CmsJudgmentExecutionVMs;

        private List<CmsRegisteredCase> cmsRegisteredCases;
        private List<HearingVM> HearingVMs;
        private List<OutcomeHearingVM> OutcomeHearingVMs;
        private List<OutcomeAndHearingVM> OutcomeAndHearingVMs;
        private List<JudgementVM> JudgementVMs;
        private List<TransferHistoryVM> TransferHistoryVMs;
        private List<CmsRegisteredCaseStatusHistoryVM> CmsRegisteredCaseStatusHistoryVMs;
        private List<CmsRequestDocumentsVM> cmsRequestDocumentsVMs;
        private List<SchedulingCourtVisitVM> SchedulingCourtVisitVMs;
        private readonly CMSCaseRequestRepository _caseRequestRepository;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public CmsJudgmentExecution cmsJudgmentExecution { get; set; }
        private List<CmsCaseRequestResponseVM> _CmsCaseRequestResponseVMs;
        private CmsJudgementDetailVM _CmsJudgementDetailVM;
        private List<CaseOutcomePartyLinkHistoryVM> _casePartyOutcomeHistory;
        private List<CmsRegisteredCase> _CmsRegisteredCases;
        private readonly IMapper _mapper;
        private List<CmsRegisteredCaseTransferRequestVM> cmsRegisteredCaseTransferRequestListVMs;
        private CmsRegisteredCaseTransferRequestVM _CmsRegisteredCaseTransferRequestDetailVMs;

        #endregion

        #region Constructor
        public CmsRegisteredCaseRepository(DatabaseContext dbContext, DmsDbContext dmsdbContext, IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            _dbContext = dbContext;
            _dmsDbContext = dmsdbContext;
            _serviceScopeFactory = serviceScopeFactory;
            using var scope = _serviceScopeFactory.CreateScope();
            _caseRequestRepository = scope.ServiceProvider.GetRequiredService<CMSCaseRequestRepository>();
            _mapper = mapper;

        }

        #endregion

        #region Get All Registered Cases By Sector/Court Type
        //<History Author = 'Hassan Abbas' Date='2023-08-03' Version="1.0" Branch="master"> Get All Registered Cases By Sector/CourtType</History>
        public async Task<List<CmsRegisteredCaseDmsVM>> GetAllRegisteredCasesByCourtTypeId(int courtTypeId, string userId)
        {
            try
            {
                List<CmsRegisteredCaseDmsVM> _CmsRegisteredCases;
                string StoredProc = $"exec pCmsRegisteredCasesListForDms @courtTypeId ='{courtTypeId}', @userId = '{userId}'";
                _CmsRegisteredCases = await _dbContext.CmsRegisteredCaseDmsVMs.FromSqlRaw(StoredProc).ToListAsync();
                return _CmsRegisteredCases;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Registered Case detail by Id

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master">Get Registered Case detail by Id </History>
        public async Task<CmsRegisteredCaseDetailVM> GetRegisteredCaseDetailByIdVM(Guid caseId, string userId)
        {
            try
            {
                var CmsRegisteredCaseDetail = new CmsRegisteredCaseDetailVM();

                if (CmsRegisteredCaseDetailVMs == null)
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                    using (_DbContext)
                    {
                        string StoredProc = $"exec pCmsRegisteredCaseDetailById @caseId ='{caseId}', @userId ='{userId}' ";

                        CmsRegisteredCaseDetailVMs = await _DbContext.CmsRegisteredCaseDetailVM.FromSqlRaw(StoredProc).ToListAsync();
                        CmsRegisteredCaseDetail = CmsRegisteredCaseDetailVMs.FirstOrDefault();
                        if (CmsRegisteredCaseDetail != null)
                            CmsRegisteredCaseDetail.IsMojExecutionRequest = _DbContext.MojExecutionRequest.Where(x => x.CaseId == caseId).Any();
                    }
                }
                return CmsRegisteredCaseDetail;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get Hearings by Case Id

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master">Get Hearing By Case Id </History>
        public async Task<List<HearingVM>> GetHearingsByCase(Guid caseId)
        {
            try
            {
                if (HearingVMs == null)
                {
                    string StoredProc = $"exec pHearingListByCase @caseId ='{caseId}' ";

                    HearingVMs = await _dbContext.HearingVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return HearingVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get Hearing Detail

        //<History Author = 'Hassan Abbas' Date='2023-11-21' Version="1.0" Branch="master"> Get Hearing detail</History>
        public async Task<HearingDetailVM> GetHearingDetail(Guid hearingId)
        {
            try
            {
                string StoredProc = $"exec pCmsHearingDetailById @hearingId ='{hearingId}' ";
                var result = await _dbContext.HearingDetailVM.FromSqlRaw(StoredProc).ToListAsync();
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Hearing> GetHearingDetailByHearingId(Guid hearingId)
        {
            try
            {
                var result = await _dbContext.Hearings.Where(x => x.Id == hearingId).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Execution Detail by Execution Id
        public async Task<CmsJudgmentExecutionDetailVM> GetExecutionDetail(Guid executionId)
        {
            try
            {
                string StoredProc = $"exec pGetExecutionDetailById @Id ='{executionId}' ";
                var result = await _dbContext.CmsJudgmentExecutionDetailVMs.FromSqlRaw(StoredProc).ToListAsync();
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Outcomes of a Hearings by Case Id

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master">Get Outcomes of a Hearings by Case Id </History>
        public async Task<List<OutcomeHearingVM>> GetOutcomesHearingByCase(Guid caseId)
        {
            try
            {
                if (OutcomeHearingVMs == null)
                {
                    string StoredProc = $"exec pOutcomeHearingListByCase @caseId ='{caseId}' ";

                    OutcomeHearingVMs = await _dbContext.OutcomeHearingVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return OutcomeHearingVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get Outcomes and hearing combined list  by Case Id

        //<History Author = 'Muhammad Zaeem' Date='2024-10-3' Version="1.0" Branch="master">Get Outcomes and  Hearings by Case Id </History>
        public async Task<List<OutcomeAndHearingVM>> GetOutcomesAndHearingByCase(Guid caseId)
        {
            try
            {
                if (OutcomeAndHearingVMs == null)
                {
                    string StoredProc = $"exec pOutcomeAndHearingListByCaseId @caseId ='{caseId}' ";

                    OutcomeAndHearingVMs = await _dbContext.OutcomeAndHearingVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return OutcomeAndHearingVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get current and previous hearings against LawyerId
        //<History Author = 'Ammaar Naveed' Date='2024-04-03' Version="1.0" Branch="master"> Get upcoming + previous hearings of lawyer on condition</History>
        //<History Author = 'Ammaar Naveed' Date='2024-03-26' Version="1.0" Branch="master">Get upcoming hearings against lawyerId</History>
        public async Task<List<OutcomeAndHearingVM>> GetHearingsOfLawyer(string LawyerId, bool isPrevious)
        {
            try
            {
                if (OutcomeAndHearingVMs == null)
                {
                    string StoredProc = $"EXEC pCmsHearingsList @LawyerId='{LawyerId}', @IsPrevious='{(isPrevious ? 1 : 0)}'";

                    OutcomeAndHearingVMs = await _dbContext.OutcomeAndHearingVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return OutcomeAndHearingVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Outcome Hearing Detail by Outcome Id

        //<History Author = 'Hassan Abbas' Date='2023-11-25' Version="1.0" Branch="master"> Get Outcome detail</History>
        public async Task<OutcomeHearingDetailVM> GetOutcomeDetail(Guid outcomeId)
        {
            try
            {
                string StoredProc = $"exec pCmsOutcomeDetailById @outcomeId ='{outcomeId}' ";
                var result = await _dbContext.OutcomeHearingDetailVMs.FromSqlRaw(StoredProc).ToListAsync();
                if (result.Count > 0)
                {
                    return result.FirstOrDefault();

                }
                else
                {
                    return new OutcomeHearingDetailVM();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get Judgements by Case Id

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master">Get Judgements by Case Id </History>
        public async Task<List<JudgementVM>> GetJudgementsByCase(Guid caseId)
        {
            try
            {
                if (JudgementVMs == null)
                {
                    string StoredProc = $"exec pJudgementsByCaseId @caseId ='{caseId}'";
                    JudgementVMs = await _dbContext.JudgementVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return JudgementVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get Judgements by Outcome Id

        //<History Author = 'Hassan Abbas' Date='2023-12-06' Version="1.0" Branch="master"> Get Judgements By Outcome</History>
        public async Task<List<JudgementVM>> GetJudgementsByOutcome(Guid outcomeId)
        {
            try
            {
                if (JudgementVMs == null)
                {
                    string StoredProc = $"exec pCmsJudgementsByOutcomeId @outcomeId ='{outcomeId}' ";

                    JudgementVMs = await _dbContext.JudgementVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return JudgementVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Transfer History
        public async Task<List<TransferHistoryVM>> GetTransferHistoryByOutcome(Guid outcomeId, Guid CaseId)
        {
            try
            {
                if (TransferHistoryVMs == null)
                {
                    string StoredProc = $"exec pCmsTransferHistoryByOutcomeId @outcomeId ='{outcomeId}',@caseId='{CaseId}' ";
                    TransferHistoryVMs = await _dbContext.TransferHistoryVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return TransferHistoryVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Registered Case history status

        //<History Author = 'Hassan Abbas' Date='2022-12-10' Version="1.0" Branch="master"> Get Registered Case status history</History>
        public async Task<List<CmsRegisteredCaseStatusHistoryVM>> GetRegisteredCaseStatusHistory(Guid caseId)
        {
            try
            {
                if (CmsRegisteredCaseStatusHistoryVMs == null)
                {
                    string StoredProc = $"exec pRegisteredCaseStatusHistory @caseId = N'{caseId}'";

                    CmsRegisteredCaseStatusHistoryVMs = await _dbContext.CmsRegisteredCaseStatusHistoryVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return CmsRegisteredCaseStatusHistoryVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get Merge Request detail by Id

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master">Get Merge Request detail by Id </History>
        public async Task<MergeRequestVM> GetMergeRequestDetailById(Guid id)
        {
            try
            {
                if (CmsMergeRequestVMs == null)
                {
                    string StoredProc = $"exec pMergeRequestDetailById @id ='{id}' ";

                    CmsMergeRequestVMs = await _dbContext.CmsMergeRequestVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return CmsMergeRequestVMs.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get Merged Cases By MErge Request
        //<History Author = 'Hassan Abbas' Date='2022-11-29' Version="1.0" Branch="master"> Get Merged Cases By Merge Request</History>
        public async Task<List<CmsRegisteredCaseVM>> GetMergedCasesByMergeRequestId(Guid mergeRequestId)
        {
            try
            {
                if (_CmsRegisteredCaseVMs == null)
                {
                    string StoredProc = $"exec pMergedCasesByMergeRequestId @mergeRequestId ='{mergeRequestId}' ";

                    _CmsRegisteredCaseVMs = await _dbContext.CmsRegisteredCaseVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CmsRegisteredCaseVMs;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Merge Requests

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master">Get Registered Case detail by Id </History>
        public async Task<List<MergeRequestVM>> GetMergeRequestsForApproval()
        {
            try
            {
                if (CmsMergeRequestVMs == null)
                {
                    string StoredProc = $"exec pMergeListForApproval";

                    CmsMergeRequestVMs = await _dbContext.CmsMergeRequestVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return CmsMergeRequestVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get Registered Case  by Id

        //<History Author = 'Ijaz Ahmad' Date='2022-01-02' Version="1.0" Branch="master">Get Registered Case  by Id </History>
        public async Task<CmsRegisteredCase> GetRegisteredCaseById(Guid caseId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                using (_DbContext)
                {
                    CmsRegisteredCase entity = await _DbContext.cmsRegisteredCases.FindAsync(caseId);
                    if (entity == null)
                    {
                        throw new ArgumentNullException();
                    }
                    return entity;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        #endregion

        #region save registered case  status history
        public async Task SaveRegisteredCaseStatusHistory(CmsRegisteredCase registeredCase, int EventId, DatabaseContext dbContext, string remarks = "", string username = "")
        {
            try
            {
                CmsRegisteredCaseStatusHistory historyobj = new CmsRegisteredCaseStatusHistory();
                historyobj.HistoryId = Guid.NewGuid();
                historyobj.CaseId = registeredCase.CaseId;
                historyobj.FileId = registeredCase.FileId;
                historyobj.StatusId = (int)registeredCase.StatusId;
                historyobj.CreatedDate = DateTime.Now;
                historyobj.CreatedBy = String.IsNullOrEmpty(username) ? registeredCase.CreatedBy : username;
                historyobj.EventId = EventId;
                historyobj.Remarks = remarks;
                await dbContext.CmsRegisteredCaseStatusHistory.AddAsync(historyobj);
                await dbContext.SaveChangesAsync();
                registeredCase.RegisteredCaseHistory = historyobj;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task SaveCMSRegisteredCaseTransferHistory(CMSRegisteredCaseTransferHistoryVM cMSRegisteredCaseTransferHistoryVM, DatabaseContext dbContext)
        {
            try
            {
                CMSRegisteredCaseTransferHistory transferHistoryobj = new CMSRegisteredCaseTransferHistory();
                transferHistoryobj.Id = Guid.NewGuid();
                transferHistoryobj.ChamberFromId = cMSRegisteredCaseTransferHistoryVM.ChamberFromId;
                transferHistoryobj.ChamberToId = cMSRegisteredCaseTransferHistoryVM.ChamberToId;
                transferHistoryobj.ChamberNumberFromId = cMSRegisteredCaseTransferHistoryVM.ChamberNumberFromId;
                transferHistoryobj.ChamberNumberToId = cMSRegisteredCaseTransferHistoryVM.ChamberNumberToId;
                transferHistoryobj.OutcomeId = cMSRegisteredCaseTransferHistoryVM.OutcomeId;
                transferHistoryobj.CreatedDate = DateTime.Now;
                transferHistoryobj.CreatedBy = cMSRegisteredCaseTransferHistoryVM.createdBy;
                await dbContext.CmsRegisteredCaseTransferHistory.AddAsync(transferHistoryobj);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Save Case File Assignment to lawyer
        public async Task SaveRegisteredCaseFileAssignmentToLawyer(Guid referenceId, CaseAssignment caseRequestLawyerAssignment, DatabaseContext dbcontext)
        {
            try
            {
                dbcontext.CaseFileAssignment.RemoveRange(dbcontext.CaseFileAssignment.Where(p => p.ReferenceId == referenceId));
                await dbcontext.SaveChangesAsync();
                if ((bool)caseRequestLawyerAssignment.SelectedUsers?.Any())
                {
                    foreach (var lawyer in caseRequestLawyerAssignment.SelectedUsers)
                    {
                        CaseAssignment lawyerObj = new CaseAssignment();
                        lawyerObj.ReferenceId = referenceId;
                        lawyerObj.SupervisorId = caseRequestLawyerAssignment.SupervisorId;
                        lawyerObj.Remarks = caseRequestLawyerAssignment.Remarks;
                        lawyerObj.LawyerId = lawyer.Id;
                        if (lawyer.Id == caseRequestLawyerAssignment.PrimaryLawyerId)
                        {
                            lawyerObj.IsPrimary = true;
                        }
                        lawyerObj.CreatedBy = caseRequestLawyerAssignment.CreatedBy;
                        lawyerObj.CreatedDate = caseRequestLawyerAssignment.CreatedDate;

                        await dbcontext.CaseFileAssignment.AddAsync(lawyerObj);
                        await dbcontext.SaveChangesAsync();
                        //Save Assignment History Lawyer
                        await SaveCaseFileAssignmentHistory(referenceId, caseRequestLawyerAssignment.CreatedBy, lawyer.Id, caseRequestLawyerAssignment.Remarks, dbcontext);
                    }

                    //Save Assignment History Supervisor
                    await SaveCaseFileAssignmentHistory(referenceId, caseRequestLawyerAssignment.CreatedBy, caseRequestLawyerAssignment.SupervisorId, caseRequestLawyerAssignment.Remarks, dbcontext);
                }
                else
                {
                    CaseAssignment lawyerObj = new CaseAssignment();
                    lawyerObj.ReferenceId = referenceId;
                    lawyerObj.SupervisorId = caseRequestLawyerAssignment.SupervisorId;
                    lawyerObj.Remarks = caseRequestLawyerAssignment.Remarks;
                    lawyerObj.LawyerId = caseRequestLawyerAssignment.LawyerId;
                    lawyerObj.IsPrimary = true;
                    lawyerObj.CreatedBy = caseRequestLawyerAssignment.CreatedBy;
                    lawyerObj.CreatedDate = caseRequestLawyerAssignment.CreatedDate;
                    await dbcontext.CaseFileAssignment.AddAsync(lawyerObj);
                    await dbcontext.SaveChangesAsync();

                    //Save Assignment History Lawyer and Supervisor
                    await SaveCaseFileAssignmentHistory(referenceId, caseRequestLawyerAssignment.CreatedBy, caseRequestLawyerAssignment.LawyerId, caseRequestLawyerAssignment.Remarks, dbcontext);
                    await SaveCaseFileAssignmentHistory(referenceId, caseRequestLawyerAssignment.CreatedBy, caseRequestLawyerAssignment.SupervisorId, caseRequestLawyerAssignment.Remarks, dbcontext);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Save Registered Case File Assignment history
        public async Task SaveCaseFileAssignmentHistory(Guid referenceId, string userId, string assigneeId, string remarks, DatabaseContext dbcontext)
        {
            try
            {
                CaseAssignmentHistory historyObj = new CaseAssignmentHistory();
                historyObj.ReferenceId = referenceId;
                historyObj.AssigneeId = assigneeId;
                historyObj.Remarks = remarks;
                historyObj.CreatedBy = userId;
                historyObj.CreatedDate = DateTime.Now;
                await dbcontext.CaseFileAssignmentHistory.AddAsync(historyObj);
                await dbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Add Hearing for a case

        //<History Author = 'Hassan Abbas' Date='2022-11-21' Version="1.0" Branch="master">Add hearing for a case </History>
        //<History Author = 'Hassan Abbas' Date='2023-03-23' Version="1.0" Branch="master">Send Portfolio Request to MOJ for Hearing </History>
        public async Task<bool> AddHearing(Hearing hearing)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (hearing.IsUpdated == true)
                        {
                            _dbContext.Hearings.Update(hearing);
                            await _dbContext.SaveChangesAsync();
                        }
                        else
                        {
                            await _dbContext.Hearings.AddAsync(hearing);
                            await _dbContext.SaveChangesAsync();
                            CmsRegisteredCase registeredCase = await _dbContext.CmsRegisteredCases.FindAsync(hearing.CaseId);
                            await SaveRegisteredCaseStatusHistory(registeredCase, (int)RegisteredCaseEventEnum.HearingScheduled, _dbContext, hearing.Description, hearing.CreatedBy);
                        }
                        transaction.Commit();
                        return true;
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

        #region Add Outcome of Hearing
        //<History Author = 'Hassan Abbas' Date='2022-11-21' Version="1.0" Branch="master">Add outcome of hearing for a case </History>
        //<History Author = 'Muhammad Zaeem' Date='2023-12-28' Version="1.0" Branch="master">Add Case parties in the outcome and also the attachments with each party and aslo delete the selected the parties and save the record in outcome party link history</History>
        public async Task AddOutcomeHearing(OutcomeHearing outcomeHearing)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        CmsRegisteredCase registeredCase = await _dbContext.CmsRegisteredCases.FindAsync(outcomeHearing.CaseId);
                        if (outcomeHearing.IsExist)
                        {
                            _dbContext.OutcomeHearings.Update(outcomeHearing);
                            await _dbContext.SaveChangesAsync();
                        }
                        else
                        {
                            Hearing hearing = await _dbContext.Hearings.FindAsync(outcomeHearing.HearingId);
                            hearing.StatusId = (int)HearingStatusEnum.HearingAttended;
                            _dbContext.Hearings.Update(hearing);
                            await _dbContext.OutcomeHearings.AddAsync(outcomeHearing);
                            await _dbContext.SaveChangesAsync();
                            await SaveRegisteredCaseStatusHistory(registeredCase, (int)RegisteredCaseEventEnum.HearingAttended, _dbContext, "", outcomeHearing.CreatedBy);
                            await SaveRegisteredCaseStatusHistory(registeredCase, (int)RegisteredCaseEventEnum.OutcomeAdded, _dbContext, outcomeHearing.Remarks, outcomeHearing.CreatedBy);

                        }
                        if (outcomeHearing.caseParties.Count > 0)
                        {
                            foreach (var outcomeParticipant in outcomeHearing.caseParties)
                            {
                                outcomeParticipant.CreatedBy = outcomeHearing.CreatedBy;
                                var partyResult = await SaveCaseParty(outcomeParticipant, _dbContext);
                                if (partyResult)
                                {
                                    await SaveCasePartyAttachment(outcomeParticipant, _dmsDbContext);
                                    await SaveOutcomePartyHistory(outcomeHearing.Id, outcomeParticipant.Id, (int)CaseOutcomePartyActionEnum.Added, outcomeHearing.CreatedBy, _dbContext);
                                }
                            }
                        }
                        if (outcomeHearing.DeletedParties.Count > 0)
                        {
                            foreach (var deletedParties in outcomeHearing.DeletedParties)
                            {
                                deletedParties.DeletedBy = outcomeHearing.CreatedBy;
                                var result = await DeleteCaseParty(deletedParties, _dbContext);
                                if (result)
                                {
                                    await SaveOutcomePartyHistory(outcomeHearing.Id, deletedParties.Id, (int)CaseOutcomePartyActionEnum.Deleted, outcomeHearing.CreatedBy, _dbContext);
                                }
                            }
                        }
                        if (outcomeHearing.outcomeJudgement.Count > 0)
                        {
                            List<Judgement> judgement = new List<Judgement>();
                            judgement = _mapper.Map<List<Judgement>>(outcomeHearing.outcomeJudgement);
                            foreach (var outcomeJudgement in judgement)
                            {

                                if (outcomeJudgement.IsUpdated == true)
                                {
                                    _dbContext.Judgements.Update(outcomeJudgement);
                                    await _dbContext.SaveChangesAsync();
                                }
                                else
                                {
                                    await _dbContext.Judgements.AddAsync(outcomeJudgement);
                                    await _dbContext.SaveChangesAsync();
                                    await SaveRegisteredCaseStatusHistory(registeredCase, outcomeJudgement.IsFinal ? (int)RegisteredCaseEventEnum.FinalJudgementAdded : (int)RegisteredCaseEventEnum.JudgementAdded, _dbContext, outcomeJudgement.Remarks, outcomeJudgement.CreatedBy);
                                }
                            }
                            foreach (var item in outcomeHearing.outcomeJudgement)
                            {
                                if (item.mojExecutionRequest != null)
                                {
                                    await AddExecutionRequest(item.mojExecutionRequest);
                                }
                            }
                        }
                        if (outcomeHearing.caseTransferRequestsVM.Any(x => x.IsAlreadyExist == false))
                        {
                            foreach (var caseTransferRequest in outcomeHearing.caseTransferRequestsVM)
                            {
                                await AddCaseTransferRequest(caseTransferRequest);
                            }
                        }
                        transaction.Commit();
                        //For Notification
                        outcomeHearing.NotificationParameter.Entity = "Case";
                        outcomeHearing.NotificationParameter.CaseNumber = registeredCase.CaseNumber;
                        outcomeHearing.NotificationParameter.FileNumber = _dbContext.CaseFiles.Where(x => x.FileId == registeredCase.FileId).FirstOrDefault().FileNumber;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        #endregion

        #region cmsre Registered Case 

        //<History Author = 'Hassan Abbas' Date='2022-11-21' Version="1.0" Branch="master">Create Regsitered Case </History>
        public async Task<CmsRegisteredCase> CreateRegisteredCase(CmsRegisteredCase registeredCase)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.CmsRegisteredCases.Add(registeredCase);
                        await _dbContext.SaveChangesAsync();
                        await SaveRegisteredCaseStatusHistory(registeredCase, (int)RegisteredCaseStatusEnum.Open, _dbContext);

                        var copyAttachments = await CopyPartiesFromCaseFile(registeredCase, _dbContext);
                        if (copyAttachments.Any())
                        {
                            registeredCase.CopyAttachmentVMs.AddRange(copyAttachments);
                        }
                        await UpdateCaseFileStatusAfterMojRegistration(registeredCase, (int)CaseFileStatusEnum.RegisteredInMoj, _dbContext);
                        await UpdateMojRegistrationRequestStatus((Guid)registeredCase.MojRegistrationRequestId, _dbContext);
                        // Add Hearing
                        Hearing hearing = new Hearing();
                        hearing.CaseId = registeredCase.CaseId;
                        hearing.HearingDate = registeredCase.HearingDate;
                        hearing.HearingTime = registeredCase.HearingTime;
                        hearing.StatusId = (int)HearingStatusEnum.HearingScheduled;
                        hearing.CreatedDate = registeredCase.CreatedDate;
                        hearing.CreatedBy = registeredCase.CreatedBy;
                        await _dbContext.Hearings.AddAsync(hearing);
                        await _dbContext.SaveChangesAsync();
                        transaction.Commit();
                        registeredCase.Hearing = hearing;

                        // For Notification
                        registeredCase.NotificationParameter.FileNumber = _dbContext.CaseFiles.Where(x => x.FileId == registeredCase.FileId).FirstOrDefault().FileNumber;
                        registeredCase.NotificationParameter.CaseNumber = registeredCase.CaseNumber;

                        return registeredCase;

                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-28' Version="1.0" Branch="master">Save Registered Case Attachments </History>
        private async Task SaveAttachmentsByRegisteredCase(CmsRegisteredCase registeredCase, DmsDbContext dmsDbContext)
        {
            try
            {
                //add new attachments
                var attachements = await dmsDbContext.TempAttachements.Where(x => x.Guid == registeredCase.CaseId).ToListAsync();
                foreach (var file in attachements)
                {
                    UploadedDocument documentObj = new UploadedDocument();
                    documentObj.Description = file.Description;
                    documentObj.CreatedDateTime = DateTime.Now;
                    documentObj.CreatedBy = registeredCase.CreatedBy;
                    documentObj.DocumentDate = (DateTime)file.DocumentDate;
                    documentObj.FileName = file.FileName;
                    documentObj.StoragePath = file.StoragePath;
                    documentObj.DocType = file.DocType;
                    documentObj.ReferenceGuid = registeredCase.CaseId;
                    documentObj.IsActive = true;
                    documentObj.CreatedAt = file.StoragePath;
                    documentObj.AttachmentTypeId = file.AttachmentTypeId;
                    documentObj.IsDeleted = false;
                    documentObj.OtherAttachmentType = file.OtherAttachmentType;
                    documentObj.ReferenceNo = file.ReferenceNo;
                    documentObj.ReferenceDate = file.ReferenceDate;
                    await dmsDbContext.UploadedDocuments.AddAsync(documentObj);
                    await dmsDbContext.SaveChangesAsync();
                    await Task.Delay(200);
                    dmsDbContext.TempAttachements.Remove(file);
                    await dmsDbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Copy Case parties from Case File </History>
        private async Task<List<CopyAttachmentVM>> CopyPartiesFromCaseFile(CmsRegisteredCase registeredCase, DatabaseContext dbContext)
        {
            List<CopyAttachmentVM> copyAttachments = new List<CopyAttachmentVM>();
            try
            {
                var requestParties = await dbContext.CasePartyLink.Where(p => p.ReferenceGuid == registeredCase.FileId).ToListAsync();
                foreach (var party in requestParties)
                {
                    var partyId = party.Id;
                    CasePartyLink fileParty = party;
                    fileParty.Id = Guid.NewGuid();
                    fileParty.ReferenceGuid = registeredCase.CaseId;
                    fileParty.CreatedBy = registeredCase.CreatedBy;
                    fileParty.CreatedDate = DateTime.Now;
                    await dbContext.CasePartyLink.AddAsync(fileParty);
                    await dbContext.SaveChangesAsync();

                    registeredCase.CasePartyLink.Add(fileParty);
                    copyAttachments.Add(
                        new CopyAttachmentVM()
                        {
                            SourceId = partyId,
                            DestinationId = fileParty.Id,
                            CreatedBy = registeredCase.CreatedBy
                        });
                }
            }
            catch (Exception)
            {
                throw;
            }
            return copyAttachments;
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-28' Version="1.0" Branch="master">Copy Registered Case Attachments from Case File </History>
        private async Task CopyAttachmentsFromCaseFile(CmsRegisteredCase registeredCase, DmsDbContext dmsDbContext)
        {
            try
            {
                var requestDocs = await dmsDbContext.UploadedDocuments.Where(p => p.ReferenceGuid == registeredCase.FileId).ToListAsync();
                foreach (var reqDoc in requestDocs)
                {
                    UploadedDocument fileDoc = reqDoc;
                    fileDoc.UploadedDocumentId = 0;
                    fileDoc.ReferenceGuid = registeredCase.CaseId;
                    fileDoc.CreatedBy = registeredCase.CreatedBy;
                    fileDoc.CreatedDateTime = DateTime.Now;
                    await dmsDbContext.UploadedDocuments.AddAsync(fileDoc);
                    await dmsDbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Update Case File status 

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Update Case File Status</History>
        private async Task UpdateCaseFileStatusAfterMojRegistration(CmsRegisteredCase registeredCase, int StatusId, DatabaseContext dbContext)
        {
            try
            {
                var file = await dbContext.CaseFiles.FindAsync(registeredCase.FileId);
                file.StatusId = StatusId;
                dbContext.CaseFiles.Update(file);
                await dbContext.SaveChangesAsync();
                await SaveCaseFileStatusHistory(registeredCase.CreatedBy, file, (int)CaseFileEventEnum.RegisteredAtMoj, dbContext);
                await UpdateCaseRequestStatus(registeredCase.CreatedBy, file.RequestId, (int)CaseRequestStatusEnum.RegisteredInMOJ, dbContext);
                if (registeredCase.AttachmentTypeId != (int)AttachmentTypeEnum.PerformOrderNotes
                    && registeredCase.AttachmentTypeId != (int)AttachmentTypeEnum.OrderOnPetitionNotes)
                {
                    await RemoveCaseAssigneesAndUpdateTaskStatus(file.FileId, dbContext);
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Update MOJ Registration request Status

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Update MOJ Registration request Status</History>
        private async Task UpdateMojRegistrationRequestStatus(Guid requestId, DatabaseContext dbContext)
        {
            try
            {
                var request = await dbContext.MojRegistrationRequests.FindAsync(requestId);
                request.IsRegistered = true;
                dbContext.MojRegistrationRequests.Update(request);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Update Case Request status 

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Update Case File Status</History>
        public async Task UpdateCaseRequestStatus(string userName, Guid requestId, int StatusId, DatabaseContext dbContext)
        {
            try
            {
                var request = await dbContext.CaseRequests.FindAsync(requestId);
                request.StatusId = StatusId;
                dbContext.CaseRequests.Update(request);
                await dbContext.SaveChangesAsync();
                await _caseRequestRepository.SaveCaseRequestStatusHistory(userName, request, (int)CaseRequestEventEnum.RegisteredInMOJ, dbContext);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Remove Case Assignment

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Remove current assignees of Reference Entity Case or File and mark the task as done</History>
        private async Task RemoveCaseAssigneesAndUpdateTaskStatus(Guid referenceId, DatabaseContext dbContext)
        {
            try
            {
                CaseAssignment primaryLawyer = await dbContext.CaseFileAssignment.Where(p => p.ReferenceId == referenceId && p.IsPrimary).FirstOrDefaultAsync();

                dbContext.CaseFileAssignment.RemoveRange(await dbContext.CaseFileAssignment.Where(f => f.ReferenceId == referenceId).ToListAsync());
                await dbContext.SaveChangesAsync();

                if (primaryLawyer != null)
                {
                    UserTask task = await dbContext.Tasks.Where(t => t.ReferenceId == referenceId && t.TypeId == (int)TaskTypeEnum.Assignment && t.ModuleId == (int)WorkflowModuleEnum.CaseManagement && t.AssignedTo == primaryLawyer.LawyerId).FirstOrDefaultAsync();
                    if (task != null)
                    {
                        task.TaskStatusId = (int)TaskStatusEnum.Done;
                        task.ModifiedDate = DateTime.Now;
                        dbContext.Tasks.Update(task);
                        await dbContext.SaveChangesAsync();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region save Case File status history

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Save Case File Status History</History>
        private async Task SaveCaseFileStatusHistory(string userName, CaseFile caseFile, int EventId, DatabaseContext dbContext)
        {
            try
            {
                CmsCaseFileStatusHistory historyobj = new CmsCaseFileStatusHistory();
                historyobj.HistoryId = Guid.NewGuid();
                historyobj.FileId = caseFile.FileId;
                historyobj.StatusId = caseFile.StatusId;
                historyobj.CreatedDate = DateTime.Now;
                historyobj.CreatedBy = userName;
                historyobj.EventId = EventId;
                //historyobj.Remarks = caseRequest.Remarks;
                await dbContext.CmsCaseFileStatusHistory.AddAsync(historyobj);
                await dbContext.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Create Merge Request
        //<History Author = 'Hassan Abbas' Date='2022-11-21' Version="1.0" Branch="master">Create Merge Request </History>
        public async Task CreateMergeRequest(MergeRequest mergeRequest)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        mergeRequest.StatusId = (int)MergeRequestStatusEnum.SentForApproval;
                        await _dbContext.MergeRequests.AddAsync(mergeRequest);
                        await _dbContext.SaveChangesAsync();
                        await SaveMergeRequestSecondaryCases(mergeRequest, _dbContext);
                        //await SaveAttachmentsByMergeRequest(mergeRequest, _dbContext);

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

        //<History Author = 'Hassan Abbas' Date='2022-11-21' Version="1.0" Branch="master">Save Merged Request Secondary Cases </History>
        protected async Task SaveMergeRequestSecondaryCases(MergeRequest mergeRequest, DatabaseContext dbContext)
        {
            try
            {
                foreach (var regCase in mergeRequest.RegisteredCases.Where(c => c.CaseId != mergeRequest.PrimaryId))
                {
                    MergeRequestSecondaries requestSecondary = new MergeRequestSecondaries
                    {
                        MergeRequestId = mergeRequest.Id,
                        SecondaryId = regCase.CaseId
                    };
                    await dbContext.MergeRequestSecondaries.AddAsync(requestSecondary);
                    await dbContext.SaveChangesAsync();
                }
                // For Notification 
                mergeRequest.NotificationParameter.CaseNumber = dbContext.cmsRegisteredCases.Where(x => x.CaseId == mergeRequest.PrimaryId).FirstOrDefault().CaseNumber;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //<History Author = 'Hassan Abbas' Date='2022-09-28' Version="1.0" Branch="master">Save Merge Request Attachments </History>
        private async Task SaveAttachmentsByMergeRequest(MergeRequest mergeRequest, DmsDbContext dmsDbContext)
        {
            try
            {
                //add new attachments
                var attachements = await dmsDbContext.TempAttachements.Where(x => x.Guid == mergeRequest.Id).ToListAsync();
                foreach (var file in attachements)
                {
                    UploadedDocument documentObj = new UploadedDocument();
                    documentObj.Description = file.Description;
                    documentObj.CreatedDateTime = DateTime.Now;
                    documentObj.CreatedBy = mergeRequest.CreatedBy;
                    documentObj.DocumentDate = (DateTime)file.DocumentDate;
                    documentObj.FileName = file.FileName;
                    documentObj.StoragePath = file.StoragePath;
                    documentObj.DocType = file.DocType;
                    documentObj.ReferenceGuid = mergeRequest.Id;
                    documentObj.IsActive = true;
                    documentObj.CreatedAt = file.StoragePath;
                    documentObj.AttachmentTypeId = file.AttachmentTypeId;
                    documentObj.IsDeleted = false;
                    documentObj.OtherAttachmentType = file.OtherAttachmentType;
                    documentObj.ReferenceNo = file.ReferenceNo;
                    documentObj.ReferenceDate = file.ReferenceDate;
                    await dmsDbContext.UploadedDocuments.AddAsync(documentObj);
                    await dmsDbContext.SaveChangesAsync();
                    await Task.Delay(200);
                    dmsDbContext.TempAttachements.Remove(file);
                    await dmsDbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region update registered Case  status 
        private async Task UpdateRegisteredCaseStatus(string userId, Guid CaseId, int StatusId, int EventId, DatabaseContext dbContext)
        {
            try
            {
                var regCase = await dbContext.CmsRegisteredCases.FindAsync(CaseId);
                regCase.StatusId = StatusId;
                dbContext.CmsRegisteredCases.Update(regCase);
                await dbContext.SaveChangesAsync();
                regCase.CreatedBy = userId;
                regCase.CreatedDate = DateTime.Now;
                await SaveRegisteredCaseStatusHistory(regCase, EventId, dbContext);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Approve Merge Request

        //<History Author = 'Hassan Abbas' Date='2022-11-21' Version="1.0" Branch="master">Approve Merge Request </History>
        public async Task ApproveMergeRequest(Guid mergeRequestId, string loggedInUser)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var mergeRequest = await _dbContext.MergeRequests.FindAsync(mergeRequestId);
                            //mergeRequest.RegisteredCases = await GetMergedCasesByMergeRequestId(mergeRequestId);
                            var regCase = await GetCaseByPrimaryLawyerId(loggedInUser);

                            await UpdateMergeRequestStatus(mergeRequest.Id, (int)MergeRequestStatusEnum.Approved, _dbContext);
                            await UpdatePrimaryCase(mergeRequest.PrimaryId, _dbContext);
                            //foreach (var regCase in mergeRequest.RegisteredCases)
                            //{
                            await LinkMergedCaseWithPrimaryCase(mergeRequest.PrimaryId, regCase.CaseId, _dbContext);
                            await DissolveMergedCase(regCase.CaseId, _dbContext);
                            string StoredProc = $"exec pSubCasesByCaseId @caseId= '{regCase.CaseId}'";
                            var subcases = await _dbContext.CmsRegisteredCaseVMs.FromSqlRaw(StoredProc).ToListAsync();
                            foreach (var subcase in subcases)
                            {
                                await DissolveMergedCase(subcase.CaseId, _dbContext);
                            }
                            //}
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Reject Merge Request

        //<History Author = 'Hassan Abbas' Date='2022-11-21' Version="1.0" Branch="master">Reject Merge Request </History>
        public async Task RejectMergeRequest(Guid mergeRequestId)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var mergeRequest = await _dbContext.MergeRequests.FindAsync(mergeRequestId);
                            mergeRequest.RegisteredCases = await GetMergedCasesByMergeRequestId(mergeRequestId);
                            await UpdateMergeRequestStatus(mergeRequest.Id, (int)MergeRequestStatusEnum.Rejected, _dbContext);
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
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //<History Author = 'Hassan Abbas' Date='2022-11-21' Version="1.0" Branch="master">Dissolve Merged Case </History>
        protected async Task LinkMergedCaseWithPrimaryCase(Guid primaryCaseId, Guid mergedCaseId, DatabaseContext dbContext)
        {
            try
            {
                CmsRegisteredCaseMergedCase caseMergedCase = new CmsRegisteredCaseMergedCase
                {
                    PrimaryCaseId = primaryCaseId,
                    MergedCaseId = mergedCaseId
                };
                await dbContext.CmsRegisteredCaseMergedCases.AddAsync(caseMergedCase);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-21' Version="1.0" Branch="master">Dissolve Merged Case </History>
        protected async Task UpdateMergeRequestStatus(Guid mergeRequestId, int statusId, DatabaseContext dbContext)
        {
            try
            {
                var mergeRequest = await dbContext.MergeRequests.FindAsync(mergeRequestId);
                mergeRequest.StatusId = statusId;
                dbContext.Update(mergeRequest);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-21' Version="1.0" Branch="master">Update Primary Case</History>
        protected async Task UpdatePrimaryCase(Guid caseId, DatabaseContext dbContext)
        {
            try
            {
                var regCase = await dbContext.CmsRegisteredCases.FindAsync(caseId);
                regCase.IsPrimary = true;
                regCase.CaseNumber = "MER-" + regCase.CaseNumber;
                dbContext.Update(regCase);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-21' Version="1.0" Branch="master">Dissolve Merged Case </History>
        protected async Task DissolveMergedCase(Guid caseId, DatabaseContext dbContext)
        {
            try
            {
                var regCase = await dbContext.CmsRegisteredCases.FindAsync(caseId);
                regCase.IsDissolved = true;
                dbContext.Update(regCase);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Add Judgement for a case

        //<History Author = 'Hassan Abbas' Date='2022-11-21' Version="1.0" Branch="master">Add Judgement for a case </History>
        public async Task AddJudgement(Judgement judgement)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        CmsRegisteredCase registeredCase = await _dbContext.CmsRegisteredCases.FindAsync(judgement.CaseId);
                        if (judgement.IsUpdated == true)
                        {
                            _dbContext.Judgements.Update(judgement);
                            await _dbContext.SaveChangesAsync();
                        }
                        else
                        {
                            await _dbContext.Judgements.AddAsync(judgement);
                            await _dbContext.SaveChangesAsync();

                            await SaveRegisteredCaseStatusHistory(registeredCase, judgement.IsFinal ? (int)RegisteredCaseEventEnum.FinalJudgementAdded : (int)RegisteredCaseEventEnum.JudgementAdded, _dbContext, judgement.Remarks, judgement.CreatedBy);
                        }
                        transaction.Commit();
                        //For Notification
                        judgement.NotificationParameter.CaseNumber = registeredCase.CaseNumber;
                        judgement.NotificationParameter.FileNumber = _dbContext.CaseFiles.Where(x => x.FileId == registeredCase.FileId).FirstOrDefault().FileNumber;
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

        #region Postpone Hearing Request

        //<History Author = 'Hassan Abbas' Date='2022-12-05' Version="1.0" Branch="master"> Add Postpone Hearing Request</History>
        public async Task AddPostponeHearingRequest(PostponeHearing postponeHearing)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        Hearing hearing = await _dbContext.Hearings.FindAsync(postponeHearing.HearingId);
                        hearing.StatusId = (int)HearingStatusEnum.HearingCancelled;
                        _dbContext.Hearings.Update(hearing);
                        await _dbContext.PostponeHearings.AddAsync(postponeHearing);
                        await _dbContext.SaveChangesAsync();
                        CmsRegisteredCase registeredCase = await _dbContext.CmsRegisteredCases.FindAsync(hearing.CaseId);
                        await SaveRegisteredCaseStatusHistory(registeredCase, (int)RegisteredCaseEventEnum.HearingCancelled, _dbContext, postponeHearing.Reason, postponeHearing.CreatedBy);
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

        #region Get Sub cases by Case Id
        //<History Author = 'Ijaz Ahmad' Date='2022-12-08' Version="1.0" Branch="master">Get Sub  Cases  by  Case Id </History>
        public async Task<List<CmsRegisteredCaseVM>> GetSubCasesByCaseId(Guid caseId)
        {
            try
            {
                if (_CmsRegisteredCaseVMs == null)
                {
                    string StoredProc = $"exec pSubCasesByCaseId @caseId= '{caseId}'";
                    _CmsRegisteredCaseVMs = await _dbContext.CmsRegisteredCaseVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CmsRegisteredCaseVMs;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }

        #endregion

        #region Get Merge Cases By Case Id
        //<History Author = 'Ijaz Ahmad' Date='2022-12-12' Version="1.0" Branch="master">Get Merge Registered Case By Case Id </History>
        public async Task<List<CmsRegisteredCaseVM>> GetMergedCasesbyCaseId(Guid caseId)
        {
            try
            {
                if (_CmsRegisteredCaseVMs == null)
                {
                    string StoredProc = $"exec pMergedCasesByCaseId @caseId= '{caseId}'";
                    _CmsRegisteredCaseVMs = await _dbContext.CmsRegisteredCaseVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CmsRegisteredCaseVMs;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }




        #endregion

        #region Get All Registered Cases 
        //<History Author = 'Danish' Date='2022-11-29' Version="1.0" Branch="master"> Get All Registered Cases By File</History>
        public async Task<List<CmsRegisteredCaseVM>> GetAllRegisteredCases()
        {
            try
            {
                if (_CmsRegisteredCaseVMs == null)
                {
                    string StoredProc = $"exec pCmsRegisteredCasesList";

                    _CmsRegisteredCaseVMs = await _dbContext.CmsRegisteredCaseVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CmsRegisteredCaseVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }

        }
        #endregion

        #region Create Request For Document
        //< History Author = 'Danish' Date = '2022-12-09' Version = "1.0" Branch = "master" >Request For Document</History>
        public async Task CreateRequestForDocument(MojRequestForDocument item)
        {
            try
            {
                await _dbContext.MojRequestForDocument.AddAsync(item);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Requested Documents 
        //<History Author = 'Ijaz Ahmad' Date='2022-12-12' Version="1.0" Branch="master">Get Requested  Documents  </History>
        public async Task<List<CmsRequestDocumentsVM>> GetRequestedDocuments()
        {
            try
            {
                if (cmsRequestDocumentsVMs == null)
                {
                    string StoredProc = $"exec pCmsRequestedDocuments";
                    cmsRequestDocumentsVMs = await _dbContext.CmsRequestDocumentsVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return cmsRequestDocumentsVMs;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }
        #endregion
        //<History Author = 'Nabeel Ur Rehman' Date='2022-10-21' Version="1.0" Branch="master"> Save Sub Case/History>
        #region Sub Case Request
        public async Task<CmsRegisteredCase> CreateSubCase(CmsRegisteredCase cmsRegisteredCase)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        cmsRegisteredCase.IsSubCase = true;
                        await _dbContext.cmsRegisteredCases.AddAsync(cmsRegisteredCase);
                        await _dbContext.SaveChangesAsync();
                        await CreateSubCaseManytoMany(cmsRegisteredCase, _dbContext);
                        //await SaveAttachmentsBySubCase(cmsRegisteredCase, _dbContext);
                        //await CopyAttachmentsFromRegisteredCase(cmsRegisteredCase, _dbContext);
                        //await CopyPartiesFromRegisteredCase(cmsRegisteredCase, _dbContext);
                        //add Copy Attachment of Case Parties 
                        cmsRegisteredCase.CopyAttachmentVMs.Add(new CopyAttachmentVM()
                        {
                            SourceId = (Guid)cmsRegisteredCase.ParentCaseId,
                            DestinationId = (Guid)cmsRegisteredCase.CaseId,
                            CreatedBy = cmsRegisteredCase.CreatedBy
                        });

                        var copyAttachments = await _caseRequestRepository.CopyCasePartiesFromSourceToDestination((Guid)cmsRegisteredCase.ParentCaseId, cmsRegisteredCase.CaseId, cmsRegisteredCase.CreatedBy, _dbContext);
                        if (copyAttachments.Any())
                        {
                            cmsRegisteredCase.CopyAttachmentVMs.AddRange(copyAttachments);
                        }
                        // Fetching The lawyer Id who Added  The judgment in Case, for purpose when new subcase will added for Stop Execution by partial/urgent sector
                        // so we will notify the lawyer who initiate the request for stopping exec.
                        var userEmail = _dbContext.Judgements.Where(x => x.CaseId == cmsRegisteredCase.ParentCaseId).Select(x => x.CreatedBy).FirstOrDefault();
                        if (userEmail != null)
                        {
                            var userID = _dbContext.Users.Where(x => x.Email == userEmail).Select(x => x.Id).FirstOrDefault();
                            if (userID != null)
                                cmsRegisteredCase.LawyerId = userID;
                        }
                       // await UpdateMojRegistrationRequestStatus((Guid)cmsRegisteredCase.CaseId, _dbContext);
                        var mojreuest = _dbContext.MojRegistrationRequests.Where(x => x.FileId == cmsRegisteredCase.FileId).FirstOrDefault();
                        if (mojreuest is not null)
                        {
                            mojreuest.IsRegistered = true;
                            _dbContext.Update(mojreuest);
                            await _dbContext.SaveChangesAsync();
                        }
                        transaction.Commit();
                        // For Notification
                        cmsRegisteredCase.NotificationParameter.CaseNumber = cmsRegisteredCase.CaseNumber;
                        cmsRegisteredCase.NotificationParameter.CANNumber = cmsRegisteredCase.CANNumber;


                        return cmsRegisteredCase;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }
        //#region Sub Case Request
        //public async Task CreateSubCase(CmsRegisteredCase cmsRegisteredCase)
        //{
        //    using (_dbContext)
        //    {
        //        using (var transaction = _dbContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                cmsRegisteredCase.IsSubCase = true;
        //                await _dbContext.cmsRegisteredCases.AddAsync(cmsRegisteredCase);
        //                await _dbContext.SaveChangesAsync();
        //                await CreateSubCaseManytoMany(cmsRegisteredCase, _dbContext);
        //                await SaveAttachmentsBySubCase(cmsRegisteredCase, _dbContext);
        //                await CopyAttachmentsFromRegisteredCase(cmsRegisteredCase, _dbContext);
        //                await CopyPartiesFromRegisteredCase(cmsRegisteredCase, _dbContext);

        //               
        //                transaction.Commit();
        //             
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                throw new Exception(ex.Message);
        //            }
        //        }
        //    }
        //}
        //<History Author = 'Hassan Abbas' Date='2022-09-28' Version="1.0" Branch="master">Save Registered Case Attachments </History>   
        public async Task SaveAttachmentsBySubCase(CmsRegisteredCase registeredCase, DmsDbContext dmsDbContext)
        {
            try
            {
                //add new attachments
                var attachements = await dmsDbContext.TempAttachements.Where(x => x.Guid == registeredCase.CaseId).ToListAsync();
                foreach (var file in attachements)
                {
                    UploadedDocument documentObj = new UploadedDocument();
                    documentObj.Description = file.Description;
                    documentObj.CreatedDateTime = DateTime.Now;
                    documentObj.CreatedBy = registeredCase.CreatedBy;
                    documentObj.DocumentDate = (DateTime)file.DocumentDate;
                    documentObj.FileName = file.FileName;
                    documentObj.StoragePath = file.StoragePath;
                    documentObj.DocType = file.DocType;
                    documentObj.ReferenceGuid = registeredCase.CaseId;
                    documentObj.IsActive = true;
                    documentObj.CreatedAt = file.StoragePath;
                    documentObj.AttachmentTypeId = file.AttachmentTypeId;
                    documentObj.IsDeleted = false;
                    documentObj.OtherAttachmentType = file.OtherAttachmentType;
                    documentObj.ReferenceNo = file.ReferenceNo;
                    documentObj.ReferenceDate = file.ReferenceDate;
                    await dmsDbContext.UploadedDocuments.AddAsync(documentObj);
                    await dmsDbContext.SaveChangesAsync();
                    await Task.Delay(200);
                    dmsDbContext.TempAttachements.Remove(file);
                    await dmsDbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CopyPartiesFromRegisteredCase(CmsRegisteredCase registeredCase,DatabaseContext dbContext, DmsDbContext dmsDbContext)
        {
            try
            {
                var requestParties = await dbContext.CasePartyLink.Where(p => p.ReferenceGuid == registeredCase.ParentCaseId).ToListAsync();
                foreach (var party in requestParties)
                {
                    var requestDocs = await dmsDbContext.UploadedDocuments.Where(p => p.ReferenceGuid == party.Id).ToListAsync();
                    CasePartyLink fileParty = party;
                    fileParty.Id = Guid.NewGuid();
                    fileParty.ReferenceGuid = registeredCase.CaseId;
                    fileParty.CreatedBy = registeredCase.CreatedBy;
                    fileParty.CreatedDate = DateTime.Now;
                    await dbContext.CasePartyLink.AddAsync(fileParty);
                    await dbContext.SaveChangesAsync();
                    //party attachment if any
                    foreach (var reqDoc in requestDocs)
                    {
                        UploadedDocument fileDoc = reqDoc;
                        fileDoc.UploadedDocumentId = 0;
                        fileDoc.ReferenceGuid = fileParty.Id;
                        await dmsDbContext.UploadedDocuments.AddAsync(fileDoc);
                        await dmsDbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //<History Author = 'Nabeel ur Rehaman' Date='2022-09-28' Version="1.0" Branch="master">Save Sub case many to many relation table </History>
        public async Task CreateSubCaseManytoMany(CmsRegisteredCase cmsRegisteredSubCase, DatabaseContext dbContext)
        {
            try
            {
                CmsRegisteredCaseSubCase cmsRegisteredCaseSubCase = new CmsRegisteredCaseSubCase();
                cmsRegisteredCaseSubCase.SubCaseId = cmsRegisteredSubCase.CaseId;
                cmsRegisteredCaseSubCase.CaseId = cmsRegisteredSubCase.ParentCaseId;
                await _dbContext.CmsRegisteredCaseSubCases.AddAsync(cmsRegisteredCaseSubCase);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //<History Author = 'Rehman' Date='2022-09-28' Version="1.0" Branch="master">Copy Registered Case Attachments from Case File </History>
        public async Task CopyAttachmentsFromRegisteredCase(CmsRegisteredCase registeredCase, DmsDbContext dmsDbContext)
        {
            try
            {
                var requestDocs = await dmsDbContext.UploadedDocuments.Where(p => p.ReferenceGuid == registeredCase.ParentCaseId).ToListAsync();
                foreach (var reqDoc in requestDocs)
                {
                    UploadedDocument fileDoc = reqDoc;
                    fileDoc.UploadedDocumentId = 0;
                    fileDoc.ReferenceGuid = registeredCase.CaseId;
                    fileDoc.CreatedBy = registeredCase.CreatedBy;
                    fileDoc.CreatedDateTime = DateTime.Now;
                    await dmsDbContext.UploadedDocuments.AddAsync(fileDoc);
                    await dmsDbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Create Scheduling Court Vists
        //< History Author = 'Danish' Date = '2022-12-14' Version = "1.0" Branch = "master" >Create Scheduling Court Vists</History>
        public async Task CreateSchedulingCourtVists(SchedulingCourtVisits item)
        {
            try
            {
                await _dbContext.SchedulingCourtVisits.AddAsync(item);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Link CANs

        //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master"> Link CANs </History>
        public async Task LinkCANs(LinkCANsVM linkCAN)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var primaryFileId = await _dbContext.CmsRegisteredCases.Where(c => c.CANNumber == linkCAN.PrimaryCAN).Select(c => c.FileId).FirstOrDefaultAsync();
                        foreach (var can in linkCAN.LinkedCANs.Where(c => c == linkCAN.PrimaryCAN))
                        {
                            List<CmsRegisteredCase> cases = await _dbContext.CmsRegisteredCases.Where(c => c.CANNumber == can).ToListAsync();
                            foreach (var registeredCase in cases)
                            {
                                registeredCase.FileId = primaryFileId;
                                registeredCase.OldCANNumber = registeredCase.CANNumber;
                                registeredCase.CANNumber = linkCAN.PrimaryCAN;
                                _dbContext.CmsRegisteredCases.Update(registeredCase);
                                await _dbContext.SaveChangesAsync();
                                await SaveRegisteredCaseStatusHistory(registeredCase, (int)RegisteredCaseEventEnum.Linked, _dbContext);
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion

        #region Save & Close Case Files

        //<History Author = 'Ijaz Ahmad' Date='2022-12-22' Version="1.0" Branch="master">Save & Close Case File </History>
        public async Task SaveAndCloseCaseFiles(CmsSaveCloseCaseFile saveCloseCaseFile)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        await UpdateRegisteredCaseStatus(saveCloseCaseFile.CreatedBy, saveCloseCaseFile.CaseId, (int)RegisteredCaseStatusEnum.Closed, (int)RegisteredCaseEventEnum.Closed, _dbContext);
                        await RemoveCaseAssigneesAndUpdateTaskStatus(saveCloseCaseFile.CaseId, _dbContext);
                        await _dbContext.cmsSaveCloseCaseFiles.AddAsync(saveCloseCaseFile);
                        await _dbContext.SaveChangesAsync();
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

        #region Get Schedule Court Visit by Id        
        //<History Author = 'Danish' Date='2022-12-26' Version="1.0" Branch="master">Get Schedule Court Visit by Id</History>
        public async Task<List<SchedulingCourtVisitVM>> GetSchedulCourtVisitByHearingId(Guid HearingId)
        {
            try
            {
                if (SchedulingCourtVisitVMs == null)
                {
                    string StoredProc = $"exec pSchedulingCourtVisitListById @HearingId = '{HearingId}'";
                    SchedulingCourtVisitVMs = await _dbContext.SchedulingCourtVisitVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return SchedulingCourtVisitVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region GetCaseByPrimaryLawyerId  

        public async Task<CmsRegisteredCaseVM> GetCaseByPrimaryLawyerId(string loggedInUser)
        {
            try
            {
                string storedProc = $"exec pCaseByPrimaryLawyerId @PrimaryLawyerId = '{loggedInUser}'";
                var result = await _dbContext.CmsRegisteredCaseVMs.FromSqlRaw(storedProc).ToListAsync();
                if (result is not null)
                    return result.FirstOrDefault();
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Get Judgment Execution
        //<History Author = 'Danish' Date='2022-01-03' Version="1.0" Branch="master">Get Judgment Execution</History>
        public async Task<List<CmsJudgmentExecutionVM>> GetJudgmentExecutions(Guid caseId)
        {
            try
            {
                if (CmsJudgmentExecutionVMs == null)
                {
                    string StoredProc = $"exec pGetAllExecutionList @caseId ='{caseId}'";

                    CmsJudgmentExecutionVMs = await _dbContext.CmsJudgmentExecutionVM.FromSqlRaw(StoredProc).ToListAsync();
                }

                return CmsJudgmentExecutionVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Add Execution Request
        //< History Author = 'Danish' Date = '2023-01-04' Version = "1.0" Branch = "master" >Add Execution Request</History>
        public async Task AddExecutionRequest(MojExecutionRequest executionRequest)
        {
            try
            {
                await _dbContext.MojExecutionRequest.AddAsync(executionRequest);
                await _dbContext.SaveChangesAsync();
                CmsRegisteredCase registeredCase = await _dbContext.CmsRegisteredCases.FindAsync(executionRequest.CaseId);
                await SaveRegisteredCaseStatusHistory(registeredCase, (int)RegisteredCaseEventEnum.ExecutionRequestSent, _dbContext, executionRequest.Remarks, executionRequest.CreatedBy);
                //For Notification
                executionRequest.NotificationParameter.Entity = "Case";
                executionRequest.NotificationParameter.CaseNumber = registeredCase.CaseNumber;
                executionRequest.NotificationParameter.FileNumber = _dbContext.CaseFiles.Where(x => x.FileId == registeredCase.FileId).FirstOrDefault().FileNumber; ;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Add Judgment Execution
        //< History Author = 'Danish' Date = '2023-01-04' Version = "1.0" Branch = "master" >Add Judgment Execution</History>
        //< History Author = 'Hassan Abbas' Date = '2023-04-06' Version = "1.0" Branch = "master" >Modified the function to handle Execution Requests and Regenerate Execution Requests as well</History>
        public async Task AddJudgmentExecution(CmsJudgmentExecution cmsJudgmentExecution)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (cmsJudgmentExecution.IsUpdated == true)
                        {
                            _dbContext.CmsJudgmentExecutions.Update(cmsJudgmentExecution);
                            await _dbContext.SaveChangesAsync();
                        }
                        else
                        {
                            await _dbContext.CmsJudgmentExecutions.AddAsync(cmsJudgmentExecution);
                            await _dbContext.SaveChangesAsync();
                        }
                        if (cmsJudgmentExecution.ExecutionRequestId != null && cmsJudgmentExecution.ExecutionRequestId != Guid.Empty)
                        {
                            User user = await _dbContext.Users.Where(u => u.UserName == cmsJudgmentExecution.CreatedBy).FirstOrDefaultAsync();
                            if (user != null)
                            {
                                MojExecutionRequestAssignee mojExecutionRequestAssignee = await _dbContext.MojExecutionRequestAssignees.Where(t => t.UserId == user.Id && t.RequestId == cmsJudgmentExecution.ExecutionRequestId).FirstOrDefaultAsync();
                                if (mojExecutionRequestAssignee != null)
                                {
                                    mojExecutionRequestAssignee.IsDeleted = true;
                                    _dbContext.MojExecutionRequestAssignees.Update(mojExecutionRequestAssignee);
                                    await _dbContext.SaveChangesAsync();
                                    UserTask task = await _dbContext.Tasks.Where(u => u.AssignedTo == user.Id && u.ReferenceId == cmsJudgmentExecution.ExecutionRequestId).FirstOrDefaultAsync();
                                    if(task != null)
                                    {
                                        task.TaskStatusId = (int)TaskStatusEnum.Done;
                                        await _dbContext.SaveChangesAsync();
                                    }
                                }
                            }
                        }
                        if (cmsJudgmentExecution.DecisionId != null && cmsJudgmentExecution.DecisionId != Guid.Empty)
                        {
                            CmsCaseDecision caseDecision = await _dbContext.CmsCaseDecisions.FindAsync(cmsJudgmentExecution.DecisionId);
                            if (caseDecision != null)
                            {
                                caseDecision.StatusId = (int)CaseDecisionStatusEnum.MojReplied;
                                _dbContext.CmsCaseDecisions.Update(caseDecision);
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                        CmsRegisteredCase registeredCase = await _dbContext.CmsRegisteredCases.FindAsync(cmsJudgmentExecution.CaseId);
                        await SaveRegisteredCaseStatusHistory(registeredCase, (int)RegisteredCaseEventEnum.ExecutionAdded, _dbContext, cmsJudgmentExecution.Remarks, cmsJudgmentExecution.CreatedBy);
                        transaction.Commit();

                        cmsJudgmentExecution.NotificationParameter.ReferenceNumber = cmsJudgmentExecution.ExecutionFileNumber;
                        cmsJudgmentExecution.NotificationParameter.CaseNumber = registeredCase.CaseNumber;
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

        #region Edit Judgment Execution
        //< History Author = 'Danish' Date = '2023-01-04' Version = "1.0" Branch = "master" >Edit Judgment Execution</History>
        public async Task EditJudgmentExecution(CmsJudgmentExecution cmsJudgmentExecution)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            _dbContext.CmsJudgmentExecutions.Update(cmsJudgmentExecution);
                            await _dbContext.SaveChangesAsync();
                            CmsApprovalTracking approvalTracking = await _dbContext.CmsApprovalTracking.Where(x => x.ReferenceId == cmsJudgmentExecution.ExecutionRequest.Id && x.SectorTo == cmsJudgmentExecution.ExecutionRequest.SectorTypeId && x.ProcessTypeId == (int)ApprovalProcessTypeEnum.ExecutionRequest).FirstOrDefaultAsync();
                            if (approvalTracking is not null)
                            {
                                _dbContext.CmsApprovalTracking.Remove(approvalTracking);
                                await _dbContext.SaveChangesAsync();
                            }
                            transaction.Commit();

                            cmsJudgmentExecution.NotificationParameter.ReferenceNumber = cmsJudgmentExecution.ExecutionFileNumber;
                            cmsJudgmentExecution.NotificationParameter.CaseNumber = _dbContext.CmsRegisteredCases.Where( x => x.CaseId == cmsJudgmentExecution.CaseId).FirstOrDefault().CaseNumber;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }

            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Execution By Id
        //< History Author = 'Hassan Abbas' Date = '2023-04-05' Version = "1.0" Branch = "master" >Get Execution Request By Id</History>
        public async Task<MojExecutionRequest> GetExecutionRequestById(Guid ExecutionId)
        {
            try
            {
                return await _dbContext.MojExecutionRequest.Where(x => x.Id == ExecutionId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Judgement Execution By Execution Request Id
        //< History Author = 'Danish' Date = '2023-01-04' Version = "1.0" Branch = "master" >Get Execution By Id</History>
        public async Task<CmsJudgmentExecution> GetJudgementExecutionByExecutionRequestId(Guid ExecutionId)
        {
            try
            {
                return await _dbContext.CmsJudgmentExecutions.Where(x => x.ExecutionRequestId == ExecutionId).FirstOrDefaultAsync() ?? new CmsJudgmentExecution();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Execution By Id
        //< History Author = 'Danish' Date = '2023-01-04' Version = "1.0" Branch = "master" >Get Execution By Id</History>
        public async Task<CmsJudgmentExecution> GetExecutionById(Guid ExecutionId)
        {
            try
            {
                return await _dbContext.CmsJudgmentExecutions.Where(x => x.Id == ExecutionId).FirstOrDefaultAsync();


            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Request For Document By Id
        //< History Author = 'Hassan Abbas' Date = '2023-03-24' Version = "1.0" Branch = "master" >Get Request For Document By Id</History>
        public async Task<MojRequestForDocument> GetRequestForDocumentById(Guid Id)
        {
            try
            {
                var documentRequest = await _dbContext.MojRequestForDocument.Where(x => x.Id == Id).FirstOrDefaultAsync();
                var user = _dbContext.Users.Where(e => e.Email == documentRequest.CreatedBy).FirstOrDefault();
                if (user != null)
                    documentRequest.User = _dbContext.UserPersonalInformation.Where(u => u.UserId == user.Id).FirstOrDefault();
                return documentRequest;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //try
            //{
            //    if (_MojRequestForDocument == null)
            //    {
            //        string StoredProc = $"exec pJudgmentDecisionByCaseId @caseId= '{Id}'";
            //        var list_MojRequestForDocument = await _dbContext.MojRequestForDocument.FromSqlRaw(StoredProc).ToListAsync();
            //        return list_MojRequestForDocument.FirstOrDefault();
            //    }
            //    return _MojRequestForDocument;
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //    throw new Exception(ex.Message);
            //}
        }
        #endregion

        #region Update Document Portfolio Request
        public async Task UpdateDocumentPortfolioRequest(Guid Id)
        {
            try
            {
                MojRequestForDocument requestForDocument = await _dbContext.MojRequestForDocument.FindAsync(Id);
                requestForDocument.IsAddressed = true;
                _dbContext.Update(requestForDocument);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Add Judgment Decision
        //CmsRegisteredCase cmsRegistered;
        public async Task<CmsCaseDecision> AddJudgementDecision(CmsCaseDecision cmsCaseDecision)
        {
            try
            {
                await _dbContext.CmsCaseDecisions.AddAsync(cmsCaseDecision);
                await _dbContext.SaveChangesAsync();
                var cmsRegistered = await _dbContext.cmsRegisteredCases.FindAsync(cmsCaseDecision.CaseId);
                cmsCaseDecision.SectorTypeId = cmsRegistered.SectorTypeId;
                return cmsCaseDecision;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region  Get Judgment Decision
        //<History Author = 'Muhammad Zaeem' Date='2023-03-31' Version="1.0" Branch="master">Get Judgment Decision </History>
        public async Task<List<CmsJugdmentDecisionVM>> GetJudgmentDecision(Guid caseId)
        {
            try
            {
                if (_CmsJugdmentDecisionVMs == null)
                {
                    string StoredProc = $"exec pJudgmentDecisionByCaseId @caseId= '{caseId}'";
                    _CmsJugdmentDecisionVMs = await _dbContext.CmsJugdmentDecisionVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CmsJugdmentDecisionVMs;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }

        #endregion

        #region Get Judgment Decision Detail by Id

        //<History Author = 'Muhammad Zaeem' Date='2023-1-04' Version="1.0" Branch="master">Get Judgment Decision Detail by Id</History>
        public async Task<CmsJugdmentDecisionVM> GetJudgmentDecisionDetailbyId(Guid decisionId)
        {
            try
            {
                if (_CmsJugdmentDecisionVMs == null)
                {
                    string StoredProc = $"exec pGetJudgmentDecisionDetailById @decisionId ='{decisionId}'";

                    _CmsJugdmentDecisionVMs = await _dbContext.CmsJugdmentDecisionVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CmsJugdmentDecisionVMs.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Approve/Reject Decision
        public async Task<bool> RejectDecision(CmsJugdmentDecisionVM cmsJugdmentDecisionVM)
        {
            bool isSaved = true;
            try
            {
                CmsCaseDecision cmsCaseDecision = _dbContext.CmsCaseDecisions.FirstOrDefault(m => m.Id == cmsJugdmentDecisionVM.Id);
                if (cmsCaseDecision is not null)
                {
                    cmsCaseDecision.StatusId = cmsJugdmentDecisionVM.StatusId;

                    cmsCaseDecision.ModifiedBy = cmsJugdmentDecisionVM.ModifiedBy;
                    cmsCaseDecision.ModifiedDate = DateTime.Now;

                    _dbContext.CmsCaseDecisions.Update(cmsCaseDecision);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                isSaved = false;
            }
            return isSaved;
        }

        public async Task<bool> ApproveDecision(CmsJugdmentDecisionVM cmsJugdmentDecisionVM)
        {
            bool isSaved = true;
            try
            {
                CmsCaseDecision cmsCaseDecision = _dbContext.CmsCaseDecisions.FirstOrDefault(m => m.Id == cmsJugdmentDecisionVM.Id);
                if (cmsCaseDecision is not null)
                {
                    cmsCaseDecision.StatusId = cmsJugdmentDecisionVM.StatusId;

                    cmsCaseDecision.ModifiedBy = cmsJugdmentDecisionVM.ModifiedBy;
                    cmsCaseDecision.ModifiedDate = DateTime.Now;

                    _dbContext.CmsCaseDecisions.Update(cmsCaseDecision);
                    await SaveCaseDecisionAssignee(cmsJugdmentDecisionVM, cmsCaseDecision.Id);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                isSaved = false;
            }
            return isSaved;
        }
        #endregion

        #region Save Case Decision Assignee 
        public async Task SaveCaseDecisionAssignee(CmsJugdmentDecisionVM cmsJugdmentDecisionVM, Guid referenceId)
        {
            try
            {
                CmsCaseDecisionAssignee descisionObj = new CmsCaseDecisionAssignee();
                descisionObj.Id = Guid.NewGuid();
                descisionObj.DecisionId = referenceId;
                descisionObj.CreatedBy = cmsJugdmentDecisionVM.ModifiedBy;
                descisionObj.CreatedDate = DateTime.Now;
                User user = await GetHOSBySectorId((int)OperatingSectorTypeEnum.Execution);
                descisionObj.UserId = Guid.Parse(user.Id);
                await _dbContext.CmsCaseDecisionAssignees.AddAsync(descisionObj);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Sector HOS
        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"> Get HOS By Sector</History>
        public async Task<User> GetHOSBySectorId(int sectorTypeId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                string StoreProc = $"exec pGetHOSBSectorId @sectorTypeId = '{sectorTypeId}'";
                var users = await _DbContext.Users.FromSqlRaw(StoreProc).ToListAsync();
                return users.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region  Get Judgment Decision List
        //<History Author = 'Muhammad Zaeem' Date='2023-03-31' Version="1.0" Branch="master">Get Judgment Decision </History>
        public async Task<List<CmsJugdmentDecisionVM>> GetJudgmentDecisionList(Guid userId, int pageNumber, int pageSize)
        {
            try
            {
                if (_CmsJugdmentDecisionVMs == null)
                {
                    string StoredProc = $"exec pJudgmentDecisionList @userId= '{userId}', @PageNumber = '{pageNumber}', @PageSize = '{pageSize}'";
                    _CmsJugdmentDecisionVMs = await _dbContext.CmsJugdmentDecisionVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CmsJugdmentDecisionVMs;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }

        #endregion

        #region Send Decision To Moj
        public async Task<bool> SendDecisionToMoj(CmsJugdmentDecisionVM cmsJugdmentDecisionVM)
        {
            bool isSaved = true;
            try
            {
                CmsCaseDecision cmsCaseDecision = _dbContext.CmsCaseDecisions.FirstOrDefault(m => m.Id == cmsJugdmentDecisionVM.Id);
                if (cmsCaseDecision is not null)
                {
                    cmsCaseDecision.StatusId = cmsJugdmentDecisionVM.StatusId;

                    cmsCaseDecision.ModifiedBy = cmsJugdmentDecisionVM.ModifiedBy;
                    cmsCaseDecision.ModifiedDate = DateTime.Now;

                    _dbContext.CmsCaseDecisions.Update(cmsCaseDecision);
                    await SaveCaseDecisionAssigneeMoj(cmsJugdmentDecisionVM, cmsCaseDecision.Id);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                isSaved = false;
            }
            return isSaved;
        }
        #endregion

        #region Save Case Decision Assignee MOj
        public async Task SaveCaseDecisionAssigneeMoj(CmsJugdmentDecisionVM cmsJugdmentDecisionVM, Guid referenceId)
        {
            try
            {
                CmsCaseDecisionAssignee descisionObj = new CmsCaseDecisionAssignee();
                descisionObj.Id = Guid.NewGuid();
                descisionObj.DecisionId = referenceId;
                descisionObj.CreatedBy = cmsJugdmentDecisionVM.ModifiedBy;
                descisionObj.CreatedDate = DateTime.Now;
                User user = await GetMojBySectorId((int)OperatingSectorTypeEnum.Execution);
                descisionObj.UserId = Guid.Parse(user.Id);
                await _dbContext.CmsCaseDecisionAssignees.AddAsync(descisionObj);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Sector Moj 
        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"> Get HOS By Sector</History>
        public async Task<User> GetMojBySectorId(int sectorTypeId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                string StoreProc = $"exec pGetMojBySectorId @sectorTypeId = '{sectorTypeId}'";
                var users = await _DbContext.Users.FromSqlRaw(StoreProc).ToListAsync();
                return users.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Registered Case Need More Detail
        public async Task<CmsCaseRequestResponseVM> GetRegisteredCaseNeedMoreDetail(Guid CaseId, Guid CommunicationId)
        {
            try
            {

                if (_CmsCaseRequestResponseVMs == null)
                {
                    string StoredProc = $"exec pRegisteredCaseRequestNeedMoreDetail @CaseId = N'{CaseId}', @CommunicationType = N'{(int)CommunicationTypeEnum.RequestMoreInfo}',@CommunicationId = N'{CommunicationId}'";
                    _CmsCaseRequestResponseVMs = await _dbContext.CmsCaseRequestResponseVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CmsCaseRequestResponseVMs.FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region get  Request for document id by hearing id
        public async Task<Guid> GetRequestfordocumentbyCaseId(Guid CaseId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            using (_DbContext)
            {
                try
                {
                    var hearingId = await _DbContext.MojRequestForDocument.Where(x => x.CaseId == CaseId).FirstOrDefaultAsync();
                    if (hearingId != null)
                    {

                        return hearingId.Id;
                    }
                    return Guid.Empty;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        #endregion

        #region Upadate Registered Case Chamber Number
        public async Task UpdateRegisteredCaseChamberNumber(CMSRegisteredCaseTransferHistoryVM cMSRegisteredCaseTransferHistoryVM)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var registeredCase = await _dbContext.CmsRegisteredCases.FindAsync(cMSRegisteredCaseTransferHistoryVM.CaseId);
                        if (registeredCase != null)
                        {
                            registeredCase.ChamberNumberId = cMSRegisteredCaseTransferHistoryVM.ChamberNumberToId;
                            registeredCase.ChamberId = cMSRegisteredCaseTransferHistoryVM.ChamberToId;
                            _dbContext.CmsRegisteredCases.Update(registeredCase);
                            await _dbContext.SaveChangesAsync();
                            await SaveRegisteredCaseStatusHistory(registeredCase, (int)RegisteredCaseEventEnum.CaseTransferedToAnotherChamber, _dbContext, string.Empty, cMSRegisteredCaseTransferHistoryVM.createdBy);
                            await SaveCMSRegisteredCaseTransferHistory(cMSRegisteredCaseTransferHistoryVM, _dbContext);
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

        #region Get Judgement Detail By Judgement Id

        public async Task<CmsJudgementDetailVM> GetJudgementDetailById(Guid judgementId)
        {
            try
            {
                string StoredProc = $"exec pCmsJudgementDetailByJudgementId @judgementId = '{judgementId}'";
                var result = await _dbContext.JudgementDetailVMs.FromSqlRaw(StoredProc).ToListAsync();
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Judgement> GetJudgementDetailByJudgementId(Guid judgementId)
        {
            try
            {
                Judgement judgement = await _dbContext.Judgements.Where(x => x.Id == judgementId).FirstOrDefaultAsync();
                return judgement;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get Execution File Status

        public async Task<List<CmsExecutionFileStatus>> GetExecutionFileStatus()
        {
            try
            {
                var result = await _dbContext.CmsExecutionFileStatuses.Where(x => x.IsActive == true).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Save Case Party
        //<History Author = 'Muhammad Zaeem' Date='2023-12-28' Version="1.0" Branch="master"> Save Case Parties in Outcome hearing/History>
        public async Task<bool> SaveCaseParty(CasePartyLinkVM party, DatabaseContext dbContext)
        {
            try
            {
                CasePartyLink caseParty = new CasePartyLink
                {
                    Id = party.Id,
                    ReferenceGuid = party.ReferenceGuid,
                    CategoryId = (int)party.CasePartyCategory,
                    TypeId = (int)party.CasePartyType,
                    Name = party.Name,
                    CivilId = party.CivilId,
                    CRN = party.CRN,
                    EntityId = party.EntityId,
                    RepresentativeId = party.RepresentativeId,
                    PACINumber = party.PACINumber,
                    Address = party.Address,
                    CompanyCivilId = party.CompanyCivilId,
                    MOCIFileNumber = party.MOCIFileNumber,
                    LicenseNumber = party.LicenseNumber,
                    MembershipNumber = party.MembershipNumber,
                    CreatedBy = party.CreatedBy,
                    CreatedDate = DateTime.Now
                };
                await dbContext.CasePartyLink.AddAsync(caseParty);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Delete case Party
        //<History Author = 'Muhammad Zaeem' Date='2023-12-28' Version="1.0" Branch="master"> Delete Case Parties in Outcome hearing/History>
        public async Task<bool> DeleteCaseParty(CasePartyLinkVM party, DatabaseContext dbContext)
        {
            try
            {
                var caseParty = await dbContext.CasePartyLink.FindAsync(party.Id);
                caseParty.DeletedBy = party.DeletedBy;
                caseParty.DeletedDate = DateTime.Now;
                caseParty.IsDeleted = true;
                dbContext.CasePartyLink.Update(caseParty);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion

        #region Save Case Party Attachment
        //<History Author = 'Muhammad Zaeem' Date='2023-12-28' Version="1.0" Branch="master"> Save Case Parties Attachment in Outcome hearing/History>

        public async Task SaveCasePartyAttachment(CasePartyLinkVM party, DmsDbContext dmsDbContext)
        {
            try
            {
                var attachements = await dmsDbContext.TempAttachements.Where(x => x.Guid == party.Id).ToListAsync();
                foreach (var file in attachements)
                {
                    UploadedDocument documentObj = new UploadedDocument();
                    documentObj.Description = file.Description;
                    documentObj.CreatedDateTime = DateTime.Now;
                    documentObj.CreatedBy = party.CreatedBy;
                    documentObj.DocumentDate = DateTime.Now;
                    documentObj.FileName = file.FileName;
                    documentObj.StoragePath = file.StoragePath;
                    documentObj.DocType = file.DocType;
                    documentObj.ReferenceGuid = party.Id;
                    documentObj.IsActive = true;
                    documentObj.CreatedAt = file.StoragePath;
                    documentObj.AttachmentTypeId = file.AttachmentTypeId;
                    documentObj.IsDeleted = false;
                    await dmsDbContext.UploadedDocuments.AddAsync(documentObj);
                    await dmsDbContext.SaveChangesAsync();
                    await Task.Delay(200);
                    dmsDbContext.TempAttachements.Remove(file);
                    await dmsDbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Save Outcome Party History
        //<History Author = 'Muhammad Zaeem' Date='2023-12-29' Version="1.0" Branch="master">Save Outcome Party History</History>

        public async Task SaveOutcomePartyHistory(Guid OutcomeId, Guid CasePartyLinkId, int ActionId, string CreatedBy, DatabaseContext databaseContext)
        {
            try
            {
                CmsOutcomePartyHistory cmsOutcomePartyHistory = new CmsOutcomePartyHistory
                {
                    Id = Guid.NewGuid(),
                    OutcomeId = OutcomeId,
                    CasePartyLinkId = CasePartyLinkId,
                    ActionId = ActionId,
                    CreatedBy = CreatedBy,
                    CreatedDate = DateTime.Now,

                };
                await databaseContext.CmsOutcomePartyHistories.AddAsync(cmsOutcomePartyHistory);
                await databaseContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        #endregion

        #region Get case outcome  party history detail by Outcome Id

        //<History Author = 'Muhammad Zaeem' Date='2023-12-29' Version="1.0" Branch="master">Get case outcome  party history detail by Outcome Id </History>

        public async Task<List<CaseOutcomePartyLinkHistoryVM>> GetCMSCaseOutcomePartyHistoryDetailById(string Id)
        {
            try
            {
                if (_casePartyOutcomeHistory == null)
                {
                    string StoredProc = $"exec pCaseOutcomePartyLinkHistoryDetail @OutcomeId = N'{Id}'";
                    _casePartyOutcomeHistory = await _dbContext.CaseOutcomePartyLinkHistoryVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _casePartyOutcomeHistory.ToList();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Outcome By Id
        public async Task<OutcomeHearing> GetOutcomeByOutcomeId(Guid outcomeId)
        {
            try
            {
                OutcomeHearing outcome = await _dbContext.OutcomeHearings.Where(x => x.Id == outcomeId).FirstOrDefaultAsync();
                return outcome;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get RegisterCases By ChamberId
        public async Task<List<CmsRegisteredCase>> GetRegisteredCasesByChamberNumberId(int chamberNumberId)
        {
            try
            {

                if (_CmsRegisteredCaseVMs == null)
                {
                    _CmsRegisteredCases = await _dbContext.cmsRegisteredCases.Where(x => x.ChamberNumberId == chamberNumberId).ToListAsync();
                    


                }
                return _CmsRegisteredCases;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);


            }


        }
		#endregion
		#region
		public async Task<string> GetFileAndCommunicationTypeInfo(int communicationTypeId, Guid referenceId)
        {
            string fileNumber =null;
            string caseNumber = null;
            string canNumber = null;
            string subject = null;
			using var scope = _serviceScopeFactory.CreateScope();
			var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
			var attachmentName = await _DbContext.CommunicationTypes.Where(x => x.CommunicationTypeId == communicationTypeId).Select(y => y.NameAr).FirstOrDefaultAsync();

			fileNumber = await _DbContext.CaseFiles.Where(x => x.FileId == referenceId).Select(y => y.FileNumber).FirstOrDefaultAsync();
            if ( fileNumber == null)
            {
                fileNumber = await _DbContext.ConsultationFiles.Where(x => x.FileId == referenceId).Select(y => y.FileNumber).FirstOrDefaultAsync();
            }
				 if ( fileNumber == null)
            {
	         var file = await _DbContext.cmsRegisteredCases.Where(x => x.CaseId == referenceId).FirstOrDefaultAsync();
                canNumber = file.CANNumber;
               caseNumber = file.CaseNumber;
			}
                if( caseNumber != null)
            {
                subject = attachmentName + '-' + caseNumber + '-' + canNumber;
                
            }
            else
            {
                subject = attachmentName + '-' + fileNumber;    
            }
            return subject;
		}

        #endregion
        public async Task SaveImportantCase(CaseUserImportant importantCase)
        {
            try
            {

                await _dbContext.CaseUserImportants.AddAsync(importantCase);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task DeleteImportantCase(CaseUserImportant importantCase)
        {
            try
            {

               var a = await _dbContext.CaseUserImportants.Where(x => x.ReferenceId == importantCase.ReferenceId && x.UserId == importantCase.UserId ).FirstOrDefaultAsync();   
               if(a != null)
                {
                    _dbContext.CaseUserImportants.Remove(a);
                    await _dbContext.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {

                throw;
            }

        }
        #region Mobile Application
        //<History Author = 'Noman Khan' Date='2024-07-10' Version="1.0" Branch="master">Get Hearing list By user Id </History>
        public async Task<List<MobileAppHearingListVM>> GetHearingListForMobileApp(string userId)
        {
            try
            {
                string StoredProc = $"exec pMobileAppHearingsListByUserId @userId ='{userId}' ";
                return await _dbContext.MobileAppHearingListVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<MobileAppHearingDetailVM> GetHearingDetailsForMobileApp(string hearingId)
        {
            try
            {
                string storedProc = $"exec pMobileAppHearingDetailsById @hearingId='{hearingId}'";
                var result = await _dbContext.MobileAppHearingDetailVM.FromSqlRaw(storedProc).ToListAsync();
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region CMS Registered Case Transfer Request
        public async Task AddCaseTransferRequest(CmsRegisteredCaseTransferRequestVM caseTransferRequestVM)
        {
            try
            {
                CmsRegisteredCaseTransferRequest caseTransferRequest = new CmsRegisteredCaseTransferRequest();
                caseTransferRequest.Id = caseTransferRequestVM.Id;
                caseTransferRequest.OutcomeId = caseTransferRequestVM.OutcomeId;
                caseTransferRequest.ChamberFromId = caseTransferRequestVM.ChamberFromId;
                caseTransferRequest.ChamberToId = caseTransferRequestVM.ChamberToId;
                caseTransferRequest.ChamberNumberFromId = caseTransferRequestVM.ChamberNumberFromId;
                caseTransferRequest.ChamberNumberToId = caseTransferRequestVM.ChamberNumberToId;
                caseTransferRequest.Remarks = caseTransferRequestVM.Remarks;
                caseTransferRequest.StatusId = caseTransferRequestVM.StatusId;
                caseTransferRequest.CreatedBy = caseTransferRequestVM.CreatedBy;
                caseTransferRequest.CreatedDate = DateTime.Now;
                caseTransferRequest.IsDeleted = false;
                await _dbContext.CmsRegisteredCaseTransferRequest.AddAsync(caseTransferRequest);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<CmsRegisteredCaseTransferRequestVM>> GetRegisterdCaseTransferRequestList(Guid outcomeId)
        {
            try
            {
                if (cmsRegisteredCaseTransferRequestListVMs == null)
                {
                    string StoredProc = $"exec pCmsCaseRegisteredTranferRequestDetailbyId @ReferenceId = N'{outcomeId}'";
                    cmsRegisteredCaseTransferRequestListVMs = await _dbContext.CmsRegisteredCaseTransferRequestVM.FromSqlRaw(StoredProc).ToListAsync();
                    if (cmsRegisteredCaseTransferRequestListVMs.Any())
                    {
                        foreach (var item in cmsRegisteredCaseTransferRequestListVMs)
                        {
                            item.rejectReason = await _dbContext.RejectReasons.FirstOrDefaultAsync(x => x.ReferenceId == item.Id);
                        }
                    }
                }
                return cmsRegisteredCaseTransferRequestListVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task RejectRegisteredCaseTransferRequest(CmsRegisteredCaseTransferRequestVM caseTransferRequestVM)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        RejectReason rejectReason = new RejectReason();
                        rejectReason.RejectionId = Guid.NewGuid();
                        rejectReason.ReferenceId = caseTransferRequestVM.Id;
                        rejectReason.Reason = caseTransferRequestVM.RejectionReason;
                        rejectReason.CreatedBy = caseTransferRequestVM.UserName;
                        rejectReason.CreatedDate = DateTime.Now;
                        await _dbContext.RejectReasons.AddAsync(rejectReason);
                        await SaveCMSRegisteredCaseTransferHistory(caseTransferRequestVM.caseTransferHistoryVM, _dbContext);
                        await UpdateRegisteredCaseTransferRequestStatus(caseTransferRequestVM, _dbContext);
                        await _dbContext.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public async Task ApproveRegisteredCaseTransferRequest(CmsRegisteredCaseTransferRequestVM caseTransferRequestVM)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var registeredCase = await _dbContext.CmsRegisteredCases.FindAsync(caseTransferRequestVM.CaseId);
                        if (registeredCase != null)
                        {
                            registeredCase.ChamberNumberId = caseTransferRequestVM.caseTransferHistoryVM.ChamberNumberToId;
                            registeredCase.ChamberId = caseTransferRequestVM.caseTransferHistoryVM.ChamberToId;
                            _dbContext.CmsRegisteredCases.Update(registeredCase);
                            await _dbContext.SaveChangesAsync();
                            await SaveRegisteredCaseStatusHistory(registeredCase, (int)RegisteredCaseEventEnum.CaseTransferedToAnotherChamber, _dbContext);
                            await SaveCMSRegisteredCaseTransferHistory(caseTransferRequestVM.caseTransferHistoryVM, _dbContext);
                            await UpdateRegisteredCaseTransferRequestStatus(caseTransferRequestVM, _dbContext);
                            transaction.Commit();
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public async Task UpdateRegisteredCaseTransferRequestStatus(CmsRegisteredCaseTransferRequestVM caseTransferRequestVM, DatabaseContext dbContext)
        {
            try
            {
                CmsRegisteredCaseTransferRequest caseTranferRequest = await _dbContext.CmsRegisteredCaseTransferRequest.Where(d => d.Id == caseTransferRequestVM.Id).FirstOrDefaultAsync();
                if (caseTranferRequest != null)
                {
                    caseTranferRequest.StatusId = (int)caseTransferRequestVM.StatusId;
                    caseTranferRequest.ModifiedBy = caseTransferRequestVM.UserName;
                    caseTranferRequest.ModifiedDate = DateTime.Now;
                    dbContext.CmsRegisteredCaseTransferRequest.Update(caseTranferRequest);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<CmsRegisteredCaseTransferRequestVM> GetResgisteredCaseTansferRequestDetailById(Guid ReferenceId)
        {
            try
            {
                if (_CmsRegisteredCaseTransferRequestDetailVMs == null)
                {
                    string StoredProc = $"exec pCmsCaseRegisteredTranferRequestDetailbyId @ReferenceId = N'{ReferenceId}'";
                    var result = await _dbContext.CmsRegisteredCaseTransferRequestVM.FromSqlRaw(StoredProc).ToListAsync();
                    if (result.Any())
                    {
                        _CmsRegisteredCaseTransferRequestDetailVMs = result.FirstOrDefault();
                        _CmsRegisteredCaseTransferRequestDetailVMs.rejectReason = await _dbContext.RejectReasons.FirstOrDefaultAsync(x => x.ReferenceId == ReferenceId);
                    }
                }
                return _CmsRegisteredCaseTransferRequestDetailVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> SoftDeleteCaseTransferRequest(Guid Id, string userName)
        {
            try
            {
                CmsRegisteredCaseTransferRequest caseTranferRequest = await _dbContext.CmsRegisteredCaseTransferRequest.Where(d => d.Id == Id && d.IsDeleted==false).FirstOrDefaultAsync();
                if (caseTranferRequest != null)
                {
                    caseTranferRequest.IsDeleted = true;
                    caseTranferRequest.DeletedBy = userName;
                    caseTranferRequest.DeletedDate = DateTime.Now;
                    _dbContext.CmsRegisteredCaseTransferRequest.Update(caseTranferRequest);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Case detail

        //<History Author = 'Danish' Date='2024-09-10' Version="1.0" Branch="master">Get Case detail</History>
        public async Task<CaseDetailMOJVM> GetCaseDetailForMOJ(Guid caseId)
        {
            try
            {
                var caseDetailMOJVM = new CaseDetailMOJVM();

                if (caseDetailMOJVMs == null)
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                    using (_DbContext)
                    {
                        string StoredProc = $"exec pGetCaseDetailInMOJByCaseId @caseId ='{caseId}'";
                        caseDetailMOJVMs = await _DbContext.CaseDetailMOJVMs.FromSqlRaw(StoredProc).ToListAsync(); 
                    }
                }
                return caseDetailMOJVMs.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

    }
}



