using AutoMapper;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Interfaces.Communication;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.Shared;
using FATWA_INFRASTRUCTURE.Database;
using FATWA_INFRASTRUCTURE.Repository.CaseManagement;
using FATWA_INFRASTRUCTURE.Repository.NotificationRepo;
using FATWA_INFRASTRUCTURE.Repository.PatternNumber;
using FATWA_INFRASTRUCTURE.Repository.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;

namespace FATWA_INFRASTRUCTURE.Repository.Communications
{
    public class CommunicationTarasolRPARepository : ICommunicationTarasolRPA
    {
        #region Variables declaration

        private readonly DatabaseContext _dbContext;
        private readonly DmsDbContext _dmsDbContext;

        private readonly TaskRepository _taskRepository;
        private readonly RoleRepository _roleRepository;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly CommunicationRepository _communicationRepository;
        private readonly IMapper _mapper;
        private readonly CMSCOMSInboxOutboxPatternNumberRepository _cMSCOMSInboxOutboxPatternNumberRepository;
        private readonly NotificationRepository _notificationRepository;
        private SendCommunicationVM communication1;
        private CommunicationTarasolRPAPayload tarassolPayLoadSend;
        public GEDepartments GEDepartments { get; set; } = new GEDepartments();
        public GovernmentEntity Entity { get; set; } = new GovernmentEntity();
        #endregion

