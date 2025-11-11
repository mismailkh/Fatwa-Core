using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.PACIVM
{
   
    public class PACIRequestStatusHistoryVM
    {
        [Key]
        public Guid? id { get; set; }
        public Guid? RequestId { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateOn { get; set; }
    }
}