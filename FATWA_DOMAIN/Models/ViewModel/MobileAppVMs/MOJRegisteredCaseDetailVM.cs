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
    public class MOJRegisteredCaseDetailVM
    {
        [DisplayName("Case_Number")]
        public string? CaseNumber { get; set; }
        [DisplayName("File_Number")]
        public string FileNumber { get; set; }
        [DisplayName("CAN_Number")]
        public string? CANNumber { get; set; }
        [DisplayName("Case_Open_Date")]
        public DateTime? CaseDate { get; set; }
        [DisplayName("File_Date")]
        public DateTime? FileDate { get; set; }
        [DisplayName("Is_Confidential")]
        public bool? IsConfidential { get; set; }
        [DisplayName("Case_Requirements")]
        public string? CaseRequirements { get; set; }
        [DisplayName("Government_Entity")]
        public string? GovtEntityName { get; set; }
        [DisplayName("Status")]
        public string? StatusName { get; set; }
        [DisplayName("Court_Type")]
        public string? CourtTypeName { get; set; }
        [DisplayName("Court_Name")]
        public string? CourtName { get; set; }
        [DisplayName("Court_Number")]
        public string? CourtNumber { get; set; }
        [DisplayName("Chamber_Name")]
        public string? ChamberName { get; set; }
        [DisplayName("Chamber_Number")]
        public string? ChamberNumber { get; set; }
        [DisplayName("Floor_Number")]
        public string FloorNumber { get; set; }
        [DisplayName("Room_Number")]
        public string RoomNumber { get; set; }
        [DisplayName("Announcement_Number")]
        public string AnnouncementNumber { get; set; }
        [DisplayName("Case_Type")]
        public string? RequestTypeName { get; set; }
    }
}
