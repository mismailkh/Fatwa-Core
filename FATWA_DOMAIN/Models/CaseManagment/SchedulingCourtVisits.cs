using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.CaseManagment
{

    //< History Author = 'Danish' Date = '2022-12-13' Version = "1.0" Branch = "master" >Schedulng Court Visits</History>
    [Table("CMS_COURT_VISIT")]
    public partial class SchedulingCourtVisits : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid HearingId { get; set; }       
        public string LawyerId { get; set; }
        public string ActionName { get; set; }
        public string PurposeName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
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

    }
}
