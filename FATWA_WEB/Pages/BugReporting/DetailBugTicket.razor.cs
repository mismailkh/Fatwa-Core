using FATWA_DOMAIN.Models.BugReporting;
using FATWA_DOMAIN.Models.ViewModel.BugReportingVMs;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.RichTextEditor;
using System.Text.RegularExpressions;
using static FATWA_DOMAIN.Enums.BugReporting.BugReportingEnum;
using static FATWA_GENERAL.Helper.Response;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Syncfusion.Blazor.DropDowns;
using Microsoft.JSInterop;
using FATWA_WEB.Extensions;
using FATWA_DOMAIN.Enums;
using FATWA_WEB.Services.General;
using DocumentFormat.OpenXml.Office2019.Excel.ThreadedComments;
using FATWA_WEB.Data;

namespace FATWA_WEB.Pages.BugReporting
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
        protected List<BugTicketCommentVM> bugTicketResolution { get; set; } = new List<BugTicketCommentVM>();
        public bool FromTicketFlag { get { return Convert.ToBoolean(FromTicket); } set { FromTicketFlag = value; } }
        public string commentValue { get; set; } = "";
        public BugCommentFeedBack bugTicketComment { get; set; } = new BugCommentFeedBack();
        public bool isShowEditor { get; set; } = false;
        public Guid editingCommentId = Guid.Empty;
        public Guid replyToCommentId = Guid.Empty;
        protected RadzenDataGrid<BugTicketCommentVM> feedbackGrid = new RadzenDataGrid<BugTicketCommentVM>();
        protected RadzenDataGrid<TicketStatusHistoryVM> historyGrid = new RadzenDataGrid<TicketStatusHistoryVM>();
        protected RadzenDataGrid<BugTicketCommentVM> reasonGrid = new RadzenDataGrid<BugTicketCommentVM>();
        public bool IsShowCommentActions { get; set; } = false;
        protected IEnumerable<UserListMentionVM> users = new List<UserListMentionVM>();
        public SfRichTextEditor editor { get; set; } = new SfRichTextEditor();
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
            await JsInterop.InitilizeMentionUserComponent(DotNetObjectReference.Create(this));

            translationState.TranslateGridFilterLabels(feedbackGrid);
            translationState.TranslateGridFilterLabels(historyGrid);
            translationState.TranslateGridFilterLabels(reasonGrid);
            spinnerService.Hide();
        }
        #endregion

        #region Load
        protected async Task Load()
        {
            await GetBugTicketDetail();
            await PopulateHistory();
            await LoadBugTicketComments();
            await LoadBugTicketFeedBack();
            await LoadBugTicketReopenReason();
            await LoadBugTicketResolution();
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

        #region Get Bug Ticket Details
        protected async Task GetBugTicketDetail()
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
        #endregion

        #region PopulateHistory
        public async Task PopulateHistory()
        {
            var response = await bugReportingService.GetTicketStatusHistoryById(Guid.Parse(TicketId));
            if (response.IsSuccessStatusCode)
            {
                ticketStatusHistories = (List<TicketStatusHistoryVM>)response.ResultData;
            }
        }
        #endregion

        #region LoadBugTicketReason
        protected async Task LoadBugTicketReopenReason()
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
        #endregion

        #region Load FeedBack 
        protected async Task LoadBugTicketFeedBack()
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
        #endregion

        #region LoadBugTicketComments
        protected async Task LoadBugTicketComments()
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
        #endregion

        #region LoadBugTicketResolution
        protected async Task LoadBugTicketResolution()
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
        #endregion

        #region Button Click Event
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

        #region Open Close Ticket
        protected async Task ReOpenCloseTicket()
        {
            try
            {
                await dialogService.OpenAsync<DecisionStatusPopup>(translationState.Translate("Reopen_Close_Ticket"),
                new Dictionary<string, object>() { { "ReferenceId", Ticket.Id } },
                new DialogOptions() { Width = "40%", CloseDialogOnOverlayClick = true });
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

        #region Add Feedback
        protected async Task AddFeedBack()
        {
            await dialogService.OpenAsync<AddCommentFeedBack>(
           translationState.Translate("FeedBack"),
           new Dictionary<string, object>() { { "ReferenceId", Ticket.Id }, { "RemarksType", (int)RemarksTypeEnum.Feedback } },
           new DialogOptions() { Width = "40%", CloseDialogOnOverlayClick = true });
            await LoadBugTicketFeedBack();
            await PopulateHistory();

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

        #region Add Comment
        protected async Task AddComment()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(commentValue) || Regex.IsMatch(commentValue, RegexPatterns.specificEmptyContentRichTextEditorPattern))
                {
                    if (!string.IsNullOrEmpty(commentValue))
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
        #endregion

        #region Add Comment Reply
        protected async Task AddReply(Guid ParentCommentId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(commentValue) || Regex.IsMatch(commentValue, RegexPatterns.specificEmptyContentRichTextEditorPattern))
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

        #region Edit Comment
        protected async Task EditComment(BugTicketCommentVM args)
        {
            isShowEditor = false;
            editingCommentId = args.Id;
            commentValue = args.Comment;
            IsShowCommentActions = true;
        }
        #endregion

        #region Reply Events
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
                    StateHasChanged();
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
                }
            }
        }
        #endregion

        #region Button Cancel
        protected async Task OnCancel()
        {
            isShowEditor = false;
            commentValue = string.Empty;
            editingCommentId = Guid.Empty;
            IsShowCommentActions = false;
        }
        #endregion


        #region Get Mention User Detail
        [JSInvokable]
        public async Task GetMentionUserDetailById(string id)
        {
            try
            {
                await PopulateUserDetailById(id);
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
        #region Populate User Detail
        protected async Task PopulateUserDetailById(string UserId)
        {
            try
            {
                var response = await userService.GetEmployeeDetailById(Guid.Parse(UserId));
                if (response.IsSuccessStatusCode)
                {
                    userDetail = (AddEmployeeVM)response.ResultData;
                    isVisible = !isVisible;
                    StateHasChanged();
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
    }
}
