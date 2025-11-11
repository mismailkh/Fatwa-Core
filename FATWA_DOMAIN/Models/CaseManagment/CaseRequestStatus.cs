using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-09-29' Version = "1.0" Branch = "master" >
    //      -> Case Request Status
    //      -> Values will be managed from FATWA-PORTAL
    //      -> Values should also be synced with CaseRequestStatusEnum
    //</History>
    [Table("CMS_CASE_REQUEST_STATUS_G2G_LKP")]
    public class CaseRequestStatus :TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
          public bool IsActive { get; set; }
    }
}
