using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.LegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_DOMAIN.Models.ViewModel.LegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.DigitalSignature;
using FATWA_DOMAIN.Models.Lms;
using FATWA_DOMAIN.Models.ArchivedCasesModels;

namespace FATWA_DOMAIN.Interfaces
{
    //public interface IFileUpload<T> where T : class
    public interface ITempFileUpload
    {
        Task<List<AttachmentType>> GetAttachmentTypes(int? ModuleId, int? sectorTypeId, bool ShowHidden = false);
        Task<AttachmentType> GetAttachmentTypeDetailById(int? AttachmentTypeId);
        Task<TempAttachement> CreateTempAttachement(TempAttachement documentObj);
        Task<List<TempAttachement>> GetTempAttachementsByFileAndUserName(string fileName, string userName, int typeId);
        Task<string> GetDocumentById(Guid referenceGuid);
        Task<string> GetDocumentById(string referenceGuid, string isReferenceGuid);
        Task<List<UploadedDocumentVM>> GetUploadedAttachementsByLiteratureId(int literatureId);
        Task<List<UploadedDocumentVM>> GetUploadedAttachementsByReferenceGuid(Guid referenceGuid);
        Task<List<UploadedDocumentVM>> GetOfficialDocuments(Guid referenceGuid);
        Task<List<TempAttachementVM>> GetTempAttachementsByReferenceGuid(Guid referenceGuid, int attachementId = 0);
        Task<List<AttachmentType>> GetAllAttachmentTypes();
        Task DeleteTempAttachement(int attachementId);
        Task UploadTempAttachmentToUploadedDocument(Guid referenceId, string createdBy);
        Task<UploadedDocument> DeleteUploadedDocument(int id);
        //Task DeleteAttachement(object id);
        Task<string> GetUploadedDocument(int id);
        Task<UploadedDocument> GetUploadedAttachementById(int Id, Guid? referenceGuid = null, int _literatureId = 0);
        Task<int> AddCopyAttachments(int DocumentId, string createdBy);
        Task<bool> UpdateUploadedAttachementMojFlagById(int Id);
        Task<List<DmsDocumentClassification>> GetDocumentClassifications();
        Task<DmsAddedDocument> GetDocumentNumberAndVersion(Guid Id);
        Task RemoveTempAttachementsByReferenceId(Guid referenceId, string basePath);
        Task<DmsAddedDocument> SaveAddedDocument(DmsAddedDocument document);
        Task UpdateDMSDocument(DmsAddedDocument document);
        Task CreateDMSDocumentVersion(DmsAddedDocument DmsTemplate);
        Task MoveAttachmentToAddedDocumentVersion(MoveAttachmentAddedDocumentVM attachmentDetail);
        Task<DmsAddedDocument> GetDocumentDetailByVersionId(Guid Id, Guid DocumentId);
        Task<bool> SaveTempAttachmentToUploadedDocument(FileUploadVM item);
        Task<bool> CopyAttachmentsFromSourceToDestination(List<CopyAttachmentVM> attachmentDetail);
        Task<bool> UpdateExistingDocument(List<CopyAttachmentVM> attachmentDetail);
        Task<bool> CopySelectedAttachmentsToDestination(CopySelectedAttachmentsVM attachmentDetail);
        Task CopyLegalLegislationSourceAttachments(CopyLegalLegislationSourceAttachmentsVM copyAttachments, string basePath);
        Task<bool> SaveDocumentPortfolioToDocument(CmsDocumentPortfolio documentPortfolio, string? fileName, string? physicalPath);
        Task<int> SaveDraftTemplateToDocument(CmsDraftedTemplate draft, string? fileName, string? physicalPath);
        Task<bool> SaveComsDraftTemplateToDocument(ComsDraftedTemplate draft, string? fileName, string? physicalPath);
        Task<bool> RemoveDocument(Guid referenceGuid);
        Task<bool> RemoveDocument(string referenceGuid, string isReferenceGuid);
        Task<bool> DeleteSelectedSourceDocument(List<TempAttachementVM> tempAttachementVM);
        Task<List<DMSDocumentListVM>> GetDocumentsList(DocumentListAdvanceSearchVM advanceSearchVM);
        Task<List<LLSLegalPrincipleDocumentVM>> GetLLSLegalPrincipleSourceDocuments(LLSLegalPrincipalDocumentSearchVM fileSearch);
        Task<List<LLSLegalPrinciplLegalAdviceDocumentVM>> GetLLSLegalPrincipleLegalAdviceSourceDocuments(LLSLegalPrincipalDocumentSearchVM fileSearch);
        Task<List<LLSLegalPrinciplOtherDocumentVM>> GetLLSLegalPrincipleOtherSourceDocuments(LLSLegalPrincipalDocumentSearchVM fileSearch);
        Task<LLSLegalPrincipleLinkedDocVM> GetLLSLegalPrincipleContentLinkedDocuments(Guid principleContentId);
        Task<DMSDocumentDetailVM> GetDocumentDetailById(int UploadedDocumentId);
        Task<bool> AddDocumentToFavourite(DMSDocumentListVM doc);
        Task RemoveFavouriteDocument(DMSDocumentListVM item);
        Task ShareDocument(DmsSharedDocument doc);
        Task<List<DmsFileTypes>> GetFileTypes();
        Task<List<DmsAddedDocumentReasonVM>> GetAddedDocumentReasonsByReferenceId(Guid referenceId);
        Task LinkDocumentToDestinationEntities(LinkDocumentsVM linkDocumentDetails, string encKey);
        Task<CaseTemplate> GetCaseTemplateDetail(int templateId);
        Task<UserPersonalInformation> GetUserPersonalInformationByUserId(string userId);


