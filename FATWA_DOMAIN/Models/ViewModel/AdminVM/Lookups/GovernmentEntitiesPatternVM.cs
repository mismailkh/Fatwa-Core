using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups
{
    public class GovernmentEntitiesPatternVM
    {
        [Key]
        public int EntityId { get; set; }
        public int? SelectedPatternTypeId { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
        
    }
}
