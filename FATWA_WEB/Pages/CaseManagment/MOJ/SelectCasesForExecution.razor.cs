using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.CaseManagment.MOJ
{
    //<History Author = 'Hassan Abbas' Date='2023-03-30' Version="1.0" Branch="master"> Select Cases For Execution</History>
    public partial class SelectCasesForExecution : ComponentBase
    {

        #region Parameters
        [Parameter]
        public dynamic FileId { get; set; }

        #endregion

        #region Variables

        public List<CmsRegisteredCaseVM> executionCases = new List<CmsRegisteredCaseVM>();
        protected RadzenDataGrid<CmsRegisteredCaseVM>? grid;
        protected MojExecutionRequest ExecutionRequest { get; set; } = new MojExecutionRequest();
        public bool allowRowSelectOnRowClick = true;

        #endregion

        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateExecutionCases();
            spinnerService.Hide();
        }

        #endregion

        #region Grid Events
        //<History Author = 'Hassan Abbas' Date='2022-11-30' Version="1.0" Branch="master"> Execution Cases in selected Files</History>
        protected async Task PopulateExecutionCases()
        {
            try
            {
                var response = await cmsCaseFileService.GetExecutionCasesByFileId(Guid.Parse(FileId));
                if (response.IsSuccessStatusCode)
                {
                    executionCases = new List<CmsRegisteredCaseVM>(response.ResultData);
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
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        #endregion

        #region Button Evetns

        //<History Author = 'Hassan Abbas' Date='2022-11-30' Version="1.0" Branch="master"> Close Dialog</History>
        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-30' Version="1.0" Branch="master"> Merge Cases</History>
        protected async Task SendToMOJ(MouseEventArgs args)
        {
            if(ExecutionRequest.SelectedCases.Any())
            {

                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    ExecutionRequest.CreatedBy = loginState.Username;
                    ExecutionRequest.CreatedDate = DateTime.Now;
                    var execResponse = await cmsCaseFileService.CreateMojExecutionRequest(ExecutionRequest);
                    if (execResponse.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Sent_Execution_To_Moj_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(execResponse);
                    }
                    spinnerService.Hide();
                }
            }
        }

        #endregion

    }
}
