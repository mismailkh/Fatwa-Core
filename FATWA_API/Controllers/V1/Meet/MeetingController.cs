using FATWA_API.RabbitMQ;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.Communication;
using FATWA_DOMAIN.Interfaces.Meet;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Interfaces.Tasks;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_GENERAL.Helper;
using FATWA_INFRASTRUCTURE.Repository.Communications;
using G2GTarasolServiceReference;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Net;
using System.Threading.Tasks;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.MeetingEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.UserEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V1.Meet
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + ",FatwaApiKey")]
    public class MeetingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMeeting _IMeeting;
        private readonly IAuditLog _AuditLog;
        private readonly INotification _INotification;
        private readonly ITask _ITask;
        private readonly IRole _IRole;
        private readonly IAccount _IAccount;
        private readonly ITempFileUpload _IFileUpload;
        private readonly RabbitMQClient _client;
        private readonly ICommunicationTarasolRPA _ICommunicationTarasolRPA;

        public MeetingController(IMeeting iMeeting, IAuditLog audit, INotification iNotification, ITask iTask, IConfiguration configuration, IRole iRole, 
            IAccount iAccount, ICommunicationTarasolRPA iCommunicationTarasolRPA, ITempFileUpload IFileUpload, RabbitMQClient client)
        {
            _IMeeting = iMeeting;
            _AuditLog = audit;
            _INotification = iNotification;
            _ITask = iTask;
            _configuration = configuration;
            _IRole = iRole;
            _IAccount = iAccount;
            _ICommunicationTarasolRPA = iCommunicationTarasolRPA;
            _IFileUpload = IFileUpload;
            _client = client;
        }

        #region GET

        [HttpGet("GetMeetingsList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetMeetingsList(string userName, int PageSize, int PageNumber)
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

                return Ok(await _IMeeting.GetMeetingsList(userName, PageSize, PageNumber));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetMeetingById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetMeetingById(Guid MeetingId)
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

                var result = await _IMeeting.GetMeetingById(MeetingId);
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

        [HttpGet("GetMeetingDecisionDetailById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetMeetingDecisionDetailById(Guid meetingId, int sectorId)
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

                var result = await _IMeeting.GetMeetingDecisionDetailById(meetingId, sectorId);
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

        [HttpGet("GetMeetingMOMByMeetingId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetMeetingMOMByMeetingId(Guid MeetingId)
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

                var result = await _IMeeting.GetMeetingMOMByMeetingId(MeetingId);
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
        #endregion

        #region Save

        [HttpPost("AddMeeting")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddMeeting(SaveMeetingVM meetingVM)
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

                if (meetingVM.isSaveAsDraft)
                {
                    meetingVM.Meeting.MeetingStatusId = (int)MeetingStatusEnum.SaveAsDraft;
                    var res = await _IMeeting.CheckDraftExixt(meetingVM.Meeting.MeetingId);
                    if (res)

                    {
                        await _IMeeting.EditsMeeting(meetingVM);
                    }
                    else
                    {
                        await _IMeeting.AddMeeting(meetingVM);
                    }

                    return Ok();
                }
                else
                {
                    //var assignedTo = await _IRole.GetHOSBySectorId((int)meetingVM.Meeting.SectorTypeId);
                    //meetingVM.Meeting.ApprovalReqBy = Guid.Parse(assignedTo.Id);
                    var result = await _IMeeting.AddMeeting(meetingVM);

                    if (result != null)
                    {
                        if (result.Meeting.MeetingTypeId == (int)MeetingTypeEnum.External && result.Meeting.IsApproved == true)
                        {
                            //Rabbit MQ send Messages
                            _client.SendMessage(result, RabbitMQKeys.MeetingScheduleKey);
                            _AuditLog.CreateProcessLog(new ProcessLog
                            {
                                Process = "Update Meeting Status",
                                Task = "Update Meeting Status",
                                Description = "User able to Update Meeting Status successfully.",
                                ProcessLogEventId = (int)ProcessLogEnum.Processed,
                                Message = "Update Meeting Status executed Successfully",
                                IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                                ApplicationID = (int)PortalEnum.FatwaPortal,
                                ModuleID = (int)WorkflowModuleEnum.Meeting,
                                Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                            });
                            return Ok();
                        }
                    }

                    var Attendees = meetingVM.LegislationAttendee;

                    foreach (var file in Attendees)
                    {
                        if (file.AttendeeStatusId == (int)MeetingAttendeeStatusEnum.New)
                        {
                            var taskId = Guid.NewGuid();
                            string AssignedBy = await _IAccount.UserIdByUserEmail(meetingVM.Meeting.CreatedBy);
                            int submodule = 0;
                            if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.CaseFile || meetingVM.Meeting.SubModulId == (int)SubModuleEnum.RegisteredCase)
                            {
                                if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.CaseFile)
                                {
                                    submodule = (int)SubModuleEnum.CaseFile;
                                }
                                else if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.RegisteredCase)
                                {
                                    submodule = (int)SubModuleEnum.RegisteredCase;
                                }
                                else
                                {
                                    submodule = (int)SubModuleEnum.CaseRequest;
                                }
                            }
                            else if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.ConsultationFile || meetingVM.Meeting.SubModulId == (int)SubModuleEnum.ConsultationRequest)
                            {
                                if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.ConsultationFile)
                                {
                                    submodule = (int)SubModuleEnum.ConsultationFile;
                                }
                                else
                                {
                                    submodule = (int)SubModuleEnum.ConsultationRequest;
                                }
                            }
                            else if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.OrganizingCommittee)
                            {
                                submodule = (int)SubModuleEnum.OrganizingCommittee;
                            }
                            var taskResult = await _ITask.AddTask(new SaveTaskVM
                            {
                                Task = new UserTask
                                {
                                    TaskId = taskId,
                                    Name = "Save_Meeting_Task",
                                    Date = DateTime.Now.Date,
                                    AssignedBy = AssignedBy,//FATWA ADMIN
                                    AssignedTo = file.Id,//FATWA ADMIN
                                    IsSystemGenerated = true,
                                    TaskStatusId = (int)TaskStatusEnum.Pending,
                                    ModuleId = (int)WorkflowModuleEnum.Meeting,
                                    SectorId = (int)meetingVM.Meeting.SectorTypeId,
                                    DepartmentId = (int)DepartmentEnum.Operational,
                                    TypeId = (int)TaskTypeEnum.Task,
                                    RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                                    CreatedBy = meetingVM.Meeting.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    IsDeleted = false,
                                    ReferenceId = meetingVM.Meeting.MeetingId,
                                    SubModuleId = submodule,
                                    SystemGenTypeId = (int)TaskSystemGenTypeEnum.Meeting,
                                },
                                TaskActions = new List<TaskAction>()
                                {
                                    new TaskAction()
                                    {
                                        ActionName = "Meeting Action",
                                        TaskId = taskId,
                                    }
                                }
                            },
                            "attendeedecision",
                            "meeting",
                             meetingVM.Meeting.MeetingId.ToString());
                            var notificationResult = await _INotification.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = meetingVM.Meeting.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = file.Id,
                                ModuleId = (int)WorkflowModuleEnum.Meeting,
                            },
                            (int)NotificationEventEnum.AttendeeDecisionForMeeting,
                            "attendeedecision",
                            "meeting",
                            meetingVM.Meeting.MeetingId.ToString() + "/" + taskId,
                            meetingVM.NotificationParameter);
                        }
                    }
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _AuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Create Meeting Failed",
                    Body = ex.Message,
                    Category = "User unable to Create Meeting",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Create Meeting Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }
        private async Task<DataTable> GetAttachmentsForTarasolCommunication(Guid meetingId)
        {
            DataTable DT = new DataTable("Attachments");
            DataColumn colString = new DataColumn("AttName");
            colString.DataType = System.Type.GetType("System.String");
            DT.Columns.Add(colString);
            DataColumn colByteArray = new DataColumn("Attachment");
            colByteArray.DataType = System.Type.GetType("System.Byte[]");
            DT.Columns.Add(colByteArray);

            var attachments = await _IFileUpload.GetUploadedAttachementsByReferenceGuid(meetingId);

            if (attachments.Any())
            {
                foreach (var attachment in attachments)
                {
                    DataRow DR = DT.NewRow();
                    DR["AttName"] = attachment.FileName;

#if DEBUG
                    var physicalPath = Path.Combine(_configuration.GetValue<string>("dms_file_path") + attachment.StoragePath).Replace(@"\\", @"\");
                    DR["Attachment"] = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, attachment.DocType, _configuration.GetValue<string>("DocumentEncryptionKey"));
#else
                    var physicalPath = Path.Combine(_configuration.GetValue<string>("dms_file_path") + attachment.StoragePath).Replace(@"\\", @"\");
                    physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                    DR["Attachment"] = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, attachment.DocType, _configuration.GetValue<string>("DocumentEncryptionKey"));
#endif

                    DT.Rows.Add(DR);
                }
            }
            return DT;
        }

        private async Task SendCommunicationToTarassol(Guid communicationId, Guid meetingId, Guid referenceGuid, int submoduleId, string subject)
        {
            try
            {
                CommunicationTarassolSendVM tarassolSend = new();
                var communication = await _ICommunicationTarasolRPA.GetCommunicationByCommunicationId(communicationId);
                List<SendCommunicationVM> sendCommunication = new List<SendCommunicationVM>();
                sendCommunication.Add(new SendCommunicationVM
                {
                    Communication = communication,
                    CommunicationResponse = new CommunicationResponse(),
                });
                sendCommunication.FirstOrDefault().Communication.GovtEntityId = await _ICommunicationTarasolRPA.GetGovernmentEntitiyByReferenceAndSubmoduleId(referenceGuid, submoduleId);
                var depts = await _IRole.GetDepartmentsByGEId(sendCommunication);
                var attachments = await GetAttachmentsForTarasolCommunication(meetingId);
                foreach (var department in depts)
                {
                    var user = _IAccount.GetUserByUserEmail(communication.CreatedBy);
                    tarassolSend.RBrSiteId = department.G2GBRSiteID != null ? (int)department.G2GBRSiteID : 0;
                    tarassolSend.SenderUser = user.ADUserName != null ? user.ADUserName : "";
                    tarassolSend.SBrSiteId = department.SenderBranchId != null ? (int)department.SenderBranchId : 0;
                    var client = new G2GIWSSoapClient(G2GTarasolServiceReference.G2GIWSSoapClient.EndpointConfiguration.G2GIWSSoap);
                    var result = await client.G2G_SendOutGoingDocument(tarassolSend.SenderUser, "إدارة الفتوى والتشريع", department.RecieverSiteName, "", communication.OutboxNumber, subject, tarassolSend.SBrSiteId, tarassolSend.RBrSiteId, attachments);

                    _AuditLog.CreateProcessLog(new ProcessLog
                    {
                        Process = "Sent Outgoing Correspondence to G2G Tarasol",
                        Task = "Sent Outgoing Correspondence to G2G Tarasol",
                        Description = "Send Outgoing Correspondence to G2G Tarasol",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = result,
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)ModuleEnum.CaseManagement,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                }
            }
            catch (Exception ex)
            {
                _AuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Unable to send Outgoing Correspondence to G2G Tarasol",
                    Body = ex.Message,
                    Category = "Unable to send Outgoing Correspondence to G2G Tarasol",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = ex.InnerException?.Message,
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
            }

        }

        [HttpPost("EditMeeting")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> EditMeeting(SaveMeetingVM meetingVM)
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
                if (meetingVM.isSaveAsDraft)
                {
                    meetingVM.Meeting.MeetingStatusId = (int)MeetingStatusEnum.SaveAsDraft;
                    await _IMeeting.EditsMeeting(meetingVM);

                    return Ok();
                }
                else
                {
                    var result = await _IMeeting.EditsMeeting(meetingVM);
                    if (meetingVM.Meeting.MeetingTypeId == (int)MeetingTypeEnum.External && meetingVM.Meeting.IsApproved == true)
                    {
                        //send G2G Tarasol Correspondence
#if !DEBUG
                        await SendCommunicationToTarassol(result.Communication.CommunicationId, meetingVM.Meeting.MeetingId, (Guid)meetingVM.Meeting.ReferenceGuid, meetingVM.Meeting.SubModulId, meetingVM.Meeting.Subject);
#endif
                        //Rabbit MQ send Messages
                        _client.SendMessage(result, RabbitMQKeys.MeetingScheduleKey);
                        _AuditLog.CreateProcessLog(new ProcessLog
                        {
                            Process = "Update Meeting Status",
                            Task = "Update Meeting Status",
                            Description = "User able to Update Meeting Status successfully.",
                            ProcessLogEventId = (int)ProcessLogEnum.Processed,
                            Message = "Update Meeting Status executed Successfully",
                            IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                            ApplicationID = (int)PortalEnum.FatwaPortal,
                            ModuleID = (int)WorkflowModuleEnum.Meeting,
                            Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                        });
                        foreach (var attendee in meetingVM.LegislationAttendee)
                        {
                            var Hos = await _IRole.GetHOSBySectorId((int)meetingVM.Meeting.SectorTypeId);

                            if (attendee.Id != Hos.Id)
                            {

                                var notificationResult = await _INotification.SendNotification(new Notification
                                {
                                    NotificationId = Guid.NewGuid(),
                                    DueDate = DateTime.Now.AddDays(5),
                                    CreatedBy = meetingVM.Meeting.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    IsDeleted = false,
                                    ReceiverId = attendee.Id,
                                    ModuleId = (int)WorkflowModuleEnum.Meeting,
                                },
            (int)NotificationEventEnum.AddMeetingSuccess,

          "view",
                             "meeting",
                              meetingVM.Meeting.MeetingId.ToString(),
                             meetingVM.NotificationParameter);


                            }
                        }

                        return Ok();
                    }
                    var Attendees = meetingVM.LegislationAttendee;

                    foreach (var file in Attendees)
                    {
                        if (file.AttendeeStatusId == (int)MeetingAttendeeStatusEnum.New)
                        {
                            var taskId = Guid.NewGuid();
                            string AssignedBy = await _IAccount.UserIdByUserEmail(meetingVM.Meeting.CreatedBy);
                            int submodule = 0;
                            if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.CaseFile || meetingVM.Meeting.SubModulId == (int)SubModuleEnum.CaseRequest)
                            {
                                if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.CaseFile)
                                {
                                    submodule = (int)SubModuleEnum.CaseFile;
                                }
                                else
                                {
                                    submodule = (int)SubModuleEnum.RegisteredCase;
                                }
                            }
                            else if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.ConsultationFile || meetingVM.Meeting.SubModulId == (int)SubModuleEnum.ConsultationRequest)
                            {
                                if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.ConsultationFile)
                                {
                                    submodule = (int)SubModuleEnum.ConsultationFile;
                                }
                                else
                                {
                                    submodule = (int)SubModuleEnum.ConsultationRequest;
                                }
                            }
                            else if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.OrganizingCommittee)
                            {
                                submodule = (int)SubModuleEnum.OrganizingCommittee;
                            }
                            var taskResult = await _ITask.AddTask(new SaveTaskVM
                            {
                                Task = new UserTask
                                {
                                    TaskId = taskId,
                                    Name = "Save_Meeting_Task",
                                    Date = DateTime.Now.Date,
                                    AssignedBy = AssignedBy,
                                    AssignedTo = file.Id,
                                    IsSystemGenerated = true,
                                    TaskStatusId = (int)TaskStatusEnum.Pending,
                                    ModuleId = (int)WorkflowModuleEnum.Meeting,
                                    SectorId = (int)meetingVM.Meeting.SectorTypeId,
                                    DepartmentId = (int)DepartmentEnum.Operational,
                                    TypeId = (int)TaskTypeEnum.Task,
                                    RoleId = SystemRoles.FatwaAdmin,
                                    CreatedBy = meetingVM.Meeting.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    IsDeleted = false,
                                    ReferenceId = meetingVM.Meeting.MeetingId,
                                    SubModuleId = submodule,
                                    SystemGenTypeId = (int)TaskSystemGenTypeEnum.Meeting,
                                },
                                TaskActions = new List<TaskAction>()
                    {
                        new TaskAction()
                        {
                            ActionName = "Meeting Action",
                            TaskId = taskId,
                        }
                    }
                            },
                            "attendeedecision",
                            "meeting",
                             meetingVM.Meeting.MeetingId.ToString());



                            var notification = await _INotification.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = meetingVM.Meeting.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = file.Id,
                                ModuleId = (int)WorkflowModuleEnum.Meeting,
                            },
        (int)NotificationEventEnum.AttendeeDecisionForMeeting,
     "attendeedecision",
                            "meeting",
                          meetingVM.Meeting.MeetingId.ToString(),
                         meetingVM.NotificationParameter);

                        }


                    }

                    if (meetingVM.DeletedLegislationAttendeeIds.Any())
                    {
                        string createdBy = string.Empty;
                        if (meetingVM.Meeting.ModifiedBy != null)
                        {
                            createdBy = meetingVM.Meeting.ModifiedBy;
                        }
                        else
                        {
                            createdBy = meetingVM.Meeting.CreatedBy;
                        }
                        foreach (var item in meetingVM.DeletedLegislationAttendeeIds)
                        {



                            var notificationResult = await _INotification.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = meetingVM.Meeting.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = item.ToString(),
                                ModuleId = (int)WorkflowModuleEnum.Meeting,
                            },
        (int)NotificationEventEnum.DeleteAttendeeFromMeeting,
      "list",
                         "meeting",
                          meetingVM.Meeting.MeetingId.ToString(),
                         meetingVM.NotificationParameter);
                        }
                    }

                    if (meetingVM.Meeting.IsSendToHOS != true)
                    {
                        _AuditLog.CreateProcessLog(new ProcessLog
                        {
                            Process = "Edit Meeting",
                            Task = "Edit Meeting",
                            Description = "User able to Edit Meeting successfully.",
                            ProcessLogEventId = (int)ProcessLogEnum.Processed,
                            Message = "Edit Meeting executed Successfully",
                            IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                            ApplicationID = (int)PortalEnum.FatwaPortal,
                            ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                            Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                        });
                        foreach (var attendee in meetingVM.LegislationAttendee)
                        {

                            var notificationResult = await _INotification.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = meetingVM.Meeting.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = attendee.Id,
                                ModuleId = (int)WorkflowModuleEnum.Meeting,
                            },
 (int)NotificationEventEnum.EditMeetingSuccess,
"view",
                     "meeting",
                   meetingVM.Meeting.MeetingId.ToString(),
                  meetingVM.NotificationParameter);

                        }

                    }
                    if (result != null)
                    {
                        if ((bool)result.Meeting.IsSendToHOS)

                        {
                            var assignedTo = await _IRole.GetHOSBySectorId((int)meetingVM.Meeting.SectorTypeId);
                            //meetingVM.Meeting.ApprovalReqBy = Guid.Parse(assignedTo.Id);
                            var taskId = Guid.NewGuid();
                            string AssignedBy = await _IAccount.UserIdByUserEmail(meetingVM.Meeting.CreatedBy);
                            int submodule = 0;
                            if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.CaseFile || meetingVM.Meeting.SubModulId == (int)SubModuleEnum.RegisteredCase)
                            {
                                if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.CaseFile)
                                {
                                    submodule = (int)SubModuleEnum.CaseFile;
                                }
                                else
                                {
                                    submodule = (int)SubModuleEnum.RegisteredCase;
                                }
                            }
                            else if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.ConsultationFile || meetingVM.Meeting.SubModulId == (int)SubModuleEnum.ConsultationRequest)
                            {
                                if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.ConsultationFile)
                                {
                                    submodule = (int)SubModuleEnum.ConsultationFile;
                                }
                                else
                                {
                                    submodule = (int)SubModuleEnum.ConsultationRequest;
                                }
                            }
                            else if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.OrganizingCommittee)
                            {
                                submodule = (int)SubModuleEnum.OrganizingCommittee;
                            }
                            var taskResult = await _ITask.AddTask(new SaveTaskVM
                            {
                                Task = new UserTask
                                {
                                    TaskId = taskId,
                                    Name = "Save_Meeting_Task",
                                    Date = DateTime.Now.Date,
                                    AssignedBy = AssignedBy,
                                    AssignedTo = assignedTo.Id,
                                    IsSystemGenerated = true,
                                    TaskStatusId = (int)TaskStatusEnum.Pending,
                                    ModuleId = (int)WorkflowModuleEnum.Meeting,
                                    SectorId = (int)meetingVM.Meeting.SectorTypeId,
                                    DepartmentId = (int)DepartmentEnum.Operational,
                                    TypeId = (int)TaskTypeEnum.Task,
                                    RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                                    CreatedBy = meetingVM.Meeting.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    IsDeleted = false,
                                    ReferenceId = meetingVM.Meeting.MeetingId,
                                    SubModuleId = submodule,
                                    SystemGenTypeId = (int)TaskSystemGenTypeEnum.MeetingSendToHos,
                                },
                                TaskActions = new List<TaskAction>()
                    {
                        new TaskAction()
                        {
                            ActionName = "Meeting Action",
                            TaskId = taskId,
                        }
                    }
                            },
                                 "decision",
                                 "meeting",
                                  meetingVM.Meeting.MeetingId.ToString());
                            _AuditLog.CreateProcessLog(new ProcessLog
                            {
                                Process = "Send To HOS",
                                Task = "Send To HOS",
                                Description = "User able to Send Meeting To HOS successfully.",
                                ProcessLogEventId = (int)ProcessLogEnum.Processed,
                                Message = "Edit Meeting executed Successfully",
                                IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                                ApplicationID = (int)PortalEnum.FatwaPortal,
                                ModuleID = (int)WorkflowModuleEnum.Meeting,
                                Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                            });
                            var notificationResult = await _INotification.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = meetingVM.Meeting.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = meetingVM.ToString(),
                                ModuleId = (int)WorkflowModuleEnum.Meeting,
                            },
          (int)NotificationEventEnum.MeeetingDecisionOfHOSForApproval,
        "decision",
                          "meeting",
                            meetingVM.Meeting.MeetingId.ToString() + "/" + taskId,
                           meetingVM.NotificationParameter);


                        }
                    }
                }

                if (meetingVM.Meeting.MeetingStatusId != (int)MeetingStatusEnum.ApprovedByViceHos)
                {
                    if ((bool)meetingVM.Meeting.IsSendToHOS)
                    {
                        var assignedToUsers = await _IRole.GetViceHOSOrManagerBySectorUserId((int)meetingVM.Meeting.SectorTypeId, meetingVM.Meeting.CreatedBy);
                        //meetingVM.Meeting.ApprovalReqBy = Guid.Parse(assignedTo.Id);
                        string AssignedBy = await _IAccount.UserIdByUserEmail(meetingVM.Meeting.CreatedBy);
                        int submodule = 0;
                        if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.CaseFile || meetingVM.Meeting.SubModulId == (int)SubModuleEnum.RegisteredCase)
                        {
                            if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.CaseFile)
                            {
                                submodule = (int)SubModuleEnum.CaseFile;
                            }
                            else
                            {
                                submodule = (int)SubModuleEnum.RegisteredCase;
                            }
                        }
                        else if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.ConsultationFile || meetingVM.Meeting.SubModulId == (int)SubModuleEnum.ConsultationRequest)
                        {
                            if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.ConsultationFile)
                            {
                                submodule = (int)SubModuleEnum.ConsultationFile;
                            }
                            else
                            {
                                submodule = (int)SubModuleEnum.ConsultationRequest;
                            }
                        }
                        else if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.OrganizingCommittee)
                        {
                            submodule = (int)SubModuleEnum.OrganizingCommittee;
                        }
                        foreach (var assignedTo in assignedToUsers)
                        {
                            var taskId = Guid.NewGuid();

                            var taskResult = await _ITask.AddTask(new SaveTaskVM
                            {
                                Task = new UserTask
                                {
                                    TaskId = taskId,
                                    Name = "Save_Meeting_Task",
                                    Date = DateTime.Now.Date,
                                    AssignedBy = AssignedBy,
                                    AssignedTo = assignedTo.Id,
                                    IsSystemGenerated = true,
                                    TaskStatusId = (int)TaskStatusEnum.Pending,
                                    ModuleId = (int)WorkflowModuleEnum.Meeting,
                                    SectorId = (int)meetingVM.Meeting.SectorTypeId,
                                    DepartmentId = (int)DepartmentEnum.Operational,
                                    TypeId = (int)TaskTypeEnum.Task,
                                    RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                                    CreatedBy = meetingVM.Meeting.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    IsDeleted = false,
                                    ReferenceId = meetingVM.Meeting.MeetingId,
                                    SubModuleId = submodule,
                                    SystemGenTypeId = (int)TaskSystemGenTypeEnum.Meeting,
                                },
                                TaskActions = new List<TaskAction>()
                    {
                        new TaskAction()
                        {
                            ActionName = "Meeting Action",
                            TaskId = taskId,
                        }
                    }
                            },
                            "decision",
                            "meeting",
                             meetingVM.Meeting.MeetingId.ToString());

                            var notificationResult = await _INotification.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = meetingVM.Meeting.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = assignedTo.Id,
                                ModuleId = (int)WorkflowModuleEnum.Meeting,
                            },
          (int)NotificationEventEnum.MeeetingDecisionOfHOSForApproval,
        "decision",
                          "meeting",
                            meetingVM.Meeting.MeetingId.ToString() + "/" + taskId,
                           meetingVM.NotificationParameter);

                        }
                        _AuditLog.CreateProcessLog(new ProcessLog
                        {
                            Process = "Send To HOS",
                            Task = "Send To HOS",
                            Description = "User able to Send Meeting To HOS successfully.",
                            ProcessLogEventId = (int)ProcessLogEnum.Processed,
                            Message = "Edit Meeting executed Successfully",
                            IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                            ApplicationID = (int)PortalEnum.FatwaPortal,
                            ModuleID = (int)WorkflowModuleEnum.Meeting,
                            Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                        });
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _AuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Edit Meeting Failed",
                    Body = ex.Message,
                    Category = "User unable to Edit Meeting",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Edit Meeting Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateMeetingDecision")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateMeetingDecision(MeetingDecisionVM meetingVM)
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
                var result = await _IMeeting.UpdateMeetingDecision(meetingVM);
                if (result.Meeting != null)
                {
                    if (result.Meeting.MeetingTypeId == (int)MeetingTypeEnum.External && result.Meeting.IsApproved == true)
                    {
#if !DEBUG
                    await SendCommunicationToTarassol(result.Communication.CommunicationId, meetingVM.MeetingId, (Guid)meetingVM.ReferenceGuid,meetingVM.SubModulId, meetingVM.Subject);
#endif

                        //Rabbit MQ send Messages
                        _client.SendMessage(result, RabbitMQKeys.MeetingScheduleKey);
                        _AuditLog.CreateProcessLog(new ProcessLog
                        {
                            Process = "Update Meeting Status",
                            Task = "Update Meeting Status",
                            Description = "User able to Update Meeting Status successfully.",
                            ProcessLogEventId = (int)ProcessLogEnum.Processed,
                            Message = "Update Meeting Status executed Successfully",
                            IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                            ApplicationID = (int)PortalEnum.FatwaPortal,
                            ModuleID = (int)WorkflowModuleEnum.Meeting,
                            Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                        });
                        return Ok();
                    }
                    else

                    {
                        var assignedTo = await _IRole.GetHOSBySectorId((int)meetingVM.SectorTypeId);
                        //meetingVM.Meeting.ApprovalReqBy = Guid.Parse(assignedTo.Id);
                        var taskId = Guid.NewGuid();
                        string AssignedBy = await _IAccount.UserIdByUserEmail(meetingVM.createdBy);
                        int submodule = 0;
                        if (meetingVM.SubModulId == (int)SubModuleEnum.CaseFile || meetingVM.SubModulId == (int)SubModuleEnum.RegisteredCase)
                        {
                            if (meetingVM.SubModulId == (int)SubModuleEnum.CaseFile)
                            {
                                submodule = (int)SubModuleEnum.CaseFile;
                            }
                            else
                            {
                                submodule = (int)SubModuleEnum.RegisteredCase;
                            }
                        }
                        else if (meetingVM.SubModulId == (int)SubModuleEnum.ConsultationFile || meetingVM.SubModulId == (int)SubModuleEnum.ConsultationRequest)
                        {
                            if (meetingVM.SubModulId == (int)SubModuleEnum.ConsultationFile)
                            {
                                submodule = (int)SubModuleEnum.ConsultationFile;
                            }
                            else
                            {
                                submodule = (int)SubModuleEnum.ConsultationRequest;
                            }
                        }
                        else if (meetingVM.SubModulId == (int)SubModuleEnum.OrganizingCommittee)
                        {
                            submodule = (int)SubModuleEnum.OrganizingCommittee;
                        }
                        var taskResult = await _ITask.AddTask(new SaveTaskVM
                        {
                            Task = new UserTask
                            {
                                TaskId = taskId,
                                Name = "Save_Meeting_Task",
                                Date = DateTime.Now.Date,
                                AssignedBy = AssignedBy,
                                AssignedTo = assignedTo.Id,
                                IsSystemGenerated = true,
                                TaskStatusId = (int)TaskStatusEnum.Pending,
                                ModuleId = (int)WorkflowModuleEnum.Meeting,
                                SectorId = (int)meetingVM.SectorTypeId,
                                DepartmentId = (int)DepartmentEnum.Operational,
                                TypeId = (int)TaskTypeEnum.Task,
                                RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                                CreatedBy = meetingVM.createdBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReferenceId = meetingVM.MeetingId,
                                SubModuleId = submodule,
                                SystemGenTypeId = (int)TaskSystemGenTypeEnum.MeetingSendToHos,
                            },
                            TaskActions = new List<TaskAction>()
                    {
                        new TaskAction()
                        {
                            ActionName = "Meeting Action",
                            TaskId = taskId,
                        }
                    }
                        },
                             "decision",
                             "meeting",
                              meetingVM.MeetingId.ToString());
                        _AuditLog.CreateProcessLog(new ProcessLog
                        {
                            Process = "Send To HOS",
                            Task = "Send To HOS",
                            Description = "User able to Send Meeting To HOS successfully.",
                            ProcessLogEventId = (int)ProcessLogEnum.Processed,
                            Message = "Edit Meeting executed Successfully",
                            IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                            ApplicationID = (int)PortalEnum.FatwaPortal,
                            ModuleID = (int)WorkflowModuleEnum.Meeting,
                            Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                        });
                        var notificationResult = await _INotification.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = meetingVM.createdBy,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReceiverId = assignedTo.Id,
                            ModuleId = (int)WorkflowModuleEnum.Meeting,
                        },
      (int)NotificationEventEnum.MeeetingDecisionOfHOSForApproval,
    "decision",
                      "meeting",
                        meetingVM.MeetingId.ToString() + "/" + taskId,
                       meetingVM.NotificationParameter);


                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _AuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Update Meeting Status Failed",
                    Body = ex.Message,
                    Category = "User unable to Update Meeting Status",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Update Meeting Status Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SaveMom")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveMom(SaveMomVM meetingMom)
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
                if (meetingMom.MeetingMom.isSaveAsDraft)
                {
                    meetingMom.MeetingMom.MOMStatusId = (int)MeetingStatusEnum.SaveAsDraft;
                    var response = await _IMeeting.SaveMom(meetingMom);

                    return Ok();
                }
                meetingMom.MeetingMom.MOMStatusId = (int)MeetingStatusEnum.OnHold;
                var result = await _IMeeting.SaveMom(meetingMom);

                //meetingMom.MeetingMom.MeetingAttendees = result.item2;

                //TODO Send Task to only Fatwa Attendees
                if (result)
                {
                    //Rabbit MQ send Messages
                    if (meetingMom.Meeting.MeetingTypeId == (int)MeetingTypeEnum.External)
                    {
                        _client.SendMessage(meetingMom, RabbitMQKeys.SubmitMOMMeeting);
                    }

                    if (meetingMom.MeetingMom.MeetingAttendees.Any())
                    {
                        foreach (var attendee in meetingMom.MeetingMom.MeetingAttendees)
                        {
                            var taskId = Guid.NewGuid();
                            string AssignedBy = await _IAccount.UserIdByUserEmail(meetingMom.MeetingMom.CreatedBy);
                            int submodule = 0;
                            if (attendee.SubModulId == (int)SubModuleEnum.CaseFile || attendee.SubModulId == (int)SubModuleEnum.RegisteredCase)
                            {
                                if (attendee.SubModulId == (int)SubModuleEnum.CaseFile)
                                {
                                    submodule = (int)SubModuleEnum.CaseFile;
                                }
                                else
                                {
                                    submodule = (int)SubModuleEnum.RegisteredCase;
                                }
                            }
                            else if (attendee.SubModulId == (int)SubModuleEnum.ConsultationFile || attendee.SubModulId == (int)SubModuleEnum.ConsultationRequest)
                            {
                                if (attendee.SubModulId == (int)SubModuleEnum.ConsultationFile)
                                {
                                    submodule = (int)SubModuleEnum.ConsultationFile;
                                }
                                else
                                {
                                    submodule = (int)SubModuleEnum.ConsultationRequest;
                                }
                            }
                            else if (attendee.SubModulId == (int)SubModuleEnum.OrganizingCommittee)
                            {
                                submodule = (int)SubModuleEnum.OrganizingCommittee;
                            }
                            var taskResult = await _ITask.AddTask(new SaveTaskVM
                            {
                                Task = new UserTask
                                {
                                    TaskId = taskId,
                                    Name = "Review_Minutes_of_Meeting_Task",
                                    Date = DateTime.Now.Date,
                                    AssignedBy = AssignedBy,//Meeting Created by
                                    AssignedTo = attendee.AttendeeUserId,//Attendee Id
                                    IsSystemGenerated = true,
                                    TaskStatusId = (int)TaskStatusEnum.Pending,
                                    ModuleId = (int)WorkflowModuleEnum.Meeting,
                                    SectorId = (int)attendee.SectorTypeId,
                                    DepartmentId = attendee.DepartmentId,
                                    TypeId = (int)TaskTypeEnum.Task,
                                    RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                                    CreatedBy = meetingMom.MeetingMom.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    IsDeleted = false,
                                    ReferenceId = attendee.MeetingId,
                                    SubModuleId = submodule,
                                    SystemGenTypeId = (int)TaskSystemGenTypeEnum.Meeting,
                                },
                                TaskActions = new List<TaskAction>()
                            {
                                new TaskAction()
                                {
                                    ActionName = "Meeting Action",
                                    TaskId = taskId,
                                }
                            }
                            },
                            "attendeedecision",
                            "mom",
                            meetingMom.MeetingMom.MeetingMomId.ToString());


                            var notificationResult = await _INotification.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = meetingMom.MeetingMom.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = attendee.AttendeeUserId,
                                ModuleId = (int)WorkflowModuleEnum.Meeting,
                            },
(int)NotificationEventEnum.AddMOMOfMeeting,
 "attendeedecision",
                            "mom",
                 meetingMom.MeetingMom.MeetingMomId.ToString() + "/" + taskId,
               meetingMom.NotificationParameter);


                        }
                    }

                    //-------------------------//
                    _AuditLog.CreateProcessLog(new ProcessLog
                    {
                        Process = "Save MOM",
                        Task = "Save MOM",
                        Description = "User able to Save MOM successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Save MOM executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });

                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _AuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Save MOM Failed",
                    Body = ex.Message,
                    Category = "User unable to Save MOM",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Save MOM Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("EditMom")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> EditMom(SaveMomVM meetingMom)
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
                if (meetingMom.MeetingMom.MOMStatusId == (int)MeetingStatusEnum.SaveAsDraft)
                {
                    await _IMeeting.EditMom(meetingMom);

                    return Ok();
                }
                var result = await _IMeeting.EditMom(meetingMom);
                //TODO Send Task to Attendees
                if (result)
                {

                    //Rabbit MQ send Messages
                    if (meetingMom.Meeting.MeetingTypeId == (int)MeetingTypeEnum.External)
                    {
                        _client.SendMessage(meetingMom, RabbitMQKeys.SubmitMOMMeeting);
                    }

                    if (meetingMom.MeetingMom.MeetingAttendees.Any())
                    {
                        foreach (var attendee in meetingMom.MeetingMom.MeetingAttendees)
                        {
                            var taskId = Guid.NewGuid();
                            string AssignedBy = await _IAccount.UserIdByUserEmail(meetingMom.MeetingMom.CreatedBy);
                            int submodule = 0;
                            if (attendee.SubModulId == (int)SubModuleEnum.CaseFile || attendee.SubModulId == (int)SubModuleEnum.RegisteredCase)
                            {
                                if (attendee.SubModulId == (int)SubModuleEnum.CaseFile)
                                {
                                    submodule = (int)SubModuleEnum.CaseFile;
                                }
                                else
                                {
                                    submodule = (int)SubModuleEnum.RegisteredCase;
                                }
                            }
                            else if (attendee.SubModulId == (int)SubModuleEnum.ConsultationFile || attendee.SubModulId == (int)SubModuleEnum.ConsultationRequest)
                            {
                                if (attendee.SubModulId == (int)SubModuleEnum.ConsultationFile)
                                {
                                    submodule = (int)SubModuleEnum.ConsultationFile;
                                }
                                else
                                {
                                    submodule = (int)SubModuleEnum.ConsultationRequest;
                                }
                            }
                            else if (attendee.SubModulId == (int)SubModuleEnum.OrganizingCommittee)
                            {
                                submodule = (int)SubModuleEnum.OrganizingCommittee;
                            }
                            var taskResult = await _ITask.AddTask(new SaveTaskVM
                            {
                                Task = new UserTask
                                {
                                    TaskId = taskId,
                                    Name = "Save_MOM_Meeting_Task",
                                    Date = DateTime.Now.Date,
                                    AssignedBy = AssignedBy,//Meeting Created by
                                    AssignedTo = attendee.AttendeeUserId,//Attendee Id
                                    IsSystemGenerated = true,
                                    TaskStatusId = (int)TaskStatusEnum.Pending,
                                    ModuleId = (int)WorkflowModuleEnum.Meeting,
                                    SectorId = (int)attendee.SectorTypeId,
                                    DepartmentId = attendee.DepartmentId,
                                    TypeId = (int)TaskTypeEnum.Task,
                                    RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                                    CreatedBy = meetingMom.MeetingMom.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    IsDeleted = false,
                                    ReferenceId = attendee.MeetingId,
                                    SubModuleId = submodule,
                                    SystemGenTypeId = (int)TaskSystemGenTypeEnum.Meeting,
                                },
                                TaskActions = new List<TaskAction>()
                            {
                                new TaskAction()
                                {
                                    ActionName = "Meeting Action",
                                    TaskId = taskId,
                                }
                            }
                            },
                            "attendeedecision",
                            "mom",
                            meetingMom.MeetingMom.MeetingMomId.ToString());

                            //var notificationResult = await _INotification.SendNotification(new Notification
                            //{
                            //    NotificationId = Guid.NewGuid(),
                            //    DueDate = DateTime.Now.AddDays(5),
                            //    CreatedBy = meetingMom.MeetingMom.CreatedBy,
                            //    CreatedDate = DateTime.Now,
                            //    IsDeleted = false,
                            //    ReceiverId = attendee.AttendeeUserId,//FATWA ADMIN
                            //    ReceiverTypeId = (int)NotificationReceiverTypeEnum.User,
                            //    ModuleId = (int)WorkflowModuleEnum.Meeting,
                            //    NotificationTypeId = (int)NotificationTypeEnum.Asynchronous,
                            //    NotificationCommunicationMethodId = (int)NotificationChannelEnum.Browser,
                            //    NotificationLinkId = Guid.NewGuid(),
                            //    NotificationEventId = (int)NotificationEventEnum.NewRequest,
                            //    NotificationCategoryId = (int)NotificationCategoryEnum.Important
                            //},
                            //"Add_Meeting_Success",
                            //"list",
                            //new Meeting().GetType().Name,
                            //attendee.MeetingId.ToString());

                            var notificationResult = await _INotification.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = meetingMom.MeetingMom.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = attendee.AttendeeUserId,
                                ModuleId = (int)WorkflowModuleEnum.Meeting,
                            },
(int)NotificationEventEnum.EditMOMOfMeeting,
"attendeedecision",
                "mom",
     meetingMom.MeetingMom.MeetingMomId.ToString() + "/" + taskId,
   meetingMom.NotificationParameter);



                        }
                    }
                    //-------------------------//

                    _AuditLog.CreateProcessLog(new ProcessLog
                    {
                        Process = "Edit MOM",
                        Task = "Edit MOM",
                        Description = "User able to Edit MOM successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Edit MOM executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _AuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Edit MOM Failed",
                    Body = ex.Message,
                    Category = "Edit unable to Save MOM",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Edit MOM Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SaveLegislationAttandee")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveLegislationAttandee(SaveMeetingVM meetingVM)
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
                result = await _IMeeting.SaveLegislationAttandee(meetingVM);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Update Meeting Status
        [HttpPost("UpdateMeetingStatus")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nadia Gull' Date='2023-03-3' Version="1.0" Branch="master">  Update MeetingStatus </History>
        public async Task<IActionResult> UpdateMeetingStatus(MeetingDecisionVM updateEntity)
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
                var result = await _IMeeting.UpdateMeetingStatus(updateEntity);
                if (result)
                {
                    var meetingDetail = await _IMeeting.GetMeetingDetailsById(updateEntity.MeetingId);

                    if ((updateEntity.MeetingStatusId == (int)MeetingStatusEnum.RejectedByGE) || (updateEntity.MeetingStatusId == (int)MeetingStatusEnum.OnHold))
                    {


                        var notificationResult = await _INotification.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = "System Generated",
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReceiverId = meetingDetail.CreatorId.ToString(),
                            ModuleId = (int)WorkflowModuleEnum.Meeting,
                        },
                        (int)NotificationEventEnum.GERejectMeeetingInvite,
                        "view",
                        "meeting",
                        updateEntity.MeetingId.ToString(),
                        meetingDetail.NotificationParameter);

                        if (meetingDetail.CreatorId != meetingDetail.ApprovalReqBy.ToString())
                        {
                            var notification = await _INotification.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = "System Generated",
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = meetingDetail.ApprovalReqBy.ToString(),
                                ModuleId = (int)WorkflowModuleEnum.Meeting,
                            },
                            (int)NotificationEventEnum.GERejectMeeetingInvite,
                            "view",
                            "meeting",
                            updateEntity.MeetingId.ToString(),
                            meetingDetail.NotificationParameter);
                        }
                    }
                    if ((updateEntity.MeetingStatusId == (int)MeetingStatusEnum.ApprovedByGE) || (updateEntity.MeetingStatusId == (int)MeetingStatusEnum.Scheduled))
                    {


                        var notificationResult = await _INotification.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = "System Generated",
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReceiverId = meetingDetail.CreatorId.ToString(),
                            ModuleId = (int)WorkflowModuleEnum.Meeting,
                        },
(int)NotificationEventEnum.GEAcceptMeeetingInvite,
"view",
"meeting",
updateEntity.MeetingId.ToString(),
meetingDetail.NotificationParameter);
                        if (meetingDetail.CreatorId != meetingDetail.ApprovalReqBy.ToString())
                        {


                            var notification = await _INotification.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = "System Generated",
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = meetingDetail.ApprovalReqBy.ToString(),
                                ModuleId = (int)WorkflowModuleEnum.Meeting,
                            },
