using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;
using static FATWA_DOMAIN.Enums.LegalLegislationEnum;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_DOMAIN.Enums.WorkflowEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using FATWA_DOMAIN.Models.WorkflowModels;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Enums;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using Microsoft.Extensions.DependencyInjection;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.ViewModel.MobileAppVMs;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace FATWA_INFRASTRUCTURE.Repository
{
    public class LegalLegislationRepository : ILegalLegislation
    {
        private List<LegalLegislationsVM> _LegalLegislationsVM;
        private List<LegalLegislationsDmsVM> _LegalLegislationsDmsVM;
        public LegalLegislationDecisionVM _legalLegislationDecisionVM;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        // View Model which we used to implement Db Schema
        #region Constructor
        public LegalLegislationRepository(DatabaseContext dbContext, WorkflowRepository workflowRepository, IServiceScopeFactory serviceScopeFactory, DmsDbContext dmsDbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _dmsDbContext = dmsDbContext;
            _LegalLegislationVMResult = new List<LegalLegislationVM>();
            SelectedArticlesDetails = new List<LegalArticle>();
            legalTemplateForNewTemplate = new List<LegalTemplate>();
            CountTemplateBindwithLegislation = 0;
            _workflowRepository = workflowRepository;
            _serviceScopeFactory = serviceScopeFactory;
            _Config = config;
        }
        #endregion

        #region Variables
        private readonly DatabaseContext _dbContext;
        private readonly DmsDbContext _dmsDbContext;
        private readonly IConfiguration _Config;
        private List<LegalLegislationVM> _LegalLegislationVMResult;
        private List<LegalArticle> SelectedArticlesDetails;
        protected List<LegalTemplate> legalTemplateForNewTemplate { get; set; }
        private int CountTemplateBindwithLegislation { get; set; }
        private readonly WorkflowRepository _workflowRepository;
        #endregion

        #region Get legislation detail by using id
        public async Task<LegalLegislation> GetLegalLegislationDetailByUsingId(Guid legislationId)
        {
            try
            {
                var task = await _dbContext.legalLegislations.Where(x => x.LegislationId == legislationId).FirstOrDefaultAsync();
                if (task != null)
                {
                    return task;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Get legislation type details
        public async Task<List<LegalLegislationType>> GetLegislationTypeDetails()
        {
            try
            {
                var task = await _dbContext.legalLegislationTypes.OrderByDescending(x => x.Id).Where(x => x.IsActive == true && x.IsDeleted == false).ToListAsync();
                if (task.Count() != 0)
                {
                    return task;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Get legislation status details
        public async Task<List<LegalLegislationStatus>> GetLegislationStatusDetails()
        {
            try
            {
                var task = await _dbContext.legalLegislationStatuss.OrderBy(x => x.Id).ToListAsync();
                if (task.Count() != 0)
                {
                    return task;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Get legislation flow status details
        public async Task<List<LegalLegislationFlowStatus>> GetLegislationFlowStatusDetails()
        {
            try
            {
                var task = await _dbContext.legalLegislationFlowStatuss.OrderBy(x => x.Id).ToListAsync();
                if (task.Count() != 0)
                {
                    return task;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Get legislation Tag details
        public async Task<List<LegalLegislationTag>> GetLegislationTagDetails()
        {
            try
            {
                var task = await _dbContext.legalLegislationTags.OrderByDescending(x => x.TagId).ToListAsync();
                if (task.Count() != 0)
                {
                    return task;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Get publication source name details
        public async Task<List<LegalPublicationSourceName>> GetPublicationSourceNameDetails()
        {
            try
            {
                var task = await _dbContext.legalPublicationSourceNames.OrderByDescending(x => x.PublicationNameId).Where(x => x.IsDeleted == false && x.IsActive == true).ToListAsync();
                if (task.Count() != 0)
                {
                    return task;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Get legal section parent list, new number & add
        public async Task<List<LegalSection>> GetLegalSectionParentList()
        {
            try
            {
                var task = await _dbContext.legalSections.OrderByDescending(x => x.SectionId).ToListAsync();
                if (task.Count() != 0)
                {
                    return task;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> GetLegalSectionNewNumber()
        {
            try
            {
                if (_dbContext.legalSections.Count() > 0)
                {
                    return await _dbContext.legalSections.Select(x => x.Section_Number).MaxAsync() + 1;
                }
                else
                {
                    return 1;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> AddLegalSection(LegalSection item)
        {
            try
            {
                await _dbContext.legalSections.AddAsync(item);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Get article status details
        public async Task<List<LegalArticleStatus>> GetLegalArticleStatusList()
        {
            try
            {
                var task = await _dbContext.legalArticleStatuss.OrderByDescending(x => x.Id).ToListAsync();
                if (task.Count() != 0)
                {
                    return task;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Advance search relation
        public async Task<List<LegalLegislationVM>> AdvanceSearchRelation(LegalLegislationVM item)
        {
            try
            {
                string StoredProc = $"exec pLLRelationAdvanceSearch @LegislationType = '{item.Legislation_Type}', @LegislationNumber = '{item.Legislation_Number}', @LegislationYear = '{item.Legislation_Year}'";
                if (_LegalLegislationVMResult.Count() == 0)
                {
                    _LegalLegislationVMResult = await _dbContext.legalLegislationVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                if (_LegalLegislationVMResult.Count() != 0)
                {
                    var result = await GetRelatedArtilceDetails(_LegalLegislationVMResult, _dbContext);
                    if (result.Count() != 0)
                    {
                        return result;
                    }
                }
                return new List<LegalLegislationVM>();
            }
            catch
            {
                throw;
            }
        }

        private async Task<List<LegalLegislationVM>> GetRelatedArtilceDetails(List<LegalLegislationVM> legalLegislationVMResult, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in legalLegislationVMResult)
                {
                    var resultArticles = await dbContext.legalArticles.Where(x => x.LegislationId == item.LegislationId).OrderBy(x => x.ArticleOrder).ToListAsync();
                    if (resultArticles.Count() != 0)
                    {
                        foreach (var itemArticle in resultArticles)
                        {
                            item.RelatedArticles.Add(itemArticle);
                        }
                    }
                }
                return legalLegislationVMResult;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Article Effects (Existing legislation cancel, Article cancel, modify & add)
        public async Task<LegalLegislation> ExistingLegislationStatusChange(LegalLegislationVM args)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.legalLegislations.Where(x => x.LegislationId == args.LegislationId).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Legislation_Status = (int)args.Legislation_Status;
                            task.Legislation_Flow_Status = (int)LegislationFlowStatusEnum.New;
                            task.CanceledBy = args.CanceledBy;
                            task.CanceledDate = args.CanceledDate;
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();
                            return task;
                        }
                        return new LegalLegislation();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return new LegalLegislation();
                    }
                }
            }
        }
        public async Task<LegalArticle> ExistingArticleStatusChange(LegalArticle args)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.legalArticles.Where(x => x.ArticleId == args.ArticleId).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.Article_Status = args.Article_Status;
                            task.End_Date = args.End_Date;
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            await ExistingLegislationStatusChangeToCancel(args, _dbContext);
                            await AddArticleEffectStatementInOldLegislation(args, _dbContext);
                            transaction.Commit();
                            return task;
                        }
                        return new LegalArticle();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return new LegalArticle();
                    }
                }
            }
        }

        private async Task AddArticleEffectStatementInOldLegislation(LegalArticle args, DatabaseContext dbContext)
        {
            try
            {
                LegalLegislationArticleEffectHistory Obj = new LegalLegislationArticleEffectHistory()
                {
                    Id = Guid.NewGuid(),
                    LegislationId = args.LegislationId,
                    ArticleId = args.ArticleId,
                    ArticleStatus = (int)args.Article_Status,
                    Note = args.ArticleEffectNoteHistory,
                    CreatedDate = DateTime.Now,
                };
                await dbContext.legalLegislationArticleEffectHistorys.AddAsync(Obj);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        private async Task ExistingLegislationStatusChangeToCancel(LegalArticle args, DatabaseContext dbContext)
        {
            try
            {
                var task = await _dbContext.legalLegislations.Where(x => x.LegislationId == args.LegislationId).FirstOrDefaultAsync();
                if (task != null)
                {
                    var result = await GetArticleListDetailsByUsingLegislationId(task.LegislationId, _dbContext);
                    if (result == true)
                    {
                        task.Legislation_Status = (int)LegislationStatus.Expired;

                        //task.Legislation_Flow_Status = (int)LegislationFlowStatusEnum.New;
                        task.CanceledBy = task.AddedBy;
                        task.CanceledDate = args.End_Date;
                        _dbContext.Entry(task).State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private async Task<bool> GetArticleListDetailsByUsingLegislationId(Guid legislationId, DatabaseContext dbContext)
        {
            try
            {
                int counter = 0;
                var resultArticles = await dbContext.legalArticles.Where(x => x.LegislationId == legislationId).ToListAsync();
                if (resultArticles.Count() != 0)
                {
                    foreach (var item in resultArticles)
                    {
                        if (item.Article_Status == (int)LegalArticleStatusEnum.Expired)
                        {
                            counter++;
                        }
                    }
                    if (counter == resultArticles.Count())
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<LegalArticle> AddExistingArticleNewChilFromEffectsGrid(LegalArticle args)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.legalArticles.Where(x => x.ArticleId == args.ArticleId && x.LegislationId == args.LegislationId).FirstOrDefaultAsync();
                        if (task == null)
                        {
                            await _dbContext.legalArticles.AddAsync(args);
                            await _dbContext.SaveChangesAsync();
                            await ChangeExistingArticleNextArticleIdField(args, _dbContext);
                            await AddArticleEffectStatementInOldLegislation(args, _dbContext);
                            transaction.Commit();
                            return args;
                        }
                        return new LegalArticle();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return new LegalArticle();
                    }
                }
            }
        }

        private async Task ChangeExistingArticleNextArticleIdField(LegalArticle args, DatabaseContext dbContext)
        {
            try
            {
                var res = await dbContext.legalArticles.Where(x => x.ArticleId == args.ExistingArticleId).FirstOrDefaultAsync();
                if (res != null)
                {
                    res.NextArticleId = args.ArticleId;
                    dbContext.Entry(res).State = EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<LegalArticle> ModifiedExistingArticleNewChilFromEffectsGrid(LegalArticle args)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.legalArticles.Where(x => x.ArticleId == args.ArticleId && x.LegislationId == args.LegislationId).FirstOrDefaultAsync();
                        if (task == null)
                        {
                            await _dbContext.legalArticles.AddAsync(args);
                            await _dbContext.SaveChangesAsync();
                            await ExistingArticleStatusModified(args, _dbContext);
                            await ParentLegislationStatusModified(args, _dbContext);
                            await AddArticleEffectStatementInOldLegislation(args, _dbContext);
                            //await ChangeExistingArticleNextArticleIdField(args, _dbContext);
                            transaction.Commit();
                            return args;
                        }
                        return new LegalArticle();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return new LegalArticle();
                    }
                }
            }
        }

        private async Task ParentLegislationStatusModified(LegalArticle args, DatabaseContext dbContext)
        {
            try
            {
                var task = await dbContext.legalLegislations.Where(x => x.LegislationId == args.LegislationId).FirstOrDefaultAsync();
                if (task != null)
                {
                    //task.Legislation_Flow_Status = (int)LegislationFlowStatusEnum.New;
                    task.Legislation_Status = (int)LegislationStatus.Modified;
                    _dbContext.Entry(task).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task ExistingArticleStatusModified(LegalArticle args, DatabaseContext dbContext)
        {
            try
            {
                var task = await dbContext.legalArticles.Where(x => x.ArticleId == args.ExistingArticleId).FirstOrDefaultAsync();
                if (task != null)
                {
                    if (args.Article_Status == (int)LegalArticleStatusEnum.Expired || args.Article_Status == (int)LegalArticleStatusEnum.Modified)
                    {
                        task.Article_Status = (int)LegalArticleStatusEnum.Expired;
                    }
                    task.NextArticleId = args.ArticleId;
                    _dbContext.Entry(task).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get article detail by using legislationId
        public async Task<LegalArticle> GetLatestArticleDetailByUsingLegislationId(Guid legislationId)
        {
            try
            {
                var task = await _dbContext.legalArticles.Where(x => x.LegislationId == legislationId && x.NextArticleId == Guid.Empty).FirstOrDefaultAsync();
                if (task != null)
                {
                    return task;
                }
                else
                {
                    return new LegalArticle();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Save legal legislation
        public async Task<bool> SaveLegalLegislation(LegalLegislation args)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        await _dbContext.AddAsync(args);
                        await _dbContext.SaveChangesAsync();
                        //if (args.SelectedSourceDocumentForDelete.Count() != 0)
                        //    {
                        //       await SelectedSourceDocumentDelete(args.SelectedSourceDocumentForDelete, _dbContext);
                        //  }
                        //   bool response = await InsertAttachmentFromTempToUploadDocument(args, _dbContext);
                        //   if (response)
                        //   {

                        //      // await DeleteFromTempAttachmentTable(args, _dbContext);
                        //   }
                        if (args.LegalTemplates != null && args.LegalTemplates.Legislation_Type != 0)
                        {
                            await AddNewLegalTemplate(args.LegalTemplates, _dbContext);
                        }
                        if (args.LegalTemplates.SelectedCheckBoxValues.Count() != 0)
                        {
                            await AddLegalTemplateSettingValues(args.LegislationId, args.LegalTemplates.TemplateId, args.LegalTemplates.SelectedCheckBoxValues, _dbContext);
                        }
                        if (args.LegalLegislationTags.Count() != 0)
                        {
                            await AddMangtoManyLegislationTags(args.LegislationId, args.LegalLegislationTags, _dbContext);
                        }
                        if (args.LegalPublicationSources.Count() != 0)
                        {
                            await AddLegalLegislationPublication(args.LegalPublicationSources, _dbContext);
                        }
                        if (args.LegalLegislationSignatures.Count() != 0)
                        {
                            await AddLegalLegislationSignature(args.LegalLegislationSignatures, _dbContext);
                        }
                        if (args.LegalLegislationReferences.Count() != 0)
                        {
                            await AddLegalLegislationReference(args.LegalLegislationReferences, _dbContext);
                        }
                        if (args.LegalSections.Count() != 0)
                        {
                            await AddLegalLegislationSection(args.LegalSections, _dbContext);
                        }
                        if (args.LegalArticles.Count() != 0)
                        {
                            await AddLegalLegislationArticle(args.LegalArticles, _dbContext);
                        }
                        if (args.LegalClauses.Count() != 0)
                        {
                            await AddLegalLegislationClause(args.LegalClauses, _dbContext);
                        }
                        if (args.LegalExplanatoryNotes.Count() != 0)
                        {
                            await AddLegalLegislationExplanatoryNote(args.LegalExplanatoryNotes, _dbContext);
                        }
                        if (args.legalNotes.Count() != 0)
                        {
                            await AddLegalLegislationNote(args.legalNotes, _dbContext);
                        }


                        //if (args.NewLegislationAddedId.Count() != 0)
                        //{
                        //    await NewUpdatedLegislationTemplateAddInTemplateSettingTable(args, args.NewLegislationAddedId, _dbContext);
                        //}
                        //if (args.OldTemplateDetails.Count() != 0)
                        //{
                        //    await NewUpdatedLegislationTemplateAddInTemplateTable(args, args.OldTemplateDetails, _dbContext);
                        //}
                        await _workflowRepository.LinkEntityWithActiveWorkflow(args, _dbContext, (int)WorkflowSubModuleEnum.LegalLegislations);
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        private async Task NewUpdatedLegislationTemplateAddInTemplateTable(LegalLegislation args, List<Guid> oldTemplateDetails, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in oldTemplateDetails)
                {
                    var result = await dbContext.legalTemplates.Where(x => x.TemplateId == item).FirstOrDefaultAsync();
                    if (result != null)
                    {
                        var resultTemplateCheckboxes = await dbContext.legalLegislationLegalTemplates.Where(x => x.TemplateId == result.TemplateId).ToListAsync();
                        if (resultTemplateCheckboxes.Count() == 0)
                        {
                            dbContext.legalTemplates.Remove(result);
                            await dbContext.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task NewUpdatedLegislationTemplateAddInTemplateSettingTable(LegalLegislation args, List<Guid> newLegislationAddedId, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in newLegislationAddedId)
                {
                    var result = await dbContext.legalLegislationLegalTemplates.Where(x => x.LegislationId == item).ToListAsync();
                    if (result.Count() != 0)
                    {
                        foreach (var itemModel in result)
                        {
                            dbContext.legalLegislationLegalTemplates.Remove(itemModel);
                        }
                        await dbContext.SaveChangesAsync();
                    }
                }
                foreach (var itemAdd in newLegislationAddedId)
                {
                    foreach (var itemTemplateSetting in args.LegalTemplates.SelectedCheckBoxValues)
                    {
                        LegalLegislationLegalTemplate? Obj = new LegalLegislationLegalTemplate();
                        Obj.LegislationId = itemAdd;
                        Obj.TemplateId = args.LegalTemplates.TemplateId;
                        Obj.TemplateSettingId = itemTemplateSetting;
                        await dbContext.AddAsync(Obj);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task AddLegalTemplateSettingValues(Guid legislationId, Guid templateId, List<int> selectedCheckBoxValues, DatabaseContext dbContext)
        {
            try
            {
                var res = await dbContext.legalLegislationLegalTemplates.Where(x => x.LegislationId == legislationId && x.TemplateId == templateId).ToListAsync();
                if (res.Count() == 0)
                {
                    foreach (var item in selectedCheckBoxValues)
                    {
                        LegalLegislationLegalTemplate? Obj = new LegalLegislationLegalTemplate();
                        Obj.LegislationId = legislationId;
                        Obj.TemplateId = templateId;
                        Obj.TemplateSettingId = item;
                        await dbContext.AddAsync(Obj);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task AddNewLegalTemplate(LegalTemplate? legalTemplates, DatabaseContext dbContext)
        {
            try
            {
                var res = await dbContext.legalTemplates.Where(x => x.TemplateId == legalTemplates.TemplateId).FirstOrDefaultAsync();
                if (res == null)
                {
                    await dbContext.AddAsync(legalTemplates);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        private async Task AddMangtoManyLegislationTags(Guid legislationId, List<int> legalLegislationTags, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in legalLegislationTags)
                {
                    LegalLegislationLegalTag tags = new LegalLegislationLegalTag();
                    tags.LegislationId = legislationId;
                    tags.TagId = item;
                    await dbContext.legalLegislationLegalTags.AddAsync(tags);
                }
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task AddLegalLegislationNote(List<LegalNote> legalNotes, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in legalNotes)
                {
                    await dbContext.legalNotes.AddAsync(item);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task AddLegalLegislationExplanatoryNote(List<LegalExplanatoryNote> legalExplanatoryNotes, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in legalExplanatoryNotes)
                {
                    await dbContext.legalExplanatoryNotes.AddAsync(item);
                    await dbContext.SaveChangesAsync();
                }
                //if (legalExplanatoryNotes.Count() != 0)
                //{
                //    var resultFile = await AddExplanatoryNoteAttachmentFromTempToUploadDocumentTable(legalExplanatoryNotes, dbContext);
                //    if (resultFile)
                //    {
                //        await DeleteExplanatoryNoteFromTempAttachmentTable(legalExplanatoryNotes, dbContext);
                //    }
                //}
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task AddLegalLegislationClause(List<LegalClause> legalClauses, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in legalClauses)
                {
                    if (item.Start_Date != null)
                    {
                        await dbContext.legalClauses.AddAsync(item);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task AddLegalLegislationArticle(List<LegalArticle> legalArticles, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in legalArticles)
                {
                    if (item.Start_Date != null)
                    {
                        await dbContext.legalArticles.AddAsync(item);
                        await dbContext.SaveChangesAsync();
                        if (item.ExistingArticleId != Guid.Empty)
                        {
                            await ManageNextArticleIdFieldInArticleTable(item, dbContext);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task ManageNextArticleIdFieldInArticleTable(LegalArticle item, DatabaseContext dbContext)
        {
            try
            {
                var res = await dbContext.legalArticles.Where(x => x.ArticleId == item.ExistingArticleId).FirstOrDefaultAsync();
                if (res != null)
                {
                    res.NextArticleId = item.ArticleId;
                    dbContext.Entry(res).State = EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task AddLegalLegislationSection(ICollection<LegalSection> legalSections, DatabaseContext dbContext)
        {
            foreach (var item in legalSections)
            {
                var resSection = await dbContext.legalSections.Where(x => x.SectionId == item.SectionId).FirstOrDefaultAsync();
                if (resSection == null)
                {
                    await dbContext.legalSections.AddAsync(item);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        private async Task AddLegalLegislationReference(List<LegalLegislationReference> legalLegislationReferences, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in legalLegislationReferences)
                {
                    await dbContext.legalLegislationReferences.AddAsync(item);
                }
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task AddLegalLegislationSignature(List<LegalLegislationSignature> legalLegislationSignatures, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in legalLegislationSignatures)
                {
                    await dbContext.legalLegislationSignatures.AddAsync(item);
                }
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task AddLegalLegislationPublication(List<LegalPublicationSource> legalPublicationSources, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in legalPublicationSources)
                {
                    await dbContext.legalPublicationSources.AddAsync(item);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Legislations Events
        //<History Author = 'Nadia Gull' Date='2022-10-18' Version="1.0" Branch="master">Get Legal Legislation List with AdvanceSearch</History>
        public async Task<List<LegalLegislationsVM>> GetLegalLegislations(AdvanceSearchLegalLegislationsVM advanceSearchVM)
        {
            try
            {
                if (_LegalLegislationsVM == null)
                {
                    string start_From = advanceSearchVM.Start_From != null ? Convert.ToDateTime(advanceSearchVM.Start_From).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string end_To = advanceSearchVM.End_To != null ? Convert.ToDateTime(advanceSearchVM.End_To).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string StoredProc = $"exec pLegislationListFiltered @legislation_Number ='{advanceSearchVM.Legislation_Number}' , @introduction='{advanceSearchVM.Introduction}' ,  @legislation_Title='{advanceSearchVM.LegislationTitle}' , @legislation_Status='{advanceSearchVM.Legislation_Status}' , @start_From='{start_From}' , @end_To='{end_To}',@lookupstableId='{(int)LookupsTablesEnum.LEGAL_LEGISLATION_TYPE}',@legislation_FlowStatus='{advanceSearchVM.Legislation_FlowStatus}', @legislation_Type = '{advanceSearchVM.Legislation_Type}'" +
                                        $",@PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}'";
                    _LegalLegislationsVM = await _dbContext.LegalLegislationsVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _LegalLegislationsVM;
        }
        public async Task<List<LegalLegislationsDmsVM>> GetLegalLegislationsDms()
        {
            try
            {
                if (_LegalLegislationsDmsVM == null)
                {
                    string StoredProc = $"exec pLegislationListFilteredDms @lookupstableId='{(int)LookupsTablesEnum.LEGAL_LEGISLATION_TYPE}'";
                    _LegalLegislationsDmsVM = await _dbContext.LegalLegislationsDmsVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _LegalLegislationsDmsVM;
        }
        #endregion

        #region GetLegislationForPublish
        public async Task<List<LegalLegislationsVM>> GetLegislationForPublish(AdvanceSearchLegalLegislationsVM advanceSearchVM)
        {
            try
            {
                if (_LegalLegislationsVM == null)
                {
                    string StoredProc = $"exec pLegislationListForPublish @userId ='{advanceSearchVM.UserId}',@lookupstableId='{(int)LookupsTablesEnum.LEGAL_LEGISLATION_TYPE}'" +
                                        $",@PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}'";
                    _LegalLegislationsVM = await _dbContext.LegalLegislationsVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _LegalLegislationsVM;
        }

        #endregion

        #region Get Legislations Decision
        public async Task<LegalLegislationDecisionVM> GetLegalLegislationsDecision(Guid LegislationId)
        {
            try
            {
                if (_LegalLegislationsVM == null)
                {
                    string StoredProc = $"exec pLegalLegislationDecision @LegislationId ='{LegislationId}'";
                    var res = await _dbContext.LegalLegislationDecisionVMs.FromSqlRaw(StoredProc).ToListAsync();


                    if (res != null)
                        _legalLegislationDecisionVM = res.FirstOrDefault();
                }
                return _legalLegislationDecisionVM;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateLegalLegislationDecision(LegalLegislationDecisionVM legalLegislationDecision)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            if (legalLegislationDecision.FlowStatusId > (int)LegislationFlowStatusEnum.Rejected && legalLegislationDecision.FlowStatusId < (int)LegislationFlowStatusEnum.Published)
                            {
                                legalLegislationDecision.FlowStatusId = (int)LegislationFlowStatusEnum.NeedModification;
                            }
                            if (legalLegislationDecision.FlowStatusId == (int)LegislationFlowStatusEnum.Unpublished)
                            {
                                legalLegislationDecision.FlowStatusId = (int)LegislationFlowStatusEnum.Unpublished;
                            }
                            LegalLegislation? legalLegislation = await _dbContext.legalLegislations.FindAsync(legalLegislationDecision.LegislationId);
                            if (legalLegislation != null)
                            {
                                legalLegislation.Legislation_Flow_Status = (int)legalLegislationDecision.FlowStatusId;

                                _dbContext.Entry(legalLegislation).State = EntityState.Modified;
                                await _dbContext.SaveChangesAsync();
                                await InsertCommentsByDocument(legalLegislationDecision, _dbContext);
                            }
                            //For Notification
                            legalLegislationDecision.NotificationParameter.Type = _dbContext.legalLegislationTypes.Where(x => x.Id == legalLegislation.Legislation_Type).Select(x => x.Name_En + "/" + x.Name_Ar).FirstOrDefault();
                            legalLegislationDecision.NotificationParameter.Entity = "Legal Legislation";
                            legalLegislationDecision.NotificationParameter.LegislationNumber = legalLegislation.Legislation_Number;
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
            catch
            {
                _dbContext.Entry(_dbContext).State = EntityState.Unchanged;
                throw;
            }

        }

        private async Task InsertCommentsByDocument(LegalLegislationDecisionVM legalLegislation, DatabaseContext _dbContext)
        {
            try
            {
                LdsDocumentComment commentobj = new LdsDocumentComment();
                if (legalLegislation.FlowStatusId != (int)LegislationFlowStatusEnum.Published)
                {
                    commentobj.CommentId = Guid.NewGuid();
                    commentobj.DocumentId = legalLegislation.LegislationId;
                    commentobj.Comment = legalLegislation.Legislation_Comment;
                    commentobj.Status = legalLegislation.FlowStatusId.ToString();
                    commentobj.CreatedDate = DateTime.Now;
                    commentobj.Createdby = legalLegislation.AddedBy;
                    commentobj.IsDeleted = false;
                    await _dbContext.LdsDocumentComment.AddAsync(commentobj);
                    await _dbContext.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Variables declared for legislation detail view
        private List<LegislationVM> _legislationVMs;
        public List<LegalLegislationDetailVM> _LegalLegislationDetailVMs;
        private List<LegalPublicationSourceVM> _legislationPublicationSourceVMs;
        private List<LegalArticalSectionVM> _legalArticalSectionVMs;
        private List<LegalClausesSectionVM> _legalClausesSectionVMs;
        private List<LegalLegislation> _legalLegislations;
        private List<LegalArticle> _legalArticles = new List<LegalArticle>();
        private List<LegalArticleChild> _legalSectionChild = new List<LegalArticleChild>();
        private List<LegalLegislationSignature> _legalLegislationSignatures = new List<LegalLegislationSignature>();
        private List<LegalSection> _legalSection = new List<LegalSection>();
        private List<LegalSectionArticalVM> _legalSectionArtical = new List<LegalSectionArticalVM>();
        private List<LegalLegislationReference> _legalLegislationReferences = new List<LegalLegislationReference>();
        private List<LegalPublicationSource> _legalPublicationSource = new List<LegalPublicationSource>();
        private List<LegalExplanatoryNote> _legalExplanatoryNote = new List<LegalExplanatoryNote>();
        private List<LegalNote> _legalNote = new List<LegalNote>();
        private List<LegalArticle> _legalArticlesUnderSection = new List<LegalArticle>();
        private MobileAppLegalLegislationDetailVM mobileAppLegalLegislationDetailVM;
        #endregion

        #region Legislation Detail view 
        public async Task<List<LegislationVM>> GetLegalLegislationPriviewDetailById(Guid LegislationId)
        {
            try
            {
                if (_legislationVMs == null)
                {
                    string StoredProc = $"exec plegislationSelbyPreview @LegislationId = '{LegislationId}'";
                    _legislationVMs = await _dbContext.LegislationVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _legislationVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<LegalLegislationDetailVM> GetLegalLegislationDetailById(Guid LegislationId)
        {
            try
            {
                if (_LegalLegislationDetailVMs == null)
                {
                    string StoredProc = $"exec plegislationDetailSelbyId @LegislationId = '{LegislationId}',@lookupstableId='{(int)LookupsTablesEnum.LEGAL_LEGISLATION_TYPE}'";
                    _LegalLegislationDetailVMs = await _dbContext.LegalLegislationDetailVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                if (_LegalLegislationDetailVMs.Count() != 0)
                {
                    return _LegalLegislationDetailVMs.Where(x => x.LegislationId == LegislationId).FirstOrDefault();
                }
                else
                {
                    return new LegalLegislationDetailVM();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LegalPublicationSourceVM> GetLegalPublicationSourceDetailByLegislationId(Guid LegislationId)
        {
            try
            {
                if (_legislationPublicationSourceVMs == null)
                {
                    string StoredProc = $"exec plegPublicationSourceSelbySourceId @LegislationId = '{LegislationId}',@LookupsTableId='{(int)LookupsTablesEnum.LEGAL_PUBLICATION_SOURCE_NAME}'";
                    _legislationPublicationSourceVMs = await _dbContext.LegalPublicationSourceVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                if (_legislationPublicationSourceVMs.Count() > 0)
                {
                    return _legislationPublicationSourceVMs.FirstOrDefault();
                }
                return new LegalPublicationSourceVM();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<LegalArticalSectionVM>> GetLegalArticalSectionByLegislationId(Guid LegislationId)
        {
            try
            {
                if (_legalArticalSectionVMs == null)
                {
                    string StoredProc = $"exec plegArticalSectionsSelbySourceId @LegislationId = '{LegislationId}'";
                    _legalArticalSectionVMs = await _dbContext.LegalArticalSectionVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _legalArticalSectionVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<LegalClausesSectionVM>> GetLegalClausesSectionByLegislationId(Guid LegislationId)
        {
            try
            {
                if (_legalClausesSectionVMs == null)
                {
                    string StoredProc = $"exec plegClauseSectionsSelbySourceId @LegislationId = '{LegislationId}'";
                    _legalClausesSectionVMs = await _dbContext.LegalClausesSectionVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _legalClausesSectionVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<LegalLegislationSignature>> GetLegislationSignaturesbyLegislationId(Guid LegislationId)
        {
            try
            {
                List<LegalLegislationSignature>? LegislationIdResults = await _dbContext.legalLegislationSignatures.Where(r => r.LegislationId == LegislationId).ToListAsync();
                return LegislationIdResults;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<LegalExplanatoryNote> GetLegislationExplanatoryNoteLegislationId(Guid LegislationId)
        {
            try
            {
                List<LegalExplanatoryNote>? LegislationIdResults = await _dbContext.legalExplanatoryNotes.Where(r => r.LegislationId == LegislationId).ToListAsync();
                return LegislationIdResults.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<LegalNote> GetLegislationNoteLegislationId(Guid LegislationId)
        {
            try
            {
                List<LegalNote>? LegislationIdResults = await _dbContext.legalNotes.Where(r => r.ParentId == LegislationId).ToListAsync();
                return LegislationIdResults.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<LegalLegislationReference>> GetLegislationReferencebyLegislationId(Guid LegislationId)
        {
            try
            {
                List<LegalLegislationReference>? ArticalIdResults = await _dbContext.legalLegislationReferences.Where(r => r.Reference_Parent_Id == LegislationId).ToListAsync();
                return ArticalIdResults;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Get Legislation with all dependents 
        public async Task<LegalLegislation> GetLegalLegislationDetailPreviewById(Guid legislationId)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        LegalLegislation? legallegislationDetails = await _dbContext.legalLegislations.FindAsync(legislationId);

                        if (legallegislationDetails != null)
                        {
                            var Legallegislationtype = await GetLegislationtypes(legallegislationDetails.Legislation_Type, _dbContext);
                            if (Legallegislationtype.Count() != 0)
                            {
                                legallegislationDetails.LegalLegislationTypes = Legallegislationtype;
                            }
                            var result = await GetLegislationArticals(legallegislationDetails.LegislationId, _dbContext);
                            if (result.Count() != 0)
                            {
                                legallegislationDetails.LegalArticles = result;
                            }

                            var Clouseresult = await GetLegislationClause(legallegislationDetails.LegislationId, _dbContext);
                            if (Clouseresult.Count() != 0)
                            {
                                legallegislationDetails.LegalClauses = Clouseresult;
                            }


                            var Sectionresult = await GetLegislationSection(legallegislationDetails.LegislationId, _dbContext);
                            if (Sectionresult.Count() != 0)
                            {
                                legallegislationDetails.LegalSections = Sectionresult;
                            }


                            await GetManytoManyLegislationSignatures(legallegislationDetails.LegislationId, _dbContext);
                            legallegislationDetails.LegalLegislationSignatures = _legalLegislationSignatures;
                            await GetManytoManyLegislationReference(legallegislationDetails.LegislationId, _dbContext);
                            legallegislationDetails.LegalLegislationReferences = _legalLegislationReferences;


                            await GetLegalExplanatoryNotes(legallegislationDetails.LegislationId, _dbContext);
                            legallegislationDetails.LegalExplanatoryNotes = _legalExplanatoryNote;
                            await GetLegalNotes(legallegislationDetails.LegislationId, _dbContext);
                            legallegislationDetails.legalNotes = _legalNote;
                            await GetLegalPublicationSources(legallegislationDetails.LegislationId, _dbContext);
                            legallegislationDetails.LegalPublicationSources = _legalPublicationSource;
                            var resultArticleHistory = await GetArticleEffectHistoryDetails(legallegislationDetails.LegislationId, _dbContext);
                            legallegislationDetails.LegalLegislationArticleEffectHistorys = resultArticleHistory;
                            transaction.Commit();
                            return legallegislationDetails;
                        }
                        else
                        {
                            throw new ArgumentNullException();
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return null;
                    }
                }
            }
        }
        private async Task<List<LegalLegislationArticleEffectHistory>> GetArticleEffectHistoryDetails(Guid legislationId, DatabaseContext dbContext)
        {
            try
            {
                var result = await dbContext.legalLegislationArticleEffectHistorys.OrderByDescending(x => x.CreatedDate).Where(x => x.LegislationId == legislationId).ToListAsync();
                if (result.Count != 0)
                {
                    return result;
                }
                return new List<LegalLegislationArticleEffectHistory>();
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
        LegalSectionArticalVM legalSection = new LegalSectionArticalVM();

        public async Task<LegalSectionArticalVM> GetLegalLegislationStatusDetailById(Guid legislationId)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        LegalSectionArticalVM? legallegislationDetails = await _dbContext.LegalSectionArticalVMs.FindAsync(legislationId);
                        if (legallegislationDetails != null)
                        {

                            await GetLegislationSection(legallegislationDetails.LegislationId, _dbContext);
                            //   legalSection = _legalSection;
                            //      legalSection = _legalArticlesUnderSection;

                            //await GetManytoManyLegislationSectionChilds(legallegislationDetails.LegislationId, _dbContext);
                            //legallegislationDetails.LegalSectionChild = _legalSectionChild;

                            transaction.Commit();
                            return legalSection;
                        }
                        else
                        {
                            throw new ArgumentNullException();
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return null;
                    }
                }
            }
        }


        private async Task<List<LegalArticle>> GetLegislationArticals(Guid LegislationId, DatabaseContext dbContext)
        {
            try
            {
                List<LegalArticle>? SectionIdResults = await dbContext.legalArticles.Where(r => r.LegislationId == LegislationId || r.SectionId == Guid.Empty).ToListAsync();
                if (SectionIdResults.Count() > 0)
                {
                    return SectionIdResults;
                }
                return new List<LegalArticle>();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<List<LegalLegislationType>> GetLegislationtypes(int LegislationtypeId, DatabaseContext dbContext)
        {
            try
            {
                List<LegalLegislationType>? SectionIdResults = await dbContext.legalLegislationTypes.Where(r => r.Id == LegislationtypeId).ToListAsync();
                if (SectionIdResults.Count() > 0)
                {
                    return SectionIdResults;
                }
                return new List<LegalLegislationType>();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<List<LegalClause>> GetLegislationClause(Guid LegislationId, DatabaseContext dbContext)
        {
            try
            {
                List<LegalClause>? SectionIdResults = await dbContext.legalClauses.Where(r => r.LegislationId == LegislationId || r.SectionId == Guid.Empty).ToListAsync();
                if (SectionIdResults.Count() > 0)
                {
                    return SectionIdResults;
                }
                return new List<LegalClause>();
            }
            catch (Exception)
            {

                throw;
            }
        }


        private async Task<List<LegalSection>> GetLegislationSection(Guid LegislationId, DatabaseContext dbContext)
        {
            try
            {
                List<LegalSection>? legalSectionsResutlt = await dbContext.legalSections.Where(r => r.LegislationId == LegislationId).ToListAsync();
                if (legalSectionsResutlt.Count() > 0)
                {
                    foreach (var item in legalSectionsResutlt)
                    {
                        var SectionArticalresults = await dbContext.legalArticles.Where(r => r.SectionId == item.SectionId).ToListAsync();
                        var SectionClouseresults = await dbContext.legalClauses.Where(r => r.SectionId == item.SectionId).ToListAsync();
                        if (SectionArticalresults.Count() != 0)
                        {
                            item.LegalArticlesUnderSection = SectionArticalresults;
                            item.LegalClauseUnderSection = SectionClouseresults;
                            //await GetLegislationSectionArticals(results.SectionId, dbContext);
                        }
                    }
                    return legalSectionsResutlt;
                }
                return new List<LegalSection>();
            }
            catch (Exception)
            {

                throw;
            }
        }


        private async Task GetLegalPublicationSources(Guid LegislationId, DatabaseContext dbContext)
        {
            try
            {
                List<LegalPublicationSource>? SectionIdResults = await dbContext.legalPublicationSources.Where(r => r.LegislationId == LegislationId).ToListAsync();
                if (SectionIdResults.Count() > 0)
                {
                    foreach (var item in SectionIdResults)
                    {
                        LegalPublicationSource? results = dbContext.legalPublicationSources.Where(r => r.SourceId == item.SourceId).FirstOrDefault();

                        if (results != null)
                        {
                            _legalPublicationSource.Add(new LegalPublicationSource
                            {
                                SourceId = results.SourceId,
                                LegislationId = results.LegislationId,
                                Issue_Number = results.Issue_Number,
                                Page_Start = results.Page_Start,
                                Page_End = results.Page_End,
                                PublicationDate = results.PublicationDate,
                                PublicationDate_Hijri = results.PublicationDate_Hijri,
                                PublicationNameId = results.PublicationNameId,
                            });

                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        private async Task GetLegalExplanatoryNotes(Guid LegislationId, DatabaseContext dbContext)
        {
            try
            {
                List<LegalExplanatoryNote>? legalSectionsResutlt = await dbContext.legalExplanatoryNotes.Where(r => r.LegislationId == LegislationId).ToListAsync();
                if (legalSectionsResutlt.Count() > 0)
                {
                    foreach (var item in legalSectionsResutlt)
                    {
                        _legalExplanatoryNote.Add(new LegalExplanatoryNote
                        {
                            ExplanatoryNoteId = item.ExplanatoryNoteId,
                            LegislationId = item.LegislationId,
                            ExplanatoryNote_Body = item.ExplanatoryNote_Body,
                        });
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task GetLegalNotes(Guid LegislationId, DatabaseContext dbContext)
        {
            try
            {
                List<LegalNote>? legalSectionsResutlt = await dbContext.legalNotes.Where(r => r.ParentId == LegislationId).ToListAsync();
                if (legalSectionsResutlt.Count() > 0)
                {
                    foreach (var item in legalSectionsResutlt)
                    {
                        _legalNote.Add(new LegalNote
                        {
                            NoteId = item.NoteId,
                            ParentId = item.ParentId,
                            Note_Text = item.Note_Text,
                        });

                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task GetLegislationSectionArticals(Guid SectionId, DatabaseContext dbContext)
        {
            try
            {
                List<LegalArticle>? SectionIdResults = await dbContext.legalArticles.Where(r => r.SectionId == SectionId).ToListAsync();

                foreach (var item in SectionIdResults)
                {
                    LegalArticle? results = dbContext.legalArticles.Where(r => r.ArticleId == item.ArticleId).FirstOrDefault();

                    if (results != null)
                    {
                        _legalArticles.Add(new LegalArticle
                        {
                            ArticleId = results.ArticleId,
                            SectionId = item.SectionId,
                            Article_Name = results.Article_Name,
                            Article_Title = results.Article_Title,
                            Start_Date = results.Start_Date,
                            Article_Status = results.Article_Status,
                            End_Date = results.End_Date,
                            Article_Text = results.Article_Text,
                            NextArticleId = results.NextArticleId,
                        });

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Legislations Events

        private async Task GetManytoManyLegislationSectionChild(Guid sectionId, DatabaseContext dbContext)
        {
            try
            {
                List<LegalArticleChild>? ArticalIdResults = await dbContext.legalArticleChilds.Where(r => r.ParentId == sectionId).ToListAsync();
                if (ArticalIdResults.Count() > 0)
                {
                    foreach (var item in ArticalIdResults)
                    {

                        LegalSection? results = dbContext.legalSections.Where(r => r.SectionId == item.ParentId).FirstOrDefault();

                        if (results != null)
                        {
                            _legalSection.Add(new LegalSection
                            {
                                SectionId = results.SectionId,
                                Section_Number = results.Section_Number,
                                SectionTitle = results.SectionTitle,

                            });
                        }
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task GetManytoManyLegislationSignatures(Guid LegislationId, DatabaseContext dbContext)
        {
            try
            {
                List<LegalLegislationSignature>? LegislationIdResults = await dbContext.legalLegislationSignatures.Where(r => r.LegislationId == LegislationId).ToListAsync();
                if (LegislationIdResults.Count() > 0)
                {
                    foreach (var item in LegislationIdResults)
                    {
                        LegalLegislationSignature? results = dbContext.legalLegislationSignatures.Where(r => r.SignatureId == item.SignatureId).FirstOrDefault();

                        if (results != null)
                        {
                            _legalLegislationSignatures.Add(new LegalLegislationSignature
                            {
                                SignatureId = results.SignatureId,
                                LegislationId = results.LegislationId,
                                Full_Name = results.Full_Name,
                                Job_Title = results.Job_Title,
                            });
                        }
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        private async Task GetManytoManyLegislationReference(Guid LegislationId, DatabaseContext dbContext)
        {
            try
            {
                List<LegalLegislationReference>? ArticalIdResults = await dbContext.legalLegislationReferences.Where(r => r.Reference_Parent_Id == LegislationId).ToListAsync();
                if (ArticalIdResults.Count() > 0)
                {
                    foreach (var item in ArticalIdResults)
                    {
                        LegalLegislationReference? results = dbContext.legalLegislationReferences.Where(r => r.ReferenceId == item.ReferenceId).FirstOrDefault();

                        if (results != null)
                        {
                            _legalLegislationReferences.Add(new LegalLegislationReference
                            {
                                ReferenceId = results.ReferenceId,
                                Reference_Parent_Id = results.Reference_Parent_Id,
                                Legislation_Link_Id = results.Legislation_Link_Id,
                                Legislation_Link = results.Legislation_Link,
                            });
                        }
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Check Legislation Number Duplication
        public async Task<bool> CheckLegislationNumberDuplication(int legislationType, string legislationNumber)
        {
            try
            {
                var task = await _dbContext.legalLegislations.Where(x => x.Legislation_Type == legislationType && x.Legislation_Number == legislationNumber).FirstOrDefaultAsync();
                if (task != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Get legal template details
        public async Task<List<LegalTemplate>> GetLegalTemplateDetails()
        {
            try
            {
                legalTemplateForNewTemplate = await _dbContext.legalTemplates.OrderByDescending(x => x.TemplateId).ToListAsync();
                if (legalTemplateForNewTemplate.Count() != 0)
                {
                    var resultNewTemplate = legalTemplateForNewTemplate.Where(x => x.Template_Name.Contains("New Template")).FirstOrDefault();
                    if (resultNewTemplate == null)
                    {
                        var resultAddedNewTemplate = await InsertDefaultRecordInTemplateTable(_dbContext);
                        if (resultAddedNewTemplate != null)
                        {
                            legalTemplateForNewTemplate.Add(resultAddedNewTemplate);
                            return legalTemplateForNewTemplate;
                        }
                    }
                    return legalTemplateForNewTemplate;
                }
                var result = await InsertDefaultRecordInTemplateTable(_dbContext);
                if (result != null)
                {
                    legalTemplateForNewTemplate.Add(result);
                    return legalTemplateForNewTemplate;
                }
                return new List<LegalTemplate>();
            }
            catch (Exception)
            {
                return new List<LegalTemplate>();
            }
        }

        private async Task<LegalTemplate> InsertDefaultRecordInTemplateTable(DatabaseContext dbContext)
        {
            try
            {
                LegalTemplate Obj = new LegalTemplate();
                Obj.TemplateId = Guid.NewGuid();
                Obj.Template_Name = "New Template";
                Obj.Legislation_Type = (int)LegalLegislationTypeEnum.Law;
                Obj.IsDefault = true;
                await dbContext.legalTemplates.AddAsync(Obj);
                await dbContext.SaveChangesAsync();
                return Obj;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region legal legislation Tags
        public async Task<LegalLegislationTag> CreateLegalLegislationTags(LegalLegislationTag legalLegislationTag)
        {
            try
            {
                await _dbContext.legalLegislationTags.AddAsync(legalLegislationTag);
                await _dbContext.SaveChangesAsync();
                return legalLegislationTag;
            }
            catch (Exception ex)
            {
                return new LegalLegislationTag();
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get legal template setting details
        public async Task<List<LegalTemplateSetting>> GetLegalTemplateSettingDetails()
        {
            try
            {
                var task = await _dbContext.legalTemplateSettings.OrderBy(x => x.TemplateSettingId).ToListAsync();
                if (task.Count() != 0)
                {
                    return task;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Check and get template selected value details
        public async Task<LegalTemplate> GetRegisteredTemplateDetailsByUsingSelectedTemplateId(Guid templateId)
        {
            try
            {
                var task = await _dbContext.legalTemplates.Where(x => x.TemplateId == templateId).FirstOrDefaultAsync();
                if (task != null)
                {
                    var result = await GetRelatedTemplateSettingCheckBoxDetails(task.TemplateId, _dbContext);
                    if (result.Count() != 0)
                    {
                        task.SelectedCheckBoxValues = result;
                    }
                    return task;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

        private async Task<List<int>> GetRelatedTemplateSettingCheckBoxDetails(Guid templateId, DatabaseContext dbContext)
        {
            try
            {
                var result = await dbContext.legalLegislationLegalTemplates.Where(x => x.TemplateId == templateId).Select(z => z.TemplateSettingId).Distinct().ToListAsync();
                if (result.Count() != 0)
                {
                    List<int>? Obj = new List<int>();
                    foreach (var item in result)
                    {
                        Obj.Add(item);
                    }
                    return Obj;
                }
                return new List<int>();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Count Associated Legislation In Template By Using TemplateId
        public async Task<int> CountAssociatedLegislationInTemplateByUsingTemplateId(Guid templateId)
        {
            try
            {
                int task = _dbContext.legalLegislationLegalTemplates.Where(x => x.TemplateId == templateId).GroupBy(x => x.LegislationId).Count();
                if (task != 0)
                {
                    return task;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Get Legal Legislation Details By Using LegislationId For Edit Form
        public async Task<LegalLegislation> GetLegalLegislationDetailsByUsingLegislationIdForEditForm(Guid legislationId)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.legalLegislations.Where(x => x.LegislationId == legislationId).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            var resultCheckboxes = await GetLegalTemplateSettingValuesByUsingLegislationAndTemplateId(task.LegislationId, _dbContext);
                            if (resultCheckboxes.Count() != 0)
                            {
                                List<int>? ObjCheckbox = new List<int>();
                                foreach (var item in resultCheckboxes)
                                {
                                    ObjCheckbox.Add(item.TemplateSettingId);
                                }
                                var objTemplate = resultCheckboxes.FirstOrDefault();
                                var resultTemplate = await GetLegalTemplateByUsingLegislationId(objTemplate, _dbContext);
                                if (resultTemplate != null)
                                {
                                    task.LegalTemplates = resultTemplate;
                                    task.LegalTemplates.SelectedCheckBoxValues = ObjCheckbox;
                                }
                            }
                            var resultTags = await GetLegalLegislationTagsDetails(task.LegislationId, _dbContext);
                            if (resultTags.Count() != 0)
                            {
                                task.LegalLegislationTags = resultTags;
                            }
                            var resultPublication = await GetLegalLegislationPublicationDetails(task.LegislationId, _dbContext);
                            if (resultPublication != null)
                            {
                                task.LegalPublicationSources.Add(resultPublication);
                            }
                            var resultSignatures = await GetLegalLegislationSignatureDetails(task.LegislationId, _dbContext);
                            if (resultSignatures.Count() != 0)
                            {
                                task.LegalLegislationSignatures = resultSignatures;
                            }
                            var resultReferences = await GetLegalLegislationReferenceDetails(task.LegislationId, _dbContext);
                            if (resultReferences.Count() != 0)
                            {
                                task.LegalLegislationReferences = resultReferences;
                            }
                            var resultSections = await GetLegalLegislationSectionDetails(task.LegislationId, _dbContext);
                            if (resultSections.Count() != 0)
                            {
                                task.LegalSections = resultSections;
                            }
                            var resultArticles = await GetLegalLegislationArticleDetails(task.LegislationId, _dbContext);
                            if (resultArticles.Count() != 0)
                            {
                                task.LegalArticles = resultArticles;
                            }
                            var resultClauses = await GetLegalLegislationClauseDetails(task.LegislationId, _dbContext);
                            if (resultClauses.Count() != 0)
                            {
                                task.LegalClauses = resultClauses;
                            }
                            var resultExplanatory = await GetLegalLegislationExplanatoryNoteDetail(task.LegislationId, _dbContext);
                            if (resultExplanatory != null)
                            {
                                task.LegalExplanatoryNotes.Add(resultExplanatory);
                            }
                            var resultNote = await GetLegalLegislationNoteDetail(task.LegislationId, _dbContext);
                            if (resultNote != null)
                            {
                                task.legalNotes.Add(resultNote);
                            }

                            transaction.Commit();
                            return task;
                        }
                        return new LegalLegislation();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return new LegalLegislation();
                    }
                }
            }
        }

        private async Task<LegalTemplate> GetLegalTemplateByUsingLegislationId(LegalLegislationLegalTemplate? objTemplate, DatabaseContext dbContext)
        {
            try
            {
                var resultTemplate = await dbContext.legalTemplates.Where(x => x.TemplateId == objTemplate.TemplateId).FirstOrDefaultAsync();
                if (resultTemplate != null)
                {
                    return resultTemplate;
                }
                return new LegalTemplate();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<LegalLegislationLegalTemplate>> GetLegalTemplateSettingValuesByUsingLegislationAndTemplateId(Guid legislationId, DatabaseContext dbContext)
        {
            try
            {
                var result = await dbContext.legalLegislationLegalTemplates.Where(x => x.LegislationId == legislationId).ToListAsync();
                if (result.Count() != 0)
                {
                    return result;
                }
                return new List<LegalLegislationLegalTemplate>();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private async Task<List<int>> GetLegalLegislationTagsDetails(Guid legislationId, DatabaseContext dbContext)
        {
            try
            {
                var result = await dbContext.legalLegislationLegalTags.Where(x => x.LegislationId == legislationId).ToListAsync();
                if (result.Count() != 0)
                {
                    List<int>? ObjResult = new List<int>();
                    foreach (var item in result)
                    {
                        ObjResult.Add(item.TagId);
                    }
                    return ObjResult;
                }
                return new List<int>();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private async Task<LegalPublicationSource> GetLegalLegislationPublicationDetails(Guid legislationId, DatabaseContext dbContext)
        {
            try
            {
                var result = await dbContext.legalPublicationSources.Where(x => x.LegislationId == legislationId).FirstOrDefaultAsync();
                if (result != null)
                {
                    return result;
                }
                return new LegalPublicationSource();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private async Task<List<LegalLegislationSignature>> GetLegalLegislationSignatureDetails(Guid legislationId, DatabaseContext dbContext)
        {
            try
            {
                var result = await dbContext.legalLegislationSignatures.Where(x => x.LegislationId == legislationId).ToListAsync();
                if (result.Count() != 0)
                {
                    return result;
                }
                return new List<LegalLegislationSignature>();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private async Task<List<LegalLegislationReference>> GetLegalLegislationReferenceDetails(Guid legislationId, DatabaseContext dbContext)
        {
            try
            {
                var result = await dbContext.legalLegislationReferences.Where(x => x.Reference_Parent_Id == legislationId).ToListAsync();
                if (result.Count() != 0)
                {
                    return result;
                }
                return new List<LegalLegislationReference>();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private async Task<List<LegalSection>> GetLegalLegislationSectionDetails(Guid legislationId, DatabaseContext dbContext)
        {
            try
            {
                var result = await dbContext.legalSections.Where(x => x.LegislationId == legislationId).ToListAsync();
                if (result.Count() != 0)
                {
                    return result;
                }
                return new List<LegalSection>();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private async Task<List<LegalArticle>> GetLegalLegislationArticleDetails(Guid legislationId, DatabaseContext dbContext)
        {
            try
            {
                var result = await dbContext.legalArticles.OrderBy(x => x.ArticleOrder).Where(x => x.LegislationId == legislationId).ToListAsync();
                if (result.Count() != 0)
                {
                    return result;
                }
                return new List<LegalArticle>();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private async Task<List<LegalClause>> GetLegalLegislationClauseDetails(Guid legislationId, DatabaseContext dbContext)
        {
            try
            {
                var result = await dbContext.legalClauses.OrderBy(x => x.ClauseOrder).Where(x => x.LegislationId == legislationId).ToListAsync();
                if (result.Count() != 0)
                {
                    return result;
                }
                return new List<LegalClause>();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private async Task<LegalExplanatoryNote> GetLegalLegislationExplanatoryNoteDetail(Guid legislationId, DatabaseContext dbContext)
        {
            try
            {
                var result = await dbContext.legalExplanatoryNotes.Where(x => x.LegislationId == legislationId).FirstOrDefaultAsync();
                if (result != null)
                {
                    return result;
                }
                return new LegalExplanatoryNote();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private async Task<LegalNote> GetLegalLegislationNoteDetail(Guid legislationId, DatabaseContext dbContext)
        {
            try
            {
                var result = await dbContext.legalNotes.Where(x => x.ParentId == legislationId).FirstOrDefaultAsync();
                if (result != null)
                {
                    return result;
                }
                return new LegalNote();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Soft delete

        public async Task SoftDeleteLegalLegislation(LegalLegislationsVM legalLegislationsVM)
        {

            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        LegalLegislation? legalLegislation = await _dbContext.legalLegislations.FindAsync(legalLegislationsVM.LegislationId);
                        if (legalLegislation != null)
                        {
                            legalLegislation.DeletedBy = legalLegislationsVM.ModifiedBy;
                            legalLegislation.IsDeleted = true;
                            legalLegislation.DeletedDate = DateTime.Now;
                            _dbContext.Entry(legalLegislation).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();
                            //For Notification
                            legalLegislationsVM.NotificationParameter.Type = _dbContext.legalLegislationTypes.Where(x => x.Id == legalLegislation.Legislation_Type).Select(x => x.Name_En + "/" + x.Name_Ar).FirstOrDefault();
                            legalLegislationsVM.NotificationParameter.Entity = "Legal Legislation";
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }
        #endregion

        #region Update legal legislation
        public async Task<bool> UpdateLegalLegislation(LegalLegislation args)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (args.Legislation_Flow_Status == (int)LegislationFlowStatusEnum.InReview)
                        {
                            var resultLegislation = await _dbContext.legalLegislations.Where(x => x.LegislationId == args.LegislationId).FirstOrDefaultAsync();
                            if (resultLegislation != null)
                            {
                                resultLegislation.Legislation_Type = args.Legislation_Type;
                                resultLegislation.Legislation_Number = args.Legislation_Number;
                                resultLegislation.Introduction = args.Introduction;
                                resultLegislation.IssueDate = args.IssueDate;
                                resultLegislation.IssueDate_Hijri = args.IssueDate_Hijri;
                                resultLegislation.LegislationTitle = args.LegislationTitle;
                                resultLegislation.Legislation_Comment = args.Legislation_Comment;
                                resultLegislation.Legislation_Status = args.Legislation_Status;
                                resultLegislation.Legislation_Flow_Status = args.Legislation_Flow_Status;
                                resultLegislation.ModifiedBy = args.ModifiedBy;
                                resultLegislation.ModifiedDate = args.ModifiedDate;
                                resultLegislation.CanceledBy = args.CanceledBy;
                                resultLegislation.CanceledDate = args.CanceledDate;
                                resultLegislation.DeletedBy = args.DeletedBy;
                                resultLegislation.DeletedDate = args.DeletedDate;
                                resultLegislation.IsDeleted = args.IsDeleted;
                                resultLegislation.ExistingLegislationIdForNewLegislation = args.ExistingLegislationIdForNewLegislation;
                                resultLegislation.OldLegislation_EndDate = args.OldLegislation_EndDate;
                                resultLegislation.OldLegislation_Status = args.OldLegislation_Status;
                                resultLegislation.NumberofArticle = args.NumberofArticle;
                                resultLegislation.NumberofClause = args.NumberofClause;
                                _dbContext.Entry(resultLegislation).State = EntityState.Modified;
                                await _dbContext.SaveChangesAsync();
                            }
                            else
                            {
                                await _dbContext.AddAsync(args);
                                await _dbContext.SaveChangesAsync();
                            }

                            //bool resultAttachment = await CheckNewAttachmentInUpdateCase(args.LegislationId, _dbContext);
                            //bool resultAttachment = await _tempFileUploadRepository.CheckNewAttachmentInUpdateCase(args.LegislationId);
                            //if (resultAttachment)
                            //{
                            //    var result = await _tempFileUploadRepository.RemoveDocument(args.LegislationId);
                            //    //await DeleteExistingAttachmentFromUploadDocumentTable(args.LegislationId, _dbContext);
                            //    //bool response = await InsertAttachmentFromTempToUploadDocument(args, _dbContext);
                            //    //if (response)
                            //    //{
                            //    //    await DeleteFromTempAttachmentTable(args, _dbContext);
                            //    //}
                            //}
                            //bool response = await UpdateAttachmentInUploadDocument(args, _dbContext);
                            //if (response)
                            //{
                            //    await DeleteFromTempAttachmentTable(args, _dbContext);
                            //}
                            if (args.LegalTemplates != null && args.LegalTemplates.SelectedCheckBoxValues.Count() != 0)
                            {
                                await UpdateLegalTemplateAndSelectedCheckBoxValues(args.LegislationId, args.LegalTemplates, _dbContext);
                            }
                            //if (args.LegalTemplates.SelectedCheckBoxValues.Count() != 0)
                            //{
                            //    await UpdateLegalTemplateSettingValues(args.LegislationId, args.LegalTemplates.TemplateId, args.LegalTemplates.SelectedCheckBoxValues, _dbContext);
                            ////}
                            //if (args.LegalLegislationTags.Count() != 0)
                            //{
                            await UpdateMangtoManyLegislationTags(args.LegislationId, args.LegalLegislationTags, _dbContext);
                            //}
                            //if (args.LegalPublicationSources.Count() != 0)
                            //{
                            await UpdateLegalLegislationPublication(args.LegislationId, args.LegalPublicationSources, _dbContext);
                            //}
                            if (args.LegalLegislationSignatures.Count() != 0)
                            {
                                await UpdateLegalLegislationSignature(args.LegalLegislationSignatures, _dbContext);
                            }
                            //if (args.LegalLegislationReferences.Count() != 0)
                            //{
                            await UpdateLegalLegislationReference(args.LegislationId, args.LegalLegislationReferences, _dbContext);
                            //}
                            if (args.LegalSections.Count() != 0)
                            {
                                await UpdateLegalLegislationSection(args.LegalSections, _dbContext);
                            }
                            //if (args.LegalArticles.Count() != 0)
                            //{
                            await UpdateLegalLegislationArticle(args.LegislationId, args.LegalArticles, _dbContext);
                            //}
                            //if (args.LegalClauses.Count() != 0)
                            //{
                            await UpdateLegalLegislationClause(args.LegislationId, args.LegalClauses, _dbContext);
                            //}
                            //if (args.LegalExplanatoryNotes.Count() != 0)
                            //{
                            await UpdateLegalLegislationExplanatoryNote(args.LegislationId, args.LegalExplanatoryNotes, _dbContext);
                            //}
                            //if (args.legalNotes.Count() != 0)
                            //{
                            await UpdateLegalLegislationNote(args.LegislationId, args.legalNotes, _dbContext);
                            //}
                            await UpdateLinkedWorkflowInstance(args, _dbContext);
                            if (args.SelectedSourceDocumentForDelete.Count() != 0)
                            {
                                await SelectedSourceDocumentDelete(args.SelectedSourceDocumentForDelete, _dmsDbContext);
                            }
                            if (args.SelectedSourceDocumentForDelete.Count() != 0)
                            {
                                //await DeleteSelectedSourceDocumentFromUploadedDocumentTable(args.SelectedSourceDocumentForDelete, _dbContext);
                            }

                            transaction.Commit();
                            return true;
                        }
                        else
                        {
                            var resultLegislation = await _dbContext.legalLegislations.Where(x => x.LegislationId == args.LegislationId).FirstOrDefaultAsync();
                            if (resultLegislation != null)
                            {
                                resultLegislation.Legislation_Type = args.Legislation_Type;
                                resultLegislation.Legislation_Number = args.Legislation_Number;
                                resultLegislation.Introduction = args.Introduction;
                                resultLegislation.IssueDate = args.IssueDate;
                                resultLegislation.IssueDate_Hijri = args.IssueDate_Hijri;
                                resultLegislation.LegislationTitle = args.LegislationTitle;
                                resultLegislation.Legislation_Comment = args.Legislation_Comment;
                                resultLegislation.Legislation_Status = args.Legislation_Status;
                                resultLegislation.Legislation_Flow_Status = args.Legislation_Flow_Status;
                                resultLegislation.StartDate = args.StartDate;
                                resultLegislation.AddedBy = args.AddedBy;
                                resultLegislation.AddedDate = args.AddedDate;
                                resultLegislation.ModifiedBy = args.ModifiedBy;
                                resultLegislation.ModifiedDate = args.ModifiedDate;
                                resultLegislation.CanceledBy = args.CanceledBy;
                                resultLegislation.CanceledDate = args.CanceledDate;
                                resultLegislation.DeletedBy = args.DeletedBy;
                                resultLegislation.DeletedDate = args.DeletedDate;
                                resultLegislation.IsDeleted = args.IsDeleted;
                                resultLegislation.ExistingLegislationIdForNewLegislation = args.ExistingLegislationIdForNewLegislation;
                                resultLegislation.OldLegislation_EndDate = args.OldLegislation_EndDate;
                                resultLegislation.OldLegislation_Status = args.OldLegislation_Status;
                                resultLegislation.NumberofArticle = args.NumberofArticle;
                                resultLegislation.NumberofClause = args.NumberofClause;
                                _dbContext.Entry(resultLegislation).State = EntityState.Modified;
                                await _dbContext.SaveChangesAsync();
                            }
                            else
                            {
                                await _dbContext.AddAsync(args);
                                await _dbContext.SaveChangesAsync();
                            }

                            //bool response = await UpdateAttachmentInUploadDocument(args, _dbContext);
                            //if (response)
                            //{
                            //    await DeleteFromTempAttachmentTable(args, _dbContext);
                            //}
                            if (args.LegalTemplates != null && args.LegalTemplates.SelectedCheckBoxValues.Count() != 0)
                            {
                                await UpdateLegalTemplateAndSelectedCheckBoxValues(args.LegislationId, args.LegalTemplates, _dbContext);
                            }
                            //if (args.LegalTemplates.SelectedCheckBoxValues.Count() != 0)
                            //{
                            //    await UpdateLegalTemplateSettingValues(args.LegislationId, args.LegalTemplates.TemplateId, args.LegalTemplates.SelectedCheckBoxValues, _dbContext);
                            ////}
                            //if (args.LegalLegislationTags.Count() != 0)
                            //{
                            await UpdateMangtoManyLegislationTags(args.LegislationId, args.LegalLegislationTags, _dbContext);
                            //}
                            //if (args.LegalPublicationSources.Count() != 0)
                            //{
                            await UpdateLegalLegislationPublication(args.LegislationId, args.LegalPublicationSources, _dbContext);
                            //}
                            if (args.LegalLegislationSignatures.Count() != 0)
                            {
                                await UpdateLegalLegislationSignature(args.LegalLegislationSignatures, _dbContext);
                            }
                            //if (args.LegalLegislationReferences.Count() != 0)
                            //{
                            await UpdateLegalLegislationReference(args.LegislationId, args.LegalLegislationReferences, _dbContext);
                            //}
                            if (args.LegalSections.Count() != 0)
                            {
                                await UpdateLegalLegislationSection(args.LegalSections, _dbContext);
                            }
                            //if (args.LegalArticles.Count() != 0)
                            //{
                            await UpdateLegalLegislationArticle(args.LegislationId, args.LegalArticles, _dbContext);
                            //}
                            //if (args.LegalClauses.Count() != 0)
                            //{
                            await UpdateLegalLegislationClause(args.LegislationId, args.LegalClauses, _dbContext);
                            //}
                            //if (args.LegalExplanatoryNotes.Count() != 0)
                            //{
                            await UpdateLegalLegislationExplanatoryNote(args.LegislationId, args.LegalExplanatoryNotes, _dbContext);
                            //}
                            //if (args.legalNotes.Count() != 0)
                            //{
                            await UpdateLegalLegislationNote(args.LegislationId, args.legalNotes, _dbContext);
                            //}
                            // await DeleteExistingAttachmentFromUploadDocumentTable(args.LegislationId, _dbContext);
                            //if (args.SelectedSourceDocumentForDelete.Count() != 0)
                            //{
                            //    await SelectedSourceDocumentDelete(args.SelectedSourceDocumentForDelete, _dbContext);
                            //}

                            //if (args.SelectedSourceDocumentForDelete.Count() != 0)
                            //{
                            //   
                            //    //await DeleteSelectedSourceDocumentFromUploadedDocumentTable(args.SelectedSourceDocumentForDelete, _dbContext);
                            //}
                            transaction.Commit();
                            //For Notification
                            args.NotificationParameter.Type = _dbContext.legalLegislationTypes.Where(x => x.Id == resultLegislation.Legislation_Type).Select(x => x.Name_En + "/" + x.Name_Ar).FirstOrDefault();
                            args.NotificationParameter.Entity = "Legal Legislation";
                            return true;


                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        private async Task UpdateLegalLegislationNote(Guid legislationId, List<LegalNote> legalNotes, DatabaseContext dbContext)
        {
            try
            {
                if (legalNotes.Count() != 0)
                {
                    foreach (var item in legalNotes)
                    {
                        if (item.ParentId != Guid.Empty)
                        {
                            var resultNotes = await dbContext.legalNotes.Where(x => x.ParentId == item.ParentId && x.NoteId == item.NoteId).FirstOrDefaultAsync();
                            if (resultNotes != null)
                            {
                                resultNotes.ParentId = item.ParentId;
                                resultNotes.Note_Text = item.Note_Text;
                                resultNotes.Note_Location = item.Note_Location;
                                resultNotes.Note_Date = item.Note_Date;
                                dbContext.Entry(resultNotes).State = EntityState.Modified;
                            }
                            else
                            {
                                await dbContext.legalNotes.AddAsync(item);
                            }
                        }
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task UpdateLegalLegislationExplanatoryNote(Guid legislationId, List<LegalExplanatoryNote> legalExplanatoryNotes, DatabaseContext dbContext)
        {
            try
            {
                var resultSavedExplanatory = await dbContext.legalExplanatoryNotes.Where(x => x.LegislationId == legislationId).ToListAsync();
                if (resultSavedExplanatory.Count() != 0)
                {
                    foreach (var item in resultSavedExplanatory)
                    {
                        dbContext.legalExplanatoryNotes.Remove(item);
                    }
                    await dbContext.SaveChangesAsync();
                }
                if (legalExplanatoryNotes.Count() != 0)
                {
                    foreach (var item in legalExplanatoryNotes)
                    {
                        if (item.LegislationId != Guid.Empty)
                        {
                            var resultExplanatory = await dbContext.legalExplanatoryNotes.Where(x => x.LegislationId == item.LegislationId).FirstOrDefaultAsync();
                            if (resultExplanatory != null)
                            {
                                resultExplanatory.LegislationId = item.LegislationId;
                                resultExplanatory.ExplanatoryNote_Body = item.ExplanatoryNote_Body;
                                dbContext.Entry(resultExplanatory).State = EntityState.Modified;
                            }
                            else
                            {
                                await dbContext.legalExplanatoryNotes.AddAsync(item);
                            }
                        }
                    }
                    await dbContext.SaveChangesAsync();

                    //if (legalExplanatoryNotes.Count() != 0)
                    //{
                    //    var resultFile = await AddExplanatoryNoteAttachmentFromTempToUploadDocumentTable(legalExplanatoryNotes, dbContext);
                    //    if (resultFile)
                    //    {
                    //        await DeleteExplanatoryNoteFromTempAttachmentTable(legalExplanatoryNotes, dbContext);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task UpdateLegalLegislationClause(Guid legislationId, List<LegalClause> legalClauses, DatabaseContext dbContext)
        {
            try
            {
                var resultSavedClauses = await dbContext.legalClauses.Where(x => x.LegislationId == legislationId).ToListAsync();
                if (resultSavedClauses.Count() != 0)
                {
                    foreach (var item in resultSavedClauses)
                    {
                        var resultClauses = await dbContext.legalClauses.Where(x => x.LegislationId == item.LegislationId).FirstOrDefaultAsync();
                        if (resultClauses != null)
                        {
                            dbContext.legalClauses.Remove(resultClauses);
                            await dbContext.SaveChangesAsync();
                        }
                    }
                }
                if (legalClauses.Count() != 0)
                {
                    foreach (var itemModel in legalClauses)
                    {
                        if (itemModel.Start_Date != null)
                        {
                            await dbContext.legalClauses.AddAsync(itemModel);
                            await dbContext.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task UpdateLegalLegislationArticle(Guid legislationId, List<LegalArticle> legalArticles, DatabaseContext dbContext)
        {
            try
            {
                var resultSavedArticles = await dbContext.legalArticles.Where(x => x.LegislationId == legislationId).ToListAsync();
                if (resultSavedArticles.Count() != 0)
                {
                    foreach (var item in resultSavedArticles)
                    {
                        var resultArticles = await dbContext.legalArticles.Where(x => x.LegislationId == item.LegislationId).FirstOrDefaultAsync();
                        if (resultArticles != null)
                        {
                            dbContext.legalArticles.Remove(resultArticles);
                            await dbContext.SaveChangesAsync();
                        }
                    }
                }
                if (legalArticles.Count() != 0)
                {
                    foreach (var itemModel in legalArticles)
                    {
                        if (itemModel.Start_Date != null)
                        {
                            await dbContext.legalArticles.AddAsync(itemModel);
                            await dbContext.SaveChangesAsync();
                            if (itemModel.ExistingArticleId != Guid.Empty)
                            {
                                await ManageNextArticleIdFieldInArticleTable(itemModel, dbContext);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task UpdateLegalLegislationSection(List<LegalSection> legalSections, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in legalSections)
                {
                    var resultSections = await dbContext.legalSections.Where(x => x.LegislationId == item.LegislationId).FirstOrDefaultAsync();
                    if (resultSections != null)
                    {
                        dbContext.legalSections.Remove(resultSections);
                        await dbContext.SaveChangesAsync();
                    }
                }

                foreach (var itemModel in legalSections)
                {
                    await dbContext.legalSections.AddAsync(itemModel);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task UpdateLegalLegislationReference(Guid legislationId, List<LegalLegislationReference> legalLegislationReferences, DatabaseContext dbContext)
        {
            try
            {
                //var resultSavedReferences = await dbContext.legalLegislationReferences.Where(x => x.Reference_Parent_Id == legislationId).ToListAsync();
                //if (resultSavedReferences.Count() != 0)
                //{
                //    foreach (var item in resultSavedReferences)
                //    {
                //        dbContext.legalLegislationReferences.Remove(item);
                //    }
                //    await dbContext.SaveChangesAsync();
                //}
                if (legalLegislationReferences.Count() != 0)
                {
                    foreach (var item in legalLegislationReferences)
                    {
                        var resultReferences = await dbContext.legalLegislationReferences.Where(x => x.ReferenceId == item.ReferenceId).FirstOrDefaultAsync();
                        if (resultReferences != null)
                        {
                            resultReferences.Reference_Parent_Id = item.Reference_Parent_Id;
                            resultReferences.Legislation_Link_Id = item.Legislation_Link_Id;
                            resultReferences.Legislation_Link = item.Legislation_Link;
                            dbContext.Entry(resultReferences).State = EntityState.Modified;
                        }
                        else
                        {
                            await dbContext.legalLegislationReferences.AddAsync(item);
                        }
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task UpdateLegalLegislationSignature(List<LegalLegislationSignature> legalLegislationSignatures, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in legalLegislationSignatures)
                {
                    var resultSignatures = await dbContext.legalLegislationSignatures.Where(x => x.LegislationId == item.LegislationId).FirstOrDefaultAsync();
                    if (resultSignatures != null)
                    {
                        resultSignatures.LegislationId = item.LegislationId;
                        resultSignatures.Full_Name = item.Full_Name;
                        resultSignatures.Job_Title = item.Job_Title;
                        dbContext.Entry(resultSignatures).State = EntityState.Modified;
                    }
                    else
                    {
                        await dbContext.legalLegislationSignatures.AddAsync(item);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task UpdateLegalLegislationPublication(Guid legislationId, List<LegalPublicationSource> legalPublicationSources, DatabaseContext dbContext)
        {
            try
            {
                //var resultSavedPublications = await dbContext.legalPublicationSources.Where(x => x.LegislationId == legislationId).FirstOrDefaultAsync();
                //if (resultSavedPublications != null)
                //{
                //    dbContext.legalPublicationSources.Remove(resultSavedPublications);
                //}
                //await dbContext.SaveChangesAsync();

                if (legalPublicationSources.Count() != 0)
                {
                    foreach (var item in legalPublicationSources)
                    {
                        if (item.PublicationNameId != 0)
                        {
                            var resultPublications = await dbContext.legalPublicationSources.Where(x => x.LegislationId == item.LegislationId && x.SourceId == item.SourceId).FirstOrDefaultAsync();
                            if (resultPublications != null)
                            {
                                resultPublications.PublicationNameId = item.PublicationNameId;
                                resultPublications.LegislationId = item.LegislationId;
                                resultPublications.Issue_Number = item.Issue_Number;
                                resultPublications.PublicationDate = item.PublicationDate;
                                resultPublications.PublicationDate_Hijri = item.PublicationDate_Hijri;
                                resultPublications.Page_Start = item.Page_Start;
                                resultPublications.Page_End = item.Page_End;
                                _dbContext.Entry(resultPublications).State = EntityState.Modified;
                                //await dbContext.SaveChangesAsync();
                            }
                            else
                            {
                                await dbContext.legalPublicationSources.AddAsync(item);
                            }
                        }
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task UpdateMangtoManyLegislationTags(Guid legislationId, List<int> legalLegislationTags, DatabaseContext dbContext)
        {
            try
            {
                var resultTags = await dbContext.legalLegislationLegalTags.Where(x => x.LegislationId == legislationId).ToListAsync();
                if (resultTags.Count() != 0)
                {
                    foreach (var item in resultTags)
                    {
                        dbContext.legalLegislationLegalTags.Remove(item);
                    }
                    await dbContext.SaveChangesAsync();
                }
                if (legalLegislationTags.Count() != 0)
                {
                    foreach (var item in legalLegislationTags)
                    {
                        LegalLegislationLegalTag Obj = new LegalLegislationLegalTag();
                        Obj.LegislationId = legislationId;
                        Obj.TagId = item;
                        await dbContext.legalLegislationLegalTags.AddAsync(Obj);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task UpdateLegalTemplateSettingValues(Guid legislationId, Guid templateId, List<int> selectedCheckBoxValues, DatabaseContext dbContext)
        {
            try
            {
                var resultTemplateSetting = await dbContext.legalLegislationLegalTemplates.Where(x => x.LegislationId == legislationId).ToListAsync();
                if (resultTemplateSetting.Count() != 0)
                {
                    foreach (var item in resultTemplateSetting)
                    {
                        dbContext.legalLegislationLegalTemplates.Remove(item);
                    }
                    await dbContext.SaveChangesAsync();
                }
                foreach (var item in selectedCheckBoxValues)
                {
                    LegalLegislationLegalTemplate? Obj = new LegalLegislationLegalTemplate();
                    Obj.LegislationId = legislationId;
                    Obj.TemplateId = templateId;
                    Obj.TemplateSettingId = item;
                    await dbContext.legalLegislationLegalTemplates.AddAsync(Obj);
                }
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task UpdateLegalTemplateAndSelectedCheckBoxValues(Guid legislationId, LegalTemplate legalTemplates, DatabaseContext dbContext)
        {
            try
            {
                var resultTemplateIdDetails = await dbContext.legalLegislationLegalTemplates.Where(x => x.LegislationId == legislationId).ToListAsync();
                if (resultTemplateIdDetails.Count() != 0)
                {
                    var resultTemplateId = resultTemplateIdDetails.Select(x => x.TemplateId).FirstOrDefault();
                    // first delete saved checkboxes record from Legal_Legislation_Legal_Template table
                    foreach (var item in resultTemplateIdDetails)
                    {
                        dbContext.legalLegislationLegalTemplates.Remove(item);
                    }
                    await dbContext.SaveChangesAsync();

                    // check if this template also associated with others legislation. If yes then not delete from legal template table. 
                    var getTemplateAssociatedLegislationList = await dbContext.legalLegislationLegalTemplates.Where(x => x.TemplateId == resultTemplateId).ToListAsync();
                    if (getTemplateAssociatedLegislationList.Count() != 0)
                    {
                        foreach (var item in getTemplateAssociatedLegislationList)
                        {
                            if (item.LegislationId != legislationId)
                            {
                                CountTemplateBindwithLegislation++;
                            }
                        }
                        if (CountTemplateBindwithLegislation == 0)
                        {
                            var resultTemplate = await dbContext.legalTemplates.Where(x => x.TemplateId == resultTemplateId).FirstOrDefaultAsync();
                            if (resultTemplate != null)
                            {
                                dbContext.legalTemplates.Remove(resultTemplate);
                                await dbContext.SaveChangesAsync();
                            }
                        }
                    }
                    else
                    {
                        var resultTemplateModel = await dbContext.legalTemplates.Where(x => x.TemplateId == resultTemplateId).FirstOrDefaultAsync();
                        if (resultTemplateModel != null)
                        {
                            dbContext.legalTemplates.Remove(resultTemplateModel);
                            await dbContext.SaveChangesAsync();
                        }
                    }

                    // now add new selected checkbox values to Legal_Legislation_Legal_Template table
                    if (legalTemplates != null && legalTemplates.SelectedCheckBoxValues.Count() != 0)
                    {
                        await AddNewLegalTemplate(legalTemplates, dbContext);
                        await AddLegalTemplateSettingValues(legislationId, legalTemplates.TemplateId, legalTemplates.SelectedCheckBoxValues, dbContext);
                    }


                    //var resultTemplate = await dbContext.legalTemplates.Where(x => x.TemplateId == resultTemplateId.TemplateId).FirstOrDefaultAsync();
                    //if (resultTemplate != null)
                    //{
                    //    // first check if this template also associated with others legislation. If yes then not delete only change association with this legislation. 
                    //    var getTemplateAssociatedLegislationList = await dbContext.legalLegislationLegalTemplates.Where(x => x.TemplateId == resultTemplate.TemplateId).ToListAsync();
                    //    if (getTemplateAssociatedLegislationList.Count() != 0)
                    //    {
                    //        var getLegislationId = getTemplateAssociatedLegislationList.Where(x => x.LegislationId == legislationId).FirstOrDefault();
                    //        if (getLegislationId != null)
                    //        {
                    //            dbContext.legalTemplates.Remove(resultTemplate);
                    //            await dbContext.SaveChangesAsync();
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    var resultTemplateSettingIds = await dbContext.legalLegislationLegalTemplates.Where(x => x.LegislationId == legislationId).ToListAsync();
                    //    if (resultTemplateSettingIds.Count() != 0)
                    //    {
                    //        foreach (var item in resultTemplateSettingIds)
                    //        {
                    //            dbContext.legalLegislationLegalTemplates.Remove(item);
                    //        }
                    //        await dbContext.SaveChangesAsync();
                    //    }
                    //}
                }
                else // will be executed in that case when legislation saved under the relation way.
                {
                    await AddNewLegalTemplate(legalTemplates, dbContext);
                    await AddLegalTemplateSettingValues(legislationId, legalTemplates.TemplateId, legalTemplates.SelectedCheckBoxValues, dbContext);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        //private async Task<bool> UpdateAttachmentInUploadDocument(LegalLegislation args, DatabaseContext dbContext)
        //{
        //    try
        //    {
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception(ex.Message);
        //    }
        //}


        #endregion

        #region Get Explanatory Note Attachment From Temp Table By Using ExplanatoryNoteId
        public async Task<List<TempAttachement>> GetExplanatoryNoteAttachmentFromTempTableByUsingId(Guid explanatoryNoteId)
        {
            try
            {
                List<TempAttachement>? getList = new List<TempAttachement>();
                var task = await _dmsDbContext.TempAttachements.Where(x => x.Guid == explanatoryNoteId).ToListAsync();
                if (task.Count() != 0)
                {
                    foreach (var item in task)
                    {
                        getList.Add(item);
                    }
                }
                var resultUpload = await _dmsDbContext.UploadedDocuments.Where(x => x.ReferenceGuid == explanatoryNoteId).ToListAsync();
                if (resultUpload.Count() != 0)
                {
                    int counter = 0;
                    foreach (var attachment in resultUpload)
                    {
                        TempAttachement tempObj = new TempAttachement();

                        tempObj.AttachementId = counter++;
                        tempObj.AttachmentTypeId = attachment.AttachmentTypeId;
                        tempObj.FileName = attachment.FileName;
                        tempObj.StoragePath = attachment.StoragePath;
                        tempObj.Guid = attachment.ReferenceGuid;
                        tempObj.UploadedBy = attachment.CreatedBy;
                        tempObj.UploadedDate = attachment.CreatedDateTime;
                        tempObj.DocType = attachment.DocType;
                        tempObj.Description = attachment.Description;
                        tempObj.FileNameWithoutTimeStamp = attachment.FileName;
                        tempObj.DocumentDate = attachment.DocumentDate;
                        getList.Add(tempObj);
                    }
                }
                return getList;
            }
            catch (Exception ex)
            {
                return new List<TempAttachement>();
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update Selected Section As Parent HasChild Column
        public async Task<bool> UpdateSelectedSectionAsParentHasChildColumn(Guid sectionId)
        {
            try
            {
                var result = await _dbContext.legalSections.Where(x => x.SectionId == sectionId).FirstOrDefaultAsync();
                if (result != null)
                {
                    result.HasChildren = true;
                    _dbContext.Entry(result).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
                throw;
            }
        }
        #endregion

        #region workflow
        private async Task LinkEntityWithActiveWorkflow(LegalLegislation legalLegislation, DatabaseContext dbContext)
        {
            try
            {
                string StoredProc = $"exec pWorkflowSelAdvanceSearch @statusId = '{(int)WorkflowStatusEnum.Active}', @submoduleId = '{(int)WorkflowSubModuleEnum.Legislations}'";
                var activeWorkflow = await dbContext.WorkflowVM.FromSqlRaw(StoredProc).ToListAsync();
                if (activeWorkflow?.Count() > 0)
                {
                    WorkflowActivity firstActivity = await dbContext.WorkflowActivity.Where(a => a.WorkflowId == (int)activeWorkflow.FirstOrDefault().WorkflowId).OrderBy(a => a.SequenceNumber).FirstOrDefaultAsync();
                    WorkflowInstance workflowInstance = new WorkflowInstance { ReferenceId = legalLegislation.LegislationId, StatusId = (int)WorkflowInstanceStatusEnum.InProgress, WorkflowId = (int)activeWorkflow.FirstOrDefault().WorkflowId, WorkflowActivityId = firstActivity.WorkflowActivityId, IsSlaExecuted = false };
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

        private async Task UpdateLinkedWorkflowInstance(LegalLegislation legalLegislation, DatabaseContext dbContext)
        {
            try
            {
                string StoredProc = $"exec pWorkflowSelAdvanceSearch @statusId = '{(int)WorkflowStatusEnum.Active}', @submoduleId = '{(int)WorkflowSubModuleEnum.LegalLegislations}'";
                var activeWorkflow = await dbContext.WorkflowVM.FromSqlRaw(StoredProc).ToListAsync();
                if (activeWorkflow?.Count() > 0)
                {
                    WorkflowActivity firstActivity = await dbContext.WorkflowActivity.Where(a => a.WorkflowId == (int)activeWorkflow.FirstOrDefault().WorkflowId).OrderBy(a => a.SequenceNumber).FirstOrDefaultAsync();
                    WorkflowInstance workflowInstance = await dbContext.WorkflowInstance.Where(w => w.ReferenceId == legalLegislation.LegislationId).FirstOrDefaultAsync();
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
                        workflowInstance = new WorkflowInstance { ReferenceId = legalLegislation.LegislationId, StatusId = (int)WorkflowInstanceStatusEnum.InProgress, WorkflowId = (int)activeWorkflow.FirstOrDefault().WorkflowId, WorkflowActivityId = firstActivity.WorkflowActivityId, IsSlaExecuted = false };
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

        #region Delete Attachment From Temp Table
        public async Task<bool> DeleteAttachmentFromTempTable(Guid legislationId)
        {
            try
            {
                TempAttachement? task = await _dmsDbContext.TempAttachements.Where(x => x.Guid == legislationId).FirstOrDefaultAsync();
                if (task != null)
                {
                    _dmsDbContext.TempAttachements.Remove(task);
                    await _dmsDbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
                throw;
            }
        }
        #endregion

        #region  Get Legal Legislation Rerference By Legislation Id
        public async Task<List<LegalLegislationReference>> GetLegalLegislationReferenceByLegislationId(Guid legislationId)
        {
            try
            {
                List<LegalLegislationReference>? task = await _dbContext.legalLegislationReferences.Where(x => x.Legislation_Link_Id == legislationId).ToListAsync();

                if (task.Count > 0)
                {
                    foreach (var item in task)
                    {
                        var resultLegislation = await _dbContext.legalLegislations.Where(x => x.LegislationId == item.Reference_Parent_Id).ToListAsync();
                        if (resultLegislation != null)
                        {
                            item.ReferenceAssociate = resultLegislation;
                        }
                    }
                    return task;
                }
                else
                {
                    return new List<LegalLegislationReference>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Revoke  delete Legislation 

        public async Task RevokeDeleteLegalLegislation(LegalLegislationsVM legalLegislationsVM)
        {

            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        LegalLegislation? legalLegislation = await _dbContext.legalLegislations.FindAsync(legalLegislationsVM.LegislationId);
                        if (legalLegislation != null)
                        {
                            legalLegislation.DeletedBy = legalLegislationsVM.ModifiedBy;
                            legalLegislation.IsDeleted = false;
                            legalLegislation.DeletedDate = DateTime.Now;
                            _dbContext.Entry(legalLegislation).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();
                            //For Notification
                            legalLegislationsVM.NotificationParameter.Type = _dbContext.legalLegislationTypes.Where(x => x.Id == legalLegislation.Legislation_Type).Select(x => x.Name_En + "/" + x.Name_Ar).FirstOrDefault();
                            legalLegislationsVM.NotificationParameter.Entity = "Legal Legislation";

                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }
        #endregion

        #region Get Delete Legislations 
        //<History Author = 'ijaz Ahmad' Date='2022-12-09' Version="1.0" Branch="master">Get Delete Legislation List </History>
        public async Task<List<LegalLegislationsVM>> GetDeleteLegalLegislations(int PageSize, int PageNumber)
        {
            try
            {
                if (_LegalLegislationsVM == null)
                {

                    string StoredProc = $"exec pDeleteLegislationList @lookupstableId='{(int)LookupsTablesEnum.LEGAL_LEGISLATION_TYPE}' ,@PageNumber ='{PageNumber}',@PageSize ='{PageSize}' ";
                    _LegalLegislationsVM = await _dbContext.LegalLegislationsVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _LegalLegislationsVM;
        }
        #endregion

        #region Source Documents Grid Delete Button Click
        public async Task SelectedSourceDocumentDelete(List<TempAttachementVM> selectedSourceDocumentForDelete, DmsDbContext dmsDbContext)
        {
            try
            {
                if (selectedSourceDocumentForDelete.Count() != 0)
                {
                    foreach (var item in selectedSourceDocumentForDelete)
                    {
                        var resultTempDocument = await dmsDbContext.TempAttachements.Where(x => x.AttachementId == item.AttachementId).FirstOrDefaultAsync();
                        if (resultTempDocument != null)
                        {
                            dmsDbContext.TempAttachements.Remove(resultTempDocument);
                        }
                    }
                    await dmsDbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Approved Legislation List
        //<History Author = 'ijaz Ahmad' Date='2022-12-09' Version="1.0" Branch="master">Get Approved Legislation List </History>
        public async Task<List<LegalLegislationsVM>> GetApporvedLegislation(string UserId, int PageSize, int PageNumber)
        {
            try
            {

                if (_LegalLegislationsVM == null)
                {

                    string StoredProc = $"exec pLegislationListFilteredByStatus @status = N'{(int)LegislationFlowStatusEnum.Approved}' , @userId ='{UserId}',@lookupstableId='{(int)LookupsTablesEnum.LEGAL_LEGISLATION_TYPE}'" +
                                        $",@PageNumber ='{PageNumber}',@PageSize ='{PageSize}'";
                    _LegalLegislationsVM = await _dbContext.LegalLegislationsVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _LegalLegislationsVM;
        }
        #endregion

        #region Get Article Number For Article Effect By Using LegislationId
        public async Task<List<ArticleNumberForEffect>> GetArticleNumberForArticleEffectByUsingLegislationId(Guid legislationId)
        {
            try
            {
                var task = _dbContext.legalArticles.Where(x => x.LegislationId == legislationId)
                    .OrderBy(x => x.ArticleOrder)
                    .AsEnumerable()
                    .Select((x, index) => new ArticleNumberForEffect
                    {
                        ArticleNumber = index + 1,
                        ArticleId = x.ArticleId
                    }).ToList();

                if (task.Count() != 0)
                {
                    //List<ArticleNumberForEffect> ObjResult = new List<ArticleNumberForEffect>();

                    //foreach (var item in task)
                    //{
                    //    ArticleNumberForEffect Obj = new ArticleNumberForEffect();
                    //    Obj.ArticleNumber = item.ArticleNumber;
                    //    Obj.ArticleId = item.ArticleId;
                    //    ObjResult.Add(Obj);

                    //}
                    return task;
                }
                else
                {
                    return new List<ArticleNumberForEffect>();
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Get legislation comments details
        public async Task<List<LegalLegislationCommentVM>> GetLegalLegislationCommentsDetailByUsingId(Guid legislationId)
        {
            try
            {
                string StoredProc = $"exec pLegislationCommentDetailByUsingId @LegislationId = '{legislationId}'";
                var legalLegislationCommentDetailVMs = await _dbContext.LegalLegislationCommentVMs.FromSqlRaw(StoredProc).ToListAsync();

                if (legalLegislationCommentDetailVMs.Count() != 0)
                {
                    return legalLegislationCommentDetailVMs;
                }
                else
                {
                    return new List<LegalLegislationCommentVM>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get All Template Setting Details
        public async Task<List<LegalLegislationLegalTemplate>> GetAllTemplateSettingDetails()
        {
            try
            {
                List<LegalLegislationLegalTemplate> Obj = new List<LegalLegislationLegalTemplate>();
                Obj = await _dbContext.legalLegislationLegalTemplates.ToListAsync();
                return Obj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region For Mobile Apps
        public async Task<MobileAppLegalLegislationDetailVM> GetLegalLegislationsDetailsForMobileApp(Guid LegislationId)
        {
            try
            {
                if (mobileAppLegalLegislationDetailVM == null)
                {
                    string StoredProc = $"exec pMobileAppLegalLegislationDetailbyId @LegislationId = '{LegislationId}'";
                    var result = await _dbContext.MobileAppLegalLegislationDetailVM.FromSqlRaw(StoredProc).ToListAsync();
                    if (result.Any())
                    {
                        mobileAppLegalLegislationDetailVM = result.FirstOrDefault();
                        string StoredProcDms = $"exec pMobileAppUploadDocumentsList @ReferenceId = N'{LegislationId}'";
                        mobileAppLegalLegislationDetailVM.Attachments = await _dmsDbContext.MobileAppUploadDocumentsVM.FromSqlRaw(StoredProcDms).ToListAsync();
                        if (mobileAppLegalLegislationDetailVM.Attachments.Any())
                        {
                            foreach (var item in mobileAppLegalLegislationDetailVM.Attachments)
                            {
                                var physicalPath = Path.Combine(_Config.GetValue<string>("dms_file_path") + item.FileUrl).Replace(@"\\", @"\");
#if !DEBUG
						physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
#endif
                                item.FileUrl = physicalPath;
                            }
                        }
                    }
                }
                return mobileAppLegalLegislationDetailVM;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Legal Legislation Approvals Details

        public async Task<List<LegalLegislationsVM>> GetLegalLegislationApprovals(string UserId, int PageSize, int PageNumber)
        {
            try
            {
                List<LegalLegislationsVM> resultList = new List<LegalLegislationsVM>();
                string StoredProc = $"exec pLegislationListFilteredByStatus @status = N'{(int)LegislationFlowStatusEnum.InReview}' , @userId ='{UserId}',@lookupstableId='{(int)LookupsTablesEnum.LEGAL_LEGISLATION_TYPE}'" +
                                        $",@PageNumber ='{PageNumber}',@PageSize ='{PageSize}'";
                resultList = await _dbContext.LegalLegislationsVMs.FromSqlRaw(StoredProc).ToListAsync();
                return resultList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}

