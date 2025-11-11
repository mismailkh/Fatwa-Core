using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.MojRollsVM;
using FATWA_GENERAL.Helper;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.PdfViewer;
using Syncfusion.Blazor.PdfViewerServer;
using Telerik.Blazor.Components;
using static FATWA_GENERAL.Helper.Response;
namespace FATWA_WEB.Pages.CaseManagment.HearingRoll
{
    public partial class MOjUpcomingHearingRollsList : ComponentBase
    {

        #region Variables 
        public DateTime? SessionDate;
        public TelerikPdfViewer PdfViewerRef { get; set; }
        public byte[] FileData { get; set; }
        public bool ShowDocumentViewer { get; set; }
        public string DocumentPath { get; set; }
        public SfPdfViewerServer viewer { get; set; } = new SfPdfViewerServer();
        public bool DisplayDocumentViewer { get; set; }
        public bool busyPreviewBtn { get; set; }
        public bool isVisible { get; set; }
        List<ToolbarItem> ToolbarItems = new List<ToolbarItem>();
        public string Chamber_Name { get; set; }
        public string ChamberTypeCode_Name { get; set; }
        public string Court_Name { get; set; }
        protected RadzenDataGrid<MOJRollsRequestListVM>? gridupcomingrolls;
        #endregion

        #region OnInitialized
        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                await LoadUpcomingHearing();
                System.Timers.Timer t_loadpending = new System.Timers.Timer();
                t_loadpending.Elapsed += async (s, e) =>
                {
                    await LoadUpcomingHearing();
                    await InvokeAsync(StateHasChanged);
                };
                t_loadpending.Interval = 1000 * 60 * 5; // Adjusted interval to 5 minutes
                t_loadpending.Start();
                spinnerService.Hide();
            }
            catch (Exception ex)
            {

                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
        #region Get UpComing Rolls
        string _search;
        protected string search
        {
            get
            {
                return _search;
            }
            set
            {
                if (!Equals(_search, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }


        IEnumerable<MOJRollsRequestListVM> _getMojUcompingRollsList;
        IEnumerable<MOJRollsRequestListVM> FilteredGetMojUpComingRollsList { get; set; } = new List<MOJRollsRequestListVM>();
        protected IEnumerable<MOJRollsRequestListVM> getMojUpComingRollsList
        {
            get
            {
                return _getMojUcompingRollsList;
            }
            set
            {
                if (!object.Equals(_getMojUcompingRollsList, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getMojUpComingRollsList", NewValue = value, OldValue = _getMojUcompingRollsList };
                    _getMojUcompingRollsList = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        protected async Task OnSearchInputUpcomingRolls()
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                    search = "";
                else
                    search = search.ToLower();
                if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                {

                    FilteredGetMojUpComingRollsList = await gridSearchExtension.Filter(getMojUpComingRollsList, new Query()
                    {
                        Filter = $@"i => (i.ChamberNumber != null && i.ChamberNumber.ToString().ToLower().Contains(@0)) || (i.CourtName_Ar != null && i.CourtName_Ar.ToLower().Contains(@1))",
                        FilterParameters = new object[] { search, search, search }
                    });
                }
                else
                {
                    FilteredGetMojUpComingRollsList = await gridSearchExtension.Filter(getMojUpComingRollsList, new Query()
                    {
                        Filter = $@"i => ( i.ChamberNumber != null && i.ChamberNumber.ToString().ToLower().Contains(@0) ) || ( i.CourtName_Ar != null && i.CourtName_Ar.ToLower().Contains(@1) )",
                        FilterParameters = new object[] { search, search, search }
                    });

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected async Task LoadUpcomingHearing()
        {
            translationState.TranslateGridFilterLabels(gridupcomingrolls);
            var response = await mojRollsService.GetMojUpocomingHearingRollsList(loginState.UserRoles.Any(u => u.RoleId == SystemRoles.FatwaAdmin) ? null : loginState.UserDetail.UserId);
            if (response.IsSuccessStatusCode)
            {
                getMojUpComingRollsList = (IEnumerable<MOJRollsRequestListVM>)response.ResultData;
                FilteredGetMojUpComingRollsList = getMojUpComingRollsList;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        #region on change


        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {

        }
        #endregion
        #region View File
        /*<History Author = 'Hassan Abbas' Date='2024-03-14' Version="1.0" Branch="master"> Append Custom Action into the Pdf Viewer Toolbar through Html</History>*/
        protected async Task DocumentLoaded(LoadEventArgs args)
        {
            await JSRuntime.InvokeVoidAsync("addRotateButtonToPdfToolbar");
        }

        //<History Author = 'ijaz Ahmad' Date='2023-05-23' Version="1.0" Branch="master"> File view</History>
        protected async Task ViewAttachement(MOJRollsRequestListVM mojrollsrequest)
        {
            try
            {
                var Result = FilteredGetMojUpComingRollsList.Where(x => x.Id == mojrollsrequest.Id).FirstOrDefault();
                if (Result != null)
                {
                    DateTime SDate = (DateTime)Result.SessionDate;
                    SessionDate = Convert.ToDateTime(SDate.ToString("dd-MM-yyyy"));
                    Chamber_Name = Result.ChamberName_Ar;
                    ChamberTypeCode_Name = Result.ChamberNumber;
                    Court_Name = Result.CourtName_Ar;
                    var response = new ApiCallResponse();
                    if (Result.DocumentId != null)
                    {
                        response = await mojRollsService.GetMojRollAttachements(Result.DocumentId);
                        if (response.IsSuccessStatusCode)
                        {
                            var theUpdatedItem = (TempAttachementVM)response.ResultData;
                            try
                            {
                                if (theUpdatedItem != null)
                                {
                                    ToolbarItems = new List<ToolbarItem>()
                                {
                                    ToolbarItem.PageNavigationTool,
                                    ToolbarItem.MagnificationTool,
                                    ToolbarItem.SelectionTool,
                                    ToolbarItem.PanTool,
                                    ToolbarItem.SearchOption,
                                    ToolbarItem.PrintOption,
                                    ToolbarItem.DownloadOption
                                };
                                    spinnerService.Show();
                                    DisplayDocumentViewer = false;
                                    busyPreviewBtn = true;
                                    await Task.Run(() => DecryptDocument(theUpdatedItem));
                                    await Task.Delay(1000);
                                    busyPreviewBtn = false;
                                    spinnerService.Hide();
                                }
                                else
                                {
                                    notificationService.Notify(new NotificationMessage()
                                    {
                                        Severity = NotificationSeverity.Error,
                                        Detail = translationState.Translate("File_Not_Found"),
                                        Summary = translationState.Translate("Error"),
                                        Style = "position: fixed !important; left: 0; margin: auto; "
                                    });
                                }

                            }
                            catch (Exception)
                            {

                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Error,
                                    Detail = translationState.Translate("Something_Went_Wrong"),
                                    Summary = translationState.Translate("Error"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                            }
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                    }
                    else
                    {
                        spinnerService.Show();
                        DisplayDocumentViewer = true;
                        spinnerService.Hide();

                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("File_Not_Found"),
                            Summary = translationState.Translate("Error"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }

                }

            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> View Attachment either Pdf or Image (read in stream and convert to PDF document) in Pdf Viewer</History>
        //<History Author = 'Hassan Abbas' Date='2023-06-20' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then view, also added functionality to convert .msg file to .pdf for preview</History>
        protected async Task DecryptDocument(TempAttachementVM theUpdatedItem)
        {
            try
            {
                var physicalPath = string.Empty;
#if DEBUG
                {
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + theUpdatedItem.StoragePath).Replace(@"\\", @"\");
                    
                }
#else
				{

                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + theUpdatedItem.StoragePath).Replace(@"\\", @"\");
                    physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
				}
#endif
                if (!String.IsNullOrEmpty(physicalPath))
                {
                    string base64String = await DocumentEncryptionService.GetDecryptedDocumentBase64(physicalPath, theUpdatedItem.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                    DocumentPath = "data:application/pdf;base64," + base64String;
                    DisplayDocumentViewer = true;
                }
                else
                {

                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("File_Not_Found"),
                        Summary = translationState.Translate("Error"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });


                }
            }
            catch (Exception)
            {

                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Summary = translationState.Translate("Error"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }


        }
        #endregion
        #region Redirect Function
        private void AddHearingRollls()
        {
            navigationManager.NavigateTo("/addhearing-rolls");
        }
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion
        #region Assignto lawyer
        //<History Author = 'ijaz Ahmad' Date='2023-05-23' Version="1.0" Branch="master"> Assign Hearing rolls to lawyer</History>
        protected async Task Assigntolawyer(MouseEventArgs args, dynamic data)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<AssignHearingRollsToLawyer>(

                translationState.Translate("Assign_To_Lawyer"),
                 new Dictionary<string, object>() {
                     { "ChamberNumberId", data.ChamberNumberId } ,
                     { "HearingDate",data.SessionDate}
                 },//Chamber Id
               new DialogOptions() { Width = "35% !important", CloseDialogOnOverlayClick = true, });
                await Task.Delay(200);
                await LoadUpcomingHearing();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
    }
}
