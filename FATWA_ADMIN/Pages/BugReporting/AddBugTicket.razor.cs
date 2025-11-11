using DocumentFormat.OpenXml.Spreadsheet;
using FATWA_ADMIN.Data;
using FATWA_DOMAIN.Models.BugReporting;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Syncfusion.Blazor.RichTextEditor;
using System.Text.RegularExpressions;
using static FATWA_DOMAIN.Enums.BugReporting.BugReportingEnum;
using static FATWA_GENERAL.Helper.Response;
using Group = FATWA_DOMAIN.Models.AdminModels.UserManagement.Group;

namespace FATWA_ADMIN.Pages.BugReporting
{
    //< History Author = 'Muhammad Zaeem' Date = '2024-04-28' Version = "1.0" Branch = "master" Create Bug Ticket</History>

    public partial class AddBugTicket : ComponentBase
    {
        #region Parameter
        [Parameter]
        public string TicketId { get; set; }
        [Parameter]
        public string BugId { get; set; }
        #endregion

        #region Variables Declaration

        BugTicket addBugTicket = new BugTicket { Id = Guid.NewGuid(), CreatedDate = DateTime.Now };
        protected List<BugApplication> applications { get; set; } = new List<BugApplication>();
        protected List<BugModule> modules { get; set; } = new List<BugModule>();
        protected List<BugIssueType> issueTypes { get; set; } = new List<BugIssueType>();
        protected List<Priority> priorities { get; set; } = new List<Priority>();
        protected List<BugSeverity> severities { get; set; } = new List<BugSeverity>();
        protected List<BugStatus> bugStatuses { get; set; } = new List<BugStatus>();
        protected List<UserDataVM> users { get; set; } = new List<UserDataVM>();
        public ReportedBug reportedBug = new ReportedBug();
        protected string dateValidationMsg = "";
        protected List<BugModuleTypeAssignment> assignedModule { get; set; } = new List<BugModuleTypeAssignment>();
        protected List<Group> Groups { get; set; } = new List<Group>();
        public string? descriptionValidationMessage { get; set; } = "";


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
        #region Load Component
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }
        protected async Task Load()
        {
            if (TicketId == null)
            {
                await GetAutoGeneratedId();
                addBugTicket.StatusId = (int)BugStatusEnum.New;
            }
            else
            {
                await GetBugTicketById();

            }
            if (!string.IsNullOrEmpty(BugId))
            {
                await GetReportedBugById();
            }
            await PopulateDropDownsData();
        }
        #endregion

        #region Populate DropDown Data
        protected async Task PopulateDropDownsData()
        {
            await PopulateBugStatuses();
        }
        #endregion

        #region Get Auto Generated Ids
        protected async Task GetAutoGeneratedId()
        {
            try
            {
                var response = await bugReportingService.GetAutoGeneratedIds();
                if (response.IsSuccessStatusCode)
                {
                    var ticketResult = (BugTicket)response.ResultData;
                    addBugTicket.TicketId = ticketResult.TicketId;
                    addBugTicket.ShortNumber = ticketResult.ShortNumber;

                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Populate Data
        protected async Task PopulateBugStatuses()
        {
            try
            {
                var response = await lookupService.GetBugStatuses();
                if (response.IsSuccessStatusCode)
                {
                    bugStatuses = (List<BugStatus>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            StateHasChanged();
        }

        #endregion
        #region Get Bug Detail
        protected async Task GetReportedBugById()
        {
            try
            {
                var response = await bugReportingService.GetReportedBugById(Guid.Parse(BugId));
                if (response.IsSuccessStatusCode)
                {
                    reportedBug = (ReportedBug)response.ResultData;
                    addBugTicket.BugId = Guid.Parse(BugId);
                    addBugTicket.BugNumber = reportedBug.PrimaryBugId;
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

        #region Get Bug Ticket Detail
        protected async Task GetBugTicketById()
        {
            var response = await bugReportingService.GetBugTicketById(Guid.Parse(TicketId));
            if (response.IsSuccessStatusCode)
            {
                addBugTicket = (BugTicket)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Form Submit
        protected async Task FormSubmit(BugTicket ticket)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ticket.Description) || Regex.IsMatch(ticket.Description, RegexPatterns.specificEmptyContentRichTextEditorPattern))
                {
                    descriptionValidationMessage = String.IsNullOrEmpty(ticket.Description) ? translationState.Translate("Required_Field") : Regex.IsMatch(ticket.Description, RegexPatterns.specificEmptyContentRichTextEditorPattern) ? translationState.Translate("Cannot_have_trailing_spaces") : "";
                }
                else
                {
                    descriptionValidationMessage = "";
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
                            spinnerService.Show();
                            ApiCallResponse response = new ApiCallResponse();
                            ticket.StatusId = (int)BugStatusEnum.New;
                            ticket.PortalId = (int)ApplicationEnums.FatwaAdminPortal;
                            if (TicketId == null)
                            {
                                ticket.ReportedBy = loginState.Username;
                                response = await bugReportingService.CreateBugTicket(ticket);
                            }
                            else
                            {
                                response = await bugReportingService.UpdateBugTicket(ticket);
                            }
                            if (response.IsSuccessStatusCode)
                            {
                                await SaveTempAttachementToUploadedDocument();
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate(TicketId == null ? "Bug_Ticket_Added_Successfully" : "Bug_Ticket_Updated_Successfully"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                                dialogService.Close();
                            }
                            else
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                            }
                            spinnerService.Hide();

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

        #region Save Ticket As Draft 
        protected async Task SaveAsDraft(BugTicket draftBugTicket)
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                    translationState.Translate("Sure_Save_Draft"),
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
                        spinnerService.Show();
                        ApiCallResponse response = new ApiCallResponse();
                        draftBugTicket.StatusId = (int)BugStatusEnum.Draft;
                        draftBugTicket.PortalId = (int)ApplicationEnums.FatwaAdminPortal;
                        if (TicketId == null)
                        {
                            draftBugTicket.ReportedBy = loginState.Username;
                            response = await bugReportingService.CreateBugTicket(draftBugTicket);
                        }
                        else
                        {
                            response = await bugReportingService.UpdateBugTicket(draftBugTicket);
                        }
                        if (response.IsSuccessStatusCode)
                        {
                            await SaveTempAttachementToUploadedDocument();
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate(TicketId == null ? "Bug_Ticket_Added_Successfully" : "Bug_Ticket_Updated_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            dialogService.Close();
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                        spinnerService.Hide();

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
                   translationState.Translate("Cancel"),
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

        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        protected async Task RedirectBack()
        {
            await jSRuntime.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

        #region upload Documents
        protected async Task SaveTempAttachementToUploadedDocument()
        {
            try
            {
                var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                {
                    RequestIds = new List<Guid> { addBugTicket.Id },
                    CreatedBy = loginState.UserDetail.Email,
                    FilePath = _config.GetValue<string>("dms_file_path"),
                    DeletedAttachementIds = addBugTicket.DeletedAttachementIds
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
