using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-12-05' Version = "1.0" Branch = "master">View Model for Outcomes</History>
    public class OutcomeHearingVM : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid HearingId { get; set; }
        public DateTime HearingDate { get; set; }
        public DateTime? NextHearingDate { get; set; }
        public TimeSpan HearingTime { get; set; }
        public string LawyerId { get; set; }
        public string Remarks { get; set; }
        public string LawyerNameEn { get; set; }
        public string LawyerNameAr { get; set; }
    }

    public class OutcomeHearingDetailVM : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid HearingId { get; set; }
        public DateTime HearingDate { get; set; }
        public DateTime? NextHearingDate { get; set; } 
        public TimeSpan HearingTime { get; set; }
        public string LawyerId { get; set; }
        public string Remarks { get; set; }
        public string LawyerNameEn { get; set; }
        public string LawyerNameAr { get; set; }
    }
}
