using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.Consultation;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_INFRASTRUCTURE.Repository.Consultation
{
    public class COMSConsultationFileRepository : ICOMSConsultationFile
    {
        #region Variables declaration
        private List<ConsultationFileListVM> _ConsultationFileListVM;
        private ConsultationFileDetailVM _ConsultationFileDetailVM;
        private List<ConsultationFileHistoryVM> _ConsultationFileHistoryVM;
        private List<ConsultationFileAssignmentVM> _ConsultationFileAssignmentVM;
        private List<ConsultationFileAssignmentHistoryVM> _ConsultationFileAssignmentHistoryVM;
        private List<ConsultationParty> _ConsultationParty;
        private readonly DatabaseContext _dbContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ITempFileUpload _IFileUpload;
        #endregion

        #region Constructor
        public COMSConsultationFileRepository(DatabaseContext databaseContext, IServiceScopeFactory serviceScopeFactory, ITempFileUpload iFileUpload)
        {
            _dbContext = databaseContext;
            _serviceScopeFactory = serviceScopeFactory;
            _IFileUpload = iFileUpload;
        }
        #endregion

        #region Get All Consultation File List
        public async Task<List<ConsultationFileListVM>> GetConsultationFileList(AdvanceSearchConsultationCaseFile advanceSearchVM)
        {
            try
            {
                if (_ConsultationFileListVM == null)
                {
                    string CreatedfromDate = advanceSearchVM.CreatedFrom != null ? Convert.ToDateTime(advanceSearchVM.CreatedFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string CreatedtoDate = advanceSearchVM.CreatedTo != null ? Convert.ToDateTime(advanceSearchVM.CreatedTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;

                    string StoreProc = $"exec pConsultationFileList @fileNumber = '{advanceSearchVM.FileNumber}' , @userId = '{advanceSearchVM.UserId}'  , @createdFrom = '{CreatedfromDate}' , @createdTo = '{CreatedtoDate}' ,@sectorTypeId = '{advanceSearchVM.SectorTypeId}',@requestTypeId = '{advanceSearchVM.RequestTypeId}', @statusId='{advanceSearchVM.StatusId}', @govEntityId='{advanceSearchVM.GovEntityId}'" +
                        $",@PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}'";
                    _ConsultationFileListVM = await _dbContext.ConsultationFileListVMs.FromSqlRaw(StoreProc).ToListAsync();
                }
                return _ConsultationFileListVM;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get GetConsultationFileDetailById
        public async Task<ConsultationFileDetailVM> GetConsultationFileDetailById(Guid FileId)
        {
            try
            {

                string StoreProc = $"exec pConsultationFileDetailById @FileId = '{FileId}'";
                var res = await _dbContext.ConsultationFileDetailVMs.FromSqlRaw(StoreProc).ToListAsync();
                if (res.Count > 0)
                {
                    _ConsultationFileDetailVM = res.FirstOrDefault();
                    //if (_ConsultationFileDetailVM != null && _ConsultationFileDetailVM.SectorTypeId == (int)OperatingSectorTypeEnum.Contracts)
                    //{
                    //    var resultParty = await GetConsultationPartyDetailForFileByConsultationId(_ConsultationFileDetailVM.ConsultationRequestId);
                    //    _ConsultationFileDetailVM.ConsultationPartysDetailsForFile = resultParty;
                    //    var resultArticle = await GetConsultationArticleDetailForFileByConsultationId(_ConsultationFileDetailVM.ConsultationRequestId);
                    //    _ConsultationFileDetailVM.ConsultationArticlesDetailsForFile = resultArticle;
                    //    var resultIntroduction = await GetConsultationIntroductionDetailForFileByConsultationId(_ConsultationFileDetailVM.ConsultationRequestId);
                    //    _ConsultationFileDetailVM.ConsultationIntroductionForFile = resultIntroduction;
                    //}
                }

                return _ConsultationFileDetailVM;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        private async Task<List<ConsultationPartyListVM>> GetConsultationPartyDetailForFileByConsultationId(Guid consultationId)
        {
            try
            {
                string StoredProc = $"exec pConsultationPartyList @consultationId ='{consultationId}'";
                var result = await _dbContext.ConsultationPartyVMs.FromSqlRaw(StoredProc).ToListAsync();
                if (result.Count() != 0)
                {
                    return result;
                }
                return new List<ConsultationPartyListVM>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<List<ConsultationArticleByConsultationIdListVM>> GetConsultationArticleDetailForFileByConsultationId(Guid consultationId)
        {
            try
            {
                string StoredProc = $"exec pConsultationArticleByConsultationId @consultationId ='{consultationId}'";
                var result = await _dbContext.ConsultationArticleByConsultationIdListVMs.FromSqlRaw(StoredProc).ToListAsync();
                if (result.Count() != 0)
                {
                    return result;
                }
                return new List<ConsultationArticleByConsultationIdListVM>();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        private async Task<string> GetConsultationIntroductionDetailForFileByConsultationId(Guid consultationId)
        {
            try
            {
                var result = await _dbContext.ConsultationRequests.Where(x => x.ConsultationRequestId == consultationId).Select(x => x.Introduction).FirstOrDefaultAsync();
                if (result != null)
                {
                    return result;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region GetConslutationFileStatusHistory
        public async Task<List<ConsultationFileHistoryVM>> GetConslutationFileStatusHistory(Guid FileId)
        {
            try
            {

                string StoreProc = $"exec pConsultationFileStatusHistory @FileId = '{FileId}'";
                _ConsultationFileHistoryVM = await _dbContext.ConsultationFileHistoryVMs.FromSqlRaw(StoreProc).ToListAsync();

                return _ConsultationFileHistoryVM;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region GetConsultationAssigneeList
        public async Task<List<ConsultationFileAssignmentVM>> GetConsultationAssigneeList(Guid FileId)
        {
            try
            {

                string StoreProc = $"exec pConsultationAssigneeList @referenceId = '{FileId}'";
                _ConsultationFileAssignmentVM = await _dbContext.ConsultationFileAssignmentVMs.FromSqlRaw(StoreProc).ToListAsync();

                return _ConsultationFileAssignmentVM;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Case File Assignment History


        public async Task<List<ConsultationAssignment>> GetConsultationAssigment(Guid referenceId)
        {
            try
            {
                var res = _dbContext.ConsultationAssignments.Where(x => x.ReferenceId == referenceId).ToList();
                if (res.Count != 0)
                {
                    return res;
                }
                else
                {
                    return res = new List<ConsultationAssignment>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<ConsultationFileAssignmentHistoryVM>> GetConsultationAssigmentHistory(Guid referenceId)
        {
            try
            {
                if (_ConsultationFileAssignmentHistoryVM == null)
                {
                    string StoredProc = $"exec pConsultationAssignmentHistory @referenceId ='{referenceId}' ";
                    _ConsultationFileAssignmentHistoryVM = await _dbContext.ConsultationFileAssignmentHistoryVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _ConsultationFileAssignmentHistoryVM;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Consultation File(Main Model)
        public async Task<ConsultationFile> GetConsultationFile(Guid fileId)
        {

            using var scope = _serviceScopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (_dbContext)
            {
                try
                {
                    return await _dbContext.ConsultationFiles.Where(f => f.FileId == fileId).FirstOrDefaultAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        #endregion

        #region Assignment
        public async Task UpdateConsultationFileStatus(Guid fileId, int StatusId, int EventId, int? FatwaPriorityId, DatabaseContext dbContext)
        {
            try
            {
                var file = await dbContext.ConsultationFiles.FindAsync(fileId);
                file.StatusId = StatusId;
                file.FatwaPriorityId = FatwaPriorityId;
                dbContext.ConsultationFiles.Update(file);
                await dbContext.SaveChangesAsync();
                await SaveConsultationFileStatusHistory(file, EventId, dbContext);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        public async Task SaveConsultationFileStatusHistory(ConsultationFile consultationFile, int EventId, DatabaseContext dbContext)
        {
            try
            {
                ConsultationFileHistory historyobj = new ConsultationFileHistory();
                historyobj.HistoryId = Guid.NewGuid();
                historyobj.FileId = consultationFile.FileId;
                historyobj.StatusId = consultationFile.StatusId;
                historyobj.CreatedDate = DateTime.Now;
                historyobj.CreatedBy = consultationFile.CreatedBy;
                historyobj.EventId = EventId;
                await dbContext.AddAsync(historyobj);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Copy Consultation File
        public async Task<ConsultationFile> CopyConsultationFile(Guid oldCaseFileId, string username, DatabaseContext dbContext)
        {
            try
            {
                var oldConsultationFile = await dbContext.ConsultationFiles.FindAsync(oldCaseFileId);
                ConsultationRequest casereq = await dbContext.ConsultationRequests.FindAsync(oldConsultationFile.RequestId);
                GovernmentEntity entity = await dbContext.GovernmentEntity.FindAsync(casereq.GovtEntityId);
                ConsultationFile consultationFile = new ConsultationFile();
                consultationFile.FileNumber = DateTime.Now.Year + "CO" + (dbContext.ConsultationFiles.Any() ? await dbContext.ConsultationFiles.Select(x => x.ShortNumber).MaxAsync() + 1 : 1).ToString().PadLeft(6, '0');
                consultationFile.ShortNumber = dbContext.ConsultationFiles.Any() ? await dbContext.ConsultationFiles.Select(x => x.ShortNumber).MaxAsync() + 1 : 1;
                consultationFile.FileName = consultationFile.FileNumber + "_" + entity?.Name_En + "_" + DateOnly.FromDateTime(DateTime.Now).ToString();
                consultationFile.RequestId = oldConsultationFile.RequestId;
                consultationFile.CreatedBy = username;
                consultationFile.CreatedDate = DateTime.Now;
                consultationFile.StatusId = (int)CaseFileStatusEnum.InProgress;
                await dbContext.ConsultationFiles.AddAsync(consultationFile);
                await dbContext.SaveChangesAsync();
                await SaveConsultationFileStatusHistory(consultationFile, (int)CaseFileEventEnum.ReceivedCopy, dbContext);
                return consultationFile;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Consultation File Communication 
        // For G2G to Fatwa Communication //
        public async Task<ConsultationFile> ConsultationFileDetailWithPartiesAndAttachments(Guid fileId)
        {
            var consultationFile = await GetConsultationFile(fileId);
            var consultationParties = await GetCOMSConsultationPartyDetailByGuid(consultationFile.FileId);
            foreach (var consultationParty in consultationParties)
            {
                consultationFile.PartyLinks.Add(consultationParty);
            }
            return consultationFile;
        }

        #endregion

        #region Get Consultation Party detail by Consultation Request Id(Guid)
        public async Task<List<ConsultationParty>> GetCOMSConsultationPartyDetailByGuid(Guid Id)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (_dbContext)
            {
                try
                {
                    if (_ConsultationParty == null)
                    {
                        _ConsultationParty = await _dbContext.ConsultationParties.Where(x => x.ConsultationRequestId == Id).ToListAsync();
                    }
                    return _ConsultationParty;
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }
        }
        #endregion

        #region GetConslutationFileStatusHistoryforList
        public async Task<List<ConsultationFileHistoryVM>> GetConslutationFileStatusHistoryForList(Guid FileId)
        {
            try
            {

                string StoreProc = $"exec pConsultationFileStatusHistoryForList @FileId = '{FileId}'";
                _ConsultationFileHistoryVM = await _dbContext.ConsultationFileHistoryVMs.FromSqlRaw(StoreProc).ToListAsync();

                return _ConsultationFileHistoryVM;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ConsultationFileListDmsVM>> GetAllConsultationFileListBySectorTypeId(int sectorTypeId, string userId)
        {
            try
            {
                List<ConsultationFileListDmsVM> _ConsultationFiles;
                string StoredProc = $"exec pConsultationFileListForDms @sectorTypeId ='{sectorTypeId}', @userId = '{userId}'";
                _ConsultationFiles = await _dbContext.ConsultationFileListDmsVMs.FromSqlRaw(StoredProc).ToListAsync();
                return _ConsultationFiles;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<ConsultationFileHistory> SaveConsultationFileStatusHistoryReturn(ConsultationFile consultationFile, int EventId, DatabaseContext dbContext)
        {
            try
            {
                ConsultationFileHistory historyobj = new ConsultationFileHistory();
                historyobj.HistoryId = Guid.NewGuid();
                historyobj.FileId = consultationFile.FileId;
                historyobj.StatusId = consultationFile.StatusId;
                historyobj.CreatedDate = DateTime.Now;
                historyobj.CreatedBy = consultationFile.CreatedBy;
                historyobj.EventId = EventId;
                historyobj.Remarks = null;
                await dbContext.AddAsync(historyobj);
                await dbContext.SaveChangesAsync();
                return historyobj;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Drafted Consultation Request List
        public async Task<List<ConsultationDraftedRequestListVM>> GetDraftedConsultationRequestList(AdvanceSearchConsultationRequestVM advanceSearchVM)
        {
            try
            {
                string fromDate = advanceSearchVM.RequestFrom != null ? Convert.ToDateTime(advanceSearchVM.RequestFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string toDate = advanceSearchVM.RequestTo != null ? Convert.ToDateTime(advanceSearchVM.RequestTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string storProc = $"exec pGetComsDraftedConsultationRequestList @requestNumber= '{advanceSearchVM.RequestNumber}' , @createdBy= '{advanceSearchVM.userDetail.UserName}', @statusId='{advanceSearchVM.StatusId}' , @requestFrom='{fromDate}' , @requestTo='{toDate}' , @govEntityId='{advanceSearchVM.GovEntityId}', @PageSize='{advanceSearchVM.PageSize}', @PageNumber='{advanceSearchVM.PageNumber}'";
                var result = await _dbContext.ConsultationDraftedRequestListVMs.FromSqlRaw(storProc).ToListAsync();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new List<ConsultationDraftedRequestListVM>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
        #endregion


    }
}
