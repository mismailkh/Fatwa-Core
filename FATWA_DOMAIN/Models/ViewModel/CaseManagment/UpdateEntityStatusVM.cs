using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.Consultation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public class UpdateEntityStatusVM
    {
        [Key]
        public Guid ReferenceId { get; set; }
        public int StatusId { get; set; }
        public int SubModuleId { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? Reason { get; set; }
        [NotMapped]
        public ComsWithdrawRequest? ComsWithdrawRequests { get; set; }
    }

    public class UpdateEntityHistoryVM : TransactionalBaseModel
    {
        [Key]
        public Guid HistoryId { get; set; }
        public Guid ReferenceId { get; set; }
        public int EventId { get; set; }
        public string? Remarks { get; set; }
        public int StatusId { get; set; }
        public int SubModuleId { get; set; }

    }
}
