using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-09-29' Version = "1.0" Branch = "master" > Case File Detail VM </History>
    public class CmsCaseFileDetailVM : TransactionalBaseModel
    {
        [Key]
        public Guid FileId { get; set; }
        public Guid RequestId { get; set; }
        public string FileNumber { get; set; }
        public string FileName { get; set; }
        public int StatusId { get; set; }
        public string StatusNameEn { get; set; }
        public string StatusNameAr { get; set; }
        public int? TransferStatusId { get; set; }
        public bool? IsAssignedBack { get; set; }
        public bool IsAssigned { get; set; }
        [NotMapped]
        public string? TaskDescription { get; set; }
        [NotMapped]
        public int? SectorTypeId { get; set; }
        [NotMapped]
        public string? Remarks { get; set; }
        [NotMapped]
        public IList<CaseRequestDetailVM>?  CaseRequest { get; set; } = new List<CaseRequestDetailVM>();
        [NotMapped]
        public IList<CasePartyLinkVM>? CasePartyLinks { get; set; } = new List<CasePartyLinkVM>();
        [NotMapped]
        public string LawyerId { get; set; }
        [NotMapped]
        public bool IsCaseRegistered { get; set; }
        [NotMapped]
        public bool ShowClaimStatement { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
        public bool IsImportant { get; set; }   
    }
}
