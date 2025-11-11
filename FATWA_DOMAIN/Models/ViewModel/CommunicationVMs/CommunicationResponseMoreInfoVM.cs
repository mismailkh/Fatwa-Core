using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.CommunicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CommunicationVMs
{
    public class CommunicationResponseMoreInfoVM
    {
        public CommunicationResponse? CommunicationResponse { get; set; }
        public Communication? Communication { get; set; }
        public CommunicationTargetLink? CommunicationTargetLink { get; set; }
        public List<LinkTarget>? LinkTarget { get; set; }
    }
}
