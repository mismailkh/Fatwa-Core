using AutoMapper;
using FATWA_API.RabbitMQ;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.CaseManagement;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Interfaces.Tasks;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.UserEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V1
{
    //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Cms Case File Controller</History>

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CmsCaseFileController : ControllerBase
    {
        private readonly ICmsCaseFile _ICmsCaseFile;
        private readonly IAuditLog _auditLogs;
        private readonly INotification _iNotifications;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly RabbitMQClient _client;
        private readonly ITask _ITask;

        public CmsCaseFileController(ICmsCaseFile iCmsCaseFile, IAuditLog audit, INotification iNotifications, IConfiguration configuration, 
            IMapper mapper, RabbitMQClient client, ITask ITask)
        {
            _ICmsCaseFile = iCmsCaseFile;
            _auditLogs = audit;
            _iNotifications = iNotifications;
            _configuration = configuration;
            _mapper = mapper;
            _client = client;
            _ITask = ITask;
        }

        #region Get All Registered Case file
        //<History Author = 'Danish' Date='2022-11-29' Version="1.0" Branch="master"> Get All Registered Case file</History>
        [HttpPost("GetRegisteredCaseFile")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetRegisteredCaseFile(AdvanceSearchCmsCaseFileVM advanceSearchVM)
        {
            try
            {
                return Ok(await _ICmsCaseFile.GetRegisteredCaseFile(advanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Registered Cases by File Id

        [HttpGet("GetAllRegisteredCasesByFileId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Danish' Date='2022-11-29' Version="1.0" Branch="master"> Get All Registered Cases</History>
        public async Task<IActionResult> GetAllRegisteredCasesByFileId(Guid fileId, bool? isFinal)
        {
            try
            {
                return Ok(await _ICmsCaseFile.GetAllRegisteredCasesByFileId(fileId, isFinal));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Execution Cases

        [HttpGet("GetExecutionCasesByFileId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2023-03-30' Version="1.0" Branch="master"> Get All Execution Cases</History>
        public async Task<IActionResult> GetExecutionCasesByFileId(Guid fileId)
        {
            try
            {
                return Ok(await _ICmsCaseFile.GetExecutionCasesByFileId(fileId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get All Case file

        [HttpPost("GetAllCaseFile")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllCmsCaseFile(AdvanceSearchCmsCaseFileVM advanceSearchVM)
        {
            try
            {
                return Ok(await _ICmsCaseFile.GetAllCmsCaseFile(advanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get All Case file

        [HttpGet("GetAllCaseFilesBySector")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllCaseFilesBySector(int sectorTypeId, string userId)
        {
            try
            {
                return Ok(await _ICmsCaseFile.GetAllCaseFilesBySector(sectorTypeId, userId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Case File Detail by Id

        [HttpGet("GetCaseFileDetailByIdVM")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Get Case File detail by Id</History>
        public async Task<IActionResult> GetCaseFileDetailByIdVM(Guid fileId, string userName)
        {
            try
            {
                return Ok(await _ICmsCaseFile.GetCaseFileDetailByIdVM(fileId, userName));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Case File Detail by Id

        [HttpGet("GetCaseFileById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-12-24' Version="1.0" Branch="master"> Get Case File by Id</History>
        public async Task<IActionResult> GetCaseFileById(Guid fileId)
        {
            try
            {
                return Ok(await _ICmsCaseFile.GetCaseFileById(fileId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get CaseRequest By FileId

        [HttpGet("GetCaseRequestByFileId")]
        [MapToApiVersion("1.0")]
        
        public async Task<IActionResult> GetCaseRequestByFileId(Guid fileId)
        {
            try
            {
                return Ok(await _ICmsCaseFile.GetCaseRequestByFileId(fileId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Case Assignment 
        [HttpGet("GetCaseAssigment")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCaseAssigment(Guid referenceId)
        {
            try
            {
                return Ok(await _ICmsCaseFile.GetCaseAssigment(referenceId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Case Assignment History
        [HttpGet("GetCaseAssigmentHistory")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCaseAssigmentHistory(Guid referenceId)
        {
            try
            {
                return Ok(await _ICmsCaseFile.GetCaseAssigmentHistory(referenceId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Case Lawyer Request
        [HttpGet("GetCaseAssigeeList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCaseAssigeeList(Guid referenceId)
        {
            try
            {
                return Ok(await _ICmsCaseFile.GetCaseAssigeeList(referenceId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region get case File history status
        [HttpGet("GetCMSCaseFileStatusHistory")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2022-09-30' Version="1.0" Branch="master"> Get Case Request status history</History>
        public async Task<IActionResult> GetCMSCaseFileStatusHistory(Guid FileId)
        {
            try
            {
                return Ok(await _ICmsCaseFile.GetCMSCaseFileStatusHistory(FileId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Moj Registration Requests

        [HttpGet("GetMojRegistrationRequests")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Get Moj Registration Requests</History>
        public async Task<IActionResult> GetMojRegistrationRequests(int? sectorTypeId, bool? IsRegistered, int? pageNumber, int? pageSize)
        {
            try
            {
                return Ok(await _ICmsCaseFile.GetMojRegistrationRequests(sectorTypeId, IsRegistered, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Moj Registration Requests

        [HttpGet("GetMojDocumentPortfolioRequests")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2023-03-23' Version="1.0" Branch="master"> Get Moj Document Portfolio Requests</History>
        public async Task<IActionResult> GetMojDocumentPortfolioRequests(int? sectorTypeId, int pageNumber, int pageSize)
        {
            try
            {
                return Ok(await _ICmsCaseFile.GetMojDocumentPortfolioRequests(sectorTypeId, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Moj Registration Request By Id

        [HttpGet("GetMojRegistrationRequestById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Get Moj Registration Request By Id</History>
        public async Task<IActionResult> GetMojRegistrationRequestById(Guid id)
        {
            try
            {
                return Ok(await _ICmsCaseFile.GetMojRegistrationRequestById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Create Case Party

        [HttpPost("CreateCaseParty")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-10-02' Version="1.0" Branch="master"> Create Case Party</History>
        public async Task<IActionResult> CreateCaseParty(CasePartyLinkVM party)
        {
            //try
            //{
            //    await _ICmsCaseFile.CreateCaseParty(party);
            //    return Ok(party);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.Message);
            //}
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                var result = await _ICmsCaseFile.CreateCaseParty(party);
                if (result)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        #endregion

        #region Create Moj Execution Request

        [HttpPost("CreateMojExecutionRequest")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2023-03-30' Version="1.0" Branch="master"> Create MOJ Execution Request</History>
        public async Task<IActionResult> CreateMojExecutionRequest(MojExecutionRequest request)
        {
            try
            {
                await _ICmsCaseFile.CreateMojExecutionRequest(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        #endregion

        #region Delete Case Party

        [HttpPost("DeleteCaseParty")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-10-02' Version="1.0" Branch="master"> Delete Case Party</History>
        public async Task<IActionResult> DeleteCaseParty(CasePartyLinkVM party)
        {
            try
            {
                await _ICmsCaseFile.DeleteCaseParty(party);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Create MOJ Registration Request

        [HttpPost("CreateMojRegistrationRequest")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Create Moj Registration Request</History>
        public async Task<IActionResult> CreateMojRegistrationRequest(List<MojRegistrationRequest> registrationRequestList)
        {
            try
            {
                var historyobj = await _ICmsCaseFile.CreateMojRegistrationRequest(registrationRequestList);
                if (historyobj != null)
                {
                    //Rabbit MQ send Messages
                    var mapObj = _mapper.Map<UpdateEntityHistoryVM>(historyobj);
                    mapObj.ReferenceId = historyobj.FileId;
                    mapObj.SubModuleId = (int)SubModuleEnum.CaseFile;
                    _client.SendMessage(mapObj, RabbitMQKeys.HistoryKey);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Link Case Fies with Primary File

        //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master"> Link Selected Case Files with Primary File </History>
        [HttpPost("LinkCaseFiles")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> LinkCaseFiles(LinkCaseFilesVM linkCaseFile)
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
            var result = await _ICmsCaseFile.LinkCaseFiles(linkCaseFile);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Link Case Requests",
                    Task = "To Link Case Requests",
                    Description = "To Link Case Requests",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Case Requests Linked.",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "To Link Case Requests",
                    Body = ex.Message,
                    Category = "User unable To Link Case Requests",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable To Link Case Requests",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        #endregion

        #region Get Linked Files By Pimary File

        [HttpGet("GetLinkedFilesByPrimaryFileId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"> Get Linked Case Files by Primary File</History>
        public async Task<IActionResult> GetLinkedFilesByPrimaryFileId(Guid primaryFileId)
        {
            try
            {
                return Ok(await _ICmsCaseFile.GetLinkedFilesByPrimaryFileId(primaryFileId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        #endregion

        #region get case File history status

        [HttpGet("GetCaseFileHistoryDetailByHistoryId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Zain Ul Islam' Date='2023-01-19' Version="1.0" Branch="master"> Get Case File History Detail By HistoryId</History>
        public async Task<IActionResult> GetCaseFileHistoryDetailByHistoryId(Guid HistoryId)
        {
            try
            {
                return Ok(await _ICmsCaseFile.GetCaseFileHistoryDetailByHistoryId(HistoryId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Execution Requests

        [HttpGet("GetMojExecutionRequests")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2023-03-30' Version="1.0" Branch="master"> Get Moj Execution Requests</History>
        public async Task<IActionResult> GetMojExecutionRequests(string username, int? pageNumber, int? pageSize)
        {
            try
            {
                return Ok(await _ICmsCaseFile.GetMojExecutionRequests(username, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Update Case File IsAssigned Back Status
        [HttpPost("UpdateCaseFileIsAssignedBack")]
        [MapToApiVersion("1.0")]
        //<History Author = 'ijaz Ahmad' Date='2023-04-18' Version="1.0" Branch="master"> Update Case File IsAssigned Back Status</History>
        public async Task<IActionResult> UpdateCaseFileIsAssignedBack(Guid FileId)
        {
            try
            {
                return Ok(await _ICmsCaseFile.UpdateCaseFileIsAssigned(FileId, false));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Update CaseFile Status and Add History
        [HttpPost("UpdateCaseFileStatusandAddHistory")]
        [MapToApiVersion("1.0")]      
        public async Task<IActionResult> UpdateCaseFileStatusandAddHistory(Guid FileId,string UserName)
        {
            try
            {
                await _ICmsCaseFile.UpdateCaseFileStatusandAddHistory(FileId, UserName);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        [HttpPost("ProcessCaseFile")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Create Moj Registration Request</History>
        public async Task<IActionResult> ProcessCaseFile(Guid MojRegistrationRequestId, string CreatedBy)
        {
            try
            {
                await _ICmsCaseFile.ProcessCaseFile(MojRegistrationRequestId, CreatedBy);
                //if (historyobj != null)
                //{
                //    //Rabbit MQ send Messages
                //    var mapObj = _mapper.Map<UpdateEntityHistoryVM>(historyobj);
                //    mapObj.ReferenceId = historyobj.FileId;
                //    mapObj.SubModuleId = (int)SubModuleEnum.CaseFile;
                //    _client.SendMessage(mapObj, RabbitMQKeys.HistoryKey);
                //}
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetLawyersByCaseAndCanNumber")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Create Moj Registration Request</History>
        public async Task<IActionResult> GetLawyersByCaseAndCanNumber(string caseNumber, string canNumber)
        {
            try
            {
              
                return Ok(await _ICmsCaseFile.GetLawyersByCaseAndCanNumber(caseNumber, canNumber));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAllLawyersBySectorId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllLawyersBySectorId(int sectorId)
        {
            try
            {
                return Ok(await _ICmsCaseFile.GetAllLawyersBySectorId(sectorId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #region Get Case File Sector Assignment
        [HttpGet("GetCaseFileSectorAssigmentByFileId")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCaseFileSectorAssigmentByFileId(Guid fileId, int sectorTypeId)
        {
            try
            {
                return Ok(await _ICmsCaseFile.GetCaseFileSectorAssigmentByFileId(fileId, sectorTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Create Case File

        [HttpPost("CreateCaseFile")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCaseFile(CaseRequest caseRequest)
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
                var caseRequestCom = await _ICmsCaseFile.CreateCaseFile(caseRequest);
                if (caseRequestCom != null && caseRequest.StatusId != (int)CaseRequestStatusEnum.Draft)
                {
                    if(caseRequestCom.CaseFile != null)
                    {
                        caseRequestCom.CaseFile = await _ICmsCaseFile.CaseFileDetailWithPartiesAndAttachments(caseRequestCom.CaseFile.FileId);
                    }
                    //Rabbit MQ send Messages
                    _client.SendMessage(caseRequestCom, RabbitMQKeys.CreateCaseRequestFromFatwa);
                    if (caseRequest.CourtTypeId == (int)CourtTypeEnum.Regional)
                    {
                        await _ITask.AddTaskAndNotificationForHOSAndViceHOSOfSector(new SaveTaskVM
                        {
                            Task = new UserTask
                            {
                                Name = "Request_Created",
                                Description = "",
                                Date = DateTime.Now.Date,
                                AssignedBy = "System Generated",
                                IsSystemGenerated = true,
                                TaskStatusId = (int)TaskStatusEnum.Pending,
                                ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                                SectorId = (int)caseRequestCom.CaseRequest.SectorTypeId,
                                DepartmentId = (int)DepartmentEnum.Operational,
                                TypeId = (int)TaskTypeEnum.Task,
                                RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                                CreatedBy = caseRequestCom.CaseRequest.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReferenceId = caseRequestCom.CaseRequest.RequestId,
                                SubModuleId = (int)SubModuleEnum.CaseRequest,
                                SystemGenTypeId = (int)TaskSystemGenTypeEnum.CreateCaseRequest,
                                EntityId = (int)caseRequestCom.CaseRequest.GovtEntityId,
                            },
                            Action = "view",
                            EntityName = "caserequest",
                            EntityId = caseRequestCom.CaseRequest.RequestId.ToString()
                        },
                   new Notification
                   {
                       NotificationId = Guid.NewGuid(),
                       DueDate = DateTime.Now.AddDays(5),
                       CreatedBy = "System Generated",
                       CreatedDate = DateTime.Now,
                       IsDeleted = false,
                       ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                       EventId = (int)NotificationEventEnum.NewRequest,
                       Action = "view",
                       EntityName = new CaseRequest().GetType().Name,
                       EntityId = caseRequestCom.CaseRequest.RequestId.ToString(),
                       NotificationParameter = caseRequestCom.CaseRequest.NotificationParameter
                   },
                   (int)caseRequestCom.CaseRequest.SectorTypeId,
                   false,//Send True if required to Check Vice HOS Responsibility for All or for only Manager, send False if need to send Tasks/Notifications for All Vice HOS
                   true,//Send True if need to Include HOS as well along Vice HOSs
                   0//Send Chamber Number Id if need to send Tasks for Vice HOSs of specific Chamber Number
                   );
                    }
                }
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "New Case File Created",
                    Task = "To submit the new case file",
                    Description = "User able to Create Case file successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "file has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok(caseRequestCom);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for a new case Failed",
                    Body = ex.Message,
                    Category = "User unable to Create Case Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for a new case Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Drafted Case Request List
        [HttpPost("GetDraftedCaseRequestList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetDraftedCaseRequestList(AdvanceSearchCmsCaseRequestVM advanceSearchCmsCaseRequestVM)
        {
            try
            {
                return Ok(await _ICmsCaseFile.GetDraftedCaseRequestList(advanceSearchCmsCaseRequestVM));
            }
            catch(Exception ex)
            {
                return BadRequest( new BadRequestResponse { Message = ex.Message , InnerException = ex.InnerException?.Message});
            }
        }
        #endregion
    }
}
