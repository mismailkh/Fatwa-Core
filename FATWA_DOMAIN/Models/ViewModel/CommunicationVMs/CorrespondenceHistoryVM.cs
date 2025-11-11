using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CommunicationVMs
{
    public class CorrespondenceHistoryVM
    {
        [Key]
        public Guid HistoryId { get; set; }
        public string? SenderNameEn { get; set; }
        public string? RecieverNameEn { get; set; }
        public string? SenderNameAr { get; set; }
        public string? RecieverNameAr { get; set; }
        public string? ActionEn { get; set; }
        public string? ActionAr { get; set; }
        public string? Reason { get; set; }
        public DateTime? Date { get; set; }
    }
}
