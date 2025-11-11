using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    [Table("CMS_EXECUTION_STATUS_G2G_LKP")]
    public class CmsExecutionFileStatus : TransactionalBaseModel
    {
        [Key]
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public bool IsActive { get; set; }

    }
}
