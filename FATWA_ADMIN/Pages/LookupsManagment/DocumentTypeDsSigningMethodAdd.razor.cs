using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class DocumentTypeDsSigningMethodAdd : ComponentBase
    {
        #region Parameter
        [Parameter]
        public List<AttachmentTypeVM> AttachmentTypeList { get; set; } = new List<AttachmentTypeVM>();
        #endregion

        #region Variables
        protected RadzenDataGrid<AttachmentTypeVM>? AttachmentTypeGrid = new RadzenDataGrid<AttachmentTypeVM>();
        protected AttachmentType documenttypelist = new AttachmentType();
        protected IEnumerable<EpDesignationVM> Designations { get; set; } = new List<EpDesignationVM>();
        protected IEnumerable<DsSigningMethods> SigningMethods { get; set; } = new List<DsSigningMethods>();
        public int AttachmentId { get; set; }
        protected bool HasDesValue = false;
        protected bool HasSignMethodsValue = false;
        #endregion

        #region OnInitilized
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await GetDesignations();
            await GetSigningMethods();
            spinnerService.Hide();
        }
        #endregion

        #region Submit
        protected async Task Submit()
        {
            try
            {
                HasDesValue = !(documenttypelist.DesignationIds?.Any() ?? false);
                HasSignMethodsValue = !(documenttypelist.SigningMethodIds?.Any() ?? false);
                if (HasDesValue || HasSignMethodsValue) return;
                if (await dialogService.Confirm(
                                    translationState.Translate("Sure_Submit"),
                                    translationState.Translate("Confirm"),
                                    new ConfirmOptions()
                                    {
                                        OkButtonText = translationState.Translate("OK"),
                                        CancelButtonText = translationState.Translate("Cancel")
                                    }) == true)
                {
                    spinnerService.Show();
                    documenttypelist.DesignationIds ??= new List<int>();
                    documenttypelist.AttachmentTypeIds = AttachmentTypeList.Select(x => x.AttachmentTypeId).ToList();
                    var res = await lookupService.UpdateDocumentType(documenttypelist);
                    if (res.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Document_Type_Signing_Method_Added_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(res);
                    }
                }
                dialogService.Close(true);
                StateHasChanged();
                spinnerService.Hide();
            }

            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                spinnerService.Hide();
            }
        }
        #endregion

        #region Functions
        private void ChangeSigningMethod(bool isChecked, int methodId)
        {
            if (isChecked)
            {
                if (!documenttypelist.SigningMethodIds.Contains(methodId))
                {
                    documenttypelist.SigningMethodIds.Add(methodId);
                }
            }
            else
            {
                if (documenttypelist.SigningMethodIds.Contains(methodId))
                {
                    documenttypelist.SigningMethodIds.Remove(methodId);
                }
            }
        }


        protected async Task CancelClick()
        {
            dialogService.Close();
        }
        protected async Task GetDesignations()
        {
            var result = await lookupService.GetEpDesignationList();
            if (result.IsSuccessStatusCode)
            {
                Designations = (IEnumerable<EpDesignationVM>)result.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
            await InvokeAsync(StateHasChanged);
        }
        protected async Task GetSigningMethods()
        {
            var result = await lookupService.GetSigningMethodsList();
            if (result.IsSuccessStatusCode)
            {
                SigningMethods = (IEnumerable<DsSigningMethods>)result.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
            await InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
