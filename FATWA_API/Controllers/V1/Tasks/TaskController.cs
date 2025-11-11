using FATWA_API.RabbitMQ;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Interfaces.Tasks;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.DigitalSignature;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_DOMAIN.Models.WorkflowModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.DmsEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.UserEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V1.Tasks
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TaskController : ControllerBase
    {
        private readonly ITask _ITask;
        private readonly IAuditLog _IAuditLog;
        private readonly IAccount _IAccount;
        private readonly INotification _INotification;
        private readonly IConfiguration _config;
        private readonly RabbitMQClient _client;


        public TaskController(ITask iTask, IAuditLog iAudit, IAccount iAccount, INotification iNotification, IConfiguration config,RabbitMQClient client)
        {
            _ITask = iTask;
            _IAuditLog = iAudit;
            _IAccount = iAccount;
            _INotification = iNotification;
            _config = config;
            _client = client;
        }

        #region GET

        [HttpPost("GetTasksList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTasksList(AdvanceSearchTaskVM advanceSearchVM)
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
                var result = await _ITask.GetTasksList(advanceSearchVM);
                if (result != null)
                {
                    return Ok(new ApiCallResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        IsSuccessStatusCode = true,
                        ResultData = result,
                        Message = "success"
                    });
                }
                return NotFound(new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccessStatusCode = false,
                    ResultData = result,
                    Message = "No_record_found"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccessStatusCode = false,
                    ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message },
                    Message = "Error_Ocurred"
                });
            }
        }


        [HttpPost("GetDraftList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetDraftList(AdvanceSearchDraftVM advanceSearchVM)
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

                return Ok(await _ITask.GetDraftList(advanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }

        [HttpGet("GetTaskById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskById(Guid taskId)
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

                return Ok(await _ITask.GetTaskById(taskId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }

        [HttpGet("GetTaskDetailById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskDetailById(Guid taskId)
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

                return Ok(await _ITask.GetTaskDetailById(taskId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }
        [HttpGet("GetTaskDetailByReferenceAndUserId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskDetailByReferenceAndUserId(Guid referenceId, string userId)
        {
            try
            {
                return Ok(await _ITask.GetTaskDetailByReferenceAndUserId(referenceId, userId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }

        //HISTORY HASSAN IFTIKHAR
        [HttpGet("GetTaskDashBoard")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskDashBoard(string item)
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
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token);
                var tokenS = jsonToken as JwtSecurityToken;
                return Ok(await _ITask.GetTaskDashboard(item, tokenS.Subject));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }

        [HttpGet("GetMaxTaskNumber")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetMaxTaskNumber()
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

                return Ok(await _ITask.GetMaxTaskNumber());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }


        [HttpGet("GetTaskActionsByTaskId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskActionsByTaskId(Guid taskId)
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

                return Ok(await _ITask.GetTaskActionsByTaskId(taskId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }

        [HttpGet("GetTaskResponseStatus")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskResponseStatus()
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

                var result = await _ITask.GetTaskResponseStatus();
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
		[HttpPost("GetTasksListForOSS")]
		[MapToApiVersion("1.0")]
		public async Task<IActionResult> GetTasksListForOSS(AdvanceSearchTaskVM advanceSearchVM)
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
				var result = await _ITask.GetTasksListForOSS(advanceSearchVM);
				if (result != null)
				{
					return Ok(new ApiCallResponse
					{
						StatusCode = HttpStatusCode.OK,
						IsSuccessStatusCode = true,
						ResultData = result,
						Message = "success"
					});
				}
				return NotFound(new ApiCallResponse
				{
					StatusCode = HttpStatusCode.NotFound,
					IsSuccessStatusCode = false,
					ResultData = result,
					Message = "No_record_found"
				});
			}
			catch (Exception ex)
			{
				return BadRequest(new ApiCallResponse
				{
					StatusCode = HttpStatusCode.BadRequest,
					IsSuccessStatusCode = false,
					ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message },
					Message = "Error_Ocurred"
				});
			}
		}
        [HttpGet("GetTaskDetailByIdForOSS")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskDetailByIdForOSS(Guid taskId)
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

                return Ok(await _ITask.GetTaskDetailByIdForOSS(taskId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }
        [HttpPost("GetOSSSystemGeneratedTasks")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetOSSSystemGeneratedTasks(AdvanceSearchTaskVM advanceSearchVM)
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
                var result = await _ITask.GetOSSSystemGeneratedTasks(advanceSearchVM);
                if (result != null)
                {
                    return Ok(new ApiCallResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        IsSuccessStatusCode = true,
                        ResultData = result,
                        Message = "success"
                    });
                }
                return NotFound(new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccessStatusCode = false,
                    ResultData = result,
                    Message = "No_record_found"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccessStatusCode = false,
                    ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message },
                    Message = "Error_Ocurred"
                });
            }
        }

        #endregion

        #region Save

        [HttpPost("AddSystemGeneratedTask")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddSystemGeneratedTask(SaveTaskVM task, string action, string entityName, string entityId)
        {
            try
            {
                var result = await _ITask.AddTask(task, action, entityName, entityId);
                _IAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Add Task",
                    Task = "Add Task",
                    Description = "User able to Add Task successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Add Task executed Successfully",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _IAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Add Task Failed",
                    Body = ex.Message,
                    Category = "User unable to Add Task",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Add Task Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddTask")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddTask(SaveTaskVM task)
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

                task.Task.AssignedBy = await _IAccount.UserIdByUserEmail(task.Task.AssignedBy);

                var result = await _ITask.AddTask(task, null, null, null);
                _IAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Add Task",
                    Task = "Add Task",
                    Description = "User able to Add Task successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Add Task executed Successfully",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _IAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Add Task Failed",
                    Body = ex.Message,
                    Category = "User unable to Add Task",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Add Task Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }


        [HttpPost("EditTask")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> EditTask(SaveTaskVM task)
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

                task.Task.AssignedBy = await _IAccount.UserIdByUserEmail(task.Task.AssignedBy);
                var result = await _ITask.EditTask(task, null, null, null);
                _IAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Edit Task",
                    Task = "Edit Task",
                    Description = "User able to Edit Task successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Edit Task executed Successfully",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _IAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Edit Task Failed",
                    Body = ex.Message,
                    Category = "User unable to Edit Task",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Edit Task Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }

        // HISTORY HASSAN IFTIKHAR
        [HttpPost("SaveToDoList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveToDoList(TaskDashboardVM item)
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

                return Ok(await _ITask.SaveToDoList(item));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region Task Response 

        [HttpPost("AddTaskResponseDecision")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddTaskResponseDecision(TaskResponseVM task)
        {
            bool result;
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                result = await _ITask.SaveTaskResponseDecision(task, false);
                _IAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Add Task Response",
                    Task = "Add Task Response",
                    Description = "User able to Add Task Response successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Add Task Response executed Successfully",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _IAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Update Add Task Response Failed",
                    Body = ex.Message,
                    Category = "User unable to Add Task Response",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Add Task Response Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("EditTaskResponseDecision")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> EditTaskResponseDecision(TaskResponseVM task)
        {
            bool result;
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                result = await _ITask.SaveTaskResponseDecision(task, true);
                _IAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Add Task Response",
                    Task = "Add Task Response",
                    Description = "User able to Add Task Response successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Add Task Response executed Successfully",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _IAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Update Add Task Response Failed",
                    Body = ex.Message,
                    Category = "User unable to Add Task Response",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Add Task Response Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(ex.Message);
            }
        }

        #endregion


        [HttpPost("DecisionTaskByStatusAndRefId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DecisionTaskByStatusAndRefId(TaskDetailVM task)
        {
            bool result;
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                result = await _ITask.DecisionTaskByStatusAndRefId(task);
                _IAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Update Task Status",
                    Task = "Update Task Status",
                    Description = "User able to Update Task Status successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Update Task Status executed Successfully",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _IAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Update Task Status Failed",
                    Body = ex.Message,
                    Category = "User unable to Update Task Status",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Update Task Status Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SaveCaseAssignment")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveCaseAssignment(TaskDetailVM task)
        {
            bool result;
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                result = await _ITask.SaveCaseAssignment(task);
                if (result)
                {
                    //Rabbit MQ send Messages
                    // Update Case Request Status
                    UpdateEntityStatusVM updateEntity = new UpdateEntityStatusVM();
                    updateEntity.ReferenceId = (Guid)task.ReferenceId;
                    updateEntity.StatusId = (int)CaseFileStatusEnum.AssignToLawyer;
                    updateEntity.SubModuleId = (int)SubModuleEnum.CaseFile;
                    _client.SendMessage(updateEntity, RabbitMQKeys.RequestStatusKey);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SaveConsultationAssignment")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveConsultationAssignment(TaskDetailVM task)
        {
            bool result;
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                result = await _ITask.SaveConsultationAssignment(task);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("NotifyTaskAssignedBy")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> NotifyTaskAssignedBy(TaskDetailVM task)
        {
            bool result = false;
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }

                result = await _ITask.RemoveAllTempCaseAssignement(task);
                result = await _ITask.DecisionTask(task);
                if (result)
                {
                    //Rabbit MQ send Messages
                    // Update Case Request Status
                    UpdateEntityStatusVM updateEntity = new UpdateEntityStatusVM();
                    updateEntity.ReferenceId = (Guid)task.ReferenceId;
                    updateEntity.StatusId = (int)CaseFileStatusEnum.InProgress;
                    _client.SendMessage(updateEntity, RabbitMQKeys.RequestStatusKey);
                }

                //To remove Web url from URL
                string url = task.Url.Replace(_config.GetValue<string>("web_url"), "");
                string entityName = url.Split('/')[0].Split('-')[0].ToUpper();
                string entityId = url.Split('/')[1];

                //result = await _INotification.SendNotification(new Notification
                //{
                //    NotificationId = Guid.NewGuid(),
                //    DueDate = DateTime.Now.AddDays(5),
                //    CreatedBy = task.ModifiedBy,
                //    CreatedDate = DateTime.Now,
                //    IsDeleted = false,
                //    ReceiverId = task.AssignedBy,
                //    ReceiverTypeId = (int)NotificationReceiverTypeEnum.User,
                //    ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                //    NotificationTypeId = (int)NotificationTypeEnum.Asynchronous,
                //    NotificationCommunicationMethodId = (int)NotificationChannelEnum.Browser,
                //    NotificationLinkId = Guid.NewGuid(),
                //    NotificationEventId = (int)NotificationEventEnum.Announcement,
                //    NotificationCategoryId = (int)NotificationCategoryEnum.Important
                //},
                //"Task_RejectedBy_Assignee",
                //"view",
                //entityName,
                //entityId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
        #region Decision Task
        [HttpPost("DecisionTask")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DecisionTask(TaskDetailVM task)
        {
            bool result;
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                result = await _ITask.DecisionTask(task);
                _IAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Update Task Status",
                    Task = "Update Task Status",
                    Description = "User able to Update Task Status successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Update Task Status executed Successfully",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _IAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Update Task Status Failed",
                    Body = ex.Message,
                    Category = "User unable to Update Task Status",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Update Task Status Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }
        #endregion
        [HttpPost("ConsultationTaskRejection")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ConsultationTaskRejection(TaskDetailVM task)
        {
            bool result = false;
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }

                result = await _ITask.RemoveAllTempCaseAssignement(task);
                result = await _ITask.DecisionTask(task);
                if (result)
                {
                    //Rabbit MQ send Messages
                    // Update Case Request Status
                    UpdateEntityStatusVM updateEntity = new UpdateEntityStatusVM();
                    updateEntity.ReferenceId = (Guid)task.ReferenceId;
                    updateEntity.StatusId = (int)CaseFileStatusEnum.InProgress;
                    _client.SendMessage(updateEntity, RabbitMQKeys.RequestStatusKey);
                }

                //To remove Web url from URL
                string url = task.Url.Replace(_config.GetValue<string>("web_url"), "");
                string entityName = url.Split('/')[1].Split('-')[0].ToUpper();
                string entityId = url.Split('/')[2];

                //result = await _INotification.SendNotification(new Notification
                //{
                //    NotificationId = Guid.NewGuid(),
                //    DueDate = DateTime.Now.AddDays(5),
                //    CreatedBy = task.ModifiedBy,
                //    CreatedDate = DateTime.Now,
                //    IsDeleted = false,
                //    ReceiverId = task.AssignedBy,
                //    ReceiverTypeId = (int)NotificationReceiverTypeEnum.User,
                //    ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                //    NotificationTypeId = (int)NotificationTypeEnum.Asynchronous,
                //    NotificationCommunicationMethodId = (int)NotificationChannelEnum.Browser,
                //    NotificationLinkId = Guid.NewGuid(),
                //    NotificationEventId = (int)NotificationEventEnum.Announcement,
                //    NotificationCategoryId = (int)NotificationCategoryEnum.Important
                //},
                //"Task_RejectedBy_Assignee",
                //"view",
                //entityName,
                //entityId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //#region Delete Draft
        //// History HAssan Iftikhar
        //[HttpPost("SoftDeleteDraft")]
        //      [MapToApiVersion("1.0")]
        //      public async Task<IActionResult> SoftDeleteDraft(DraftListVM draft)
        //      {
        //          bool result;
        //          try
        //          {
        //              if (!ModelState.IsValid)
        //              {
        //                  return BadRequest(new RequestFailedResponse
        //                  {
        //                      Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
        //                  });
        //              }
        //              result = await _ITask.SoftDeleteDraft(draft);
        //              _IAuditLog.CreateProcessLog(new ProcessLog
        //              {
        //                  Process = "Delete Draft",
        //                  Task = "Delete Draft",
        //                  Description = "User able to Delete Draft successfully.",
        //                  ProcessLogEventId = (int)ProcessLogEnum.Processed,
        //                  Message = "Delete Draft executed Successfully",
        //                  IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
        //                  ApplicationID = (int)PortalEnum.FatwaPortal,
        //                  ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
        //                  Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
        //              });

        //              return Ok(result);
        //          }
        //          catch (Exception ex)
        //          {
        //              _IAuditLog.CreateErrorLog(new ErrorLog
        //              {
        //                  ErrorLogEventId = (int)ErrorLogEnum.Error,
        //                  Subject = "Delete Draft Failed",
        //                  Body = ex.Message,
        //                  Category = "User unable to Delete Draft",
        //                  Source = ex.Source,
        //                  Type = ex.GetType().Name,
        //                  Message = "Delete Draft Failed",
        //                  IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
        //                  ApplicationID = (int)PortalEnum.FatwaPortal,
        //                  ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
        //                  Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
        //              });

        //              return BadRequest(ex.Message);
        //          }
        //      }
        //      #endregion

        #region Reject Reason


        [HttpPost("RejectReason")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> RejectReason(RejectReason reject)
        {
            bool result;
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                result = await _ITask.RejectReason(reject);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        #endregion

        #region Cms Task List
        //<History Author = "Hassan Abbas" Date="2023-03-08" Version="1.0" Branch="master">List of Tasks related to Case Management filtered based on screen, task type and submodule</History>
        [HttpPost("GetCmsTasksList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCmsTasksList(AdvanceSearchTaskVM advanceSearchVM)
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

                return Ok(await _ITask.GetCmsTasksList(advanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //<History Author = "Hassan Abbas" Date="2023-04-18" Version="1.0" Branch="master">List of Tasks related to Case Management</History>
        [HttpPost("GetAllCmsTasks")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllCmsTasks(AdvanceSearchTaskVM advanceSearchVM)
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
                var result = await _ITask.GetAllCmsTasks(advanceSearchVM);
                if (result != null)
                {
                    return Ok(new ApiCallResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        IsSuccessStatusCode = true,
                        ResultData = result,
                        Message = "success"
                    });
                }
                return NotFound(new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccessStatusCode = false,
                    ResultData = result,
                    Message = "No_record_found"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccessStatusCode = false,
                    ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message },
                    Message = "Error_Ocurred"
                });
            }
        }

        //<History Author = "Hassan Abbas" Date="2023-03-08" Version="1.0" Branch="master">List of Tasks related to Case Management filtered based on screen, task type and submodule</History>
        [HttpPost("GetCountCmsTasksList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCountCmsTasksList(AdvanceSearchTaskVM advanceSearchVM)
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

                return Ok(await _ITask.GetCountCmsTasksList(advanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #endregion

        #region Consultation Task List
        //<History Author = "Hassan Abbas" Date="2023-03-08" Version="1.0" Branch="master">List of Tasks related to Case Management filtered based on screen, task type and submodule</History>
        [HttpPost("GetComsTasksList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetComsTasksList(AdvanceSearchTaskVM advanceSearchVM)
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

                return Ok(await _ITask.GetComsTasksList(advanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Get Count Consultation Tasks List
        //<History Author = "Muhammad Zaeem" Date="2023-03-14" Version="1.0" Branch="master">List of Tasks related to Consultation Management filtered based on screen, task type and submodule</History>
        [HttpPost("GetCountComsTasksList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCountComsTasksList(AdvanceSearchTaskVM advanceSearchVM)
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

                return Ok(await _ITask.GetCountComsTasksList(advanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get All Task list for consultation 
        //<History Author = "Muhammad Zaeem" Date="2023-04-19" Version="1.0" Branch="master">List of Tasks related to Consultation Management</History>
        [HttpPost("GetAllComsTasks")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllComsTasks(AdvanceSearchTaskVM advanceSearchVM)
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
                var result = await _ITask.GetAllComsTasks(advanceSearchVM);
                if (result != null)
                {
                    return Ok(new ApiCallResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        IsSuccessStatusCode = true,
                        ResultData = result,
                        Message = "success"
                    });
                }
                return NotFound(new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccessStatusCode = false,
                    ResultData = result,
                    Message = "No_record_found"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccessStatusCode = false,
                    ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message },
                    Message = "Error_Ocurred"
                });
            }
        }
        #endregion

        [HttpGet("GetTaskListByFileId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskListByFileId(Guid referenceId)
        {
            try
            {
                return Ok(await _ITask.GetTaskListByFileId(referenceId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }

        #region Get All Task list for Case/consultation 
        // Get All Task list for Case/consultation  for POS/FPO
        [HttpPost("GetAllCMSComsTasks")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllCMSComsTasks(AdvanceSearchTaskVM advanceSearchVM)
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
                var result = await _ITask.GetAllCMSComsTasks(advanceSearchVM);
                if (result != null)
                {
                    return Ok(new ApiCallResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        IsSuccessStatusCode = true,
                        ResultData = result,
                        Message = "success"
                    });
                }
                return NotFound(new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccessStatusCode = false,
                    ResultData = result,
                    Message = "No_record_found"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccessStatusCode = false,
                    ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message },
                    Message = "Error_Ocurred"
                });
            }
        }
        #endregion

        #region Task Update Status With Service 
        [HttpGet("TaskUpdateWithService")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> TaskUpdateWithService(Guid ReferenceId, string userId)
        {

            try
            {
                var userids = await _IAccount.UserIdByUserEmail(userId);
                if (userids != null)
                {
                    var task = await _ITask.GetTaskDetailByReferenceAndAssignedToId(ReferenceId, userids);
                    if (task != null)
                    {
                        if (task.TaskStatusId == (int)TaskStatusEnum.Pending)
                        {
                            return Ok(await _ITask.UpdateTaskStatus(task.TaskId, (int)TaskStatusEnum.Done));
                        }
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion

        #region Get Task by referenceId and assigned to Id
        [HttpGet("GetTaskDetailByReferenceAndAssignedToId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskDetailByReferenceAndAssignedToId(Guid ReferenceId, string userId)
        {

            try
            {
                return Ok(await _ITask.GetTaskDetailByReferenceAndAssignedToId(ReferenceId, userId));

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }
        #endregion

        #region Complete All Pending Tasks 
        [HttpPost("CompleteAllPendingTasks")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CompleteAllPendingTasks(Guid RequestId, string User)
        {
            try
            {
                await _ITask.CompleteAllPendingTasks(RequestId, User);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #region Complete Assign Tasks 
        [HttpPost("CompleteAssignTask")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CompleteAssignTask(Guid FileId)
        {
            try
            {
                await _ITask.CompleteAssignTask(FileId);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        [HttpGet("GetTaskDetailByFileId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskDetailByFileId(Guid fileId)
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

                return Ok(await _ITask.GetTaskDetailByFileId(fileId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }

        [HttpGet("ApproveTaskByReferenceId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ApproveTaskByReferenceId(Guid MeetingId, string User, bool IsViceHos)
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

                return Ok(await _ITask.ApproveTaskByReferenceId(MeetingId, User, IsViceHos));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }
        [HttpGet("GetTaskEntityHistoryByReferenceIdAndSubmodule")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskEntityHistoryByReferenceIdAndSubmodule(Guid referenceId, int submoduleId)
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
                return Ok(await _ITask.GetTaskEntityHistoryByReferenceIdAndSubmodule(referenceId, submoduleId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }
        #region Mobile App
        [HttpPost("GetAllTasksListForMobileApp")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllTasksListForMobileApp(AdvanceSearchTaskMobileAppVM advanceSearchVM)
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
                var result = await _ITask.GetAllTasksListForMobileApp(advanceSearchVM);
                if (result != null && result.Count > 0)
                {
                    return Ok(new ApiCallResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        IsSuccessStatusCode = true,
                        ResultData = result,
                        Message = "success"
                    });
                }
                return NotFound(new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccessStatusCode = false,
                    ResultData = result,
                    Message = "No_record_found"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccessStatusCode = false,
                    ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message },
                    Message = "Error_Ocurred"
                });
            }
        }
        [HttpGet("GetTasksDetailsForMobileApp")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTasksDetailsForMobileApp([Required] string taskId, [Required] string cultureType)
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
                var result = await _ITask.GetTasksDetailsForMobileApp(taskId, cultureType);
                if (result != null)
                {
                    return Ok(new ApiCallResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        IsSuccessStatusCode = true,
                        ResultData = result,
                        Message = "success"
                    });
                }
                return NotFound(new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccessStatusCode = false,
                    ResultData = result,
                    Message = "No_record_found"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccessStatusCode = false,
                    ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message },
                    Message = "Error_Ocurred"
                });
            }
        }
        #endregion
        [HttpGet("AddOrUpdateUserTaskViewTime")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddOrUpdateUserTaskViewTime(string userId, Guid? referenceId)
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

                return Ok(await _ITask.AddOrUpdateUserTaskViewTime(Guid.Parse(userId), referenceId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }

        #region Save Task List
        [HttpPost("AddTaskList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddTaskList(List<SaveTaskVM> task)
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
                return Ok(await _ITask.AddTaskList(task));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }

        #endregion

        [HttpPost("CreateTaskForSignature")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateTaskForSignature(DsSigningRequestTaskLog taskForDS)
        {
            try
            {
                await _ITask.CreateTaskForSignature(taskForDS);
                var taskIds = Guid.NewGuid();
                await _ITask.AddTask(new SaveTaskVM
                {
                    Task = new UserTask
                    {
                        TaskId = taskIds,
                        Name = "Review_Document_For_DS_Task",
                        Description = taskForDS.Remarks,
                        Date = DateTime.Now.Date,
                        AssignedBy = taskForDS.SenderId,
                        AssignedTo = taskForDS.ReceiverId,
                        IsSystemGenerated = true,
                        TaskStatusId = (int)TaskStatusEnum.Pending,
                        ModuleId = taskForDS.ModuleId,
                        SectorId = taskForDS.SectorTypeId,
                        DepartmentId = (int)DepartmentEnum.Operational,
                        TypeId = (int)TaskTypeEnum.Task,
                        RoleId = "d6b3075c-3f70-4b44-baa4-1fdc599a6bb2", //FATWA ADMIN
                        CreatedBy = taskForDS.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReferenceId = Guid.Parse(taskForDS.ReferenceId),
                        SubModuleId = taskForDS.SubModuleId,
                        SystemGenTypeId = 1,
                        EntityId = null
                    },
                    TaskActions = new List<TaskAction>()
                    {
                        new TaskAction()
                        {
                            ActionName = "Review Draft Document",
                            TaskId = taskIds,
                        }
                    }
                },
                "documentForSigning",
                "review",
                taskForDS.SigningTaskId.ToString() + '/' + taskForDS.SubModuleId);
                var notificationResponse = await _INotification.SendNotification(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = taskForDS.CreatedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = taskForDS.ReceiverId,// Assign To  Id
                    ModuleId = taskForDS.ModuleId,
                },
                (int)NotificationEventEnum.DocumentSendForSigning,
                "documentForSigning",
                "review",
                taskForDS.SigningTaskId.ToString() + '/' + taskForDS.SubModuleId + '/'+ taskIds,
                taskForDS.NotificationParameter);

                _IAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Task Send For Digital Signature Signing",
                    Task = "Create Task For DS Signing Process",
                    Description = "Digital Signature Signing Task has been created Successfully",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Task Successfully Created For Digital Signature Signing", 
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.DigitalSignature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _IAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Create Task For Digital Signature Signing Failed ",
                    Body = ex.Message,
                    Category = "Task Creation Failed for Digital Signature Signing Process",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Task Creation Failed For Digital Signature Signing",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }

        [HttpGet("GetTaskForSignature")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskForSignature(Guid SigningTaskId)
        {
            try
            {
                
                return Ok(await _ITask.GetTaskForSignature(SigningTaskId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }

        [HttpPost("UpdateTaskForSignature")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateTaskForSignature(DsSigningRequestTaskLog taskForDS)
        {
            try
            {
                await _ITask.UpdateTaskForSignature(taskForDS);
                if(taskForDS.StatusId == (int)SigningTaskStatusEnum.Signed)
                {
                    var notificationResponse = await _INotification.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = taskForDS.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = taskForDS.SenderId,// Assign To  Id
                        ModuleId = taskForDS.ModuleId,
                    },
                   (int)NotificationEventEnum.DocumentReceiveAfterSigning,
                   "documentForSigning",
                   "review",
                   taskForDS.SigningTaskId.ToString(),
                   taskForDS.NotificationParameter);
                }
                _IAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Action has been taken on Digital Signature Signing Task",
                    Task = "Update Task For DS Signing Process",
                    Description = "Digital Signature Signing Task/Status has been Updated Successfully",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Digital Signature Signing Process Staus has been change",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.DigitalSignature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _IAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Update Task For DS Signing Failed ",
                    Body = ex.Message,
                    Category = "Task Updating Failed for Signing Process",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Action has been taken on Digital Signature Signing Task Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("GetAllTasksForSignature")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllTasksForSignature(int DocumentId)
        {
            try
            {
                return Ok(await _ITask.GetAllTasksForSignature(DocumentId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetTaskResponseByTasktId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskResponseByTasktId(Guid Id)
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
                return Ok(await _ITask.GetTaskResponseByTasktId(Id));

            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }
    }
}
