using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace FATWA_GENERAL.Helper
{
    public class Response
    {
        public class RequestFailedResponse
        {
            public IEnumerable<String> Errors { get; set; }
            public string ErrorMessage { get; set; }
        }
        public class UserSucessResponse
        {
            public string Token { get; set; }
            public string RefreshToken { get; set; }
            public string ProfilePicUrl { get; set; }
            public IdentityUser User { get; set; }
            public UserDetailVM? UserDetail { get; set; }
            public IEnumerable<ClaimSucessResponse> UserClaims { get; set; }
            public IEnumerable<TranslationSucessResponse> TranslationsList { get; set; }
        }
        public class Pagination
        {
            public int length { get; set; }
            public int size { get; set; }
            public int page { get; set; }
            public int lastPage { get; set; }
            public int startIndex { get; set; }
            public int endIndex { get; set; }

        }
        public class GenericResponseForCreateUpdate
        {
            public Guid Id { get; set; }
        }

        public class ApiResponse<T>
        {
            public int Count { get; set; }
            public IEnumerable<T> Value { get; set; }
        }

        //<History Author = 'Hassan Abbas' Date='2022-07-05' Version="1.0" Branch="master"> Response class for workflow activity</History>
        public class WorkflowActivityResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
        }
        //<History Author = 'Hassan Abbas' Date='2022-07-05' Version="1.0" Branch="master"> Response class for Api Call example used in MainLayout  > OnParametersSetAsync > GetRoleClaims</History>
        public class ApiCallResponse
        {
            public HttpStatusCode StatusCode { get; set; }
            public bool IsSuccessStatusCode { get; set; }
            public object ResultData { get; set; }
            public string Message { get; set; }
        }
        //<History Author = 'Hassan Abbas' Date='2022-07-21' Version="1.0" Branch="master"> Response class for Api Call example used in MainLayout  > OnParametersSetAsync > GetRoleClaims</History>
        public class BadRequestResponse
        {
            public string Message { get; set; }
            public string InnerException { get; set; }
            [NotMapped]
            public ErrorLog? errorLog { get; set; }
        }
        //<History Author = 'Hassan Abbas' Date='2022-07-21' Version="1.0" Branch="master"> Response class for Api Call example used in MainLayout  > OnParametersSetAsync > GetRoleClaims</History>
        public class FileUploadSuccessResponse
        {
            public string StoragePath { get; set; }
            public int AttachementId { get; set; }
        }
    }
}
