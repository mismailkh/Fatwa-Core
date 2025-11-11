using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using FATWA_DOMAIN.Models.ViewModel.MobileAppVMs;

namespace FATWA_DOMAIN.Interfaces
{
    public interface ILegalLegislation
    {
        #region Legallegislation Detail View 
        Task<List<LegislationVM>> GetLegalLegislationPriviewDetailById(Guid LegislationId);
        Task<LegalLegislationDetailVM> GetLegalLegislationDetailById(Guid LegislationId);
        Task<LegalPublicationSourceVM> GetLegalPublicationSourceDetailByLegislationId(Guid LegislationId);
        Task<List<LegalArticalSectionVM>> GetLegalArticalSectionByLegislationId(Guid LegislationId);
        Task<List<LegalClausesSectionVM>> GetLegalClausesSectionByLegislationId(Guid LegislationId);
        Task<List<LegalLegislationSignature>> GetLegislationSignaturesbyLegislationId(Guid LegislationId);
        Task<LegalExplanatoryNote> GetLegislationExplanatoryNoteLegislationId(Guid LegislationId);
        Task<LegalNote> GetLegislationNoteLegislationId(Guid LegislationId);
        Task<List<LegalLegislationReference>> GetLegislationReferencebyLegislationId(Guid LegislationId);
        Task<LegalLegislation> GetLegalLegislationDetailPreviewById(Guid legislationId);
        #endregion

        Task<LegalLegislation> GetLegalLegislationDetailByUsingId(Guid legislationId);
        Task<List<LegalLegislationType>> GetLegislationTypeDetails();
        Task<List<LegalLegislationStatus>> GetLegislationStatusDetails();
        Task<List<LegalLegislationFlowStatus>> GetLegislationFlowStatusDetails();
        Task<List<LegalLegislationTag>> GetLegislationTagDetails();
        Task<List<LegalPublicationSourceName>> GetPublicationSourceNameDetails();

        Task<LegalLegislationTag> CreateLegalLegislationTags(LegalLegislationTag legalLegislationTag);
        Task<List<LegalLegislationsVM>> GetLegalLegislations(AdvanceSearchLegalLegislationsVM advanceSearchVM);
        Task<List<LegalLegislationsDmsVM>> GetLegalLegislationsDms();
        Task<List<LegalLegislationsVM>> GetLegislationForPublish(AdvanceSearchLegalLegislationsVM advanceSearchVM);
        Task<LegalLegislationDecisionVM> GetLegalLegislationsDecision(Guid legalDecesion);
        Task UpdateLegalLegislationDecision(LegalLegislationDecisionVM item);
        Task<List<LegalLegislationVM>> AdvanceSearchRelation(LegalLegislationVM item);
        Task<List<LegalSection>> GetLegalSectionParentList();
        Task<int> GetLegalSectionNewNumber();
        Task<bool> AddLegalSection(LegalSection item);
        Task<List<LegalArticleStatus>> GetLegalArticleStatusList();
        Task<LegalLegislation> ExistingLegislationStatusChange(LegalLegislationVM args);
        Task<LegalArticle> ExistingArticleStatusChange(LegalArticle args);
        Task<LegalArticle> AddExistingArticleNewChilFromEffectsGrid(LegalArticle args);
        Task<LegalArticle> ModifiedExistingArticleNewChilFromEffectsGrid(LegalArticle args);
        Task<LegalArticle> GetLatestArticleDetailByUsingLegislationId(Guid legislationId);
        Task<bool> SaveLegalLegislation(LegalLegislation args);
        Task<bool> UpdateLegalLegislation(LegalLegislation args);
        Task<bool> CheckLegislationNumberDuplication(int legislationType, string legislationNumber);
        Task<List<LegalTemplate>> GetLegalTemplateDetails();
        Task<List<LegalTemplateSetting>> GetLegalTemplateSettingDetails();
        Task<LegalTemplate> GetRegisteredTemplateDetailsByUsingSelectedTemplateId(Guid templateId);
        Task<int> CountAssociatedLegislationInTemplateByUsingTemplateId(Guid templateId);
        Task SoftDeleteLegalLegislation(LegalLegislationsVM legalLegislationsVM);
        Task<LegalLegislation> GetLegalLegislationDetailsByUsingLegislationIdForEditForm(Guid legislationId);
        Task<List<TempAttachement>> GetExplanatoryNoteAttachmentFromTempTableByUsingId(Guid explanatoryNoteId);
        Task<bool> UpdateSelectedSectionAsParentHasChildColumn(Guid sectionId);
        Task<bool> DeleteAttachmentFromTempTable(Guid legislationId);
        Task<List<LegalLegislationReference>> GetLegalLegislationReferenceByLegislationId(Guid legislationId);
        Task<List<LegalLegislationsVM>> GetDeleteLegalLegislations(int PageSize, int PageNumber);
        Task RevokeDeleteLegalLegislation(LegalLegislationsVM legalLegislationsVM);
        Task<List<LegalLegislationsVM>> GetApporvedLegislation(string UserId, int PageSize, int PageNumber);
		Task<List<ArticleNumberForEffect>> GetArticleNumberForArticleEffectByUsingLegislationId(Guid legislationId);
		Task<List<LegalLegislationCommentVM>> GetLegalLegislationCommentsDetailByUsingId(Guid legislationId);
        Task<List<LegalLegislationLegalTemplate>> GetAllTemplateSettingDetails();
        Task<MobileAppLegalLegislationDetailVM> GetLegalLegislationsDetailsForMobileApp(Guid LegislationId);
        Task<List<LegalLegislationsVM>> GetLegalLegislationApprovals(string UserId, int PageSize, int PageNumber);

    }
}