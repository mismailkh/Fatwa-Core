using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models
{
    [Table("LEGAL_LEGISLATION_LEGAL_TAG", Schema = "dbo")]
    public partial class LegalLegislationLegalTag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid LegislationId { get; set; }
        public int TagId { get; set; }
    }
}
