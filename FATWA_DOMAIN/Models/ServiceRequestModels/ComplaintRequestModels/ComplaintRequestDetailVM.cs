namespace FATWA_DOMAIN.Models.ServiceRequestModels.ComplaintRequestModels
{
    public class ComplaintRequestDetailVM
    {
        public Guid Id { get; set; }
        public Guid ServiceRequestId { get; set; }
        public string ServiceRequestNumber { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ComplaintTypeEn { get; set; } = null!;
        public string ComplaintTypeAr { get; set; } = null!;
        public string OtherComplaintType { get; set; } = null!;
        public string PriorityAr { get; set; } = null!;
        public string PriorityEn { get; set; } = null!;
        public DateTime? IssueStartDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string StatusEn { get; set; } = null!;
        public string StatusAr { get; set; } = null!;
        public int? ServiceRequestStatusId { get; set; }
        public int? PriorityId { get; set; }
        public int? ComplaintTypeId { get; set; }
    }
}
