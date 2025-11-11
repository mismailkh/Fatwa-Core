using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs;
using FATWA_DOMAIN.Models.WorkerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Interfaces.TimeInterval
{
    public interface IWorkerService
    {
        Task AddWorkerServiceExecution(WSWorkerServiceExecution workerServiceExecution);
        Task<List<WSExecutionDetailVM>> GetWorkerServiceExecutionDetail(WSExecutionAdvanceSearchVM wSExecutionAdvanceSearchVM);
        Task<List<WSExecutionStatus>> GetWSExecutionStatuses();
        Task<List<WSWorkerServices>> GetWorkerServices();
    }
}
