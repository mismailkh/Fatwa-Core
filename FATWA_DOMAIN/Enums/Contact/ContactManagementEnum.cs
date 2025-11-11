using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Enums.Contact
{
	public class ContactManagementEnum
	{
		public enum ContactTypeEnum 
		{ 
			Internal = 1, 
			External = 2
		}
		public enum ContactJobRoleEnum
		{
			Role1 = 1,
			Role2 = 2,
			Role3 = 4
		}
		public enum ContactLinkType 
		{
			File = 1, 
			Request = 2,
			Meeting = 4 
		}
	}
}
