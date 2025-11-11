using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement
{
    public class UserTransferVM
    {
        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string CurrentDepartmentNameAr { get; set; }
        public string CurrentDepartmentNameEn { get; set; }
        public string PreviousDepartmentNameEn { get; set; }
        public string PreviousDepartmentNameAr { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime TranferValidityDate { get; set; }

    }
}
