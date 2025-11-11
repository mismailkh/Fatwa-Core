using AutoMapper;
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Extensions;
using FATWA_DOMAIN.Interfaces.CaseManagement;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.Shared;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_INFRASTRUCTURE.Database;
using FATWA_INFRASTRUCTURE.Repository.PatternNumber;
using FATWA_INFRASTRUCTURE.Repository.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.UserEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_INFRASTRUCTURE.Repository.NotificationRepo;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using System.Reflection.Metadata;
using System;
using FATWA_DOMAIN.Models.ViewModel.MojRollsVM;
using FATWA_INFRASTRUCTURE.Repository.G2G;



namespace FATWA_INFRASTRUCTURE.Repository.CaseManagement
{
    //<!-- <History Author = 'Hassan Abbas' Date='2024-02-01' Version="1.0" Branch="master">API Controller For Data Migration of MOJ</History> -->
    public class CmsMojRPARepository : ICmsMojRPA
    {
        #region Variables declaration

        private readonly DatabaseContext _dbContext;
        private readonly DmsDbContext _dmsDbContext;

        private readonly G2GRepository _g2gRepository;
        private readonly CmsRegisteredCaseRepository _registeredCaseRepository;
        private readonly CmsCaseFileRepository _caseFileRepository;
        private readonly CMSCaseRequestRepository _caseRequestRepository;
        private readonly TaskRepository _taskRepository;
        private readonly RoleRepository _roleRepository;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private List<CmsCaseRequestVM> _CmsCaseRequestVMs;
        private CmsAssignCaseFileBackToHos _sendBackToHos;
        private List<CmsAssignCaseFileBackToHos> _sendBackToHosList;
        private CaseAssignment _caseAssignment;
        private WorkflowRepository _workflowRepository;
        private List<CmsTransferHistoryVM> _cmsTransferHistoryVMs;
        private List<RequestListVM> _CmsRequestVMs;
        private List<CasePartyLinkExecutionVM> _CaseParty;
        private readonly IMapper _mapper;
        private readonly CMSCOMSInboxOutboxPatternNumberRepository _cMSCOMSInboxOutboxPatternNumberRepository;
        private readonly NotificationRepository _notificationRepository;
        #endregion

        #region Constructor
        public CmsMojRPARepository(DatabaseContext dbContext, DmsDbContext dmsdbContext, IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            _dbContext = dbContext;
            _dmsDbContext = dmsdbContext;
            _serviceScopeFactory = serviceScopeFactory;
            using var scope = _serviceScopeFactory.CreateScope();
            _g2gRepository = scope.ServiceProvider.GetRequiredService<G2GRepository>();
            _caseRequestRepository = scope.ServiceProvider.GetRequiredService<CMSCaseRequestRepository>();
            _caseFileRepository = scope.ServiceProvider.GetRequiredService<CmsCaseFileRepository>();
            _registeredCaseRepository = scope.ServiceProvider.GetRequiredService<CmsRegisteredCaseRepository>();
            _cMSCOMSInboxOutboxPatternNumberRepository = scope.ServiceProvider.GetRequiredService<CMSCOMSInboxOutboxPatternNumberRepository>();
            _notificationRepository = scope.ServiceProvider.GetRequiredService<NotificationRepository>();
            _roleRepository = scope.ServiceProvider.GetRequiredService<RoleRepository>();
            _mapper = mapper;
        }
        #endregion

        #region Add Case Data

