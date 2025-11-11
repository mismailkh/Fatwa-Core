using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    public partial class CreateRequestForDocument : ComponentBase
    {

        #region Parameter
        [Parameter]
        public Guid CaseId { get; set; }
        #endregion

        #region Variable
        protected List<CmsRegisteredCaseVM> cmsRegisteredCases = new List<CmsRegisteredCaseVM>();
        protected CmsRegisteredCaseVM cmsRegisteredCase = new CmsRegisteredCaseVM();
        protected CmsRegisteredCaseDetailVM cmsRegisteredcasesDetail = new CmsRegisteredCaseDetailVM() { CaseDate = null };
        protected List<CmsRegisteredCaseDetailVM> cmsRegisteredcasesDetails = new List<CmsRegisteredCaseDetailVM>();
        protected List<AttachmentType> AttachmentTypes { get; set; } = new List<AttachmentType>();
        protected TempAttachementVM TempAttachement { get; set; } = new TempAttachementVM();
        protected MojRequestForDocument requestForDocument = new MojRequestForDocument() { Id = Guid.NewGuid(), HearingDate = DateTime.Now, AttachmentTypeId = (int)AttachmentTypeEnum.DocumentPortfolio };
        protected string typeValidationMsg = "";
        protected string dateValidationMsg = "";

        #endregion

        #region Component Load
        //<History Author = 'Danish' Date='2022-12-02' Version="1.0" Branch="master"> Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            await PopulateAttachmentTypes();
            await PopulateCases();

        }
        #endregion

        #region Submit Button
        //<History Author = 'Danish' Date='2022-12-02' Version="1.0" Branch="master"> Populate Subtypes data </History>
        protected async Task FormSubmit(MojRequestForDocument item)
        {
            try
            {
                if (requestForDocument.HearingDate != default(DateTime))
                {
                    bool? dialogResponse = await dialogService.Confirm(
                      translationState.Translate("Sure_Submit"),
                      translationState.Translate("Confirm"),
                      new ConfirmOptions()
                      {
                          OkButtonText = @translationState.Translate("OK"),
                          CancelButtonText = @translationState.Translate("Cancel")
                      });
                    if (dialogResponse == true)
                    {
                        requestForDocument.CaseId = CaseId;
                        requestForDocument.SectorTypeId = loginState.UserDetail.SectorTypeId != null ? (int)loginState.UserDetail.SectorTypeId : 0;
                        var response = await cmsRegisteredCaseService.CreateRequestForDocument(requestForDocument);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Document_Portfolio_Request_Sent"),
                                Style = "position: fixed !important; left: 0; margin: auto;"
                            });
                            dialogService.Close(requestForDocument);
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                    }
                }
                else
                {
                    dateValidationMsg = requestForDocument.HearingDate == default(DateTime) ? translationState.Translate("Required_Field") : "";
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

        #endregion

        #region Cancel
        //< History Author = 'Danish' Date = '2022-12-09' Version = "1.0" Branch = "master" >ButtonCloseDialog</History>
        protected Task ButtonCloseDialog(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/case-view/" + CaseId);
            return Task.CompletedTask;
        }
        #endregion

        #region Remote Dropdown Data and Dropdown Change Events
        //<History Author = 'Danish' Date='2022-12-02' Version="1.0" Branch="master"> Populate Subtypes data </History>
        protected async Task PopulateCases()
        {
            var response = await cmsRegisteredCaseService.GetAllRegisteredCases();
            if (response.IsSuccessStatusCode)
            {
                cmsRegisteredCases = (List<CmsRegisteredCaseVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        //< History Author = 'Danish' Date = '2022-12-09' Version = "1.0" Branch = "master" >PopulateAttachmentTypes</History>
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
        //< History Author = 'Danish' Date = '2022-12-09' Version = "1.0" Branch = "master" >OnChange Event Request For  Document</History>
        protected async Task OnChange(object args)
        {
            try
            {
                if ((Guid)args != Guid.Empty)
                {
                    var response = await cmsRegisteredCaseService.GetRegisteredCaseDetailByIdVM(CaseId);
                    if (response.IsSuccessStatusCode)
                    {
                        cmsRegisteredcasesDetail = (CmsRegisteredCaseDetailVM)response.ResultData;

                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    StateHasChanged();
                }

            }
            catch (Exception ex)
            {

            }
        }
        #endregion

    }
}
