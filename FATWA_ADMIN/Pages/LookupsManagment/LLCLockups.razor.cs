using FATWA_ADMIN.Services.General;
using FATWA_ADMIN.Services.UserManagement;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.GeneralEnums;


namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class LLCLockups : ComponentBase
    {

        #region Service Injection

        [Inject]
        protected RoleService roleService { get; set; }
        #endregion

        #region Variable Declaration

        protected bool isLoading { get; set; }
        protected int count { get; set; }
        protected bool isCheckedRole = false;

        protected RadzenDataGrid<ClaimVM>? grid = new RadzenDataGrid<ClaimVM>();
        protected RadzenDataGrid<LmsLiteratureTagVM>? gridLmsLiteratureTag = new RadzenDataGrid<LmsLiteratureTagVM>();
        protected RadzenDataGrid<LookupsHistory>? gridLookupHistoryVM = new RadzenDataGrid<LookupsHistory>();
        protected RadzenDataGrid<BookAuthorVM>? BookAuthorGrid = new RadzenDataGrid<BookAuthorVM>();
        protected RadzenDataGrid<LiteratureDeweyNumberPatternVM>? LiteratureDeweyNumberPatternGrid = new RadzenDataGrid<LiteratureDeweyNumberPatternVM>();
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        protected string search { get; set; }

        string _searchHistory;
        protected string searchHistory
        {
            get
            {
                return _searchHistory;
            }
            set
            {
                if (!object.Equals(_searchHistory, value))
                {
                    var args = new FATWA_ADMIN.Services.General.PropertyChangedEventArgs() { Name = "searchHistory", NewValue = value, OldValue = _searchHistory };
                    _searchHistory = value;

                    Reload();
                }
            }
        }

        IEnumerable<LmsLiteratureTagVM> _lmsLiteratureTagVM;
        protected IEnumerable<LmsLiteratureTagVM> FilteredlmsLiteratureTagVM { get; set; } = new List<LmsLiteratureTagVM>();
        protected IEnumerable<LmsLiteratureTagVM> lmsLiteratureTagVM
        {
            get
            {
                return _lmsLiteratureTagVM;
            }
            set
            {
                if (!Equals(_lmsLiteratureTagVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "SubTypeVM", NewValue = value, OldValue = _lmsLiteratureTagVM };
                    _lmsLiteratureTagVM = value;
                    Reload();
                }
            }
        }

        IEnumerable<LookupsHistory> _LookupHistoryVM;
        protected IEnumerable<LookupsHistory> LookupHistoryVM
        {
            get
            {
                return _LookupHistoryVM;
            }
            set
            {
                if (!Equals(_LookupHistoryVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "SubTypeVM", NewValue = value, OldValue = _LookupHistoryVM };
                    _LookupHistoryVM = value;
                    Reload();
                }
            }
        }
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion

        #region Fuctions
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();

        }

        protected async Task Load()
        {
            await GetLegallmsLiteratureTaglist();
            await GetBookAuthorList();
            await GetLiteratureDeweyNumberPatternsList();
        }
        #endregion

        #region  Book Author List 
        IEnumerable<BookAuthorVM> _BookAuthor;
        protected IEnumerable<BookAuthorVM> FilteredBookAuthor { get; set; } = new List<BookAuthorVM>();
        protected IEnumerable<BookAuthorVM> BookAuthor
        {
            get
            {
                return _BookAuthor;
            }
            set
            {
                if (!Equals(_BookAuthor, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "BookAuthor", NewValue = value, OldValue = _BookAuthor };
                    _BookAuthor = value;

                    Reload();
                }
            }
        }
        protected async Task GetBookAuthorList()
        {
            var result = await lookupService.GetBookAuthorList();
            if (result.IsSuccessStatusCode)
            {
                BookAuthor = (IEnumerable<BookAuthorVM>)result.ResultData;
                FilteredBookAuthor = (IEnumerable<BookAuthorVM>)result.ResultData;
                count = BookAuthor.Count();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
            await InvokeAsync(StateHasChanged);
        }
        protected async Task OnSearchBookAuthor(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredBookAuthor = await gridSearchExtension.Filter(BookAuthor, new Query()
                    {
                        Filter = $@"i => ((i.Address_En != null && i.Address_En.ToLower().Contains(@0)) || (i.Address_Ar != null && i.Address_Ar.ToLower().Contains(@1)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@2)) || (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@3)) || (i.CreatedDate.HasValue != null && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@4)) || (i.FullName_Ar != null && i.FullName_Ar.ToLower().Contains(@5)) || (i.FullName_En != null && i.FullName_En.ToLower().Contains(@6)))",
                        FilterParameters = new object[] { search, search, search, search, search, search, search }
                    }); await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
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

        #region  Literature Dewey Number Patterns List 
        IEnumerable<LiteratureDeweyNumberPatternVM> _LiteratureDeweyNumberPatterns;
        protected IEnumerable<LiteratureDeweyNumberPatternVM> FilteredLiteratureDeweyNumberPatterns { get; set; } = new List<LiteratureDeweyNumberPatternVM>();
        protected IEnumerable<LiteratureDeweyNumberPatternVM> LiteratureDeweyNumberPatterns
        {
            get
            {
                return _LiteratureDeweyNumberPatterns;
            }
            set
            {
                if (!Equals(_LiteratureDeweyNumberPatterns, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "LiteratureDeweyNumberPatterns", NewValue = value, OldValue = _LiteratureDeweyNumberPatterns };
                    _LiteratureDeweyNumberPatterns = value;

                    Reload();
                }
            }
        }
        protected async Task GetLiteratureDeweyNumberPatternsList()
        {
            var result = await lookupService.GetLiteratureDeweyNumberPatternsList();
            if (result.IsSuccessStatusCode)
            {
                LiteratureDeweyNumberPatterns = (IEnumerable<LiteratureDeweyNumberPatternVM>)result.ResultData;
                FilteredLiteratureDeweyNumberPatterns = (IEnumerable<LiteratureDeweyNumberPatternVM>)result.ResultData;
                count = LiteratureDeweyNumberPatterns.Count();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
            await InvokeAsync(StateHasChanged);
        }
        protected async Task OnSearchLiteratureDeweyNumberPatterns(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredLiteratureDeweyNumberPatterns = await gridSearchExtension.Filter(LiteratureDeweyNumberPatterns, new Query()
                    {
                        Filter = $@"i => ((i.SeriesNumber != null && i.SeriesNumber.ToLower().Contains(@0)) ||(i.literatureDeweyNumberPatternType != null && i.literatureDeweyNumberPatternType.ToString().ToLower().Contains(@1))|| (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@2)) || (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@3)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@4)))",
                        FilterParameters = new object[] { search, search, search, search, search }
                    });
                    await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
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

        #region Literature Tag History
        IEnumerable<LookupHistoryVM> _lookupHistoryVM;
        protected IEnumerable<LookupHistoryVM> lookupHistoryVM
        {
            get
            {
                return _lookupHistoryVM;
            }
            set
            {
                if (!Equals(_lookupHistoryVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "lmsLiteratureTagVM", NewValue = value, OldValue = _lookupHistoryVM };
                    _lookupHistoryVM = value;

                    Reload();
                }
            }
        }
        protected async Task GetLegallmsLiteratureTaglist()
        {
            var result = await lookupService.GetLmsLiteratureTags();
            if (result.IsSuccessStatusCode)
            {
                lmsLiteratureTagVM = (IEnumerable<LmsLiteratureTagVM>)result.ResultData;
                FilteredlmsLiteratureTagVM = (IEnumerable<LmsLiteratureTagVM>)result.ResultData;
                count = lmsLiteratureTagVM.Count();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
            await InvokeAsync(StateHasChanged);
        }
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredlmsLiteratureTagVM = await gridSearchExtension.Filter(lmsLiteratureTagVM, new Query()
                    {
                        Filter = $@"i => ((i.Description != null && i.Description.ToLower().Contains(@0)) ||(i.TagNo != null && i.TagNo.ToLower().Contains(@1))|| (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").ToLower().Contains(@2))|| (i.UserFullNameAr != null && i.UserFullNameAr.ToLower().Contains(@3)) || (i.UserFullNameEn != null && i.UserFullNameEn.ToLower().Contains(@4)))",
                        FilterParameters = new object[] { search, search, search, search, search }
                    }); await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
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

        #region redirect events

        protected async Task ExpandDraftVersionsLiteratureTag(LmsLiteratureTagVM lmsLiteratureTagVM)
        {
            await GetLookupHistoryListByRefernceId(lmsLiteratureTagVM.Id, (int)LookupsTablesEnum.LMS_LITERATURE_TAG);
        }

        protected async Task GetLookupHistoryListByRefernceId(int Id, int LookupstableId)
        {
            var result = await lookupService.GetLookupHistoryListByRefernceId(Id, LookupstableId);

            LookupHistoryVM = (IEnumerable<LookupsHistory>)result.ResultData;

            await InvokeAsync(StateHasChanged);
        }

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

        #endregion

        #region Book Tag Action Button Detial
        #region Add Book/Literature Tag
        protected async Task ButtonLiteratureTagClick(MouseEventArgs args)
        {
            try
            {
                if (await dialogService.OpenAsync<LiteratureTagsAdd>(
                     translationState.Translate("Add_Literature_Tag"),
                    null,
                    new DialogOptions()
                    {
                        CloseDialogOnOverlayClick = false,
                        CloseDialogOnEsc = false,
                        Width = "30%"
                    }) == true)
                {
                    await Task.Delay(100);
                    await GetLegallmsLiteratureTaglist();
                }
                StateHasChanged();
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
        #endregion
        #region Edit Book/Literature Tag
        protected async Task EditLiteratureTagItem(LmsLiteratureTagVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<LiteratureTagsAdd>(
                translationState.Translate("Edit_Literature_Tag"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false,
                    Width = "30%"
                }) == true)
                {
                    await Task.Delay(100);
                    await GetLegallmsLiteratureTaglist();
                }
                StateHasChanged();
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
        #endregion
        #region Delete Book/Literature Tag
        protected async Task DeleteLiteratureTagItem(LmsLiteratureTagVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Delete_Literature_Tag"),
                translationState.Translate("Delete"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await lookupService.DeleteLiteratureTags(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetLegallmsLiteratureTaglist();
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
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("An_unexpected_error_occurred") + $": {response.StatusCode}",
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
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
                    Detail = translationState.Translate("An_unexpected_error_occurred") + ": " + ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }
        #endregion
        #region Active Book/Literature Tag
        protected async Task GridActiveLiteratureTagClick1(LmsLiteratureTagVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_you_want_to_update_Status"),
                translationState.Translate("Status"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    args.IsActive = args.IsActive ? false : true;

                    spinnerService.Show();
                    var response = await lookupService.ActiveLiteratureTags(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetLegallmsLiteratureTaglist();
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
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("An_unexpected_error_occurred") + $": {response.StatusCode}",
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
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
                    Detail = translationState.Translate("An_unexpected_error_occurred") + ": " + ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
        #endregion

        #region  Book Author  Action Button Detail
        #region Add Book Author   
        protected async Task BookAuthorAdd(MouseEventArgs args)
        {
            try
            {
                if (await dialogService.OpenAsync<BookAuthorDetailAdd>(
                    translationState.Translate("Add_Book_Author"),
                    null,
                    new DialogOptions()
                    {
                        CloseDialogOnOverlayClick = false,
                        CloseDialogOnEsc = false
                    }) == true)
                {
                    await Task.Delay(100);
                    await GetBookAuthorList();
                }
                StateHasChanged();
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
        #endregion
        #region Edit BookAuthor
        protected async Task EditBookAuthor(BookAuthorVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<BookAuthorDetailAdd>(
                translationState.Translate("Edit_Book_Author"),
                new Dictionary<string, object>() { { "AuthorId", args.AuthorId } },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false
                }) == true)
                {
                    await Task.Delay(100);
                    await GetBookAuthorList();
                }

                StateHasChanged();
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
        #endregion
        #region Delete Book Author
        protected async Task DeleteBookAuthor(BookAuthorVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Delete_book_Author?"),
                translationState.Translate("Delete"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await lookupService.DeleteBookAuthor(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetBookAuthorList();
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
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("An_unexpected_error_occurred") + $": {response.StatusCode}",
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
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
                    Detail = translationState.Translate("An_unexpected_error_occurred") + ": " + ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
        #endregion

        #region  Literature Dewey Number Patterns  Action Button Detail
        #region Add Literature Dewey Number Pattern  
        protected async Task LiteratureDeweyNumberPatternsAdd(MouseEventArgs args)
        {
            try
            {
                if (await dialogService.OpenAsync<LiteratureDeweyNumberPatternAdd>(
                    translationState.Translate("Add_Literature_Dewey_Number_Pattern"),
                    null,
                    new DialogOptions()
                    {
                        CloseDialogOnOverlayClick = false,
                        CloseDialogOnEsc = false
                    }) == true)
                {
                    await Task.Delay(100);
                    await GetLiteratureDeweyNumberPatternsList();
                }
                StateHasChanged();
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
        #endregion
        #region Edit Literature Dewey Number Pattern
        protected async Task EditLiteratureDeweyNumberPatterns(LiteratureDeweyNumberPatternVM args)
        {
            try
            {
                if (await dialogService.OpenAsync<LiteratureDeweyNumberPatternAdd>(
                translationState.Translate("Edit_Literature_Dewey_Number_Pattern"),
                new Dictionary<string, object>() { { "Id", args.Id } },
                new DialogOptions()
                {
                    CloseDialogOnOverlayClick = false,
                    CloseDialogOnEsc = false
                }) == true)
                {
                    await Task.Delay(100);
                    await GetLiteratureDeweyNumberPatternsList();
                }
                StateHasChanged();
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
        #endregion
        #region Delete Literature Dewey Number Patterns
        protected async Task DeleteLiteratureDeweyNumberPattern(LiteratureDeweyNumberPatternVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Delete_Literature_Dewey_Number_Pattern"),
                translationState.Translate("Delete"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await lookupService.DeleteLiteratureDeweyNumberPattern(args);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await GetLiteratureDeweyNumberPatternsList();
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
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("An_unexpected_error_occurred") + $": {response.StatusCode}",
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("An_unexpected_error_occurred") + ": " + ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }


        }
        #endregion
        #endregion

    }
}
