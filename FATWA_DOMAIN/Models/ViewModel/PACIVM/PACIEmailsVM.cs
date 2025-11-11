using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.PACIVM
{
    public class PACIEmails
    {
        [Key]
        public int id { get; set; }
        public string To { get; set; }
        public string? From { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }

        public string? Attachment { get; set; }
        public string? EmailType { get; set; }
        public DateTime? EmailDatetime { get; set; }
        public Guid? RequestsId { get; set; }
    }
}
