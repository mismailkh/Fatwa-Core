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
    [Table("CMS_LAWYER_SUPERVISOR")]
    public class CmsLawyerSupervisor : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public string LawyerId { get; set; }
        public string SupervisorId { get; set; }
        public string? ManagerId { get; set; }
    }
}
