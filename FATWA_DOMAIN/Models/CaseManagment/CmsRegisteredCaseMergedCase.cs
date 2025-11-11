using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-11-15' Version = "1.0" Branch = "master" >Registered Case and Merged Cases relation</History>
    [Table("CMS_REGISTERED_CASE_MERGED_CASE")]
    public class CmsRegisteredCaseMergedCase
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PrimaryCaseId { get; set; }
        public Guid MergedCaseId { get; set; }
    }
}
