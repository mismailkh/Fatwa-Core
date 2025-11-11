using FATWA_API.RabbitMQ;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V1.Notif
{
    //<History Author = 'Hassan Iftikhar' Date='2022-03-15' Version="1.0" Branch="own"> create api controller & add functionality</History>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationController : ControllerBase
    {
        private readonly INotification _INotification;
        private readonly RabbitMQClient _client;


        public NotificationController(INotification iNotification, RabbitMQClient client)
        {
            _INotification = iNotification;
            _client = client;
        }

        #region Get

        [HttpGet("GetNotifNotificationDetails")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetNotifNotificationDetails(NotificationListAdvanceSearchVM advanceSearchVM)
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

                return Ok(await _INotification.GetNotifNotificationDetails(advanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetNotificationById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetNotificationById(Guid NotificationId)
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

                var result = await _INotification.GetNotificationById(NotificationId);
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

        [HttpGet("GetNotificationDetailView")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetNotificationDetailView(Guid NotificationId, string user)
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

                return Ok(await _INotification.GetNotificationDetailView(NotificationId, user));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetBellNotifications")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetBellNotifications(string userId, int NotificationStatusId,int channelId)
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
                var result = await _INotification.GetBellNotifications(userId, NotificationStatusId, channelId);
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

        [HttpPost("MarkNotificationAsRead")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> MarkNotificationAsRead(List<Guid> notificationIds, int channelId = 0)
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
                await _INotification.MarkNotificationAsRead(notificationIds);
                if (channelId > 0)
                {
                    return Ok(new ApiCallResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        IsSuccessStatusCode = true,
                        ResultData = null,
                        Message = "success"
                    });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                if (channelId > 0)
                {
                    return BadRequest(new ApiCallResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccessStatusCode = false,
                        ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message },
                        Message = "Error_Ocurred"
                    });
                }
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("MarkNotificationAsRead")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> MarkNotificationAsRead(Guid notificationId)
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

                return Ok(await _INotification.MarkNotificationAsRead(notificationId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Delete  

        [HttpDelete("{item}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteNotification(NotificationVM item)
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
                await _INotification.DeleteNotification(item);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Save

        [HttpPost("SendNotification")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        //<History Author = 'Zain Ul Islam' Date='2022-10-17' Version="1.0" Branch="master"> Send Notification Endpoint</History> 
        public async Task<IActionResult> SendNotification(SendNotificationVM notificationVM)
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

                result = await _INotification.SendNotification
                (
                    notificationVM.Notification,
                    notificationVM.EventId,
                    notificationVM.Action,
                    notificationVM.EntityName,
                    notificationVM.EntityId,
                    notificationVM.NotificationParameter
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Get Event List
        [HttpGet("GetEventList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEventList()
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

                var result = await _INotification.GetEventList();
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

        #region Get Event Drop Down
        [HttpGet("GetEvent")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEvent()
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

                var result = await _INotification.GetEvent();
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
        #region Get Channel Drop Down
        [HttpGet("GetChannel")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetChannel()
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

                var result = await _INotification.GetChannel();
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
        #region Get Place Holders Drop Down
        [HttpGet("GetPlaceHoldersByEventId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetPlaceHoldersByEventId(int EventId)
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

                var result = await _INotification.GetPlaceHoldersByEventId(EventId);
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
        #region Get Place Holders Drop Down
        [HttpPost("CreateNotificationEventTemplate")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateNotificationEventTemplate(NotificationTemplate item)
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

                var result = await _INotification.CreateNotificationEventTemplate(item);
                if (result)
                {
                    _client.SendMessage(item, RabbitMQKeys.CreateNotificationTemplate);
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
        #region Get Notification Event Template By Id
        [HttpGet("GetNotificationEventTemplateById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetNotificationEventTemplateById(Guid Id)
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

                var result = await _INotification.GetNotificationEventTemplateById(Id);
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

        [HttpPost("UpdateNotificationEventTemplate")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateNotificationEventTemplate(NotificationTemplate template)
        {
            try
            {
                var res = await _INotification.UpdateNotificationEventTemplate(template);
                if (res)
                {
                    _client.SendMessage(template, RabbitMQKeys.UpdateNotificationEventTemplate);
                    return Ok(res);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        //[HttpPost("CheckNotification")]
        //[MapToApiVersion("1.0")]
        ////<History Author = 'Zain Ul Islam' Date='2022-03-23' Version="1.0" Branch="master"> create Literature Borrow Details</History>
        //public async Task<IActionResult> CheckNotification(CheckNotificationVM notification)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(new RequestFailedResponse
        //            {
        //                Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
        //            });
        //        }

        //        return Ok(await _INotification.CheckNotification(notification));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}




        #region Get Event List
        [HttpGet("GetTemplateListByEventId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTemplateListByEventId(int EventId)
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

                var result = await _INotification.GetTemplateListByEventId(EventId);
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
        #region Get Event List
        [HttpPost("DeleteEventTemplate")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteEventTemplate(NotificationTemplateListVM Template)
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

                var result = await _INotification.DeleteEventTemplate(Template);
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

        [HttpPost("UpdateEventStatus")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateEventStatus(NotificationEventListVM Event)
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

                var result = await _INotification.UpdateEventStatus(Event);
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

        #region Delete  

        [HttpDelete("DeleteAllNotification")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteAllNotification(List<Guid> notificationIds, String userId)
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
                await _INotification.DeleteAllNotification(notificationIds, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion 

        #region Get Notification Event By Id
        [HttpGet("GetNotificationEventByEventId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetNotificationEventByEventId(int EventId)
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

                var result = await _INotification.GetNotificationEventByEventId(EventId);
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

        #region Edit Notification Event
        [HttpPost("EditNotificationEvent")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> EditNotificationEvent(NotificationEvent Event)
        {
            try
            {
                await _INotification.EditNotificationEvent(Event);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Receiver Type
        [HttpGet("GetReceiverType")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetReceiverType()
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

                var result = await _INotification.GetReceiverType();
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
        #region Send FCM Push Notification
        //<History Author = 'Noman Khan' Date='2024-05-09' Version="1.0" Branch="master">Gets employee login/logout activities.</History>
        [HttpGet("SendPushNotificationFCM")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> SendPushNotificationFCM(string userId)
        {
            try
            {
                string response = string.Empty;
                if (!string.IsNullOrEmpty(userId))
                {
                    response = await _INotification.SendPushNotificationFCM(userId, null);
                }
                return Ok(new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccessStatusCode = true,
                    ResultData = response,
                    Message = "success"
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
        //<History Author = 'Noman Khan' Date='2024-05-12' Version="1.0" Branch="master">Gets employee login/logout activities.</History>
        [HttpGet("SendMultipleDevicePushNotificationFCM")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> SendMultipleDevicePushNotificationFCM(string userId)
        {
            try
            {
                string response = string.Empty;
                if (!string.IsNullOrEmpty(userId))
                {
                    response = await _INotification.SendMultipleDevicePushNotificationFCM(userId, null);
                }
                return Ok(new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    IsSuccessStatusCode = true,
                    ResultData = response,
                    Message = "FCM_Notification_Sent"
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

        [HttpPost("SendNotificationList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SendNotificationList(List<Notification> notifications)
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

                result = await _INotification.SendNotificationList(notifications);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}
