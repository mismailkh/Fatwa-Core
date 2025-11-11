using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.WorkflowModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_DOMAIN.Enums.DmsEnums;
using static FATWA_GENERAL.Helper.Response;


namespace FATWA_ADMIN.Pages.LookupsManagment
{

    public partial class DocumentTypeAdd : ComponentBase
    {


        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        [Parameter]
        public dynamic AttachmentTypeId { get; set; }
        #region Variable
        public List<Module> GetModule { get; set; } = new List<Module>();
        public List<Subtype> GetSubtype { get; set; } = new List<Subtype>();
        protected IEnumerable<EpDesignationVM> Designations { get; set; }
        protected IEnumerable<DsSigningMethods> SigningMethods { get; set; }= new List<DsSigningMethods>();
        public bool Disable;
        bool method1Selected { get; set; }
        bool method2Selected { get; set; }
        bool method3Selected { get; set; }
        #endregion
        AttachmentType _documenttypelist;
        protected AttachmentType documenttypelist
        {
            get
            {
                return _documenttypelist;
            }
            set
            {
                if (!object.Equals(_documenttypelist, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "documenttypelist", NewValue = value, OldValue = _documenttypelist };
                    _documenttypelist = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            method1Selected = documenttypelist.SigningMethodIds.Contains((int)SigningMethodEnum.LocalSigning);
            method2Selected = documenttypelist.SigningMethodIds.Contains((int)SigningMethodEnum.RemoteSigning);
            method3Selected = documenttypelist.SigningMethodIds.Contains((int)SigningMethodEnum.ExternalSigning);
            await GetModuleId();
            await GetSubTypeId();
            await GetDesignations();
            await GetSigningMethods();
            spinnerService.Hide();
        }
        #region populate  Module Id 
        protected async Task GetModuleId()
        {

            var response = await lookupService.GetModuleId();
            if (response.IsSuccessStatusCode)
            {
                GetModule = (List<Module>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();

        }
        #endregion

        #region populate Subtype Id
        protected async Task GetSubTypeId()
        {

            var response = await lookupService.GetSubTypeId();
            if (response.IsSuccessStatusCode)
            {
                GetSubtype = (List<Subtype>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();

        }
        #endregion
        #region Populate Designation List

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

        ApiCallResponse response = new ApiCallResponse();
        protected async Task Load()
        {
            if (AttachmentTypeId == null)
            {
                Disable = false;
                spinnerService.Show();
                documenttypelist = new AttachmentType() { };
                //int AttachmentTypeId = GetNextUniqueId();
                spinnerService.Hide();
            }
            else
            {
                Disable = true;
                spinnerService.Show();
                response = await lookupService.GetDocumentTypeById(AttachmentTypeId);
                if (response.IsSuccessStatusCode)
                {
                    documenttypelist = (AttachmentType)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }

        }
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
        protected async Task SaveChanges(AttachmentType args)
        {
            try
            {
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
                    documenttypelist.SigningMethodIds ??= new List<int>();
                    documenttypelist.DesignationIds ??= new List<int>();
                    if (AttachmentTypeId == null)
                    {
                        var fatwaDbCreatelegalPrincipleTagResult = await lookupService.SaveDocumentType(documenttypelist);
                        if (fatwaDbCreatelegalPrincipleTagResult.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Document_type_Added_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                    }
                    else
                    {
                        var fatwaDbUpdateLmsLiteratureTypeResult = await lookupService.UpdateDocumentType(documenttypelist);
                        if (fatwaDbUpdateLmsLiteratureTypeResult.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Document_type_Updated_Successfully"),
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
                    Detail = AttachmentTypeId == null ? translationState.Translate("Could_not_create_a_new_Document_Type") : translationState.Translate("Document_type_could_not_be_updated"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                spinnerService.Hide();
            }
        }
        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(false);
        }
    }
}
