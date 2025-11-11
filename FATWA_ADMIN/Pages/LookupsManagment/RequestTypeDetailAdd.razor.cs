using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class RequestTypeDetailAdd : ComponentBase
    {
        #region Functions
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        #endregion

        #region Paramter
        [Parameter]
        public dynamic Id { get; set; }
        #endregion

        #region Variables
        ApiCallResponse response = new ApiCallResponse();

        public List<RequestType> RequestType { get; set; } = new List<RequestType>();

        RequestType _CmsChamberG2GLKP;
        protected RequestType CmsChamberG2GLKP
        {
            get
            {
                return _CmsChamberG2GLKP;
            }
            set
            {
                if (!object.Equals(_CmsChamberG2GLKP, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "CmsChamberG2GLKP", NewValue = value, OldValue = _CmsChamberG2GLKP };
                    _CmsChamberG2GLKP = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        #endregion

        #region On Load
        protected override async Task OnInitializedAsync()
        {
            await GetRequestType();
            await Load();
        }
        protected async Task Load()
        {
            if (Id == null)
            {
                CmsChamberG2GLKP = new RequestType() { };
            }
            else
            {
                spinnerService.Show();
                response = await lookupService.GetRequestTypeById(Id);
                if (response.IsSuccessStatusCode)
                {
                    CmsChamberG2GLKP = (RequestType)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }
        }
        #endregion

        #region populate Court Name 
        protected async Task GetRequestType()
        {
            var response = await lookupService.GetRequestTypes();
            if (response.IsSuccessStatusCode)
            {
                RequestType = (List<RequestType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

            StateHasChanged();

        }
        #endregion

        #region Form Submit
        protected async Task SaveChanges(RequestType args)
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                                    translationState.Translate("Sure_Submit"),
                                    translationState.Translate("Confirm"),
                                    new ConfirmOptions()
                                    {
                                        OkButtonText = translationState.Translate("OK"),
                                        CancelButtonText = translationState.Translate("Cancel")
                                    });
                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    if (Id != null)
                    {
                        var G2GDbUpdateLmsLiteratureTypeResult = await lookupService.UpdateRequestType(CmsChamberG2GLKP);
                        if (G2GDbUpdateLmsLiteratureTypeResult.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Request_Updated_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });

                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                    }

                    dialogService.Close(true);
                    StateHasChanged();
                    spinnerService.Hide();
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = Id == null ? translationState.Translate("Could_not_Create_a_new_Request") : translationState.Translate("Request_could_not_be_updated"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                spinnerService.Hide();
            }
        }

        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(false);
        }
        #endregion
    }
}
