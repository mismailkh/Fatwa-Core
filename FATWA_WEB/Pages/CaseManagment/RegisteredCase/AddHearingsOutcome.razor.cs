using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.MojRollsVM;
using FATWA_WEB.Pages.CaseManagment.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    //<History Author = 'Hassan Abbas' Date='2024-04-15' Version="1.0" Branch="master"> Outcomes of Hearing Roll in Bulk along with Parties Add/Remove, Documents and option to Save As Draft</History>
    public partial class AddHearingsOutcome : ComponentBase
    {

        #region Variables
        protected string dateValidationMsg = "";
        protected string timeValidationMsg = "";
        protected string statusValidationMsg = "";
        protected List<CopyAttachmentVM> copyAttachments = new List<CopyAttachmentVM>();
        protected int hearingIndex = 0;
        protected bool displayDocumentGrid = true;
        protected RadzenDataGrid<CasePartyLinkVM> PartiesGrid;
        protected List<CasePartyLinkVM> CasePartyLinks;
        List<CmsPrintHearingRollDetailVM> hearings = new List<CmsPrintHearingRollDetailVM>();
        MOJRollsRequestListVM hearingRollDetail = new MOJRollsRequestListVM();
        #endregion

        #region Component Load

        //<History Author = 'Muhammad Zaeem' Date='2024-04-7' Version = "1.0" Branch = "master" >Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                await PopulateHearingDetails();
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

        #region Dropdown Change Events
        protected async Task PopulateHearingDetails()
        {
            try
            {
                if (dataCommunicationService.hearingRollRequest == null)
                {
                    await RedirectBack();
                    return;
                }
                hearingRollDetail = dataCommunicationService.hearingRollRequest;
                dataCommunicationService.hearingRollRequest = null;

                CmsHearingRollDetailSearchVM cmsHearingRollDetailSearch = new CmsHearingRollDetailSearchVM
                {
                    HearingDate = (DateTime)hearingRollDetail.SessionDate,
                    ChamberNumberId = (int)hearingRollDetail.ChamberNumberId,
                    HearingRollId = hearingRollDetail.Id
                };

                var response = await mojRollsService.GetHearingRollDetailForPrintingAndOutcome(cmsHearingRollDetailSearch);
                if (response.IsSuccessStatusCode)
                {
                    hearings = (List<CmsPrintHearingRollDetailVM>)response.ResultData;
                    hearings.ForEach(x => { x.OutcomeHearing.HearingId = x.HearingId; x.OutcomeHearing.HearingDate = (DateTime)hearingRollDetail.SessionDate; x.OutcomeHearing.LawyerId = loginState.UserDetail.UserId; x.OutcomeHearing.CreatedBy = loginState.Username; });
                    if (hearings.Any())
                    {
                        await PopulatePartiesGrid(hearings[hearingIndex].CaseId);
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
        protected async Task PopulatePartiesGrid(Guid caseId)
        {
            var partyResponse = await caseRequestService.GetCMSCasePartyDetailById(caseId);
            if (partyResponse.IsSuccessStatusCode)
            {
                CasePartyLinks = (List<CasePartyLinkVM>)partyResponse.ResultData;
                CasePartyLinks = CasePartyLinks?.Select(c => { c.CasePartyCategory = (CasePartyCategoryEnum)c.CategoryId; c.CasePartyType = (CasePartyTypeEnum)c.TypeId; return c; }).ToList();
                CasePartyLinks = new List<CasePartyLinkVM>(CasePartyLinks?.Where(x => !hearings[hearingIndex].OutcomeHearing.DeletedParties.Any(p => p.Id == x.Id)).Concat(hearings[hearingIndex].OutcomeHearing.caseParties).ToList());
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(partyResponse);
            }
        }
        //<History Author = 'Muhammad Zaeem' Date='2024-04-7' Version="1.0" Branch="master">Check if Attachments</History>
        public void PartyRowRender(RowRenderEventArgs<CasePartyLinkVM> args)
        {
            try
            {
                if (args.Data.AttachmentCount <= 0)
                {
                    args.Attributes.Add("class", "no-party-attachment");
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Redirect and Dialog Events

        //< History Author = 'Hassan Abbas' Date = '2022-12-05' Version = "1.0" Branch = "master" >Next Outcome</History>
        protected async Task NextOutcome()
        {
            try
            {
                dateValidationMsg = string.Empty;
                hearingIndex++;
                displayDocumentGrid = false;
                await PopulatePartiesGrid(hearings[hearingIndex].CaseId);
                displayDocumentGrid = true;

            }
            catch (Exception ex)
            {

            }
        }

        //< History Author = 'Hassan Abbas' Date = '2022-12-05' Version = "1.0" Branch = "master" >Next Outcome</History>
        protected async Task PreviousOutcome()
        {
            try
            {
                dateValidationMsg = string.Empty;
                hearingIndex--;
                displayDocumentGrid = false;
                await PopulatePartiesGrid(hearings[hearingIndex].CaseId);
                displayDocumentGrid = true;
            }
            catch (Exception ex)
            {

            }
        }

        //< History Author = 'Hassan Abbas' Date = '2024-04-14' Version = "1.0" Branch = "master" >Submit Hearing Roll Outcome Details</History>
        protected async Task SaveAsDraft()
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Save_Draft"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    CmsHearingRollOutcomeDraftPayload payload = new CmsHearingRollOutcomeDraftPayload
                    {
                        Id = Guid.NewGuid(),
                        HearingRollId = hearingRollDetail.Id,
                        Payload = JsonConvert.SerializeObject(hearings),
                        CreatedBy = loginState.Username,
                        CreatedDate = DateTime.Now,
                    };
                    var response = await mojRollsService.SaveHearingRollOutcomesAsDraft(payload);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Changes_saved_successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await RedirectBack();
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

        //< History Author = 'Hassan Abbas' Date = '2024-04-14' Version = "1.0" Branch = "master" >Submit Hearing Roll Outcome Details</History>
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
                    var response = await mojRollsService.SaveHearingRollOutcomes(hearings);
                    if (response.IsSuccessStatusCode)
                    {
                        AssignHearingRollToLawyerVM hearingRollToLawyerVM = new AssignHearingRollToLawyerVM
                        {
                            HearingDate = (DateTime)hearingRollDetail.SessionDate,
                            ChamberNumberid = (int)hearingRollDetail.ChamberNumberId
                        };
                        //Update Hearing Roll HasOutcome Flag
                        var UpdateMojRollsReqeuest = await mojRollsService.UpdateMojRollsRequestHasOutcomeFlag(hearingRollToLawyerVM);
                        // Save TempAttachement To Upload Documents
                        await SaveTempAttachementToUploadedDocument();
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Hearing_Added_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await RedirectBack();
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
                    RequestIds = hearings.Select(x => x.OutcomeHearing.Id).ToList(),
                    CreatedBy = loginState.Username,
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

        #region Add Party, Delete Party

        //<History Author = 'Muhammad Zaeem' Date='2024-04-7' Version="1.0" Branch="master"> add the party return in the dialog result in Case Party Grid and also save it temporarily in outcome object</History>
        protected async Task AddParty(int categoryId, Guid caseId)
        {
            try
            {
                var result = await dialogService.OpenAsync<AddCaseParty>(translationState.Translate("Add_Case_Party"),
                    new Dictionary<string, object>()
                    {
                        { "CategoryId", categoryId },
                        { "ReferenceId", caseId.ToString() },
                        {"IsAutoSave", false },
                    },
                    new DialogOptions() { Width = "40% !important", CloseDialogOnOverlayClick = true }
                );
                var party = (CasePartyLinkVM)result;
                if (party != null)
                {
                    hearings[hearingIndex].OutcomeHearing.caseParties.Add(party);
                    CasePartyLinks.Add(party);
                    await PartiesGrid.Reload();
                    StateHasChanged();
                }
            }
            catch (Exception)
            {

            }
        }

        //<History Author = 'Muhammad Zaeem' Date='2024-04-7' Version="1.0" Branch="master"> Remove the Auto Delete from the Database on clicking the action and apply the delete functionality on form submit</History>
        protected async Task DeleteParty(CasePartyLinkVM party)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Delete"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                spinnerService.Show();
                if (hearings[hearingIndex].OutcomeHearing.caseParties.Contains(party))
                {
                    hearings[hearingIndex].OutcomeHearing.caseParties.Remove(party);
                }
                else
                {
                    hearings[hearingIndex].OutcomeHearing.DeletedParties.Add(party);
                }
                CasePartyLinks.Remove(party);
                PartiesGrid.Reload();
                spinnerService.Hide();
            }
        }
        #endregion

        #region Grid Button
        //<History Author = 'Ijaz Ahmad' Date='2024-02-07' Version="1.0" Branch="master"> Detail  Case Party Info</History>
        protected void DetailCaseParty(CasePartyLinkVM args)
        {
            navigationManager.NavigateTo("/caseparty-view/" + args.Id);
        }
        #endregion

    }
}
