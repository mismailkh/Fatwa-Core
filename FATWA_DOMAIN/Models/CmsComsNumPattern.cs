using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.CaseManagment;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FATWA_DOMAIN.Enums.WorkflowEnums;

namespace FATWA_DOMAIN.Models
{
    [Table("CMS_COMS_NUM_PATTERN")]
    //<History Author = 'Nabeel ur Rehman' Date='2023-08-24' Version="1.0" Branch="master">Add Case and consultation number and file Number</History>
    public partial class CmsComsNumPattern : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public int? PatternTypId { get; set; }
        public string? Day { get; set; }
        public int? D_Order { get; set; }
        public string? Month { get; set; }
        public int? M_Order { get; set; }
        public string? Year { get; set; }
        public int? Y_Order { get; set; }
        public string StaticTextPattern { get; set; }
        public int? STP_Order { get; set; }
        public string? SequanceNumber { get; set; }
        public int? SN_Order { get; set; }
        public string? SequanceFormatResult { get; set; }
        public string? SequanceResult { get; set; }
        public bool ResetYearly { get; set; }
        public bool IsModified { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        [NotMapped]
        public string? UserName { get; set; }
        [NotMapped]
        public List<CmsGovtEntityNumPattern> CmsGovtEntityNumPatternGroup { get; set; } = new List<CmsGovtEntityNumPattern>();
        [NotMapped]
        public List<Group> usersGroup { get; set; } = new List<Group>();
        [NotMapped]
        public IEnumerable<Guid> GroupIds { get; set; } = new List<Guid>();
        [NotMapped]
        public IEnumerable<int> GovernamentEntityIds { get; set; } = new List<int>();
    }
}
