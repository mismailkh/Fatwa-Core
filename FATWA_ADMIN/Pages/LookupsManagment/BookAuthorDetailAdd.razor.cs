using FATWA_DOMAIN.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class BookAuthorDetailAdd : ComponentBase
    {
        #region Paramter
        [Parameter]
        public dynamic AuthorId { get; set; }
        #endregion

        #region Variables

        protected LmsLiteratureAuthor LmsLiteratureAuthor;
        ApiCallResponse response = new ApiCallResponse();
        #endregion

        #region On Load
        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        protected async Task Load()
        {
            if (AuthorId == null)
            {
                spinnerService.Show();
                LmsLiteratureAuthor = new LmsLiteratureAuthor() { };
                spinnerService.Hide();
            }
            else
            {
                spinnerService.Show();
                response = await lookupService.GetBookAuthorById(AuthorId);
                if (response.IsSuccessStatusCode)
                {
                    LmsLiteratureAuthor = (LmsLiteratureAuthor)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }

        }
        #endregion

        #region Form Submit
        protected async Task SaveChanges(LmsLiteratureAuthor args)
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

                    if (AuthorId == null)
                    {
                        var fatwaDbCreateLmsLiteratureAuthorResult = await lookupService.SaveBookAuthor(LmsLiteratureAuthor);
                        if (fatwaDbCreateLmsLiteratureAuthorResult.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Book_Author_Added_Successfully"),
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
                        var fatwaDbUpdateLmsLiteratureTypeResult = await lookupService.UpdateBookAuthor(LmsLiteratureAuthor);
                        if (fatwaDbUpdateLmsLiteratureTypeResult.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Book_Author_Updated_Successfully"),
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
                    Detail = AuthorId == null ? translationState.Translate("Could_not_create_a_new_Book_Author") : translationState.Translate("Book_Author_could_not_be_updated"),
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
    }
}
