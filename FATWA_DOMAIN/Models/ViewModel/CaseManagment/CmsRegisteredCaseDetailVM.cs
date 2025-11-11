using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-10-24' Version = "1.0" Branch = "master" > Registered Case Detail VM </History>
    public class CmsRegisteredCaseDetailVM : TransactionalBaseModel
    {
        [Key]
        public Guid CaseId { get; set; }
        public Guid? FileId { get; set; }
        public string? CANNumber { get; set; }
        public string? CaseNumber { get; set; }
        public DateTime? CaseDate { get; set; }
        public int? CourtId { get; set; }
        public int? CourtTypeId { get; set; }
        public int? ChamberId { get; set; }
        public int? StatusId { get; set; }
        public int? GovtEntityId { get; set; }
        public bool? IsConfidential { get; set; }
        public bool IsAssigned { get; set; }
        public bool? IsDissolved { get; set; }
        public string? CaseRequirements { get; set; }
        public string? GovtEntityNameEn { get; set; }
        public string? GovtEntityNameAr { get; set; }
        public string? StatusNameEn { get; set; }
        public string? StatusNameAr { get; set; }
        public string? CourtTypeNameEn { get; set; }
        public string? CourtTypeNameAr { get; set; }
        public string? CourtNameEn { get; set; }
        public string? CourtNameAr { get; set; }
        public string? CourtNumber { get; set; }
        public string? CourtDistrict { get; set; }
        public string? CourtLocation { get; set; }
        public string? ChamberNameEn { get; set; }
        public string? ChamberNameAr { get; set; }
        public string? ChamberNumber { get; set; }
        public int ChamberNumberId { get; set; }
        public string? ChamberAddress { get; set; }
        public string? FileNumber { get; set; }
		public string FloorNumber { get; set; }
		public string RoomNumber { get; set; }
		public string AnnouncementNumber { get; set; }
		public int? RequestTypeId { get; set; }
		public string? RequestTypeNameEn { get; set; }
		public string? RequestTypeNameAr { get; set; }
        public bool IsImportant { get; set; }
        // For Communication
        public string CaseRequstCreatedBy { get; set; }
        [NotMapped]
        public IList<CasePartyLinkVM>? CasePartyLinks { get; set; } = new List<CasePartyLinkVM>();
		[NotMapped]
		public bool IsMojExecutionRequest { get; set; }
	}
}
