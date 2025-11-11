using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class LegalLegislationsVM : GridMetadata
    {

        [Key]
        public Guid LegislationId { get; set; }
        public int Legislation_Type { get; set; }
        public string? Legislation_Number { get; set; }
        public string? Introduction { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? IssueDate_Hijri { get; set; }
        public string? LegislationTitle { get; set; }
        public int Legislation_Status { get; set; }
        public int Legislation_Flow_Status { get; set; }
        public DateTime? StartDate { get; set; }
        public string? AddedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? Legislation_Type_Ar { get; set; }
        public string? Legislation_Type_En { get; set; }
        public string? Legislation_Status_Ar { get; set; }
        public string? Legislation_Status_En { get; set; }
        public string? Legislation_Flow_Status_Ar { get; set; }
        public string? Legislation_Flow_Status_En { get; set; }
        [NotMapped]
        public bool IsDeleted { get; set; } 
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
    }
    public class LegalLegislationsDmsVM
    {
        [Key]
        public Guid LegislationId { get; set; }
        public int Legislation_Type { get; set; }
        public string? Legislation_Number { get; set; }
        public string? Introduction { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? IssueDate_Hijri { get; set; }
        public string? LegislationTitle { get; set; }
        public int Legislation_Status { get; set; }
        public int Legislation_Flow_Status { get; set; }
        public DateTime? StartDate { get; set; }
        public string? AddedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? Legislation_Type_Ar { get; set; }
        public string? Legislation_Type_En { get; set; }
        public string? Legislation_Status_Ar { get; set; }
        public string? Legislation_Status_En { get; set; }
        public string? Legislation_Flow_Status_Ar { get; set; }
        public string? Legislation_Flow_Status_En { get; set; }
        [NotMapped]
        public bool IsDeleted { get; set; }
    }

    public class AdvanceSearchLegalLegislationsVM : GridPagination
    {
        public string? Legislation_Number { get; set; }
        public int Legislation_Type { get; set; }
        public string? Introduction { get; set; }
        public DateTime? Start_From { get; set; }
        public DateTime? End_To { get; set; }
        public string? LegislationTitle { get; set; }
        public int Legislation_Status { get; set; }
        public int Legislation_FlowStatus { get; set; }
        public string? Legislation_Type_Ar { get; set; }
        public string? Legislation_Type_En { get; set; }
        public string? UserId { get; set; }
    }
}
