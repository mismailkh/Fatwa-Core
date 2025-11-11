using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups
{
    public class GovernmentEntitiesSectorsVM
    {
        [Key]
        public int Id { get; set; }
        public int EntityId { get; set; }
        public int? G2GBRSiteID { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
        public string? GovernmentEntityEn { get; set; }
        public string? GovernmentEntityAr { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public string UserFullNameEn { get; set; }
        public string UserFullNameAr { get; set; }
        public string? DeptProfession { get; set; }

    }
}
