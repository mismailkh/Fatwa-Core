using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    public partial class AddSubCaseDetail : ComponentBase
    {

        #region Parameter

        [Parameter]
        public dynamic FileId { get; set; }
        [Parameter]
        public dynamic CaseId { get; set; }
        #endregion

        #region variables
        public string ChamberNumber { get; set; } = string.Empty;
        public string CannNumber { get; set; }
        public bool ShowWizard { get; set; } = true;
        public bool SubCaseAccordian { get; set; } = true;
        public bool DocumentAccordian { get; set; }

        public bool ValidateCheck { get; set; } = true;
        public IList<User> users { get; set; } = new List<User>();
        public IList<Court> courts { get; set; } = new List<Court>();
        public IList<CourtType> courtTypes { get; set; } = new List<CourtType>();
        public IList<Chamber> chambers { get; set; } = new List<Chamber>();
        public List<ChamberNumber> ChamberNumbers { get; set; } = new List<ChamberNumber>();
        public int Value { get; set; } = 0;
        public int CourtTypeId { get; set; } = 0;
        bool isBasicStep = false;
        public DateTime Max = new DateTime(DateTime.Now.Date.Ticks);
        public DateTime Min = new DateTime(2001, 1, 1);


        public int MaxPersent { get; set; } = 100;
        public int MinPersent { get; set; } = 0;
        public bool chbHasRequirment { get; set; }
        public bool NumReqPer { get; set; } = true;
        protected CaseRequest caseRequest { get; set; } = new CaseRequest();

        public CmsRegisteredCase cmsRegisteredCase = new CmsRegisteredCase { IsSubCase = true, CaseId = Guid.NewGuid(), CreatedDate = DateTime.Now, StatusId = (int)RegisteredCaseStatusEnum.Open };
        public CmsRegisteredCaseSubCase cmsRegisteredSubCaseManytoMany = new CmsRegisteredCaseSubCase { };
        protected CmsRegisteredCaseDetailVM registeredCase { get; set; } = new CmsRegisteredCaseDetailVM();
        protected List<CopyAttachmentVM> copyAttachments = new List<CopyAttachmentVM>();
        public TelerikForm BasicSectionForm { get; set; }
        public Court court = new Court();
        public Chamber chamber = new Chamber();
        public string caseIdUrl { get; set; }
        #endregion

        #region Model full property Instance

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        #endregion

        void Change()
        {
            if (SubCaseAccordian == false)
            {
                SubCaseAccordian = true;
            }
            else
            {
                SubCaseAccordian = false;
            }
        }

        //<History Author = 'ijaz ahmad' Date='2023-03-01' Version="1.0" Branch="master">Save Sub case many to many relation table </History>
        #region Submit button click
        protected async Task Form0Submit(CmsRegisteredCase args)
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
               translationState.Translate("Sure_Submit_Request"),
               translationState.Translate("Confirm"),
               new ConfirmOptions()
               {
                   OkButtonText = @translationState.Translate("Yes"),
                   CancelButtonText = @translationState.Translate("No")
               });
                if (dialogResponse == true)
                {
                    cmsRegisteredCase.FileId = Guid.Parse(FileId);
                    cmsRegisteredCase.ParentCaseId = Guid.Parse(CaseId);
                    cmsRegisteredCase.CreatedBy = loginState.Username;

                    var response = await cmsRegisteredCaseService.CreateSubCase(cmsRegisteredCase);
                    spinnerService.Hide();
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Sub_Case_Sucess_Saved"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });


                        cmsRegisteredCase = (CmsRegisteredCase)response.ResultData;
                        var docResponse = await fileUploadService.CopyAttachmentsFromSourceToDestination(cmsRegisteredCase.CopyAttachmentVMs);
                        await SaveTempAttachementToUploadedDocument();
                        dialogService.Close(cmsRegisteredCase);
                        if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases)
                            navigationManager.NavigateTo("/moj-registration-requests/");
                        else
                            navigationManager.NavigateTo("/case-view/" + CaseId);

                    }
                    else
                    {
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
            catch (Exception)
            {

            }
        }
        protected async Task SaveTempAttachementToUploadedDocument()
        {
            try
            {
                List<Guid> requestIds = new List<Guid>
                {
                    cmsRegisteredCase.CaseId
                };

                var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                {
                    RequestIds = requestIds,
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
        #endregion
        #region On Change Event 

        protected async void OnChangeCourt()
        {
            cmsRegisteredCase.ChamberId = 0;
            cmsRegisteredCase.ChamberNumberId = 0;
            var response = await lookupService.GetChamberByCourtId(cmsRegisteredCase.CourtId);
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
            cmsRegisteredCase.ChamberNumberId = 0;
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
        protected async void OnchangeChecked()
        {
            if (!chbHasRequirment)
            {
                if (NumReqPer != true)
                {
                    NumReqPer = true;
                    ValidateCheck = true;

                }
                else
                {
                    NumReqPer = false;
                    ValidateCheck = false;
                }
            }

        }

        #endregion

        #region Component Load

        #region Lookups
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
        protected async Task PopulateCaseTypedropdown()
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
        protected async Task PopulateCasedropdown()
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
        protected async Task PopulateCourtType()
        {
            if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeRegionalCases)
            {
                CourtTypeId = (int)CourtTypeEnum.Regional;
                cmsRegisteredCase.SectorTypeId = (int)OperatingSectorTypeEnum.AdministrativeRegionalCases;
                cmsRegisteredCase.RequestTypeId = (int)RequestTypeEnum.Administrative;
            }
            else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialRegionalCases)
            {
                CourtTypeId = (int)CourtTypeEnum.Regional;
                cmsRegisteredCase.SectorTypeId = (int)OperatingSectorTypeEnum.CivilCommercialRegionalCases;
                cmsRegisteredCase.RequestTypeId = (int)RequestTypeEnum.CivilCommercial;
            }
            else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeAppealCases)
            {
                CourtTypeId = (int)CourtTypeEnum.Appeal;
                cmsRegisteredCase.SectorTypeId = (int)OperatingSectorTypeEnum.AdministrativeAppealCases;
                cmsRegisteredCase.RequestTypeId = (int)RequestTypeEnum.Administrative;
            }
            else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialAppealCases)
            {
                CourtTypeId = (int)CourtTypeEnum.Appeal;
                cmsRegisteredCase.SectorTypeId = (int)OperatingSectorTypeEnum.CivilCommercialAppealCases;
                cmsRegisteredCase.RequestTypeId = (int)RequestTypeEnum.CivilCommercial;
            }
            else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeSupremeCases)
            {
                CourtTypeId = (int)CourtTypeEnum.Supreme;
                cmsRegisteredCase.SectorTypeId = (int)OperatingSectorTypeEnum.AdministrativeSupremeCases;
                cmsRegisteredCase.RequestTypeId = (int)RequestTypeEnum.Administrative;
            }
            else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases)
            {
                CourtTypeId = (int)CourtTypeEnum.Supreme;
                cmsRegisteredCase.SectorTypeId = (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases;
                cmsRegisteredCase.RequestTypeId = (int)RequestTypeEnum.CivilCommercial;
            }
            else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases)
            {
                cmsRegisteredCase.SectorTypeId = (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases;
                cmsRegisteredCase.RequestTypeId = (int)RequestTypeEnum.CivilCommercial;
            }
            else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.PublicOperationalSector)
            {
                cmsRegisteredCase.SectorTypeId = (int)OperatingSectorTypeEnum.PublicOperationalSector;
                registeredCase.RequestTypeId = caseRequest.RequestTypeId;
            }
        }
        protected async Task PopulateChamberdropdown()
        {
            var response = await lookupService.GetChamber();
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
        #endregion
        protected override async Task OnInitializedAsync()
        {
            if (FileId != null)
            {
                caseIdUrl = "/case-view/" + CaseId;
                var response = await cmsRegisteredCaseService.GetRegisteredCaseDetailByIdVM(Guid.Parse(CaseId));
                if (response.IsSuccessStatusCode)
                {
                    registeredCase = (CmsRegisteredCaseDetailVM)response.ResultData;
                    cmsRegisteredCase.CANNumber = registeredCase.CANNumber;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                await GetCaseRequestByFileId();
                await PopulateCourtType();
                await PopulateChamberdropdown();
                await PopulateCasedropdown();
                await PopulateCaseTypedropdown();
            }
            else
            {

            }
            spinnerService.Hide();
        }

        #endregion

        #region Validation
        protected bool ValidateBasicDetails()
        {
            bool basicDetailsValid = true;
            //if (lpsPrinciple.CategoryId == Guid.Empty)
            //{
            //    validations.Category = "k-invalid";
            //    basicDetailsValid = false;
            //}
            //else
            //{
            //    validations.Category = "k-valid";
            //}
            //if (String.IsNullOrWhiteSpace(lpsPrinciple.Title))
            //{
            //    validations.Title = "k-invalid";
            //    basicDetailsValid = false;
            //}
            //else
            //{
            //    validations.Title = "k-valid";
            //}
            //if (SelectedTagIds.Count() == 0)
            //{
            //    validations.Tags = "k-invalid";
            //    basicDetailsValid = false;
            //}
            //else
            //{
            //    validations.Tags = "k-valid";
            //}
            return basicDetailsValid;
        }

        #endregion

        #region back  Buttons
        protected async Task ButtonCancelClick(MouseEventArgs args)
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
        #endregion

    }

}
