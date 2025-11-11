using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models
{
    [Table("CMS_GOVERNMENT_ENTITY_NUM_PATTERN")]
    public class CmsGovtEntityNumPattern : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public int? GovtEntityId { get; set; }
        public Guid CMSRequestPatternId { get; set; } 
        public Guid COMSRequestPatternId { get; set; }
    }
}
