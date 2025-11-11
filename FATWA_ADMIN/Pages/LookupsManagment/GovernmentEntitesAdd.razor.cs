using FATWA_DOMAIN.Models.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class GovernmentEntitesAdd : ComponentBase
    {
        #region Paramter

        [Parameter]
        public dynamic EntityId { get; set; }
        #endregion

        #region Variables
        protected RadzenDataGrid<CmsBankGovernmentEntity>? grid1 = new RadzenDataGrid<CmsBankGovernmentEntity>();

        protected string search;

        protected GovernmentEntity GovernmentEntity;
        ApiCallResponse response = new ApiCallResponse();
        #endregion

        #region On Load

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async Task Load()
        {
            if (EntityId == null)
            {
                spinnerService.Show();
                GovernmentEntity = new GovernmentEntity() { };
                spinnerService.Hide();
            }
            else
            {
                spinnerService.Show();
                response = await lookupService.GetGovernmentEntitysById(EntityId);
                if (response.IsSuccessStatusCode)
                {
                    GovernmentEntity = (GovernmentEntity)response.ResultData;
                    response = await lookupService.GetBankDetailByEntityId(EntityId);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        GovernmentEntity.CmsBankGovernmentEntities = (List<CmsBankGovernmentEntity>)response.ResultData;
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
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
        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(false);
        }
        protected async Task SaveChanges(GovernmentEntity args)
        {
            try
            {
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
                    if (EntityId != null)
                    {
                        var response = await lookupService.UpdateGovernmentEntity(GovernmentEntity);
                        if (response.IsSuccessStatusCode)
                        {
                            spinnerService.Hide();
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Government_Entity_Updated_Successfully"),
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
                        var response = await lookupService.SaveGovernmentEntity(GovernmentEntity);
                        if (response.IsSuccessStatusCode)
                        {
                            spinnerService.Hide();
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Government_Entity_Added_Successfully"),
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
                    Detail = EntityId == null ? translationState.Translate("Could_not_create_a_new_Government_Entities") : translationState.Translate("Government_Entities_could_not_be_updated"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                spinnerService.Hide();
            }
        }
        #endregion

        #region Add Bank 
        protected async Task AddBankDetail(MouseEventArgs args)
        {
            try
            {

                var dialogResult = await dialogService.OpenAsync<GovernmentEntityBankDetailAdd>(
                    translationState.Translate("Add_Bank_Detail"),
                    null,
                    new Radzen.DialogOptions() { Width = "30%", CloseDialogOnOverlayClick = false });

                if (dialogResult != null)
                {
                    var newBankDetail = (CmsBankGovernmentEntity)dialogResult;
                    GovernmentEntity.CmsBankGovernmentEntities.Add(newBankDetail);
                    await grid1.Reload();
                    await RefreshGrid();
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        private async Task RefreshGrid()
        {
            StateHasChanged();
            await Task.Delay(200);
        }
        #endregion

        #region Delete Bank Detail 
        protected async Task DeleteItem(CmsBankGovernmentEntity args, bool deleteFromDatabase = true)
        {
            if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Delete_Record"), translationState.Translate("Delete"),
                    new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    }) == true)
            {
                if (deleteFromDatabase)
                {
                    // Delete the item from the database
                    var response = await lookupService.DeleteBankDetail(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        return;
                    }
                }

                // Remove the item from the supervisorsAndManagersGrid
                GovernmentEntity.CmsBankGovernmentEntities.Remove(args);
                await grid1.Reload();
                StateHasChanged();
            }
        }
        #endregion
    }
}
