using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.CaseManagment
{

    //< History Author = 'Hassan Abbas' Date = '2022-12-22' Version = "1.0" Branch = "master" >
    //      -> Request Type Table for FTW Portal
    //      -> Values should also be synced with RequestTypeEnum
    //</History>
    [Table("CMS_REQUEST_TYPE_G2G_LKP")]
    public class RequestType: TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Code { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
        public bool IsActive { get; set; }
    }
}
