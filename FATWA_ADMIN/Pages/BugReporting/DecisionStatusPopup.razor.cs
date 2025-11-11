using DocumentFormat.OpenXml.Spreadsheet;
using FATWA_ADMIN.Data;
using FATWA_DOMAIN.Models.ViewModel.BugReportingVMs;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Syncfusion.Blazor.RichTextEditor;
using System.Text.RegularExpressions;
using static FATWA_DOMAIN.Enums.BugReporting.BugReportingEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.BugReporting
{
    public partial class DecisionStatusPopup : ComponentBase
    {
        #region Parameter
        [Parameter]
        public Guid ReferenceId { get; set; }
        [Parameter]
        public int Status { get; set; }
        [Parameter]
        public int DecisionFrom { get; set; }
        #endregion
        #region Variable Declaration 
        public DecisionStatusVM decisionStatus = new DecisionStatusVM();
        public string reasonValidationMessage { get; set; } = "";
        public string StatusValidationMessage { get; set; } = "";
        public List<int>? DeletedAttachementIds { get; set; } = new List<int>();
        public class StautsEnumTemp
        {
            public int StatusEnumValue { get; set; }
            public string StatusEnumName { get; set; }
        }
        public List<StautsEnumTemp> bugStatuses { get; set; } = new List<StautsEnumTemp>();

        #endregion
        #region SyncFusion Rich Text Editor
        private List<ToolbarItemModel> Tools = new List<ToolbarItemModel>()
        {
        new ToolbarItemModel() { Command = ToolbarCommand.Formats },
        new ToolbarItemModel() { Command = ToolbarCommand.Separator },
        new ToolbarItemModel() { Command = ToolbarCommand.Bold },
        new ToolbarItemModel() { Command = ToolbarCommand.Italic },
        new ToolbarItemModel() { Command = ToolbarCommand.Underline },
        new ToolbarItemModel() { Command = ToolbarCommand.Separator },
        new ToolbarItemModel() { Command = ToolbarCommand.FontColor },
        new ToolbarItemModel() { Command = ToolbarCommand.Separator },
        new ToolbarItemModel() { Command = ToolbarCommand.NumberFormatList },
        new ToolbarItemModel() { Command = ToolbarCommand.BulletFormatList },
        new ToolbarItemModel() { Command = ToolbarCommand.Separator },
        new ToolbarItemModel() { Command = ToolbarCommand.CreateLink },
        new ToolbarItemModel() { Command = ToolbarCommand.Image },
        new ToolbarItemModel() { Command = ToolbarCommand.CreateTable },
        new ToolbarItemModel() { Command = ToolbarCommand.InsertCode },
        new ToolbarItemModel() { Command = ToolbarCommand.SourceCode },
        new ToolbarItemModel() { Command = ToolbarCommand.Separator },
        new ToolbarItemModel() { Command = ToolbarCommand.Undo },
        new ToolbarItemModel() { Command = ToolbarCommand.Redo }
        };
        private List<TableToolbarItemModel> TableQuickToolbarItems = new List<TableToolbarItemModel>()
        {
        new TableToolbarItemModel() { Command = TableToolbarCommand.TableHeader },
        new TableToolbarItemModel() { Command = TableToolbarCommand.TableRows },
        new TableToolbarItemModel() { Command = TableToolbarCommand.TableColumns },
        new TableToolbarItemModel() { Command = TableToolbarCommand.TableCell },
        new TableToolbarItemModel() { Command = TableToolbarCommand.HorizontalSeparator },
        new TableToolbarItemModel() { Command = TableToolbarCommand.TableRemove },
        new TableToolbarItemModel() { Command = TableToolbarCommand.BackgroundColor },
        new TableToolbarItemModel() { Command = TableToolbarCommand.TableCellVerticalAlign },
        new TableToolbarItemModel() { Command = TableToolbarCommand.Styles }
        };
        private List<ImageToolbarItemModel> ImageTools = new List<ImageToolbarItemModel>()
        {
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.Replace },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.Align },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.Caption },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.Remove },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.HorizontalSeparator },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.InsertLink },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.Display },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.AltText },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.Dimension }
        };
        private List<string> allowedTypes = new List<string> { ".png", ".jpg", ".jpeg" };
        #endregion

        #region Load Componenet
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }
        protected async Task Load()
        {
            await PopulateStatuses();
        }
        #endregion
        #region Populate 
        protected async Task PopulateStatuses()
        {
            try
            {
                try
                {
                    foreach (BugStatusEnum item in Enum.GetValues(typeof(BugStatusEnum)))
                    {
                        if (item == BugStatusEnum.Closed)
                        {
                            bugStatuses.Add(new StautsEnumTemp { StatusEnumName = translationState.Translate("Ticket_Close"), StatusEnumValue = (int)item });
                        }
                        if (item == BugStatusEnum.Reopened)
                        {
                            bugStatuses.Add(new StautsEnumTemp { StatusEnumName = translationState.Translate("Ticket_Reopen"), StatusEnumValue = (int)item });
                        }
                        if (Status > 0)
                        {
                            if (item == BugStatusEnum.Rejected)
                            {
                                bugStatuses.Add(new StautsEnumTemp { StatusEnumName = translationState.Translate("Reject"), StatusEnumValue = (int)item });

                            }
                            if (item == BugStatusEnum.Resolved)
                            {
                                bugStatuses.Add(new StautsEnumTemp { StatusEnumName = translationState.Translate("Resolve"), StatusEnumValue = (int)item });

                            }
                            decisionStatus.StatusId = Status;
                        }
                        else
                        {
                            decisionStatus.StatusId = (int)BugStatusEnum.Closed;

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
        #region Submit
        protected async Task SubmitStatus()
        {
            try
            {
                if (decisionStatus.StatusId != (int)BugStatusEnum.Closed && (string.IsNullOrEmpty(decisionStatus.Reason) || Regex.IsMatch(decisionStatus.Reason, RegexPatterns.specificEmptyContentRichTextEditorPattern) || decisionStatus.StatusId == 0))
                {
                    reasonValidationMessage = string.IsNullOrEmpty(decisionStatus.Reason) ? translationState.Translate("Required_Field") : Regex.IsMatch(decisionStatus.Reason, RegexPatterns.specificEmptyContentRichTextEditorPattern) ? translationState.Translate("Cannot_have_trailing_spaces") : "";
                    StatusValidationMessage = decisionStatus.StatusId == 0 ? translationState.Translate("Required_Field") : "";
                }
                else
                {
                    reasonValidationMessage = "";
                    StatusValidationMessage = "";
                    bool? dialogResponse = await dialogService.Confirm(
                    translationState.Translate("Sure_Submit"),
                    translationState.Translate("Submit"),
                    new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    });
                    if (dialogResponse != null)
                    {
                        if ((bool)dialogResponse)
                        {
                            decisionStatus.ReferenceId = ReferenceId;
                            ApiCallResponse response = new ApiCallResponse();
                            if (DecisionFrom == (int)DecisionFromOptionEnum.FromTicket)
                            {
                                response = await bugReportingService.UpdateTicketStatus(decisionStatus);
                            }
                            else if (DecisionFrom == (int)DecisionFromOptionEnum.FromBug)
                            {
                                response = await bugReportingService.UpdateBugStatus(decisionStatus);
                            }

                            if (response.IsSuccessStatusCode)
                            {
                                if (decisionStatus.StatusId == (int)BugStatusEnum.Reopened)
                                {
                                    await SaveTempAttachementToUploadedDocument();

                                }
                                if (DecisionFrom == (int)DecisionFromOptionEnum.FromTicket)
                                {
                                    notificationService.Notify(new NotificationMessage()
                                    {
                                        Severity = NotificationSeverity.Success,
                                        Detail = translationState.Translate(decisionStatus.StatusId == (int)BugStatusEnum.Closed ? "Ticket_Closed_Successfully" : decisionStatus.StatusId == (int)BugStatusEnum.Reopened ? "Ticket_Reopened_Successfully" : decisionStatus.StatusId == (int)BugStatusEnum.Rejected ? "Ticket_Rejected_Successfully" : "Ticket_Resolved_Successfully"),
                                        Style = "position: fixed !important; left: 0; margin: auto; "
                                    });
                                }
                                else if (DecisionFrom == (int)DecisionFromOptionEnum.FromBug)
                                {
                                    notificationService.Notify(new NotificationMessage()
                                    {
                                        Severity = NotificationSeverity.Success,
                                        Detail = translationState.Translate("Bug_Resolved_Successfully"),
                                        Style = "position: fixed !important; left: 0; margin: auto; "
                                    });
                                }
                                dialogService.Close(true);
                            }
                            else
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                            }
                        }
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
        #region Redirect and Dialog Events
        protected async void ButtonCancelClick(MouseEventArgs args)
        {
            bool? dialogResponse = await dialogService.Confirm(
                   translationState.Translate("Sure_Cancel"),
                   translationState.Translate("Submit"),
                   new ConfirmOptions()
                   {
                       OkButtonText = translationState.Translate("OK"),
                       CancelButtonText = translationState.Translate("Cancel")
                   });
            if (dialogResponse != null)
            {
                if ((bool)dialogResponse)
                {
                    dialogService.Close();
                }
            }
        }
        #endregion

        #region upload Documents
        protected async Task SaveTempAttachementToUploadedDocument()
        {
            try
            {
                var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                {
                    RequestIds = new List<Guid> { (Guid)decisionStatus.CommentId },
                    CreatedBy = loginState.UserDetail.Email,
                    FilePath = _config.GetValue<string>("dms_file_path"),
                    DeletedAttachementIds = DeletedAttachementIds
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
