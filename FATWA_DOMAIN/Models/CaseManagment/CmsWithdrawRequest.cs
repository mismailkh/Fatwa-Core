using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//< History Author = 'Muhammad Zaeem' Date = '2022-10-17' Version = "1.0" Branch = "master" >Created Withdraw case Request Model</History>

namespace FATWA_DOMAIN.Models.AdminModels.CaseManagment
{
    [Table("CMS_WITHDRAW_REQUEST")]
    public class CmsWithdrawRequest : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CaseRequestId { get; set; }
        public int RequestNumber { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public string? Reason { get; set; }
        public string? RejectionReason { get; set; }
        public int? StatusId { get; set; }
        [NotMapped]
        public UploadedDocument? UploadedDocument { get; set; }
        [NotMapped]
        public int SectorTypeId { get; set; }
        [NotMapped]
        public int GovtEntityId { get; set; }
        [NotMapped]
        public int DepartmentId { get; set; }

    }
}
