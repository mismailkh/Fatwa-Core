using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_DOMAIN.Models;
using FATWA_INFRASTRUCTURE.Database;
using FATWA_DOMAIN.Models.ViewModel.Shared;
using Microsoft.EntityFrameworkCore;
using FATWA_DOMAIN.Models.CaseManagment;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.WorkflowEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Interfaces.CaseManagement;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using Newtonsoft.Json;
using FATWA_INFRASTRUCTURE.Repository.Communications;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.WebSockets;
using AutoMapper;
using static FATWA_DOMAIN.Enums.UserEnum;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using FATWA_INFRASTRUCTURE.Repository.CommonRepos;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using System.Text.RegularExpressions;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_INFRASTRUCTURE.Repository.PatternNumber;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace FATWA_INFRASTRUCTURE.Repository.CaseManagement
{
    //<History Author = 'Hassan Abbas' Date='2022-11-02' Version="1.0" Branch="master"> Case Template Repo</History>
    public class CmsCaseTemplateRepository : ICmsCaseTemplate
    {
        private readonly DatabaseContext _dbContext;
        private readonly DmsDbContext _dbDmsContext;
        private List<CaseTemplateParametersVM> _Parameters;
        private List<CmsDraftedDocumentVM> _CmsDraftedDocumentVM;
        private readonly CommunicationRepository _communicationRepository;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;
        private readonly CMSCOMSInboxOutboxPatternNumberRepository _cMSCOMSInboxOutboxPatternNumberRepository;
        public CommonRepository _commonRepo { get; }
        private readonly WorkflowRepository _workflowRepository;

        public CmsCaseTemplateRepository(DatabaseContext dbContext, DmsDbContext dmsDbContext, IServiceScopeFactory serviceScopeFactory, IMapper mapper, CommonRepository commonRepo, CMSCOMSInboxOutboxPatternNumberRepository CMSCOMSInboxOutboxPatternNumberRepository, WorkflowRepository workflowRepository)
        {
            _dbContext = dbContext;
            _dbDmsContext = dmsDbContext;
            _serviceScopeFactory = serviceScopeFactory;
            using var scope = _serviceScopeFactory.CreateScope();
            _communicationRepository = scope.ServiceProvider.GetRequiredService<CommunicationRepository>();
            _mapper = mapper;
            _commonRepo = commonRepo;
            _cMSCOMSInboxOutboxPatternNumberRepository = CMSCOMSInboxOutboxPatternNumberRepository;
            _workflowRepository = workflowRepository;
        }

        #region Get Template and related data Fucntions

        //<History Author = 'Hassan Abbas' Date='2022-10-02' Version="1.0" Branch="master"> Get Case Template Sections</History>
        public async Task<List<CaseTemplateSectionsVM>> GetTemplateSections(int templateId)
        {
            try
            {
                string StoredProc = $"exec pCaseTemplateSectionsList @templateId = '{templateId}'";
                return await _dbContext.CaseTemplateSectionsVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-02' Version="1.0" Branch="master"> Get Case Template Sections</History>
        public async Task<List<CaseTemplateSectionsVM>> GetDraftedTemplateSections(Guid versionId)
        {
            try
            {
                string StoredProc = $"exec pDraftedTemplateSectionsList @versionId = '{versionId}'";
                return await _dbContext.CaseTemplateSectionsVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //<History Author = 'Hassan Abbas' Date='2022-10-02' Version="1.0" Branch="master"> Get Case Template Parameters</History>
        public async Task<List<CaseTemplateParametersVM>> GetTemplateSectionParameters(Guid templateSectionId)
        {
            try
            {
                string StoredProc = $"exec pCaseTemplateSectionParametersList @templateSectionId = '{templateSectionId}'";
                return await _dbContext.CaseTemplateParametersVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-02' Version="1.0" Branch="master"> Get Case Template Parameters</History>
        public async Task<List<CaseTemplateParametersVM>> GetDraftedTemplateSectionParameters(Guid templateSectionId)
        {
            try
            {
                string StoredProc = $"exec pDraftedTemplateSectionParametersList @templateSectionId = '{templateSectionId}'";
                return await _dbContext.CaseTemplateParametersVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //<History Author = 'Hassan Abbas' Date='2022-10-02' Version="1.0" Branch="master"> Get Doc detail with sections and parameters</History>
        public async Task<CmsDraftedDocumentDetailVM> GetDraftDocDetailWithSectionAndParameters(Guid draftId, Guid? versionId)
        {
            try
            {
                CmsDraftedDocumentDetailVM document;
                if (versionId == null)
                {
                    var draftVersions = await _dbContext.CmsDraftedTemplateVersions.Where(x => x.DraftedTemplateId == draftId).OrderByDescending(x => x.CreatedDate).ToListAsync();
                    versionId = draftVersions.FirstOrDefault().VersionId;
                }
                string StoredProc = $"exec pCaseDraftDocumentDetailById @draftId='{draftId}', @versionId='{versionId}'";
                document = (await _dbContext.CmsDraftedDocumentDetailVM.FromSqlRaw(StoredProc).ToListAsync()).FirstOrDefault();
                if (document != null)
                {
                    document.VersionId = versionId;
                    StoredProc = $"exec pCaseDraftDocumentSectionsList @VersionId='{versionId}'";
                    document.TemplateSections = await _dbContext.CaseTemplateSectionsVM.FromSqlRaw(StoredProc).ToListAsync();
                    foreach (var section in document.TemplateSections)
                    {
                        StoredProc = $"exec pCaseDraftDocumentSectionParametersList @draftedTemplateSectionId='{section.Id}'";
                        section.SectionParameters = await _dbContext.CaseTemplateParametersVM.FromSqlRaw(StoredProc).ToListAsync();
                    }
                }
                return document;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-02' Version="1.0" Branch="master"> Get draft doc detail by id</History>
        public async Task<CmsDraftedTemplate> GetDraftedTemplateDetailById(Guid draftId, Guid versionId)
        {
            try
            {
                var draftedTemplate = await _dbContext.CmsDraftedTemplate.FindAsync(draftId);
                draftedTemplate.DraftedTemplateVersion = await _dbContext.CmsDraftedTemplateVersions.FindAsync(versionId);
                var TemplateSections = await _dbContext.CmsDraftedTemplateSection.Where(x => x.DraftedTemplateVersionId == versionId).ToListAsync();
                draftedTemplate.TemplateSections = _mapper.Map<List<CaseTemplateSectionsVM>>(TemplateSections);
                foreach (var TemplateSection in draftedTemplate.TemplateSections)
                {
                    var parameter = await _dbContext.CmsDraftedTemplateSectionParameter.Where(x => x.DraftedTemplateSectionId == TemplateSection.Id).ToListAsync();
                    TemplateSection.SectionParameters = _mapper.Map<List<CaseTemplateParametersVM>>(parameter);
                }
                return draftedTemplate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<DmsAddedDocument> GetDMSDocumentDetailById(Guid documentId, Guid versionId)
        {
            try
            {
                var documentTemplate = await _dbDmsContext.DmsAddedDocuments.FindAsync(documentId);
                documentTemplate.DocumentVersion = await _dbDmsContext.DmsAddedDocumentVersions.FindAsync(versionId);
                return documentTemplate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-02' Version="1.0" Branch="master"> Get Case Template Content</History>
        //<History Author = 'Umer Zaman' Date='2023-11-10' Version="1.0" Branch="master"> Modified get case template content according to contract temple scenario.</History>

        public async Task<CaseTemplate> GetCaseTemplateContent(int templateId, Guid? DraftId, Guid? VersionId)
        {
            try
            {
                CaseTemplate caseTemplate = new CaseTemplate();
                caseTemplate = await _dbContext.CaseTemplate.Where(x => x.Id == templateId).FirstOrDefaultAsync();
                if (VersionId != null)
                {
                    var draftVersions = await _dbContext.CmsDraftedTemplateVersions.Where(x => x.DraftedTemplateId == DraftId).OrderByDescending(x => x.CreatedDate).ToListAsync();
                    caseTemplate.Content = draftVersions.FirstOrDefault().Content;
                }
                return caseTemplate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<CaseTemplate>> GetHeaderFooter()
        {
            try
            {
                var response = await _dbContext.CaseTemplate.Where(x => x.Id >= (int)CaseTemplateEnum.Footer && x.Id <= (int)CaseTemplateEnum.HeaderAr).ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //<History Author = 'Hassan Abbas' Date='2022-10-02' Version="1.0" Branch="master"> Get Case Template Content</History>
        public async Task<CaseTemplate> GetCaseTemplateDetail(int templateId)
        {
            try
            {
                return _dbContext.CaseTemplate.Where(t => t.Id == templateId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-02' Version="1.0" Branch="master"> Get Case Template Content</History>
        public async Task GetTemplateDataFromCaseFile(Guid fileId)
        {
            try
            {
                //return _dbContext.CaseTemplate.Where(t => t.Id == templateId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-02' Version="1.0" Branch="master"> Get Version and Draft number for new doc</History>
        public async Task<CmsDraftedTemplate> GetDraftNumberVersionNumber(Guid? draftId, Guid? versionId)
        {
            try
            {
                CmsDraftedTemplate draftedTemplate = new CmsDraftedTemplate();
                if (draftId == null)
                {
                    if (draftedTemplate.DraftedTemplateVersion == null)
                        draftedTemplate.DraftedTemplateVersion = new CmsDraftedTemplateVersions();
                    draftedTemplate.Id = Guid.NewGuid();
                    draftedTemplate.DraftNumber = _dbContext.CmsDraftedTemplate.Any() ? await _dbContext.CmsDraftedTemplate.Select(x => x.DraftNumber).MaxAsync() + 1 : 1;
                    if (draftedTemplate.DraftNumber == CmsDraftedTemplate.LatestDraftNumber)
                    {
                        draftedTemplate.DraftNumber += 1;
                        CmsDraftedTemplate.LatestDraftNumber = draftedTemplate.DraftNumber;
                    }
                    else
                    {
                        CmsDraftedTemplate.LatestDraftNumber = draftedTemplate.DraftNumber;
                    }
                    draftedTemplate.DraftedTemplateVersion.VersionNumber = 0.1M;
                    draftedTemplate.CreatedDate = DateTime.Now;
                    draftedTemplate.DraftedTemplateVersion.CreatedDate = DateTime.Now;
                }
                else
                {
                    draftedTemplate = _dbContext.CmsDraftedTemplate.Where(d => d.Id == draftId).FirstOrDefault();
                    draftedTemplate.DraftedTemplateVersion = _dbContext.CmsDraftedTemplateVersions.Where(d => d.VersionId == versionId).FirstOrDefault();
                }
                return draftedTemplate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        #endregion

        #region Create Case Draft Document
        //<History Author = 'Hassan Abbas' Date='2022-10-02' Version="1.0" Branch="master"> Create Case Draft Doc</History>
        public async Task CreateCaseDraftDocument(CmsDraftedTemplate document)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (document.DraftedTemplateVersion.StatusId == (int)DraftVersionStatusEnum.InReview)
                        {
                            if (document.DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo || document.DraftEntityType == (int)DraftEntityTypeEnum.FileNeedMoreInfo || document.DraftEntityType == (int)DraftEntityTypeEnum.CaseNeedMoreInfo)
                            {
                                SendCommunicationVM communication = JsonConvert.DeserializeObject<SendCommunicationVM>(document.Payload);
                                communication.Communication.ColorId = (int)CommunicationColorEnum.Yellow;
                                List<SendCommunicationVM> listCommunication = new List<SendCommunicationVM>();
                                if (communication.CommunicationResponse.PartyEntityIds.Count() > 0)
                                {
                                    bool isFirstItem = true;
                                    foreach (var entity in communication.CommunicationResponse.PartyEntityIds)
                                    {
                                        SendCommunicationVM copyCommunication = new SendCommunicationVM();
                                        copyCommunication = JsonConvert.DeserializeObject<SendCommunicationVM>(document.Payload);
                                        copyCommunication.Communication.ColorId = (int)CommunicationColorEnum.Yellow;

                                        if (isFirstItem)
                                        {
                                            copyCommunication.CommunicationResponse.PartyEntityId = entity;
                                            isFirstItem = false;
                                        }
                                        else
                                        {
                                            copyCommunication.Communication.CommunicationId = Guid.NewGuid();
                                            copyCommunication.CommunicationResponse.CommunicationResponseId = Guid.NewGuid();
                                            copyCommunication.CommunicationResponse.CommunicationId = copyCommunication.Communication.CommunicationId;
                                            copyCommunication.CommunicationTargetLink.CommunicationId = copyCommunication.Communication.CommunicationId;
                                            copyCommunication.CommunicationResponse.PartyEntityId = entity;
                                            copyCommunication.LinkTarget.Where(x => x.ReferenceId == communication.Communication.CommunicationId).ToList().ForEach(y => { y.ReferenceId = copyCommunication.Communication.CommunicationId; y.LinkTargetId = Guid.NewGuid(); });
                                            copyCommunication.LinkTarget.Where(x => x.ReferenceId == communication.CommunicationResponse.CommunicationResponseId).ToList().ForEach(y => { y.ReferenceId = copyCommunication.CommunicationResponse.CommunicationResponseId; y.LinkTargetId = Guid.NewGuid(); });
                                            copyCommunication.LinkTarget.Where(x => x.IsPrimary == true).ToList().ForEach(y => { y.LinkTargetId = Guid.NewGuid(); });
                                        }
                                        await SaveCommunicationRecord(copyCommunication, _dbContext);
                                        listCommunication.Add(copyCommunication);
                                    }
                                }
                                else
                                {
                                    await SaveCommunicationRecord(communication, _dbContext);
                                    listCommunication.Add(communication);
                                }
                                document.Payload = JsonConvert.SerializeObject(listCommunication);
                            }
                            await _dbContext.CmsDraftedTemplate.AddAsync(document);
                            await _dbContext.SaveChangesAsync();
                            document.DraftedTemplateVersion.DraftedTemplateId = document.Id;
                            await _dbContext.CmsDraftedTemplateVersions.AddAsync(document.DraftedTemplateVersion);
                            await _dbContext.SaveChangesAsync();
                            await _workflowRepository.LinkEntityWithActiveWorkflow(document, _dbContext, document.subModuleId);
                            transaction.Commit();
                        }
                        else
                        {
                            await _dbContext.CmsDraftedTemplate.AddAsync(document);
                            await _dbContext.SaveChangesAsync();
                            document.DraftedTemplateVersion.DraftedTemplateId = document.Id;
                            await _dbContext.CmsDraftedTemplateVersions.AddAsync(document.DraftedTemplateVersion);
                            await _dbContext.SaveChangesAsync();
                            if(document.DraftedTemplateVersion.StatusId == (int)DraftVersionStatusEnum.Draft)
                            {
                                await CreateDraftTemplateLogs(document, document.DraftedTemplateVersion.DraftActionId);
                            }
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public async Task CreateDraftTemplateLogs(CmsDraftedTemplate document, int actionId)
        {
            try
            {
                CmsDraftedTemplateVersionLogs cmsDraftedTemplateVersionLogs = new CmsDraftedTemplateVersionLogs()
                {
                    Id = Guid.NewGuid(),
                    VersionId = document.DraftedTemplateVersion.VersionId,
                    UserId = (Guid)document.UserId,
                    ActionId = actionId,
                    CreatedBy = document.userName,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = null,
                    ModifiedDate = null,
                    ReviewerUserId = actionId == (int)DraftActionIdEnum.CreatedAndDraft || actionId == (int)DraftActionIdEnum.EditedAndDraft ? "" :
                    (!string.IsNullOrEmpty(document.DraftedTemplateVersion.ReviewerUserId) ? document.DraftedTemplateVersion.ReviewerUserId : document.DraftedTemplateVersion.ReviewerRoleId)
                };
                await _dbContext.CmsDraftedTemplateVersionLogs.AddAsync(cmsDraftedTemplateVersionLogs);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task SaveCommunicationRecord(SendCommunicationVM sendCommunication, DatabaseContext dbContext)
        {
            try
            {
                await _communicationRepository.SaveCommunication(sendCommunication.Communication, dbContext);
                await _communicationRepository.SaveCommResponse(sendCommunication.CommunicationResponse, sendCommunication.Communication.CommunicationId, dbContext);
                if (sendCommunication.CommunicationResponse.EntityIds != null)
                {
                    await _communicationRepository.SaveCommResponseGovtEntit(sendCommunication.CommunicationResponse, dbContext);
                }
                await _communicationRepository.SaveCommunicationTargetLink(sendCommunication.CommunicationTargetLink, dbContext);
                await _communicationRepository.SaveLinkTarget(sendCommunication.LinkTarget, sendCommunication.CommunicationTargetLink.TargetLinkId, dbContext);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task CreateDraftDocumentVersion(CmsDraftedTemplate DraftedTemplate)
        {
            try
            {
                _dbContext.CmsDraftedTemplate.Update(DraftedTemplate);
                var value = await _dbContext.CmsDraftedTemplateVersions.Where(x => x.DraftedTemplateId == DraftedTemplate.Id && x.StatusId == (int)DraftVersionStatusEnum.Draft).AnyAsync();
                if (value == true)
                {
                    _dbContext.CmsDraftedTemplateVersions.Update(DraftedTemplate.DraftedTemplateVersion);
                }
                else
                {
                    await _dbContext.CmsDraftedTemplateVersions.AddAsync(DraftedTemplate.DraftedTemplateVersion);
                }
                await _dbContext.SaveChangesAsync();
                if (DraftedTemplate.DraftedTemplateVersion.StatusId == (int)DraftVersionStatusEnum.Draft)
                {
                    await CreateDraftTemplateLogs(DraftedTemplate, DraftedTemplate.DraftedTemplateVersion.DraftActionId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Update Draft Document

        //<History Author = 'Hassan Abbas' Date='2022-10-02' Version="1.0" Branch="master"> Create Case Draft Doc</History>
        public async Task UpdateCaseDraftDocument(CmsDraftedTemplate document)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.CmsDraftedTemplate.Update(document);
                        if (document.DraftedTemplateVersion.StatusId == (int)DraftVersionStatusEnum.Draft)
                        {
                            _dbContext.CmsDraftedTemplateVersions.Update(document.DraftedTemplateVersion);
                            await _dbContext.SaveChangesAsync();
                        }
                        else if (document.DraftedTemplateVersion.StatusId == (int)DraftVersionStatusEnum.Reject)
                        {
                            document.DraftedTemplateVersion.VersionId = Guid.NewGuid();
                            document.DraftedTemplateVersion.CreatedBy = document.CreatedBy;
                            document.DraftedTemplateVersion.CreatedDate = document.CreatedDate;
                            if (document.IsSubmit)
                            {
                                document.DraftedTemplateVersion.StatusId = document.DraftedTemplateVersion.StatusId == (int)CaseDraftDocumentStatusEnum.RejectedByHOS ? (int)CaseDraftDocumentStatusEnum.ApproveBySupervisor : (int)CaseDraftDocumentStatusEnum.InReview;
                                document.DraftedTemplateVersion.VersionNumber = Decimal.Add(document.DraftedTemplateVersion.VersionNumber, 0.1M);
                                _dbContext.CmsDraftedTemplateVersions.Add(document.DraftedTemplateVersion);
                                await _dbContext.SaveChangesAsync();
                                if (document.DraftedTemplateVersion.StatusId == (int)DraftVersionStatusEnum.InReview)
                                {
                                    //await LinkEntityWithActiveWorkflow(document, _dbContext);
                                }
                            }
                            else
                            {
                                document.DraftedTemplateVersion.StatusId = (int)DraftVersionStatusEnum.Draft;
                                document.DraftedTemplateVersion.VersionNumber = Decimal.Add(document.DraftedTemplateVersion.VersionNumber, 0.1M);
                                _dbContext.CmsDraftedTemplateVersions.Add(document.DraftedTemplateVersion);
                                await _dbContext.SaveChangesAsync();
                            }

                        }
                        else if (document.DraftedTemplateVersion.StatusId == (int)DraftVersionStatusEnum.InReview)
                        {
                            _dbContext.CmsDraftedTemplateVersions.Update(document.DraftedTemplateVersion);
                            await _dbContext.SaveChangesAsync();
                            await _workflowRepository.LinkEntityWithActiveWorkflow(document, _dbContext, document.subModuleId);

                            if (document.DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo || document.DraftEntityType == (int)DraftEntityTypeEnum.FileNeedMoreInfo || document.DraftEntityType == (int)DraftEntityTypeEnum.CaseNeedMoreInfo)
                            {

                                SendCommunicationVM sendCommunication = JsonConvert.DeserializeObject<SendCommunicationVM>(document.Payload);
                                if (_dbContext.Communications.Find(sendCommunication.Communication.CommunicationId) == null)
                                {
                                    await _communicationRepository.SaveCommunication(sendCommunication.Communication, _dbContext);
                                    await _communicationRepository.SaveCommResponse(sendCommunication.CommunicationResponse, sendCommunication.Communication.CommunicationId, _dbContext);
                                    if (sendCommunication.CommunicationResponse.EntityIds != null)
                                    {
                                        await _communicationRepository.SaveCommResponseGovtEntit(sendCommunication.CommunicationResponse, _dbContext);
                                    }
                                    await _communicationRepository.SaveCommunicationTargetLink(sendCommunication.CommunicationTargetLink, _dbContext);
                                    await _communicationRepository.SaveLinkTarget(sendCommunication.LinkTarget, sendCommunication.CommunicationTargetLink.TargetLinkId, _dbContext);
                                }
                            }
                        }
                        else if (document.DraftedTemplateVersion.StatusId == (int)DraftVersionStatusEnum.Approve)
                        {
                            _dbContext.CmsDraftedTemplateVersions.Update(document.DraftedTemplateVersion);
                            await _dbContext.SaveChangesAsync();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task UpdateDraftDocumentStatus(CmsDraftedTemplateVersions document)
        {
            try
            {

                var drafteDocument = _dbContext.CmsDraftedTemplateVersions.Where(x => x.VersionId == document.OldVersionId).FirstOrDefault();
                var attachmentTypeId = _dbContext.CmsDraftedTemplate.Where(x => x.Id == drafteDocument.DraftedTemplateId).FirstOrDefault().AttachmentTypeId;
                document.DraftedTemplateId = drafteDocument.DraftedTemplateId;
                document.Content = drafteDocument.Content;
                if (document.StatusId == (int)DraftVersionStatusEnum.SendToMOJ)
                {
                    document.VersionNumber = Decimal.Add(drafteDocument.VersionNumber, 1m);
                    _dbContext.CmsDraftedTemplateVersions.Add(document);
                    await _dbContext.SaveChangesAsync();
                    //For Notification
                    document.NotificationParameter.Type = _dbDmsContext.AttachmentType.Where(x => x.AttachmentTypeId == attachmentTypeId)
                                                        .Select(x => x.Type_En + "/" + x.Type_Ar).FirstOrDefault();
                    document.NotificationParameter.FileNumber = _dbContext.CaseFiles.Where(x => x.FileId == document.FileId).Select(x => x.FileNumber).FirstOrDefault();
                }
                else
                {
                    var DocumentVersion = _dbContext.CmsDraftedTemplateVersions.Where(x => x.DraftedTemplateId == document.DraftedTemplateId && x.StatusId == (int)DraftVersionStatusEnum.SendToMOJ).FirstOrDefault();
                    if (DocumentVersion != null)
                    {
                        DocumentVersion.StatusId = (int)DraftVersionStatusEnum.RegisteredInMOJ;
                        DocumentVersion.CreatedBy = document.CreatedBy;
                        DocumentVersion.CreatedDate = DateTime.Now;
                        DocumentVersion.ModifiedBy = document.CreatedBy;
                        DocumentVersion.ModifiedDate = DateTime.Now;
                        _dbContext.Entry(DocumentVersion).State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Workflow

        //<History Author = 'Hassan Abbas' Date='2022-11-07' Version="1.0" Branch="master">Create Workflow Instance and link entity with workflow</History>
        private async Task LinkEntityWithActiveWorkflow(CmsDraftedTemplate document, DatabaseContext dbContext)
        {
            try
            {
                var moduleTriggerId = document.ModuleId == (int)WorkflowModuleEnum.CaseManagement ?
                   (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft :
                   (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft;
                string StoredProc = $"exec pWorkflowSelAdvanceSearch @statusId = '{(int)WorkflowStatusEnum.Active}', @moduleId = '{document.ModuleId}', @moduleTriggerId = '{moduleTriggerId}',@attachmenttypeId ='{document.AttachmentTypeId}', @submoduleId='{document.subModuleId}'";
                var activeWorkflow = await dbContext.WorkflowVM.FromSqlRaw(StoredProc).ToListAsync();
                if (activeWorkflow?.Count() > 0)
                {
                    WorkflowActivity firstActivity = await dbContext.WorkflowActivity.Where(a => a.WorkflowId == (int)activeWorkflow.FirstOrDefault().WorkflowId).OrderBy(a => a.SequenceNumber).FirstOrDefaultAsync();
                    WorkflowInstance workflowInstance = new WorkflowInstance { ReferenceId = document.Id, StatusId = (int)WorkflowInstanceStatusEnum.InProgress, WorkflowId = (int)activeWorkflow.FirstOrDefault().WorkflowId, WorkflowActivityId = firstActivity.WorkflowActivityId, IsSlaExecuted = false };
                    SLA sla = await dbContext.SLA.Where(s => s.WorkflowActivityId == workflowInstance.WorkflowActivityId).FirstOrDefaultAsync();
                    if (sla != null)
                    {
                        workflowInstance.ApplySla = true;
                        workflowInstance.SlaStartDate = DateTime.Now.Date.AddDays(1);
                        workflowInstance.SlaEndDate = workflowInstance.SlaStartDate.AddDays(sla.Duration - 1);
                    }
                    else
                    {
                        workflowInstance.ApplySla = false;
                        workflowInstance.SlaStartDate = DateTime.Now.Date;
                        workflowInstance.SlaEndDate = DateTime.Now.Date;
                    }
                    await dbContext.WorkflowInstance.AddAsync(workflowInstance);
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task UpdateLinkedWorkflowInstance(CmsDraftedTemplate document, DatabaseContext dbContext)
        {
            try
            {
                string StoredProc = $"exec pWorkflowSelAdvanceSearch @statusId = '{(int)WorkflowStatusEnum.Active}', @moduleId = '{(int)WorkflowModuleEnum.LPSPrinciple}'";
                var activeWorkflow = await dbContext.WorkflowVM.FromSqlRaw(StoredProc).ToListAsync();
                if (activeWorkflow?.Count() > 0)
                {
                    WorkflowActivity firstActivity = await dbContext.WorkflowActivity.Where(a => a.WorkflowId == (int)activeWorkflow.FirstOrDefault().WorkflowId).OrderBy(a => a.SequenceNumber).FirstOrDefaultAsync();
                    WorkflowInstance workflowInstance = await dbContext.WorkflowInstance.Where(w => w.ReferenceId == document.Id).FirstOrDefaultAsync();
                    if (workflowInstance != null)
                    {
                        workflowInstance.WorkflowActivityId = firstActivity.WorkflowActivityId;
                        workflowInstance.WorkflowId = (int)activeWorkflow.FirstOrDefault().WorkflowId;
                        workflowInstance.StatusId = (int)WorkflowInstanceStatusEnum.InProgress;
                        workflowInstance.IsSlaExecuted = false;

                        SLA sla = await dbContext.SLA.Where(s => s.WorkflowActivityId == workflowInstance.WorkflowActivityId).FirstOrDefaultAsync();
                        if (sla != null)
                        {
                            workflowInstance.ApplySla = true;
                            workflowInstance.SlaStartDate = DateTime.Now.Date.AddDays(1);
                            workflowInstance.SlaEndDate = workflowInstance.SlaStartDate.AddDays(sla.Duration - 1);
                        }
                        else
                        {
                            workflowInstance.ApplySla = false;
                            workflowInstance.SlaStartDate = DateTime.Now.Date;
                            workflowInstance.SlaEndDate = DateTime.Now.Date;
                        }
                        dbContext.Entry(workflowInstance).State = EntityState.Modified;
                        await dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        workflowInstance = new WorkflowInstance { ReferenceId = document.Id, StatusId = (int)WorkflowInstanceStatusEnum.InProgress, WorkflowId = (int)activeWorkflow.FirstOrDefault().WorkflowId, WorkflowActivityId = firstActivity.WorkflowActivityId, IsSlaExecuted = false };
                        SLA sla = await dbContext.SLA.Where(s => s.WorkflowActivityId == workflowInstance.WorkflowActivityId).FirstOrDefaultAsync();
                        if (sla != null)
                        {
                            workflowInstance.ApplySla = true;
                            workflowInstance.SlaStartDate = DateTime.Now.Date.AddDays(1);
                            workflowInstance.SlaEndDate = workflowInstance.SlaStartDate.AddDays(sla.Duration - 1);
                        }
                        else
                        {
                            workflowInstance.ApplySla = false;
                            workflowInstance.SlaStartDate = DateTime.Now.Date;
                            workflowInstance.SlaEndDate = DateTime.Now.Date;
                        }
                        await dbContext.WorkflowInstance.AddAsync(workflowInstance);
                        await dbContext.SaveChangesAsync();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get Draft Docs List

        //<History Author = 'Hassan Abbas' Date='2022-10-02' Version="1.0" Branch="master"> Get Case Draft Documents List</History>
        public async Task<List<CmsDraftedDocumentVM>> GetCaseDraftDocuments(string userName, AdvanceSearchCmsDraftedDocumentVM advanceSearchVM)
        {
            try
            {
                string start_From = advanceSearchVM.Start_From != null ? Convert.ToDateTime(advanceSearchVM.Start_From).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string end_To = advanceSearchVM.End_To != null ? Convert.ToDateTime(advanceSearchVM.End_To).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string StoredProc = $"exec pCaseDraftDocumentsList @userName='{userName}',@DraftNumber='{advanceSearchVM.DraftNumber}',@DocumentType='{advanceSearchVM.Document_Type}',@start_From='{advanceSearchVM.Start_From}',@end_To='{advanceSearchVM.End_To}'";
                _CmsDraftedDocumentVM = await _dbContext.CmsDraftedDocumentVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _CmsDraftedDocumentVM;
        }

        #endregion

        #region Get Draft Docs List

        //<History Author = 'Nabeel ur Rehman' Date='2022-10-02' Version="1.0" Branch="master"> Get Case Draft Documents List by Case</History>
        //<History Author = 'Hassan Abbas' Date='2023-01-02' Version="1.0" Branch="master"> Moved method from CaseRequestRepo</History>
        public async Task<List<CmsDraftedDocumentVM>> GetCaseDraftListByReferenceId(Guid referenceId)
        {
            try
            {
                string StoredProc = $"exec pGetDraftCaseRequestList @ReferenceId='{referenceId}'";
                var response = await _dbContext.CmsDraftedDocumentVM.FromSqlRaw(StoredProc).ToListAsync();
                return response;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion

        #region Get Draft Docs Reasons

        //<History Author = 'Hassan Abbas' Date='2023-01-02' Version="1.0" Branch="master"> Moved method from CaseRequestRepo</History>
        public async Task<List<CmsDraftedDocumentReasonVM>> GetDraftDocumentReasonsByReferenceId(Guid referenceId)
        {
            try
            {
                string StoredProc = $"exec pCmsDraftedDocumentReasonList @ReferenceId='{referenceId}'";
                return await _dbContext.CmsDraftedDocumentReasonVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Draft Docs Opinion

        //<History Author = 'Muhammad Zaeem' Date='2023-09-24' Version="1.0" Branch="master"> Get opinion by version id</History>
        public async Task<List<CmsDraftedDocumentOpioninVM>> GetDraftDocumentOpinionByReferenceId(Guid referenceId)
        {
            try
            {
                string StoredProc = $"exec pCmsDraftedDocumentOpinionList @ReferenceId='{referenceId}'";
                return await _dbContext.CmsDraftedDocumentOpioninVMs.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Consultation Draft DraftNumberVersionNumber
        public async Task<ComsDraftedTemplate> GetComsDraftNumberVersionNumber(Guid? draftId)
        {
            try
            {
                ComsDraftedTemplate draftedTemplate = new ComsDraftedTemplate();
                if (draftId == null)
                {
                    draftedTemplate.Id = Guid.NewGuid();
                    draftedTemplate.DraftNumber = _dbContext.ComsDraftedTemplate.Any() ? await _dbContext.ComsDraftedTemplate.Select(x => x.DraftNumber).MaxAsync() + 1 : 1;
                    draftedTemplate.VersionNumber = 1.0;
                    draftedTemplate.CreatedDate = DateTime.Now;
                    draftedTemplate.ModifiedDate = DateTime.Now;
                }
                else
                {
                    draftedTemplate = _dbContext.ComsDraftedTemplate.Where(d => d.Id == draftId).FirstOrDefault();
                }
                return draftedTemplate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Create Consultation Draft Document

        //<History Author = 'Hassan Abbas' Date='2022-10-02' Version="1.0" Branch="master"> Create Case Draft Doc</History>
        public async Task CreateConsultationDraftDocument(ComsDraftedTemplate document)
        {

            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (document.StatusId == (int)CaseDraftDocumentStatusEnum.InReview)
                        {
                            await _dbContext.ComsDraftedTemplate.AddAsync(document);
                            await _dbContext.SaveChangesAsync();
                            await SaveComsDraftSections(document, _dbContext);
                            await ComsLinkEntityWithActiveWorkflow(document, _dbContext);
                            if (document.DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo || document.DraftEntityType == (int)DraftEntityTypeEnum.FileNeedMoreInfo || document.DraftEntityType == (int)DraftEntityTypeEnum.CaseNeedMoreInfo)
                            {
                                SendCommunicationVM sendCommunication = JsonConvert.DeserializeObject<SendCommunicationVM>(document.Payload);
                                await _communicationRepository.SaveCommunication(sendCommunication.Communication, _dbContext);
                                await _communicationRepository.SaveCommResponse(sendCommunication.CommunicationResponse, sendCommunication.Communication.CommunicationId, _dbContext);
                                if (sendCommunication.CommunicationResponse.EntityIds != null)
                                {
                                    await _communicationRepository.SaveCommResponseGovtEntit(sendCommunication.CommunicationResponse, _dbContext);
                                }
                                await _communicationRepository.SaveCommunicationTargetLink(sendCommunication.CommunicationTargetLink, _dbContext);
                                await _communicationRepository.SaveLinkTarget(sendCommunication.LinkTarget, sendCommunication.CommunicationTargetLink.TargetLinkId, _dbContext);
                            }
                            transaction.Commit();
                        }
                        else
                        {
                            await _dbContext.ComsDraftedTemplate.AddAsync(document);
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-02' Version="1.0" Branch="master"> Save Draft Doc Sections</History>

        public async Task SaveComsDraftSections(ComsDraftedTemplate document, DatabaseContext dbContext)
        {
            try
            {
                //Save sections
                foreach (var section in document.TemplateSections)
                {
                    ComsDraftedTemplateSection templateSection = new ComsDraftedTemplateSection
                    {
                        DraftedTemplateId = document.Id,
                        SectionId = section.SectionId,
                        AdditionalName = document.TemplateId == (int)CaseTemplateEnum.BlankTemplate ? section.SectionNameEn : "",
                        SequenceNumber = section.SequenceNumber
                    };

                    await dbContext.ComsDraftedTemplateSection.AddAsync(templateSection);
                    await dbContext.SaveChangesAsync();

                    //Save Parameters in section
                    foreach (var parm in section.SectionParameters)
                    {
                        ComsDraftedTemplateSectionParameter sectionParm = new ComsDraftedTemplateSectionParameter
                        {
                            DraftedTemplateSectionId = templateSection.Id,
                            ParameterId = (int)parm.ParameterId,
                            Value = parm.Value
                        };
                        await dbContext.ComsDraftedTemplateSectionParameter.AddAsync(sectionParm);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Consultation Workflow

        //<History Author = 'Muhammad Zaeem' Date='2023-22-02' Version="1.0" Branch="master">Create Workflow Instance and link entity with workflow</History>
        private async Task ComsLinkEntityWithActiveWorkflow(ComsDraftedTemplate document, DatabaseContext dbContext)
        {
            try
            {
                string StoredProc = $"exec pWorkflowSelAdvanceSearch @statusId = '{(int)WorkflowStatusEnum.Active}', @moduleId = '{(int)WorkflowModuleEnum.COMSConsultationManagement}', @moduleTriggerId = '{(int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft}'";
                var activeWorkflow = await dbContext.WorkflowVM.FromSqlRaw(StoredProc).ToListAsync();
                if (activeWorkflow?.Count() > 0)
                {
                    WorkflowActivity firstActivity = await dbContext.WorkflowActivity.Where(a => a.WorkflowId == (int)activeWorkflow.FirstOrDefault().WorkflowId).OrderBy(a => a.SequenceNumber).FirstOrDefaultAsync();
                    WorkflowInstance workflowInstance = new WorkflowInstance { ReferenceId = document.Id, StatusId = (int)WorkflowInstanceStatusEnum.InProgress, WorkflowId = (int)activeWorkflow.FirstOrDefault().WorkflowId, WorkflowActivityId = firstActivity.WorkflowActivityId, IsSlaExecuted = false };
                    SLA sla = await dbContext.SLA.Where(s => s.WorkflowActivityId == workflowInstance.WorkflowActivityId).FirstOrDefaultAsync();
                    if (sla != null)
                    {
                        workflowInstance.ApplySla = true;
                        workflowInstance.SlaStartDate = DateTime.Now.Date.AddDays(1);
                        workflowInstance.SlaEndDate = workflowInstance.SlaStartDate.AddDays(sla.Duration - 1);
                    }
                    else
                    {
                        workflowInstance.ApplySla = false;
                        workflowInstance.SlaStartDate = DateTime.Now.Date;
                        workflowInstance.SlaEndDate = DateTime.Now.Date;
                    }
                    await dbContext.WorkflowInstance.AddAsync(workflowInstance);
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get Consultation draft document Detail
        public async Task<ComsDraftedDocumentDetailVM> GetConsultationDraftDocDetailWithSectionAndParameters(Guid draftId)
        {
            try
            {
                ComsDraftedDocumentDetailVM document;
                string StoredProc = $"exec pConsultationDraftDocumentDetailById @draftId='{draftId}'";
                var doc = await _dbContext.ComsDraftedDocumentDetailVM.FromSqlRaw(StoredProc).ToListAsync();
                document = doc.FirstOrDefault();
                if (document != null)
                {
                    StoredProc = $"exec pConsultationDraftDocumentSectionsList @draftedTemplateId='{document.Id}'";
                    document.TemplateSections = await _dbContext.CaseTemplateSectionsVM.FromSqlRaw(StoredProc).ToListAsync();
                    foreach (var section in document.TemplateSections)
                    {
                        StoredProc = $"exec pConsultationDraftDocumentSectionParametersList @draftedTemplateSectionId='{section.Id}'";
                        section.SectionParameters = await _dbContext.CaseTemplateParametersVM.FromSqlRaw(StoredProc).ToListAsync();
                    }
                }
                return document;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region get consultation drafted template detail by Id
        public async Task<ComsDraftedTemplate> GetConsultationDraftedTemplateDetailById(Guid draftId)
        {
            try
            {
                return await _dbContext.ComsDraftedTemplate.FindAsync(draftId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Consultation Draft Docs List

        //<History Author = 'Muhammad Zaeem' Date='2023-03-02' Version="1.0" Branch="master"> Moved method from COnsultataion RequestRepo</History>
        public async Task<List<ComsDraftedDocumentVM>> GetConsultationDraftListByReferenceId(Guid referenceId)
        {
            try
            {
                string StoredProc = $"exec pGetDraftConsultationRequestList @ReferenceId='{referenceId}'";
                return await _dbContext.ComsDraftedDocumentVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Consultation DraftedTemplateSectionParameters
        public async Task<List<CaseTemplateParametersVM>> GetCOMSDraftedTemplateSectionParameters(Guid templateSectionId)
        {
            try
            {
                string StoredProc = $"exec pComsDraftedTemplateSectionParametersList @templateSectionId = '{templateSectionId}'";
                return await _dbContext.CaseTemplateParametersVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Consultation Drafted TemplateSections
        public async Task<List<CaseTemplateSectionsVM>> GetComsDraftedTemplateSections(Guid draftId)
        {
            try
            {
                string StoredProc = $"exec pComsDraftedTemplateSectionsList @draftId = '{draftId}'";
                return await _dbContext.CaseTemplateSectionsVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update Consultation Draft Document

        //<History Author = 'Muhammad Zaeem' Date='2023-07-03' Version="1.0" Branch="master"> Update Consultation Draft Doc</History>
        public async Task UpdateConsultationDraftDocument(ComsDraftedTemplate document)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.ComsDraftedTemplate.Update(document);
                        await _dbContext.SaveChangesAsync();
                        await UpdateConsultationDraftSections(document, _dbContext);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        //<History Author = 'Muhammad Zaeem' Date='2023-07-03' Version="1.0" Branch="master"> Create Consultation Draft Sections</History>
        public async Task UpdateConsultationDraftSections(ComsDraftedTemplate document, DatabaseContext dbContext)
        {
            try
            {
                var oldSections = await dbContext.ComsDraftedTemplateSection.Where(t => t.DraftedTemplateId == document.Id).ToListAsync();
                foreach (var section in oldSections)
                {
                    var oldParams = await dbContext.ComsDraftedTemplateSectionParameter.Where(t => t.DraftedTemplateSectionId == section.Id).ToListAsync();
                    foreach (var parm in oldParams)
                    {
                        dbContext.ComsDraftedTemplateSectionParameter.Remove(parm);
                        await dbContext.SaveChangesAsync();
                    }
                    dbContext.ComsDraftedTemplateSection.Remove(section);
                    await dbContext.SaveChangesAsync();
                }

                //Save sections
                foreach (var section in document.TemplateSections)
                {
                    ComsDraftedTemplateSection templateSection = new ComsDraftedTemplateSection
                    {
                        DraftedTemplateId = document.Id,
                        SectionId = section.SectionId,
                        AdditionalName = document.TemplateId == (int)CaseTemplateEnum.BlankTemplate ? section.SectionNameEn : "",
                        SequenceNumber = section.SequenceNumber
                    };

                    await dbContext.ComsDraftedTemplateSection.AddAsync(templateSection);
                    await dbContext.SaveChangesAsync();

                    //Save Parameters in section
                    foreach (var parm in section.SectionParameters)
                    {
                        ComsDraftedTemplateSectionParameter sectionParm = new ComsDraftedTemplateSectionParameter
                        {
                            DraftedTemplateSectionId = templateSection.Id,
                            ParameterId = (int)parm.ParameterId,
                            Value = parm.Value
                        };
                        await dbContext.ComsDraftedTemplateSectionParameter.AddAsync(sectionParm);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get Contract Template
        //<History Author = 'Umer Zaman' Date='2023-01-02' Version="1.0" Branch="master">Populate contract template details</History>

        public async Task<List<ConsultationTemplateSection>> GetSelectedConsultationTemplateSectionDetailsUsingTemplateId(int templateId)
        {
            try
            {
                List<ConsultationTemplateSection>? ObjSection = new List<ConsultationTemplateSection>();
                var resultTemplateSectionDetails = await _dbContext.ConsultationTemplateTemplateSections.Where(x => x.TemplateId == templateId).ToListAsync();
                if (resultTemplateSectionDetails.Count() != 0)
                {
                    foreach (var item in resultTemplateSectionDetails)
                    {
                        var resultSectionModel = await _dbContext.ConsultationTemplateSections.Where(x => x.TemplateSectionId == item.TemplateSectionId).FirstOrDefaultAsync();
                        if (resultSectionModel != null)
                        {
                            ObjSection.Add(resultSectionModel);
                        }
                    }
                    return ObjSection;
                }
                return new List<ConsultationTemplateSection>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Save drafted consultation request

        //<History Author = 'Hassan Abbas' Date='2022-10-02' Version="1.0" Branch="master"> Create Case Draft Doc</History>
        public async Task<bool> SaveDraftFileConsultationRequest(ConsultationRequest consultationRequests)
        {

            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var consultationResult = await _dbContext.ConsultationRequests.Where(x => x.ConsultationRequestId == consultationRequests.ConsultationRequestId).FirstOrDefaultAsync();
                        if (consultationResult != null)
                        {
                            consultationResult.Introduction = consultationRequests.Introduction;
                            _dbContext.ConsultationRequests.Update(consultationResult);
                            await _dbContext.SaveChangesAsync();
                            if (consultationRequests.ConsultationArticles.Count() != 0)
                            {
                                await UpdateDraftConsultationArticle(consultationRequests.ConsultationRequestId, consultationRequests.ConsultationArticles, _dbContext);
                            }
                            transaction.Commit();
                            return true;
                        }
                        return false;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        private async Task UpdateDraftConsultationArticle(Guid consultationRequestId, IList<ConsultationArticle> consultationArticles, DatabaseContext dbContext)
        {
            try
            {
                var resultArticle = await dbContext.ConsultationArticles.Where(x => x.ConsultationRequestId == consultationRequestId).ToListAsync();
                if (resultArticle.Count() != 0)
                {
                    foreach (var item in resultArticle)
                    {
                        dbContext.ConsultationArticles.Remove(item);
                        await dbContext.SaveChangesAsync();
                    }
                }
                foreach (var item in consultationArticles)
                {
                    await dbContext.ConsultationArticles.AddAsync(item);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get Consultation Draft Docs Reasons

        //<History Author = 'Muhammad Zaeem' Date='2023-03-23' Version="1.0" Branch="master"> Reason Consultation Draft document rejection</History>
        public async Task<List<ComsDraftedDocumentReasonVM>> GetComsDraftDocumentReasonsByReferenceId(Guid referenceId)
        {
            try
            {
                string StoredProc = $"exec pComsDraftedDocumentReasonList @ReferenceId='{referenceId}'";
                return await _dbContext.ComsDraftedDocumentReasonVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get CmsDrafted Template Version Logs List
        public async Task<List<CmsDraftTemplateVersionLogVM>> GetCmsDraftTemplateVersionLogsList(Guid versionId)
        {
            try
            {
                string StoredProc = $"exec pCmsDraftedTemplateVersionLogs @versionId='{versionId}'";
                var response = await _dbContext.CmsDraftTemplateVersionLogVM.FromSqlRaw(StoredProc).ToListAsync();
                return response;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion

        #region Get Parent Entity Detail of Draft
        //<History Author = 'Hassan Abbas' Date='2024-07-28' Version="1.0" Branch="master"> Get Parent Entity Detail of Draft</History>
        public async Task<CmsDraftedDocumentParentEntityDetailVM> GetDraftParentEntityDetails(Guid referenceId, string userId)
        {
            try
            {
                CmsDraftedDocumentParentEntityDetailVM entityDetailVM = new CmsDraftedDocumentParentEntityDetailVM { Id = referenceId };
                var isMatchFound = _dbContext.CmsRegisteredCases.Any(c => c.CaseId == referenceId);
                if(isMatchFound)
                {
                    string StoredProc = $"exec pCmsRegisteredCaseDetailById @caseId ='{referenceId}', @userId ='{userId}' ";
                    var caseResult = await _dbContext.CmsRegisteredCaseDetailVM.FromSqlRaw(StoredProc).ToListAsync();
                    CmsRegisteredCaseDetailVM caseDetail = caseResult.FirstOrDefault();
                    if(caseDetail != null)
                    {
                        entityDetailVM.Payload = caseDetail;
                        entityDetailVM.SubmoduleId = (int)SubModuleEnum.RegisteredCase;
                    }
                    return entityDetailVM;
                }
                isMatchFound = _dbContext.CaseFiles.Any(c => c.FileId == referenceId);
                if(isMatchFound)
                {
                    string StoredProc = $"exec pCmsCaseFileDetailById @fileId ='{referenceId}', @userName='{userId}' ";
                    var fileResult = await _dbContext.CmsCaseFileDetailVM.FromSqlRaw(StoredProc).ToListAsync();
                    CmsCaseFileDetailVM caseFile = fileResult.FirstOrDefault();
                    if(caseFile != null)
                    {
                        StoredProc = $"exec pCaseRequestViewDetail @RequestId = N'{caseFile.RequestId}'";
                        var requestResult = await _dbContext.CaseRequestDetailVMs.FromSqlRaw(StoredProc).ToListAsync();
                        CaseRequestDetailVM caseRequest = requestResult.FirstOrDefault();
                        if(caseRequest != null)
                        {
                            caseFile.CaseRequest.Add(caseRequest);
                            entityDetailVM.Payload = caseFile;
                            entityDetailVM.SubmoduleId = (int)SubModuleEnum.CaseFile;
                        }
                    }
                    return entityDetailVM;
                }
                return entityDetailVM;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        #region Cancel Draft Document
        public async Task<bool> SoftDeleteDraftDocumentById(Guid draftId, string userName)
        {
            try
            {
                CmsDraftedTemplate draftedTemplate = await _dbContext.CmsDraftedTemplate.FindAsync(draftId);
                if (draftedTemplate != null)
                {
                    draftedTemplate.IsDeleted = true;
                    draftedTemplate.ModifiedBy = userName;
                    draftedTemplate.ModifiedDate = DateTime.Now;
                    _dbContext.CmsDraftedTemplate.Update(draftedTemplate);
                    await _dbContext.SaveChangesAsync();
                    var isntance = _dbContext.WorkflowInstance.Where(i => i.ReferenceId == draftId).FirstOrDefault();
                    if (isntance != null)
                    {
                        isntance.StatusId = (int)WorkflowInstanceStatusEnum.Success;
                        _dbContext.WorkflowInstance.Update(isntance);
                        await _dbContext.SaveChangesAsync();
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
