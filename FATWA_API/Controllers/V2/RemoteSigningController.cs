using FATWA_DOMAIN.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RemoteSigningServiceReference;

namespace FATWA_API.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //<History Author = 'Nadia Gull' Date='2022-12-6' Version="2.0" Branch="master"> add Start signing Document</History>
    public class RemoteSigningController : ControllerBase
    {
        private readonly ITempFileUpload _tempFileUpload;

        private readonly IWebHostEnvironment _hostingEnv;

        public RemoteSigningController(ITempFileUpload tempFileUpload, IWebHostEnvironment hostingEnv)
        {
            _tempFileUpload = tempFileUpload;
            _hostingEnv = hostingEnv;
        }

        [HttpGet("Test")]
        [MapToApiVersion("2.0")]
        [AllowAnonymous]
        public async Task<IActionResult> Test()
        {
            return Ok("test");
        }

        [HttpGet("SignDocument")]
        [MapToApiVersion("2.0")]
        [AllowAnonymous]
        public async Task<StartSignDocumentResponse> SignDocument(int id)
        {
            var dbPath = await _tempFileUpload.GetUploadedDocument(id);
            var physicalPath = Path.Combine(Directory.GetCurrentDirectory() + dbPath);

            // pdf to byte array
            var pdfDocumentInByteArray = System.IO.File.ReadAllBytes(physicalPath);
            var res = StartSignDocument(pdfDocumentInByteArray);
            return res;
        }

        private StartSignDocumentResponse StartSignDocument(byte[] pdfDocumentInByteArray)
        {
            Random rd = new Random();
            int rand_num = rd.Next(1000, 2000);
            Console.WriteLine(rand_num);

            SignatureAppearanceData SignAppearanceDetails = new SignatureAppearanceData();
            SignAppearanceDetails.SelectedReason = "Fatwa Testing";
            SignAppearanceDetails.SelectedLocation = "DPSK";
            SignAppearanceDetails.PageNumber = 1;

            StartSignDocumentResponse res = new StartSignDocumentResponse();

            ExtendedRemoteSigningRequest req = new ExtendedRemoteSigningRequest();
            req.UserID = "dspadmin1";
            req.SignatureProfileName = "DPS Remote Signing";
            req.Data = pdfDocumentInByteArray;
            req.ExternalId = rand_num + "";
            req.DataTitle = "PO Signature Request"; //will be shown in mobile app
            req.DataDescription = "Please approve this to digitally sign PO #" + rand_num + " for Vendor XYZ"; //will be shown in mobile app
            req.CallbackURL = "http://115.186.185.190:1040/api/v1/Account/RemoteSigingServiceResponse";
            req.SignAppearanceDetails = SignAppearanceDetails;

            StartSignDocumentRequest startSignRequest = new StartSignDocumentRequest();
            startSignRequest.signerObj = req;

            using (var client = new RemoteSigningServiceClient())
            {
                try
                {
                    res = client.StartSignDocument(startSignRequest);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return res;
        }
    }
}
