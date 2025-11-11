using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CommunicationVMs
{
    public class OutboxInfoVM
    {
        [Key]
        public Guid RequestId { get; set; }
        public Guid CommunicationId { get; set; }
        public string OutboxNumber { get; set; }
        public DateTime? OutboxDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
