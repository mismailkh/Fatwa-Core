using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-12-05' Version = "1.0" Branch = "master">View Model for Hearings</History>
    public class HearingVM : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CaseId { get; set; }
        public DateTime HearingDate { get; set; }
        public TimeSpan HearingTime { get; set; }
        public int StatusId { get; set; }
        public string Description { get; set; }
        public string StatusEn { get; set; }
        public string StatusAr { get; set; }
		public string FirstNameEn { get; set; }
		public string FirstNameAr { get; set; }
		[NotMapped]
        public IList<SchedulingCourtVisitVM>? ScheduleCourtVisit { get; set; } = new List<SchedulingCourtVisitVM>();
    }
    public class HearingDetailVM : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CaseId { get; set; }
        public DateTime HearingDate { get; set; }
        public TimeSpan HearingTime { get; set; }
        public int StatusId { get; set; }
        public string? Description { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public string? LawyerId { get; set; }
        public string? LawyerNameEn { get; set; }
        public string? LawyerNameAr { get; set; }
        public string? PostponeReason { get; set; }
        public string? PortfolioStoragePath { get; set; }
    }
}
