using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Muhammad Zaeem' Date = '2022-10-17' Version = "1.0" Branch = "master" >
    //      -> Withdraw Request Status
    //</History>
    [Table("CMS_WITHDRAW_REQUEST_STATUS_G2G_LKP")]
    public class CmsWithdrawRequestStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
    }
}
