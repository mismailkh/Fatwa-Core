using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.LegalLegislationEnum;

namespace FATWA_WEB.Pages.Lds
{
    public partial class AddLegislationRelation : ComponentBase
    {
        #region Constructor
        public AddLegislationRelation()
        {
            legislationVMSearchResult = new List<LegalLegislationVM>();
            legislationVMResult = new LegalLegislationVM();
            GetLegislationTypeDetails = new List<LegalLegislationType>();
            LegislationYearCollection = new List<int>(Enumerable.Range(1900, DateTime.Now.Year - 1900 + 1).ToList());
            legalLegislationVM = new List<LegalLegislationVM>();
            lLReferencesAddToIntroArea = new LegalLegislationReference();
            RegisteredExistingLegislation = new LegalLegislation();
            GlobalListForExistingLegislationChanges = new List<LegalLegislation>();
            GlobalListForExistingArticleChanges = new List<LegalArticle>();
            ArticleStatusDetails = new List<LegalArticleStatus>();
        }
        #endregion

        #region Parameter
        [Parameter]
        public bool ArticleEffectCheckInRelationPage { get; set; }
		#endregion

		#region Parameter (Legislation model for article effect)
		[Parameter]
		public LegalLegislation NewLegislationDetailForArticleEffect { get; set; }
		#endregion

		#region Variables 
		public List<LegalLegislationVM> legislationVMSearchResult { get; set; }

        public LegalLegislationVM legislationVMResult { get; set; }
        public List<LegalLegislationType> GetLegislationTypeDetails { get; set; }
        protected IList<int> LegislationYearCollection { get; set; }
        protected RadzenDataGrid<LegalLegislationVM> gridRelation;
        protected LegalLegislationReference lLReferencesAddToIntroArea { get; set; }
        public List<LegalArticleStatus> ArticleStatusDetails { get; set; }
        #endregion

        #region Variables (Article Effects (Existing legislation cancel, Article cancel, modify & add))
        protected LegalLegislation RegisteredExistingLegislation { get; set; }

        protected List<LegalLegislation> GlobalListForExistingLegislationChanges { get; set; }
        protected List<LegalArticle> GlobalListForExistingArticleChanges { get; set; }
        #endregion

        #region Model full property Instance
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        protected List<LegalLegislationVM> legalLegislationVM { get; set; }
        #endregion

