using FATWA_DOMAIN.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Interfaces
{
    public interface IDashboard
    {
        Task<DashboardVM?> GetDashboardDetails(); 
    }
}
