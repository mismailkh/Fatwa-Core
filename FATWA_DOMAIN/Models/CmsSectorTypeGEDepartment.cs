using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups
{
    [Table("CMS_SECTOR_TYPE_GE_DEPARTMENT")]
    public class CmsSectorTypeGEDepartment:TransactionalBaseModel
    {
        [Key]
        public int Id { get; set; }
        public int SectorTypeId { get; set; }
        public int DepartmentId { get; set; }
        [NotMapped] 
        public FatwaSectorTypeEnum CmsFatwaSectorType { get; set; }
        [NotMapped] 
        public IEnumerable<int> SelectedDepartments { get; set; } = null;

    }
}
