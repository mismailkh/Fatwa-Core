using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace FATWA_WEB.Pages.CaseManagment.Files
{
    //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master">Select Primary File for Linking selected Files </History>
    public partial class SelectPrimaryFileForLinkingPopup : ComponentBase
    {

        #region Parameter

        [Parameter]
        public dynamic? Files { get; set; }
        [Parameter]
        public bool IsUnderFile { get; set; }
        public List<CmsCaseFileVM> LinkedFiles { get { if (IsUnderFile) { return (List<CmsCaseFileVM>)Files; } else { return null; } } set { Files = value; } }
        public List<RegisteredCaseFileVM> LinkedRegisteredFiles { get { if (!IsUnderFile) { return (List<RegisteredCaseFileVM>)Files; } else { return null; } } set { Files = value; } }

        #endregion

        #region Variables
        protected LinkCaseFilesVM linkCaseFile = new LinkCaseFilesVM();
        public bool allowRowSelectOnRowClick = true;
        protected string LinkedFileNos { get; set; }
        #endregion

        #region Component Load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateLinkedFileNos();
            spinnerService.Hide();
        }

        #endregion

        #region Remote Dropdown Data and Dropdown Change Events
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Linked Request Nos</History>
        protected async Task PopulateLinkedFileNos()
        {
            try
            {
                if(IsUnderFile)
                {
                    var length = LinkedFiles?.Count();
                    for (int i = 0; i < length; i++)
                    {
                        var seperator = i + 1 == length ? "" : ", ";
                        LinkedFileNos += LinkedFiles[i].FileNumber + seperator;
                    }
                }
                else
                {
                    var length = LinkedRegisteredFiles?.Count();
                    for (int i = 0; i < length; i++)
                    {
                        var seperator = i + 1 == length ? "" : ", ";
                        LinkedFileNos += LinkedRegisteredFiles[i].FileNumber + seperator;
                    }
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

        #region Redirect Function
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

        #region Button Events
        //<History Author = 'Hassan Abbas' Date='2022-11-30' Version="1.0" Branch="master"> Link Case Files</History>
        protected async Task LinkCaseFiles()
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    if(IsUnderFile)
                        linkCaseFile.LinkedFileIds = LinkedFiles.Where(f => f.FileId != linkCaseFile.PrimaryFileId).Select(f => f.FileId).ToList();
                    else
                        linkCaseFile.LinkedFileIds = LinkedRegisteredFiles.Where(f => f.FileId != linkCaseFile.PrimaryFileId).Select(f => f.FileId).ToList();

                    spinnerService.Show();
                    var response = await cmsCaseFileService.LinkCaseFiles(linkCaseFile);
                    if (response.IsSuccessStatusCode)
                    {
                        // add the Copy Attachments From Source To Destination
                        linkCaseFile = (LinkCaseFilesVM)response.ResultData;
                        var docResponse = await fileUploadService.CopyAttachmentsFromSourceToDestination(linkCaseFile.CopyAttachmentVMs);
                        await SaveTempAttachementToUploadedDocument();
                        dialogService.Close(1);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();
                }
            }
            catch (Exception ex)
            {
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        //<History Author = 'ijaz Ahmad' Date='2023-03-03' Version="1.0" Branch="master"> Link Case Files</History>
        protected async Task SaveTempAttachementToUploadedDocument()
        {
            try
            {
                List<Guid> requestIds = new List<Guid>
                {
                    linkCaseFile.LinkedFileIds.First(),
                };

                var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                {
                    RequestIds = requestIds,
                    CreatedBy = loginState.Username,
                    FilePath = _config.GetValue<string>("dms_file_path"),
                    DeletedAttachementIds = null
                });

                if (!docResponse.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Attachment_Save_Failed"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
            }
            catch (Exception)
            {
                return;
                throw;
            }
        }

        #endregion
    }
}
