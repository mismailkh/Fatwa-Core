using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Models.ViewModel.LiteratureAdvancedSearchVM;

namespace FATWA_WEB.Pages.Lms
{
    public partial class AddBorrowLiteraturePopUp : ComponentBase
    {
        #region parameter
        [Parameter]
        public dynamic? Barcode { get; set; }
        [Parameter]
        public dynamic? Id { get; set; }
        [Parameter]
        public dynamic? BookReturnDuration {  get; set; }   
        [Parameter]
        public dynamic FromBarcode { get; set; }
        #endregion

        #region Varriable
        public BorrowedLiteratureVM borrowedLiteratureVM { get; set; } = new();
        public List<BorrowedLiteratureVM> ListborrowedLiteratureVM { get; set; } = new();
        protected RadzenDataGrid<BorrowedLiteratureVM> grid0;
        public string advancedSearchEnumName { get; set; }
        protected LiteratureAdvancedSearchVM advancedSearchVM = new LiteratureAdvancedSearchVM();
        protected List<object> AdvancedSearchOptions { get; set; } = new List<object>();
        protected bool AdvanceSearchGridSHow = false;
        protected IEnumerable<LmsLiteratureIndex> LiteratureIndexeDetails { get; set; } = new List<LmsLiteratureIndex>();
        public IEnumerable<LmsLiteratureAuthor> LmsLiteratureAuthor { get; set; } = new List<LmsLiteratureAuthor>();
        public class AdvancedSearchEnumTypes
        {
            public LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum advancedSearchEnumValue { get; set; }
            public string advancedSearchEnumName { get; set; }
        }


        #endregion

        #region On Initialize

        protected override async Task OnInitializedAsync()
        {

            if (FromBarcode == true)
            {
                var response = await lmsLiteratureBorrowDetailService.GetLiteratureByBarcode(Barcode);
                if (response.ResultData.LiteratureId != null)
                {
                    borrowedLiteratureVM = (BorrowedLiteratureVM)response.ResultData;

                    borrowedLiteratureVM.BarrowedDate = DateTime.Now;
                    borrowedLiteratureVM.DueDate = DateTime.Now.AddDays(BookReturnDuration);
                    borrowedLiteratureVM.BarrowedDate = DateTime.Now;
                    borrowedLiteratureVM.IsNew = true;
                    borrowedLiteratureVM.LoggedInUser = loginState.UserDetail.UserName;
                    borrowedLiteratureVM.BorrowerUserId = Id;
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Info,
                        Detail = translationState.Translate("Book_not_Available"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    dialogService.Close();
                }

                //dialogService.Close();
            }
            else
            {
                await PopulateAdvancedSearchOptions();
            }
            translationState.TranslateGridFilterLabels(grid0);

        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Add Literature From Grid And From Detail 
        protected async Task AddBorrowLiterature()
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await lmsLiteratureBorrowDetailService.UpdateLiteratureReturnExtendDetail(borrowedLiteratureVM);
                    if (response.IsSuccessStatusCode)
                    {
                        dialogService.Close(true);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
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
        protected async Task AddBookFromGrid(BorrowedLiteratureVM item)
        {


            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Borrow_Book"), translationState.Translate("Confirm_Selection"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    item.BarrowedDate = DateTime.Now;
                    item.DueDate = DateTime.Now.AddDays(BookReturnDuration);
                    item.BarrowedDate = DateTime.Now;
                    item.IsNew = true;
                    item.LoggedInUser = loginState.UserDetail.UserName;
                    item.BorrowerUserId = Id;
                    var response = await lmsLiteratureBorrowDetailService.UpdateLiteratureReturnExtendDetail(item);
                    if (response.IsSuccessStatusCode)
                    {
                        dialogService.Close(true);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
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

        #region Cancel
        protected async Task Cancel()
        {

            dialogService.Close();
        }
        #endregion

        #region On Change
        protected async Task OnChangeSearchValue()
        {
            spinnerService.Show();
            AdvanceSearchGridSHow = false;
            advancedSearchVM.KeywordsType = null;
            advancedSearchVM.GenericsIntergerKeyword = 0;
            switch ((int)advancedSearchVM.EnumSearchValue)
            {
                case (int)AdvancedSearchDropDownEnum.Barcode:
                    break;

                case (int)AdvancedSearchDropDownEnum.Author_Name:
                    await GetAuthors();
                    break;

                case (int)AdvancedSearchDropDownEnum.Book_Index:
                    await GetLiteratureIndexDetails();
                    break;
                default:
                    break;
            }
            spinnerService.Hide();
            StateHasChanged();
        }
        #endregion

        #region LKPs
        protected async Task PopulateAdvancedSearchOptions()
        {
            foreach (LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum item in Enum.GetValues(typeof(LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum)))
            {
                AdvancedSearchOptions.Add(new AdvancedSearchEnumTypes { advancedSearchEnumName = translationState.Translate(item.ToString()), advancedSearchEnumValue = item });
            }
            StateHasChanged();
        }
        private async Task GetLiteratureIndexDetails()
        {
            LiteratureIndexeDetails = await lmsLiteratureIndexService.GetLiteratureIndexDetails();
        }
        private async Task GetAuthors()
        {
            try
            {
                var response = await lmsLiteratureService.GetAuthorItems();
                if (response.IsSuccessStatusCode)
                {
                    LmsLiteratureAuthor = (IEnumerable<LmsLiteratureAuthor>)response.ResultData;
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

        #region Search
        protected async Task Search()
        {
            try
            {
                ListborrowedLiteratureVM.Clear();   
                if (advancedSearchVM.LiteratureId == 0 && advancedSearchVM.ClassificationId == 0 && advancedSearchVM.IndexId == 0 && string.IsNullOrEmpty(advancedSearchVM.KeywordsType) && advancedSearchVM.GenericsIntergerKeyword == 0)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Please_Enter_Value_For_Search"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    if(grid0 != null)
                    {
                        grid0.Reload();
                    }
                }
                else
                {
                    var response = await lmsLiteratureBorrowDetailService.GetLmsBorrowLiteraturesAdvanceSearch(advancedSearchVM);

                    if (response.IsSuccessStatusCode)
                    {
                        ListborrowedLiteratureVM = (List<BorrowedLiteratureVM>)response.ResultData;
                        await InvokeAsync(StateHasChanged);
                        AdvanceSearchGridSHow = true;
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
            catch (Exception)
            {
            }
        }
        #endregion

    }
}
