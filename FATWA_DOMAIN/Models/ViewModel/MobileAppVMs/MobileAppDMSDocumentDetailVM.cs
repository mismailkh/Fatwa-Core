using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MobileAppVMs
{
    public class MobileAppDMSDocumentDetailVM
    {
        [DisplayName("Document_Number")]
        public int DocumentNumber { get; set; }
        [DisplayName("Document_Name")]
        public string DocumentName { get; set; }
        [DisplayName("Document_Description")]
        public string? Description { get; set; }
        [DisplayName("Is_Confidential")]
        public bool IsConfidential { get; set; }
        [DisplayName("Version_Number")]
        public decimal DocumentVersionNumber { get; set; }

    }
}
