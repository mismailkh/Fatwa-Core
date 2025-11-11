using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    
    [Table("CMS_DECISION_REQUEST_ASSIGNEE")]
    public class CmsCaseDecisionAssignee : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid DecisionId { get; set; }
        public Guid UserId { get; set; }

        [NotMapped]
        public IList<LawyerVM>? SelectedUsers { get; set; } = new List<LawyerVM>();
    }
}
