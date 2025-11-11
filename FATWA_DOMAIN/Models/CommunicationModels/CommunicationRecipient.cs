using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CommunicationModels
{
    [Table("COMM_COMMUNICATION_Recipient")]
    public class CommunicationRecipient : TransactionalBaseModel
    {
        public Guid Id { get; set; }
        public Guid CommunicationId { get; set; }
        public Guid RecipientId { get; set; }
    }
}
