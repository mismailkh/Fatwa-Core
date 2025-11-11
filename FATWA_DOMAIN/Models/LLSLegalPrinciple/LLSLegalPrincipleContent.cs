using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.LLSLegalPrinciple
{
    [Table("LLS_LEGAL_PRINCIPLE_CONTENT")]
    public class LLSLegalPrincipleContent : TransactionalBaseModel
    {
        [Key]
        public Guid PrincipleContentId { get; set; }
        public Guid PrincipleId { get; set; }
        public string PrincipleContent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [NotMapped]
        public string BackgroundColor { get; set; } = "";
        [NotMapped]
        public int? MainPrincipleFlowStatusId { get; set; }
    }
}
