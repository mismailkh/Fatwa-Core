using Blazored.LocalStorage;
using DocumentFormat.OpenXml.Wordprocessing;
using FATWA_DOMAIN.Models.Lms;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_WEB.Data;
using Syncfusion.XlsIO;
using System.Data;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Telerik.Blazor.Components.FileSelect.Stream;
using Telerik.Documents.SpreadsheetStreaming;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Services
{
    public class ExcelImportService
    {
        private readonly IConfiguration _config;
        private readonly ILocalStorageService _browserStorage;
        private readonly LoginState _loginState;
        public ImportStockTakingResponse importStockTakingResponse { get; set; }

        public ExcelImportService(IConfiguration config, ILocalStorageService browserStorage, LoginState loginState)
        {
            _config = config;
            _browserStorage = browserStorage;
            _loginState = loginState;
        }
        public async Task<ApiCallResponse> ImportFromExcel(FileInfoStream fileInfoStream)
        {
            try
            {
                var excelData = new List<ImportEmployeeTemplate>();
                var buffer = new byte[fileInfoStream.Length];
                await fileInfoStream.ReadAsync(buffer);
                MemoryStream stream = new MemoryStream(buffer);
                using (stream)
                {
                    ExcelEngine excelEngine = new ExcelEngine();
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Open(stream);
                    IWorksheet worksheet = workbook.Worksheets[0];
                    foreach (var row in worksheet.Rows.Skip(1))
                    {
                        var entity = new ImportEmployeeTemplate();
                        foreach (var propertyInfo in typeof(ImportEmployeeTemplate).GetProperties())
                        {
                            // Assuming the column names in the Excel file match the property names in the view model
                            var columnIndex = GetColumnIndex(worksheet, propertyInfo.Name);

                            if (columnIndex != -1)
                            {
                                var cellValue = row.Cells[columnIndex].Value.ToString();
                                if (propertyInfo.PropertyType == typeof(DateTime))
                                    propertyInfo.SetValue(entity, DateTime.Parse(cellValue));
                                else
                                    propertyInfo.SetValue(entity, Convert.ChangeType(cellValue, propertyInfo.PropertyType));

                            }
                            if (propertyInfo.Name == "CreatedBy")
                                propertyInfo.SetValue(entity, _loginState.UserDetail.Email);
                        }
                        excelData.Add(entity);
                    }
                }
                var response = await AddBulkEmployees(excelData);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = response.ResultData };
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

        private int GetColumnIndex(IWorksheet worksheet, string columnName)
        {
            var headerRow = worksheet.Rows[0];
            for (int i = 0; i < headerRow.Columns.Length; i++)
            {
                if (headerRow.Cells[i].Value.ToString() == columnName)
                {
                    return i; // Return 0-based index
                }
            }

            return -1; // Column not found
        }
        public async Task<ApiCallResponse> AddBulkEmployees(List<ImportEmployeeTemplate> employees)
        {
            try
            {
                bool cultureEn = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? true : false;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Users/AddBulkEmployees?cultureEn=" + cultureEn);
                var postBody = employees;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<string>>();
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
        public async Task<ImportStockTakingResponse> StockTakingImportFromExcel(FileInfoStream fileInfoStream, List<string> HeaderValues)
        {
            try
            {
                DataTable GridData = new DataTable();
                var excelData = new List<StockTakingImportTemplate>();
                var buffer = new byte[fileInfoStream.Length];
                await fileInfoStream.ReadAsync(buffer);
                MemoryStream stream = new MemoryStream(buffer);
                using (stream)
                {
                    ExcelEngine excelEngine = new ExcelEngine();
                    IWorkbook workbook = excelEngine.Excel.Workbooks.Open(stream);
                    IWorksheet worksheet = workbook.Worksheets[0];
                    var isValidFile = await CheckImportedFileValidation(worksheet, HeaderValues);
                    if (isValidFile)
                    {
                        foreach (var row in worksheet.Rows.Skip(1))
                        {
                            var entity = new StockTakingImportTemplate();
                            foreach (var propertyInfo in typeof(StockTakingImportTemplate).GetProperties())
                            {
                                // Assuming the column names in the Excel file match the property names in the view model
                                var columnIndex = GetColumnIndex(worksheet, propertyInfo.Name);

                                if (columnIndex != -1)
                                {
                                    var cellValue = row.Cells[columnIndex].Value.ToString();
                                    if (propertyInfo.PropertyType == typeof(DateTime))
                                        propertyInfo.SetValue(entity, DateTime.Parse(cellValue));
                                    else
                                        propertyInfo.SetValue(entity, Convert.ChangeType(cellValue, propertyInfo.PropertyType));
                                }
                            }
                            excelData.Add(entity);
                        }
                        return new ImportStockTakingResponse { IsStatusCode = true, stockTakingImports = excelData };
                    }
                    else
                    {
                        return new ImportStockTakingResponse { IsStatusCode = false, stockTakingImports = new List<StockTakingImportTemplate>() };

                    }
                }
            }
            catch (Exception ex)
            {
                return new ImportStockTakingResponse { IsStatusCode = false, stockTakingImports = new List<StockTakingImportTemplate>()};
            }
        }
        public async Task<bool> CheckImportedFileValidation(IWorksheet worksheet, List<string> HeaderValues)
        {
            try
            {
                var headerRow = worksheet.Rows[0];
                foreach (string headerValue in HeaderValues)
                {
                    bool headerValueFound = false;
                    for (int i = 0; i < headerRow.Columns.Length; i++)
                    {
                        if (headerRow.Cells[i].Value.ToString() == headerValue)
                        {
                            headerValueFound = true;
                            break;
                        }
                    }
                    if (!headerValueFound)
                    {
                        return false;
                    }
                }
                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }
    }
}
