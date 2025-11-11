using AutoMapper;
using FATWA_DOMAIN.Enums;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ArchivedCasesModels;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.DigitalSignature;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.BugReportingVMs;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SelectPdf;
using System.Data.Entity.Core;
using System.Security.Cryptography;
using System.Text;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.DmsEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.WorkflowEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;

namespace FATWA_INFRASTRUCTURE.Repository
{
    //<History Author = 'Hassan Abbas' Date='2022-04-11' Version="1.0" Branch="master"> Reposiory for file uploads/remove</History>
    public class TempFileUploadRepository : ITempFileUpload
    {
        private readonly DatabaseContext _dbContext;
        private readonly DmsDbContext _dmsDbContext;
        private readonly ArchivedCasesDbContext _archivedCasesDbContext;
        private List<DMSDocumentListVM> _DmsDocumentListVMs1;
        private List<DMSDocumentListVM> _DmsDocumentListVMs2;
        private List<DMSTemplateListVM> _DmsTemplateListVMs;
        private List<DMSKayPublicationDocumentListVM> _dMSKayPublicationDocumentListVMs;
        private List<MojDocumentVM> _mojDocumentListVMs;
        private DMSDocumentDetailVM _ViewDocumentVM;

        public TempFileUploadRepository(DatabaseContext dbContext, DmsDbContext dmsDbContext, ArchivedCasesDbContext archivedCasesDbContext)
        {
            _dbContext = dbContext;
            _dmsDbContext = dmsDbContext;
            _archivedCasesDbContext = archivedCasesDbContext;
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-28' Version="1.0" Branch="master"> Get attachement types</History>
        //<History Author = 'Hassan Abbas' Date='2023-07-11' Version="1.0" Branch="master"> Remove certain attachment types based on user Sector</History>
        public async Task<List<AttachmentType>> GetAttachmentTypes(int? ModuleId, int? SectorTypeId, bool ShowHidden = false)
        {
            List<AttachmentType> attachmentTypes = new List<AttachmentType>();
            try
            {
                if (ModuleId != 0)
                {
                    attachmentTypes = await _dmsDbContext.AttachmentType.Where(t => (t.ModuleId == ModuleId) && (t.IsDeleted != true) && (t.IsActive == true) && (ShowHidden ? true : !new List<int> { 60, 61, 62, 63, 101, 104, 107, 108, 109, 111 }.Contains(t.AttachmentTypeId))).OrderByDescending(u => u.AttachmentTypeId).ToListAsync();
                    if (SectorTypeId != null)
                    {
                        if (SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeAppealCases || SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialAppealCases)
                        {
                            attachmentTypes.Add(_dmsDbContext.AttachmentType.Where(a => a.AttachmentTypeId == (int)AttachmentTypeEnum.CmsAppealJudgement).FirstOrDefault());
                            attachmentTypes.Add(_dmsDbContext.AttachmentType.Where(a => a.AttachmentTypeId == (int)AttachmentTypeEnum.AppealPartialJudgment).FirstOrDefault());
                            attachmentTypes.Add(_dmsDbContext.AttachmentType.Where(a => a.AttachmentTypeId == (int)AttachmentTypeEnum.AppealJudgmentCopyOriginal).FirstOrDefault());
                            attachmentTypes.Add(_dmsDbContext.AttachmentType.Where(a => a.AttachmentTypeId == (int)AttachmentTypeEnum.AppealUrgentJudgment).FirstOrDefault());
                        }
                        if (SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeSupremeCases || SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases)
                        {
                            attachmentTypes.Add(_dmsDbContext.AttachmentType.Where(a => a.AttachmentTypeId == (int)AttachmentTypeEnum.CmsSupremeJudgement).FirstOrDefault());
                            attachmentTypes.Add(_dmsDbContext.AttachmentType.Where(a => a.AttachmentTypeId == (int)AttachmentTypeEnum.SupremeInitialJudgment).FirstOrDefault());
                            attachmentTypes.Add(_dmsDbContext.AttachmentType.Where(a => a.AttachmentTypeId == (int)AttachmentTypeEnum.SupremeJudgmentCopy).FirstOrDefault());
                            attachmentTypes.Add(_dmsDbContext.AttachmentType.Where(a => a.AttachmentTypeId == (int)AttachmentTypeEnum.SupremeJudgmentCopyOriginal).FirstOrDefault());
                        }
                        if (SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeComplaints)
                        {
                            attachmentTypes.Add(_dmsDbContext.AttachmentType.Where(a => a.AttachmentTypeId == (int)AttachmentTypeEnum.ComsAdministrativeComplaints).FirstOrDefault());
                        }
                        if (SectorTypeId == (int)OperatingSectorTypeEnum.LegalAdvice)
                        {
                            attachmentTypes.Add(_dmsDbContext.AttachmentType.Where(a => a.AttachmentTypeId == (int)AttachmentTypeEnum.ComsLegalAdvice).FirstOrDefault());
                        }
                    }
                }
                else
                {
                    attachmentTypes = await _dmsDbContext.AttachmentType.Where(t => (t.IsDeleted != true) && (t.IsActive == true)).OrderByDescending(u => u.AttachmentTypeId).ToListAsync();
                }

                return attachmentTypes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-28' Version="1.0" Branch="master"> Create temp attachement</History>
        public async Task<TempAttachement> CreateTempAttachement(TempAttachement documentObj)
        {
            try
            {
                await _dmsDbContext.TempAttachements.AddAsync(documentObj);
                await _dmsDbContext.SaveChangesAsync();
                return documentObj;
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-28' Version="1.0" Branch="master"> Delete temp attachement</History>
        public async Task DeleteTempAttachement(int attachementId)
        {
            try
            {
                var attachement = await _dmsDbContext.TempAttachements.FindAsync(attachementId);
                _dmsDbContext.TempAttachements.Remove(attachement);
                await _dmsDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-28' Version="1.0" Branch="master"> Delete uploaded document</History>
        public async Task<UploadedDocument> DeleteUploadedDocument(int id)
        {
            try
            {
                var attachement = await _dmsDbContext.UploadedDocuments.FindAsync(id);
                _dmsDbContext.UploadedDocuments.Remove(attachement);
                await _dmsDbContext.SaveChangesAsync();
                return attachement;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-28' Version="1.0" Branch="master"> Get uploaded attachements for literature</History>
        public async Task<List<UploadedDocumentVM>> GetUploadedAttachementsByLiteratureId(int literatureId)
        {
            try
            {
                string StoredProc = $"exec pUploadedDocuments @literatureId = '{literatureId}'";
                return await _dmsDbContext.UploadedDocumentVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-28' Version="1.0" Branch="master"> Get uploaded attachements using reference</History>
        public async Task<List<UploadedDocumentVM>> GetUploadedAttachementsByReferenceGuid(Guid referenceGuid)
        {
            try
            {
                string StoredProc = $"exec pUploadedDocuments @referenceGuid = '{referenceGuid}'";
                return await _dmsDbContext.UploadedDocumentVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-28' Version="1.0" Branch="master"> Get uploaded attachements using reference</History>
        public async Task<List<UploadedDocumentVM>> GetOfficialDocuments(Guid referenceGuid)
        {
            try
            {
                string storedProc = $"exec pOfficialDocuments @referenceGuid = '{referenceGuid}'";
                return await _dmsDbContext.UploadedDocumentVM.FromSqlRaw(storedProc).ToListAsync();
                //return await _dmsDbContext.UploadedDocuments.Where(x => x.ReferenceGuid == referenceGuid).ToListAsync();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-28' Version="1.0" Branch="master"> Get uploaded attachements using reference</History>
        public async Task<List<TempAttachementVM>> GetTempAttachementsByReferenceGuid(Guid referenceGuid, int attachementId = 0)
        {
            try
            {
                string storedProc = $"exec pTempAttachments @referenceGuid = '{referenceGuid}', @attachementId='{attachementId}'";
                return await _dmsDbContext.TempAttachementVM.FromSqlRaw(storedProc).ToListAsync();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-28' Version="1.0" Branch="master"> Get temp attachements by file and username</History>
        public async Task<List<TempAttachement>> GetTempAttachementsByFileAndUserName(string fileName, string userName, int typeId)
        {
            try
            {
                return await _dmsDbContext.TempAttachements.Where(x => x.FileNameWithoutTimeStamp == fileName && x.UploadedBy == userName && x.AttachmentTypeId == typeId).ToListAsync();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-28' Version="1.0" Branch="master"> Get uploaded attachements using reference</History>
        public async Task<List<AttachmentType>> GetAllAttachmentTypes()
        {
            try
            {
                return await _dmsDbContext.AttachmentType.ToListAsync();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }


        //<History Author = 'Hassan Abbas' Date='2023-06-15' Version="1.0" Branch="master"> Get Document Classifications</History>
        public async Task<List<DmsDocumentClassification>> GetDocumentClassifications()
        {
            try
            {
                return await _dmsDbContext.DmsDocumentClassifications.ToListAsync();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public async Task<UploadedDocument> GetUploadedAttachementById(int Id, Guid? _referenceGuid = null, int literatureId = 0)
        {
            try
            {
                var result = await _dmsDbContext.UploadedDocuments.Where(x => x.UploadedDocumentId == Id && (!_referenceGuid.HasValue || x.ReferenceGuid == _referenceGuid) && (literatureId == 0 || x.LiteratureId == literatureId)).FirstOrDefaultAsync();
                if (result is not null)
                    return result;
                return null;

            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-11-21' Version="1.0" Branch="master"> Upload Temp Attachment to Uploaded Document</History>
        public async Task UploadTempAttachmentToUploadedDocument(Guid referenceId, string createdBy)
        {
            try
            {
                var attachements = await _dmsDbContext.TempAttachements.Where(x => x.Guid == referenceId).ToListAsync();
                foreach (var file in attachements)
                {
                    UploadedDocument documentObj = new UploadedDocument();
                    documentObj.Description = file.Description;
                    documentObj.CreatedDateTime = DateTime.Now;
                    documentObj.CreatedBy = createdBy;
                    documentObj.DocumentDate = DateTime.Now;
                    documentObj.FileName = file.FileName;
                    documentObj.StoragePath = file.StoragePath;
                    documentObj.DocType = file.DocType;
                    documentObj.ReferenceGuid = referenceId;
                    documentObj.IsActive = true;
                    documentObj.CreatedAt = file.StoragePath;
                    documentObj.AttachmentTypeId = file.AttachmentTypeId;
                    documentObj.OtherAttachmentType = file.OtherAttachmentType;
                    documentObj.IsDeleted = false;
                    documentObj.StatusId = (int)SigningTaskStatusEnum.UnSigned;
                    await _dmsDbContext.UploadedDocuments.AddAsync(documentObj);
                    await _dmsDbContext.SaveChangesAsync();
                    await Task.Delay(200);
                    _dmsDbContext.TempAttachements.Remove(file);
                    await _dmsDbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //<History Author = 'Nadia Gull' Date='2022-12-06' Version="1.0" Branch="master"> Get uploaded document with id</History>
        public async Task<string> GetUploadedDocument(int id)
        {
            try
            {
                var attachement = await _dmsDbContext.UploadedDocuments.FindAsync(id);
                return attachement.StoragePath;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        #region ZAIN CHANGES

        public async Task<bool> SaveTempAttachmentToUploadedDocument(FileUploadVM item)
        {
            using (_dmsDbContext)
            {
                using (var transaction = _dmsDbContext.Database.BeginTransaction())
                {
                    bool isSaved = true;
                    try
                    {
                        isSaved = await DeleteFileFromPath(item);
                        if (!item.IsRequestForMeetingSaveAsDraft)
                        {
                            isSaved = await SaveUploadedDocuments(item);
                        }
                        if (isSaved)
                            transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        isSaved = false;
                        throw;
                    }
                    return isSaved;
                }
            }
        }

        protected async Task<bool> DeleteFileFromPath(FileUploadVM item)
        {
            bool isSaved = true;
            try
            {
                if (item.DeletedAttachementIds is not null)
                {
                    foreach (var deletedAttachementId in item.DeletedAttachementIds)
                    {
                        var attachement = await _dmsDbContext.UploadedDocuments.FindAsync(deletedAttachementId);
                        if (attachement != null)
                        {
                            var basePath = Path.Combine(item.FilePath + attachement.StoragePath);
                            if (File.Exists(basePath))
                            {
                                File.Delete(basePath);
                            }
                            _dmsDbContext.UploadedDocuments.Remove(attachement);
                            await _dmsDbContext.SaveChangesAsync();
                        }

                    }
                }
            }
            catch (Exception)
            {
                isSaved = true;
                throw;
            }
            return isSaved;
        }

        protected async Task<bool> SaveUploadedDocuments(FileUploadVM item)
        {
            bool isSaved = true;
            try
            {
                foreach (var requestId in item.RequestIds)
                {
                    var attachements = await _dmsDbContext.TempAttachements.Where(x => item.isCommunication ? x.CommunicationGuid == requestId : x.Guid == requestId).ToListAsync();

                    foreach (TempAttachement file in attachements)
                    {
                        UploadedDocument documentObj = new UploadedDocument()
                        {
                            Description = file.Description,
                            CreatedDateTime = DateTime.Now,
                            CreatedBy = item.CreatedBy,
                            DocumentDate = file.DocumentDate != null ? (DateTime)file.DocumentDate : DateTime.Now,
                            //DocumentDate = (DateTime)file.DocumentDate, 
                            FileName = file.FileName,
                            StoragePath = file.StoragePath,
                            DocType = file.DocType,
                            ReferenceGuid = file.Guid,
                            CommunicationGuid = item.isCommunication ? requestId : Guid.Empty,
                            IsActive = true,
                            CreatedAt = file.StoragePath,
                            AttachmentTypeId = file.AttachmentTypeId,
                            IsDeleted = false,
                            OtherAttachmentType = file.OtherAttachmentType,
                            ReferenceNo = file.ReferenceNo,
                            ReferenceDate = file.ReferenceDate,
                            FileNumber = file.FileNumber,
                            FileTitle = file.FileTitle,
                            LiteratureId = item.LiteratureId,
                            StatusId = (int)SigningTaskStatusEnum.UnSigned
                        };

                        await _dmsDbContext.UploadedDocuments.AddAsync(documentObj);
                        await _dmsDbContext.SaveChangesAsync();
                        await Task.Delay(200);

                        _dmsDbContext.TempAttachements.Remove(file);
                        await _dmsDbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                isSaved = true;
                throw;
            }
            return isSaved;
        }

        public async Task<bool> CopyAttachmentsFromSourceToDestination(List<CopyAttachmentVM> attachmentDetail)
        {
            bool isSaved = true;
            try
            {
                foreach (var attachment in attachmentDetail)
                {
                    var requestDocs = await _dmsDbContext.UploadedDocuments.Where(p => p.ReferenceGuid == attachment.SourceId).ToListAsync();
                    foreach (var reqDoc in requestDocs)
                    {
                        UploadedDocument fileDoc = reqDoc;
                        fileDoc.UploadedDocumentId = 0;
                        fileDoc.ReferenceGuid = attachment.DestinationId;
                        fileDoc.CreatedBy = attachment.CreatedBy;
                        fileDoc.CreatedDateTime = DateTime.Now;
                        await _dmsDbContext.UploadedDocuments.AddAsync(fileDoc);
                        await _dmsDbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }

        public async Task<bool> UpdateExistingDocument(List<CopyAttachmentVM> attachmentDetail)
        {
            bool isSaved = true;
            try
            {
                foreach (var attachment in attachmentDetail)
                {
                    var requestDocs = await _dmsDbContext.UploadedDocuments.Where(p => p.ReferenceGuid == attachment.SourceId).ToListAsync();
                    foreach (var reqDoc in requestDocs)
                    {
                        reqDoc.ReferenceGuid = attachment.DestinationId;
                        _dmsDbContext.UploadedDocuments.Update(reqDoc);
                        await _dmsDbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }

        public async Task<bool> CopySelectedAttachmentsToDestination(CopySelectedAttachmentsVM attachmentDetail)
        {
            bool isSaved = true;
            try
            {
                foreach (var attachment in attachmentDetail.SelectedDocuments)
                {
                    var requestDocs = await _dmsDbContext.UploadedDocuments.Where(p => p.UploadedDocumentId == attachment.UploadedDocumentId).ToListAsync();
                    foreach (var reqDoc in requestDocs)
                    {
                        UploadedDocument fileDoc = reqDoc;
                        fileDoc.UploadedDocumentId = 0;
                        fileDoc.ReferenceGuid = attachmentDetail.DestinationId;
                        fileDoc.CreatedBy = attachmentDetail.CreatedBy;
                        fileDoc.CreatedDateTime = DateTime.Now;
                        await _dmsDbContext.UploadedDocuments.AddAsync(fileDoc);
                        await _dmsDbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }


        public async Task<bool> SaveDocumentPortfolioToDocument(CmsDocumentPortfolio documentPortfolio, string? fileName, string? physicalPath)
        {
            bool isSaved = true;
            try
            {
                UploadedDocument attachement = new UploadedDocument
                {
                    AttachmentTypeId = documentPortfolio.AttachmentTypeId,
                    ReferenceGuid = documentPortfolio.ReferenceId,
                    DocumentDate = DateTime.Now,
                    FileName = fileName,
                    DocType = Path.GetExtension(".pdf"),
                    IsActive = true,
                    CreatedBy = documentPortfolio.CreatedBy,
                    CreatedDateTime = DateTime.Now,
                    CreatedAt = physicalPath,
                    StoragePath = physicalPath,
                    StatusId = (int)SigningTaskStatusEnum.UnSigned
                };

                await _dmsDbContext.UploadedDocuments.AddAsync(attachement);
                await _dmsDbContext.SaveChangesAsync();

                documentPortfolio.StoragePath = physicalPath;
                documentPortfolio.CreatedDate = DateTime.Now;
                await _dbContext.CmsDocumentPortfolio.AddAsync(documentPortfolio);
                await _dbContext.SaveChangesAsync();

                return isSaved;
            }
            catch (Exception ex)
            {
                isSaved = false;
                throw ex;
            }
        }

        public async Task<int> SaveDraftTemplateToDocument(CmsDraftedTemplate draft, string? fileName, string? physicalPath)
        {
            try
            {
                Guid CommunicationId = Guid.Empty;
                if (draft.DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo
                    || draft.DraftEntityType == (int)DraftEntityTypeEnum.FileNeedMoreInfo
                    || draft.DraftEntityType == (int)DraftEntityTypeEnum.CaseNeedMoreInfo)
                {
                    List<SendCommunicationVM> sendCommunication = JsonConvert.DeserializeObject<List<SendCommunicationVM>>(draft.Payload);
                    CommunicationId = sendCommunication.FirstOrDefault().Communication.CommunicationId;
                }
                else if (draft.CommunicationId != null && draft.CommunicationId != Guid.Empty)
                {
                    CommunicationId = (Guid)draft.CommunicationId;
                }
                UploadedDocument attachement = new UploadedDocument
                {
                    AttachmentTypeId = draft.AttachmentTypeId,
                    ReferenceGuid = draft.ReferenceId,
                    CommunicationGuid = CommunicationId,
                    DocumentDate = DateTime.Now,
                    Description = draft.Description,
                    FileName = fileName,
                    FileNumber = draft.DraftNumber > 0 ? draft.DraftNumber.ToString() : null,
                    DocType = Path.GetExtension(".pdf"),
                    IsActive = true,
                    CreatedBy = draft.CreatedBy,
                    CreatedDateTime = DateTime.Now,
                    CreatedAt = physicalPath,
                    StoragePath = physicalPath,
                    VersionId = draft.DraftedTemplateVersion.VersionId,
                    StatusId = (int)SigningTaskStatusEnum.UnSigned
                };

                await _dmsDbContext.UploadedDocuments.AddAsync(attachement);
                await _dmsDbContext.SaveChangesAsync();
                return attachement.UploadedDocumentId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //<History Author = 'Ijaz Ahmad' Date='2023-03-14' Version="1.0" Branch="master">  Save Consultaiton Draft Template to Document</History> 
        public async Task<bool> SaveComsDraftTemplateToDocument(ComsDraftedTemplate draft, string? fileName, string? physicalPath)
        {
            bool isSaved = true;
            try
            {
                Guid CommunicationId = Guid.Empty;
                if (draft.DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo
                    || draft.DraftEntityType == (int)DraftEntityTypeEnum.FileNeedMoreInfo)
                {
                    SendCommunicationVM sendCommunication = JsonConvert.DeserializeObject<SendCommunicationVM>(draft.Payload);
                    CommunicationId = sendCommunication.Communication.CommunicationId;
                }
                UploadedDocument attachement = new UploadedDocument
                {
                    AttachmentTypeId = draft.AttachmentTypeId,
                    ReferenceGuid = draft.ReferenceId,
                    CommunicationGuid = CommunicationId,
                    DocumentDate = DateTime.Now,
                    Description = draft.Description,
                    FileName = fileName,
                    FileNumber = draft.DraftNumber > 0 ? draft.DraftNumber.ToString() : null,
                    DocType = Path.GetExtension(".pdf"),
                    IsActive = true,
                    CreatedBy = draft.CreatedBy,
                    CreatedDateTime = DateTime.Now,
                    CreatedAt = physicalPath,
                    StoragePath = physicalPath,
                    StatusId = (int)SigningTaskStatusEnum.UnSigned
                };

                await _dmsDbContext.UploadedDocuments.AddAsync(attachement);
                await _dmsDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }
        // <History Author = 'Ijaz Ahmad' Date='2023-03-29' Version="1.0" Branch="master"> Get uploaded document by id</History>
        public async Task<string> GetDocumentById(Guid referenceGuid)
        {
            try
            {

                var uploadedDocument = await _dmsDbContext.UploadedDocuments.Where(x => x.ReferenceGuid == referenceGuid).FirstOrDefaultAsync();
                if (uploadedDocument is not null)
                {
                    return uploadedDocument.StoragePath;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public async Task<string> GetDocumentById(string referenceGuid, string isReferenceGuid)
        {
            try
            {
                TempAttachement tempAttachement = new TempAttachement();
                if (isReferenceGuid == "True")
                    tempAttachement = await _dmsDbContext.TempAttachements.Where(x => x.Guid == Guid.Parse(referenceGuid)).FirstOrDefaultAsync();
                else
                    tempAttachement = await _dmsDbContext.TempAttachements.Where(x => x.AttachementId == Convert.ToInt32(referenceGuid)).FirstOrDefaultAsync();
                if (tempAttachement is not null)
                {
                    return tempAttachement.StoragePath;
                }

                UploadedDocument uploadedDocument = new UploadedDocument();
                if (isReferenceGuid == "True")
                    uploadedDocument = await _dmsDbContext.UploadedDocuments.Where(x => x.ReferenceGuid == Guid.Parse(referenceGuid)).FirstOrDefaultAsync();

                else
                    uploadedDocument = await _dmsDbContext.UploadedDocuments.Where(x => x.UploadedDocumentId == Convert.ToInt32(referenceGuid)).FirstOrDefaultAsync();
                if (uploadedDocument is not null)
                {
                    return uploadedDocument.StoragePath;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        //<History Author = 'ijaz Ahmad' Date='2023-03-29' Version="1.0" Branch="master"> Function for removing file of Ligislation</History>

        public async Task<bool> RemoveDocument(Guid referenceGuid)
        {
            bool isSaved = true;
            try
            {

                var uploadedDocument = await _dmsDbContext.UploadedDocuments.Where(x => x.ReferenceGuid == referenceGuid).ToListAsync();
                if (uploadedDocument is not null)
                {
                    foreach (var item in uploadedDocument)
                    {
                        _dmsDbContext.UploadedDocuments.Remove(item);
                    }
                    await _dmsDbContext.SaveChangesAsync();
                }

            }
            catch (Exception)
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }
        public async Task<bool> RemoveDocument(string referenceGuid, string isReferenceGuid)
        {
            bool isSaved = true;
            try
            {
                TempAttachement tempAttachement = new TempAttachement();
                if (isReferenceGuid == "True")
                    tempAttachement = await _dmsDbContext.TempAttachements.Where(x => x.Guid == Guid.Parse(referenceGuid)).FirstOrDefaultAsync();
                else
                    tempAttachement = await _dmsDbContext.TempAttachements.Where(x => x.AttachementId == Convert.ToInt32(referenceGuid)).FirstOrDefaultAsync();
                if (tempAttachement is not null)
                {
                    _dmsDbContext.TempAttachements.Remove(tempAttachement);
                    await _dmsDbContext.SaveChangesAsync();
                }
                UploadedDocument uploadedDocument = new UploadedDocument();
                if (isReferenceGuid == "True")
                    uploadedDocument = await _dmsDbContext.UploadedDocuments.Where(x => x.ReferenceGuid == Guid.Parse(referenceGuid)).FirstOrDefaultAsync();

                else
                    uploadedDocument = await _dmsDbContext.UploadedDocuments.Where(x => x.UploadedDocumentId == Convert.ToInt32(referenceGuid)).FirstOrDefaultAsync();
                if (uploadedDocument is not null)
                {
                    _dmsDbContext.UploadedDocuments.Remove(uploadedDocument);
                    await _dmsDbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }

        #endregion


        public async Task<bool> DeleteSelectedSourceDocument(List<TempAttachementVM> selectedSourceDocumentForDelete)
        {
            bool isSaved = true;
            try
            {

                if (selectedSourceDocumentForDelete.Count() != 0)
                {
                    foreach (var item in selectedSourceDocumentForDelete)
                    {
                        if (item.AttachementId != null)
                        {
                            TempAttachement tempAttachement = new TempAttachement();

                            tempAttachement = await _dmsDbContext.TempAttachements.Where(x => x.AttachementId == item.AttachementId).FirstOrDefaultAsync();
                            if (tempAttachement != null)
                            {
                                _dmsDbContext.TempAttachements.Remove(tempAttachement);
                            }

                        }
                        else
                        {
                            UploadedDocument uploadedDocument = new UploadedDocument();
                            if (item.UploadedDocumentId != null)
                            {
                                uploadedDocument = await _dmsDbContext.UploadedDocuments.Where(x => x.UploadedDocumentId == item.UploadedDocumentId).FirstOrDefaultAsync();
                                if (uploadedDocument != null)
                                {
                                    _dmsDbContext.UploadedDocuments.Remove(uploadedDocument);
                                }
                            }
                            else
                            {
                                uploadedDocument = await _dmsDbContext.UploadedDocuments.Where(x => x.ReferenceGuid == Guid.Parse(item.ReferenceGuid)).FirstOrDefaultAsync();
                                if (uploadedDocument != null)
                                {
                                    _dmsDbContext.UploadedDocuments.Remove(uploadedDocument);
                                }

                            }
                        }
                        await _dmsDbContext.SaveChangesAsync();
                    }

                }
            }
            catch (Exception ex)
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }

        public async Task<bool> UpdateUploadedAttachementMojFlagById(int Id)
        {
            try
            {
                var result = await _dmsDbContext.UploadedDocuments.Where(x => x.UploadedDocumentId == Id).FirstOrDefaultAsync();
                if (result is not null)
                {
                    result.IsMOJRegistered = true;
                    _dmsDbContext.UploadedDocuments.Update(result);
                    await _dmsDbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        #region Get Document Detail by Version Id

        //<History Author = 'Hassan Abbas' Date='2023-06-20' Version="1.0" Branch="master"> Get Document Details by Version Id</History>
        public async Task<DmsAddedDocument> GetDocumentDetailByVersionId(Guid versionId, Guid DocumentId)
        {
            try
            {
                DmsAddedDocument addedDocument = new DmsAddedDocument();
                if (DocumentId != null && DocumentId != Guid.Empty)
                {
                    var draftVersions = await _dmsDbContext.DmsAddedDocumentVersions.Where(x => x.AddedDocumentId == DocumentId).OrderByDescending(x => x.CreatedDate).ToListAsync();
                    versionId = draftVersions.FirstOrDefault().Id;
                }
                DmsAddedDocumentVersion DocumentVersion = await _dmsDbContext.DmsAddedDocumentVersions.FindAsync(versionId);
                addedDocument = await _dmsDbContext.DmsAddedDocuments.FindAsync(DocumentVersion.AddedDocumentId);
                addedDocument.DocumentVersion = DocumentVersion;
                return addedDocument;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        #endregion

        #region Get Document Number

        //<History Author = 'Hassan Abbas' Date='2023-06-20' Version="1.0" Branch="master"> Get Version and Document number for new doc</History>
        public async Task<DmsAddedDocument> GetDocumentNumberAndVersion(Guid documentId)
        {
            try
            {
                DmsAddedDocument addedDocument = new DmsAddedDocument();
                if (documentId == Guid.Empty)
                {
                    addedDocument.DocumentNumber = _dmsDbContext.DmsAddedDocuments.Any() ? await _dmsDbContext.DmsAddedDocuments.Select(x => x.DocumentNumber).MaxAsync() + 1 : 1;
                    addedDocument.CreatedDate = DateTime.Now;
                    addedDocument.ModifiedDate = DateTime.Now;

                    addedDocument.DocumentVersion = new DmsAddedDocumentVersion();
                    addedDocument.DocumentVersion.VersionNo = 0.01M;
                }
                else
                {
                    addedDocument = _dmsDbContext.DmsAddedDocuments.Where(d => d.Id == documentId).FirstOrDefault();
                    if (addedDocument != null)
                    {
                        addedDocument.DocumentVersion = await _dmsDbContext.DmsAddedDocumentVersions
                            .Where(c => c.AddedDocumentId == documentId)
                            .GroupBy(c => c.AddedDocumentId)
                            .Select(g => g.OrderByDescending(c => c.VersionNo).FirstOrDefault()).FirstOrDefaultAsync();
                    }
                }
                return addedDocument;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        #endregion

        #region Add Document 

        //<History Author = 'Hassan Abbas' Date='2023-06-21' Version="1.0" Branch="master"> Add Document</History>
        public async Task<DmsAddedDocument> SaveAddedDocument(DmsAddedDocument document)
        {
            try
            {
                #region new implementation
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            if (document.DocumentVersion.StatusId == (int)DocumentStatusEnum.InReview)
                            {
                                await _dmsDbContext.DmsAddedDocuments.AddAsync(document);
                                await _dmsDbContext.SaveChangesAsync();
                                document.DocumentVersion.AddedDocumentId = document.Id;
                                document.DocumentVersion.ModifiedDate = DateTime.Now;
                                document.DocumentVersion.ModifiedBy = document.DocumentVersion.CreatedBy;
                                await _dmsDbContext.DmsAddedDocumentVersions.AddAsync(document.DocumentVersion);
                                await _dmsDbContext.SaveChangesAsync();
                                await LinkEntityWithActiveWorkflow(document, _dbContext);
                                transaction.Commit();
                            }
                            else
                            {
                                await _dmsDbContext.DmsAddedDocuments.AddAsync(document);
                                await _dmsDbContext.SaveChangesAsync();
                                document.DocumentVersion.AddedDocumentId = document.Id;
                                document.DocumentVersion.ModifiedDate = DateTime.Now;
                                document.DocumentVersion.ModifiedBy = document.DocumentVersion.CreatedBy;
                                await _dmsDbContext.DmsAddedDocumentVersions.AddAsync(document.DocumentVersion);
                                await _dmsDbContext.SaveChangesAsync();
                                transaction.Commit();
                            }
                            return document;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateDMSDocument(DmsAddedDocument document)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dmsDbContext.DmsAddedDocuments.Update(document);
                        if (document.DocumentVersion.StatusId == (int)DocumentStatusEnum.Draft)
                        {
                            _dmsDbContext.DmsAddedDocumentVersions.Update(document.DocumentVersion);
                            await _dmsDbContext.SaveChangesAsync();
                        }
                        else if (document.DocumentVersion.StatusId == (int)DocumentStatusEnum.Rejected)
                        {
                            document.DocumentVersion.Id = Guid.NewGuid();
                            document.DocumentVersion.CreatedBy = document.CreatedBy;
                            document.DocumentVersion.CreatedDate = document.CreatedDate;
                            if (document.IsSubmit)
                            {
                                document.DocumentVersion.StatusId = document.DocumentVersion.StatusId == (int)DocumentStatusEnum.Rejected ? (int)DocumentStatusEnum.Approved : (int)DocumentStatusEnum.InReview;
                                document.DocumentVersion.VersionNo = Decimal.Add(document.DocumentVersion.VersionNo, 0.1M);
                                _dmsDbContext.DmsAddedDocumentVersions.Add(document.DocumentVersion);
                                await _dmsDbContext.SaveChangesAsync();
                            }
                            else
                            {
                                document.DocumentVersion.StatusId = (int)DocumentStatusEnum.Draft;
                                document.DocumentVersion.VersionNo = Decimal.Add(document.DocumentVersion.VersionNo, 0.1M);
                                _dmsDbContext.DmsAddedDocumentVersions.Add(document.DocumentVersion);
                                await _dmsDbContext.SaveChangesAsync();
                            }

                        }
                        else if (document.DocumentVersion.StatusId == (int)DocumentStatusEnum.InReview)
                        {
                            _dmsDbContext.DmsAddedDocumentVersions.Update(document.DocumentVersion);
                            await _dmsDbContext.SaveChangesAsync();
                            await LinkEntityWithActiveWorkflow(document, _dbContext);
                        }
                        else if (document.DocumentVersion.StatusId == (int)DocumentStatusEnum.Approved)
                        {
                            _dmsDbContext.DmsAddedDocumentVersions.Update(document.DocumentVersion);
                            await _dmsDbContext.SaveChangesAsync();
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
        public async Task CreateDMSDocumentVersion(DmsAddedDocument DmsTemplate)
        {
            try
            {
                _dmsDbContext.DmsAddedDocuments.Update(DmsTemplate);
                var value = await _dmsDbContext.DmsAddedDocumentVersions.Where(x => x.AddedDocumentId == DmsTemplate.Id && x.StatusId == (int)DocumentStatusEnum.Draft).AnyAsync();
                if (value == true)
                {
                    _dmsDbContext.DmsAddedDocumentVersions.Update(DmsTemplate.DocumentVersion);
                }
                else
                {
                    #region For Previouse Version Hide from Reviewer Document List
                    var previousVersion = await _dmsDbContext.DmsAddedDocumentVersions.Where(x => x.AddedDocumentId == DmsTemplate.Id && x.Id == DmsTemplate.DocumentVersion.PreviousVersionId).FirstOrDefaultAsync();
                    if (previousVersion != null)
                    {
                        previousVersion.ReviewerUserId = "";
                        previousVersion.ReviewerRoleId = "";
                        _dmsDbContext.Entry(previousVersion).State = EntityState.Modified;
                        await _dmsDbContext.SaveChangesAsync();
                    }
                    #endregion
                    await _dmsDbContext.DmsAddedDocumentVersions.AddAsync(DmsTemplate.DocumentVersion);
                }
                await _dmsDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private async Task LinkEntityWithActiveWorkflow(DmsAddedDocument document, DatabaseContext dbContext)
        {
            try
            {
                var moduleTriggerId = (int)WorkflowModuleTriggerEnum.UserSubmitsDMSDocument;
                string StoredProc = $"exec pWorkflowSelAdvanceSearch @statusId = '{(int)WorkflowStatusEnum.Active}', @moduleTriggerId = '{moduleTriggerId}',@attachmenttypeId='{document.AttachmentTypeId}',@submoduleId='{document.SubModuleId}'";
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

        #region Move Attachment Details to Added Document Version

        public async Task MoveAttachmentToAddedDocumentVersion(MoveAttachmentAddedDocumentVM attachmentDetail)
        {
            try
            {
                var requestDoc = await _dmsDbContext.TempAttachements.Where(p => p.Guid == attachmentDetail.ReferenceId).OrderByDescending(f => f.UploadedDate).FirstOrDefaultAsync();
                if (requestDoc != null)
                {
                    DmsAddedDocumentVersion addedDocumentVersion = await _dmsDbContext.DmsAddedDocumentVersions.FindAsync(attachmentDetail.AddedDocumentVersionId);
                    if (addedDocumentVersion != null)
                    {
                        addedDocumentVersion.FileName = requestDoc.FileName;
                        addedDocumentVersion.DocType = requestDoc.DocType;
                        addedDocumentVersion.StoragePath = requestDoc.StoragePath;
                        _dmsDbContext.Entry(addedDocumentVersion).State = EntityState.Modified;
                        await _dmsDbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Documents List
        public async Task<List<DMSDocumentListVM>> GetDocumentsList(DocumentListAdvanceSearchVM advanceSearchVM)
        {
            try
            {
                string StoredProc;
                if (_DmsDocumentListVMs1 == null)
                {
                    string requestFrom = advanceSearchVM.CreatedFrom != null ? Convert.ToDateTime(advanceSearchVM.CreatedFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string requestTo = advanceSearchVM.CreatedTo != null ? Convert.ToDateTime(advanceSearchVM.CreatedTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    StoredProc = $"exec pDmsGetDocumentList @Filename =N'{advanceSearchVM.Filename}'  , @CreatedFrom='{requestFrom}' , @CreatedTo='{requestTo}'," +
                        $" @attachmentTypeId='{advanceSearchVM.AttachmentTypeId}', @isFavourite='{Convert.ToBoolean(advanceSearchVM.isFavourite)}'," +
                        $" @UserId='{advanceSearchVM.UserId}', @RoleId='{advanceSearchVM.RoleId}', @SectorTypeId='{advanceSearchVM.SectorTypeId}', " +
                        $"@CreatedBy='{advanceSearchVM.CreatedBy}' , @PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}' ";
                    string StoredProc2 = $"exec pGetAddedDocumentList @Filename =N'{advanceSearchVM.Filename}'  , @CreatedFrom='{requestFrom}' , " +
                        $"@CreatedTo='{requestTo}', @attachmentTypeId='{advanceSearchVM.AttachmentTypeId}', @UserId='{advanceSearchVM.UserId}'," +
                        $" @CreatedBy='{advanceSearchVM.CreatedBy}', @isFavourite='{Convert.ToBoolean(advanceSearchVM.isFavourite)}'" +
                        $", @PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}'";
                    _DmsDocumentListVMs1 = await _dmsDbContext.DMSDocumentListVMs.FromSqlRaw(StoredProc).ToListAsync();
                    _DmsDocumentListVMs2 = await _dmsDbContext.DMSDocumentListVMs.FromSqlRaw(StoredProc2).ToListAsync();
                    _DmsDocumentListVMs1 = new List<DMSDocumentListVM>(_DmsDocumentListVMs1?.Concat(_DmsDocumentListVMs2).ToList());
                }
                return _DmsDocumentListVMs1.OrderByDescending(t => t.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Document Detail By Id
        public async Task<DMSDocumentDetailVM> GetDocumentDetailById(int UploadedDocumentId)
        {
            try
            {
                string StoredProc = $"exec pDocumentDetailByDocumentId @documentId ='{UploadedDocumentId}'";

                var res = await _dmsDbContext.DMSDocumentDetailVMs.FromSqlRaw(StoredProc).ToListAsync();
                if (res != null)
                {
                    _ViewDocumentVM = res.FirstOrDefault();

                }
                return _ViewDocumentVM;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Add Document To favourite
        public async Task<bool> AddDocumentToFavourite(DMSDocumentListVM doc)
        {
            bool isSaved = false;

            using (_dmsDbContext)
            {
                using (var transaction = _dmsDbContext.Database.BeginTransaction())
                {
                    try
                    {
                        isSaved = await AddUserFavouriteDocument(doc);
                        if (isSaved)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        isSaved = false;
                        transaction.Rollback();

                    }
                    return isSaved;
                }
            }

        }
        public async Task<bool> AddUserFavouriteDocument(DMSDocumentListVM doc)
        {
            bool isSaved = true;
            try
            {
                DmsUserFavouriteDocument favDoc = new DmsUserFavouriteDocument();
                {
                    if (doc.IsDocumentAddedStatus == false)
                    {

                        favDoc.DocumentId = (int)doc.UploadedDocumentId;
                        favDoc.UserId = doc.UserId;

                    }
                    else
                    {
                        favDoc.AddedDocumentVersionId = doc.VersionId;
                        favDoc.UserId = doc.UserId;
                        favDoc.DocumentId = null;

                    }
                };
                _dmsDbContext.DmsUserFavouriteDocuments.Add(favDoc);
                await _dmsDbContext.SaveChangesAsync();


            }
            catch
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }
        #endregion

        #region Remove Favourite Document

        public async Task RemoveFavouriteDocument(DMSDocumentListVM item)
        {
            try
            {
                DmsUserFavouriteDocument? favDoc = new DmsUserFavouriteDocument();
                if (item.IsDocumentAddedStatus == false)
                {
                    favDoc = await _dmsDbContext.DmsUserFavouriteDocuments.FirstOrDefaultAsync(doc => doc.DocumentId == item.UploadedDocumentId);

                }
                else
                {
                    favDoc = await _dmsDbContext.DmsUserFavouriteDocuments.FirstOrDefaultAsync(doc => doc.AddedDocumentVersionId == item.VersionId);

                }
                if (favDoc != null)
                {


                    _dmsDbContext.Entry(favDoc).State = EntityState.Deleted;
                    await _dmsDbContext.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                _dmsDbContext.Entry(_dmsDbContext).State = EntityState.Unchanged;
                throw;
            }
        }



        #endregion

        #region Share Document
        public async Task ShareDocument(DmsSharedDocument doc)
        {
            using (_dmsDbContext)
            {
                using (var transaction = _dmsDbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dmsDbContext.DmsSharedDocuments.Add(doc);
                        await _dmsDbContext.SaveChangesAsync();
                        // For Notification
                        doc.NotificationParameter.Entity = "Document";
                        doc.NotificationParameter.DocumentName = await _dmsDbContext.UploadedDocuments.Where(x => x.UploadedDocumentId == doc.DocumentId).Select(x => x.FileName).FirstOrDefaultAsync();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

        }

        #endregion

        #region Add copy attachment
        public async Task<int> AddCopyAttachments(int DocumentId, string createdBy)
        {
            try
            {
                UploadedDocument documentObj = new UploadedDocument();
                var attachements = await _dmsDbContext.UploadedDocuments.Where(x => x.UploadedDocumentId == DocumentId).FirstOrDefaultAsync();
                string fileStoragePath = attachements.StoragePath;
                string baseFilePath = Path.Combine(Directory.GetCurrentDirectory() + fileStoragePath);
                baseFilePath = baseFilePath.Replace("DMS_API", "DMS_WEB");
                bool baseFilePathExists = Directory.Exists(baseFilePath);
                if (!baseFilePathExists)
                {
                    var baseDirectoryPath = baseFilePath.Split('\\');
                    var folderStoragePath = string.Join("\\", baseDirectoryPath.Take(baseDirectoryPath.Length - 1));
                    //replacing the timestamp with new timestamp
                    string underscore = "_";
                    var lastUnderscoreIndex = attachements.FileName.LastIndexOf(underscore);
                    string newTimestamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    string result = string.Empty;
                    if (lastUnderscoreIndex >= 0 && lastUnderscoreIndex < attachements.FileName.Length - 1)
                    {
                        result = attachements.FileName.Substring(0, lastUnderscoreIndex + 1) + newTimestamp;

                    }
                    string newFileName = $"{result}{Path.GetExtension(attachements.FileName)}";
                    string newFileStoragePath = Path.Combine(folderStoragePath + "\\" + newFileName);
                    // copy of Existing File in Folder with new Name
                    File.Copy(baseFilePath, newFileStoragePath, true);
                    // Save New File Name in Db
                    var dbStoragePath = fileStoragePath.Split('\\');
                    var documentStoragePath = string.Join("\\", dbStoragePath.Take(dbStoragePath.Length - 1));
                    var storagePath = Path.Combine(documentStoragePath + "\\" + newFileName);
                    documentObj.UploadedDocumentId = 0;
                    documentObj.Description = attachements.Description;
                    documentObj.CreatedDateTime = DateTime.Now;
                    documentObj.CreatedBy = createdBy;
                    documentObj.DocumentDate = DateTime.Now;
                    documentObj.FileName = newFileName;
                    documentObj.StoragePath = storagePath;
                    documentObj.DocType = attachements.DocType;
                    documentObj.ReferenceGuid = Guid.Empty;
                    documentObj.IsActive = attachements.IsActive;
                    documentObj.CreatedAt = attachements.CreatedAt;
                    documentObj.AttachmentTypeId = attachements.AttachmentTypeId;
                    documentObj.IsDeleted = attachements.IsDeleted;
                    documentObj.StatusId = (int)SigningTaskStatusEnum.UnSigned;
                    await _dmsDbContext.UploadedDocuments.AddAsync(documentObj);
                    await _dmsDbContext.SaveChangesAsync();
                }
                return (documentObj.UploadedDocumentId);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region get file types
        public async Task<List<DmsFileTypes>> GetFileTypes()
        {
            try
            {
                return await _dmsDbContext.FileTypes.OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Document Reasons

        //<History Author = 'Hassan Abbas' Date='2023-06-24' Version="1.0" Branch="master"> Get Document Reasons</History>
        public async Task<List<DmsAddedDocumentReasonVM>> GetAddedDocumentReasonsByReferenceId(Guid referenceId)
        {
            try
            {
                string StoredProc = $"exec pDmsAddedDocumentVersionReasonList @ReferenceId='{referenceId}'";
                return await _dmsDbContext.DmsAddedDocumentReasonVMs.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get legal principle source Documents List        
        public async Task<List<LLSLegalPrincipleDocumentVM>> GetLLSLegalPrincipleSourceDocuments(LLSLegalPrincipalDocumentSearchVM fileSearch)
        {
            try
            {
                string FromDate = fileSearch.FromDate != null ? Convert.ToDateTime(fileSearch.FromDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string ToDate = fileSearch.ToDate != null ? Convert.ToDateTime(fileSearch.ToDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string StoredProc = $"exec pLLSLegalPrincipleSourceDocList @FileType = '{fileSearch.FileType}', @FromDate = '{FromDate}', @ToDate = '{ToDate}', @CourtId = '{fileSearch.CourtId}', @JudgementTypeId = '{fileSearch.JudgementTypeId}', @ChamberId = '{fileSearch.ChamberId}', @ChamberNumberId = '{fileSearch.ChamberNumberId}', @CaseNumber = N'{fileSearch.CaseNumber}', @CANNumber = N'{fileSearch.CANNumber}'" +
                                    $",@PageNumber ='{fileSearch.PageNumber}',@PageSize ='{fileSearch.PageSize}',@CourtTypeId = '{fileSearch.CourtTypeId}'";
                var sourceDocuments = await _dmsDbContext.LLSLegalPrincipleDocumentVM.FromSqlRaw(StoredProc).ToListAsync();

                return sourceDocuments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<LLSLegalPrinciplLegalAdviceDocumentVM>> GetLLSLegalPrincipleLegalAdviceSourceDocuments(LLSLegalPrincipalDocumentSearchVM fileSearch)
        {
            try
            {
                string FromDate = fileSearch.FromDate != null ? Convert.ToDateTime(fileSearch.FromDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string ToDate = fileSearch.ToDate != null ? Convert.ToDateTime(fileSearch.ToDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string StoredProc = $"exec pLLSLegalPrincipleLegalAdviceDocuments @FromDate = '{FromDate}', @ToDate = '{ToDate}' ,@PageNumber ='{fileSearch.PageNumber}',@PageSize ='{fileSearch.PageSize}'";
                //string StoredProc = $"exec pLLSLegalPrincipleSourceDocList @FileType = '{fileSearch.FileType}', @FromDate = '{FromDate}', @ToDate = '{ToDate}', @CourtId = '{fileSearch.CourtId}', @JudgementTypeId = '{fileSearch.JudgementTypeId}', @ChamberId = '{fileSearch.ChamberId}', @ChamberNumberId = '{fileSearch.ChamberNumberId}', @CaseNumber = N'{fileSearch.CaseNumber}', @CANNumber = N'{fileSearch.CANNumber}'";
                var sourceDocuments = await _dmsDbContext.LLSLegalPrinciplLegalAdviceDocumentVM.FromSqlRaw(StoredProc).ToListAsync();

                return sourceDocuments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<LLSLegalPrinciplOtherDocumentVM>> GetLLSLegalPrincipleOtherSourceDocuments(LLSLegalPrincipalDocumentSearchVM fileSearch)
        {
            try
            {
                string FromDate = fileSearch.FromDate != null ? Convert.ToDateTime(fileSearch.FromDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string ToDate = fileSearch.ToDate != null ? Convert.ToDateTime(fileSearch.ToDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string StoredProc = $"exec pLLSLegalPrincipleOtherDocuments @FromDate = '{FromDate}', @ToDate = '{ToDate}' ,@PageNumber ='{fileSearch.PageNumber}',@PageSize ='{fileSearch.PageSize}'";
                var sourceDocuments = await _dmsDbContext.LLSLegalPrinciplOtherDocumentVM.FromSqlRaw(StoredProc).ToListAsync();

                return sourceDocuments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<LLSLegalPrincipleLinkedDocVM> GetLLSLegalPrincipleContentLinkedDocuments(Guid principleContentId)
        {
            try
            {
                var linkedDocuments = new LLSLegalPrincipleLinkedDocVM();
                linkedDocuments.AppealSupremenLinkedDocuments = await GetLLSLegalPrincipleAppealSupremenContentLinkedDocuments(principleContentId);
                linkedDocuments.LegalAdviceLinkedDocuments = await GetLLSLegalPrincipleLegalAdviceContentLinkedDocuments(principleContentId);
                linkedDocuments.KuwaitAlYoumLinkedDocuments = await GetLLSLegalPrincipleKuwaitAlYoumContentLinkedDocuments(principleContentId);
                linkedDocuments.OthersLinkedDocuments = await GetLLSLegalPrincipleOtherContentLinkedDocuments(principleContentId);
                return linkedDocuments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<List<LLSLegalPrincipleAppealSupremenContentLinkedDocVM>> GetLLSLegalPrincipleAppealSupremenContentLinkedDocuments(Guid principleContentId)
        {
            try
            {
                string StoredProc = $"exec pLLSLegalPrincipleContentLinkedDocList @PrincipleContentId = '{principleContentId}'";
                var linkedDocuments = await _dmsDbContext.LLSLegalPrincipleAppealSupremenContentLinkedDocVM.FromSqlRaw(StoredProc).ToListAsync();
                return linkedDocuments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<List<LLSLegalPrincipleLegalAdviceContentLinkedDocVM>> GetLLSLegalPrincipleLegalAdviceContentLinkedDocuments(Guid principleContentId)
        {
            try
            {
                string StoredProc = $"exec pLLSLegalAdviceContentLinkedDocuments @PrincipleContentId = '{principleContentId}'";
                var linkedDocuments = await _dmsDbContext.LLSLegalPrincipleLegalAdviceContentLinkedDocVM.FromSqlRaw(StoredProc).ToListAsync();
                return linkedDocuments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<LLSLegalPrincipleKuwaitAlYoumContentLinkedDocVM>> GetLLSLegalPrincipleKuwaitAlYoumContentLinkedDocuments(Guid principleContentId)
        {
            try
            {
                string StoredProc = $"exec pLLSKuwaitAlYoumContentLinkedDocuments @PrincipleContentId = '{principleContentId}'";
                var linkedDocuments = await _dmsDbContext.LLSLegalPrincipleKuwaitAlYoumContentLinkedDocVM.FromSqlRaw(StoredProc).ToListAsync();
                return linkedDocuments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<LLSLegalPrincipleOthersContentLinkedDocVM>> GetLLSLegalPrincipleOtherContentLinkedDocuments(Guid principleContentId)
        {
            try
            {
                string StoredProc = $"exec pLLSOtherContentLinkedDocuments @PrincipleContentId = '{principleContentId}'";
                var linkedDocuments = await _dmsDbContext.LLSLegalPrincipleOthersContentLinkedDocVM.FromSqlRaw(StoredProc).ToListAsync();
                return linkedDocuments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Link Document to Destination Entities


        //<History Author = 'Hassan Abbas' Date='2023-08-07' Version="1.0" Branch="master"> Link Document to Destination Entities</History>
        public async Task LinkDocumentToDestinationEntities(LinkDocumentsVM linkDocumentDetails, string encKey)
        {
            try
            {
                using (_dmsDbContext)
                {
                    using (var transaction = _dmsDbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            int OriginalDocumentId = 0;
                            int UploadedDocumentId = 0;
                            DmsAddedDocument documentDetail = await GetDocumentDetailByVersionId(linkDocumentDetails.SourceDocumentVersionId, Guid.Empty);
                            if (documentDetail == null)
                            {
                                throw new ObjectNotFoundException();
                            }
                            if (linkDocumentDetails.ModuleId == (int)DocumentLinkModuleEnum.LegalPrinciple)
                            {
                                OriginalDocumentId = await CheckDmsDocumentAddedInUploadedTable(_dmsDbContext, documentDetail.Id);
                                if (OriginalDocumentId == 0)
                                {
                                    OriginalDocumentId = await NewCopyLinkDocumentToDestinationEntities(_dmsDbContext, documentDetail, linkDocumentDetails, documentDetail.Id, encKey);
                                }
                                UploadedDocumentId = await NewCopyLinkDocumentToDestinationEntities(_dmsDbContext, documentDetail, linkDocumentDetails, linkDocumentDetails.DestinationIds.FirstOrDefault(), encKey);

                                List<LLSLegalPrincipleContentSourceDocumentReference> linkContents = new List<LLSLegalPrincipleContentSourceDocumentReference>();
                                foreach (var item in linkDocumentDetails.PrincipleContentsDetails)
                                {
                                    linkContents.Add(new LLSLegalPrincipleContentSourceDocumentReference
                                    {
                                        PrincipleContentId = item.PrincipleContentId,
                                        PageNumber = (int)item.PageNumber,
                                        OriginalSourceDocId = OriginalDocumentId,
                                        CopySourceDocId = UploadedDocumentId,
                                        IsMaskedJudgment = false
                                    });
                                }
                                await _dbContext.LLSLegalPrincipleContentSourceDocumentReferences.AddRangeAsync(linkContents);
                                await _dbContext.SaveChangesAsync();
                            }
                            else
                            {
                                foreach (var destinationId in linkDocumentDetails.DestinationIds)
                                {
                                    UploadedDocumentId = await NewCopyLinkDocumentToDestinationEntities(_dmsDbContext, documentDetail, linkDocumentDetails, destinationId, encKey);
                                }
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<int> CheckDmsDocumentAddedInUploadedTable(DmsDbContext dmsDbContext, Guid id)
        {
            try
            {
                var result = await dmsDbContext.UploadedDocuments.Where(y => y.ReferenceGuid == id).Select(x => x.UploadedDocumentId).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        private async Task<int> NewCopyLinkDocumentToDestinationEntities(DmsDbContext dmsDbContext, DmsAddedDocument documentDetail, LinkDocumentsVM linkDocumentDetails, Guid guid, string encKey)
        {
            try
            {
                UploadedDocument uploadedDocument = new UploadedDocument
                {
                    AttachmentTypeId = linkDocumentDetails.ModuleId == (int)DocumentLinkModuleEnum.LegalPrinciple ? (int)AttachmentTypeEnum.OthersPrinciple : documentDetail.AttachmentTypeId,
                    ReferenceGuid = linkDocumentDetails.ModuleId == (int)DocumentLinkModuleEnum.LegalPrinciple ? guid : linkDocumentDetails.IsLiterature ? Guid.Empty : guid,
                    DocumentDate = DateTime.Now,
                    Description = documentDetail.Description,
                    DocType = documentDetail.DocumentVersion.DocType,
                    CreatedBy = linkDocumentDetails.CreatedBy,
                    FileTitle = documentDetail.DocumentName,
                    FileNumber = documentDetail.DocumentNumber.ToString(),
                    CreatedDateTime = DateTime.Now,
                    StatusId = (int)SigningTaskStatusEnum.UnSigned
                };

                if (documentDetail.ClassificationId == (int)DocumentClassificationEnum.External)
                {
                    string baseFilePath = Path.Combine(Directory.GetCurrentDirectory() + documentDetail.DocumentVersion.StoragePath).Replace("DMS_API", "DMS_WEB");
                    if (File.Exists(baseFilePath))
                    {
                        uploadedDocument.OtherAttachmentType = linkDocumentDetails.ModuleId == (int)DocumentLinkModuleEnum.LegalPrinciple ? documentDetail.DocumentName : string.Empty;
                        uploadedDocument.FileName = $"{Path.GetFileNameWithoutExtension(documentDetail.DocumentName)}{"_"}{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{documentDetail.DocumentVersion.DocType}";
                        string newFileStoragePath = Path.Combine(string.Join("\\", baseFilePath.Split('\\').Take(baseFilePath.Split('\\').Length - 2)) + "\\" + linkDocumentDetails.UploadFrom + "\\" + uploadedDocument.FileName);
                        // copy of Existing File in Folder with new Name
                        File.Copy(baseFilePath, newFileStoragePath, false);
                        // Save New File Name in Db
                        uploadedDocument.StoragePath = Path.Combine(string.Join("\\", documentDetail.DocumentVersion.StoragePath.Split('\\').Take(documentDetail.DocumentVersion.StoragePath.Split('\\').Length - 2)) + "\\" + linkDocumentDetails.UploadFrom + "\\" + uploadedDocument.FileName);
                    }
                }
                else
                {
                    HtmlToPdf converter = new HtmlToPdf();
                    MemoryStream stream = new MemoryStream();
                    // set converter optionsr
                    converter.Options.PdfPageSize = PdfPageSize.A4;
                    converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                    converter.Options.WebPageWidth = 1024;
                    converter.Options.WebPageHeight = 1024;
                    // create a new pdf document converting an url
                    SelectPdf.PdfDocument pdfDocument = converter.ConvertHtmlString(documentDetail.DocumentVersion.Content);
                    pdfDocument.Save(stream);
                    pdfDocument.Close();
                    stream.Close();
                    var FileData = stream.ToArray();

                    var physicalPath = string.Empty;
                    string filePath = "\\wwwroot\\Attachments\\" + linkDocumentDetails.UploadFrom + "\\";
                    string basePath = Path.Combine(Directory.GetCurrentDirectory() + filePath);
                    basePath = basePath.Replace("DMS_API", "DMS_WEB");
                    bool basePathExists = Directory.Exists(basePath);
                    if (!basePathExists)
                        Directory.CreateDirectory(basePath);

                    uploadedDocument.DocType = ".pdf";
                    uploadedDocument.FileName = $"{Path.GetFileNameWithoutExtension(documentDetail.DocumentName)}{"_"}{DateTime.Now.ToString("yyyyMMddHHmmssffff")}{".pdf"}";
                    physicalPath = Path.Combine(basePath, uploadedDocument.FileName);
                    uploadedDocument.StoragePath = Path.Combine(filePath, uploadedDocument.FileName);

                    string password = encKey;
                    UnicodeEncoding UE = new UnicodeEncoding();
                    byte[] key = UE.GetBytes(password);

                    FileStream fsCrypt = new FileStream(physicalPath, FileMode.Create);

                    RijndaelManaged RMCrypto = new RijndaelManaged();

                    CryptoStream cs = new CryptoStream(fsCrypt,
                        RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                    Stream streams = new MemoryStream(FileData);

                    int data;
                    while ((data = streams.ReadByte()) != -1)
                        cs.WriteByte((byte)data);

                    streams.Close();
                    cs.Close();
                    fsCrypt.Close();
                }
                await dmsDbContext.UploadedDocuments.AddAsync(uploadedDocument);
                await dmsDbContext.SaveChangesAsync();
                return uploadedDocument.UploadedDocumentId;
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region Get Parameters

        public async Task<List<CaseTemplateParameter>> GetTemplateParameters(int? moduleId)
        {
            try
            {
                return await _dbContext.CaseTemplateParameters.Where(p => p.IsActive && (moduleId != null ? p.ModuleId == moduleId : true)).OrderBy(u => u.ParameterId).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Save Case Template

        //<History Author = 'Hassan Abbas' Date='2023-08-21' Version="1.0" Branch="master"> Save Case Template</History>
        public async Task<CaseTemplate> SaveCaseTemplate(CaseTemplate template)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            if (template.Id > 0)
                            {
                                await UpdateCaseTemplate(template, _dbContext);
                            }
                            else
                            {
                                await CreateCaseTemplate(template, _dbContext);
                            }
                            transaction.Commit();
                            return template;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-08-21' Version="1.0" Branch="master"> Update Case Template</History>
        private async Task UpdateCaseTemplate(CaseTemplate template, DatabaseContext dbContext)
        {
            try
            {
                dbContext.CaseTemplate.Update(template);
                await dbContext.SaveChangesAsync();

                if (template.Id > (int)CaseTemplateEnum.HeaderAr)
                {
                    CaseTemplateSection section = new CaseTemplateSection();
                    section = dbContext.CaseTemplateSections.Where(s => s.TemplateId == template.Id && s.SectionId == (int)CaseTemplateSectionEnum.Body).FirstOrDefault();
                    if (section != null)
                    {
                        dbContext.CaseTemplateSectionParameters.RemoveRange(dbContext.CaseTemplateSectionParameters.Where(p => p.TemplateSectionId == section.Id));
                    }
                    if (template.Parameters.Any())
                    {
                        if (section == null)
                        {
                            section = new CaseTemplateSection();
                            section.TemplateId = template.Id;
                            section.SectionId = (int)CaseTemplateSectionEnum.Body;
                            await dbContext.CaseTemplateSections.AddAsync(section);
                            await dbContext.SaveChangesAsync();
                        }
                        foreach (var param in template.Parameters)
                        {
                            CaseTemplateSectionParameter sectionParameter = new CaseTemplateSectionParameter();
                            sectionParameter.ParameterId = param.ParameterId;
                            sectionParameter.TemplateSectionId = section.Id;
                            await dbContext.CaseTemplateSectionParameters.AddAsync(sectionParameter);
                            await dbContext.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-08-21' Version="1.0" Branch="master"> Create Case Template</History>
        private async Task CreateCaseTemplate(CaseTemplate template, DatabaseContext dbContext)
        {
            try
            {
                await dbContext.CaseTemplate.AddAsync(template);
                await dbContext.SaveChangesAsync();

                if (template.Parameters.Any())
                {
                    CaseTemplateSection section = new CaseTemplateSection();
                    section.TemplateId = template.Id;
                    section.SectionId = (int)CaseTemplateSectionEnum.Body;
                    await dbContext.CaseTemplateSections.AddAsync(section);
                    await dbContext.SaveChangesAsync();

                    foreach (var param in template.Parameters)
                    {
                        CaseTemplateSectionParameter sectionParameter = new CaseTemplateSectionParameter();
                        sectionParameter.ParameterId = param.ParameterId;
                        sectionParameter.TemplateSectionId = section.Id;
                        await dbContext.CaseTemplateSectionParameters.AddAsync(sectionParameter);
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
        #region Save Draft Stamp
        public async Task<CmsDraftStamp> SaveDraftStamp(CmsDraftStamp template)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            if (template.Id > 0)
                            {
                                _dbContext.SaveDraftStamp.Update(template);
                                await _dbContext.SaveChangesAsync();
                            }
                            else
                            {
                                await _dbContext.SaveDraftStamp.AddAsync(template);
                                await _dbContext.SaveChangesAsync();
                            }
                            transaction.Commit();
                            return template;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region Get Case Template

        //<History Author = 'Hassan Abbas' Date='2023-09-24' Version="1.0" Branch="master"> Get Case Template</History>
        public async Task<CaseTemplate> GetCaseTemplate(int templateId)
        {
            try
            {
                return await _dbContext.CaseTemplate.Where(x => x.Id == templateId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Masked docu save legislation/principle
        public async Task<TempAttachementVM> SaveMaskedDocumentInOriginalDocumentFolderForTemparory(TempAttachementVM viewFileDetail)
        {
            TempAttachementVM isSavedTemp = new TempAttachementVM();
            try
            {
                // if applied masking on judgment from legal principle then masked document will directly into uploaded document table
                if (viewFileDetail.UploadFrom.Contains("LLSLegalPrincipleSystem"))
                {
                    UploadedDocument uploadedDocument = new UploadedDocument()
                    {
                        Description = viewFileDetail.Description,
                        CreatedDateTime = (DateTime)viewFileDetail.UploadedDate,
                        CreatedBy = viewFileDetail.UploadedBy,
                        DocumentDate = viewFileDetail.DocumentDate != null ? (DateTime)viewFileDetail.DocumentDate : DateTime.Now,
                        FileName = viewFileDetail.FileName,
                        StoragePath = viewFileDetail.StoragePath,
                        DocType = viewFileDetail.DocType,
                        ReferenceGuid = viewFileDetail.Guid != null ? (Guid)viewFileDetail.Guid : new Guid(viewFileDetail.ReferenceGuid),
                        IsActive = true,
                        CreatedAt = viewFileDetail.StoragePath,
                        AttachmentTypeId = (int)viewFileDetail.AttachmentTypeId,
                        IsDeleted = false,
                        OtherAttachmentType = viewFileDetail.OtherAttachmentType,
                        ReferenceNo = viewFileDetail.ReferenceNo,
                        ReferenceDate = viewFileDetail.ReferenceDate,
                        FileNumber = viewFileDetail.FileNumber,
                        FileTitle = viewFileDetail.FileNameWithoutTimeStamp,
                        IsMaskedAttachment = viewFileDetail.IsMaskedAttachment,
                        StatusId = (int)SigningTaskStatusEnum.UnSigned
                    };
                    await _dmsDbContext.UploadedDocuments.AddAsync(uploadedDocument);
                    await _dmsDbContext.SaveChangesAsync();
                    viewFileDetail.AttachementId = uploadedDocument.UploadedDocumentId;
                }
                else
                {
                    TempAttachement attachement = new TempAttachement()
                    {
                        FileName = viewFileDetail.FileName,
                        StoragePath = viewFileDetail.StoragePath,
                        Guid = viewFileDetail.Guid != null ? (Guid)viewFileDetail.Guid : new Guid(viewFileDetail.ReferenceGuid),
                        UploadedDate = (DateTime)viewFileDetail.UploadedDate,
                        FileNameWithoutTimeStamp = viewFileDetail.FileNameWithoutTimeStamp,
                        UploadedBy = viewFileDetail.UploadedBy,
                        DocType = viewFileDetail.DocType,
                        AttachmentTypeId = (int)viewFileDetail.AttachmentTypeId,
                        Description = viewFileDetail.Description,
                        IsMaskedAttachment = viewFileDetail.IsMaskedAttachment
                    };

                    await _dmsDbContext.TempAttachements.AddAsync(attachement);
                    await _dmsDbContext.SaveChangesAsync();
                    viewFileDetail.AttachementId = attachement.AttachementId;
                }
                isSavedTemp = viewFileDetail;
            }
            catch (Exception)
            {
                isSavedTemp = new TempAttachementVM();
                throw;
            }
            return isSavedTemp;
        }
        public async Task<bool> LegislationAttachmentSaveFromTempAttachementToUploadedDocument(LegalLegislation resultLegislationObject)
        {
            using (_dmsDbContext)
            {
                using (var transaction = _dmsDbContext.Database.BeginTransaction())
                {
                    bool isSaved = true;
                    try
                    {
                        isSaved = await DeleteSelectedSourceDocumentList(resultLegislationObject.SelectedSourceDocumentForDelete, _dmsDbContext);
                        isSaved = await FromTempToUploadTable(resultLegislationObject.AttachedDocumentList, _dmsDbContext);
                        isSaved = await DeleteGarbageAttachmentFromTempTable(resultLegislationObject.MaskedDocumentAttachmentIdList, _dmsDbContext);
                        if (isSaved)
                            transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        isSaved = false;
                        throw;
                    }
                    return isSaved;
                }
            }
        }

        private async Task<bool> DeleteGarbageAttachmentFromTempTable(List<int?> maskedDocumentAttachmentIdList, DmsDbContext dmsDbContext)
        {
            bool isSaved = true;
            try
            {
                if (maskedDocumentAttachmentIdList.Count() != 0)
                {
                    foreach (var item in maskedDocumentAttachmentIdList)
                    {
                        var data = await dmsDbContext.TempAttachements.Where(x => x.AttachementId == item).FirstOrDefaultAsync();
                        if (data != null)
                        {
                            dmsDbContext.TempAttachements.Remove(data);
                        }
                    }
                    await dmsDbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                isSaved = false;
                throw new NotImplementedException();
            }
            return isSaved;
        }

        private async Task<bool> FromTempToUploadTable(List<TempAttachementVM> attachedDocumentList, DmsDbContext dmsDbContext)
        {
            bool isSaved = true;
            try
            {
                if (attachedDocumentList.Count() != 0)
                {
                    foreach (var item in attachedDocumentList)
                    {
                        var dataTemp = await dmsDbContext.TempAttachements.Where(x => (item.Guid != null ? x.Guid == item.Guid : x.Guid == new Guid(item.ReferenceGuid)) && x.FileName == item.FileName).FirstOrDefaultAsync();
                        if (dataTemp != null)
                        {
                            UploadedDocument ObjFill = new UploadedDocument()
                            {
                                Description = dataTemp.Description,
                                CreatedDateTime = dataTemp.UploadedDate,
                                CreatedBy = dataTemp.UploadedBy,
                                DocumentDate = dataTemp.DocumentDate != null ? (DateTime)dataTemp.DocumentDate : DateTime.Now,
                                FileName = dataTemp.FileName,
                                StoragePath = dataTemp.StoragePath,
                                DocType = dataTemp.DocType,
                                ReferenceGuid = dataTemp.Guid,
                                IsActive = true,
                                CreatedAt = dataTemp.StoragePath,
                                AttachmentTypeId = dataTemp.AttachmentTypeId,
                                IsDeleted = false,
                                OtherAttachmentType = dataTemp.OtherAttachmentType,
                                ReferenceNo = dataTemp.ReferenceNo,
                                ReferenceDate = dataTemp.ReferenceDate,
                                FileNumber = dataTemp.FileNumber,
                                FileTitle = dataTemp.FileNameWithoutTimeStamp,
                                IsMaskedAttachment = false,
                            };

                            await dmsDbContext.UploadedDocuments.AddAsync(ObjFill);
                            await dmsDbContext.SaveChangesAsync();
                            await Task.Delay(200);

                            dmsDbContext.TempAttachements.Remove(dataTemp);
                            await dmsDbContext.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception)
            {
                isSaved = false;
                throw new NotImplementedException();
            }
            return isSaved;
        }

        private async Task<bool> DeleteSelectedSourceDocumentList(List<TempAttachementVM> selectedSourceDocumentForDelete, DmsDbContext dmsDbContext)
        {
            bool isSaved = true;
            try
            {
                if (selectedSourceDocumentForDelete.Count() != 0)
                {
                    foreach (var item in selectedSourceDocumentForDelete)
                    {
                        if (item.Guid != null)
                        {
                            var resultFromTemp = await dmsDbContext.TempAttachements.Where(x => x.Guid == item.Guid && x.FileName == item.FileName).FirstOrDefaultAsync();
                            if (resultFromTemp != null)
                            {
                                if (resultFromTemp.StoragePath != null)
                                {
#if DEBUG
                                    {
                                        string physicalPath = Path.Combine(Directory.GetCurrentDirectory() + resultFromTemp.StoragePath);
                                        physicalPath = physicalPath.Replace("DMS_API", "DMS_WEB");

                                        if (System.IO.File.Exists(physicalPath))
                                        {
                                            System.IO.File.Delete(physicalPath);
                                            dmsDbContext.TempAttachements.Remove(resultFromTemp);
                                            await dmsDbContext.SaveChangesAsync();
                                        }
                                    }
#else
                                    {
                                        dmsDbContext.TempAttachements.Remove(resultFromTemp);
                                        await dmsDbContext.SaveChangesAsync();
                                    }
#endif
                                }
                            }
                        }
                        else
                        {
                            var resultFromUpload = await dmsDbContext.UploadedDocuments.Where(x => x.ReferenceGuid == new Guid(item.ReferenceGuid) && x.FileName == item.FileName).FirstOrDefaultAsync();
                            if (resultFromUpload != null)
                            {
                                if (resultFromUpload.StoragePath != null)
                                {
#if DEBUG
                                    {
                                        string physicalPath = Path.Combine(Directory.GetCurrentDirectory() + resultFromUpload.StoragePath);
                                        physicalPath = physicalPath.Replace("DMS_API", "DMS_WEB");

                                        if (System.IO.File.Exists(physicalPath))
                                        {
                                            System.IO.File.Delete(physicalPath);
                                            dmsDbContext.UploadedDocuments.Remove(resultFromUpload);
                                            await dmsDbContext.SaveChangesAsync();
                                        }
                                    }
#else
                                    {
                                        dmsDbContext.UploadedDocuments.Remove(resultFromUpload);
                                        await dmsDbContext.SaveChangesAsync();
                                    }
#endif
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                isSaved = false;
                throw new NotImplementedException();
            }
            return isSaved;
        }
        #endregion

        #region Get Templates List
        public async Task<List<DMSTemplateListVM>> GetTemplatesList(TemplateListAdvanceSearchVM advanceSearchVM)

        {
            try
            {
                string StoredProc;
                if (_DmsTemplateListVMs == null)
                {
                    string requestFrom = advanceSearchVM.CreatedFrom != null ? Convert.ToDateTime(advanceSearchVM.CreatedFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string requestTo = advanceSearchVM.CreatedTo != null ? Convert.ToDateTime(advanceSearchVM.CreatedTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    StoredProc = $"exec pDmsGetTemplateList @Templatename =N'{advanceSearchVM.Templatename}'  , @CreatedFrom='{requestFrom}' , @CreatedTo='{requestTo}', @attachmentTypeId='{advanceSearchVM.AttachmentTypeId}', @CreatedBy='{advanceSearchVM.CreatedBy}'";
                    _DmsTemplateListVMs = await _dbContext.DMSTemplateListVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _DmsTemplateListVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-09-03' Version="1.0" Branch="master">Get Header Footer Templates</History>
        public async Task<List<CaseTemplate>> GetHeaderFooterTemplates()
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
        #endregion
        #region Get Attachment Type Detail By Id
        public async Task<AttachmentType> GetAttachmentTypeDetailById(int? AttachmentTypeId)
        {
            try
            {
                return await _dmsDbContext.AttachmentType.Where(x => x.AttachmentTypeId == AttachmentTypeId).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        #endregion

        #region Convert contract template into document and save into uploaded document table 
        public async Task<bool> SaveContractTemplateToDocument(ConsultationRequest item, string? fileName, string? physicalPath)
        {
            bool isSaved = true;
            try
            {
                // Delete existing attachment with PreviousAttachmentTypeId.
                if (item.PreviousAttachmentTypeId != null)
                {
                    var attachmentToDelete = await _dmsDbContext.UploadedDocuments.Where(x => x.AttachmentTypeId == (int)item.PreviousAttachmentTypeId && x.ReferenceGuid == item.ConsultationRequestId).FirstOrDefaultAsync();
                    if (attachmentToDelete != null)
                    {
                        _dmsDbContext.UploadedDocuments.Remove(attachmentToDelete);
                        await _dmsDbContext.SaveChangesAsync();
                    }
                }
                // Add New attachment with AttachmentTypeId.
                Guid CommunicationId = Guid.Empty;
                UploadedDocument attachment = new UploadedDocument
                {
                    AttachmentTypeId = (int)item.AttachmentTypeId,
                    ReferenceGuid = item.ConsultationRequestId,
                    CommunicationGuid = CommunicationId,
                    DocumentDate = DateTime.Now,
                    Description = item.Description != null ? item.Description : null,
                    FileName = fileName,
                    FileNumber = null,
                    DocType = Path.GetExtension(".pdf"),
                    IsActive = true,
                    CreatedBy = item.CreatedBy,
                    CreatedDateTime = DateTime.Now,
                    CreatedAt = physicalPath,
                    StoragePath = physicalPath,
                    StatusId = (int)SigningTaskStatusEnum.UnSigned
                };

                await _dmsDbContext.UploadedDocuments.AddAsync(attachment);
                await _dmsDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }
        #endregion

        #region Convert MOM template into document and save into uploaded document table
        public async Task<bool> SaveMOMTemplateToDocument(MeetingMom meetingMom, string? fileName, string? physicalPath)
        {
            bool isSaved = true;
            try
            {
                Guid CommunicationId = Guid.Empty;
                UploadedDocument attachement = new UploadedDocument()
                {
                    AttachmentTypeId = (int)AttachmentTypeEnum.SignedMOMAttachment,
                    ReferenceGuid = meetingMom.MeetingMomId,
                    CommunicationGuid = CommunicationId,
                    DocumentDate = DateTime.Now,
                    Description = meetingMom.Description != null ? meetingMom.Description : null,
                    FileName = fileName,
                    FileNumber = null,
                    DocType = Path.GetExtension(".pdf"),
                    IsActive = true,
                    CreatedBy = meetingMom.CreatedBy,
                    CreatedDateTime = DateTime.Now,
                    CreatedAt = physicalPath,
                    StoragePath = physicalPath,
                    StatusId = (int)SigningTaskStatusEnum.UnSigned
                };

                await _dmsDbContext.UploadedDocuments.AddAsync(attachement);
                await _dmsDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }
        #endregion

        #region Update Template Status
        public async Task UpdateTemplateStatus(bool isAtive, int id)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var template = _dbContext.CaseTemplate.Where(i => i.Id == id).FirstOrDefault();
                            template.IsActive = isAtive;
                            _dbContext.Entry(template).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Copy LDS Source Attachments
        //<History Author = 'ijaz Ahmad' Date='2023-04-07' Version="1.0" Branch="master"> Copy Kuwait Alaywm Publication  Documents  to Temp</History>
        public async Task CopyLegalLegislationSourceAttachments(CopyLegalLegislationSourceAttachmentsVM copyAttachments, string basePath)
        {
            try
            {
                using (_dmsDbContext)
                {
                    using (var transaction = _dmsDbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var documentId in copyAttachments.KayselectedDocumentsIds)
                            {
                                var requestKayDoc = await _dmsDbContext.KayPublications.FindAsync(documentId);
                                if (requestKayDoc != null)
                                {
                                    TempAttachement tempAttachement = new TempAttachement
                                    {
                                        Guid = copyAttachments.DestinationId,
                                        UploadedBy = copyAttachments.CreatedBy,
                                        UploadedDate = DateTime.Now,
                                        DocType = Path.GetExtension(requestKayDoc.FileTitle),
                                        OtherAttachmentType = null,
                                        Description = null,
                                        ReferenceNo = null,
                                        ReferenceDate = requestKayDoc.PublicationDate,
                                        //  DocumentDate = requestKayDoc.PublicationDate,
                                        FileTitle = requestKayDoc.DocumentTitle,
                                        FileNumber = requestKayDoc.EditionNumber
                                    };

                                    tempAttachement.AttachmentTypeId = (int)AttachmentTypeEnum.KuwaitAlYawm;
                                    string kayFilePath = requestKayDoc.StoragePath;
                                    if (File.Exists(kayFilePath))
                                    {
                                        //replacing the timestamp with new timestamp
                                        var lastUnderscoreIndex = requestKayDoc.FileTitle.LastIndexOf("_");
                                        string result = string.Empty;
                                        if (lastUnderscoreIndex >= 0 && lastUnderscoreIndex < requestKayDoc.FileTitle.Length - 1)
                                        {
                                            tempAttachement.FileNameWithoutTimeStamp = requestKayDoc.FileTitle.Substring(0, lastUnderscoreIndex + 1);
                                            result = requestKayDoc.FileTitle.Substring(0, lastUnderscoreIndex + 1) + DateTime.Now.ToString("yyyyMMddHHmmssffff");
                                        }
                                        tempAttachement.FileName = $"{result}{Path.GetExtension(requestKayDoc.FileTitle)}";
                                        string newFileStoragePath = basePath + "\\" + "Legislation" + "\\" + tempAttachement.FileName;
                                        // copy of Existing File in Folder with new Name
                                        File.Copy(kayFilePath, newFileStoragePath, true);
                                        // Save New File Name in Db
                                        tempAttachement.StoragePath = Path.Combine(string.Join("\\", "\\wwwroot\\Attachments" + "\\" + "Legislation" + "\\" + tempAttachement.FileName));

                                        await _dmsDbContext.TempAttachements.AddAsync(tempAttachement);
                                        await _dmsDbContext.SaveChangesAsync();
                                    }
                                }
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Kay Publication
        //<History Author = 'ijaz Ahmad' Date='2023-12-26' Version="1.0" Branch="master"> Create Kuwait Alyawm publication</History>
        public async Task<KayPublication> SaveKayPublicationDocument(KayPublication documentObj)
        {
            try
            {
                await _dmsDbContext.KayPublications.AddAsync(documentObj);
                await _dmsDbContext.SaveChangesAsync();
                return documentObj;
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }
        }
        #endregion

        #region Get Kay Publication Documents List
        public async Task<List<DMSKayPublicationDocumentListVM>> GetKayDocumentsList(KayDocumentListAdvanceSearchVM advanceSearchVM)
        {
            try
            {

                string StoredProc;
                if (_dMSKayPublicationDocumentListVMs == null)
                {
                    string requestFrom = advanceSearchVM.PublicationFrom != null ? Convert.ToDateTime(advanceSearchVM.PublicationFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string requestTo = advanceSearchVM.PublicationTo != null ? Convert.ToDateTime(advanceSearchVM.PublicationTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    StoredProc = $"exec pDmsGetKayPublicationDocumentList @EditionNumber =N'{advanceSearchVM.EditionNumber}', @PublicationFrom='{requestFrom}' , @PublicationTo='{requestTo}'" +
                        $", @EditionType='{advanceSearchVM.EditionType}', @DocumentTitle='{advanceSearchVM.DocumentTitle}', @PublicationDateHijri= '{advanceSearchVM.PublicationDateHijri}'" +
                        $" ,@PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}', @FromLegalLegislationForm = '{advanceSearchVM.FromLegalLegislationForm}',@IsFullEdition='{advanceSearchVM.IsFullEdition}'";
                    _dMSKayPublicationDocumentListVMs = await _dmsDbContext.DMSKayPublicationDocumentListVMs.FromSqlRaw(StoredProc).ToListAsync();

                }
                return _dMSKayPublicationDocumentListVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<DMSKayPublicationDocumentListVM> GetkayDocumentAccordingEditionNumber(string editionNumber)
        {
            try
            {
                var kayPublication = await _dmsDbContext.KayPublications.FirstOrDefaultAsync(x => x.EditionNumber == editionNumber && x.IsFullEdition);
                if (kayPublication != null)
                {
                    return new DMSKayPublicationDocumentListVM
                    {
                        Id = kayPublication.Id,
                        EditionNumber = kayPublication.EditionNumber,
                        EditionType = kayPublication.EditionType,
                        IsFullEdition = kayPublication.IsFullEdition,
                        PublicationDate = kayPublication.PublicationDate,
                        PublicationDateHijri = kayPublication.PublicationDateHijri,
                        FileTitle = kayPublication.FileTitle,
                        DocumentTitle = kayPublication.DocumentTitle,
                        StoragePath = kayPublication.StoragePath,
                        CreatedDate = kayPublication.CreatedDate,
                        TotalCount = 0
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        
        public async Task<List<LLSLegalPrincipleKuwaitAlYoumDocuments>> GetKayDocumentsListForLLSLegalPrinciple(LLSLegalPrincipalDocumentSearchVM advanceSearchVM)
        {
            try
            {
                string requestFrom = advanceSearchVM.FromDate != null ? Convert.ToDateTime(advanceSearchVM.FromDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string requestTo = advanceSearchVM.ToDate != null ? Convert.ToDateTime(advanceSearchVM.ToDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string storedProc = $"exec pLLSKuwaitAlYoumSourceDocuments @EditionNumber =N'{advanceSearchVM.EditionNumber}', @PublicationFrom='{requestFrom}' , @PublicationTo='{requestTo}', @EditionType='{advanceSearchVM.EditionType}', @DocumentTitle='{advanceSearchVM.DocumentTitle}', @PublicationDateHijri= '{advanceSearchVM.PublicationDateHijri}' " +
                                    $",@PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}'";
                var kuwaitAlYoumDocuments = await _dmsDbContext.LLSLegalPrincipleKuwaitAlYoumDocuments.FromSqlRaw(storedProc).ToListAsync();
                return kuwaitAlYoumDocuments;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Moj ROlls
        //<History Author = 'ijaz Ahmad' Date='2023-12-26' Version="1.0" Branch="master"> Save File to Upload Moj Rolls Request Document</History>
        public async Task<UploadedDocument> SaveUploadedDocument(UploadedDocument documentObj)
        {
            try
            {
                documentObj.StatusId = (int)SigningTaskStatusEnum.UnSigned;
                await _dmsDbContext.UploadedDocuments.AddAsync(documentObj);
                await _dmsDbContext.SaveChangesAsync();
                return documentObj;
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }
        }
        // <History Author = 'Ijaz Ahmad' Date='2024-01-24' Version="1.0" Branch="master"> Get Moj Roll uploaded document by id</History>
        public async Task<UploadedDocument> GetMojRollDocumentById(int? DocumentId)
        {
            try
            {

                var uploadedDocument = await _dmsDbContext.UploadedDocuments.Where(x => x.UploadedDocumentId == DocumentId).FirstOrDefaultAsync();
                if (uploadedDocument is not null)
                {
                    return uploadedDocument;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion

        #region Save MOJ Pushing Document
        public async Task<UploadedDocument> SaveMojImageDocument(MojDocument mojDocument)
        {
            try
            {
                await _dmsDbContext.MojDocuments.AddAsync(mojDocument);
                await _dmsDbContext.SaveChangesAsync();

                UploadedDocument documentObj = null;
                var registeredCase = await _dbContext.cmsRegisteredCases.FirstOrDefaultAsync(x => x.CaseNumber == mojDocument.CaseNumber);
                if (registeredCase != null)
                {
                    //  save the UploadedDocument
                    documentObj = new UploadedDocument
                    {
                        Description = "-",
                        CreatedDateTime = mojDocument.DocumentDate,
                        CreatedBy = mojDocument.CreatedBy,
                        DocumentDate = mojDocument.DocumentDate,
                        FileName = mojDocument.FileName,
                        StoragePath = mojDocument.StoragePath,
                        DocType = ".pdf",
                        ReferenceGuid = registeredCase.CaseId,
                        IsActive = true,
                        CreatedAt = mojDocument.StoragePath,
                        AttachmentTypeId = mojDocument.AttachmentTypeId,
                        IsDeleted = false,
                        StatusId = (int)SigningTaskStatusEnum.UnSigned
                    };
                    await _dmsDbContext.UploadedDocuments.AddAsync(documentObj);
                    await _dmsDbContext.SaveChangesAsync();
                }

                return documentObj;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<List<MojDocumentVM>> GetMojImageDocumentList(MojDocumentAdvanceSearchVM advanceSearchVM)
        {
            try
            {

                string StoredProc;
                if (_mojDocumentListVMs == null)
                {
                    string requestFrom = advanceSearchVM.CreatedFrom != null ? Convert.ToDateTime(advanceSearchVM.CreatedFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string requestTo = advanceSearchVM.CreatedTo != null ? Convert.ToDateTime(advanceSearchVM.CreatedTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    StoredProc = $"exec pMojImagesDocumentList @CANNumber =N'{advanceSearchVM.CANNumber}', @CreatedFrom='{requestFrom}', @CreatedTo='{requestTo}', @AttachmentTypeId='{advanceSearchVM.AttachmentTypeId}' ";
                    _mojDocumentListVMs = await _dmsDbContext.MojDocumentVMs.FromSqlRaw(StoredProc).ToListAsync();

                }
                return _mojDocumentListVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<MojDocumentVM>> GetMojDocumentByCaseNumber(string caseNumber)
        {
            try
            {

                string StoredProc;
                if (_mojDocumentListVMs == null)
                {

                    StoredProc = $"exec pGetMojImagesDocumentListbyCaseNumber @CaseNumber =N'{caseNumber}' ";
                    _mojDocumentListVMs = await _dmsDbContext.MojDocumentVMs.FromSqlRaw(StoredProc).ToListAsync();

                }
                return _mojDocumentListVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Save Archived Case Documents
        //<History Author = 'Ammaar Naveed' Date='17-12-2024' Version="1.0" Branch="master">Save archived case documents</History>
        public async Task SaveArchivedCaseDocuments(ArchivedCaseDocuments documentObj)
        {
            try
            {
                await _archivedCasesDbContext.ArchivedCaseDocuments.AddAsync(documentObj);
                await _archivedCasesDbContext.SaveChangesAsync();
                return;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Template Detail

        //<History Author = 'Hassan Abbas' Date='2024-03-27' Version="1.0" Branch="master"> Get Case Template Information</History>
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
        #endregion

        #region User Personal Info Detail

        //<History Author = 'Hassan Abbas' Date='2024-03-27' Version="1.0" Branch="master"> Get User Personal Information</History>
        public async Task<UserPersonalInformation> GetUserPersonalInformationByUserId(string userId)
        {
            try
            {
                return await _dbContext.UserPersonalInformation.Where(t => t.UserId == userId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Remove Temp Attachments By ReferenceId

        //<History Author = 'Hassan Abbas' Date='2024-04-01' Version="1.0" Branch="master">Remove Temp Attachments </History>
        public async Task RemoveTempAttachementsByReferenceId(Guid referenceId, string basePath)
        {
            try
            {
                var tempAttachments = await _dmsDbContext.TempAttachements.Where(t => t.Guid == referenceId).ToListAsync();
                foreach (var attachment in tempAttachments)
                {
                    string filePath = basePath + attachment.StoragePath;
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    _dmsDbContext.TempAttachements.Remove(attachment);
                    await _dmsDbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion


        //<History Author = 'Muhammad Abuzar' Date='2024-04-22' Version="1.0" Branch="master">temp attachement</History>
        public async Task<List<UploadedDocumentVM>> GetLLSLegalPrincipleReferenceUploadedAttachements(Guid principleId)
        {
            try
            {
                string StoredProc = $"exec pGetLLSlegalPrincipleReferenceDocuments @PrincipleId = '{principleId}'";
                var res = await _dmsDbContext.UploadedDocumentVM.FromSqlRaw(StoredProc).ToListAsync();
                return res;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public async Task<int> UpdateDocument(UploadedDocument uploadedDocument)
        {
            int isSaved = 0;
            try
            {
                _dmsDbContext.UploadedDocuments.Update(uploadedDocument);
                await _dmsDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                isSaved = 1; // 1 mean error because Diyar consider 0 as success
                throw;
            }
            return isSaved;
        }

        public async Task GetLatestVersionAndUpdateDocumentVersion(Guid versionId)
        {
            try
            {
                var DraftedTemplateId = await _dbContext.CmsDraftedTemplateVersions.Where(x => x.VersionId == versionId).Select(x => x.DraftedTemplateId).FirstOrDefaultAsync();
                var latestVersions = await _dbContext.CmsDraftedTemplateVersions.Where(x => x.DraftedTemplateId == DraftedTemplateId).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();


                var document = _dmsDbContext.UploadedDocuments.Where(x => x.VersionId == versionId).FirstOrDefault();
                document.VersionId = latestVersions.VersionId;
                _dmsDbContext.UploadedDocuments.Update(document);
                await _dmsDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Digital Signature
        public async Task<UploadedDocument> GetSignatureImagePath(string userId)
        {
            try
            {
                return await _dmsDbContext.UploadedDocuments.Where(x => x.ReferenceGuid == Guid.Parse(userId) && x.AttachmentTypeId == (int)AttachmentTypeEnum.SignatureImage).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<string> GetCivilId(string UserId)
        {
            try
            {

                return await _dbContext.UserPersonalInformation.Where(x => x.UserId == UserId).Select(x => x.CivilId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task SaveDSPRequestLog(DSPRequestLog requestLog)
        {
            try
            {
                await _dmsDbContext.DSPRequestLogs.AddAsync(requestLog);
                await _dmsDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DSPRequestLog> UpdateDSPRequestLog(string RequestId, string RequestStatus)
        {
            try
            {
                var requestlog = await _dmsDbContext.DSPRequestLogs.Where(x => x.RequestId == RequestId).FirstOrDefaultAsync();
                requestlog.RequestStatus = RequestStatus;
                requestlog.ModifiedBy = "DSP System";
                requestlog.ModifiedDate = DateTime.Now;
                _dmsDbContext.Update(requestlog);
                await _dmsDbContext.SaveChangesAsync();
                return requestlog;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GetDSPSigningRequestStatus(string RequestId)
        {
            try
            {
                return await _dmsDbContext.DSPRequestLogs.Where(x => x.RequestId == RequestId).Select(x => x.RequestStatus).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> GetIsAlreadySigned(string civilId, int documentId)
        {
            try
            {
                return await _dmsDbContext.DSPRequestLogs.Where(x => x.CivilId == civilId && x.DocumentId == documentId && x.RequestStatus == SigningRequestStatusEnum.Approved.GetDisplayName()).AnyAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveDSPAuthenticationRequestLog(DSPAuthenticationRequestLog authenticationRequestLog)
        {
            try
            {
                await _dmsDbContext.DSPAuthenticationRequestLogs.AddAsync(authenticationRequestLog);
                await _dmsDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateDSPAuthenticationRequestLog(DSPAuthenticationResponse dSPAuthenticationResponse)
        {
            try
            {
                var authRequest = await _dmsDbContext.DSPAuthenticationRequestLogs
                                        .Where(x => x.RequestId == dSPAuthenticationResponse.MIDAuthSignResponse.RequestDetails.RequestID)
                                        .FirstOrDefaultAsync();
                authRequest.ResponsePayload = Newtonsoft.Json.JsonConvert.SerializeObject(dSPAuthenticationResponse);
                authRequest.ModifiedBy = "DSP System";
                authRequest.ModifiedDate = DateTime.Now;
                _dmsDbContext.DSPAuthenticationRequestLogs.Update(authRequest);
                await _dmsDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DSPAuthenticationRequestLog> GetDSPAuthenticationRequestLog(string RequestId)
        {
            try
            {
                return await _dmsDbContext.DSPAuthenticationRequestLogs.Where(x => x.RequestId == RequestId).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DsSigningMethods>> GetSigningMethods()
        {
            try
            {
                return await _dmsDbContext.DsSigningMethods.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> GetSignatureProfileName(int methodId)
        {
            try
            {
                return await _dmsDbContext.DsSigningMethods.Where(x => x.MethodId == methodId).Select(x => x.SignatureProfileName).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Save Literature Uploaded Document ("this region only used for Adding and Editing Literature" ) 

        public async Task<bool> SaveLiteratureTempAttachmentToUploadedDocument(FileUploadVM item)
        {
            using (_dmsDbContext)
            {
                using (var transaction = _dmsDbContext.Database.BeginTransaction())
                {
                    bool isSaved = true;
                    try
                    {
                        isSaved = await DeleteFileFromPath(item);
                        if (!item.IsRequestForMeetingSaveAsDraft)
                        {
                            isSaved = await SaveLiteratureUploadedDocuments(item);
                        }
                        if (isSaved)
                            transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        isSaved = false;
                        throw;
                    }
                    return isSaved;
                }
            }
        }

        protected async Task<bool> SaveLiteratureUploadedDocuments(FileUploadVM item)
        {
            bool isSaved = true;
            try
            {
                List<TempAttachement> tempAttachement = new List<TempAttachement>();
                foreach (var requestId in item.RequestIds)
                {
                    var attachements = await _dmsDbContext.TempAttachements.Where(x => item.isCommunication ? x.CommunicationGuid == requestId : x.Guid == requestId).ToListAsync();
                    foreach (int LiteratureId in item.LiteratureIds)
                    {
                        foreach (TempAttachement file in attachements)
                        {
                            UploadedDocument documentObj = new UploadedDocument()
                            {
                                Description = file.Description,
                                CreatedDateTime = DateTime.Now,
                                CreatedBy = item.CreatedBy,
                                DocumentDate = file.DocumentDate != null ? (DateTime)file.DocumentDate : DateTime.Now,
                                //DocumentDate = (DateTime)file.DocumentDate, 
                                FileName = file.FileName,
                                StoragePath = file.StoragePath,
                                DocType = file.DocType,
                                ReferenceGuid = file.Guid,
                                CommunicationGuid = item.isCommunication ? requestId : Guid.Empty,
                                IsActive = true,
                                CreatedAt = file.StoragePath,
                                AttachmentTypeId = file.AttachmentTypeId,
                                IsDeleted = false,
                                OtherAttachmentType = file.OtherAttachmentType,
                                ReferenceNo = file.ReferenceNo,
                                ReferenceDate = file.ReferenceDate,
                                FileNumber = file.FileNumber,
                                FileTitle = file.FileTitle,
                                //LiteratureId = item.LiteratureId
                                LiteratureId = LiteratureId,
                                StatusId = (int)SigningTaskStatusEnum.UnSigned
                            };
                            await _dmsDbContext.UploadedDocuments.AddAsync(documentObj);
                            await _dmsDbContext.SaveChangesAsync();
                            await Task.Delay(200);

                            //_dmsDbContext.TempAttachements.Remove(file);
                            //await _dmsDbContext.SaveChangesAsync();
                            tempAttachement.Add(file);
                        }
                    }

                }
                _dmsDbContext.TempAttachements.RemoveRange(tempAttachement);
                await _dmsDbContext.SaveChangesAsync();

            }
            catch (Exception)
            {
                isSaved = true;
                throw;
            }
            return isSaved;
        }

        #region  Get Uploaded Attachements By LiteratureId and add With new Entery

        public async Task<bool> GetUploadedAttachementAndWithNewOne(FileUploadVM item)
        {
            try
            {
                foreach (var requestId in item.RequestIds)
                {
                    //List<UploadedDocument> literatureAttechement = GetUploadedAttachementsByLiteratureId(item.LiteratureId);
                    var attachements = await _dmsDbContext.UploadedDocuments.Where(x => x.LiteratureId == item.LiteratureId).ToListAsync();
                    foreach (int LiteratureId in item.LiteratureIds)
                    {
                        foreach (var file in attachements)
                        {
                            UploadedDocument documentObj = new UploadedDocument()
                            {
                                Description = file.Description,
                                CreatedDateTime = DateTime.Now,
                                CreatedBy = item.CreatedBy,
                                DocumentDate = file.DocumentDate != null ? (DateTime)file.DocumentDate : DateTime.Now,
                                //DocumentDate = (DateTime)file.DocumentDate, 
                                FileName = file.FileName,
                                StoragePath = file.StoragePath,
                                DocType = file.DocType,
                                ReferenceGuid = file.ReferenceGuid,
                                CommunicationGuid = item.isCommunication ? requestId : Guid.Empty,
                                IsActive = true,
                                CreatedAt = file.StoragePath,
                                AttachmentTypeId = file.AttachmentTypeId,
                                IsDeleted = false,
                                OtherAttachmentType = file.OtherAttachmentType,
                                ReferenceNo = file.ReferenceNo,
                                ReferenceDate = file.ReferenceDate,
                                FileNumber = file.FileNumber,
                                FileTitle = file.FileTitle,
                                //LiteratureId = item.LiteratureId
                                LiteratureId = LiteratureId,
                                StatusId = (int)SigningTaskStatusEnum.UnSigned
                            };
                            await _dmsDbContext.UploadedDocuments.AddAsync(documentObj);
                            await _dmsDbContext.SaveChangesAsync();
                            await Task.Delay(200);

                            //_dmsDbContext.TempAttachements.Remove(file);
                            //await _dmsDbContext.SaveChangesAsync();
                            //tempAttachement.Add(file);
                        }
                    }

                }

                return true;
            }
            catch (Exception)
            {
                throw;
                return false;
            }
        }
        #endregion


        public async Task<List<TempAttachement>> CheckingAttachementInTemp(FileUploadVM item)
        {
            try
            {
                List<TempAttachement> attachements = new List<TempAttachement>();
                foreach (var requestId in item.RequestIds)
                {
                    attachements = await _dmsDbContext.TempAttachements.Where(x => item.isCommunication ? x.CommunicationGuid == requestId : x.Guid == requestId).ToListAsync();
                }
                return attachements;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region get document by Id for legal legislation
        public async Task<KayPublication> GetAttachementById(int Id)
        {
            try
            {
                var result = await _dmsDbContext.KayPublications.Where(x => x.Id == Id).FirstOrDefaultAsync();
                if (result is not null)
                    return result;
                return null;

            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        #endregion

        #region Remove Document By Reference Id and Attachment Type Id

        public async Task RemoveDocumentByReferenceGuidAndAttachmentTypeId(string basePath, Guid referenceGuid, int AttachmentTypeId)
        {
            try
            {
                var uploadedDocument = await _dmsDbContext.UploadedDocuments.Where(t => t.ReferenceGuid == referenceGuid && t.AttachmentTypeId == AttachmentTypeId).ToListAsync();
                if (uploadedDocument.Any())
                {
                    foreach (var attachment in uploadedDocument)
                    {
                        string filePath = basePath + attachment.StoragePath;
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        _dmsDbContext.UploadedDocuments.Remove(attachment);
                        await _dmsDbContext.SaveChangesAsync();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Remove Temp by party id

        public async Task<string> GetTemById(Guid referenceGuid)
        {
            try
            {

                var tempDocument = await _dmsDbContext.TempAttachements.Where(x => x.Guid == referenceGuid).FirstOrDefaultAsync();
                if (tempDocument is not null)
                {
                    return tempDocument.StoragePath;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        public async Task<bool> RemoveDocumentFromTemp(Guid referenceGuid)
        {
            bool isSaved = true;
            try
            {

                var temp = await _dmsDbContext.TempAttachements.Where(x => x.Guid == referenceGuid).ToListAsync();
                if (temp is not null)
                {
                    foreach (var item in temp)
                    {
                        _dmsDbContext.TempAttachements.Remove(item);
                    }
                    await _dmsDbContext.SaveChangesAsync();
                }

            }
            catch (Exception)
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }
        #endregion
    }

}

