using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.CaseManagment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("CMS_GOVERNMENT_ENTITY_DEPARTMENT_G2G_LKP")]
    public class GEDepartments : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {  get; set; }
        public int? EntityId { get; set; }
        public string? Name_En { get; set; }
        public string? Name_Ar { get; set; }
        public bool? IsActive { get; set; }
        public GovernmentEntity Entity { get; set; }
        public int? G2GBRSiteID { get; set; } 
        public bool DefaultReceiver { get; set; }
        public string? DeptProfession { get; set; }
        //Use For Tarassol
        [NotMapped]
        public int G2GSiteID { get; set; }
        [NotMapped]
        public int? SenderBranchId { get; set; }
		[NotMapped]
		public string? RecieverSiteName { get; set; }  

    }
}