        #region Templates
        Task<List<CaseTemplateParameter>> GetTemplateParameters(int? moduleId);
        Task<CaseTemplate> SaveCaseTemplate(CaseTemplate template);
        Task<CmsDraftStamp> SaveDraftStamp(CmsDraftStamp template);
        Task<CaseTemplate> GetCaseTemplate(int templateId);
        Task<List<DMSTemplateListVM>> GetTemplatesList(TemplateListAdvanceSearchVM advanceSearchVM);
        Task<List<CaseTemplate>> GetHeaderFooterTemplates();
        Task UpdateTemplateStatus(bool isAtive, int id);
        #endregion

        #region Masked docu save legislation/principle
        Task<TempAttachementVM> SaveMaskedDocumentInOriginalDocumentFolderForTemparory(TempAttachementVM viewFileDetail);
        Task<bool> LegislationAttachmentSaveFromTempAttachementToUploadedDocument(LegalLegislation resultLegislationObject);
        #endregion

        #region Convert contract template into document and save into uploaded document table
        Task<bool> SaveContractTemplateToDocument(ConsultationRequest item, string? fileName, string? physicalPath);
        #endregion
        #region Convert MOM template into document and save into uploaded document table
        Task<bool> SaveMOMTemplateToDocument(MeetingMom meetingMom, string? fileName, string? physicalPath);
        #endregion
        #region  Kuwait Alyawm publication 
        Task<KayPublication> SaveKayPublicationDocument(KayPublication documentObj);
        Task SaveArchivedCaseDocuments(ArchivedCaseDocuments documentObj);
        Task<List<DMSKayPublicationDocumentListVM>> GetKayDocumentsList(KayDocumentListAdvanceSearchVM advanceSearchVM);
        Task<DMSKayPublicationDocumentListVM> GetkayDocumentAccordingEditionNumber(string editionNumber);
        Task<List<LLSLegalPrincipleKuwaitAlYoumDocuments>> GetKayDocumentsListForLLSLegalPrinciple(LLSLegalPrincipalDocumentSearchVM advanceSearchVM);
        #endregion
        #region Moj Roll
        Task<UploadedDocument> SaveUploadedDocument(UploadedDocument documentObj);
        Task<UploadedDocument> GetMojRollDocumentById(int? DocumentId);
        #endregion
        #region  MOJ Images Pushing Document to DMS

        Task<UploadedDocument> SaveMojImageDocument(MojDocument mojDocument);
        Task<List<MojDocumentVM>> GetMojImageDocumentList(MojDocumentAdvanceSearchVM advanceSearchVM);
        Task<List<MojDocumentVM>> GetMojDocumentByCaseNumber(string caseNumber);
        #endregion

        Task<List<UploadedDocumentVM>> GetLLSLegalPrincipleReferenceUploadedAttachements(Guid principleId);
        Task<int> UpdateDocument(UploadedDocument uploadedDocument);
        Task GetLatestVersionAndUpdateDocumentVersion(Guid versionId);
        Task<UploadedDocument> GetSignatureImagePath(string userId);
        Task<string> GetCivilId(string UserId);
        Task SaveDSPRequestLog(DSPRequestLog requestLog);
        Task<DSPRequestLog> UpdateDSPRequestLog(string RequestId, string RequestStatus);
        Task<string> GetDSPSigningRequestStatus(string RequestId);
        Task<bool> GetIsAlreadySigned(string civilId, int documentId);
        Task SaveDSPAuthenticationRequestLog(DSPAuthenticationRequestLog authenticationRequestLog);
        Task UpdateDSPAuthenticationRequestLog(DSPAuthenticationResponse dSPAuthenticationResponse);
        Task<DSPAuthenticationRequestLog> GetDSPAuthenticationRequestLog(string RequestId);
        Task<List<DsSigningMethods>> GetSigningMethods();
        Task<string> GetSignatureProfileName(int methodId);
        #region Save Literature Uploaded Document ( "this region only used for Adding and editing Literature" ) 
        Task<bool> SaveLiteratureTempAttachmentToUploadedDocument(FileUploadVM item);
        Task<bool> GetUploadedAttachementAndWithNewOne(FileUploadVM item);
        Task<List<TempAttachement>> CheckingAttachementInTemp(FileUploadVM item);
        #endregion
        Task<KayPublication> GetAttachementById(int Id);

        Task RemoveDocumentByReferenceGuidAndAttachmentTypeId(string basePath, Guid referenceGuid, int AttachmentTypeId);

        Task<bool> RemoveDocumentFromTemp(Guid referenceGuid);
        Task<string> GetTemById(Guid referenceGuid);

    }
}
