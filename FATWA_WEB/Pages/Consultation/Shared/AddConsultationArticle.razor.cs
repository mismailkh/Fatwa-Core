using FATWA_DOMAIN.Models.Consultation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using System.ComponentModel;
using static FATWA_DOMAIN.Enums.Consultation.ConsultationEnums;

namespace FATWA_WEB.Pages.Consultation.Shared
{
    public partial class AddConsultationArticle : ComponentBase
    {
        #region Constructor
        public AddConsultationArticle()
        {
            consultationArticles = new ConsultationArticle();
            SectionParentDetails = new List<ConsultationSection>();
            ArticleStatusDetails = new List<ConsultationArticleStatus>();
            consultationSectionModel = new ConsultationSection();
        }
        #endregion

        #region Article check
        [Parameter]
        public ConsultationArticle ArticleDetail { get; set; }
        #endregion

        #region Variables 
        protected List<ConsultationSection> SectionParentDetails { get; set; }
        public DateTime Max = new DateTime(DateTime.Now.Date.Ticks);
        public DateTime Min = new DateTime(1900, 1, 1);
        public List<ConsultationArticleStatus> ArticleStatusDetails { get; set; }
        public ConsultationSection consultationSectionModel { get; set; }
        #endregion

        #region Model full property Instance
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        public ConsultationArticle consultationArticles { get; set; }
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
            if (ArticleDetail.ConsultationRequestId != Guid.Empty && ArticleDetail.ArticleId != Guid.Empty)
            {
                consultationArticles = ArticleDetail;
            }
            else
            {
                consultationArticles.ArticleId = Guid.NewGuid();
                consultationArticles.ArticleStatusId = (int)ConsultationArticleStatusEnum.New;
            }
            var resultParent = await consultationRequestService.GetSectionParentList();
            if (resultParent.IsSuccessStatusCode)
            {
                SectionParentDetails = (List<ConsultationSection>)resultParent.ResultData;
            }
            var resultStatus = await consultationRequestService.GetArticleStatusList();
            if (resultStatus.IsSuccessStatusCode)
            {
                ArticleStatusDetails = (List<ConsultationArticleStatus>)resultStatus.ResultData;
            }
        }

        #endregion

        #region Submit button click
        protected async Task Form0Submit(ConsultationArticle args)
        {
            try
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
                    if (args.ArticleStatusId != 0)
                    {
                        var articleStatusDetail = ArticleStatusDetails.Where(x => x.Id == args.ArticleStatusId).FirstOrDefault();
                        if (articleStatusDetail != null)
                        {
                            args.Article_Status_Name_En = articleStatusDetail.Name_En;
                            args.Article_Status_Name_Ar = articleStatusDetail.Name_Ar;
                        }
                    }
                    dialogService.Close(args);
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
