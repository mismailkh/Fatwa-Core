using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.RabbitMQ
{
    [Table("RMQ_UNPUBLISH_MESSAGES")]
    public class RMQ_UnpublishMessage : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public string RoutingKey { get; set; }
        public string RQMessages { get; set; }
        public int Re_AttemptCount { get; set; }
        public bool IsPublished { get; set; }
    }
}
