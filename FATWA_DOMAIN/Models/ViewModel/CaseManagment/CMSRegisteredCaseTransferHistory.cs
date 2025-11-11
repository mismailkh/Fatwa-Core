using FATWA_DOMAIN.Models.BaseModels;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    [Table("CMS_RESGISTERED_CASE_TRANSFER_HISTORY")]
    public class CMSRegisteredCaseTransferHistory: TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public int ChamberFromId { get; set; }
        public int ChamberToId { get; set; }
        public int ChamberNumberFromId { get; set; }
        public int ChamberNumberToId { get; set; }
        public Guid OutcomeId { get; set; }
    }
    public class CMSRegisteredCaseTransferHistoryVM
    {
        public int ChamberId { get; set; }
        public int ChamberNumberId { get; set; }
        public Guid CaseId { get; set; }
        public int SelectedChamberNumber { get; set; }
        public int ChamberFromId { get; set; }
        public int ChamberToId { get; set; }
        public int ChamberNumberFromId { get; set; }
        public int ChamberNumberToId { get; set; }
        public Guid OutcomeId { get; set; }
        public string createdBy { get; set; }
    }
}
