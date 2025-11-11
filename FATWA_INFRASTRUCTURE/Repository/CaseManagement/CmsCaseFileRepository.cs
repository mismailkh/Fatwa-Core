using AutoMapper;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.CaseManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.DigitalSignature;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_INFRASTRUCTURE.Database;
using FATWA_INFRASTRUCTURE.Repository.Communications;
using FATWA_INFRASTRUCTURE.Repository.G2G;
using FATWA_INFRASTRUCTURE.Repository.PatternNumber;
using FATWA_INFRASTRUCTURE.Repository.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SelectPdf;
using System.Transactions;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;

namespace FATWA_INFRASTRUCTURE.Repository.CaseManagement
{
    //<!-- <History Author = 'Hassan Abbas' Date='2022-11-08' Version="1.0" Branch="master">Repo for Case File operations</History> -->
    public class CmsCaseFileRepository : ICmsCaseFile
    {
        #region Variables declaration

        private List<CmsCaseRequestVM> _CmsCaseRequestVMs;
        private List<CmsCaseFileDetailVM> _CmsCaseFileDetailVMs;
        private List<CmsDraftedDocumentVM> _CmsDraftedDocumentVMs;
        private List<CmsCaseFileVM> _RegisterdRequestVMs;
        private List<MojExecutionRequestVM> _MojExecutionRequestVMs;
        private List<CmsCaseAssigneesHistoryVM> _CmsCaseAssigneesHistoryVMs;
        private List<CmsCaseAssigneeVM> _CmsCaseAssigneeVMs;
        private List<RegisteredCaseFileVM> _RegisteredCaseFileVMs;
        private List<CmsRegisteredCaseVM> _CmsRegisteredCaseVMs;
        private List<CmsRegisteredCaseFileDetailVM> _CmsRegisteredCaseFileDetailVMs;
        private List<UserBasicDetailVM> _UserBasicDetailVMs;
        private List<CmsCaseFileStatusHistoryVM> _cmsCaseFileHistoryVMs;
        private List<MojRegistrationRequestVM> _MojRegistrationRequestVMs;
        private List<MojDocumentPortfolioRequestVM> _MojDocumentPortfolioRequestVMs;
        private List<CasePartyLink> _CaseParty;
        private List<CmsDraftedRequestListVM> _cmsDraftedRequestListVMs;
        private readonly DatabaseContext _dbContext;
        private readonly CMSCaseRequestRepository _caseRequestRepository;
        private readonly CmsRegisteredCaseRepository _registeredCaseRepository;
        private readonly TaskRepository _taskRepository;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ITempFileUpload _IFileUpload;
        private readonly G2GRepository _g2gRepository;
        private List<CasePartyLink> _casePartiesList { get; set; }
        private readonly CMSCOMSInboxOutboxPatternNumberRepository _cMSCOMSInboxOutboxPatternNumberRepository;
        private readonly CommunicationRepository _communicationRepo;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor
        public CmsCaseFileRepository(DatabaseContext databaseContext, IServiceScopeFactory serviceScopeFactory, ITempFileUpload iFileUpload, CMSCOMSInboxOutboxPatternNumberRepository CMSCOMSInboxOutboxPatternNumberRepository , CommunicationRepository communicationRepository, IMapper mapper)
        {
            _dbContext = databaseContext;
            _serviceScopeFactory = serviceScopeFactory;
            using var scope = _serviceScopeFactory.CreateScope();
            _caseRequestRepository = scope.ServiceProvider.GetRequiredService<CMSCaseRequestRepository>();
            _registeredCaseRepository = scope.ServiceProvider.GetRequiredService<CmsRegisteredCaseRepository>();
            _taskRepository = scope.ServiceProvider.GetRequiredService<TaskRepository>();
            _IFileUpload = iFileUpload;
            _cMSCOMSInboxOutboxPatternNumberRepository = CMSCOMSInboxOutboxPatternNumberRepository;
            _communicationRepo = communicationRepository;
            _g2gRepository = scope.ServiceProvider.GetRequiredService<G2GRepository>();
            _mapper = mapper;
        }

        #endregion

