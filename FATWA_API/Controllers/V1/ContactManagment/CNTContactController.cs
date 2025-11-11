using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.CaseManagement;
using FATWA_DOMAIN.Interfaces.Tasks;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.CaseManagment;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;
using FATWA_DOMAIN.Interfaces.Communication;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.ViewModel.ContactManagmentVMs;
using FATWA_DOMAIN.Interfaces.ContactManagment;
using FATWA_DOMAIN.Models.Contact;

namespace FATWA_API.Controllers.V1.ContactManagment
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CNTContactController : ControllerBase
    {
        private readonly ICNTContact _icntContact;
        private readonly IAuditLog _auditLogs;
        private readonly ITask _ITask;
        private readonly IAccount _IAccount;
        private readonly IConfiguration _configuration;
        private readonly INotification _INotification;
        private readonly IRole _IRole;

        #region Constructor

        public CNTContactController(ICNTContact icntContact, IAuditLog audit, ITask iTask, IAccount iAccount, ICommunication iCommunicationRepo, IConfiguration configuration, INotification iNotification, IRole iRole)
        {
            _icntContact = icntContact;
            _auditLogs = audit;
            _ITask = iTask;
            _IAccount = iAccount;
            _configuration = configuration;
            _INotification = iNotification;
            _IRole = iRole;
        }

        #endregion


        #region Get Contact List
        [HttpPost("GetContactList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetContactList(ContactAdvanceSearchVM AdvanceSearchVM)
        {
            try
            {
                return Ok(await _icntContact.GetContactList(AdvanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Contact  By Id
        //Author: Hassan Iftikhar
        [HttpGet("GetContactDetailById")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetContactDetailById(Guid contactId)
        {
            try
            {
                return Ok(await _icntContact.GetContactDetailById(contactId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion 
        #region Delete Contact
        //Author: Hassan Iftikhar
        [HttpPost("DeleteContact")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteContact(ContactListVM contact)
        {
            try
            {
                return Ok(await _icntContact.DeleteContact(contact));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion 

        #region Get Case List By ContactId
        [HttpGet("GetCaseListByContactId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCaseListByContactId(Guid contactId)
        {
            try
            {
                return Ok(await _icntContact.GetCaseListByContactId(contactId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Get Consultation List By ContactId
        [HttpGet("GetConsultationListByContactId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetConsultationListByContactId(Guid contactId)
        {
            try
            {
                return Ok(await _icntContact.GetConsultationListByContactId(contactId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Get Consultation Request List By ContactId
        [HttpGet("GetConsultationRequestListByContactId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetConsultationRequestListByContactId(Guid contactId)
        {
            try
            {
                return Ok(await _icntContact.GetConsultationRequestListByContactId(contactId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Get Case Request List By ContactId
        [HttpGet("GetCaseRequestListByContactId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCaseRequestListByContactId(Guid contactId)
        {
            try
            {
                return Ok(await _icntContact.GetCaseRequestListByContactId(contactId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Case File List
        [HttpGet("GetCaseFileList")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetCaseFileList()
        {
            try
            {
                return Ok(await _icntContact.GetCaseFileList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetConsultationFileList")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetConsultationFileList()
        {
            try
            {
                return Ok(await _icntContact.GetConsultationFileList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetCaseRequestList")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetCaseRequestList()
        {
            try
            {
                return Ok(await _icntContact.GetCaseRequestList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetConsultationRequestList")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetConsultationRequestList()
        {
            try
            {
                return Ok(await _icntContact.GetConsultationRequestList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion 

        #region Create Contact

        [HttpPost("CreateContact")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateContact(CntContact args)
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

                var result = await _icntContact.CreateContact(args);
                if (result)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Adding contact",
                        Task = "Adding contact process",
                        Description = "User able to Adding contact successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Adding contact executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });

                    var notificationResponse = await _INotification.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = args.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = "436e82d2-70d8-455c-a643-7909b8689667",//FATWA ADMIN
                        ModuleId = (int)WorkflowModuleEnum.CNTContactManagement,
                    },
                    (int)NotificationEventEnum.AddContact,
                    "list",
                    args.GetType().Name,
                    args.ContactId.ToString(),
                    args.NotificationParameter);
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Adding contact Failed",
                    Body = ex.Message,
                    Category = "User unable to contact legislation",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Adding contact Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
		#endregion

		#region Check Email, phone and civilid
		[HttpGet("CheckEmailExists")]
		[MapToApiVersion("1.0")]

		public async Task<IActionResult> CheckEmailExists(Guid contactId, string email)
		{
			try
			{
				return Ok(await _icntContact.CheckEmailExists(contactId, email));
			}
			catch (Exception ex)
			{
				return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
			}
		}
		[HttpGet("CheckPhoneNumberExists")]
		[MapToApiVersion("1.0")]

		public async Task<IActionResult> CheckPhoneNumberExists(Guid contactId, string phoneNumber)
		{
			try
			{
				return Ok(await _icntContact.CheckPhoneNumberExists(contactId, phoneNumber));
			}
			catch (Exception ex)
			{
				return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
			}
		}
		[HttpGet("CheckCivilIdExists")]
		[MapToApiVersion("1.0")]

		public async Task<IActionResult> CheckCivilIdExists(Guid contactId, string civilId)
		{
			try
			{
				return Ok(await _icntContact.CheckCivilIdExists(contactId, civilId));
			}
			catch (Exception ex)
			{
				return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
			}
		}
        #endregion

        #region Get Contact Detail By Using ContactId

        [HttpGet("GetContactDetailByUsingContactId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetContactDetailByUsingContactId(Guid contactId)
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

                var result = await _icntContact.GetContactDetailByUsingContactId(contactId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
		#endregion

		#region Update contact request

		[HttpPost("UpdateContact")]
		[MapToApiVersion("1.0")]
		//<History Author = 'Umer Zaman' Date='2023-04-06' Version="1.0" Branch="master"> Update contact request</History>
		public async Task<IActionResult> UpdateContact(CntContact args)
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
				var consultationRequestCommunication = await _icntContact.UpdateContact(args);
				
				_auditLogs.CreateProcessLog(new ProcessLog
				{
					Process = "Update the contact Request",
					Task = "Update the contact",
					Description = "Contact has been updated.",
					ProcessLogEventId = (int)ProcessLogEnum.Processed,
					Message = "Contact has been updated",
					IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
					ApplicationID = (int)PortalEnum.FatwaPortal,
					ModuleID = (int)ModuleEnum.LegalLibrarySystem,
					Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
				});
				return Ok();
			}
			catch (Exception ex)
			{
				_auditLogs.CreateErrorLog(new ErrorLog
				{
					ErrorLogEventId = (int)ErrorLogEnum.Error,
					Subject = "Update the contact Request Failed",
					Body = ex.Message,
					Category = "User unable to edit the contact Request",
					Source = ex.Source,
					Type = ex.GetType().Name,
					Message = "The request could not be updated",
					IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
					ApplicationID = (int)PortalEnum.FatwaPortal,
					ModuleID = (int)ModuleEnum.LegalLibrarySystem,
					Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
				});
				return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
			}
		}

		#endregion
	}
}
