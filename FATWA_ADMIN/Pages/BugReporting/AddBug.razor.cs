using DocumentFormat.OpenXml.Spreadsheet;
using FATWA_ADMIN.Data;
using FATWA_DOMAIN.Models.BugReporting;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Syncfusion.Blazor.RichTextEditor;
using System.Text.RegularExpressions;
using static FATWA_DOMAIN.Enums.BugReporting.BugReportingEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.BugReporting
{
    public partial class AddBug : ComponentBase
    {
        #region Parameter
        [Parameter]
        public string BugId { get; set; }
        #endregion

        #region Variable Declaration
        public ReportedBug addBug = new ReportedBug { Id = Guid.NewGuid(), CreatedDate = DateTime.Now };
        protected List<BugApplication> applications { get; set; } = new List<BugApplication>();
        protected List<BugModule> modules { get; set; } = new List<BugModule>();
        protected List<BugIssueType> issueTypes { get; set; } = new List<BugIssueType>();
        protected List<BugIssueType> filteredIssueTypes { get; set; } = new List<BugIssueType>();
        protected List<BugModuleTypeAssignment> assignedModule { get; set; } = new List<BugModuleTypeAssignment>();
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
            if (BugId == null)
            {
                await GetAutoGeneratedId();
            }
            else
            {
                await GetReportedBugById();
                await PopulateAssignedModuleByTypeId(false);
                await PopulateModulesByApplicationId((int)addBug.ApplicationId);
            }
            await PopulateDropDownsData();

        }
        #endregion

        #region Get Auto Generated Ids
        protected async Task GetAutoGeneratedId()
        {
            try
            {
                var response = await bugReportingService.GetAutoGeneratedBugNumber();
                if (response.IsSuccessStatusCode)
                {
                    var ticketResult = (ReportedBug)response.ResultData;
                    addBug.PrimaryBugId = ticketResult.PrimaryBugId;
                    addBug.ShortNumber = ticketResult.ShortNumber;

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

        #region Populate DropDown Data
        protected async Task PopulateDropDownsData()
        {
            await PopulateAllApplications();
            await PopulateIssuesTypes();

        }
        #endregion

        #region Populate Data
        protected async Task PopulateAllApplications()
        {
            try
            {
                var response = await lookupService.GetAllApplications();
                if (response.IsSuccessStatusCode)
                {
                    applications = (List<BugApplication>)response.ResultData;
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
        protected async Task PopulateIssuesTypes()
        {
            try
            {
                var response = await lookupService.GetIssuesTypes();
                if (response.IsSuccessStatusCode)
                {
                    issueTypes = (List<BugIssueType>)response.ResultData;
                    filteredIssueTypes = issueTypes.Where(x => x.IsDeleted != true).ToList();
                    if (BugId != null)
                    {
                        var bugType = issueTypes.Where(x => x.Id == addBug.TypeId && x.IsSystemGenerated != true).FirstOrDefault();
                        if (bugType != null)
                        {
                            filteredIssueTypes.Add(bugType);
                        }
                    }
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
                    addBug = (ReportedBug)response.ResultData;

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

        #region Populate Modules By ApplicationId
        protected async Task PopulateModulesByApplicationId(int Id)
        {
            try
            {
                var response = await lookupService.GetModulesByApplicationId(Id);
                if (response.IsSuccessStatusCode)
                {
                    modules = (List<BugModule>)response.ResultData;

                    modules = modules.Where(module => assignedModule.Select(assigned => assigned.ModuleId).Contains(module.Id)).ToList();
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
            StateHasChanged();
        }
        #endregion

        #region Populate Assigned Module By Type Id
        protected async Task PopulateAssignedModuleByTypeId(bool isChange)
        {
            try
            {
                if (addBug.TypeId != null)
                {
                    var response = await bugReportingService.GetAssignTypeModulesById((int)addBug.TypeId);
                    if (response.IsSuccessStatusCode)
                    {
                        assignedModule = (List<BugModuleTypeAssignment>)response.ResultData;
                        if (isChange)
                        {
                            addBug.ApplicationId = 0;
                            addBug.ModuleId = 0;
                        }
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
        #endregion

        #region On Change Application
        protected async Task OnChangeApplication(object arg)
        {
            modules = new List<BugModule>();
            if (arg != null)
            {
                await PopulateModulesByApplicationId((int)arg);
            }
            else
            {
                addBug.ModuleId = 0;
            }
        }
        #endregion

        #region Submit Add Bug
        protected async Task SubmitForm(ReportedBug reportedBug)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(reportedBug.Description) || Regex.IsMatch(reportedBug.Description, RegexPatterns.specificEmptyContentRichTextEditorPattern))
                {
                    descriptionValidationMessage = String.IsNullOrEmpty(reportedBug.Description) ? translationState.Translate("Required_Field") : Regex.IsMatch(reportedBug.Description, RegexPatterns.specificEmptyContentRichTextEditorPattern) ? translationState.Translate("Cannot_have_trailing_spaces") : "";
                }
                else
                {
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
                            reportedBug.StatusId = (int)BugStatusEnum.New;
                            ApiCallResponse response = new ApiCallResponse();
                            if (BugId == null)
                            {
                                response = await bugReportingService.CreateBug(reportedBug);
                            }
                            else
                            {
                                response = await bugReportingService.UpdateBug(reportedBug);
                            }
                            if (response.IsSuccessStatusCode)
                            {
                                await SaveTempAttachementToUploadedDocument();
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate(BugId == null ? "Bug_Added_Successfully" : "Bug_Updated_Successfully"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                                await RedirectBack();
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
                    await RedirectBack();
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
                    RequestIds = new List<Guid> { addBug.Id },
                    CreatedBy = addBug.CreatedBy,
                    FilePath = _config.GetValue<string>("dms_file_path"),
                    DeletedAttachementIds = addBug.DeletedAttachementIds
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