        #region Component Initial
        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                await Load();
                await SearchRelationButtonClick();
                spinnerService.Hide();
            }
            catch (Exception ex)
            {

            }
        }
        protected async Task Load()
        {
            var resultArticleStatus = await legalLegislationService.GetLegalArticleStatusList();
            if (resultArticleStatus.IsSuccessStatusCode)
            {
                ArticleStatusDetails = (List<LegalArticleStatus>)resultArticleStatus.ResultData;
            }
            var resultType = await legalLegislationService.GetLegislationTypeDetails();
            if (resultType.IsSuccessStatusCode)
            {
                GetLegislationTypeDetails = (List<LegalLegislationType>)resultType.ResultData;
            }
        }

        #endregion

        #region Search Relation Button Click
        protected async Task SearchRelationButtonClick()
        {
            try
            {
                var resultRelation = await legalLegislationService.AdvanceSearchRelation(legislationVMResult);
                if (resultRelation.IsSuccessStatusCode)
                {
                    legalLegislationVM = (List<LegalLegislationVM>)resultRelation.ResultData;
                    if (legalLegislationVM.Count() != 0 && NewLegislationDetailForArticleEffect != null)
                    {
                        var currentLegislationDetail = legalLegislationVM.Where(x => x.LegislationId == NewLegislationDetailForArticleEffect.LegislationId).FirstOrDefault();
                        if (currentLegislationDetail != null)
                        {
                            legalLegislationVM.Remove(currentLegislationDetail);
						}
                    }
                    foreach (var item in legalLegislationVM)
                    {
                        if (item.RelatedArticles.Count() != 0)
                        {
                            foreach (var itemArticle in item.RelatedArticles)
                            {
                                var resultArticleStatus = ArticleStatusDetails.Where(x => x.Id == itemArticle.Article_Status).FirstOrDefault();
                                if (resultArticleStatus != null)
                                {
                                    itemArticle.Article_Status_Name_En = resultArticleStatus.Name_En;
                                    itemArticle.Article_Status_Name_Ar = resultArticleStatus.Name_Ar;
                                }
                            }
                        }
                    }
                }
                else
                {
                    legalLegislationVM = new List<LegalLegislationVM>();
                    gridRelation.Reset();
                    gridRelation.Reload();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Grid add relation button click
        protected async Task GridAddRelationButtonClick(LegalLegislationVM args)
        {
            try
            {
                if (args != null)
                {
                    lLReferencesAddToIntroArea = new LegalLegislationReference()
                    {
                        Legislation_Link_Id = args.LegislationId,
                        Legislation_Link = !string.IsNullOrEmpty(args.LegislationTitleEn) ? args.LegislationTitleEn : translationState.Translate("Reference_Law")
                    };
                    dialogService.Close(lLReferencesAddToIntroArea);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Radzen datagrid
        protected void RowRender(RowRenderEventArgs<LegalLegislationVM> args)
        {
            args.Expandable = args.Data.RelatedArticles.Count() != 0;
        }
        protected void OnGroupRowRender(GroupRowRenderEventArgs args)
        {
            if (args.FirstRender) { args.Expanded = false; }
        }
        #endregion

        #region Article Effects (Existing legislation cancel, article cancel, modify & add)
        protected async Task GridArticleCancelClick(LegalArticle args)
        {
            try
            {
                if (args != null)
                {
                    args.Article_Source = (int)LegalArticleSourceEnum.Existing;
                    args.Article_Status = (int)LegalArticleStatusEnum.Expired;
                    var startDate = NewLegislationDetailForArticleEffect.LegalTemplates.SelectedCheckBoxValues.Where(x => x == (int)LegalTemplateSettingEnum.Legislation_Start_Date).Select(x => NewLegislationDetailForArticleEffect.StartDate).FirstOrDefault();
                    if (startDate != null)
                    {
                        args.End_Date = startDate;
                    }
                    else
                    {
                        args.End_Date = DateTime.Now;
                    }
                    var result = await dialogService.OpenAsync<AddLegislationArticle>(translationState.Translate("Add_Article"),
                                        new Dictionary<string, object>()
                                        {
                                            { "SelectedArticleFromEffectGrid", args }
                                        });
                    var ArticleCancelResult = (LegalArticle)result;
                    if (ArticleCancelResult != null)
                    {
						if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            ArticleCancelResult.ArticleEffectNoteHistory = translationState.Translate("The_Article_Expired_Start_Message") +" "+ ArticleCancelResult.End_Date.Value.ToString("dd/MM/yyyy") +" "+ translationState.Translate("The_Article_Expired_Middle_Message") + " " + GetLegislationTypeDetails.Where(x => x.Id == NewLegislationDetailForArticleEffect.Legislation_Type).Select(x => x.Name_En).FirstOrDefault() + " " + translationState.Translate("The_Article_Expired_Last_Message") + " " + NewLegislationDetailForArticleEffect.LegalTemplates.SelectedCheckBoxValues.Where(x => x == (int)LegalTemplateSettingEnum.Legislation_Number).Select(x => NewLegislationDetailForArticleEffect.Legislation_Number).FirstOrDefault() + ".";
						}
                        else
                        {
							ArticleCancelResult.ArticleEffectNoteHistory = translationState.Translate("The_Article_Expired_Start_Message") +" "+ ArticleCancelResult.End_Date.Value.ToString("dd/MM/yyyy") + " "+ translationState.Translate("The_Article_Expired_Middle_Message") + " " + GetLegislationTypeDetails.Where(x => x.Id == NewLegislationDetailForArticleEffect.Legislation_Type).Select(x => x.Name_Ar).FirstOrDefault() + " " + translationState.Translate("The_Article_Expired_Last_Message") + " " + NewLegislationDetailForArticleEffect.LegalTemplates.SelectedCheckBoxValues.Where(x => x == (int)LegalTemplateSettingEnum.Legislation_Number).Select(x => NewLegislationDetailForArticleEffect.Legislation_Number).FirstOrDefault() + ".";
						}
					}
                    var resultExistingArticle = await legalLegislationService.ExistingArticleStatusChange(ArticleCancelResult);
                    if (resultExistingArticle.IsSuccessStatusCode)
                    {
                        var RegisteredExistingArticle = (LegalArticle)resultExistingArticle.ResultData;
                        if(RegisteredExistingArticle != null)
                        {
                            if (GlobalListForExistingLegislationChanges.Count() != 0)
                            {
                                foreach (var item in GlobalListForExistingLegislationChanges)
                                {
                                    if (item.LegislationId == RegisteredExistingArticle.LegislationId)
                                    {
                                        item.LegalArticles.Add(RegisteredExistingArticle);
                                    }
                                }
                            }
                            await SearchRelationButtonClick();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        string  IsArticalEffect = "IsArticalEffect";
        protected async Task GridArticleAddClick(LegalArticle args)
        {
            try
            {
                args.Article_Source = (int)LegalArticleSourceEnum.New;
                args.Article_Status = (int)LegalArticleStatusEnum.Active;
                var result = await dialogService.OpenAsync<AddLegislationArticle>(translationState.Translate("Add_Article"),
                                    new Dictionary<string, object>()
                                    {
                                        { "SelectedArticleFromEffectGrid", args },
                                        { "NewLegislationDetailForArticleEffect", NewLegislationDetailForArticleEffect},
                                        { "IsArticalEffect", IsArticalEffect },
                                    });
                var ArticleLinkResult = (LegalArticle)result;
                if (ArticleLinkResult != null)
                {
					if (ArticleLinkResult != null)
					{
                        var response = await legalLegislationService.GetArticleNumberForArticleEffectByUsingLegislationId(ArticleLinkResult.LegislationId);
                        var resultArticleEffect = (List<ArticleNumberForEffect>)response.ResultData;
                        var maxArticleNumber = resultArticleEffect.Max(x => x.ArticleNumber);
                        maxArticleNumber += 1;
                        var resultIssueDate = NewLegislationDetailForArticleEffect.LegalTemplates.SelectedCheckBoxValues.Where(x => x == (int)LegalTemplateSettingEnum.Legislation_Issue_Date).Select(x => NewLegislationDetailForArticleEffect.IssueDate?.ToString("dd/MM/yyyy") ?? "").FirstOrDefault();
                        if (resultIssueDate != null)
                        {
                            if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
						    {
                            
                                ArticleLinkResult.ArticleEffectNoteHistory = translationState.Translate("The_Article_Effect_Add_Start_Message") + " " + maxArticleNumber + " " + translationState.Translate("The_Article_Effect_Add_Middle_Message") + " " + GetLegislationTypeDetails.Where(x => x.Id == NewLegislationDetailForArticleEffect.Legislation_Type).Select(x => x.Name_En).FirstOrDefault() + " " + translationState.Translate("The_Article_Effect_Add_Middle_Two_Message") + " " + NewLegislationDetailForArticleEffect.LegalTemplates.SelectedCheckBoxValues.Where(x => x == (int)LegalTemplateSettingEnum.Legislation_Number).Select(x => NewLegislationDetailForArticleEffect.Legislation_Number).FirstOrDefault() + " " + translationState.Translate("The_Article_Effect_Add_Last_Message") + " " + resultIssueDate + ".";
						    }
						    else
						    {
							    ArticleLinkResult.ArticleEffectNoteHistory = translationState.Translate("The_Article_Effect_Add_Start_Message") + " " + maxArticleNumber + " " + translationState.Translate("The_Article_Effect_Add_Middle_Message") + " " + GetLegislationTypeDetails.Where(x => x.Id == NewLegislationDetailForArticleEffect.Legislation_Type).Select(x => x.Name_Ar).FirstOrDefault() + " " + translationState.Translate("The_Article_Effect_Add_Middle_Two_Message") + " " + NewLegislationDetailForArticleEffect.LegalTemplates.SelectedCheckBoxValues.Where(x => x == (int)LegalTemplateSettingEnum.Legislation_Number).Select(x => NewLegislationDetailForArticleEffect.Legislation_Number).FirstOrDefault() + " " + translationState.Translate("The_Article_Effect_Add_Last_Message") + " " + resultIssueDate + ".";
						    }
                        }
                        else
                        {
                            if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                            {

                                ArticleLinkResult.ArticleEffectNoteHistory = translationState.Translate("The_Article_Effect_Add_Start_Message") + " " + maxArticleNumber + " " + translationState.Translate("The_Article_Effect_Add_Middle_Message") + " " + GetLegislationTypeDetails.Where(x => x.Id == NewLegislationDetailForArticleEffect.Legislation_Type).Select(x => x.Name_En).FirstOrDefault() + " " + translationState.Translate("The_Article_Effect_Add_Middle_Two_Message") + " " + NewLegislationDetailForArticleEffect.LegalTemplates.SelectedCheckBoxValues.Where(x => x == (int)LegalTemplateSettingEnum.Legislation_Number).Select(x => NewLegislationDetailForArticleEffect.Legislation_Number).FirstOrDefault() + ".";
                            }
                            else
                            {
                                ArticleLinkResult.ArticleEffectNoteHistory = translationState.Translate("The_Article_Effect_Add_Start_Message") + " " + maxArticleNumber + " " + translationState.Translate("The_Article_Effect_Add_Middle_Message") + " " + GetLegislationTypeDetails.Where(x => x.Id == NewLegislationDetailForArticleEffect.Legislation_Type).Select(x => x.Name_Ar).FirstOrDefault() + " " + translationState.Translate("The_Article_Effect_Add_Middle_Two_Message") + " " + NewLegislationDetailForArticleEffect.LegalTemplates.SelectedCheckBoxValues.Where(x => x == (int)LegalTemplateSettingEnum.Legislation_Number).Select(x => NewLegislationDetailForArticleEffect.Legislation_Number).FirstOrDefault() + ".";
                            }
                        }
                        maxArticleNumber = 0;
                    }
					var responseArticleEffect = await legalLegislationService.AddExistingArticleNewChilFromEffectsGrid(ArticleLinkResult);
                    if (responseArticleEffect.IsSuccessStatusCode)
                    {
                        var addExistingArticle = (LegalArticle)responseArticleEffect.ResultData;
                        GlobalListForExistingArticleChanges.Add(addExistingArticle); // add record in global article list incase if user cancel the form.
                        foreach (var items in legalLegislationVM) // also add article to grid list
                        {
                            if (items.LegislationId == addExistingArticle.LegislationId)
                            {
                                items.RelatedArticles.Add(addExistingArticle);
                            }
                        }
                        await SearchRelationButtonClick();
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        protected async Task GridArticleModifyClick(LegalArticle args)
        {
            try
            {
                args.Article_Source = (int)LegalArticleSourceEnum.Repeated;
                args.Article_Status = (int)LegalArticleStatusEnum.Modified;
                var result = await dialogService.OpenAsync<AddLegislationArticle>(translationState.Translate("Add_Article"),
                                    new Dictionary<string, object>()
                                    {
                                        { "SelectedArticleFromEffectGrid", args }
                                    });
                var ArticleLinkResult = (LegalArticle)result;
                if (ArticleLinkResult != null)
                {
                    var resultIssueDateForModify = NewLegislationDetailForArticleEffect.LegalTemplates.SelectedCheckBoxValues.Where(x => x == (int)LegalTemplateSettingEnum.Legislation_Issue_Date).Select(x => NewLegislationDetailForArticleEffect.IssueDate?.ToString("dd/MM/yyyy") ?? "").FirstOrDefault();
                    if (resultIssueDateForModify != null)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
					    {
						    ArticleLinkResult.ArticleEffectNoteHistory = translationState.Translate("The_Article_Modified_Start_Message") + " " + GetLegislationTypeDetails.Where(x => x.Id == NewLegislationDetailForArticleEffect.Legislation_Type).Select(x => x.Name_En).FirstOrDefault() + " " + translationState.Translate("The_Article_Effect_Modified_Middle_Two_Message") + " " + NewLegislationDetailForArticleEffect.LegalTemplates.SelectedCheckBoxValues.Where(x => x == (int)LegalTemplateSettingEnum.Legislation_Number).Select(x => NewLegislationDetailForArticleEffect.Legislation_Number).FirstOrDefault() + " " + translationState.Translate("The_Article_Effect_Add_Last_Message") + " " + resultIssueDateForModify + ".";
					    }
					    else
					    {
						    ArticleLinkResult.ArticleEffectNoteHistory = translationState.Translate("The_Article_Modified_Start_Message") + " " + NewLegislationDetailForArticleEffect.LegalTemplates.SelectedCheckBoxValues.Where(x => x == (int)LegalTemplateSettingEnum.Legislation_Number).Select(x => NewLegislationDetailForArticleEffect.Legislation_Number).FirstOrDefault() + " " + translationState.Translate("The_Article_Effect_Modified_Middle_Two_Message") + " " + GetLegislationTypeDetails.Where(x => x.Id == NewLegislationDetailForArticleEffect.Legislation_Type).Select(x => x.Name_Ar).FirstOrDefault() + " " + translationState.Translate("The_Article_Effect_Add_Last_Message") + " " + resultIssueDateForModify + ".";
					    }
                    }
                    else
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            ArticleLinkResult.ArticleEffectNoteHistory = translationState.Translate("The_Article_Modified_Start_Message") + " " + GetLegislationTypeDetails.Where(x => x.Id == NewLegislationDetailForArticleEffect.Legislation_Type).Select(x => x.Name_En).FirstOrDefault() + " " + translationState.Translate("The_Article_Effect_Modified_Middle_Two_Message") + " " + NewLegislationDetailForArticleEffect.LegalTemplates.SelectedCheckBoxValues.Where(x => x == (int)LegalTemplateSettingEnum.Legislation_Number).Select(x => NewLegislationDetailForArticleEffect.Legislation_Number).FirstOrDefault() + ".";
                        }
                        else
                        {
                            ArticleLinkResult.ArticleEffectNoteHistory = translationState.Translate("The_Article_Modified_Start_Message") + " " + NewLegislationDetailForArticleEffect.LegalTemplates.SelectedCheckBoxValues.Where(x => x == (int)LegalTemplateSettingEnum.Legislation_Number).Select(x => NewLegislationDetailForArticleEffect.Legislation_Number).FirstOrDefault() + " " + translationState.Translate("The_Article_Effect_Modified_Middle_Two_Message") + " " + GetLegislationTypeDetails.Where(x => x.Id == NewLegislationDetailForArticleEffect.Legislation_Type).Select(x => x.Name_Ar).FirstOrDefault() + ".";
                        }
                    }
                    var responseArticleEffect = await legalLegislationService.ModifiedExistingArticleNewChilFromEffectsGrid(ArticleLinkResult);
                    if (responseArticleEffect.IsSuccessStatusCode)
                    {
                        var addExistingArticle = (LegalArticle)responseArticleEffect.ResultData;
                        GlobalListForExistingArticleChanges.Add(addExistingArticle); // add record in global article list incase if user cancel the form.
                        foreach (var items in legalLegislationVM) // also add article to grid list
                        {
                            if (items.LegislationId == addExistingArticle.LegislationId)
                            {
                                items.RelatedArticles.Add(addExistingArticle);
                            }
                        }
                        await SearchRelationButtonClick();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected async Task GridLegislationCancelClick(LegalLegislationVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Article_Effect_Grid_Legislation_Cancel_Confirm_Message"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    if (args != null)
                    {
                        args.Legislation_Status = (int)LegislationStatus.Expired;
                        var startDate = NewLegislationDetailForArticleEffect.LegalTemplates.SelectedCheckBoxValues.Where(x => x == (int)LegalTemplateSettingEnum.Legislation_Start_Date).Select(x => NewLegislationDetailForArticleEffect.StartDate).FirstOrDefault();
                        if (startDate != null)
                        {
                            args.CanceledDate = startDate;
                        }
                        else
                        {
                            args.CanceledDate = DateTime.Now;
                        }
                        var resultExistingLegislation = await legalLegislationService.ExistingLegislationStatusChange(args);
                        if (resultExistingLegislation.IsSuccessStatusCode)
                        {
                            RegisteredExistingLegislation = (LegalLegislation)resultExistingLegislation.ResultData;
                            if (RegisteredExistingLegislation != null)
                            {
                                GlobalListForExistingLegislationChanges.Add(RegisteredExistingLegislation);
                            }
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Article_Effect_Grid_Legislation_Cancel_Success_Message"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            await SearchRelationButtonClick();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        //protected async Task GridLegislationViewClick(LegalLegislationVM args)
        //{
        //    try
        //    {
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        #endregion

        #region Add new legislation form open in Relation form
        protected async Task AddNewRelationButtonClick()
        {
            try
            {
                var result = await dialogService.OpenAsync<AddNewLegislationRelation>(translationState.Translate("Add_New_Legislation"),
                    new Dictionary<string, object>()
                {
                    //{"TemplateDetails", TemplateForAddNewRelation },
                    //{"ExistingLegislationIdForNewLegislation", LegislationIdForAddNewLegislationAttachment }
                    
                });
                var NewRelationResult = (LegalLegislation)result;
                if (NewRelationResult != null)
                {
                    GlobalListForExistingLegislationChanges.Add(NewRelationResult);

                    // now add this new created legislation into intro text area of main add legislation form
                    lLReferencesAddToIntroArea = new LegalLegislationReference()
                    {
                        Legislation_Link_Id = NewRelationResult.LegislationId,
                        Legislation_Link = !string.IsNullOrEmpty(NewRelationResult.LegislationTitle) ? NewRelationResult.LegislationTitle : translationState.Translate("Reference_Law"),
                        CheckNewLegislation = true
                    };
                    dialogService.Close(lLReferencesAddToIntroArea);
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
