using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    //<History Author = 'Danish' Date='2022-01-03' Version="1.0" Branch="master">CmsJudgmentExecutionVM</History>
    public class CmsJudgmentExecutionVM 
    {
        [Key]
        public Guid Id { get; set; }
        public string? ExecutorNumber { get; set; }
        public string? ExecutionFileNumber { get; set; }        
        public DateTime? FileOpeningDate { get; set; }
        public DateTime? FileAttachDate { get; set; }
        public int? FileStatusId { get; set; }
        public string? FileStatusEn { get; set; }
        public string? FileStatusAr { get; set; }
        public string? PayerName { get; set; }
        public string? ReceiverName { get; set; }
        public string? GovtEntityPayer_En { get; set; }
        public string? GovtEntityPayer_Ar { get; set; }
        public string? GovtEntityReceiver_En { get; set; }
        public string? GovtEntityReceiver_Ar { get; set; }
        public decimal? FileBalance { get; set; }
        public decimal? Amount { get; set; }
        public decimal? PaidAmount { get; set; }
        public string? AttachedFileNo { get; set; }
        public string? AttachedFileSection { get; set; }
        public string? Remarks { get; set; }
        [NotMapped]
        public int? AttachmentCount { get; set; }
        public int? PayerTypeId { get; set; }
		public int? ReceiverTypeId { get; set; }

		[NotMapped]
		public CasePartyTypeEnum CasePartyType { get; set; }
	}

    //<History Author = 'Muhammad Abuzar' Date='2023-12-06' Version="1.0" Branch="master">Execution Detail VM</History>
    public class CmsJudgmentExecutionDetailVM
    {
        [Key]
        public Guid Id { get; set; }
        public string? ExecutionFileNumber { get; set; }
        public DateTime? FileOpeningDate { get; set; }
        public int? FileStatusId { get; set; }
        public string? FileStatusEn { get; set; }
        public string? FileStatusAr { get; set; }
        public string? PayerName { get; set; }
        public string? ReceiverName { get; set; }
        public string? GovtEntityPayer_En { get; set; }
        public string? GovtEntityPayer_Ar { get; set; }
        public string? GovtEntityReceiver_En { get; set; }
        public string? GovtEntityReceiver_Ar { get; set; }
        public decimal? FileBalance { get; set; }
        public decimal? Amount { get; set; }
        public decimal? PaidAmount { get; set; }
        public string? Remarks { get; set; }
        public int? PayerTypeId { get; set; }
        public int? ReceiverTypeId { get; set; }

        [NotMapped]
        public CasePartyTypeEnum CasePartyType { get; set; }
    }

}
