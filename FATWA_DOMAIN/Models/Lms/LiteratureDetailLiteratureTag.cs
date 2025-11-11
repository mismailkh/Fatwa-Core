using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Lms
{
    [Table("LMS_LITERATURE_DETAIL_LITERATURE_TAG")]
    public class LiteratureDetailLiteratureTag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int LiteratureId { get; set; }
        public int TagId { get; set; }
        public string Value { get; set; }
    }
}
