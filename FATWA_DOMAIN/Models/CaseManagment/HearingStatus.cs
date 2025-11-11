using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{

    //< History Author = 'Hassan Abbas' Date = '2022-12-05' Version = "1.0" Branch = "master">Statuses of a Hearing</History>
    [Table("CMS_HEARING_STATUS_G2G_LKP")]
    public class HearingStatus
    {
        [Key]
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }
}
