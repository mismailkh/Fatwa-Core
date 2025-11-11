using Append.Blazor.Printing;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using static FATWA_DOMAIN.Enums.GeneralEnums;

namespace FATWA_WEB.Pages.Tasks
{
    public partial class ItemHistoryPopup : ComponentBase
    {
        #region Paramater
        [Parameter]
        public TaskVM task { get; set; }
        #endregion
        #region Variable Declaration
        public IEnumerable<TaskEntityHistoryVM> taskEntityHistory { get; set; } = new List<TaskEntityHistoryVM>();
        protected IEnumerable<CmsTransferHistoryVM> cmsCaseFileTransferHistory = new List<CmsTransferHistoryVM>();
        public IEnumerable<CmsRegisteredCaseStatusHistoryVM> caseStatusHistory { get; set; } = new List<CmsRegisteredCaseStatusHistoryVM>();
        protected List<TransferHistoryVM> TransferHistoryVMs = new List<TransferHistoryVM>();
        protected List<CaseTemplate> HeaderFooterTemplates { get; set; }
        protected List<TaskEntityHistoryVM> taskEntityHistoryForPrint { get; set; }

        #endregion
        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }

        protected async Task Load()
        {
            await GetTasKEntityHistoryByReferenceId();
        }
        #endregion
        #region Populate Data Grids
        protected async Task GetTasKEntityHistoryByReferenceId()
        {
            var response = await taskService.GetTaskEntityHistoryByReferenceIdAndSubmodule((Guid)task.ReferenceId, (int)task.SubModuleId);
            if (response.IsSuccessStatusCode)
            {
                taskEntityHistory = (List<TaskEntityHistoryVM>)response.ResultData;
                taskEntityHistoryForPrint = (List<TaskEntityHistoryVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Printing Grid All Pages
        /*< History Author = 'Ijaz Ahmad' Date = '2024-18-03' Version = "1.0" Branch = "master" > Function to Print the Grid list of Task History</History > */
        protected async Task PrintTaskHistoryDetails()
        {
            try
            {
                PdfDocument pdfDocument = new PdfDocument();
                PdfPage pdfPage = pdfDocument.Pages.Add();

                AddHeader(pdfDocument);
                AddFooter(pdfDocument);
                AddContent(pdfPage, taskEntityHistoryForPrint);

                MemoryStream pdfStream = new MemoryStream();
                pdfDocument.Save(pdfStream);
                pdfDocument.Close(true);
                pdfStream.Position = 0;
                string base64String = Convert.ToBase64String(pdfStream.ToArray());
                await PrintingService.Print(new PrintOptions(base64String) { Base64 = true });
                pdfStream.Close();
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
        public void AddContent(PdfPage pdfPage, List<TaskEntityHistoryVM> taskEntityHistory)
        {
            //Fonts Start
            FileStream fontStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\fonts\\arial.ttf"), FileMode.Open, FileAccess.Read);
            PdfFont fontHeader = new PdfTrueTypeFont(fontStream, 12, PdfFontStyle.Bold);
            PdfFont font = new PdfTrueTypeFont(fontStream, 11);
            //Fonts end

            //Formats Start
            PdfStringFormat stringFormatArbic = new PdfStringFormat();
            PdfStringFormat stringFormat = new PdfStringFormat();
            PdfStringFormat LabelStringFormat = new PdfStringFormat();

            stringFormatArbic.TextDirection = PdfTextDirection.RightToLeft;
            stringFormatArbic.Alignment = PdfTextAlignment.Right;

            stringFormat.TextDirection =PdfTextDirection.LeftToRight;
            stringFormat.Alignment = PdfTextAlignment.Left;
            stringFormat.ParagraphIndent = 35f;
            stringFormat.WordWrap = PdfWordWrapType.Word;

            LabelStringFormat.Alignment = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? PdfTextAlignment.Left : PdfTextAlignment.Right;
            LabelStringFormat.TextDirection = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? PdfTextDirection.LeftToRight : PdfTextDirection.RightToLeft;
            //Formats end

            //Grid Cell Style
            PdfGridCellStyle gridCellStyle = new PdfGridCellStyle();
            gridCellStyle.CellPadding = new PdfPaddings(5, 5, 5, 5);

            //Top Row after Header Start
            PdfGraphics graphics = pdfPage.Graphics;


            var labelReference = translationState.Translate(task.SubModuleId == (int)SubModuleEnum.CaseRequest ? "Request_Number" :
                                                                 task.SubModuleId == (int)SubModuleEnum.CaseFile ? "File_Number" :
                                                                 task.SubModuleId == (int)SubModuleEnum.RegisteredCase ? "Case_Number" :
                                                                 task.SubModuleId == (int)SubModuleEnum.ConsultationFile ? "File_Number" :
                                                                 task.SubModuleId == (int)SubModuleEnum.ConsultationRequest ? "Request_Number" :
                                                                 "");

            var referenceNumber = taskEntityHistory.FirstOrDefault()?.ReferenceNumber;
            var labelCANNumber = translationState.Translate(task.SubModuleId == (int)SubModuleEnum.RegisteredCase ? "CAN_Number" : "");
            var cANNumber = taskEntityHistory.FirstOrDefault()?.CANNumber ?? "";


            PdfGrid pdfGridTopRow = new PdfGrid();
            pdfGridTopRow.Columns.Add(4);
            PdfGridRow topRow = pdfGridTopRow.Headers.Add(1)[0];
            if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
            {
                topRow.Cells[0].Value = new PdfTextElement(labelReference, fontHeader, PdfBrushes.Black) { StringFormat = stringFormat };
                topRow.Cells[1].Value = new PdfTextElement(referenceNumber, font, PdfBrushes.Black) { StringFormat = stringFormat };
                topRow.Cells[2].Value = new PdfTextElement(labelCANNumber, fontHeader, PdfBrushes.Black) { StringFormat = stringFormat };
                topRow.Cells[3].Value = new PdfTextElement(cANNumber, font, PdfBrushes.Black) { StringFormat = stringFormat };
            }
            else
            {
                topRow.Cells[0].Value = new PdfTextElement(cANNumber, font, PdfBrushes.Black) { StringFormat = stringFormatArbic };
                topRow.Cells[1].Value = new PdfTextElement(labelCANNumber, fontHeader, PdfBrushes.Black) { StringFormat = stringFormatArbic };
                topRow.Cells[2].Value = new PdfTextElement(referenceNumber, font, PdfBrushes.Black) { StringFormat = stringFormatArbic };
                topRow.Cells[3].Value = new PdfTextElement(labelReference, fontHeader, PdfBrushes.Black) { StringFormat = stringFormatArbic };
            }
            topRow.ApplyStyle(gridCellStyle);

            pdfGridTopRow.ApplyBuiltinStyle(PdfGridBuiltinStyle.TableGridLight);
            pdfGridTopRow.Draw(pdfPage, new PointF(0, 10));
            //Top Row after Header End

            //Grid Start
            PdfGrid pdfGrid = new PdfGrid();
            pdfGrid.RepeatHeader = true;
            pdfGrid.AllowRowBreakAcrossPages = true;
            pdfGrid.Columns.Add(3);

            PdfGridRow headerRow = pdfGrid.Headers.Add(1)[0];

            if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
            {
                headerRow.Cells[0].Value = new PdfTextElement(translationState.Translate("Entity_Action"), fontHeader, PdfBrushes.Black) { StringFormat = stringFormat };
                headerRow.Cells[1].Value = new PdfTextElement(translationState.Translate("Employee_Name"), fontHeader, PdfBrushes.Black) { StringFormat = stringFormat };
                headerRow.Cells[2].Value = new PdfTextElement(translationState.Translate("Date"), fontHeader, PdfBrushes.Black) { StringFormat = stringFormat };
            }
            else
            {
                headerRow.Cells[0].Value = new PdfTextElement(translationState.Translate("Date"), fontHeader, PdfBrushes.Black) { StringFormat = stringFormatArbic };
                headerRow.Cells[1].Value = new PdfTextElement(translationState.Translate("Employee_Name"), fontHeader, PdfBrushes.Black) { StringFormat = stringFormatArbic };
                headerRow.Cells[2].Value = new PdfTextElement(translationState.Translate("Entity_Action"), fontHeader, PdfBrushes.Black) { StringFormat = stringFormatArbic };
            }

            headerRow.ApplyStyle(gridCellStyle);

            foreach (var item in taskEntityHistory)
            {
                PdfGridRow row = pdfGrid.Rows.Add();
                row.ApplyStyle(gridCellStyle);
                string action = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? item.ActionEn : item.ActionAr;
                string userName = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? item.UserNameEn : item.UserNameAr;
                string createdDate = item.CreatedDate.ToString();

                if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                {
                    row.Cells[0].Value = new PdfTextElement(action, font, PdfBrushes.Black) { StringFormat = stringFormat };
                    row.Cells[1].Value = new PdfTextElement(userName, font, PdfBrushes.Black) { StringFormat = stringFormat };
                    row.Cells[2].Value = new PdfTextElement(createdDate, font, PdfBrushes.Black) { StringFormat = stringFormat };
                }
                else
                {
                    row.Cells[0].Value = new PdfTextElement(createdDate, font, PdfBrushes.Black) { StringFormat = stringFormatArbic };
                    row.Cells[1].Value = new PdfTextElement(userName, font, PdfBrushes.Black) { StringFormat = stringFormatArbic };
                    row.Cells[2].Value = new PdfTextElement(action, font, PdfBrushes.Black) { StringFormat = stringFormatArbic };
                }
            }

            //Grid End

            //Set properties to paginate the grid
            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
            layoutFormat.Break = PdfLayoutBreakType.FitPage;
            layoutFormat.Layout = PdfLayoutType.Paginate;

            //Draw grid to the page of PDF document
            pdfGrid.Draw(pdfPage, new PointF(0, 50), layoutFormat);

        }
        public void AddHeader(PdfDocument pdfDocument)
        {
            RectangleF bounds = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 100);
            PdfPageTemplateElement header = new PdfPageTemplateElement(bounds);
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "fatwaPortfolioHeader.PNG");
            if (File.Exists(imagePath))
            {
                using (FileStream imageStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    PdfImage image = new PdfBitmap(imageStream);
                    header.Graphics.DrawImage(image, new PointF(0, 0), new SizeF(510, 80));

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

            string FooterimagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "fatwaPortfolioFooter.PNG");
            FileStream imageStream = new FileStream(FooterimagePath, FileMode.Open, FileAccess.Read);
            PdfImage image = new PdfBitmap(imageStream);

            footer.Graphics.DrawImage(image, new PointF(0, 16), new SizeF(540, 40));
            compositeField.Draw(footer.Graphics, new PointF(540, 40));

            pdfDocument.Template.Bottom = footer;
        }

        #endregion

        #region Dialog Close Button
        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        #endregion
    }
}
