using Append.Blazor.Printing;
using AraibcPdfUnicodeGlyphsResharper;
using FATWA_DOMAIN.Models.Lms;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using static FATWA_DOMAIN.Enums.LiteratureEnum;


namespace FATWA_WEB.Pages.Lms
{
    public partial class AddStockTakingReport : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic StockTakingId { get; set; }
        #endregion
        #region Variable Declaration
        public DateTime Max = new DateTime(DateTime.Now.Date.Ticks);
        public DateTime Min = new DateTime(1950, 1, 1);
        public List<LmsStockTakingStatus> StockTakingStatuses { get; set; } = new List<LmsStockTakingStatus>();
        public SaveStockTakingVm saveStockTaking { get; set; } = new SaveStockTakingVm();
        public bool isListGenerated { get; set; } = false;
        public bool isPrintEnabled { get; set; } = false;
        public bool isSubmitEnabled { get; set; } = false;
        public bool isApproveEnabled { get; set; } = true;
        private static byte[] FileData { get; set; }
        public bool isReportPrinted { get; set; }
        public bool IsGenerateListEnabled { get; set; } = true;
        protected RadzenDataGrid<LmsStockTakingBooksReportListVm> grid0 { get; set; } = new RadzenDataGrid<LmsStockTakingBooksReportListVm>();



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
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion
        #region Load Component
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            translationState.TranslateGridFilterLabels(grid0);
            spinnerService.Hide();
        }
        protected async Task Load()
        {
            if (StockTakingId == null)
            {
                await GetTotalNoOfBooks();
                saveStockTaking.stockTaking.Id = Guid.NewGuid();
                saveStockTaking.stockTaking.StockTakingDate = DateTime.Now;
                saveStockTaking.stockTaking.CreatedBy = loginState.Username;
            }
            else
            {
                await GetLmsStockTaking(Guid.Parse(StockTakingId));
                await GetLmsBookStockTakingReportList(Guid.Parse(StockTakingId));
                saveStockTaking.IsEdit = true;
                isReportPrinted = true;
                IsGenerateListEnabled = false;
                isPrintEnabled = true;
                isSubmitEnabled = true;
                await GetPerformersByStockTakingId();
            }
            await GetStockTakingStatuses();


        }
        #endregion

        #region Get Stock Taking Statuses
        protected async Task GetStockTakingStatuses()
        {
            try
            {
                var response = await lmsLiteratureService.GetStockTakingStatuses();
                if (response.IsSuccessStatusCode)
                {
                    StockTakingStatuses = (List<LmsStockTakingStatus>)response.ResultData;
                    if (StockTakingId == null)
                    {
                        saveStockTaking.stockTaking.StatusId = (int)StockTakingStatusEnum.New;
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
        protected async Task GetTotalNoOfBooks()
        {
            try
            {
                var response = await lmsLiteratureService.GetTotalNoOfBooks();
                if (response.IsSuccessStatusCode)
                {
                    saveStockTaking.stockTaking.TotalBooks = (int)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
        #endregion

        #region Get Book Stock Taking Report List
        protected async Task GetLmsBookStockTakingReportList(Guid? StockTakingId)
        {
            try
            {
                var response = await lmsLiteratureService.GetLmsBookStockTakingReportList(StockTakingId);
                if (response.IsSuccessStatusCode)
                {
                    saveStockTaking.lmsStockTakingBooksReportListVms = (List<LmsStockTakingBooksReportListVm>)response.ResultData;
                    await grid0.Reload();
                    StateHasChanged();
                    isListGenerated = true;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
        #endregion

        #region Get Auto Generated Report Number
        protected async Task GetAutoGeneratedReportNumber()
        {
            try
            {
                var response = await lmsLiteratureService.GetAutoGeneratedReportNumber();
                if (response.IsSuccessStatusCode)
                {
                    var result = (LmsStockTaking)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
        #endregion

        #region Get Lms StockTaking
        protected async Task GetLmsStockTaking(Guid Id)
        {
            try
            {
                var response = await lmsLiteratureService.GetLmsStockTakingById(Id);
                if (response.IsSuccessStatusCode)
                {
                    saveStockTaking.stockTaking = (LmsStockTaking)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
        #endregion

        #region Get StockTaking Performers by StocktakingId
        protected async Task GetPerformersByStockTakingId()
        {
            try
            {
                var response = await lmsLiteratureService.GetPerformersByStockTakingId(Guid.Parse(StockTakingId));
                if (response.IsSuccessStatusCode)
                {
                    var result = (List<StockTakingPerformerVm>)response.ResultData;
                    if (result.Count > 0)
                    {
                        saveStockTaking.StockTakingPerformerIds = result.Select(x => x.UserId).ToList();
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
        #endregion

        #region On Cell Render
        protected void CellRender(RowRenderEventArgs<LmsStockTakingBooksReportListVm> args)
        {
            if (string.IsNullOrEmpty(args.Data.RFIDValue) || args.Data.IsRfIdNotMatch == true || args.Data.RFIDValue == "0")
            {
                args.Attributes.Add("style", $"background-color: #e3d4c9;");
            }
        }
        #endregion

        #region Import List And Compare
        protected async Task ImportListAndCompare(MouseEventArgs args)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<ImportStockTakingReportDialog>(
                         translationState.Translate("Import_And_Compare"),
                         new Dictionary<string, object>()
                         {
                             { "StockTakingBooksList", saveStockTaking.lmsStockTakingBooksReportListVms},
                             { "SelectedPerformerIds", saveStockTaking.StockTakingPerformerIds.ToList() }
                         },
                         new DialogOptions() { CloseDialogOnOverlayClick = true, Width = "60%" });
                if (dialogResult != null)
                {
                    spinnerService.Show();

                    var result = (SaveStockTakingVm)dialogResult;
                    saveStockTaking.lmsStockTakingBooksReportListVms = result.lmsStockTakingBooksReportListVms
                        .OrderByDescending(x => !x.IsRfIdNotMatch)
                        .ToList();
                    saveStockTaking.StockTakingPerformerIds = result.StockTakingPerformerIds;
                    isPrintEnabled = true;
                    isApproveEnabled = false;
                    await grid0.Reload();
                    StateHasChanged();
                    spinnerService.Hide();
                }
                await Task.Delay(200);
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
        #endregion

        #region Submit Stock Taking Report
        protected async Task SubmitStockTakingReport()
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                        @translationState.Translate("Are_you_sure_you_want_to_save_the_report"),
                        @translationState.Translate("Confirm"),
                        new ConfirmOptions()
                        {
                            OkButtonText = @translationState.Translate("OK"),
                            CancelButtonText = @translationState.Translate("Cancel")
                        });
                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    saveStockTaking.stockTaking.StatusId = (int)StockTakingStatusEnum.InProgress;
                    var response = await lmsLiteratureService.SubmitStockTakingReport(saveStockTaking);
                    if (response.IsSuccessStatusCode)
                    {
                        await SaveTempAttachementToUploadedDocument();
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("StockTaking_Report_Successfully_Saved"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });

                        await RedirectBack();
                        StateHasChanged();

                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();
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
        #endregion

        #region Print StockTaking Report
        protected async Task PrintStockTakingReport()
        {
            try
            {
                spinnerService.Show();
                isReportPrinted = false;
                PdfDocument pdfDocument = new PdfDocument();
                pdfDocument.PageSettings.Orientation = PdfPageOrientation.Landscape;
                PdfPage pdfPage = pdfDocument.Pages.Add();

                AddHeader(pdfDocument);
                AddFooter(pdfDocument);
                AddContent(pdfPage, saveStockTaking.lmsStockTakingBooksReportListVms, saveStockTaking.stockTaking.StockTakingDate);

                MemoryStream pdfStream = new MemoryStream();
                pdfDocument.Save(pdfStream);
                FileData = pdfStream.ToArray();
                saveStockTaking.stockTaking.FileData = FileData;
                pdfDocument.Close(true);
                pdfStream.Position = 0;
                string base64String = Convert.ToBase64String(pdfStream.ToArray());
                var docResponse = await fileUploadService.SaveStockTakingReportToDocument(saveStockTaking.stockTaking);
                if (docResponse.IsSuccessStatusCode)
                {

                    isReportPrinted = true;
                    isSubmitEnabled = true;
                    StateHasChanged();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(docResponse);
                }
                await PrintingService.Print(new PrintOptions(base64String) { Base64 = true });
                pdfStream.Close();
                spinnerService.Hide();

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
        #endregion

        #region Option Row
        protected async Task EditOptionRow(LmsStockTakingBooksReportListVm remarkValue)
        {
            await grid0.EditRow(remarkValue);
        }
        protected async Task SaveRow(LmsStockTakingBooksReportListVm remarkValue)
        {

            if (!string.IsNullOrEmpty(remarkValue.Remarks))
            {
                await grid0.UpdateRow(remarkValue);
            }
            else
            {
                await grid0.UpdateRow(remarkValue);
            }
        }
        #endregion

        #region Printing Grid All Pages
        /*< History Author = 'Muhammad Zaeem' Date = '2024-10-09' Version = "1.0" Branch = "master" > Function to Print the Grid list of StockTaking Report</History > */

        public void AddContent(PdfPage pdfPage, List<LmsStockTakingBooksReportListVm> stockTakingReport, DateTime StockTakingDate)
        {
            //Fonts Start
            FileStream fontStream = new FileStream(System.IO.Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\fonts\\arial.ttf"), FileMode.Open, FileAccess.Read);
            PdfFont fontHeader = new PdfTrueTypeFont(fontStream, 12, PdfFontStyle.Bold);
            PdfFont font = new PdfTrueTypeFont(fontStream, 11);
            PdfFont titlefont = new PdfTrueTypeFont(fontStream, 20, PdfFontStyle.Bold);
            //Fonts end

            //Formats Start
            PdfStringFormat stringFormatArbic = new PdfStringFormat();
            PdfStringFormat stringFormat = new PdfStringFormat();
            PdfStringFormat LabelStringFormat = new PdfStringFormat();

            stringFormatArbic.TextDirection = PdfTextDirection.RightToLeft;
            stringFormatArbic.Alignment = PdfTextAlignment.Right;

            stringFormat.TextDirection = PdfTextDirection.LeftToRight;
            stringFormat.Alignment = PdfTextAlignment.Left;
            stringFormat.ParagraphIndent = 35f;
            stringFormat.WordWrap = PdfWordWrapType.Word;

            LabelStringFormat.Alignment = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? PdfTextAlignment.Left : PdfTextAlignment.Right;
            LabelStringFormat.TextDirection = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? PdfTextDirection.LeftToRight : PdfTextDirection.RightToLeft;

            //Grid Cell Style
            PdfGridCellStyle gridCellStyle = new PdfGridCellStyle();
            gridCellStyle.CellPadding = new PdfPaddings(5, 5, 5, 5);

            //Formats end
            PdfGraphics graphics = pdfPage.Graphics;
            string reportTitle = translationState.Translate("StockTaking_Report_Title");
            PdfStringFormat format = new PdfStringFormat();
            PdfStringFormat arabicFormat = new PdfStringFormat();
            //Set text direction in English Mode
            format.TextDirection = PdfTextDirection.LeftToRight;
            format.Alignment = PdfTextAlignment.Center;
            format.ParagraphIndent = 15f;
            //Set text direction in Arabic Mode

            arabicFormat.Alignment = PdfTextAlignment.Center;
            //Draw the text.
            if(Thread.CurrentThread.CurrentCulture.Name == "en-US")
            {
                graphics.DrawString(reportTitle + ": " + StockTakingDate.ToString("dd/MM/yyyy"), titlefont, PdfBrushes.Black, new RectangleF(0, 30, pdfPage.GetClientSize().Width, pdfPage.GetClientSize().Height), format);

            }
            else
            {
                graphics.DrawString(StockTakingDate.ToString("dd/MM/yyyy") + " :" + reportTitle.ArabicWithFontGlyphsToPfd(), titlefont, PdfBrushes.Black,  new RectangleF(0, 30, pdfPage.GetClientSize().Width, pdfPage.GetClientSize().Height), arabicFormat);

            }
            //Grid Start
            PdfGrid pdfGrid = new PdfGrid();
            pdfGrid.RepeatHeader = true;
            pdfGrid.AllowRowBreakAcrossPages = true;

            pdfGrid.Columns.Add(6);
            PdfGridRow headerRow = pdfGrid.Headers.Add(1)[0];
            if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
            {
                //Set Column Width in En culture
                pdfGrid.Columns[0].Width = 100;
                pdfGrid.Columns[1].Width = 120;
                pdfGrid.Columns[2].Width = 120;
                pdfGrid.Columns[3].Width = 90;
                pdfGrid.Columns[4].Width = 94;
                pdfGrid.Columns[5].Width = 220;

                headerRow.Cells[0].Value = new PdfTextElement(translationState.Translate("Book_Name"), fontHeader, PdfBrushes.Black) { StringFormat = stringFormat };
                headerRow.Cells[1].Value = new PdfTextElement(translationState.Translate("RFID_Value"), fontHeader, PdfBrushes.Black) { StringFormat = stringFormat };
                headerRow.Cells[2].Value = new PdfTextElement(translationState.Translate("Barcode_No"), fontHeader, PdfBrushes.Black) { StringFormat = stringFormat };
                headerRow.Cells[3].Value = new PdfTextElement(translationState.Translate("Excess"), fontHeader, PdfBrushes.Black) { StringFormat = stringFormat };
                headerRow.Cells[4].Value = new PdfTextElement(translationState.Translate("Shortage"), fontHeader, PdfBrushes.Black) { StringFormat = stringFormat };
                headerRow.Cells[5].Value = new PdfTextElement(translationState.Translate("Remarks"), fontHeader, PdfBrushes.Black) { StringFormat = stringFormat };
            }
            else
            {
                //Set Column Width in Ar culture
                pdfGrid.Columns[0].Width = 220;
                pdfGrid.Columns[1].Width = 94;
                pdfGrid.Columns[2].Width = 90;
                pdfGrid.Columns[3].Width = 120;
                pdfGrid.Columns[4].Width = 120;
                pdfGrid.Columns[5].Width = 100;

                headerRow.Cells[0].Value = new PdfTextElement(translationState.Translate("Remarks"), fontHeader, PdfBrushes.Black) { StringFormat = stringFormatArbic };
                headerRow.Cells[1].Value = new PdfTextElement(translationState.Translate("Shortage"), fontHeader, PdfBrushes.Black) { StringFormat = stringFormatArbic };
                headerRow.Cells[2].Value = new PdfTextElement(translationState.Translate("Excess"), fontHeader, PdfBrushes.Black) { StringFormat = stringFormatArbic };
                headerRow.Cells[3].Value = new PdfTextElement(translationState.Translate("Barcode_No"), fontHeader, PdfBrushes.Black) { StringFormat = stringFormatArbic };
                headerRow.Cells[4].Value = new PdfTextElement(translationState.Translate("RFID_Value"), fontHeader, PdfBrushes.Black) { StringFormat = stringFormatArbic };
                headerRow.Cells[5].Value = new PdfTextElement(translationState.Translate("Book_Name"), fontHeader, PdfBrushes.Black) { StringFormat = stringFormatArbic };
            }

            headerRow.ApplyStyle(gridCellStyle);

            foreach (var item in stockTakingReport)
            {
                PdfGridRow row = pdfGrid.Rows.Add();
                row.ApplyStyle(gridCellStyle);
                if (item.Shortage != null && item.Shortage != 0)
                {
                    row.Style.BackgroundBrush = PdfBrushes.Red;

                }
                string bookName = item.BookName;
                string RfIdValue = item.RFIDValue != null ? item.RFIDValue : "";
                string barcodeNumber = item.BarCodeNumber;
                string excess = item.Excess != null ? item.Excess.ToString() : "";
                string shortage = item.Shortage != null ? item.Shortage.ToString() : "";
                string remarks = item.Remarks != null ? item.Remarks.ToString() : "";


                if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                {
                    row.Cells[0].Value = new PdfTextElement(bookName, font, PdfBrushes.Black) { StringFormat = stringFormat };
                    row.Cells[1].Value = new PdfTextElement(RfIdValue, font, PdfBrushes.Black) { StringFormat = stringFormat };
                    row.Cells[2].Value = new PdfTextElement(barcodeNumber, font, PdfBrushes.Black) { StringFormat = stringFormat };
                    row.Cells[3].Value = new PdfTextElement(excess, font, PdfBrushes.Black) { StringFormat = stringFormat };
                    row.Cells[4].Value = new PdfTextElement(shortage, font, PdfBrushes.Black) { StringFormat = stringFormat };
                    row.Cells[5].Value = new PdfTextElement(remarks, font, PdfBrushes.Black) { StringFormat = stringFormat };
                }
                else
                {
                    row.Cells[0].Value = new PdfTextElement(remarks, font, PdfBrushes.Black) { StringFormat = stringFormatArbic };
                    row.Cells[1].Value = new PdfTextElement(shortage, font, PdfBrushes.Black) { StringFormat = stringFormatArbic };
                    row.Cells[2].Value = new PdfTextElement(excess, font, PdfBrushes.Black) { StringFormat = stringFormatArbic };
                    row.Cells[3].Value = new PdfTextElement(barcodeNumber, font, PdfBrushes.Black) { StringFormat = stringFormatArbic };
                    row.Cells[4].Value = new PdfTextElement(RfIdValue, font, PdfBrushes.Black) { StringFormat = stringFormatArbic };
                    row.Cells[5].Value = new PdfTextElement(bookName, font, PdfBrushes.Black) { StringFormat = stringFormatArbic };
                }
            }

            //Grid End

            //Set properties to paginate the grid
            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
            layoutFormat.Break = PdfLayoutBreakType.FitPage;
            layoutFormat.Layout = PdfLayoutType.Paginate;

            //Draw grid to the page of PDF document
            pdfGrid.Draw(pdfPage, new PointF(0, 90), layoutFormat);

        }
        public void AddHeader(PdfDocument pdfDocument)
        {
            RectangleF bounds = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 100);
            PdfPageTemplateElement header = new PdfPageTemplateElement(bounds);
            header.Alignment = PdfAlignmentStyle.MiddleCenter;

            string imagePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "fatwaPortfolioHeaderLandscape.PNG");
            if (File.Exists(imagePath))
            {
                using (FileStream imageStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    PdfImage image = new PdfBitmap(imageStream);
                    header.Graphics.DrawImage(image, new PointF(0, 0), new SizeF(pdfDocument.Pages[0].GetClientSize().Width, 80));
                }

            }
            pdfDocument.Template.Top = header;
        }

        public void AddFooter(PdfDocument pdfDocument)
        {
            RectangleF rect = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 50);

            //Create a page template
            PdfPageTemplateElement footer = new PdfPageTemplateElement(rect);
            PdfCompositeField compositeField = new PdfCompositeField();
            compositeField.Bounds = footer.Bounds;

            string FooterimagePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "fatwaPortfolioFooter.PNG");
            FileStream imageStream = new FileStream(FooterimagePath, FileMode.Open, FileAccess.Read);
            PdfImage image = new PdfBitmap(imageStream);

            footer.Graphics.DrawImage(image, new PointF(0, 16), new SizeF(750, 40));
            compositeField.Draw(footer.Graphics, new PointF(540, 40));

            pdfDocument.Template.Bottom = footer;
        }

        #endregion

        #region Save Temp Attachment to  uploaded Documents
        protected async Task SaveTempAttachementToUploadedDocument()
        {
            try
            {
                var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                {
                    RequestIds = new List<Guid> { saveStockTaking.stockTaking.Id },
                    CreatedBy = saveStockTaking.stockTaking.CreatedBy,
                    FilePath = _config.GetValue<string>("dms_file_path"),
                    DeletedAttachementIds = null
                });
                if (!docResponse.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Attachment_Save_Failed"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
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
                return;
            }
        }
        #endregion

        #region Approve Report
        protected async Task ApproveReport()
        {
            try
            {

                bool? dailogResponse = await dialogService.Confirm(
                    translationState.Translate("Approved_StockTaking_Report"),
                    translationState.Translate("Confirm"),
                    new ConfirmOptions()
                    {
                        OkButtonText = @translationState.Translate("Yes"),
                        CancelButtonText = @translationState.Translate("No")
                    }

                    );
                if (dailogResponse == true)
                {
                    var response = await lmsLiteratureService.ApproveStockTakingReport(Guid.Parse(StockTakingId));
                    if (response.IsSuccessStatusCode)
                    {
                        var result = (bool)response.ResultData;
                        if (result)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("StockTaking_Report_has_been_Approved_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            await RedirectBack();
                            StateHasChanged();
                        }

                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        protected async Task DeleteReport()
        {
            try
            {
                bool? dialogResult = await dialogService.Confirm(
                    translationState.Translate("Sure_Delete_The_Record"),
                    translationState.Translate("Confirm"),
                    new ConfirmOptions
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    });
                if (dialogResult == true)
                {
                    var lmsStockTakingListVM = mapper.Map<LmsStockTakingListVM>(saveStockTaking.stockTaking);
                    var response = await lmsLiteratureService.DeleteLmsStockTaking(lmsStockTakingListVM);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("StockTaking_Report") + " " + lmsStockTakingListVM.ReportNumber + " " + translationState.Translate("Has_Been_Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "

                        });
                        await RedirectBack();

                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
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
    }
}
