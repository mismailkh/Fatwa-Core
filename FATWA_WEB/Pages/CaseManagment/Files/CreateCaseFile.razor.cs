using DocumentFormat.OpenXml.Spreadsheet;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Extensions;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CMSCOMSRequestNumberVMs;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_GENERAL.Helper;
using FATWA_WEB.Data;
using FATWA_WEB.Pages.CaseManagment.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.PdfViewer;
using Syncfusion.Blazor.PdfViewerServer;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.CaseManagment.Files
{
    public partial class CreateCaseFile : ComponentBase
    {
        #region Parameter
        [Parameter]
        public string RequestId { get; set; }
        #endregion

        #region Variables
        protected List<CaseRequestStatus> Statuses { get; set; } = new List<CaseRequestStatus>();
        protected List<GovernmentEntity> GovtEntities { get; set; } = new List<GovernmentEntity>();
        protected List<GEDepartments> Departments { get; set; } = new List<GEDepartments>();
        protected List<RequestType> RequestTypes { get; set; } = new List<RequestType>();
        protected List<Subtype> Subtypes { get; set; } = new List<Subtype>();
        protected List<CourtType> Courttypes { get; set; } = new List<CourtType>();
        protected List<PreCourtType> PreCourtTypes { get; set; } = new List<PreCourtType>();
        protected List<Priority> Priorities { get; set; } = new List<Priority>();

        protected RadzenDataGrid<CasePartyLinkVM>? PartiesGrid;
        protected bool ReloadFileUploader = true;
        List<string> ValidFiles { get; set; } = new List<string>() { ".pdf", ".jpg", ".png" };
        int MaxFileSize { get; set; } = 25 * 1024 * 1024; // 25 MB
        public TelerikPdfViewer? PdfViewerRef { get; set; }
        public byte[] FileData { get; set; }
        public bool ShowDocumentViewer { get; set; }
        public bool IsConfidentialDisabled;

        public string FileNumber { get; set; }
        public CaseRequestCommunicationVM? caseRequestCommunicationVMs { get; set; }
        protected CaseAssignment caseRequestLawyerAssignment = new CaseAssignment();
        protected bool PledgeErrorShow { get; set; } = false;

        public SfPdfViewerServer PdfViewerPreview;

        public PdfViewerToolbarSettings ToolbarSettings = new PdfViewerToolbarSettings()
        {
            ToolbarItems = new List<ToolbarItem>()
            {
                ToolbarItem.MagnificationTool,
                ToolbarItem.SelectionTool,
                ToolbarItem.UndoRedoTool,
                ToolbarItem.SearchOption,
                ToolbarItem.PanTool
            },
            AnnotationToolbarItems = new List<AnnotationToolbarItem>()
            {
                AnnotationToolbarItem.ShapeTool,
                AnnotationToolbarItem.ColorEditTool
            }
        };
        public string DocumentPathPreview { get; set; } = string.Empty;
        private DotNetObjectReference<CreateCaseFile>? dotNetRef;

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
            try
            {
                spinnerService.Show();
                draftHandlerService.SaveDraftForCase = SaveAsDraftForCase;

                if (dotNetRef is null)
                {
                    dotNetRef ??= DotNetObjectReference.Create(this);
                    await JSRuntime.InvokeVoidAsync("attachUnloadSaveListenerForCMS", dotNetRef);
                }

                await PopulateGovtEntities();

                //Populate dropdowns
                if (RequestId == null)
                {
                    //check if user is redirecting from List View
                    if (dataCommunicationService?.caseRequest != null)
                    {
                        dataCommunicationService.caseRequest.StatusId = (int)CaseRequestStatusEnum.Draft;
                        //by Default Case Request  Priorities is Low
                        dataCommunicationService.caseRequest.PriorityId = (int)CasePrioritiesEnum.Low;
                        dataCommunicationService.caseRequest.DepartmentId = loginState.UserDetail.DepartmentId;
                        //populate new file number

                    }
                    else
                    {
                        navigationManager.NavigateTo("registerd-requests");
                    }
                }
                else
                {
                    var response = await caseRequestService.GetCaseRequestById(Guid.Parse(RequestId));
                    if (response.IsSuccessStatusCode)
                    {
                        dataCommunicationService.caseRequest = (CaseRequest)response.ResultData;
                        var partyResponse = await caseRequestService.GetCMSCasePartyDetailById(Guid.Parse(RequestId));
                        if (partyResponse.IsSuccessStatusCode)
                        {
                            dataCommunicationService.caseRequest.CasePartyLinks = (List<CasePartyLinkVM>)partyResponse.ResultData;
                            dataCommunicationService.caseRequest.CasePartyLinks = dataCommunicationService.caseRequest?.CasePartyLinks?.Select(c => { c.CasePartyCategory = (CasePartyCategoryEnum)c.CategoryId; c.CasePartyType = (CasePartyTypeEnum)c.TypeId; return c; }).ToList();
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
                if (dataCommunicationService.caseRequest != null)
                {
                    await PopulateNewFileNumber();
                }
                await PopulateDropdowns();
                spinnerService.Hide();
            }
            catch (Exception)
            {
                spinnerService.Hide();
                throw;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadAuthorityLetter();
            }
        }
        #endregion

        #region Remote Dropdown Data and Dropdown Change Events
        protected async Task PopulateDropdowns()
        {
            await PopulateCaseRequestStatuses();
            //await PopulateDepartments(); // need to confirm
            await PopulatePriorities();
            await PopulateRequestTypes();
            await PopulateSectorSubtypes();
            await PapulateCourtTypes();
            await PapulatePreCourtTypes();
            await PopulateSupervisorId();
        }
        protected async Task PopulateRequestTypes()
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
        protected async Task OnSubTypeChange(object args)
        {
            try
            {
                ReloadFileUploader = false;
                await Task.Delay(100);
                ReloadFileUploader = true;
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
        protected async Task PopulateSectorSubtypes()
        {
            var response = await lookupService.GetAllRequestSubtypes();
            if (response.IsSuccessStatusCode)
            {
                Subtypes = (List<Subtype>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PapulateCourtTypes()
        {
            var response = await lookupService.GetCourtTypesByRequestType(dataCommunicationService.caseRequest != null ? (int)dataCommunicationService.caseRequest.RequestTypeId : 0);
            if (response.IsSuccessStatusCode)
            {
                Courttypes = (List<CourtType>)response.ResultData;
                if (dataCommunicationService.caseRequest != null)
                    dataCommunicationService.caseRequest.CourtTypeId = CaseConsultationExtension.GetCourtTypeIdBasedOnSectorId(loginState.UserDetail.SectorTypeId != null ? (int)loginState.UserDetail.SectorTypeId : 0);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PapulatePreCourtTypes()
        {
            var response = await lookupService.GetPreCourtTypes();
            if (response.IsSuccessStatusCode)
            {
                PreCourtTypes = (List<PreCourtType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulatePriorities()
        {
            var response = await lookupService.GetCasePriorities();
            if (response.IsSuccessStatusCode)
            {
                Priorities = (List<Priority>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulateGovtEntities()
        {
            var response = await lookupService.GetGovernmentEntities();
            if (response.IsSuccessStatusCode)
            {
                GovtEntities = (List<GovernmentEntity>)response.ResultData;
                if (dataCommunicationService.caseRequest != null)
                {
                    dataCommunicationService.caseRequest.IsConfidential = GovtEntities.Where(x => x.EntityId == dataCommunicationService.caseRequest.GovtEntityId).Select(y => y.IsConfidential).FirstOrDefault();
                    IsConfidentialDisabled = dataCommunicationService.caseRequest.IsConfidential == true;
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulateCaseRequestStatuses()
        {
            var response = await lookupService.GetCaseRequestStatuses();
            if (response.IsSuccessStatusCode)
            {
                Statuses = (List<CaseRequestStatus>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulateDepartments()
        {
            var response = await lookupService.GetDepartments();
            if (response.IsSuccessStatusCode)
            {
                Departments = (List<GEDepartments>)response.ResultData;

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulateNewFileNumber()
        {
            var response = await cmsCaseTemplateService.GenerateNumberPattern(0, (int)CmsComsNumPatternTypeEnum.CaseFileNumber);
            if (response.IsSuccessStatusCode)
            {
                var result = (NumberPatternResult)response.ResultData;
                FileNumber = result.GenerateRequestNumber;
                dataCommunicationService.caseRequest.cMSCOMSInboxOutbox = result;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        public void PartyRowRender(RowRenderEventArgs<CasePartyLinkVM> args)
        {
            try
            {
                if (args.Data.AttachmentCount <= 0)
                {
                    args.Attributes.Add("class", "no-party-attachment");
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected async void AuthorityLetterUpdated(bool isSelected)
        {
            ShowDocumentViewer = false;
            FileData = new byte[0];
            StateHasChanged();
            await LoadAuthorityLetter(true);
        }
        protected async Task LoadAuthorityLetter(bool isTemp = false)
        {
            try
            {
                ObservableCollection<TempAttachementVM> response = new ObservableCollection<TempAttachementVM>();
                if (RequestId == null)
                {
                    response = await fileUploadService.GetTempAttachements(dataCommunicationService.caseRequest?.RequestId);
                }
                else
                {
                    if (isTemp)
                        response = await fileUploadService.GetTempAttachements(dataCommunicationService.caseRequest?.RequestId);
                    else
                        response = await fileUploadService.GetUploadedAttachements(false, 0, Guid.Parse(RequestId));
                }
                if (response != null)
                {
                    var attachments = response;
                    var authorityLetter = attachments?.FirstOrDefault(t => t.AttachmentTypeId == (int)AttachmentTypeEnum.AuthorityLetter);
                    if (authorityLetter != null)
                    {
                        int data;
                        var physicalPath = string.Empty;
#if DEBUG
                        {
                            physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + authorityLetter.StoragePath).Replace(@"\\", @"\");
                        }
#else
						{
						    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + authorityLetter.StoragePath).Replace(@"\\", @"\");
							physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
						}
#endif
                        if (!string.IsNullOrEmpty(physicalPath))
                        {

                            string base64String = await DocumentEncryptionService.GetDecryptedDocumentBase64(physicalPath, authorityLetter.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                            DocumentPathPreview = "data:application/pdf;base64," + base64String;
                            ShowDocumentViewer = true;
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
                }
                else
                {
                    //await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
        protected async Task PopulateSupervisorId()
        {
            try
            {
                if (loginState.UserRoles.Any(x => x.RoleId == SystemRoles.Lawyer))
                {
                    var response = await lookupService.GetSupervisorByLawyerId(loginState.UserDetail.UserId);
                    if (response.IsSuccessStatusCode)
                    {
                        caseRequestLawyerAssignment.SupervisorId = (string)response.ResultData;
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
        #endregion

        #region Add Party, Delete Party
        protected async Task AddParty()
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<AddCaseParty>(
                    translationState.Translate("Add_Case_Party"),
                    new Dictionary<string, object>()
                    {
                        { "ReferenceId", dataCommunicationService.caseRequest.RequestId.ToString() },
                        { "IsAutoSave", false }
                    },
                    new DialogOptions() { CloseDialogOnOverlayClick = true });
                await Task.Delay(200);

                var party = (CasePartyLinkVM)dialogResult;
                if (party != null)
                {
                    dataCommunicationService.caseRequest.CasePartyLinks.Add(party);
                    await PartiesGrid.Reload();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected async Task DeleteParty(CasePartyLinkVM party)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Delete"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                dataCommunicationService.caseRequest.CasePartyLinks.Remove(party);
                var docResponse = await fileUploadService.RemoveDocument(party.Id.ToString(), true);
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
                await PartiesGrid.Reload();
            }
        }

        #endregion

        #region Button Events
        protected async Task OnChangePledge(object args)
        {
            if (args != null && (bool)args && PledgeErrorShow)
            {
                PledgeErrorShow = false;
            }
        }
        protected async Task Form0Submit(CaseRequest args)
        {
            try
            {
                if (!dataCommunicationService.caseRequest.CasePartyLinks.Any())
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Atleast_One_Party"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
                if (dataCommunicationService.caseRequest.MandatoryTempFiles.Count() <= 0 || dataCommunicationService.caseRequest.MandatoryTempFiles.Where(f => String.IsNullOrEmpty(f.FileName)).Count() > 0)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Must_Attach_Mandatory_Documents"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
                if (dataCommunicationService.caseRequest?.Pledge == false)
                {
                    PledgeErrorShow = true;
                    return;
                }
                else
                {
                    PledgeErrorShow = false;
                }
                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    if (dataCommunicationService.caseRequest.StatusId == (int)CaseRequestStatusEnum.Draft)
                    {
                        dataCommunicationService.caseRequest.StatusId = (int)CaseRequestStatusEnum.Submitted;
                    }
                    else
                    {
                        dataCommunicationService.caseRequest.StatusId = (int)CaseRequestStatusEnum.Resubmitted;
                    }
                    ApiCallResponse response;
                    //send create/update request
                    if (RequestId != null)
                    {
                        dataCommunicationService.caseRequest.IsEdit = true;
                    }
                    response = await cmsCaseFileService.CreateCaseFile(dataCommunicationService.caseRequest);

                    List<Guid> requestIds = new List<Guid>();
                    if (dataCommunicationService.caseRequest.CasePartyLinks.Any())
                    {
                        foreach (var caseParty in dataCommunicationService.caseRequest.CasePartyLinks)
                        {
                            requestIds.Add(caseParty.Id);
                        }
                    }
                    requestIds.Add(dataCommunicationService.caseRequest.RequestId);

                    var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                    {
                        RequestIds = requestIds,
                        CreatedBy = dataCommunicationService.caseRequest.CreatedBy,
                        FilePath = _config.GetValue<string>("dms_file_path"),
                        DeletedAttachementIds = dataCommunicationService.caseRequest.DeletedAttachementIds
                    });

                    caseRequestCommunicationVMs = (CaseRequestCommunicationVM)response.ResultData;
                    var docResponse2 = await fileUploadService.CopyAttachmentsFromSourceToDestination(caseRequestCommunicationVMs?.CaseRequest.CopyAttachmentVMs);

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
                    spinnerService.Hide();
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = RequestId == null ? translationState.Translate("file_Submitted_Successfully") : translationState.Translate("file_Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        if (dataCommunicationService.caseRequest.CourtTypeId != (int)CourtTypeEnum.Regional)
                        {
                            await AssignFileToLawyer(caseRequestCommunicationVMs?.CaseFile.FileId, caseRequestCommunicationVMs?.CaseRequest.RequestId);
                        }
                        else
                        {
                            await RedirectBack();
                            dataCommunicationService.caseRequest = null;
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
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        protected async Task CancelRequest()
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Cancel"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                navigationManager.NavigateTo("registerd-requests");
            }
        }
        public async Task<bool> SaveAsDraftForCase(bool isCultureTriggered = false)
        {
            if (dataCommunicationService.caseRequest?.Subject?.Length > 100 ||
               dataCommunicationService.caseRequest?.CaseRequirements?.Length > 300 ||
               dataCommunicationService.caseRequest?.Remarks?.Length > 300 || (dataCommunicationService.caseRequest?.Subject is not null
                && !Regex.IsMatch(dataCommunicationService.caseRequest?.Subject, RegexPatterns.NoLeadingSpacesTextAreaPattern)) || (dataCommunicationService.caseRequest?.CaseRequirements is not null
                && !Regex.IsMatch(dataCommunicationService.caseRequest?.CaseRequirements, RegexPatterns.NoLeadingSpacesTextAreaPattern)) || (dataCommunicationService.caseRequest?.Remarks is not null
                && !Regex.IsMatch(dataCommunicationService.caseRequest?.Remarks, RegexPatterns.NoLeadingSpacesTextAreaPattern)))
            {
                await JsInterop.InvokeVoidAsync("ScrollCaseRequestToTop");
                return false;
            }
            else
            {
                if (isCultureTriggered || await dialogService.Confirm(translationState.Translate("Sure_Draft"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    dataCommunicationService.caseRequest.StatusId = (int)CaseRequestStatusEnum.Draft;

                    ApiCallResponse response;
                    //send create/update request
                    if (RequestId != null)
                    {
                        dataCommunicationService.caseRequest.IsEdit = true;
                    }

                    response = await cmsCaseFileService.CreateCaseFile(dataCommunicationService.caseRequest);

                    spinnerService.Hide();
                    if (response.IsSuccessStatusCode)
                    {
                        List<Guid> requestIds = new List<Guid>();
                        if (dataCommunicationService.caseRequest.CasePartyLinks.Any())
                        {
                            foreach (var caseParty in dataCommunicationService.caseRequest.CasePartyLinks)
                            {
                                requestIds.Add(caseParty.Id);
                            }
                        }
                        requestIds.Add(dataCommunicationService.caseRequest.RequestId);


                        var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                        {
                            RequestIds = requestIds,
                            CreatedBy = dataCommunicationService.caseRequest.CreatedBy,
                            FilePath = _config.GetValue<string>("dms_file_path"),
                            DeletedAttachementIds = dataCommunicationService.caseRequest.DeletedAttachementIds
                        });

                        if (!docResponse.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Attachment_Save_Failed"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            return false;
                        }
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Draft_Saved"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        if (!isCultureTriggered)
                        {
                            await RedirectBack();
                            dataCommunicationService.caseRequest = null;
                        }
                        return true;
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        return false;
                    }
                }
                else { return false; }
            }

        }

        protected async Task SubmitClicked()
        {
            if (dataCommunicationService.caseRequest?.PriorityId == null ||
                 dataCommunicationService.caseRequest?.PriorityId <= 0 ||
                 dataCommunicationService.caseRequest?.CourtTypeId == null ||
                 dataCommunicationService.caseRequest?.CourtTypeId <= 0 ||
             String.IsNullOrEmpty(dataCommunicationService.caseRequest?.Subject) ||
                 dataCommunicationService.caseRequest?.Subject?.Length > 50 || (dataCommunicationService.caseRequest?.Subject is not null
                 && !Regex.IsMatch(dataCommunicationService.caseRequest?.Subject, RegexPatterns.NoLeadingSpacesTextAreaPattern))
                 ||
                 dataCommunicationService.caseRequest?.CaseRequirements?.Length > 1000 || (dataCommunicationService.caseRequest?.CaseRequirements is not null
                 && !Regex.IsMatch(dataCommunicationService.caseRequest?.CaseRequirements, RegexPatterns.NoLeadingSpacesTextAreaPattern)) ||
                 dataCommunicationService.caseRequest?.Remarks?.Length > 1000 || (dataCommunicationService.caseRequest?.Remarks is not null
                 && !Regex.IsMatch(dataCommunicationService.caseRequest?.Remarks, RegexPatterns.NoLeadingSpacesTextAreaPattern))
                 ||
                 dataCommunicationService.caseRequest?.ClaimAmount > 20 ||
                 dataCommunicationService.caseRequest?.ClaimAmount < 0 ||
                 (dataCommunicationService.caseRequest?.RequestTypeId != (int)RequestTypeEnum.Administrative &&
                 (dataCommunicationService.caseRequest?.SubTypeId == null ||
                     dataCommunicationService.caseRequest?.SubTypeId <= 0 ||
                     dataCommunicationService.caseRequest?.PreCourtTypeId == null ||
                     dataCommunicationService.caseRequest?.PreCourtTypeId <= 0)))
            {
                await JsInterop.InvokeVoidAsync("ScrollCaseRequestToTop");
            }
        }

        #endregion
        #region Assign File To Lawyer
        protected async Task AssignFileToLawyer(Guid? FileId, Guid? RequestId)
        {
            try
            {
                if (loginState.UserRoles.Any(x => x.RoleId == SystemRoles.Lawyer))
                {
                    if (caseRequestLawyerAssignment.Remarks == null)
                    {
                        caseRequestLawyerAssignment.Remarks = string.Empty;
                    }
                    caseRequestLawyerAssignment.RequestId = RequestId;
                    caseRequestLawyerAssignment.AssignCaseToLawyerType = (int)AssignCaseToLawyerTypeEnum.CaseFile;
                    caseRequestLawyerAssignment.ReferenceId = (Guid)FileId;
                    caseRequestLawyerAssignment.SectorTypeId = loginState.UserDetail.SectorTypeId;
                    caseRequestLawyerAssignment.LawyerId = loginState.UserDetail.UserId;
                    var DepartmentandGovtEntityUser = await BrowserStorage.GetItemAsync<UserDetailVM>("UserDetail");

                    var response = await cmsSharedService.AssignCaseRequestToLawyer(caseRequestLawyerAssignment, DepartmentandGovtEntityUser.UserName);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Request_Assigned"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                    }
                }
                else
                {
                    var result = await dialogService.OpenAsync<AssignToLawyer>(translationState.Translate("Assign_To_Lawyer"),
                    new Dictionary<string, object>()
                    {
                             { "ReferenceId",  (Guid)FileId},
                             { "AssignCaseLawyerType", (int)AssignCaseToLawyerTypeEnum.CaseFile },
                    },
                    new DialogOptions() { Width = "45% !important", CloseDialogOnOverlayClick = true });
                }
                navigationManager.NavigateTo("registerd-requests");
                dataCommunicationService.caseRequest = null;
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

        #region Dispose
        public void Dispose()
        {
            dotNetRef?.Dispose();
            dotNetRef = null;
            dataCommunicationService.caseRequest = null;
            draftHandlerService.SaveDraftForCase = null;
            JSRuntime.InvokeVoidAsync("window.removeUnloadListenerCMS");
        }
        #endregion
    }
}
