using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement
{
    public class UserClaimsVM
    {
        public string UserId { get; set; }
        public string FullName_En { get; set; }
        public string FullName_Ar { get; set; }
        public int? DesignationId { get; set; }
        public string ClaimTitle_En { get; set; }
        public string ClaimTitle_Ar { get; set; }
        public string ClaimValue { get; set; }
        public long? ModuleId { get; set; }
        [NotMapped]
        public int? SectorTypeId { get; set; }
        [NotMapped]
        public List<string> SelectedUserIds { get; set; }
        [NotMapped]
        public string SelectedUserId { get; set; }
        [NotMapped]
        public IEnumerable<string> SelectedUserClaims { get; set; }        
        [NotMapped]
        public string CreatedBy { get; set; }
    }
}
