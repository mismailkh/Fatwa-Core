using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models
{

	public class Tokens
	{
		public string Token { get; set; }
		public string RefreshToken { get; set; }
	}
}
