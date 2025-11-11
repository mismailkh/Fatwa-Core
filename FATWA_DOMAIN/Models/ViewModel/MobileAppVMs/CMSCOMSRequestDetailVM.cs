using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MobileAppVMs
{
    public class CMSCOMSRequestDetailVM
    {
        [DisplayName("Request_Number")]
        public string? RequestNumber { get; set; }
        [DisplayName("Request_Date")]
        public DateTime? RequestDate { get; set; }
        [DisplayName("Type_Request")]
        public string? RequestTypeName { get; set; }
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
        [DisplayName("Request_Title")]
        public string? RequestTitle { get; set; }
    }
}
