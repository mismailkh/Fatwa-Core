using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.DmsVMs
{
    public class KayDocumentListAdvanceSearchVM : GridPagination
    {
        public string? EditionNumber { get; set; }
        public string? EditionType { get; set; }
        public string? DocumentTitle { get; set; }
        public string? PublicationDateHijri { get; set; }
        public DateTime? PublicationFrom { get; set; }
        public DateTime? PublicationTo { get; set; }
        public bool? IsFullEdition { get; set; } = false;
        [NotMapped]
        public bool FromLegalLegislationForm { get; set; } = false;
    }
}
