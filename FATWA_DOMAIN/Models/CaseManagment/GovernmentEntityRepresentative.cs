using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //<History Author = 'Hassan Abbas' Date='2022-02-27' Version="1.0" Branch="master">Government Entity Representatives</History>
    [Table("CMS_GOVERNMENT_ENTITY_REPRESENTATIVES")]
    public partial class GovernmentEntityRepresentative : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int GovtEntityId { get; set; }
        public string? RepresentativeCode { get; set; }
        public string? Representative_Designation_EN { get; set; }
        public string? Representative_Designation_AR { get; set;}
        public bool IsActive { get; set;}
    }
    public class GovtEntityIdsPayload
    {
        public List<int> EntityIds { get; set; }     
    }    
    public class GovtEntityRepresentativeNamesResponseVM
    {
        public string RepresentativeNameEn { get; set; }
        public string RepresentativeNameAr { get; set; }
        public string GovtEntityNameEn { get; set; }
        public string GovtEntityNameAr { get; set; }
    }    
}
