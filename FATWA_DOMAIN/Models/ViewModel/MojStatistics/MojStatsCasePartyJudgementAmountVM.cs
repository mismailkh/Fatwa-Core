using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MojStatistics
{
    public class MojStatsCasePartyJudgementAmountVM
    {
        public int Id { get; set; }
        public int? CANJudgementStatusId { get; set; }
        public int? PartyId { get; set; }
        public decimal? AmountPaid { get; set; }
        public decimal? AmountReceived { get; set; }
        public decimal? LawyerExpense { get; set; }
        public bool? IsDeleted { get; set; }
        public string? CANid { get; set; }
    }
}
