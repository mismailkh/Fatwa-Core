using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using static FATWA_DOMAIN.Enums.LiteratureEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;
using NotificationCategoryEnum = FATWA_GENERAL.Helper.Enum.NotificationCategoryEnum;
using NotificationEventEnum = FATWA_GENERAL.Helper.Enum.NotificationEventEnum;
//using NotificationTypeEnum = FATWA_GENERAL.Helper.Enum.NotificationTypeEnum;

namespace FATWA_API.Controllers.V1.Lms
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //<History Author = 'Zain Ul Islam' Date='2022-03-23' Version="1.0" Branch="master"> API Controller for handling request/responses</History>
    public class LmsLiteratureBorrowDetailsController : ControllerBase
    {
        private readonly ILmsLiterature _ILmsLiteratures;
        private readonly ILmsLiteratureBorrowDetail _ILmsLiteratureBorrowDetails;
        private readonly IAuditLog _auditLogs;
        private readonly INotification _INotification;
        private readonly IAccount _IAccount;



        public LmsLiteratureBorrowDetailsController(ILmsLiterature iLmsLiteratures, ILmsLiteratureBorrowDetail iLmsLiteratureBorrowDetails, IAuditLog audit, INotification iNotification, IAccount iAccount)
        {
            _ILmsLiteratures = iLmsLiteratures;
            _ILmsLiteratureBorrowDetails = iLmsLiteratureBorrowDetails;
            _auditLogs = audit;
            _INotification = iNotification;
            _IAccount = iAccount;
        }

        #region Literature Borrow Detail CRUD

        [HttpPost("GetLmsLiteratureBorrowDetails")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Zain Ul Islam' Date='2022-03-23' Version="1.0" Branch="master"> Return List of Literature Borrow Details</History>
        public async Task<IActionResult> GetLmsLiteratureBorrowDetails(UserDetailVM? loggedUser)
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
                return Ok(await _ILmsLiteratureBorrowDetails.GetLmsLiteratureBorrowDetails(loggedUser));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetLmsLiteratureReturnDetails")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel Ur Rehman' Date='2022-03-23' Version="1.0" Branch="master"> Return List of Literature Return Details</History>
        public async Task<IActionResult> GetLmsLiteratureReturnDetails(UserDetailVM? loggedUser)
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
                return Ok(await _ILmsLiteratureBorrowDetails.GetLmsLiteratureReturnDetails(loggedUser));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetLmsLiteratureBorrowDetailById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Zain Ul Islam' Date='2022-03-23' Version="1.0" Branch="master"> Get Literature Classifcation on Id</History>
        public async Task<IActionResult> Get(int id)
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
                LmsLiteratureBorrowDetail lit = await _ILmsLiteratureBorrowDetails.GetLmsLiteratureBorrowDetailById((int)id);
                if (lit != null)
                {
                    return Ok(lit);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateLmsLiteratureBorrowDetail")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Zain Ul Islam' Date='2022-03-23' Version="1.0" Branch="master"> create Literature Borrow Details</History>
        public async Task<IActionResult> CreateLmsLiteratureBorrowDetail(LmsLiteratureBorrowDetail literatureBorrow)
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
                var entityId = await _ILmsLiteratureBorrowDetails.CreateLmsLiteratureBorrowDetail(literatureBorrow);
                if (entityId == null)
                    return NotFound();

                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Create LMS Literature Borrow Detail",
                    Task = "Create LMS Literature Borrow Detail",
                    Description = "User able to Create LMS Literature Borrow Detail successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Create LMS Literature Borrow Detail executed Successfully",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });


                if (literatureBorrow.RoleId == null)
                {
                    var lmsAdminUsers = await _ILmsLiteratures.GetLmsAdminUser();
                    foreach (var user in lmsAdminUsers)
                    {
                        var notificationResult = await _INotification.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = literatureBorrow.CreatedBy,
                            CreatedDate = literatureBorrow.CreatedDate,
                            IsDeleted = false,
                            ReceiverId = user.UserId, //LIBRARY ADMIN
                            ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                        },
                        (int)NotificationEventEnum.CreateLmsLiteratureBorrowRequest,
                        "list",
                        "lmsliteratureborrowdetail-approval",
                        null,
                        literatureBorrow.NotificationParameter);

                    }
                    var fatwaAdminUsers = await _ILmsLiteratures.GetFatwaAdminUser();
                    foreach (var user in fatwaAdminUsers)
                    {
                        var notificationResult = await _INotification.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = literatureBorrow.CreatedBy,
                            CreatedDate = literatureBorrow.CreatedDate,
                            IsDeleted = false,
                            ReceiverId = user.UserId, //FATWA ADMIN
                            ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                        },
                        (int)NotificationEventEnum.CreateLmsLiteratureBorrowRequest,
                        "list",
                        "lmsliteratureborrowdetail-approval",
                        null,
                        literatureBorrow.NotificationParameter);

                    }
                }

                // Notification For Request created
                //var notificationResult = await _INotification.SendNotification(new Notification
                //{
                //    NotificationId = Guid.NewGuid(),
                //    DueDate = DateTime.Now.AddDays(5),
                //    CreatedBy = literatureBorrow.CreatedBy,
                //    CreatedDate = DateTime.Now,
                //    IsDeleted = false,
                //    ReceiverId = "9779af5f-e7a4-4ebf-9af0-52beb251b4eb", //LIBRARY ADMIN
                //    ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                //},
                //(int)NotificationEventEnum.CreateLmsLiteratureBorrowRequest,
                //"edit",
                //literatureBorrow.GetType().Name,
                //entityId.ToString(),
                //literatureBorrow.NotificationParameter);

                // Notification For Request received
                //notificationResult = await _INotification.SendNotification(new Notification
                //{
                //    NotificationId = Guid.NewGuid(),
                //    DueDate = DateTime.Now.AddDays(5),
                //    CreatedBy = literatureBorrow.CreatedBy,
                //    CreatedDate = DateTime.Now,
                //    IsDeleted = false,
                //    ReceiverId = "9779af5f-e7a4-4ebf-9af0-52beb251b4eb", //LIBRARY ADMIN
                //    ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                //},
                //(int)NotificationEventEnum.ReceivedLmsLiteratureBorrowRequest,
                //"edit",
                //literatureBorrow.GetType().Name,
                //entityId.ToString(),
                //literatureBorrow.NotificationParameter);


                return Ok(literatureBorrow);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Create LMS Literature Borrow Detail Failed",
                    Body = ex.Message,
                    Category = "User unable to Get Role List",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Create LMS Literature Borrow Detail Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }
        }

        [HttpPost("UpdateLmsLiteratureBorrowDetail")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Zain Ul Islam' Date='2022-03-23' Version="1.0" Branch="master"> Update Literature Classifcation</History>
        public async Task<IActionResult> UpdateLmsLiteratureBorrowDetail(LmsLiteratureBorrowDetail literatureBorrow)
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
                await _ILmsLiteratureBorrowDetails.UpdateLmsLiteratureBorrowDetail(literatureBorrow);

                if (literatureBorrow.ReturnDate != null)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Returning Borrowed Book",
                        Task = "Returning Borrowed Book",
                        Description = "User able to Return Borrowed Book successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Get Role List executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                    string AssignedBy = await _IAccount.UserIdByUserEmail(literatureBorrow.CreatedBy);
                    var notificationResult = await _INotification.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = literatureBorrow.ModifiedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = AssignedBy, // Notification for creator
                        ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                    },
                    (int)NotificationEventEnum.LmsLiteratureBorrowRequestReturned,
                    "list",
                    "lmsliteratureborrowdetail",
                     null,
                    literatureBorrow.NotificationParameter);

                }
                else if (literatureBorrow.BorrowApprovalStatus == (int)BorrowApprovalStatus.Approved)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Returning Borrowed Book",
                        Task = "Returning Borrowed Book",
                        Description = "User able to Return Borrowed Book successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Get Role List executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                    //var notificationResult = await _INotification.SendNotification(new Notification
                    //{
                    //    NotificationId = Guid.NewGuid(),
                    //    DueDate = DateTime.Now.AddDays(5),
                    //    CreatedBy = literatureBorrow.CreatedBy,
                    //    CreatedDate = DateTime.Now,
                    //    IsDeleted = false,
                    //    ReceiverId = "9779af5f-e7a4-4ebf-9af0-52beb251b4eb", //LIBRARY ADMIN
                    //    ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                    //},
                    //             (int)NotificationEventEnum.LmsLiteratureBorrowRequestForApproval,
                    //             "edit",
                    //             literatureBorrow.GetType().Name,
                    //             literatureBorrow.BorrowId.ToString(),
                    //             literatureBorrow.NotificationParameter);

                    string AssignedBy = await _IAccount.UserIdByUserEmail(literatureBorrow.CreatedBy);
                    var notificationResult = await _INotification.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = literatureBorrow.ModifiedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = AssignedBy, // Notification for creator
                        ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                    },
                    (int)NotificationEventEnum.LmsLiteratureBorrowRequestApproved,
                    "list",
                    "lmsliteratureborrowdetail",
                     null,
                    literatureBorrow.NotificationParameter);
                }

                else if (literatureBorrow.BorrowApprovalStatus == (int)BorrowApprovalStatus.Rejected)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Borrowed Book Rejected",
                        Task = "Borrowed Book Rejected",
                        Description = "User able to Return Borrowed Book successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Get Role List executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                    //var notificationResult = await _INotification.SendNotification(new Notification
                    //{
                    //    NotificationId = Guid.NewGuid(),
                    //    DueDate = DateTime.Now.AddDays(5),
                    //    CreatedBy = literatureBorrow.CreatedBy,
                    //    CreatedDate = DateTime.Now,
                    //    IsDeleted = false,
                    //    ReceiverId = "9779af5f-e7a4-4ebf-9af0-52beb251b4eb", //LIBRARY ADMIN
                    //    ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                    //},
                    //              (int)NotificationEventEnum.LmsLiteratureBorrowRequestForRejection,
                    //              "edit",
                    //              literatureBorrow.GetType().Name,
                    //              literatureBorrow.BorrowId.ToString(),
                    //              literatureBorrow.NotificationParameter);

                    string AssignedBy = await _IAccount.UserIdByUserEmail(literatureBorrow.CreatedBy);
                    var notificationResult = await _INotification.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = literatureBorrow.ModifiedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = AssignedBy, // Notification for creator
                        ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                    },
                    (int)NotificationEventEnum.LmsLiteratureBorrowRequestRejected,
                    "list",
                    "lmsliteratureborrowdetail",
                     null,
                    literatureBorrow.NotificationParameter);
                }
                else if (literatureBorrow.Extended)
                {
                    string AssignedBy = await _IAccount.UserIdByUserEmail(literatureBorrow.CreatedBy);
                    var notificationResult = await _INotification.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = literatureBorrow.ModifiedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = AssignedBy, // Notification for creator
                        ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                    },
                    (int)NotificationEventEnum.LmsLiteratureBorrowRequestForExtended,
                    "list",
                    "lmsliteratureborrowdetail",
                     null,
                    literatureBorrow.NotificationParameter);
                }

                return Ok(literatureBorrow);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Return Borrowed Book Failed",
                    Body = ex.Message,
                    Category = "User unable to Return Borrowed Book ",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Return Borrowed Book Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPost("UpdateLmsLiteratureRetunDetail")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-03-23' Version="1.0" Branch="master"> Update Literature return Date </History>
        public async Task<IActionResult> UpdateLmsLiteratureRetunDetail(BorrowDetailVM literatureBorrow)
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
                await _ILmsLiteratureBorrowDetails.UpdateLmsLiteratureRetunDetail(literatureBorrow);

                if (literatureBorrow.ApplyReturnDate != null)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Returning Borrowed Book",
                        Task = "Returning Borrowed Book",
                        Description = "User able to Return Borrowed Book successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Get Role List executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });

                    //var notificationResult = await _INotification.SendNotification(new Notification
                    //{
                    //    NotificationId = Guid.NewGuid(),
                    //    DueDate = DateTime.Now.AddDays(5),
                    //    CreatedBy = literatureBorrow.CreatedBy,
                    //    CreatedDate = DateTime.Now,
                    //    IsDeleted = false,
                    //    ReceiverId = "9779af5f-e7a4-4ebf-9af0-52beb251b4eb", //LIBRARY ADMIN
                    //    ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                    //},
                    //              (int)NotificationEventEnum.LmsLiteratureBorrowRequestForReturn,
                    //              "edit",
                    //              literatureBorrow.GetType().Name,
                    //              literatureBorrow.BorrowId.ToString(),
                    //              literatureBorrow.NotificationParameter);

                    if (literatureBorrow.RoleId == null)
                    {
                        var lmsAdminUsers = await _ILmsLiteratures.GetLmsAdminUser();
                        foreach (var user in lmsAdminUsers)
                        {
                            var notificationResult = await _INotification.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = literatureBorrow.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = user.UserId,
                                ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                            },
                            (int)NotificationEventEnum.LmsLiteratureBorrowRequestForReturn,
                            "list",
                            "lmsliteratureReturndetail",
                            null,
                            literatureBorrow.NotificationParameter);
                        }
                        var fatwaAdminUsers = await _ILmsLiteratures.GetFatwaAdminUser();
                        foreach (var user in fatwaAdminUsers)
                        {
                            var notificationResult = await _INotification.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = literatureBorrow.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = user.UserId,
                                ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                            },
                            (int)NotificationEventEnum.LmsLiteratureBorrowRequestForReturn,
                            "list",
                            "lmsliteratureReturndetail",
                            null,
                            literatureBorrow.NotificationParameter);
                        }
                    }
                }
                else if (literatureBorrow.Extended)
                {
                    if (literatureBorrow.RoleId == null)
                    {
                        var lmsAdminUsers = await _ILmsLiteratures.GetLmsAdminUser();
                        foreach (var user in lmsAdminUsers)
                        {
                            var notificationResult = await _INotification.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = literatureBorrow.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = user.UserId,
                                ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                            },
                            (int)NotificationEventEnum.LmsLiteratureBorrowRequestForExtension,
                            "list",
                            "lmsliteratureReturndetail",
                            null,
                            literatureBorrow.NotificationParameter);
                        }
                        var fatwaAdminUsers = await _ILmsLiteratures.GetFatwaAdminUser();
                        foreach (var user in fatwaAdminUsers)
                        {
                            var notificationResult = await _INotification.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = literatureBorrow.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = user.UserId,
                                ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                            },
                            (int)NotificationEventEnum.LmsLiteratureBorrowRequestForExtension,
                            "list",
                            "lmsliteratureReturndetail",
                            null,
                            literatureBorrow.NotificationParameter);
                        }
                    }
                }
                return Ok(literatureBorrow);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Return Borrowed Book Failed",
                    Body = ex.Message,
                    Category = "User unable to Return Borrowed Book ",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Return Borrowed Book Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpDelete("{item}")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Zain Ul Islam' Date='2022-03-23' Version="1.0" Branch="master"> Delete Literature Classifcation</History>
        public async Task<IActionResult> DeleteLmsLiteratureBorrow(BorrowDetailVM item)
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

                bool isDeleted = await _ILmsLiteratureBorrowDetails.DeleteLiteratureBorrow(item);
                if (isDeleted)
                {
                    var notificationResponse = await _INotification.DeleteNotificationByEntityAndId("/lmsliteratureReturndetail-list/", null, item.DeletedBy, (int)WorkflowModuleEnum.LMSLiterature, item.CreatedDate);
                    if (notificationResponse)
                        return Ok();
                    else
                        return BadRequest();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Literature Extension Borrow Approval

        [HttpGet("GetLmsLiteratureBorrowExtensionApprovals")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Zain Ul Islam' Date='2022-03-23' Version="1.0" Branch="master"> Return List of Literature Borrow Details</History>
        public async Task<IActionResult> GetLmsLiteratureBorrowExtensionApprovals()
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
                var result = await _ILmsLiteratureBorrowDetails.GetLmsLiteratureBorrowExtensionApprovals();
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

        [HttpGet("GetLiteratureBorrowApprovalTypes")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLiteratureBorrowApprovalTypes()
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
                IEnumerable<LiteratureBorrowApprovalType> types = await _ILmsLiteratureBorrowDetails.GetLiteratureBorrowApprovalTypes();
                if (types != null)
                {
                    return Ok(types);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateLiteratureBorrowApprovalStatus")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Zain Ul Islam' Date='2022-03-23' Version="1.0" Branch="master"> Update Literature Classifcation</History>
        public async Task<IActionResult> UpdateLiteratureBorrowApprovalStatus(LmsLiteratureBorrowDetail LmsLiteratureBorrowDetail)
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

                await _ILmsLiteratureBorrowDetails.UpdateLiteratureBorrowApprovalStatus(LmsLiteratureBorrowDetail);
                if (LmsLiteratureBorrowDetail.Extended == true && LmsLiteratureBorrowDetail.ExtensionApprovalStatus == (int)BorrowApprovalStatus.Extended)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Approved Borrowed Extension Period",
                        Task = "Approve Borrowed Extension Period",
                        Description = "User able to Approve Borrowed Extension Period successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Approve Borrowed Extension Period executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                    string AssignedBy = await _IAccount.UserIdByUserEmail(LmsLiteratureBorrowDetail.CreatedBy);

                    var notificationResult = await _INotification.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = LmsLiteratureBorrowDetail.ModifiedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = AssignedBy,
                        ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                    },
                    (int)NotificationEventEnum.LmsLiteratureBorrowExtensionRequestApproved,
                    "list",
                    "lmsliteratureborrowdetail",
                     null,
                    LmsLiteratureBorrowDetail.NotificationParameter);

                }

                if (LmsLiteratureBorrowDetail.Extended == true && LmsLiteratureBorrowDetail.ExtensionApprovalStatus == (int)BorrowApprovalStatus.ExtensionRejected)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Rejected Borrowed Extension Period",
                        Task = "Reject Borrowed Extension Period",
                        Description = "User able to Approve Borrowed Extension Period successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Reject Borrowed Extension Period executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                    string AssignedBy = await _IAccount.UserIdByUserEmail(LmsLiteratureBorrowDetail.CreatedBy);
                    var notificationResult = await _INotification.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = LmsLiteratureBorrowDetail.ModifiedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = AssignedBy,
                        ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                    },
                    (int)NotificationEventEnum.LmsLiteratureBorrowExtensionRequestRejected,
                    "list",
                    "lmsliteratureborrowdetail",
                     null,
                    LmsLiteratureBorrowDetail.NotificationParameter);
                }
                return Ok(LmsLiteratureBorrowDetail);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Approve Borrowed Extension Period Failed",
                    Body = ex.Message,
                    Category = "User unable to Approve Borrowed Extension Period",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Approve Borrowed Extension Period Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }
        }


        #endregion

        #region Literature Borrow Approval

        [HttpGet("GetLiteratureBorrowApprovals")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Zain Ul Islam' Date='2022-03-23' Version="1.0" Branch="master"> Return List of Literature Borrow Details</History>
        public async Task<IActionResult> GetLiteratureBorrowApprovals()
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
                var result = await _ILmsLiteratureBorrowDetails.GetLiteratureBorrowApprovals();
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

        #region Get Borrow Approval Status Details
        [HttpGet("GetBorrowApprovalStatusDetails")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Umer Zaman' Date='2024-08-13' Version="1.0" Branch="master"> Return List of Details</History>
        public async Task<IActionResult> GetBorrowApprovalStatusDetails()
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
                var result = await _ILmsLiteratureBorrowDetails.GetBorrowApprovalStatusDetails();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #region Get Borrow Approval Status Details
        [HttpGet("GetBorrowedLiteratureAndUserDetailByUserIdAndCivilId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Umer Zaman' Date='2024-08-13' Version="1.0" Branch="master"> Return List of Details</History>
        public async Task<IActionResult> GetBorrowedLiteratureAndUserDetailByUserIdAndCivilId(string? UserId = null, string? civilId = null)
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
                var result = await _ILmsLiteratureBorrowDetails.GetBorrowedLiteratureAndUserDetailByUserIdAndCivilId(UserId, civilId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
        [HttpPost("UpdateLiteratureReturnExtendDetail")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateLiteratureReturnExtendDetail(BorrowedLiteratureVM literatureBorrow)
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
                await _ILmsLiteratureBorrowDetails.UpdateLiteratureReturnExtendDetail(literatureBorrow);
                literatureBorrow.NotificationParameter.Name = literatureBorrow.BookName;
                if (literatureBorrow.ReturnDate != null)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Returning Borrowed Book",
                        Task = "Returning Borrowed Book",
                        Description = "User able to Return Borrowed Book successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Get Role List executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });

                    var notificationResult = await _INotification.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = literatureBorrow.LoggedInUser,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = literatureBorrow.BorrowerUserId, 
                        ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                    },
                       (int)NotificationEventEnum.LmsLiteratureBorrowRequestReturned,
                       "list",
                       "lmsliterature",
                       null,
                       literatureBorrow.NotificationParameter);

                }
                if (literatureBorrow.IsNew == true)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "New Book Borrowed",
                        Task = "Borrowing New Book",
                        Description = "User able to Borrow Book successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Get Role List executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                    var notificationResult = await _INotification.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = literatureBorrow.LoggedInUser,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = literatureBorrow.BorrowerUserId,
                        ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                    },
                      (int)NotificationEventEnum.CreateLmsLiteratureBorrowRequest,
                      "list",
                      "lmsliterature",
                      null,
                      literatureBorrow.NotificationParameter);

                }
                if (literatureBorrow.Extended == true && literatureBorrow.ReturnDate == null)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Book Extension",
                        Task = "Extending Borrow Book",
                        Description = "User able to Extend Book successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Get Role List executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                    var notificationResult = await _INotification.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = literatureBorrow.LoggedInUser,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = literatureBorrow.BorrowerUserId,
                        ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                    },
                      (int)NotificationEventEnum.LmsLiteratureBorrowRequestForExtended,
                      "list",
                      "lmsliterature",
                      null,
                      literatureBorrow.NotificationParameter);

                }
                return Ok(literatureBorrow);

            }
            catch (Exception ex)
            {
                if (literatureBorrow.IsNew == true)
                {
                    _auditLogs.CreateErrorLog(new ErrorLog
                    {
                        ErrorLogEventId = (int)ErrorLogEnum.Error,
                        Subject = "Add New Borrow Book Failed",
                        Body = ex.Message,
                        Category = "User unable to Add New Borrow Book ",
                        Source = ex.Source,
                        Type = ex.GetType().Name,
                        Message = "Return Borrowed Book Failed",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                }
                if (literatureBorrow.ReturnDate != null)
                { 
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Return Borrowed Book Failed",
                    Body = ex.Message,
                    Category = "User unable to Return Borrowed Book ",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Return Borrowed Book Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
            }
                if (literatureBorrow.Extended == true)
                { 
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Extend Borrowed Book Failed",
                    Body = ex.Message,
                    Category = "User unable to Extend Borrowed Book ",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Return Borrowed Book Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
            }

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
      
        [HttpGet("GetLiteratureByBarcode")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLiteratureByBarcode(string barCode)
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
                var res = await _ILmsLiteratureBorrowDetails.GetLiteratureByBarcode(barCode);



                return Ok(res);

            }
            catch (Exception ex)
            {
            

                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("GetUserBorrowHistoryByUserId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUserBorrowHistoryByUserId(string userId)
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
                var res = await _ILmsLiteratureBorrowDetails.GetUserBorrowHistoryByUserId(userId);



                return Ok(res);

            }
            catch (Exception ex)
            {
            

                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAllLmsUserList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllLmsUserList()
        {
            try
            {
                var result = await _ILmsLiteratureBorrowDetails.GetAllLmsUserList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("GetLmsBorrowLiteraturesAdvanceSearch")]

        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLmsBorrowLiteraturesAdvanceSearch(LiteratureAdvancedSearchVM advancedSearch)
        {
            try
            {
                return Ok(await _ILmsLiteratureBorrowDetails.GetLmsBorrowLiteraturesAdvanceSearch(advancedSearch));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
