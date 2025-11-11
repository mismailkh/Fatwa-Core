using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.Email
{
    [Table("EmailConfiguration")]
    public class EmailConfiguration : TransactionalBaseModel
    {
        [Key]
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string SmtpHost { get; set; }
        public string SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
        [NotMapped]
        public int BodyType { get; set; }
    }
}
