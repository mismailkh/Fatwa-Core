using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;
using NotificationCategoryEnum = FATWA_GENERAL.Helper.Enum.NotificationCategoryEnum;
using NotificationEventEnum = FATWA_GENERAL.Helper.Enum.NotificationEventEnum;
//using notificationtypeenum = fatwa_general.helper.enum.notificationtypeenum;

namespace FATWA_API.Controllers.V1
{
    //<!-- <History Author = 'Umer Zaman' Date='2022-07-06' Version="1.0" Branch="master">Create class for manage api controller</History>

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LmsLiteratureParentIndexController : ControllerBase
    {
        private readonly ILmsLiteratureParentIndex _iLmsLiteratureParentIndex;
        private readonly INotification _INotification;


        public LmsLiteratureParentIndexController(ILmsLiteratureParentIndex iLmsLiteratureParentIndex, INotification iNotification)
        {
            _iLmsLiteratureParentIndex = iLmsLiteratureParentIndex;
            _INotification = iNotification;

        }
        #region Get Lms Literature parent Index's Details / By Id

        [HttpGet("GetLmsLiteratureParentIndexs")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLmsLiteratureParentIndexs()
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
                return Ok(await _iLmsLiteratureParentIndex.GetLmsLiteratureParentIndexs());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetLmsLiteratureParentIndexSync")]
        [MapToApiVersion("1.0")]
        public IActionResult GetLmsLiteratureParentIndexSync()
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
                return Ok(_iLmsLiteratureParentIndex.GetLmsLiteratureParentIndexSync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetLiteratureParentIndexDetailById")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLiteratureParentIndexDetailById(int parentIndexId)
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

                LmsLiteratureParentIndex lit = await _iLmsLiteratureParentIndex.GetLiteratureParentIndexDetailById(parentIndexId);
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
        [HttpGet("CheckLiteratureParentIndexByUsingParentIndexNumber")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public IActionResult CheckLiteratureParentIndexByUsingParentIndexNumber(string parentIndexNumber, string name_En, string name_Ar)
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

                var result = _iLmsLiteratureParentIndex.CheckLiteratureParentIndexByUsingParentIndexNumber(parentIndexNumber, name_En, name_Ar);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("CheckLiteratureParentIndexByUsingParentNumber")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public IActionResult CheckLiteratureParentIndexByUsingParentNumber(string parentIndexNumber)
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

                var result = _iLmsLiteratureParentIndex.CheckLiteratureParentIndexByUsingParentNumber(parentIndexNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetLmsLiteratureParentIndexDetailByNumber")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLmsLiteratureParentIndexDetailByNumber(string parentIndexNumber)
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
                return Ok(await _iLmsLiteratureParentIndex.GetLmsLiteratureParentIndexDetailByNumber(parentIndexNumber));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Create

        [HttpPost("CreateLmsLiteratureParentIndex")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateLmsLiteratureParentIndex(LmsLiteratureParentIndex literatureParentIndex)
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
                await _iLmsLiteratureParentIndex.CreateLmsLiteratureParentIndex(literatureParentIndex);

                //await _INotification.SendNotification(new Notification
                //{
                //    NotificationId = Guid.NewGuid(),
                //    DueDate = DateTime.Now.AddDays(5),
                //    CreatedBy = literatureParentIndex.CreatedBy,
                //    CreatedDate = DateTime.Now,
                //    IsDeleted = false,
                //    SenderId = literatureParentIndex.CreatedBy,
                //    ReceiverId = "436e82d2-70d8-455c-a643-7909b8689667",//FATWA ADMIN
                //    ReceiverTypeId = (int)NotificationReceiverTypeEnum.User,
                //    ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                //    NotificationTypeId = (int)NotificationTypeEnum.Asynchronous,
                //    NotificationCommunicationMethodId = (int)NotificationChannelEnum.Browser,
                //    NotificationLinkId = Guid.NewGuid(),
                //    NotificationTemplateId = Guid.Parse("3FA85F64-5717-4562-B3FC-2C963F66AFA6"),
                //    NotificationEventId = (int)NotificationEventEnum.NewRequest,
                //    NotificationCategoryId = (int)NotificationCategoryEnum.Important
                //},
                //"Add_LiteratureIndex_Success",
                //"list",
                //literatureParentIndex.GetType().Name,
                //literatureParentIndex.ParentIndexId.ToString());

                return Ok(literatureParentIndex);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }
        }
        #endregion

        #region Update

        [HttpPost("UpdateLmsLiteratureParentIndex")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateLmsLiteratureParentIndex(LmsLiteratureParentIndex lmsLiteraturesParentIndex)
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
                await _iLmsLiteratureParentIndex.UpdateLmsLiteratureParentIndex(lmsLiteraturesParentIndex);
                
                return Ok(lmsLiteraturesParentIndex);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }
        }
        #endregion

        #region Delete

        [HttpPost("SoftDeleteLiteratureParentIndex")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SoftDeleteLiteratureParentIndex(LmsLiteratureParentIndex literatureParentIndex)
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
                await _iLmsLiteratureParentIndex.SoftDeleteLiteratureParentIndex(literatureParentIndex);
                var userId = await _iLmsLiteratureParentIndex.GetUserIdByName(literatureParentIndex.DeletedBy);
                var notificationResult = await _INotification.SendNotification(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = literatureParentIndex.DeletedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = userId,
                    ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                },
                (int)NotificationEventEnum.SaveLegalLegislation, 
                "lmsliteratureparentindex",
                "list",
                literatureParentIndex.ParentIndexId.ToString(),
                literatureParentIndex.NotificationParameter);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
