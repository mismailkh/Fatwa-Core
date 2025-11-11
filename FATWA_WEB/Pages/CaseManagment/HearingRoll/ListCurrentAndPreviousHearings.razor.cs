using Append.Blazor.Printing;
using AraibcPdfUnicodeGlyphsResharper;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.MojRollsVM;
using FATWA_GENERAL.Helper;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.PdfViewer;
using Syncfusion.Blazor.PdfViewerServer;
using System.Security.Cryptography;
using System.Text;
using Telerik.Blazor.Components;
using static FATWA_GENERAL.Helper.Response;
using ToolbarItem = Syncfusion.Blazor.PdfViewer.ToolbarItem;


namespace FATWA_WEB.Pages.CaseManagment.HearingRoll
{
    //<History Author = 'Ammaar Naveed' Date='2024-03-26' Version="1.0" Branch="master"> Detail of upcoming hearings</History>
    public partial class ListCurrentAndPreviousHearings : ComponentBase
    {

        #region Variables
        protected List<OutcomeAndHearingVM> UpcomingHearings;
        protected List<OutcomeAndHearingVM> PreviousHearings;
        protected RadzenDataGrid<MOJRollsRequestListVM>? gridCurrentHearinggrolls;
        public TelerikPdfViewer PdfViewerRef { get; set; }
        public byte[] FileData { get; set; }
        public bool ShowDocumentViewer { get; set; }
        public string DocumentPath { get; set; }
        public SfPdfViewerServer viewer { get; set; } = new SfPdfViewerServer();
        public bool DisplayDocumentViewer { get; set; }
        public bool busyPreviewBtn { get; set; }
        public bool isVisible { get; set; }
        public DateTime? SessionDate;
        public string Chamber_Name { get; set; }
        public string ChamberTypeCode_Name { get; set; }
        public string Court_Name { get; set; }
        List<CmsPrintHearingRollDetailVM> canAndCases = new List<CmsPrintHearingRollDetailVM>();
        List<ToolbarItem> ToolbarItems = new List<ToolbarItem>();
        #endregion

        #region On Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await LoadCurrentHearingRolls();
            spinnerService.Hide();

        }
        #endregion

