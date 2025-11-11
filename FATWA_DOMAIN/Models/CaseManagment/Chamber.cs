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
    //      -> Chamber
    //      -> Values will be managed from FATWA-PORTAL
    //</History>
    [Table("CMS_CHAMBER_G2G_LKP")]
    public partial class Chamber : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Number { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
        public string? Address { get; set; }
        //public int CourtId { get; set; }
        public bool IsActive { get; set; }
        public string ChamberCode { get; set; }
        public string? Description { get; set; } 
        [NotMapped]
        public IEnumerable<int> SelectedCourtIds { get; set; } = new List<int>();
    }
}
