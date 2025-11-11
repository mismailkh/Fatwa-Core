using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.ConsultationVMs
{
    public class ConsultationFileAssignmentVM : TransactionalBaseModel
    {
        public Guid Id { get; set; }
        public Guid ReferenceId { get; set; }
        public string LawyerId { get; set; }
        public string SupervisorId { get; set; }
        public bool IsPrimary { get; set; }
        public string? Remarks { get; set; }
        public string? LawyerNameEn { get; set; }
        public string? LawyerNameAr { get; set; }
        public string? SupervisorNameEn { get; set; }
        public string? SupervisorNameAr { get; set; }
    }
}
