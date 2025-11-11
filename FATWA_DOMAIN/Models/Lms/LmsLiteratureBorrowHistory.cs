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
    [Table("LMS_BORROW_LITERATURE_HISTORY")]
    public class LmsLiteratureBorrowHistory : TransactionalBaseModel
    {
       
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public int EventId { get; set; }
        public int LiteratureId { get; set; }
        public int? BorrowId { get; set; }
    }
}