        #region Constructor
        public CommunicationTarasolRPARepository(DatabaseContext dbContext, DmsDbContext dmsdbContext, IServiceScopeFactory serviceScopeFactory, IMapper mapper, CommunicationRepository communicationRepository)
        {
            _dbContext = dbContext;
            _dmsDbContext = dmsdbContext;
            _serviceScopeFactory = serviceScopeFactory;
            _communicationRepository = communicationRepository;

            using var scope = _serviceScopeFactory.CreateScope();
            _cMSCOMSInboxOutboxPatternNumberRepository = scope.ServiceProvider.GetRequiredService<CMSCOMSInboxOutboxPatternNumberRepository>();
            _notificationRepository = scope.ServiceProvider.GetRequiredService<NotificationRepository>();
            _roleRepository = scope.ServiceProvider.GetRequiredService<RoleRepository>();
            _mapper = mapper;
            communication1 = new SendCommunicationVM();
            communication1.Communication = new Communication();
            communication1.CommunicationTargetLink = new CommunicationTargetLink();
            communication1.LinkTarget = new List<LinkTarget>();
        }
        #endregion
        public async Task<SendCommunicationVM> AddCommunicationData(CommunicationTarasolRPAPayload payload)
        {

            bool isSaved;
            using (_dbContext)
            {

                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        communication1.Communication.CommunicationId = payload.Guid;
                        communication1.Communication.SourceId = (int)CommunicationSourceEnum.Tarasul;
                        communication1.Communication.ReferenceNumber = payload.LetterNumber;
                        communication1.Communication.Title = payload.Subject;
                        communication1.Communication.Description = payload.Comments;
                        communication1.CommunicationTargetLink.CommunicationId = payload.Guid;


                        DateTime parsedDateTime = DateTime.ParseExact(payload.SendingDate, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

                        string formattedDateTime = parsedDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                        communication1.Communication.ReferenceDate = DateTime.ParseExact(formattedDateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);


                        DateTime parsedDateTime1 = DateTime.ParseExact(payload.RecievingDate, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

                        string formattedDateTime1 = parsedDateTime1.ToString("yyyy-MM-dd HH:mm:ss");

                        communication1.Communication.InboxDate = DateTime.ParseExact(formattedDateTime1, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);


                        communication1.CommunicationTargetLink.TargetLinkId = Guid.NewGuid();

                        var governmentEntity = _dbContext.GovernmentEntity.Where(x => x.Name_Ar == payload.SenderSiteName).FirstOrDefault();
                        int entityId = 0;
                        int deptId = 0;
                        if (governmentEntity != null)
                        {
                            communication1.Communication.GovtEntityId = governmentEntity.EntityId;
                            var dept = _dbContext.GeDepartments.Where(x => x.Name_Ar == payload.SenderDepartmentSiteName && x.EntityId == governmentEntity.EntityId).FirstOrDefault();
                            if (dept == null)
                            {
                                GEDepartments.EntityId = governmentEntity.EntityId;
                                GEDepartments.Name_Ar = payload.SenderDepartmentSiteName;
                                GEDepartments.CreatedDate = DateTime.Now;
                                GEDepartments.CreatedBy = "G2G TARASOL RPA";
                                await _dbContext.GEDepartments.AddAsync(GEDepartments);
                                communication1.Communication.DepartmentId = GEDepartments.Id;
                                await _dbContext.SaveChangesAsync();
                            }
                            else
                            {
                                communication1.Communication.DepartmentId = dept.Id;

                            }

                        }
                        else
                        {
                            //Entity.EntityId = a.EntityId;
                            Entity.Name_Ar = payload.SenderSiteName;
                            Entity.Name_En = string.Empty;
                            Entity.CreatedDate = DateTime.Now;
                            Entity.CreatedBy = "G2G TARASOL RPA";
                            Entity.IsConfidential = false;
                            Entity.IsActive = false;
                            await _dbContext.GovernmentEntity.AddAsync(Entity);
                            await _dbContext.SaveChangesAsync();

                            entityId = Entity.EntityId;
                            //entityId = _dbContext.GovernmentEntity.Where(x => x.Name_Ar == payload.SenderSiteName).Select(y => y.EntityId).FirstOrDefault();

                            GEDepartments.EntityId = entityId;
                            GEDepartments.Name_Ar = payload.SenderDepartmentSiteName;
                            GEDepartments.CreatedDate = DateTime.Now;
                            GEDepartments.CreatedBy = "G2G TARASOL RPA";
                            await _dbContext.GEDepartments.AddAsync(GEDepartments);
                            await _dbContext.SaveChangesAsync();
                            //entityId =  _dbContext.GovernmentEntity.Where(x => x.Name_Ar == payload.SenderSiteName).Select(y => y.EntityId).FirstOrDefault();
                            communication1.Communication.GovtEntityId = entityId;
                            communication1.Communication.DepartmentId = GEDepartments.Id;


                        }

                        isSaved = await SaveRPACommunication(communication1.Communication, _dbContext);
                        isSaved = await SaveRPACommunicationTargetLink(communication1.CommunicationTargetLink, _dbContext);
                        // we have to bind value for Is primary and link target type will be communication
                        LinkTarget newLinkTarget = new LinkTarget();
                        newLinkTarget.TargetLinkId = communication1.CommunicationTargetLink.TargetLinkId;
                        newLinkTarget.LinkTargetTypeId = (int)LinkTargetTypeEnum.Communication;
                        communication1.LinkTarget.Add(newLinkTarget);
                        isSaved = await SaveRPALinkTarget(communication1.LinkTarget, communication1.CommunicationTargetLink.TargetLinkId, _dbContext);
                        if (isSaved == true)
                        {
                            CommunicationTarasolRpaPayload CommunicationTarasolPayload = new CommunicationTarasolRpaPayload
                            {
                                Id = Guid.NewGuid(),
                                CorrespondenceId = payload.CorrespondenceId,
                                Payload = JsonConvert.SerializeObject(payload),
                                isSucceeded = true,
                                CreatedBy = "G2G TARASOL RPA",
                                CreatedDate = DateTime.Now
                            };
                            await _dbContext.CommunicationTarasolRpaPayloads.AddAsync(CommunicationTarasolPayload);
                            await _dbContext.SaveChangesAsync();
                            await SaveCommunicationHistory(payload);
                            transaction.Commit();
                        }
                    }


                    catch (Exception)
                    {
                        transaction.Rollback();
                        communication1 = new SendCommunicationVM();
                        throw;
                    }
                }
            }
            return communication1;
        }
        public async Task SaveCommunicationHistory(CommunicationTarasolRPAPayload payload)
        {
            var governmentEntity = _dbContext.GovernmentEntity.Where(x => x.Name_Ar == payload.RecieverSiteName).FirstOrDefault();
            if(governmentEntity != null)
            {
                var operatingSectorId = (from dept in _dbContext.GEDepartments
                                         join sector in _dbContext.OperatingSectorType
                                         on dept.G2GBRSiteID equals sector.G2GBRSiteID
                                         where dept.Name_Ar == payload.RecieverDepartmentSiteName && dept.EntityId == governmentEntity.EntityId
                                         select sector.Id).FirstOrDefault();

                string StoreProc = $"exec pGetHOSBSectorId @sectorTypeId = '{operatingSectorId}'";
                var users = await _dbContext.Users.FromSqlRaw(StoreProc).ToListAsync();
                var headOfSectors = users.FirstOrDefault();

                var defaultReceivers = await (from epi in _dbContext.UserEmploymentInformation
                                                     where epi.SectorTypeId == operatingSectorId && epi.IsDefaultCorrespondenceReceiver
                                                     select epi).ToListAsync();

                var communicationRecepients = new List<Guid>();

                if (headOfSectors != null)
                {
                    var headOfSectorId = Guid.Parse(headOfSectors.Id);
                    communicationRecepients.Add(headOfSectorId);

                    var defaultReceiverIds = defaultReceivers.Select(receiver => Guid.Parse(receiver.UserId.ToString()));

                    defaultReceiverIds = defaultReceiverIds.Except(new List<Guid> { headOfSectorId });

                    communicationRecepients.AddRange(defaultReceiverIds);
                }
                else
                {
                    communicationRecepients.AddRange(defaultReceivers.Select(receiver => Guid.Parse(receiver.UserId.ToString())));
                }

                foreach (var recipientId in communicationRecepients)
                {
                    CommunicationHistory communicationHistory = new();
                    communicationHistory.SentBy = Guid.Empty;
                    communicationHistory.SentTo = recipientId;
                    communicationHistory.CreatedBy = "G2G TARASOL RPA";
                    communicationHistory.ActionId = (int)CommunicationHistoryEnum.RecieveFromTarasol;
                    communicationHistory.CreatedDate = DateTime.Now;
                    communicationHistory.ReferenceId = payload.Guid;
                    await _dbContext.CommunicationHistories.AddAsync(communicationHistory);

                    CommunicationRecipient recipient = new();
                    recipient.CommunicationId = payload.Guid;
                    recipient.RecipientId = recipientId;
                    recipient.CreatedDate = DateTime.Now;
                    recipient.CreatedBy = "G2G TARASOL RPA";
                    communication1.RecieverId = recipientId.ToString();
                    await _dbContext.CommunicationRecipients.AddAsync(recipient);
                }

                await _dbContext.SaveChangesAsync();
            }
        }



