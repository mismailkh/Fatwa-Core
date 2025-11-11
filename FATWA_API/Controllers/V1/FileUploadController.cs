using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V1
{
    //<History Author = 'Hassan Abbas' Date='2022-04-11' Version="1.0" Branch="master"> Controller for file upload component</History>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FileUploadController : ControllerBase
    {
        //private readonly ITempFileUpload _IFileUpload;

        //public FileUploadController(ITempFileUpload iFileUpload)
        //{
        //    _IFileUpload = iFileUpload;
        //}

        //[HttpGet("GetAttachmentTypes")]
        //[MapToApiVersion("1.0")]
        //public async Task<IActionResult> GetAttachmentTypes(int? ModuleId)
        //{
        //    try
        //    {
        //        return Ok(await _IFileUpload.GetAttachmentTypes(ModuleId));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        ////<History Author = 'Hassan Abbas' Date='2022-04-11' Version="1.0" Branch="master"> Function for file upload with Guid and Username</History>
        //[MapToApiVersion("1.0")]
        //[HttpPost("Upload")]
        //[EnableCors("FileUploadPolicies")]
        //[AllowAnonymous]
        //[IgnoreAntiforgeryToken]
        ////[RequestFormLimits(ValueCountLimit = 2147482222, ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        //public async Task<IActionResult> Upload(IEnumerable<IFormFile> files, [FromForm] Guid _pEntityIdentifierGuid, [FromForm] string _userName, [FromForm] int _typeId, [FromForm] string _uploadFrom,
        //    [FromForm] string _project, [FromForm] string? _otherAttachmentType, [FromForm] string? _description, [FromForm] string? _referenceNo, [FromForm] DateTime? _referenceDate, [FromForm] DateTime? _documentDate)
        //{
        //    try
        //    {
        //        if (files != null)
        //        {
        //            var physicalPath = string.Empty;
        //            string filePath = "\\wwwroot\\Attachments\\" + _uploadFrom + "\\";
        //            string basePath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
        //            basePath = basePath.Replace("FATWA_API", _project);
        //            bool basePathExists = Directory.Exists(basePath);
        //            if (!basePathExists)
        //                Directory.CreateDirectory(basePath);

        //            foreach (var file in files)
        //            {
        //                var fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}{"_"}{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{Path.GetExtension(file.FileName)}";
        //                physicalPath = Path.Combine(basePath, fileName);
        //                using (var fileStream = new FileStream(physicalPath, FileMode.Create))
        //                {
        //                    await file.OpenReadStream().CopyToAsync(fileStream);
        //                }
        //                physicalPath = Path.Combine(filePath, fileName);
        //                TempAttachement attachement = await _IFileUpload.CreateTempAttachement(new TempAttachement
        //                {
        //                    StoragePath = physicalPath,
        //                    FileName = fileName,
        //                    FileNameWithoutTimeStamp = file.FileName,
        //                    Guid = _pEntityIdentifierGuid,
        //                    AttachmentTypeId = _typeId,
        //                    UploadedBy = _userName,
        //                    UploadedDate = DateTime.Now,
        //                    DocType = Path.GetExtension(file.FileName),
        //                    OtherAttachmentType = _otherAttachmentType,
        //                    Description = _description,
        //                    ReferenceNo = _referenceNo,
        //                    ReferenceDate = _referenceDate,
        //                    DocumentDate = _documentDate
        //                });
        //            }
        //            return Ok(JsonConvert.SerializeObject(new FileUploadSuccessResponse { StoragePath = physicalPath }));
        //        }
        //        else
        //        {
        //            return new BadRequestResult();
        //        }
        //    }
        //    catch
        //    {
        //        return new BadRequestResult();
        //    }
        //}

        //[MapToApiVersion("1.0")]
        //[HttpPost("SingleUpload")]
        //[EnableCors("FileUploadPolicies")]
        //[AllowAnonymous]
        //[IgnoreAntiforgeryToken]
        //public async Task<IActionResult> SingleUpload(IEnumerable<IFormFile> files, [FromForm] Guid _pEntityIdentifierGuid, [FromForm] string _userName, [FromForm] string _uploadFrom, [FromForm] string? _oldImagePath , [FromForm] string? _FileNumber, [FromForm] string? _FileTitle , [FromForm] int _attachmentType)
        //{
        //    try
        //    {
        //        if (files != null)
        //        {
        //            string filePath = "\\wwwroot\\Attachments\\" + _uploadFrom + "\\";
        //            string basePath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
        //            basePath = basePath.Replace("FATWA_API", "FATWA_WEB");
        //            bool basePathExists = Directory.Exists(basePath);
        //            if (!basePathExists)
        //                Directory.CreateDirectory(basePath);

        //            foreach (var file in files)
        //            {
        //                //string fileName = $"{Path.GetFileNameWithoutExtension(_uploadFrom + "_" + _pEntityIdentifierGuid)}{Path.GetExtension(file.FileName)}"; 
        //                string fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}{"_"}{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{Path.GetExtension(file.FileName)}";
        //                string physicalPath = Path.Combine(basePath, fileName);

        //                if (_oldImagePath != null && _oldImagePath != "null")
        //                {
        //                    string oldImageBasePath = Path.Combine(Directory.GetCurrentDirectory() + _oldImagePath);
        //                    oldImageBasePath = oldImageBasePath.Replace("FATWA_API", "FATWA_WEB");

        //                    if (System.IO.File.Exists(oldImageBasePath))
        //                    {
        //                        System.IO.File.Delete(oldImageBasePath);
        //                    }
        //                }

        //                using (FileStream fileStream = new FileStream(physicalPath, FileMode.Create))
        //                {
        //                    await file.OpenReadStream().CopyToAsync(fileStream);
        //                }

        //                physicalPath = Path.Combine(filePath, fileName);
        //                var tempAttachment = new TempAttachement();
        //                if (_attachmentType > 0)
        //                {
        //                    tempAttachment.StoragePath = physicalPath;
        //                    tempAttachment.FileName = fileName;
        //                    tempAttachment.FileNameWithoutTimeStamp = file.FileName;
        //                    tempAttachment.Guid = _pEntityIdentifierGuid;
        //                    tempAttachment.AttachmentTypeId = _attachmentType;
        //                    tempAttachment.FileTitle = _FileTitle;
        //                    tempAttachment.FileNumber = _FileNumber;
        //                    tempAttachment.UploadedBy = _userName;
        //                    tempAttachment.UploadedDate = DateTime.Now;
        //                    tempAttachment.DocType = Path.GetExtension(file.FileName);
        //                }
        //                else
        //                {


        //                    tempAttachment.StoragePath = physicalPath;
        //                    tempAttachment.FileName = fileName;
        //                    tempAttachment.FileNameWithoutTimeStamp = file.FileName;
        //                    tempAttachment.Guid = _pEntityIdentifierGuid;
        //                    tempAttachment.AttachmentTypeId = 6;
        //                    tempAttachment.FileTitle = _FileTitle;
        //                    tempAttachment.FileNumber = _FileNumber;
        //                    tempAttachment.UploadedBy = _userName;
        //                    tempAttachment.UploadedDate = DateTime.Now;
        //                    tempAttachment.DocType = Path.GetExtension(file.FileName);

        //                }

        //                TempAttachement attachement = await _IFileUpload.CreateTempAttachement(tempAttachment);
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        // implement error handling here, this merely indicates a failure to the upload
        //        return new BadRequestResult();
        //    }

        //    // Return an empty string message in this case
        //    return new OkResult();
        //}

        //// Return an empty string message in this case 

        ////<History Author = 'Hassan Abbas' Date='2022-04-11' Version="1.0" Branch="master"> Function for removing temp file</History>
        //[MapToApiVersion("1.0")]
        //[HttpPost("Remove")]
        //[EnableCors("FileUploadPolicies")]
        //[AllowAnonymous]
        //public async Task<IActionResult> Remove([FromForm] string _fileToRemove, [FromForm] string _userName, [FromForm] string _uploadFrom, [FromForm] string _project) // must match RemoveField which defaults to "files"
        //{
        //    try
        //    {
        //        if (_fileToRemove != null)
        //        {
        //            var attachements = await _IFileUpload.GetTempAttachementsByFileAndUserName(_fileToRemove, _userName);
        //            foreach (var file in attachements)
        //            {
        //                string filePath = "\\wwwroot\\Attachments\\" + _uploadFrom + "\\";
        //                string basePath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
        //                basePath = basePath.Replace("FATWA_API", _project);

        //                var physicalPath = Path.Combine(basePath, file.FileName);

        //                if (System.IO.File.Exists(physicalPath))
        //                {
        //                    System.IO.File.Delete(physicalPath);
        //                    await _IFileUpload.DeleteTempAttachement(file.AttachementId);
        //                }
        //            }
        //        }
        //        return new EmptyResult();
        //    }
        //    catch
        //    {
        //        return new BadRequestResult();
        //    }
        //}

        ////<History Author = 'Hassan Abbas' Date='2022-04-11' Version="1.0" Branch="master"> Function for removing temp file</History>
        //[MapToApiVersion("1.0")]
        //[HttpPost("RemoveUploadedDocument")]
        //[EnableCors("FileUploadPolicies")]
        //[AllowAnonymous]
        //public async Task<IActionResult> RemoveUploadedDocument([FromForm] int _uploadedDocumentId) // must match RemoveField which defaults to "files"
        //{
        //    try
        //    {
        //        if (_uploadedDocumentId != null && _uploadedDocumentId > 0)
        //        {
        //            try
        //            {
        //                var file = await _IFileUpload.GetUploadedDocumentById(_uploadedDocumentId);
        //                var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\LmsLiteratureAttachments\\");
        //                var physicalPath = Path.Combine(basePath, file.FileName);

        //                if (System.IO.File.Exists(physicalPath))
        //                {
        //                    System.IO.File.Delete(physicalPath);
        //                    await _IFileUpload.DeleteUploadedDocument(file.UploadedDocumentId);
        //                }
        //            }
        //            catch
        //            {
        //                Response.StatusCode = 500;
        //                Response.WriteAsync("some error message");
        //            }
        //        }
        //        return new EmptyResult();
        //    }
        //    catch
        //    {
        //        return new BadRequestResult();
        //    }
        //}

        ////<History Author = 'Hassan Abbas' Date='2022-04-22' Version="1.0" Branch="master"> Function for getting attachements list</History>
        //[MapToApiVersion("1.0")]
        //[HttpPost("GetUploadedAttachements")]
        //public async Task<IActionResult> GetUploadedAttachements([FromForm] bool _isLiterature, [FromForm] int _literatureId, [FromForm] Guid _guid)
        //{
        //    try
        //    {
        //        try
        //        {
        //            if (_isLiterature && _literatureId > 0)
        //            {
        //                return Ok(await _IFileUpload.GetUploadedAttachementsByLiteratureId(_literatureId));
        //            }
        //            else
        //            {
        //                return Ok(await _IFileUpload.GetUploadedAttachementsByReferenceGuid(_guid));
        //            }
        //        }
        //        catch
        //        {
        //            Response.StatusCode = 500;
        //            Response.WriteAsync("some error message");
        //            return new BadRequestResult();
        //        }
        //    }
        //    catch
        //    {
        //        return new BadRequestResult();
        //    }
        //}


        ////<History Author = 'Hassan Abbas' Date='2022-04-22' Version="1.0" Branch="master"> Function for getting official letters list</History>
        //[MapToApiVersion("1.0")]
        //[HttpPost("GetOfficialDocuments")]
        //public async Task<IActionResult> GetOfficialDocuments([FromForm] Guid _guid)
        //{
        //    try
        //    {
        //        return Ok(await _IFileUpload.GetOfficialDocuments(_guid));
        //    }
        //    catch
        //    {
        //        return new BadRequestResult();
        //    }
        //}

        ////<History Author = 'Hassan Abbas' Date='2022-04-22' Version="1.0" Branch="master"> Function for getting attachements list</History>
        //[MapToApiVersion("1.0")]
        //[HttpPost("GetTempAttachements")]
        //public async Task<IActionResult> GetTempAttachements([FromForm] Guid _guid)
        //{
        //    try
        //    {
        //        try
        //        {
        //            return Ok(await _IFileUpload.GetTempAttachementsByReferenceGuid(_guid));
        //        }
        //        catch
        //        {
        //            Response.StatusCode = 500;
        //            Response.WriteAsync("some error message");
        //            return new BadRequestResult();
        //        }
        //    }
        //    catch
        //    {
        //        return new BadRequestResult();
        //    }
        //}

        ////<History Author = 'Hassan Abbas' Date='2022-04-22' Version="1.0" Branch="master"> Function for getting attachements list</History>
        //[MapToApiVersion("1.0")]
        //[HttpPost("GetAttachementsByReferenceGuid")]
        //public async Task<IActionResult> GetAttachementsByReferenceGuid(Guid _guid)
        //{
        //    try
        //    {
        //        List<UploadedDocumentVM> attachements = new List<UploadedDocumentVM>();
        //        try
        //        {
        //            attachements = await _IFileUpload.GetUploadedAttachementsByReferenceGuid(_guid);
        //        }
        //        catch
        //        {
        //            Response.StatusCode = 500;
        //            Response.WriteAsync("some error message");
        //        }
        //        return Ok(attachements);
        //    }
        //    catch
        //    {
        //        return new BadRequestResult();
        //    }
        //}

        ////<History Author = 'Hassan Abbas' Date='2022-11-21' Version="1.0" Branch="master"> Upload Temp Attachment to Uploaded Document</History>
        //[MapToApiVersion("1.0")]
        //[HttpPost("UploadTempAttachmentToUploadedDocument")]
        //public async Task<IActionResult> UploadTempAttachmentToUploadedDocument(Guid referenceId, string createdBy)
        //{
        //    try
        //    {
        //        await _IFileUpload.UploadTempAttachmentToUploadedDocument(referenceId, createdBy);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
        //    }
        //}
    }
}
