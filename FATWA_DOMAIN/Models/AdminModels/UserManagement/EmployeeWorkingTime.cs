using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("EP_WORKINGTIME")]
    public class EmployeeWorkingTime
    {
        [Key]
        public int Id { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
    }
}
