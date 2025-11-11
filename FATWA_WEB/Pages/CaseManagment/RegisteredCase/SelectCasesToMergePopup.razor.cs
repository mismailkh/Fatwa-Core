using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    public partial class SelectCasesToMergePopup : ComponentBase
    {
        #region Parameters
        [Parameter]
        public dynamic Files { get; set; }
        public List<RegisteredCaseFileVM> CaseFiles { get { return (List<RegisteredCaseFileVM>)Files; } set { Files = value; } }

        #endregion

        #region Variables

        public List<CmsRegisteredCaseVM> mergedCases = new List<CmsRegisteredCaseVM>();
        protected RadzenDataGrid<CmsRegisteredCaseVM>? grid;
        public bool allowRowSelectOnRowClick = true;
        public IList<CmsRegisteredCaseVM> selectedCases;

        #endregion

        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await ConcatRegisteredCases();
            translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
        }

        #endregion

        #region Grid Events
        //<History Author = 'Hassan Abbas' Date='2022-11-30' Version="1.0" Branch="master"> Concatinate Cases in selected Files</History>
        protected async Task ConcatRegisteredCases()
        {
            try
            {
                foreach(var file in CaseFiles)
                {
                    var response = await cmsCaseFileService.GetAllRegisteredCasesByFileId(file.FileId);
                    if (response.IsSuccessStatusCode)
                    {
                        mergedCases = new List<CmsRegisteredCaseVM>(mergedCases?.Concat((List<CmsRegisteredCaseVM>)response.ResultData).ToList());
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
            }
            catch(Exception ex)
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
        protected async Task MergeCases(MouseEventArgs args)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                spinnerService.Show();
                dataCommunicationService.registeredCases = selectedCases.ToList();
                navigationState.ReturnUrl = "case-files";
                navigationManager.NavigateTo("/merge-cases");
                spinnerService.Hide();
            }
        }

        #endregion
    }
}
