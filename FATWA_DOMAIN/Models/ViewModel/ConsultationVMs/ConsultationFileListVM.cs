using System.ComponentModel.DataAnnotations.Schema;
using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.ViewModel.ConsultationVMs
{
    public class ConsultationFileListVM : GridMetadata
    {
        public Guid? FileId { get; set; }
        public string? FileNumber { get; set; }
       // public Guid? FileNumber { get; set; }
        public string? RequestTypeNameEn { get; set; }
        public string? RequestTypeNameAr { get; set; }
        public string? Subject { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? StatusId { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public int? PriorityId { get; set; }
        public string? GovernmentEntityNameEn { get; set; }
        public string? GovernmentEntityNameAr { get; set; }
        public string? LawyerNameEn { get; set; }
        public string? LawyerNameAr { get; set; }
        public int? SectorTypeId { get; set; }
        //public string? Lawyer { get; set; }
        [NotMapped]
        public IList<ConsultationFileHistoryVM> LatestHistory { get; set; } = new List<ConsultationFileHistoryVM>();
        public string? ComplainantName { get; set; }
        public string? LastActionEn { get; set; }
        public string? LastActionAr { get; set; }
    }
    public class ConsultationFileListDmsVM
    {
        public Guid FileId { get; set; }
        public string? FileNumber { get; set; }
        public string? RequestTypeNameEn { get; set; }
        public string? RequestTypeNameAr { get; set; }
        public string? Subject { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public string? PriorityEn { get; set; }
        public string? PriorityAr { get; set; }
        public string? GovernmentEntityNameEn { get; set; }
        public string? GovernmentEntityNameAr { get; set; }
        public string? LawyerNameEn { get; set; }
        public string? LawyerNameAr { get; set; }
        public int? SectorTypeId { get; set; }
        //public string? Lawyer { get; set; }
        [NotMapped]
        public IList<ConsultationFileHistoryVM> LatestHistory { get; set; } = new List<ConsultationFileHistoryVM>();
    }
}