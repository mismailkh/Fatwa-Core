using AutoMapper;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FATWA_DOMAIN.Interfaces.LLSLegalPrinciple;
using static FATWA_GENERAL.Helper.Response;
using FATWA_DOMAIN.Models.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using FATWA_DOMAIN.Models;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.ViewModel.LegalPrinciple;
using static FATWA_DOMAIN.Enums.LegalPrinciple.LegalPrincipleEnum;
using System.DirectoryServices.AccountManagement;
using System.Net;

namespace FATWA_API.Controllers.V1.LLSLegalPrinciple
{
	//<!-- <History Author = 'Umer Zaman' Date='2024-04-18' Version="1.0" Branch="master">Create class to manage api controller</History>

	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class LLSLegalPrincipleController : ControllerBase
	{
		#region Constructure
		public LLSLegalPrincipleController(ILLSLegalPrinciple iLLSLegalPrinciple, IAuditLog audit, INotification iNotification, IConfiguration configuration, IMapper mapper , IAccount iAccount)
		{
			_iLLSLegalPrinciple = iLLSLegalPrinciple;
			_auditLogs = audit;
			_INotification = iNotification;
			_configuration = configuration;
			_mapper = mapper;
            _IAccount = iAccount;

        }
        #endregion

        #region Variable declarations
        private readonly ILLSLegalPrinciple _iLLSLegalPrinciple;
		private readonly IAuditLog _auditLogs;
		private readonly INotification _INotification;
		private readonly IConfiguration _configuration;
		private readonly IMapper _mapper;
        private readonly IAccount _IAccount;
        #endregion

        #region Get legal principle category details
        [HttpGet("GetLLSLegalPrincipleCategory")]
		[MapToApiVersion("1.0")]
		public async Task<IActionResult> GetLLSLegalPrincipleCategory()
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
				var result = await _iLLSLegalPrinciple.GetLLSLegalPrincipleCategory();
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		#endregion

		#region Save legal principle category

		[HttpPost("SaveLegalPrincipleCategory")]
		[MapToApiVersion("1.0")]
		//<History Author = 'Umer Zaman' Date='2024-04-20' Version="1.0" Branch="master"> Handle create  legal principle category</History>
		public async Task<IActionResult> SaveLegalPrincipleCategory(LLSLegalPrincipleCategory item)
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
				var result = await _iLLSLegalPrinciple.SaveLegalPrincipleCategory(item);
				if (result)
				{
					_auditLogs.CreateProcessLog(new ProcessLog
					{
						Process = "Adding legal principle category",
						Task = "Adding legal principle category process",
						Description = "User able to legal principle category successfully.",
						ProcessLogEventId = (int)ProcessLogEnum.Processed,
						Message = "Adding legal principle category executed Successfully",
						IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
						ApplicationID = (int)PortalEnum.FatwaPortal,
						ModuleID = (int)ModuleEnum.LegalLibrarySystem,
						Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
					});

				}
                return Ok(result);
            }
            catch (Exception ex)
			{
				_auditLogs.CreateErrorLog(new ErrorLog
				{
					ErrorLogEventId = (int)ErrorLogEnum.Error,
					Subject = "Adding legal principle category Failed",
					Body = ex.Message,
					Category = "User unable to legal principle category",
					Source = ex.Source,
					Type = ex.GetType().Name,
					Message = "Adding legal principle category Failed",
					IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
					ApplicationID = (int)PortalEnum.FatwaPortal,
					ModuleID = (int)ModuleEnum.LegalLibrarySystem,
					Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
				});

				return BadRequest(ex.Message);
			}
		}

		#endregion

		#region Advance search principle relation

		[HttpPost("AdvanceSearchPrincipleRelation")]
		[MapToApiVersion("1.0")]
		public async Task<IActionResult> AdvanceSearchPrincipleRelation(LLSLegalPrinciplesRelationVM item)
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
				return Ok(await _iLLSLegalPrinciple.AdvanceSearchPrincipleRelation(item));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		#endregion

		#region Get legal principle detail by using document id
		[HttpGet(nameof(GetLLSLegalPrinciples))]
		[MapToApiVersion("1.0")]
		//<History Author = 'Muhammad Abuzar' Date='2024-04-20' Version="1.0" Branch="master"> </History>
		public async Task<IActionResult> GetLLSLegalPrinciples(int uploadedDocumentId)
		{
			try
			{
				var result = await _iLLSLegalPrinciple.GetLLSLegalPrinciples(uploadedDocumentId);
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

		#region Save legal principle
		[HttpPost("SaveLLSLegalPrinciple")]
		[MapToApiVersion("1.0")]
		//<History Author = 'Umer Zaman' Date='2024-04-20' Version="1.0" Branch="master"> Create legal principle</History>
		public async Task<IActionResult> SaveLLSLegalPrinciple(LLSLegalPrincipleSystem lLSLegalPrinciple)
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
				var result = await _iLLSLegalPrinciple.SaveLLSLegalPrinciple(lLSLegalPrinciple);
				if (result)
				{
					if (lLSLegalPrinciple.ModifiedBy == null)
					{
                        _auditLogs.CreateProcessLog(new ProcessLog
                        {
                            Process = "Create New Legal Principle ",
                            Task = "Create New Legal Principle ",
                            Description = "User able to Create New Legal Principle  successfully.",
                            ProcessLogEventId = (int)ProcessLogEnum.Processed,
                            Message = "Create New Legal Principle  executed Successfully",
                            IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                            ApplicationID = (int)PortalEnum.FatwaPortal,
                            ModuleID = (int)WorkflowModuleEnum.LPSPrinciple,
                            Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                        });
                        // notification requirement is missing
                    }
					else
					{
                        _auditLogs.CreateProcessLog(new ProcessLog
                        {
                            Process = "Modified Legal Principle ",
                            Task = "Modified Legal Principle ",
                            Description = "User able to Modified Legal Principle  successfully.",
                            ProcessLogEventId = (int)ProcessLogEnum.Processed,
                            Message = "Modified Legal Principle  executed Successfully",
                            IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                            ApplicationID = (int)PortalEnum.FatwaPortal,
                            ModuleID = (int)WorkflowModuleEnum.LPSPrinciple,
                            Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                        });
                        // notification requirement is missing
                    }

                }
				return Ok();
			}
			catch (Exception ex)
			{
				if (lLSLegalPrinciple.ModifiedBy == null)
				{
                    _auditLogs.CreateErrorLog(new ErrorLog
                    {
                        ErrorLogEventId = (int)ErrorLogEnum.Error,
                        Subject = "Creating New Legal Principle  Failed",
                        Body = ex.Message,
                        Category = "User unable to Create New Legal Principle ",
                        Source = ex.Source,
                        Type = ex.GetType().Name,
                        Message = "Creating New Legal Principle  Failed",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LPSPrinciple,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                }
				else
				{
                    _auditLogs.CreateErrorLog(new ErrorLog
                    {
                        ErrorLogEventId = (int)ErrorLogEnum.Error,
                        Subject = "Modified Legal Principle  Failed",
                        Body = ex.Message,
                        Category = "User unable to Modified Legal Principle ",
                        Source = ex.Source,
                        Type = ex.GetType().Name,
                        Message = "Modifying Legal Principle  Failed",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LPSPrinciple,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                }
				
				return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
			}
		}
		#endregion

		#region Update legal principle
		[HttpPost("UpdateLLSLegalPrinciple")]
		[MapToApiVersion("1.0")]
		//<History Author = 'Umer Zaman' Date='2024-05-27' Version="1.0" Branch="master"> Create legal principle</History>
		public async Task<IActionResult> UpdateLLSLegalPrinciple(LLSLegalPrincipleSystem lLSLegalPrinciple)
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
				var result = await _iLLSLegalPrinciple.UpdateLLSLegalPrinciple(lLSLegalPrinciple);
				if (result)
				{
					_auditLogs.CreateProcessLog(new ProcessLog
					{
						Process = "Modified Legal Principle ",
						Task = "Modified Legal Principle ",
						Description = "User able to Modified Legal Principle  successfully.",
						ProcessLogEventId = (int)ProcessLogEnum.Processed,
						Message = "Modified Legal Principle  executed Successfully",
						IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
						ApplicationID = (int)PortalEnum.FatwaPortal,
						ModuleID = (int)WorkflowModuleEnum.LPSPrinciple,
						Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
					});
					// notification requirement is missing
				}
				return Ok();
			}
			catch (Exception ex)
			{
				_auditLogs.CreateErrorLog(new ErrorLog
				{
					ErrorLogEventId = (int)ErrorLogEnum.Error,
					Subject = "Modified Legal Principle  Failed",
					Body = ex.Message,
					Category = "User unable to Modified Legal Principle ",
					Source = ex.Source,
					Type = ex.GetType().Name,
					Message = "Modifying Legal Principle  Failed",
					IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
					ApplicationID = (int)PortalEnum.FatwaPortal,
					ModuleID = (int)WorkflowModuleEnum.LPSPrinciple,
					Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
				});

				return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
			}
		}
		#endregion

		#region Get legal Principle detail by principle Id
		[HttpGet(nameof(GetLLSLegalPrincipleDetailById))]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Abuzar' Date='2024-04-20' Version="1.0" Branch="master"> </History>
        public async Task<IActionResult> GetLLSLegalPrincipleDetailById(Guid principleId)
        {
            try
            {
                var result = await _iLLSLegalPrinciple.GetLLSLegalPrincipleDetailById(principleId);
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

		#region Get legal Principle References by principle Id
        [HttpGet(nameof(GetLLSLegalPrincipleReferencesById))]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Abuzar' Date='2024-04-20' Version="1.0" Branch="master"> </History>
        public async Task<IActionResult> GetLLSLegalPrincipleReferencesById(Guid principleId)
        {
            try
            {
                var result = await _iLLSLegalPrinciple.GetLLSLegalPrincipleReferencesById(principleId);
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

		#region Get LLS Legal Principle Details By Using PrincipleId For Edit Form

		[HttpGet("GetLLSLegalPrincipleDetailsByUsingPrincipleContentId")]
		[MapToApiVersion("1.0")]
		public async Task<IActionResult> GetLLSLegalPrincipleDetailsByUsingPrincipleContentId(Guid principleContentId)
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

				var result = await _iLLSLegalPrinciple.GetLLSLegalPrincipleDetailsByUsingPrincipleContentId(principleContentId);
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

        #region Soft Delete Legal Principle
        [HttpPost("DeleteLLSLegalPrinciple")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteLLSLegalPrinciple(LLSLegalPrinciplesVM args)
        {

            try
            {
                var result = await _iLLSLegalPrinciple.DeleteLLSLegalPrinciple(args);
				if(result)
				{
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Delete Legal Principle ",
                        Task = "Delete Legal Principle",
                        Description = "User able to Delete Legal Principle successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Deleting Legal Principle executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LPSPrinciple,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
					});

     //               string assignedTo = await _IAccount.UserIdByUserEmail(legalPrinciplesVM.AddedBy);
     //               var notificationResult = await _INotification.SendNotification(new Notification
     //               {
     //                   NotificationId = Guid.NewGuid(),
     //                   DueDate = DateTime.Now.AddDays(5),
     //                   CreatedBy = legalPrinciplesVM.ModifiedBy,
     //                   CreatedDate = DateTime.Now,
     //                   IsDeleted = false,
     //                   ReceiverId = assignedTo,
     //                   ModuleId = (int)WorkflowModuleEnum.LPSPrinciple,
     //               },
     //                (int)NotificationEventEnum.SoftDeleteLegalPrinciple,
     //                "detail",
					//new FATWA_DOMAIN.Models.LegalPrinciple.LegalPrinciple().GetType().Name,
     //                legalPrinciplesVM.PrincipleId.ToString() + "/" + "LegalPrincipleDelete",
     //                legalPrinciplesVM.NotificationParameter);
                }               

                return Ok(result);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Delete Legal Principle Failed",
                    Body = ex.Message,
                    Category = "User unable to Delete Legal Principle",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Delete Legal Principle Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LPSPrinciple,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                throw new Exception(ex.Message);
            }


        }
		#endregion

		#region Get LLS Legal Principle Categories Detail VM

		[HttpGet("GetLLSLegaPrincipleCategories")]
		[MapToApiVersion("1.0")]
		public async Task<IActionResult> GetLLSLegaPrincipleCategories()
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

				var result = await _iLLSLegalPrinciple.GetLLSLegaPrincipleCategories();
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
        [HttpPost("GetLLSLegaPrincipleCategoriesAdvanceSearch")]
		[MapToApiVersion("1.0")]
		public async Task<IActionResult> GetLLSLegaPrincipleCategoriesAdvanceSearch(LLSLegalPrincipleCategoryAdvanceSearchVm advanceSearchVm )
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

				var result = await _iLLSLegalPrinciple.GetLLSLegaPrincipleCategoriesAdvanceSearch(advanceSearchVm);
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

        #region Update legal principle category

        [HttpPost("UpdateLegalPrincipleCategory")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Umer Zaman' Date='2024-04-20' Version="1.0" Branch="master"> Handle create  legal principle category</History>
        public async Task<IActionResult> UpdateLegalPrincipleCategory(LLSLegalPrincipleCategory item)
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
                var result = await _iLLSLegalPrinciple.UpdateLegalPrincipleCategory(item);
                if (result)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Update legal principle category",
                        Task = "Update legal principle category process",
                        Description = "User able to update legal principle category successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Update legal principle category executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });

                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Update legal principle category Failed",
                    Body = ex.Message,
                    Category = "User unable to Update legal principle category",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Update legal principle category Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region Delete legal principle category

        [HttpPost("DeleteLegalPrincipleCategory")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Umer Zaman' Date='2024-04-20' Version="1.0" Branch="master"> Handle create  legal principle category</History>
        public async Task<IActionResult> DeleteLegalPrincipleCategory(LLSLegalPrincipleCategory item)
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
                var result = await _iLLSLegalPrinciple.DeleteLegalPrincipleCategory(item);
                if (result)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Delete legal principle category",
                        Task = "Delete legal principle category process",
                        Description = "User able to Delete legal principle category successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Delete legal principle category executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });

                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Delete legal principle category Failed",
                    Body = ex.Message,
                    Category = "User unable to Delete legal principle category",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Delete legal principle category Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Get LLS Legal Principle Contents

        [HttpGet(nameof(GetLLSPrincipleContents))]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLLSPrincipleContents(int categoryId = 0)
        {
            try
            {
                var result = await _iLLSLegalPrinciple.GetLLSPrincipleContents(categoryId);
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
        
        #region Get LLS Legal Principle Contents

        [HttpGet(nameof(GetLLSLegalPrincipleContentCategories))]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLLSLegalPrincipleContentCategories(Guid? principleContentId)
        {
            try
            {
                var result = await _iLLSLegalPrinciple.GetLLSLegalPrincipleContentCategories(principleContentId);
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


        #region Link LegalPrinciple Contents

        [HttpPost(nameof(LinkLegalPrincipleContents))]
        [MapToApiVersion("1.0")]
        //<History Author = 'Umer Zaman' Date='2024-04-20' Version="1.0" Branch="master"> Handle create  legal principle category</History>
        public async Task<IActionResult> LinkLegalPrincipleContents(List<LLSLegalPrincipleContentSourceDocumentReference> linkContents)
        {
            try
            {
                var result = await _iLLSLegalPrinciple.LinkLegalPrincipleContents(linkContents);
                if (result)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Link legal principle contents",
                        Task = "Link legal principle contents process",
                        Description = "User able to link legal principle contents successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Link legal principle contents executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });

                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Link legal principle contents Failed",
                    Body = ex.Message,
                    Category = "User unable to link legal principle contents",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "link legal principle contents Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region  Check Copy Document Exist
        [HttpGet(nameof(CheckCopyDocumentExists))]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CheckCopyDocumentExists(int uploadDocumentId)
        {
            try
            {
                var result = await _iLLSLegalPrinciple.CheckCopyDocumentExists(uploadDocumentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion    
        
        #region  Get Legal Principle Content By Id
        [HttpGet(nameof(GetLLSLegalPrincipleContentById))]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLLSLegalPrincipleContentById(Guid principleContentId)
        {
            try
            {
                var result = await _iLLSLegalPrinciple.GetLLSLegalPrincipleContentById(principleContentId);
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

        #region Legal Principle

        [HttpPost("GetLegalPrinciples")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegalPrinciplesReviewList(LLSLegalPrincipleAdvanceSearchVM search)
        {
            try
            {
                return Ok(await _iLLSLegalPrinciple.GetLegalPrinciples(search));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get legal Principle References by principle Id
        [HttpGet("GetLegalPrincipleDetailById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Abuzar' Date='2024-04-20' Version="1.0" Branch="master"> </History>
        public async Task<IActionResult> GetLegalPrincipleDetailById(Guid principleId)
        {
            try
            {
                var result = await _iLLSLegalPrinciple.GetLegalPrincipleDetailById(principleId);
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


        #region Update Legal Principle Decision
        [HttpPost("UpdateLLSLegalPrincipleDecision")]
        [MapToApiVersion("1.0")]
        //<History Author = 'ijaz Ahmad' Date='2022-10-22' Version="1.0" Branch="master"> Update Legal Principle Decision</History>
        public async Task<IActionResult> UpdateLegalPrincipleDecision(LLSLegalPrincipleDecisionVM legalPrincipleDecisionVM)
        {
            try
            {
                await _iLLSLegalPrinciple.UpdateLegalPrincipleDecision(legalPrincipleDecisionVM);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Update Legal Principle Decision",
                    Task = "Update LMS Literature",
                    Description = "User able to Update Legal Principle Decision successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Updating Legal Principle Decision executed Successfully",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LPSPrinciple,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                if (legalPrincipleDecisionVM.FlowStatusId == (int)PrincipleFlowStatusEnum.Unpublished)
                {
                    var entity = "principle-content";
                    var notificationResult = await _INotification.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = legalPrincipleDecisionVM.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = await _IAccount.UserIdByUserEmail(legalPrincipleDecisionVM.ReceiverEmail),
                        ModuleId = (int)WorkflowModuleEnum.LDSDocument,
                    },
                       (int)NotificationEventEnum.UpdateLegalPrinciple,
                    entity,
                    "details",
                        legalPrincipleDecisionVM.PrincipleId.ToString(),
                        legalPrincipleDecisionVM.NotificationParameter);
                }
                return Ok();

            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Update Legal Principle Decision Failed",
                    Body = ex.Message,
                    Category = "User unable to Update The Legal Principle Decision",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Update Legal Principle Decision Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LPSPrinciple,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        #endregion

        #region Get Legal principle Details By Using PrincipleId 

        [HttpGet("GetLegalPrincipleDetailsByUsingPrincipleId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegalPrincipleDetailsByUsingPrincipleId(Guid principleId)
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

                var result = await _iLLSLegalPrinciple.GetLegalPrincipleDetailsByUsingPrincipleId(principleId);
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

        #region GetLLSLegalPrincipleContentDetailsByUsingPrincipleId

        [HttpGet("GetLLSLegalPrincipleContentDetailsByUsingPrincipleId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLLSLegalPrincipleContentDetailsByUsingPrincipleId(Guid principleId)
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

                var result = await _iLLSLegalPrinciple.GetLLSLegalPrincipleContentDetailsByUsingPrincipleId(principleId);
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

        #region Get Principle flow status details

        [HttpGet("GetPrincipleFlowStatusDetails")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetPrincipleFlowStatusDetails()
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

                var result = await _iLLSLegalPrinciple.GetPrincipleFlowStatusDetails();
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

        #region Mobile Application End Point APIs
        [HttpGet("GetLLSLegaPrincipleCategoriesForMobileApp")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLLSLegaPrincipleCategoriesForMobileApp()
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

                var result = await _iLLSLegalPrinciple.GetLLSLegaPrincipleCategories(true);
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
        [HttpGet("GetLLSLegalPrincipleContentCategoriesForMobileApp")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLLSLegalPrincipleContentCategoriesForMobileApp(Guid? principleContentId)
        {
            try
            {
                var result = await _iLLSLegalPrinciple.GetLLSLegalPrincipleContentCategories(principleContentId);
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
        [HttpGet("GetLLSLegalPrincipleContentByIdForMobileApp")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLLSLegalPrincipleContentByIdForMobileApp(Guid principleContentId)
        {
            try
            {
                var result = await _iLLSLegalPrinciple.GetLLSLegalPrincipleContentByIdForMobileApp(principleContentId);
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

        //#region Get Legal Principle Dms
        //[HttpPost("GetLegalPrincipleDms")]
        //[MapToApiVersion("1.0")]
        //public async Task<IActionResult> GetLegalPrincipleDms()
        //{
        //    try
        //    {
        //        var res = await _iLLSLegalPrinciple.GetLegalPrincipleDms();
        //        return Ok(res);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        //#endregion

    }
}
