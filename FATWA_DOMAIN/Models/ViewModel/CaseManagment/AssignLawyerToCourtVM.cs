using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.CaseManagment;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public class AssignLawyerToCourtVM: TransactionalBaseModel
    {

        [Key]
        public Guid Id { get; set; }
        public string? LawyerFirstNameEn { get; set; }
        public string? LawyerFirstNameAr { get; set; }
        public string? LawyerFullNameEn { get; set; }
        public string? LawyerFullNameAr { get; set; }
        public string? ChamberNumber { get; set; }
        public string? CourtTypeEn { get; set; }
        public string? CourtTypeAr { get; set; }
        public string? CourtNameEn { get; set; }
        public string? CourtNameAr { get; set; }
        public string? ChamberNameAr { get; set; }
        public string? ChamberNameEn { get; set; }
        public int TotalCount { get; set; }
        [NotMapped]
        public IList<User> users { get; set; } = new List<User>();

        [NotMapped]
        public IList<CourtType> CourtTypes { get; set; } = new List<CourtType>();

        [NotMapped]
        public IList<Court> Courts { get; set; } = new List<Court>();

        [NotMapped]
        public IList<Chamber> Chambers { get; set; } = new List<Chamber>();
    }
    public class AdvanceSearchVMAssignLawyerToCourt : GridPagination
    {
        public string? LawyerName { get; set; }
        public int? CourtName { get; set; }
        public int? CourtType { get; set; }
        public int? ChamberName { get; set; }
        public int? ChamberNumber { get; set; }
        public int? SectorTypeId { get; set; }
    }
}
