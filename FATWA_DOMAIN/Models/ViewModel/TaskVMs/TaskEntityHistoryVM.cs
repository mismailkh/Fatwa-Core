namespace FATWA_DOMAIN.Models.ViewModel.TaskVMs
{
    public class TaskEntityHistoryVM
    {
        public Guid? ReferenceId { get; set; }
        public string? ActionEn { get; set; }
        public string? ActionAr { get; set; }
        public string? UserNameEn { get; set; }
        public string? UserNameAr { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ReferenceNumber { get; set; }
        public int? EntityId { get; set; }
        public string? GovtEntityNameEn { get; set; }
        public string? GovtEntityNameAr { get; set; }
        public string? CANNumber { get; set; }

    }
}


