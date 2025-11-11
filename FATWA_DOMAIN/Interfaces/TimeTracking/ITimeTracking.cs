using FATWA_DOMAIN.Models.ViewModel.TimeTrackingVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Interfaces.TimeTracking
{
    public interface  ITimeTracking
    {
        public Task<List<TimeTrackingVM>> GetTimeTracking(TimeTrackingAdvanceSearchVM advanceSearchVM);
    }
}
