using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Merge Cases</History>
    public partial class MergeCases : ComponentBase
    {
        #region Variables

        MergeRequest mergeRequest = new MergeRequest { Id = Guid.NewGuid(), IsMergeTypeCase = true };
        protected Guid PrimaryCaseId { get; set; }
        protected string Reason { get; set; }
        protected string MergedCaseNos { get; set; }

        protected string primaryValidationMsg = "";
        protected string reasonValidationMsg = "";

        #endregion

        #region Component Load

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateMergedCaseNos();
            mergeRequest.RegisteredCases = dataCommunicationService.registeredCases;
            spinnerService.Hide();
        }

        #endregion

        #region Data Population Events

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Concat Merged CANs</History>
        protected async Task PopulateMergedCaseNos()
        {
            try
            {
                var length = dataCommunicationService.registeredCases?.Count();
                for(int i = 0; i < length; i++)
                {
                    var seperator = i + 1 == length ? "" : ", ";
                    MergedCaseNos += dataCommunicationService.registeredCases[i].CaseNumber + seperator;
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

        #region Button Events

        ////< History Author = 'Hassan Abbas' Date = '2022-11-20' Version = "1.0" Branch = "master" >Submit Form</History>
        //protected async Task Form0Submit(MergeRequest args)
        //{
        //    try
        //    {
        //        if(mergeRequest.PrimaryId == Guid.Empty || String.IsNullOrEmpty(mergeRequest.Reason))
        //        {
        //            primaryValidationMsg = mergeRequest.PrimaryId == Guid.Empty ? translationState.Translate("Required_Field") : "";
        //            reasonValidationMsg = String.IsNullOrEmpty(mergeRequest.Reason) ? translationState.Translate("Required_Field") : "";
        //            return;
        //        }
        //        if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
        //        {
        //            OkButtonText = translationState.Translate("OK"),
        //            CancelButtonText = translationState.Translate("Cancel")
        //        }) == true)
        //        {
        //            spinnerService.Show();
        //            var response = await cmsRegisteredCaseService.CreateMergeRequest(mergeRequest);
        //            spinnerService.Hide();
        //            if (response.IsSuccessStatusCode)
        //            {
        //                notificationService.Notify(new NotificationMessage()
        //                {
        //                    Severity = NotificationSeverity.Success,
        //                    Detail = translationState.Translate("Merge_Request_Submitted"),
        //                    Style = "position: fixed !important; left: 0; margin: auto; "
        //                });
        //                await Task.Delay(1500);
        //                navigationManager.NavigateTo(navigationState.ReturnUrl);
        //            }
        //            else
        //            {
        //                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        notificationService.Notify(new NotificationMessage()
        //        {
        //            Severity = NotificationSeverity.Error,
        //            Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
        //            Style = "position: fixed !important; left: 0; margin: auto; "
        //        });
        //    }
        //}

        #endregion
        #region ijaz changes
        //< History Author = 'Ijaz Ahmad' Date = '2022-03-04' Version = "1.0" Branch = "master" >Submit Form</History>
        protected async Task Form0Submit(MergeRequest args)
        {
            try
            {
                if (mergeRequest.PrimaryId == Guid.Empty || String.IsNullOrEmpty(mergeRequest.Reason))
                {
                    primaryValidationMsg = mergeRequest.PrimaryId == Guid.Empty ? translationState.Translate("Required_Field") : "";
                    reasonValidationMsg = String.IsNullOrEmpty(mergeRequest.Reason) ? translationState.Translate("Required_Field") : "";
                    return;
                }
                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await cmsRegisteredCaseService.CreateMergeRequest(mergeRequest);
                    await SaveTempAttachementToUploadedDocument();
                    spinnerService.Hide();
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Merge_Request_Submitted"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await Task.Delay(1500);
                        navigationManager.NavigateTo(navigationState.ReturnUrl);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
        protected async Task SaveTempAttachementToUploadedDocument()
        {
            try
            {
                List<Guid> requestIds = new List<Guid>
                {
                    mergeRequest.Id
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
        #region Redirect Function

        //<History Author = 'Hassan Abbas' Date='2022-11-25' Version="1.0" Branch="master">Redirect to respective page</History>
        protected async Task RedirectBack()
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Cancel"), translationState.Translate("Confirm_Cancel"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                navigationManager.NavigateTo(navigationState.ReturnUrl);
            }
        }
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

    }
}
