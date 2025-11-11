using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Consultation
{

    //< History Author = 'Muhammad Zaeem' Date = '2023-1-9' Version = "1.0" Branch = "master" >Tracking Approvals for Case Management -> Transfer, Assignment, Copy etc</History>
    [Table("COMS_APPROVAL_TRACKING")]
    public class ComsApprovalTracking : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ReferenceId { get; set; }
        public int SectorTo { get; set; }
        public int SectorFrom { get; set; }
        public int StatusId { get; set; }
        public int ProcessTypeId { get; set; }
        public string? Remarks { get; set; }
    }
}
