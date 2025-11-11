using FATWA_DOMAIN.Models.ViewModel.Lms;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using System.Globalization;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.LiteratureEnum;
using static FATWA_DOMAIN.Enums.UserEnum;

namespace FATWA_WEB.Pages.Lms
{
    public partial class LiteratureBorrowReturnList : ComponentBase
    {
        #region Parameter
        public string? UserId { get; set; }
        #endregion

        #region Varriables

        public string? CivilId { get; set; }
        public bool IsSearch { get; set; } = false;
        protected RadzenDataGrid<BorrowedLiteratureVM>? grid0 = new();
        public bool allowRowSelectOnRowClick;
        public RadzenDropDown<string> userDropDown = new();
        public bool IsEnabled { get; set; }
        public string Barcode { get; set; }
        public string imagebase64 { get; set; }
        public bool isStockTakingInPrgoress { get; set; }

        public UserAndLiteratureVM? LiteratureAndUserDetail { get; set; }
        IList<BorrowedLiteratureVM> FilteredBorrowedLiterature { get; set; } = new List<BorrowedLiteratureVM>();
        bool? IsReturn { get; set; }
        string _search;
        public IEnumerable<AllLmsUserDetailVM> getUserDetails { get; set; }
        public string Token { get; set; } = "";

        #endregion


        #region On Intialize
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            Token = await BrowserStorage.GetItemAsync<string>("Token");
            await CheckAnyInProgressStockTaking();
            translationState.TranslateGridFilterLabels(grid0);

            await FetchUsers();
            spinnerService.Hide();

        }

        #endregion
        #region On Change Event
        public async Task OnChangeUser()
        {
            if (UserId != null && LiteratureAndUserDetail?.UserDetail?.FirstOrDefault()?.UserId != UserId)
            {
                CivilId = "";
            }
        }
        #endregion

