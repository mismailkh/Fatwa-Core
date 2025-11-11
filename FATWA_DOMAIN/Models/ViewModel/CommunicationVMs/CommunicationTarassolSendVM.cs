using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CommunicationVMs
{
	public class CommunicationTarassolSendVM
	{
		public string SenderUser { get; set; }
		//Sender Site
		public string SSite { get; set; }
       // ReceiverSiteName
        public string RSite { get; set; }
        public string Remarks { get; set; }
        public string BookNo { get; set; }
        public string G2GSubject { get; set; }

        //Sender Branch Site ID
        public int SBrSiteId { get; set; }
        //Reciever Branch Site ID
        public int RBrSiteId { get; set; }
	}
}
