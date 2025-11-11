using Itenso.TimePeriod;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.CommunicationVMs
{
    public partial class CommunicationListVM
    {
        public Guid CommunicationId { get; set; }
        public int CommunicationTypeId { get; set; }
        public int CorrespondenceTypeId { get; set; }
        public string? Activity_En { get; set; }
        public string? Activity_Ar { get; set; }
        public string? CreatedBy { get; set; }
        public string? Remarks { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? InboxNumber { get; set; }
        public DateTime? InboxDate { get; set; }
        public string? OutboxNumber { get; set; }
        public DateTime? OutboxDate { get; set; }
        public string? CorrespondenceTypeAr { get; set; }
        public string? CorrespondenceTypeEn { get; set; }
        public string? ReferenceNo { get; set; }
        public string? ReferenceId { get; set; }
        public string? CanNo { get; set; }
        public int SubModuleId { get; set; }
        public string SubModule_En { get; set; }
        public string SubModule_Ar { get; set; }
        public bool IsRead { get; set; }
        public string? SectorNameEn { get; set; }
        public string? SectorNameAr { get; set; }
        public int? DateDifference { get; set; }
        public bool? IsReminderSent { get; set; } = false;
        public bool? IsReplied { get; set; } = false;
        public string? HexCode { get; set; }
        public string? PartyEntityEn { get; set; }
        public string? PartyEntityAr { get; set; }

    }
}