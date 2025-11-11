using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MobileAppVMs
{
    public class MobileAppCaseTransferRequestVM
    {
        [DisplayName("File_Number")]
        public string? FileNumber { get; set; }
        [Key]
        public Guid Id { get; set; }
        [DisplayName("Chamber_Name_From")]
        public string? ChamberFromName { get; set; }
        [DisplayName("Chamber_Name_To")]
        public string? ChamberToName { get; set; }
        [DisplayName("Chamber_Number_From")]
        public string? ChamberNumberFrom { get; set; }
        [DisplayName("Chamber_Number_To")]
        public string? ChamberNumberTo { get; set; }
        [DisplayName("Remarks")]
        public string? Remarks { get; set; }
        [DisplayName("Status")]
        public string? StatusName { get; set; }
        [DisplayName("Request_Date")]
        public DateTime? CreatedDate { get; set; }
        [DisplayName("Created_By")]
        public string? CreatedBy { get; set; }
        [DisplayName("Modified_Date")]
        public DateTime? ModifiedDate { get; set; }
        //Case Details
        [DisplayName("Case_Number")]
        public string? CaseNumber { get; set; }
        [DisplayName("CAN_Number")]
        public string? CANNumber { get; set; }
        [DisplayName("Case_Open_Date")]
        public DateTime? CaseDate { get; set; }
        [DisplayName("Announcement_Number")]
        public string? AnnouncementNumber { get; set; }
        [DisplayName("Floor_Number")]
        public string? FloorNumber { get; set; }
        [DisplayName("Room_Number")]
        public string? RoomNumber { get; set; }
        [DisplayName("Government_Entity")]
        public string? GovtEntityName { get; set; }
        [DisplayName("Case_Status")]
        public string? CaseStatusName { get; set; }
        [DisplayName("Court_level")]
        public string? CourtTypeName { get; set; }
        [DisplayName("Court_Name")]
        public string? CourtName { get; set; }
        [DisplayName("Court_Number")]
        public string? CourtNumber { get; set; }
        [DisplayName("Court_District")]
        public string? CourtDistrict { get; set; }
        [DisplayName("Court_Location")]
        public string? CourtLocation { get; set; }
        [DisplayName("File_Name")]
        public string? FileName { get; set; }
        [DisplayName("Case_Type")]
        public string? RequestTypeName { get; set; }
    }
}