(int)NotificationEventEnum.GEAcceptMeeetingInvite,
"view",
"meeting",
updateEntity.MeetingId.ToString(),
meetingDetail.NotificationParameter);

                        }
                    }

                    _AuditLog.CreateProcessLog(new ProcessLog
                    {
                        Process = "Update Meeting Status",
                        Task = "To Update Meeting Status",
                        Description = "Meeting Status has been updated.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Meeting Status been updated",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _AuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Update Meeting Status Failed",
                    Body = ex.Message,
                    Category = "User unable to Update Meeting Status",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "The Meeting Status not be updated",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }
        #endregion


        [HttpGet("GetMeetingAttendeeDecisionById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Hassan' Date='2022-10-25' Version="1.0" Branch="master"> </History>
        public async Task<IActionResult> GetMeetingAttendeeDecisionById(string meetingId, string userId, bool isMomAttendeeDecision)
        {
            try
            {
                var result = await _IMeeting.GetMeetingAttendeeDecisionById(Guid.Parse(meetingId), userId, isMomAttendeeDecision);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }
        }
        [HttpPost("UpdateMeetingAttendeeDecision")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Hassan' Date='2022-10-25' Version="1.0" Branch="master"> </History>
        public async Task<IActionResult> UpdateMeetingAttendeeDecision(string userId, AttendeeDecisionVM decision, bool isMomAttendeeDecision)
        {
            try
            {
                var result = await _IMeeting.UpdateMeetingAttendeeDecision(decision, userId, isMomAttendeeDecision);
                if (result != null)
                {
                    if (decision.AttendeeStatusId == (int)MeetingAttendeeStatusEnum.Accept)
                    {
                        if (decision.InitiatorId == null)
                        {
                            var assignedTo = await _IRole.GetHOSBySectorId((int)decision.SectorTypeId);
                            decision.InitiatorId = assignedTo.Id;
                        }
                        decision = await _IMeeting.GetNotificationParameters(decision);


                        var notificationResult = await _INotification.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = decision.LoggedInUser,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReceiverId = decision.InitiatorId,
                            ModuleId = (int)WorkflowModuleEnum.Meeting,
                        },
       (int)NotificationEventEnum.AttendeeAcceptMeetingInvite,
     "view",
                        "meeting",
                         decision.MeetingId.ToString(),
                        decision.NotificationParameter);





                    }
                    if (decision.AttendeeStatusId == (int)MeetingAttendeeStatusEnum.Reject)
                    {


                        decision.NotificationParameter.Entity = new Meeting().GetType().Name;
                        var notificationResult = await _INotification.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = decision.LoggedInUser,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReceiverId = decision.InitiatorId,
                            ModuleId = (int)WorkflowModuleEnum.Meeting,
                        },
       (int)NotificationEventEnum.AttendeeRejectMeetingInvite,
     "view",
                        "meeting",
                         decision.MeetingId.ToString(),
                        decision.NotificationParameter);




                    }
                    _AuditLog.CreateProcessLog(new ProcessLog
                    {
                        Process = "User Took the decision Against Meeting",
                        Task = "To Attend The Meeting Or Not",
                        Description = "User has taken the decision Against Meeting.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "User has taken the decision Against Meeting Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _AuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "User  decision Against Meeting Failed",
                    Body = ex.Message,
                    Category = "User unable to takethe decision Against Meeting",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "The Meeting Decision could not be updated",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }
        }

        #region Get MOM attendees decision details
        [HttpGet("PopulateMOMAttendeesDecisionDetails")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Umer Zaman' Date='2022-10-25' Version="1.0" Branch="master">Get MOM attendees decision details by using mom and meeting id </History>
        public async Task<IActionResult> PopulateMOMAttendeesDecisionDetails(Guid meetingMomId, Guid meetingId)
        {
            try
            {
                var result = await _IMeeting.PopulateMOMAttendeesDecisionDetails(meetingMomId, meetingId);
                if (result.Count() != 0)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }
        }
        #endregion

        #region Meeting detail By Meeting Id
        [HttpGet("GetMeetingDetailById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetMeetingDetailById(Guid MeetingId)
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

                var result = await _IMeeting.GetMeetingDetailById(MeetingId);
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
        #endregion

        #region Edit Meeting Status 
        [HttpPost("EditMeetingStatus")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> EditMeetingStatus(MeetingStatusVM meeting)
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

                var result = await _IMeeting.EditMeetingStatus(meeting);
                if (result != null && result.MeetingId != Guid.Empty)
                {
                    //Rabbit MQ send Messages
                    _client.SendMessage(meeting, RabbitMQKeys.MeetingStatusUpdateKey);
                    _AuditLog.CreateProcessLog(new ProcessLog
                    {
                        Process = "Update Meeting Status",
                        Task = "Update Meeting Status",
                        Description = "User able to Update Meeting Status successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Update Meeting Status executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.Meeting,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Meeting Attendee Detail For MOM
        [HttpGet("GetAttendeeDetailsById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAttendeeDetails(Guid MeetingId)
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

                var result = await _IMeeting.GetAttendeeDetails(MeetingId);
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
        #endregion

        #region Submit MOM
        [HttpPost("SubmitMom")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SubmitMom(SaveMomVM meetingMom)
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

                var result = await _IMeeting.SubmitMom(meetingMom);

                if (result)
                {
                    //Rabbit MQ send Messages
                    if (meetingMom.Meeting.MeetingTypeId == (int)MeetingTypeEnum.External)
                    {
                        _client.SendMessage(meetingMom, RabbitMQKeys.SubmitMOMMeeting);
                    }

                    if (meetingMom.LegislationAttendee.Any())
                    {
                        foreach (var attendee in meetingMom.LegislationAttendee)
                        {
                            var taskId = Guid.NewGuid();
                            string AssignedBy = await _IAccount.UserIdByUserEmail(meetingMom.MeetingMom.CreatedBy);
                            int submodule = 0;
                            if (meetingMom.Meeting.SubModulId == (int)SubModuleEnum.CaseFile || meetingMom.Meeting.SubModulId == (int)SubModuleEnum.CaseRequest)
                            {
                                if (meetingMom.Meeting.SubModulId == (int)SubModuleEnum.RegisteredCase)
                                {
                                    submodule = (int)SubModuleEnum.CaseFile;
                                }
                                else
                                {
                                    submodule = (int)SubModuleEnum.RegisteredCase;
                                }
                            }
                            else
                            {
                                if (meetingMom.Meeting.SubModulId == (int)SubModuleEnum.ConsultationFile)
                                {
                                    submodule = (int)SubModuleEnum.ConsultationFile;
                                }
                                else
                                {
                                    submodule = (int)SubModuleEnum.ConsultationRequest;
                                }
                            }
                            var notificationResult = await _INotification.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = meetingMom.MeetingMom.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = attendee.Id,
                                ModuleId = (int)WorkflowModuleEnum.Meeting,
                            },
(int)NotificationEventEnum.MOMCreatedSuccessfully,
"view",
"meeting",
meetingMom.Meeting.MeetingId.ToString(),
meetingMom.NotificationParameter);

                        }
                    }

                    _AuditLog.CreateProcessLog(new ProcessLog
                    {
                        Process = "Submit MOM",
                        Task = "Submit MOM",
                        Description = "User able to Submit MOM successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Submit MOM executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });

                    return Ok(result);
                }
                return NotFound();

            }
            catch (Exception ex)
            {
                _AuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Save MOM Failed",
                    Body = ex.Message,
                    Category = "User unable to Save MOM",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Save MOM Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get Department By GE Id

        [HttpGet("GetDepartmentsByGeId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetDepartmentsByGeId(int GeId)
        {
            try
            {
                var result = await _IMeeting.GetDepartmentsByGeId(GeId);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }
        }
        #endregion

        #region Take Request For Meeting Decision From Fatwa

        [HttpPost("TakeRequestForMeetingDecisionFromFatwa")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> TakeRequestForMeetingDecisionFromFatwa(SaveMeetingVM meetingVM)
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
                var assignedTo = await _IRole.GetHOSBySectorId((int)meetingVM.Meeting.SectorTypeId);
                meetingVM.Meeting.ApprovalReqBy = Guid.Parse(assignedTo.Id);
                var result = await _IMeeting.TakeRequestForMeetingDecisionFromFatwa(meetingVM);
                if (result != null)
                {
                    if (result.Meeting.MeetingTypeId == (int)MeetingTypeEnum.External && result.Meeting.IsApproved == true)
                    {
                        //Rabbit MQ send Messages
                        _client.SendMessage(result, RabbitMQKeys.RequestForMeetingFromG2GKey);
                        _AuditLog.CreateProcessLog(new ProcessLog
                        {
                            Process = "Fatwa HOS completed task against meeting request",
                            Task = "Fatwa HOS completed task against meeting request",
                            Description = "Fatwa HOS completed task against meeting request successfully.",
                            ProcessLogEventId = (int)ProcessLogEnum.Processed,
                            Message = "Fatwa HOS completed task against meeting request Successfully",
                            IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                            ApplicationID = (int)PortalEnum.FatwaPortal,
                            ModuleID = (int)WorkflowModuleEnum.Meeting,
                            Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                        });
                        return Ok();
                    }
                }

                var Attendees = meetingVM.LegislationAttendee;
                if (Attendees.Count() != 0)
                {
                    foreach (var file in Attendees)
                    {
                        if (file.AttendeeStatusId == (int)MeetingAttendeeStatusEnum.New)
                        {
                            var taskId = Guid.NewGuid();
                            string AssignedBy = await _IAccount.UserIdByUserEmail(meetingVM.Meeting.ModifiedBy);
                            int submodule = 0;
                            if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.CaseFile || meetingVM.Meeting.SubModulId == (int)SubModuleEnum.RegisteredCase)
                            {
                                if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.CaseFile)
                                {
                                    submodule = (int)SubModuleEnum.CaseFile;
                                }
                                else
                                {
                                    submodule = (int)SubModuleEnum.RegisteredCase;
                                }
                            }
                            else 
                            {
                                if (meetingVM.Meeting.SubModulId == (int)SubModuleEnum.ConsultationFile)
                                {
                                    submodule = (int)SubModuleEnum.ConsultationFile;
                                }
                                else
                                {
                                    submodule = (int)SubModuleEnum.ConsultationRequest;
                                }
                            }
                            
                            var taskResult = await _ITask.AddTask(new SaveTaskVM
                            {
                                Task = new UserTask
                                {
                                    TaskId = taskId,
                                    Name = "Review Meeting Attendee Task",
                                    Date = DateTime.Now.Date,
                                    AssignedBy = AssignedBy,
                                    AssignedTo = file.Id,
                                    IsSystemGenerated = true,
                                    TaskStatusId = (int)TaskStatusEnum.Pending,
                                    ModuleId = (int)WorkflowModuleEnum.Meeting,
                                    SectorId = (int)meetingVM.Meeting.SectorTypeId,
                                    DepartmentId = (int)file.DepartmentId,
                                    TypeId = (int)TaskTypeEnum.Task,
                                    RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                                    CreatedBy = meetingVM.Meeting.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    IsDeleted = false,
                                    ReferenceId = meetingVM.Meeting.MeetingId,
                                    SubModuleId = submodule,
                                    SystemGenTypeId = (int)TaskSystemGenTypeEnum.Meeting,
                                },
                                TaskActions = new List<TaskAction>()
                                {
                                    new TaskAction()
                                    {
                                        ActionName = "Meeting Action",
                                        TaskId = taskId,
                                    }
                                }
                            },
                            "attendeedecision",
                            "requestformeeting",
                                meetingVM.Meeting.MeetingId.ToString());


                            var notificationResult = await _INotification.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = meetingVM.Meeting.ModifiedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = file.Id,
                                ModuleId = (int)WorkflowModuleEnum.Meeting,
                            },
         (int)NotificationEventEnum.AttendeeDecisionForMeeting,
        "attendeedecision",
                            "requestformeeting",
                           meetingVM.Meeting.MeetingId.ToString() + "/" + taskId,
                          meetingVM.NotificationParameter);




                        }
                    }
                }

                return Ok();

            }
            catch (Exception ex)
            {
                _AuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Create Meeting Failed",
                    Body = ex.Message,
                    Category = "User unable to Create Meeting",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Create Meeting Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }
        #endregion

        [HttpGet("GetGeAttendeeByReferenceId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGeAttendeeByReferenceId(Guid ReferenceId)
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
                _client.SendMessage(ReferenceId.ToString(), RabbitMQKeys.GetGEAttendee);
                return Ok(ReferenceId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("GetGEAttendeeDetails")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGEAttendeeDetails(MeetingAttendeeVM args)
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
                if (args != null)
                {
                    return Ok(args);

                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #region Update Meeting MOM attendee decision
        [HttpPost("UpdateMeetingMOMAttendeeDecision")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Umer Zaman' Date='03-01-2024' Version="1.0" Branch="master"> Update Meeting MOM attendee decision</History>
        public async Task<IActionResult> UpdateMeetingMOMAttendeeDecision(MomAttendeeDecision item)
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
                var result = await _IMeeting.UpdateMeetingMOMAttendeeDecision(item);
                if (result)
                {
                    var assignedTo = await _IMeeting.GetMOMCreatedByIdByUsingMOMId(item.MeetingMomId);
                    if (assignedTo != null)
                    {
                        //var notificationResponse = await _INotification.SendNotification(new Notification
                        //{
                        //    NotificationId = Guid.NewGuid(),
                        //    DueDate = DateTime.Now.AddDays(5),
                        //    CreatedBy = assignedTo.CreatedBy,
                        //    CreatedDate = DateTime.Now,
                        //    IsDeleted = false,
                        //    ReceiverId = assignedTo.Id,
                        //    ReceiverTypeId = (int)NotificationReceiverTypeEnum.User,
                        //    ModuleId = (int)WorkflowModuleEnum.Meeting,
                        //    NotificationTypeId = (int)NotificationTypeEnum.Asynchronous,
                        //    NotificationCommunicationMethodId = (int)NotificationChannelEnum.Browser,
                        //    NotificationLinkId = Guid.NewGuid(),
                        //    NotificationEventId = (int)NotificationEventEnum.SubmitRequest,
                        //    NotificationCategoryId = (int)NotificationCategoryEnum.Important
                        //},
                        // "G2G_MOM_Decision",
                        // "list",
                        // "meeting",
                        // item.MeetingId.ToString());
                    }

                    _AuditLog.CreateProcessLog(new ProcessLog
                    {
                        Process = "GE User Took the decision Against Minutes of Meeting",
                        Task = "To Check The Minutes of Meeting Or Not",
                        Description = "User has taken the decision Against Minutes of Meeting.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "User has taken the decision Against Minutes of Meeting Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _AuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "User decision Against Minutes of Meeting Failed",
                    Body = ex.Message,
                    Category = "User unable to take the decision Against Minutes of Meeting",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "The Minutes of Meeting Decision could not be updated",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        [HttpGet("GetAttendeeStatusesByMeetingId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAttendeeStatusesByMeetingId(Guid meetingId)
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
                var result = await _IMeeting.GetAttendeeStatusesByMeetingId(meetingId);
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

        #region Meeting detail By Meeting Id
        [HttpGet("CheckViceHosApproval")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CheckViceHosApproval(int sectorTypeId)
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

                var result = await _IMeeting.CheckViceHosApproval(sectorTypeId);
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
        #endregion
        #region Mobile App End Points
        [HttpGet("GetMeetingsForMobileApp")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetMeetingsForMobileApp(string userName, int channelId)
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
                var result = await _IMeeting.GetMeetingsForMobileApp(userName, channelId);
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
        #endregion
    }
}
