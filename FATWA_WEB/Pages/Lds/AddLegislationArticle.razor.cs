using FATWA_DOMAIN.Models;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using System.Web;
using static FATWA_DOMAIN.Enums.LegalLegislationEnum;

namespace FATWA_WEB.Pages.Lds
{
    public partial class AddLegislationArticle : ComponentBase
    {
        #region Constructor
        public AddLegislationArticle()
        {
            legalArticles = new LegalArticle() {Start_Date = DateTime.Now };
            SectionParentDetails = new List<LegalSection>();
            validations = new Validation();
            ArticleStatusDetails = new List<LegalArticleStatus>();
            RelationLinkResult = new LegalLegislationReference();
            TreeExpandedItems = new List<object>();
            SelectedItems = new List<object>();
            legslSectionModel = new LegalSection();
            TreeViewLegalSectionData = new List<LegalSection>();
        }
        #endregion

        #region Show Explanatory Note In Article
        [Parameter]
        public bool ShowExplanatoryNoteInArticle { get; set; }
        #endregion

        #region Parameter(Selected Article From Effect Grid)
        [Parameter]
        public LegalArticle SelectedArticleFromEffectGrid { get; set; }
        [Parameter]
        public string IsArticalEffect { get; set; }
        #endregion

        #region Parameter (Selected article from article gird)
        [Parameter]
        public LegalArticle AddArticleFromGrid { get; set; }
        #endregion

        #region Parameter (Legislation model for article effect)
        [Parameter]
        public LegalLegislation NewLegislationDetailForArticleEffect { get; set; }
        #endregion

        #region Article with/without section check
        [Parameter]
        public bool ArticleWithSectionCheck { get; set; }
        #endregion

        #region Variables 
        protected List<LegalSection> SectionParentDetails { get; set; }
        protected Validation validations { get; set; }
        public DateTime Max = new DateTime(DateTime.Now.Date.Ticks);
        public DateTime Min = new DateTime(1900, 1, 1);
        public List<LegalArticleStatus> ArticleStatusDetails { get; set; }
        public LegalLegislationReference RelationLinkResult { get; set; }
        public IEnumerable<object> TreeExpandedItems { get; set; }
        public IEnumerable<object> SelectedItems { get; set; }
        public LegalSection legslSectionModel { get; set; }
        public IEnumerable<LegalSection> TreeViewLegalSectionData { get; set; }
        #endregion

        #region Model full property Instance
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        public LegalArticle legalArticles { get; set; }
        #endregion

