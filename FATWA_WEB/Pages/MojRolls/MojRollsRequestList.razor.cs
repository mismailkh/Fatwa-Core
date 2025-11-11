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
namespace FATWA_WEB.Pages.MojRolls
{
    public partial class MojRollsRequestList : ComponentBase
    {
        #region Variables 
        [Parameter]
        public dynamic Id { get; set; }
        public bool IsView { get; set; } = true;
        public DateTime? SessionDate;
        public int RollsId;
        public int ChamberId;
        public int ChamberTypeCodeId;
        public int CourtId;
        public DateTime Min = new DateTime(2001, 1, 1);
        public DateTime Minimum = new DateTime(1950, 1, 1);
        public int MOJRollsCourtsId { get; set; }
        public int MOJRollsChamberId { get; set; }
        public int MOJRollsChamberTypeId { get; set; }
        public int MOJRollsId { get; set; }
        DateTime? MOJRollsSessionDates;
        public int selectedIndex { get; set; } = 0;
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
        protected List<MOJRollsCourts> RmsCourts { get; set; } = new List<MOJRollsCourts>();
        protected RadzenDataGrid<MOJRollsRequestListVM> grid;
        protected RadzenDataGrid<MOJRollsRequestListVM>? gridcustomrolls;
        protected RadzenDataGrid<MOJRollsRequestListVM>? gridupcomingrolls;
        #endregion

