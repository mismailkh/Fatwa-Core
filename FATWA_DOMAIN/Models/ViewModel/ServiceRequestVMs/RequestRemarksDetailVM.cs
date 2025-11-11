using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.ServiceRequestVMs
{
    public class RequestRemarksDetailVM
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ReferenceId { get; set; }
        public string RemarkContent { get; set; } = null!;
        public int TypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string RemarkByEn { get; set; } = null!;
        public string RemarkByAr { get; set; } = null!;
    }
}
