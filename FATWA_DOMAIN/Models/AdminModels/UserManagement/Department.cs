using FATWA_DOMAIN.Models.CaseManagment;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("EP_Department")]
    public class Department : TransactionalBaseModel
    {
       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name_En { get; set; }
        public string? Name_Ar { get; set; }
        public bool IsActive { get; set; }
        public int Borrow_Return_Day_Duration { get; set; }
        public ICollection<OperatingSectorType> SectorTypes { get; set; }
    }
}
