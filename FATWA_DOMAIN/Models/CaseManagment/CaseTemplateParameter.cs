using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-10-20' Version = "1.0" Branch = "master" > Drafted Case Draft Model</History>
    [Table("CMS_TEMPLATE_PARAMETER")]
    public class CaseTemplateParameter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParameterId { get; set; }
        public string Name { get; set; }
        public string PKey { get; set; }
        public bool Mandatory { get; set; }
        public bool IsAutoPopulated { get; set; }
        public int ModuleId { get; set; }
        public bool IsActive { get; set; }
    }
}
