using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("EP_GRADE")]
    public class Grade:TransactionalBaseModel
    {
        [Key]
        public int Id { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
        public int? GradeTypeId { get; set; }
        public GradeType GradeType { get; set; }
        public bool IsActive { get; set; }
    }
}
