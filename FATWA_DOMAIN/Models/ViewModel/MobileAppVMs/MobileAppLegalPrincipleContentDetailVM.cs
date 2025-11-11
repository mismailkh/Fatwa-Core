using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.MobileAppVMs
{
    public class MobileAppLegalPrincipleContentDetailVM
    {
        public int PrincipleNumber { get; set; }
        [Key]
        public Guid PrincipleContentId { get; set; }
        public Guid PrincipleId { get; set; }
        public string PrincipleContent { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } 
        public string FlowStatusNameEn { get; set; }
        public string FlowStatusNameAr { get; set; }
        [NotMapped]
        public List<MobileAppUploadDocumentsVM> Attachments { get; set; } = new List<MobileAppUploadDocumentsVM>();
    }
}
