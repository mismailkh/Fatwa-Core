using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups
{
    public class GovernmentEntitiesRepresentativeVM
    {
        [Key]
        public Guid id { get; set; }
        public int GovtEntityId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string RepresentativeCode { get; set; }
        public string Representative_Designation_EN { get; set; }
        public string Representative_Designation_AR { get; set; }
        public string GovernmentEntityEn { get; set; }
        public string GovernmentEntityAr { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string UserFullNameEn { get; set; }
        public string UserFullNameAr { get; set; }

    }
}
