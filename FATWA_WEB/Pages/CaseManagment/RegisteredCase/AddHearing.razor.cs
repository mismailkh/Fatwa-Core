using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.PdfViewer;
using Syncfusion.Blazor.PdfViewerServer;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Drawing;
using Syncfusion.Pdf.Parsing;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;
using PdfDocument = Syncfusion.Pdf.PdfDocument;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    //<History Author = 'Hassan Abbas' Date='2022-12-05' Version="1.0" Branch="master"> Add Hearing For a Case</History>
    public partial class AddHearing : ComponentBase
    {
        #region Parameter

        [Parameter]
        public string CaseId { get; set; }
        [Parameter]
        public dynamic HearingId { get; set; }
        [Parameter]
        public dynamic Isedit { get; set; }
        #endregion

        #region Variables
        public Hearing hearing { get; set; } = new Hearing { Id = Guid.NewGuid(), HearingDate = DateTime.Now, StatusId = (int)HearingStatusEnum.HearingScheduled, RequestForDocument = new MojRequestForDocument() };
        protected HearingDetailVM hearingDetailVM { get; set; }
        protected List<HearingStatus> hearingStatuses { get; set; } = new List<HearingStatus>();
        protected string dateValidationMsg = "";
        protected string timeValidationMsg = "";
        protected string statusValidationMsg = "";
        public IEnumerable<LawyerVM> users { get; set; }
        protected string lawyerValidationMsg = "";
        protected List<CopyAttachmentVM> copyAttachments = new List<CopyAttachmentVM>();
        public string Plaintiffs { get; set; }
        public string Defendants { get; set; }
        public string CaseNumber { get; set; }
        public string ChamberNameAndNumber { get; set; }
        public DateTime? HearingDate { get; set; }
        #endregion

        #region Document Portolfio Variables
        protected CmsDocumentPortfolio documentPortfolio { get; set; } = new CmsDocumentPortfolio { UploadFrom = "CaseManagement" };
        protected List<AttachmentType> AttachmentTypes { get; set; }
        protected RadzenDataGrid<TempAttachementVM> gridAttachments { get; set; }
        protected IEnumerable<TempAttachementVM> attachments { get; set; }
        public bool allowRowSelectOnRowClick = true;
        public TelerikPdfViewer PdfViewerRef { get; set; }
        public TelerikPdfViewer PdfViewerRef2 { get; set; }
        public byte[] FileDataViewer { get; set; }
        protected byte[] FileDataPortoflio { get; set; }
        public string DocumentPath { get; set; }
        public int? PreviewedDocumentId { get; set; }
        public int? PreviewedAttachementId { get; set; }
        List<ToolbarItem> ToolbarItems = new List<ToolbarItem>();
        public bool DisplayDocumentViewer { get; set; }
        public bool ShowDocumentViewerPortfolio { get; set; }
        public bool busyPreviewBtn { get; set; }
        public bool isVisible { get; set; }
        protected List<OperatingSectorType> SectorTypes { get; set; } = new List<OperatingSectorType>();
        public SfPdfViewerServer pdfViewer;

        #endregion

        #region Component Load

        //< History Author = 'Hassan Abbas' Date = '2022-09-29' Version = "1.0" Branch = "master" >Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                if (HearingId != null)
                {
                    await PopulateHearingDetails();
                }
                else
                {
                    await ExpandDocumentPortfolio();
                    hearing.LawyerId = loginState.UserDetail.UserId;
                }
                await PopulateRegisteredCaseDetail();
                await PopulateHearingStatuses();
                await PopulateSectorTypes();
                await PopulatePartiesFromCaseFile();
                spinnerService.Hide();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Summary = translationState.Translate("Error"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion

        #region Dropdown Change Events
        protected async Task PopulatePartiesFromCaseFile()
        {
            ApiCallResponse partyResponse;
            if (CaseId == null)
            {
                partyResponse = await caseRequestService.GetCMSCasePartyDetailById(Guid.Parse(CaseId));
            }
            else
            {
                partyResponse = await caseRequestService.GetCMSCasePartyDetailById(Guid.Parse(CaseId));
            }
            if (partyResponse.IsSuccessStatusCode)
            {
                List<CasePartyLinkVM> CasePartyLinks = (List<CasePartyLinkVM>)partyResponse.ResultData;
                List<CasePartyLinkVM> CasePartyPlaintiffs = CasePartyLinks?.Where(p => p.CategoryId == (int)CasePartyCategoryEnum.Plaintiff).ToList();
                List<CasePartyLinkVM> CasePartyDefendants = CasePartyLinks?.Where(p => p.CategoryId == (int)CasePartyCategoryEnum.Defendant).ToList();
                if ((bool)CasePartyPlaintiffs?.Any())
                {
                    var length = CasePartyPlaintiffs.Count();
                    for (int i = 0; i < length; i++)
                    {
                        var seperator = i + 1 == length ? "" : ", ";
                        Plaintiffs += (CasePartyPlaintiffs[i].TypeId == (int)CasePartyTypeEnum.GovernmentEntity ? Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? CasePartyPlaintiffs[i].GovtEntity_En : CasePartyPlaintiffs[i].GovtEntity_Ar : CasePartyPlaintiffs[i].Name) + seperator;
                    }
                }
                if ((bool)CasePartyDefendants?.Any())
                {
                    var length = CasePartyDefendants.Count();
                    for (int i = 0; i < length; i++)
                    {
                        var seperator = i + 1 == length ? "" : ", ";
                        Defendants += (CasePartyDefendants[i].TypeId == (int)CasePartyTypeEnum.GovernmentEntity ? Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? CasePartyDefendants[i].GovtEntity_En : CasePartyDefendants[i].GovtEntity_Ar : CasePartyDefendants[i].Name) + seperator;
                    }
                }

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(partyResponse);
            }
            StateHasChanged();
        }
        protected async Task PopulateSectorTypes()
        {

            var response = await lookupService.GetOperatingSectorTypes();
            if (response.IsSuccessStatusCode)
            {
                SectorTypes = (List<OperatingSectorType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        //<History Author = 'Hassan Abbas' Date='2023-12-04' Version="1.0" Branch="master">Populate Case Details</History>
        protected async Task PopulateRegisteredCaseDetail()
        {
            var result = await cmsRegisteredCaseService.GetRegisteredCaseDetailByIdVM(Guid.Parse(CaseId));
            if (result.IsSuccessStatusCode)
            {
                var registeredCase = (CmsRegisteredCaseDetailVM)result.ResultData;
                CaseNumber = registeredCase.CaseNumber;
                ChamberNameAndNumber = registeredCase.ChamberNameAr + "/" + registeredCase.ChamberNumber;

                await PopulateLawyerslist(registeredCase.ChamberNumberId);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Hearing Statuses</History>
        protected async Task PopulateHearingStatuses()
        {
            var response = await lookupService.GetCaseHearingStatuses();
            if (response.IsSuccessStatusCode)
            {
                hearingStatuses = (List<HearingStatus>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task PopulateLawyerslist(int chamberNumberId)
        {
            var userresponse = await lookupService.GetLawyersBySectorAndChamber(loginState.UserDetail.SectorTypeId, chamberNumberId);
            if (userresponse.IsSuccessStatusCode)
            {
                users = (IEnumerable<LawyerVM>)userresponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(userresponse);
            }

        }
        protected async Task PopulateHearingDetails()
        {
            try
            {
                var response = await cmsRegisteredCaseService.GetHearingDetailByHearingId(Guid.Parse(HearingId));
                if (response.IsSuccessStatusCode)
                {
                    hearing = (Hearing)response.ResultData;
                    if (hearing != null)
                    {
                        CaseId = hearing.CaseId.ToString();
                        hearing.IsUpdated = true;
                        HearingDate = hearing.HearingDate;
                        if (string.IsNullOrEmpty(hearing.LawyerId))
                        {
                            hearing.LawyerId = loginState.UserDetail.UserId;
                        }
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                var responseVM = await cmsRegisteredCaseService.GetHearingDetail(Guid.Parse(HearingId)); if (response.IsSuccessStatusCode)
                {
                    hearingDetailVM = (HearingDetailVM)responseVM.ResultData;
                    if (hearingDetailVM != null && String.IsNullOrWhiteSpace(hearingDetailVM.PortfolioStoragePath))
                    {
                        await ExpandDocumentPortfolio();
                    }
                    else
                    {
                        await LoadDocumentPortfolio();
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(responseVM);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Redirect and Dialog Events

        protected void ButtonCancelClick(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/case-view/" + CaseId);
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

        //< History Author = 'Hassan Abbas' Date = '2022-12-05' Version = "1.0" Branch = "master" >Submit Hearing Details</History>
        //<History Author = 'Hassan Abbas' Date='2023-03-23' Version="1.0" Branch="master">Send Portfolio Request to MOJ for Hearing </History>
        protected async Task FormSubmit()
        {
            try
            {
               
                    if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    }) == true)
                    {
                        spinnerService.Show();
                        if (hearing.IsUpdated == false)
                        {
                            hearing.CaseId = Guid.Parse(CaseId);
                            hearing.CreatedBy = loginState.Username;
                        }
                        var response = await cmsRegisteredCaseService.AddHearing(hearing);
                        if (response.IsSuccessStatusCode)
                        {
                            // Save TempAttachement To Upload Documents
                            await SaveTempAttachementToUploadedDocument();
                            await CopyAttachmentsFromSourceToDestination(hearing);
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Hearing_Added_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            if (!hearing.SendPortfolioRequestMoj && documentPortfolio.SelectedDocuments.Any())
                            {
                                await Task.Run(() => GeneratePortfolioDocument());
                                documentPortfolio.HearingId = hearing.Id;
                                documentPortfolio.ReferenceId = Guid.Parse(CaseId);
                                documentPortfolio.AttachmentTypeId = (int)AttachmentTypeEnum.DocumentPortfolio;
                                response = await fileUploadService.SaveDocumentPortfolioToDocument(documentPortfolio);
                                if (response.IsSuccessStatusCode)
                                {
                                    //notificationService.Notify(new NotificationMessage()
                                    //{
                                    //    Severity = NotificationSeverity.Success,
                                    //    Detail = translationState.Translate("Document_Portfolio_Saved"),
                                    //    Style = "position: fixed !important; left: 0; margin: auto; "
                                    //});
                                }
                                else
                                {
                                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                                }
                            }
                            navigationManager.NavigateTo("/case-view/" + CaseId);
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
                spinnerService.Show();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }

        #endregion

        #region upload Documents
        protected async Task SaveTempAttachementToUploadedDocument()
        {
            try
            {
                var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                {
                    RequestIds = new List<Guid> { hearing.Id },
                    CreatedBy = hearing.CreatedBy,
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
                return;
                throw;
            }
        }
        protected async Task CopyAttachmentsFromSourceToDestination(Hearing hearing)
        {
            try
            {
                var docResponse = await fileUploadService.CopyAttachmentsFromSourceToDestination(new List<CopyAttachmentVM> { new CopyAttachmentVM()
                    {
                        SourceId = hearing.Id,
                        DestinationId = hearing.CaseId,
                        CreatedBy = hearing.CreatedBy
                    }});
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
                return;
                throw;
            }
        }
        #endregion

        #region Attachments 


        //<History Author = 'Hassan Abbas' Date='2023-03-20' Version="1.0" Branch="master"> Populate Attachments Grid</History>
        protected async Task PopulateAttachementsGrid()
        {
            try
            {
                attachments = await fileUploadService.GetUploadedAttachements(false, 0, Guid.Parse(CaseId));
                if (attachments != null && attachments.Any())
                {
                    Regex regex = new Regex("_[0-9]+", RegexOptions.RightToLeft);
                    attachments = new List<TempAttachementVM>(attachments?.Select(f => { if (f.FileName.Contains("Document_Portfolio")) return f; else f.FileName = regex.Replace(f.FileName, "", 1); return f; }).ToList());
                }
                await gridAttachments?.Reload();
                StateHasChanged();
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

        /*<History Author = 'Hassan Abbas' Date='2024-03-14' Version="1.0" Branch="master"> Append Custom Action into the Pdf Viewer Toolbar through Html</History>*/
        protected async Task DocumentLoaded(LoadEventArgs args)
        {
            await JSRuntime.InvokeVoidAsync("addRotateButtonToPdfToolbar");
        }

        //<History Author = 'Hassan Abbas' Date='2024-03-21' Version="1.0" Branch="master"> Open Document in New Window</History>
        protected async Task OpenInNewWindow(MouseEventArgs args)
        {
            var url = PreviewedDocumentId > 0 ? $"/preview-document/{CaseId}/{PreviewedDocumentId}" : $"/preview-attachement/{CaseId}/{PreviewedAttachementId}";
            await JsInterop.InvokeVoidAsync("openNewWindow", url);
        }

        //<History Author = 'Hassan Abbas' Date='2023-03-20' Version="1.0" Branch="master"> View Attachment either Pdf or Image (read in stream and convert to PDF document) in Pdf Viewer</History>
        //<History Author = 'Hassan Abbas' Date='2023-07-05' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then  merge</History>
        protected async Task ViewAttachement(TempAttachementVM theUpdatedItem)
        {
            try
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
                PreviewedDocumentId = theUpdatedItem.UploadedDocumentId != null ? theUpdatedItem.UploadedDocumentId : 0;
                PreviewedAttachementId = theUpdatedItem.AttachementId != null ? theUpdatedItem.AttachementId : 0;
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
                    DisplayDocumentViewer = true;
                    DocumentPath = "data:application/pdf;base64," + base64String;
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
        #endregion

        #region Document Portfolio

        //<History Author = 'Hassan Abbas' Date='2023-03-20' Version="1.0" Branch="master"> Populate Portfolio Section</History>
        protected async Task ExpandDocumentPortfolio()
        {
            if (!hearing.SendPortfolioRequestMoj)
            {
                spinnerService.Show();
                documentPortfolio.Name = "Document_Portfolio_" + hearing.HearingDate.ToString("dd-MM-yyyy");
                await PopulateAttachmentTypes();
                await PopulateAttachementsGrid();
                translationState.TranslateGridFilterLabels(gridAttachments);
                spinnerService.Hide();
            }
        }


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
                    await Task.Run(() => GeneratePortfolioDocument());
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

        //<History Author = 'Hassan Abbas' Date='2022-11-18' Version="1.0" Branch="master">Open advance search popup </History>
        protected async Task TogglePreviewWindow()
        {
            isVisible = !isVisible;
        }


        //<History Author = 'Hassan Abbas' Date='2023-03-21' Version="1.0" Branch="master">Create Cover Letter for Portfolio having header/footer and detail of documents like name, no of pages etc</History>
        //<History Author = 'Hassan Abbas' Date='2023-07-05' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then add details like page count</History>
        //<History Author = 'Hassan Abbas' Date='2024-05-27' Version="1.0" Branch="master"> Modified the function to generate Cover Letter by first generating Word Document with Header/Fotter and all the content Billingual then convert to Pdf</History>
        protected async Task<PdfDocument> CreateCoverLetter()
        {
            try
            {

                //byte[] htmlBytes = Encoding.UTF8.GetBytes("");
                //string base64String = Convert.ToBase64String(htmlBytes);
                //byte[] byteArray = Convert.FromBase64String(base64String);
                //Stream stream = new MemoryStream(byteArray);
                //byteArray = null;
                //Syncfusion.Blazor.DocumentEditor.WordDocument documenteditor = Syncfusion.Blazor.DocumentEditor.WordDocument.Load(stream, ImportFormatType.Html);

                //A new document is created.
                int totalPages = 0;
                WordDocument document = new WordDocument();
                //Adding a new section to the document.
                WSection section = document.AddSection() as WSection;
                //Set Margin of the section.
                section.PageSetup.PageSize = PageSize.A4;
                section.PageSetup.Margins.Left = 20;
                section.PageSetup.Margins.Right = 20;
                section.PageSetup.Margins.Top = 10;
                section.PageSetup.Margins.Bottom = 10;

                //Header Footer Start
                IWParagraph headerFooterParagraph = section.HeadersFooters.Header.AddParagraph();
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "fatwaPortfolioHeader.PNG");
                FileStream imageStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                IWPicture picture = headerFooterParagraph.AppendPicture(imageStream);
                picture.Height = 80;
                picture.Width = 550;
                headerFooterParagraph.ParagraphFormat.AfterSpacing = 15;


                headerFooterParagraph = section.HeadersFooters.Footer.AddParagraph();
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "fatwaPortfolioFooter.PNG");
                imageStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                picture = headerFooterParagraph.AppendPicture(imageStream);
                picture.Height = 40;
                picture.Width = 550;
                headerFooterParagraph.ParagraphFormat.BeforeSpacing = 5;
                //Header Footer End

                //Top Section Start
                IWParagraph paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.HorizontalAlignment = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                paragraph.ParagraphFormat.AfterSpacing = 20;
                var sector = SectorTypes.Where(x => x.Id == loginState.UserDetail.SectorTypeId).FirstOrDefault();
                WTextRange textRange = paragraph.AppendText(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? sector.Name_En : sector.Name_Ar) as WTextRange;

                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                textRange = paragraph.AppendText(translationState.Translate("Document_Portfolio_Portfolio")) as WTextRange;
                textRange.CharacterFormat.UnderlineStyle = UnderlineStyle.Single;
                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                paragraph.ParagraphFormat.AfterSpacing = 20;
                textRange = paragraph.AppendText(translationState.Translate("Document_Portfolio_Introduction")) as WTextRange;
                textRange.CharacterFormat.UnderlineStyle = UnderlineStyle.Single;
                //Top Section End

                //Plaintiff Table Start
                IWTable table = section.AddTable();
                table.TableFormat.Borders.BorderType = Syncfusion.DocIO.DLS.BorderStyle.None;
                table.ResetCells(1, 3);
                paragraph = table[0, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? 0 : 2].AddParagraph();
                textRange = paragraph.AppendText(String.Concat(translationState.Translate("Document_Portfolio_Mr"), Plaintiffs)) as WTextRange;
                paragraph.ParagraphFormat.HorizontalAlignment = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                textRange.CharacterFormat.FontName = "Arial";
                textRange.CharacterFormat.FontSize = 12;
                paragraph = table[0, 1].AddParagraph();
                textRange = paragraph.AppendText(translationState.Translate("Document_Portfolio_Capacity")) as WTextRange;
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                textRange.CharacterFormat.FontName = "Arial";
                textRange.CharacterFormat.FontSize = 12;
                paragraph = table[0, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? 2 : 0].AddParagraph();
                textRange = paragraph.AppendText(translationState.Translate("Document_Portfolio_Aged")) as WTextRange;
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                textRange.CharacterFormat.FontName = "Arial";
                textRange.CharacterFormat.FontSize = 12;

                //Plaintiff Table End

                //ضد Start
                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                paragraph.ParagraphFormat.BeforeSpacing = 15;
                paragraph.ParagraphFormat.AfterSpacing = 20;
                textRange = paragraph.AppendText(translationState.Translate("Document_Portfolio_Against")) as WTextRange;
                textRange.CharacterFormat.UnderlineStyle = UnderlineStyle.Single;

                //ضد End


                //Defendants Table Start
                table = section.AddTable();
                table.TableFormat.Borders.BorderType = Syncfusion.DocIO.DLS.BorderStyle.None;
                table.ResetCells(1, 3);
                paragraph = table[0, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? 0 : 2].AddParagraph();
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Right;
                textRange = paragraph.AppendText(String.Concat(translationState.Translate("Document_Portfolio_Mr"), Defendants)) as WTextRange;
                paragraph.ParagraphFormat.HorizontalAlignment = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                textRange.CharacterFormat.FontName = "Arial";
                textRange.CharacterFormat.FontSize = 12;
                paragraph = table[0, 1].AddParagraph();
                textRange = paragraph.AppendText("") as WTextRange;
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                textRange.CharacterFormat.FontName = "Arial";
                textRange.CharacterFormat.FontSize = 12;
                paragraph = table[0, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? 2 : 0].AddParagraph();
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                textRange = paragraph.AppendText(translationState.Translate("Document_Portfolio_Appellant")) as WTextRange;
                textRange.CharacterFormat.FontName = "Arial";
                textRange.CharacterFormat.FontSize = 12;

                //Defendants Table End

                //Empty Paragraph Start
                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                paragraph.ParagraphFormat.AfterSpacing = 5;
                textRange = paragraph.AppendText("") as WTextRange;
                textRange.CharacterFormat.UnderlineStyle = UnderlineStyle.Single;

                //Empty Paragraph End

                //Case Details Table Start
                table = section.AddTable();
                table.TableFormat.Borders.BorderType = Syncfusion.DocIO.DLS.BorderStyle.None;
                table.ResetCells(1, 3);
                paragraph = table[0, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? 0 : 2].AddParagraph();
                textRange = paragraph.AppendText(String.Concat(translationState.Translate("Document_Portfolio_Appeal_No"), CaseNumber)) as WTextRange;
                paragraph.ParagraphFormat.HorizontalAlignment = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                textRange.CharacterFormat.FontName = "Arial";
                textRange.CharacterFormat.FontSize = 12;
                paragraph = table[0, 1].AddParagraph();
                textRange = paragraph.AppendText(ChamberNameAndNumber) as WTextRange;
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                textRange.CharacterFormat.FontName = "Arial";
                textRange.CharacterFormat.FontSize = 12;
                paragraph = table[0, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? 2 : 0].AddParagraph();
                textRange = paragraph.AppendText(String.Concat(translationState.Translate("Document_Portfolio_Session"), hearing.HearingDate != default ? hearing.HearingDate.ToString("dd/MM/yyyy") : translationState.Translate("Document_Portfolio_Session_Under_Appointment"))) as WTextRange;
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                textRange.CharacterFormat.FontName = "Arial";
                textRange.CharacterFormat.FontSize = 12;

                //Case Details Table End

                //Empty Paragraph Start
                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                paragraph.ParagraphFormat.AfterSpacing = 5;
                textRange = paragraph.AppendText("") as WTextRange;
                textRange.CharacterFormat.UnderlineStyle = UnderlineStyle.Single;

                //Empty Paragraph End

                //Documents Grid Start

                table = section.AddTable();
                table.TableFormat.Borders.BorderType = Syncfusion.DocIO.DLS.BorderStyle.Single;
                table.ResetCells(documentPortfolio.SelectedDocuments.Count() + 1, 4);
                int cellIndex = 0;
                WTableRow row = table.Rows[0];

                //Sr Column Title
                cellIndex = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? 0 : 3;
                paragraph = table[0, cellIndex].AddParagraph();
                textRange = paragraph.AppendText(translationState.Translate("Document_Portfolio_Sr")) as WTextRange;
                paragraph.ParagraphFormat.HorizontalAlignment = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                textRange.CharacterFormat.FontName = "Arial";
                textRange.CharacterFormat.FontSize = 12;
                textRange.CharacterFormat.Bold = true;
                WTableCell cell = row.Cells[cellIndex];
                cell.Width = 100;
                cell.CellFormat.VerticalAlignment = VerticalAlignment.Middle;

                //Document Date Column Title
                cellIndex = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? 1 : 2;
                paragraph = table[0, cellIndex].AddParagraph();
                textRange = paragraph.AppendText(translationState.Translate("Document_Portfolio_Document_Date")) as WTextRange;
                paragraph.ParagraphFormat.HorizontalAlignment = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                textRange.CharacterFormat.FontName = "Arial";
                textRange.CharacterFormat.FontSize = 12;
                textRange.CharacterFormat.Bold = true;
                cell = row.Cells[cellIndex];
                cell.Width = 100;
                cell.CellFormat.VerticalAlignment = VerticalAlignment.Middle;

                //No of Pages Column Title
                cellIndex = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? 2 : 1;
                paragraph = table[0, cellIndex].AddParagraph();
                textRange = paragraph.AppendText(translationState.Translate("Document_Portfolio_No_Of_Pages")) as WTextRange;
                paragraph.ParagraphFormat.HorizontalAlignment = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                textRange.CharacterFormat.FontName = "Arial";
                textRange.CharacterFormat.FontSize = 12;
                textRange.CharacterFormat.Bold = true;
                cell = row.Cells[cellIndex];
                cell.Width = 100;
                cell.CellFormat.VerticalAlignment = VerticalAlignment.Middle;

                //FileName Column Title
                cellIndex = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? 3 : 0;
                paragraph = table[0, cellIndex].AddParagraph();
                textRange = paragraph.AppendText(translationState.Translate("Document_Portfolio_FileName")) as WTextRange;
                paragraph.ParagraphFormat.HorizontalAlignment = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                textRange.CharacterFormat.FontName = "Arial";
                textRange.CharacterFormat.FontSize = 12;
                textRange.CharacterFormat.Bold = true;
                cell = row.Cells[cellIndex];
                cell.Width = 250;
                cell.CellFormat.VerticalAlignment = VerticalAlignment.Middle;

                for (int i = 0; i < documentPortfolio.SelectedDocuments.Count(); i++)
                {
                    string pageCount = string.Empty;
                    //Encryption/Descyption Key
                    string password = _config.GetValue<string>("DocumentEncryptionKey");
                    UnicodeEncoding UE = new UnicodeEncoding();
                    byte[] key = UE.GetBytes(password);
                    RijndaelManaged RMCrypto = new RijndaelManaged();
                    MemoryStream fsOut = new MemoryStream();
                    int data;
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#if DEBUG
                    {
                        var physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + documentPortfolio.SelectedDocuments[i].StoragePath).Replace(@"\\", @"\");
                        FileStream fsCrypt = new FileStream(physicalPath, FileMode.Open);
                        CryptoStream cs = new CryptoStream(fsCrypt,
                            RMCrypto.CreateDecryptor(key, key),
                            CryptoStreamMode.Read);
                        while ((data = cs.ReadByte()) != -1)
                            fsOut.WriteByte((byte)data);
                        Stream[] streams = { fsOut };

                        PdfLoadedDocument loadedDocument = new PdfLoadedDocument(fsOut);
                        pageCount = loadedDocument.Pages.Count.ToString();
                        totalPages += loadedDocument.Pages.Count;
                        loadedDocument.Close(true);
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

                        PdfLoadedDocument loadedDocument = new PdfLoadedDocument(fsOut);
                        pageCount = loadedDocument.Pages.Count.ToString();
                        totalPages += loadedDocument.Pages.Count;
                        loadedDocument.Close(true);
                        fsOut.Close();
                        cs.Close();
                        fsCrypt.Close();
                    }
                }
#endif
                    row = table.Rows[i + 1];

                    //Sr Column
                    cellIndex = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? 0 : 3;
                    paragraph = table[i + 1, cellIndex].AddParagraph();
                    textRange = paragraph.AppendText((i + 1).ToString()) as WTextRange;
                    paragraph.ParagraphFormat.HorizontalAlignment = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                    textRange.CharacterFormat.FontName = "Arial";
                    textRange.CharacterFormat.FontSize = 12;
                    cell = row.Cells[cellIndex];
                    cell.Width = 100;
                    cell.CellFormat.VerticalAlignment = VerticalAlignment.Middle;

                    //Document Date Column
                    cellIndex = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? 1 : 2;
                    paragraph = table[i + 1, cellIndex].AddParagraph();
                    textRange = paragraph.AppendText(Convert.ToDateTime(documentPortfolio.SelectedDocuments[i].DocumentDate).ToString("dd/MM/yyyy")) as WTextRange;
                    paragraph.ParagraphFormat.HorizontalAlignment = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                    textRange.CharacterFormat.FontName = "Arial";
                    textRange.CharacterFormat.FontSize = 12;
                    cell = row.Cells[cellIndex];
                    cell.Width = 100;
                    cell.CellFormat.VerticalAlignment = VerticalAlignment.Middle;

                    //No of Pages Column
                    cellIndex = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? 2 : 1;
                    paragraph = table[i + 1, cellIndex].AddParagraph();
                    textRange = paragraph.AppendText(pageCount) as WTextRange;
                    paragraph.ParagraphFormat.HorizontalAlignment = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                    textRange.CharacterFormat.FontName = "Arial";
                    textRange.CharacterFormat.FontSize = 12;
                    cell = row.Cells[cellIndex];
                    cell.Width = 100;
                    cell.CellFormat.VerticalAlignment = VerticalAlignment.Middle;

                    //FileName Column
                    string fileName = Regex.Replace(documentPortfolio.SelectedDocuments[i].FileName, ".pdf", "", RegexOptions.IgnoreCase);
                    cellIndex = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? 3 : 0;
                    paragraph = table[i + 1, cellIndex].AddParagraph();
                    textRange = paragraph.AppendText(fileName) as WTextRange;
                    paragraph.ParagraphFormat.HorizontalAlignment = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? HorizontalAlignment.Left : HorizontalAlignment.Right;
                    textRange.CharacterFormat.FontName = "Arial";
                    textRange.CharacterFormat.FontSize = 12;
                    cell = row.Cells[cellIndex];
                    cell.Width = 250;
                    cell.CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                }

                //Documents Grid End

                //Portfolio Pages Start

                int rowcount = documentPortfolio.SelectedDocuments.Count();
                string documentPages = rowcount == 1 ? translationState.Translate("Document_Portfolio_Document") + rowcount + translationState.Translate("Document_Portfolio_From") + totalPages + translationState.Translate("Document_Portfolio_Sheets") : translationState.Translate("Document_Portfolio_No_Of_Documents") + rowcount + translationState.Translate("Document_Portfolio_From") + totalPages + translationState.Translate("Document_Portfolio_Sheets");
                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                paragraph.ParagraphFormat.BeforeSpacing = 20;
                paragraph.ParagraphFormat.AfterSpacing = 30;
                textRange = paragraph.AppendText(documentPages) as WTextRange;
                //Portfolio Pages End

                //Signature Start
                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.HorizontalAlignment = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? HorizontalAlignment.Right : HorizontalAlignment.Left;
                textRange = paragraph.AppendText(translationState.Translate("Document_Portfolio_Appelant_Capacity")) as WTextRange;
                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.HorizontalAlignment = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? HorizontalAlignment.Right : HorizontalAlignment.Left;
                textRange = paragraph.AppendText(translationState.Translate("Document_Portfolio_Member_Fatwa")) as WTextRange;
                //Portfolio Pages End

                //Renderer and Word to Pdf Start
                DocIORenderer render = new DocIORenderer();
                PdfDocument pdfDocument = render.ConvertToPDF(document);
                render.Dispose();
                document.Dispose();
                //Renderer and Word to Pdf End

                return pdfDocument;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //<History Author = 'Hassan Abbas' Date='2023-03-21' Version="1.0" Branch="master">Merge Selected Documents into one PDf and Attach Cover Letter on top of it</History>
        protected async Task GeneratePortfolioDocument()
        {
            try
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
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Syncfusion.Pdf.PdfDocument finalDocument = new Syncfusion.Pdf.PdfDocument();
                PdfDocument documentCoverLetter = await CreateCoverLetter();
                MemoryStream streamCoverLetter = new MemoryStream();
                MemoryStream outputStream = new MemoryStream();

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                documentCoverLetter.Save(streamCoverLetter);
                documentCoverLetter.Close();

                Stream[] streams1 = { streamCoverLetter };
                Syncfusion.Pdf.PdfDocumentBase.Merge(finalDocument, streams1);

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

                        Stream[] streams = { fsOut };
                        Syncfusion.Pdf.PdfDocumentBase.Merge(finalDocument, streams);
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

                            Stream[] streams = { fsOut };
                            Syncfusion.Pdf.PdfDocumentBase.Merge(finalDocument, streams);
                            cs.Close();
                            fsCrypt.Close();
                        }
                    }
#endif
                }

                finalDocument.Save(outputStream);
                finalDocument.Close(true);
                outputStream.Close();
                streamCoverLetter.Close();
                documentPortfolio.FileData = outputStream.ToArray();
                string base64String = Convert.ToBase64String(documentPortfolio.FileData);
                documentPortfolio.DocumentPath = "data:application/pdf;base64," + base64String;
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
                throw ex;
            }
        }

        #endregion

        #region Load Portfolio
        //<History Author = 'Hassan Abbas' Date='2023-07-05' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then view, also added functionality to convert .msg file to .pdf for preview</History>
        protected async Task LoadDocumentPortfolio()
        {
            try
            {
                var physicalPath = string.Empty;
#if DEBUG
                {
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + hearingDetailVM.PortfolioStoragePath).Replace(@"\\", @"\");

                }
#else
                {
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + hearingDetailVM.PortfolioStoragePath).Replace(@"\\", @"\");
                    physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                           
                }
#endif

                if (!string.IsNullOrEmpty(physicalPath))
                {
                    FileDataPortoflio = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, ".pdf", _config.GetValue<string>("DocumentEncryptionKey"));
                    string base64String = Convert.ToBase64String(FileDataPortoflio);
                    DocumentPath = "data:application/pdf;base64," + base64String;
                    ShowDocumentViewerPortfolio = true;
                    StateHasChanged();
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Authority_Letter_Not_Loaded"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }

            }
            catch
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
    }
}
