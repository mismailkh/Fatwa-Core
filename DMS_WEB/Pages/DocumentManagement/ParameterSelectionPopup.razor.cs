using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.WorkflowModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;

namespace DMS_WEB.Pages.DocumentManagement
{
    //< History Author = 'Hassan Abbas' Date = '2023-08-16' Version = "1.0" Branch = "master" >Select Parameter for Template Key</History>
    public partial class ParameterSelectionPopup : ComponentBase
    {
        #region Parameters

        [Parameter]
        public dynamic? ModuleId { get; set; }
        public int ParameterModuleId { get { return Convert.ToInt32(ModuleId); } set { ModuleId = value; } }
        #endregion

        #region Variables 
        CaseTemplateParameter selectedParamter = new CaseTemplateParameter();
        protected List<CaseTemplateParameter> Parameters { get; set; } = new List<CaseTemplateParameter>();

        #endregion

        #region Component Load
        //< History Author = 'Hassan Abbas' Date = '2022-09-29' Version = "1.0" Branch = "master" >Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                await PopulateTemplateParameters();
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

        //<History Author = 'Hassan Abbas' Date='2023-08-16' Version="1.0" Branch="master">Populate Template Parameters</History>
        protected async Task PopulateTemplateParameters()
        {
            var response = await fileUploadService.GetTemplateParameters(ParameterModuleId);
            if (response.IsSuccessStatusCode)
            {
                Parameters = (List<CaseTemplateParameter>)response.ResultData;
            }
            else
            {
                await ReturnBadRequestNotification(response);
            }
        }

        #endregion
        #region Dialog Events
        //< History Author = 'Hassan Abbas' Date = '2023-08-16' Version = "1.0" Branch = "master" >Submit Selection</History>
        protected async Task SubmitSelection()
        {
            try
            {
                dialogService.Close(Parameters.Where(p => p.ParameterId == selectedParamter.ParameterId).FirstOrDefault());
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

        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }

        #endregion

        #region Badrequest Notification

        //<History Author = 'Hassan Abbas' Date='2022-07-22' Version="1.0" Branch="master"> Handle bad request and display error messages in whole class</History>
        protected async Task ReturnBadRequestNotification(ApiCallResponse response)
        {
            try
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Token_Expired"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await Task.Delay(5000);
                    await BrowserStorage.RemoveItemAsync("User");
                    await BrowserStorage.RemoveItemAsync("Token");
                    await BrowserStorage.RemoveItemAsync("RefreshToken");
                    await BrowserStorage.RemoveItemAsync("UserDetail");
                    await BrowserStorage.RemoveItemAsync("SecurityStamp");
                    loginState.IsLoggedIn = false;
                    loginState.IsStateChecked = true;
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
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
    }
}
