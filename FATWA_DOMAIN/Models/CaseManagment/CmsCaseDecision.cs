using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Muhammad Zaeem' Date = '2022-03-30' Version = "1.0" Branch = "master" >Case Decision</History>
   
    [Table("CMS_CASE_DECISION")]
    public class CmsCaseDecision : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public int DecisionTypeId { get; set; }
        public Guid CaseId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ReferenceNo { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public bool? isExecutionLost { get; set; }
        public int StatusId { get; set; }
        [NotMapped]
        public UploadedDocument? UploadedDocument { get; set; }
        [NotMapped]
        public int? SectorTypeId { get; set; }

    }
}
