using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.ConsultationVMs
{
    public class ConsultationDraftedRequestListVM : GridMetadata
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
        public string? GovtEntityEn { get; set; }
        public string? GovtEntityAr { get; set; }
        public string? LastActionEn { get; set; }
        public string? LastActionAr { get; set; }
        public string? ComplainantName { get; set; }
    }
}
