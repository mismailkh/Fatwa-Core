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
    //< History Author = 'Hassan Abbas' Date = '2022-10-20' Version = "1.0" Branch = "master" > Registered Case Model</History>
    [Table("CMS_EXECUTION_ANOUNCEMENT")]
    public partial class ExecutionAnouncement : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ExecutionId { get; set; }
        public int AnouncementStatusId { get; set; }
        public int AnouncementTypeId { get; set; }
        public string? PersonToBeanounced { get; set; }
        public DateTime? ProcedureDate { get; set; }
    }
}
