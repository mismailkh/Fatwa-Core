using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.CommunicationModels
{
    [Table("COMM_COMMUNICATION_RESPONSE")]
    public partial class CommunicationResponse
    {
        [Key]
        public Guid CommunicationResponseId { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ResponseDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? ResponseTypeId { get; set; }
        public string Reason { get; set; }
        public string Other { get; set; }
        public bool IsUrgent { get; set; }
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

        public Guid CommunicationId { get; set; }
        public int? EntityId { get; set; }
        public int? PartyEntityId { get; set; }
        [NotMapped]
        public IEnumerable<int> EntityIds { get; set; } = new List<int>();
        public int? PriorityId { get; set; }
        public int? FrequencyId { get; set; }

        [NotMapped]
        public IEnumerable<int> PartyEntityIds { get; set; } = new List<int>();

        #endregion

    }
}
