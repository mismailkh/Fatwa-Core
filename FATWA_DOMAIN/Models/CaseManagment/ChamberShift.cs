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
	[Table("CMS_CHAMBER_SHIFT_G2G_LKP")]
	public partial class ChamberShift : TransactionalBaseModel
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string NameEn { get; set; }
		public string NameAr { get; set; }
		public bool IsActive { get; set; }
	}
}
