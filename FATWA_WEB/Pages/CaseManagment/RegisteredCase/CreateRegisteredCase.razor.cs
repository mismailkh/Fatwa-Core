using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Create Registered Case</History>
    public partial class CreateRegisteredCase : ComponentBase
    {
        #region Paramter

        [Parameter]
        public string FileId { get; set; }
        [Parameter]
        public string MojRegistrationRequestId { get; set; }
        [Parameter]
        public string DocumentId { get; set; }
        [Parameter]
        public string AttachmentTypeId { get; set; }

        #endregion

        #region Variables

        CmsRegisteredCase registeredCase = new CmsRegisteredCase { CaseId = Guid.NewGuid(), CaseDate = DateTime.Now, HearingDate = DateTime.Now };
        protected CaseRequest caseRequest { get; set; } = new CaseRequest();
        public IList<CourtType> courtTypes { get; set; } = new List<CourtType>();
        public IList<Court> courts { get; set; } = new List<Court>();
        public IList<Chamber> chambers { get; set; } = new List<Chamber>();
        public List<ChamberNumber> ChamberNumbers { get; set; } = new List<ChamberNumber>();
        public int CourtTypeId { get; set; }
        public Chamber selectedChamber { get; set; } = new Chamber();
        public Court selectedCourt { get; set; } = new Court();
        protected MojRegistrationRequest MojRegistrationRequest { get; set; }
        public bool CaseDetailAccordian { get; set; } = true;
        public bool NumReqPer { get; set; } = true;
        public bool ValidateCheck { get; set; } = true;
        protected List<CopyAttachmentVM> copyAttachments = new List<CopyAttachmentVM>();
        protected List<RequestType> RequestTypes { get; set; } = new List<RequestType>();
        public int MandatoryAttachmentTypeId { get; set; }
        public Guid ReferenceGuid { get; set; }
        protected RadzenDataGrid<CmsRegisteredCaseFileDetailVM> gridRegionalCases;
        protected RadzenDataGrid<CmsRegisteredCaseFileDetailVM> gridAppealCases;
        protected RadzenDataGrid<CmsRegisteredCaseFileDetailVM> gridSupremeCases;
        protected List<CmsRegisteredCaseFileDetailVM> RegisteredCases { get; set; } = new List<CmsRegisteredCaseFileDetailVM>();

        #endregion

        #region Component Load

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            if (int.Parse(AttachmentTypeId) == (int)AttachmentTypeEnum.OrderOnPetitionNotes)
            {
                MandatoryAttachmentTypeId = (int)AttachmentTypeEnum.OrderOnPetitionCourtDecision;
                ReferenceGuid = Guid.Parse(FileId);
            }
            else if (int.Parse(AttachmentTypeId) == (int)AttachmentTypeEnum.PerformOrderNotes)
            {
                MandatoryAttachmentTypeId = (int)AttachmentTypeEnum.PerformOrderCourtDecision;
                ReferenceGuid = Guid.Parse(FileId);
            }
            else if (int.Parse(AttachmentTypeId) == (int)AttachmentTypeEnum.ClaimStatement)
            {
                MandatoryAttachmentTypeId = (int)AttachmentTypeEnum.ClaimStatement;
                ReferenceGuid = registeredCase.CaseId;
            }
            else
            {
                MandatoryAttachmentTypeId = (int)AttachmentTypeEnum.StopExecutionOfJudgment;
                ReferenceGuid = registeredCase.CaseId;
            }

            PopulateRequestTypes();
            await GetCaseRequestByFileId();
            await PopulateMojRegistrationRequest();
            await PopulateCourtTypes();
            await PopulateCourts();
            await PopulateRegisteredCasesByFileId();
            spinnerService.Hide();
        }

        #endregion

        #region Remote Dropdown Data  
        protected async void PopulateRequestTypes()
        {
            var response = await lookupService.GetRequestTypes();
            if (response.IsSuccessStatusCode)
            {
                RequestTypes = (List<RequestType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Populate MOJ Registration Request</History>
        protected async Task PopulateMojRegistrationRequest()
        {

            var response = await cmsCaseFileService.GetMojRegistrationRequestById(Guid.Parse(MojRegistrationRequestId));
            if (response.IsSuccessStatusCode)
            {
                MojRegistrationRequest = (MojRegistrationRequest)response.ResultData;
                if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeRegionalCases)
                {
                    CourtTypeId = (int)CourtTypeEnum.Regional;
                    registeredCase.SectorTypeId = (int)OperatingSectorTypeEnum.AdministrativeRegionalCases;
                    registeredCase.RequestTypeId = (int)RequestTypeEnum.Administrative;

                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialRegionalCases)
                {
                    CourtTypeId = (int)CourtTypeEnum.Regional;
                    registeredCase.SectorTypeId = (int)OperatingSectorTypeEnum.CivilCommercialRegionalCases;
                    registeredCase.RequestTypeId = (int)RequestTypeEnum.CivilCommercial;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeAppealCases)
                {
                    CourtTypeId = (int)CourtTypeEnum.Appeal;
                    registeredCase.SectorTypeId = (int)OperatingSectorTypeEnum.AdministrativeAppealCases;
                    registeredCase.RequestTypeId = (int)RequestTypeEnum.Administrative;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialAppealCases)
                {
                    CourtTypeId = (int)CourtTypeEnum.Appeal;
                    registeredCase.SectorTypeId = (int)OperatingSectorTypeEnum.CivilCommercialAppealCases;
                    registeredCase.RequestTypeId = (int)RequestTypeEnum.CivilCommercial;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeSupremeCases)
                {
                    CourtTypeId = (int)CourtTypeEnum.Supreme;
                    registeredCase.SectorTypeId = (int)OperatingSectorTypeEnum.AdministrativeSupremeCases;
                    registeredCase.RequestTypeId = (int)RequestTypeEnum.Administrative;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases)
                {
                    CourtTypeId = (int)CourtTypeEnum.Supreme;
                    registeredCase.SectorTypeId = (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases;
                    registeredCase.RequestTypeId = (int)RequestTypeEnum.CivilCommercial;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases)
                {
                    //CourtTypeId = (int)CourtTypeEnum.Regional;
                    registeredCase.SectorTypeId = (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases;
                    registeredCase.RequestTypeId = (int)RequestTypeEnum.CivilCommercial;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.PublicOperationalSector)
                {
                    registeredCase.SectorTypeId = (int)OperatingSectorTypeEnum.PublicOperationalSector;
                    registeredCase.RequestTypeId = caseRequest.RequestTypeId;
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task PopulateCourtTypes()
        {
            var response = await lookupService.GetCourtType();
            if (response.IsSuccessStatusCode)
            {
                courtTypes = (List<CourtType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        protected async Task PopulateCourts()
        {
            var response = await lookupService.GetCourt();
            if (response.IsSuccessStatusCode)
            {
                courts = (List<Court>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        protected async Task GetCaseRequestByFileId()
        {
            var caseRequestResponse = await cmsCaseFileService.GetCaseRequestByFileId(Guid.Parse(FileId));
            if (caseRequestResponse.IsSuccessStatusCode)
            {
                caseRequest = (CaseRequest)caseRequestResponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(caseRequestResponse);
            }
        }
        protected async Task PopulateRegisteredCasesByFileId()
        {
            var response = await cmsCaseFileService.GetAllRegisteredCasesByFileId(Guid.Parse(FileId));
            if (response.IsSuccessStatusCode)
            {
                RegisteredCases = (List<CmsRegisteredCaseFileDetailVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

            if (gridRegionalCases != null)
                translationState.TranslateGridFilterLabels(gridRegionalCases);
            if (gridAppealCases != null)
                translationState.TranslateGridFilterLabels(gridAppealCases);
            if (gridSupremeCases != null)
                translationState.TranslateGridFilterLabels(gridSupremeCases);
        }
        #endregion

        #region Dropdown Change Events

        protected async void OnChangeCourt()
        {
            registeredCase.ChamberId = 0;
            registeredCase.ChamberNumberId = 0;
            var response = await lookupService.GetChamberByCourtId(registeredCase.CourtId);
            if (response.IsSuccessStatusCode)
            {
                chambers = (List<Chamber>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        protected async Task OnChangeChamber(int chamberId)
        {
            registeredCase.ChamberNumberId = 0;
            var response = await lookupService.GetChamberNumbersByChamberId(chamberId);
            if (response.IsSuccessStatusCode)
            {
                ChamberNumbers = (List<ChamberNumber>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
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

        #endregion

        #region Button Events
        protected async Task FormInvalidSubmit()
        {
            try
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Fill_Required_Fields"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }


        //< History Author = 'Hassan Abbas' Date = '2022-11-20' Version = "1.0" Branch = "master" >Submit Form</History>
        //< History Author = 'Zain Ul Islam' Date = '2023-02-16' Version = "1.0" Branch = "master" >DMS implementation done separately for CreateRegisteredCase</History>

        protected async Task Form0Submit(CmsRegisteredCase args)
        {
            try
            {
                var attachments = await fileUploadService.GetTempAttachements(ReferenceGuid);
                if (int.Parse(AttachmentTypeId) == (int)AttachmentTypeEnum.ClaimStatement && !attachments.Where(t => t.AttachmentTypeId == (int)AttachmentTypeEnum.ClaimStatement).Any())
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Upload_Claim_Statement"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
                else if ((int.Parse(AttachmentTypeId) == (int)AttachmentTypeEnum.PerformOrderNotes
                            || int.Parse(AttachmentTypeId) == (int)AttachmentTypeEnum.OrderOnPetitionNotes)
                            && (!attachments.Where(t => t.AttachmentTypeId == (int)AttachmentTypeEnum.PerformOrderCourtDecision
                            || t.AttachmentTypeId == (int)AttachmentTypeEnum.OrderOnPetitionCourtDecision).Any()))
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Upload_Court_Decision"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }

                if (await dialogService.Confirm(translationState.Translate("Sure_Submit_Case"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    registeredCase.AttachmentTypeId = int.Parse(AttachmentTypeId);
                    registeredCase.FileId = Guid.Parse(FileId);
                    registeredCase.StatusId = (int)RegisteredCaseStatusEnum.Open;
                    registeredCase.HearingTime = registeredCase.Time.TimeOfDay;
                    registeredCase.MojRegistrationRequestId = Guid.Parse(MojRegistrationRequestId);
                    registeredCase.ClaimStatementCreatedBy = MojRegistrationRequest.CreatedBy;
                    var response = await cmsRegisteredCaseService.CreateRegisteredCase(registeredCase);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Case_Submitted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });

                        registeredCase = (CmsRegisteredCase)response.ResultData;
                        var mojRegReqResponse = await cmsCaseFileService.GetMojRegistrationRequestById(Guid.Parse(MojRegistrationRequestId));
                        await SaveTempAttachementToUploadedDocument();
                        await CopyAttachmentsFromSourceToDestination(registeredCase);
                        // Update Draft Document Status
                        var documents = await fileUploadService.GetUploadedAttachements(false, 0, Guid.Parse(FileId));
                        var VersionId = documents.Where(x => x.UploadedDocumentId == int.Parse(DocumentId)).Select(x => x.VersionId).FirstOrDefault();
                        CmsDraftedTemplateVersions draftedTemplateVersion = new CmsDraftedTemplateVersions();
                        draftedTemplateVersion.OldVersionId = (Guid)VersionId;
                        draftedTemplateVersion.StatusId = (int)DraftVersionStatusEnum.RegisteredInMOJ;
                        draftedTemplateVersion.CreatedBy = loginState.UserDetail.UserName;
                        draftedTemplateVersion.CreatedDate = DateTime.Now;
                        await cmsCaseTemplateService.UpdateDraftDocumentStatus(draftedTemplateVersion);

                        await Task.Delay(1500);
                        navigationManager.NavigateTo("/moj-registration-requests");
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response, "CaseNumber_Exists");
                    }

                    spinnerService.Hide();
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

        protected async Task SaveTempAttachementToUploadedDocument()
        {
            try
            {
                List<Guid> requestIds = new List<Guid>
                {
                    ReferenceGuid
                };

                var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                {
                    RequestIds = requestIds,
                    CreatedBy = registeredCase.CreatedBy,
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

        protected async Task CopyAttachmentsFromSourceToDestination(CmsRegisteredCase item)
        {
            try
            {
                AddCopyAttachment(new CopyAttachmentVM()
                {
                    SourceId = item.FileId,
                    DestinationId = item.CaseId,
                    CreatedBy = item.CreatedBy
                });

                if (item.CopyAttachmentVMs.Any())
                {
                    copyAttachments.AddRange(item.CopyAttachmentVMs);
                }

                var docResponse = await fileUploadService.CopyAttachmentsFromSourceToDestination(copyAttachments);
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

        protected void AddCopyAttachment(CopyAttachmentVM copyAttachment)
        {
            try
            {
                copyAttachments.Add(
                    new CopyAttachmentVM()
                    {
                        SourceId = copyAttachment.SourceId,
                        DestinationId = copyAttachment.DestinationId,
                        CreatedBy = copyAttachment.CreatedBy
                    });
            }
            catch (Exception)
            {
                return;
                throw;
            }

        }

        #endregion

        #region Redirect Function

        //<History Author = 'Hassan Abbas' Date='2022-11-25' Version="1.0" Branch="master">Redirect to respective page</History>
        protected async Task RedirectBack()
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Cancel"), translationState.Translate("Confirm_Cancel"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                //navigationManager.NavigateTo("/register-to-moj/" + FileId);
                navigationManager.NavigateTo("moj-registration-requests/");
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

        protected async Task RedirectBackBtn()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

    }
}
