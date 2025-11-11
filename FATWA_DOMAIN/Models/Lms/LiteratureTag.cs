using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Lms
{
    [Table("LMS_LITERATURE_TAG")]
    public class LiteratureTag : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TagNo { get; set; }
        public string Description { get; set; }
        public string Description_Ar { get; set; }
        public bool Active { get; set; }
        public bool IsActive { get; set; }
    }
}
