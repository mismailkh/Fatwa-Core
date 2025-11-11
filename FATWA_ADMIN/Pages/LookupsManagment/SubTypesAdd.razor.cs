using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class SubTypesAdd : ComponentBase
    {
        #region Parameters
        [Parameter]
        public RequestTypeVM SelectedRequestType { get; set; }
        [Parameter]
        public dynamic Id { get; set; }

        #endregion

        #region Variable and Objects Initilizations
        string ExistingNameEn = "";
        string ExistingNameAr = "";
        public bool IsNameArModified { get; set; } = false;
        public List<Subtype> subTypes { get; set; } = new List<Subtype>();
        public List<CourtType> courtTypes { get; set; } = new List<CourtType>();

        SubTypeVM subTypeVM = new SubTypeVM();

        Subtype Subtype = new Subtype();
        RequestType RequestType = new RequestType();
        ApiCallResponse response = new ApiCallResponse();

        protected string RequestTypeIdString
        {
            get => RequestType?.Id.ToString();
            set
            {
                if (RequestType != null && int.TryParse(value, out int id))
                {
                    RequestType.Id = id;
                }
            }
        }
        #endregion

        #region Functions

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        #endregion

        #region OnLoad
        protected override async Task OnInitializedAsync()
        {
            await GetSubTypes();
            await Load();
        }
        protected async Task Load()
        {

            if (Id == null)
            {
                spinnerService.Show();

                if (SelectedRequestType != null)
                {
                    Subtype.RequestTypeId = SelectedRequestType.Id;
                    Subtype.Code = SelectedRequestType.Code;
                }
                spinnerService.Hide();
            }
            else
            {
            }

        }
        #endregion

        #region populate Court TypeID 
        protected async Task GetSubTypes()
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
        #endregion

        #region Form Submit
        protected async Task SaveChanges(Subtype args)
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
                    if (Id == null)
                    {
                        var checkExistanceofNameEnAndNameAr = await ExistsSubTypeNames(args);
                        if (checkExistanceofNameEnAndNameAr)
                        {
                            spinnerService.Hide();
                            return;
                        }

                        var G2GDbCreateCourtResult = await lookupService.SaveSubTypes(Subtype);
                        if (G2GDbCreateCourtResult.IsSuccessStatusCode)
                        {

                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Sub_Type_Added_Successfully"),
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
                    Detail = Id == null ? translationState.Translate("Could_not_create_a_sub_type") : translationState.Translate("Could_not_update_a_sub_type"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                spinnerService.Hide();
            }
        }
        #endregion

        #region  Check Existanse of Name EN and Name AR for SubTypes
        bool isNameEnExist = false;
        bool isNameArExist = false;
        protected async Task<bool> ExistsSubTypeNames(Subtype subtypes)
        {
            if (ExistingNameEn != subTypeVM.Name_En && ExistingNameAr != subTypeVM.Name_Ar)
            {
                isNameEnExist = await lookupService.CheckNameEnExists(subtypes.Name_En, subtypes.RequestTypeId, subtypes.Id);
                isNameArExist = await lookupService.CheckNameArExists(subtypes.Name_Ar, subtypes.RequestTypeId, subtypes.Id);

                string errorMessage = string.Empty;

                if (isNameEnExist && isNameArExist)
                {
                    errorMessage = translationState.Translate("Both_English_And_Arabic_Names_Already_Exist");
                }
                else if (isNameEnExist)
                {
                    errorMessage = translationState.Translate("English_Name_Already_Exist");
                }
                else if (isNameArExist)
                {
                    errorMessage = translationState.Translate("Arabic_Name_Already_Exist");
                }

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = errorMessage,
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });

                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Cancel click
        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(false);
        }
        #endregion
    }
}
