using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CMSCOMSRequestNumberVMs
{
	public class NumberPatternResult
	{
		//Model
		public string GenerateRequestNumber { get; set; }
		public string FormatRequestNumber { get; set; }
		public Guid? RequestPatternId { get; set; }
        public string PatternSequenceResult { get; set; }
    }
}
