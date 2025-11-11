using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{

    //<History Author = 'Nabeel ur Rehman' Date='2023-08-24' Version="1.0" Branch="master"> Add document entity</History>
    public partial class CmsComsNumPatternVM : TransactionalBaseModel
    {
        public Guid Id { get; set; }
        public string PattrenName { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }

        public string? Year { get; set; }
        public string? Day { get; set; }
        public string? Month { get; set; }
        public string SequanceNumber { get; set; }
        public string? StaticTextPattern { get; set; }
        //public Guid? UserId { get; set; }
        public string SequanceResult { get; set; }
        public bool ResetYearly { get; set; }
        public bool IsDefault { get; set; }
        public string UserFullNameEn { get; set; }
        public string UserFullNameAr { get; set; }
        //public int? GovtEntityNumPatternId { get; set; }
        [NotMapped]
        public int PatternTypeId { get; set; }
    }
}
