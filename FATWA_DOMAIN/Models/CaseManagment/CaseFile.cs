using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-09-29' Version = "1.0" Branch = "master" >
    //      -> Case File
    //</History>
    [Table("CMS_CASE_FILE")]
    public partial class CaseFile : TransactionalBaseModel
    {
        [Key]
        public Guid FileId { get; set; }
        public Guid RequestId { get; set; }
        public string FileNumber { get; set; }
        public int ShortNumber { get; set; }
        public string FileName { get; set; }
        public int StatusId { get; set; }
        public int? TransferStatusId { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsLinked { get; set; }
        public bool? IsAssignedBack { get; set; }
        public string CaseFileNumberFormat { get; set; }
        public string PatternSequenceResult { get; set; }
        [NotMapped]
        public IList<CasePartyLinkVM> CasePartyLinks { get; set; } = new List<CasePartyLinkVM>();
        [NotMapped]
        public int? SectorTypeId { get; set; } 
        [NotMapped]
        public int? SectorTo { get; set; }
        [NotMapped]
        public int? SectorFrom { get; set; }
        [NotMapped]
        public string? Remarks { get; set; }
        [NotMapped]
        public bool? IsAssignment { get; set; }
        // for fatwa to G2G Communication
        [NotMapped]
        public IList<CasePartyLink> PartyLinks { get; set; } = new List<CasePartyLink>();
        [NotMapped]
        public IList<UploadedDocumentVM> UploadedDocuments { get; set; } = new List<UploadedDocumentVM>();

        [NotMapped]
        public List<CopyAttachmentVM>? CopyAttachmentVMs { get; set; } = new List<CopyAttachmentVM>();
        [NotMapped]
        public string LoggedInUserId { get; set; }
    }
}
