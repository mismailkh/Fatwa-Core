using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups
{
    public class AttachmentTypeVM
    {
        [Key]
        public int AttachmentTypeId { get; set; }
        public string Type_Ar { get; set; }
        public string Type_En { get; set; }
        public int? ModuleId { get; set; }
        public int? SubTypeId { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsOfficialLetter { get; set; }
        public bool IsGePortalType { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsSystemDefine { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string? Description { get; set; }
        public string UserFullNameEn { get; set; }
        public string UserFullNameAr { get; set; }
        public string? SigningMethodNameEN { get; set; }
        public string? SigningMethodNameAr { get; set; }
        public string? DesignationNameEn { get; set; }
        public string? DesignationNameAr { get; set; }



    }
}
