using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.CommunicationModels
{
    [Table("COMM_COMMUNICATION")]
    public partial class Communication
    {
        [Key]
        public Guid CommunicationId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? InboxNumber { get; set; }
        public DateTime? InboxDate { get; set; }
        public string? OutboxNumber { get; set; }
        public DateTime? OutboxDate { get; set; }
        public int? CorrespondenceTypeId { get; set; }
        public int OutboxShortNum { get; set; }
        public int InboxShortNum { get; set; }
        public string? ReferenceNumber { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public Guid? PreCommunicationId { get; set; }
        public string? InboxNumberFormat { get; set; }
        public string? OutBoxNumberFormat { get; set; }
        public string? PatternSequenceResult { get; set; }

        #region Common

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }

        #endregion

        #region Foreign Keys

        public int CommunicationTypeId { get; set; }
        public int SectorTypeId { get; set; }
        public int SourceId { get; set; }
        public int? GovtEntityId { get; set; }
        public string? ReceivedBy { get; set; }
        public string? SentBy { get; set; }
        public int? DepartmentId { get; set; }

        #endregion

        public int ColorId { get; set; }
        public bool IsRead { get; set; } = false;
        public bool IsReminderSent { get; set; } = false;
        public bool IsReplied { get; set; } = false;
        public bool Archive { get; set; } = false;
        public bool ReturnCorrespondence { get; set; } = false;
        [NotMapped]
        public List<int>? DeletedAttachementIds { get; set; } = new List<int>();
    }
}
