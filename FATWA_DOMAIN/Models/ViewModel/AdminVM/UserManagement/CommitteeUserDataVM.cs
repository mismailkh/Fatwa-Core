using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement
{
    public class CommitteeUserDataVM
    {
        public string? UserId { get; set; }
        public bool? IsActive { get; set; }
        public int? SectorTypeId { get; set; }
        public int? SectorParentId { get; set; }
        public string? FirstName_En { get; set; }
        public string? LastName_En { get; set; }
        public string? FirstName_Ar { get; set; }
        public string? LastName_Ar { get; set; }
        public string? FullName_En { get; set; }
        public string? FullName_Ar { get; set; }        
        public string? RoleId { get; set; }
        public int? TotalCommittee { get; set; }
        public int? TotalTasks { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public string? SectorTypeEn { get; set; }
        public string? SectorTypeAr { get; set; }
    }    
    
}
