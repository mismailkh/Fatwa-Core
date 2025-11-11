using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("EP_GRADE_TYPE")]
    public class GradeType : TransactionalBaseModel
    {
        [Key]
        public int Id { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
        public int? DepartmentId { get; set; }
    }
}
