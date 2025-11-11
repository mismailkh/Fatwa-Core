using AutoMapper;
using FATWA_DOMAIN.Interfaces.AutomationMonitoring;
using FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V1.AutomationMonitoring
{
    //<!-- <History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master">API Controller For Automation Monitoring</History> -->
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + ",FatwaApiKey")]
    public class AutomationMonitoringController : ControllerBase
    {
        #region Variable Declaration
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IAutomationMonitoring _automationMonitoring;

        #endregion
        #region Constructor
        public AutomationMonitoringController(
            IConfiguration configuration,
            IMapper mapper,
            IAutomationMonitoring automationMonitoring
            )
        {

            _configuration = configuration;
            _mapper = mapper;
            _automationMonitoring = automationMonitoring;
        }

        #endregion

        [HttpPost("AddItemToQueue")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> Save Queue Item</History>
        public async Task<IActionResult> AddItemToQueue(AMSAddWorkQueueItemVM queueItem)
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
                bool result = await _automationMonitoring.CheckIfAlreadyPushedToQueue(queueItem.Data);
                if (result)
                {
                    return Ok(new { success = false, message = "This queue item has already been pushed to the queue" });
                }
                var Queue = await _automationMonitoring.GetQueueById(queueItem.QueueId);
                if (Queue == null)
                {
                    return NotFound(new { success = false, message = "Queue Not Found" });
                }
                var QueueItemStatus = await _automationMonitoring.GetQueueItemStatusByQueueId(queueItem.QueueId);

                if (QueueItemStatus == null)
                {
                    return NotFound(new { success = false, message = "Queue Item Status Not Found" });
                }

                AMSWorkQueueItem workQueueItem = new AMSWorkQueueItem();
                workQueueItem = await _automationMonitoring.AddItemToQueue(queueItem, QueueItemStatus.Id);
                var response = new
                {
                    status = "success",
                    message = "The item added successfully",
                    Item_id = workQueueItem.Id,
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }
        [HttpPost("AMSCaseDateExtraction")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> AMS CaseDate Extraction</History>
        public async Task<IActionResult> AMSCaseDateExtraction(AMSWorkQueueItem queueItem)
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
                await _automationMonitoring.AMSCaseDateExtraction(queueItem);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }
        [HttpPost("MarkCompleted")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> Mark completed </History>
        public async Task<IActionResult> MarkCompleted(int ItemId)
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
                var result = await _automationMonitoring.MarkCompleted(ItemId);
                if (result)
                {
                    var response = new
                    {
                        status = "success",
                        message = "Item marked completed successfully",
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound(new { success = false, message = "Queue Item Not Found Against ItemId" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        [HttpPost("MarkException")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> Mark MarkException </History>
        public async Task<IActionResult> MarkException(AMSMarkExceptionPayLoadVM exceptionPayLoad)
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
                var result = await _automationMonitoring.MarkException(exceptionPayLoad);
                if (result)
                {
                    var response = new
                    {
                        status = "success",
                        message = "Item marked as exception successfully",
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound(new { success = false, message = "Work Queue Item Not Found" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        [HttpPost("GetNextItem")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> Mark Next Item </History>
        public async Task<IActionResult> GetNextItem(AMSNextItemPayLoadVM nextItemPayLoad)
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
                AMSWorkQueueItem QueueItem = new AMSWorkQueueItem();
                QueueItem = await _automationMonitoring.GetNextItem(nextItemPayLoad);
                if (QueueItem != null)
                {
                    var response = new
                    {
                        status = "success",
                        itemId = QueueItem.Id,
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound(new { success = false, message = "Queue Not Found" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }
        [HttpPost("GeItemId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-08-06' Version="1.0" Branch="master"> Get Item Id </History>
        public async Task<IActionResult> GeItemId(string data, string queuename)
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
                AMSWorkQueueItem QueueItem = new AMSWorkQueueItem();
                QueueItem = await _automationMonitoring.GetItemId(data, queuename);
                if (QueueItem != null)
                {
                    var response = new
                    {
                        status = "success",
                        itemId = QueueItem.Id,
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound(new { success = false, message = "Queue Item Not Found" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        [HttpPost("GetItemData")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-08-06' Version="1.0" Branch="master"> Get Item Data </History>
        public async Task<IActionResult> GetItemData(int ItemId, string queuename)
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
                AMSWorkQueueItem QueueItem = new AMSWorkQueueItem();
                QueueItem = await _automationMonitoring.GetItemData(ItemId, queuename);
                if (QueueItem != null)
                {
                    var response = new
                    {
                        status = "success",
                        Item_Data = QueueItem.Data,
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound(new { success = false, message = "Queue Item  Not Found" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        [HttpPost("MarkQueueItemStatus")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-08-06' Version="1.0" Branch="master"> Mark Queue Item Status</History>
        public async Task<IActionResult> MarkQueueItemStatus(int ItemId, string queuename, int StatusCode, string ResourceName, int ResourceId)
        {
            try
            {
                var result = await _automationMonitoring.MarkQueueItemStatus(ItemId, queuename, StatusCode, ResourceName, ResourceId);
                if (result)
                {
                    var response = new
                    {
                        status = "success",
                        message = "Mark Item Status updated successfully",
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound(new { success = false, message = "Mark Item Status Not Updated" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }

        }

        [HttpPost("SetPriority")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master">Set Priority </History>
        public async Task<IActionResult> SetPriority(AMSSetPriorityPayLoadVM setPriorityPayLoad)
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
                var result = await _automationMonitoring.SetPriority(setPriorityPayLoad);
                if (result)
                {
                    var response = new
                    {
                        status = "success",
                        message = "Item Priority updated successfully",
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound(new { success = false, message = "Item Priority not updated" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        [HttpPost("GetAllPendingItems")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master">Get All Pending Items </History>
        public async Task<IActionResult> GetAllPendingItems()
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

                return Ok(await _automationMonitoring.GetAllPendingItems());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        [HttpPost("TagItem")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master">Tag Item </History>
        public async Task<IActionResult> TagItem(AMSTagItemPayLoadVM tagItemPayLoad)
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
                var result = await _automationMonitoring.TagItem(tagItemPayLoad);
                if (result)
                {
                    var response = new
                    {
                        status = "success",
                        message = "Item tagged successfully",
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound(new { success = false, message = "Queue Item Not Found Against ItemId" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        [HttpPost("UnTagItem")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master">Set UnTag Item </History>
        public async Task<IActionResult> UnTagItem(int ItemId)
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
                var reslut = await _automationMonitoring.UnTagItem(ItemId);
                if (reslut)
                {
                    var response = new
                    {
                        status = "success",
                        message = "Item tags cleared successfully",
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound(new { success = false, message = "Queue Item Not Found Against ItemId" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        [HttpPost("CreateSession")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master">Create Session </History>
        public async Task<IActionResult> CreateSession(CreateSessionPayLoadVM createSessionPayLoad)
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
                FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring.AMSSession session = new FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring.AMSSession();
                session = await _automationMonitoring.CreateSession(createSessionPayLoad);
                var response = new
                {
                    status = "success",
                    message = "Session Created successfully",
                    SessionId = session.SessionId,
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        [HttpPost("CreateSessionLog")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master">Create Session </History>
        public async Task<IActionResult> CreateSessionLog(AMSSessionLogsPayLoadVM aMSSessionLogsPayLoadVM)
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
                FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring.AMSSessionLog session = new FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring.AMSSessionLog();
                session = await _automationMonitoring.CreateSessionLog(aMSSessionLogsPayLoadVM);
                if (session != null)
                {
                    var response = new
                    {
                        status = "success",
                        message = "Session Logs Created successfully",
                        sessionlogId = session.Id,
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound(new { success = false, message = "Session Not Found Against SessionId" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }
        [HttpPost("MarkSessionRunning")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master">Mark Session Running </History>
        public async Task<IActionResult> MarkSessionRunning(int SessionId)
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
                var result = await _automationMonitoring.MarkSessionRunning(SessionId);
                if (result)
                {
                    var response = new
                    {
                        status = "success",
                        message = "Session Marked as running successfully",
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound(new { success = false, message = "Session Not Found Against SessionId" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        [HttpPost("MarkSessionTerminated")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master">Mark Session Terminated </History>
        public async Task<IActionResult> MarkSessionTerminated(int SessionId)
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
                var result = await _automationMonitoring.MarkSessionTerminated(SessionId);
                if (result)
                {
                    var response = new
                    {
                        status = "success",
                        message = "Session Marked as Terminated successfully",
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound(new { success = false, message = "Session Not Found Against SeesionId" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }
        [HttpPost("MarkSessionStopped")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master">Mark Session Stopped </History>
        public async Task<IActionResult> MarkSessionStopped(int SessionId)
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
                var result = await _automationMonitoring.MarkSessionStopped(SessionId);
                if (result)
                {
                    var response = new
                    {
                        status = "success",
                        message = "Session Marked as Stoped successfully",
                    };
                    return Ok(response);
                }

                else
                {
                    return NotFound(new { success = false, message = "Session Not Found Against SeesionId" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        [HttpPost("MarkSessionCompleted")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master">Mark Session Completed </History>
        public async Task<IActionResult> MarkSessionCompleted(int SessionId)
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
                var result = await _automationMonitoring.MarkSessionCompleted(SessionId);
                if (result)
                {
                    var response = new
                    {
                        status = "success",
                        message = "Session Marked as Completed successfully",
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound(new { success = false, message = "Session Not Found Against SessionId" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        [HttpPost("AddResource")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master">Set UnTag Item </History>
        public async Task<IActionResult> AddResource(AMSAddResourcePayLoadVM addResourcePayLoad)
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
                var newResource = await _automationMonitoring.AddResource(addResourcePayLoad);
                return Ok(new
                {
                    status = "success",
                    responseContent = "Resource added successfully",
                    ResourceId = newResource.Id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }
        [HttpPost("MarkResourceIdle")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master">Set UnTag Item </History>
        public async Task<IActionResult> MarkResourceIdle(int ResourceId)
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
                var result = await _automationMonitoring.MarkResourceIdle(ResourceId);
                if (result)
                {
                    var response = new
                    {
                        status = "success",
                        message = "Resource marked as Idle successfully",
                    };
                    return Ok(response);

                }
                else
                {
                    return NotFound(new { success = false, message = "Resource Not Found Against ResourceId" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }
        [HttpPost("MarkResourceWorking")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master">Set UnTag Item </History>
        public async Task<IActionResult> MarkResourceWorking(int ResourceId)
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
                var result = await _automationMonitoring.MarkResourceWorking(ResourceId);
                if (result)
                {
                    var response = new
                    {
                        status = "success",
                        message = "Resource marked as Working successfully",
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound(new { success = false, message = "Resource Not Found Against ResourceId" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }
        [HttpPost("MarkResourceLoggedOut")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master">Set UnTag Item </History>
        public async Task<IActionResult> MarkResourceLoggedOut(int ResourceId)
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
                var result = await _automationMonitoring.MarkResourceLoggedOut(ResourceId);
                if (result)
                {
                    var response = new
                    {
                        status = "success",
                        message = "Resource marked as Logged Out successfully",
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound(new { success = false, message = "Resource Not Found Against ResourceId" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        [HttpPost("SaveProcess")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> Save Process</History>
        public async Task<IActionResult> SaveProcess(AMSProcesses mProcess)
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
                AMSProcesses processes = new AMSProcesses();
                processes = await _automationMonitoring.SaveProcess(mProcess);
                var response = new
                {
                    status = "success",
                    message = "process saved successfully",
                    process_id = processes.Id,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }



        [HttpPost("GetProcessesList")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> Get Automation Monitoring Interface Processes</History>
        public async Task<IActionResult> GetProcessesList(AdvanceSearchProcessVM advanceSearch)
        {
            try
            {
                return Ok(await _automationMonitoring.GetProcessesList(advanceSearch));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }

        }

        [HttpPost("GetProcessesById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> Get Automation Monitoring Interface Processes</History>
        public async Task<IActionResult> GetProcessesById(int Id)
        {
            try
            {
                return Ok(await _automationMonitoring.GetProcessesById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }

        }
        [HttpPost("SaveQueue")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> Save Work Queue</History>
        public async Task<IActionResult> SaveQueue(AMSWorkQueue queue)
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
                AMSWorkQueue workQueue = new AMSWorkQueue();
                workQueue = await _automationMonitoring.SaveQueue(queue);
                var response = new
                {
                    status = "success",
                    message = "workQueue saved successfully",
                    workQueue_id = workQueue.Id,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        [HttpPost("CreateQueueLog")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> Create Queue Log</History>
        public async Task<IActionResult> CreateQueueLog(AMSQueueLogPayLoadVM queueLog)
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
                AMSWorkQueueLog workqueuelog = new AMSWorkQueueLog();
                workqueuelog = await _automationMonitoring.CreateQueueLog(queueLog);
                if (workqueuelog != null)
                {
                    var response = new
                    {
                        status = "success",
                        message = "work Queue Log saved successfully",
                        workQueueLog_id = workqueuelog.Id,
                    };

                    return Ok(response);
                }
                else
                {
                    return NotFound(new { success = false, message = "Item Not Found " });
                }


            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        [HttpPost("GetQueueList")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> Get Automation Monitoring Interface Queue List</History>
        public async Task<IActionResult> GetQueueList(int ProcessId, AdvanceSearchQueueVM advanceSearch)
        {
            try
            {
                return Ok(await _automationMonitoring.GetQueueList(ProcessId, advanceSearch));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }

        }

        [HttpPost("GetQueueItemsListByQueueId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> Get Automation Monitoring Interface Queue Items List</History>
        public async Task<IActionResult> GetQueueItemsListByQueueId(AdvanceSearchQueueVM advanceSearch, int QueueId)
        {
            try
            {
                return Ok(await _automationMonitoring.GetQueueItemsListByQueueId(advanceSearch, QueueId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }

        }
        [HttpPost("GetCaseDataExtraction")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> Get Automation Monitoring Interface Case Data Extraction</History>
        public async Task<IActionResult> GetCaseDataExtraction(AdvanceSearchQueueVM advanceSearch)
        {
            try
            {
                return Ok(await _automationMonitoring.GetCaseDataExtraction(advanceSearch));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }

        }
        [HttpPost("UpdateProcess")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> Get Automation Monitoring Interface Queue Items</History>
        public async Task<IActionResult> UpdateProcess(AMSProcesses aMProcess)
        {
            try
            {
                await _automationMonitoring.UpdateProcess(aMProcess);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }

        }

        [HttpPost("UpdateQueueItem")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> Get Automation Monitoring Interface Queue Items</History>
        public async Task<IActionResult> UpdateQueueItem(AMSWorkQueueItem aMProcess)
        {
            try
            {
                await _automationMonitoring.UpdateQueueItem(aMProcess);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }

        }

        [HttpGet("GetQueueItemStatus")]
        [MapToApiVersion("1.0")]
        //<History Author = 'ijaz Ahmad' Date='2022-09-30' Version="1.0" Branch="master"> Get Queue Item Status</History>
        public async Task<IActionResult> GetQueueItemStatus()
        {
            try
            {
                return Ok(await _automationMonitoring.GetQueueItemStatus());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }

        [HttpGet("GetSessionStatus")]
        [MapToApiVersion("1.0")]
        //<History Author = 'ijaz Ahmad' Date='2022-09-30' Version="1.0" Branch="master"> Get Queue Item Status</History>
        public async Task<IActionResult> GetSessionStatus()
        {
            try
            {
                return Ok(await _automationMonitoring.GetSessionStatus());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        [HttpPost("GetSessionList")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> Get Automation Monitoring Interface Queue Wrok</History>
        public async Task<IActionResult> GetSessionList(AdvanceSearchSessionVM advanceSearch, int ProcessId)
        {
            try
            {
                return Ok(await _automationMonitoring.GetSessions(advanceSearch, ProcessId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }

        }

        [HttpPost("GetSessionLogs")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> Get Automation Monitoring Interface Queue Wrok</History>
        public async Task<IActionResult> GetSessionLogs(int SessionId)
        {
            try
            {
                return Ok(await _automationMonitoring.GetSessionLogs(SessionId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }

        }

        [HttpPost("GetItemLogs")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> Get Automation Monitoring Interface Queue Wrok</History>
        public async Task<IActionResult> GetItemLogs(int ItemId)
        {
            try
            {
                return Ok(await _automationMonitoring.GetItemLogs(ItemId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }

        }
        [HttpPost("GetExceptionDetails")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> Get Automation Monitoring Interface Queue Wrok</History>
        public async Task<IActionResult> GetExceptionDetails(int ItemId)
        {
            try
            {
                return Ok(await _automationMonitoring.GetExceptionDetails(ItemId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }

        }

        [HttpPost("CheckIfAlreadyPushedToQueue")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master">Funtion Check Already Pushed To Queue</History>
        public async Task<IActionResult> CheckIfAlreadyPushedToQueue(string Data)
        {
            try
            {
                bool result = await _automationMonitoring.CheckIfAlreadyPushedToQueue(Data);
                if (result != false)
                {
                    return Ok(result);
                }
                return NotFound();

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }

        }

        [HttpPost("UpdateSession")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> Get Automation Monitoring Interface Queue Items</History>
        public async Task<IActionResult> UpdateSession(AMSSession aMProcess)
        {
            try
            {
                await _automationMonitoring.UpdateSession(aMProcess);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }

        }

        [HttpPost("UpdateQueueItemEndDateTime")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master">Update Queue End Date Time </History>
        public async Task<IActionResult> UpdateQueueItemEndDateTime(int ItemId)
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
                var reslut = await _automationMonitoring.UpdateQueueItemEndDateTime(ItemId);
                if (reslut)
                {
                    var response = new
                    {
                        status = "success",
                        message = "Item End Date time Updated successfully",
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound(new { success = false, message = "Queue Item Not Found Against ItemId" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        [HttpPost("GetResourcesByProcessId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> Get Resources By processesId</History>
        public async Task<IActionResult> GetResourcesByProcessId(int ProcessId)
        {
            try
            {
                return Ok(await _automationMonitoring.GetResourcesByProcessId(ProcessId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }

        }

        [HttpPost("GetQueueDetialsByProcessId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> Get Resources By processesId</History>
        public async Task<IActionResult> GetQueueDetialsByProcessId(int ProcessId)
        {
            try
            {
                return Ok(await _automationMonitoring.GetQueueDetialsByProcessId(ProcessId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }

        }
        [HttpPost("GetQueueListByQueueId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2024-06-27' Version="1.0" Branch="master"> Get Automation Monitoring Interface Queue List</History>
        public async Task<IActionResult> GetQueueListByQueueId(int QueueId)
        {
            try
            {
                return Ok(await _automationMonitoring.GetQueueListByQueueId(QueueId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }

        }

        [HttpPost("GetTagItem")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTagItem(int ItemId)
        {
            try
            {
                var tagItem = await _automationMonitoring.GetTagItem(ItemId);
                if (tagItem != null)
                {
                    return Ok(new { status = "success", Tag_Item = tagItem });
                }
                else
                {
                    return NotFound(new { success = false, message = "Queue Item Not Found Against ItemId" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }
    }
}
