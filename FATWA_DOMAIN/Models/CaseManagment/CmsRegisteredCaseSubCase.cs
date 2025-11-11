using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-11-15' Version = "1.0" Branch = "master" >Registered Case and Subcase relation</History>
    [Table("CMS_REGISTERED_CASE_SUB_CASE")]
    public class CmsRegisteredCaseSubCase
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CaseId { get; set; }
        public Guid SubCaseId { get; set; }
    }
}
