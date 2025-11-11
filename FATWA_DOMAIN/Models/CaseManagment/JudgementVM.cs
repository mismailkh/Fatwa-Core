using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-12-05' Version = "1.0" Branch = "master">View Model for Judgements</History>
    public class JudgementVM : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? OutcomeId { get; set; }
        public Guid? CaseId { get; set; }
        public DateTime? JudgementDate { get; set; }
        public Decimal? Amount { get; set; }
        public Decimal? AmountCollected { get; set; }
        public DateTime? HearingDate { get; set; }
        public bool IsFinal { get; set; }
        public int? TypeId { get; set; }
        public string? TypeEn { get; set; }
        public string? TypeAr { get; set; }
        public int? StatusId { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryEn { get; set; }
        public string? CategoryAr { get; set; }
        public int? ExecutionFileLevelId { get; set; }
        public string? SerialNumber { get; set; } = "";
        public bool? OpenExecutionFile { get; set; }
        public string? Remarks { get; set; }
        [NotMapped]
        public bool IsUpdated { get; set; }
        [NotMapped]
        public string ExecutionFileLevelEn { get; set; }
        [NotMapped]
        public string ExecutionFileLevelAr { get; set; }
         [NotMapped]
        public int SectorTypeId { get; set; }
        [NotMapped]
        public IList<TempAttachementVM> SelectedDocuments = new List<TempAttachementVM>();
        [NotMapped]
        public MojExecutionRequest mojExecutionRequest { get; set; }
        [NotMapped]
        public DateTime? CreatedDateTime { get; set; }
    }
    //< History Author = 'Muhammad Abuzar' Date = '2023-11-30' Version = "1.0" Branch = "master">View Model for Judgements Detail</History>
    public class CmsJudgementDetailVM
	{
		[Key]
		public Guid Id { get; set; }
		public DateTime? HearingDate { get; set; }
		public DateTime? JudgementDate { get; set; }
		public Decimal? Amount { get; set; }
		public Decimal? AmountCollected { get; set; }
        public bool? IsFinal { get; set; } = false;
        public string? TypeEn { get; set; }
		public string? TypeAr { get; set; }
		public string? SerialNumber { get; set; }
		public string? StatusEn { get; set; }
		public string? StatusAr { get; set; }
		public string? Remarks { get; set; }
		public string? ExecutionFileLevelEn { get; set; }
		public string? ExecutionFileLevelAr { get; set; }
		public int? ExecutionFileLevelId { get; set; }
		public string? CategoryEn { get; set; }
		public string? CategoryAr { get; set; }
    }
}
