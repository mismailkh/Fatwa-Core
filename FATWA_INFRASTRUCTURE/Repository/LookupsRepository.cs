using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_INFRASTRUCTURE.Repository
{
    public class LookupsRepository : ILookups
    {
        private readonly DatabaseContext _dbContext;
        public LookupsRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-29' Version="1.0" Branch="master"> Get Government Entities</History>
        public async Task<List<GovernmentEntity>> GetGovernmentEntities()
        {
            try
            {
                return await _dbContext.GovernmentEntity.OrderBy(u => u.EntityId).ToListAsync();
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

        #region get court types
        //<History Author = 'Muhammad Zaeem' Date='2022-11-9' Version="1.0" Branch="master"> Get court types</History>
        public async Task<List<CourtType>> GetCourtTypes()
        {
            try
            {
                return await _dbContext.CourtTypes.OrderBy(u => u.Id).ToListAsync();
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
                return await _dbContext.Courts.OrderBy(u => u.Id).ToListAsync();
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
                return await _dbContext.Chambers.OrderBy(u => u.Id).ToListAsync();
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


            try
            {

                CmsAssignLawyerToCourt lawyerObj = new CmsAssignLawyerToCourt();


                lawyerObj.LawyerId = cmsAssignLawyerToCourt.LawyerId;
                lawyerObj.CourtTypeId = cmsAssignLawyerToCourt.CourtTypeId;
                lawyerObj.CourtId = cmsAssignLawyerToCourt.CourtId;
                lawyerObj.ChamberId = cmsAssignLawyerToCourt.ChamberId;
                lawyerObj.CreatedBy = cmsAssignLawyerToCourt.CreatedBy;
                lawyerObj.CreatedDate = cmsAssignLawyerToCourt.CreatedDate;

                await _dbContext.CmsAssignLawyerToCourts.AddAsync(lawyerObj);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region lookups by zain
       

        //<History Author = 'Hassan Abbas' Date='2022-09-29' Version="1.0" Branch="master"> Get Sector Subtypes</History>
        public async Task<List<Subtype>> GetAllSectorSubtypes()
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
        public async Task<List<Subtype>> GetSectorSubtypesBySector(int sectorType)
        {
            try
            {
                return await _dbContext.Subtypes.Where(t => t.SectorTypeId == sectorType).OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        

        //<History Author = 'Hassan Abbas' Date='2022-09-29' Version="1.0" Branch="master"> Get Ministries</History>
        public async Task<List<Ministry>> GetMinistries()
        {
            try
            {
                return await _dbContext.Ministries.OrderBy(u => u.Id).ToListAsync();
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
                return await _dbContext.Departments.OrderBy(u => u.Id).ToListAsync();
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

        //<History Author = 'Hassan Abbas' Date='2022-09-29' Version="1.0" Branch="master"> Get Case Templates based on Attachment Type</History>
        public async Task<List<CaseTemplate>> GetCaseTemplates(int attachmentTypeId)
        {
            try
            {
                return await _dbContext.CaseTemplate.Where(t => t.AttachmentTypeId == attachmentTypeId || t.AttachmentTypeId == 0).OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
