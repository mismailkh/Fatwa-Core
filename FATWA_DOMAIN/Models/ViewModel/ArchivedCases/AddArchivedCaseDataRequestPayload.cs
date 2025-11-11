namespace FATWA_DOMAIN.Models.ViewModel.ArchivedCases
{
    //< History Author = 'Ammaar Naveed' Date = '2024-12-12' Version = "1.0" Branch = "master">Added and modified class for request add archived data payload and corresponding classes</History>

    public class AddArchivedCaseDataRequestPayload
    {
        public string CANNumber { get; set; }
        public DateTime? MigrationDateTime { get; set; }
        public List<AddArchivedCaseVM> Cases { get; set; } = new List<AddArchivedCaseVM>();
        public List<AddArchivedCasePartyVM> CasesParties { get; set; } = new List<AddArchivedCasePartyVM>();
    }
    public class AddArchivedCaseVM
    {
        public Guid CaseId { get; set; }
        public string CaseNumber { get; set; }
        public DateTime CaseDate { get; set; }
        public int CourtCode { get; set; }
        public int ChamberCode { get; set; }
        public int ChamberNumber { get; set; }
    }
    public class AddArchivedCasePartyVM
    {
        public int PartyRoleId { get; set; }
        public string Name { get; set; }
        public string? MojPartyId { get; set; }
    }

    public class AddArchivedCaseDocumentsVM
    {
        public string DocumentTitle { get; set; }
        public Guid CaseId { get; set; }
        public int DocumentTypeId { get; set; }
        public DateTime DocumentDate { get; set; }
        public int NumberOfPages { get; set; }
    }
}
