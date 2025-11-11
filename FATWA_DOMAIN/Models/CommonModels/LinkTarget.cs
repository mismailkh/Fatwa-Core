
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.CommonModels
{
    [Table("LINK_TARGET")]

    public partial class LinkTarget
    {
        [Key]
		public Guid LinkTargetId { get; set; }
        public bool IsPrimary { get; set; }
        #region Foreign Keys
        public Guid TargetLinkId { get; set; }
        public Guid? ReferenceId { get; set; }
        public int LinkTargetTypeId { get; set; }
        #endregion
       
    }
}
