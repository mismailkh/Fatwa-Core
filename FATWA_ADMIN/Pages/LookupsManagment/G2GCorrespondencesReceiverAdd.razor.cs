using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class G2GCorrespondencesReceiverAdd : ComponentBase
    {
        #region Paramter
        [Parameter]
        public dynamic Id { get; set; }
        #endregion

        #region Variable   
        ApiCallResponse response = new ApiCallResponse();

        public List<GEDepartments> allDepartmentSelectedbyEntityId { get; set; } = new List<GEDepartments>();
        public IEnumerable<int> SelectedGovernmentEntites { get; set; } = null;
        public List<CmsSectorTypeGEDepartment> CmsSectorTypeGEDepartments { get; set; } = new List<CmsSectorTypeGEDepartment>();
        protected List<FatwaSectorTypeEnumTemp> CmsFatwaSectorTypeOptions { get; set; } = new List<FatwaSectorTypeEnumTemp>();
        public List<GovernmentEntity> GovernmentEntitiesName { get; set; } = new List<GovernmentEntity>();
        public class FatwaSectorTypeEnumTemp
        {
            public int FatwaSectorTypeEnumValue { get; set; }
            public string FatwaSectorTypeEnumName { get; set; }
        }
        protected CmsSectorTypeGEDepartment cmsSectorTypeGEDepartments;
        public bool SelectedDepartmentsValues { get; set; }
        #endregion

        #region On Load

        protected override async Task OnInitializedAsync()
        {
            foreach (FatwaSectorTypeEnum item in Enum.GetValues(typeof(FatwaSectorTypeEnum)))
            {
                CmsFatwaSectorTypeOptions.Add(new FatwaSectorTypeEnumTemp { FatwaSectorTypeEnumName = translationState.Translate(item.ToString()), FatwaSectorTypeEnumValue = (int)item });
            }
            await GetGovernmentEntities();
            await Load();
        }
        protected async Task Load()
        {
            spinnerService.Show();
            cmsSectorTypeGEDepartments = new CmsSectorTypeGEDepartment() { };
            spinnerService.Hide();
        }
        #endregion

        #region populate Government Entities  Name 
        protected async Task GetGovernmentEntities()
        {

            var response = await lookupService.GetGovernmentEntities();
            if (response.IsSuccessStatusCode)
            {
                GovernmentEntitiesName = (List<GovernmentEntity>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

            StateHasChanged();

        }
        #endregion

        #region Dropdown Change Events   
        protected async void OnChangeGovernmentEntities()
        {
            if (SelectedGovernmentEntites == null || !SelectedGovernmentEntites.Any())
            {
                allDepartmentSelectedbyEntityId.Clear();
                cmsSectorTypeGEDepartments.SelectedDepartments = null;
            }
            else
            {
                var response = await lookupService.GetDepartmentByGEEntityId(SelectedGovernmentEntites.ToList());
                if (response.IsSuccessStatusCode)
                {
                    foreach (var department in (List<GEDepartments>)response.ResultData)
                    {
                        allDepartmentSelectedbyEntityId.Add(department);
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
        }

        #endregion

        #region Form Submit

        protected async Task SaveChanges(CmsSectorTypeGEDepartment args)
        {
            try
            {
                if (cmsSectorTypeGEDepartments.SelectedDepartments == null || !cmsSectorTypeGEDepartments.SelectedDepartments.Any())
                {
                    SelectedDepartmentsValues = true;
                    return;
                }
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
                        var response = await lookupService.SaveG2GCorrespondencesReceiver(cmsSectorTypeGEDepartments);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Changes_saved_successfully"),
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
                    Detail = Id == null ? translationState.Translate("Could_not_create_a_new_G2G_Correspondences_Receiver") : translationState.Translate("G2G_Correspondences_Receiver_could_not_be_updated"),
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

        #region
        public async Task CheckSelectedValues(object args)
        {
            if (cmsSectorTypeGEDepartments.SelectedDepartments == null || !cmsSectorTypeGEDepartments.SelectedDepartments.Any())
            {
                SelectedDepartmentsValues = true;
            }
            else
            {
                SelectedDepartmentsValues = false;
            }
        }
        
        #endregion
    }
}
