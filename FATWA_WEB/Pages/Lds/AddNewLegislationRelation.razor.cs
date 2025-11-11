using FATWA_DOMAIN.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_DOMAIN.Enums.LegalLegislationEnum;

namespace FATWA_WEB.Pages.Lds
{
    public partial class AddNewLegislationRelation : ComponentBase
    {
        #region Constructor
        public AddNewLegislationRelation()
        {
            GetLegislationTypeDetails = new List<LegalLegislationType>();
            validations = new ValidationClass();
        }
        #endregion

        //#region Parameter
        //[Parameter]
        //public LegalTemplate TemplateDetails { get; set; }
        //[Parameter]
        //public Guid ExistingLegislationIdForNewLegislation { get; set; }
        //#endregion

        #region Variable declaration
        public LegalLegislation legalLegislationForAddRelation { get; set; }
        public List<LegalLegislationType> GetLegislationTypeDetails { get; set; }
        protected ValidationClass validations { get; set; }
        public DateTime Max = new DateTime(DateTime.Now.Date.Ticks);
        public DateTime Min = new DateTime(1900, 1, 1);
        #endregion

        #region Validations class
        protected class ValidationClass
        {
            public string LegislationType { get; set; } = string.Empty;
            public string LegislationNumber { get; set; } = string.Empty;
            public string IssueDate { get; set; } = string.Empty;
            public string IssueDate_Hijri { get; set; } = string.Empty;
            public string TitleEn { get; set; } = string.Empty;
        }
        #endregion

        #region Validate Basic Details OnChange
        protected bool ValidateBasicDetails(LegalLegislation item)
        {
            bool basicDetailsValid = true;
            if (item.Legislation_Type == 0)
            {
                validations.LegislationType = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.LegislationType = "k-valid";
            }
            if (String.IsNullOrWhiteSpace(item.Legislation_Number))
            {
                validations.LegislationNumber = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.LegislationNumber = "k-valid";
            }
            if (String.IsNullOrWhiteSpace(item.IssueDate.ToString()))
            {
                validations.IssueDate = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.IssueDate = "k-valid";
            }
            if (String.IsNullOrWhiteSpace(item.IssueDate_Hijri.ToString()))
            {
                validations.IssueDate_Hijri = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.IssueDate_Hijri = "k-valid";
            }
            if (String.IsNullOrWhiteSpace(item.LegislationTitle))
            {
                validations.TitleEn = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.TitleEn = "k-valid";
            }
            return basicDetailsValid;
        }
        #endregion

        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            await Load();

            spinnerService.Hide();
        }
        protected async Task Load()
        {
            try
            {
                legalLegislationForAddRelation = new LegalLegislation()
                {
                    LegislationId = Guid.NewGuid(),
                    IssueDate = DateTime.Now.Date,
                    IssueDate_Hijri = DateTime.Now.Date,
                    StartDate = DateTime.Now.Date
                };
                var resultType = await legalLegislationService.GetLegislationTypeDetails();
                if (resultType.IsSuccessStatusCode)
                {
                    GetLegislationTypeDetails = (List<LegalLegislationType>)resultType.ResultData;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Form submit
        protected async Task<bool> Form0Submit(LegalLegislation args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Legislation_Save_Popup_Confirm_Message"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    bool valid = ValidateBasicDetails(args);
                    if (valid)
                    {
                        var resultLegNumber = await legalLegislationService.CheckLegislationNumberDuplication(args.Legislation_Type, args.Legislation_Number);
                        if (resultLegNumber.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Legislation_Number_Duplication_Message"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            return false;
                        }
                        args.Legislation_Status = (int)LegislationStatus.Active;
                        args.Legislation_Flow_Status = (int)LegislationFlowStatusEnum.PartiallyCompleted;
                        var resultLegislation = await legalLegislationService.SaveLegalLegislation(args);
                        if (resultLegislation.IsSuccessStatusCode)
                        {
                            var returnResult = (bool)resultLegislation.ResultData;
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("legislation_Success_Message"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            await Task.Delay(200);
                            dialogService.Close(args);
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Form cancel
        protected async Task Button2Click(MouseEventArgs args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Legislation_Add_New_Relation_Save_Popup_Confirm_Message"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    dialogService.Close(null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
