using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

//< History Author = 'Muhammad Zaeem' Date = '2022-11-28' Version = "1.0" Branch = "master" >Created Case Request Model</History>
//< History Author = 'Hassan Abbas' Date = '2022-11-15' Version = "1.0" Branch = "master" >Modified Model from Case Request Assignment to Case File Assignment</History>

namespace FATWA_DOMAIN.Models.Consultation
{
    [Table("COMS_CONSULTATION_ASSIGNMENT")]
    public class ConsultationAssignment : TransactionalBaseModel
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
        public Guid? ConsultationRequestId { get; set; }
        [NotMapped]
        public int? AssignConsultationLawyerType { get; set; } 
        [NotMapped]
        public int? SectorTypeId { get; set; }
        [NotMapped]
        public int? FatwaPriorityId { get; set; } = 0;
        [NotMapped]
        public List<CopyAttachmentVM>? CopyAttachmentVMs { get; set; } = new List<CopyAttachmentVM>();
        [NotMapped]
        public Guid? FileId { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter(); 
    }
}