        #region Get All Lms User
        protected async Task FetchUsers()
        {
            var getUserDetail = await lmsLiteratureBorrowDetailService.GetAllLmsUserList();

            if (getUserDetail.IsSuccessStatusCode)
            {
                getUserDetails = (List<AllLmsUserDetailVM>)getUserDetail.ResultData;
                if (getUserDetails.Count() > 0)
                { 
                    getUserDetails = getUserDetails
                   .OrderBy(user => Thread.CurrentThread.CurrentUICulture.Name == "en-US"
                       ? user.FullNameEnglish
                       : user.FullNameArabic,
                       StringComparer.Create(new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? "en-US" : "ar-KW"), false))
                   .ToList();
                }

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(getUserDetail);
            }
        }
        #endregion

        #region Check If Any InProgress StockTaking
        protected async Task CheckAnyInProgressStockTaking()
        {
            try
            {
                var response = await lmsLiteratureService.CheckIfAnyInProgressStockTaking();
                if (response.IsSuccessStatusCode)
                {
                    isStockTakingInPrgoress = (bool)response.ResultData;
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

        #region Functions

        #region Search User By Civil Id or User Id 
        protected async Task Search()
        {
            spinnerService.Show();
            if (!(string.IsNullOrEmpty(CivilId)) || !(string.IsNullOrEmpty(UserId)))
            {
                if (!(string.IsNullOrEmpty(CivilId)) && !(string.IsNullOrEmpty(UserId)))
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Please_Select_One_CivilId_Or_Employee_Name"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
                }
                else
                {
                    var res = await lmsLiteratureBorrowDetailService.GetBorrowedLiteratureAndUserDetailByUserIdAndCivilId(CivilId, UserId, Token);
                    if (res.IsSuccessStatusCode)
                    {

                        LiteratureAndUserDetail = (UserAndLiteratureVM)res.ResultData;
                        if (LiteratureAndUserDetail.UserDetail.Count() > 0)
                        {
                            FilteredBorrowedLiterature = LiteratureAndUserDetail.Literature;
                            FilteredBorrowedLiterature = FilteredBorrowedLiterature.Where(item => (item.BorrowReturnApprovalStatus == (int)BorrowReturnApprovalStatus.Default) || (item.BorrowReturnApprovalStatus == (int)BorrowReturnApprovalStatus.Rejected)).ToList();
                            grid0.Reload();
                            IsSearch = true;
                            if (string.IsNullOrEmpty(CivilId))
                            {
                                CivilId = LiteratureAndUserDetail.UserDetail.FirstOrDefault().CivilId;
                            }
                            if (string.IsNullOrEmpty(UserId))
                            {
                                UserId = LiteratureAndUserDetail.UserDetail.FirstOrDefault().UserId;
                            }
                            await GetCivilIDDocument(Guid.Parse(LiteratureAndUserDetail.UserDetail?.FirstOrDefault()?.UserId));
                            await ClearDropDownSearch();
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Invalid_User"),
                                Style = "position: fixed !important; left: 0; margin: auto;"
                            });
                            CivilId = string.Empty;
                        }

                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(res);
                    }
                }
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Please_Select_CivilId_Or_UserId"),
                    Style = "position: fixed !important; left: 0; margin: auto;"
                });
            }
            spinnerService.Hide();
        }
        #endregion

        #region Reset
        protected async Task Reset()
        {
            IsSearch = false;
            CivilId = string.Empty;
            FilteredBorrowedLiterature = new List<BorrowedLiteratureVM>();
            Barcode = string.Empty;
            UserId = string.Empty;
            if (userDropDown != null)
            {
                userDropDown.Reset();
            }
            Reload();
            grid0.Reload();
        }
        #endregion
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(FATWA_WEB.Services.PropertyChangedEventArgs args)
        {
        }
        #region Return and Extend
        protected async Task Return(BorrowedLiteratureVM borrowReturn)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Returned_The_Record"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                spinnerService.Show();
                IsReturn = true;

                borrowReturn.ReturnDate = DateTime.Now;
                borrowReturn.DecisionId = (int)BorrowReturnApprovalStatus.Returned;
                borrowReturn.Extended = false;
                borrowReturn.LoggedInUser = loginState.UserDetail.UserName;
                await Submit(borrowReturn);
                FilteredBorrowedLiterature.Remove(borrowReturn);
                if (LiteratureAndUserDetail.UserDetail.FirstOrDefault().EligibleCount != null)
                {
                    LiteratureAndUserDetail.UserDetail.FirstOrDefault().EligibleCount -= 1;

                }

                grid0.Reload();
                spinnerService.Hide();
            }
            //EditLmsLiteratureBorrowDetail();
        }
        protected async Task Extend(BorrowedLiteratureVM borrowExtension)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Extend_The_Record"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                spinnerService.Show();
                IsReturn = false;

                borrowExtension.ExtensionApprovalStatus = (int)BorrowApprovalStatus.Extended;
                borrowExtension.Extended = true;
                borrowExtension.ExtendDueDate = DateTime.Now.AddDays(7);
                borrowExtension.LoggedInUser = loginState.UserDetail.UserName;
                await Submit(borrowExtension);
                //EditLmsLiteratureBorrowDetail();
                spinnerService.Hide();

            }
        }
        protected async Task Submit(BorrowedLiteratureVM literature)
        {
            try
            {

                var response = await lmsLiteratureBorrowDetailService.UpdateLiteratureReturnExtendDetail(literature);
                if (response.IsSuccessStatusCode)
                {
                    if ((bool)IsReturn)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Return_Success"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Extend_Borrow_Rquest_Success"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                    }

                    Barcode = string.Empty;
                    UserId = (LiteratureAndUserDetail.UserDetail?.FirstOrDefault()?.UserId);
                    CivilId = (LiteratureAndUserDetail.UserDetail?.FirstOrDefault()?.CivilId);

                    Reload();
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
        protected void RowRenderHandler(RowRenderEventArgs<BorrowedLiteratureVM> rowItem)
        {
            if (rowItem.Data.DueDate < DateTime.Now && (bool)!rowItem.Data.Extended && rowItem.Data.DecisionId == (int)BorrowApprovalStatus.Approved && rowItem.Data.ApplyReturnDate == null && rowItem.Data.ReturnDate == null)

            {
                rowItem.Attributes.Add("style", $"background-color: #e3d4c9;");

            }
        }
        #region Go For Barcode and cancel
        public async Task Go()
        {
            try
            {
                if (string.IsNullOrEmpty(Barcode))
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Info,
                        Detail = translationState.Translate("Barcode_Required"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });

                }
                else
                {
                    int borrowEligibility = 0;
                    if (LiteratureAndUserDetail?.UserDetail.FirstOrDefault().DepartmentId == (int)DepartmentEnum.Operational)
                    {
                        borrowEligibility = 3;
                    }
                    else if (LiteratureAndUserDetail?.UserDetail.FirstOrDefault().DepartmentId == (int)DepartmentEnum.Administrative)
                    {
                        borrowEligibility = 2;
                    }

                    if ((int)LiteratureAndUserDetail?.UserDetail.FirstOrDefault().EligibleCount >= borrowEligibility)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Info,
                            Detail = translationState.Translate("Borrow_Book_Limit"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        Barcode = string.Empty;
                    }
                    else
                    {

                        if (await dialogService.OpenAsync<AddBorrowLiteraturePopUp>(
                       translationState.Translate("Add_Borrow_Literature"),
                       new Dictionary<string, object>() {
                       { "Barcode", Barcode },
                       {"Id",(LiteratureAndUserDetail.UserDetail?.FirstOrDefault()?.UserId) },
                       {"BookReturnDuration",LiteratureAndUserDetail.UserDetail.FirstOrDefault().BookReturnDuration},
                       { "FromBarcode", true }
                       },
                       new DialogOptions()
                       {
                           Width = "70%",

                           CloseDialogOnOverlayClick = true,
                           CloseDialogOnEsc = true
                       }) is bool result && result == true)
                        {
                            CivilId = string.Empty;
                            LiteratureAndUserDetail.UserDetail.FirstOrDefault().EligibleCount += 1;
                            UserId = (LiteratureAndUserDetail.UserDetail?.FirstOrDefault()?.UserId);
                            await Search();
                            Barcode = "";
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Borrow_Success"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            StateHasChanged();
                        }
                        else
                        {
                            Barcode = string.Empty;
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
        protected async Task Cancel()
        {
            Reset();
        }
        #endregion

        #region Search Book PopUp
        public async Task SearchBook()
        {
            try
            {
                int borrowEligibility = 0;
                if (LiteratureAndUserDetail?.UserDetail.FirstOrDefault().DepartmentId == (int)DepartmentEnum.Operational)
                {
                    borrowEligibility = 3;
                }
                else if (LiteratureAndUserDetail?.UserDetail.FirstOrDefault().DepartmentId == (int)DepartmentEnum.Administrative)
                {
                    borrowEligibility = 2;
                }

                if ((int)LiteratureAndUserDetail?.UserDetail.FirstOrDefault().EligibleCount >= borrowEligibility)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Info,
                        Detail = translationState.Translate("Borrow_Book_Limit"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    Barcode = string.Empty;
                }
                else
                {

                    if (await dialogService.OpenAsync<AddBorrowLiteraturePopUp>(
                   translationState.Translate("Add_Borrow_Literature"),
                   new Dictionary<string, object>() {
                       { "Barcode", Barcode },
                       {"Id",(LiteratureAndUserDetail.UserDetail?.FirstOrDefault()?.UserId) },
                       {"BookReturnDuration",LiteratureAndUserDetail.UserDetail.FirstOrDefault().BookReturnDuration},
                       { "FromBarcode", false }
                   },
                   new DialogOptions()
                   {
                       Width = "70%",

                       CloseDialogOnOverlayClick = true,
                       CloseDialogOnEsc = true
                   }) is bool result && result == true)
                    {
                        CivilId = string.Empty;
                        LiteratureAndUserDetail.UserDetail.FirstOrDefault().EligibleCount += 1;
                        UserId = (LiteratureAndUserDetail.UserDetail?.FirstOrDefault()?.UserId);

                        await Search();
                        Barcode = "";
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Borrow_Success"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        StateHasChanged();
                    }
                    else
                    {
                        Barcode = string.Empty;
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

        #region User Borrow History
        protected async Task UserHistory()
        {
            try
            {
                if (await dialogService.OpenAsync<UserLiteratureBorrowHistory>(
                    translationState.Translate("User_Borrow_History"),
                    new Dictionary<string, object>() { { "Id", (LiteratureAndUserDetail.UserDetail?.FirstOrDefault()?.UserId) } },
                    new DialogOptions()
                    {
                        Width = "60%",
                        CloseDialogOnOverlayClick = true,
                        CloseDialogOnEsc = true
                    }) == true)
                {
                    StateHasChanged();
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
        #endregion

        #region Civil Id And Document
        protected async Task GetCivilIDDocument(Guid userId)
        {
            var attachments = await fileUploadService.GetUploadedAttachements(false, 0, userId);
            if (attachments != null && attachments.Any())
            {
                var civilIdDoc = attachments.Where(a => a.AttachmentTypeId == (int)AttachmentTypeEnum.EpCivilId).FirstOrDefault();
                if (civilIdDoc != null && new List<string> { ".jpg", ".png", ".jpeg" }.Contains(civilIdDoc.DocType.ToLower()))
                {
                    string physicalPath;
#if DEBUG
                    {
                        physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + civilIdDoc.StoragePath).Replace(@"\\", @"\");

                    }
#else
                {
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + civilIdDoc.StoragePath).Replace(@"\\", @"\");
                    physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                }
#endif
                    imagebase64 = await DocumentEncryptionService.GetDecryptedDocumentBase64(physicalPath, civilIdDoc.DocType, _config.GetValue<string>("DocumentEncryptionKey"), true);
                }
                else
                {
                    imagebase64 = "iVBORw0KGgoAAAANSUhEUgAAAgAAAAIABAMAAAAGVsnJAAAAIVBMVEUAAAB+fX1+fX1+fX1+fX1+fX1+fX1+fX1+fX1+fX1+fX1I2PRsAAAACnRSTlMAF/ClME+Kb9vEsIrXWQAACWpJREFUeNrs3T1rVEEUBuBzs1+JlbGImkpREW6lVrqVhBBCKhESIZWCIqTSgEZSKSrCVordVrrxY/P+SouEJG7uzH7k3rBz3vf5CYe9Z87MOTNrIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiMo755fWdty931pfnjU/25EGOI73vby4akWzjPk75+IIlBtlGF4X2OUKw0kXQ/nPzrnEPUTcemWsrOYboef4RZO8wgi9uM0Gri5HsvzKXWh2MqO8yApdzjKz32txZyDGG3jNzZiEHmCPQyjGm3lNzpNHB2PqOSqKsjQns+akHtjGR2+bEKib02VyoYWJ3zYF6BxPrP7T0HSYA2jRQAwDij+DgAyD+CLYBgPgjqOHM7ljKujizfUvYVZTgmyUr66AE/XT3BKsoxSdLVD1HKXqpLoWPUZJblqQGSpPm2cgSSnPTEnSQAYizwBWU6IMl57gGIK0F5lCqr5aaLk4g3BHU8B++TeEuSvbXklJH6dJaCQ/XQN6VsI3S7VlCWqhASmMjSzhCuSE4UQVyVoPHRQBpKbCJSvy2VHRQib4looUjnOvAJVTkuqWhjRMIa6EGBrAdjs6iMu8tBVuozB9LQIYKpVAMNlGhFOZnBxdBuoVwMAWwJYEsR4V6058EmhjAlgROpQC2JLCLAkyn4zkq1bMp10IBpi3xHIoQdUnXULFfNt22UISoFOqgGMvBYB1BHE3SGkJIugMzqNw1m2abCCFpj7QRQnI0jHNgU6yBCIaz8SbCKI4E5hBCshtYxDn4adNrE0Ec6+AWwii2Qx2EMWyHMkT57481EENQCDQRQ1AI1BBDsCGeRZz7MYkLOBc/bFqtIc79wfAmYghKwV1E+e8PthHl/0yoizj3V+hyRLnvEGeIYNgM0Aegjjj33TH6ADQQ5X8/rACgGE0AWhjC+6AUfQCaiPJ/JqYAoJgCwBIA+iSoAKAYTQDoK0EFAMVoAkB/HkAfAEMUwahkB1H+Z2ToGyP0rbEtxBDMydG3x+kHJBYxhPdh4RlE+b81NIc49/Py9IOS9KOy9MPS9OPysVqYoRIeNijofkwwVgpSFILDxsXdD4vr4qSuzlqOIPdzoro+rwcU9ISGHlEJLgMsi0BoGaBZBPSUlh5To39Or4FTqHKgntQsyIJUOTDQHWLoCgVrQaY6MHQ0znEkrsfV9by+/mAh+L4+0ev6+pOVgSTAlwKKrg24vyjwj70zeXUiCMJ4jU4UPAUjbifFfU4qLpiTG6i3EHHBkwvicnI/eFJRwdxcEMlJJwpaf6XPjDGTWXq6J/Owa7763QR5PNvpqq++qu6umpds/4SkyRMA8gKKEiFcEtQHF/XJTX10VZ/dnecByBygT2/r4+v6/H76BF37z8pVTAwiTAeWSgFMETAPg7ghcNYlBeqJFqlBVBU4YyOeF7ZIGHFjxMJyYMJpbozdJJEwwv4AiE5jfwBEYYT9ARCd50Z4TVIJRqgaYMY2boD3JJg+YhWQZj2YE5ZnyEuyh2QTjpaMgGJT4IweL8UhEs8jXoJ9JJLgRvoPY67Nr7QE2CxHDzyKTzaSCeIHC8JazOdwNRO7L3BNPmXyyRsSwYXcWP/9BmbCOsmKCKCXt/HDca0AcJJSPJeSFNZHBeMsnVENBTAoGLuJvdeF/4TPJLss7gEwTV+KMLpf0srZ7LgC8Q1Ks1bKsOjVTA6f03NWgIVawvNU0DOUMZuj2v//NBSijjuRaaxvy8g6/j00DR7G3p6cC/plQjahM7bMfwMiMojpia+aeFhVy4eH2YJdJ7M/V4hHsM5itvVixBXER3M/V8jMbDA2V3MJnYqPYNfA6uf6uAmGdvV8cHFkiH5Hu/nSUohRttbQ1DAugfmfT+eFDI6HIwdPK7j8gXMcuN11cNR++SaJhwZNX8Smyyei1F/6ePtUSWklxC1eZ6xqiwnOXrry7NaxO08vnS2LaeFYSr+gb/I1aofs4L6UjtE2s7VbcwWCR1J6hlWDAHtrrUBwU0zPZMjc/AoEN8V0zdYxN78CwU05p8j6XM3kJDkR9uV0zteyDZMBOdDpy5mgtm19xUfImjMRF+BpUbSNbXlr+esGdyWNz7gMQv16SBZsGYsaoDrPLhyvjIXhY1kjdKGr329egvBxJGyI8rR7y+t4l0oIHo+kjdHWmob9eexJwRoE526N5M3RnuZ6xB+fLvzi4ZUTkcRJ6qXGofe/+7hiBqxYAie+vJI6Sr2VPeAluePVMLTgYWovPoD/+AkEY/YC54rA07OR8k5V9tkTJuSG79cFSblg6Bp7ww9ywts7EmTdrrCRPWInWdE+EeQmhtqZA50zof8XZ4q4bLPDnjEgCzwwAjLIPWHvVQh0u2zQz1typN2z85y9w0INemKFZRB5zYQnTojjQ4xtLITdimKfzoT/RagU8KoOcquIPL87W8ge8HQHGPYAxg4w7QGAHFC1B9pcCFbuAZQdULoHUHZA6R6A2QHmPSDqgXWf6wHPzEAna9D3d5REvMTkoRdk4Qu1syPo4Au12Q218UYRCiHTYTIQGVgqBnGSYHkibOdQgO2oAFASNCdCb9/PSZDxGo/HlWBZRYgWAnJBAC0EZIMAXAjIBQG0EJANAnghIBME8ELAagaB7SyCb5QCqxBY7XLAazdsTkwLAHkBxZ4AiCFeao7j2IGFxiCeDFpFKRSwGLo0p5VnhP7PGaI1LIYdNKfV47E2D5S2fjasiF+UgBoD01EQUAcuaEFEHbioBcHssLwtBlcLZytimL64oUsOMBxmGhcD8wOzviCkEE6JYUQzIGUJ4CaBJA0AJ4F0GsBqCmXbQ6CVwGI10Mr7EuxvVADrimX6Y7hZcJYHAS3xjDWO1hbMNAiBs+A0DyJnwb95ELUW/FsPohqCCS+wZQDzN2wZMBUCuMXwv4IYsS22Ou0xFgitAKyDpkoIWQcxPyBoHcR8EFsHNauENrBAvtIKiJ3hGd+xhWAiBQHnoxYnpWANsT9MsJXwVAvjOoKJK4g5ITenS6DTITMG2KUA8wMCnBNPc10XQBdAY4BmAYD7w8qIu1oLqB8AnQaua2OkQbaxON7TlJY9Lfj/HiFcLywTxg+oYXqiViA+RI3TufeKhbD/84AURVEURVEURVEURVEURVEURVEURVEURVEURVEURVEURVEURVEURfndHhyQAAAAAAj6/7ofoQIAAAAAAAAAAPwEGcG4SMHdcSkAAAAASUVORK5CYII=";
                }
            }
            else
            {
                imagebase64 = "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";
            }
            StateHasChanged();
        }
        #endregion


        #region Check If Any InProgress StockTaking
        protected async Task GetAnyInProgressStockTaking()
        {
            try
            {
                var response = await lmsLiteratureService.CheckIfAnyInProgressStockTaking();
                if (response.IsSuccessStatusCode)
                {
                    isStockTakingInPrgoress = false;
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

        #region clear Search Text from drop down
        protected async Task ClearDropDownSearch()
        {
            var temp = UserId;

            userDropDown.Reset();
            await InvokeAsync(StateHasChanged);
            await FetchUsers();
            UserId = LiteratureAndUserDetail?.UserDetail?.FirstOrDefault()?.UserId;
            CivilId = LiteratureAndUserDetail?.UserDetail?.FirstOrDefault()?.CivilId;

            await InvokeAsync(StateHasChanged);
        }
        #endregion
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
    }
}


