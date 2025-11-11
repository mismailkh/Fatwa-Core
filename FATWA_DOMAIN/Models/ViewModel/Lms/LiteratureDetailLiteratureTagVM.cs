using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.Lms
{
    public class LiteratureDetailLiteratureTagVM
    {
        [Key]
        public int Id { get; set; }
        public int LiteratureId { get; set; }
        public int TagId { get; set; }
        public string Value { get; set; }
        public string? TagNo { get; set; }
        public string? Description { get; set; }
        public string? Description_Ar { get; set; }
        public string TempValue { get; set; }
        [NotMapped]
        public bool OpenForEdit { get; set; }
    }
}
