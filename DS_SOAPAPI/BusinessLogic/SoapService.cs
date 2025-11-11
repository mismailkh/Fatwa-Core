using System.ServiceModel;
using System.Text;

namespace DS_SOAPAPI.BusinessLogic
{

    [ServiceContract]
    public interface ISoapService
    {
        [OperationContract]
        bool VerifyToken(string userId, string sessionToken);
        [OperationContract]
        byte[] GetSignatureImage(string userId, int signatureImageTypeId, string sessionToken);
        [OperationContract]
        byte[] GetDocument(string? userId, string documentId, string? sessionToken);
        [OperationContract]
        int SubmitSignedDocumentVersion(LocalSigningRequest localSigningRequest);
    }

    public class SoapService : ISoapService
    {
        private readonly IConfiguration _config;
        public SoapService(IConfiguration configuration)
        {
            _config = configuration;
        }
        public bool VerifyToken(string userId, string sessionToken)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("dms_api_url") + "/DigitalSignature/VerifyToken?userId=" + userId + "&sessionToken=" + sessionToken);
                var response = new HttpClient().SendAsync(request);
                if (response.Result.IsSuccessStatusCode)
                {
                    var content = response.Result.Content.ReadFromJsonAsync<bool>();
                    return content.Result;
                }
                else
                {
                    // apply your logic
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public byte[] GetSignatureImage(string userId, int signatureImageTypeId, string sessionToken)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("dms_api_url") + "/DigitalSignature/GetSignatureImage?userId=" + userId + "&signatureImageTypeId=" + signatureImageTypeId + "&sessionToken=" + sessionToken);
                var response = new HttpClient().SendAsync(request);
                if (response.Result.IsSuccessStatusCode)
                {
                    var content = response.Result.Content.ReadAsStringAsync();
                    var bytes = Convert.FromBase64String(content.Result);

                    System.IO.File.WriteAllBytes(Directory.GetCurrentDirectory()+ "/" + "image.png", bytes);
                    return bytes;
                }
                else
                {
                    // apply your logic
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public byte[] GetDocument(string userId, string documentId, string sessionToken)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("dms_api_url") + "/DigitalSignature/GetDocument?userId=" + userId + "&documentId=" + documentId + "&sessionToken=" + sessionToken);
                var response = new HttpClient().SendAsync(request);
                if (response.Result.IsSuccessStatusCode)
                {
                    var content = response.Result.Content.ReadAsStringAsync();
                    var bytes = Convert.FromBase64String(content.Result);
                    
                    return bytes;
                }
                else
                {
                    // apply your logic
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int SubmitSignedDocumentVersion(LocalSigningRequest localSigningRequest)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("dms_api_url") + "/DigitalSignature/SubmitSignedDocumentVersion");
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(localSigningRequest), Encoding.UTF8, "application/json");
                var response = new HttpClient().SendAsync(request);
                if (response.Result.IsSuccessStatusCode)
                {
                    var content = response.Result.Content.ReadFromJsonAsync<int>();
                    return content.Result;
                }
                else
                {
                    // apply your logic
                    return 1; // 1 mean error because Diyar consider 0 as success
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
