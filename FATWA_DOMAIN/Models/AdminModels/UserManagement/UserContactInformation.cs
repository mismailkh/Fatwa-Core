using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{

    [Table("EP_CONTACT_INFORMATION")]
    public class UserContactInformation : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public int? ContactTypeId { get; set; }
        public string? ContactNumber { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsPrimary { get; set; }
        public UserPersonalInformation UserPersonalInformation { get; set; }
        public ContactType ContactType { get; set; }
    }
}
