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
    //      -> Court
    //      -> Values will be managed from FATWA-PORTAL
    //</History>
    [Table("CMS_COURT_G2G_LKP")]
    public partial class Court : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
         public string Name_En { get; set; }
        public string Name_Ar { get; set; }
        public string District { get; set; }
        public string Location { get; set; }
        public int TypeId { get; set; }
        public bool IsActive { get; set; }
        public string CourtCode { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }

    }
}
