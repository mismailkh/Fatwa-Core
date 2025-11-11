using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.ViewModel.ConsultationVMs
{
    public class AdvanceSearchConsultationCaseFile : GridPagination
    {
        public Guid? ReferenceId { get; set; }
        public string? UserId { get; set; }
        public int? SectorTypeId { get; set; }
        public string? FileNumber { get; set; }
        public int? RequestTypeId { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public int? StatusId { get; set; }
        public int? GovEntityId { get; set; }


        //public UserDetailVM? userDetail { get; set; } = new UserDetailVM();
    }
}
