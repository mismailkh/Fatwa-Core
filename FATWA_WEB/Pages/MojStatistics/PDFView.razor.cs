using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Microsoft.EntityFrameworkCore;
using Telerik.Blazor.Components;
using System.Security.Policy;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Buffers.Text;
using Microsoft.AspNetCore.Mvc;
using FATWA_DOMAIN.Models.ViewModel.MojStatistics;
using FATWA_DOMAIN.Models.ViewModel.MojStatistics;

namespace FATWA_WEB.Pages.MojStatistics
{
    //<History Author = 'Hassan Abbas' Date='2024-02-13' Version="1.0" Branch="master"> Migrated from Statistics Case Study Project</History>
    public partial class PDFView : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }


        [Parameter]
        public dynamic FilesId { get; set; }
        public string ReportURL { get; set; }
        public string ViewFileName { get; set; }

        public int valueDropdown { get; set; }
        public string SessionDate { get; set; }
        public string Roll_Name { get; set; }
        public string Chamber_Name { get; set; }
        public string ChamberTypeCode_Name { get; set; }
        public string Court_Name { get; set; }

        public TelerikPdfViewer PdfViewerRef { get; set; }
        public byte[] FileData { get; set; }
        public bool ShowDocumentViewer { get; set; }
        public string DocumentPath { get; set; }
        public bool DisplayDocumentViewer { get; set; }
        public IEnumerable<MojStatsFilesDetailsVM> filesDetails { get; set; } = new List<MojStatsFilesDetailsVM>();

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await viewFiles();
        }


        protected async Task viewFiles()
        {

            try
            {
                var response = await mojStatisticsService.GetAttachment(Convert.ToInt32(FilesId));
                if (response.IsSuccessStatusCode)
                {
                    var result = (List<MojStatsFilesDetailsVM>)response.ResultData;
                    var res = result.FirstOrDefault(x => x.FilesId == FilesId);

                    try
                    {

#if DEBUG
                        {



                            if (res.FileName != null)
                            {
                                string fileName = Path.GetFileName(res.FileName); //
                                var physicalPath = Path.Combine(Directory.GetCurrentDirectory() + res.FileName).Replace(@"\\", @"\");
                                if (System.IO.File.Exists(physicalPath))
                                {
                                    FileData = File.ReadAllBytes(physicalPath);
                                    DocumentPath = "data:application/pdf;base64," + Convert.ToBase64String(FileData);
                                    ShowDocumentViewer = true;
                                    //PdfViewerRef.Rebind();
                                    StateHasChanged();
                                }

                            }



                        }
#else
                        {



                            string fileName = Path.GetFileName(res.FileName);
                            // Construct the physical path of the file on the server
                            var physicalPath = Path.Combine(_config.GetValue<string>("file_path") + res.FileName).Replace(@"\\", @"\");
                        // Remove the wwwroot/Attachments part of the path to get the actual file path
                                physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", @"\");
                                     // Create a new HttpClient instance to download the file
                                    using var httpClient = new HttpClient();
                                    var httpresponse = await httpClient.GetAsync(physicalPath);
                                    // Check if the file was downloaded successfully
                                    if (httpresponse.IsSuccessStatusCode)
                                    {
                                        // Read the file content as a byte array
                                        var fileData = await httpresponse.Content.ReadAsByteArrayAsync();
                                        FileData = fileData;
                                        DocumentPath = "data:application/pdf;base64," + Convert.ToBase64String(FileData);
                                        ShowDocumentViewer = true;
                                        StateHasChanged();
                                    }


                                }

#endif


                    }
                    catch (Exception)
                    {

                        //notificationService.Notify(new NotificationMessage()
                        //{
                        //    Severity = NotificationSeverity.Error,
                        //    Detail = "Something Went Wrong",
                        //    Summary = "Error",
                        //    Style = "position: fixed !important; left: 0; margin: auto; "
                        //});
                    }
                }
                else
                {
                    //await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
    }
}
