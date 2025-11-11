using FATWA_DOMAIN.Models;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_WEB.Pages.Lds
{
    public partial class AddLegislationSection : ComponentBase
    {
        #region Constructor
        public AddLegislationSection()
        {
            legalSections = new LegalSection() { SectionId = Guid.NewGuid() };
            SectionParentDetails = new List<LegalSection>();
            validations = new Validation();
        }
        #endregion

        #region Variables 
        protected List<LegalSection> SectionParentDetails { get; set; }
        protected Validation validations { get; set; }
        #endregion

        #region Model full property Instance
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        public LegalSection legalSections { get; set; }
        #endregion

        #region Validateion Class
        protected class Validation
        {
            public string SectionTitleEn { get; set; } = string.Empty;
        }
        protected bool ValidateDetails()
        {
            bool basicDetailsValid = true;
            if (String.IsNullOrWhiteSpace(legalSections.SectionTitle))
            {
                validations.SectionTitleEn = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.SectionTitleEn = "k-valid";
            }           
            return basicDetailsValid;
        }
        #endregion

        #region Component Initial
        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                await Load();
                spinnerService.Hide();
            }
            catch (Exception ex)
            {

            }
        }
        protected async Task Load()
        {
            var resultNumber = await legalLegislationService.GetLegalSectionNewNumber();
            if (resultNumber.IsSuccessStatusCode)
            {
                legalSections.Section_Number = (int)resultNumber.ResultData;
            }
            var resultParent = await legalLegislationService.GetLegalSectionParentList();
            if (resultParent.IsSuccessStatusCode)
            {
                SectionParentDetails = resultParent.ResultData as List<LegalSection>;

                if (SectionParentDetails != null)
                {
                    // Apply LINQ to remove duplicates based on SectionTitle
                    SectionParentDetails = SectionParentDetails
                                            .GroupBy(section => section.SectionTitle)
                                            .Select(group => group.First())
                                            .ToList();
                }
            }
        }

        #endregion

        #region Submit button click

        protected async Task Form0Submit(LegalSection args)
        {
            try
            {
                bool valid = ValidateDetails();
                if (valid)
                {
                    dialogService.Close(args);
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Fill_Required_Fields"),
                        //Summary = $"" + translationState.Translate("Error"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Dialog close
        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        #endregion

    }
}
