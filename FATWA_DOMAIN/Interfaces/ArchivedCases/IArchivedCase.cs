using FATWA_DOMAIN.Models.ArchivedCasesModels;
using FATWA_DOMAIN.Models.ViewModel.ArchivedCases;

namespace FATWA_DOMAIN.Interfaces.ArchivedCases
{
    public interface IArchivedCase
    {
        Task<List<ArchivedCaseListVM>> GetArchivedCaseList(ArchivedCaseAdvanceSearchVM archivedCaseAdvanceSearchVM);
        Task AddArchivedCaseData(AddArchivedCaseDataRequestPayload addArchivedCaseData);
        Task<ArchivedCaseDetailVM> GetArchivedCaseDetailByCaseId(Guid CaseId);
        Task<ArchivedCaseDocuments> GetArchivedCaseDocumentDetailById(Guid documentId);
    }
}
