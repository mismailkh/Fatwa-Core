using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Interfaces.Tasks;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models.ViewModel.GeneralVMs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FATWA_API.Controllers.V1.OrganizingCommittee
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrganizingCommitteeController : ControllerBase
    {
        #region Variable Declaration
        private readonly INotification _INotification;
        private readonly IAuditLog _auditLogs;
        private readonly ITask _ITask;

        public ApiReturnTaskNotifAuditLogVM apiReturn { get; set; } = new ApiReturnTaskNotifAuditLogVM();

        #endregion

        #region Constructor

        public OrganizingCommitteeController(IConfiguration configuration, INotification iNotification, IAuditLog auditLogs, ITask task)
        {
            _INotification = iNotification;
            _auditLogs = auditLogs;
            _ITask = task;
        }
        #endregion
    }
}
