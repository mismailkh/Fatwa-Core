using FATWA_DOMAIN.Interfaces.Common;
using FATWA_DOMAIN.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static FATWA_GENERAL.Helper.Response;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Interfaces.TimeInterval;
using System.Linq;
using FATWA_DOMAIN.Interfaces.WorkerService;
using FATWA_DOMAIN.Models.TimeInterval;
using FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_DOMAIN.Models.ViewModel.TimeIntervalVMs;

namespace FATWA_API.Controllers.V1.TimeInterval
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TimeIntervalController : ControllerBase
    {
        #region Properties
        private readonly ITimeIntervals _TimeInterval;
        private readonly IAuditLog _auditLogs;
        private readonly IAccount _IAccount;
        private readonly INotification _INotification;
        private readonly IConfiguration _configuration;
        private readonly IWorkerService _iWorkerService;
        #endregion

        #region Constructor
        public TimeIntervalController(ITimeIntervals iTimeInterval, IAuditLog iAudit, IConfiguration iConfiguration, IWorkerService iWorkerService)
        {
            _TimeInterval = iTimeInterval;
            _auditLogs = iAudit;
            _configuration = iConfiguration;
            _iWorkerService = iWorkerService;
        }
        #endregion

        #region  GetTimeInttervals
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpGet("GetTimeIntervals")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTimeIntervals()
        {
            try
            {
                return Ok(await _TimeInterval.GetTimeIntervals());
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetTimeIntervalHistoryList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTimeIntervalHistoryList()
        {
            try
            {
                return Ok(await _TimeInterval.GetTimeIntervalHistoryList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Update Interval Status
        [MapToApiVersion("1.0")]
        [HttpPost("UpdateIntervalStatus")]
        public async Task<IActionResult> UpdateIntervalStatus([FromForm] bool isActive, [FromForm] int id)
        {
            try
            {
                await _TimeInterval.UpdateIntervalStatus(isActive, id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Save Cms Coms Reminder 
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("SaveCmsComsReminder")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>
        public async Task<IActionResult> SaveCmsComsReminder(CmsComsReminder cmsComsReminder)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                await _TimeInterval.SaveCmsComsReminder(cmsComsReminder);


                return Ok(cmsComsReminder);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Update Cms Coms Reminder
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("UpdateCmsComsReminder")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateCmsComsReminder(CmsComsReminder cmsComsReminder)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                await _TimeInterval.UpdateCmsComsReminder(cmsComsReminder);


                return Ok(cmsComsReminder);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region  Get CmsComsReminder
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpGet("GetCmsComsReminderById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Abuzar' Date='2024-01-11' Version="1.0" Branch="master"> Get Reminder By Id</History>

        public async Task<IActionResult> GetCmsComsReminderById(int Id)
        {
            try
            {
                return Ok(await _TimeInterval.GetCmsComsReminderById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetReminderIntervalById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Abuzar' Date='2024-01-31' Version="1.0" Branch="master"> Get Reminder Interval By Id</History>

        public async Task<IActionResult> GetReminderIntervalById(int Id)
        {
            try
            {
                return Ok(await _TimeInterval.GetReminderIntervalById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetCmsComsReminderType")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Abuzar' Date='2024-02-02' Version="1.0" Branch="master"> Get CmsComs Reminder Types</History>
        public async Task<IActionResult> GetCmsComsReminderType()
        {

            try
            {
                var result = await _TimeInterval.GetCmsComsReminderType();
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get WorkerService Execution Detail

        [HttpPost("GetWorkerServiceExecutionDetail")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Abuzar' Date='2024-02-02' Version="1.0" Branch="master"> Get WorkerService Execution Detail</History>
        public async Task<IActionResult> GetWorkerServiceExecutionDetail(WSExecutionAdvanceSearchVM wSExecutionAdvanceSearchVM)
        {

            try
            {
                var result = await _iWorkerService.GetWorkerServiceExecutionDetail(wSExecutionAdvanceSearchVM);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetWSExecutionStatuses")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Abuzar' Date='2024-02-02' Version="1.0" Branch="master"> Get WorkerService Execution Statuses</History>
        public async Task<IActionResult> GetWSExecutionStatuses()
        {
            try
            {
                return Ok(await _iWorkerService.GetWSExecutionStatuses());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetWorkerServices")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Abuzar' Date='2024-02-02' Version="1.0" Branch="master"> Get WorkerService Execution Statuses</History>
        public async Task<IActionResult> GetWorkerServices()
        {
            try
            {
                return Ok(await _iWorkerService.GetWorkerServices());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Public Holidays
        [MapToApiVersion("1.0")]
        [HttpPost("AddPublicHoliday")]
        public async Task<IActionResult> AddPublicHoliday(PublicHoliday publicHolidays)
        {
            try
            {
                return Ok(await _TimeInterval.AddPublicHoliday(publicHolidays));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
		[MapToApiVersion("1.0")]
		[HttpPost("UpdatePublicHoliday")]
		public async Task<IActionResult> UpdatePublicHoliday(PublicHolidaysVM publicHoliday)
		{
			try
			{
				return Ok(await _TimeInterval.UpdatePublicHoliday(publicHoliday));
			}
			catch (Exception ex)
			{
				return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
			}
		}

		[MapToApiVersion("1.0")]
        [HttpGet("GetPublicHolidays")]
        public async Task<IActionResult> GetPublicHolidays()
        {
            try
            {
                return Ok(await _TimeInterval.GetPublicHolidays());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [MapToApiVersion("1.0")]
        [HttpGet("GetPublicHolidayById")]
        public async Task<IActionResult> GetPublicHolidayById(int id)
        {
            try
            {
                return Ok(await _TimeInterval.GetPublicHolidayById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [MapToApiVersion("1.0")]
        [HttpPost("DeletePublicHoliday")]
        public async Task<IActionResult> DeletePublicHoliday(PublicHolidaysVM publicHoliday)
        {
            try
            {
                return Ok(await _TimeInterval.DeletePublicHoliday(publicHoliday));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
    }
}
