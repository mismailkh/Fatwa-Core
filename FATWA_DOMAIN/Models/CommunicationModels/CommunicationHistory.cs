using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CommunicationModels
{
    [Table("COMM_COMMUNICATION_HISTORY")]
    public class CommunicationHistory : TransactionalBaseModel
    {
        public Guid Id { get; set; }   
        //Communication Id
        public Guid ReferenceId { get; set; }   
        public Guid SentBy { get; set; }     
        public Guid SentTo { get; set; }
        [NotMapped]
        public List<string> RecieversId { get; set; }   
        public int? ActionId { get; set; }   
        public string? Reason { get; set; }
        [NotMapped]
        public int? SectorId { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();

    }
}



