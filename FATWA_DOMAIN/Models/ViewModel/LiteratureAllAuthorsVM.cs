using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class LiteratureAllAuthorsVM
    {
        [Key]
        public int? AuthorId { get; set; }
        public string? LitratureAuthorFullNameEn { get; set; }
        public string? LitratureAuthorFullNameAr { get; set; }
        public string? LiteratureAuthorAdderssEn { get; set; }
        public string? LiteratureAuthorAdderssAr { get; set; }

    }
}
