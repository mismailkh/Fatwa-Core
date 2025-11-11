using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.CaseManagment;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("CMS_COMS_NUM_PATTERN_HISTORY")]
    public  class CmsComsNumPatternHistory : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public int? PatternTypId { get; set; }
        public Guid? PatternId { get; set; }
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
        public string? UpdatedGovtEntities { get; set; }
        public bool ResetYearly { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        [NotMapped]
        public string? UserName { get; set; }


        [NotMapped]
        public List<CmsGovtEntityNumPattern> CmsGovtEntityNumPatternGroup { get; set; } = new List<CmsGovtEntityNumPattern>();
        //[NotMapped]
        //public List<GovernmentEntity> GovernamentEntityGroup { get; set; } = new List<GovernmentEntity>(); // later on remove 
        [NotMapped]
         public List<Group> usersGroup { get; set; } = new List<Group>();
        [NotMapped]
        public IEnumerable<Guid> GroupIds { get; set; } = new List<Guid>();
        [NotMapped]
        public IEnumerable<int>? GovernamentEntityIds { get; set; } = new List<int>();

    }
}

  