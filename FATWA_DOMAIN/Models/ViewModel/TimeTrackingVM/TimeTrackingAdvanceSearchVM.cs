using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.TimeTrackingVM
{
    public class TimeTrackingAdvanceSearchVM : GridPagination
    {
        //public string? ActivityName { get; set; }
        public Guid? ReferenceId { get; set; }
        public int SectortypeId { get; set; }
        public string? UserId { get; set; }
        public int ModuleId { get; set; } 
        public DateTime? AssignedOn { get; set; }
        public DateTime? CompletedOn { get; set; }
        public int? StatusId { get; set; }
        public string? UserName { get; set; }
    }
}
