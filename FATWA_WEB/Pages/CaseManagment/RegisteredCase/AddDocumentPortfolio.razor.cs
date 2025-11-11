using AraibcPdfUnicodeGlyphsResharper;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.PdfViewerServer;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    //<History Author = 'Hassan Abbas' Date='2023-03-20' Version="1.0" Branch="master"> Add Document Portfolio for a Hearing</History>
    public partial class AddDocumentPortfolio : ComponentBase
    {

        #region Parameters
        [Parameter]
        public string HearingId { get; set; }
        [Parameter]
        public string CaseId { get; set; }
        [Parameter]
        public string Date { get; set; }
        #endregion

        #region Variables
        protected CmsDocumentPortfolio documentPortfolio { get; set; } = new CmsDocumentPortfolio { UploadFrom = "CaseManagement" };
        protected List<AttachmentType> AttachmentTypes { get; set; }
        protected RadzenDataGrid<TempAttachementVM> gridAttachments { get; set; }
        protected ObservableCollection<TempAttachementVM> attachments { get; set; }
        public bool allowRowSelectOnRowClick = true;
        public TelerikPdfViewer PdfViewerRef { get; set; }
        public TelerikPdfViewer PdfViewerRef2 { get; set; }
        public byte[] FileDataViewer { get; set; }
        public bool DisplayDocumentViewer { get; set; }
        public bool busyPreviewBtn { get; set; }
        public bool isVisible { get; set; }
        public SfPdfViewerServer pdfViewer;
        public string DocumentPath { get; set; } = string.Empty;
        #endregion

        #region Component Load

        //<History Author = 'Hassan Abbas' Date='2023-03-21' Version="1.0" Branch="master"> Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            documentPortfolio.Name = "Document_Portfolio_" + Date;
            await PopulateAttachmentTypes();
            await PopulateAttachementsGrid();
            translationState.TranslateGridFilterLabels(gridAttachments);
            spinnerService.Hide();
        }

        #endregion

        #region Load Events

        //<History Author = 'Hassan Abbas' Date='2023-03-20' Version="1.0" Branch="master"> Populate Attachments Grid</History>
        protected async Task PopulateAttachementsGrid()
        {
            try
            {
                attachments = await fileUploadService.GetUploadedAttachements(false, 0, Guid.Parse(CaseId));
                if (attachments != null && attachments.Any())
                {
                    Regex regex = new Regex("_[0-9]+", RegexOptions.RightToLeft);
                    attachments = new ObservableCollection<TempAttachementVM>(attachments?.Select(f => { if (f.FileName.Contains("Document_Portfolio")) return f; else f.FileName = regex.Replace(f.FileName, "", 1); return f; }).ToList());
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }


        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Subtypes data</History>
        protected async Task PopulateAttachmentTypes()
        {
            var response = await lookupService.GetAttachmentTypes((int)WorkflowModuleEnum.CaseManagement);
            if (response.IsSuccessStatusCode)
            {
                AttachmentTypes = (List<AttachmentType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion

        #region Redirect Function
        //<History Author = 'Hassan Abbas' Date='2022-02-03' Version="1.0" Branch="master"> Redirect back to previous page from browser history</History>
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

        #region Document Events

        //<History Author = 'Hassan Abbas' Date='2023-03-20' Version="1.0" Branch="master"> View Attachment either Pdf or Image (read in stream and convert to PDF document) in Pdf Viewer</History>
        //<History Author = 'Hassan Abbas' Date='2023-07-05' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then  merge</History>
        protected async Task ViewAttachement(TempAttachementVM theUpdatedItem)
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
                    FileDataViewer = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, theUpdatedItem.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                    string base64String = Convert.ToBase64String(FileDataViewer);
                    DocumentPath = "data:application/pdf;base64," + base64String;
                    DisplayDocumentViewer = true;
                    StateHasChanged();

                    await Task.Delay(500);
                    await JsInterop.InvokeVoidAsync("ScrollPortfolioGridToBottom");
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

        private Stream GetFileStream(string filePath)
        {
            MemoryStream ms = new MemoryStream();
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                file.CopyTo(ms);
            return ms;
        }
        #endregion

        #region Preview Draft
        //< History Author = 'Hassan Abbas' Date = '2023-03-20' Version = "1.0" Branch = "master" >Preview Final Document</History>
        protected async Task PreviewDocumentPortfolio()
        {
            try
            {
                if (documentPortfolio.SelectedDocuments.Any())
                {
                    busyPreviewBtn = true;
                    StateHasChanged();
                    isVisible = true;
                    await Task.Run(() => MergeAndGenerateNewPdf());
                    busyPreviewBtn = false;
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Select_Documents_For_Portfolio"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
            catch (Exception)
            {
                busyPreviewBtn = false;
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-18' Version="1.0" Branch="master">Open advance search popup </History>
        protected async Task TogglePreviewWindow()
        {
            isVisible = !isVisible;
        }

        //<History Author = 'Hassan Abbas' Date='2023-03-21' Version="1.0" Branch="master">Create Cover Letter for Portfolio having header/footer and detail of documents like name, no of pages etc</History>
        //<History Author = 'Hassan Abbas' Date='2023-07-05' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then add details like page count</History>
        protected async Task SamplePage1(PdfSharp.Pdf.PdfDocument document, string openingStatement)
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
            MigraDoc.DocumentObjectModel.Paragraph para = section.AddParagraph();
            para.Format.Alignment = ParagraphAlignment.Right;
            para.Format.Font.Name = "Arial";
            para.Format.Font.Size = 13;
            para.Format.Font.Bold = true;
            para.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.Black;
            para.AddText(openingStatement.ArabicWithFontGlyphsToPfd());
            para.AddLineBreak();

            MigraDoc.DocumentObjectModel.Tables.Table table = section.AddTable();
            // Create the item table
            table.Style = "Table";
            table.Borders.Width = 0.25;
            table.Borders.Left.Width = 0.5;
            table.Borders.Right.Width = 0.5;
            table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            MigraDoc.DocumentObjectModel.Tables.Column column = table.AddColumn("4.5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("5cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = table.AddColumn("8cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            column = table.AddColumn("1cm");
            column.Format.Alignment = ParagraphAlignment.Right;

            // Create the header of the table
            MigraDoc.DocumentObjectModel.Tables.Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Cells[0].AddParagraph("عدد الصفحات".ArabicWithFontGlyphsToPfd());
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[1].AddParagraph("يكتب".ArabicWithFontGlyphsToPfd());
            row.Cells[1].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[1].Format.Font.Bold = true;
            row.Cells[2].AddParagraph("الاسم".ArabicWithFontGlyphsToPfd());
            row.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[2].Format.Font.Bold = true;
            row.Cells[3].AddParagraph("م".ArabicWithFontGlyphsToPfd());
            row.Cells[3].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[3].Format.Font.Bold = true;


            for (int i = 0; i < documentPortfolio.SelectedDocuments.Count(); i++)
            {
                //Encryption/Descyption Key
                string password = _config.GetValue<string>("DocumentEncryptionKey");
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);
                RijndaelManaged RMCrypto = new RijndaelManaged();
                MemoryStream fsOut = new MemoryStream();
                int data;
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                row = table.AddRow();
#if DEBUG
                {
                    var physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + documentPortfolio.SelectedDocuments[i].StoragePath).Replace(@"\\", @"\");
                    FileStream fsCrypt = new FileStream(physicalPath, FileMode.Open);
                    CryptoStream cs = new CryptoStream(fsCrypt,
                        RMCrypto.CreateDecryptor(key, key),
                        CryptoStreamMode.Read);
                    while ((data = cs.ReadByte()) != -1)
                        fsOut.WriteByte((byte)data);

                    using (PdfSharp.Pdf.PdfDocument inp = PdfReader.Open(fsOut, PdfDocumentOpenMode.Import))
                    {
                        row.Cells[0].AddParagraph(inp.PageCount.ToString());
                        row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
                    }
                    fsOut.Close();
                    cs.Close();
                    fsCrypt.Close();
                }
#else
                {
                    var physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + documentPortfolio.SelectedDocuments[i].StoragePath).Replace(@"\\", @"\");
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
                        {
                            row.Cells[0].AddParagraph(inp.PageCount.ToString());
                            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
                        }
                        fsOut.Close();
                        cs.Close();
                        fsCrypt.Close();
                    }
                }
#endif
                row.Cells[1].AddParagraph(documentPortfolio.SelectedDocuments[i].Type_Ar.ArabicWithFontGlyphsToPfd());
                row.Cells[1].Format.Alignment = ParagraphAlignment.Right;
                row.Cells[2].AddParagraph(documentPortfolio.SelectedDocuments[i].FileName);
                row.Cells[2].Format.Alignment = ParagraphAlignment.Right;
                row.Cells[3].AddParagraph((i + 1).ToString());
                row.Cells[3].Format.Alignment = ParagraphAlignment.Center;
            }

            MigraDoc.Rendering.DocumentRenderer docRenderer = new MigraDoc.Rendering.DocumentRenderer(doc);
            docRenderer.PrepareDocument();

            // Render the paragraph. You can render tables or shapes the same way.
            docRenderer.RenderObject(gfx, XUnit.FromCentimeter(1.5), XUnit.FromCentimeter(0.5), "12cm", image);
            docRenderer.RenderObject(gfx, XUnit.FromCentimeter(1.5), XUnit.FromCentimeter(28.5), "12cm", imageFooter);
            docRenderer.RenderObject(gfx, XUnit.FromCentimeter(1.5), XUnit.FromCentimeter(6), "18.5cm", para);
            docRenderer.RenderObject(gfx, XUnit.FromCentimeter(1.5), XUnit.FromCentimeter(7), "18cm", table);
        }

        //<History Author = 'Hassan Abbas' Date='2023-03-21' Version="1.0" Branch="master">Merge Selected Documents into one PDf and Attach Cover Letter on top of it</History>
        protected async Task MergeAndGenerateNewPdf()
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                DateTime now = DateTime.Now;
                PdfSharp.Pdf.PdfDocument documentCoverLetter = new PdfSharp.Pdf.PdfDocument();
                await SamplePage1(documentCoverLetter, translationState.Translate("Temp_Portfolio_Opening_Statement"));
                Debug.WriteLine("seconds=" + (DateTime.Now - now).TotalSeconds.ToString());

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

                foreach (var file in documentPortfolio.SelectedDocuments)
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
                        var physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + file.StoragePath).Replace(@"\\", @"\");
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
                        var physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + file.StoragePath).Replace(@"\\", @"\");
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
                documentPortfolio.FileData = stream.ToArray();
            }
            catch (Exception ex)
            {
                busyPreviewBtn = false;
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
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
        #endregion

        #region Submit Document Portfolio

        protected async Task SubmitPortfolio()
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                spinnerService.Show();
                isVisible = false;
                StateHasChanged();
                ApiCallResponse response = new ApiCallResponse();
                documentPortfolio.ReferenceId = Guid.Parse(CaseId);
                documentPortfolio.AttachmentTypeId = (int)AttachmentTypeEnum.DocumentPortfolio;

                response = await fileUploadService.SaveDocumentPortfolioToDocument(documentPortfolio);
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Document_Portfolio_Saved"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await RedirectBack();
                }
                else
                {
                    isVisible = false;
                    StateHasChanged();
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }
        }
        #endregion
    }
}
