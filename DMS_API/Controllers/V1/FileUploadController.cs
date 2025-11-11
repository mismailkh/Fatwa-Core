using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;
using FATWA_DOMAIN.Models.MeetModels;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using FATWA_GENERAL.Helper;
using System.Net;
using FATWA_DOMAIN.Models.Lms;
using FATWA_DOMAIN.Models.ArchivedCasesModels;
using static FATWA_GENERAL.Helper.Permissions;

namespace DMS_API.Controllers.V1
{
    //<History Author = 'Hassan Abbas' Date='2022-04-11' Version="1.0" Branch="master"> Controller for File Upload Component</History>
    //<History Author = 'Zain Ul Islam' Date='2023-01-25' Version="1.0" Branch="master"> Controller moved to DMS API</History> 
    [ApiVersion("1.1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + ",DmsApiKey")]
    public class FileUploadController : ControllerBase
    {
        private readonly ITempFileUpload _IFileUpload;
        private readonly IConfiguration _Config;
        private readonly ILookups _ILookups;
        private readonly IAuditLog _auditLogs;

        public FileUploadController(ITempFileUpload iFileUpload, IConfiguration config, ILookups lookups, IAuditLog auditLog)
        {
            _IFileUpload = iFileUpload;
            _Config = config;
            _ILookups = lookups;
            _auditLogs = auditLog;
        }

        #region GET

        //<History Author = 'Zain Ul Islam' Date='2023-01-25' Version="1.0" Branch="master"> Function to get Attachment Types</History> 
        [HttpGet("GetAttachmentTypes")]
        //[MapToApiVersion("1.1")]
        public async Task<IActionResult> GetAttachmentTypes(int? moduleId, int? sectorTypeId, bool showHidden = false)
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

                return Ok(await _IFileUpload.GetAttachmentTypes(moduleId, sectorTypeId, showHidden));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        [HttpGet("GetAttachmentTypeDetailById")]
        //[MapToApiVersion("1.1")]
        public async Task<IActionResult> GetAttachmentTypeDetailById(int? AttachmentTypeId)
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

                return Ok(await _IFileUpload.GetAttachmentTypeDetailById(AttachmentTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }


        //<History Author = 'Hassan Abbas' Date='2023-06-15' Version="1.0" Branch="master"> Get Document Classifications</History> 
        [HttpGet("GetDocumentClassifications")]
        //[MapToApiVersion("1.1")]
        public async Task<IActionResult> GetDocumentClassifications()
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

                var result = await _IFileUpload.GetDocumentClassifications();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        //<History Author = 'Zain Ul Islam' Date='2023-01-25' Version="1.0" Branch="master"> Function for Getting Attachments List</History>
        [MapToApiVersion("1.1")]
        [HttpPost("GetUploadedAttachements")]
        public async Task<IActionResult> GetUploadedAttachements([FromForm] bool _isLiterature, [FromForm] int _literatureId, [FromForm] Guid _guid)
        {
            try
            {
                if (_isLiterature && _literatureId > 0)
                {
                    var result = await _IFileUpload.GetUploadedAttachementsByLiteratureId(_literatureId);
                    if (result is not null)
                        return Ok(result);
                    return BadRequest();
                }
                else
                {
                    var result = await _IFileUpload.GetUploadedAttachementsByReferenceGuid(_guid);
                    if (result is not null)
                        return Ok(result);
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        //<History Author = 'Zain Ul Islam' Date='2023-01-25' Version="1.0" Branch="master"> Function for getting official letters list</History>
        [MapToApiVersion("1.1")]
        [HttpPost("GetOfficialDocuments")]
        public async Task<IActionResult> GetOfficialDocuments([FromForm] Guid _guid)
        {
            try
            {
                var result = await _IFileUpload.GetOfficialDocuments(_guid);
                if (result is not null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        //<History Author = 'Zain Ul Islam' Date='2023-01-25' Version="1.0" Branch="master"> Function for getting Temp Attachements List</History>
        [MapToApiVersion("1.1")]
        [HttpPost("GetTempAttachements")]
        public async Task<IActionResult> GetTempAttachements([FromForm] Guid _guid, [FromForm] int _attachementId = 0)
        {
            try
            {
                var result = await _IFileUpload.GetTempAttachementsByReferenceGuid(_guid, _attachementId);
                if (result is not null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        //<History Author = 'Zain Ul Islam' Date='2023-01-25' Version="1.0" Branch="master"> Function for getting Temp Attachements List</History>
        [MapToApiVersion("1.1")]
        [HttpPost("GetTempAndUploadAttachementsLegislation")]
        public async Task<IActionResult> GetTempAndUploadAttachementsLegislation([FromForm] Guid _guid)
        {
            try
            {
                var tempAttachments = await _IFileUpload.GetTempAttachementsByReferenceGuid(_guid);
                if (tempAttachments != null && tempAttachments.Count > 0)
                {
                    return Ok(tempAttachments);
                }
                else
                {
                    var uploadedAttachments = await _IFileUpload.GetUploadedAttachementsByReferenceGuid(_guid);
                    if (uploadedAttachments != null && uploadedAttachments.Count > 0)
                    {
                        return Ok(uploadedAttachments);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //<History Author = 'Zain Ul Islam' Date='2023-01-25' Version="1.0" Branch="master"> Function for getting Temp Attachements List</History>
        [MapToApiVersion("1.1")]
        [HttpPost("CheckNewAttachmentInUpdateCase")]
        public async Task<IActionResult> CheckNewAttachmentInUpdateCase([FromForm] Guid _guid)
        {


            try
            {
                var result = await _IFileUpload.GetTempAttachementsByReferenceGuid(_guid);
                if (result is not null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        //<History Author = 'Zain Ul Islam' Date='2023-01-25' Version="1.0" Branch="master"> Function for Getting Attachements List By Reference Guid</History>
        [MapToApiVersion("1.1")]
        [HttpPost("GetAttachementsByReferenceGuid")]
        public async Task<IActionResult> GetAttachementsByReferenceGuid(Guid _guid)
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

                var result = await _IFileUpload.GetUploadedAttachementsByReferenceGuid(_guid);
                if (result is not null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        //<History Author = 'Zain Ul Islam' Date='2023-02-15' Version="1.0" Branch="master"> Function to Get Attachment Types</History> 
        [MapToApiVersion("1.1")]
        [HttpGet("GetAllAttachmentTypes")]
        public async Task<IActionResult> GetAllAttachmentTypes()
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

                var result = await _IFileUpload.GetAllAttachmentTypes();
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

        //<History Author = 'Nadia Gull' Date='2023-04-14' Version="1.0" Branch="master"> Get Uploaded Attachement By Id </History>
        [MapToApiVersion("1.1")]
        [HttpPost("GetUploadedAttachementById")]
        public async Task<IActionResult> GetUploadedAttachementById([FromForm] int Id, [FromForm] Guid? _referenceGuid = null, [FromForm] int _literatureId = 0)
        {
            try
            {
                var result = await _IFileUpload.GetUploadedAttachementById(Id, _referenceGuid);
                if (result is not null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        #endregion

        #region SAVE

        //<History Author = 'Hassan Abbas' Date='2023-01-25' Version="1.0" Branch="master"> Function for FILE upload with Guid and Username</History>
        //<History Author = 'Hassan Abbas' Date='2023-06-24' Version="1.0" Branch="master"> Document Encryption</History>
        [MapToApiVersion("1.1")]
        [HttpPost("Upload")]
        [EnableCors("FileUploadPolicies")]
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Upload(IEnumerable<IFormFile> files, [FromForm] Guid _pEntityIdentifierGuid, [FromForm] Guid? _pCommunicationGuid, [FromForm] string _userName, [FromForm] int _typeId,
            [FromForm] string _uploadFrom, [FromForm] string _project, [FromForm] string? _otherAttachmentType, [FromForm] string? _description,
            [FromForm] string? _referenceNo, [FromForm] DateTime? _referenceDate, [FromForm] DateTime? _documentDate, [FromForm] string? _FileNumber, [FromForm] string? _FileTitle)
        {
            try
            {
                if (files != null)
                {
                    var fileName = string.Empty;
                    var physicalPath = string.Empty;
                    TempAttachement attachements = new();
                    string filePath = $"\\wwwroot\\Attachments\\{_uploadFrom}\\";
                    if (_typeId == (int)AttachmentTypeEnum.SignatureImage)
                    {
                        filePath = filePath + "Signature";
                    }
                    string basePath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
                    basePath = basePath.Replace("DMS_API", "DMS_WEB");
                    bool basePathExists = Directory.Exists(basePath);
                    if (!basePathExists)
                        Directory.CreateDirectory(basePath);

                    foreach (var file in files)
                    {
                        fileName = $"{(_FileTitle != null ? _FileTitle : Path.GetFileNameWithoutExtension(file.FileName))}{"_"}{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{Path.GetExtension(file.FileName)}";
                        physicalPath = Path.Combine(basePath, fileName);

                        if (_project == "FATWA_WEB" || _project == "G2G_WEB" || _project == "FATWA_ADMIN" || _project == "G2G_ADMIN")
                        {
                            string password = _Config.GetValue<string>("DocumentEncryptionKey");
                            UnicodeEncoding UE = new UnicodeEncoding();
                            byte[] key = UE.GetBytes(password);

                            FileStream fsCrypt = new FileStream(physicalPath, FileMode.Create);

                            RijndaelManaged RMCrypto = new RijndaelManaged();

                            CryptoStream cs = new CryptoStream(fsCrypt,
                                RMCrypto.CreateEncryptor(key, key),
                            CryptoStreamMode.Write);

                            Stream streams = file.OpenReadStream();
                            FileStream fsIn = streams as FileStream;

                            int data;
                            while ((data = streams.ReadByte()) != -1)
                                cs.WriteByte((byte)data);


                            streams.Close();
                            cs.Close();
                            fsCrypt.Close();
                        }
                        else
                        {
                            using (var fileStream = new FileStream(physicalPath, FileMode.Create))
                            {
                                await file.OpenReadStream().CopyToAsync(fileStream);
                            }
                        }

                        physicalPath = Path.Combine(filePath, fileName);
                        TempAttachement ObjFill = new TempAttachement();
                        {
                            ObjFill.StoragePath = physicalPath;
                            ObjFill.FileName = fileName;
                            ObjFill.FileNameWithoutTimeStamp = string.IsNullOrEmpty(_FileTitle) ? file.FileName : _FileTitle + Path.GetExtension(file.FileName);
                            ObjFill.Guid = _pEntityIdentifierGuid;
                            ObjFill.CommunicationGuid = _pCommunicationGuid;
                            ObjFill.AttachmentTypeId = _typeId;
                            ObjFill.UploadedBy = _userName;
                            ObjFill.UploadedDate = DateTime.Now;
                            ObjFill.DocType = Path.GetExtension(file.FileName);
                            if (_uploadFrom == "Principle" || _uploadFrom == "Literature")
                            {
                                ObjFill.Description = _uploadFrom;
                            }
                            ObjFill.OtherAttachmentType = _otherAttachmentType;
                            ObjFill.Description = _description;
                            ObjFill.ReferenceNo = _referenceNo;
                            ObjFill.ReferenceDate = _referenceDate;
                            ObjFill.DocumentDate = _documentDate;
                            ObjFill.FileNumber = _FileNumber;
                            ObjFill.FileTitle = _FileTitle;
                        }
                        attachements = await _IFileUpload.CreateTempAttachement(ObjFill);
                    }
                    return Ok(JsonConvert.SerializeObject(value: new FileUploadSuccessResponse { StoragePath = physicalPath, AttachementId = attachements.AttachementId }));
                }
                else
                {
                    return new BadRequestResult();
                }
            }
            catch
            {
                return new BadRequestResult();
                throw;
            }
        }


        //<History Author = 'Hassan Abbas' Date='2023-01-25' Version="1.0" Branch="master"> Function for FILE upload with Guid and Username</History>
        [MapToApiVersion("1.1")]
        [HttpPost("UploadAddedDocument")]
        [EnableCors("FileUploadPolicies")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UploadAddedDocument(IEnumerable<IFormFile> files, [FromForm] string _uploadFrom)
        {
            try
            {
                if (files != null)
                {
                    DmsAddedDocumentVersion addedDocumentVersion = new DmsAddedDocumentVersion();
                    var fileName = string.Empty;
                    var physicalPath = string.Empty;
                    string filePath = $"\\wwwroot\\Attachments\\{_uploadFrom}\\";
                    string basePath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
                    basePath = basePath.Replace("DMS_API", "DMS_WEB");
                    bool basePathExists = Directory.Exists(basePath);
                    if (!basePathExists)
                        Directory.CreateDirectory(basePath);

                    foreach (var file in files)
                    {
                        fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}{"_"}{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{Path.GetExtension(file.FileName)}";
                        physicalPath = Path.Combine(basePath, fileName);

                        string password = _Config.GetValue<string>("DocumentEncryptionKey");
                        UnicodeEncoding UE = new UnicodeEncoding();
                        byte[] key = UE.GetBytes(password);

                        FileStream fsCrypt = new FileStream(physicalPath, FileMode.Create);

                        RijndaelManaged RMCrypto = new RijndaelManaged();

                        CryptoStream cs = new CryptoStream(fsCrypt,
                            RMCrypto.CreateEncryptor(key, key),
                        CryptoStreamMode.Write);

                        Stream streams = file.OpenReadStream();
                        FileStream fsIn = streams as FileStream;

                        int data;
                        while ((data = streams.ReadByte()) != -1)
                            cs.WriteByte((byte)data);


                        streams.Close();
                        cs.Close();
                        fsCrypt.Close();

                        physicalPath = Path.Combine(filePath, fileName);
                        addedDocumentVersion.DocType = Path.GetExtension(file.FileName);
                        addedDocumentVersion.StoragePath = physicalPath;
                        addedDocumentVersion.FileName = fileName;
                    }
                    return Ok(JsonConvert.SerializeObject(addedDocumentVersion));
                }
                else
                {
                    return new BadRequestResult();
                }
            }
            catch (Exception ex)
            {
                return new BadRequestResult();
            }
        }

        //<History Author = 'Zain Ul Islam' Date='2023-01-25' Version="1.0" Branch="master"> Function for SINGLE FILE upload with Guid and Username</History> 
        //<History Author = 'Hassan Abbas' Date='2023-06-24' Version="1.0" Branch="master"> Added Document Encryption</History>
        [MapToApiVersion("1.1")]
        [HttpPost("SingleUpload")]
        [EnableCors("FileUploadPolicies")]
        [IgnoreAntiforgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> SingleUpload(IEnumerable<IFormFile> files, [FromForm] Guid _pEntityIdentifierGuid, [FromForm] string _userName,
            [FromForm] string _uploadFrom, [FromForm] string? _oldImagePath, [FromForm] string? _FileNumber, [FromForm] string? _FileTitle,
            [FromForm] int _attachmentType)
        {
            try
            {
                if (files != null)
                {
                    string filePath = $"\\wwwroot\\Attachments\\{_uploadFrom}\\";
                    string basePath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
                    basePath = basePath.Replace("DMS_API", "DMS_WEB");
                    bool basePathExists = Directory.Exists(basePath);
                    if (!basePathExists)
                        Directory.CreateDirectory(basePath);

                    foreach (var file in files)
                    {
                        string fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}{"_"}{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{Path.GetExtension(file.FileName)}";
                        string physicalPath = Path.Combine(basePath, fileName);
                        if (_oldImagePath != null && _oldImagePath != "null")
                        {
                            string oldImageBasePath = Path.Combine(Directory.GetCurrentDirectory() + _oldImagePath);
                            oldImageBasePath = oldImageBasePath.Replace("DMS_API", "DMS_WEB");

                            if (System.IO.File.Exists(oldImageBasePath))
                            {
                                System.IO.File.Delete(oldImageBasePath);
                            }
                        }

                        string password = _Config.GetValue<string>("DocumentEncryptionKey");
                        UnicodeEncoding UE = new UnicodeEncoding();
                        byte[] key = UE.GetBytes(password);

                        FileStream fsCrypt = new FileStream(physicalPath, FileMode.Create);

                        RijndaelManaged RMCrypto = new RijndaelManaged();

                        CryptoStream cs = new CryptoStream(fsCrypt,
                            RMCrypto.CreateEncryptor(key, key),
                        CryptoStreamMode.Write);

                        Stream streams = file.OpenReadStream();
                        FileStream fsIn = streams as FileStream;

                        int data;
                        while ((data = streams.ReadByte()) != -1)
                            cs.WriteByte((byte)data);


                        streams.Close();
                        cs.Close();
                        fsCrypt.Close();

                        physicalPath = Path.Combine(filePath, fileName);
                        var tempAttachment = new TempAttachement();
                        if (_attachmentType > 0)
                        {
                            tempAttachment.StoragePath = physicalPath;
                            tempAttachment.FileName = fileName;
                            tempAttachment.FileNameWithoutTimeStamp = file.FileName;
                            tempAttachment.Guid = _pEntityIdentifierGuid;
                            tempAttachment.AttachmentTypeId = _attachmentType;
                            tempAttachment.FileTitle = _FileTitle;
                            tempAttachment.FileNumber = _FileNumber;
                            tempAttachment.UploadedBy = _userName;
                            tempAttachment.UploadedDate = DateTime.Now;
                            tempAttachment.DocType = Path.GetExtension(file.FileName);
                            if (_uploadFrom == "Legislation" || _uploadFrom == "LegislationExplanatoryNote")
                            {
                                tempAttachment.Description = _uploadFrom;
                            }
                        }
                        else
                        {
                            tempAttachment.StoragePath = physicalPath;
                            tempAttachment.FileName = fileName;
                            tempAttachment.FileNameWithoutTimeStamp = file.FileName;
                            tempAttachment.Guid = _pEntityIdentifierGuid;
                            tempAttachment.AttachmentTypeId = 6;
                            tempAttachment.FileTitle = _FileTitle;
                            tempAttachment.FileNumber = _FileNumber;
                            tempAttachment.UploadedBy = _userName;
                            tempAttachment.UploadedDate = DateTime.Now;
                            tempAttachment.DocType = Path.GetExtension(file.FileName);
                            if (_uploadFrom == "Legislation" || _uploadFrom == "LegislationExplanatoryNote")
                            {
                                tempAttachment.Description = _uploadFrom;
                            }
                        }

                        TempAttachement attachement = await _IFileUpload.CreateTempAttachement(tempAttachment);
                    }
                }
            }
            catch
            {
                return new BadRequestResult();
                throw;
            }
            return new OkResult();
        }

        //<History Author = 'Zain Ul Islam' Date='2023-01-25' Version="1.0" Branch="master"> Upload Temp Attachment to Uploaded Document</History>
        [MapToApiVersion("1.1")]
        [HttpPost("UploadTempAttachmentToUploadedDocument")]
        public async Task<IActionResult> UploadTempAttachmentToUploadedDocument(Guid referenceId, string createdBy)
        {
            try
            {
                await _IFileUpload.UploadTempAttachmentToUploadedDocument(referenceId, createdBy);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        #endregion[FromForm]

        #region Delete

        //<History Author = 'Zain Ul Islam' Date='2023-01-25' Version="1.0" Branch="master"> Function for removing temp file</History>  
        [MapToApiVersion("1.1")]
        [HttpPost("Remove")]
        [EnableCors("FileUploadPolicies")]
        public async Task<IActionResult> Remove([FromForm] string _fileToRemove, [FromForm] string _userName, [FromForm] string _uploadFrom, [FromForm] string _project, [FromForm] string _typeId)
        {
            try
            {
                if (_fileToRemove != null)
                {
                    var attachements = await _IFileUpload.GetTempAttachementsByFileAndUserName(_fileToRemove, _userName, Convert.ToInt32(_typeId));
                    foreach (var file in attachements)
                    {
                        string filePath = $"\\wwwroot\\Attachments\\{_uploadFrom}\\";
                        string basePath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
                        basePath = basePath.Replace("DMS_API", "DMS_WEB");

                        var physicalPath = Path.Combine(basePath, file.FileName);

#if DEBUG
                        {
                            if (System.IO.File.Exists(physicalPath))
                            {
                                System.IO.File.Delete(physicalPath);
                                await _IFileUpload.DeleteTempAttachement(file.AttachementId);
                            }
                        }
#else
                    {
							
							await _IFileUpload.DeleteTempAttachement(file.AttachementId);
                    }
#endif
                    }
                }
                return new EmptyResult();
            }
            catch
            {
                return new BadRequestResult();
                throw;
            }
        }

        #endregion

        #region ZAIN CHANGES

        //<History Author = 'Zain Ul Islam' Date='2023-01-25' Version="1.0" Branch="master"> Function for saving temp file</History>  
        [MapToApiVersion("1.1")]
        [HttpPost("SaveTempAttachementToUploadedDocument")]
        [EnableCors("FileUploadPolicies")]
        public async Task<IActionResult> SaveTempAttachmentToUploadedDocument(FileUploadVM item)
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
                var result = await _IFileUpload.SaveTempAttachmentToUploadedDocument(item);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for save temporary attachment",
                    Task = "To submit the request",
                    Description = "User able to save temporary attachment Request successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                if (result)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for save temporary attachment Failed",
                    Body = ex.Message,
                    Category = "User unable to save temporary attachment Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for save temporary attachment Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(ex.Message);
            }
        }

        //<History Author = 'Zain Ul Islam' Date='2023-01-25' Version="1.0" Branch="master"> Function for saving temp file</History>  
        [MapToApiVersion("1.1")]
        [HttpPost("CopyAttachmentsFromSourceToDestination")]
        [EnableCors("FileUploadPolicies")]
        public async Task<IActionResult> CopyAttachmentsFromSourceToDestination(List<CopyAttachmentVM> copyAttachments)
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
                var result = await _IFileUpload.CopyAttachmentsFromSourceToDestination(copyAttachments);
                if (result)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //<History Author = 'Ijaz Ahmad' Date='2023-04-14' Version="1.0" Branch="master"> Function for saving temp file</History>  
        [MapToApiVersion("1.1")]
        [HttpPost("UpdateExistingDocument")]
        [EnableCors("FileUploadPolicies")]
        public async Task<IActionResult> UpdateExistingDocument(List<CopyAttachmentVM> copyAttachments)
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
                var result = await _IFileUpload.UpdateExistingDocument(copyAttachments);
                if (result)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-04-05' Version="1.0" Branch="master"> Copy Selected Attachments to Destination</History>  
        [MapToApiVersion("1.1")]
        [HttpPost("CopySelectedAttachmentsToDestination")]
        [EnableCors("FileUploadPolicies")]
        public async Task<IActionResult> CopySelectedAttachmentsToDestination(CopySelectedAttachmentsVM copyAttachments)
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
                var result = await _IFileUpload.CopySelectedAttachmentsToDestination(copyAttachments);
                if (result)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-06-24' Version="1.0" Branch="master"> Copy Selected Attachments to Destination</History>  
        [MapToApiVersion("1.1")]
        [HttpPost("MoveAttachmentToAddedDocumentVersion")]
        [EnableCors("FileUploadPolicies")]
        public async Task<IActionResult> MoveAttachmentToAddedDocumentVersion(MoveAttachmentAddedDocumentVM attachmentDetail)
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
                await _IFileUpload.MoveAttachmentToAddedDocumentVersion(attachmentDetail);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //<History Author = 'Zain Ul Islam' Date='2023-01-25' Version="1.0" Branch="master"> Function for saving temp file</History>  
        [MapToApiVersion("1.1")]
        [HttpPost("SaveDraftTemplateToDocument")]
        [EnableCors("FileUploadPolicies")]
        public async Task<IActionResult> SaveDraftTemplateToDocument(CmsDraftedTemplate draft)
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
                CaseTemplate caseTemplate = await _IFileUpload.GetCaseTemplateDetail(draft.TemplateId);
                if (caseTemplate.IsG2GStamp)
                {
                    draft.FileData = await AddG2GStampToPdfByteArray(draft.FileData);
                }
                if (caseTemplate.IsTimeStamp)
                {
                    draft.FileData = await AddTimeStampToPdfByteArray(draft.FileData, draft.CreatedBy);
                }
                var physicalPath = string.Empty;
                string filePath = "\\wwwroot\\Attachments\\" + draft.UploadFrom + "\\";
                string basePath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
                basePath = basePath.Replace("DMS_API", "DMS_WEB");
                bool basePathExists = Directory.Exists(basePath);
                if (!basePathExists)
                    Directory.CreateDirectory(basePath);

                var fileName = $"{Path.GetFileNameWithoutExtension(draft.Name)}{"_"}{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{".pdf"}";
                physicalPath = Path.Combine(basePath, fileName);

                string password = _Config.GetValue<string>("DocumentEncryptionKey");
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(physicalPath, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                CryptoStreamMode.Write);

                Stream streams = new MemoryStream(draft.FileData);

                int data;
                while ((data = streams.ReadByte()) != -1)
                    cs.WriteByte((byte)data);

                streams.Close();
                cs.Close();
                fsCrypt.Close();

                physicalPath = Path.Combine(filePath, fileName);

                var result = await _IFileUpload.SaveDraftTemplateToDocument(draft, fileName, physicalPath);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for save draft template",
                    Task = "To submit the request",
                    Description = "User able to save draft template Request successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for save draft template Failed",
                    Body = ex.Message,
                    Category = "User unable to save draft template Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for save draft template Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(ex.Message);
            }
        }
        //<History Author = 'Ijaz Ahmad' Date='2023-03-14' Version="1.0" Branch="master">  Save Consultaiton Draft Template to Document</History>  
        [MapToApiVersion("1.1")]
        [HttpPost("SaveComsDraftTemplateToDocument")]
        [EnableCors("FileUploadPolicies")]
        public async Task<IActionResult> SaveComsDraftTemplateToDocument(ComsDraftedTemplate draft)
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

                var physicalPath = string.Empty;
                string filePath = "\\wwwroot\\Attachments\\" + draft.UploadFrom + "\\";
                string basePath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
                basePath = basePath.Replace("DMS_API", "DMS_WEB");
                bool basePathExists = Directory.Exists(basePath);
                if (!basePathExists)
                    Directory.CreateDirectory(basePath);

                var fileName = $"{Path.GetFileNameWithoutExtension(draft.Name)}{"_"}{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{".pdf"}";
                physicalPath = Path.Combine(basePath, fileName);

                string password = _Config.GetValue<string>("DocumentEncryptionKey");
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(physicalPath, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                CryptoStreamMode.Write);

                Stream streams = new MemoryStream(draft.FileData);

                int data;
                while ((data = streams.ReadByte()) != -1)
                    cs.WriteByte((byte)data);

                streams.Close();
                cs.Close();
                fsCrypt.Close();
                physicalPath = Path.Combine(filePath, fileName);

                var result = await _IFileUpload.SaveComsDraftTemplateToDocument(draft, fileName, physicalPath);
                if (result)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #region remove File
        //<History Author = 'Zain Ul Islam' Date='2023-01-25' Version="1.0" Branch="master"> Function for removing file</History>       
        [MapToApiVersion("1.1")]
        [HttpPost("RemoveDocument")]
        [EnableCors("FileUploadPolicies")]
        public async Task<IActionResult> RemoveDocument([FromForm] string referenceGuid, [FromForm] string isReferenceGuid)
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

                var filePath = await _IFileUpload.GetDocumentById(referenceGuid, isReferenceGuid);
                if (filePath is not null)
                {

#if DEBUG
                    {

                        string physicalPath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
                        physicalPath = physicalPath.Replace("DMS_API", "DMS_WEB");

                        if (System.IO.File.Exists(physicalPath))
                        {
                            System.IO.File.Delete(physicalPath);
                            result = await _IFileUpload.RemoveDocument(referenceGuid, isReferenceGuid);
                        }
                    }
#else
                    {
                        result = await _IFileUpload.RemoveDocument(referenceGuid, isReferenceGuid);
                    }
#endif
                }
                if (result)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //<History Author = 'ijaz Ahmad' Date='2023-03-29' Version="1.0" Branch="master"> Remove Legislation Existing  Source Document In Update Source Document Case</History>       
        [MapToApiVersion("1.1")]
        [HttpPost("RemoveLegislationDocument")]
        [EnableCors("FileUploadPolicies")]
        public async Task<IActionResult> RemoveLegislationDocument([FromForm] string referenceGuid)
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

                var filePath = await _IFileUpload.GetDocumentById(Guid.Parse(referenceGuid));
                if (filePath is not null)
                {

#if DEBUG
                    {
                        string physicalPath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
                        physicalPath = physicalPath.Replace("DMS_API", "DMS_WEB");
                        if (System.IO.File.Exists(physicalPath))
                        {
                            System.IO.File.Delete(physicalPath);
                            result = await _IFileUpload.RemoveDocument(Guid.Parse(referenceGuid));
                        }
                    }
#else
{
                        result = await _IFileUpload.RemoveDocument(Guid.Parse(referenceGuid));
}
#endif
                }
                if (result)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #endregion

        //<History Author = 'Ijaz Ahmad' Date='2023-03-14' Version="1.0" Branch="master">  Delete the Source Documents of grid in Update Case  </History>  
        [MapToApiVersion("1.1")]
        [HttpPost("SelectedSourceDocumentDelete")]
        [EnableCors("FileUploadPolicies")]
        public async Task<IActionResult> SelectedSourceDocumentDelete(List<TempAttachementVM> selectedSourceDocumentForDelete)
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
                var result = await _IFileUpload.DeleteSelectedSourceDocument(selectedSourceDocumentForDelete);
                if (result)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #region Save PortfolioToDocument

        //<History Author = 'Hassan Abbas' Date='2023-03-21' Version="1.0" Branch="master"> Save Document Portofolio</History>  
        [MapToApiVersion("1.1")]
        [HttpPost("SaveDocumentPortfolioToDocument")]
        [EnableCors("FileUploadPolicies")]
        public async Task<IActionResult> SaveDocumentPortfolioToDocument(CmsDocumentPortfolio documentPortfolio)
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

                var physicalPath = string.Empty;
                string filePath = "\\wwwroot\\Attachments\\" + documentPortfolio.UploadFrom + "\\";
                string basePath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
                basePath = basePath.Replace("DMS_API", "DMS_WEB");
                bool basePathExists = Directory.Exists(basePath);
                if (!basePathExists)
                    Directory.CreateDirectory(basePath);

                var fileName = $"{documentPortfolio.Name}{".pdf"}";
                physicalPath = Path.Combine(basePath, fileName);

                string password = _Config.GetValue<string>("DocumentEncryptionKey");
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(physicalPath, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                CryptoStreamMode.Write);

                Stream streams = new MemoryStream(documentPortfolio.FileData);

                int data;
                while ((data = streams.ReadByte()) != -1)
                    cs.WriteByte((byte)data);

                streams.Close();
                cs.Close();
                fsCrypt.Close();

                physicalPath = Path.Combine(filePath, fileName);

                var result = await _IFileUpload.SaveDocumentPortfolioToDocument(documentPortfolio, fileName, physicalPath);
                if (result)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Update
        //<History Author = 'Nadia Gull' Date='2023-04-14' Version="1.0" Branch="master"> Get Uploaded Attachement By Id </History>
        [MapToApiVersion("1.1")]
        [HttpPost("UpdateUploadedAttachementMojFlagById")]
        public async Task<IActionResult> UpdateUploadedAttachementMojFlagById([FromForm] int Id)
        {
            try
            {
                var result = await _IFileUpload.UpdateUploadedAttachementMojFlagById(Id);
                if (result)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        #endregion

        #region Get Document Number Version

        //<History Author = 'Hassan Abbas' Date='2023-06-20' Version="1.0" Branch="master"> Get Document Number And Version </History>
        [MapToApiVersion("1.1")]
        [HttpGet("GetDocumentNumberAndVersion")]
        public async Task<IActionResult> GetDocumentNumberAndVersion(Guid Id)
        {
            try
            {
                var result = await _IFileUpload.GetDocumentNumberAndVersion(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Document Number Version

        //<History Author = 'Hassan Abbas' Date='2023-06-20' Version="1.0" Branch="master"> Get Document Details By Version </History>
        [MapToApiVersion("1.1")]
        [HttpGet("GetDocumentDetailByVersionId")]
        public async Task<IActionResult> GetDocumentDetailByVersionId(Guid VersionId, Guid DocumentId)
        {
            try
            {
                var result = await _IFileUpload.GetDocumentDetailByVersionId(VersionId, DocumentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Add Document

        //<History Author = 'Hassan Abbas' Date='2023-06-21' Version="1.0" Branch="master"> Get Document Number And Version </History>
        [MapToApiVersion("1.1")]
        [HttpPost("SaveAddedDocument")]
        public async Task<IActionResult> SaveAddedDocument(DmsAddedDocument document)
        {
            try
            {
                await _IFileUpload.SaveAddedDocument(document);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for a new create DMS Document",
                    Task = "To submit the request",
                    Description = "User able to Create DMS Document successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok(document);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for a new create DMS Document Failed",
                    Body = ex.Message,
                    Category = "User unable to Create DMS Document Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for a new create DMS Document Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [MapToApiVersion("1.1")]
        [HttpPost("UpdateDMSDocument")]
        public async Task<IActionResult> UpdateDMSDocument(DmsAddedDocument document)
        {
            try
            {
                await _IFileUpload.UpdateDMSDocument(document);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for update DMS Document",
                    Task = "To update the request",
                    Description = "User able to update DMS Document Request successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request has been updated",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok(document);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for update DMS Document Failed",
                    Body = ex.Message,
                    Category = "User unable to update DMS Document Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for update DMS Document Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(ex.Message);
            }
        }
        [MapToApiVersion("1.1")]
        [HttpPost("CreateDMSDocumentVersion")]
        public async Task<IActionResult> CreateDMSDocumentVersion(DmsAddedDocument DmsTemplate)
        {
            try
            {
                await _IFileUpload.CreateDMSDocumentVersion(DmsTemplate);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for a new DMS Document Version",
                    Task = "To submit the request",
                    Description = "User able to DMS Document Version Request successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok(DmsTemplate);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for update DMS Document Verion Failed",
                    Body = ex.Message,
                    Category = "User unable to update DMS Document Verion Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for update DMS Document Verion Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region GetDocumentsList
        [MapToApiVersion("1.1")]
        [HttpPost("GetDocumentsList")]
        //<History Author = 'Muhammad Zaeem' Date='2023-02-15' Version="1.0" Branch="master"> Get consultation Request By RequestId</History>
        public async Task<IActionResult> GetDocumentsList(DocumentListAdvanceSearchVM AdvanceSearchVM)
        {
            try
            {
                return Ok(await _IFileUpload.GetDocumentsList(AdvanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Document Detail By Id
        [MapToApiVersion("1.1")]
        [HttpGet("GetDocumentDetailById")]
        public async Task<IActionResult> GetDocumentDetailById(int UploadedDocumentId)
        {
            try
            {
                return Ok(await _IFileUpload.GetDocumentDetailById(UploadedDocumentId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Add Document To Favourite
        [MapToApiVersion("1.1")]
        [HttpPost("AddDocumentToFavourite")]
        public async Task<IActionResult> AddDocumentToFavourite(DMSDocumentListVM doc)
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
                result = await _IFileUpload.AddDocumentToFavourite(doc);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Remove Favourite Document  
        [MapToApiVersion("1.1")]
        [HttpDelete("{RemoveFavouriteDocument}")]
        public async Task<IActionResult> RemoveFavouriteDocument(DMSDocumentListVM item)
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
                await _IFileUpload.RemoveFavouriteDocument(item);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Share Document with user
        [MapToApiVersion("1.1")]
        [HttpPost("ShareDocument")]
        //<History Author = 'Hassan Abbas' Date='2022-04-22' Version="1.0" Branch="master"> Handle create document request</History>
        public async Task<IActionResult> ShareDocument(DmsSharedDocument doc)
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
                await _IFileUpload.ShareDocument(doc);

                return Ok(doc);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Add Copy Attachment
        [MapToApiVersion("1.1")]
        [HttpPost("AddCopyAttachments")]
        public async Task<IActionResult> AddCopyAttachments(int DocumentId, string createdBy)
        {
            try
            {

                return Ok(await _IFileUpload.AddCopyAttachments(DocumentId, createdBy));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        #endregion

        #region Get File Types
        [MapToApiVersion("1.1")]
        [HttpGet("GetFileTypes")]
        public async Task<IActionResult> GetFileTypes()
        {
            try
            {
                return Ok(await _IFileUpload.GetFileTypes());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        #endregion

        #region Get Docuemnt Reasons List By ReferenceId

        [HttpGet("GetAddedDocumentReasonsByReferenceId")]
        [MapToApiVersion("1.1")]
        //<History Author = 'Hassan Abbas' Date='2023-06-16' Version="1.0" Branch="master">Reasons By Draft</History>
        public async Task<IActionResult> GetAddedDocumentReasonsByReferenceId(Guid referenceId)
        {
            try
            {
                return Ok(await _IFileUpload.GetAddedDocumentReasonsByReferenceId(referenceId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Legal Principle Source Docs
        //latest
        [MapToApiVersion("1.1")]
        [HttpPost("GetLLSLegalPrincipleSourceDocuments")]
        //<History Author = 'Hassan Abbas' Date='2023-04-07' Version="1.0" Branch="master"> Legal Principle Source Documents</History>
        public async Task<IActionResult> GetLLSLegalPrincipleSourceDocuments(LLSLegalPrincipalDocumentSearchVM fileSearch)
        {
            try
            {
                return Ok(await _IFileUpload.GetLLSLegalPrincipleSourceDocuments(fileSearch));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [MapToApiVersion("1.1")]
        [HttpPost("GetLLSLegalPrincipleLegalAdviceSourceDocuments")]
        //<History Author = 'Hassan Abbas' Date='2023-04-07' Version="1.0" Branch="master"> Legal Principle Source Documents</History>
        public async Task<IActionResult> GetLLSLegalPrincipleLegalAdviceSourceDocuments(LLSLegalPrincipalDocumentSearchVM fileSearch)
        {
            try
            {
                return Ok(await _IFileUpload.GetLLSLegalPrincipleLegalAdviceSourceDocuments(fileSearch));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [MapToApiVersion("1.1")]
        [HttpPost(nameof(GetKayDocumentsListForLLSLegalPrinciple))]
        //<History Author = 'Hassan Abbas' Date='2023-04-07' Version="1.0" Branch="master"> Legal Principle Source Documents</History>
        public async Task<IActionResult> GetKayDocumentsListForLLSLegalPrinciple(LLSLegalPrincipalDocumentSearchVM fileSearch)
        {
            try
            {
                return Ok(await _IFileUpload.GetKayDocumentsListForLLSLegalPrinciple(fileSearch));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [MapToApiVersion("1.1")]
        [HttpPost("GetLLSLegalPrincipleOtherSourceDocuments")]
        //<History Author = 'Hassan Abbas' Date='2023-04-07' Version="1.0" Branch="master"> Legal Principle Source Documents</History>
        public async Task<IActionResult> GetLLSLegalPrincipleOtherSourceDocuments(LLSLegalPrincipalDocumentSearchVM fileSearch)
        {
            try
            {
                return Ok(await _IFileUpload.GetLLSLegalPrincipleOtherSourceDocuments(fileSearch));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Get Principle Content Linked Documents
        [MapToApiVersion("1.1")]
        [HttpGet("GetLLSLegalPrincipleContentLinkedDocuments")]
        //<History Author = 'Hassan Abbas' Date='2023-04-07' Version="1.0" Branch="master"> Legal Principle Source Documents</History>
        public async Task<IActionResult> GetLLSLegalPrincipleContentLinkedDocuments(Guid principleContentId)
        {
            try
            {
                return Ok(await _IFileUpload.GetLLSLegalPrincipleContentLinkedDocuments(principleContentId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Masked document saved legislation/principle
        //<History Author = 'Umer Zaman' Date='2023-08-10' Version="1.0" Branch="master"> Function for saving masked doc into temp file</History>  
        [MapToApiVersion("1.1")]
        [HttpPost("SaveMaskedDocumentInOriginalDocumentFolderForTemparory")]
        [EnableCors("FileUploadPolicies")]
        public async Task<IActionResult> SaveMaskedDocumentInOriginalDocumentFolderForTemparory(TempAttachementVM viewFileDetail)
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

                var physicalPath = string.Empty;
                string filePath = "\\wwwroot\\Attachments\\" + viewFileDetail.UploadFrom + "\\";
                string basePath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
                basePath = basePath.Replace("DMS_API", "DMS_WEB");
                bool basePathExists = Directory.Exists(basePath);
                if (!basePathExists)
                    Directory.CreateDirectory(basePath);

                var fileName = $"{Path.GetFileNameWithoutExtension(viewFileDetail.FileNameWithoutTimeStamp)}{"_"}{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{".pdf"}";
                viewFileDetail.FileName = fileName;
                physicalPath = Path.Combine(basePath, fileName);

                string password = _Config.GetValue<string>("DocumentEncryptionKey");
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(physicalPath, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                CryptoStreamMode.Write);

                Stream streams = new MemoryStream(viewFileDetail.MaskedFileData);

                int data;
                while ((data = streams.ReadByte()) != -1)
                    cs.WriteByte((byte)data);

                streams.Close();
                cs.Close();
                fsCrypt.Close();

                physicalPath = Path.Combine(filePath, fileName);
                viewFileDetail.StoragePath = physicalPath;
                var result = await _IFileUpload.SaveMaskedDocumentInOriginalDocumentFolderForTemparory(viewFileDetail);
                if (result.AttachementId != 0)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [MapToApiVersion("1.1")]
        [HttpPost("LegislationAttachmentSaveFromTempAttachementToUploadedDocument")]
        [EnableCors("FileUploadPolicies")]
        public async Task<IActionResult> LegislationAttachmentSaveFromTempAttachementToUploadedDocument(LegalLegislation resultLegislationObject)
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
                var result = await _IFileUpload.LegislationAttachmentSaveFromTempAttachementToUploadedDocument(resultLegislationObject);
                if (result)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //<History Author = 'Hassan Abbas' Date='2023-04-07' Version="1.0" Branch="master"> Copy Legal Principle Source Documents From Dms to Temp</History>
        [MapToApiVersion("1.1")]
        [HttpPost("CopyLegalLegislationSourceAttachments")]
        public async Task<IActionResult> CopyLegalLegislationSourceAttachments(CopyLegalLegislationSourceAttachmentsVM copyAttachments)
        {
            try
            {
                await _IFileUpload.CopyLegalLegislationSourceAttachments(copyAttachments, Directory.GetCurrentDirectory().Replace("DMS_API", "DMS_WEB") + "\\wwwroot\\Attachments\\");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Link Documents

        //<History Author = 'Hassan Abbas' Date='2023-08-07' Version="1.0" Branch="master"> Link Document to Destination Entities</History>
        [MapToApiVersion("1.1")]
        [HttpPost("LinkDocumentToDestinationEntities")]
        public async Task<IActionResult> LinkDocumentToDestinationEntities(LinkDocumentsVM linkDocumentDetails)
        {
            try
            {
                await _IFileUpload.LinkDocumentToDestinationEntities(linkDocumentDetails, _Config.GetValue<string>("DocumentEncryptionKey"));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get Template Parameters
        [MapToApiVersion("1.1")]
        [HttpGet("GetTemplateParameters")]
        public async Task<IActionResult> GetTemplateParameters(int? moduleId)
        {
            try
            {
                return Ok(await _IFileUpload.GetTemplateParameters(moduleId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        #endregion

        #region Save Case Template

        //<History Author = 'Hassan Abbas' Date='2023-08-21' Version="1.0" Branch="master"> Save Case Template </History>
        [MapToApiVersion("1.1")]
        [HttpPost("SaveCaseTemplate")]
        public async Task<IActionResult> SaveCaseTemplate(CaseTemplate template)
        {
            try
            {
                return Ok(await _IFileUpload.SaveCaseTemplate(template));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Save Draft Stamp
        [MapToApiVersion("1.1")]
        [HttpPost("SaveDraftStamp")]
        public async Task<IActionResult> SaveDraftStamp(CmsDraftStamp template)
        {
            try
            {
                return Ok(await _IFileUpload.SaveDraftStamp(template));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Get Case Template

        [HttpGet("GetCaseTemplate")]
        [MapToApiVersion("1.1")]
        //<History Author = 'Hassan Abbas' Date='2023-08-24' Version="1.0" Branch="master"> Get Case Template</History>
        public async Task<IActionResult> GetCaseTemplate(int templateId)
        {
            try
            {
                return Ok(await _IFileUpload.GetCaseTemplate(templateId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Templates List
        [MapToApiVersion("1.1")]
        [HttpPost("GetTemplatesList")]
        //<History Author = 'Muhammad Zaeem' Date='2023-02-15' Version="1.0" Branch="master"> Get consultation Request By RequestId</History>
        public async Task<IActionResult> GetTemplatesList(TemplateListAdvanceSearchVM AdvanceSearchVM)
        {
            try
            {
                return Ok(await _IFileUpload.GetTemplatesList(AdvanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }


        [HttpGet("GetHeaderFooterTemplates")]
        [MapToApiVersion("1.1")]
        //<History Author = 'Hassan Abbas' Date='2023-09-03' Version="1.0" Branch="master">Get Header Footer Templates</History>
        public async Task<IActionResult> GetHeaderFooterTemplates()
        {
            try
            {
                return Ok(await _IFileUpload.GetHeaderFooterTemplates());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


        #region Document  Type
        #region Get Document Type list 
        [HttpGet("GetDocumentTypeList")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetDocumentTypeList()
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
                return Ok(await _ILookups.GetDocumentTypeList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #endregion
        #region Update Document Type

        [HttpPost("UpdateDocumentType")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> UpdateDocumentType(AttachmentType ldsDocument)
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
                await _ILookups.UpdateDocumentType(ldsDocument);


                return Ok(ldsDocument);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Save Document Type 

        [HttpPost("SaveDocumentType")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> SaveDocumentType(AttachmentType attachment)
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
                await _ILookups.SaveDocumentType(attachment);
                return Ok(attachment);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Convert contract template into document and save into uploaded document table
        [MapToApiVersion("1.1")]
        [HttpPost("SaveContractTemplateToDocument")]
        [EnableCors("FileUploadPolicies")]
        public async Task<IActionResult> SaveContractTemplateToDocument(ConsultationRequest item)
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

                var physicalPath = string.Empty;
                string filePath = "\\wwwroot\\Attachments\\" + WorkflowModuleEnum.COMSConsultationManagement.ToString() + "\\";
                string basePath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
                basePath = basePath.Replace("DMS_API", "DMS_WEB");
                bool basePathExists = Directory.Exists(basePath);
                if (!basePathExists)
                    Directory.CreateDirectory(basePath);

                var fileName = $"{Path.GetFileNameWithoutExtension(item.RequestTitle)}{"_"}{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{".pdf"}";
                physicalPath = Path.Combine(basePath, fileName);

                string password = _Config.GetValue<string>("DocumentEncryptionKey");
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(physicalPath, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                CryptoStreamMode.Write);

                Stream streams = new MemoryStream(item.FileData);

                int data;
                while ((data = streams.ReadByte()) != -1)
                    cs.WriteByte((byte)data);
                streams.Close();
                cs.Close();
                fsCrypt.Close();

                physicalPath = Path.Combine(filePath, fileName);

                var result = await _IFileUpload.SaveContractTemplateToDocument(item, fileName, physicalPath);
                if (result)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #region  Get Document  by Id 
        [HttpGet("GetDocumentTypeById")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetDocumentTypeById(int AttachmentTypeId)
        {
            try
            {
                return Ok(await _ILookups.GetDocumentTypeById(AttachmentTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #endregion


        #region Convert MOM template into document and save into uploaded document table
        [MapToApiVersion("1.1")]
        [HttpPost("SaveMOMTemplateToDocument")]
        [EnableCors("FileUploadPolicies")]
        public async Task<IActionResult> SaveMOMTemplateToDocument(MeetingMom meetingMom)
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

                var physicalPath = string.Empty;
                string filePath = "\\wwwroot\\Attachments\\" + WorkflowModuleEnum.MeetingMom.ToString() + "\\";
                string basePath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
                basePath = basePath.Replace("DMS_API", "DMS_WEB");
                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                var fileName = $"{Path.GetFileNameWithoutExtension("MOM Editor File")}{"_"}{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{".pdf"}";
                physicalPath = Path.Combine(basePath, fileName);

                if (meetingMom.Project == "FATWA_WEB")
                {
                    string password = _Config.GetValue<string>("DocumentEncryptionKey");
                    UnicodeEncoding UE = new UnicodeEncoding();
                    byte[] key = UE.GetBytes(password);

                    FileStream fsCrypt = new FileStream(physicalPath, FileMode.Create);

                    RijndaelManaged RMCrypto = new RijndaelManaged();

                    CryptoStream cs = new CryptoStream(fsCrypt,
                        RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                    Stream streams = new MemoryStream(meetingMom.FileData);

                    int data;
                    while ((data = streams.ReadByte()) != -1)
                        cs.WriteByte((byte)data);
                    streams.Close();
                    cs.Close();
                    fsCrypt.Close();
                }
                else
                {
                    System.IO.File.WriteAllBytes(physicalPath, meetingMom.FileData);
                }
                physicalPath = Path.Combine(filePath, fileName);

                var result = await _IFileUpload.SaveMOMTemplateToDocument(meetingMom, fileName, physicalPath);
                if (result)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region UpdateTemplateStatus
        [MapToApiVersion("1.1")]
        [HttpPost("UpdateTemplateStatus")]
        public async Task<IActionResult> UpdateTemplateStatus([FromForm] bool isActive, [FromForm] int id)
        {
            try
            {
                await _IFileUpload.UpdateTemplateStatus(isActive, id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Kuwait Alyawm 

        //<History Author = 'Hassan Abbas' Date='2024-01-05' Version="1.0" Branch="master"> Function for FILE upload of Kuwait Alyawm</History>

        [AllowAnonymous]

        [MapToApiVersion("1.1")]
        [HttpPost("KayUpload")]
        //[EnableCors("FileUploadPolicies")]
        //[IgnoreAntiforgeryToken]
        //[AllowAnonymous]
        //[ApiKeyAuthorize]
        public async Task<IActionResult> KayUpload(IEnumerable<IFormFile> file,
            [FromForm] string documentTitle, [FromForm] string editionNumber, [FromForm] string editionType,
             [FromForm] DateTime publicationDate, [FromForm] string publicationDateHijri, [FromForm] bool isFullEdition
               )
        {
            try
            {
                if (file != null)
                {
                    var fileName = string.Empty;
                    var physicalPath = string.Empty;
                    string filePath = $"\\{editionNumber}\\";
                    string basePath = Path.Combine(_Config.GetValue<string>("KayPublicationsPath") + filePath);
                    if (!Directory.Exists(basePath))
                        Directory.CreateDirectory(basePath);

                    foreach (var files in file)
                    {

                        fileName = $"{(documentTitle != null ? documentTitle : Path.GetFileNameWithoutExtension(files.FileName))}{"_"}{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{Path.GetExtension(files.FileName)}";
                        physicalPath = Path.Combine(basePath, fileName);
                        string password = _Config.GetValue<string>("DocumentEncryptionKey");
                        UnicodeEncoding UE = new UnicodeEncoding();
                        byte[] key = UE.GetBytes(password);

                        FileStream fsCrypt = new FileStream(physicalPath, FileMode.Create);

                        RijndaelManaged RMCrypto = new RijndaelManaged();

                        CryptoStream cs = new CryptoStream(fsCrypt,
                            RMCrypto.CreateEncryptor(key, key),
                        CryptoStreamMode.Write);

                        Stream streams = files.OpenReadStream();
                        FileStream fsIn = streams as FileStream;

                        int data;
                        while ((data = streams.ReadByte()) != -1)
                            cs.WriteByte((byte)data);


                        streams.Close();
                        cs.Close();
                        fsCrypt.Close();

                        KayPublication ObjFill = new KayPublication();
                        {
                            ObjFill.StoragePath = physicalPath;
                            ObjFill.FileTitle = fileName;
                            ObjFill.DocumentTitle = documentTitle;
                            ObjFill.EditionType = editionType;
                            ObjFill.EditionNumber = editionNumber;
                            ObjFill.PublicationDate = publicationDate;
                            ObjFill.PublicationDateHijri = publicationDateHijri;
                            ObjFill.IsFullEdition = isFullEdition;
                            ObjFill.CreatedBy = "KAY RPA";
                            ObjFill.CreatedDate = DateTime.Now;
                        }
                        KayPublication attachement = await _IFileUpload.SaveKayPublicationDocument(ObjFill);
                    }
                    return Ok(new { Message = "Document Saved Successfully." });
                }
                else
                {
                    return BadRequest(new { Message = "No Document Found, please select document to upload." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex?.InnerException.Message });
            }
        }

        [MapToApiVersion("1.1")]
        [HttpPost("GetkayDocumentsListForDms")]
        //<History Author = 'ijaz Ahmad' Date='2023-12-28' Version="1.0" Branch="master"> Get kay DocumentsList For Dms </History>
        public async Task<IActionResult> GetkayDocumentsListForDms(KayDocumentListAdvanceSearchVM AdvanceSearchVM)
        {
            try
            {
                return Ok(await _IFileUpload.GetKayDocumentsList(AdvanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetkayDocumentAccordingEditionNumber")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetkayDocumentAccordingEditionNumber(string editionNumber)
        {
            try
            {
                return Ok(await _IFileUpload.GetkayDocumentAccordingEditionNumber(editionNumber));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [MapToApiVersion("1.1")]
        [HttpPost("GetkayDocumentsListforLegalLegislation")]
        //<History Author = 'ijaz Ahmad' Date='2023-12-28' Version="1.0" Branch="master"> Get kay DocumentsList For Ligslation </History>
        public async Task<IActionResult> GetkayDocumentsListforLegalLegislation(KayDocumentListAdvanceSearchVM AdvanceSearchVM)
        {
            try
            {
                return Ok(await _IFileUpload.GetKayDocumentsList(AdvanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Push FATWA Archived Cases Documents
        //<History Author = 'Ammaar Naveed' Date='2024-12-17' Version="1.0" Branch="master">Endpoint to push documents for FATWA Archived Cases</History>

        //[AllowAnonymous]
        //[ApiKeyAuthorize]
        [MapToApiVersion("1.1")]
        [HttpPost("ArchivingDocumentUpload")]
        public async Task<IActionResult> ArchivingDocumentUpload(IEnumerable<IFormFile> file,
            [FromForm] string? documentTitle, [FromForm] Guid caseId, [FromForm] int documentTypeId, [FromForm] DateTime documentDate, [FromForm] int numberOfPages, [FromForm] string? scannedBy, [FromForm] DateTime? scannedOn)
        {
            try
            {
                if (file != null)
                {
                    var fileName = string.Empty;
                    var physicalPath = string.Empty;
                    //string filePath = $"\\{editionNumber}\\";
                    string basePath = Path.Combine(_Config.GetValue<string>("archiving_document_path"));
                    if (!Directory.Exists(basePath))
                        Directory.CreateDirectory(basePath);

                    foreach (var files in file)
                    {

                        fileName = $"{(documentTitle != null ? documentTitle : Path.GetFileNameWithoutExtension(files.FileName))}{"_"}{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{Path.GetExtension(files.FileName)}";
                        physicalPath = Path.Combine(basePath, fileName);
                        string password = _Config.GetValue<string>("DocumentEncryptionKey");
                        UnicodeEncoding UE = new UnicodeEncoding();
                        byte[] key = UE.GetBytes(password);

                        FileStream fsCrypt = new FileStream(physicalPath, FileMode.Create);

                        RijndaelManaged RMCrypto = new RijndaelManaged();

                        CryptoStream cs = new CryptoStream(fsCrypt,
                            RMCrypto.CreateEncryptor(key, key),
                        CryptoStreamMode.Write);

                        Stream streams = files.OpenReadStream();
                        FileStream fsIn = streams as FileStream;

                        int data;
                        while ((data = streams.ReadByte()) != -1)
                            cs.WriteByte((byte)data);

                        streams.Close();
                        cs.Close();
                        fsCrypt.Close();

                        ArchivedCaseDocuments ObjFill = new ArchivedCaseDocuments();
                        {
                            ObjFill.Id = Guid.NewGuid();
                            ObjFill.FilePath = physicalPath;
                            ObjFill.FileName = fileName;
                            ObjFill.DocumentTypeId = documentTypeId;
                            ObjFill.DocType = Path.GetExtension(fileName);
                            ObjFill.CaseId = caseId;
                            ObjFill.NumberOfPages = numberOfPages;
                            ObjFill.DocumentDate = documentDate.Date;
                            ObjFill.DocumentTitle = documentTitle;
                            ObjFill.ScannedBy = scannedBy;
                            ObjFill.ScannedOn = scannedOn;
                            ObjFill.CreatedBy = "System Generated";
                            ObjFill.CreatedDate = DateTime.Now;
                            ObjFill.IsDeleted = false;
                        }
                        await _IFileUpload.SaveArchivedCaseDocuments(ObjFill);
                    }
                    return Ok(new { Message = "Document Saved Successfully." });
                }
                else
                {
                    return BadRequest(new { Message = "No Document Found, please select document to upload." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex?.InnerException.Message });
            }
        }
        #endregion

        #region MOJ Rolls  Document 
        //<History Author = 'ijaz Ahmad' Date='2024-01-29' Version="1.0" Branch="master"> Function for FILE upload of Moj Roll</History>
        [MapToApiVersion("1.1")]
        [HttpPost("MojRollsUpload")]
        [EnableCors("FileUploadPolicies")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> MojRollsUpload(IEnumerable<IFormFile> file, [FromForm] Guid documentRefrenceId)
        {
            try
            {

                UploadedDocument attachement = new UploadedDocument();
                if (file != null && file.Count() > 0)
                {
                    var uploadFrom = "MOJROLLS";
                    var fileName = string.Empty;
                    var physicalPath = string.Empty;
                    string filePath = $"\\wwwroot\\Attachments\\{uploadFrom}\\";
                    string basePath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
                    basePath = basePath.Replace("DMS_API", "DMS_WEB");
                    bool basePathExists = Directory.Exists(basePath);
                    if (!basePathExists)
                        Directory.CreateDirectory(basePath);
                    foreach (var files in file)
                    {

                        fileName = $"{(Path.GetFileNameWithoutExtension(files.FileName))}{"_"}{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{Path.GetExtension(files.FileName)}";
                        physicalPath = Path.Combine(basePath, fileName);
                        string password = _Config.GetValue<string>("DocumentEncryptionKey");
                        UnicodeEncoding UE = new UnicodeEncoding();
                        byte[] key = UE.GetBytes(password);

                        FileStream fsCrypt = new FileStream(physicalPath, FileMode.Create);
                        RijndaelManaged RMCrypto = new RijndaelManaged();
                        CryptoStream cs = new CryptoStream(fsCrypt,
                            RMCrypto.CreateEncryptor(key, key),
                        CryptoStreamMode.Write);
                        Stream streams = files.OpenReadStream();
                        FileStream fsIn = streams as FileStream;

                        int data;
                        while ((data = streams.ReadByte()) != -1)
                            cs.WriteByte((byte)data);
                        streams.Close();
                        cs.Close();
                        fsCrypt.Close();
                        physicalPath = Path.Combine(filePath, fileName);
                        UploadedDocument ObjFill = new UploadedDocument();
                        {
                            ObjFill.StoragePath = physicalPath;
                            ObjFill.FileName = fileName;
                            ObjFill.ReferenceGuid = documentRefrenceId;
                            ObjFill.CreatedAt = physicalPath;
                            ObjFill.IsDeleted = false;
                            ObjFill.IsActive = true;
                            ObjFill.AttachmentTypeId = (int)AttachmentTypeEnum.HearingRollDocument;
                            ObjFill.CreatedBy = "MOJ RPA";
                            ObjFill.Description = "Moj Rolls Request Documents";
                            ObjFill.DocType = Path.GetExtension(files.FileName);
                            ObjFill.CreatedDateTime = DateTime.Now;
                            ObjFill.DocumentDate = DateTime.Now;
                        }
                        attachement = await _IFileUpload.SaveUploadedDocument(ObjFill);
                    }
                    var response = new
                    {
                        status = "success",
                        message = "Document Saved Successfully",
                        document_id = attachement.UploadedDocumentId,
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest(new { Message = "No Document Found, please select document to upload." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex?.InnerException.Message });
            }
        }
        [MapToApiVersion("1.1")]
        [HttpGet("GetMojRollDocumentById")]
        public async Task<IActionResult> GetMojRollDocumentById(int? DocumentId)
        {
            try
            {
                return Ok(await _IFileUpload.GetMojRollDocumentById(DocumentId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Pushing Extracted MOJ Images  document to DMS
        [MapToApiVersion("1.1")]
        [HttpPost("MojImageUpload")]
        [EnableCors("FileUploadPolicies")]
        [IgnoreAntiforgeryToken]
        //<History Author = 'ijaz Ahmad' Date='2024-01-05' Version="1.0" Branch="master"> Pushing Extracted MOJ  Images document to DMS</History>
        public async Task<IActionResult> MojImageUpload(IEnumerable<IFormFile> file,
           [FromForm] string cannumber, [FromForm] string casenumber, [FromForm] string documenttype,
            [FromForm] DateTime documentdate
              )
        {
            try
            {
                UploadedDocument attachement = new UploadedDocument();

                if (file != null && file.Count() > 0)
                {
                    var fileName = string.Empty;
                    var physicalPath = string.Empty;
                    var uploadFrom = "MOJImageDocument";
                    string filePath = $"\\wwwroot\\Attachments\\{uploadFrom}\\{cannumber}\\{casenumber}\\";
                    string basePath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
                    basePath = basePath.Replace("DMS_API", "DMS_WEB");
                    bool basePathExists = Directory.Exists(basePath);
                    if (!basePathExists)
                        Directory.CreateDirectory(basePath);

                    foreach (var files in file)
                    {
                        var attachmentType = await _ILookups.FindAndSaveAttachmentType(documenttype);
                        fileName = $"{Path.GetFileNameWithoutExtension(files.FileName)}{"_"}{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{Path.GetExtension(files.FileName)}";
                        physicalPath = Path.Combine(basePath, fileName);
                        string password = _Config.GetValue<string>("DocumentEncryptionKey");
                        UnicodeEncoding UE = new UnicodeEncoding();
                        byte[] key = UE.GetBytes(password);

                        FileStream fsCrypt = new FileStream(physicalPath, FileMode.Create);

                        RijndaelManaged RMCrypto = new RijndaelManaged();

                        CryptoStream cs = new CryptoStream(fsCrypt,
                            RMCrypto.CreateEncryptor(key, key),
                        CryptoStreamMode.Write);

                        Stream streams = files.OpenReadStream();
                        FileStream fsIn = streams as FileStream;

                        int data;
                        while ((data = streams.ReadByte()) != -1)
                            cs.WriteByte((byte)data);


                        streams.Close();
                        cs.Close();
                        fsCrypt.Close();

                        physicalPath = Path.Combine(filePath, fileName);
                        MojDocument ObjFill = new MojDocument();
                        {
                            ObjFill.StoragePath = physicalPath;
                            ObjFill.FileName = fileName;
                            ObjFill.AttachmentTypeId = attachmentType.AttachmentTypeId;
                            ObjFill.CANNumber = cannumber;
                            ObjFill.CaseNumber = casenumber;
                            ObjFill.DocumentDate = documentdate;
                            ObjFill.CreatedBy = "MOJ RPA";
                            ObjFill.CreatedDate = DateTime.Now;

                        }
                        attachement = await _IFileUpload.SaveMojImageDocument(ObjFill);
                    }
                    var response = new
                    {
                        status = "success",
                        message = "Document uploaded successfully",
                        document_id = attachement?.UploadedDocumentId
                    };

                    return Ok(response);

                }
                else
                {
                    return BadRequest(new { Message = "No Document Found, please select document to upload." });
                }
            }
            catch
            {
                return BadRequest(new { Message = "Something went wrong, please try again." });
                throw;
            }
        }

        [MapToApiVersion("1.1")]
        [HttpPost("GetMojImageDocumentList")]
        //<History Author = 'ijaz Ahmad' Date='2023-12-28' Version="1.0" Branch="master"> Get kay Moj Images </History>
        public async Task<IActionResult> GetMojImageDocumentList(MojDocumentAdvanceSearchVM AdvanceSearchVM)
        {
            try
            {
                return Ok(await _IFileUpload.GetMojImageDocumentList(AdvanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [MapToApiVersion("1.1")]
        [HttpGet("GetMojDocumentByCaseNumber")]
        //<History Author = 'ijaz Ahmad' Date='2023-12-28' Version="1.0" Branch="master"> Get  Moj Images Document ListBy Case Number </History>
        public async Task<IActionResult> GetMojDocumentByCaseNumber(string caseNumber)
        {
            try
            {
                return Ok(await _IFileUpload.GetMojDocumentByCaseNumber(caseNumber));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Add Stamp to the PDF
        //<History Author = 'Hassan Abbas' Date='2024-03-27' Version="1.0" Branch="master"> Add G2G Stamp on the Pdf</History>
        private async Task<byte[]> AddG2GStampToPdfByteArray(byte[] pdfBytes)
        {
            try
            {
                PdfSharp.Pdf.PdfDocument pdfDocument;
                using (MemoryStream ms = new MemoryStream(pdfBytes))
                {
                    pdfDocument = PdfReader.Open(ms, PdfDocumentOpenMode.Modify);
                }
                PdfSharp.Pdf.PdfPage page = pdfDocument.Pages[0];

                XGraphics gfx = XGraphics.FromPdfPage(page);

                string stampImagePath = $"\\Stamps\\g2gStamp.png";
                XImage stampImage = XImage.FromFile(Path.Combine(Directory.GetCurrentDirectory() + stampImagePath));
                gfx.DrawImage(stampImage, x: 40, y: 85, width: 80, height: 80);

                byte[] stampedPdfByteArray;
                using (MemoryStream outputStream = new MemoryStream())
                {
                    pdfDocument.Save(outputStream, closeStream: false);
                    stampedPdfByteArray = outputStream.ToArray();
                }
                return stampedPdfByteArray;
            }
            catch (Exception ex)
            {
                return pdfBytes;
            }
        }
        #endregion

        #region Add Stamp to the PDF
        //<History Author = 'Hassan Abbas' Date='2024-03-27' Version="1.0" Branch="master"> Add Time Stamp on the Pdf</History>
        private async Task<byte[]> AddTimeStampToPdfByteArray(byte[] pdfBytes, string userName)
        {
            try
            {
                //var userId = await _ILookups.GetUserIdByUserEmail(userName);
                //var userDetail = await _IFileUpload.GetUserPersonalInformationByUserId(userId);

                PdfSharp.Pdf.PdfDocument pdfDocument;
                using (MemoryStream ms = new MemoryStream(pdfBytes))
                {
                    pdfDocument = PdfReader.Open(ms, PdfDocumentOpenMode.Modify);
                }
                PdfSharp.Pdf.PdfPage page = pdfDocument.Pages[0];
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Arial", 8, XFontStyle.Regular);

                XPen pen = new XPen(XColors.Black, 1);
                XStringFormat format = new XStringFormat();
                format.Alignment = XStringAlignment.Center;
                format.LineAlignment = XLineAlignment.Center;

                //XRect rect1 = new XRect(gfx.PageSize.Width - 200, gfx.PageSize.Height - 200, 150, 20);
                //gfx.DrawRectangle(pen, rect1);
                //gfx.DrawString(string.Concat(userDetail?.FirstName_Ar, " ", userDetail?.SecondName_Ar," ", userDetail?.LastName_Ar).ArabicWithFontGlyphsToPfd(), font, XBrushes.Black, rect1, format);

                XRect rect2 = new XRect(gfx.PageSize.Width - 200, gfx.PageSize.Height - 180, 150, 20);
                gfx.DrawRectangle(pen, rect2);
                gfx.DrawString(DateTime.Now.ToString(), font, XBrushes.Black, rect2, format);

                byte[] stampedPdfByteArray;
                using (MemoryStream outputStream = new MemoryStream())
                {
                    pdfDocument.Save(outputStream, closeStream: false);
                    stampedPdfByteArray = outputStream.ToArray();
                }
                return stampedPdfByteArray;
            }
            catch (Exception ex)
            {
                return pdfBytes;
            }
        }
        #endregion

        #region Remove Temp Attachments By ReferenceId

        //<History Author = 'Hassan Abbas' Date='2024-04-01' Version="1.0" Branch="master">Remove Temp Attachments </History>
        [MapToApiVersion("1.1")]
        [HttpGet("RemoveTempAttachementsByReferenceId")]
        public async Task<IActionResult> RemoveTempAttachementsByReferenceId(Guid referenceId)
        {
            try
            {
                await _IFileUpload.RemoveTempAttachementsByReferenceId(referenceId, Directory.GetCurrentDirectory().Replace("DMS_API", "DMS_WEB"));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region G2G Tarasol Correspondence Document 
        //<History Author = 'Hassan Abbas' Date='2024-04-07' Version="1.0" Branch="master"> Function for Uploading Documents related to G2G Tatasol Correspondence sent to FATWA</History>
        [MapToApiVersion("1.1")]
        [HttpPost("UploadG2GTarasolCorrespondenceDocument")]
        [EnableCors("FileUploadPolicies")]
        [IgnoreAntiforgeryToken]
        //[AllowAnonymous]
        //[ApiKeyAuthorize]
        public async Task<IActionResult> UploadG2GTarasolCorrespondenceDocument(IEnumerable<IFormFile> file, [FromForm] Guid communicationGuid)
        {
            try
            {

                UploadedDocument attachement = new UploadedDocument();
                if (file != null && file.Count() > 0)
                {
                    var uploadFrom = "G2GTarasolCorrespondence";
                    var fileName = string.Empty;
                    var physicalPath = string.Empty;
                    string filePath = $"\\wwwroot\\Attachments\\{uploadFrom}\\";
                    string basePath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
                    basePath = basePath.Replace("DMS_API", "DMS_WEB");
                    bool basePathExists = Directory.Exists(basePath);
                    if (!basePathExists)
                        Directory.CreateDirectory(basePath);
                    foreach (var files in file)
                    {

                        fileName = $"{(Path.GetFileNameWithoutExtension(files.FileName))}{"_"}{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{Path.GetExtension(files.FileName)}";
                        physicalPath = Path.Combine(basePath, fileName);
                        string password = _Config.GetValue<string>("DocumentEncryptionKey");
                        UnicodeEncoding UE = new UnicodeEncoding();
                        byte[] key = UE.GetBytes(password);

                        FileStream fsCrypt = new FileStream(physicalPath, FileMode.Create);
                        RijndaelManaged RMCrypto = new RijndaelManaged();
                        CryptoStream cs = new CryptoStream(fsCrypt,
                            RMCrypto.CreateEncryptor(key, key),
                        CryptoStreamMode.Write);
                        Stream streams = files.OpenReadStream();
                        FileStream fsIn = streams as FileStream;

                        int data;
                        while ((data = streams.ReadByte()) != -1)
                            cs.WriteByte((byte)data);
                        streams.Close();
                        cs.Close();
                        fsCrypt.Close();
                        physicalPath = Path.Combine(filePath, fileName);
                        UploadedDocument ObjFill = new UploadedDocument();
                        {
                            ObjFill.StoragePath = physicalPath;
                            ObjFill.FileName = fileName;
                            ObjFill.ReferenceGuid = communicationGuid;
                            ObjFill.CreatedAt = physicalPath;
                            ObjFill.IsDeleted = false;
                            ObjFill.IsActive = true;
                            ObjFill.AttachmentTypeId = (int)AttachmentTypeEnum.G2GTarasolCorrespondenceDocument;
                            ObjFill.CreatedBy = "G2G TARASOL RPA";
                            ObjFill.Description = "G2G Tarasol Correspondence Documents";
                            ObjFill.DocType = Path.GetExtension(files.FileName);
                            ObjFill.CreatedDateTime = DateTime.Now;
                        }
                        attachement = await _IFileUpload.SaveUploadedDocument(ObjFill);
                    }
                    var response = new
                    {
                        status = "success",
                        message = "Documents Saved Successfully"
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest(new { Message = "No Document Found, please select document to upload." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex?.InnerException.Message });
            }
        }

        #endregion

        #region G2G Tarasol Correspondence Document 
        //<History Author = 'Hassan Abbas' Date='2024-04-07' Version="1.0" Branch="master"> Function for Uploading Documents related to G2G Tatasol Correspondence sent to FATWA</History>
        [MapToApiVersion("1.1")]
        [HttpPost("UploadG2GTarasolCorrespondenceDocumentRabbitMQ")]
        //[AllowAnonymous]
        //[EnableCors("FileUploadPolicies")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UploadG2GTarasolCorrespondenceDocumentRabbitMQ([FromForm] string byteArray, [FromForm] Guid communicationGuid, [FromForm] string fileName)
        {
            try
            {
                byte[] byteArrayy = JsonConvert.DeserializeObject<byte[]>(byteArray);

                UploadedDocument attachement = new UploadedDocument();
                if (byteArrayy != null && byteArrayy.Count() > 0)
                {
                    var uploadFrom = "G2GTarasolCorrespondence";
                    var physicalPath = string.Empty;
                    string filePath = $"\\wwwroot\\Attachments\\{uploadFrom}\\";
                    string basePath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
                    basePath = basePath.Replace("DMS_API", "DMS_WEB");
                    bool basePathExists = Directory.Exists(basePath);
                    if (!basePathExists)
                        Directory.CreateDirectory(basePath);

                    fileName = $"{(Path.GetFileNameWithoutExtension(fileName))}{"_"}{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{Path.GetExtension(fileName)}";
                    physicalPath = Path.Combine(basePath, fileName);
                    string password = _Config.GetValue<string>("DocumentEncryptionKey");
                    UnicodeEncoding UE = new UnicodeEncoding();
                    byte[] key = UE.GetBytes(password);

                    FileStream fsCrypt = new FileStream(physicalPath, FileMode.Create);
                    RijndaelManaged RMCrypto = new RijndaelManaged();
                    CryptoStream cs = new CryptoStream(fsCrypt,
                        RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);
                    Stream streams = new MemoryStream(byteArrayy);
                    FileStream fsIn = streams as FileStream;

                    int data;
                    while ((data = streams.ReadByte()) != -1)
                        cs.WriteByte((byte)data);
                    streams.Close();
                    cs.Close();
                    fsCrypt.Close();
                    physicalPath = Path.Combine(filePath, fileName);
                    UploadedDocument ObjFill = new UploadedDocument();
                    {
                        ObjFill.StoragePath = physicalPath;
                        ObjFill.FileName = fileName;
                        ObjFill.ReferenceGuid = communicationGuid;
                        ObjFill.CreatedAt = physicalPath;
                        ObjFill.IsDeleted = false;
                        ObjFill.IsActive = true;
                        ObjFill.AttachmentTypeId = (int)AttachmentTypeEnum.G2GTarasolCorrespondenceDocument;
                        ObjFill.CreatedBy = "G2G TARASOL RPA";
                        ObjFill.Description = "G2G Tarasol Correspondence Documents";
                        ObjFill.DocType = ".pdf";
                        ObjFill.CreatedDateTime = DateTime.Now;
                    }
                    attachement = await _IFileUpload.SaveUploadedDocument(ObjFill);

                    var response = new
                    {
                        status = "success",
                        message = "Documents Saved Successfully"
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest(new { Message = "No Document Found, please select document to upload." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex?.InnerException.Message });
            }
        }

        #endregion

        #region Get LLSLegalPrincipleReference UploadedAttachements
        //<History Author = 'Muhammad Abuzar' Date='2024-04-22' Version="1.0" Branch="master">temp attachement</History>
        [MapToApiVersion("1.1")]
        [HttpGet(nameof(GetLLSLegalPrincipleReferenceUploadedAttachements))]
        public async Task<IActionResult> GetLLSLegalPrincipleReferenceUploadedAttachements(Guid principleId)
        {
            try
            {
                var result = await _IFileUpload.GetLLSLegalPrincipleReferenceUploadedAttachements(principleId);
                if (result is not null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                //return new BadRequestResponse { Message = ex.Message };
                throw;
            }
        }
        #endregion

        #region Save Literature Tmp Attachment to Upload document ("Method use only for Adding and Editing literature")

        [MapToApiVersion("1.1")]
        [HttpPost("SaveLiteratureTempAttachmentToUploadedDocument")]
        [EnableCors("FileUploadPolicies")]
        public async Task<IActionResult> SaveLiteratureTempAttachmentToUploadedDocument(FileUploadVM item)
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
                var result = await _IFileUpload.SaveLiteratureTempAttachmentToUploadedDocument(item);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for save temporary attachment",
                    Task = "To submit the request",
                    Description = "User able to save temporary attachment Request successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                if (result)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for save temporary attachment Failed",
                    Body = ex.Message,
                    Category = "User unable to save temporary attachment Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for save temporary attachment Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Get Uploaded Attachement and Add with new Entery  ("Method use only for Adding and Editing literature")

        [MapToApiVersion("1.1")]
        [HttpPost("GetUploadedAttachementAndWithNewOne")]
        [EnableCors("FileUploadPolicies")]
        public async Task<IActionResult> GetUploadedAttachementAndWithNewOne(FileUploadVM item)
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
                var result = await _IFileUpload.GetUploadedAttachementAndWithNewOne(item);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for save temporary attachment",
                    Task = "To submit the request",
                    Description = "User able to save temporary attachment Request successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                if (result)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for save temporary attachment Failed",
                    Body = ex.Message,
                    Category = "User unable to save temporary attachment Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for save temporary attachment Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region  Check Attachment file in temp ("Method use only for Literature")
        [MapToApiVersion("1.1")]
        [HttpPost("CheckingAttachementInTemp")]
        [EnableCors("FileUploadPolicies")]
        public async Task<IActionResult> CheckingAttachementInTemp(FileUploadVM item)
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
                var result = await _IFileUpload.CheckingAttachementInTemp(item);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for save temporary attachment",
                    Task = "To submit the request",
                    Description = "User able to save temporary attachment Request successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                if (result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    result = new List<TempAttachement>();
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for save temporary attachment Failed",
                    Body = ex.Message,
                    Category = "User unable to save temporary attachment Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for save temporary attachment Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #region get document by Id for legal legislation
        [MapToApiVersion("1.1")]
        [HttpPost("GetAttachementById")]
        public async Task<IActionResult> GetAttachementById([FromForm] int Id)
        {
            try
            {
                var result = await _IFileUpload.GetAttachementById(Id);
                if (result is not null)
                    return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        #endregion


        [HttpPost("GetLatestVersionAndUpdateDocumentVersion")]
        public async Task<IActionResult> GetLatestVersionAndUpdateDocumentVersion(Guid versionid)
        {
            try
            {
                await _IFileUpload.GetLatestVersionAndUpdateDocumentVersion(versionid);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("RemoveDocumentByReferenceGuidAndAttachmentTypeId")]
        public async Task<IActionResult> RemoveDocumentByReferenceGuidAndAttachmentTypeId(string basePath, Guid referenceGuid, int AttachmentTypeId)
        {
            try
            {
                await _IFileUpload.RemoveDocumentByReferenceGuidAndAttachmentTypeId(basePath, referenceGuid, AttachmentTypeId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetSigningMethods")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetSigningMethods()
        {
            try
            {
                return Ok(await _IFileUpload.GetSigningMethods());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetDocumentBytes")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetDocumentBytes(int documentId)
        {
            try
            {
                var result = await _IFileUpload.GetUploadedAttachementById(documentId);
                if (result != null)
                {
                    var physicalPath = Path.Combine(_Config.GetValue<string>("dms_file_path") + result.StoragePath).Replace(@"\\", @"\");
#if !DEBUG
                    physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
#endif
                    string password = _Config.GetValue<string>("DocumentEncryptionKey");
                    var byteArray = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, result.DocType, password, true);
                    return Ok(new ApiCallResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        IsSuccessStatusCode = true,
                        ResultData = byteArray,
                        Message = "success"
                    });
                }
                else
                {
                    return NotFound(new ApiCallResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccessStatusCode = false,
                        Message = "No_record_found"
                    });
                }
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
        //<History Author = 'Muhammad Zaeem' Date='2024-09-11' Version="1.0" Branch="master"> Function for saving temp file</History>  
        [MapToApiVersion("1.1")]
        [HttpPost("SaveStockTakingReportToDocument")]
        [EnableCors("FileUploadPolicies")]
        public async Task<IActionResult> SaveStockTakingReportToDocument(LmsStockTaking item)
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
                var physicalPath = string.Empty;
                string filePath = "\\wwwroot\\Attachments\\" + item.UploadFrom + "\\";
                string basePath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
                basePath = basePath.Replace("DMS_API", "DMS_WEB");
                bool basePathExists = Directory.Exists(basePath);
                if (!basePathExists)
                    Directory.CreateDirectory(basePath);

                var fileName = $"{Path.GetFileNameWithoutExtension("Stock Taking Report")}{"_"}{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{".pdf"}";
                physicalPath = Path.Combine(basePath, fileName);

                string password = _Config.GetValue<string>("DocumentEncryptionKey");
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(physicalPath, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                CryptoStreamMode.Write);

                Stream streams = new MemoryStream(item.FileData);

                int data;
                while ((data = streams.ReadByte()) != -1)
                    cs.WriteByte((byte)data);

                streams.Close();
                cs.Close();
                fsCrypt.Close();

                physicalPath = Path.Combine(filePath, fileName);
                TempAttachement attachement = new TempAttachement
                {
                    AttachmentTypeId = (int)AttachmentTypeEnum.StockTakingReport,
                    Guid = item.Id,
                    DocumentDate = DateTime.Now,
                    Description = item.Note,
                    FileName = fileName,
                    FileNameWithoutTimeStamp = "Stock Taking Report" + Path.GetExtension(fileName),
                    FileNumber = null,
                    DocType = Path.GetExtension(".pdf"),
                    UploadedBy = item.CreatedBy,
                    DocDateTime = DateTime.Now,
                    UploadedDate = DateTime.Now,
                    StoragePath = physicalPath,
                };

                var result = await _IFileUpload.CreateTempAttachement(attachement);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for save draft template",
                    Task = "To submit the request",
                    Description = "User able to save draft template Request successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for save draft template Failed",
                    Body = ex.Message,
                    Category = "User unable to save draft template Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for save draft template Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }


        [MapToApiVersion("1.1")]
        [HttpPost("RemoveFromTemp")]
        [EnableCors("FileUploadPolicies")]
        public async Task<IActionResult> RemoveFromTemp([FromForm] string referenceGuid)
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

                var filePath = await _IFileUpload.GetTemById(Guid.Parse(referenceGuid));
                if (filePath is not null)
                {

#if DEBUG
                    {
                        string physicalPath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
                        physicalPath = physicalPath.Replace("DMS_API", "DMS_WEB");
                        if (System.IO.File.Exists(physicalPath))
                        {
                            System.IO.File.Delete(physicalPath);
                            result = await _IFileUpload.RemoveDocumentFromTemp(Guid.Parse(referenceGuid));
                        }
                    }
#else
{
                        result = await _IFileUpload.RemoveDocumentFromTemp(Guid.Parse(referenceGuid));
}
#endif
                }
                if (result)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
