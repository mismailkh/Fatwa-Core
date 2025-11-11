using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Models.AdminModels.CaseManagment;
using FATWA_DOMAIN.Interfaces.Common;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_INFRASTRUCTURE.Database;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using Microsoft.EntityFrameworkCore;
using FATWA_DOMAIN.Models.Email;

namespace FATWA_INFRASTRUCTURE.Repository.CommonRepos
{
    public class CommonRepository
    {
        private readonly DatabaseContext _dbContext;
        public CommonRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<EmailConfiguration> GetEmailConfiguration(int ApplicationId)
        {
            try
            {
                return await _dbContext.EmailConfigurations.Where(u => u.ApplicationId == ApplicationId).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Update Entity Status 

        //<History Author = 'Nadia Gull' Date='2023-01-13' Version="1.0" Branch="master"> Update CaseRequest / CaseFile Status</History>
        public async Task<bool> UpdateEntityStatus(UpdateEntityStatusVM updateEntity)
        {
            try
            {
                if (updateEntity.SubModuleId == (int)SubModuleEnum.CaseRequest)
                {
                    await UpdateCaseRequest(updateEntity);
                    return true;
                }
                if (updateEntity.SubModuleId == (int)SubModuleEnum.CaseFile)
                {
                    await UpdateCaseFile(updateEntity);
                    return true;
                }
                if (updateEntity.SubModuleId == (int)SubModuleEnum.RegisteredCase)
                {
                    await UpdateRegisteredCase(updateEntity);
                    return true;
                }
                if (updateEntity.SubModuleId == (int)SubModuleEnum.ConsultationRequest)
                {
                    await UpdateConsultationRequest(updateEntity);
                    return true;
                }
                
                if (updateEntity.SubModuleId == (int)SubModuleEnum.ConsultationFile)
                {
                    await UpdateConsultationFile(updateEntity);
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

        #region Update CaseRequest  
        private async Task UpdateCaseRequest(UpdateEntityStatusVM updateEntity)
        {
            try
            {
                var caseRequest = await _dbContext.CaseRequests.FindAsync(updateEntity.ReferenceId);
                if (caseRequest != null)
                {
                    caseRequest.StatusId = updateEntity.StatusId;
                    _dbContext.CaseRequests.Update(caseRequest);
                    await _dbContext.SaveChangesAsync();
                    if (caseRequest.StatusId == (int)CaseRequestStatusEnum.PendingForGEResponse)
                    {
                        await SaveCaseRequestStatusHistory(updateEntity, (int)CaseRequestEventEnum.LegalNotificationSend, _dbContext);
                    }
                    else if(caseRequest.StatusId == (int)CaseRequestStatusEnum.Resubmitted)
                    {
                        await SaveCaseRequestStatusHistory(updateEntity, (int)CaseRequestEventEnum.ReceiveCommunication, _dbContext);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region save case request status history

        private async Task SaveCaseRequestStatusHistory(UpdateEntityStatusVM updateEntity, int EventId, DatabaseContext dbContext)
        {
            try
            {
                CmsCaseRequestHistory historyobj = new CmsCaseRequestHistory();
                historyobj.HistoryId = Guid.NewGuid();
                historyobj.RequestId = updateEntity.ReferenceId;
                historyobj.StatusId = updateEntity.StatusId;
                if (EventId == (int)CaseRequestEventEnum.LegalNotificationSend)
                {
                    historyobj.CreatedDate = (DateTime)updateEntity.ModifiedDate;
                    historyobj.CreatedBy = updateEntity.ModifiedBy;
                }
                else if (EventId == (int)CaseRequestEventEnum.ReceiveCommunication)
                {
                    historyobj.CreatedDate = (DateTime)updateEntity.ModifiedDate;
                    historyobj.CreatedBy = updateEntity.ModifiedBy;
                }
                historyobj.EventId = EventId;
                await dbContext.CmsCaseRequestHistories.AddAsync(historyobj);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update CaseFile  
        private async Task<CaseFile> UpdateCaseFile(UpdateEntityStatusVM updateEntity)
        {
            try
            {
                var caseFile = await _dbContext.CaseFiles.FindAsync(updateEntity.ReferenceId);
                if (caseFile != null)
                {
                    // update casefile status
                    caseFile.StatusId = updateEntity.StatusId;
                    _dbContext.CaseFiles.Update(caseFile);
                    await _dbContext.SaveChangesAsync();
                }
                return caseFile;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update RegisteredCase  
        private async Task<CmsRegisteredCase> UpdateRegisteredCase(UpdateEntityStatusVM updateEntity)
        {
            try
            {
                var registeredCase = await _dbContext.cmsRegisteredCases.FindAsync(updateEntity.ReferenceId);
                if (registeredCase != null)
                {
                    // update casefile status
                    registeredCase.StatusId = updateEntity.StatusId;
                    _dbContext.cmsRegisteredCases.Update(registeredCase);
                    await _dbContext.SaveChangesAsync();
                }
                return registeredCase;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update ConsultationFile 
        private async Task<ConsultationFile> UpdateConsultationFile(UpdateEntityStatusVM updateEntity)
        {
            try
            {
                var consultationFile = await _dbContext.ConsultationFiles.FindAsync(updateEntity.ReferenceId);
                if (consultationFile != null)
                {
                    // update casefile status
                    consultationFile.StatusId = updateEntity.StatusId;
                    _dbContext.ConsultationFiles.Update(consultationFile);
                    await _dbContext.SaveChangesAsync();
                }
                return consultationFile;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update ConsultationRequest  
        private async Task UpdateConsultationRequest(UpdateEntityStatusVM updateEntity)
        {
            try
            {
                var consultationRequest = await _dbContext.ConsultationRequests.FindAsync(updateEntity.ReferenceId);
                if (consultationRequest != null)
                {
                    consultationRequest.RequestStatusId = updateEntity.StatusId;
                    _dbContext.ConsultationRequests.Update(consultationRequest);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region  Get Governament Entity User List
        public async Task<List<GovernmentEntitiesUsersVM>> GetAllGEUserList()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pGetAllGEUserList";

                var result = await _dbContext.GovernmentEntitiesUsersVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion

        #region  Get Governament Entity User List
        public async Task<List<GovernmentEntitiesUsersVM>> GetAllCaseRequestNumber()
        {
            string storedProc;
            try
            {
                storedProc = $"exec pGetAllGEUserList";

                var result = await _dbContext.GovernmentEntitiesUsersVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion
    }
}
