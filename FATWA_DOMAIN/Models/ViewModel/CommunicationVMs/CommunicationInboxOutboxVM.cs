using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.CommunicationVMs
{
    public partial class CommunicationInboxOutboxVM : GridMetadata
    {
        [Key]
        public Guid CommunicationId { get; set; }
        public int CommunicationTypeId { get; set; }
        public int LinkTargetTypeId { get; set; }
        public int CorrespondenceTypeId { get; set; }
        public string? Activity_En { get; set; }
        public string? Activity_Ar { get; set; }
        public string? CreatedBy { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? InboxNumber { get; set; }
        public DateTime? InboxDate { get; set; }
        public string? OutboxNumber { get; set; }
        public DateTime? OutboxDate { get; set; }
        public string? CorrespondenceTypeAr { get; set; }
        public string? CorrespondenceTypeEn { get; set; }
        public string? SourceEn { get; set; }
        public string? SourceAr { get; set; }
        public string? GovtEntityEn { get; set; }
        public string? GovtEntityAr { get; set; }
        public string? SectorEn { get; set; }
        public string? SectorAr { get; set; }
        public Guid? ReferenceId { get; set; }
        public int? SectorTypeId { get; set; }
    }
}
