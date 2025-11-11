using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.PACIVM
{
    //<History Author = 'Nabeel ur Rehman' Date='2022-07-20' Version="1.0" Branch="master"> created VM for Showing PACI Request </History>
    public class PACIRequestListVM
    {
        [Key]
        public Guid RequestId { get; set; }
        public string Names { get; set; }
        public DateTime? RequestDate { get; set; }
        public string? CaseNumber { get; set; }
        public string? Year { get; set; }
        public string? RequestStatus { get; set; }
        public string? RequestStatusAr { get; set; }
        public int? EmailSentStatusId { get; set; }
        public string EmailSentStatus { get; set; }
        public string EmailSentStatusAr { get; set; }
        public int? RequestStatusId { get; set; }
        public string? RequestedDetails { get; set; }
        public string? RequestDocument { get; set; }
        public string? ResponseDocument { get; set; }
        public string? UpdatedBy { get; set; }
        public int? RefrenceNumber { get; set; }
        public string? CreatedBy { get; set; }
    }
}
