using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.DmsVMs
{
    public class DMSDocumentDetailVM : TransactionalBaseModel
    {
        public int? UploadedDocumentId { get; set; }
        public string? FileName { get; set; }
        public string? FileNumber { get; set; }
        public int? AttachmentTypeId { get; set; }
        public string? AttachmentTypeEn { get; set; }
        public string? AttachmentTypeAr { get; set; }
         public string? Description { get; set; }
        public string? StoragePath { get; set; }
        public string? DocType { get; set; }
        public bool? IsConfidential { get; set; }



    }
}
