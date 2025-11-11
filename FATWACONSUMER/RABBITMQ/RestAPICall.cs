using FATWA_DOMAIN.Models.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using static FATWA_GENERAL.Helper.Request;
using static FATWA_GENERAL.Helper.Response;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace FATWACONSUMER.RABBITMQ
{
    public class RestAPICall
    {
        public RestAPICall() { }
        public async Task<ApiCallResponse> GetToken(string BaseURL, IdentityRequest identityUser)
        {
            try
            {
                var response = await new HttpClient().PostAsJsonAsync(BaseURL + "/Account/Login", identityUser);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<UserSucessResponse>();
                    return new ApiCallResponse { StatusCode = HttpStatusCode.OK, IsSuccessStatusCode = true, ResultData = data.Token };
                }
                else
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = false };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }

        public async Task<ApiCallResponse> GetEmailConfiguration(string token, int obj, string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url + "?ApplicationId=" + obj);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<EmailConfiguration>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = false };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }

        public async Task<ApiCallResponse> post1Data(string apiKey, object obj, string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
                request.Headers.Add("FatwaApiKey", apiKey);
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode};
                }
                else
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = false };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }

    }
}
