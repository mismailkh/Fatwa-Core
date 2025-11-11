using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Dms
{
	[Table("DMS_DOCUMENT_CLASSIFICATION_LKP")]
	//<History Author = 'Hassan Abbas' Date='2023-06-15' Version="1.0" Branch="master"> Document Classification Model</History>
	public partial class DmsDocumentClassification
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string NameEn { get; set; }
		public string NameAr { get; set; }
	}
}