        /*public async Task SaveCommunicationHistory(CommunicationTarasolRPAPayload payload)
        {
            var operatingSectorId = (from dept in _dbContext.GEDepartments
                                     join sector in _dbContext.OperatingSectorType
                                     on dept.G2GBRSiteID equals sector.G2GBRSiteID
                                     where dept.Name_Ar == payload.RecieverDepartmentSiteName
                                     select sector.Id).FirstOrDefault();

            string StoreProc = $"exec pGetHOSBSectorId @sectorTypeId = '{operatingSectorId}'";
            var users = await _dbContext.Users.FromSqlRaw(StoreProc).ToListAsync();
            var headOfSectors = users.FirstOrDefault();

            var defaultCorrespondenceReceivers = from epi in _dbContext.UserEmploymentInformation
                        where epi.SectorTypeId == operatingSectorId && epi.IsDefaultCorrespondenceReceiver
                        select epi;

            var defaultReceivers = defaultCorrespondenceReceivers.ToList();

            CommunicationHistory communicationHistory = new();
            communicationHistory.SentBy = Guid.Empty;
            communicationHistory.SentTo = Guid.Parse(headOfSectors.Id);
            communicationHistory.CreatedBy = "G2G TARASOL RPA";
            communicationHistory.ActionId = (int)CommunicationHistoryEnum.RecieveFromTarasol;
            communicationHistory.CreatedDate = DateTime.Now;
            communicationHistory.ReferenceId = payload.Guid;
            await _dbContext.CommunicationHistories.AddAsync(communicationHistory);
            CommunicationRecipient recipient = new();
            recipient.CommunicationId = payload.Guid;
            recipient.RecipientId = Guid.Parse(headOfSectors.Id);
            recipient.CreatedDate = DateTime.Now;
            recipient.CreatedBy = "G2G TARASOL RPA";
            communication1.RecieverId = headOfSectors.Id;
            await _dbContext.CommunicationRecipients.AddAsync(recipient);
            await _dbContext.SaveChangesAsync();
            //return operatingSectorId;
        }*/


