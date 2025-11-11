using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//< History Author = 'Muhammad Zaeem' Date = '2022-11-28' Version = "1.0" Branch = "master" >Created Case Request Model</History>
//< History Author = 'Hassan Abbas' Date = '2022-11-15' Version = "1.0" Branch = "master" >Modified Model from Case Request Assignment to Case File Assignment</History>

namespace FATWA_DOMAIN.Models.CaseManagment
{
    [Table("CMS_CASE_ASSIGNMENT")]
    public class CaseAssignment : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ReferenceId { get; set; }
        public string? LawyerId { get; set; }
        public string? SupervisorId { get; set; }
        public string? Remarks { get; set; }
        public bool IsPrimary { get; set; }
        [NotMapped]
        public IList<LawyerVM>? SelectedUsers { get; set; } = new List<LawyerVM>();
        [NotMapped]
        public string? PrimaryLawyerId { get; set; }
        [NotMapped]
        public Guid? RequestId { get; set; }
        [NotMapped]
        public int? AssignCaseToLawyerType { get; set; } 
        [NotMapped]
        public int? SectorTypeId { get; set; }
        [NotMapped]
        public Guid? FileId { get; set; } 
        [NotMapped]
        public List<CopyAttachmentVM>? CopyAttachmentVMs { get; set; } = new List<CopyAttachmentVM>();
        [NotMapped]
        public string TaskUserId { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
    }

   
}