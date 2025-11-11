using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("ATTACHMENT_TYPE")]
    //<History Author = 'Hassan Abbas' Date='2022-04-22' Version="1.0" Branch="master"> Added Attachment Type Model</History>
    public partial class AttachmentType : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachmentTypeId { get; set; }
        public string Type_Ar { get; set; }
        public string Type_En { get; set; }
        public int? ModuleId { get; set; }
        public int? SubTypeId { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsOfficialLetter { get; set; }
        public bool IsDigitallySign { get; set; }
        public bool IsGePortalType { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsSystemDefine { get; set; }
        public string? Description { get; set; }
        public bool IsOpinion { get; set; }
        public bool IsMojExtracted { get; set; }
        [NotMapped]
        public IEnumerable<int> DesignationIds { get; set; } = new List<int>();
        [NotMapped]
        public IList<int> SigningMethodIds { get; set; } = new List<int>();
        [NotMapped]
        public List<int> AttachmentTypeIds { get; set; } = new List<int>();
    }
}
