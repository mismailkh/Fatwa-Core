using FATWA_API.RabbitMQ;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.LegalPrinciple;
using FATWA_DOMAIN.Models.Lms;
using FATWA_DOMAIN.Models.ServiceRequestModels;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.ContactManagmentVMs;
using FATWA_DOMAIN.Models.ViewModel.ServiceRequestVMs;
using G2GTarasolServiceReference;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using System.Collections.Generic;
using System;
using System.Data;
using System.Net;
using System.Threading.Tasks;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;
using Court = FATWA_DOMAIN.Models.CaseManagment.Court;
using Department = FATWA_DOMAIN.Models.AdminModels.UserManagement.Department;


namespace FATWA_API.Controllers.V1.Common
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + ",FatwaApiKey")]
    public class LookupsController : ControllerBase
    {
        private readonly ILookups _ILookups;
        private readonly IAuditLog _auditLogs;
        private readonly RabbitMQClient _client;



        public LookupsController(ILookups iLookups, IAuditLog audit, RabbitMQClient client)
        {
            _ILookups = iLookups;
            _auditLogs = audit;
            _client = client;
        }

        [HttpGet("GetGovernmentEntities")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get goverment entities</History>
        public async Task<IActionResult> GetGovernmentEntities(string Culture)
        {
            try
            {
                return Ok(await _ILookups.GetGovernmentEntities(Culture));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetGovernmentEntity")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get goverment entities</History>
        public async Task<IActionResult> GetGovernmentEntity()
        {
            try
            {
                return Ok(await _ILookups.GetGovernmentEntity());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetGeRepresentatives")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-02-27' Version="1.0" Branch="master"> Get GE Representatives</History>
        public async Task<IActionResult> GetGeRepresentatives(int? govtEntitiyId)
        {
            try
            {
                return Ok(await _ILookups.GetGeRepresentatives(govtEntitiyId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetOperatingSectorTypes")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get goverment entities</History>
        public async Task<IActionResult> GetOperatingSectorTypes()
        {

            try
            {
                return Ok(await _ILookups.GetOperatingSectorTypes());

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }


        }

        [HttpGet("GetRequestTypes")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get goverment entities</History>
        public async Task<IActionResult> GetRequestTypes()
        {

            try
            {
                return Ok(await _ILookups.GetRequestTypes());

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }


        }

        [HttpGet("GetAllRequestSubtypes")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get all Request subtypes</History>
        public async Task<IActionResult> GetAllRequestSubtypes()
        {
            try
            {
                return Ok(await _ILookups.GetAllRequestSubtypes());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetRequestSubtypesByRequestId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get request subtypes</History>
        public async Task<IActionResult> GetRequestSubtypesByRequestId(int requestTypeId)
        {
            try
            {
                return Ok(await _ILookups.GetRequestSubtypesByRequestId(requestTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetCaseRequestStatuses")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get case request statuses</History>
        public async Task<IActionResult> GetCaseRequestStatuses()
        {
            try
            {
                return Ok(await _ILookups.GetCaseRequestStatuses());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetCaseFileStatuses")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get case request statuses</History>
        public async Task<IActionResult> GetCaseFileStatuses()
        {
            try
            {
                return Ok(await _ILookups.GetCaseFileStatuses());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        #region Get Case File Status list 
        [HttpGet("GetCaseFileStatusList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCaseFileStatusList()
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
                return Ok(await _ILookups.GetCaseFileStatusList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region  Get Case File Status By Id 
        [HttpGet("GetCaseFileStatusById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCaseFileStatusById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetCaseFileStatusById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region  Update Case File Status

        [HttpPost("UpdateCaseFileStatus")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateCaseFileStatus(CaseFileStatus caseFileStatus)
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
                var result = await _ILookups.UpdateCaseFileStatus(caseFileStatus);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.UpdateCaseFileStatusEnumKey);
                }

                return Ok(caseFileStatus);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        [HttpGet("GetFileNumbers")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get case request statuses</History>
        public async Task<IActionResult> GetFileNumbers()
        {
            try
            {
                return Ok(await _ILookups.GetFileNumbers());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }

        [HttpGet("GetReferenceNumber")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nadia Gull' Date='2023-01-27' Version="1.0" Branch="master"> Get RequestNumber/FileNumbers</History>
        public async Task<IActionResult> GetReferenceNumber(Guid ReferenceId, int SubModulId)
        {
            try
            {
                return Ok(await _ILookups.GetReferenceNumber(ReferenceId, SubModulId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        [HttpGet("GetConsultationReferenceNumber")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nadia Gull' Date='2023-01-27' Version="1.0" Branch="master"> Get RequestNumber/FileNumbers</History>
        public async Task<IActionResult> GetConsultationReferenceNumber(Guid ReferenceId)
        {
            try
            {
                return Ok(await _ILookups.GetConsultationReferenceNumber(ReferenceId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        [HttpGet("GetConsultationFileNumber")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-1-16' Version="1.0" Branch="master"> Get consultation file numbers</History>
        public async Task<IActionResult> GetConsultationFileNumber()
        {
            try
            {
                return Ok(await _ILookups.GetConsultationFileNumber());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }

        [HttpGet("GetRequestNumber")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Zain Ul Islam' Date='2022-12-15' Version="1.0" Branch="master"> Get Request Numbers</History>
        public async Task<IActionResult> GetRequestNumber()
        {
            try
            {
                return Ok(await _ILookups.GetRequestNumber());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }

        [HttpGet("GetMeetingTypes")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetMeetingTypes()
        {
            try
            {
                return Ok(await _ILookups.GetMeetingTypes());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }

        [HttpGet("GetCasePriorities")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get case priorities</History>
        public async Task<IActionResult> GetCasePriorities()
        {
            try
            {
                return Ok(await _ILookups.GetCasePriorities());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetDepartments")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetDepartments()
        {
            try
            {
                return Ok(await _ILookups.GetDepartments());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetCaseTemplates")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get case priorities</History>
        public async Task<IActionResult> GetCaseTemplates(int attachmentTypeId)
        {
            try
            {
                return Ok(await _ILookups.GetCaseTemplates(attachmentTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetCaseHearingStatuses")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get Hearing Statuses</History>
        public async Task<IActionResult> GetCaseHearingStatuses()
        {
            try
            {
                return Ok(await _ILookups.GetCaseHearingStatuses());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetCaseJudgementTypes")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get Judgement Types</History>
        public async Task<IActionResult> GetCaseJudgementTypes()
        {
            try
            {
                return Ok(await _ILookups.GetCaseJudgementTypes());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetCaseJudgementStatuses")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2023-11-28' Version="1.0" Branch="master"> Get Judgement Statuses</History>
        public async Task<IActionResult> GetCaseJudgementStatuses()
        {
            try
            {
                return Ok(await _ILookups.GetCaseJudgementStatuses());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetCaseJudgementCategories")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2023-11-28' Version="1.0" Branch="master"> Get Judgement Cateogries</History>
        public async Task<IActionResult> GetCaseJudgementCategories()
        {
            try
            {
                return Ok(await _ILookups.GetCaseJudgementCategories());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetExecutionFileLevels")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2023-11-28' Version="1.0" Branch="master"> Get Execution File Levels</History>
        public async Task<IActionResult> GetExecutionFileLevels()
        {
            try
            {
                return Ok(await _ILookups.GetExecutionFileLevels());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }



        #region Get court type
        [HttpGet("GetCourtType")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2022-11-11' Version="1.0" Branch="master">Get Chamber</History>
        public async Task<IActionResult> GetCourtType()
        {
            try
            {
                return Ok(await _ILookups.GetCourtTypes());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region get Court
        [HttpGet("GetCourt")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2022-11-11' Version="1.0" Branch="master">Get court</History>
        public async Task<IActionResult> GetCourt()
        {
            try
            {
                return Ok(await _ILookups.GetCourts());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Chamber
        [HttpGet("GetChamber")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2022-11-11' Version="1.0" Branch="master">Get Chamber</History>
        public async Task<IActionResult> GetChamber()
        {
            try
            {
                return Ok(await _ILookups.GetChambers());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Shift
        [HttpGet("GetShift")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Ali' Date='2024-06-25' Version="1.0" Branch="master">Get Chamber</History>
        public async Task<IActionResult> GetShift()
        {
            try
            {
                return Ok(await _ILookups.GetShift());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Chamber Numbers

        [HttpGet("GetChamberNumbersByChamberId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetChamberNumbersbyChamberId(int chamberId)
        {
            try
            {
                return Ok(await _ILookups.GetChamberNumbersbyChamberId(chamberId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Submit Lawyer To Court Assignment

        [HttpPost("SubmitLawyerToCourt")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2022-11-11' Version="1.0" Branch="master"> Submit Lawyer To Court Assignment</History>
        public async Task<IActionResult> SubmitLawyerToCourt(CmsAssignLawyerToCourt cmsAssignLawyerToCourt)
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
                await _ILookups.SaveAssignLawyerToCourt(cmsAssignLawyerToCourt);


                return Ok(cmsAssignLawyerToCourt);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        [HttpGet("GetResponseTypes")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetResponseTypes()
        {
            try
            {
                return Ok(await _ILookups.GetResponseTypes());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #region GetTask Type

        [HttpGet("GetTaskType")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskType()
        {
            try
            {
                return Ok(await _ILookups.GetTaskType());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region GetTask Sub Type

        [HttpGet("GetTaskSubType")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskSubType()
        {
            try
            {
                return Ok(await _ILookups.GetTaskSubType());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get File Number

        [HttpGet("GetFileNumber")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetFileNumber()
        {
            try
            {
                return Ok(await _ILookups.GetFileNumber());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        [HttpGet("GetFrequency")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetFrequency()
        {
            try
            {
                return Ok(await _ILookups.GetFrequency());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }


        #region Get Roles

        //<History Author = 'Zain Ul Islam' Date='2022-12-13' Version="1.0" Branch="master">Get Roles</History>

        [HttpGet("GetRoles")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                return Ok(await _ILookups.GetRoles());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region

        [HttpGet("GetCourtVisitTypes")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-09-30' Version="1.0" Branch="master"> Get Court Visit Types</History>
        public async Task<IActionResult> GetCourtVisitTypes()
        {
            try
            {
                return Ok(await _ILookups.GetCourtVisitTypes());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        [HttpGet("GetUsersBySector")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUsersBySector(int? sectorTypeId)
        {
            try
            {
                return Ok(await _ILookups.GetUsersBySector(sectorTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUsersBySectorForCourtAssignment")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUsersBySectorForCourtAssignment(int? sectorTypeId)
        {
            try
            {
                return Ok(await _ILookups.GetUsersBySectorForCourtAssignment(sectorTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAssignLawyerToCourt")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAssignLawyerToCourt(AdvanceSearchVMAssignLawyerToCourt advanceSearchVM)
        {
            try
            {
                return Ok(await _ILookups.GetAssignLawyerToCourt(advanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetUserIdByUserEmail")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUserIdByUserEmail(string email)
        {
            try
            {
                return Ok(await _ILookups.GetUserIdByUserEmail(email));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }


        //<History Author = 'Zain Ul Islam' Date='2022-01-16' Version="1.0" Branch="master">Get Lawyers List By Sector</History>
        //<History Author = 'Hassan Abbas' Date='2022-03-07' Version="1.0" Branch="master">Modified to return only those who have role lawyer</History>    
        [HttpGet("GetLawyersBySector")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLawyersBySector(int? sectorTypeId)
        {
            try
            {
                return Ok(await _ILookups.GetLawyersBySector(sectorTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetLawyerSupervisorAssignmentListBySector")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLawyerSupervisorAssignmentListBySector(int? sectorTypeId)
        {
            try
            {
                return Ok(await _ILookups.GetLawyerSupervisorAssignmentListBySector(sectorTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-12-04' Version="1.0" Branch="master">List of Lawyers based on Sector And Chamber</History>    
        [HttpGet("GetLawyersBySectorAndChamber")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLawyersBySectorAndChamber(int? sectorTypeId, string? UserId, int chamberNumberId = 0)
        {
            try
            {
                return Ok(await _ILookups.GetLawyersBySectorAndChamber(sectorTypeId, UserId, chamberNumberId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-03-07' Version="1.0" Branch="master">Get Supervisors List By Sector</History>    
        [HttpGet("GetSupervisorsBySector")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSupervisorsBySector(int? sectorTypeId)
        {
            try
            {
                return Ok(await _ILookups.GetSupervisorsBySector(sectorTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetAssignLawyertoCourtById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAssignLawyertoCourtById(Guid Id)
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

                return Ok(await _ILookups.GetAssignLawyertoCourtById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }
        [HttpPost("EditAssignLawyertoCourt")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> EditAssignLawyertoCourt(CmsAssignLawyerToCourt cmsAssignLawyerToCourt)
        {
            try
            {
                await _ILookups.EditAssignLawyertoCourt(cmsAssignLawyerToCourt);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        #region SoftDelete Assign Lawyer To Court
        [HttpPost("DeleteAssignLawyerToCourt")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteAssignLawyerToCourt(AssignLawyerToCourtVM assignLawyerToCourtVMs)
        {

            try
            {
                await _ILookups.DeleteAssignLawyerToCourt(assignLawyerToCourtVMs);
                return Ok();
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

        #region Assign Supervisor To Lawyers

        [HttpPost("AssignSupervisorToLawyers")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AssignSupervisorAndManagerToLawyers(CmsLawyerSupervisorVM item)
        {
            try
            {
                await _ILookups.AssignSupervisorAndManagerToLawyers(item);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Get Supervisor By Lawyer


        [HttpGet("GetSupervisorByLawyerId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSupervisorByLawyerId(string lawyerId)
        {
            try
            {
                return Ok(await _ILookups.GetSupervisorByLawyerId(lawyerId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Consultatio Part Types
        [HttpGet("GetConsultationPartyTypes")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetConsultationPartyTypes()
        {
            try
            {
                return Ok(await _ILookups.GetConsultationPartyTypes());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Consultation Request Number
        [HttpGet("GetConsultationRequestNumber")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-03-23' Version="1.0" Branch="master"> Get Consulatation Request Numbers</History>
        public async Task<IActionResult> GetConsultationRequestNumber()
        {
            try
            {
                return Ok(await _ILookups.GetConsultationRequestNumber());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        #endregion

        #region Contact Managment
        [HttpGet("GetContactJobRole")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Iftikhar' Date='2022-09-30' Version="1.0" Branch="master"> Get contact role details</History>
        public async Task<IActionResult> GetContactJobRole()
        {
            try
            {
                return Ok(await _ILookups.GetContactJobRole());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetContactType")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Umer Zaman' Date='2022-04-02' Version="1.0" Branch="master"> Get contact types details</History>
        public async Task<IActionResult> GetContactType()
        {
            try
            {
                return Ok(await _ILookups.GetContactType());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        [HttpGet("GetMeetingStatus")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetMeetingStatus()
        {
            try
            {
                return Ok(await _ILookups.GetMeetingStatus());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        #region  Get Reference Number By SubmoduleId
        [HttpGet("GetReferenceNumberBySubmoduleId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Umer Zaman' Date='2022-04-02' Version="1.0" Branch="master"> Get contact types details</History>
        public async Task<IActionResult> GetReferenceNumberBySubmoduleId(int SubmoduleId, int SectorId)
        {
            try
            {
                return Ok(await _ILookups.GetReferenceNumberBySubmoduleId(SubmoduleId, SectorId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Modules
        [HttpGet("GetModules")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetModules()
        {
            try
            {
                return Ok(await _ILookups.GetModules());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        #endregion

        #region Get Events
        [HttpGet("GetEvents")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEvents()
        {
            try
            {
                return Ok(await _ILookups.GetNotificationEvents());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        #endregion

        #region Get Submodule
        [HttpGet("GetSubmodule")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSubmodule()
        {
            try
            {
                return Ok(await _ILookups.GetSubmodule());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        #endregion

        #region Get Floors

        [HttpGet("GetFloors")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get goverment entities</History>
        public async Task<IActionResult> GetFloors()
        {

            try
            {
                return Ok(await _ILookups.GetFloors());

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }


        }
        #endregion

        #region Get Store Keepers

        [HttpGet("GetStoreKeepers")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetStoreKeepers(string userId, int userTypeId)
        {
            try
            {
                return Ok(await _ILookups.GetStoreKeepers(userId, userTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Get List Inv Users
        [HttpGet("GetStoreInchargesbySectortype")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetStoreInchargesBySectortype(int sectortypeId)
        {
            try
            {
                return Ok(await _ILookups.GetListofStoreInchargesbySectortypeId(sectortypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Get user by role Id
        [HttpGet("GetUserbyRoleId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUserbyRoleId(Guid roleId)
        {
            try
            {
                return Ok(await _ILookups.GetUserbyRoleId(roleId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Get USer By Department
        [HttpGet("GetUsersByDepartment")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUsersByDepartment(int DepartmentId)
        {
            try
            {
                return Ok(await _ILookups.GetUsersByDepartment(DepartmentId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region  Get Case  Status By Id 
        [HttpGet("GetCaseStatusById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCaseStatusById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetCaseStatusById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Module Id
        [HttpGet("GetModule")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetModule()
        {
            try
            {
                return Ok(await _ILookups.GetModule());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Subtype Id
        [HttpGet("GetSubTypeId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSubTypeId()
        {
            try
            {
                return Ok(await _ILookups.GetSubTypeId());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Sector Type Enums
        #region Get Sector Type  list 
        [HttpGet("GetSectorTypeList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSectorTypeList()
        {
            try
            {
                return Ok(await _ILookups.GetSectorTypeList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region  Get Sector Type by Id 
        [HttpGet("GetSectorTypeById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSectorTypeById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetSectorTypeById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetSectorBuildings")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ammaar Naveed' Date='05/03/2024' Version="1.0" Branch="master">Get buildings lookup values</History>
        public async Task<IActionResult> GetSectorBuilding()
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
                return Ok(await _ILookups.GetSectorBuilding());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetSectorFloors")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ammaar Naveed' Date='05/03/2024' Version="1.0" Branch="master">Get floors lookup values</History>
        public async Task<IActionResult> GetSectorFloor()
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
                return Ok(await _ILookups.GetSectorFloor());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #region Update Sector Type

        [HttpPost("UpdateSectorType")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateSectorType(OperatingSectorType SectorType)
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
                await _ILookups.UpdateSectorType(SectorType);


                return Ok(SectorType);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #endregion

        #region Get Government Entities User list 
        [HttpGet("GetAllUserGroupsList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllUserGroupsList()
        {
            try
            {
                return Ok(await _ILookups.GetAllUserGroupsList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetCmsPatternById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCmsPatternById(Guid Id)
        {
            try
            {
                return Ok(await _ILookups.GetCmsPatternById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetCmsComsNumberPatternGroupById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCmsComsNumberPatternGroupById(Guid Id)
        {
            try
            {
                return Ok(await _ILookups.GetCmsComsNumberPatternGroupById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Get legal pricipal types
        [HttpGet("GetLegalPrincipleTypes")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegalPrincipleTypes()
        {
            try
            {
                return Ok(await _ILookups.GetLegalPrincipleTypes());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }

        }


        #endregion

        #region Save Legal legislation Type

        [HttpPost("SavelegislationType")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-11-11' Version="1.0" Branch="master"> Submit Legal Legislation type</History>
        public async Task<IActionResult> SavelegislationType(LegalLegislationType legislationType)
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
                await _ILookups.SavelegislationType(legislationType);


                return Ok(legislationType);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Update Legal legislation Type

        [HttpPost("UpdatelegislationType")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-11-11' Version="1.0" Branch="master"> Update Legal Legislation type</History>
        public async Task<IActionResult> UpdatelegislationType(LegalLegislationType legislationType)
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
                await _ILookups.UpdatelegislationType(legislationType);


                return Ok(legislationType);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Update Legal legislation Type

        [HttpPost("ActivelegislationType")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-11-11' Version="1.0" Branch="master"> Update Legal Legislation type</History>
        public async Task<IActionResult> ActivelegislationType(LegallegislationtypesVM legislationType)
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
                await _ILookups.ActivelegislationType(legislationType);


                return Ok(legislationType);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Delete Legal legislation Type

        [HttpPost("DeletelegislationType")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-11-11' Version="1.0" Branch="master"> Update Legal Legislation type</History>
        public async Task<IActionResult> DeletelegislationType(LegallegislationtypesVM legislationType)
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
                await _ILookups.DeletelegislationType(legislationType);


                return Ok(legislationType);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="legislationType"></param>
        /// <returns></returns


        #region Legislation Types
        [HttpGet("GetLegalLegislationtype")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2023-08-03' Version="1.0" Branch="master"> Get Legal legislation types </History>
        public async Task<IActionResult> GetLegalLegislationtype()
        {

            try
            {
                return Ok(await _ILookups.Getlegallegislationtypes());

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }


        }
        [HttpGet("GetLegalLegislationtypeById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2023-08-03' Version="1.0" Branch="master"> Get Legal legislation types </History>
        public async Task<IActionResult> GetLegalLegislationtypeById(int Id)
        {

            try
            {
                return Ok(await _ILookups.GetLegalLegislationtypeById(Id));

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }


        }
        #endregion


        #region Update legal Principle Type

        [HttpPost("SavelegalPrincipleTypes")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>
        public async Task<IActionResult> SavelegalPrincipleTypes(LegalPrincipleType legalPrincipleTypes)
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
                await _ILookups.SavelegalPrincipleTypes(legalPrincipleTypes);


                return Ok(legalPrincipleTypes);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Update legal Principle Type

        [HttpPost("UpdatelegalPrincipleTypes")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-11-11' Version="1.0" Branch="master"> Update Legal Legislation type</History>
        public async Task<IActionResult> UpdatelegalPrincipleTypes(LegalPrincipleType legalPrincipleTypes)
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
                await _ILookups.UpdatelegalPrincipleTypes(legalPrincipleTypes);


                return Ok(legalPrincipleTypes);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Delete Legal Principle Type

        [HttpPost("DeletelegalPrincipleTypes")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-11-11' Version="1.0" Branch="master"> Update Legal Legislation type</History>
        public async Task<IActionResult> DeletelegalPrincipleTypes(LegalPrincipleTypeVM legalPrincipleTypes)
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
                await _ILookups.DeletelegalPrincipleTypes(legalPrincipleTypes);


                return Ok(legalPrincipleTypes);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        [HttpPost("ActivelegalPrincipleTypes")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-11-11' Version="1.0" Branch="master"> Update Legal Legislation type</History>
        public async Task<IActionResult> ActivelegalPrincipleTypes(LegallegislationtypesVM legalPrincipleTypes)
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
                await _ILookups.ActivelegalPrincipleTypes(legalPrincipleTypes);


                return Ok(legalPrincipleTypes);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpPost("ActiveDocumentTypes")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-11-11' Version="1.0" Branch="master"> Update Legal Legislation type</History>
        public async Task<IActionResult> ActiveDocumentTypes(AttachmentTypeVM attachmentType)
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
                await _ILookups.ActiveDocumentTypes(attachmentType);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }


        #region Save legal Publication Source Names

        [HttpPost("SavelegalPublicationSourceNames")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-11-11' Version="1.0" Branch="master"> Submit Legal Legislation type</History>
        public async Task<IActionResult> SavelegalPublicationSourceNames(LegalPublicationSourceName legalPublicationSourceNames)
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
                await _ILookups.SavelegalPublicationSourceNames(legalPublicationSourceNames);


                return Ok(legalPublicationSourceNames);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Update legal Publication Source Names

        [HttpPost("UpdatePublicationSourceNames")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-11-11' Version="1.0" Branch="master"> Update Legal Legislation type</History>
        public async Task<IActionResult> UpdatePublicationSourceNames(LegalPublicationSourceName legalPublicationSourceNames)
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
                await _ILookups.UpdatelegalPublicationSourceName(legalPublicationSourceNames);


                return Ok(legalPublicationSourceNames);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Delete legal Publication Source Name

        [HttpPost("DeletelegalPublicationSourceNames")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-11-11' Version="1.0" Branch="master"> Update Legal Legislation type</History>
        public async Task<IActionResult> DeletelegalPublicationSourceNames(LegalPublicationSourceNameVM legalPublicationSourceName)
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
                await _ILookups.DeletelegalPublicationSourceNames(legalPublicationSourceName);

                return Ok(legalPublicationSourceName);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPost("ActivePublicationSourceNames")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-11-11' Version="1.0" Branch="master"> Update Legal Legislation type</History>
        public async Task<IActionResult> ActivePublicationSourceNames(LegalPublicationSourceNameVM legalPublicationSourceName)
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
                await _ILookups.ActivePublicationSourceNames(legalPublicationSourceName);

                return Ok(legalPublicationSourceName);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Lms Literature Tags
        [HttpGet("GetLmsLiteratureTags")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2023-08-03' Version="1.0" Branch="master"> Get Lms Literature Tags  </History>
        public async Task<IActionResult> GetLmsLiteratureTags()
        {
            try
            {
                return Ok(await _ILookups.GetLmsLiteratureTags());

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        #endregion

        #region  Get Legal Principle Types by Id 
        [HttpGet("GetLegalPrincipleTypesById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegalPrincipleTypesById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetLegalPrincipleTypesById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Legal Publication Source Name 
        [HttpGet("GetLegalPublicationSourceName")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegalPublicationSourceName()
        {
            try
            {
                return Ok(await _ILookups.GetLegalPublicationSourceName());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Legal Publication Source Name By ID
        [HttpGet("GetLegalPublicationSourceNameById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegalPublicationSourceNameById(int PublicationNameId)
        {
            try
            {
                return Ok(await _ILookups.GetLegalPublicationSourceNameById(PublicationNameId));
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        [HttpGet("GetLmsLiteratureTagsbyId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2023-08-03' Version="1.0" Branch="master"> Get Legal legislation types </History>
        public async Task<IActionResult> GetLmsLiteratureTagsbyId(int Id)
        {

            try
            {
                return Ok(await _ILookups.GetLmsLiteratureTagsById(Id));

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }


        }

        #region Save Save Literature Tags

        [HttpPost("SaveLiteratureTags")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>
        public async Task<IActionResult> SaveLiteratureTags(LiteratureTag literatureTag)
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
                await _ILookups.SaveLiteratureTags(literatureTag);



                return Ok(literatureTag);

            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Update literature Tag
        [HttpPost("UpdateLiteratureTags")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> UpdateLiteratureTags(LiteratureTag LiteratureTag)
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
                await _ILookups.UpdateLiteratureTags(LiteratureTag);

                return Ok(LiteratureTag);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Delete Department

        [HttpPost("DeleteLiteratureTags")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteLiteratureTags(LmsLiteratureTagVM literatureTagVM)
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
                await _ILookups.DeleteLiteratureTags(literatureTagVM);


                return Ok(literatureTagVM);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Active staus of Department
        [HttpPost("ActiveLiteratureTag")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> ActiveLiteratureTag(LmsLiteratureTagVM lmsLiteratureTagVM)
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
                await _ILookups.ActiveLiteratureTags(lmsLiteratureTagVM);

                return Ok(lmsLiteratureTagVM);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region  Get Legal Principle Types by Id 
        //[HttpGet("GetLegalPrincipleTypesById")]
        //[MapToApiVersion("1.0")]
        //public async Task<IActionResult> GetLegalPrincipleTypesById(int Id)
        //{
        //	try
        //	{
        //		return Ok(await _ILookups.GetLegalPrincipleTypesById(Id));
        //	}
        //	catch (Exception ex)
        //	{
        //		return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
        //	}
        //}
        #endregion

        #region Government Entities lookup
        #region Get Government Entities list 
        [HttpGet("GetGovernmentEntiteslist")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGovernmentEntiteslist()
        {
            try
            {
                return Ok(await _ILookups.GetGovernmentEntiteslist());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region  Get Government Entites  by Id 
        [HttpGet("GetGovernmentEntitysById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGovernmentEntitysById(int EntityId)
        {
            try
            {
                return Ok(await _ILookups.GetGovernmentEntitysById(EntityId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Save Government Entity 

        [HttpPost("SaveGovernmentEntity")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>
        public async Task<IActionResult> SaveGovernmentEntity(GovernmentEntity governmentEntity)
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
                var result = await _ILookups.SaveGovernmentEntity(governmentEntity);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.GovEntityKey);
                }

                return Ok(governmentEntity);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Update Government Entity

        [HttpPost("UpdateGovernmentEntity")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-11-11' Version="1.0" Branch="master"> Update Legal Legislation type</History>
        public async Task<IActionResult> UpdateGovernmentEntity(GovernmentEntity governmentEntity)
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
                var result = await _ILookups.UpdateGovernmentEntity(governmentEntity);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.UpdateGovEntityKey);
                }

                return Ok(governmentEntity);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Delete Government   

        [HttpPost("DeleteGovernmentEntity")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteGovernmentEntity(GovernmentEntitiesVM governmentEntity)
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
                var result = await _ILookups.DeleteGovernmentEntity(governmentEntity);

                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.DeleteGovEntityKey);
                }
                return Ok(governmentEntity);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Active staus of Government Entity 
        [HttpPost("ActiveGovernmentEntities")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> ActiveGovernmentEntities(GovernmentEntitiesVM governmentEntities)
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
                var result = await _ILookups.ActiveGovernmentEntities(governmentEntities);

                if (result != null)
                {
                    _client.SendMessage(result, RabbitMQKeys.ActiveGovEntityKey);
                }
                return Ok(governmentEntities);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Sync Government Entities And Departments
        [HttpPost("SyncGEsAndDepartments")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2024-06-06' Version="1.0" Branch="master"> Sync Government Entity and Departments from Tarasol</History>
        public async Task<IActionResult> SyncGEsAndDepartments(string username)
        {
            try
            {
                using (G2GIWSSoapClient client = new G2GIWSSoapClient(G2GTarasolServiceReference.G2GIWSSoapClient.EndpointConfiguration.G2GIWSSoap))
                {
                    DataSet sitesList = await client.GetSitesListAsync();
                    DataSet sitesBranchList = await client.GetSitesBranchListAsync();
                    await _ILookups.SyncGEsAndDepartments(username, sitesList, sitesBranchList);
                }

                //if (result != null)
                //{
                //    
                //    _client.SendMessage(result, RabbitMQKeys.ActiveGovEntityKey);
                //}
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Sync GEs and Departments",
                    Task = "Sync GEs and Departments",
                    Description = "Sync GEs and Departments",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "GEs and Departments synchronized successfully.",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Unable to complete sync of GEs and Departments",
                    Body = ex.Message,
                    Category = "Unable to complete sync of GEs and Departments",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = ex.InnerException?.Message,
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetLatestGEsAndDepartmentsSyncLog")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2024-06-06' Version="1.0" Branch="master"> Get Sync Government Entity and Departments Log</History>
        public async Task<IActionResult> GetLatestGEsAndDepartmentsSyncLog()
        {
            try
            {
                return Ok(await _ILookups.GetLatestGEsAndDepartmentsSyncLog());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #endregion

        #region Court Types lookup
        #region Get Court Types list 
        [HttpGet("GetCourtTypeList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCourtTypeList()
        {
            try
            {
                return Ok(await _ILookups.GetCourtTypeList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region  Get Court Type  by Id 
        [HttpGet("GetCourtTypesById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCourtTypesById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetCourtTypesById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Save Save Court Types 

        [HttpPost("SaveCourtType")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>
        public async Task<IActionResult> SaveCourtType(Court courts)
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
                var result = await _ILookups.SaveCourtType(courts);

                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.CourtNameKey);
                }

                return Ok(courts);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Update Court Type

        [HttpPost("UpdateCourtType")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateCourtType(FATWA_DOMAIN.Models.CaseManagment.Court courts)
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
                var result = await _ILookups.UpdateCourtType(courts);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.UpdateCourtNameKey);
                }
                return Ok(courts);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Delete Court Type  

        [HttpPost("DeleteCourtType")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteCourtType(CourtDetailVM courts)
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
                var result = await _ILookups.DeleteCourtType(courts);
                {

                    _client.SendMessage(result, RabbitMQKeys.DeleteCourtNameKey);
                }

                return Ok(courts);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Active  of Court Type
        [HttpPost("ActiveCourtTypes")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> ActiveCourtTypes(CourtDetailVM courts)
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
                var result = await _ILookups.ActiveCourtTypes(courts);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.ActiveCourtsKey);
                }

                return Ok(courts);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #endregion

        #region Chamber lookup
        #region Get Chamber  list 
        [HttpGet("GetChamberList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetChamberList()
        {
            try
            {
                return Ok(await _ILookups.GetChamberList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region  Get Chamber  by Id 
        [HttpGet("GetChamberById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetChamberById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetChamberById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Save Chamber 

        [HttpPost("SaveChamber")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>
        public async Task<IActionResult> SaveChamber(Chamber chamber)
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
                var result = await _ILookups.SaveChamber(chamber);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.ChamberKey);
                }

                return Ok(chamber);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Update Chamber

        [HttpPost("UpdateChamber")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateChamber(Chamber chamber)
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
                var result = await _ILookups.UpdateChamber(chamber);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.UpdateChamberKey);
                }

                return Ok(chamber);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Delete Chamber

        [HttpPost("DeleteChamber")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteChamber(ChamberDetailVM chamber)
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
                var result = await _ILookups.DeleteChamber(chamber);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.DeleteChamberKey);
                }

                return Ok(chamber);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Active staus of Chamber
        [HttpPost("ActiveChamber")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> ActiveChamber(ChamberDetailVM chamber)
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
                var result = await _ILookups.ActiveChamber(chamber);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.ActiveChamberKey);
                }
                return Ok(chamber);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region  Get Chambers view Detail  by Id 
        [HttpGet("GetChamberDetailById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetChamberDetailById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetChamberDetailById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Save Chamber Operating Sector 
        [HttpPost("SaveChamberOperatingSector")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ihsaan Abbas' Date='2024-02-26' Version="1.0" Branch="master"> Save Chamber Operating Sector</History>
        public async Task<IActionResult> SaveChamberOperatingSector(ChamberOperatingSector chamberOperatingSector)
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
                await _ILookups.SaveChamberOperatingSector(chamberOperatingSector);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region  Get Chambers  by  Court Id
        [HttpGet("GetChamberByCourtId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetChamberByCourtId(int courtId)
        {
            try
            {
                return Ok(await _ILookups.GetChamberByCourtId(courtId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #endregion 

        #region Department lookup
        #region Get Department  list 
        [HttpGet("GetDepartmentList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetDepartmentList()
        {
            try
            {
                return Ok(await _ILookups.GetDepartmentList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region  Get Department  by Id 
        [HttpGet("GetDepartmentById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetDepartmentById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetDepartmentById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Save Department 

        [HttpPost("SaveDepartment")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>
        public async Task<IActionResult> SaveDepartment(Department department)
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
                await _ILookups.SaveDepartment(department);
                //if(result != null)
                //{
                //    
                //    _client.SendMessage(result, SaveDepartmentKey);
                //}

                return Ok(department);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Update Department
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("UpdateDepartment")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateDepartment(Department department)
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
                var result = await _ILookups.UpdateDepartment(department);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.UpdateDepartmentKey);
                }

                return Ok(department);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Delete Department
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("DeleteDepartment")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteDepartment(DepartmentDetailVM department)
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
                var result = await _ILookups.DeleteDepartment(department);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.DeleteGovEntityDepartmentKey);
                }

                return Ok(department);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Active staus of Department
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("ActiveDepartment")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> ActiveDepartment(DepartmentDetailVM department)
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
                var result = await _ILookups.ActiveDepartment(department);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.ActiveDepartmentsKey);
                }
                return Ok(department);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #endregion

        #region TaskType lookup
        #region Get TaskType  list 
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpGet("GetTaskTypeList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTaskTypeList()
        {
            try
            {
                return Ok(await _ILookups.GetTaskTypeList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region  Get TaskType  by Id 
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpGet("GetTaskTypeById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        public async Task<IActionResult> GetTaskTypeById(int TypeId)
        {
            try
            {
                return Ok(await _ILookups.GetTaskTypeById(TypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Update TaskType
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("UpdateTaskType")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateTaskType(TaskType taskType)
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
                var result = await _ILookups.UpdateTaskType(taskType);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.GovEntityPatternKey);
                }

                return Ok(taskType);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #endregion

        #region Communication Type 
        #region Get Communicationtype  list 
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpGet("GetCommunicationTypeList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCommunicationTypeList()
        {
            try
            {
                return Ok(await _ILookups.GetCommunicationTypeList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region  Get Communication  by Id 
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpGet("GetCommunicationByTypeId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCommunicationByTypeId(int CommunicationTypeId)
        {
            try
            {
                return Ok(await _ILookups.GetCommunicationByTypeId(CommunicationTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Save Communication Type
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("SaveCommunicationType")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>
        public async Task<IActionResult> SaveCommunicationType(CommunicationType communication)
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
                var result = await _ILookups.SaveCommunicationType(communication);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.SaveCommunicationTypeKey);
                }

                return Ok(communication);

            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Update Communication Type
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("UpdateCommunicationType")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateCommunicationType(CommunicationType communication)
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
                var result = await _ILookups.UpdateCommunicationType(communication);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.UpdateCommunicationTypeKey);
                }

                return Ok(communication);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #endregion
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpGet("GetAllCaseRequestNumber")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllCaseRequestNumber(int PatternTypeId)
        {
            try
            {
                return Ok(await _ILookups.GetAllCaseRequestNumber(PatternTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #region Save Case File Number Pattren
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>
        [HttpPost("SaveCMSCOMSPattrenNumber")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>
        public async Task<IActionResult> SaveCMSCOMSPattrenNumber(CmsComsNumPattern chamber)
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
                var result = await _ILookups.SaveCMSCOMSPattrenNumber(chamber);
                if (result.PatternTypId == (int)CmsComsNumPatternTypeEnum.CaseRequestNumber || result.PatternTypId == (int)CmsComsNumPatternTypeEnum.ConsultationRequestNumber)
                {

                    _client.SendMessage(result, RabbitMQKeys.GovEntityPatternKey);
                }

                return Ok(chamber);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Update Case File Number Pattren
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        /*        [HttpPost("UpdateCaseFileNumberPattren")]
                [MapToApiVersion("1.0")]
                public async Task<IActionResult> UpdateCaseFileNumberPattren(CmsComsNumPattern chamber)
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
                        var result = await _ILookups.UpdateCaseFileNumberPattren(chamber);
                        if (result != null)
                        {
                            
                            _client.SendMessage(result, RabbitMQKeys.GovEntityPatternKey);
                        }

                        return Ok(chamber);
                    }
                    catch (Exception ex)
                    {

                        return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
                    }
                }
        */
        #endregion
        #region Update Case File Number Pattren History
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("UpdateCaseFileNumberPattrenHistory")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateCaseFileNumberPattrenHistory(CmsComsNumPatternHistory PatternHistory)
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
                var result = await _ILookups.UpdateCaseFileNumberPattrenHistory(PatternHistory);

                return Ok(PatternHistory);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Update Chamber
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpGet("GetCmsComsNumberPatterntype")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCmsComsNumberPatterntype()
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
                return Ok(await _ILookups.GetCmsComsNumberPatterntype());
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }


        #endregion
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("DeleteCmComsPattern")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteCmComsPattern(CmsComsNumPatternVM cmsComsNumPatternVM)
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
                var result = await _ILookups.DeleteCmComsPattern(cmsComsNumPatternVM);


                if (result != null)
                {

                    _client.SendMessage(cmsComsNumPatternVM, RabbitMQKeys.GovEntityPatternDeleteKey);
                }
                return Ok(cmsComsNumPatternVM);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse
                {
                    Message = ex.Message,
                    InnerException = ex.InnerException?.Message
                });
            }

        }
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("UpdateCaseRequestStatus")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateCaseRequestStatus(CaseRequestStatus caseRequestStatus)
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
                await _ILookups.UpdateCaseRequestStatus(caseRequestStatus);

                return Ok(caseRequestStatus);

            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        #region  Get Case Request Status By Id 
        [HttpGet("GetCaseRequestStatusById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCaseRequestStatusById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetCaseRequestStatusById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Get Case Request Status list 
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpGet("GetCaseRequestStatusList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCaseRequestStatusList()
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
                return Ok(await _ILookups.GetCaseRequestStatusList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #endregion

        #region Case  Status 
        #region Get Case  Status list 
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpGet("GetCaseStatusList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCaseStatusList()
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
                return Ok(await _ILookups.GetCaseStatusList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #endregion
        #region  Update Case Status
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("UpdateCaseStatus")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateCaseStatus(CmsRegisteredCaseStatus RegisteredCaseStatus)
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
                await _ILookups.UpdateCaseStatus(RegisteredCaseStatus);


                return Ok(RegisteredCaseStatus);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #endregion


        #region  GetTimeInttervals
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        //      [HttpGet("GetTimeInttervals")]
        //[MapToApiVersion("1.0")]
        //public async Task<IActionResult> GetTimeInttervals()
        //{
        //	try
        //	{ 
        //		return Ok(await _ILookups.GetTimeInttervals());
        //	}
        //	catch (Exception ex)    
        //	{

        //		return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
        //	}
        //}
        #endregion

        #region  Get Communication ResponseDetail By Id
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpGet("GetCommunicationResponseDetailByid")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCommunicationResponseDetailByid(int id)
        {
            try
            {
                return Ok(await _ILookups.GetCommunicationResponseDetailByid(id));
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        #region get Communication Response
        [HttpGet("GetCommunicationType")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2022-11-11' Version="1.0" Branch="master">Get court</History>
        public async Task<IActionResult> GetCommunicationType()
        {
            try
            {
                return Ok(await _ILookups.GetCommunicationType());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion



        #region  Get Chamber  by Id 
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpGet("GetRequestTypeList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetRequestTypeList()
        {
            try
            {
                return Ok(await _ILookups.GetRequestTypeList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region SubType lookup
        #region Get SubType  list 
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpGet("GetSubTypeList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSubTypeList(int RequestTypeId)
        {
            try
            {
                return Ok(await _ILookups.GetSubTypeList(RequestTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region  Get Get Request Type By Id
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpGet("GetRequestTypeById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetRequestTypeById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetRequestTypeById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region  Get Get Sub Type By Id
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpGet("GetSubTypeById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSubTypeById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetSubTypeById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region  Update Request Type By Id
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("UpdateRequestType")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateRequestType(RequestType requestType)
        {
            try
            {
                var result = await _ILookups.UpdateRequestType(requestType);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.UpdateRequesttypeKey);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Save SubType 

        [HttpPost("SaveSubType")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>
        public async Task<IActionResult> SaveSubType(Subtype chamber)
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
                var result = await _ILookups.SaveSubType(chamber);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.SaveSubTypeKey);
                }

                return Ok(chamber);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion



        #region Update SubType
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("UpdateSubtype")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateSubtype(Subtype subtype)
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
                var result = await _ILookups.UpdateSubType(subtype);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.UpdateSubtypeKey);
                }

                return Ok(subtype);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Delete SubType
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("DeleteSubType")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteSubType(SubTypeVM SubtypeVM)
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
                var result = await _ILookups.DeleteSubtype(SubtypeVM);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.DeleteSubTypeKey);
                }
                return Ok(SubtypeVM);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Active staus of SubType
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("ActiveSubType")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> ActiveSubType(SubTypeVM subType)
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
                var result = await _ILookups.ActiveSubType(subType);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.ActiveSubTypeKey);
                }
                return Ok(subType);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #endregion

        #region ConsultationLegislationFileType lookup
        #region Get ConsultationLegislationFileType  list 
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpGet("GetConsultationLegislationFileTypeList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetConsultationLegislationFileTypeList()
        {
            try
            {
                return Ok(await _ILookups.GetConsultationLegislationFileTypeList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region  Get Get Sub Type By Id
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpGet("GetConsultationLegislationFileTypeById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetConsultationLegislationFileTypeById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetConsultationLegislationFileTypeById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Save ConsultationLegislationFileType 
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("SaveConsultationLegislationFileType")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>
        public async Task<IActionResult> SaveConsultationLegislationFileType(ConsultationLegislationFileType chamber)
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
                var result = await _ILookups.SaveConsultationLegislationFileType(chamber);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.SaveConsultationLegislationFileTypeKey);
                }

                return Ok(chamber);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Update ConsultationLegislationFileType
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("UpdateConsultationLegislationFileType")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateConsultationLegislationFileType(ConsultationLegislationFileType ConsultationLegislationFileType)
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
                var result = await _ILookups.UpdateConsultationLegislationFileType(ConsultationLegislationFileType);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.UpdateConsultationLegislationFileTypeKey);
                }

                return Ok(ConsultationLegislationFileType);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Delete ConsultationLegislationFileType
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("DeleteConsultationLegislationFileType")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        public async Task<IActionResult> DeleteConsultationLegislationFileType(ConsultationLegislationFileTypeVM ConsultationLegislationFileTypeVM)
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
                var result = await _ILookups.DeleteConsultationLegislationFileType(ConsultationLegislationFileTypeVM);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.DeleteConsultationLegislationFileTypeKey);
                }
                return Ok(ConsultationLegislationFileTypeVM);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Active staus of ConsultationLegislationFileType
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("ActiveConsultationLegislationFileType")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> ActiveConsultationLegislationFileType(ConsultationLegislationFileTypeVM ConsultationLegislationFileType)
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
                var result = await _ILookups.ActiveConsultationLegislationFileType(ConsultationLegislationFileType);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.ActiveConsultationLegislationFileTypeKey);
                }
                return Ok(ConsultationLegislationFileType);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #endregion


        #region ComsInternationalArbitrationType lookup
        #region Get ComsInternationalArbitrationType  list 
        [HttpGet("GetComsInternationalArbitrationTypeList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetComsInternationalArbitrationTypeList()
        {
            try
            {
                return Ok(await _ILookups.GetComsInternationalArbitrationTypeList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region  Get Get Sub Type By Id
        [HttpGet("GetComsInternationalArbitrationTypeById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetComsInternationalArbitrationTypeById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetComsInternationalArbitrationTypeById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Save ComsInternationalArbitrationType 

        [HttpPost("SaveComsInternationalArbitrationType")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>
        public async Task<IActionResult> SaveComsInternationalArbitrationType(ComsInternationalArbitrationType chamber)
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
                var result = await _ILookups.SaveComsInternationalArbitrationType(chamber);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.SaveComsInternationalArbitrationTypeKey);
                }

                return Ok(chamber);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Update ComsInternationalArbitrationType

        [HttpPost("UpdateComsInternationalArbitrationType")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateComsInternationalArbitrationType(ComsInternationalArbitrationType ComsInternationalArbitrationType)
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
                var result = await _ILookups.UpdateComsInternationalArbitrationType(ComsInternationalArbitrationType);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.UpdateComsInternationalArbitrationTypeKey);
                }

                return Ok(ComsInternationalArbitrationType);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Delete ComsInternationalArbitrationType

        [HttpPost("DeleteComsInternationalArbitrationType")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteComsInternationalArbitrationType(ComsInternationalArbitrationTypeVM ComsInternationalArbitrationTypeVM)
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
                var result = await _ILookups.DeleteComsInternationalArbitrationType(ComsInternationalArbitrationTypeVM);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.DeleteComsInternationalArbitrationTypeKey);
                }
                return Ok(ComsInternationalArbitrationTypeVM);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        //#region Get USer By Department
        //[HttpGet("GetUsersByDepartment")]
        //[MapToApiVersion("1.0")]
        //public async Task<IActionResult> GetUsersByDepartment(int DepartmentId)
        //{
        //    try
        //    {
        //        return Ok(await _ILookups.GetUsersByDepartment(DepartmentId));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        //#endregion
        #region Save Sub Types 
        //<History Author = 'Muhammad Ali' Date='2024-07-01' Version="1.0" Branch="master"> Save Sub types</History>

        [HttpPost("SaveSubTypes")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Ali' Date='2024-07-01' Version="1.0" Branch="master"> Save Sub types</History>
        public async Task<IActionResult> SaveSubTypes(Subtype subtype)
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
                var result = await _ILookups.SaveSubTypes(subtype);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.SaveSubTypesKey);
                }

                return Ok(subtype);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Get Company List

        [HttpGet("GetCompanyList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCompanyList()
        {
            try
            {
                return Ok(await _ILookups.GetCompanyList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get City List

        [HttpGet("GetCityList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCityList()
        {
            try
            {
                return Ok(await _ILookups.GetCityList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Add New Company

        [HttpPost("AddNewCompany")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddNewCompany(Company args)
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

                var result = await _ILookups.AddNewCompany(args);
                if (result)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Adding company",
                        Task = "Adding company process",
                        Description = "User able to Adding company successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Adding company executed Successfully",
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
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Adding company Failed",
                    Body = ex.Message,
                    Category = "User unable to company legislation",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Adding company Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Add New Designation

        [HttpPost("AddNewDesignation")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddNewDesignation(Designation args)
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

                var result = await _ILookups.AddNewDesignation(args);
                if (result)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Adding designation",
                        Task = "Adding designation process",
                        Description = "User able to Adding designation successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Adding designation executed Successfully",
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
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Adding designation Failed",
                    Body = ex.Message,
                    Category = "User unable to designation legislation",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Adding designation Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get Contact Details For File

        [HttpGet("GetContactDetailsForFile")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetContactDetailsForFile(Guid fileId)
        {
            try
            {
                return Ok(await _ILookups.GetContactDetailsForFile(fileId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion


        [HttpGet("GetAttendeeMeetingStatus")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAttendeeMeetingStatus()
        {
            try
            {
                return Ok(await _ILookups.GetAttendeeMeetingStatus());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #region Remove selected contact from file
        [HttpPost("RemoveContact")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> RemoveContact(ContactFileLinkVM args)
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
                return Ok(await _ILookups.RemoveContact(args));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Add Selected Contact to file
        [HttpPost("AddSelectedContactToFile")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> AddSelectedContactToFile(IList<ContactListVM> selectedContact, Guid? fileId, int? fileModule, string CurrentUser)
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
                return Ok(await _ILookups.AddSelectedContactToFile(selectedContact, fileId, fileModule, CurrentUser));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion


        //[HttpGet("GetAttendeeMeetingStatus")]
        //[MapToApiVersion("1.0")]
        //public async Task<IActionResult> GetAttendeeMeetingStatus()
        //{
        //    try
        //    {
        //        return Ok(await _ILookups.GetAttendeeMeetingStatus());
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
        //    }
        //}
        //    #region Remove selected contact from file
        //    [HttpPost("RemoveContact")]
        //    [MapToApiVersion("1.0")]
        //    public async Task<IActionResult> RemoveContact(ContactFileLinkVM args)
        //    {
        //        try
        //        {
        //if (!ModelState.IsValid)
        //{
        //	return BadRequest(new RequestFailedResponse
        //	{
        //		Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
        //	});
        //}
        //return Ok(await _ILookups.RemoveContact(args));
        //        }
        //        catch (Exception ex)
        //        {
        //            return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
        //        }
        //    }
        //    #endregion
        #region Active staus of ComsInternationalArbitrationType
        [HttpPost("ActiveComsInternationalArbitrationType")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> ActiveComsInternationalArbitrationTypes(ComsInternationalArbitrationTypeVM ComsInternationalArbitrationType)
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
                var result = await _ILookups.ActiveComsInternationalArbitrationType(ComsInternationalArbitrationType);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.ActiveComsInternationalArbitrationTypeKey);
                }
                return Ok(ComsInternationalArbitrationType);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        [HttpGet("GetLookupHistoryListByRefernceId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLookupHistoryListByRefernceId(int Id, int LookupstableId)
        {
            try
            {
                var result = await _ILookups.GetLookupHistoryListByRefernceId(Id, LookupstableId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion


        #endregion
        [HttpGet("GetCmsComNumPatternHistoryDetail")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCmsComNumPatternHistoryDetail(Guid Id)
        {
            try
            {
                return Ok(await _ILookups.GetCmsComNumPatternHistoryDetail(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetCmsPatternHistoryById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCmsPatternHistoryById(Guid Id)
        {
            try
            {
                return Ok(await _ILookups.GetCmsPatternHistoryById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #region Get All GE User List Pattern Attached 
        [HttpGet("GetAllAGEUserListPatternAttached")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllAGEUserListPatternAttached(Guid Id, int SelectedPatternTypeId, bool IsDefault)
        {
            try
            {
                return Ok(await _ILookups.GetAllAGEUserListPatternAttached(Id, SelectedPatternTypeId, IsDefault));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Check Already Attached Govt Entity
        [HttpPost("CheckPatternAlreadyAttachedGovtid")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CheckPatternAlreadyAttachedGovtid(List<int> EntityId, int SelectedPatternTypeId)
        {
            try
            {
                return Ok(await _ILookups.CheckPatternAlreadyAttachedGovtid(EntityId, SelectedPatternTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        //#region Add Selected Contact to file
        //[HttpPost("AddSelectedContactToFile")]
        //[MapToApiVersion("1.0")]

        //public async Task<IActionResult> AddSelectedContactToFile(IList<ContactListVM> selectedContact, Guid? fileId, int? fileModule, string CurrentUser)
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
        //        return Ok(await _ILookups.AddSelectedContactToFile(selectedContact, fileId, fileModule, CurrentUser));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
        //    }
        //}
        //#endregion


        //[HttpGet("GetLookupHistoryListByRefernceId")]
        //[MapToApiVersion("1.0")]
        //public async Task<IActionResult> GetLookupHistoryListByRefernceId(int Id, int LookupstableId)
        //{
        //    try
        //    {
        //        var result = await _ILookups.GetLookupHistoryListByRefernceId(Id, LookupstableId);

        //            return Ok(result);
        //        }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
        //    }
        //}



        //[HttpGet("GetCmsComsReminderType")]
        //[MapToApiVersion("1.0")]
        ////<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get goverment entities</History>
        //public async Task<IActionResult> GetCmsComsReminderType()
        //{

        //    try
        //    {
        //        var result = await _ILookups.GetCmsComsReminderType();
        //        return Ok(result);

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
        //    }
        //}

        //[HttpGet("GetCmsComNumPatternHistoryDetail")]
        //[MapToApiVersion("1.0")]
        //public async Task<IActionResult> GetCmsComNumPatternHistoryDetail(Guid Id)
        //{
        //    try
        //    {
        //        return Ok(await _ILookups.GetCmsComNumPatternHistoryDetail(Id));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
        //    }
        //}

        //[HttpGet("GetCmsPatternHistoryById")]
        //[MapToApiVersion("1.0")]
        //public async Task<IActionResult> GetCmsPatternHistoryById(Guid Id)
        //{
        //    try
        //    {
        //        return Ok(await _ILookups.GetCmsPatternHistoryById(Id));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
        //    }
        //}
        //#region Get All GE User List Pattern Attached 
        //[HttpGet("GetAllAGEUserListPatternAttached")]
        //      [MapToApiVersion("1.0")]
        //      public async Task<IActionResult> GetAllAGEUserListPatternAttached(Guid Id )
        //      {
        //          try
        //          {
        //              return Ok(await _ILookups.GetAllAGEUserListPatternAttached(Id));
        //          }
        //          catch (Exception ex)
        //          {
        //              return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
        //          }
        //      }
        //#endregion

        //#region Check Already Attached Govt Entity
        //[HttpGet("CheckPatternAlreadyAttachedGovtid")]
        //[MapToApiVersion("1.0")]
        //public async Task<IActionResult> CheckPatternAlreadyAttachedGovtid(int EntityId)
        //{
        //	try
        //	{
        //              var result = await _ILookups.CheckPatternAlreadyAttachedGovtid(EntityId);
        //		return Ok(result);
        //	}
        //	catch (Exception ex)
        //	{
        //		return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
        //	}
        //}
        //      #endregion
        //[HttpGet("GetIntervalHistoryListByRefernceId")]
        //[MapToApiVersion("1.0")]
        //public async Task<IActionResult> GetIntervalHistoryListByRefernceId(int Id, int IntervalTypeId)
        //{
        //    try
        //    {
        //        var result = await _ILookups.GetIntervalHistoryListByRefernceId(Id, IntervalTypeId);

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
        //    }
        //}

        //#region Add Selected Contact to file
        //[HttpPost("AddSelectedContactToFile")]
        //[MapToApiVersion("1.0")]

        //public async Task<IActionResult> AddSelectedContactToFile(IList<ContactListVM> selectedContact, Guid? fileId, int? fileModule, string CurrentUser)
        //{
        //	try
        //	{
        //		if (!ModelState.IsValid)
        //		{
        //			return BadRequest(new RequestFailedResponse
        //			{
        //				Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
        //			});
        //		}
        //		return Ok(await _ILookups.AddSelectedContactToFile(selectedContact, fileId, fileModule, CurrentUser));
        //	}
        //	catch (Exception ex)
        //	{
        //		return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
        //	}
        //}
        //#endregion

        #region Chamber Number CRUD 

        #region Get Chamber Number List 
        [HttpGet("GetChamberNumberList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetChamberNumberList()
        {
            try
            {
                var result = await _ILookups.GetChamberNumberList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Delete Chamber Number 

        [HttpPost("DeleteChambersNumber")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteChambersNumber(ChambersNumberDetailVM chambersNumber)
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
                var result = await _ILookups.DeleteChambersNumber(chambersNumber);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.DeleteChamberNumberKey);
                }

                return Ok(chambersNumber);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Active staus of Chamber Number 
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("ActiveChambersNumber")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> ActiveChambersNumber(ChambersNumberDetailVM chambersNumber)
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
                var result = await _ILookups.ActiveChambersNumber(chambersNumber);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.ActiveChamberNumberKey);
                }
                return Ok(chambersNumber);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region  Get Chamber Number Detail  by Id 
        [HttpGet("GetChamberNumberById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetChamberNumberById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetChamberNumberById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Save Chamber Number 
        [HttpPost("SaveChamberNumber")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>
        public async Task<IActionResult> SaveChamberNumber(ChamberNumber chambersNumber)
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
                var result = await _ILookups.SaveChamberNumber(chambersNumber);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.SaveChamberNumberKey);
                }
                return Ok(chambersNumber);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Update Chamber Number 
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("UpdateChamberNumber")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateChamberNumber(ChamberNumber chambersNumber)
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
                var result = await _ILookups.UpdateChamberNumber(chambersNumber);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.UpdateChamberNumberKey);
                }
                return Ok(chambersNumber);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region  Get Chambers Number Detail  by Id 
        [HttpGet("GetChamberNumberDetailById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetChamberNumberDetailById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetChamberNumberDetailById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #endregion

        #region Chamber Number Hearing CRUD
        #region  Get Hearing Days  
        [HttpGet("GetHearingDays")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetHearingDays()
        {
            try
            {
                var result = await _ILookups.GetHearingDays();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region  Get Chamber Number 
        [HttpGet("GetChamberNumber")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetChamberNumber()
        {
            try
            {
                var result = await _ILookups.GetChamberNumber();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Chamber Number Hearing List 
        [HttpGet("GetChamberNumberHearingList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetChamberNumberHearingList()
        {
            try
            {
                var result = await _ILookups.GetChamberNumberHearingList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Save Chamber Number Hearing
        [HttpPost("SaveChamberNumberHearing")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ihsaan Abbas' Date='2024-19-03' Version="1.0" Branch="master"> Save Chamber Number Hearing</History>
        public async Task<IActionResult> SaveChamberNumberHearing(ChamberNumberHearing chamberNumberHearing)
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
                await _ILookups.SaveChamberNumberHearing(chamberNumberHearing);
                return Ok(chamberNumberHearing);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region  Get Chamber Number Hearing  By Id
        [HttpGet("GetChamberNumberHearingById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetChamberNumberHearingById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetChamberNumberHearingById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Update Chamber Number Hearing   

        [HttpPost("UpdateChamberNumberHearing")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateChamberNumberHearing(ChamberNumberHearing chamberNumberHearing)
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
                var result = await _ILookups.UpdateChamberNumberHearing(chamberNumberHearing);

                return Ok(chamberNumberHearing);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Delete  Chamber Number Hearing  
        [HttpPost("DeleteChambersNumberHearing")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteChambersNumberHearing(ChamberNumberHearingDetailVM chamberNumberHearing)
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
                var result = await _ILookups.DeleteChambersNumberHearing(chamberNumberHearing);
                return Ok(chamberNumberHearing);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #endregion

        #region Government Entity Department CRUD 
        #region Get Government Entity Department List 
        [HttpGet("GetGovernmentEntityDepartmentList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGovernmentEntityDepartmentList()
        {
            try
            {
                var result = await _ILookups.GetGovernmentEntityDepartmentList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Delete Government Entity Department   
        [HttpPost("DeleteGovernmentEntityDepartment")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteGovernmentEntityDepartment(GovernmentEntitiesSectorsVM GESector)
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
                var result = await _ILookups.DeleteGovernmentEntityDepartment(GESector);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.DeleteGovEntityDepartmentKey);
                }

                return Ok(GESector);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Active staus of Government Entity Department 
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("ActiveGovernmentEntityDepartment")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> ActiveGovernmentEntityDepartment(GovernmentEntitiesSectorsVM GESector)
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
                var result = await _ILookups.ActiveGovernmentEntityDepartment(GESector);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.ActiveGEDepartmentsKey);
                }
                return Ok(GESector);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region  Get Government Entity Department Detail  by Id 
        [HttpGet("GetGovtEntityDepartmentById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGovtEntityDepartmentById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetGovtEntityDepartmentById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Save Government Enttity Department  
        [HttpPost("SaveGovernmentEntityDepartment")]
        [MapToApiVersion("1.0")]
        //<History Author = 'ijaz Ahmad' Date='2023-08-08' Version="1.0" Branch="master"> Save Gov Entity Department</History>
        public async Task<IActionResult> SaveGovernmentEntityDepartment(GEDepartments GESector)
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
                var result = await _ILookups.SaveGovernmentEntityDepartment(GESector);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.SaveGEDepartmentKey);
                }

                return Ok(GESector);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Update Goverenment Entity Department  

        [HttpPost("UpdateGovernmentEntityDepartment")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateGovernmentEntityDepartment(GEDepartments GESector)
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
                var result = await _ILookups.UpdateGovernmentEntityDepartment(GESector);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.UpdateGEDepartmentKey);

                }

                return Ok(GESector);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region  Check Default Receiver Already Attached
        [HttpGet("CheckDefaultReceiverAlreadyAttached")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CheckDefaultReceiverAlreadyAttached(int EntityId, int DepartmentId)
        {
            try
            {
                return Ok(await _ILookups.CheckDefaultReceiverAlreadyAttached(EntityId, DepartmentId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #endregion

        #region Government Entity Representative CRUD 
        #region Get Government Entity Representative List 
        [HttpGet("GetGovernmentEntiteRepresentativesList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGovernmentEntiteRepresentativesList()
        {
            try
            {
                var result = await _ILookups.GetGovernmentEntiteRepresentativesList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Delete Government Entity Representative
        [HttpPost("DeleteGovernmentEntityRepresentative")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteGovernmentEntityRepresentative(GovernmentEntitiesRepresentativeVM GERepresentative)
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
                var result = await _ILookups.DeleteGovernmentEntityRepresentative(GERepresentative);
                if (result != null)
                {

                    _client.SendMessage(result, RabbitMQKeys.DeleteGovernmentEntityRepresentativeKey);
                }

                return Ok(GERepresentative);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Active staus of Government Entity Representative 
        //<History Author = 'Nabeel ur Rehman' Date='2023-08-08' Version="1.0" Branch="master"> Save Legal Legislation type</History>

        [HttpPost("ActiveGovernmentEntityRepresentative")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> ActiveGovernmentEntityRepresentative(GovernmentEntitiesRepresentativeVM GERepresentative)
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
                var result = await _ILookups.ActiveGovernmentEntityRepresentative(GERepresentative);
                if (result != null)
                {
                    _client.SendMessage(result, RabbitMQKeys.ActiveGovernmentEntityRepresentativeKey);
                }
                return Ok(GERepresentative);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region  Get Government Entity Representative Detail  by Id 
        [HttpGet("GetGovernmentRepresentativeById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGovernmentRepresentativeById(Guid Id)
        {
            try
            {
                return Ok(await _ILookups.GetGovernmentRepresentativeById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Save Government Entity Representative   
        [HttpPost("SaveGovernmentEntityRepresentative")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveGovernmentEntityRepresentative(GovernmentEntityRepresentative GERepresentative)
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
                var result = await _ILookups.SaveGovernmentEntityRepresentative(GERepresentative);
                if (result != null)
                {
                    _client.SendMessage(result, RabbitMQKeys.SaveGovernmentEntityRepresentativeKey);
                }

                return Ok(GERepresentative);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Update Goverenment Entity Representative   

        [HttpPost("UpdateGovernmentEntityRepresentative")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateGovernmentEntityRepresentative(GovernmentEntityRepresentative GERepresentative)
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
                var result = await _ILookups.UpdateGovernmentEntityRepresentative(GERepresentative);
                if (result != null)
                {
                    _client.SendMessage(result, RabbitMQKeys.UpdateGovernmentEntityRepresentativeKey);
                }

                return Ok(GERepresentative);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #endregion

        #region Get the record from number pattern history table when user editing 
        [HttpGet("GetCmsComsNumberPatternHistoryForEditing")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCmsComsNumberPatternHistoryForEditing(Guid Id)
        {
            try
            {
                return Ok(await _ILookups.GetCmsComsNumberPatternHistoryForEditing(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Chamber, Chamber Number & Court for Moj ROll 
        [HttpGet("GetChamberByUserId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetChamberByUserId(string UserId)
        {
            try
            {
                return Ok(await _ILookups.GetChamberByUserId(UserId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetCourtByUserId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCourtByUserId(string UserId)
        {
            try
            {
                return Ok(await _ILookups.GetCourtByUserId(UserId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetChamberNumberByUserId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetChamberNumberByUserId(string UserId)
        {
            try
            {
                return Ok(await _ILookups.GetChamberNumberByUserId(UserId));

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetAllChamberCourtChamberNumberForMojRollsByUserId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllChamberCourtChamberNumberForMojRollsByUserId(string UserId)
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
                return Ok(await _ILookups.GetAllChamberCourtChamberNumberForMojRollsByUserId(UserId));
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        [HttpGet("GetAllChamberCourtChamberNumberForMojRolls")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllChamberCourtChamberNumberForMojRolls()
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
                return Ok(await _ILookups.GetAllChamberCourtChamberNumberForMojRolls());
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Bank Details 
        #region Get Bank Name
        [HttpGet("GetBankNames")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ihsaan Abbas' Date='2024-03-11' Version="1.0" Branch="master">Get Bank Names</History>
        public async Task<IActionResult> GetBankNames()
        {
            try
            {
                return Ok(await _ILookups.GetBankNames());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Bank Name By Id
        [HttpGet("GetBankNameById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ihsaan Abbas' Date='2024-03-11' Version="1.0" Branch="master">Get Bank Names</History>
        public async Task<IActionResult> GetBankNameById(int bankId)
        {
            try
            {
                return Ok(await _ILookups.GetBankNameById(bankId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Bank Detail  By Entity Id
        [HttpGet("GetBankDetailByEntityId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ihsaan Abbas' Date='2024-03-11' Version="1.0" Branch="master">Get Bank Names</History>
        public async Task<IActionResult> GetBankDetailByEntityId(int EntityId)
        {
            try
            {
                return Ok(await _ILookups.GetBankDetailByEntityId(EntityId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Delete Bank Detail 

        [HttpPost("DeleteBankDetail")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteBankDetail(CmsBankGovernmentEntity cmsBankGovernmentEntity)
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
                var result = await _ILookups.DeleteBankDetail(cmsBankGovernmentEntity);

                return Ok(cmsBankGovernmentEntity);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #endregion

        #region EP Nationality CRUD 
        #region Get EP Nationality  List 
        [HttpGet("GetEpNationalityList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEpNationalityList()
        {
            try
            {
                var result = await _ILookups.GetEpNationalityList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region DeleteEP Nationality 

        [HttpPost("DeleteEpNationality")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteEpNationality(EpNationalityVM EPNationality)
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
                var result = await _ILookups.DeleteEpNationality(EPNationality);
                //if (result != null)
                //{
                //    
                //    _client.SendMessage(result, GovEntityPatternKey);
                //}

                return Ok(EPNationality);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Active staus of Ep Nationality

        [HttpPost("ActiveEpNationality")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> ActiveEpNationality(EpNationalityVM EPNationality)
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
                var result = await _ILookups.ActiveEpNationality(EPNationality);
                //if (result != null)
                //{
                //    
                //    _client.SendMessage(result, GovEntityPatternKey);
                //}
                return Ok(EPNationality);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region  Get Ep Nationality Details by Id 
        [HttpGet("GetEpNationalityById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEpNationalityById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetEpNationalityById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Save  Ep Nationality 
        [HttpPost("SaveNationality")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveNationality(Nationality EPNationality)
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
                await _ILookups.SaveNationality(EPNationality);
                //if(result != null)
                //{
                //    
                //    _client.SendMessage(result, SaveDepartmentKey);
                //}

                return Ok(EPNationality);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Update Ep Nationality  

        [HttpPost("UpdateNationality")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateNationality(Nationality EPNationality)
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
                var result = await _ILookups.UpdateNationality(EPNationality);
                //if (result != null)
                //{
                //    
                //    _client.SendMessage(result, UpdateDepartmentKey);
                //    //_client.SendMessage(result, GovEntityPatternKey);
                //}

                return Ok(EPNationality);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #endregion

        #region EP Grade CRUD 
        #region Get EP Grade  List 
        [HttpGet("GetEpGradeList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEpGradeList()
        {
            try
            {
                var result = await _ILookups.GetEpGradeList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Delete EP Grade 

        [HttpPost("DeleteEpGrade")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteEpGrade(EpGradeVM EPGrade)
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
                var result = await _ILookups.DeleteEpGrade(EPGrade);
                //if (result != null)
                //{
                //    
                //    _client.SendMessage(result, GovEntityPatternKey);
                //}

                return Ok(EPGrade);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Active staus of Ep Grade

        [HttpPost("ActiveEpGrade")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> ActiveEpGrade(EpGradeVM EPGrade)
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
                var result = await _ILookups.ActiveEpGrade(EPGrade);
                //if (result != null)
                //{
                //    
                //    _client.SendMessage(result, GovEntityPatternKey);
                //}
                return Ok(EPGrade);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region  Get Ep Grade Details by Id 
        [HttpGet("GetEpGradeById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEpGradeById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetEpGradeById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Save  Ep Grade 
        [HttpPost("SaveGrade")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveGrade(Grade EPGrade)
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
                await _ILookups.SaveGrade(EPGrade);
                //if(result != null)
                //{
                //    
                //    _client.SendMessage(result, SaveDepartmentKey);
                //}

                return Ok(EPGrade);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Update Ep Grade  

        [HttpPost("UpdateGrade")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateGrade(Grade EPGrade)
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
                var result = await _ILookups.UpdateGrade(EPGrade);
                //if (result != null)
                //{
                //    
                //    _client.SendMessage(result, UpdateDepartmentKey);
                //    //_client.SendMessage(result, GovEntityPatternKey);
                //}

                return Ok(EPGrade);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #endregion

        #region Gender   
        #region Get Gender  list 
        [HttpGet("GetGenderList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGenderList()
        {
            try
            {
                return Ok(await _ILookups.GetGenderList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region  Get Gender  by Id  
        [HttpGet("GetGenderById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGenderById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetGenderById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Update Gender 
        [HttpPost("UpdateGender")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateGender(Gender gender)
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
                var result = await _ILookups.UpdateGender(gender);
                //if (result != null)
                //{
                //    
                //    _client.SendMessage(result, UpdateCommunicationTypeKey);
                //}

                return Ok(gender);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #endregion

        #region EP Designation CRUD 
        #region Get EP Designation  List 
        [HttpGet("GetEpDesignationList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEpDesignationList()
        {
            try
            {
                var result = await _ILookups.GetEpDesignationList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Delete EP Grade 

        [HttpPost("DeleteEpDesignation")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteEpDesignation(EpDesignationVM EPDesignation)
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
                var result = await _ILookups.DeleteEpDesignation(EPDesignation);
                //if (result != null)
                //{
                //    
                //    _client.SendMessage(result, GovEntityPatternKey);
                //}

                return Ok(EPDesignation);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region  Get Ep Designation Details by Id 
        [HttpGet("GetEpDesignationById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEpDesignationById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetEpDesignationById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Save  Ep  Designation
        [HttpPost("SaveDesignation")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveDesignation(Designation EPDesignation)
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
                await _ILookups.SaveDesignation(EPDesignation);
                //if(result != null)
                //{
                //    
                //    _client.SendMessage(result, SaveDepartmentKey);
                //}

                return Ok(EPDesignation);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Update Ep Grade  

        [HttpPost("UpdateDesignation")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateDesignation(Designation EPDesignation)
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
                var result = await _ILookups.UpdateDesignation(EPDesignation);
                //if (result != null)
                //{
                //    
                //    _client.SendMessage(result, UpdateDepartmentKey);
                //    //_client.SendMessage(result, GovEntityPatternKey);
                //}

                return Ok(EPDesignation);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #endregion

        #region  Get Court  by UserId 
        [HttpGet("GetCourtById")]
        [MapToApiVersion("1.0")]
        //[AllowAnonymous]
        //[ApiKeyAuthorize]
        public async Task<IActionResult> GetCourtById(string UserId)
        {
            try
            {
                return Ok(await _ILookups.GetCourtById(UserId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region EP Contract Type CRUD 
        #region Get EP Contract Type   List 
        [HttpGet("GetEpContractTypeList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEpContractTypeList()
        {
            try
            {
                var result = await _ILookups.GetEpContractTypeList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Delete EP Contract Type 

        [HttpPost("DeleteEpContractType")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteEpContractType(EpContractTypeVM epContractTypeVM)
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
                var result = await _ILookups.DeleteEpContractType(epContractTypeVM);

                return Ok(epContractTypeVM);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion


        #region  Get Ep Contract Type  Details by Id 
        [HttpGet("GetEpContractTypeById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEpContractTypeById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetEpContractTypeById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Save  Ep  Contract Type 
        [HttpPost("SaveContractType")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveContractType(ContractType contractType)
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
                await _ILookups.SaveContractType(contractType);
                return Ok(contractType);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Update Ep Conract Type   

        [HttpPost("UpdateContractType")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateContractType(ContractType contractType)
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
                var result = await _ILookups.UpdateContractType(contractType);

                return Ok(contractType);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #endregion

        #region EP Grade Type CRUD 
        #region Get EP Grade Type List 
        [HttpGet("GetEpGradeTypeList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEpGradeTypeList()
        {
            try
            {
                var result = await _ILookups.GetEpGradeTypeList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Delete EP Grade Type

        [HttpPost("DeleteEpGradeType")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteEpGradeType(EpGradeTypeVM EPGradeType)
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
                var result = await _ILookups.DeleteEpGradeType(EPGradeType);
                //if (result != null)
                //{
                //    
                //    _client.SendMessage(result, GovEntityPatternKey);
                //}

                return Ok(EPGradeType);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region  Get Ep Grade Type Details by Id 
        [HttpGet("GetEpGradeTypeById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEpGradeTypeById(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetEpGradeTypeById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Save  Ep Grade Type
        [HttpPost("SaveGradeType")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveGradeType(GradeType EPGradeType)
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
                await _ILookups.SaveGradeType(EPGradeType);
                //if(result != null)
                //{
                //    
                //    _client.SendMessage(result, SaveDepartmentKey);
                //}

                return Ok(EPGradeType);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Update Ep Grade Type

        [HttpPost("UpdateGradeType")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateGradeType(GradeType EPGradeType)
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
                var result = await _ILookups.UpdateGradeType(EPGradeType);
                //if (result != null)
                //{
                //    
                //    _client.SendMessage(result, UpdateDepartmentKey);
                //    //_client.SendMessage(result, GovEntityPatternKey);
                //}

                return Ok(EPGradeType);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #endregion

        #region  Book Author CRUD 
        #region Get Book Author List 
        [HttpGet("GetBookAuthorList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetBookAuthorList()
        {
            try
            {
                var result = await _ILookups.GetBookAuthorList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Delete Book Author 

        [HttpPost("DeleteBookAuthor")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteBookAuthor(BookAuthorVM bookAuthorVM)
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
                var result = await _ILookups.DeleteBookAuthor(bookAuthorVM);

                return Ok(bookAuthorVM);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region  Get Book Author Details by Id 
        [HttpGet("GetBookAuthorById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetBookAuthorById(int AuthorId)
        {
            try
            {
                return Ok(await _ILookups.GetBookAuthorById(AuthorId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Save Book Author  
        [HttpPost("SaveBookAuthor")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveBookAuthor(LmsLiteratureAuthor lmsLiteratureAuthor)
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
                await _ILookups.SaveBookAuthor(lmsLiteratureAuthor);
                return Ok(lmsLiteratureAuthor);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Update Book Author   

        [HttpPost("UpdateBookAuthor")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateBookAuthor(LmsLiteratureAuthor lmsLiteratureAuthor)
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
                var result = await _ILookups.UpdateBookAuthor(lmsLiteratureAuthor);

                return Ok(lmsLiteratureAuthor);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #endregion

        #region  G2G Correspondences Receiver   
        #region Get G2G Correspondences Receiver List 
        [HttpGet("GetG2GCorrespondencesReceiverList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetG2GCorrespondencesReceiverList()
        {
            try
            {
                var result = await _ILookups.GetG2GCorrespondencesReceiverList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Delete G2G Correspondences Receiver

        [HttpPost("DeleteG2GCorrespondencesReceiver")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteG2GCorrespondencesReceiver(CmsSectorTypeGEDepartmentVM cmsSectorTypeGEDepartment)
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
                var result = await _ILookups.DeleteG2GCorrespondencesReceiver(cmsSectorTypeGEDepartment);

                return Ok(cmsSectorTypeGEDepartment);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region  Get G2G Correspondences Receiver by Id 
        [HttpPost("GetDepartmentByGEEntityId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetDepartmentByGEEntityId([FromBody] List<int> EntityIds)
        {
            try
            {
                return Ok(await _ILookups.GetDepartmentByGEEntityId(EntityIds));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Save G2G Correspondences Receiver  
        [HttpPost("SaveG2GCorrespondencesReceiver")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveG2GCorrespondencesReceiver(CmsSectorTypeGEDepartment cmsSectorTypeGEDepartment)
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
                await _ILookups.SaveG2GCorrespondencesReceiver(cmsSectorTypeGEDepartment);
                return Ok(cmsSectorTypeGEDepartment);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #endregion

        #region  Literature Dewey Number Patterns
        #region Get Literature Dewey Number Patterns List 
        [HttpGet("GetLiteratureDeweyNumberPatternsList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLiteratureDeweyNumberPatternList()
        {
            try
            {
                var result = await _ILookups.GetLiteratureDeweyNumberPatternsList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Delete  Literature Dewey Number Pattern

        [HttpPost("DeleteLiteratureDeweyNumberPattern")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteLiteratureDeweyNumberPattern(LiteratureDeweyNumberPatternVM literatureDeweyNumberPatternVM)
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
                var result = await _ILookups.DeleteLiteratureDeweyNumberPattern(literatureDeweyNumberPatternVM);

                return Ok(literatureDeweyNumberPatternVM);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region  Get Literature Dewey  Number Pattern Details by Id 
        [HttpGet("GetLiteratureDeweyNumberPatternById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLiteratureDeweyNumberPatternById(Guid Id)
        {
            try
            {
                return Ok(await _ILookups.GetLiteratureDeweyNumberPatternById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Save literature Dewey Number Pattern  
        [HttpPost("SaveLiteratureDeweyNumberPattern")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveLiteratureDeweyNumberPattern(LiteratureDeweyNumberPattern literatureDeweyNumberPattern)
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
                await _ILookups.SaveLiteratureDeweyNumberPattern(literatureDeweyNumberPattern);
                return Ok(literatureDeweyNumberPattern);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Update Literature Dewey Number Pattern  

        [HttpPost("UpdateLiteratureDeweyNumberPattern")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateLiteratureDeweyNumberPattern(LiteratureDeweyNumberPattern literatureDeweyNumberPattern)
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
                var result = await _ILookups.UpdateLiteratureDeweyNumberPattern(literatureDeweyNumberPattern);

                return Ok(literatureDeweyNumberPattern);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #endregion

        #region Check Name En and Name Ar Already Exists 
        [HttpGet("CheckNameEnExists")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CheckNameEnExists(string NameEn, int requestTypeId, int subTypeId)
        {
            try
            {
                bool result = await _ILookups.CheckNameEnExists(NameEn, requestTypeId, subTypeId);
                if (result != false)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("CheckNameArExists")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CheckNameArExists(string NameAr, int requestTypeId, int subTypeId)
        {
            try
            {
                bool result = await _ILookups.CheckNameArExists(NameAr, requestTypeId, subTypeId);
                if (result != false)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion

        #region Get Operating Sector Types By Department Id
        [HttpGet("GetOperatingSectorsByDepartmentId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem Abbas' Date='2024-07-10' Version="1.0" Branch="master"> Get Operating Sector Types By Department Id</History>
        public async Task<IActionResult> GetOperatingSectorsByDepartmentId(int DepartmentId)
        {

            try
            {
                return Ok(await _ILookups.GetOperatingSectorsByDepartmentId(DepartmentId));

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }


        }
        #endregion

        #region Weekdays Settings
        [HttpGet(nameof(GetWeekdaysSettings))]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetWeekdaysSettings()
        {

            try
            {
                return Ok(await _ILookups.GetWeekdaysSettings());

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }


        }
        #endregion

        #region Get Users List By Sector Type Id and Role Id
        [HttpGet("GetUsersListBySectorIdAndRoleId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUsersListBySectorIdAndRoleId(string RoleId, int SectorTypeId)
        {
            try
            {
                var result = await _ILookups.GetUsersListBySectorIdAndRoleId(RoleId, SectorTypeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Government Entity Representatives List
        //<History Author = 'Ammaar Naveed' Date='2024-09-03' Version="1.0" Branch="master">Get Government Entity Representatives By Govt Entity EntityId</History>
        [HttpPost("GetGovernmentEntityRepresentatives")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGovernmentEntityRepresentatives(GovtEntityIdsPayload govtEntityIds)
        {
            try
            {
                return Ok(await _ILookups.GetGovernmentEntityRepresentatives(govtEntityIds));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Courts, Chambers, Chamber Numbers for Mobile App

        [HttpGet("GetCourtByUserIdForMobileApp")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCourtByUserIdForMobileApp(string userId)
        {
            try
            {
                var result = await _ILookups.GetCourtByUserIdForMobileApp(userId);
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



        [HttpGet("GetChambersByUserIdForMobileApp")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetChambersByUserIdForMobileApp(int courtId, string userId)
        {
            try
            {
                var result = await _ILookups.GetChambersByUserIdForMobileApp(courtId, userId);
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

        [HttpGet("GetChamberNumberByUserIdForMobileApp")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetChamberNumberByUserIdForMobileApp(int courtId, int chamberId, string userId)
        {
            try
            {
                var result = await _ILookups.GetChamberNumberByUserIdForMobileApp(courtId, chamberId, userId);
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
        #region Get Stock Taking Status
        [HttpGet("GetStockTakingStatus")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetStockTakingStatus()
        {
            try
            {
                return Ok(await _ILookups.GetStockTakingStatus());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Sector Roles Crud

        [HttpGet("GetRolesBySectorTypeIds")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetRolesBySectorTypeIds(List<int> sectorIds)
        {
            try
            {
                return Ok(await _ILookups.GetRolesBySectorIds(sectorIds));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet(nameof(GetSectorRolesBySectorId))]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSectorRolesBySectorId(int sectorId)
        {
            try
            {
                var respone = await _ILookups.GetRolesOfSectorBySectorId(sectorId);
                return respone is not null ? Ok(respone) : BadRequest(respone);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion 

        #region Service Request Approval ( CRUD )

        [HttpPost("AddServiceRequestFinalApproval")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddServiceRequestFinalApproval(ServiceRequestFinalApprovalVM serviceRequestFinalApproval)
        {
            try
            {
                return Ok(await _ILookups.AddServiceRequestApproval(serviceRequestFinalApproval));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetAllServiceRequestApprovalList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllServiceRequestApprovalList()
        {
            try
            {
                return Ok(await _ILookups.GetAllServiceRequestApprovalList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetServiceRequestApprovalDetail")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetServiceRequestApprovalDetail(int Id)
        {
            try
            {
                return Ok(await _ILookups.GetServiceRequestApprovalDetail(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPut("UpdateServiceRequestApproval")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateServiceRequestApproval(ServiceRequestFinalApprovalVM serviceRequestFinalApproval)
        {
            try
            {
                return Ok(await _ILookups.UpdateServiceRequestApproval(serviceRequestFinalApproval));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetServiceRequestApprovalHistory")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetServiceRequestApprovalHistory(int approvalId)
        {
            try
            {
                return Ok(await _ILookups.GetServiceRequestApprovalHistory(approvalId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion


        [HttpGet("GetPreCourtTypes")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetPreCourtTypes()
        {
            try
            {
                return Ok(await _ILookups.GetPreCourtTypes());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetCourtTypesByRequestType")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCourtTypesByRequestType(int requestTypeId)
        {
            try
            {
                return Ok(await _ILookups.GetCourtTypesByRequestType(requestTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #region Get Consultation Legislation File Types
        [HttpGet("GetConsultationLegislationFileTypes")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2024-10-10' Version="1.0" Branch="master">Get consultation legislation file type Details</History>
        public async Task<IActionResult> GetConsultationLegislationFileTypes()
        {
            try
            {
                return Ok(await _ILookups.GetConsultationLegislationFileTypes());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        [HttpGet("GetConsultationInternationalArbitrationTypes")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Umer Zaman' Date='2023-01-03' Version="1.0" Branch="master">Get type Details</History>
        public async Task<IActionResult> GetConsultationInternationalArbitrationTypes()
        {
            try
            {
                return Ok(await _ILookups.GetConsultationInternationalArbitrationTypes());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #region Get Manager Task Reminder Data

        [HttpGet("GetManagerTaskReminderData")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2024-01-12' Version="1.0" Branch="master">Get Manager Task Reminder Data</History>
        public async Task<IActionResult> GetManagerTaskReminderData(Guid TaskId)
        {
            try
            {
                return Ok(await _ILookups.GetManagerTaskReminderData(TaskId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
    }
}
