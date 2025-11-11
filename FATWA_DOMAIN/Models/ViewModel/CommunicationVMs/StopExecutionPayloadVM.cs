using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CommunicationVMs
{
    public class StopExecutionPayloadVM
    {

        public dynamic CommunicationId { get; set; }
        public dynamic ReferenceId { get; set; }
        public dynamic CommunicationTypeId { get; set; }
        public int SubModuleId { get; set; }

    }
}
