using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_WEB.Pages.CaseManagment.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    //<History Author = 'Hassan Abbas' Date='2022-12-05' Version="1.0" Branch="master"> Add Outcome of Hearing</History>
    public partial class AddOutcomeOfHearing : ComponentBase
    {
        #region Parameter

        [Parameter]
        public dynamic HearingId { get; set; }
        [Parameter]
        public dynamic CaseId { get; set; }
        [Parameter]
        public dynamic OutcomeId { get; set; }
        #endregion

        #region Variables
        public OutcomeHearing outcomeHearing { get; set; } = new OutcomeHearing { Id = Guid.NewGuid(), HearingDate = DateTime.Now, NextHearingDate = DateTime.Now.AddDays(1) };
        protected List<HearingStatus> hearingStatuses { get; set; } = new List<HearingStatus>();
        protected HearingDetailVM hearingDetail { get; set; } = new HearingDetailVM();
        protected CmsRegisteredCaseDetailVM registeredCase { get; set; } = new CmsRegisteredCaseDetailVM();
        public IEnumerable<LawyerVM> users { get; set; }
        protected string dateValidationMsg = "";
        protected string timeValidationMsg = "";
        protected string lawyerValidationMsg = "";
        List<string> ValidFiles { get; set; } = new List<string>() { ".pdf", ".jpg", ".png" };
        public bool CaseDetailAccordian { get; set; } = true;
        protected bool RefreshFileUploadGrid { get; set; } = true;
        protected List<CopyAttachmentVM> copyAttachments = new List<CopyAttachmentVM>();

        protected RadzenDataGrid<CasePartyLinkVM> PartiesGrid;
        protected RadzenDataGrid<JudgementVM> judgementGrid;
        protected RadzenDataGrid<CmsRegisteredCaseTransferRequestVM> CaseTransferRequestGrid;
        protected List<CasePartyLinkVM> CasePartyLinks;
        protected List<CmsRegisteredCaseTransferRequestVM> CmsRegisteredCaseTransferRequestList = new List<CmsRegisteredCaseTransferRequestVM>();
        public bool allowRowSelectOnRowClick = true;

        #endregion

        #region Component Load

        //< History Author = 'Hassan Abbas' Date = '2022-09-29' Version = "1.0" Branch = "master" >Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                await PopulateRegisteredCaseDetail();
                await PopulateHearingDetails();
                await PopulatePartiesGrid();
                if (OutcomeId != null)
                {
                    await PopulateOutcomeDetailById();
                    await PopulateJudgements();
                    outcomeHearing.Id = Guid.Parse(OutcomeId);
                    await PopulateRegisteredCaseTransferRequests();
                    if (outcomeHearing.caseTransferRequestsVM.Any())
                    {
                        outcomeHearing.caseTransferRequestsVM.Where(x => x.OutcomeId == Guid.Parse(OutcomeId)).ToList().ForEach(x => x.IsAlreadyExist = true);
                    }
                }
                if (dataCommunicationService.outcomeHearing != null)
                {
                    outcomeHearing = dataCommunicationService.outcomeHearing;
                    CasePartyLinks = dataCommunicationService.outcomeHearing.CasePartyLinks;
                }
                spinnerService.Hide();
            }
            catch (Exception)
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

        #region Populate Dropdown Events

        protected async Task PopulateHearingDetails()
        {
            try
            {
                var response = await cmsRegisteredCaseService.GetHearingDetail(Guid.Parse(HearingId));
                if (response.IsSuccessStatusCode)
                {
                    hearingDetail = (HearingDetailVM)response.ResultData;
                    outcomeHearing.LawyerId = hearingDetail.LawyerId;
                    outcomeHearing.HearingDate = hearingDetail.HearingDate;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //<History Author = 'Hassan Abbas' Date='2023-12-04' Version="1.0" Branch="master">Populate Case Details</History>
        protected async Task PopulateRegisteredCaseDetail()
        {
            var result = await cmsRegisteredCaseService.GetRegisteredCaseDetailByIdVM(Guid.Parse(CaseId));
            if (result.IsSuccessStatusCode)
            {
                registeredCase = (CmsRegisteredCaseDetailVM)result.ResultData;
                await PopulateLawyerslist(registeredCase.ChamberNumberId);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }
        //< History Author = 'Hassan Abbas' Date = '2022-09-29' Version = "1.0" Branch = "master" >Populate Lawyer List</History>
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
        protected async Task PopulatePartiesGrid()
        {
            var partyResponse = await caseRequestService.GetCMSCasePartyDetailById(Guid.Parse(CaseId));
            if (partyResponse.IsSuccessStatusCode)
            {
                CasePartyLinks = (List<CasePartyLinkVM>)partyResponse.ResultData;
                CasePartyLinks = CasePartyLinks?.Select(c => { c.CasePartyCategory = (CasePartyCategoryEnum)c.CategoryId; c.CasePartyType = (CasePartyTypeEnum)c.TypeId; return c; }).ToList();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(partyResponse);
            }
        }
        //<History Author = 'Hassan Abbas' Date='2023-03-11' Version="1.0" Branch="master">Check if Attachments</History>
        public void PartyRowRender(RowRenderEventArgs<CasePartyLinkVM> args)
        {
            try
            {
                if (args.Data.AttachmentCount <= 0)
                {
                    args.Attributes.Add("class", "no-party-attachment");
                }
            }
            catch (Exception )
            {
            }
        }
        public async Task PopulateOutcomeDetailById()
        {
            var response = await cmsRegisteredCaseService.GetOutcomeByOutcomeId(Guid.Parse(OutcomeId));
            if (response.IsSuccessStatusCode)
            {
                outcomeHearing = (OutcomeHearing)response.ResultData;
                if (outcomeHearing != null)
                {
                    outcomeHearing.IsExist = true;
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task PopulateJudgements()
        {
            try
            {
                var response = await cmsRegisteredCaseService.GetJudgementsByOutcome(Guid.Parse(OutcomeId));
                if (response.IsSuccessStatusCode)
                {
                    var outcomeJudgements = (List<JudgementVM>)response.ResultData;
                    if (outcomeJudgements.Count > 0)
                    {
                        outcomeHearing.outcomeJudgement = outcomeJudgements;
                        outcomeHearing.outcomeJudgement = outcomeHearing.outcomeJudgement.Select(judgement =>
                        {
                            judgement.IsUpdated = true;
                            return judgement;
                        }).ToList();
                        StateHasChanged();
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region Redirect and Dialog Events

        //< History Author = 'Hassan Abbas' Date = '2022-12-05' Version = "1.0" Branch = "master" >Submit Hearing Details</History>
        protected async Task Form0Submit(OutcomeHearing outcomeHearing)
        {
            try
            {
                if (!String.IsNullOrEmpty(outcomeHearing.LawyerId))
                {
                    if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    }) == true)
                    {
                        spinnerService.Show();
                        outcomeHearing.HearingTime = outcomeHearing.Time.TimeOfDay;
                        outcomeHearing.HearingId = Guid.Parse(HearingId);
                        outcomeHearing.CaseId = Guid.Parse(CaseId);
                        var response = await cmsRegisteredCaseService.AddOutcomeHearing(outcomeHearing);
                        if (response.IsSuccessStatusCode)
                        {
                            // Save TempAttachement To Upload Documents
                            await SaveTempAttachementToUploadedDocument(outcomeHearing.Id, outcomeHearing.CreatedBy);
                            await CopyAttachmentsFromSourceToDestination(outcomeHearing.Id, outcomeHearing.CreatedBy, outcomeHearing.CaseId);
                            if (outcomeHearing.outcomeJudgement.Count > 0)
                            {
                                var judgements = outcomeHearing.outcomeJudgement.Where(x => x.CreatedDateTime != null && x.CreatedDateTime.Value.Date == DateTime.Now.Date);
                                foreach (var outcomeJudgement in judgements)
                                {
                                    await SaveTempAttachementToUploadedDocument(outcomeJudgement.Id, outcomeHearing.CreatedBy);
                                    await CopyAttachmentsFromSourceToDestination(outcomeJudgement.Id, outcomeJudgement.CreatedBy, outcomeHearing.CaseId);
                                    await CopySelectedAttachmentsToDestinationForJudgement(outcomeJudgement);
                                    if (outcomeJudgement.mojExecutionRequest != null)
                                    {
                                        await CopyAttachmentsFromSourceToDestination(outcomeJudgement.Id, outcomeJudgement.mojExecutionRequest.CreatedBy, outcomeJudgement.mojExecutionRequest.Id);
                                    }
                                }
                            }
                            if (outcomeHearing.caseTransferRequestsVM.Any())
                            {
                                foreach (var cmsCaseTransferRequest in outcomeHearing.caseTransferRequestsVM)
                                {
                                    await SaveTempAttachementToUploadedDocument(cmsCaseTransferRequest.OutcomeId, cmsCaseTransferRequest.CreatedBy);
                                }
                            }
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Outcome_Added_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            dataCommunicationService.outcomeHearing = null;
                            navigationManager.NavigateTo("/case-view/" + CaseId);
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                        spinnerService.Hide();
                    }
                }
                else
                {
                    lawyerValidationMsg = String.IsNullOrEmpty(outcomeHearing.LawyerId) ? translationState.Translate("Required_Field") : "";
                }
            }
            catch (Exception)
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
        protected async Task SaveTempAttachementToUploadedDocument(Guid ReferenceId, string CreatedBy)
        {
            try
            {
                List<Guid> requestIds = new List<Guid> { ReferenceId };

                var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                {
                    RequestIds = requestIds,
                    CreatedBy = CreatedBy,
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
        protected async Task CopyAttachmentsFromSourceToDestination(Guid SourceId, string CreatedBy, Guid DestinationId)
        {
            try
            {
                var docResponse = await fileUploadService.CopyAttachmentsFromSourceToDestination(new List<CopyAttachmentVM> { new CopyAttachmentVM()
                    {
                        SourceId = SourceId,
                        DestinationId = DestinationId,
                        CreatedBy = CreatedBy
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
        protected async Task CopySelectedAttachmentsToDestination(MojExecutionRequest executionRequest)
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
        protected async Task CopySelectedAttachmentsToDestinationForJudgement(JudgementVM judgementVM)
        {
            try
            {
                CopySelectedAttachmentsVM copyAttachments = new CopySelectedAttachmentsVM
                {
                    SelectedDocuments = judgementVM.SelectedDocuments.ToList(),
                    DestinationId = judgementVM.Id,
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
        protected async Task CloseDialog(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/case-view/" + CaseId);
        }
        protected async void OnChangeAccordion()
        {
            if (CaseDetailAccordian == false)
            {
                CaseDetailAccordian = true;
            }
            else
            {
                CaseDetailAccordian = false;
            }
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
        #region Add Party, Delete Party

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Open Add Party dialog</History>
        //<History Author = 'Muhammad Zaeem' Date='2023-12-28' Version="1.0" Branch="master"> add the party return in the dialog result in Case Party Grid and also save it temporarily in outcome object</History>
        protected async Task AddParty(int categoryId)
        {
            try
            {
                var result = await dialogService.OpenAsync<AddCaseParty>(translationState.Translate("Add_Case_Party"),
                    new Dictionary<string, object>()
                    {
                        { "CategoryId", categoryId },
                        { "ReferenceId", CaseId.ToString() },
                        {"IsAutoSave", false },
                    },
                    new DialogOptions() { Width = "40% !important", CloseDialogOnOverlayClick = true }
                );
                var party = (CasePartyLinkVM)result;
                if (party != null)
                {
                    outcomeHearing.caseParties.Add(party);
                    CasePartyLinks.Add(party);
                    await PartiesGrid.Reload();
                    StateHasChanged();
                }
            }
            catch (Exception)
            {

            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Delete Party</History>
        //<History Author = 'Muhammad Zaeem' Date='2023-12-28' Version="1.0" Branch="master"> Remove the Auto Delete from the Database on clicking the action and apply the delete functionality on form submit</History>
        protected async Task DeleteParty(CasePartyLinkVM party)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Delete"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                spinnerService.Show();
                if (outcomeHearing.caseParties.Contains(party))
                {
                    outcomeHearing.caseParties.Remove(party);
                }
                else
                {
                    outcomeHearing.DeletedParties.Add(party);
                }
                CasePartyLinks.Remove(party);
                PartiesGrid.Reload();
                spinnerService.Hide();
            }
        }
        #endregion

        #region Add / Edit / View Judgement
        protected async Task AddJudgement()
        {
           if(outcomeHearing != null)
            {
                outcomeHearing.CasePartyLinks = CasePartyLinks;
                dataCommunicationService.outcomeHearing = outcomeHearing;

            }
            navigationManager.NavigateTo("add-judgement/" + outcomeHearing.Id + "/" + Guid.Parse(CaseId));

        }
        protected async Task EditJudgement(JudgementVM args)
        {
            if (outcomeHearing != null)
            {
                outcomeHearing.CasePartyLinks = CasePartyLinks;
                dataCommunicationService.outcomeHearing = outcomeHearing;
            }
            navigationManager.NavigateTo("add-judgement/" + args.Id + "/" + args.OutcomeId + "/" + args.CaseId);
        }
        protected async Task DetailJudgement(JudgementVM args)
        {
            if (outcomeHearing != null)
            {
                dataCommunicationService.outcomeHearing = outcomeHearing;
            }
            navigationManager.NavigateTo("judgement-view/" + args.Id);
        }
        #endregion

        #region Grid Button
        //<History Author = 'Ijaz Ahmad' Date='2024-02-07' Version="1.0" Branch="master"> Detail  Case Party Info</History>
        protected void DetailCaseParty(CasePartyLinkVM args)
        {
            navigationManager.NavigateTo("/caseparty-view/" + args.Id);
        }
        #endregion

        #region CMS Registered Case Transfer Request
        protected async Task PopulateRegisteredCaseTransferRequests()
        {
            try
            {
                var response = await cmsRegisteredCaseService.GetRegisterdCaseTransferRequestList(Guid.Parse(OutcomeId));
                if (response.IsSuccessStatusCode)
                {
                    outcomeHearing.caseTransferRequestsVM = (List<CmsRegisteredCaseTransferRequestVM>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async Task SelectChamberDetailForTransfer(MouseEventArgs args)
        {
            try
            {
                var result = await dialogService.OpenAsync<CreateRegisteredCaseTransferRequest>(translationState.Translate("Request_For_Transfer"),
                new Dictionary<string, object> {
                    { "ChamberId", registeredCase.ChamberId },
                    {"CaseId", registeredCase.CaseId },
                    {"ChamberNumberId", registeredCase.ChamberNumberId },
                    {"CourtId",registeredCase.CourtId },
                    {"OutComeId", outcomeHearing.Id},
                    {"ChamberNameEn", registeredCase.ChamberNameEn},
                    {"ChamberNameAr", registeredCase.ChamberNameAr},
                    {"ChamberNumber", registeredCase.ChamberNumber},
                },
                new DialogOptions() { Width = "60%", Resizable = true, CloseDialogOnOverlayClick = true }
                );
                if (result == true)
                {
                    if (dataCommunicationService.outcomeHearing != null && dataCommunicationService.outcomeHearing.caseTransferRequestsVM.Count > 0)
                    {
                        outcomeHearing.caseTransferRequestsVM = dataCommunicationService.outcomeHearing.caseTransferRequestsVM;
                        await CaseTransferRequestGrid.Reload();
                        StateHasChanged();
                    }
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
        protected async Task DeleteCaseTransferRequest(CmsRegisteredCaseTransferRequestVM cmsRegisteredCaseTransferRequestVM)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Delete"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                spinnerService.Show();
                if (outcomeHearing.caseTransferRequestsVM.Contains(cmsRegisteredCaseTransferRequestVM))
                {
                    outcomeHearing.caseTransferRequestsVM.Remove(cmsRegisteredCaseTransferRequestVM);
                }
                await CaseTransferRequestGrid.Reload();
                spinnerService.Hide();
            }
        }
        protected async Task SoftDeleteCaseTransferRequest(string userName)
        {
            try
            {
                var response = await cmsRegisteredCaseService.SoftDeleteCaseTransferRequest(Guid.Parse(OutcomeId), userName);
                if (response.IsSuccessStatusCode)
                {
                    var result = (bool)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
