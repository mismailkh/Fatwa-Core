using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    //<History Author = 'Hassan Abbas' Date='2023-03-23' Version="1.0" Branch="master"> Moj Document Portfolio Request View Model</History>
    public class MojDocumentPortfolioRequestVM : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public int AttachmentTypeId { get; set; }
        public Guid CaseId { get; set; }
        public string? CaseNumber { get; set; }
        public DateTime HearingDate { get; set; }
        public string? RequiredDocuments { get; set; }
        public bool? IsAddressed { get; set; }
        public int TotalCount { get; set; }
    }
}
