using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    //<History Author = 'Hassan Abbas' Date='2022-11-29' Version="1.0" Branch="master"> Moj Registration Request View Model</History>
    public class MojRegistrationRequestVM : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid FileId { get; set; }
        public string? MessengerId { get; set; }
        public string? FileNumber { get; set; }
        public string? FileName { get; set; }
        public bool? IsRegistered { get; set; }
        public int DocumentId { get; set; }
        public Guid? CaseId { get; set; }
        public int TotalCount { get; set; }
    }
}
