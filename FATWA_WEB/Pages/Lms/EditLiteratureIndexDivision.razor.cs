using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using FATWA_WEB.Services;
using FATWA_DOMAIN.Models;
using FATWA_WEB.Data;

namespace FATWA_WEB.Pages.Lms
{
    public partial class EditLiteratureIndexDivision : ComponentBase
    {
        public EditLiteratureIndexDivision()
        {
            DivisionAisleNumberCheck = false;
        }
        #region Variables declaration
        [Parameter]
        public dynamic DivisionAisleId { get; set; }
        private IEnumerable<LmsLiteratureIndexDivisionAisle> registeredIndexNumber { get; set; }
        private bool DivisionAisleNumberCheck;
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        LmsLiteratureIndexDivisionAisle _lmsLiteratureIndexDivision;
        protected LmsLiteratureIndexDivisionAisle lmsLiteratureIndexDivision
        {
            get
            {
                return _lmsLiteratureIndexDivision;
            }
            set
            {
                if (!object.Equals(_lmsLiteratureIndexDivision, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "lmsLiteratureIndexDivision", NewValue = value, OldValue = _lmsLiteratureIndexDivision };
                    _lmsLiteratureIndexDivision = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        #endregion

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        

        #region Initialization

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async Task Load()
        {
            spinnerService.Show();
            lmsLiteratureIndexDivision = await lmsLiteratureIndexDivisionServices.GetLiteratureIndexDivisionDetail(Convert.ToInt32(DivisionAisleId));
            spinnerService.Hide();
        }
        #endregion

        #region Update

        protected async Task Form0Submit(LmsLiteratureIndexDivisionAisle args)
        {
            try
            {
                // get all divisions & aisle records by using index id to check if division & aisle number is already created.
                registeredIndexNumber = await lmsLiteratureIndexDivisionServices.GetLmsLiteratureDivisionDetailsByUsingIndexId(args.IndexId);
                if (registeredIndexNumber.Count() != 0)
                {
                    foreach (var item in registeredIndexNumber)
                    {
                        // check if already division and aisle number saved in table then make it false.
                        if (item.DivisionNumber == args.DivisionNumber && item.AisleNumber == args.AisleNumber)
                        {
                            DivisionAisleNumberCheck = true;
                        }
                    }
                }
                if (DivisionAisleNumberCheck == false)
                {
                    var fatwaDbUpdateLmsLiteratureIndexResult = await lmsLiteratureIndexDivisionServices.UpdateLmsLiteratureIndexDivision(Convert.ToInt32(DivisionAisleId), args);
                    dialogService.Close(lmsLiteratureIndexDivision);
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Changes_saved_successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Info,
                        Detail = translationState.Translate("Division_aisle_number_is_already_saved"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    DivisionAisleNumberCheck = false;
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Literature_type_could_not_be_updated"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion

        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
    }
}