        #region Get All Registered Case File Details
        //<History Author = 'Danish' Date='2022-11-29' Version="1.0" Branch="master"> Get All Registered Case File Details</History>
        public async Task<List<RegisteredCaseFileVM>> GetRegisteredCaseFile(AdvanceSearchCmsCaseFileVM advanceSearchVM)
        {
            try
            {
                if (_RegisteredCaseFileVMs == null)
                {
                    string CreatedfromDate = advanceSearchVM.CreatedFrom != null ? Convert.ToDateTime(advanceSearchVM.CreatedFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string CreatedtoDate = advanceSearchVM.CreatedTo != null ? Convert.ToDateTime(advanceSearchVM.CreatedTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string StoreProc = $"exec pRegisterCaseFileList @fileNumber = '{advanceSearchVM.FileNumber}' , @statusId = '{advanceSearchVM.StatusId}'  , @createdFrom = '{CreatedfromDate}' " +
                        $", @createdTo = '{CreatedtoDate}' , @CANNumber = '{advanceSearchVM.CANNumber}' , @caseNumber = '{advanceSearchVM.CaseNumber}', @sectorTypeId = '{advanceSearchVM.SectorTypeId}'" +
                        $", @userId = '{advanceSearchVM.UserId}' ,@govEntityId = '{advanceSearchVM.GovEntityId}',@courtId = '{advanceSearchVM.CourtId}',@chamberId = '{advanceSearchVM.ChamberId}'" +
                        $",@chamberNumberId = '{advanceSearchVM.ChamberNumberId}',@isImpportant = '{advanceSearchVM.IsImpportant}',@isJudgment = '{advanceSearchVM.isFinalJudgment}',@LawyerId = N'{advanceSearchVM.LawyerId}'" +
                        $", @PlaintiffName = N'{advanceSearchVM.PlaintiffName}', @DefendantName = N'{advanceSearchVM.DefendantName}'" +
                        $",@PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}'";
                    _RegisteredCaseFileVMs = await _dbContext.RegisteredCaseFileVMs.FromSqlRaw(StoreProc).ToListAsync();
                }
                return _RegisteredCaseFileVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get All Registered Cases By File
        //<History Author = 'Danish' Date='2022-11-29' Version="1.0" Branch="master"> Get All Registered Cases By File</History>
        public async Task<List<CmsRegisteredCaseFileDetailVM>> GetAllRegisteredCasesByFileId(Guid fileId, bool? isFinal)
        {
            try
            {
                if (_CmsRegisteredCaseFileDetailVMs == null)
                {
                    string StoredProc = $"exec pCmsRegisteredCasesListByFileId @fileId ='{fileId}', @IsJudgment = '{isFinal}' ";

                    _CmsRegisteredCaseFileDetailVMs = await _dbContext.CmsRegisteredCaseFileDetailVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CmsRegisteredCaseFileDetailVMs;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get All Execution Cases By File
        //<History Author = 'Danish' Date='2022-11-29' Version="1.0" Branch="master"> Get All Execution Cases By File</History>
        public async Task<List<CmsRegisteredCaseVM>> GetExecutionCasesByFileId(Guid fileId)
        {
            try
            {
                if (_CmsRegisteredCaseVMs == null)
                {
                    string StoredProc = $"exec pCmsExecutionListByFileId @fileId ='{fileId}' ";

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

        #region Get All Case File Details
        public async Task<List<CmsCaseFileVM>> GetAllCmsCaseFile(AdvanceSearchCmsCaseFileVM advanceSearchVM)
        {
            try
            {
                if (_RegisterdRequestVMs == null)
                {
                    string CreatedfromDate = advanceSearchVM.CreatedFrom != null ? Convert.ToDateTime(advanceSearchVM.CreatedFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string CreatedtoDate = advanceSearchVM.CreatedTo != null ? Convert.ToDateTime(advanceSearchVM.CreatedTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string ModifiedfromDate = advanceSearchVM.ModifiedFrom != null ? Convert.ToDateTime(advanceSearchVM.ModifiedFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string ModifiedtoDate = advanceSearchVM.ModifiedTo != null ? Convert.ToDateTime(advanceSearchVM.ModifiedTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;

                    string StoreProc = $"exec pCmsCaseFileList @fileNumber = '{advanceSearchVM.FileNumber}' , @statusId = '{advanceSearchVM.StatusId}'  , @createdFrom = '{CreatedfromDate}' , @createdTo = '{CreatedtoDate}' , @modifiedFrom = '{ModifiedfromDate}' , @modifiedTo = '{ModifiedtoDate}', @sectorTypeId = '{advanceSearchVM.SectorTypeId}', @userId = '{advanceSearchVM.UserId}' , @govEntityId= '{advanceSearchVM.GovEntityId}',@PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}'";
                    _RegisterdRequestVMs = await _dbContext.RegisterdRequestVMs.FromSqlRaw(StoreProc).ToListAsync();
                }
                return _RegisterdRequestVMs;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion
        #region Get All Case File By Sector, User
        public async Task<List<CmsCaseFileDmsVM>> GetAllCaseFilesBySector(int sectorTypeId, string userId)
        {
            try
            {
                List<CmsCaseFileDmsVM> _CaseFiles;
                string StoredProc = $"exec pCmsCaseFileListForDms @sectorTypeId ='{sectorTypeId}', @userId = '{userId}'";
                _CaseFiles = await _dbContext.CmsCaseFileDmsVMs.FromSqlRaw(StoredProc).ToListAsync();
                return _CaseFiles;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get Case detail by Id

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master">Get Case File detail by Id </History>
        public async Task<CmsCaseFileDetailVM> GetCaseFileDetailByIdVM(Guid fileId, string userName)
        {
            try
            {
                string StoredProc = $"exec pCmsCaseFileDetailById @fileId ='{fileId}', @userName='{userName}' ";
                var caseFiles = await _dbContext.CmsCaseFileDetailVM.FromSqlRaw(StoredProc).ToListAsync();
                var CmsCaseFileDetail = caseFiles.FirstOrDefault();
                if(CmsCaseFileDetail!= null)
                {
                    CmsCaseFileDetail.IsCaseRegistered = _dbContext.cmsRegisteredCases.Where(x => x.FileId == fileId).Any();
                }
                return CmsCaseFileDetail;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion  

        #region Get Case File Id

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master">Get Case File By Id </History>
        public async Task<CaseFile> GetCaseFileById(Guid fileId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (_dbContext)
            {
                try
                {
                    return await _dbContext.CaseFiles.Where(f => f.FileId == fileId).FirstOrDefaultAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        #endregion

        #region Get CaseRequest By FileId

        public async Task<CaseRequest> GetCaseRequestByFileId(Guid fileId)
        {
            try
            {
                var requestId = await _dbContext.CaseFiles.Where(f => f.FileId == fileId).Select(x => x.RequestId).FirstOrDefaultAsync();
                return await _dbContext.CaseRequests.Where(x => x.RequestId == requestId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion  

        #region Get Case File Assignment History

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Get Case File Assigment History </History>

        public async Task<List<CmsCaseAssigneesHistoryVM>> GetCaseAssigmentHistory(Guid referenceId)
        {
            try
            {
                if (_CmsCaseAssigneesHistoryVMs == null)
                {
                    string StoredProc = $"exec pCmsCaseAssignmentHistory @referenceId ='{referenceId}' ";
                    _CmsCaseAssigneesHistoryVMs = await _dbContext.CmsCaseAssigneesHistoryVM.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CmsCaseAssigneesHistoryVMs;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Case File Assignment 


        public async Task<List<CaseAssignment>> GetCaseAssigment(Guid referenceId)
        {
            try
            {
                var res = await _dbContext.CaseFileAssignment.Where(x => x.ReferenceId == referenceId).ToListAsync();
                if (res.Count != 0)
                {
                    return res;
                }
                else
                {
                    return res = new List<CaseAssignment>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Case Request Lawyer

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Get Case File Current Assignee List </History>
        public async Task<List<CmsCaseAssigneeVM>> GetCaseAssigeeList(Guid referenceId)
        {
            try
            {
                if (_CmsCaseAssigneeVMs == null)
                {
                    string StoredProc = $"exec pCmsCaseAssigneeList @referenceId ='{referenceId}' ";
                    _CmsCaseAssigneeVMs = await _dbContext.CmsCaseAssigneeVM.FromSqlRaw(StoredProc).ToListAsync();

                }
                return _CmsCaseAssigneeVMs;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get case File status history by Id

        //<History Author = 'Muhammad Zaeem' Date='2022-10-12' Version="1.0" Branch="master">Get  Case Requests detail by Id  </History>
        public async Task<List<CmsCaseFileStatusHistoryVM>> GetCMSCaseFileStatusHistory(Guid fileId)
        {
            try
            {
                if (_cmsCaseFileHistoryVMs == null)
                {
                    string StoredProc = $"exec pCaseFileStatusHistory @fileId = N'{fileId}'";
                    _cmsCaseFileHistoryVMs = await _dbContext.CmsCaseFileHistoryVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _cmsCaseFileHistoryVMs;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Moj Registration Requests

        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Get Moj Registration Requests</History>
        public async Task<List<MojRegistrationRequestVM>> GetMojRegistrationRequests(int? sectorTypeId, bool? IsRegistered, int? pageNumber, int? pageSize)
        {
            try
            {
                if (_MojRegistrationRequestVMs == null)
                {
                    string StoredProc = $"exec pMojRegistrationRequests @sectorTypeId='{sectorTypeId}',@IsRegistered='{IsRegistered}', @PageNumber='{pageNumber}', @PageSize='{pageSize}'";
                    _MojRegistrationRequestVMs = await _dbContext.MojRegistrationRequestVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _MojRegistrationRequestVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get Moj Registration Requests
        //<History Author = 'Hassan Abbas' Date='2023-03-23' Version="1.0" Branch="master"> Get Moj Document Portfolio Requests</History>
        public async Task<List<MojDocumentPortfolioRequestVM>> GetMojDocumentPortfolioRequests(int? sectorTypeId, int pageNumber, int pageSize)
        {
            try
            {
                if (_MojDocumentPortfolioRequestVMs == null)
                {
                    string StoredProc = $"exec pMojDocumentPortfolioRequests @sectorTypeId='{sectorTypeId}', @PageNumber = '{pageNumber}', @PageSize = '{pageSize}'";
                    _MojDocumentPortfolioRequestVMs = await _dbContext.MojDocumentPortfolioRequestVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _MojDocumentPortfolioRequestVMs;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get Moj Registration Request By Id

        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Get Moj Registration Requests</History>
        public async Task<MojRegistrationRequest> GetMojRegistrationRequestById(Guid id)
        {
            try
            {
                return await _dbContext.MojRegistrationRequests.FindAsync(id);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get CMS_CASE_ASSIGNMENT By CaseId

        //<History Author = 'Zain Ul Islam' Date='2023-01-03' Version="1.0" Branch="master"> Get Primary Case Assignment By CaseId</History>
        public async Task<CaseAssignment> GetPrimaryCaseAssignmentByCaseId(Guid CaseId)
        {
            CaseAssignment caseAssignment = null;
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                using (_dbContext)
                {
                    caseAssignment = await _dbContext.CaseFileAssignment.FirstOrDefaultAsync(x => x.ReferenceId == CaseId && x.IsPrimary == true);
                    if (caseAssignment != null)
                        caseAssignment.NotificationParameter.CaseNumber = _dbContext.CmsRegisteredCases.Where(x => x.CaseId == CaseId).FirstOrDefault().CaseNumber;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return caseAssignment;
        }

        #endregion

        #region Create Case Party

        //<History Author = 'Hassan Abbas' Date='2022-10-02' Version="1.0" Branch="master"> Create Case Party with attachment</History>
        public async Task<bool> CreateCaseParty(CasePartyLinkVM party)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    bool isSaved = false;
                    try
                    {
                        await SaveCaseParty(party, _dbContext);
                        //await SaveCasePartyAttachment(party, _dbContext);
                        transaction.Commit();
                        isSaved = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                    return isSaved;
                }
            }
        }
        //public async Task<bool> SaveTempAttachmentToUploadedDocument(FileUploadVM item)
        //{
        //    using (_dbContext)
        //    {
        //        using (var transaction = _dbContext.Database.BeginTransaction())
        //        {
        //            bool isSaved = true;
        //            try
        //            {
        //                isSaved = await DeleteFileFromPath(item);
        //                isSaved = await SaveUploadedDocuments(item);
        //                if (isSaved)
        //                    transaction.Commit();
        //            }
        //            catch (Exception)
        //            {
        //                transaction.Rollback();
        //                isSaved = false;
        //                throw;
        //            }
        //            return isSaved;
        //        }
        //    }
        //}

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Save Case Party</History>
        public async Task SaveCaseParty(CasePartyLinkVM party, DatabaseContext dbContext)
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Create Moj Execution Request

        //<History Author = 'Hassan Abbas' Date='2023-03-30' Version="1.0" Branch="master"> Create MOJ Execution Request</History>
        public async Task CreateMojExecutionRequest(MojExecutionRequest request)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (request.SelectedCases.Any())
                        {
                            foreach (var exeCase in request.SelectedCases)
                            {
                                MojExecutionRequest executionRequest = new MojExecutionRequest
                                {
                                    CaseId = exeCase.CaseId,
                                    CreatedBy = request.CreatedBy,
                                    CreatedDate = DateTime.Now
                                };
                                await _dbContext.MojExecutionRequest.AddAsync(executionRequest);
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            await _dbContext.MojExecutionRequest.AddAsync(request);
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

        #region Delete case Party

        //<History Author = 'Hassan Abbas' Date='2022-10-02' Version="1.0" Branch="master"> Delete Case Party </History>
        public async Task DeleteCaseParty(CasePartyLinkVM party)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var caseParty = await _dbContext.CasePartyLink.FindAsync(party.Id);
                        caseParty.DeletedBy = party.DeletedBy;
                        caseParty.DeletedDate = DateTime.Now;
                        caseParty.IsDeleted = true;
                        _dbContext.CasePartyLink.Update(caseParty);
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

        #region Get CaseParty
        public async Task<List<CasePartyLink>> GetCMSCasePartyDetailByGuid(Guid Id)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (_dbContext)
            {
                try
                {
                    if (_CaseParty == null)
                    {
                        _CaseParty = await _dbContext.CasePartyLink.Where(x => x.ReferenceGuid == Id).ToListAsync();
                    }
                    return _CaseParty;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        #endregion

        #region Create Moj Registration Request

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Create Moj Registration Request</History>
        public async Task<CmsCaseFileStatusHistory> CreateMojRegistrationRequest(List<MojRegistrationRequest> registrationRequestList)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var file = await _dbContext.CaseFiles.FindAsync(registrationRequestList[0].FileId);
                        await SaveMojRegistrationRequest(registrationRequestList, _dbContext);
                        await UpdateCaseFileStatus(file, (int)CaseFileStatusEnum.PendingForRegistrationAtMoj, _dbContext);
                        var historyobj = await SaveCaseFileStatusHistory(registrationRequestList[0].CreatedBy, file, (int)CaseFileEventEnum.SentToMojTeam, _dbContext);
                        transaction.Commit();
                        return historyobj;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return null;
                        throw;
                    }
                }
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Save Moj Registration Request</History>
        public async Task SaveMojRegistrationRequest(List<MojRegistrationRequest> registrationRequestList, DatabaseContext dbContext)
        {
            try
            {
                foreach (var registrationRequest in registrationRequestList)
                {
                    await dbContext.MojRegistrationRequests.AddAsync(registrationRequest);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Update Case File status 

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Update Case File Status</History>
        public async Task UpdateCaseFileStatus(CaseFile file, int StatusId, DatabaseContext dbContext)
        {
            try
            {
                file.StatusId = StatusId;
                dbContext.CaseFiles.Update(file);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Save/Update Case File Sector Assignment
        //<History Author = 'Hassan Abbas' Date='2022-12-24' Version="1.0" Branch="master"> Save Case File Sector Assignment</History>
        public async Task UpdateCaseFileSectorAssignment(Guid fileId, int sectorTypeId, string username, DatabaseContext dbContext)
        {
            try
            {
                CmsCaseFileSectorAssignment assignment = await dbContext.CmsCaseFileSectorAssignment.Where(f => f.FileId == fileId).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
                if (assignment != null)
                {
                    assignment.FileId = fileId;
                    assignment.SectorTypeId = sectorTypeId;
                    assignment.CreatedBy = username;
                    assignment.CreatedDate = DateTime.Now;
                    dbContext.CmsCaseFileSectorAssignment.Update(assignment);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task SaveCaseFileSectorAssignment(Guid fileId, int sectorTypeId, string username, DatabaseContext dbContext)
        {
            try
            {
                CmsCaseFileSectorAssignment assignment = await dbContext.CmsCaseFileSectorAssignment.Where(f => f.FileId == fileId && f.SectorTypeId == sectorTypeId).FirstOrDefaultAsync();
                if (assignment == null)
                {
                    CmsCaseFileSectorAssignment assignmentobj = new CmsCaseFileSectorAssignment();
                    assignmentobj.FileId = fileId;
                    assignmentobj.SectorTypeId = sectorTypeId;
                    assignmentobj.CreatedBy = username;
                    assignmentobj.CreatedDate = DateTime.Now;
                    await dbContext.CmsCaseFileSectorAssignment.AddAsync(assignmentobj);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task UpdateReadOnlyCaseFileSectorAssignment(Guid fileId, int sectorTypeId, DatabaseContext dbContext, string username)
        {
            try
            {
                int courtTypeId = 0;
                CmsCaseFileSectorAssignment assignment = await dbContext.CmsCaseFileSectorAssignment.Where(f => f.FileId == fileId && f.SectorTypeId == sectorTypeId).FirstOrDefaultAsync();
                if (assignment != null)
                {
                    if (sectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeRegionalCases || sectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialRegionalCases)
                    {
                        courtTypeId = (int)CourtTypeEnum.Regional;
                    }
                    else if (sectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeAppealCases || sectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialAppealCases)
                    {
                        courtTypeId = (int)CourtTypeEnum.Appeal;
                    }
                    else if (sectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeSupremeCases || sectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases)
                    {
                        courtTypeId = (int)CourtTypeEnum.Supreme;
                    }
                    string StoredProc = $"exec pCmsRegisteredCasesByFileIdandCourtId @FileId='{fileId}',@CourtTypeId='{courtTypeId}'";
                    var result = await dbContext.CmsRegisteredCasesAssigneeVM.FromSqlRaw(StoredProc).ToListAsync();
                    if (result != null && result.Count > 0)
                    {
                        if (result.All(x => x.IsFinalJudgement == true))
                        {
                            assignment.IsReadOnly = true;
                            dbContext.CmsCaseFileSectorAssignment.Update(assignment);
                            await dbContext.SaveChangesAsync();
                        }
                        foreach (var item in result)
                        {
                            if (item.IsFinalJudgement == true)
                            {
                                var caseRegisterd = await dbContext.CmsRegisteredCases.Where(c => c.CaseId == item.CaseId).FirstOrDefaultAsync();
                                if (caseRegisterd != null)
                                {
                                    caseRegisterd.StatusId = (int)RegisteredCaseStatusEnum.Closed;
                                    dbContext.CmsRegisteredCases.Update(caseRegisterd);
                                    await dbContext.SaveChangesAsync();
                                    await _registeredCaseRepository.SaveRegisteredCaseStatusHistory(caseRegisterd, (int)RegisteredCaseEventEnum.Closed, dbContext, "", username);
                                }
                            }
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

        #region Remove Case File Sector Assignment
        //<History Author = 'Hassan Abbas' Date='2022-12-24' Version="1.0" Branch="master"> Save Case File Sector Assignment</History>
        public async Task RemoveCaseFileSectorAssignment(Guid fileId, int sectorTypeId, string username, DatabaseContext dbContext)
        {
            try
            {
                CmsCaseFileSectorAssignment assignmentobj = await dbContext.CmsCaseFileSectorAssignment.Where(f => f.FileId == fileId && f.SectorTypeId == sectorTypeId).FirstOrDefaultAsync();
                assignmentobj.DeletedBy = username;
                assignmentobj.DeletedDate = DateTime.Now;
                assignmentobj.IsDeleted = true;
                dbContext.CmsCaseFileSectorAssignment.Update(assignmentobj);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region save Case File status history
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Save Case File Status History</History>
        public async Task<CmsCaseFileStatusHistory> SaveCaseFileStatusHistory(string userName, CaseFile caseFile, int EventId, DatabaseContext dbContext, string remarks = null)
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
                historyobj.Remarks = remarks;
                if (caseFile.StatusId == (int)CaseFileStatusEnum.AssignedToRegionalSector)
                {
                    historyobj.SectorTo = (int)caseFile.SectorTo;
                }
                await dbContext.CmsCaseFileStatusHistory.AddAsync(historyobj);
                await dbContext.SaveChangesAsync();
                return historyobj;

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Link Case Requests with Primary Request

        //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master"> Link Selected Case Requests with Primary Request </History>
        //public async Task LinkCaseFiles(LinkCaseFilesVM linkCaseFile)
        //{
        //    try
        //    {
        //        using (_dbContext)
        //        {
        //            using (var transaction = _dbContext.Database.BeginTransaction())
        //            {
        //                try
        //                {
        //                    CaseFile primaryFile = await UpdatePrimaryCaseFile(linkCaseFile.PrimaryFileId, _dbContext);
        //                    foreach (var fileId in linkCaseFile.LinkedFileIds)
        //                    {
        //                        CaseFile caseFile = await UpdateLinkedCaseFile(fileId, _dbContext);
        //                        await SaveCaseFileStatusHistory(linkCaseFile.CreatedBy, caseFile, (int)CaseFileEventEnum.Linked, _dbContext);
        //                        await LinkCaseFileWithPrimaryFile(linkCaseFile, caseFile.FileId, _dbContext);
        //                        //Copy Parties and Attachments from Secondary to Primary File
        //                        await _caseRequestRepository.CopyCasePartiesFromSourceToDestination(fileId, primaryFile.FileId, linkCaseFile.CreatedBy, _dbContext);
        //                        await _caseRequestRepository.CopyCaseAttachmentsFromSourceToDestination(fileId, primaryFile.FileId, linkCaseFile.CreatedBy, _dbContext);
        //                        //Link Case Requests of Secondary Files with Case Request of Primary File
        //                        await LinkCaseRequestsWithRequestOfPrimaryFile(linkCaseFile, primaryFile, caseFile, _dbContext);
        //                        await UpdateCasesWithPrimaryFile(linkCaseFile, primaryFile.FileId, caseFile.FileId, _dbContext);
        //                    }
        //                    transaction.Commit();
        //                }
        //                catch (Exception)
        //                {
        //                    transaction.Rollback();
        //                    throw;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #region ijaz changes of Link Case Files
        //<History Author = 'Ijaz Ahmad' Date='2023-03-03' Version="1.0" Branch="master"> updated  Link Selected Case Requests with Primary Request for DMS Attachment </History>
        //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master"> Link Selected Case Requests with Primary Request </History>
        public async Task<LinkCaseFilesVM> LinkCaseFiles(LinkCaseFilesVM linkCaseFile)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            CaseFile primaryFile = await UpdatePrimaryCaseFile(linkCaseFile.PrimaryFileId, _dbContext);
                            foreach (var fileId in linkCaseFile.LinkedFileIds)
                            {
                                CaseFile caseFile = await UpdateLinkedCaseFile(fileId, _dbContext);
                                await SaveCaseFileStatusHistory(linkCaseFile.CreatedBy, caseFile, (int)CaseFileEventEnum.Linked, _dbContext);
                                await LinkCaseFileWithPrimaryFile(linkCaseFile, caseFile.FileId, _dbContext);

                                //Copy Parties and Attachments from Secondary to Primary File
                                linkCaseFile.CopyAttachmentVMs.Add(new CopyAttachmentVM()
                                {
                                    SourceId = fileId,
                                    DestinationId = primaryFile.FileId,
                                    CreatedBy = linkCaseFile.CreatedBy
                                });
                                var copyAttachments = await _caseRequestRepository.CopyCasePartiesFromSourceToDestination(fileId, primaryFile.FileId, linkCaseFile.CreatedBy, _dbContext);
                                if (copyAttachments.Any())
                                {
                                    linkCaseFile.CopyAttachmentVMs.AddRange(copyAttachments);
                                }
                                //await _caseRequestRepository.CopyCasePartiesFromSourceToDestination(fileId, primaryFile.FileId, linkCaseFile.CreatedBy, _dbContext);
                                //await _caseRequestRepository.CopyCaseAttachmentsFromSourceToDestination(fileId, primaryFile.FileId, linkCaseFile.CreatedBy, _dbContext);
                                //Link Case Requests of Secondary Files with Case Request of Primary File
                                await LinkCaseRequestsWithRequestOfPrimaryFile(linkCaseFile, primaryFile, caseFile, _dbContext);
                                await UpdateCasesWithPrimaryFile(linkCaseFile, primaryFile.FileId, caseFile.FileId, _dbContext);
                            }
                            transaction.Commit();
                            return linkCaseFile;
                        }
                        catch (Exception)
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
        //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master"> Create Link between Primary and Linked Files </History>
        private async Task LinkCaseFileWithPrimaryFile(LinkCaseFilesVM linkCaseFile, Guid linkedFileId, DatabaseContext dbContext)
        {
            try
            {
                CmsCaseFileLinkedFile caseFileLinkedFile = new CmsCaseFileLinkedFile
                {
                    PrimaryFileId = linkCaseFile.PrimaryFileId,
                    LinkedFileId = linkedFileId,
                    CreatedBy = linkCaseFile.CreatedBy,
                    CreatedDate = DateTime.Now
                };
                await dbContext.CmsCaseFileLinkedFiles.AddAsync(caseFileLinkedFile);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master"> Link Case Requests of Secondary Files With Case Request Of Primary File </History>
        private async Task LinkCaseRequestsWithRequestOfPrimaryFile(LinkCaseFilesVM linkCaseFile, CaseFile primaryCaseFile, CaseFile secondaryFile, DatabaseContext dbContext)
        {
            try
            {
                //Primary Case Request of Secondary File
                CaseRequest primaryRequestSecondaryFile = await _caseRequestRepository.UpdateLinkedCaseRequest(secondaryFile.RequestId, dbContext);
                await _caseRequestRepository.LinkCaseRequestWithPrimaryRequest(new LinkCaseRequestsVM { PrimaryRequestId = primaryCaseFile.RequestId, CreatedBy = linkCaseFile.CreatedBy }, primaryRequestSecondaryFile.RequestId, dbContext);
                await _caseRequestRepository.SaveCaseRequestStatusHistory(linkCaseFile.CreatedBy, primaryRequestSecondaryFile, (int)CaseRequestEventEnum.Linked, dbContext);

                //Link selected Secondary Requests
                List<CmsCaseRequestVM> linkedRequestsOfSecondaryRequest = await GetLinkedRequestsByPrimaryRequestId(primaryRequestSecondaryFile.RequestId);
                foreach (var request in linkedRequestsOfSecondaryRequest)
                {
                    CaseRequest caseRequest = await dbContext.CaseRequests.FindAsync(request.RequestId);
                    await _caseRequestRepository.LinkCaseRequestWithPrimaryRequest(new LinkCaseRequestsVM { PrimaryRequestId = primaryCaseFile.RequestId, CreatedBy = linkCaseFile.CreatedBy }, request.RequestId, dbContext);
                    await _caseRequestRepository.SaveCaseRequestStatusHistory(linkCaseFile.CreatedBy, caseRequest, (int)CaseRequestEventEnum.Linked, dbContext);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<CmsCaseRequestVM>> GetLinkedRequestsByPrimaryRequestId(Guid RequestId)
        {
            try
            {
                if (_CmsCaseRequestVMs == null)
                {
                    string StoredProc = $"exec pLinkedRequestsByPrimaryRequestId @requestId = N'{RequestId}'";
                    _CmsCaseRequestVMs = await _dbContext.CmsCaseRequestVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _CmsCaseRequestVMs;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master"> Update Linked Case Request</History>
        private async Task<CaseFile> UpdateLinkedCaseFile(Guid linkedFileId, DatabaseContext dbContext)
        {
            try
            {
                CaseFile caseFile = await dbContext.CaseFiles.FindAsync(linkedFileId);
                caseFile.IsLinked = true;
                dbContext.CaseFiles.Update(caseFile);
                await dbContext.SaveChangesAsync();
                return caseFile;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master"> Update Primary Case File</History>
        private async Task<CaseFile> UpdatePrimaryCaseFile(Guid primaryFileId, DatabaseContext dbContext)
        {
            try
            {
                CaseFile caseFile = await dbContext.CaseFiles.FindAsync(primaryFileId);
                caseFile.IsPrimary = true;
                dbContext.CaseFiles.Update(caseFile);
                await dbContext.SaveChangesAsync();
                return caseFile;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master"> Update Primary Case File</History>
        private async Task UpdateCasesWithPrimaryFile(LinkCaseFilesVM linkCaseFile, Guid primaryFileId, Guid linkedFileId, DatabaseContext dbContext)
        {
            try
            {
                List<CmsRegisteredCase> cases = await dbContext.CmsRegisteredCases.Where(c => c.FileId == linkedFileId).ToListAsync();
                foreach (var regCase in cases)
                {
                    regCase.FileId = primaryFileId;
                    dbContext.CmsRegisteredCases.Update(regCase);
                    await dbContext.SaveChangesAsync();
                    regCase.CreatedBy = linkCaseFile.CreatedBy;
                    await _registeredCaseRepository.SaveRegisteredCaseStatusHistory(regCase, (int)RegisteredCaseEventEnum.Linked, dbContext);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Update Case File Transfer Status

        public async Task<bool> UpdateCaseFileTransferStatus(CmsApprovalTracking approvalTracking, int taskStatusId, int transferCaseType)
        {
            bool isSaved = false;

            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                CaseFile caseFile = await _DbContext.CaseFiles.FirstOrDefaultAsync(x => x.FileId == approvalTracking.ReferenceId);
                if (caseFile is not null)
                {
                    caseFile.TransferStatusId = taskStatusId;
                    caseFile.ModifiedBy = approvalTracking.CreatedBy;
                    caseFile.ModifiedDate = DateTime.Now;
                    _DbContext.CaseFiles.Update(caseFile);
                    await _DbContext.SaveChangesAsync();
                    await _caseRequestRepository.SaveTransferHistory(approvalTracking, taskStatusId, transferCaseType, _DbContext);
                    isSaved = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return isSaved;
        }
        public async Task<bool> UpdateCaseFileTransferStatus(CmsApprovalTracking approvalTracking, int transferCaseType)
        {
            bool isSaved = false;
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    using (var serviceScope = _serviceScopeFactory.CreateScope())
                    {
                        var dbContext = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
                        CmsRegisteredCase caseRequest = await dbContext.CmsRegisteredCases.FirstOrDefaultAsync(x => x.CaseId == approvalTracking.ReferenceId);
                        CaseFile caseFile = await dbContext.CaseFiles.FirstOrDefaultAsync(x => x.FileId == caseRequest.FileId);
                        if (caseFile != null)
                        {
                            caseFile.TransferStatusId = (int)ApprovalStatusEnum.Approved;
                            caseFile.StatusId = (int)CaseFileStatusEnum.AssignedToPartialUrgentSector;
                            caseFile.ModifiedBy = approvalTracking.CreatedBy;
                            caseFile.ModifiedDate = DateTime.Now;
                            dbContext.CaseFiles.Update(caseFile);
                            await dbContext.SaveChangesAsync();
                            approvalTracking.ReferenceId = caseFile.FileId;
                            await SaveCaseFileStatusHistory(approvalTracking.CreatedBy, caseFile, (int)CaseFileEventEnum.AssignToSector, dbContext);
                            await _caseRequestRepository.SaveTransferHistory(approvalTracking, (int)CaseFileStatusEnum.AssignedToRegionalSector, transferCaseType, dbContext);
                            await UpdateCaseFileSectorAssignment(caseFile.FileId, approvalTracking.SectorTo, approvalTracking.CreatedBy, dbContext);
                            isSaved = true;
                            scope.Complete();
                        }
                        approvalTracking.NotificationParameter.ReferenceNumber = caseFile.FileNumber;
                        approvalTracking.NotificationParameter.SectorFrom = dbContext.OperatingSectorType.Where(x => x.Id == approvalTracking.SectorFrom).Select(x => x.Name_En + "/" + x.Name_Ar).FirstOrDefault();
                        approvalTracking.NotificationParameter.SectorTo = dbContext.OperatingSectorType.Where(x => x.Id == approvalTracking.SectorTo).Select(x => x.Name_En + "/" + x.Name_Ar).FirstOrDefault();
                    }
                }
                catch (Exception)
                {
                    scope.Dispose();
                    throw;
                }
            }
            return isSaved;
        }

        #endregion

        #region Copy Case File

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Copy Case Request</History>
        public async Task<CaseFile> CopyCaseFile(Guid oldCaseFileId, string userName, DatabaseContext dbContext)
        {
            try
            {
                var oldFile = await dbContext.CaseFiles.FindAsync(oldCaseFileId);
                CaseRequest casereq = await dbContext.CaseRequests.FindAsync(oldFile.RequestId);
                GovernmentEntity entity = await dbContext.GovernmentEntity.FindAsync(casereq.GovtEntityId);
                CaseFile casefile = new CaseFile();
                var resultCaseFileNumber = await _cMSCOMSInboxOutboxPatternNumberRepository.GenerateNumberPattern(0, (int)CmsComsNumPatternTypeEnum.CaseFileNumber);
                casefile.FileNumber = resultCaseFileNumber.GenerateRequestNumber;
                casefile.CaseFileNumberFormat = resultCaseFileNumber.FormatRequestNumber;
                casefile.PatternSequenceResult = resultCaseFileNumber.PatternSequenceResult;
                casefile.FileName = casefile.FileNumber + "_" + entity?.Name_En + "_" + DateOnly.FromDateTime(DateTime.Now).ToString("dd/MM/yyyy");
                casefile.RequestId = oldFile.RequestId;
                casefile.CreatedBy = userName;
                casefile.CreatedDate = DateTime.Now;
                casefile.StatusId = (int)CaseFileStatusEnum.InProgress;
                casefile.IsAssignedBack = false;
                await dbContext.CaseFiles.AddAsync(casefile);
                await dbContext.SaveChangesAsync();
                await SaveCaseFileStatusHistory(userName, casefile, (int)CaseFileEventEnum.ReceivedCopy, dbContext);
                return casefile;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Get Linked Files By Primary File
        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"> Get Linked Case Files by Primary File</History>
        public async Task<List<CmsCaseFileVM>> GetLinkedFilesByPrimaryFileId(Guid primaryFileId)
        {
            try
            {
                if (_RegisterdRequestVMs == null)
                {
                    string StoredProc = $"exec pLinkedFilesByPrimaryFileId @fileId = N'{primaryFileId}'";
                    _RegisterdRequestVMs = await _dbContext.RegisterdRequestVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _RegisterdRequestVMs;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        // For G2G to Fatwa Communication //
        public async Task<CaseFile> CaseFileDetailWithPartiesAndAttachments(Guid fileId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            var caseFile = await GetCaseFileById(fileId);
            var caseParties = await GetCMSCasePartyDetailByGuid(caseFile.FileId);
            foreach (var caseParty in caseParties)
            {
                caseFile.PartyLinks.Add(caseParty);
            }
            return caseFile;
        }

        #region Get case File status history by Id

        //<History Author = 'Muhammad Zaeem' Date='2022-10-12' Version="1.0" Branch="master">Get  Case Requests detail by Id  </History>
        public async Task<CmsCaseFileStatusHistoryVM> GetCaseFileHistoryDetailByHistoryId(Guid historyId)
        {
            try
            {
                string storedProc = $"exec pCaseFileStatusHistory @HistoryId = N'{historyId}'";
                var result = await _dbContext.CmsCaseFileHistoryVMs.FromSqlRaw(storedProc).ToListAsync();
                if (result is not null)
                {
                    return result.FirstOrDefault();
                }
                return null;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        #endregion


        #region Get Linked Files By Primary File
        //<History Author = 'Hassan Abbas' Date='2023-03-30' Version="1.0" Branch="master"> Get Moj Execution Requests</History>
        public async Task<List<MojExecutionRequestVM>> GetMojExecutionRequests(string username, int? pageNumber, int? pageSize)
        {
            try
            {
                if (_MojExecutionRequestVMs == null)
                {
                    string StoredProc = $"exec pListMojExecutionRequests @username='{username}', @PageNumber='{pageNumber}', @PageSize='{pageSize}'";
                    _MojExecutionRequestVMs = await _dbContext.MojExecutionRequestVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _MojExecutionRequestVMs;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Assign Case File Back to Hos
        //<History Author = 'Ijaz Ahmad' Date='2023-04-17' Version="1.0" Branch="master">Assign to HOS Case File  </History>
        public async Task AssignCaseFileBackToHos(CmsAssignCaseFileBackToHos assignBackToHos)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        await _dbContext.CmsAssignCaseFileBackToHos.AddAsync(assignBackToHos);
                        await _dbContext.SaveChangesAsync();
                        await UpdateCaseFileAssignedBackToHos(assignBackToHos.ReferenceId, true, assignBackToHos.CreatedBy);
                        var file = await _dbContext.CaseFiles.FindAsync(assignBackToHos.ReferenceId);
                        if (file is not null)
                        {
                            await UpdateCaseFileStatus(file, (int)CaseFileStatusEnum.RejectedByLawyer, _dbContext);
                            await SaveCaseFileStatusHistory(assignBackToHos.CreatedBy, file, (int)CaseFileEventEnum.AssignedBackToHos, _dbContext);
                            var taskstatus = await _taskRepository.DeleteTask(assignBackToHos.TaskId);
                            var CasefileAssigment = await _dbContext.CaseFileAssignment.Where(x => x.ReferenceId == file.FileId && x.LawyerId == assignBackToHos.TaskUserId).FirstOrDefaultAsync();
                            _dbContext.Entry(CasefileAssigment).State = EntityState.Deleted;
                            await _dbContext.SaveChangesAsync();
                            await SaveCaseFileStatusHistory(assignBackToHos.CreatedBy, file, (int)CaseFileEventEnum.AssignedBackToHos, _dbContext, assignBackToHos.Remarks);
                        }
                        assignBackToHos.NotificationParameter.Entity = new CaseFile().GetType().Name;
                        assignBackToHos.NotificationParameter.FileNumber = file.FileNumber;
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

        #region Update Case File IsAssignedBack to Hos Status

        public async Task<bool> UpdateCaseFileAssignedBackToHos(Guid referenceId, bool IsAssignedBack, string createdBy)
        {
            bool isSaved = false;
            try
            {

                CaseFile? caseFile = await _dbContext.CaseFiles.Where(x => x.FileId == referenceId).FirstOrDefaultAsync();
                if (caseFile is not null)
                {
                    caseFile.IsAssignedBack = IsAssignedBack;
                    caseFile.ModifiedBy = createdBy;
                    caseFile.ModifiedDate = DateTime.Now;
                    _dbContext.CaseFiles.Update(caseFile);
                    await _dbContext.SaveChangesAsync();
                    isSaved = true;
                }
            }
            catch (Exception)
            {
                throw;

            }

            return isSaved;
        }

        #endregion

        #region Update Case File IsAssigned 

        public async Task<bool> UpdateCaseFileIsAssigned(Guid referenceId, bool IsAssignedBack)
        {
            bool isSaved = false;
            try
            {

                CaseFile? caseFile = await _dbContext.CaseFiles.Where(x => x.FileId == referenceId).FirstOrDefaultAsync();
                if (caseFile is not null)
                {
                    caseFile.IsAssignedBack = IsAssignedBack;
                    caseFile.ModifiedDate = DateTime.Now;
                    _dbContext.CaseFiles.Update(caseFile);
                    await _dbContext.SaveChangesAsync();
                    isSaved = true;
                }
            }
            catch (Exception)
            {
                throw;

            }

            return isSaved;
        }

        #endregion

        public async Task UpdateCaseFileStatusandAddHistory(Guid FileId, string UserName)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var file = await _dbContext.CaseFiles.FindAsync(FileId);
                        await UpdateCaseFileStatus(file, (int)CaseFileStatusEnum.SaveAndCloseCaseFile, _dbContext);
                        await SaveCaseFileStatusHistory(UserName, file, (int)CaseFileEventEnum.SaveAndClose, _dbContext);
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }
        public async Task ProcessCaseFile(Guid MojRegistrationRequestId, string CreatedBy)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var request = await _dbContext.MojRegistrationRequests.FindAsync(MojRegistrationRequestId);
                        request.IsRegistered = true;
                        _dbContext.MojRegistrationRequests.Update(request);
                        await _dbContext.SaveChangesAsync();
                        var file = await _dbContext.CaseFiles.FindAsync(request.FileId);
                        file.StatusId = (int)CaseFileStatusEnum.RegisteredInMoj;
                        _dbContext.CaseFiles.Update(file);
                        await _dbContext.SaveChangesAsync();
                        await SaveCaseFileStatusHistory(CreatedBy, file, (int)CaseFileEventEnum.RegisteredAtMoj, _dbContext);
                        await _registeredCaseRepository.UpdateCaseRequestStatus(CreatedBy, file.RequestId, (int)CaseRequestStatusEnum.RegisteredInMOJ, _dbContext);
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
        public async Task<List<UserBasicDetailVM>> GetLawyersByCaseAndCanNumber(string caseNumber, string canNumber)
        {
            if (_UserBasicDetailVMs == null)
            {
                string StoreProc = $"exec pGetUserBasicDetailByCaseAndCanNumber @caseNumber = '{caseNumber}' , @canNumber = '{canNumber}'";
                _UserBasicDetailVMs = await _dbContext.UserBasicDetailVMs.FromSqlRaw(StoreProc).ToListAsync();
            }
            return _UserBasicDetailVMs;
        }

        public async Task<List<UserBasicDetailVM>> GetAllLawyersBySectorId(int sectorId)
        {
            try
            {
                var roleId = SystemRoles.Lawyer;

                string StoreProc = $"exec pGetLawyerBySectorId @sectorId = '{sectorId}' , @roleId = '{roleId}'";
                var a = await _dbContext.UserBasicDetailVMs.FromSqlRaw(StoreProc).ToListAsync();

                return a;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region Get Case File Sector Assignment
        public async Task<List<CmsCaseFileSectorAssignment>> GetCaseFileSectorAssigmentByFileId(Guid fileId, int sectorTypeId)
        {
            try
            {
                var result = await _dbContext.CmsCaseFileSectorAssignment.Where(x => x.FileId == fileId && x.SectorTypeId == sectorTypeId).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Create CaseFile 
        public async Task<CaseRequestCommunicationVM> CreateCaseFile(CaseRequest caseRequest)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        CaseRequestCommunicationVM caseRequestCommunication = new CaseRequestCommunicationVM();
                        caseRequest = OperateSectortype(caseRequest);
                        if (caseRequest.IsEdit)
                        {
                            _dbContext.Entry(caseRequest).State = EntityState.Modified;

                        }
                        else
                        {
                            var result = await _cMSCOMSInboxOutboxPatternNumberRepository.GenerateNumberPattern((int)caseRequest.GovtEntityId, (int)CmsComsNumPatternTypeEnum.CaseRequestNumber);
                            caseRequest.RequestNumber = result.GenerateRequestNumber;
                            caseRequest.RequestNumberFormat = result.FormatRequestNumber;
                            caseRequest.RequestPatternId = result.RequestPatternId;
                            caseRequest.PatternSequenceResult = result.PatternSequenceResult;
                            caseRequest.ApprovedBy = caseRequest.ReviewedBy = caseRequest.AssignedBy;
                            await _dbContext.CaseRequests.AddAsync(caseRequest);
                        }
                        await _dbContext.SaveChangesAsync();
                        await _caseRequestRepository.SaveCaseRequestStatusHistory(caseRequest.CreatedBy,caseRequest, caseRequest.IsEdit ? (int)CaseRequestEventEnum.Edited : (int)CaseRequestEventEnum.Created, _dbContext);
                        caseRequest.CaseParties = _mapper.Map<IList<CasePartyLink>>(caseRequest.CasePartyLinks);
                        caseRequest.CaseParties.ToList().ForEach(x => { x.CreatedBy = caseRequest.CreatedBy; x.CreatedDate = DateTime.Now; });
                        await _caseRequestRepository.SaveCaseParties(_dbContext, caseRequest);
                        if(caseRequest.StatusId == (int)CaseRequestStatusEnum.Submitted)
                        {
                            caseRequestCommunication = await SaveCommunicationOfCaseRequest(_dbContext, caseRequest);
                        }
                       
                        // File Creation
                        if(caseRequest.CourtTypeId != (int)CourtTypeEnum.Regional && caseRequest.StatusId != (int)CaseRequestStatusEnum.Draft)
                        {

                            var file = await _caseRequestRepository.CreateCasefile((Guid)caseRequest.RequestId, caseRequest.CreatedBy, _dbContext, (int)CaseFileStatusEnum.InProgress, caseRequest.cMSCOMSInboxOutbox);
                            await _caseRequestRepository.UpdateCaseRequestStatus(caseRequest.CreatedBy, (Guid)caseRequest.RequestId, (int)CaseRequestStatusEnum.ConvertedToFile, (int)CaseRequestEventEnum.AssignToLawyer, _dbContext);
                            await SaveCaseFileStatusHistory(file.CreatedBy, file, (int)CaseFileEventEnum.Created, _dbContext);

                            caseRequest.CopyAttachmentVMs.Add(new CopyAttachmentVM()
                            {
                                SourceId = (Guid)caseRequest.RequestId,
                                DestinationId = (Guid)file.FileId,
                                CreatedBy = caseRequest.CreatedBy
                            });

                            var copyAttachments = await _caseRequestRepository.CopyCasePartiesFromSourceToDestination((Guid)caseRequest.RequestId, file.FileId, caseRequest.CreatedBy, _dbContext);
                            if (copyAttachments.Any())
                            {
                                caseRequest.CopyAttachmentVMs.AddRange(copyAttachments);
                            }
                            caseRequestCommunication.CaseFile = file;
                        }
                        if(caseRequest.StatusId != (int)CaseRequestStatusEnum.Draft && caseRequest.GovtEntityId != null)
                        {
                            var g2gUser = await _g2gRepository.GetNextGEUserForRequestAssignment((int)caseRequest.GovtEntityId, true);
                            if(!string.IsNullOrEmpty(g2gUser.Item1))
                            {
                                caseRequest.CreatedBy = g2gUser.Item1;
                                caseRequest.DepartmentId = g2gUser.Item2;
                                _dbContext.CaseRequests.Update(caseRequest);
                            }
                        }
                        await _dbContext.SaveChangesAsync();
                        transaction.Commit();

                        

                        // For Notification 
                        caseRequest.NotificationParameter.RequestNumber = caseRequest.RequestNumber;
                        caseRequest.NotificationParameter.Entity = new CaseRequest().GetType().Name;
                        caseRequestCommunication.CaseRequest = caseRequest;
                        return caseRequestCommunication;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private CaseRequest OperateSectortype(CaseRequest caseRequest)
        {
            try
            {
                if (_dbContext.GovernmentEntity.Where(x => x.EntityId == caseRequest.GovtEntityId).Select(x => x.IsConfidential).FirstOrDefault())
                {
                    caseRequest.SectorTypeId = (int)OperatingSectorTypeEnum.PrivateOperationalSector;
                }
                else
                {
                    if (caseRequest.RequestTypeId == (int)RequestTypeEnum.Administrative && caseRequest.CourtTypeId == (int)CourtTypeEnum.Regional)
                    {
                        caseRequest.SectorTypeId = (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases;
                    }
                    else if (caseRequest.RequestTypeId == (int)RequestTypeEnum.Administrative && caseRequest.CourtTypeId == (int)CourtTypeEnum.Supreme)
                    {
                        caseRequest.SectorTypeId = (int)OperatingSectorTypeEnum.AdministrativeSupremeCases;
                    }
                    else if (caseRequest.RequestTypeId == (int)RequestTypeEnum.Administrative && caseRequest.CourtTypeId == (int)CourtTypeEnum.Appeal)
                    {
                        caseRequest.SectorTypeId = (int)OperatingSectorTypeEnum.AdministrativeAppealCases;
                    }
                    else if (caseRequest.RequestTypeId == (int)RequestTypeEnum.CivilCommercial && caseRequest.ClaimAmount > 5000)
                    {
                        if (caseRequest.CourtTypeId == (int)CourtTypeEnum.Regional)
                            caseRequest.SectorTypeId = (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases;
                        if (caseRequest.CourtTypeId == (int)CourtTypeEnum.Appeal)
                            caseRequest.SectorTypeId = (int)OperatingSectorTypeEnum.CivilCommercialAppealCases;
                        if (caseRequest.CourtTypeId == (int)CourtTypeEnum.Supreme)
                            caseRequest.SectorTypeId = (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases;
                    }
                    else if (caseRequest.RequestTypeId == (int)RequestTypeEnum.CivilCommercial && caseRequest.ClaimAmount < 5000)
                    {
                        caseRequest.SectorTypeId = (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases;
                    }
                }
                return caseRequest;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get Drafted Case Request List
        public async Task<List<CmsDraftedRequestListVM>> GetDraftedCaseRequestList(AdvanceSearchCmsCaseRequestVM advanceSearchVM)
        {
            try
            {
                if (_cmsDraftedRequestListVMs == null)
                {
                    string fromDate = advanceSearchVM.RequestFrom != null ? Convert.ToDateTime(advanceSearchVM.RequestFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string toDate = advanceSearchVM.RequestTo != null ? Convert.ToDateTime(advanceSearchVM.RequestTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string StoredProc = $"exec pGetCmsDraftedCaseRequestList @requestNumber= '{advanceSearchVM.RequestNumber}' , @createdBy= '{advanceSearchVM.CreatedBy}', @statusId='{advanceSearchVM.StatusId}' , @requestFrom='{fromDate}' , @requestTo='{toDate}' , @govEntityId='{advanceSearchVM.GovEntityId}',@PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}'";
                    _cmsDraftedRequestListVMs = await _dbContext.CmsDraftedRequestListVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                if(_cmsDraftedRequestListVMs != null)
                {
                    return _cmsDraftedRequestListVMs;

                }
                else
                {
                    return new List<CmsDraftedRequestListVM>();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion
        #region Save Communication for CaseRequest
        private async Task<CaseRequestCommunicationVM> SaveCommunicationOfCaseRequest(DatabaseContext dbContext, CaseRequest caseRequest)
        {
            try
            {
                Communication communication = new Communication();
                communication.CommunicationId = new Guid();
                communication.CommunicationTypeId = (int)CommunicationTypeEnum.CaseRequest;
                communication.Title = "Create_Case_Request";
                communication.OutboxNumber = caseRequest.MandatoryTempFiles.Where(x => x.AttachmentTypeId == (int)AttachmentTypeEnum.AuthorityLetter).Select(x => x.ReferenceNo).First();
                communication.OutboxDate = caseRequest.MandatoryTempFiles.Where(x => x.AttachmentTypeId == (int)AttachmentTypeEnum.AuthorityLetter).Select(x => x.ReferenceDate).First();
                communication.CorrespondenceTypeId = (int)CommunicationCorrespondenceTypeEnum.Outbox;
                communication.CreatedBy = caseRequest.CreatedBy;
                communication.CreatedDate = caseRequest.CreatedDate;
                communication.SectorTypeId = (int)caseRequest.SectorTypeId;
                communication.GovtEntityId = caseRequest.GovtEntityId;
                communication.SourceId = (int)CommunicationSourceEnum.FATWA;
                communication.SentBy = caseRequest.CreatedBy;
                await _communicationRepo.SaveCommunication(communication, dbContext);

                CommunicationTargetLink comTargetLink = new CommunicationTargetLink();
                comTargetLink.TargetLinkId = new Guid();
                comTargetLink.CommunicationId = communication.CommunicationId;
                comTargetLink.CreatedBy = caseRequest.CreatedBy;
                comTargetLink.CreatedDate = caseRequest.CreatedDate;
                await _communicationRepo.SaveCommunicationTargetLink(comTargetLink, dbContext);

                List<LinkTarget> linkTargets = new List<LinkTarget>();
                LinkTarget linkTarget;
                linkTarget = new LinkTarget()
                {
                    LinkTargetId = new Guid(),
                    IsPrimary = true,
                    ReferenceId = caseRequest.RequestId,
                    LinkTargetTypeId = (int)LinkTargetTypeEnum.CaseRequest
                };
                linkTargets.Add(linkTarget);
                linkTarget = new LinkTarget()
                {
                    LinkTargetId = new Guid(),
                    ReferenceId = communication.CommunicationId,
                    IsPrimary = false,
                    LinkTargetTypeId = (int)LinkTargetTypeEnum.Communication
                };
                linkTargets.Add(linkTarget);
                await _communicationRepo.SaveLinkTarget(linkTargets, comTargetLink.TargetLinkId, dbContext);

                CaseRequestCommunicationVM sendCommunication = new CaseRequestCommunicationVM();
                sendCommunication.Communication = communication;
                sendCommunication.CommunicationTargetLink = comTargetLink;
                sendCommunication.LinkTarget = linkTargets;
                return sendCommunication;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
