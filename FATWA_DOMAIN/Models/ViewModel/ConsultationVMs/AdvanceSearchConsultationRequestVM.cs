using FATWA_DOMAIN.Models.BaseModels;


namespace FATWA_DOMAIN.Models.ViewModel.ConsultationVMs
{
    public class AdvanceSearchConsultationRequestVM : GridPagination
    {
        public string? RequestNumber { get; set; }
        public int? StatusId { get; set; }
        public string? Subject { get; set; }
        public int? RequestTypeId { get; set; }
        public DateTime? RequestFrom { get; set; }
        public DateTime? RequestTo { get; set; }
        public int? SectorTypeId { get; set; }
        public bool ShowUndefinedRequest { get; set; }

        public UserDetailVM? userDetail { get; set; } = new UserDetailVM();
        public int? GovEntityId { get; set; }

    }
}
