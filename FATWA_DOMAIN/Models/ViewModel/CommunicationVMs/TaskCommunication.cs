using FATWA_DOMAIN.Models.CommonModels;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CommunicationVMs
{
	public partial class TaskCommunication
	{
		
		public string TaskName { get; set; }
		public string FirstUrl { get; set; }
		public string SecondUrl { get; set; }
		public string ThirdUrl { get; set; }
		public string AssignedTo { get; set; }
		public LinkTarget Linktarget { get; set; }

	}
}
