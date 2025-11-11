using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace G2G_DOMAIN.Models.CommonModels
{
    [Table("LINK_TARGET_TYPE")]

    public partial class LinkTargetType
    {
        [Key] 
        public int LinkTargetTypeId { get; set; }
        public string? NameEn { get; set; }
    }
}
