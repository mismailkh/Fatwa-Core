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
    //< History Author = 'Hassan Abbas' Date = '2022-09-29' Version = "1.0" Branch = "master" >Sector Subtype Table for G2G Portal</History>
    [Table("CMS_SUBTYPE_G2G_LKP")]
    public class Subtype : TransactionalBaseModel   
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int RequestTypeId { get; set; }
        public string? Code { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
        public bool IsActive { get; set; }
    }
}
