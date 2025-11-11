using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.WorkflowModels
{
    public class AttachmentTypeListVM
    {
        [Key]
        public int? Id { get; set; }
        public int? WorkflowId { get; set; }
        public int? AttachmentTypeId{ get; set; }
        public bool? IsActiveFlow{ get; set; }
        public string? Type_Ar{ get; set; }
        public string? Type_En{ get; set; }
    }
}
