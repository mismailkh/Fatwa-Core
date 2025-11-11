using AutoMapper;
using DMS_API.Model;
using DSPExternalAuthenticationService;
using DSPExternalSigningService;
using DSPRemoteSigning;
using DSPVerificationServiceForExternalAndRemote;
using FATWA_DOMAIN.Enums;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models.DigitalSignature;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using Syncfusion.Pdf.Parsing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using static FATWA_DOMAIN.Enums.DmsEnums;
using static FATWA_GENERAL.Helper.Response;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace DMS_API.Controllers.V1.DigitalSignature
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DigitalSignatureController : ControllerBase
    {
        private readonly IConfiguration _Config;
        private readonly ITempFileUpload _IFileUpload;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostingEnv;

        public DigitalSignatureController(IConfiguration config, ITempFileUpload iFileUpload, IMapper mapper, IWebHostEnvironment hostingEnv)
        {
            _Config = config;
            _IFileUpload = iFileUpload;
            _mapper = mapper;
            _hostingEnv = hostingEnv;
        }

        private static readonly JsonSerializerSettings _options = new() { NullValueHandling = NullValueHandling.Ignore };

        #region LocalSigning
        [HttpPost("VerifyToken")]
        [AllowAnonymous]
        public bool VerifyToken(string userId, string sessionToken)
        {
            Log.Information("VerifyToken Start");
            string password = _Config.GetValue<string>("DocumentEncryptionKey");
            var decryptedToken = EncryptionDecryption.DecryptText(sessionToken, password);
            decryptedToken = decryptedToken.Split("_")[0];
            Log.Information("VerifyToken successfully");
            return decryptedToken == userId;
        }

        [HttpPost("GetSignatureImage")]
        [AllowAnonymous]
        public async Task<string> GetSignatureImage(string userId, int signatureImageTypeId, string sessionToken)
        {
            Log.Information("GetSignatureImage Start");
            var userdocument = await _IFileUpload.GetSignatureImagePath(userId);
            var imagefilePath = Path.Combine(_Config.GetValue<string>("dms_file_path") + userdocument.StoragePath).Replace(@"\\", @"\");
#if !DEBUG
            imagefilePath = imagefilePath.Replace("\\wwwroot\\Attachments\\", "\\");
#endif
            Log.Information("ImagePath" + imagefilePath);
            //Descyption Key
            string password = _Config.GetValue<string>("DocumentEncryptionKey");
            var byteArray = await DocumentEncryptionService.GetDecryptedDocumentBytes(imagefilePath, userdocument.DocType, password,true);
            Log.Information("GetSignatureImage end");
            return Convert.ToBase64String(byteArray);
        }

        [HttpPost("GetDocument")]
        [AllowAnonymous]
        public async Task<string> GetDocument(string? userId, string documentId, string? sessionToken)
        {
            Log.Information("GetDocument Start");
            Log.Information("documentId" + documentId);
            var result = await _IFileUpload.GetUploadedAttachementById(int.Parse(documentId));
            var path = string.IsNullOrEmpty(result.SignedDocStoragePath) ? result.StoragePath : result.SignedDocStoragePath;
            var physicalPath = Path.Combine(_Config.GetValue<string>("dms_file_path") + path).Replace(@"\\", @"\");
            physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
            Log.Information("GetDocument end");
            Log.Information("physicalPath" + physicalPath);

            //Descyption Key
            string password = _Config.GetValue<string>("DocumentEncryptionKey");
            var byteArray = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, result.DocType, password);
            return Convert.ToBase64String(byteArray);
        }

        [HttpPost("SubmitSignedDocumentVersion")]
        [AllowAnonymous]
        public int SubmitSignedDocumentVersion(LocalSigningRequest localSigningRequest)
        {
            Log.Information("SubmitSignedDocumentVersion Start");
            var result = _IFileUpload.GetUploadedAttachementById(int.Parse(localSigningRequest.DocumentId));
            var filePath = result.Result.StoragePath.Remove(result.Result.StoragePath.LastIndexOf('\\'));
            filePath = Path.Combine(filePath + "Signed");
            Log.Information("filePath" + filePath);
            if (localSigningRequest.SignedDocumentBytes != null && localSigningRequest.SignedDocumentBytes.Count() > 0)
            {
                string basePath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
                basePath = basePath.Replace("DMS_API", "DMS_WEB");
                bool basePathExists = Directory.Exists(basePath);
                if (!basePathExists)
                    Directory.CreateDirectory(basePath);

                var fileName = $"{(Path.GetFileNameWithoutExtension(result.Result.FileName))}{"_"}{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{Path.GetExtension(result.Result.FileName)}";
                var physicalPath = Path.Combine(basePath, fileName);
                string password = _Config.GetValue<string>("DocumentEncryptionKey");
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(physicalPath, FileMode.Create);
                RijndaelManaged RMCrypto = new RijndaelManaged();
                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                CryptoStreamMode.Write);
                Stream streams = new MemoryStream(localSigningRequest.SignedDocumentBytes);
                FileStream fsIn = streams as FileStream;

                int data;
                while ((data = streams.ReadByte()) != -1)
                    cs.WriteByte((byte)data);
                streams.Close();
                cs.Close();
                fsCrypt.Close();
                physicalPath = Path.Combine(filePath, fileName);
                result.Result.SignedDocStoragePath = physicalPath;
                result.Result.StatusId = (int)SigningTaskStatusEnum.Signed;
                var res = _IFileUpload.UpdateDocument(result.Result);

                DSPRequestLog requestLog = new DSPRequestLog();
                requestLog.DocumentId = int.Parse(localSigningRequest.DocumentId);
                requestLog.CreatedBy = localSigningRequest.UserId;
                requestLog.CreatedDate = DateTime.Now;
                requestLog.CivilId = _IFileUpload.GetCivilId(localSigningRequest.UserId).Result;
                requestLog.ExternalId = requestLog.CivilId + "_" + int.Parse(localSigningRequest.DocumentId) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                requestLog.RequestId = "";
                requestLog.Status = true;
                requestLog.SigningMethodId = (int)SigningMethodEnum.LocalSigning;
                requestLog.RequestStatus = SigningRequestStatusEnum.Approved.GetDisplayName();
                _IFileUpload.SaveDSPRequestLog(requestLog);
                Log.Information("SubmitSignedDocumentVersion end");
                return res.Result;
            }
            return 1; // 1 mean error because Diyar consider 0 as success
        }
        #endregion

        #region ExternalSigning
        [HttpPost("ExternalSigningRequest")]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalSigningRequest(ExternalSigningRequest externalSigningRequest)
        {
            try
            {
                Log.Information("External Signing initiate");
                if (externalSigningRequest.UserId != null)
                    externalSigningRequest.CivilId = await _IFileUpload.GetCivilId(externalSigningRequest.UserId);
                Log.Information("civilId" + externalSigningRequest.CivilId);
                DSPRequestLog requestLog = new DSPRequestLog();
                requestLog.DocumentId = externalSigningRequest.DocumentId;
                requestLog.CreatedBy = externalSigningRequest.CreatedBy;
                requestLog.CreatedDate = DateTime.Now;
                Dictionary<byte[], int> docWithPageCount = await GetDocumentAndPageNumber(externalSigningRequest.DocumentId);
                DSPExternalSigningService.SignatureAppearanceData signatureAppearance = new DSPExternalSigningService.SignatureAppearanceData();
                signatureAppearance.ImageData = Convert.FromBase64String(await GetSignatureImage(externalSigningRequest.UserId, 0, null));
                Log.Information("Write Image ByteArray to ImageData");
                signatureAppearance.SelectedReason = externalSigningRequest.SelectedReason;
                signatureAppearance.PageNumber = docWithPageCount.Values.FirstOrDefault();

                DSPExternalSigningService.ExtendedRemoteSigningRequest extendedRemoteSigning = new DSPExternalSigningService.ExtendedRemoteSigningRequest();
                extendedRemoteSigning.SignatureProfileName = await _IFileUpload.GetSignatureProfileName((int)SigningMethodEnum.ExternalSigning);
                requestLog.ExternalId = extendedRemoteSigning.ExternalId = externalSigningRequest.CivilId + "_" + externalSigningRequest.DocumentId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                requestLog.CivilId = extendedRemoteSigning.UserID = externalSigningRequest.CivilId;
                extendedRemoteSigning.Data = docWithPageCount.Keys.FirstOrDefault();
                Log.Information("get document byte array success");
                extendedRemoteSigning.DataTitle = externalSigningRequest.DataTitle;
                extendedRemoteSigning.DataDescription = externalSigningRequest.DataDescription;
                extendedRemoteSigning.CallbackURL = _Config.GetValue<string>("callback_url") + "/DigitalSignature/SigningServiceResponse";
                extendedRemoteSigning.SignAppearanceDetails = signatureAppearance;
                AsyncSignDataResponse res = new AsyncSignDataResponse();
                using (var client = new ExternalSigningServiceClient())
                {
                    try
                    {
                        Log.Information("start client");
                        client.Endpoint.Binding.SendTimeout = new TimeSpan(0, 5, 0);
                        client.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 5, 0);
                        res = await client.SignDocumentWithExternalUserAsyncAsync(extendedRemoteSigning);
                        Log.Information("receive response");
                        // write response in json file
                        var resfileName = "SignDocumentWithExternalUserAsyncAsync.json";
                        var jsonString = JsonConvert.SerializeObject(res, _options);
                        System.IO.File.AppendAllText(resfileName, jsonString);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(res.ErrorMessage);
                    }
                }
                requestLog.RequestId = res.RequestId;
                requestLog.Status = res.Success;
                requestLog.ErrorCode = res.ErrorCode;
                requestLog.ErrorMessage = res.ErrorMessage;
                requestLog.SigningMethodId = (int)SigningMethodEnum.ExternalSigning;
                if (res.Success)
                {
                    Log.Information("Success");
                    requestLog.RequestStatus = SigningRequestStatusEnum.Initiated.GetDisplayName();
                    await _IFileUpload.SaveDSPRequestLog(requestLog);
                    return Ok(res.RequestId);
                }
                else
                {
                    Log.Information("fail");
                    requestLog.RequestStatus = SigningRequestStatusEnum.Failed.GetDisplayName();
                    await _IFileUpload.SaveDSPRequestLog(requestLog);
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("SigningServiceResponse")]
        [AllowAnonymous]
        public void SigingServiceCallback(DSPExternalSigningService.RemoteResultResponseBase results)
        {
            try
            {
                Log.Information("Start RemoteResultResponseBase object");
                var JsonfileName = "response.json";
                var jsonString = JsonConvert.SerializeObject(results, _options);
                System.IO.File.AppendAllText(JsonfileName, jsonString);
                Log.Information("end");
                if (results.Success)
                {
                    var requestLog = _IFileUpload.UpdateDSPRequestLog(results.RequestId, SigningRequestStatusEnum.Approved.GetDisplayName());
                    Console.WriteLine("update database" + requestLog.Result.RequestStatus);
                    Console.WriteLine("document id " + requestLog.Result.DocumentId);
                    var result = _IFileUpload.GetUploadedAttachementById(requestLog.Result.DocumentId);
                    Console.WriteLine("Get document from db success");
                    var filePath = result.Result.StoragePath.Remove(result.Result.StoragePath.LastIndexOf('\\'));
                    filePath = Path.Combine(filePath + "Signed");
                    Console.WriteLine("File Path" + filePath);

                    var documentByte = results.ResultContainer.SignedData;
                    if (documentByte != null && documentByte.Count() > 0)
                    {
                        Console.WriteLine("get byte array");

                        string basePath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
                        basePath = basePath.Replace("DMS_API", "DMS_WEB");
                        bool basePathExists = Directory.Exists(basePath);
                        if (!basePathExists)
                            Directory.CreateDirectory(basePath);

                        var fileName = $"{(Path.GetFileNameWithoutExtension(result.Result.FileName))}{"_"}{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{Path.GetExtension(result.Result.FileName)}";
                        var physicalPath = Path.Combine(basePath, fileName);
                        string password = _Config.GetValue<string>("DocumentEncryptionKey");
                        UnicodeEncoding UE = new UnicodeEncoding();
                        byte[] key = UE.GetBytes(password);
                        Console.WriteLine("start encryption");
                        FileStream fsCrypt = new FileStream(physicalPath, FileMode.Create);
                        RijndaelManaged RMCrypto = new RijndaelManaged();
                        CryptoStream cs = new CryptoStream(fsCrypt,
                            RMCrypto.CreateEncryptor(key, key),
                        CryptoStreamMode.Write);
                        Stream streams = new MemoryStream(documentByte);
                        FileStream fsIn = streams as FileStream;

                        int data;
                        while ((data = streams.ReadByte()) != -1)
                            cs.WriteByte((byte)data);
                        streams.Close();
                        cs.Close();
                        fsCrypt.Close();
                        Console.WriteLine("end encryption");
                        physicalPath = Path.Combine(filePath, fileName);
                        Console.WriteLine("physicalPath  " + physicalPath);
                        result.Result.StatusId = (int)SigningTaskStatusEnum.Signed;
                        result.Result.SignedDocStoragePath = physicalPath;
                        var res = _IFileUpload.UpdateDocument(result.Result);
                        Console.WriteLine("update database  " + res);
                    }
                }
                else
                {
                    Console.WriteLine("update database  " + results.RequestId + "Declined");
                    var requestLog = _IFileUpload.UpdateDSPRequestLog(results.RequestId, SigningRequestStatusEnum.Declined.GetDisplayName());
                    Console.WriteLine("update database  " + results.RequestId + "Declined done");
                }
            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost("TestExternalSigningRequest")]
        [AllowAnonymous]
        public async Task<IActionResult> TestExternalSigningRequest(ExternalSigningRequest externalSigningRequest)
        {
            try
            {
                //var filePath = Path.Combine(_hostingEnv.WebRootPath, "Attachments", "Bug Reporting Module. V.01.pdf");
                var filePath = Path.Combine(_hostingEnv.WebRootPath, "Attachments", externalSigningRequest.fileName);

                DSPRequestLog requestLog = new DSPRequestLog();
                requestLog.DocumentId = 1001;
                requestLog.CreatedBy = externalSigningRequest.CreatedBy;
                requestLog.CreatedDate = DateTime.Now;

                DSPExternalSigningService.SignatureAppearanceData signatureAppearance = new DSPExternalSigningService.SignatureAppearanceData();
                signatureAppearance.ImageData = Convert.FromBase64String(await GetSignatureImage(externalSigningRequest.UserId, 0, null)); ;
                signatureAppearance.SelectedReason = externalSigningRequest.SelectedReason;
                signatureAppearance.PageNumber = externalSigningRequest.PageNumber;

                DSPExternalSigningService.ExtendedRemoteSigningRequest extendedRemoteSigning = new DSPExternalSigningService.ExtendedRemoteSigningRequest();
                extendedRemoteSigning.SignatureProfileName = externalSigningRequest.SignatureProfileName;
                requestLog.ExternalId = extendedRemoteSigning.ExternalId = externalSigningRequest.UserId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                requestLog.CivilId = extendedRemoteSigning.UserID = externalSigningRequest.UserId;
                Log.Information(filePath);
                extendedRemoteSigning.Data = System.IO.File.ReadAllBytes(filePath);
                Log.Information("get document byte array success");
                extendedRemoteSigning.DataTitle = externalSigningRequest.DataTitle;
                extendedRemoteSigning.DataDescription = externalSigningRequest.DataDescription;
                extendedRemoteSigning.CallbackURL = _Config.GetValue<string>("callback_url") + "/DigitalSignature/TestSigningServiceResponse";
                extendedRemoteSigning.SignAppearanceDetails = signatureAppearance;
                Log.Information("object created");
                Log.Information(extendedRemoteSigning.ToString());


                AsyncSignDataResponse res = new AsyncSignDataResponse();
                using (var client = new ExternalSigningServiceClient())
                {
                    try
                    {
                        client.Endpoint.Binding.SendTimeout = new TimeSpan(0, 5, 0);
                        client.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 5, 0);
                        res = await client.SignDocumentWithExternalUserAsyncAsync(extendedRemoteSigning);
                        Log.Information("send request");
                        Log.Information("send request");

                        // write response in json file
                        var resfileName = "SignDocumentWithExternalUserAsyncAsync.json";
                        var jsonString = JsonConvert.SerializeObject(res, _options);
                        System.IO.File.AppendAllText(resfileName, jsonString);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Log.Information("1st exception");
                    }
                }
                requestLog.RequestId = res.RequestId;
                requestLog.Status = res.Success;
                requestLog.ErrorCode = res.ErrorCode;
                requestLog.ErrorMessage = res.ErrorMessage;
                requestLog.SigningMethodId = (int)SigningMethodEnum.ExternalSigning;
                if (res.Success)
                {
                    Log.Information("Success");
                    requestLog.RequestStatus = SigningRequestStatusEnum.Initiated.GetDisplayName();
                    await _IFileUpload.SaveDSPRequestLog(requestLog);
                    return Ok(res.RequestId);
                }
                else
                {
                    Log.Information("fail");
                    requestLog.RequestStatus = SigningRequestStatusEnum.Failed.GetDisplayName();
                    await _IFileUpload.SaveDSPRequestLog(requestLog);
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("TestSigningServiceResponse")]
        [AllowAnonymous]
        public void TestSigingServiceCallback(DSPExternalSigningService.RemoteResultResponseBase results)
        {
            try
            {
                Log.Information("Start RemoteResultResponseBase object");
                var JsonfileName = "response.json";
                var jsonString = JsonConvert.SerializeObject(results, _options);
                System.IO.File.AppendAllText(JsonfileName, jsonString);
                Log.Information("end");
                if (results.Success)
                {
                    var requestLog = _IFileUpload.UpdateDSPRequestLog(results.RequestId, SigningRequestStatusEnum.Approved.GetDisplayName());
                    Console.WriteLine("update database" + requestLog.Result.RequestStatus);
                    Console.WriteLine("document id " + requestLog.Result.DocumentId);
                    var result = _IFileUpload.GetUploadedAttachementById(requestLog.Result.DocumentId);
                    Console.WriteLine("Get document from db success");
                    var filePath = result.Result.StoragePath.Remove(result.Result.StoragePath.LastIndexOf('\\'));
                    filePath = Path.Combine(filePath + "Signed");
                    Console.WriteLine("File Path" + filePath);

                    var documentByte = results.ResultContainer.SignedData;
                    if (documentByte != null && documentByte.Count() > 0)
                    {
                        Console.WriteLine("get byte array");

                        string basePath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
                        basePath = basePath.Replace("DMS_API", "DMS_WEB");
                        bool basePathExists = Directory.Exists(basePath);
                        if (!basePathExists)
                            Directory.CreateDirectory(basePath);

                        var fileName = $"{(Path.GetFileNameWithoutExtension(result.Result.FileName))}{"_"}{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{Path.GetExtension(result.Result.FileName)}";
                        var physicalPath = Path.Combine(basePath, fileName);
                        string password = _Config.GetValue<string>("DocumentEncryptionKey");
                        UnicodeEncoding UE = new UnicodeEncoding();
                        byte[] key = UE.GetBytes(password);
                        Console.WriteLine("start encryption");
                        FileStream fsCrypt = new FileStream(physicalPath, FileMode.Create);
                        RijndaelManaged RMCrypto = new RijndaelManaged();
                        CryptoStream cs = new CryptoStream(fsCrypt,
                            RMCrypto.CreateEncryptor(key, key),
                        CryptoStreamMode.Write);
                        Stream streams = new MemoryStream(documentByte);
                        FileStream fsIn = streams as FileStream;

                        int data;
                        while ((data = streams.ReadByte()) != -1)
                            cs.WriteByte((byte)data);
                        streams.Close();
                        cs.Close();
                        fsCrypt.Close();
                        Console.WriteLine("end encryption");
                        physicalPath = Path.Combine(filePath, fileName);
                        Console.WriteLine("physicalPath  " + physicalPath);
                        result.Result.SignedDocStoragePath = physicalPath;
                        var res = _IFileUpload.UpdateDocument(result.Result);
                        Console.WriteLine("update database  " + res);
                    }
                }
                else
                {
                    Console.WriteLine("update database  " + results.RequestId + "Declined");
                    var requestLog = _IFileUpload.UpdateDSPRequestLog(results.RequestId, SigningRequestStatusEnum.Declined.GetDisplayName());
                    Console.WriteLine("update database  " + results.RequestId + "Declined done");
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region RemoteSigning
        [HttpPost("RemoteSigningRequest")]
        [AllowAnonymous]
        public async Task<IActionResult> RemoteSigningRequest(ExternalSigningRequest externalSigningRequest)
        {
            try
            {
                Log.Information("Remote Signing initiate");
                if (externalSigningRequest.UserId != null)
                    externalSigningRequest.CivilId = await _IFileUpload.GetCivilId(externalSigningRequest.UserId);
                Log.Information("civilId" + externalSigningRequest.CivilId);
                DSPRequestLog requestLog = new DSPRequestLog();
                requestLog.DocumentId = externalSigningRequest.DocumentId;
                requestLog.CreatedBy = externalSigningRequest.CreatedBy;
                requestLog.CreatedDate = DateTime.Now;
                Dictionary<byte[], int> docWithPageCount = await GetDocumentAndPageNumber(externalSigningRequest.DocumentId);

                DSPRemoteSigning.SignatureAppearanceData signatureAppearance = new DSPRemoteSigning.SignatureAppearanceData();
                signatureAppearance.ImageData = Convert.FromBase64String(await GetSignatureImage(externalSigningRequest.UserId, 0, null));
                signatureAppearance.SelectedReason = externalSigningRequest.SelectedReason;
                signatureAppearance.PageNumber = docWithPageCount.Values.FirstOrDefault();

                DSPRemoteSigning.ExtendedRemoteSigningRequest extendedRemoteSigning = new DSPRemoteSigning.ExtendedRemoteSigningRequest();
                extendedRemoteSigning.SignatureProfileName = await _IFileUpload.GetSignatureProfileName((int)SigningMethodEnum.RemoteSigning);
                requestLog.ExternalId = extendedRemoteSigning.ExternalId = externalSigningRequest.CivilId + "_" + externalSigningRequest.DocumentId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                requestLog.CivilId = extendedRemoteSigning.UserID = externalSigningRequest.CivilId;
                extendedRemoteSigning.Data = docWithPageCount.Keys.FirstOrDefault();
                Log.Information("get document byte array success");
                extendedRemoteSigning.DataTitle = externalSigningRequest.DataTitle;
                extendedRemoteSigning.DataDescription = externalSigningRequest.DataDescription;
                extendedRemoteSigning.CallbackURL = _Config.GetValue<string>("callback_url") + "/DigitalSignature/SigningServiceResponse";
                extendedRemoteSigning.SignAppearanceDetails = signatureAppearance;
                StartSignResponse res = new StartSignResponse();
                using (var client = new RemoteSigningServiceClient())
                {
                    try
                    {
                        Log.Information("start client");
                        client.Endpoint.Binding.SendTimeout = new TimeSpan(0, 5, 0);
                        client.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 5, 0);
                        res = await client.StartSignDocumentAsync(extendedRemoteSigning);
                        Log.Information("receive response");
                        // write response in json file
                        var resfileName = "SignDocumentWithExternalUserAsyncAsync.json";
                        var jsonString = JsonConvert.SerializeObject(res, _options);
                        System.IO.File.AppendAllText(resfileName, jsonString);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                requestLog.RequestId = res.RequestId;
                requestLog.Status = res.Success;
                requestLog.ErrorCode = res.ErrorCode;
                requestLog.ErrorMessage = res.ErrorMessage;
                requestLog.SigningMethodId = (int)SigningMethodEnum.RemoteSigning;
                if (res.Success)
                {
                    Log.Information("Success");
                    requestLog.RequestStatus = SigningRequestStatusEnum.Initiated.GetDisplayName();
                    await _IFileUpload.SaveDSPRequestLog(requestLog);
                    return Ok(res.RequestId);
                }
                else
                {
                    Log.Information("fail");
                    requestLog.RequestStatus = SigningRequestStatusEnum.Failed.GetDisplayName();
                    await _IFileUpload.SaveDSPRequestLog(requestLog);
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region ExternalAuthentication 
        [HttpPost("InitiateAuthRequestPN")]
        [AllowAnonymous]
        public async Task<IActionResult> InitiateAuthRequestPN(AuthenticateRequestVM authRequestDataVM)
        {
            var authRequestData = _mapper.Map<AuthenticateRequest>(authRequestDataVM);
            authRequestData.PersonCivilNo = await _IFileUpload.GetCivilId(authRequestDataVM.UserId);
            authRequestData.SPCallbackURL = _Config.GetValue<string>("callback_url") + "/DigitalSignature/AuthenticationServiceCallback";

            DSPAuthenticationRequestLog authenticationRequestLog = new DSPAuthenticationRequestLog();
            using (var client = new MIDWrapperClient())
            {
                try
                {
                    client.Endpoint.Binding.SendTimeout = new TimeSpan(0, 5, 0);
                    client.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 5, 0);
                    MIDAuthSignClientResponseOfstring res = await client.InitiateAuthRequestPNWithAssuranceLevelAsync(authRequestData, (MIDAssuranceLevel)20);
                    // write response in json file
                    var resfileName = "InitiateAuthRequestPN.json";
                    var jsonString = JsonConvert.SerializeObject(res, _options);
                    System.IO.File.AppendAllText(resfileName, jsonString);

                    authenticationRequestLog.RequestId = res.Data;
                    Log.Information("res:" + res.ToString());
                    Log.Information("data:" + res.Data.ToString());
                    if (res.Error != null)
                    {
                        Log.Information("error message:" + res.Error.Message);
                        Log.Information("error code:" + res.Error.Code.ToString());
                        authenticationRequestLog.ErrorCode = res.Error.Code.ToString();
                    }
                    authenticationRequestLog.RequestPayload = Newtonsoft.Json.JsonConvert.SerializeObject(authRequestData);
                    authenticationRequestLog.CreatedDate = DateTime.Now;
                    await _IFileUpload.SaveDSPAuthenticationRequestLog(authenticationRequestLog);
                    return Ok(res.Data.ToString());
                }
                catch (Exception ex)
                {
                    Log.Information("exception:" + ex.Message);
                    Console.WriteLine(ex.Message);
                    return BadRequest(ex.Message);
                }
            }
        }


        [HttpPost("AuthenticationServiceCallback")]
        [AllowAnonymous]
        public async Task AuthenticationServiceCallback(DSPAuthenticationResponse results)
        {
            try
            {
                var JsonfileName = "AuthenticationResponse.json";
                var jsonString = JsonConvert.SerializeObject(results, _options);
                System.IO.File.AppendAllText(JsonfileName, jsonString);
                await _IFileUpload.UpdateDSPAuthenticationRequestLog(results);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Signature Verification
        [HttpGet("SignatureVerification")]
        [AllowAnonymous]
        public async Task<IActionResult> SignatureVerificationAsync(int DocumentId)
        {
            Log.Information("Signature Verification Start");
            Dictionary<byte[], int> docWithPageCount = await GetDocumentAndPageNumber(DocumentId);

            VerificationResult response = new VerificationResult();
            using (var client = new VerificationExtendedServicesContractClient())
            {
                try
                {
                    response = await client.VerifyPDFSignautreAsync(docWithPageCount.Keys.FirstOrDefault());
                    Log.Information("send request for signature verification");

                    // write response in json file
                    var resfileName = "SignatureVerification.json";
                    var jsonString = JsonConvert.SerializeObject(response, _options);
                    System.IO.File.AppendAllText(resfileName, jsonString);
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Log.Information(ex, "An error occurred while verifying the PDF signature");
                    return BadRequest(ex.Message);
                }
            }
        }
        #endregion

        private async Task<Dictionary<byte[], int>> GetDocumentAndPageNumber(int documentId)
        {
            Dictionary<byte[], int> base64PageCount = new Dictionary<byte[], int>();
            Log.Information("GetDocument Start");
            Log.Information("documentId" + documentId);
            var result = await _IFileUpload.GetUploadedAttachementById(documentId);
            var path = string.IsNullOrEmpty(result.SignedDocStoragePath) ? result.StoragePath : result.SignedDocStoragePath;
            var physicalPath = Path.Combine(_Config.GetValue<string>("dms_file_path") + path).Replace(@"\\", @"\");
#if !DEBUG
            physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
#endif
            Log.Information("GetDocument end");
            Log.Information("physicalPath" + physicalPath);

            //Descyption Key
            string password = _Config.GetValue<string>("DocumentEncryptionKey");
            var byteArray = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, result.DocType, password);
            var pageCount = 1;
            if (byteArray.Length > 0)
            {
                PdfLoadedDocument loadedDocument = new PdfLoadedDocument(byteArray);
                pageCount = loadedDocument.Pages.Count;
            }
            base64PageCount.Add(byteArray, pageCount);
            Log.Information("Add to dictionary successfully");
            return base64PageCount;
        }


        [HttpGet("GetDSPAuthenticationRequestLog")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDSPAuthenticationRequestLog(string requestId)
        {
            try
            {
                return Ok(await _IFileUpload.GetDSPAuthenticationRequestLog(requestId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetDSPSigningRequestStatus")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDSPSigningRequestStatus(string requestId)
        {
            try
            {
                return Ok(await _IFileUpload.GetDSPSigningRequestStatus(requestId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetIsAlreadySigned")]
        [AllowAnonymous]
        public async Task<IActionResult> GetIsAlreadySigned(string civilId, int documentId)
        {
            try
            {
                return Ok(await _IFileUpload.GetIsAlreadySigned(civilId, documentId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPost("UpdateDSPRequestLog")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateDSPRequestLog(string RequestId, string RequestStatus)
        {
            try
            {
                return Ok(await _IFileUpload.UpdateDSPRequestLog(RequestId, RequestStatus));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
