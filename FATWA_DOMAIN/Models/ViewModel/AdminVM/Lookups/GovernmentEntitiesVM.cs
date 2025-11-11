using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups
{
    public class GovernmentEntitiesVM
    {
        [Key]
        public int EntityId { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
        public int? G2GSiteId { get; set; }
        public bool? IsConfidential { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public string GECode { get; set; }
        public string UserFullNameEn { get; set; }
        public string UserFullNameAr { get; set; }

    }
}
