using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{    //<History Author = 'Danish' Date='2022-12-26' Version="1.0" Branch="master">SchedulingCourtVisitVM</History>   
    public class SchedulingCourtVisitVM : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid HearingId { get; set; }
        public string LawyerId { get; set; }
        public string ActionName { get; set; }
        public string PurposeName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsUrgent { get; set; }
        public bool IsReccuring { get; set; }
        public int? VisitTypeId { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? VisitDate { get; set; }
        public string? Other { get; set; }
        public string? Duration { get; set; }
        public string? Notes { get; set; }
        public string? LawyerNameEn { get; set; }
        public string? LawyerNameAr { get; set; }
    }
}