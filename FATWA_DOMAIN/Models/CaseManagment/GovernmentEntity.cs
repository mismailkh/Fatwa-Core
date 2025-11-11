using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{

    //<History Author = 'Hassan Abbas' Date='2022-04-22' Version="1.0" Branch="master"> Add government entity</History>
    [Table("CMS_GOVERNMENT_ENTITY_G2G_LKP")]
    public partial class GovernmentEntity :TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EntityId { get; set; }
        public string Name_Ar { get; set; }
        public string Name_En { get; set; }
        public bool? IsActive { get; set; }
        public bool IsConfidential { get; set; } = false;
        public string? GECode { get; set; }
        public int? G2GSiteID { get; set; }
        public int? G2GSiteCode { get; set; }
        [NotMapped]
        public List<CmsBankGovernmentEntity> CmsBankGovernmentEntities { get; set; } = new List<CmsBankGovernmentEntity>();
    }
}
