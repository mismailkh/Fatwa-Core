using FATWA_DOMAIN.Interfaces.ArchivedCases;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ArchivedCasesModels;
using FATWA_DOMAIN.Models.ViewModel.ArchivedCases;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace FATWA_INFRASTRUCTURE.Repository.ArchivedCases
{
    public class ArchivedCasesRepository : IArchivedCase
    {
        #region Variables Declaration
        private readonly ArchivedCasesDbContext _dbContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private List<ArchivedCaseListVM> ArchivedCaseListVM = new();

        #endregion

        #region Contructor
        public ArchivedCasesRepository(ArchivedCasesDbContext dbContext, IServiceScopeFactory serviceScopeFactory)
        {
            _dbContext = dbContext;
            _serviceScopeFactory = serviceScopeFactory;
        }
        #endregion

        #region Save Archived Cases

        #region Add Archived Cases Data
        public async Task AddArchivedCaseData(AddArchivedCaseDataRequestPayload addArchivedCaseDataPayload)
        {
            #region Save Payload
            ArchivedCaseDataPayload archivedCaseDataPayload = new ArchivedCaseDataPayload
            {
                Id = Guid.NewGuid(),
                Payload = JsonConvert.SerializeObject(addArchivedCaseDataPayload),
                CreatedBy = "System Generated",
                CreatedDate = DateTime.Now,
                IsDeleted = false
            };

            await _dbContext.ArchivedCaseDataPayload.AddAsync(archivedCaseDataPayload);
            await _dbContext.SaveChangesAsync();
            #endregion

            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var canId = await AddArchivedCaseAutomatedNumber(addArchivedCaseDataPayload);
                        await AddArchivedCases(addArchivedCaseDataPayload.Cases, canId);
                        await AddArchivedCaseParties(addArchivedCaseDataPayload.CasesParties, canId);
                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }
        #endregion

        #region Add Archived Case Automated Number
        private async Task<Guid> AddArchivedCaseAutomatedNumber(AddArchivedCaseDataRequestPayload addArchivedCaseData)
        {
            try
            {
                var archivedCANData = new ArchivedCaseAutomatedNumber
                {
                    Id = Guid.NewGuid(),
                    CaseAutomatedNumber = addArchivedCaseData.CANNumber,
                    MigrationDateTime = addArchivedCaseData.MigrationDateTime,
                    CreatedBy = "System Generated",
                    CreatedDate = DateTime.Now,
                    IsDeleted = false
                };

                _dbContext.ArchivedCaseAutomatedNumber.Add(archivedCANData);
                await _dbContext.SaveChangesAsync();

                return archivedCANData.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Archived Case
        private async Task AddArchivedCases(List<AddArchivedCaseVM> cases, Guid CANId)
        {
            try
            {
                foreach (var caseData in cases)
                {
                    var archivedCase = new ArchivedCasesModel
                    {
                        Id = caseData.CaseId,
                        CANId = CANId,
                        CaseNumber = caseData.CaseNumber,
                        CaseDate = caseData.CaseDate.Date,
                        CourtTypeId = caseData.CourtCode,
                        ChamberTypeId = caseData.ChamberCode,
                        ChamberNumberId = caseData.ChamberNumber,
                        CreatedBy = "System Generated",
                        CreatedDate = DateTime.Now,
                        IsDeleted = false
                    };

                    _dbContext.ArchivedCases.Add(archivedCase);
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Archived Case Parties
        private async Task AddArchivedCaseParties(List<AddArchivedCasePartyVM> caseParties, Guid CANId)
        {
            try
            {
                foreach (var casePartiesData in caseParties)
                {
                    var caseParty = new ArchivedCaseParties
                    {
                        Id = Guid.NewGuid(),
                        CANId = CANId,
                        PartyRoleId = casePartiesData.PartyRoleId,
                        PartyName = casePartiesData.Name,
                        MOJPartyId = casePartiesData.MojPartyId,                        
                        CreatedBy = "System Generated",
                        CreatedDate = DateTime.Now,
                        IsDeleted = false
                    };

                    _dbContext.ArchivedCaseParties.Add(caseParty);
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #endregion

        #region Get Archived Case List
        public async Task<List<ArchivedCaseListVM>> GetArchivedCaseList(ArchivedCaseAdvanceSearchVM archivedCaseAdvanceSearchVM)
        {
            try
            {                
                string sp = $"exec pArcGetArchivedCaseList @CaseAutomatedNumber = '{archivedCaseAdvanceSearchVM.CaseAutomatedNumber}', @CaseNumber = '{archivedCaseAdvanceSearchVM.CaseNumber}', @ChamberTypeId = '{archivedCaseAdvanceSearchVM.ChamberTypeId}', @ChamberNumberId = '{archivedCaseAdvanceSearchVM.ChamberNumberId}', @CourtTypeId = '{archivedCaseAdvanceSearchVM.CourtTypeId}',@PlaintiffName = N'{archivedCaseAdvanceSearchVM.PlaintiffName}',@DefendantName = N'{archivedCaseAdvanceSearchVM.DefendantName}', @PageSize='{archivedCaseAdvanceSearchVM.PageSize}', @PageNumber='{archivedCaseAdvanceSearchVM.PageNumber}'";
                var result = _dbContext.ArchivedCaseListVM.FromSqlRaw(sp).AsNoTracking().ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Archived Case Detail
        public async Task<ArchivedCaseDetailVM> GetArchivedCaseDetailByCaseId(Guid caseId)
        {
            try
            {
                string storedProc = $"EXEC pArcGetArchivedCaseDetail @CaseId='{caseId}'";
                var archivedCaseDetails = _dbContext.ArchivedCaseDetailVM.FromSqlRaw(storedProc).AsNoTracking().ToList();
                var archivedCase = archivedCaseDetails.FirstOrDefault();

                if(archivedCase != null)
                {
                    string storedProcParties = $"EXEC pArcGetArchivedCaseParties @CANId='{archivedCase.CANId}'";
                    archivedCase.CasePartiesList = await _dbContext.ArchivedCasePartiesVM.FromSqlRaw(storedProcParties).AsNoTracking().ToListAsync();

                    string storedProcDocuments = $"EXEC pArcGetArchivedCaseDocuments @CaseId='{archivedCase.CaseId}'";
                    archivedCase.CaseDocumentsList = await _dbContext.ArchivedCaseDocumentsVM.FromSqlRaw(storedProcDocuments).AsNoTracking().ToListAsync();
                }

                return archivedCase;
            }
            catch (Exception )
            {
                throw;
            }
        }
        #endregion

        #region Get Archived Case Document Detail
        public async Task<ArchivedCaseDocuments> GetArchivedCaseDocumentDetailById(Guid documentId)
        {
            try
            {
                var result = await _dbContext.ArchivedCaseDocuments.Where(x => x.Id == documentId).FirstOrDefaultAsync();
                if (result is not null)
                    return result;
                return null;

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
