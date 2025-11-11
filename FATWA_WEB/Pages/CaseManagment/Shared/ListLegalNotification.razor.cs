using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;

namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    public partial class ListLegalNotification : ComponentBase
    {
        [Parameter]
        public Guid ReferenceId { get; set; }
        [Parameter]
        public dynamic SubModuleId { get; set; }

        #region Variables
        public int SubModuleid { get { return Convert.ToInt32(SubModuleId); } set { SubModuleId = value; } }
        protected RadzenDataGrid<CommunicationListVM>? communicationListgrid = new RadzenDataGrid<CommunicationListVM>();
        public IEnumerable<CommunicationListVM> FiletergetCommunicationList = new List<CommunicationListVM>();
        protected IEnumerable<CommunicationListVM> getCommunicationList { get; set; }
        protected RadzenDataGrid<CommunicationListVM> grid = new RadzenDataGrid<CommunicationListVM>();
        public int count { get; set; } = 0;
        #endregion
       
        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            translationState.TranslateGridFilterLabels(communicationListgrid);
            spinnerService.Hide();
        }
        #endregion

        #region Grid Events

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected async Task Load()
        {
            try
            {
                if (SubModuleid == (int)SubModuleEnum.CaseFile)
                {
                    await GetCommunicationListByCaseFileId();
                }
                else if (SubModuleid == (int)SubModuleEnum.RegisteredCase)
                {
                    await GetCommunicationListByRegisteredCaseId();
                }
                else if (SubModuleid == (int)SubModuleEnum.ConsultationFile)
                {
                    await PopulateConslutationFileCommunicationGrid();
                }

                await communicationListgrid.Reload();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #region view response
        protected async Task ViewResponse(CommunicationListVM item)
        {
            var RedirectURL = "/request-need-more-detail/" + item.ReferenceId + "/" + item.CommunicationId + "/" + item.SubModuleId + "/" + item.CommunicationTypeId;
            navigationManager.NavigateTo(RedirectURL);
        }
        #endregion
        #endregion
       
        protected async Task GetCommunicationListByCaseFileId()
        {
            var response = await communicationService.GetCommunicationListByCaseFileId(ReferenceId, (int)CommunicationCorrespondenceTypeEnum.Outbox);
            if (response.IsSuccessStatusCode)
            {
                getCommunicationList = (IEnumerable<CommunicationListVM>)response.ResultData;
                FiletergetCommunicationList = (IEnumerable<CommunicationListVM>)response.ResultData;
                count = getCommunicationList.Count();
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        protected async Task GetCommunicationListByRegisteredCaseId()
        {
            try
            {
                var response = await communicationService.GetCommunicationListByCaseId(ReferenceId, (int)CommunicationCorrespondenceTypeEnum.Outbox);
                if (response.IsSuccessStatusCode)
                {

                    getCommunicationList = (IEnumerable<CommunicationListVM>)response.ResultData;
                    FiletergetCommunicationList = (List<CommunicationListVM>)response.ResultData;
                    count = getCommunicationList.Count();

                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected async Task PopulateConslutationFileCommunicationGrid()
        {
            var response = await communicationService.GetConslutationFileCommunication(ReferenceId, (int)CommunicationCorrespondenceTypeEnum.Outbox);
            if (response.IsSuccessStatusCode)
            {

                getCommunicationList = (IEnumerable<CommunicationListVM>)response.ResultData;
                FiletergetCommunicationList = (List<CommunicationListVM>)response.ResultData;
                count = getCommunicationList.Count();

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
    }
}