        //<History Author = 'Hassan Abbas' Date='2024-02-01' Version="1.0" Branch="master"Add Case Data </History>
        //<History Author = 'Hassan Abbas' Date='2024-02-22' Version="1.0" Branch="master"Modified the function to pick Sector Responsible for the Chamber of the case </History>
        public async Task<CmsMojRPACaseData> AddCaseData(CmsMojRPAPayloadVM payload)
        {
            CmsMojRPACaseData caseData = new CmsMojRPACaseData();
            CmsMojRpaPayload mojRpaPayload = new CmsMojRpaPayload { Id = Guid.NewGuid(), Payload = JsonConvert.SerializeObject(payload), CreatedBy = "MOJ RPA", CreatedDate = DateTime.Now };
            await _dbContext.CmsMojRpaPayloads.AddAsync(mojRpaPayload);
            await _dbContext.SaveChangesAsync();

            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        payload.CourtId = await _dbContext.Courts.Where(c => c.CourtCode == payload.CourtCode).Select(c => c.Id).FirstOrDefaultAsync();
                        payload.ChamberId = await _dbContext.Chambers.Where(c => c.ChamberCode == payload.ChamberCode).Select(c => c.Id).FirstOrDefaultAsync();
                        payload.ChamberNumberId = await _dbContext.ChamberNumbers.Where(c => c.Number == payload.ChamberNumber).Select(c => c.Id).FirstOrDefaultAsync();

                        int sectorTypeId = await _dbContext.ChamberOperatingSectors.Where(c => c.ChamberId == payload.ChamberId).Select(c => c.SectorTypeId).FirstOrDefaultAsync();

                        CaseFile caseFile;
                        CmsRegisteredCase existingCase = await _dbContext.CmsRegisteredCases.FirstOrDefaultAsync(x => x.CANNumber == payload.CANNumber);
                        if (existingCase != null)
                        {
                            caseData.CaseFile = await _dbContext.CaseFiles.FirstOrDefaultAsync(x => x.FileId == existingCase.FileId);
                            var assignment = await _dbContext.CmsCaseFileSectorAssignment.Where(c => c.FileId == caseData.CaseFile.FileId).OrderByDescending(c => c.CreatedDate).FirstOrDefaultAsync();
                            if (assignment != null)
                            {
                                sectorTypeId = (int)assignment.SectorTypeId;
                            }
                        }
                        else
                        {
                            caseData.CaseRequest = await CreateCaseRequest(payload, sectorTypeId, _dbContext);
                            caseData.CaseFile = await CreateCaseFile(payload, caseData.CaseRequest, caseData, _dbContext);
                        }
                        caseData.RegisteredCase = await CreateRegisteredCase(payload, caseData.CaseFile, sectorTypeId, caseData, _dbContext);

                        CmsMojRpaPayload g2gPayload = new CmsMojRpaPayload { Id = Guid.NewGuid(), Payload = "G2G Payload: " + JsonConvert.SerializeObject(caseData), CreatedBy = "MOJ RPA", CreatedDate = DateTime.Now };
                        await _dbContext.CmsMojRpaPayloads.AddAsync(g2gPayload);
                        await _dbContext.SaveChangesAsync();
                        transaction.Commit();
                        // For Notification
                        payload.NotificationParameter.FileNumber = caseData.CaseFile.FileNumber;
                        payload.NotificationParameter.CaseNumber = payload.CaseNumber;

                        return caseData;
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

        #region Create Case Request 
        //<History Author = 'Hassan Abbas' Date='2024-02-01' Version="1.0" Branch="master"> Create Case Request </History>
        public async Task<CaseRequest> CreateCaseRequest(CmsMojRPAPayloadVM payload, int sectorTypeId, DatabaseContext dbContext)
        {
            try
            {
                var g2gUser = await _g2gRepository.GetNextGEUserForRequestAssignment(payload.GovtEntityId, true);
                CaseRequest caseRequest = new CaseRequest();
                int requestTypeId = CaseConsultationExtension.GetRequestTypeIdBasedOnSectorIdForCases(sectorTypeId);
                int courtTypeId = await dbContext.Courts.Where(x => x.Id == payload.CourtId).Select(y => y.TypeId).FirstOrDefaultAsync();
                var resultCaseRequestNumber = await _cMSCOMSInboxOutboxPatternNumberRepository.GenerateNumberPattern(payload.GovtEntityId, (int)CmsComsNumPatternTypeEnum.CaseRequestNumber);
                caseRequest.RequestNumber = resultCaseRequestNumber.GenerateRequestNumber;
                caseRequest.RequestNumberFormat = resultCaseRequestNumber.FormatRequestNumber;
                caseRequest.PatternSequenceResult = resultCaseRequestNumber.PatternSequenceResult;
                caseRequest.RequestId = Guid.NewGuid();
                caseRequest.Subject = payload.Subject;
                caseRequest.GovtEntityId = payload.GovtEntityId;
                caseRequest.RequestTypeId = requestTypeId > 0 ? requestTypeId : null;
                caseRequest.SectorTypeId = sectorTypeId > 0 ? sectorTypeId : null;
                caseRequest.PriorityId = (int)PriorityEnum.Low;
                caseRequest.StatusId = (int)CaseRequestStatusEnum.RegisteredInMOJ;
                caseRequest.CourtTypeId = courtTypeId;
                caseRequest.IsViewed = true;
                caseRequest.AssignedBy = "MOJ RPA";
                caseRequest.CreatedBy = string.IsNullOrEmpty(g2gUser.Item1) ? "MOJ RPA" : g2gUser.Item1;
                caseRequest.DepartmentId = string.IsNullOrEmpty(g2gUser.Item1) ? null : g2gUser.Item2;
                caseRequest.CreatedDate = DateTime.Now;
                caseRequest.Pledge = true;
                await dbContext.CaseRequests.AddAsync(caseRequest);
                await dbContext.SaveChangesAsync();
                return caseRequest;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Create Case file 
        //<History Author = 'Hassan Abbas' Date='2024-02-01' Version="1.0" Branch="master"> Create Case File </History>
        public async Task<CaseFile> CreateCaseFile(CmsMojRPAPayloadVM payload, CaseRequest caseRequest, CmsMojRPACaseData caseData, DatabaseContext dbContext)
        {
            try
            {
                CmsRegisteredCase regCase = await dbContext.CmsRegisteredCases.FirstOrDefaultAsync(x => x.CANNumber == payload.CANNumber);
                if (regCase != null)
                {
                    return await dbContext.CaseFiles.FirstOrDefaultAsync(x => x.FileId == regCase.FileId);
                }
                CaseFile casefile = new CaseFile();
                GovernmentEntity entity = await _dbContext.GovernmentEntity.FindAsync(payload.GovtEntityId);
                var resultCaseFileNumber = await _cMSCOMSInboxOutboxPatternNumberRepository.GenerateNumberPattern(0, (int)CmsComsNumPatternTypeEnum.CaseFileNumber);
                casefile.FileNumber = resultCaseFileNumber.GenerateRequestNumber;
                casefile.CaseFileNumberFormat = resultCaseFileNumber.FormatRequestNumber;
                casefile.PatternSequenceResult = resultCaseFileNumber.PatternSequenceResult;
                //casefile.FileNumber = DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + "CAF" + (_dbContext.CaseFiles.Any() ? await _dbContext.CaseFiles.Select(x => x.ShortNumber).MaxAsync() + 1 : 1).ToString().PadLeft(6, '0');
                //casefile.CaseFileNumberFormat = "YY/-/MM/-/CAF/-/000000";
                //casefile.ShortNumber = _dbContext.CaseFiles.Any() ? await _dbContext.CaseFiles.Select(x => x.ShortNumber).MaxAsync() + 1 : 1;

                casefile.RequestId = caseRequest.RequestId;
                casefile.FileName = casefile.FileNumber + "_" + entity?.Name_En + "_" + DateOnly.FromDateTime(DateTime.Now).ToString("dd/MM/yyyy");
                casefile.CreatedBy = "MOJ RPA";
                casefile.CreatedDate = DateTime.Now;
                casefile.StatusId = (int)CaseFileStatusEnum.InProgress;
                casefile.SectorTypeId = caseRequest.SectorTypeId;
                await dbContext.CaseFiles.AddAsync(casefile);
                await dbContext.SaveChangesAsync();
                if (casefile.SectorTypeId > 0)
                    await SaveCaseFileSectorAssignment(casefile, dbContext);
                caseData.FileStatusHistory = await SaveCaseFileStatusHistory("MOJ RPA", casefile, (int)CaseFileEventEnum.MigratedFromMOJ, dbContext);
                return casefile;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CmsCaseFileStatusHistory> SaveCaseFileStatusHistory(string userName, CaseFile caseFile, int EventId, DatabaseContext dbContext)
        {
            try
            {
                CmsCaseFileStatusHistory historyobj = new CmsCaseFileStatusHistory();
                historyobj.HistoryId = Guid.NewGuid();
                historyobj.FileId = caseFile.FileId;
                historyobj.StatusId = (int)caseFile.StatusId;
                historyobj.CreatedBy = userName;
                historyobj.CreatedDate = DateTime.Now;
                historyobj.EventId = EventId;
                historyobj.Remarks = caseFile.Remarks;
                await dbContext.CmsCaseFileStatusHistory.AddAsync(historyobj);
                await dbContext.SaveChangesAsync();
                return historyobj;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CasePartyLink>> CopyCasePartiesFromSourceToDestination(Guid sourceId, Guid destinationId, CmsMojRPACaseData caseData, DatabaseContext dbContext)
        {
            List<CasePartyLink> copyCaseParties = new List<CasePartyLink>();
            try
            {
                var requestParties = await dbContext.CasePartyLink.Where(p => p.ReferenceGuid == sourceId).AsNoTracking().ToListAsync();
                foreach (var party in requestParties)
                {
                    var partyId = party.Id;
                    CasePartyLink fileParty = party;
                    fileParty.Id = Guid.NewGuid();
                    fileParty.ReferenceGuid = destinationId;
                    fileParty.CreatedBy = "MOJ RPA";
                    fileParty.CreatedDate = DateTime.Now;
                    await dbContext.CasePartyLink.AddAsync(fileParty);
                    await dbContext.SaveChangesAsync();
                    caseData.CasePartyLinks.Add(fileParty);
                }
                return copyCaseParties;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Create Registered Case
        //<History Author = 'Hassan Abbas' Date='2024-02-01' Version="1.0" Branch="master"> Create Case File </History>
        public async Task<CmsRegisteredCase> CreateRegisteredCase(CmsMojRPAPayloadVM payload, CaseFile caseFile, int sectorTypeId, CmsMojRPACaseData caseData, DatabaseContext dbContext)
        {
            try
            {
                int requestTypeId = CaseConsultationExtension.GetRequestTypeIdBasedOnSectorIdForCases(sectorTypeId);
                var regCase = _mapper.Map<CmsRegisteredCase>(payload);
                regCase.FileId = caseFile.FileId;
                regCase.RequestTypeId = requestTypeId > 0 ? requestTypeId : null;
                regCase.SectorTypeId = sectorTypeId > 0 ? sectorTypeId : null;
                regCase.StatusId = (int)RegisteredCaseStatusEnum.Open;
                regCase.FloorNumber = "0";
                regCase.AnnouncementNumber = "0";
                regCase.RoomNumber = "0";
                regCase.CreatedBy = "MOJ RPA";
                regCase.CreatedDate = DateTime.Now;
                await dbContext.CmsRegisteredCases.AddAsync(regCase);
                await dbContext.SaveChangesAsync();
                await AddCaseParties(payload.CaseParties, regCase.CaseId, caseData, dbContext);
                await CopyCasePartiesFromSourceToDestination(regCase.CaseId, regCase.FileId, caseData, dbContext);
                await AddHearings(payload.Hearings, regCase.CaseId, caseData, dbContext);
                await AddExecutions(payload.Executions, regCase.CaseId, caseData, dbContext);
                await AddCaseAnnouncements(payload.Announcements, regCase.CaseId, caseData, dbContext);
                await SaveRegisteredCaseStatusHistory(regCase, (int)RegisteredCaseEventEnum.MigratedFromMOJ, caseData, dbContext, "", "MOJ RPA");
                return regCase;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task SaveRegisteredCaseStatusHistory(CmsRegisteredCase registeredCase, int EventId, CmsMojRPACaseData caseData, DatabaseContext dbContext, string remarks = "", string username = "")
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
                if (caseData != null)
                {
                    caseData.CaseStatusHistory = historyobj;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        //<History Author = 'Hassan Abbas' Date='2024-02-01' Version="1.0" Branch="master"> Create Case Parties </History>
        public async Task AddCaseParties(List<CasePartyRPA> parties, Guid caseId, CmsMojRPACaseData caseData, DatabaseContext dbContext)
        {
            try
            {
                foreach (var party in parties)
                {
                    var caseParty = _mapper.Map<CasePartyLink>(party);
                    if (party.RepresentativeNumber != null)
                    {
                        var representative = await dbContext.GovernmentEntityRepresentative.FirstOrDefaultAsync(x => x.RepresentativeCode == party.RepresentativeNumber);
                        if (representative != null)
                        {
                            caseParty.RepresentativeId = representative.Id;
                        }
                        else
                        {
                            GovernmentEntityRepresentative newRepresentative = await SaveGovtEntityRepresentative(party, dbContext);
                            caseData.GovernmentEntityRepresentatives.Add(newRepresentative);
                            caseParty.RepresentativeId = newRepresentative.Id;
                        }
                    }
                    caseParty.ReferenceGuid = caseId;
                    caseParty.EntityId = party.GovtEntityId;
                    caseParty.CreatedBy = "MOJ RPA";
                    caseParty.CreatedDate = DateTime.Now;
                    await dbContext.CasePartyLink.AddAsync(caseParty);
                    await dbContext.SaveChangesAsync();
                    caseData.CasePartyLinks.Add(caseParty);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2024-02-01' Version="1.0" Branch="master"> Save Govt Entity Represenative </History>
        public async Task<GovernmentEntityRepresentative> SaveGovtEntityRepresentative(CasePartyRPA party, DatabaseContext dbContext)
        {
            try
            {
                GovernmentEntityRepresentative representative = new GovernmentEntityRepresentative();
                representative.NameEn = party.RepresentativeNameEn;
                representative.NameAr = party.RepresentativeNameAr;
                representative.RepresentativeCode = party.RepresentativeNumber;
                representative.GovtEntityId = party.GovtEntityId != null ? (int)party.GovtEntityId : 0; //can be null ??????
                representative.IsActive = true;
                representative.CreatedBy = "MOJ RPA";
                representative.CreatedDate = DateTime.Now;
                await dbContext.GovernmentEntityRepresentative.AddAsync(representative);
                await dbContext.SaveChangesAsync();
                return representative;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        //<History Author = 'Hassan Abbas' Date='2024-02-01' Version="1.0" Branch="master"> Add Hearings, Its Outcome and the Judgements of a Execution.</History>
        public async Task AddHearings(List<HearingRPA> hearings, Guid caseId, CmsMojRPACaseData caseData, DatabaseContext dbContext)
        {
            try
            {
                foreach (var hearing in hearings)
                {
                    var caseHearing = _mapper.Map<Hearing>(hearing);
                    caseHearing.CaseId = caseId;
                    caseHearing.CreatedBy = "MOJ RPA";
                    caseHearing.CreatedDate = DateTime.Now;
                    await dbContext.Hearings.AddAsync(caseHearing);
                    await dbContext.SaveChangesAsync();
                    caseData.Hearings.Add(caseHearing);
                    if (hearing.OutcomeHearing != null && hearing.OutcomeHearing.Any())
                    {
                        var outcomeRPA = hearing.OutcomeHearing.FirstOrDefault();
                        var outcomeHearing = _mapper.Map<OutcomeHearing>(outcomeRPA);
                        outcomeHearing.HearingId = caseHearing.Id;
                        outcomeHearing.CaseId = caseId;
                        outcomeHearing.CreatedBy = "MOJ RPA";
                        outcomeHearing.CreatedDate = DateTime.Now;
                        await dbContext.OutcomeHearings.AddAsync(outcomeHearing);
                        await dbContext.SaveChangesAsync();
                        caseData.OutcomeHearings.Add(outcomeHearing);
                        await AddJudgements(outcomeRPA.Judgements, outcomeHearing.Id, caseId, caseData, dbContext);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2024-02-01' Version="1.0" Branch="master"> Add Jusgements </History>
        private async Task AddJudgements(List<JudgementRPA> judgements, Guid outcomeHearingId, Guid caseId, CmsMojRPACaseData caseData, DatabaseContext dbContext)
        {
            try
            {
                foreach (var judgement in judgements)
                {
                    int judgementTypeId = await FindOrSaveJudgementType(judgement.Type, caseData, dbContext);
                    int judgementCategoryId = await FindOrSaveJudgementCategory(judgement.Category, caseData, dbContext);
                    var caseJudgement = _mapper.Map<Judgement>(judgement);
                    caseJudgement.OutcomeId = outcomeHearingId;
                    caseJudgement.TypeId = judgementTypeId;
                    caseJudgement.CategoryId = judgementCategoryId;
                    caseJudgement.CaseId = caseId;
                    caseJudgement.CreatedBy = "MOJ RPA";
                    caseJudgement.CreatedDate = DateTime.Now;
                    await dbContext.Judgements.AddAsync(caseJudgement);
                    await dbContext.SaveChangesAsync();
                    caseData.Judgements.Add(caseJudgement);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2024-02-22' Version="1.0" Branch="master"> Add new Judgement Type or return Rexisting </History>
        private async Task<int> FindOrSaveJudgementType(string type, CmsMojRPACaseData caseData, DatabaseContext dbContext)
        {
            try
            {
                var existingJudgementType = await dbContext.JudgementTypes.FirstOrDefaultAsync(x => x.NameAr == type);
                if (existingJudgementType == null)
                {
                    JudgementType newJudgementType = new JudgementType
                    {
                        NameAr = type,
                        NameEn = type
                    };
                    await dbContext.JudgementTypes.AddAsync(newJudgementType);
                    await dbContext.SaveChangesAsync();
                    caseData.JudgementTypes.Add(newJudgementType);
                    return newJudgementType.Id;
                }
                else
                {
                    return existingJudgementType.Id;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2024-02-22' Version="1.0" Branch="master"> Add new Judgement Type or return Rexisting </History>
        private async Task<int> FindOrSaveJudgementCategory(string category, CmsMojRPACaseData caseData, DatabaseContext dbContext)
        {
            try
            {
                var existingJudgementCategory = await dbContext.JudgementCategories.FirstOrDefaultAsync(x => x.NameAr == category);
                if (existingJudgementCategory == null)
                {
                    JudgementCategory newJudgementCategory = new JudgementCategory
                    {
                        NameAr = category,
                        NameEn = category,
                        CreatedBy = "MOJ RPA",
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                    };
                    await dbContext.JudgementCategories.AddAsync(newJudgementCategory);
                    await dbContext.SaveChangesAsync();
                    caseData.JudgementCategories.Add(newJudgementCategory);
                    return newJudgementCategory.Id;
                }
                else
                {
                    return existingJudgementCategory.Id;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //<History Author = 'Hassan Abbas' Date='2024-02-01' Version="1.0" Branch="master"> Add Executions </History>
        public async Task AddExecutions(List<ExecutionRPA> executions, Guid caseId, CmsMojRPACaseData caseData, DatabaseContext dbContext)
        {
            try
            {
                foreach (var execution in executions)
                {
                    if (execution.PayerId == null || execution.PayerId == Guid.Empty)
                    {
                        if (!String.IsNullOrEmpty(execution.MismatchedPayerName))
                        {
                            ExecutionPartyLink executionParty = new ExecutionPartyLink
                            {
                                Id = Guid.NewGuid(),
                                Name = execution.MismatchedPayerName,
                                CreatedBy = "MOJ RPA",
                                CreatedDate = DateTime.Now,
                            };
                            await dbContext.ExecutionPartyLinks.AddAsync(executionParty);
                            await dbContext.SaveChangesAsync();
                            caseData.ExecutionPartyLinks.Add(executionParty);
                            execution.PayerId = executionParty.Id;
                        }
                    }
                    if (execution.ReceiverId == null || execution.ReceiverId == Guid.Empty)
                    {
                        if (!String.IsNullOrEmpty(execution.MismatchedRecieverName))
                        {
                            ExecutionPartyLink executionParty = new ExecutionPartyLink
                            {
                                Id = Guid.NewGuid(),
                                Name = execution.MismatchedRecieverName,
                                CreatedBy = "MOJ RPA",
                                CreatedDate = DateTime.Now,
                            };
                            await dbContext.ExecutionPartyLinks.AddAsync(executionParty);
                            await dbContext.SaveChangesAsync();
                            caseData.ExecutionPartyLinks.Add(executionParty);
                            execution.ReceiverId = executionParty.Id;
                        }
                    }
                    var caseExecution = _mapper.Map<CmsJudgmentExecution>(execution);
                    caseExecution.FileStatusId = (int)ExecutionStatusEnum.Open;
                    caseExecution.CaseId = caseId;
                    caseExecution.CreatedBy = "MOJ RPA";
                    caseExecution.CreatedDate = DateTime.Now;
                    await dbContext.CmsJudgmentExecutions.AddAsync(caseExecution);
                    await dbContext.SaveChangesAsync();
                    caseData.CmsJudgmentExecutions.Add(caseExecution);
                    foreach (var executionAnouncement in execution.ExecutionAnouncements)
                    {
                        var anouncement = _mapper.Map<ExecutionAnouncement>(executionAnouncement);
                        anouncement.ExecutionId = caseExecution.Id;
                        anouncement.CreatedBy = "MOJ RPA";
                        anouncement.CreatedDate = DateTime.Now;
                        await dbContext.ExecutionAnouncements.AddAsync(anouncement);
                        await dbContext.SaveChangesAsync();
                        caseData.ExecutionAnouncements.Add(anouncement);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2024-03-10' Version="1.0" Branch="master"> Add Case Announcements </History>
        public async Task AddCaseAnnouncements(List<AnnouncementRPA> announcements, Guid caseId, CmsMojRPACaseData caseData, DatabaseContext dbContext)
        {
            try
            {
                foreach (var announcement in announcements)
                {
                    var caseAnnouncement = _mapper.Map<CaseAnnouncement>(announcement);
                    caseAnnouncement.CaseId = caseId;
                    caseAnnouncement.CreatedBy = "MOJ RPA";
                    caseAnnouncement.CreatedDate = DateTime.Now;
                    await dbContext.CaseAnnouncements.AddAsync(caseAnnouncement);
                    await dbContext.SaveChangesAsync();
                    caseData.CaseAnnouncements.Add(caseAnnouncement);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Save Case File Sector Assignment
        //<History Author = 'Hassan Abbas' Date='2022-12-24' Version="1.0" Branch="master"> Save Case File Sector Assignment</History>
        public async Task SaveCaseFileSectorAssignment(CaseFile file, DatabaseContext dbContext)
        {
            try
            {
                CmsCaseFileSectorAssignment assignmentobj = new CmsCaseFileSectorAssignment();
                assignmentobj.FileId = file.FileId;
                assignmentobj.SectorTypeId = (int)file.SectorTypeId;
                assignmentobj.CreatedBy = "MOJ RPA";
                assignmentobj.CreatedDate = DateTime.Now;
                await dbContext.CmsCaseFileSectorAssignment.AddAsync(assignmentobj);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Get Migrated Unassigned Cases from MOJ 
        //<History Author = 'Hassan Abbas' Date='2024-02-26' Version="1.0" Branch="master"> Get cases which were migrated from MOJ but are not assigned to any sector</History>
        public async Task<List<MojUnassignedCaseFileVM>> GetUnassignedMigratedCaseFilesList(AdvanceSearchCmsCaseFileVM advanceSearchVM)
        {
            try
            {
                string CreatedfromDate = advanceSearchVM.CreatedFrom != null ? Convert.ToDateTime(advanceSearchVM.CreatedFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string CreatedtoDate = advanceSearchVM.CreatedTo != null ? Convert.ToDateTime(advanceSearchVM.CreatedTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string StoreProc = $"exec pCmsMojUnassignedCaseFileList @fileNumber = '{advanceSearchVM.FileNumber}', @createdFrom = '{CreatedfromDate}' , @createdTo = '{CreatedtoDate}', @PageNumber='{advanceSearchVM.PageNumber}', @PageSize='{advanceSearchVM.PageSize}'";
                var registerdRequestVMs = await _dbContext.MojUnassignedCaseFileVMs.FromSqlRaw(StoreProc).ToListAsync();
                return registerdRequestVMs;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Assign Unassigned Files To Sector
        //<History Author = 'Hassan Abbas' Date='2024-02-26' Version="1.0" Branch="master"> Assign case files which were migrated from MOJ to selected sector</History>
        public async Task<AssignMojCaseFileToSectorVM> AssignUnassignedFilesToSector(AssignMojCaseFileToSectorVM sectorAssignment)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        sectorAssignment.RequestTypeId = CaseConsultationExtension.GetRequestTypeIdBasedOnSectorIdForCases(sectorAssignment.SectorTypeId);
                        foreach (var fileId in sectorAssignment.FileIds)
                        {
                            CaseFile caseFile = await _dbContext.CaseFiles.FindAsync(fileId);
                            CaseRequest caseRequest = await _dbContext.CaseRequests.FindAsync(caseFile.RequestId);
                            caseFile.SectorTypeId = sectorAssignment.SectorTypeId;
                            caseRequest.SectorTypeId = sectorAssignment.SectorTypeId;
                            caseRequest.RequestTypeId = sectorAssignment.RequestTypeId;
                            _dbContext.CaseRequests.Update(caseRequest);

                            await SaveCaseFileSectorAssignment(caseFile, _dbContext);
                            var regCases = await _dbContext.CmsRegisteredCases.Where(c => c.FileId == fileId).ToListAsync();
                            foreach (var regCase in regCases)
                            {
                                regCase.SectorTypeId = sectorAssignment.SectorTypeId;
                                regCase.RequestTypeId = sectorAssignment.RequestTypeId;
                                _dbContext.CmsRegisteredCases.Update(regCase);
                            }
                        }
                        await _dbContext.SaveChangesAsync();
                        transaction.Commit();
                        // For Notification
                        sectorAssignment.NotificationParameter.SectorTo = _dbContext.OperatingSectorType
                                                                        .Where(x => x.Id == sectorAssignment.SectorTypeId)
                                                                        .Select(x => x.Name_En + "/" + x.Name_Ar)
                                                                        .FirstOrDefault();
                        return sectorAssignment;
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
        #region Add Hearing & OutCome daeta Data
        //<History Author = 'Ijaz Ahmad' Date='2024-03-21' Version="1.0" Branch="master"> MOJ RPA Add Hearing Data</History>
        public async Task<List<CanAndCaseNumber>> AddHearingData(List<CanAndCaseNumber> canAndCaseNumbers, DateTime HearingDate, int DocumentId)
        {


            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        List<CanAndCaseNumber> CanAndCaseNumber = new List<CanAndCaseNumber>();
                        var MojrollsAttachment = await _dmsDbContext.UploadedDocuments.Where(x => x.UploadedDocumentId == DocumentId).FirstOrDefaultAsync();
                        foreach (var item in canAndCaseNumbers)
                        {
                            //CanAndCaseNumber cmsMojRPAHearing = new CanAndCaseNumber();
                            //cmsMojRPAHearing.NotificationParameter = item.NotificationParameter;
                            var registeredCase = await _dbContext.cmsRegisteredCases.FirstOrDefaultAsync(x => x.CANNumber == item.CANNumber && x.CaseNumber == item.CaseNumber);
                            if (registeredCase != null)
                            {

                                // DateTime hearingdate = HearingDate.Date;
                                var existingHearing = await _dbContext.Hearings.FirstOrDefaultAsync(h => h.CaseId == registeredCase.CaseId && h.HearingDate == HearingDate);

                                if (existingHearing == null)
                                {
                                    var caseAssignmentLawyer = await _dbContext.CaseFileAssignment.FirstOrDefaultAsync(x => x.ReferenceId == registeredCase.CaseId && x.IsPrimary == true);
                                    var hearing = new Hearing
                                    {
                                        HearingDate = HearingDate,
                                        CaseId = registeredCase.CaseId,
                                        StatusId = (int)RegisteredCaseEventEnum.HearingScheduled,
                                        LawyerId = caseAssignmentLawyer != null && !String.IsNullOrEmpty(caseAssignmentLawyer.LawyerId) ? caseAssignmentLawyer.LawyerId : null,
                                        Description = "-",
                                        CreatedBy = "MOJ RPA",
                                        CreatedDate = DateTime.Now
                                    };

                                    //if (caseAssignmentLawyer != null) for later  Implementastion when Notification module is complete
                                    //{


                                    //    await SendHearingNotification(caseAssignmentLawyer.ReferenceId, caseAssignmentLawyer.LawyerId, cmsMojRPAHearing.NotificationParameter);
                                    //}
                                    //else
                                    //{
                                    //    var assignedTo = await _roleRepository.GetHOSBySectorId((int)registeredCase.SectorTypeId);
                                    //    await SendHearingNotification(registeredCase.CaseId, assignedTo.Id, cmsMojRPAHearing.NotificationParameter);

                                    //}
                                    await _dbContext.Hearings.AddAsync(hearing);
                                    await _dbContext.SaveChangesAsync();
                                    //  upload document
                                    if (MojrollsAttachment != null)
                                    {
                                        await CreateMojRollsAttachmentForHearing(MojrollsAttachment, hearing.Id);
                                    }
                                }
                                else
                                {
                                    if (MojrollsAttachment != null)
                                    {
                                        var AttachementExist = await _dmsDbContext.UploadedDocuments.FirstOrDefaultAsync(x => x.ReferenceGuid == existingHearing.Id && x.StoragePath == MojrollsAttachment.StoragePath);
                                        if (AttachementExist == null)
                                        {
                                            await CreateMojRollsAttachmentForHearing(MojrollsAttachment, existingHearing.Id);
                                        }
                                    }
                                }
                                CanAndCaseNumber.Add(item);
                            }


                        }
                        transaction.Commit();
                        return CanAndCaseNumber;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);

                    }
                }
            }
        }

        //<History Author = 'Ijaz Ahmad' Date='2024-03-25' Version="1.0" Branch="master"> Add OutcomeHearing Data</History>
        public async Task<List<CanAndCaseNumber>> AddOutcomeHearingData(List<CanAndCaseNumber> canAndCaseNumbers, int DocumentId, DateTime HearingDate, DateTime? NextHearingDate)
        {


            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        List<CanAndCaseNumber> CanAndCaseNumber = new List<CanAndCaseNumber>();
                        var MojrollsAttachment = await _dmsDbContext.UploadedDocuments.Where(x => x.UploadedDocumentId == DocumentId).FirstOrDefaultAsync();
                        foreach (var item in canAndCaseNumbers)
                        {
                            var registeredCase = await _dbContext.cmsRegisteredCases.FirstOrDefaultAsync(x => x.CANNumber == item.CANNumber && x.CaseNumber == item.CaseNumber);
                            if (registeredCase != null)
                            {
                                var ExistingHearing = await _dbContext.Hearings.Where(x => x.HearingDate == HearingDate && x.CaseId == registeredCase.CaseId).FirstOrDefaultAsync();
                                if (ExistingHearing != null)
                                {
                                    var caseAssignmentLawyer = await _dbContext.CaseFileAssignment.FirstOrDefaultAsync(x => x.ReferenceId == registeredCase.CaseId);
                                    OutcomeHearing outcomehearing = new OutcomeHearing
                                    {
                                        HearingDate = HearingDate,
                                        NextHearingDate = NextHearingDate,
                                        CaseId = registeredCase.CaseId,
                                        HearingId = ExistingHearing.Id,//Hearing Id
                                        LawyerId = caseAssignmentLawyer != null && !String.IsNullOrEmpty(caseAssignmentLawyer.LawyerId) ? caseAssignmentLawyer.LawyerId : null,
                                        Remarks = "-",
                                        CreatedBy = "MOJ RPA",
                                        CreatedDate = DateTime.Now,
                                        HearingTime = TimeSpan.Zero
                                    };

                                    await _dbContext.OutcomeHearings.AddAsync(outcomehearing);
                                    await _dbContext.SaveChangesAsync();
                                    if (MojrollsAttachment != null)
                                    {
                                        await CreateMojRollsAttachmentForHearing(MojrollsAttachment, outcomehearing.Id);
                                    }

                                    CanAndCaseNumber.Add(item);
                                }

                            }
                        }
                        transaction.Commit();
                        return CanAndCaseNumber;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);

                    }
                }
            }
        }
        ////<History Author = 'Ijaz Ahmad' Date='2024-03-25' Version="1.0" Branch="master"> Send Hearing Notification</History>
        public async Task<bool> SendHearingNotification(Guid CaseId, string ReceiverId, NotificationParameter notificationParameter)
        {
            bool IsSaved;

            var notificationResponse = new Notification
            {
                NotificationId = Guid.NewGuid(),
                DueDate = DateTime.Now.AddDays(5),
                CreatedBy = "MOJ RPA",
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                ReceiverId = ReceiverId, // Assign To Lawyer Id
                ModuleId = (int)WorkflowModuleEnum.CaseManagement,
            };

            IsSaved = await _notificationRepository.SendNotification(notificationResponse, (int)NotificationEventEnum.HearingDataPushedFromRPA, "view", "case", CaseId.ToString(), notificationParameter);

            return IsSaved;

        }
        //<History Author = 'Ijaz Ahmad' Date='2024-03-25' Version="1.0" Branch="master"> Create MojRolls Attachment For Hearing</History>
        public async Task CreateMojRollsAttachmentForHearing(UploadedDocument hearingDocument, Guid hearingId)
        {
            try
            {

                var documentObj = new UploadedDocument
                {
                    Description = hearingDocument.Description,
                    CreatedDateTime = DateTime.Now,
                    CreatedBy = "MOJ RPA",
                    DocumentDate = hearingDocument.DocumentDate,
                    FileName = hearingDocument.FileName,
                    StoragePath = hearingDocument.StoragePath,
                    DocType = hearingDocument.DocType,
                    ReferenceGuid = hearingId,
                    IsActive = hearingDocument.IsActive,
                    CreatedAt = hearingDocument.StoragePath,
                    AttachmentTypeId = hearingDocument.AttachmentTypeId,
                    IsDeleted = hearingDocument.IsDeleted
                };
                await _dmsDbContext.UploadedDocuments.AddAsync(documentObj);
                await _dmsDbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task AssignHearingRollsToLawyer(AssignHearingRollToLawyerVM hearingRollToLawyerVM)
        {
            try
            {
                string hearingDate = hearingRollToLawyerVM.HearingDate != null ? Convert.ToDateTime(hearingRollToLawyerVM.HearingDate).ToString("yyyy/MM/dd") : null;
                string StoredProc = $"exec pGetHearingDateByHearingDateAndChamberNumberId @HearingDate ='{hearingDate}', @ChamberNumberid ='{hearingRollToLawyerVM.ChamberNumberid}' ";

                var result = await _dbContext.Hearings.FromSqlRaw(StoredProc).ToListAsync();
                if (result.Count > 0)
                {
                    await AssignHearingRollsToLawyer(result, hearingRollToLawyerVM.LawyerId);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task AssignHearingRollsToLawyer(List<Hearing> hearings, string lawyerid)
        {


            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in hearings)
                        {
                            item.LawyerId = lawyerid;
                            _dbContext.Hearings.Update(item);
                            await _dbContext.SaveChangesAsync();
                        }
                        transaction.Commit();

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



        #region Get Hearing Roll Detail

        //<History Author = 'Hassan Abbas' Date='2024-04-05' Version="1.0" Branch="master"> Get Hearing Roll Deail for Printing</History>

        public async Task<List<CmsPrintHearingRollDetailVM>> GetHearingRollDetailForPrintingAndOutcome(CmsHearingRollDetailSearchVM cmsHearingRollDetailSearch)
        {
            try
            {
                CmsHearingRollOutcomeDraftPayload outcomeDraftPayload = null;
                if (cmsHearingRollDetailSearch.HearingRollId > 0)
                {
                    outcomeDraftPayload = await _dbContext.CmsHearingRollOutcomeDraftPayloads.Where(x => x.HearingRollId == cmsHearingRollDetailSearch.HearingRollId).FirstOrDefaultAsync();
                }
                if (outcomeDraftPayload == null)
                {
                    var hearingDate = cmsHearingRollDetailSearch.HearingDate != null ? Convert.ToDateTime(cmsHearingRollDetailSearch.HearingDate).ToString("yyyy/MM/dd").ToString() : null;
                    string StoredProc = $"exec pCmsGetHearingRollDetailForPrinting @HearingDate ='{hearingDate}', @ChamberNumberid ='{cmsHearingRollDetailSearch.ChamberNumberId}' ";
                    return await _dbContext.CmsPrintHearingRollDetailVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                else
                {
                    return JsonConvert.DeserializeObject<List<CmsPrintHearingRollDetailVM>>(outcomeDraftPayload.Payload);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion

        #region Save Hearing Roll Outcomes
        public async Task SaveHearingRollOutcomesAsDraft(CmsHearingRollOutcomeDraftPayload payload)
        {
            try
            {
                var outcomeDraftPayload = await _dbContext.CmsHearingRollOutcomeDraftPayloads.Where(x => x.HearingRollId == payload.HearingRollId).FirstOrDefaultAsync();
                if (outcomeDraftPayload == null)
                {
                    await _dbContext.CmsHearingRollOutcomeDraftPayloads.AddAsync(payload);
                }
                else
                {
                    outcomeDraftPayload.Payload = payload.Payload;
                    outcomeDraftPayload.ModifiedBy = payload.CreatedBy;
                    outcomeDraftPayload.ModifiedDate = DateTime.Now;
                    _dbContext.CmsHearingRollOutcomeDraftPayloads.Update(outcomeDraftPayload);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task SaveHearingRollOutcomes(List<CmsPrintHearingRollDetailVM> hearings)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var caseHearing in hearings)
                        {
                            OutcomeHearing outcomeHearing = caseHearing.OutcomeHearing;
                            CmsRegisteredCase registeredCase = await _dbContext.CmsRegisteredCases.FindAsync(caseHearing.CaseId);
                            Hearing hearing = await _dbContext.Hearings.FindAsync(caseHearing.HearingId);
                            hearing.StatusId = (int)HearingStatusEnum.HearingAttended;
                            _dbContext.Hearings.Update(hearing);
                            outcomeHearing.CreatedDate = DateTime.Now;
                            await _dbContext.OutcomeHearings.AddAsync(outcomeHearing);
                            await _dbContext.SaveChangesAsync();
                            await SaveRegisteredCaseStatusHistory(registeredCase, (int)RegisteredCaseEventEnum.OutcomeAdded, null, _dbContext, outcomeHearing.Remarks, outcomeHearing.CreatedBy);

                            if (outcomeHearing.caseParties.Count > 0)
                            {
                                foreach (var outcomeParticipant in outcomeHearing.caseParties)
                                {
                                    outcomeParticipant.CreatedBy = outcomeHearing.CreatedBy;
                                    var partyResult = await _registeredCaseRepository.SaveCaseParty(outcomeParticipant, _dbContext);
                                    if (partyResult)
                                    {
                                        await _registeredCaseRepository.SaveCasePartyAttachment(outcomeParticipant, _dmsDbContext);
                                        await _registeredCaseRepository.SaveOutcomePartyHistory(outcomeHearing.Id, outcomeParticipant.Id, (int)CaseOutcomePartyActionEnum.Added, outcomeHearing.CreatedBy, _dbContext);
                                    }
                                }
                            }
                            if (outcomeHearing.DeletedParties.Count > 0)
                            {
                                foreach (var deletedParties in outcomeHearing.DeletedParties)
                                {
                                    deletedParties.DeletedBy = outcomeHearing.CreatedBy;
                                    var result = await _registeredCaseRepository.DeleteCaseParty(deletedParties, _dbContext);
                                    if (result)
                                    {
                                        await _registeredCaseRepository.SaveOutcomePartyHistory(outcomeHearing.Id, deletedParties.Id, (int)CaseOutcomePartyActionEnum.Deleted, outcomeHearing.CreatedBy, _dbContext);
                                    }
                                }
                            }
                        }
                        transaction.Commit();
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
    }
}
