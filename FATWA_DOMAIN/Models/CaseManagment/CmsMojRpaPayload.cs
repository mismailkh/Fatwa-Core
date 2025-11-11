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
	//< History Author = 'Hassan Abbas' Date = '2022-09-29' Version = "1.0" Branch = "master" >For Logging RPA Payload</History>
	[Table("CMS_MOJ_RPA_PAYLOAD")]
	public class CmsMojRpaPayload : TransactionalBaseModel
	{
		[Key]
		public Guid Id { get; set; }
		public string Payload { get; set; }
	}
}
