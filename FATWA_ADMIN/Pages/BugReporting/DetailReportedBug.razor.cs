using DocumentFormat.OpenXml.Spreadsheet;
using FATWA_ADMIN.Data;
using FATWA_DOMAIN.Models.BugReporting;
using FATWA_DOMAIN.Models.ViewModel.BugReportingVMs;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.RichTextEditor;
using System.Text.RegularExpressions;
using static FATWA_DOMAIN.Enums.BugReporting.BugReportingEnum;
using static FATWA_GENERAL.Helper.Response;


namespace FATWA_ADMIN.Pages.BugReporting
{
    public partial class DetailReportedBug : ComponentBase
    {
        #region Parameters
        [Parameter]
        public string BugId { get; set; }
        [Parameter]
        public bool IsCrashReport { get; set; }
        #endregion

        #region Variable 
        public ReportedBugDetailVM detailReportedBug { get; set; } = new ReportedBugDetailVM();
        protected List<BugTicketCommentVM> bugTicketComments { get; set; } = new List<BugTicketCommentVM>();
        protected List<BugTicketCommentVM> bugTicketFeedBack { get; set; } = new List<BugTicketCommentVM>();
        public string commentValue { get; set; } = "";
        public BugCommentFeedBack bugComment { get; set; } = new BugCommentFeedBack();
        public bool isShowEditor { get; set; } = false;
        public Guid editingCommentId = Guid.Empty;
        public Guid replyToCommentId = Guid.Empty;
        protected List<TicketStatusHistoryVM> bugStatusHistories { get; set; } = new List<TicketStatusHistoryVM>();
        IEnumerable<TicketListVM> getTicket = new List<TicketListVM>();
        protected List<BugTicketCommentVM> bugResolution { get; set; } = new List<BugTicketCommentVM>();
        protected RadzenDataGrid<BugTicketCommentVM> feedbackGrid = new RadzenDataGrid<BugTicketCommentVM>();
        protected RadzenDataGrid<TicketStatusHistoryVM> historyGrid = new RadzenDataGrid<TicketStatusHistoryVM>();
        protected RadzenDataGrid<TicketListVM> ticketGrid = new RadzenDataGrid<TicketListVM>();

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
            translationState.TranslateGridFilterLabels(ticketGrid);
            spinnerService.Hide();
        }
        #endregion

        #region Load
        protected async Task Load()
        {
            await GetReportedBugDetail();
            await LoadBugComments();
            await LoadBugFeedBack();
            await PopulateHistory();
            await PopulateTicketListByBugId();
            await LoadBugResolution();
        }
        #endregion

        #region Populate Data
        public async Task PopulateHistory()
        {
            try
            {
                var response = await bugReportingService.GetTicketStatusHistoryById(Guid.Parse(BugId));
                if (response.IsSuccessStatusCode)
                {
                    bugStatusHistories = (List<TicketStatusHistoryVM>)response.ResultData;
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
        protected async Task GetReportedBugDetail()
        {
            try
            {
                var response = await bugReportingService.GetReportedBugDetail(Guid.Parse(BugId));
                if (response.IsSuccessStatusCode)
                {
                    detailReportedBug = (ReportedBugDetailVM)response.ResultData;
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
        protected async Task LoadBugComments()
        {
            try
            {
                var response = await bugReportingService.GetBugTicketCommentFeedBack(Guid.Parse(BugId), (int)RemarksTypeEnum.Comment);
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
        protected async Task LoadBugFeedBack()
        {
            try
            {
                var response = await bugReportingService.GetBugTicketCommentFeedBack(Guid.Parse(BugId), (int)RemarksTypeEnum.Feedback);
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
        protected async Task PopulateTicketListByBugId()
        {
            try
            {
                var response = await bugReportingService.GetTicketsListByBugId(Guid.Parse(BugId));
                if (response.IsSuccessStatusCode)
                {
                    getTicket = (IEnumerable<TicketListVM>)response.ResultData;
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
        #region LoadBugTicketResolution
        protected async Task LoadBugResolution()
        {
            try
            {
                var response = await bugReportingService.GetBugTicketCommentFeedBack(Guid.Parse(BugId), (int)RemarksTypeEnum.Resolution);
                if (response.IsSuccessStatusCode)
                {
                    bugResolution = (List<BugTicketCommentVM>)response.ResultData;
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
        #endregion

        #region Button Click Event
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

        #region Add Comment Click Event

        protected async Task OnClickAddComment()
        {
            commentValue = string.Empty;
            isShowEditor = true;
            editingCommentId = Guid.Empty;
            replyToCommentId = Guid.Empty;
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
                    if (editingCommentId == Guid.Empty)
                    {
                        bugComment.Id = Guid.NewGuid();
                        bugComment.ReferenceId = detailReportedBug.Id;
                        bugComment.Comment = commentValue;
                        bugComment.RemarkType = (int)RemarksTypeEnum.Comment;
                        response = await bugReportingService.AddCommentFeedBack(bugComment);
                    }
                    else
                    {
                        bugComment.Comment = commentValue;
                        bugComment.Id = editingCommentId;
                        response = await bugReportingService.UpdateCommentFeedBack(bugComment);
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
                        await LoadBugComments();
                        commentValue = string.Empty;
                        bugComment = new BugCommentFeedBack();
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
        }

        #endregion

        #region Edit Comment
        protected async Task EditComment(BugTicketCommentVM args)
        {
            isShowEditor = false;
            editingCommentId = args.Id;
            commentValue = args.Comment;
        }
        #endregion

        #region Add Comment Reply
        protected async Task AddReply(Guid ParentCommentId)
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
                    bugComment.Id = Guid.NewGuid();
                    bugComment.ParentCommentId = ParentCommentId;
                    bugComment.ReferenceId = detailReportedBug.Id;
                    bugComment.Comment = commentValue;
                    bugComment.RemarkType = (int)RemarksTypeEnum.Comment;
                    response = await bugReportingService.AddCommentFeedBack(bugComment);

                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate(editingCommentId == Guid.Empty ? "Comment_Added_Successfully" : "Comment_Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await LoadBugComments();
                        commentValue = string.Empty;
                        bugComment = new BugCommentFeedBack();
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
        }
        private void CancelReply()
        {
            replyToCommentId = Guid.Empty;
            commentValue = string.Empty;
        }
        #endregion


        #region Add Comment/Feedback
        protected async Task AddFeedBack()
        {
            await dialogService.OpenAsync<AddCommentFeedBack>(
           translationState.Translate("FeedBack"),
           new Dictionary<string, object>() { { "ReferenceId", detailReportedBug.Id }, { "RemarksType", (int)RemarksTypeEnum.Feedback } },
           new DialogOptions() { Width = "40%", CloseDialogOnOverlayClick = false });
            await LoadBugFeedBack();
        }
        #endregion

        #region Delete Comment
        protected async Task DeleteComment(BugTicketCommentVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Delete_Bug_Comment"), translationState.Translate("Delete"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    var response = await bugReportingService.DeleteComment(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await LoadBugComments();
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

        #region View Ticket Detail
        protected async Task ViewBugTicket(TicketListVM ticket)
        {
            navigationManager.NavigateTo("/bugticket-view/" + ticket.Id + "/" + true);
        }
        #endregion
    }
}
