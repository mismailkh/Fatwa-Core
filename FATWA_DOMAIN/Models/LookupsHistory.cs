using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LOOKUPS_HISTORY")]
    public partial class LookupsHistory: TransactionalBaseModel
    {
        [Key]
        public Guid LookupsHistroyId { get; set; }
        public int LookupsId { get; set; }
        public string? NameEn { get; set; }
        public string? NameAr { get; set; }
        public string? TagNo { get; set; }
        public string? Description { get; set; }
        public string? DescriptionAr { get; set; }
        public int LookupsTableId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? StatusId { get; set; }
        public bool IsActive { get; set; }
        [NotMapped]
        public string UserFullNameEn { get; set; }
        [NotMapped]
        public string UserFullNameAr { get; set; }
    }
}
