using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    public partial class SaveCloseCaseFile : ComponentBase
    {

        #region Parameter
        [Parameter]
        public dynamic CaseId { get; set; }

        #endregion

        #region Variables
        protected string RequesttypevalidationnMsg = "";
        protected string typeValidationMsg = "";
        protected string governmentEntitiesValidationMsg = "";
        public bool IsVisible { get; set; }
        protected CmsRegisteredCaseDetailVM cmsRegisteredCaseDetailVM { get; set; } = new CmsRegisteredCaseDetailVM();
        public CmsSaveCloseCaseFile? cmsSaveCloseCaseFiles = new CmsSaveCloseCaseFile();
        protected List<GovernmentEntity> governmentEntities { get; set; }
        protected List<Frequency> frequency { get; set; } = new List<Frequency>();
        protected List<Priority> Priorities { get; set; } = new List<Priority>();
        protected List<ResponseType> ResponseType { get; set; }
        public int saveAndCloseCaseFileType { get;set; }

        protected Validations validations { get; set; } = new Validations();

        #endregion

        #region OnInitialized
        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                saveAndCloseCaseFileType = (int)SaveAndCloseCaseFileEnum.SaveAndCloseCaseFile;
                var response = await cmsRegisteredCaseService.GetRegisteredCaseDetailByIdVM(Guid.Parse(CaseId));
                if (response.IsSuccessStatusCode)
                {
                    cmsRegisteredCaseDetailVM = (CmsRegisteredCaseDetailVM)response.ResultData;
                    await PopulateFrequency();
                    await PopulatePriorities();
                    await PopulateGovernmentEntities();
                    await PopulateResponseTypes();
                    spinnerService.Hide();
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
                    Summary = translationState.Translate("Error"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }
        #endregion



        #region Form Submit
        protected async Task FormSubmit(CmsSaveCloseCaseFile saveCloseCaseFile)
        {
            try
            {



                
                governmentEntitiesValidationMsg = "";
                bool res = ValidateBasicDetailsOnChange();

                if (res != false)
                {


                    if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    }) == true)
                    {
                        spinnerService.Show();

                        saveCloseCaseFile.CaseId = Guid.Parse(CaseId);

                        var response = await cmsRegisteredCaseService.SaveAndCloseCaseFiles(saveCloseCaseFile);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Save_And_Close_Files_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });

                            navigationManager.NavigateTo(navigationState.ReturnUrl);
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                        spinnerService.Hide();
                    }
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
        #endregion
        #region Populate Dropdown

        protected async Task PopulateFrequency()
        {
            var response = await lookupService.GetFrequency();
            if (response.IsSuccessStatusCode)
            {
                frequency = (List<Frequency>)response.ResultData;


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
        protected async Task PopulateGovernmentEntities()
        {
            var response = await lookupService.GetGovernmentEntities();
            if (response.IsSuccessStatusCode)
            {
                governmentEntities = (List<GovernmentEntity>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        protected async Task PopulateResponseTypes()
        {
            var response = await lookupService.GetResponseTypes();
            if (response.IsSuccessStatusCode)
            {

                cmsSaveCloseCaseFiles.ResponseTypeId = (int)SaveAndCloseCaseFileEnum.SaveAndCloseCaseFile;
                ResponseType = (List<ResponseType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion

        #region Redirect Function
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo(navigationState.ReturnUrl);
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

        #region Validation
        protected class Validations
        {
            public string reason { get; set; } = string.Empty;
            public string types { get; set; } = string.Empty;

            public string govEntities { get; set; } = string.Empty;

        }
        protected bool ValidateBasicDetailsOnChange()
        {
            bool basicDetailsValid = true;
            if (cmsSaveCloseCaseFiles.ResponseTypeId == null)
            {
                typeValidationMsg = @translationState.Translate("Required_Field");
                validations.types = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.types = "k-valid";
            }
            if (cmsSaveCloseCaseFiles.EntityId == null)
            {
                governmentEntitiesValidationMsg = translationState.Translate("Required_Field");
                validations.govEntities = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.govEntities = "k-valid";
            }

            if (basicDetailsValid != true)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Fill_Required_Fields"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });


            }

            return basicDetailsValid;
        }
        #endregion

    }
}
