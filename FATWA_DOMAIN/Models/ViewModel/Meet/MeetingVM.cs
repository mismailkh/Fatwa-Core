using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.ViewModel.Meet
{
    public partial class MeetingVM :GridMetadata
    {  
		public int? SerialNo { get; set; }
		public Guid? MeetingId { get; set; } 
		public string? Subject { get; set; }
		public int? MeetingTypeId { get; set; }
        public string? TypeEn { get; set; }
        public string? TypeAr { get; set; }
        public string? Location { get; set; }
        public string? CreatedBy { get; set; }
		public int? MeetingStatusId { get; set; } 
		public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public string? FileNumber { get; set; }
        public DateTime DateTime { get; set; }  
        public string? UserNameEn { get; set; }
        public string? UserNameAr { get; set; }
        public bool? IsSendToHOS { get; set; }  
        public Guid? ReferenceGuid { get; set; }
        public int? MOMStatusId { get; set; }
        public Guid? CommunicationId { get; set; }
    }
}
