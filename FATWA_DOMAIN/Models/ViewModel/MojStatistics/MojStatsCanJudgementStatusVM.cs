using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MojStatistics
{
    public class MojStatsCanJudgementStatusVM
    {
        public int Id { get; set; }
        public int? CANid { get; set; }
        public int? JudgementStatusId { get; set; }
        public string? JudgementAmount { get; set; }
        public string? JudgementAmountCollected { get; set; }
        public bool? OpenExecution { get; set; }
        public string? Remarks { get; set; }
        public int? ExecutionFilelevelId { get; set; }
        public int? RaisedById { get; set; }
    }
}
