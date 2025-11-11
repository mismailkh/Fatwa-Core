using FATWA_ADMIN.Data;
using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Enums;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.BugReporting;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.BugReportingVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.RichTextEditor;
using System.Text.RegularExpressions;
using static FATWA_DOMAIN.Enums.BugReporting.BugReportingEnum;
using static FATWA_GENERAL.Helper.Response;


namespace FATWA_ADMIN.Pages.BugReporting
{
    public partial class DetailBugTicket : ComponentBase
    {
        #region Parameters
        [Parameter]
        public string TicketId { get; set; }
        [Parameter]
        public string FromTicket { get; set; }
        #endregion

        #region Variable 
        public TicketDetailVM Ticket { get; set; } = new TicketDetailVM();
        protected List<TicketStatusHistoryVM> ticketStatusHistories { get; set; } = new List<TicketStatusHistoryVM>();
        protected List<BugTicketCommentVM> bugTicketComments { get; set; } = new List<BugTicketCommentVM>();
        protected List<BugTicketCommentVM> bugTicketFeedBack { get; set; } = new List<BugTicketCommentVM>();
        protected List<BugTicketCommentVM> bugTicketReopenReason { get; set; } = new List<BugTicketCommentVM>();
        public bool FromTicketFlag { get { return Convert.ToBoolean(FromTicket); } set { FromTicketFlag = value; } }
        public string commentValue { get; set; } = "";
        public BugCommentFeedBack bugTicketComment { get; set; } = new BugCommentFeedBack();
        public bool isShowEditor { get; set; } = false;
        public Guid editingCommentId = Guid.Empty;
        public Guid replyToCommentId = Guid.Empty;
        public DecisionStatusVM decisionStatus = new DecisionStatusVM();
        protected List<BugTicketCommentVM> bugTicketResolution { get; set; } = new List<BugTicketCommentVM>();
        protected List<BugTicketCommentVM> bugTicketRejectReason { get; set; } = new List<BugTicketCommentVM>();
        protected List<ReportedBugDetailVM> TaggedBug { get; set; } = new List<ReportedBugDetailVM>();
        protected RadzenDataGrid<BugTicketCommentVM> feedbackGrid = new RadzenDataGrid<BugTicketCommentVM>();
        protected RadzenDataGrid<TicketStatusHistoryVM> historyGrid = new RadzenDataGrid<TicketStatusHistoryVM>();
        protected RadzenDataGrid<BugTicketCommentVM> reasonGrid = new RadzenDataGrid<BugTicketCommentVM>();
        protected RadzenDataGrid<BugTicketCommentVM> rejectionGrid = new RadzenDataGrid<BugTicketCommentVM>();
        protected RadzenDataGrid<ReportedBugDetailVM> bugGrid = new RadzenDataGrid<ReportedBugDetailVM>();
        public bool IsShowCommentActions { get; set; } = false;
        protected IEnumerable<UserListMentionVM> users = new List<UserListMentionVM>();
        public SfRichTextEditor editor { get; set; } = new SfRichTextEditor();
        public SfMention<UserListMentionVM> mention { get; set; } = new SfMention<UserListMentionVM>();
        public bool isVisible { get; set; }
        public AddEmployeeVM userDetail { get; set; } = new AddEmployeeVM();

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
        #region OnInitialized
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            translationState.TranslateGridFilterLabels(feedbackGrid);
            translationState.TranslateGridFilterLabels(historyGrid);
            translationState.TranslateGridFilterLabels(reasonGrid);
            translationState.TranslateGridFilterLabels(rejectionGrid);
            translationState.TranslateGridFilterLabels(bugGrid);
            bugTicketComment.MentionedUserTranslatedName = translationState.Translate("IT_Support_Team");
            spinnerService.Hide();
        }
        #endregion

