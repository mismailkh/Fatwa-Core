using Blazored.LocalStorage;
using FATWA_ADMIN.Data;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Services.General
{
    //<History Author = 'Hassan Abbas' Date='2022-04-20' Version="1.0" Branch="master">File upload service for performing attachement operations</History>
    public partial class FileUploadService
    {
        private readonly IConfiguration _config;
        private readonly NavigationManager navigationManager;
        private readonly TranslationState translationState;
        private readonly ILocalStorageService browserStorage;

        public FileUploadService(IConfiguration configuration, NavigationManager _navigationManager, TranslationState _translationState, ILocalStorageService _browserStorage)
        {
            _config = configuration;
            navigationManager = _navigationManager;
            translationState = _translationState;
            browserStorage = _browserStorage;
        }

        #region Get Attachment Type Functions

        private static List<AttachmentType> AllTypes { get; set; }

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Remote Type Data for Add Attachment Drop Down</History>
        public async Task<DataEnvelope<AttachmentType>> GetAttachmentTypeItems(DataSourceRequest request, int? ModuleId)
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("dms_api_url") + "/FileUpload/GetAttachmentTypes?ModuleId=" + ModuleId);
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(translationState.Translate("Contact_Administrator"));
            }
            AllTypes = await response.Content.ReadFromJsonAsync<List<AttachmentType>>();
            var result = await AllTypes.ToDataSourceResultAsync(request);
            DataEnvelope<AttachmentType> dataToReturn = new DataEnvelope<AttachmentType>
            {
                Data = result.Data.Cast<AttachmentType>().ToList(),
                Total = result.Total
            };
            return await Task.FromResult(dataToReturn);
        }
        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Filter Item based on value</History>
        public async Task<AttachmentType> GetAttachmentTypeItemFromValue(int selectedValue)
        {
            if (AllTypes != null)
            {
                return await Task.FromResult(AllTypes.FirstOrDefault(x => selectedValue == x.AttachmentTypeId));
            }
            else
            {
                return await Task.FromResult<AttachmentType>(null);
            }
        }


        #endregion

        #region Upload Attachment to Actual Table

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Upload</History>
        public async Task<ApiCallResponse> UploadTempAttachmentToUploadedDocument(Guid referenceId)
        {
            try
            {
                var createdBy = await browserStorage.GetItemAsync<string>("User");
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("dms_api_url") + "/FileUpload/UploadTempAttachmentToUploadedDocument?referenceId=" + referenceId + "&createdBy=" + createdBy);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }

            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }

        public async Task<ApiCallResponse> SaveTempAttachementToUploadedDocument(FileUploadVM item)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("dms_api_url") + "/FileUpload/SaveTempAttachementToUploadedDocument");
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(item), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", item.Token != null ? item.Token : await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }

        #endregion

        #region File Remove
        //<History Author = 'Hassan Abbas' Date='2022-04-20' Version="1.0" Branch="master">Remove attachement</History>
        public async Task<ApiCallResponse> RemoveDocument(string referenceGuid, bool isReferenceGuid)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("dms_api_url") + "/FileUpload/RemoveDocument");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                request.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("referenceGuid", referenceGuid.ToString()),
                        new KeyValuePair<string, string>("isReferenceGuid", isReferenceGuid.ToString()),
                    });

                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        //<History Author = 'ijaz Ahmad' Date='2023-03-29' Version="1.0" Branch="master">Remove attachement</History>
        public async Task<ApiCallResponse> RemoveDocument(string referenceGuid)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("dms_api_url") + "/FileUpload/RemoveLegislationDocument");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                request.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("referenceGuid", referenceGuid.ToString()),

                    });

                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion
        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Remote Type Data for Add Attachment Drop Down</History>
        public async Task<ApiCallResponse> GetAttachmentTypes(int? ModuleId, bool ShowHidden = false)
        {
            try
            {
                var userDetail = await browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("dms_api_url") + "/FileUpload/GetAttachmentTypes?ModuleId=" + ModuleId + "&SectorTypeId=" + userDetail.SectorTypeId + "&ShowHidden=" + ShowHidden);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var attachTypes = await response.Content.ReadFromJsonAsync<List<AttachmentType>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = attachTypes };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        public async Task<ApiCallResponse> GetAllAttachmentTypes()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("dms_api_url") + "/FileUpload/GetAllAttachmentTypes");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    var content = new List<AttachmentType>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<AttachmentType>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-04-20' Version="1.0" Branch="master">Remove temp attachement</History>
        public async Task<bool> RemoveTempAttachement(string fileName, string userName, string _uploadFrom, string _project, int? typeId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("dms_api_url") + "/FileUpload/Remove");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                request.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("_fileToRemove", fileName),
                        new KeyValuePair<string, string>("_userName", userName),
                        new KeyValuePair<string, string>("_uploadFrom", _uploadFrom),
                        new KeyValuePair<string, string>("_project", _project),
                        new KeyValuePair<string, string>("_typeId", (typeId != null ? typeId : 0).ToString())
                    });

                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                var content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-04-20' Version="1.0" Branch="master">Remove attachement</History>
        public async Task<bool> RemoveUploadedDocument(int uploadedDocumentId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("dms_api_url") + "/FileUpload/RemoveUploadedDocument");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                request.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("_uploadedDocumentId", uploadedDocumentId.ToString()),
                    });

                var response = await new HttpClient().SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-04-20' Version="1.0" Branch="master">Remove temp attachement</History>
        public async Task<ObservableCollection<TempAttachementVM>>  GetUploadedAttachements(bool IsLiterature, int? literatureId, Guid? guid)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("dms_api_url") + "/FileUpload/GetUploadedAttachements");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                request.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("_isLiterature", IsLiterature.ToString()),
                        new KeyValuePair<string, string>("_guid", guid.ToString()),
                        new KeyValuePair<string, string>("_literatureId", literatureId.ToString())
                    });

                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ObservableCollection<TempAttachementVM>>();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-04-20' Version="1.0" Branch="master">Fetch all Temp attachments with ReferenceGuid </History>
        //<History Author = 'AttiqueRehman' Date='14/01/2025' Version="1.0" Branch="master">Fetch specific Temp attachments with ReferenceGuid and AttachmentId for preview document </History>
        public async Task<ObservableCollection<TempAttachementVM>> GetTempAttachements(Guid? guid, int attachementId = 0)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("dms_api_url") + "/FileUpload/GetTempAttachements");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                request.Content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("_guid", guid.ToString()),
                new KeyValuePair<string, string>("_attachementId", attachementId.ToString())
                });
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ObservableCollection<TempAttachementVM>>();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        #region get document by Id for legal legislation
        public async Task<KayPublication> GetAttachementById(int Id)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("dms_api_url") + "/FileUpload/GetAttachementById");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                request.Content = new FormUrlEncodedContent(new[]{
                new KeyValuePair<string, string>("Id", Id.ToString())
            });
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<KayPublication>();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
        #region Get Uploaded Attachment By Id
        public async Task<UploadedDocument> GetUploadedAttachementById(int Id, Guid? referenceGuid = null)
        {
            try
            {
                var token = await browserStorage.GetItemAsync<string>("Token");
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("dms_api_url") + "/FileUpload/GetUploadedAttachementById");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                request.Content = new FormUrlEncodedContent(new[]
               {
                new KeyValuePair<string, string>("Id", Id.ToString()),
                new KeyValuePair<string, string>("_referenceGuid", referenceGuid.ToString())
                });
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<UploadedDocument>();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
        #region Get DMS Template List
        public async Task<ApiCallResponse> GetTemplatesList(TemplateListAdvanceSearchVM AdvanceSearchVM)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("dms_api_url") + "/FileUpload/GetTemplatesList");
                var postBody = AdvanceSearchVM;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<IEnumerable<DMSTemplateListVM>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }


            }
            catch (Exception ex)
            {

                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };

            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-09-03' Version="1.0" Branch="master">Populate Header Footer Templates</History>
        public async Task<ApiCallResponse> GetHeaderFooterTemplates()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("dms_api_url") + "/FileUpload/GetHeaderFooterTemplates");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var templates = await response.Content.ReadFromJsonAsync<List<CaseTemplate>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = templates };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion
        #region Update Template Status

        //<History Author = 'Muhammad Abuzar' Date='2024-01-02' Version="1.0" Branch="master">Update Template Status </History>
        public async Task<bool> UpdateTemplateStatus(bool isActive, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("dms_api_url") + "/FileUpload/UpdateTemplateStatus");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            request.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("isActive", isActive.ToString()),
                        new KeyValuePair<string, string>("id", id.ToString()),
                    });
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        #endregion
        #region Get Attachment Type Functions

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Remote Type Data for Add Attachment Drop Down</History>
        public async Task<ApiCallResponse> GetAttachmentTypesByModuleId(int? ModuleId, bool ShowHidden = false)
        {
            try
            {
                var userDetail = await browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("dms_api_url") + "/FileUpload/GetAttachmentTypes?ModuleId=" + ModuleId + "&SectorTypeId=" + null + "&ShowHidden=" + ShowHidden);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var attachTypes = await response.Content.ReadFromJsonAsync<List<AttachmentType>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = attachTypes };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }


        #endregion
        #region Get Parameters
        //<History Author = 'Hassan Abbas' Date='2023-08-16' Version="1.0" Branch="master">Populate Template Parameters</History>
        public async Task<ApiCallResponse> GetTemplateParameters(int? moduleId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("dms_api_url") + "/FileUpload/GetTemplateParameters?moduleId=" + moduleId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<CaseTemplateParameter>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
            }
            catch (Exception)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false };
            }
        }

        #endregion
        #region Get Template Details

        //<History Author = 'Hassan Abbas' Date='2023-08-25' Version="1.0" Branch="master"> Get Case Template Content</History>
        public async Task<ApiCallResponse> GetCaseTemplate(int templateId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("dms_api_url") + "/FileUpload/GetCaseTemplate?templateId=" + templateId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var templates = await response.Content.ReadFromJsonAsync<CaseTemplate>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = templates };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion
        #region Get Request Subtypes By RequestId
        public async Task<ApiCallResponse> GetRequestSubtypesByRequestId(int contracts)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetRequestSubtypesByRequestId?requestTypeId=" + contracts);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var contents = await response.Content.ReadFromJsonAsync<List<Subtype>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = contents };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion
        #region Save Case Template 

        //<History Author = 'Hassan Abbas' Date='2023-08-21' Version="1.0" Branch="master">Save Case Template </History>
        public async Task<ApiCallResponse> SaveCaseTemplate(CaseTemplate template)
        {
            try
            {
                template.CreatedBy = await browserStorage.GetItemAsync<string>("User");
                template.CreatedDate = DateTime.Now;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("dms_api_url") + "/FileUpload/SaveCaseTemplate");
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(template), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<CaseTemplate>();
                    return new ApiCallResponse { IsSuccessStatusCode = response.IsSuccessStatusCode, StatusCode = response.StatusCode, ResultData = content };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { IsSuccessStatusCode = response.IsSuccessStatusCode, StatusCode = response.StatusCode, ResultData = content };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion
        #region Get File types 

        //<History Author = 'Muhammad Zaeem' Date='2023-06-20' Version="1.0" Branch="master">Get File Types</History>    
        public async Task<ApiCallResponse> GetFileTypes()
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("dms_api_url") + "/FileUpload/GetFileTypes");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<DmsFileTypes>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = users };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }

        #endregion
    }
}
