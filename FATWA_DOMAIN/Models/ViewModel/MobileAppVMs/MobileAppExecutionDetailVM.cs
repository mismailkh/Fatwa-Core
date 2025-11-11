using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MobileAppVMs
{
    public class MobileAppExecutionDetailVM
    {
        [DisplayName("Case_Number")]
        public string? CaseNumber { get; set; }
        [DisplayName("File_Number")]
        public string? FileNumber { get; set; }
        //Execution Request Detail
        [DisplayName("Remarks")]
        public string? Remarks { get; set; }
        [DisplayName("Request_Date")]
        public DateTime RequestDate { get; set; }
        //Case Detail
        [DisplayName("CAN_Number")]
        public string? CANNumber { get; set; }
        [DisplayName("Case_Date")]
        public DateTime? CaseDate { get; set; }
        [DisplayName("Court_Type")]
        public string? CourtTypeName { get; set; }
        [DisplayName("Court_Name")]
        public string? CourtName { get; set; }
        [DisplayName("Chamber_Name")]
        public string? ChamberName { get; set; }
        [DisplayName("Chamber_Number")]
        public string? ChamberNumber { get; set; }
        [DisplayName("Is_Confidential")]
        public bool? IsConfidential { get; set; }
        //Execution Judgement Detail
        [DisplayName("Execution_File_Number")]
        public string? ExecutionFileNumber { get; set; }
        [DisplayName("File_Opening_Date")]
        public DateTime? FileOpeningDate { get; set; }
        [DisplayName("File_Balance")]
        public decimal? FileBalance { get; set; }
        [DisplayName("Execution_Remarks")]
        public string? ExecutionRemarks { get; set; }
    }
}
