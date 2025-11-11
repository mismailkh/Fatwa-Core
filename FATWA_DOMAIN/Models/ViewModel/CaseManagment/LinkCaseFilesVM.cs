using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{

    //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master"> Case File and Linked Files</History>
    public class LinkCaseFilesVM : TransactionalBaseModel
    {
        public Guid PrimaryFileId { get; set; }
        public string Reason { get; set; }
        public IList<Guid> LinkedFileIds { get; set; } = new List<Guid>();
        //<History Author = 'Hassan Abbas' Date='2023-03-03' Version="1.0" Branch="master"> Copy of Linked file attachment</History>
        [NotMapped]
        public List<CopyAttachmentVM>? CopyAttachmentVMs { get; set; } = new List<CopyAttachmentVM>();
    }
}
