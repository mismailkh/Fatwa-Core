using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.DmsVMs
{
    public class DMSTemplateListVM : TransactionalBaseModel
    {
        public int TemplateId { get; set; }
        public string? NameEn { get; set; }
        public string? NameAr { get; set; }
        public string? Content { get; set; }
        public bool isActive { get; set; }
        public int? AttachmentTypeId { get; set; }
        public string? AttachmentTypeEn { get; set; }
        public string? AttachmentTypeAr { get; set; }
        public int? ModuleId { get; set; }
        public int? SubTypeId { get; set; }
    }
}
