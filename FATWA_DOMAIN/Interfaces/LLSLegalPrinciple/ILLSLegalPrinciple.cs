using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.LegalPrinciple;
using FATWA_DOMAIN.Models.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel.LegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel.MobileAppVMs;

namespace FATWA_DOMAIN.Interfaces.LLSLegalPrinciple
{
	public interface ILLSLegalPrinciple
    {
        Task<List<LLSLegalPrinciplesVM>> GetLLSLegalPrinciples(int UploadedDocumentId);
		Task<List<LLSLegalPrincipleCategory>> GetLLSLegalPrincipleCategory();
		Task<bool> SaveLegalPrincipleCategory(LLSLegalPrincipleCategory item);
		Task<List<LLSLegalPrinciplesRelationVM>> AdvanceSearchPrincipleRelation(LLSLegalPrinciplesRelationVM item);
		Task<LLSLegalPrincipleDetailVM> GetLLSLegalPrincipleDetailById(Guid principleId);
		Task<List<LLSLegalPrinciplReferenceVM>> GetLLSLegalPrincipleReferencesById(Guid principleId);
		Task<bool> SaveLLSLegalPrinciple(LLSLegalPrincipleSystem lLSLegalPrinciple);
		Task<bool> UpdateLLSLegalPrinciple(LLSLegalPrincipleSystem lLSLegalPrinciple);
		Task<LLSLegalPrincipleSystem> GetLLSLegalPrincipleDetailsByUsingPrincipleContentId(Guid principleContentId);
		Task<bool> DeleteLLSLegalPrinciple(LLSLegalPrinciplesVM args);
		Task<List<LLSLegalPrincipleCategoriesVM>> GetLLSLegaPrincipleCategories(bool showPublishedOnly=false);
		Task<List<LLSLegalPrincipleCategoriesVM>> GetLLSLegaPrincipleCategoriesAdvanceSearch(LLSLegalPrincipleCategoryAdvanceSearchVm advanceSearch);
		Task<bool> UpdateLegalPrincipleCategory(LLSLegalPrincipleCategory item);
		Task<bool> DeleteLegalPrincipleCategory(LLSLegalPrincipleCategory item);
        Task<List<LLSLegalPrincipleContent>> GetLLSPrincipleContents(int categoryId);
        Task<List<LLSLegalPrincipleContentCategoriesVM>> GetLLSLegalPrincipleContentCategories(Guid? principleContentId);
		Task<bool> LinkLegalPrincipleContents(List<LLSLegalPrincipleContentSourceDocumentReference> linkContents);
		Task<int?> CheckCopyDocumentExists(int uploadDocumentId);
		Task<LLSLegalPrincipleContent> GetLLSLegalPrincipleContentById(Guid principleContentId);
       
        Task<LLSLegalPrinciplesReviewVM> GetLegalPrincipleDetailById(Guid principleId);
        Task UpdateLegalPrincipleDecision(LLSLegalPrincipleDecisionVM item);
        Task<LLSLegalPrincipleSystem> GetLegalPrincipleDetailsByUsingPrincipleId(Guid principleId);
        Task<List<LLSLegalPrinciplesContentVM>> GetLLSLegalPrincipleContentDetailsByUsingPrincipleId(Guid principleId);
        Task<List<LegalPrincipleFlowStatus>> GetPrincipleFlowStatusDetails();
        Task<List<LLSLegalPrinciplesReviewVM>> GetLegalPrinciples(LLSLegalPrincipleAdvanceSearchVM search);
		Task<MobileAppLegalPrincipleContentDetailVM> GetLLSLegalPrincipleContentByIdForMobileApp(Guid? principleContentId);
        //Task<List<LegalPrinciplesDmsVM>> GetLegalPrincipleDms();
    }
}
