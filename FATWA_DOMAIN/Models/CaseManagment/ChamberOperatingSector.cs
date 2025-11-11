using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2024-02-21' Version = "1.0" Branch = "master" > Chamber and Sector Linking</History>
    [Table("CMS_CHAMBER_OPERATING_SECTOR")]
    public partial class ChamberOperatingSector : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ChamberId { get; set; }
        public int SectorTypeId { get; set; }
        [NotMapped]
        public List<int> SelectedChamberIds { get; set; } = new List<int>();
    }
}
