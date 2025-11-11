using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.DmsVMs
{
    public class DsAttachmentTypeDetailVM
    {
        [Key]
        public int AttachmentTypeId { get; set; }
        public int? DesignationId { get; set; }  
        public string? DesignationNameEn { get; set; }
        public string? DesignationNameAr { get; set; }
        public int? MethodId { get; set; }
        public string? SigningMethodNameEn { get; set; }
        public string? SigningMethodNameAr { get; set; }
    }
}
