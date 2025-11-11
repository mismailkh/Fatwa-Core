using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.ViewModel.Shared;
using FATWA_DOMAIN.Models.WorkflowModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Interfaces.CaseManagement
{
    public interface ICmsCaseTemplate
    {
        Task<List<CaseTemplate>> GetHeaderFooter();
        Task<List<CaseTemplateSectionsVM>> GetTemplateSections(int templateId);
        Task<List<CaseTemplateSectionsVM>> GetDraftedTemplateSections(Guid draftId);
         Task<List<CaseTemplateSectionsVM>> GetComsDraftedTemplateSections(Guid draftId);
        Task<List<CaseTemplateParametersVM>> GetTemplateSectionParameters(Guid templateSectionId);
        Task<List<CaseTemplateParametersVM>> GetDraftedTemplateSectionParameters(Guid templateSectionId);
       Task<List<CaseTemplateParametersVM>> GetCOMSDraftedTemplateSectionParameters(Guid templateSectionId);
        Task<CaseTemplate> GetCaseTemplateContent(int templateId, Guid? DraftId, Guid? VersionId);
        Task<CaseTemplate> GetCaseTemplateDetail(int templateId);
        Task<CmsDraftedDocumentDetailVM> GetDraftDocDetailWithSectionAndParameters(Guid draftId, Guid? VersionId);
        Task<ComsDraftedDocumentDetailVM> GetConsultationDraftDocDetailWithSectionAndParameters(Guid draftId);
        Task<CmsDraftedTemplate> GetDraftedTemplateDetailById(Guid draftId, Guid versionId);
        Task<DmsAddedDocument> GetDMSDocumentDetailById(Guid documentId, Guid versionId);
        Task<ComsDraftedTemplate> GetConsultationDraftedTemplateDetailById(Guid draftId);
        Task GetTemplateDataFromCaseFile(Guid fileId);
        Task<CmsDraftedTemplate> GetDraftNumberVersionNumber(Guid? draftId, Guid? versionId);
        Task CreateCaseDraftDocument(CmsDraftedTemplate document);
        Task CreateDraftDocumentVersion(CmsDraftedTemplate DraftedTemplate);
        Task UpdateCaseDraftDocument(CmsDraftedTemplate document);
        Task UpdateDraftDocumentStatus(CmsDraftedTemplateVersions document);
		Task CreateConsultationDraftDocument(ComsDraftedTemplate document);
        Task<List<CmsDraftedDocumentVM>> GetCaseDraftListByReferenceId(Guid referenceId);
        Task<List<CmsDraftedDocumentReasonVM>> GetDraftDocumentReasonsByReferenceId(Guid referenceId);
        Task<List<CmsDraftedDocumentOpioninVM>> GetDraftDocumentOpinionByReferenceId(Guid referenceId);
        Task<ComsDraftedTemplate> GetComsDraftNumberVersionNumber(Guid? draftId);
        Task<List<ComsDraftedDocumentVM>> GetConsultationDraftListByReferenceId(Guid referenceId);
        Task UpdateConsultationDraftDocument(ComsDraftedTemplate document);
        Task<List<ConsultationTemplateSection>> GetSelectedConsultationTemplateSectionDetailsUsingTemplateId(int templateId);
        Task<bool> SaveDraftFileConsultationRequest(ConsultationRequest consultationRequests);
        Task<List<ComsDraftedDocumentReasonVM>> GetComsDraftDocumentReasonsByReferenceId(Guid referenceId);
        Task<List<CmsDraftTemplateVersionLogVM>> GetCmsDraftTemplateVersionLogsList(Guid versionId);
        Task<CmsDraftedDocumentParentEntityDetailVM> GetDraftParentEntityDetails(Guid referenceId, string userId);
        Task<bool> SoftDeleteDraftDocumentById(Guid draftId, string userName);
        //Task<OutBoxNumberResult> GetNewCaseOutBoxNumberbyPattern(int OutBoxNumberTypeId, string userName);
    }
}
