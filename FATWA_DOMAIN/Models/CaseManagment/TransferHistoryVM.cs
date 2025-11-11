using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
	public class TransferHistoryVM
	{
		[Key]
		public Guid Id { get; set; }
		public int ChamberFromId { get; set; }
		public int ChamberToId { get; set; }
		public int ChamberNumberFromId { get; set; }
		public int ChamberNumberToId { get; set; }
		public Guid OutcomeId { get; set; }
		public string ChamberNameFromEn { get; set; }
		public string ChamberNameFromAr { get; set; }
        public string ChamberNameToEn { get; set; }
        public string ChamberNameToAr { get; set; }
        public string ChamberNumberFrom { get; set; }
        public string ChamberNumberTo { get; set; }
		public string CreatedByNameEn { get; set; }
		public string CreatedByNameAr { get; set; }
		public DateTime TransferDate { get; set; }
		public int RequestStatusId { get; set; }
		public string? RejectionReason { get; set; }
	}
}
