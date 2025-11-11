using FATWA_DOMAIN.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using System.ComponentModel;
using System.Web;
using static FATWA_DOMAIN.Enums.LegalLegislationEnum;

namespace FATWA_WEB.Pages.Lds
{
    public partial class AddLegislationClause : ComponentBase
    {
        #region Constructor
        public AddLegislationClause()
        {
            legalClauses = new LegalClause() { Start_Date = DateTime.Now };
            SectionParentDetails = new List<LegalSection>();
            validations = new Validation();
            ArticleStatusDetails = new List<LegalArticleStatus>();
            RelationLinkResult = new LegalLegislationReference();
        }
        #endregion

        #region Parameter (Selected clause from clause gird)
        [Parameter]
        public LegalClause AddClauseFromGrid { get; set; }
        #endregion

        #region Clause with/without section check
        [Parameter]
        public bool ClauseWithSectionCheck { get; set; }
        #endregion

        #region Variables 
        protected List<LegalSection> SectionParentDetails { get; set; }
        protected Validation validations { get; set; }
        public DateTime Max = new DateTime(DateTime.Now.Date.Ticks);
        public DateTime Min = new DateTime(1900, 1, 1);
        public List<LegalArticleStatus> ArticleStatusDetails { get; set; }
        public LegalLegislationReference RelationLinkResult { get; set; }
        #endregion

        #region Model full property Instance
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        public LegalClause legalClauses { get; set; }
        #endregion

        #region Validateion Class
        protected class Validation
        {
            public string ClauseNameEn { get; set; } = string.Empty;
            public string ClauseStartDate { get; set; } = string.Empty;
            public string ClaueStatus { get; set; } = string.Empty;
            public string ClauseEndDate { get; set; } = string.Empty;
            public string ClauseContentArea { get; set; } = string.Empty;
            public string SectionWithClause { get; set; } = string.Empty;
        }
        protected bool ValidateDetails()
        {
            bool basicDetailsValid = true;
            if (ClauseWithSectionCheck)
            {
                if (legalClauses.SectionId == null)
                {
                    validations.SectionWithClause = "k-invalid";
                    basicDetailsValid = false;
                }
                else
                {
                    validations.SectionWithClause = "k-valid";
                }
            }
            if (string.IsNullOrWhiteSpace(legalClauses.Clause_Name))
            {
                validations.ClauseNameEn = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.ClauseNameEn = "k-valid";
            }
            if (legalClauses.Clause_Content != null && !string.IsNullOrWhiteSpace(HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(legalClauses.Clause_Content)).Trim()))
            {
                validations.ClauseContentArea = "k-valid";
            }
            else
            {
                validations.ClauseContentArea = "k-invalid";
                basicDetailsValid = false;
            }
            if (legalClauses.Clause_Status == 0)
            {
                validations.ClaueStatus = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.ClaueStatus = "k-valid";
            }
            if (string.IsNullOrWhiteSpace(legalClauses.Start_Date.ToString()))
            {
                validations.ClauseStartDate = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.ClauseStartDate = "k-valid";
            }
            if (legalClauses.Clause_Status == (int)LegalArticleStatusEnum.Expired)
            {
                if (string.IsNullOrWhiteSpace(legalClauses.End_Date.ToString()))
                {
                    validations.ClauseEndDate = "k-invalid";
                    basicDetailsValid = false;
                }
                else
                {
                    validations.ClauseEndDate = "k-valid";
                }
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
            var resultParent = await legalLegislationService.GetLegalSectionParentList();
            if (resultParent.IsSuccessStatusCode)
            {
                SectionParentDetails = (List<LegalSection>)resultParent.ResultData;
                if (SectionParentDetails != null)
                {
                    // Apply LINQ to remove duplicates based on SectionTitle
                    SectionParentDetails = SectionParentDetails
                                            .GroupBy(section => section.SectionTitle)
                                            .Select(group => group.First())
                                            .ToList();
                }
            }
            var resultStatus = await legalLegislationService.GetLegalArticleStatusList();
            if (resultStatus.IsSuccessStatusCode)
            {
                ArticleStatusDetails = (List<LegalArticleStatus>)resultStatus.ResultData;
            }
            if (AddClauseFromGrid != null)
            {
                legalClauses.ClauseId = AddClauseFromGrid.ClauseId;
                legalClauses.LegislationId = AddClauseFromGrid.LegislationId;
                legalClauses.Clause_Status = (int)LegalArticleStatusEnum.Active;
                legalClauses.SectionId = AddClauseFromGrid.SectionId;
                legalClauses.Clause_Name = AddClauseFromGrid.Clause_Name;
                legalClauses.Clause_Content = AddClauseFromGrid.Clause_Content;
            }
            else
            {
                legalClauses = new LegalClause()
                {
                    ClauseId = Guid.NewGuid(),
                    Clause_Status = (int)LegalArticleStatusEnum.Active,
                    Start_Date = DateTime.Now
                };
                if (legalClauses.Clause_Status == (int)LegalArticleStatusEnum.Expired)
                {
                    legalClauses.End_Date = DateTime.Now;
                }
            }
        }

        #endregion

        #region Wizard Relation
        protected async Task AddRelation()
        {
            try
            {
                var result = await dialogService.OpenAsync<AddLegislationRelation>(translationState.Translate("Add_Relation"),
                new Dictionary<string, object>()
                {
                   { "ArticleEffectCheckInRelationPage", false }
                });
                RelationLinkResult = (LegalLegislationReference)result;
                if (RelationLinkResult != null)
                {
                    string linkText = RelationLinkResult.Legislation_Link.Replace(RelationLinkResult.Legislation_Link, "<a href='javascript:void(0)' id='" + RelationLinkResult.Legislation_Link_Id + "' class='relation-attachment-popup' name='Legislation'>" + RelationLinkResult.Legislation_Link + "</a>");
                    RelationLinkResult.Legislation_Link = linkText;
                    legalClauses.Clause_Content = legalClauses.Clause_Content + " " + RelationLinkResult.Legislation_Link;
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Submit button click
        protected async Task Form0Submit(LegalClause args)
        {
            try
            {
                bool valid = ValidateDetails();
                if (valid)
                {
                    if (await dialogService.Confirm(translationState.Translate("Legislation_Save_Popup_Confirm_Message"), translationState.Translate("Confirm"), new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    }) == true)
                    {
                        //now add section id & title in clause list and show this list in grid
                        if (args.SectionId != null) // get section title for grid
                        {
                            var resultSection = SectionParentDetails.Where(x => x.SectionId == args.SectionId).FirstOrDefault();
                            if (resultSection != null)
                            {
                                args.ShowSectionTitle = resultSection.SectionTitle;
                            }
                        }
                        dialogService.Close(args);
                    }
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
                throw new Exception(ex.Message);
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