        #region OnInitialized
        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                await PopulateRMSCourtsLookUP();
                System.Timers.Timer t_loadpending = new System.Timers.Timer();
                t_loadpending.Elapsed += async (s, e) =>
                {
                    await Load();
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
        #region on change
        public async Task OnTabChange(int index)
        {
            spinnerService.Show();
            search = "";
            selectedIndex = index;
            if (index == 1)
            {
                await Load();
            }
            else if (index == 2)
            {
                await LoadCustomRolls();
            }

            Reload();
            spinnerService.Hide();
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {

        }
        #endregion
        #region Drop Dwon List
        protected async Task PopulateRMSCourtsLookUP()
        {

            var response = await mojRollsService.GetRMSCourtsLookUP(loginState.UserRoles.Any(u => u.RoleId == SystemRoles.FatwaAdmin) ? null : loginState.UserDetail.UserId);
            if (response.IsSuccessStatusCode)
            {
                GetMOJRollsCourtsddl = (List<MOJRollsCourts>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulateCourts()
        {
            var response = await mojRollsService.GetCourt();
            if (response.IsSuccessStatusCode)
            {
                RmsCourts = (List<MOJRollsCourts>)response.ResultData;

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulateChambersLookuUp()
        {
            var response = await mojRollsService.GetRmsChambersLookuUp(loginState.UserRoles.Any(u => u.RoleId == SystemRoles.FatwaAdmin) ? null : loginState.UserDetail.UserId);
            if (response.IsSuccessStatusCode)
            {
                MOJRollsChamberddl = (List<MOJRollsChamberVM>)response.ResultData;
                RMSChambersDdl = MOJRollsChamberddl.Distinct(new MOJRollsChamberVMEqualityComparer()).ToList();
                MOJRollsChamberddl = RMSChambersDdl;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulateMojRolleChamberNumber()
        {

            var response = await mojRollsService.GetMojRolleChamberNumberByUserId(loginState.UserRoles.Any(u => u.RoleId == SystemRoles.FatwaAdmin) ? null : loginState.UserDetail.UserId);
            if (response.IsSuccessStatusCode)
            {
                MOJRollsChamberTypeCodeddl = (List<MOJRollsChamberTypeCode>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulateRMSChamberTypeCodesLookUP()
        {
            var response = await mojRollsService.GetRMSChamberTypeCodesLookUP();
            if (response.IsSuccessStatusCode)
            {
                MOJRollsChamberTypeCodeddl = (List<MOJRollsChamberTypeCode>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion 
        #region Lookups 
        public IEnumerable<MOJRollsCourts> GetMOJRollsCourtsddl;
        public IEnumerable<MOJRollsChamberVM> MOJRollsChamberddl;
        public IEnumerable<MOJRollsChamberVM> RMSChambersDdl;
        public IEnumerable<MOJRollsChamberTypeCode> MOJRollsChamberTypeCodeddl;
        public IEnumerable<MOJRollsVM> MOJRollsddl;
        public IEnumerable<FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups.MOJRollsChamberNumberVM> MOJRollsChamberVMNumbeddl;
        #endregion
        #region Get
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

        IEnumerable<MOJRollsRequestListVM> _getMojRollsRequestDetails;
        IEnumerable<MOJRollsRequestListVM> FilteredGetMojRollsList { get; set; } = new List<MOJRollsRequestListVM>();
        protected IEnumerable<MOJRollsRequestListVM> getMojRollsRequestDetails
        {
            get
            {
                return _getMojRollsRequestDetails;
            }
            set
            {
                if (!object.Equals(_getMojRollsRequestDetails, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getMojRollsRequestDetails", NewValue = value, OldValue = _getMojRollsRequestDetails };
                    _getMojRollsRequestDetails = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        protected async Task Load()
        {
            translationState.TranslateGridFilterLabels(grid);
            var response = await mojRollsService.GetAllRMSRequests(loginState.UserRoles.Any(u => u.RoleId == SystemRoles.FatwaAdmin) ? "Admin" : loginState.UserDetail.ActiveDirectoryUserName);
            if (response.IsSuccessStatusCode)
            {
                getMojRollsRequestDetails = (IEnumerable<MOJRollsRequestListVM>)response.ResultData;
                FilteredGetMojRollsList = getMojRollsRequestDetails;


            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task OnSearchInput()
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                    search = "";
                else
                    search = search.ToLower();
                if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                {

                    FilteredGetMojRollsList = await gridSearchExtension.Filter(getMojCustomRollsList, new Query()
                    {
                        Filter = $@"i => (i.ChamberNumber != null && i.ChamberNumber.ToString().ToLower().Contains(@0)) || (i.CourtName_Ar != null && i.CourtName_Ar.ToLower().Contains(@1))",
                        FilterParameters = new object[] { search, search, search }
                    });
                }
                else
                {
                    FilteredGetMojRollsList = await gridSearchExtension.Filter(getMojCustomRollsList, new Query()
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
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public class MOJRollsChamberVMEqualityComparer : IEqualityComparer<MOJRollsChamberVM>
        {
            public bool Equals(MOJRollsChamberVM x, MOJRollsChamberVM y)
            {
                if (x == null || y == null)
                    return false;

                return x.Id == y.Id && x.CourtId == y.CourtId;
            }

            public int GetHashCode(MOJRollsChamberVM obj)
            {
                if (obj == null)
                    return 0;

                return HashCode.Combine(obj.Id, obj.CourtId);
            }
        }

        public async Task OnChangeMOJRollsCourts(object value)
        {
            var str = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value.ToString();
            if (int.TryParse(str, out int formID))
            {
                var courttype = GetMOJRollsCourtsddl.Where(x => x.Id == formID).FirstOrDefault();
                var response = await mojRollsService.GetRMSRollsAgainstCourtTypeId((int)courttype.TypeId);
                if (response.IsSuccessStatusCode)
                {
                    MOJRollsddl = (List<MOJRollsVM>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                await PopulateChambersLookuUp();
                RMSChambersDdl = MOJRollsChamberddl.Where(x => x.CourtId == formID).ToList();
                if (RMSChambersDdl != null && RMSChambersDdl.Any())
                {
                    // Apply Distinct using the custom comparer
                    RMSChambersDdl = RMSChambersDdl.Distinct(new MOJRollsChamberVMEqualityComparer()).ToList();
                    MOJRollsChamberddl = RMSChambersDdl;

                }
                else
                {
                    await PopulateChambersLookuUp();
                }
                StateHasChanged();
                CourtId = formID;

                if (SessionDate != null && RollsId > 0 && ChamberId > 0 && ChamberTypeCodeId > 0 && CourtId > 0)
                {
                    IsView = false;
                }
            }
            else
            {

            }
        }
        public void OnChangeMOJRollsChamberTypeCode(object value)
        {
            var str = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;
            int formID = Convert.ToInt32(str);
            ChamberTypeCodeId = formID;

            if (SessionDate != null && RollsId! > 0 && ChamberId! > 0 && ChamberTypeCodeId! > 0 && CourtId! > 0)
            {
                IsView = false;
            }
        }
        public class MOJRollsChamberTypeCodeEqualityComparer : IEqualityComparer<MOJRollsChamberTypeCode>
        {
            public bool Equals(MOJRollsChamberTypeCode x, MOJRollsChamberTypeCode y)
            {
                if (x == null || y == null)
                    return false;

                return x.Id == y.Id && x.Name == y.Name && x.ChamberId == y.ChamberId;
            }

            public int GetHashCode(MOJRollsChamberTypeCode obj)
            {
                if (obj == null)
                    return 0;

                return HashCode.Combine(obj.Id, obj.Name, obj.ChamberId);
            }
        }

        public async Task OnChangeMOJRollsChamberVM(object value)
        {
            //MOJRollsChamberTypeCodeddl consider as Chamber Number 
            await PopulateMojRolleChamberNumber();

            // Apply distinct filter based on ChamberId
            MOJRollsChamberTypeCodeddl = MOJRollsChamberTypeCodeddl
                    .Where(x => x.ChamberId == (int)value)
                .Select(x => new MOJRollsChamberTypeCode
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .Distinct(new MOJRollsChamberTypeCodeEqualityComparer()) // Apply distinct using the custom comparer
                .ToList();

            StateHasChanged();

            var str = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;
            int formID = Convert.ToInt32(str);
            ChamberId = formID;

            if (SessionDate != null && RollsId! > 0 && ChamberId! > 0 && ChamberTypeCodeId! > 0 && CourtId! > 0)
            {
                IsView = false;
            }
        }
        void GetSessionDAte(DateTime? MOJRollsSessionDates, string name, string format)
        {
            SessionDate = MOJRollsSessionDates;

            if (SessionDate != null && RollsId != 0 && ChamberId != 0 && ChamberTypeCodeId != 0 && CourtId != 0)
            {
                IsView = false;
            }
        }
        public void OnChangeMOJRolls(object value)
        {
            var str = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;
            int formID = Convert.ToInt32(str);
            RollsId = formID;
            if (SessionDate != null && RollsId != 0 && ChamberId != 0 && ChamberTypeCodeId != 0 && CourtId != 0)
            {
                IsView = false;
            }

        }
        #endregion
        #region Create Request 
        protected async Task CreateRequest()
        {
            if (SessionDate != null && MOJRollsCourtsId != 0 && MOJRollsId != 0 && MOJRollsChamberId != 0 && MOJRollsChamberTypeId != 0)
            {
                IsView = true;
                MOJRollsRequest addDMOJRollsRequest = new MOJRollsRequest()
                {
                    SessionDate = SessionDate,
                    RollId_LookUp = RollsId,
                    CourtType_LookUp = CourtId,
                    ChamberType_LookUp = ChamberId,
                    ChamberTypeCode_LookUp = ChamberTypeCodeId,
                    RequestedBy = loginState.UserDetail.ActiveDirectoryUserName,
                    RequestStatus_LookUp = 1,// 1 for pending 
                    IsAssigned = false,
                    IsFatwaManual = true


                };
                var response = await mojRollsService.CreateRMSRequests(addDMOJRollsRequest);
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Request_Success_Massage"),
                        Style = "position: fixed !important; left: 0; right: 0; margin: auto; text-align: center;",
                    });
                    await Task.Delay(300);
                    ResetFormValues();
                    Reload();
                    OnTabChange(1);
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Fill_Required_Fields"),
                    Style = "position: fixed !important; left: 0; right: 0; margin: auto; text-align: center;",
                    Duration = 900
                });
            }
        }

        void ResetFormValues()
        {
            MOJRollsCourtsId = 0;
            MOJRollsId = 0;
            MOJRollsChamberId = 0;
            MOJRollsChamberTypeId = 0;
            RollsId = 0;
            CourtId = 0;
            ChamberId = 0;
            ChamberTypeCodeId = 0;
            MOJRollsSessionDates = null;
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
                SessionDate = (DateTime)mojrollsrequest.SessionDate;
                Chamber_Name = mojrollsrequest.ChamberName_Ar;
                ChamberTypeCode_Name = mojrollsrequest.ChamberNumber;
                Court_Name = mojrollsrequest.CourtName_Ar;
                var response = new ApiCallResponse();
                if (mojrollsrequest.DocumentId != null)
                {
                    response = await mojRollsService.GetMojRollAttachements(mojrollsrequest.DocumentId);
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
        protected async Task ViewCustomRollsAttachement(MOJRollsRequestListVM mojrollsrequest)
        {
            try
            {
                var Result = FilteredGetMojCustomRollsList.Where(x => x.Id == mojrollsrequest.Id).FirstOrDefault();
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
        #region Exception View
        //<History Author = 'ijaz Ahmad' Date='2023-05-23' Version="1.0" Branch="master"> Exception View</History>
        protected async Task ExceptionView(MouseEventArgs args, dynamic data)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<MojRollsExceptionView>(

                translationState.Translate("Request_Details"),
                 new Dictionary<string, object>() { { "Id", data.Id } },//RequestId
               new DialogOptions() { Width = "100% !important", CloseDialogOnOverlayClick = true, });
                await Task.Delay(200);
                await Load();
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
        #region Redirect Function
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

        #region Get Custom Rolls
        IEnumerable<MOJRollsRequestListVM> _getMojCustomRollsList;
        IEnumerable<MOJRollsRequestListVM> FilteredGetMojCustomRollsList { get; set; } = new List<MOJRollsRequestListVM>();
        IEnumerable<MOJRollsRequestListVM> MojUserSpecificUser { get; set; } = new List<MOJRollsRequestListVM>();
        protected IEnumerable<MOJRollsRequestListVM> getMojCustomRollsList
        {
            get
            {
                return _getMojCustomRollsList;
            }
            set
            {
                if (!object.Equals(_getMojCustomRollsList, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getMojRollesRequestList", NewValue = value, OldValue = _getMojCustomRollsList };
                    _getMojCustomRollsList = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected async Task LoadCustomRolls()
        {
            translationState.TranslateGridFilterLabels(grid);
            var response = await mojRollsService.GetCustomRollsRequestsList(loginState.UserRoles.Any(u => u.RoleId == SystemRoles.FatwaAdmin) ? null : loginState.UserDetail.UserId);

            if (response.IsSuccessStatusCode)
            {
                getMojCustomRollsList = (IEnumerable<MOJRollsRequestListVM>)response.ResultData;
                FilteredGetMojCustomRollsList = getMojCustomRollsList;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }


        protected async Task OnSearchInputCustomRolls()
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                    search = "";
                else
                    search = search.ToLower();
                if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                {

                    FilteredGetMojCustomRollsList = await gridSearchExtension.Filter(getMojCustomRollsList, new Query()
                    {
                        Filter = $@"i => (i.ChamberNumber != null && i.ChamberNumber.ToString().ToLower().Contains(@0)) || (i.CourtName_Ar != null && i.CourtName_Ar.ToLower().Contains(@1))",
                        FilterParameters = new object[] { search, search, search }
                    });
                }
                else
                {
                    FilteredGetMojCustomRollsList = await gridSearchExtension.Filter(getMojCustomRollsList, new Query()
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


        #endregion
    }
}
