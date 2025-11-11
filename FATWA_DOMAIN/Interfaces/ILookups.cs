
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.MeetModels;

namespace FATWA_DOMAIN.Interfaces
{
    public interface ILookups
    {
        Task<List<GovernmentEntity>> GetGovernmentEntities();
        Task<List<OperatingSectorType>> GetOperatingSectorTypes();
        Task<List<CaseRequestStatus>> GetCaseRequestStatuses();
        Task<List<CaseFileStatus>> GetCaseFileStatuses();
        Task<List<Priority>> GetCasePriorities();
        Task<List<CourtType>> GetCourtTypes();
        Task<List<Court>> GetCourts();
        Task<List<Chamber>> GetChambers();
        Task SaveAssignLawyerToCourt(CmsAssignLawyerToCourt cmsAssignLawyerToCourt);


        Task<List<ResponseType>> GetResponseTypes();

        #region
        Task<List<Subtype>> GetSectorSubtypesBySector(int sectorType);
        Task<List<Subtype>> GetAllSectorSubtypes();
        Task<List<Ministry>> GetMinistries();
        Task<List<Department>> GetDepartments();
        Task<List<MeetingType>> GetMeetingTypes();
        #endregion
        Task<List<CaseTemplate>> GetCaseTemplates(int attachmentTypeId);
    }
}
