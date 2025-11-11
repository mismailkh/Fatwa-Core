using FATWA_DOMAIN.Models.TaskModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.TaskVMs
{
    public class TaskResponseStatusVM 
    {
        [Key]
        public Guid TaskResponseId { get; set; }
        public string Reason { get; set; }
        public int TaskResponeStatusId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? TaskId { get; set; }
        public string? TaskStatusEn { get; set; }
        public string? TaskStatusAr { get; set; }
    }
}
