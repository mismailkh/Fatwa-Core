namespace FATWA_DOMAIN.Models.ViewModel.CommunicationVMs
{
	public partial class CommunicationResponseVM
	{
        public DateTime? RequestDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public int ResponseTypeId { get; set; }
        public string? Reason { get; set; } 
        public string? Other { get; set; } 
        public string? FileName { get; set; }
        public string? StoragePath { get; set; }
        public Guid CommunicationId { get; set; }
    }
}
