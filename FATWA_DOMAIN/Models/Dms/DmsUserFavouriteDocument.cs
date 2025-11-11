using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Dms
{
    [Table("DMS_USER_FAVOURITE_DOCUMENT")]
    public class DmsUserFavouriteDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int? DocumentId { get; set; }
        public string UserId { get; set; }
        public Guid? AddedDocumentVersionId { get; set; }

    }
}
