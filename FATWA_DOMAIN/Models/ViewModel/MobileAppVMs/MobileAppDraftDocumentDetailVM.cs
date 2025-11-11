using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MobileAppVMs
{
    public class MobileAppDraftDocumentDetailVM
    {
        [DisplayName("Draft_Number")]
        public int? DraftNumber { get; set; }
        [DisplayName("Draft_Name")]
        public string? Name { get; set; }
        [DisplayName("Version_Number")]
        public decimal VersionNumber { get; set; }
        [DisplayName("Doc_Type")]
        public string? AttachementType { get; set; }
        [DisplayName("Status")]
        public string? Status { get; set; }
        [DisplayName("Description")]
        public string? Description { get; set; }
        [DisplayName("Template_Name")]
        public string? TemplateName { get; set; }
        [DisplayName("File_Name")]
        public string? FileName { get; set; }
        [DisplayName("Subject")]
        public string? Subject { get; set; }
        [DisplayName("Created_By")]
        public string? CreatedBy { get; set; }
        [DisplayName("Created_Datetime")]
        public DateTime CreatedDate { get; set; }
    }
}
