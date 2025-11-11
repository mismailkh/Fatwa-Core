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
    //< History Author = 'Muhammad Abuzar' Date = '2023-11-29' Version = "1.0" Branch = "master">Chamber Numbers</History>
    [Table("CMS_CHAMBER_NUMBER_G2G_LKP")]
    public partial class ChamberNumber : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Number { get; set; }
       // public int ChamberId { get; set; }
        public string? Code { get; set; }
        public bool IsActive { get; set; }
        public int? ShiftId { get; set; }
		[NotMapped]
        public IEnumerable<int>? ChamberIds { get; set;}= new List<int>();
	
	}
}
