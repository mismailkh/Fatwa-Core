using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;


namespace FATWA_DOMAIN.Models.ViewModel.ConsultationVMs
{
    public  class ConsultationRequestVM : GridMetadata
    {
        [Key]
        public Guid ConsultationRequestId { get; set; }
        public string? RequestNumber { get; set; }
        public string? Subject { get; set; }
        public int? RequestTypeId { get; set; }
        public string? RequestType_Name_En { get; set; }
        public string? RequestType_Name_Ar { get; set; }
        public int? RequestStatusId { get; set; }
        public int? SectorTypeId { get; set; }
        public string? Status_Name_En { get; set; }
        public string? Status_Name_Ar { get; set; }
        public string? SectorType_Name_Ar { get; set; }
        public string? SectorType_Name_En { get; set; }
        public DateTime? RequestDate { get; set; }
        public int? WithdrawCount { get; set; }
    }
    public class ConsultationRequestDmsVM
    {
        [Key]
        public Guid ConsultationRequestId { get; set; }
        public string? RequestNumber { get; set; }
        public string? Subject { get; set; }
        public int? RequestTypeId { get; set; }
        public string? RequestType_Name_En { get; set; }
        public string? RequestType_Name_Ar { get; set; }
        public int? RequestStatusId { get; set; }
        public int? SectorTypeId { get; set; }
        public string? Status_Name_En { get; set; }
        public string? Status_Name_Ar { get; set; }
        public string? SectorType_Name_Ar { get; set; }
        public string? SectorType_Name_En { get; set; }
        public DateTime? RequestDate { get; set; }
        public int? WithdrawCount { get; set; }
    }

}
