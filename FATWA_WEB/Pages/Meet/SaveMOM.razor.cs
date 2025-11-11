using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using SelectPdf;
using Syncfusion.Blazor.PdfViewerServer;
using System.Text.RegularExpressions;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.DmsEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.MeetingEnums;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Meet
{
    public partial class SaveMOM : ComponentBase
    {
        #region Parameters

        [Parameter]
        public dynamic MeetingId { get; set; }
        [Parameter]
        public dynamic? ReferenceGuid { get; set; }
        [Parameter]
        public dynamic isEdit { get; set; }
        [Parameter]
        public dynamic isReview { get; set; }


        #endregion

        #region Constructor  
        public SaveMomVM meetingMomVM = new SaveMomVM()
        {
            Meeting = new Meeting(),
            MeetingMom = new MeetingMom(),
            GeAttendee = new List<MeetingAttendeeVM>(),
            LegislationAttendee = new List<FatwaAttendeeVM>()
        };
        protected DmsAddedDocument AddedDocument { get; set; } = new DmsAddedDocument { DocumentVersion = new DmsAddedDocumentVersion() };
        protected List<DmsDocumentClassification> DocumentClassifications { get; set; } = new List<DmsDocumentClassification>();
        protected List<MomAttendeeDecisionVM> momAttendeesDecisionDetails { get; set; } = new List<MomAttendeeDecisionVM>();
        public List<MeetingAttendeeStatus> attendeeStatus { get; set; } = new List<MeetingAttendeeStatus>();
        protected List<FatwaAttendeeVM> GetLegislationAttendees { get; set; } = new List<FatwaAttendeeVM>();
        protected RadzenDataGrid<MomAttendeeDecision>? grid = new RadzenDataGrid<MomAttendeeDecision>();
        protected List<FatwaAttendeeVM> LegislationAttendees { get; set; } = new List<FatwaAttendeeVM>();
        protected List<FatwaAttendeeVM> meetingAttendeedetail { get; set; } = new List<FatwaAttendeeVM>();
        protected List<GovernmentEntity> GovtEntities { get; set; } = new List<GovernmentEntity>();
        protected MeetingAttendeeVM GeAttendee { get; set; } = new MeetingAttendeeVM();
        protected List<Department> Departments { get; set; } = new List<Department>();
        protected RadzenDataGrid<MeetingAttendeeVM> GeAttendeeGrid { get; set; } = new RadzenDataGrid<MeetingAttendeeVM>();
        protected List<MeetingAttendeeVM> GetGeAttendees = new List<MeetingAttendeeVM>();
        public IList<MeetingAttendeeVM> selectedGEAttandee { get; set; } = new List<MeetingAttendeeVM>();

        #endregion

        #region Variable
        protected User LegislationAttendee = new User();
        protected RadzenDropDown<string> ddlLegislationAttendees;
        protected bool IsReview { get { return bool.Parse(isReview); } set { isReview = value; } }
        protected bool IsEdit { get { return bool.Parse(isEdit); } set { isEdit = value; } }
        protected RadzenHtmlEditor editor = new RadzenHtmlEditor();
        private Meeting meetingdetail = new Meeting();
        private MeetingMom meetingMom = new MeetingMom();
        bool isSaveAsDraft;
        bool isSubmitMOM;
        int? atttendeeLegislationSerialNo = 0;
        public bool AllApproved = false;
        protected RadzenDropDown<int> ddlGovtEntities;
        protected RadzenDropDown<int?> ddlDepartments;
        int atttendeeGeSerialNo = 0;
        public bool ShowAttandenceCheckBoxs { get; set; }
        public bool allowRowSelectOnRowClick = true;
        protected RadzenDataGrid<FatwaAttendeeVM> LegislationAttendeeGrid;
        protected RadzenDataGrid<MomAttendeeDecisionVM> radzenDataGrid;
        public IList<FatwaAttendeeVM> selectedLegislationAttandee { get; set; } = new List<FatwaAttendeeVM>();
        bool IsPresent;
        public bool busyPreviewBtn { get; set; }
        public List<string> validFiles { get; set; } = new List<string>() { ".pdf" };
		public bool isVisible { get; set; }
		protected List<CaseTemplate> HeaderFooterTemplates { get; set; }
		public TelerikPdfViewer PdfViewerRef { get; set; }
        public SfPdfViewerServer pdfViewer;
        public string DocumentPath { get; set; } = string.Empty;
        #endregion

        #region On Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            if (meetingMomVM.MeetingMom.MeetingMomId == Guid.Empty)
            {
                meetingMomVM.MeetingMom.MeetingMomId = Guid.NewGuid();
                meetingMomVM.MeetingMom.MeetingId = meetingMomVM.Meeting.MeetingId;
            }
            else
            {
                //await populateHeaderFooter();
                if (MeetingId != null && Guid.Parse(MeetingId) != Guid.Empty)
                {
                    meetingMomVM.MeetingMom.MeetingId = Guid.Parse(MeetingId);
                    //meetingMom.MeetingId = Guid.Parse(MeetingId);
                }
                var res = await meetingService.GetMeetingMOMByMeetingId(Guid.Parse(MeetingId));
                if (res.IsSuccessStatusCode)
                {
                    meetingMom = (MeetingMom)res.ResultData;
                    meetingMomVM.MeetingMom = meetingMom;
                }
                if (meetingMomVM.MeetingMom.MOMStatusId == (int)MeetingStatusEnum.SaveAsDraft)
                {
                    AddedDocument.ClassificationId = meetingMom.Content != string.Empty ? 2 : 1;
                }

            }
            if (IsReview)
            {
                await PopulateMOMAttendeesDecisionDetails(meetingMom.MeetingMomId, meetingMom.MeetingId);
            }

            spinnerService.Hide();
        }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public async Task Load()
        {
            try
            {
                await PopulateDocumentClassifications();
                await PopulateMeetingDetail();
                await PopulateMeetingAttendeeStatus();
				await PopulateHeaderFooter();
				//await PopulateLegislationAttendees();

			}
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get MOM attendees decision details
        private async Task PopulateMOMAttendeesDecisionDetails(Guid meetingMomId, Guid meetingId)
        {
            try
            {
                var response = await meetingService.PopulateMOMAttendeesDecisionDetails(meetingMomId, meetingId);
                if (response.IsSuccessStatusCode)
                {
                    momAttendeesDecisionDetails = (List<MomAttendeeDecisionVM>)response.ResultData;
                    AllApproved = momAttendeesDecisionDetails.ToList().All(x => x.AttendeeStatusId == (int)MeetingAttendeeStatusEnum.Accept);
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
		#endregion

		#region Preview Draft

		//<History Author = 'Umer Zaman' Date = '02-02-2024' Version="1.0" Branch="master">Open advance search popup </History>
		protected async Task TogglePreviewWindow(bool? value)
		{
			if (value != null)
				isVisible = (bool)value;
			else
				isVisible = !isVisible;
		}

		//< History Author = 'Umer Zaman' Date = '02-02-2024' Version = "1.0" Branch = "master" >Preview editor content</History>
		protected async Task PreviewDraft()
		{
			try
			{
               
                string pattern = @"<([^>]+)>|&nbsp;|\s+";
                string cleanString = meetingMomVM.MeetingMom.Content == null ? "" : meetingMomVM.MeetingMom.Content;
                cleanString = Regex.Replace(cleanString, pattern, string.Empty);

                if (String.IsNullOrWhiteSpace(cleanString))
                    {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Fill_Draft_Content"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
                else
                {
                    busyPreviewBtn = true;
                    StateHasChanged();
                    isVisible = true;
                    await Task.Run(() => PopulatePdfFromHtml());
                    busyPreviewBtn = false;
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
		#endregion

		#region Submit and Save As Draft Function
		protected async Task FormSubmit(SaveMomVM meetingMom)
        {
            if (isSubmitMOM)
            {
                meetingMom.LegislationAttendee = (List<FatwaAttendeeVM>)selectedLegislationAttandee;
                meetingMom.GeAttendee = (List<MeetingAttendeeVM>)selectedGEAttandee;
                string dialogMessage = string.Empty;
                dialogMessage = translationState.Translate("Sure_Submit_MOM");
                bool? dialogResponse = await dialogService.Confirm(
                  dialogMessage,
                  translationState.Translate("Confirm"),
                  new ConfirmOptions()
                  {
                      OkButtonText = @translationState.Translate("OK"),
                      CancelButtonText = @translationState.Translate("Cancel")
                  });

                if (dialogResponse == true)
                {
                    var response = await meetingService.SubmitMom(meetingMom);
                    // To Check the document response
                    if (response.IsSuccessStatusCode)
                    {
                        //Save Temp Attachement To UploadedDocument
                        List<Guid> requestIds = new List<Guid>
                    {
                        meetingMom.MeetingMom.MeetingId
                    };
                        var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                        {
                            RequestIds = requestIds,
                            CreatedBy = meetingMom.MeetingMom.CreatedBy,
                            FilePath = _config.GetValue<string>("dms_file_path"),
                            DeletedAttachementIds = meetingMom.MeetingMom.DeletedAttachementIds
                        });

                        if (!docResponse.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Attachment_Save_Failed"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }

                        await PopulatePdfFromHtml();
                        //Save Draft Template To Document
                        var contractDocResponse = await fileUploadService.SaveMOMTemplateToDocument(meetingMom.MeetingMom);
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("MOM_Submit"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        navigationManager.NavigateTo("/meeting-list");
                    }
                }
            }
            else
            {
                if (selectedLegislationAttandee.Count() != 0 || selectedGEAttandee.Count() != 0)
                {

                    var tempAttachments = await fileUploadService.GetTempAttachements(meetingMom.MeetingMom.MeetingMomId);
                    if (!tempAttachments.Where(x => x.AttachmentTypeId == (int)AttachmentTypeEnum.MOMAttachment).Any())
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("RequiredDocument"), 
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        return;
                    }
                    else
                    {
                        meetingMom.LegislationAttendee = (List<FatwaAttendeeVM>)selectedLegislationAttandee;
                        meetingMom.GeAttendee = (List<MeetingAttendeeVM>)selectedGEAttandee;
                        List<TempAttachementVM> momAttachments = new List<TempAttachementVM>();
                        var uploadedAttachments = await fileUploadService.GetUploadedAttachements(false, 0, meetingMom.MeetingMom.MeetingMomId);

                        if (tempAttachments.Count == 0)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Please_Select_File"),
                                Summary = translationState.Translate("Error"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        else
                        {
                            try
                            {
                                string dialogMessage = string.Empty;
                                dialogMessage = translationState.Translate("Sure_Save_MOM");
                                if (isSaveAsDraft)
                                {
                                    meetingMom.MeetingMom.isSaveAsDraft = true;
                                    dialogMessage = translationState.Translate("Sure_SaveAsDarft");
                                }
                                bool? dialogResponse = await dialogService.Confirm(
                                  dialogMessage,
                                  translationState.Translate("Confirm"),
                                  new ConfirmOptions()
                                  {
                                      OkButtonText = @translationState.Translate("OK"),
                                      CancelButtonText = @translationState.Translate("Cancel")
                                  });
                                if (dialogResponse == true)
                                {
                                    meetingMom.MeetingMom.Content = AddedDocument.ClassificationId == 1 ? string.Empty : meetingMom.MeetingMom.Content;
                                    ApiCallResponse response = null;
                                    if (meetingMom.MeetingMom.isSaveAsDraft)
                                    {
                                        meetingMom.MeetingMom.MOMStatusId = (int)MeetingStatusEnum.SaveAsDraft;
                                    }
                                    else
                                    {
                                        meetingMom.MeetingMom.MOMStatusId = (int)MeetingStatusEnum.OnHold;
                                    }
                                    if (!IsEdit)
                                    {
                                        response = await meetingService.SaveMom(meetingMomVM);
                                    }
                                    else
                                    {
                                        response = await meetingService.EditMom(meetingMomVM);
                                    }
                                    if (response.IsSuccessStatusCode)
                                    {
                                        //Save Temp Attachement To UploadedDocument
                                        List<Guid> requestIds = new List<Guid>
                                    {
                                        meetingMom.MeetingMom.MeetingId
                                    };
                                        var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                                        {
                                            RequestIds = requestIds,
                                            CreatedBy = meetingMom.MeetingMom.CreatedBy,
                                            FilePath = _config.GetValue<string>("dms_file_path"),
                                            DeletedAttachementIds = meetingMom.MeetingMom.DeletedAttachementIds
                                        });

                                        if (!docResponse.IsSuccessStatusCode)
                                        {
                                            notificationService.Notify(new NotificationMessage()
                                            {
                                                Severity = NotificationSeverity.Error,
                                                Detail = translationState.Translate("Attachment_Save_Failed"),
                                                Style = "position: fixed !important; left: 0; margin: auto; "
                                            });
                                        }
                                        notificationService.Notify(new NotificationMessage()
                                        {
                                            Severity = NotificationSeverity.Success,
                                            Detail = translationState.Translate("MOM_Saved"),
                                            Style = "position: fixed !important; left: 0; margin: auto;"
                                        });
                                        dialogService.Close();
                                        navigationManager.NavigateTo("/meeting-list");
                                    }
                                    else
                                        meetingMom.MeetingMom.isSaveAsDraft = false;
                                }
                            }
                            catch (Exception)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Error,
                                    Detail = translationState.Translate("Something_Went_Wrong"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                            }
                        }
                    }


                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Please_MarkAttendance_Atleast_One_Attendee"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
            }
        }

        protected async Task SaveAsDraft()
        {
            isSaveAsDraft = true;
            await FormSubmit(meetingMomVM);
            isSaveAsDraft = false;
        }

        protected async Task SubmitMOM()
        {
            isSubmitMOM = true;
            await FormSubmit(meetingMomVM);
            isSubmitMOM = false;
        }

        #endregion

        #region Cancel
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }

        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        protected async Task ButtonCloseDialog(MouseEventArgs args)
        {
            bool? dialogResponse = await dialogService.Confirm(translationState.Translate("Sure_Cancel"), translationState.Translate("Confirm"),
                                    new ConfirmOptions()
                                    {
                                        OkButtonText = @translationState.Translate("OK"),
                                        CancelButtonText = @translationState.Translate("Cancel")
                                    });
            if (dialogResponse == true)
            {
                navigationManager.NavigateTo("/meeting-list");
                //return Task.CompletedTask;
            }
        }

        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

        #region Populate Function
        public async Task PopulateMeetingDetail()
        {
            try
            {
                var response = await meetingService.GetMeetingDetailById(Guid.Parse(MeetingId));
                if (response.IsSuccessStatusCode)
                {
                    meetingMomVM = (SaveMomVM)response.ResultData;

                    meetingMomVM.MeetingMom.MeetingId = meetingMomVM.Meeting.MeetingId;
                    meetingMomVM.MeetingMom.MeetingMomId = meetingMomVM.MeetingMom.MeetingMomId;
                    if (meetingMomVM.LegislationAttendee.Any())
                    {
                        GetLegislationAttendees = meetingMomVM.LegislationAttendee;
                        selectedLegislationAttandee = meetingMomVM.LegislationAttendee.Where(x => x.IsPresent == true).ToList();
                        atttendeeLegislationSerialNo = meetingMomVM.LegislationAttendee.Count();
                    }

                    if (meetingMomVM.GeAttendee.Any())
                    {
                        GetGeAttendees = meetingMomVM.GeAttendee;
                        atttendeeGeSerialNo = meetingMomVM.GeAttendee.Count();
                        selectedGEAttandee = meetingMomVM.GeAttendee.Where(x => x.IsPresent == true).ToList(); ;
                    }
                    meetingMomVM.MeetingMom.Content = meetingMomVM.MeetingMom.Content;

                    Reload();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected async Task PopulateMeetingAttendeeStatus()
        {
            var response = await lookupService.GetAttendeeMeetingStatus();
            if (response.IsSuccessStatusCode)
            {
                attendeeStatus = (List<MeetingAttendeeStatus>)response.ResultData;

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        protected async Task PopulatePdfFromHtml()
        {
            try
            {
                HtmlToPdf converter = new HtmlToPdf();
                MemoryStream stream = new MemoryStream();

				converter.Options.DisplayHeader = true;
				converter.Options.DisplayFooter = true;
				converter.Header.DisplayOnFirstPage = true;
				converter.Header.DisplayOnOddPages = true;
				converter.Header.DisplayOnEvenPages = true;
				converter.Header.Height = 100;
				converter.Footer.Height = 50;
				converter.Footer.DisplayOnFirstPage = true;
				converter.Footer.DisplayOnOddPages = true;
				converter.Footer.DisplayOnEvenPages = true;
				string headerHtmlContent = HeaderFooterTemplates.Where(x => x.Id == (Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? (int)CaseTemplateEnum.HeaderEn : (int)CaseTemplateEnum.HeaderAr)).Select(x => x.Content).FirstOrDefault();
				string footerHtmlContent = HeaderFooterTemplates.Where(x => x.Id == (int)CaseTemplateEnum.Footer).Select(x => x.Content).FirstOrDefault();
				PdfHtmlSection headerHtml = new PdfHtmlSection(headerHtmlContent, "");
				PdfHtmlSection footerHtml = new PdfHtmlSection(footerHtmlContent, "");
				headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
				footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
				converter.Header.Add(headerHtml);
				converter.Footer.Add(footerHtml);

				// set converter options
				converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                converter.Options.WebPageWidth = 1024;
                converter.Options.WebPageHeight = 1024;
				converter.Options.MarginRight = 30;
				converter.Options.MarginLeft = 30;
				converter.Options.MarginRight = 30;
				converter.Options.MarginLeft = 30;
				converter.Options.EmbedFonts = true; 
				meetingMomVM.MeetingMom.Content = string.Concat($"<style>@font-face {{font-family: 'Sultan';src: url('{Directory.GetCurrentDirectory().Replace("\\", "/") + "/wwwroot/fonts/Sultan/arfonts-sultan-normal.ttf"}') format('truetype');font-weight: normal;font-style: normal;font-display: swap;}} @font-face {{font-family: 'Sultan Medium';src: url('{Directory.GetCurrentDirectory().Replace("\\", "/") + "/wwwroot/fonts/Sultan/arfonts-sultan-mudaim.ttf"}') format('truetype');font-weight: normal;font-style: normal;font-display: swap;}}</style>", meetingMomVM.MeetingMom.Content);
                SelectPdf.PdfDocument pdfDocument = converter.ConvertHtmlString(meetingMomVM.MeetingMom.Content != null ? meetingMomVM.MeetingMom.Content : string.Empty);

                pdfDocument.Save(stream);
                pdfDocument.Close();
                stream.Close();
                //PreviewFileData = stream.ToArray();
                meetingMomVM.MeetingMom.FileData = stream.ToArray();
                string base64String = Convert.ToBase64String(meetingMomVM.MeetingMom.FileData);
                DocumentPath = "data:application/pdf;base64," + base64String; 
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
        #endregion

        #region Other Function
        protected async Task OnClassificationChange()
        {
            if (AddedDocument.ClassificationId == (int)DocumentClassificationEnum.FreeEditor)
            {
                AddedDocument.DocumentVersion.Content = meetingMom.Content;
            }
        }
        protected async Task PopulateDocumentClassifications()
        {
            var response = await lookupService.GetDocumentClassifications();
            if (response.IsSuccessStatusCode)
            {
                DocumentClassifications = (List<DmsDocumentClassification>)response.ResultData;
                DocumentClassifications.RemoveAt(2);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
		protected async Task PopulateHeaderFooter()
		{
			var response = await cmsCaseTemplateService.GetHeaderFooter();
			if (response.IsSuccessStatusCode)
			{
				HeaderFooterTemplates = (List<CaseTemplate>)response.ResultData;
			}
		}
		
        #endregion

    }
}