        #region Validateion Class
        protected class Validation
        {
            public string ArticleTitleEn { get; set; } = string.Empty;
            public string ArticleNameEn { get; set; } = string.Empty;
            public string ArticleStartDate { get; set; } = string.Empty;
            public string ArticleStatus { get; set; } = string.Empty;
            public string ArticleEndDate { get; set; } = string.Empty;
            public string ArticleTextArea { get; set; } = string.Empty;
            public string SectionInArticle { get; set; } = string.Empty;
            
        }
        protected bool ValidateDetails()
        {
            bool basicDetailsValid = true;
            //if (ArticleWithSectionCheck)
            //{
            //    if (legalArticles.SectionId == null)
            //    {
            //        validations.SectionInArticle = "k-invalid";
            //        basicDetailsValid = false;
            //    }
            //    else
            //    {
            //        validations.SectionInArticle = "k-valid";
            //    }
            //}
            if (string.IsNullOrWhiteSpace(legalArticles.Article_Name))
            {
                validations.ArticleNameEn = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.ArticleNameEn = "k-valid";
            }
            if (string.IsNullOrWhiteSpace(legalArticles.Article_Title))
            {
                validations.ArticleTitleEn = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.ArticleTitleEn = "k-valid";
            }
            if (legalArticles.Article_Text != null && !string.IsNullOrWhiteSpace(HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(legalArticles.Article_Text)).Trim()))
            {
                validations.ArticleTextArea = "k-valid";
            }
            else
            {
                validations.ArticleTextArea = "k-invalid";
                basicDetailsValid = false;
            }
            if (string.IsNullOrWhiteSpace(legalArticles.Start_Date.ToString()))
            {
                validations.ArticleStartDate = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.ArticleStartDate = "k-valid";
            }
            if (legalArticles.Article_Status == (int)LegalArticleStatusEnum.Expired)
            {
                if (string.IsNullOrWhiteSpace(legalArticles.End_Date.ToString()))
                {
                    validations.ArticleEndDate = "k-invalid";
                    basicDetailsValid = false;
                }
                else
                {
                    validations.ArticleEndDate = "k-valid";
                }
            }
            if (legalArticles.Article_Status == 0)
            {
                validations.ArticleStatus = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.ArticleStatus = "k-valid";
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
                throw new Exception(ex.Message);
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
            if (SelectedArticleFromEffectGrid != null)
            {
                if (SelectedArticleFromEffectGrid.Article_Status == (int)LegalArticleStatusEnum.Modified)
                {
                    legalArticles.Article_Name = SelectedArticleFromEffectGrid.Article_Name;
                    legalArticles.Article_Title = SelectedArticleFromEffectGrid.Article_Title;
                    legalArticles.Article_Text = SelectedArticleFromEffectGrid.Article_Text;
                    legalArticles.Article_Explanatory_Note = SelectedArticleFromEffectGrid.Article_Explanatory_Note;
                    legalArticles.LegislationId = SelectedArticleFromEffectGrid.LegislationId;
                    legalArticles.ExistingArticleId = SelectedArticleFromEffectGrid.ArticleId;
                    legalArticles.SectionId = SelectedArticleFromEffectGrid.SectionId;
                    legalArticles.ArticleId = Guid.NewGuid();
                    legalArticles.Start_Date = DateTime.Now;
                    legalArticles.ArticleOrder = SelectedArticleFromEffectGrid.ArticleOrder.Value.AddMilliseconds(1000);
                    legalArticles.Article_Status = SelectedArticleFromEffectGrid.Article_Status;
                    if (SelectedArticleFromEffectGrid.NextArticleId != Guid.Empty)
                    {
                        legalArticles.NextArticleId = SelectedArticleFromEffectGrid.NextArticleId;
                    }
                    else
                    {
                        legalArticles.NextArticleId = Guid.Empty;
                    }
                }
                if (SelectedArticleFromEffectGrid.Article_Status == (int)LegalArticleStatusEnum.Expired)
                {
                    legalArticles = SelectedArticleFromEffectGrid;
                }
                if (SelectedArticleFromEffectGrid.Article_Status == (int)LegalArticleStatusEnum.Active)
                {                    
                    legalArticles.LegislationId = SelectedArticleFromEffectGrid.LegislationId;
                    legalArticles.ExistingArticleId = SelectedArticleFromEffectGrid.ArticleId;
                    legalArticles.ArticleId = Guid.NewGuid();
                    var issueDate = NewLegislationDetailForArticleEffect.LegalTemplates.SelectedCheckBoxValues.Where(x => x == (int)LegalTemplateSettingEnum.Legislation_Issue_Date).Select(x => NewLegislationDetailForArticleEffect.IssueDate).FirstOrDefault();
                    //var response = await legalLegislationService.GetLegalLegislationDetailByUsingId(SelectedArticleFromEffectGrid.LegislationId);
                    if (issueDate != null)
                    {
                        //var result = (LegalLegislation)response.ResultData;
                        legalArticles.Start_Date = issueDate;
                    }
                    else
                    {
                        legalArticles.Start_Date = DateTime.Now;
                    }
                    legalArticles.ArticleOrder = SelectedArticleFromEffectGrid.ArticleOrder.Value.AddMilliseconds(1000);
                    legalArticles.Article_Status = SelectedArticleFromEffectGrid.Article_Status;
                    if (SelectedArticleFromEffectGrid.NextArticleId != Guid.Empty)
                    {
                        legalArticles.NextArticleId = SelectedArticleFromEffectGrid.NextArticleId;
                    }
                    else
                    {
                        legalArticles.NextArticleId = Guid.Empty;
                    }
                }
            }
            else if (AddArticleFromGrid != null)
            {
                legalArticles.ArticleId = AddArticleFromGrid.ArticleId;
                legalArticles.LegislationId = AddArticleFromGrid.LegislationId;
                legalArticles.Article_Status = (int)LegalArticleStatusEnum.Active;
                legalArticles.SectionId = AddArticleFromGrid.SectionId;
                legalArticles.Article_Title = AddArticleFromGrid.Article_Title;
                legalArticles.Article_Name = AddArticleFromGrid.Article_Name;
                legalArticles.Article_Text = AddArticleFromGrid.Article_Text;
                legalArticles.Article_Explanatory_Note = AddArticleFromGrid.Article_Explanatory_Note;
                legalArticles.Start_Date = AddArticleFromGrid.Start_Date;
            }
            else
            {
                legalArticles = new LegalArticle() 
                { 
                    ArticleId = Guid.NewGuid(), 
                    Article_Status = (int)LegalArticleStatusEnum.Active,
                    Start_Date = DateTime.Now
                };
                if (legalArticles.Article_Status == (int)LegalArticleStatusEnum.Expired)
                {
                    legalArticles.End_Date = DateTime.Now;
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
                    { "ArticleEffectCheckInRelationPage", false },
				    { "NewLegislationDetailForArticleEffect", NewLegislationDetailForArticleEffect }
                   //{"TemplateForAddNewRelation", TemplateForAddNewRelation },
                   // {"LegislationIdForAddNewLegislationAttachment", LegislationIdForAddNewLegislationAttachment }
                });
                RelationLinkResult = (LegalLegislationReference)result;
                if (RelationLinkResult != null)
                {
                    string linkText = RelationLinkResult.Legislation_Link.Replace(RelationLinkResult.Legislation_Link, "<a href='javascript:void(0)' id='" + RelationLinkResult.Legislation_Link_Id + "' class='relation-attachment-popup' name='Legislation'>" + RelationLinkResult.Legislation_Link + "</a>");
                    RelationLinkResult.Legislation_Link = linkText;
                    legalArticles.Article_Text = legalArticles.Article_Text + " " + RelationLinkResult.Legislation_Link;
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Add article effects
        protected async Task AddArticleEffect()
        {
            try
            {
               var result = await dialogService.OpenAsync<AddLegislationRelation>(translationState.Translate("Add_Article_Effect"),
               new Dictionary<string, object>()
               {
                   { "ArticleEffectCheckInRelationPage", true },
				   { "NewLegislationDetailForArticleEffect", NewLegislationDetailForArticleEffect }
			  });
                var ArticleLinkResult = (LegalArticle)result;
                if (ArticleLinkResult != null)
                {
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Submit button click
        protected async Task Form0Submit(LegalArticle args)
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

        #region Article effect (Article cancel button click)
        protected async Task ArticleCancelButtonClick()
        {
            if (await dialogService.Confirm(translationState.Translate("Article_Effect_Grid_Legislation_Cancel_Confirm_Message"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                dialogService.Close(legalArticles);
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
