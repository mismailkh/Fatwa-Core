using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{

    //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master"> Model fors stroing Linking information of Case Files</History>
    [Table("CMS_CASE_FILE_LINKED_FILE")]
    public class CmsCaseFileLinkedFile : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PrimaryFileId { get; set; }
        public Guid LinkedFileId { get; set; }
        public string Reason { get; set; }
    }
}
