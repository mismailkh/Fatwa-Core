using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_INFRASTRUCTURE.Repository
{
    public class DashboardRepository : IDashboard
    {
        private readonly DatabaseContext _dbContext;
        private DashboardVM? _dashboardVM;

        public DashboardRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext; 
        }

        #region GET FUNCSTIONS

        public async Task<DashboardVM?> GetDashboardDetails()
        {
            try
            {
                if (_dashboardVM == null)
                {
                    string StoredProc = "exec pDashboardDetails";
                    var result = await _dbContext.DashboardVMs.FromSqlRaw(StoredProc).ToListAsync();
                    if (result != null)
                        _dashboardVM = result.FirstOrDefault();
                }
                return _dashboardVM; 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion
    }
}
