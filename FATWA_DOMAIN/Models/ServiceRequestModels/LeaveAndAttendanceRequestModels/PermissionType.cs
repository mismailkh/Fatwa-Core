using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ServiceRequestModels.LeaveAndAttendance
{ 
    public class PermissionType
    { 
        public int PermissionId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }
}
