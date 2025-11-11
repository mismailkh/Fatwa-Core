using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.TimeTrackingVM
{
    public class TimeTrackingVM : GridMetadata
    {
        //[Key] 
        //public Guid TimeTrackingId { get; set; }
        public string ActivityName { get; set; }
        public string AssignedBy { get; set; }
        public string AssignedTo { get; set; }
        public DateTime? AssignedOn { get; set; }
        public DateTime? CompleteOn { get; set; }
        public int Duration { get; set; }
        public string StatusEn { get; set; }
        public string StatusAr { get; set; }
        public string AssignedToDepartmentNameEn { get; set; }
        public string AssignedToDepartmentNameAr { get; set; }
        public string AssignedByDepartmentNameEn { get; set; }
        public string AssignedByDepartmentNameAr { get; set; }
        public string AssignedByEn { get; set; }
        public string AssignedByAr { get; set; }
        public string AssignedToAr { get; set; }
        public string AssignedToEn { get; set; }
        public string? GovtEntityNameEn { get; set; }
        public string? GovtEntityNameAr { get; set; }
        public int? EntityId { get; set; }
        public string? DepartmentName_En { get; set; }
        public string? DepartmentName_Ar { get; set; }
        [NotMapped]
        public Guid? ReferenceId { get; set; }
    }
}
