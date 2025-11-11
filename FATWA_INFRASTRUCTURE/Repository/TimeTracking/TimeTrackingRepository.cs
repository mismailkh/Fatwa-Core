using FATWA_DOMAIN.Interfaces.TimeTracking;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.TimeTrackingVM;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_GENERAL.Helper.Enum;

namespace FATWA_INFRASTRUCTURE.Repository.TimeTracking
{
    public class TimeTrackingRepository : ITimeTracking
    {
        #region Variable
        private readonly IConfiguration _config;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private List<TimeTrackingVM> _TimeTrackingVMs;

        private readonly DatabaseContext _dbContext;
        #endregion

        #region Constructor
        public TimeTrackingRepository(DatabaseContext dbContext, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            _dbContext = dbContext;
            _config = configuration;
            _serviceScopeFactory = serviceScopeFactory;
        }

        #endregion

        #region Time TRracking List
        public async Task<List<TimeTrackingVM>> GetTimeTracking(TimeTrackingAdvanceSearchVM advanceSearchVM)
        {
            try
            {
                if (_TimeTrackingVMs == null)
                {
                    string AssignedOnfromDate = advanceSearchVM.AssignedOn != null ? Convert.ToDateTime(advanceSearchVM.AssignedOn).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string CompletedOnDate = advanceSearchVM.CompletedOn != null ? Convert.ToDateTime(advanceSearchVM.CompletedOn).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    if (advanceSearchVM.ModuleId == (int)ModuleEnum.CaseManagement)
                    {
                        string StoreProc = $"exec pTimeTrackingList @statusId ='{advanceSearchVM.StatusId}', @referenceId='{advanceSearchVM.ReferenceId}', @sectorTypeId='{advanceSearchVM.SectortypeId}' , " +
                            $"@userId='{advanceSearchVM.UserId}' , @assignedOn='{AssignedOnfromDate}' , @completedOn='{CompletedOnDate}', @userName='{advanceSearchVM.UserName}' " +
                            $",@PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}'";
                        _TimeTrackingVMs = await _dbContext.TimeTrackingVMs.FromSqlRaw(StoreProc).ToListAsync();
                    }
                    else if (advanceSearchVM.ModuleId == (int)ModuleEnum.ConsultationManagement)
                    {
                        string StoreProc = $"exec pTimeTrackingConsultationList @statusId ='{advanceSearchVM.StatusId}', @referenceId='{advanceSearchVM.ReferenceId}', @sectorTypeId='{advanceSearchVM.SectortypeId}'," +
                            $" @userId ='{advanceSearchVM.UserId}' , @assignedOn='{AssignedOnfromDate}' , @completedOn='{CompletedOnDate}' , @userName='{advanceSearchVM.UserName}'" +
                            $",@PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}'";
                        _TimeTrackingVMs = await _dbContext.TimeTrackingVMs.FromSqlRaw(StoreProc).ToListAsync();
                    }
                    else
                    {
                        string StoreProc = $"exec pTimeTrackingList @statusId ='{advanceSearchVM.StatusId}', @referenceId='{advanceSearchVM.ReferenceId}', @sectorTypeId='{advanceSearchVM.SectortypeId}' ," +
                            $" @userId='{advanceSearchVM.UserId}' , @assignedOn='{AssignedOnfromDate}' , @completedOn='{CompletedOnDate}' , @userName='{advanceSearchVM.UserName}'" +
                            $",@PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}'";
                        _TimeTrackingVMs = await _dbContext.TimeTrackingVMs.FromSqlRaw(StoreProc).ToListAsync();
                    }

                }
                return _TimeTrackingVMs;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
