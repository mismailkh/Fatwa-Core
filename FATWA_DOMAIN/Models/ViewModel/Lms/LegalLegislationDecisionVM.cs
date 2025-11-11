using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.Lms
{
    public class LegalLegislationDecisionVM
    {
        [Key]
        public Guid LegislationId { get; set; }
        public string? Legislation_Number { get; set; }
        public string? Introduction { get; set; }
        public string? Legislation_TitleEn { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? IssueDate_Hijri { get; set; }
        public string? Legislation_Type_Ar { get; set; }
        public string? Legislation_Comment { get; set; } 
        public string? AddedBy { get; set; }
        public string? Legislation_Type_En { get; set; }
        public string? Legislation_Status_Ar { get; set; }
        public string? Legislation_Status_En { get; set; }
        public int? StatusId { get; set; }
        public string? LegislationFlowStatusAr { get; set; }
        public string? LegislationFlowStatusEn { get; set; }
        public int? FlowStatusId { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
        [NotMapped]
        public string? ModifiedBy { get; set; }

    }
}
