using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    [Table("CMS_CASE_USER_IMPORTANT")]
    public class CaseUserImportant : TransactionalBaseModel
	{
		[Id]
		public Guid Id { get; set; }
		public Guid ReferenceId { get; set; }
		public Guid UserId  { get; set; }
	}
}
