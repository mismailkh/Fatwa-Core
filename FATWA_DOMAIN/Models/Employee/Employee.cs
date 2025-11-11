//Hassan Iftikhar Demo

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Employee
{
    [Table("EMPLOYEE")]
    public partial class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required (ErrorMessage ="Enter Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter Age")]
        public string Age { get; set; }
        [Required(ErrorMessage = "Enter CNIC")]
        public string Cnic { get; set; }
        public DateTime JoiningDate { get; set; }
    }
}