        #region Populate Grid
        //<History Author = 'Ammaar Naveed' Date='2024-04-03' Version="1.0" Branch="master"> Populate upcoming & previous hearings of logged in user on the basis of condition.</History>
        //<History Author = 'Ammaar Naveed' Date='2024-03-26' Version="1.0" Branch="master"> Populate upcoming hearings of logged in user.</History>
        protected async Task PopulateHearingsGrid(bool isPrevious)
        {
            string LawyerId = loginState.UserDetail.UserId;
            var response = await cmsRegisteredCaseService.GetHearingsOfLawyer(LawyerId, isPrevious);
            if (response.IsSuccessStatusCode)
            {
                var hearings = (List<OutcomeAndHearingVM>)response.ResultData;
                if (isPrevious)
                {
                    PreviousHearings = hearings;
                }
                else
                {
                    UpcomingHearings = hearings;
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {

        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
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
        IEnumerable<MOJRollsRequestListVM> _getMojCurrentHearingRollsList;
        IEnumerable<MOJRollsRequestListVM> _getMojPreviousHearingRollsList;
        IEnumerable<MOJRollsRequestListVM> FilteredCurrentHearingRollsList { get; set; } = new List<MOJRollsRequestListVM>();
        IEnumerable<MOJRollsRequestListVM> FilteredPreviousHearingRollsList { get; set; } = new List<MOJRollsRequestListVM>();
        protected IEnumerable<MOJRollsRequestListVM> getMojCurrentHearingRollsList
        {
            get
            {
                return _getMojCurrentHearingRollsList;
            }
            set
            {
                if (!object.Equals(_getMojCurrentHearingRollsList, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getMojCurrentHearingRollsList", NewValue = value, OldValue = _getMojCurrentHearingRollsList };
                    _getMojCurrentHearingRollsList = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        protected IEnumerable<MOJRollsRequestListVM> getMojPreviousHearingRollsList
        {
            get
            {
                return _getMojPreviousHearingRollsList;
            }
            set
            {
                if (!object.Equals(_getMojPreviousHearingRollsList, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getMojPreviousHearingRollsList", NewValue = value, OldValue = _getMojPreviousHearingRollsList };
                    _getMojPreviousHearingRollsList = value;
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

                    FilteredCurrentHearingRollsList = await gridSearchExtension.Filter(getMojCurrentHearingRollsList, new Query()
                    {
                        Filter = $@"i => (i.ChamberNumber != null && i.ChamberNumber.ToString().ToLower().Contains(@0)) || (i.CourtName_Ar != null && i.CourtName_Ar.ToLower().Contains(@1))",
                        FilterParameters = new object[] { search, search, search }
                    });
                }
                else
                {
                    FilteredCurrentHearingRollsList = await gridSearchExtension.Filter(getMojCurrentHearingRollsList, new Query()
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
        protected async Task OnSearchInputPreviousRolls()
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                    search = "";
                else
                    search = search.ToLower();
                if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                {

                    FilteredCurrentHearingRollsList = await gridSearchExtension.Filter(getMojCurrentHearingRollsList, new Query()
                    {
                        Filter = $@"i => (i.ChamberNumber != null && i.ChamberNumber.ToString().ToLower().Contains(@0)) || (i.CourtName_Ar != null && i.CourtName_Ar.ToLower().Contains(@1))",
                        FilterParameters = new object[] { search, search, search }
                    });
                }
                else
                {
                    FilteredCurrentHearingRollsList = await gridSearchExtension.Filter(getMojCurrentHearingRollsList, new Query()
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
        protected async Task LoadCurrentHearingRolls()
        {

            translationState.TranslateGridFilterLabels(gridCurrentHearinggrolls);
            var response = await mojRollsService.GetMojCurrentPreviousHearingRollsList(loginState.UserDetail.UserId, false);
            if (response.IsSuccessStatusCode)
            {
                getMojCurrentHearingRollsList = (IEnumerable<MOJRollsRequestListVM>)response.ResultData;
                FilteredCurrentHearingRollsList = getMojCurrentHearingRollsList;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task LoadPreviousHearingRolls()
        {
            translationState.TranslateGridFilterLabels(gridCurrentHearinggrolls);
            var response = await mojRollsService.GetMojCurrentPreviousHearingRollsList(loginState.UserDetail.UserId, true);
            if (response.IsSuccessStatusCode)
            {
                getMojPreviousHearingRollsList = (IEnumerable<MOJRollsRequestListVM>)response.ResultData;
                FilteredPreviousHearingRollsList = getMojPreviousHearingRollsList;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Grid Actions
        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Add Outcome</History>
        protected async Task AddOutcome(MOJRollsRequestListVM args)
        {
            dataCommunicationService.hearingRollRequest = args;
            navigationManager.NavigateTo("add-hearingoutcome");
        }
        #endregion

        #region Tabs Change 
        //<History Author = 'Ammaar Naveed' Date='2024-04-03' Version="1.0" Branch="master"> Get respective data on tab change.</History>
        protected async Task TabChanged(int index)
        {
            if (index == 0) // Upcoming Hearings tab
            {
                await LoadCurrentHearingRolls();
            }
            else if (index == 1) // Previous Hearings tab
            {
                await LoadPreviousHearingRolls();
            }
        }
        #endregion

        #region view File
        /*<History Author = 'Hassan Abbas' Date='2024-03-14' Version="1.0" Branch="master"> Append Custom Action into the Pdf Viewer Toolbar through Html</History>*/
        protected async Task DocumentLoaded(LoadEventArgs args)
        {
            await JSRuntime.InvokeVoidAsync("addRotateButtonToPdfToolbar");
        }
        protected async Task ViewAttachement(MOJRollsRequestListVM mojrollsrequest)
        {
            try
            {
                var result = FilteredCurrentHearingRollsList.FirstOrDefault(x => x.Id == mojrollsrequest.Id) ?? FilteredPreviousHearingRollsList.FirstOrDefault(x => x.Id == mojrollsrequest.Id);

                if (result != null)
                {
                    DateTime SDate = (DateTime)result.SessionDate;
                    SessionDate = Convert.ToDateTime(SDate.ToString("dd-MM-yyyy"));
                    Chamber_Name = result.ChamberName_Ar;
                    ChamberTypeCode_Name = result.ChamberNumber;
                    Court_Name = result.CourtName_Ar;
                    var response = new ApiCallResponse();
                    if (result.DocumentId != null)
                    {
                        response = await mojRollsService.GetMojRollAttachements(result.DocumentId);
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
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

        #region Print Hearing Roll Detail
        protected async Task PrintDetails(MOJRollsRequestListVM hearingRollData)
        {
            try
            {
                CmsHearingRollDetailSearchVM cmsHearingRollDetailSearch = new CmsHearingRollDetailSearchVM
                {
                    HearingDate = (DateTime)hearingRollData.SessionDate,
                    ChamberNumberId = (int)hearingRollData.ChamberNumberId
                };
                var responseHearingDetail = await mojRollsService.GetHearingRollDetailForPrintingAndOutcome(cmsHearingRollDetailSearch);
                if (responseHearingDetail.IsSuccessStatusCode)
                {
                    canAndCases = (List<CmsPrintHearingRollDetailVM>)responseHearingDetail.ResultData;
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    DateTime now = DateTime.Now;
                    PdfSharp.Pdf.PdfDocument documentCoverLetter = new PdfSharp.Pdf.PdfDocument();
                    await CreateCoverLetter(documentCoverLetter, hearingRollData);
                    MemoryStream streamCoverLetter = new MemoryStream();
                    MemoryStream stream = new MemoryStream();
                    PdfSharp.Pdf.PdfDocument outPdf = new PdfSharp.Pdf.PdfDocument();
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    documentCoverLetter.Save(streamCoverLetter);
                    documentCoverLetter.Close();

                    using (PdfSharp.Pdf.PdfDocument inp = PdfReader.Open(streamCoverLetter, PdfDocumentOpenMode.Import))
                    using (outPdf)
                    {
                        CopyPages(inp, outPdf);
                    }

                    foreach (var file in canAndCases.Where(c => !String.IsNullOrEmpty(c.PortfolioStoragePath)))
                    {
                        //Encryption/Descyption Key
                        string password = _config.GetValue<string>("DocumentEncryptionKey");
                        UnicodeEncoding UE = new UnicodeEncoding();
                        byte[] key = UE.GetBytes(password);
                        RijndaelManaged RMCrypto = new RijndaelManaged();
                        MemoryStream fsOut = new MemoryStream();
                        int data;
#if DEBUG
                        {
                            var physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + file.PortfolioStoragePath).Replace(@"\\", @"\");
                            FileStream fsCrypt = new FileStream(physicalPath, FileMode.Open);
                            CryptoStream cs = new CryptoStream(fsCrypt,
                                RMCrypto.CreateDecryptor(key, key),
                                CryptoStreamMode.Read);
                            while ((data = cs.ReadByte()) != -1)
                                fsOut.WriteByte((byte)data);

                            using (PdfSharp.Pdf.PdfDocument inp = PdfReader.Open(fsOut, PdfDocumentOpenMode.Import))
                            using (outPdf)
                            {
                                CopyPages(inp, outPdf);
                            }

                            fsOut.Close();
                            cs.Close();
                            fsCrypt.Close();
                        }
#else
                    {
                        var physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + file.PortfolioStoragePath).Replace(@"\\", @"\");
                        physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                        using var httpClient = new HttpClient();
                        var response = await httpClient.GetAsync(physicalPath);
                        if (response.IsSuccessStatusCode)
                        {
                            var fsCrypt = await response.Content.ReadAsStreamAsync();
                            CryptoStream cs = new CryptoStream(fsCrypt,
                                RMCrypto.CreateDecryptor(key, key),
                                CryptoStreamMode.Read);
                            while ((data = cs.ReadByte()) != -1)
                                fsOut.WriteByte((byte)data);

                            using (PdfSharp.Pdf.PdfDocument inp = PdfReader.Open(fsOut, PdfDocumentOpenMode.Import))
                            using (outPdf)
                            {
                                CopyPages(inp, outPdf);
                            }
                            fsOut.Close();
                            cs.Close();
                            fsCrypt.Close();
                        }
                    }
#endif
                    }
                    outPdf.Save(stream);
                    outPdf.Close();
                    stream.Close();
                    streamCoverLetter.Close();
                    var FileData = stream.ToArray();
                    await PrintingService.Print(new PrintOptions(Convert.ToBase64String(FileData)) { Base64 = true });
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(responseHearingDetail);
                }
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


        void CopyPages(PdfSharp.Pdf.PdfDocument from, PdfSharp.Pdf.PdfDocument to)
        {
            for (int i = 0; i < from.PageCount; i++)
            {
                to.AddPage(from.Pages[i]);
            }
        }
        public static byte[] Combine(byte[] first, byte[] second)
        {
            if (first == null)
                return second;

            byte[] bytes = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
            return bytes;
        }

        //<History Author = 'Hassan Abbas' Date='2023-03-21' Version="1.0" Branch="master">Create Cover Letter for Portfolio having header/footer and detail of documents like name, no of pages etc</History>
        //<History Author = 'Hassan Abbas' Date='2023-07-05' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then add details like page count</History>
        protected async Task CreateCoverLetter(PdfSharp.Pdf.PdfDocument document, MOJRollsRequestListVM hearingRollData)
        {
            PdfSharp.Pdf.PdfPage page = document.AddPage(new PdfSharp.Pdf.PdfPage { Size = PdfSharp.PageSize.A4 });
            XGraphics gfx = XGraphics.FromPdfPage(page);
            // HACK²
            gfx.MUH = PdfFontEncoding.Unicode;
            MigraDoc.DocumentObjectModel.Document doc = new MigraDoc.DocumentObjectModel.Document();
            Section section = doc.AddSection();
            section.PageSetup.DifferentFirstPageHeaderFooter = false;

            // Header image
            Image image = section.Headers.Primary.AddImage(Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\images\\fatwaPortfolioHeader.PNG"));
            image.Height = "3cm";
            image.Width = "17cm";
            image.LockAspectRatio = true;
            image.RelativeVertical = RelativeVertical.Line;
            image.RelativeHorizontal = RelativeHorizontal.Margin;
            image.Top = ShapePosition.Top;
            image.Left = ShapePosition.Right;
            image.WrapFormat.Style = WrapStyle.Through;
            // Footer image
            Image imageFooter = section.Footers.Primary.AddImage(Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\images\\fatwaPortfolioFooter.PNG"));
            imageFooter.Height = "0.8cm";
            imageFooter.Width = "18cm";
            imageFooter.LockAspectRatio = true;
            imageFooter.RelativeVertical = RelativeVertical.Line;
            imageFooter.RelativeHorizontal = RelativeHorizontal.Margin;
            imageFooter.Top = ShapePosition.Top;
            imageFooter.Left = ShapePosition.Right;
            imageFooter.WrapFormat.Style = WrapStyle.Through;
            //Add a single paragraph with some text and format information.
            MigraDoc.DocumentObjectModel.Paragraph paraSessionDateLabel = section.AddParagraph();
            paraSessionDateLabel.Format.Alignment = ParagraphAlignment.Right;
            paraSessionDateLabel.Format.Font.Name = "Arial";
            paraSessionDateLabel.Format.Font.Size = 10;
            paraSessionDateLabel.Format.Font.Bold = true;
            paraSessionDateLabel.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.Black;
            paraSessionDateLabel.AddText("تاريخ الجلسة: ".ArabicWithFontGlyphsToPfd());
            paraSessionDateLabel.AddLineBreak();
            MigraDoc.DocumentObjectModel.Paragraph paraSessionDate = section.AddParagraph();
            paraSessionDate.Format.Alignment = ParagraphAlignment.Right;
            paraSessionDate.Format.Font.Name = "Arial";
            paraSessionDate.Format.Font.Size = 10;
            paraSessionDate.Format.Font.Bold = true;
            paraSessionDate.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.Black;
            paraSessionDate.AddText(Convert.ToDateTime(hearingRollData.SessionDate).ToString("dd/MM/yyyy"));
            paraSessionDate.AddLineBreak();
            MigraDoc.DocumentObjectModel.Paragraph paraCourtName = section.AddParagraph();
            paraCourtName.Format.Alignment = ParagraphAlignment.Right;
            paraCourtName.Format.Font.Name = "Arial";
            paraCourtName.Format.Font.Size = 10;
            paraCourtName.Format.Font.Bold = true;
            paraCourtName.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.Black;
            paraCourtName.AddText(("المحكمة: " + hearingRollData.CourtName_Ar).ArabicWithFontGlyphsToPfd());
            paraCourtName.AddLineBreak();
            MigraDoc.DocumentObjectModel.Paragraph paraChamberName = section.AddParagraph();
            paraChamberName.Format.Alignment = ParagraphAlignment.Right;
            paraChamberName.Format.Font.Name = "Arial";
            paraChamberName.Format.Font.Size = 10;
            paraChamberName.Format.Font.Bold = true;
            paraChamberName.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.Black;
            paraChamberName.AddText(("الدائرة: " + hearingRollData.ChamberName_Ar).ArabicWithFontGlyphsToPfd());
            paraChamberName.AddLineBreak();
            MigraDoc.DocumentObjectModel.Paragraph paraChamberNumber = section.AddParagraph();
            paraChamberNumber.Format.Alignment = ParagraphAlignment.Right;
            paraChamberNumber.Format.Font.Name = "Arial";
            paraChamberNumber.Format.Font.Size = 10;
            paraChamberNumber.Format.Font.Bold = true;
            paraChamberNumber.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.Black;
            paraChamberNumber.AddText(("رقم الدائرة: " + hearingRollData.ChamberNumber).ArabicWithFontGlyphsToPfd());
            paraChamberNumber.AddLineBreak();
            MigraDoc.DocumentObjectModel.Paragraph paraLawyerName = section.AddParagraph();
            paraLawyerName.Format.Alignment = ParagraphAlignment.Right;
            paraLawyerName.Format.Font.Name = "Arial";
            paraLawyerName.Format.Font.Size = 10;
            paraLawyerName.Format.Font.Bold = true;
            paraLawyerName.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.Black;
            paraLawyerName.AddText(("تم حضور الجلسة من قبل: " + hearingRollData.LawyerFullNameAr).ArabicWithFontGlyphsToPfd());
            paraLawyerName.AddLineBreak();

            MigraDoc.DocumentObjectModel.Tables.Table table = section.AddTable();
            // Create the item table
            table.Style = "Table";
            table.Borders.Width = 0.25;
            table.Borders.Left.Width = 0.5;
            table.Borders.Right.Width = 0.5;
            table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            MigraDoc.DocumentObjectModel.Tables.Column column =

            table.AddColumn("3.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = table.AddColumn("6.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = table.AddColumn("4cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = table.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = table.AddColumn("2.5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = table.AddColumn("1cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            // Create the header of the table
            MigraDoc.DocumentObjectModel.Tables.Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Right;
            row.Format.Font.Bold = true;
            row.Cells[0].AddParagraph("حافظة مستندات".ArabicWithFontGlyphsToPfd());
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[1].AddParagraph("ملخص ما هو مطلوب".ArabicWithFontGlyphsToPfd());
            row.Cells[1].Format.Font.Bold = true;
            row.Cells[1].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[2].AddParagraph("اسم العضو المختص".ArabicWithFontGlyphsToPfd());
            row.Cells[2].Format.Font.Bold = true;
            row.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[3].AddParagraph("رقم القضية".ArabicWithFontGlyphsToPfd());
            row.Cells[3].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[3].Format.Font.Bold = true;
            row.Cells[4].AddParagraph("الرقم الآلي للقضية".ArabicWithFontGlyphsToPfd());
            row.Cells[4].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[4].Format.Font.Bold = true;
            row.Cells[5].AddParagraph("م".ArabicWithFontGlyphsToPfd());
            row.Cells[5].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[5].Format.Font.Bold = true;

            for (int i = 0; i < canAndCases.Count(); i++)
            {
                row = table.AddRow();
                row.Cells[0].AddParagraph(String.IsNullOrEmpty(canAndCases[i].PortfolioStoragePath) ? "0" : "1");
                row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
                row.Cells[1].AddParagraph((String.IsNullOrEmpty(canAndCases[i].HearingDescription) ? "" : canAndCases[i].HearingDescription).ArabicWithFontGlyphsToPfd());
                row.Cells[1].Format.Alignment = ParagraphAlignment.Right;
                string? lawyerName = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? canAndCases[i].PrimaryLawyerNameEn : canAndCases[i].PrimaryLawyerNameAr;
                row.Cells[2].AddParagraph((String.IsNullOrEmpty(lawyerName) ? "" : lawyerName).ArabicWithFontGlyphsToPfd());
                row.Cells[2].Format.Alignment = ParagraphAlignment.Right;
                row.Cells[3].AddParagraph(canAndCases[i].CaseNumber);
                row.Cells[3].Format.Alignment = ParagraphAlignment.Right;
                row.Cells[4].AddParagraph(canAndCases[i].CANNumber);
                row.Cells[4].Format.Alignment = ParagraphAlignment.Right;
                row.Cells[5].AddParagraph((i + 1).ToString());
                row.Cells[5].Format.Alignment = ParagraphAlignment.Right;
                if (String.IsNullOrEmpty(canAndCases[i].HearingDescription) || canAndCases[i].HearingDescription == "-")
                {
                    row.Shading.Color = new Color(255, 100, 100);
                }
            }

            MigraDoc.Rendering.DocumentRenderer docRenderer = new MigraDoc.Rendering.DocumentRenderer(doc);
            docRenderer.PrepareDocument();

            // Render the paragraph. You can render tables or shapes the same way.
            docRenderer.RenderObject(gfx, XUnit.FromCentimeter(1.5), XUnit.FromCentimeter(0.5), "12cm", image);
            docRenderer.RenderObject(gfx, XUnit.FromCentimeter(1.5), XUnit.FromCentimeter(28.5), "12cm", imageFooter);
            docRenderer.RenderObject(gfx, XUnit.FromCentimeter(9), XUnit.FromCentimeter(5), "5cm", paraSessionDateLabel);
            docRenderer.RenderObject(gfx, XUnit.FromCentimeter(7.3), XUnit.FromCentimeter(5), "5cm", paraSessionDate);
            docRenderer.RenderObject(gfx, XUnit.FromCentimeter(3.5), XUnit.FromCentimeter(5), "5cm", paraLawyerName);
            docRenderer.RenderObject(gfx, XUnit.FromCentimeter(1.5), XUnit.FromCentimeter(4), "5cm", paraChamberNumber);
            docRenderer.RenderObject(gfx, XUnit.FromCentimeter(6.5), XUnit.FromCentimeter(4), "5cm", paraChamberName);
            docRenderer.RenderObject(gfx, XUnit.FromCentimeter(11.5), XUnit.FromCentimeter(4), "5cm", paraCourtName);
            docRenderer.RenderObject(gfx, XUnit.FromCentimeter(0.5), XUnit.FromCentimeter(6), "20cm", table);
        }
        #endregion
    }
}
