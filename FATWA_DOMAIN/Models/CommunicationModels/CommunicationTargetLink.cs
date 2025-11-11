using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.CommunicationModels
{
    [Table("COMM_COMMUNICATION_TARGET_LINK")]
    public partial class CommunicationTargetLink
	{
		[Key]
		public Guid TargetLinkId { get; set; }

        #region Foreign Keys 
        
        public Guid CommunicationId { get; set; }

        #endregion

        #region Common

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }

        #endregion

    }
}
