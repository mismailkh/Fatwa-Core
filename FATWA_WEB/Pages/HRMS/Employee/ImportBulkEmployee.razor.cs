using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.FileSelect;

namespace FATWA_WEB.Pages.HRMS.Employee
{
    public partial class ImportBulkEmployee : ComponentBase
    {
        protected List<string> FileTypes = new List<string> { ".xlsx" };
        protected FileSelectFileInfo selectedfile;
        protected List<string> errorlist = new List<string>();
        protected string errorMessages;

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }

        protected async Task Load()
        {

        }

        protected async Task DownloadTemplate()
        {
            // Display confirm dialog
            bool? result = await dialogService.Confirm(
            translationState.Translate("Export_File"),
            translationState.Translate("Confirm"),
            new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel"),
                CloseDialogOnOverlayClick = false,
                CloseDialogOnEsc = true
            });
            if ((bool)result)
            {
                spinnerService.Show();
                var excelFileContent = await excelExportService.ExportToExcel();
                await JsInterop.InvokeVoidAsync("downloadFile", excelFileContent, "bulkImportTemplate.xlsx");
                spinnerService.Hide();
            }
            else
            {
                dialogService.Close(null);
            }

        }

        protected void HandleFileSelect(FileSelectEventArgs args)
        {
            selectedfile = args.Files[0];
        }
        protected void HandleFileRemove(FileSelectEventArgs args)
        {
            selectedfile = null;
        }
        protected async Task BulkImport()
        {
            spinnerService.Show();
            if (selectedfile != null)
            {
                var result = await excelImportService.ImportFromExcel(selectedfile.Stream);
                if(result.IsSuccessStatusCode)
                {
                    errorlist = (List<string>)result.ResultData;
                    errorMessages = string.Join("\n", errorlist);
                    if (errorlist.IsNullOrEmpty())
                    {
                        dialogService.Close(null);
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = result.IsSuccessStatusCode ? NotificationSeverity.Success : NotificationSeverity.Error,
                            Detail = result.IsSuccessStatusCode ? translationState.Translate("Import_Successful") : translationState.Translate("Import_Failed"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(result);
                }
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Select_File"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
            spinnerService.Hide();
        }
        protected void ButtonCancelClick(MouseEventArgs args)
        {
            selectedfile = null;
            dialogService.Close(null);
        }
       
    }
}