        public async Task AddFaultyCommunicationData(CommunicationTarasolRpaPayload payload)
        {
            try
            {
                await _dbContext.CommunicationTarasolRpaPayloads.AddAsync(payload);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<bool> SaveRPACommunicationTargetLink(CommunicationTargetLink communicationTargetLink, DatabaseContext _dbContext)
        {
            bool isSaved;
            try
            {
                if (communicationTargetLink.TargetLinkId == null)
                    communicationTargetLink.TargetLinkId = new Guid();
                communicationTargetLink.CreatedBy = "G2G TARASOL RPA";
                communicationTargetLink.CreatedDate = DateTime.Now;
                _dbContext.CommunicationTargetLinks.Add(communicationTargetLink);
                await _dbContext.SaveChangesAsync();

                isSaved = true;
            }
            catch
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }
        public async Task<bool> SaveRPALinkTarget(List<LinkTarget> linkTarget, Guid targetLinkId, DatabaseContext _dbContext)
        {
            bool isSaved;
            try
            {
                foreach (var target in linkTarget)
                {
                    target.ReferenceId = communication1.Communication.CommunicationId;
                    target.TargetLinkId = targetLinkId;
                    _dbContext.LinkTargets.Add(target);
                }
                await _dbContext.SaveChangesAsync();

                isSaved = true;
            }
            catch
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }

        public async Task<bool> SaveRPACommunication(Communication communication, DatabaseContext _dbContext)
        {
            bool isSaved;
            try
            {

                communication1.Communication.CorrespondenceTypeId = (int)CommunicationCorrespondenceTypeEnum.Inbox;
                communication1.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.G2GTarasolCorrespondence;
                // WE WILL GET THE LIST OF INBOX AND OUTBOX ON THE BASIS OF THIS CREATEDBY  IF CHANGE THEN MODIFY INBOX LIST PROCEDURE
                communication.CreatedBy = "G2G TARASOL RPA";
                var resultInboxNumber = await _cMSCOMSInboxOutboxPatternNumberRepository.GenerateNumberPattern(0, (int)CmsComsNumPatternTypeEnum.InboxNumber);
                communication.InboxNumber = resultInboxNumber.GenerateRequestNumber;
                communication.InboxNumberFormat = resultInboxNumber.FormatRequestNumber;
                communication.PatternSequenceResult = resultInboxNumber.PatternSequenceResult;
                communication.OutboxNumber = null;
                communication.OutboxDate = null;
                communication.ColorId = 1;
                communication.OutboxShortNum = 0;

                communication.CreatedDate = DateTime.Now;

                await _dbContext.Communications.AddAsync(communication);
                await _dbContext.SaveChangesAsync();


                //if (communication.PreCommunicationId != Guid.Empty)
                //{
                //    var oldCommunication = await _dbContext.Communications.Where(x => x.CommunicationId == communication.PreCommunicationId).FirstOrDefaultAsync();
                //    if (communication.CommunicationTypeId == (int)CommunicationTypeEnum.RequestForMoreInformationReminder)
                //        oldCommunication.IsReminderSent = true;
                //    else
                //        oldCommunication.IsReplied = true;
                //    _dbContext.Communications.Update(oldCommunication);
                //    await _dbContext.SaveChangesAsync();
                //}

                isSaved = true;

            }
            catch
            {
                isSaved = false;
                throw;
            }
            return true;
        }

        #region Get Correspondences
        public async Task<List<CommunicationTarasolMarkedCorrespondencesVM>> GetCorrespondences()
        {
            try
            {
                string StoredProc = $"exec pCommGetTarasolCorrespondences";
                return await _dbContext.CommunicationTarasolMarkedCorrespondencesVMs.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public async Task<SendCommunicationVM> SendTarassolCommunication(SendCommunicationVM communicationVM)
        {
            bool isSaved;
            using (_dbContext)
            {

                bool check = false;
                Communication oldCommunication = new();

                if (communicationVM.Communication.PreCommunicationId != Guid.Empty)
                {
                    oldCommunication = await _dbContext.Communications.FirstOrDefaultAsync(x => x.CommunicationId == communicationVM.Communication.PreCommunicationId);
                    check = true;
                    oldCommunication.IsReplied = true;
                }
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var resultIOutboxNumber = await _cMSCOMSInboxOutboxPatternNumberRepository.GenerateNumberPattern(0, (int)CmsComsNumPatternTypeEnum.OutboxNumber);

                        communicationVM.Communication.OutboxNumber = resultIOutboxNumber.GenerateRequestNumber;
                        communicationVM.Communication.OutBoxNumberFormat = resultIOutboxNumber.FormatRequestNumber;
                        communicationVM.Communication.PatternSequenceResult = resultIOutboxNumber.PatternSequenceResult;

                        communicationVM.Communication.OutboxDate = DateTime.Now;
                        await _dbContext.Communications.AddAsync(communicationVM.Communication);
                        await _dbContext.SaveChangesAsync();
                        if (check)
                        {
                            _dbContext.Communications.Update(oldCommunication);
                            await _dbContext.SaveChangesAsync();
                        }
                        communicationVM.CommunicationTargetLink.CommunicationId = communicationVM.Communication.CommunicationId;
                        isSaved = await _communicationRepository.SaveCommunicationTargetLink(communicationVM.CommunicationTargetLink, _dbContext);
                        isSaved = await _communicationRepository.SaveLinkTarget(communicationVM.LinkTarget, communicationVM.CommunicationTargetLink.TargetLinkId, _dbContext);
                        isSaved = await _communicationRepository.SaveCommResponse(communicationVM.CommunicationResponse, communicationVM.Communication.CommunicationId, _dbContext);
                        if (isSaved == true)
                            transaction.Commit();

                        return communicationVM;

                    }
                    catch (Exception)
                    {
                        isSaved = false;

                        transaction.Rollback();
                        throw;
                    }
                }

            }
        }

        //<History Author = 'IHassan' Date='2024-04-11' Version="1.0" Branch="master"> </History> 
        public async Task<GovernmentEntity> GetGovernmentEntitiyById(int Id)
        {
            try
            {
                return await _dbContext.GovernmentEntity.Where(x => x.EntityId == Id).FirstOrDefaultAsync();

            }
            catch
            {

                throw;

            }
        }
        //<History Author = 'IHassan' Date='2024-04-11' Version="1.0" Branch="master"> </History> 
        public async Task<GEDepartments> GetGEDepartmentById(int Id)
        {
            try
            {
                return await _dbContext.GeDepartments.Where(x => x.Id == Id).FirstOrDefaultAsync();

            }
            catch
            {

                throw;

            }
        }
        public async Task<TarasolRPAPayloadWithDocuments> GetRPAFaultyMessages()
        {
            try
            {
                TarasolRPAPayloadWithDocuments tarasolRPAPayloadWithDocuments = new TarasolRPAPayloadWithDocuments();
                var tarasolRPAPayloads = await _dbContext.CommunicationTarasolRpaPayloads.Where(x => x.isSucceeded == false).ToListAsync();
                foreach (var tarasolRPAPayload in tarasolRPAPayloads)
                {
                    if (tarasolRPAPayload.CommunicationPayload)
                    {
                        var CommunicationPayload = JsonConvert.DeserializeObject<CommunicationTarasolRPAPayloadOutput>(tarasolRPAPayload.Payload);
                        CommunicationPayload.RecieveFromDeadQueue = tarasolRPAPayload.CreatedDate.ToString();
                        tarasolRPAPayloadWithDocuments.CommunicationPayload.Add(CommunicationPayload);
                    }
                    else if (tarasolRPAPayload.CommunicationDocumentPayload)
                    {
                        var DocumentPayload = JsonConvert.DeserializeObject<CommunicationDocumentPayloadOutput>(tarasolRPAPayload.Payload);
                        DocumentPayload.RecieveFromDeadQueue = tarasolRPAPayload.CreatedDate.ToString();
                        tarasolRPAPayloadWithDocuments.DocumentPayload.Add(DocumentPayload);
                    }
                }
                return tarasolRPAPayloadWithDocuments;
            }
            catch
            {
                throw;
            }
        }
        public async Task<int> GetGovernmentEntitiyByReferenceAndSubmoduleId(Guid referenceId, int subModuleId)
        {

            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            int EntityId = 0;
            if(subModuleId == (int)SubModuleEnum.CaseFile)
            {
                EntityId = (int)await _DbContext.CaseFiles
            .Where(cf => cf.FileId == referenceId)
            .Join(_DbContext.CaseRequests,
                cf => cf.RequestId,
                cr => cr.RequestId,
                (cf, cr) => cr.GovtEntityId)
            .FirstOrDefaultAsync();
            }
            else if(subModuleId == (int)SubModuleEnum.ConsultationFile)
            {
                EntityId = (int)await _DbContext.ConsultationFiles
            .Where(cf => cf.FileId == referenceId)
            .Join(_DbContext.ConsultationRequests,
                cf => cf.RequestId,
                cr => cr.ConsultationRequestId,
                (cf, cr) => cr.GovtEntityId)
            .FirstOrDefaultAsync();
            }
            else if(subModuleId == (int)SubModuleEnum.RegisteredCase)
            {
                EntityId = (int)await _DbContext.CmsRegisteredCases
                .Where(rc => rc.CaseId == referenceId)
                .Join(_DbContext.CaseFiles,
                rc => rc.FileId,
                cf => cf.FileId,
                (rc, cf) => new { CaseFile = cf, RegisteredCase = rc })
                .SelectMany(joined => _DbContext.CaseRequests
                .Where(cr => cr.RequestId == joined.CaseFile.RequestId)
                .Select(cr => cr.GovtEntityId))
                .FirstOrDefaultAsync();
            }
            return EntityId;
        }
        public async Task<Communication> GetCommunicationByCommunicationId(Guid  CommunicationId)
        {

            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            return await _DbContext.Communications.FirstOrDefaultAsync(x => x.CommunicationId == CommunicationId);
        }


    }
}
