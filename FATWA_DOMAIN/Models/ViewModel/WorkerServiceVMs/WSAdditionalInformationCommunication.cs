using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs
{
    public class WSAdditionalInformationCommunication
    {
        public Guid FileId { get; set; }
        public bool IsReplied { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CommunicationId { get; set; }
        public Guid? PreCommunicationId { get; set; }
        public int CommunicationTypeId { get; set; }
    }
}
