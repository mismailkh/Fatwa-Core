using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.BugReporting
{
    [Table("BUG_TYPE_USER_ASSIGNMENT")]
    public class BugUserTypeAssignment:TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public int BugTypeId { get; set; }
        public string? UserId { get; set; }
        public Guid? GroupId { get; set; }
        [NotMapped]
        public IEnumerable<string> selectedUserIds { get; set; } = new List<string>();
        [NotMapped]
        public IEnumerable<Guid> selectedGroupIds { get; set; } = new List<Guid>();
    }
}
