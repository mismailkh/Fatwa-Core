using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MobileAppVMs
{
    public class CMSCOMSFileDetailVM
    {
        [DisplayName("File_Number")]
        public string? FileNumber { get; set; }
        [DisplayName("File_Date")]
        public DateTime? FileDate { get; set; }
        [DisplayName("Case_Type")]
        public string? CaseTypeName { get; set; }
        [DisplayName("Subject")]
        public string? Subject { get; set; }
        [DisplayName("Government_Entity")]
        public string? EntityName { get; set; }
        [DisplayName("Department")]
        public string? DepartmentName { get; set; }
        [DisplayName("Claim_Amount")]
        public decimal? ClaimAmount { get; set; }
        [DisplayName("Court_level")]
        public string? CourtTypeName { get; set; }
        [DisplayName("Priority")]
        public string? PriorityName { get; set; }
        [DisplayName("Is_Confidential")]
        public bool IsConfidential { get; set; }
        [DisplayName("Status")]
        public string? StatusName { get; set; }
        [DisplayName("Remarks")]
        public string? Remarks { get; set; }
        [DisplayName("Case_Requirements")]
        public string? CaseRequirements { get; set; }
        [DisplayName("Govermemt_Opinion")]
        public string? GovtOpinion { get; set; }
    }
}
