using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Telerik.Blazor.Components;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    //<History Author = 'Hassan Abbas' Date='2022-12-05' Version="1.0" Branch="master"> Add Judgement For a Case</History>
    public partial class AddJudgement : ComponentBase
    {

        #region Parameter

        [Parameter]
        public dynamic CaseId { get; set; }
        [Parameter]
        public dynamic OutcomeId { get; set; }
        [Parameter]
        public dynamic JudgementId { get; set; }

        #endregion

        #region Variables
        public Judgement judgement { get; set; } = new Judgement { Id = Guid.NewGuid(), Amount = 0, AmountCollected = 0 };
        public OutcomeHearingDetailVM outcomeDetail { get; set; } = new OutcomeHearingDetailVM();
        public CmsRegisteredCaseDetailVM registeredCase { get; set; }
        protected List<JudgementType> judgementTypes { get; set; } = new List<JudgementType>();
        protected List<JudgementStatus> judgementStatuses { get; set; } = new List<JudgementStatus>();
        protected List<JudgementCategory> judgementCategories { get; set; } = new List<JudgementCategory>();
        protected List<ExecutionFileLevel> executionFileLevels { get; set; } = new List<ExecutionFileLevel>();
        protected string judgementDateValidationMsg = "";
        protected string hearingDateValidationMsg = "";
        protected string descriptionValidationMsg = "";
        protected string typeValidationMsg = "";
        protected string categoryValidationMsg = "";
        protected string statusValidationMsg = "";
        protected RadzenDataGrid<TempAttachementVM> gridAttachments { get; set; }
        protected ObservableCollection<TempAttachementVM> attachments { get; set; }
        public bool allowRowSelectOnRowClick = true;
        protected MojExecutionRequest executionRequest { get; set; } = new MojExecutionRequest { Id = Guid.NewGuid() };
        public byte[] FileDataViewer { get; set; }
        public bool DisplayDocumentViewer { get; set; }
        protected List<AttachmentType> AttachmentTypes { get; set; }
        public TelerikPdfViewer PdfViewerRef { get; set; }
        public List<JudgementVM> JudgementDetail { get; set; } = new List<JudgementVM>();
        #endregion
        #region Component Load
        //< History Author = 'Hassan Abbas' Date = '2022-09-29' Version = "1.0" Branch = "master" >Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                if (JudgementId != null)
                {
                    await PopulateJudgementDetails();
                }
                else
                {
                    await PopulateOutcomeDetails();
                }
                await PopulateJudgementTypes();
                await PopulateJudgementStatuses();
                await PopulateJudgementCategories();
                await PopulateExecutionFileLevels();
                await PopulateAttachementsGrid();
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

        public async Task PopulateJudgementDetails()
        {
            try
            {
                if (dataCommunicationService.outcomeHearing.outcomeJudgement.Count > 0)
                {
                    var outcomeJudgement = dataCommunicationService.outcomeHearing.outcomeJudgement.Where(x => x.Id == Guid.Parse(JudgementId)).FirstOrDefault();
                    judgement = mapper.Map<Judgement>(outcomeJudgement);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-11-28' Version="1.0" Branch="master">Populate Outcome Details</History>
        protected async Task PopulateOutcomeDetails()
        {
            try
            {
                if (OutcomeId != null)
                {
                    var response = await cmsRegisteredCaseService.GetOutcomeDetail(Guid.Parse(OutcomeId));
                    if (response.IsSuccessStatusCode)
                    {
                        outcomeDetail = (OutcomeHearingDetailVM)response.ResultData;
                        if (outcomeDetail.HearingDate != default(DateTime))
                        {
                            judgement.HearingDate = outcomeDetail.HearingDate;
                            judgement.JudgementDate = outcomeDetail.HearingDate;
                        }
                        else
                        {
                            judgement.HearingDate = DateTime.Now;
                            judgement.JudgementDate = DateTime.Now;

                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Judgement types</History>
        protected async Task PopulateJudgementTypes()
        {
            var response = await lookupService.GetCaseJudgementTypes();
            if (response.IsSuccessStatusCode)
            {
                judgementTypes = (List<JudgementType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-11-28' Version="1.0" Branch="master">Populate Judgement Statuses</History>
        protected async Task PopulateJudgementStatuses()
        {
            var response = await lookupService.GetCaseJudgementStatuses();
            if (response.IsSuccessStatusCode)
            {
                judgementStatuses = (List<JudgementStatus>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-11-28' Version="1.0" Branch="master">Populate Judgement Categories</History>
        protected async Task PopulateJudgementCategories()
        {
            var response = await lookupService.GetCaseJudgementCategories();
            if (response.IsSuccessStatusCode)
            {
                judgementCategories = (List<JudgementCategory>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-11-28' Version="1.0" Branch="master">Populate Execution File Levels</History>
        protected async Task PopulateExecutionFileLevels()
        {
            var response = await lookupService.GetExecutionFileLevels();
            if (response.IsSuccessStatusCode)
            {
                executionFileLevels = (List<ExecutionFileLevel>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Validate Fields</History>
        protected async Task ValidateFields()
        {
            judgementDateValidationMsg = judgement.JudgementDate == default(DateTime) ? translationState.Translate("Required_Field") : "";
            hearingDateValidationMsg = judgement.HearingDate == default(DateTime) ? translationState.Translate("Required_Field") : "";
            typeValidationMsg = judgement.TypeId <= 0 ? translationState.Translate("Required_Field") : "";
            categoryValidationMsg = judgement.CategoryId <= 0 ? translationState.Translate("Required_Field") : "";
            statusValidationMsg = judgement.StatusId <= 0 ? translationState.Translate("Required_Field") : "";
        }

        #endregion

        #region Redirect and Dialog Events

        //< History Author = 'Hassan Abbas' Date = '2022-12-05' Version = "1.0" Branch = "master" >Submit Judgement Details</History>
        protected async Task Form0Submit(Judgement judgement)
        {
            try
            {
                if (judgement.HearingDate != default(DateTime) && judgement.JudgementDate != default(DateTime))
                {
                    if (!judgement.IsUpdated && judgement.OpenExecutionFile)
                    {
                        if (!executionRequest.SelectedDocuments.Any())
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Select_Documents_For_Execution"),
                                Style = "position: fixed !important; left: 0; margin: auto;"
                            });
                            return;
                        }
                    }
                    if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    }) == true)
                    {
                        spinnerService.Show();
                        if (judgement.IsUpdated == false)
                        {
                            judgement.CaseId = Guid.Parse(CaseId);
                            judgement.OutcomeId = Guid.Parse(OutcomeId);
                            judgement.CreatedBy = loginState.UserDetail.UserName;
                            judgement.CreatedDate = DateTime.Now;
                            judgement.IsDeleted = false;
                        }
                        else
                        {
                            judgement.ModifiedBy = loginState.UserDetail.UserName;
                            judgement.ModifiedDate = DateTime.Now;
                        }
                        var OldJudgment = dataCommunicationService.outcomeHearing.outcomeJudgement.Where(x => x.Id == judgement.Id).FirstOrDefault();
                        var mappedJudgement = mapper.Map<JudgementVM>(judgement);
                        mappedJudgement.SectorTypeId = (int)loginState.UserDetail.SectorTypeId;
                        mappedJudgement.CategoryEn = judgementCategories.Where(x => x.Id == judgement.CategoryId).FirstOrDefault().NameEn;
                        mappedJudgement.CategoryAr = judgementCategories.Where(x => x.Id == judgement.CategoryId).FirstOrDefault().NameAr;
                        mappedJudgement.StatusEn = judgementStatuses.Where(x => x.Id == judgement.StatusId).FirstOrDefault().NameEn;
                        mappedJudgement.StatusAr = judgementStatuses.Where(x => x.Id == judgement.StatusId).FirstOrDefault().NameAr;
                        mappedJudgement.SelectedDocuments = executionRequest.SelectedDocuments;
                        mappedJudgement.CreatedDateTime = judgement.CreatedDate;
                        if (judgement.OpenExecutionFile)
                        {
                            mappedJudgement.ExecutionFileLevelEn = executionFileLevels.Where(x => x.Id == judgement.ExecutionFileLevelId).FirstOrDefault().NameEn;
                            mappedJudgement.ExecutionFileLevelAr = executionFileLevels.Where(x => x.Id == judgement.ExecutionFileLevelId).FirstOrDefault().NameAr;
                            executionRequest.CaseId = Guid.Parse(CaseId);
                            executionRequest.SectorTypeId = (int)loginState.UserDetail.SectorTypeId;
                            executionRequest.CreatedBy = loginState.UserDetail.UserName;
                            executionRequest.CreatedDate = DateTime.Now;
                            executionRequest.IsDeleted = false;
                            mappedJudgement.mojExecutionRequest = executionRequest;
                        }
                        if (OldJudgment != null)
                        {
                            var index = dataCommunicationService.outcomeHearing.outcomeJudgement.IndexOf(OldJudgment);
                            dataCommunicationService.outcomeHearing.outcomeJudgement.Remove(OldJudgment);
                            await Task.Delay(100);
                            dataCommunicationService.outcomeHearing.outcomeJudgement.Insert(index, mappedJudgement);
                        }
                        else
                        {
                            dataCommunicationService.outcomeHearing.outcomeJudgement.Add(mappedJudgement);
                        }
                        await RedirectBack();
                        spinnerService.Hide();
                    }
                }
                else
                {
                    judgementDateValidationMsg = judgement.JudgementDate == default(DateTime) ? translationState.Translate("Required_Field") : "";
                    hearingDateValidationMsg = judgement.HearingDate == default(DateTime) ? translationState.Translate("Required_Field") : "";
                    typeValidationMsg = judgement.TypeId <= 0 ? translationState.Translate("Required_Field") : "";
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
                    RequestIds = new List<Guid> { judgement.Id },
                    CreatedBy = judgement.CreatedBy,
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
        protected async Task CopyAttachmentsFromSourceToDestination(Guid destinationId)
        {
            try
            {
                var docResponse = await fileUploadService.CopyAttachmentsFromSourceToDestination(new List<CopyAttachmentVM> { new CopyAttachmentVM()
                    {
                        SourceId = judgement.Id,
                        DestinationId = destinationId,
                        CreatedBy = judgement.CreatedBy
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
        protected async Task CopySelectedAttachmentsToDestination()
        {
            try
            {
                CopySelectedAttachmentsVM copyAttachments = new CopySelectedAttachmentsVM
                {
                    SelectedDocuments = executionRequest.SelectedDocuments.ToList(),
                    DestinationId = executionRequest.Id,
                    CreatedBy = loginState.Username
                };
                var docResponse = await fileUploadService.CopySelectedAttachmentsToDestination(copyAttachments);
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
        protected async Task PopulateAttachementsGrid()
        {
            try
            {
                if (CaseId != null)
                {
                    attachments = await fileUploadService.GetUploadedAttachements(false, 0, Guid.Parse(CaseId));
                    if (attachments != null && attachments.Any())
                    {
                        Regex regex = new Regex("_[0-9]+", RegexOptions.RightToLeft);
                        attachments = new ObservableCollection<TempAttachementVM>(attachments?.Select(f => { if (f.FileName.Contains("Document_Portfolio")) return f; else f.FileName = regex.Replace(f.FileName, "", 1); return f; }).ToList());
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
        #endregion

        #region Redirect and Dialog Events

        protected async void ButtonCancelClick(MouseEventArgs args)
        {
            await RedirectBack();
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

        protected void OnClickJudgementAmount(bool IsAmmountCollected)
        {
            try
            {
                if (judgement.Amount <= 0 && !IsAmmountCollected)
                    judgement.Amount = null;
                else if (judgement.AmountCollected <= 0 && IsAmmountCollected)
                    judgement.AmountCollected = null;
            }
            catch (Exception ex)
            {

            }

        }
        protected void OnFocusOutJudgementAmount(bool IsAmmountCollected)
        {
            try
            {

                if (judgement.Amount == null && !IsAmmountCollected)
                    judgement.Amount = 0;
                else if (judgement.AmountCollected == null && IsAmmountCollected)
                    judgement.AmountCollected = 0;
            }
            catch (Exception ex)
            {

            }

        }
    }
}
