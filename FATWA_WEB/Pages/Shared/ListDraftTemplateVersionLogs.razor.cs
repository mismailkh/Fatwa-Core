using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_WEB.Pages.Shared
{
    public partial class ListDraftTemplateVersionLogs : ComponentBase
    {
        #region Paramater
        [Parameter]
        public string versionId { get; set; }
        #endregion
        #region Variable Declaration
        public IEnumerable<CmsDraftTemplateVersionLogVM> cmsDraftTemplateVersionLogs { get; set; } = new List<CmsDraftTemplateVersionLogVM>();
        #endregion
        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }
        protected async Task Load()
        {
            await GetCmsDraftTemplateVersionLogsList();
        }
        #endregion
        #region Populate Data Grids
        protected async Task GetCmsDraftTemplateVersionLogsList()
        {
            var response = await cmsCaseTemplateService.GetCmsDraftTemplateVersionLogsList(Guid.Parse(versionId));
            if (response.IsSuccessStatusCode)
            {
                cmsDraftTemplateVersionLogs = (List<CmsDraftTemplateVersionLogVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion
        #region Dialog Close Button
        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        #endregion
        public string GetActionTypeName(DraftActionIdEnum actionTypeId)
        {
            var displayName = typeof(DraftActionIdEnum).GetMember(actionTypeId.ToString()).First().GetCustomAttribute<DisplayAttribute>().GetName();
            return displayName;
        }
    }
}
