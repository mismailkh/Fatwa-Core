using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public class AssignToLawyerVM
    {
       
        public Guid RequestId { get; set; }
        public string? LawyerId { get; set; }
       
        public string? PrimaryLaywerId { get; set; }
      
        public string? SupervisorId { get; set; }

        public IList<UserVM>? SelectedUsers { get; set; } = new List<UserVM>();

    }
}
