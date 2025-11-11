using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Enums;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.Lms;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using FATWA_WEB.Data;
using FATWA_WEB.Pages.CaseManagment;
using FATWA_WEB.Pages.Lds;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Drawing;
using System.Drawing.Printing;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Lms
{
    public partial class DetailLiterature : ComponentBase
    {
        [Parameter]
		public string LiteratureId { get; set; }


        #region Variable Declaration
        public LmsLiterature Literature { get; set; } = new LmsLiterature();
        public List<LmsLiteratureAuthor> LiteratureAuthor { get; set; } = new List<LmsLiteratureAuthor>();
        protected RadzenDataGrid<LiteratureDetailLiteratureTagVM> TagsGrid;
        protected RadzenDataGrid<LiteratureAllAuthorsVM> AuthorGridRef;
        protected RadzenDataGrid<BorrowDetailVM> BorrowGrid;
        protected RadzenDataGrid<LmsLiteratureBarcode> BarCodeGrid;
        protected List<LmsLiteratureBarcode> ListBarCodes { get; set; } = new List<LmsLiteratureBarcode>();
        protected List<BorrowDetailVM> GetLmsLiteratureBorrowDetailsResult { get; set; } = new List<BorrowDetailVM>(); 

		protected List<LiteratureTag> ActiveLiteratureTags { get; set; } = new List<LiteratureTag>();
        public bool IsPrintDialogVisible { get; set; } = false;
        protected PrintCommandEnum PrintCommand { get; set; }
        protected String PrintValue { get; set; }
        protected UserDetailVM userDetails { get; set; } = new UserDetailVM();
        #endregion

        #region On Load

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
         
        LiteratureAllDetailsVM _getLiteratureAllDetailsVM;
        protected LiteratureAllDetailsVM getLiteratureAllDetailsVMResult
        {
            get
            {
                return _getLiteratureAllDetailsVM;
            }
            set
            {
                if (!object.Equals(_getLiteratureAllDetailsVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLiteratureAllDetailsVMResult", NewValue = value, OldValue = _getLiteratureAllDetailsVM };
                    _getLiteratureAllDetailsVM = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        List<LiteratureAllAuthorsVM> _getLiteratureAuthorVM;
        protected List<LiteratureAllAuthorsVM> getLiteratureAuthorVM
        {
            get
            {
                return _getLiteratureAuthorVM;
            }
            set
            {
                if (!object.Equals(_getLiteratureAuthorVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLiteratureAuthorVM", NewValue = value, OldValue = _getLiteratureAuthorVM };
                    _getLiteratureAuthorVM = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        LiteratureTag _getLiteratureTag;
        protected LiteratureTag getLiteratureTag
        {
            get
            {
                return _getLiteratureTag;
            }
            set
            {
                if (!object.Equals(_getLiteratureAllDetailsVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLiteratureTag", NewValue = value, OldValue = _getLiteratureTag };
                    _getLiteratureTag = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            userDetails = await BrowserStorage.GetItemAsync<UserDetailVM>("UserDetail");
            await Load();

            spinnerService.Hide(); 
        }
		protected async Task Load()
        {
            try
            {
				var tagsResponse = await lmsLiteratureService.GetAllActiveLiteratureTags();
                if (tagsResponse.IsSuccessStatusCode)
                {
					ActiveLiteratureTags = (List<LiteratureTag>)tagsResponse.ResultData;
				}

				Literature = await lmsLiteratureService.GetLmsLiteratureTagLiteratureById(int.Parse(LiteratureId));
                 ApiCallResponse lms = await lmsLiteratureService.GetLMSLiteratureDetailById(int.Parse(LiteratureId));
                if (lms.IsSuccessStatusCode)
                {
                    getLiteratureAllDetailsVMResult = (LiteratureAllDetailsVM)lms.ResultData;
                    ListBarCodes = getLiteratureAllDetailsVMResult.literatureBarCodeList;
                }
                 

                ApiCallResponse Author = await lmsLiteratureService.GetLMSLiteratureAuthorsById(int.Parse(LiteratureId));
                if (Author.IsSuccessStatusCode)
                {
                    getLiteratureAuthorVM = (List<LiteratureAllAuthorsVM>)Author.ResultData;
                }
                var DetailResponse = await lmsLiteratureService.GetBorrowDetailById(int.Parse(LiteratureId));
                if(DetailResponse.IsSuccessStatusCode)
                {
					GetLmsLiteratureBorrowDetailsResult = (List<BorrowDetailVM>)DetailResponse.ResultData;

				}

                await Task.Delay(100);
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        #endregion


        #region Print Events

        protected void Print(PrintCommandEnum printCommand, string? value)

        {
            PrintCommand = printCommand;
            PrintValue = value != null ? value : "";
            IsPrintDialogVisible = true;
        }

        protected void PrintSticker()
        {
            try
            {
                PrintDocument pd = new PrintDocument();
                pd.DefaultPageSettings.PaperSize = new PaperSize("pprnm", Literature.Characters.Length * 40, 100);
                pd.PrintPage += new PrintPageEventHandler(PrintStickerPage);
                pd.Print();
                pd.EndPrint += (o, e) =>
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("The_barcode_has_been_printed_successfully"),
                        Summary = $"!نجاح",
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                };
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    //Summary = $"???!",
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        private void PrintStickerPage(object sender, PrintPageEventArgs ev)
        {
            ev.HasMorePages = false;
            ev.Graphics.DrawString(Literature.Characters, new Font("Microsoft Sans Serif", (float)26.75, FontStyle.Bold, GraphicsUnit.Point), Brushes.Black, 0, 0);
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
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

    }
}