        #region Load
        protected async Task Load()
        {
            await GetBugTicketDetail();
            await GetBugTaggedWithTicket();
            await PopulateHistory();
            await LoadBugTicketComments();
            await LoadBugTicketFeedBack();
            await LoadBugTicketReopenReason();
            await LoadBugTicketResolution();
            await LoadBugTicketRejectionReason();
            await GetAllUsers();

        }
        #endregion
        #region Get User List
        protected async Task GetAllUsers()
        {
            try
            {
                var response = await userService.GetUsersListForMention(Guid.Parse(TicketId), loginState.UserDetail.Email);
                if (response.IsSuccessStatusCode)
                {
                    users = (List<UserListMentionVM>)response.ResultData;
                    if (Ticket?.PortalId == (int)ApplicationEnums.G2GPortal || Ticket?.PortalId == (int)ApplicationEnums.G2GAdminPortal)
                    {
                        var userList = users.ToList();
                        userList.Add(new UserListMentionVM { UserFullNameEn = translationState.Translate(MentionUserEnum.GEUser.GetDisplayName()), UserFullNameAr = translationState.Translate(MentionUserEnum.GEUser.GetDisplayName()), FullNameAbbrevation = "G", UserId = Convert.ToString((int)MentionUserEnum.GEUser) });
                        users = userList;
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception)
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
        #region Load Bug Tagged with Ticket
        protected async Task GetBugTaggedWithTicket()
        {
            try
            {
                if (Ticket.BugId != null)
                {
                    var response = await bugReportingService.GetReportedBugDetail((Guid)Ticket.BugId);
                    if (response.IsSuccessStatusCode)
                    {
                        var reportedBug = (ReportedBugDetailVM)response.ResultData;
                        TaggedBug = new List<ReportedBugDetailVM>();
                        TaggedBug.Add(reportedBug);
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
        #region LoadBugTicketResolution
        protected async Task LoadBugTicketResolution()
        {
            try
            {
                var response = await bugReportingService.GetBugTicketCommentFeedBack(Guid.Parse(TicketId), (int)RemarksTypeEnum.Resolution);
                if (response.IsSuccessStatusCode)
                {
                    bugTicketResolution = (List<BugTicketCommentVM>)response.ResultData;
                    await InvokeAsync(StateHasChanged);
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

        #region Get Bug Ticket Details
        protected async Task GetBugTicketDetail()
        {
            try
            {
                var response = await bugReportingService.GetBugTicketDetail(Guid.Parse(TicketId));
                if (response.IsSuccessStatusCode)
                {
                    Ticket = (TicketDetailVM)response.ResultData;
                    await InvokeAsync(StateHasChanged);
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

        #region LoadBugTicketComments
        protected async Task LoadBugTicketComments()
        {
            try
            {
                var response = await bugReportingService.GetBugTicketCommentFeedBack(Guid.Parse(TicketId), (int)RemarksTypeEnum.Comment);
                if (response.IsSuccessStatusCode)
                {
                    bugTicketComments = (List<BugTicketCommentVM>)response.ResultData;
                    await InvokeAsync(StateHasChanged);
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

        #region LoadBugTicketReason
        protected async Task LoadBugTicketReopenReason()
        {
            try
            {
                var response = await bugReportingService.GetBugTicketCommentFeedBack(Guid.Parse(TicketId), (int)RemarksTypeEnum.Reason);
                if (response.IsSuccessStatusCode)
                {
                    bugTicketReopenReason = (List<BugTicketCommentVM>)response.ResultData;
                    await InvokeAsync(StateHasChanged);
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

        #region Load Bug Ticket Rejection Reason
        protected async Task LoadBugTicketRejectionReason()
        {
            try
            {
                var response = await bugReportingService.GetBugTicketCommentFeedBack(Guid.Parse(TicketId), (int)RemarksTypeEnum.RejectReason);
                if (response.IsSuccessStatusCode)
                {
                    bugTicketRejectReason = (List<BugTicketCommentVM>)response.ResultData;
                    await InvokeAsync(StateHasChanged);
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

        #region Load FeedBack 
        protected async Task LoadBugTicketFeedBack()
        {
            try
            {
                var response = await bugReportingService.GetBugTicketCommentFeedBack(Guid.Parse(TicketId), (int)RemarksTypeEnum.Feedback);
                if (response.IsSuccessStatusCode)
                {
                    bugTicketFeedBack = (List<BugTicketCommentVM>)response.ResultData;
                    await InvokeAsync(StateHasChanged);
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

        #region PopulateHistory
        public async Task PopulateHistory()
        {
            try
            {
                var response = await bugReportingService.GetTicketStatusHistoryById(Guid.Parse(TicketId));
                if (response.IsSuccessStatusCode)
                {
                    ticketStatusHistories = (List<TicketStatusHistoryVM>)response.ResultData;
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

        #region Button Click Event

        protected async Task OnClickAddComment()
        {
            commentValue = string.Empty;
            isShowEditor = true;
            editingCommentId = Guid.Empty;
            replyToCommentId = Guid.Empty;
            IsShowCommentActions = true;
        }
        protected async Task AddComment()
        {
            try
            {

                if (string.IsNullOrEmpty(commentValue) || Regex.IsMatch(commentValue, RegexPatterns.specificEmptyContentRichTextEditorPattern))
                {
                    if (Regex.IsMatch(commentValue, RegexPatterns.specificEmptyContentRichTextEditorPattern))
                    {
                        string strippedDescription = Regex.Replace(commentValue, "<.*?>", string.Empty);
                        commentValue = strippedDescription.Replace("&nbsp;", string.Empty).Trim();
                        commentValue = strippedDescription.Replace("br", string.Empty).Trim();
                    }
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Please_Add_Comment"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
                else
                {
                    spinnerService.Show();
                    ApiCallResponse response = new ApiCallResponse();
                    foreach (var item in bugTicketComment.MentionedUser)
                    {
                        if (!commentValue.Contains(item.Value))
                        {
                            bugTicketComment.MentionedUser.Remove(item.Key);
                        }
                    }
                    if (editingCommentId == Guid.Empty)
                    {
                        bugTicketComment.Id = Guid.NewGuid();
                        bugTicketComment.ReferenceId = Ticket.Id;
                        bugTicketComment.Comment = commentValue;
                        bugTicketComment.RemarkType = (int)RemarksTypeEnum.Comment;
                        if (string.IsNullOrEmpty(bugTicketComment.MentionedUserTranslatedName))
                        {
                            bugTicketComment.MentionedUserTranslatedName = translationState.Translate("IT_Support_Team");
                        }
                        response = await bugReportingService.AddCommentFeedBack(bugTicketComment);
                    }
                    else
                    {
                        bugTicketComment.Comment = commentValue;
                        bugTicketComment.Id = editingCommentId;
                        bugTicketComment.ReferenceId = Ticket.Id;
                        bugTicketComment.TicketStatusId = (int)Ticket.StatusId;
                        response = await bugReportingService.UpdateCommentFeedBack(bugTicketComment);
                        editingCommentId = Guid.Empty;

                    }

                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate(editingCommentId == Guid.Empty ? "Comment_Added_Successfully" : "Comment_Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await LoadBugTicketComments();
                        await PopulateHistory();
                        commentValue = string.Empty;
                        bugTicketComment = new BugCommentFeedBack();
                        IsShowCommentActions = false;

                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    isShowEditor = false;
                    spinnerService.Hide();
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
        protected async Task OnCancel()
        {
            isShowEditor = false;
            commentValue = string.Empty;
            editingCommentId = Guid.Empty;
            IsShowCommentActions = false;
        }
        protected async Task AddFeedBack()
        {
            await dialogService.OpenAsync<AddCommentFeedBack>(
           translationState.Translate("FeedBack"),
           new Dictionary<string, object>() { { "ReferenceId", Ticket.Id }, { "RemarksType", (int)RemarksTypeEnum.Feedback } },
           new DialogOptions() { Width = "40%", CloseDialogOnOverlayClick = false });
            await LoadBugTicketFeedBack();
            await PopulateHistory();
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion
        #region On Select Mention Value
        public async Task OnValueSelectingHandler(MentionValueSelectedEventArgs<UserListMentionVM> args)
        {
            if (!string.IsNullOrEmpty(args.ItemData.UserId) && !bugTicketComment.MentionedUser.ContainsKey(args.ItemData.UserId))
            {
                bugTicketComment.MentionedUser.Add(args.ItemData.UserId, Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? args.ItemData.UserFullNameEn : args.ItemData.UserFullNameAr);
            }
        }
        #endregion
        #region Edit Comment
        protected async Task EditComment(BugTicketCommentVM args)
        {
            isShowEditor = false;
            editingCommentId = args.Id;
            commentValue = args.Comment;
            IsShowCommentActions = true;
        }
        #endregion

        #region Add Comment Reply
        protected async Task AddReply(Guid ParentCommentId)
        {
            try
            {
                if (string.IsNullOrEmpty(commentValue) || Regex.IsMatch(commentValue, RegexPatterns.specificEmptyContentRichTextEditorPattern))
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Please_Add_Comment"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
                else
                {
                    spinnerService.Show();
                    ApiCallResponse response = new ApiCallResponse();
                    bugTicketComment.Id = Guid.NewGuid();
                    bugTicketComment.ParentCommentId = ParentCommentId;
                    bugTicketComment.ReferenceId = Ticket.Id;
                    bugTicketComment.Comment = commentValue;
                    bugTicketComment.RemarkType = (int)RemarksTypeEnum.Comment;
                    foreach (var item in bugTicketComment.MentionedUser)
                    {
                        if (!commentValue.Contains(item.Value))
                        {
                            bugTicketComment.MentionedUser.Remove(item.Key);
                        }
                    }
                    response = await bugReportingService.AddCommentFeedBack(bugTicketComment);

                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate(editingCommentId == Guid.Empty ? "Comment_Added_Successfully" : "Comment_Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await LoadBugTicketComments();
                        await PopulateHistory();
                        commentValue = string.Empty;
                        bugTicketComment = new BugCommentFeedBack();
                        IsShowCommentActions = false;
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    isShowEditor = false;
                    replyToCommentId = Guid.Empty;
                    spinnerService.Hide();
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

        #region Reply Button Events
        private void ShowReplyBox(Guid commentId)
        {
            replyToCommentId = commentId;
            commentValue = string.Empty;
            IsShowCommentActions = true;
        }
        private void CancelReply()
        {
            replyToCommentId = Guid.Empty;
            commentValue = string.Empty;
            IsShowCommentActions = false;
        }
        #endregion

        #region Delete Button
        protected async Task DeleteComment(BugTicketCommentVM args)
        {
            try
            {

                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Delete_Bug_Ticket_Comments"), translationState.Translate("delete"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    args.TicketStatusId = (int)Ticket.StatusId;
                    var response = await bugReportingService.DeleteComment(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await LoadBugTicketComments();
                        await PopulateHistory();
                        StateHasChanged();
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

        #region Ticket Tagging and Assignment
        protected async Task TicketAssigmentAndTagging()
        {
            try
            {
                await dialogService.OpenAsync<TagTicketBug>(
                translationState.Translate("Ticket_Assignment"),
                new Dictionary<string, object>() { { "TicketId", Ticket.Id }, { "BugId", Ticket.BugId } },
                new DialogOptions() { Width = "50%", Height = "85%", CloseDialogOnOverlayClick = false });
                await Load();
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

        #region Open Close Ticket
        protected async Task ReOpenCloseTicket()
        {
            try
            {
                var dialogResponse = await dialogService.OpenAsync<DecisionStatusPopup>(translationState.Translate("Reopen_Close_Ticket"),
                new Dictionary<string, object>() { { "ReferenceId", Ticket.Id }, { "DecisionFrom", (int)DecisionFromOptionEnum.FromTicket } },
                new DialogOptions() { Width = "60%", CloseDialogOnOverlayClick = false });
                if (dialogResponse == true)
                {
                    await Load();
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

        #region Accept Ticket
        protected async Task AcceptTicket()
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                         translationState.Translate("Sure_Submit"),
                         translationState.Translate("Confirm"),
                         new ConfirmOptions()
                         {
                             OkButtonText = @translationState.Translate("Yes"),
                             CancelButtonText = @translationState.Translate("No")
                         });

                if (dialogResponse != null)
                {
                    if ((bool)dialogResponse)
                    {
                        decisionStatus.ReferenceId = Ticket.Id;
                        decisionStatus.StatusId = (int)BugStatusEnum.InProgress;
                        var response = await bugReportingService.UpdateTicketStatus(decisionStatus);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Ticket_Accepted_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            await Load();
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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

        #region Reject Ticket
        protected async Task RejectTicket()
        {
            try
            {
                var dialogResponse = await dialogService.OpenAsync<DecisionStatusPopup>(translationState.Translate("Reject_Ticket"),
                new Dictionary<string, object>() { { "ReferenceId", Ticket.Id }, { "Status", (int)BugStatusEnum.Rejected }, { "DecisionFrom", (int)DecisionFromOptionEnum.FromTicket } },
                new DialogOptions() { Width = "40%", CloseDialogOnOverlayClick = false });
                if (dialogResponse == true)
                {
                    await Load();
                    await RedirectBack();
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

        #region Add Resolution
        protected async Task AddResolution()
        {
            try
            {
                var dialogResponse = await dialogService.OpenAsync<DecisionStatusPopup>(translationState.Translate("Ticket_Resolution"),
                new Dictionary<string, object>() { { "ReferenceId", Ticket.Id }, { "Status", (int)BugStatusEnum.Resolved }, { "DecisionFrom", (int)DecisionFromOptionEnum.FromTicket } },
                new DialogOptions() { Width = "70%", CloseDialogOnOverlayClick = false });
                if (dialogResponse == true)
                {
                    await Load();
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

        #region Add Bug Resolution
        protected async Task AddBugResolution(ReportedBugDetailVM args)
        {
            try
            {
                var dialogResponse = await dialogService.OpenAsync<DecisionStatusPopup>(translationState.Translate("Ticket_Resolution"),
                new Dictionary<string, object>() { { "ReferenceId", args.Id }, { "Status", (int)BugStatusEnum.Resolved }, { "DecisionFrom", (int)DecisionFromOptionEnum.FromBug } },
                new DialogOptions() { Width = "65%", CloseDialogOnOverlayClick = false });
                if (dialogResponse == true)
                {
                    await Load();
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

        #region Redirect Back
        protected async Task RedirectBack()
        {
            await jSRuntime.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

        #region View Bug Detail
        protected async Task ViewBug(ReportedBugDetailVM bug)
        {
            navigationManager.NavigateTo("/reportedbug-view/" + bug.Id);
        }
        #endregion
    }
}
