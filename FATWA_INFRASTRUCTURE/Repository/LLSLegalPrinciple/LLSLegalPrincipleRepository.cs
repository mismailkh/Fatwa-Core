using FATWA_DOMAIN.Interfaces.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.LegalPrinciple;
using FATWA_DOMAIN.Models.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel.MobileAppVMs;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using static FATWA_DOMAIN.Enums.LegalPrinciple.LegalPrincipleEnum;
using static FATWA_DOMAIN.Enums.WorkflowEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;

namespace FATWA_INFRASTRUCTURE.Repository.LLSLegalPrinciple
{
    //<!-- <History Author = 'Umer Zaman' Date='2024-04-18' Version="1.0" Branch="master">Create class to manage respositories</History>

    public class LLSLegalPrincipleRepository : ILLSLegalPrinciple
    {
        #region Constructure
        public LLSLegalPrincipleRepository(DatabaseContext dbContext, WorkflowRepository workflowRepository, DmsDbContext dbDmsContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _workflowRepository = workflowRepository;
            _dbDmsContext = dbDmsContext;
            _Config = config;
        }
        #endregion

        #region Variables
        private readonly DatabaseContext _dbContext;
        private readonly WorkflowRepository _workflowRepository;
        private readonly DmsDbContext _dbDmsContext;
        private readonly IConfiguration _Config;
        #endregion

        #region Get legal principle category list
        public async Task<List<LLSLegalPrincipleCategory>> GetLLSLegalPrincipleCategory()
        {
            try
            {
                var result = await _dbContext.LLSLegalPrincipleCategorys.OrderByDescending(x => x.CategoryId).ToListAsync();
                if (result.Count() != 0)
                {
                    return result;
                }
                else
                {
                    return new List<LLSLegalPrincipleCategory>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Save legal principle category
        public async Task<bool> SaveLegalPrincipleCategory(LLSLegalPrincipleCategory item)
        {
            try
            {
                await _dbContext.LLSLegalPrincipleCategorys.AddAsync(item);
                bool isAdded = await _dbContext.SaveChangesAsync() > 0;
                //var result = await _dbContext.LLSLegalPrincipleCategorys.OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
                return isAdded;

            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Advance search principle relation
        public async Task<List<LLSLegalPrinciplesRelationVM>> AdvanceSearchPrincipleRelation(LLSLegalPrinciplesRelationVM item)
        {
            try
            {
                List<LLSLegalPrinciplesRelationVM> resultPrincipleDetails = new List<LLSLegalPrinciplesRelationVM>();
                string trimmedString = null;
                if (item.PrincipleContent != null || !string.IsNullOrWhiteSpace(item.PrincipleContent))
                {
                    // Replace </div><div> sequences with a space
                    string withoutbothDivs = Regex.Replace(item.PrincipleContent, @"</div><div>", " ");
                    // Replace <div> sequences with a space
                    string withoutDivs = Regex.Replace(withoutbothDivs, @"<div>", " ");
                    // Replace <br> tags with spaces
                    string withoutBr = Regex.Replace(withoutDivs, @"<br\s*/?>", " ");
                    // Replace all html tags
                    string withoutTags = Regex.Replace(withoutBr, @"<[^>]+>", "");
                    // Replace HTML entities like &nbsp; with spaces
                    string withoutEntities = Regex.Replace(withoutTags, @"&\S+?;", " ");
                    // Remove extra spaces between strings
                    string withoutExtraSpaces = Regex.Replace(withoutEntities, @"\s+", " ");
                    // Trim leading and trailing spaces
                    trimmedString = withoutExtraSpaces.Trim();
                }
                var abc = new MarkupString(item.PrincipleContent);
                item.PrincipleContent = item.PrincipleContent is null ? trimmedString : item.PrincipleContent;
                string StoredProc = $"exec pLLSLegalPrincipleContentSearch @PrincipleContent = N'{item.PrincipleContent}', @FromPage = '{item.FromPage}'";
                resultPrincipleDetails = await _dbContext.LLSLegalPrinciplesRelationVMs.FromSqlRaw(StoredProc).ToListAsync();
                if (resultPrincipleDetails.Count() != 0)
                {
                    return resultPrincipleDetails;
                }
                return new List<LLSLegalPrinciplesRelationVM>();
            }
            catch (Exception ex)
            {
                return new List<LLSLegalPrinciplesRelationVM>();
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get legal principle detail by using document id

        public async Task<List<LLSLegalPrinciplesVM>> GetLLSLegalPrinciples(int UploadedDocumentId)
        {
            try
            {
                //string requestFrom = advanceSearchVM.PublicationFrom != null ? Convert.ToDateTime(advanceSearchVM.PublicationFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                //string requestTo = advanceSearchVM.PublicationTo != null ? Convert.ToDateTime(advanceSearchVM.PublicationTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string StoredProc = $"exec pLLSLegalPrincipleList @UploadedDocumentId = {UploadedDocumentId} ";
                var res = await _dbContext.LLSLegalPrinciplesVM.FromSqlRaw(StoredProc).ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Legal Principle Detail By Principle Id
        public async Task<LLSLegalPrincipleDetailVM> GetLLSLegalPrincipleDetailById(Guid principleId)
        {
            try
            {
                //string requestFrom = advanceSearchVM.PublicationFrom != null ? Convert.ToDateTime(advanceSearchVM.PublicationFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                //string requestTo = advanceSearchVM.PublicationTo != null ? Convert.ToDateTime(advanceSearchVM.PublicationTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string StoredProc = $"exec pLLSLegalPrincipleDetail @PrincipleId = '{principleId}' ";
                var res = await _dbContext.LLSLegalPrincipleDetailVM.FromSqlRaw(StoredProc).ToListAsync();
                return res.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Legal Principle References Principle Id
        public async Task<List<LLSLegalPrinciplReferenceVM>> GetLLSLegalPrincipleReferencesById(Guid principleId)
        {
            try
            {
                //string requestFrom = advanceSearchVM.PublicationFrom != null ? Convert.ToDateTime(advanceSearchVM.PublicationFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                //string requestTo = advanceSearchVM.PublicationTo != null ? Convert.ToDateTime(advanceSearchVM.PublicationTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string StoredProc = $"exec pLLSLegalPrincipleReference @PrincipleId = '{principleId}' ";
                var res = await _dbContext.LLSLegalPrinciplReferenceVM.FromSqlRaw(StoredProc).ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Save legal principle
        //<!-- <History Author = 'Umer Zaman' Date='2024-04-20' Version="1.0" Branch="master">save legal principle</History>

        public async Task<bool> SaveLLSLegalPrinciple(LLSLegalPrincipleSystem lLSLegalPrinciple)
        {
            using (_dbContext)
            {
                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var principlesCount = await _dbContext.LLSLegalPrinciples.CountAsync();
                        if (principlesCount > 0)
                        {
                            var resultExisting = await _dbContext.LLSLegalPrinciples.Where(x => x.PrincipleId == lLSLegalPrinciple.PrincipleId).FirstOrDefaultAsync();
                            if (resultExisting != null)
                            {
                                resultExisting.FlowStatus = lLSLegalPrinciple.FlowStatus;
                                resultExisting.ModifiedBy = lLSLegalPrinciple.ModifiedBy;
                                resultExisting.ModifiedDate = lLSLegalPrinciple.ModifiedDate;
                                _dbContext.Entry(resultExisting).State = EntityState.Modified;
                            }
                            else
                            {
                                lLSLegalPrinciple.PrincipleNumber = await _dbContext.LLSLegalPrinciples.Select(x => x.PrincipleNumber).MaxAsync() + 1;
                                await _dbContext.LLSLegalPrinciples.AddAsync(lLSLegalPrinciple);
                            }
                        }
                        else
                        {
                            lLSLegalPrinciple.PrincipleNumber = 1;
                            await _dbContext.LLSLegalPrinciples.AddAsync(lLSLegalPrinciple);
                        }

                        await _dbContext.SaveChangesAsync();



                        if (lLSLegalPrinciple.lLSLegalPrinciplesContentList.Count() != 0)
                        {
                            await SaveManyToManyPrincipleContent(lLSLegalPrinciple.lLSLegalPrinciplesContentList, lLSLegalPrinciple.PrincipleId, _dbContext);
                        }
                        if (lLSLegalPrinciple.lLSLegalPrincipleCategoryList.Count() != 0)
                        {
                            await SaveManyToManyPrincipleContentCategory(lLSLegalPrinciple.lLSLegalPrincipleCategoryList, lLSLegalPrinciple.lLSLegalPrinciplesContentList, _dbContext);
                        }
                        if (lLSLegalPrinciple.linkContents.Count() != 0)
                        {
                            await SaveManyToManyPrincipleContentSourceDocumentReference(lLSLegalPrinciple.linkContents, _dbContext);
                        }
                        await _workflowRepository.LinkEntityWithActiveWorkflow(lLSLegalPrinciple, _dbContext, (int)WorkflowSubModuleEnum.LegalPrinciples);
                        await transaction.CommitAsync();
                        return true;
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                }
            }
        }

        private async Task SaveManyToManyPrincipleContentSourceDocumentReference(List<LLSLegalPrincipleContentSourceDocumentReference> linkContents, DatabaseContext dbContext)
        {
            try
            {
                foreach (var itemAdd in linkContents)
                {
                    await dbContext.LLSLegalPrincipleContentSourceDocumentReferences.AddAsync(itemAdd);
                }
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        private async Task SaveManyToManyPrincipleContentCategory(List<LLSLegalPrincipleContentCategory> lLSLegalPrincipleCategoryList, List<LLSLegalPrincipleContent> lLSLegalPrinciplesContentList, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in lLSLegalPrinciplesContentList)
                {
                    var resultContentsCategories = await dbContext.LLSLegalPrincipleLLSLegalPrincipleCategorys.Where(x => x.PrincipleContentId == item.PrincipleContentId).ToListAsync();
                    if (resultContentsCategories.Count() != 0)
                    {
                        foreach (var itemRemove in resultContentsCategories)
                        {
                            dbContext.LLSLegalPrincipleLLSLegalPrincipleCategorys.Remove(itemRemove);
                        }
                    }
                }
                await dbContext.SaveChangesAsync();

                foreach (var itemAdd in lLSLegalPrincipleCategoryList)
                {
                    if (itemAdd.Id != 0)
                    {
                        itemAdd.Id = 0;
                    }
                    await dbContext.LLSLegalPrincipleLLSLegalPrincipleCategorys.AddAsync(itemAdd);
                }
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        private async Task SaveManyToManyPrincipleContent(List<LLSLegalPrincipleContent> lLSLegalPrinciplesContentList, Guid principleId, DatabaseContext dbContext)
        {
            try
            {
                var resultContents = await dbContext.LLSLegalPrincipleContents.Where(x => x.PrincipleId == principleId).ToListAsync();
                if (resultContents.Count() != 0)
                {
                    foreach (var item in resultContents)
                    {
                        dbContext.LLSLegalPrincipleContents.Remove(item);
                    }
                    await dbContext.SaveChangesAsync();
                }
                foreach (var item in lLSLegalPrinciplesContentList)
                {
                    await dbContext.LLSLegalPrincipleContents.AddAsync(item);
                }
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Get LLS Legal Principle Details By Using PrincipleContentId For Edit Form
        public async Task<LLSLegalPrincipleSystem> GetLLSLegalPrincipleDetailsByUsingPrincipleContentId(Guid principleContentId)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        LLSLegalPrincipleSystem lLSLegalPrincipleSystem = new LLSLegalPrincipleSystem();
                        var resultPrincipleContent = await _dbContext.LLSLegalPrincipleContents.Where(x => x.PrincipleContentId == principleContentId).FirstOrDefaultAsync();
                        if (resultPrincipleContent != null)
                        {
                            var resultPrinciple = await _dbContext.LLSLegalPrinciples.Where(x => x.PrincipleId == resultPrincipleContent.PrincipleId).FirstOrDefaultAsync();
                            if (resultPrinciple != null)
                            {
                                lLSLegalPrincipleSystem = resultPrinciple;
                                lLSLegalPrincipleSystem.lLSLegalPrinciplesContentList.Add(resultPrincipleContent);

                                var resultContentCategories = await GetLLSLegalPrincipleLegalPrincipelContentCategoryDetailsByUsingPrincipleContentId(resultPrincipleContent.PrincipleContentId, _dbContext);
                                if (resultContentCategories.Count() != 0)
                                {
                                    lLSLegalPrincipleSystem.lLSLegalPrincipleCategoryList = resultContentCategories;
                                }

                                var resultSourceList = await GetLLSLegalPrincipleContentSourceDocumentDetailByUsingPrincipleContentId(resultPrincipleContent.PrincipleContentId, _dbContext);
                                lLSLegalPrincipleSystem.linkContents = resultSourceList;
                                transaction.Commit();
                                return lLSLegalPrincipleSystem;
                            }
                        }
                        return lLSLegalPrincipleSystem;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return new LLSLegalPrincipleSystem();
                    }
                }
            }
        }

        private async Task<List<LLSLegalPrincipleContentSourceDocumentReference>> GetLLSLegalPrincipleContentSourceDocumentDetailByUsingPrincipleContentId(Guid principleContentId, DatabaseContext dbContext)
        {
            try
            {
                List<LLSLegalPrincipleContentSourceDocumentReference> lLSLegalPrincipleContentSourceDocumentReferences = new List<LLSLegalPrincipleContentSourceDocumentReference>();
                lLSLegalPrincipleContentSourceDocumentReferences = await dbContext.LLSLegalPrincipleContentSourceDocumentReferences.Where(x => x.PrincipleContentId == principleContentId).ToListAsync();

                return lLSLegalPrincipleContentSourceDocumentReferences;
            }
            catch (Exception ex)
            {
                return new List<LLSLegalPrincipleContentSourceDocumentReference>();
            }
        }

        private async Task<List<LLSLegalPrincipleContentCategory>> GetLLSLegalPrincipleLegalPrincipelContentCategoryDetailsByUsingPrincipleContentId(Guid principleContentId, DatabaseContext dbContext)
        {
            try
            {
                List<LLSLegalPrincipleContentCategory> lLSLegalPrincipleContentCategories = new List<LLSLegalPrincipleContentCategory>();
                lLSLegalPrincipleContentCategories = await dbContext.LLSLegalPrincipleLLSLegalPrincipleCategorys.Where(x => x.PrincipleContentId == principleContentId).ToListAsync();

                return lLSLegalPrincipleContentCategories;
            }
            catch (Exception ex)
            {
                return new List<LLSLegalPrincipleContentCategory>();
            }
        }
        #endregion

        #region Soft delete legal principle
        public async Task<bool> DeleteLLSLegalPrinciple(LLSLegalPrinciplesVM args)
        {

            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        LLSLegalPrincipleSystem? legalPrinciple = await _dbContext.LLSLegalPrinciples.Where(x => x.PrincipleId == args.PrincipleId).FirstOrDefaultAsync();
                        if (legalPrinciple != null)
                        {
                            legalPrinciple.DeletedBy = args.DeletedBy;
                            legalPrinciple.IsDeleted = true;
                            legalPrinciple.DeletedDate = DateTime.Now;
                            _dbContext.Entry(legalPrinciple).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();
                            return true;
                            ////For Notification
                            //legalPrinciplesVM.NotificationParameter.Type = _dbContext.legalPrincipleTypes.Where(x => x.Id == legalPrinciple.Principle_Type).Select(x => x.Name_En + "/" + x.Name_Ar).FirstOrDefault();
                            //legalPrinciplesVM.NotificationParameter.Entity = "Legal Principle";
                        }
                        return false;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        #endregion

        #region Get Legal Principle Categories Details
        public async Task<List<LLSLegalPrincipleCategoriesVM>> GetLLSLegaPrincipleCategories(bool showPublishedOnly = false)
        {
            try
            {
                string StoredProc = $"exec pLLSLegalPrincipleCategories";
                var res = await _dbContext.LLSLegalPrincipleCategoriesVM.FromSqlRaw(StoredProc).ToListAsync();

                if (res.Count == 0)
                {
                    LLSLegalPrincipleCategory defaultCategory = new LLSLegalPrincipleCategory
                    {
                        ParentId = null,
                        Name = "Thing",
                        CreatedBy = "System",
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                    };
                    await _dbContext.LLSLegalPrincipleCategorys.AddAsync(defaultCategory);
                    bool isAdded = await _dbContext.SaveChangesAsync() > 0;
                    if (isAdded)
                    {
                        await GetLLSLegaPrincipleCategories();
                    }
                }
                return await GetPrincipleContent(res, showPublishedOnly);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<LLSLegalPrincipleCategoriesVM>> GetPrincipleContent(List<LLSLegalPrincipleCategoriesVM> res, bool showPublishedOnly = false)
        {
            try
            {
                foreach (var item in res)
                {
                    var categoryIds = await _dbContext.LLSLegalPrincipleLLSLegalPrincipleCategorys.Where(X => X.CategoryId == item.CategoryId).ToListAsync();
                    if (categoryIds.Any())
                    {
                        foreach (var subItem in categoryIds)
                        {
                            var principleContents = await _dbContext.LLSLegalPrincipleContents
                                                    .Where(x => x.PrincipleContentId == subItem.PrincipleContentId && x.IsDeleted == false && x.PrincipleContent != null)
                                                    .FirstOrDefaultAsync();
                            if (principleContents != null)
                            {
                                // get main principle flow status id based on principle content id
                                if (showPublishedOnly == true)
                                {
                                    principleContents.MainPrincipleFlowStatusId = await _dbContext.LLSLegalPrinciples.Where(x => x.PrincipleId == principleContents.PrincipleId && x.FlowStatus==(int)PrincipleFlowStatusEnum.Publish && x.IsDeleted == false).Select(y => y.FlowStatus).FirstOrDefaultAsync();
                                    if (principleContents.MainPrincipleFlowStatusId == (int)PrincipleFlowStatusEnum.Publish)
                                        item.PrincipleContent.Add(principleContents);
                                }
                                else
                                {
                                    principleContents.MainPrincipleFlowStatusId = await _dbContext.LLSLegalPrinciples.Where(x => x.PrincipleId == principleContents.PrincipleId && x.IsDeleted == false).Select(y => y.FlowStatus).FirstOrDefaultAsync();
                                    item.PrincipleContent.Add(principleContents);
                                }
                            }
                        }
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<LLSLegalPrincipleCategoriesVM>> GetLLSLegaPrincipleCategoriesAdvanceSearch(LLSLegalPrincipleCategoryAdvanceSearchVm advanceSearchVm)
        {
            try
            {
                string startDate = advanceSearchVm.StartDate != null ? Convert.ToDateTime(advanceSearchVm.StartDate).ToString("yyyy/MM/dd").ToString() : null;
                string endDate = advanceSearchVm.EndDate != null ? Convert.ToDateTime(advanceSearchVm.EndDate).ToString("yyyy/MM/dd").ToString() : null;

                string StoredProc = $"exec pLLSLegalPrincipleCategoriesAdvacneSearch @PrincipleContent = N'{advanceSearchVm.PrincipleContent}' , @CategoryId='{advanceSearchVm.CategoryId}',@StartDate='{startDate}', @EndDate='{endDate}'";
                var res = await _dbContext.LLSLegalPrincipleCategoriesVM.FromSqlRaw(StoredProc).ToListAsync();
                if (res.Count() != 0)
                {
                    return await GetPrincipleContent(res);
                }
                return new List<LLSLegalPrincipleCategoriesVM>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Legal Principle Contents By CategoryId
        public async Task<List<LLSLegalPrincipleContent>> GetLLSPrincipleContents(int categoryId)
        {
            try
            {
                //string requestFrom = advanceSearchVM.PublicationFrom != null ? Convert.ToDateTime(advanceSearchVM.PublicationFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                //string requestTo = advanceSearchVM.PublicationTo != null ? Convert.ToDateTime(advanceSearchVM.PublicationTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string StoredProc = $"exec pLLSlegalPrincipleContents @CategoryId='{categoryId}'";
                var res = await _dbContext.LLSLegalPrincipleContents.FromSqlRaw(StoredProc).ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion    

        #region Get Legal Principle Contents Categories
        public async Task<List<LLSLegalPrincipleContentCategoriesVM>> GetLLSLegalPrincipleContentCategories(Guid? principleContentId)
        {
            try
            {
                string StoredProc = $"exec pLLSPrincipleContentCaegories @PrincipleContentId='{principleContentId}'";
                var res = await _dbContext.LLSLegalPrincipleContentCategoriesVM.FromSqlRaw(StoredProc).ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update legal principle category
        public async Task<bool> UpdateLegalPrincipleCategory(LLSLegalPrincipleCategory item)
        {
            try
            {
                var res = await _dbContext.LLSLegalPrincipleCategorys.Where(x => x.CategoryId == item.CategoryId).FirstOrDefaultAsync();
                if (res is not null)
                {
                    res.Name = item.Name;
                    res.ModifiedBy = item.ModifiedBy;
                    res.ModifiedBy = item.ModifiedBy;
                    _dbContext.Entry(res).State = EntityState.Modified;
                    bool isUpdated = await _dbContext.SaveChangesAsync() > 0;
                    return isUpdated;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Soft Delete legal principle category
        public async Task<bool> DeleteLegalPrincipleCategory(LLSLegalPrincipleCategory item)
        {
            try
            {
                var res = await _dbContext.LLSLegalPrincipleCategorys.Where(x => x.CategoryId == item.CategoryId).FirstOrDefaultAsync();
                if (res is not null)
                {
                    res.IsDeleted = item.IsDeleted;
                    res.DeletedBy = item.DeletedBy;
                    res.DeletedDate = item.DeletedDate;
                    _dbContext.Entry(res).State = EntityState.Modified;
                    bool isUpdated = await _dbContext.SaveChangesAsync() > 0;
                    return isUpdated;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }
        #endregion   

        #region  Link LLS Legal Principle Contents
        public async Task<bool> LinkLegalPrincipleContents(List<LLSLegalPrincipleContentSourceDocumentReference> linkContents)
        {
            try
            {
                await _dbContext.LLSLegalPrincipleContentSourceDocumentReferences.AddRangeAsync(linkContents);
                bool isAdded = await _dbContext.SaveChangesAsync() > 0;
                return isAdded;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region  Check Copy Document Exist
        public async Task<int?> CheckCopyDocumentExists(int uploadDocumentId)
        {
            try
            {
                var contentReference = await _dbContext.LLSLegalPrincipleContentSourceDocumentReferences.Where(x => x.IsMaskedJudgment == false && x.OriginalSourceDocId == uploadDocumentId).FirstOrDefaultAsync();
                if (contentReference == null)
                {
                    return 0;
                }
                return contentReference.CopySourceDocId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region  Get Legal Principle Content By Id
        public async Task<LLSLegalPrincipleContent> GetLLSLegalPrincipleContentById(Guid principleContentId)
        {
            try
            {
                var principleContent = await _dbContext.LLSLegalPrincipleContents.Where(x => x.PrincipleContentId == principleContentId).FirstOrDefaultAsync();
                return principleContent;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update legal principle
        //<!-- <History Author = 'Umer Zaman' Date='2024-05-28' Version="1.0" Branch="master">Update legal principle content</History>

        public async Task<bool> UpdateLLSLegalPrinciple(LLSLegalPrincipleSystem lLSLegalPrinciple)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (_dbContext.LLSLegalPrinciples.Any())
                        {
                            // get total contents against principle id
                            var resultPrincipleContentList = await _dbContext.LLSLegalPrincipleContents.Where(x => x.PrincipleId == lLSLegalPrinciple.PrincipleId).ToListAsync();
                            if (resultPrincipleContentList.Any())
                            {
                                // If the main principle has only one content and
                                // the user needs to delete the selected content,
                                // delete the whole main principle along with the
                                // linked source document

                                if (resultPrincipleContentList.Count() == 1)
                                {
                                    // First check that if principle content have linked with
                                    // one or multiple source document and also want to removed the link from source document
                                    // then delete whole main principle.
                                    
									if (lLSLegalPrinciple.linkContents.Any())
									{
                                        if (lLSLegalPrinciple.SourceDocumentDeletedReferenceId.Count() == lLSLegalPrinciple.linkContents.Count())
                                        {
											await SoftDeleteMainPrinciple(lLSLegalPrinciple, _dbContext);

											if (lLSLegalPrinciple.lLSLegalPrinciplesContentList.Count() != 0)
											{
												await SoftDeleteExistingPrincipleContent(lLSLegalPrinciple.lLSLegalPrinciplesContentList, lLSLegalPrinciple, _dbContext);
											}
											if (lLSLegalPrinciple.linkContents.Count() != 0)
											{
												await UpdatePrincipleContentSourceDocumentReference(lLSLegalPrinciple.SourceDocumentDeletedReferenceId, lLSLegalPrinciple.lLSLegalPrincipleCategoryList.Select(x => x.PrincipleContentId).FirstOrDefault(), lLSLegalPrinciple.linkContents, true, _dbContext);
											}
										}
										// if user changed the link from source document and also maybe update the content text.
										else
										{
											await UpdateExistingMainPrinciple(lLSLegalPrinciple, _dbContext);
											if (lLSLegalPrinciple.lLSLegalPrinciplesContentList.Count() != 0)
											{
												await UpdateExistingPrincipleContent(lLSLegalPrinciple.lLSLegalPrinciplesContentList, _dbContext);
											}
											if (lLSLegalPrinciple.linkContents.Count() != 0)
											{
												await UpdatePrincipleContentSourceDocumentReference(lLSLegalPrinciple.SourceDocumentDeletedReferenceId, lLSLegalPrinciple.lLSLegalPrincipleCategoryList.Select(x => x.PrincipleContentId).FirstOrDefault(), lLSLegalPrinciple.linkContents, false, _dbContext);
											}
										}
										
										if (lLSLegalPrinciple.lLSLegalPrincipleCategoryList.Count() != 0)
										{
											await SaveManyToManyPrincipleContentCategory(lLSLegalPrinciple.lLSLegalPrincipleCategoryList, lLSLegalPrinciple.lLSLegalPrinciplesContentList, _dbContext);
										}
										
									}

									// if user changed the link from source document and also maybe update the content text.
							        //  else if (lLSLegalPrinciple.SourceDocumentDeletedReferenceId.Count() == 0 && lLSLegalPrinciple.linkContents.Any() && lLSLegalPrinciple.linkContents.Count() == 1)
							        //  {
									//	await UpdateExistingMainPrinciple(lLSLegalPrinciple, _dbContext);
									//	if (lLSLegalPrinciple.lLSLegalPrinciplesContentList.Count() != 0)
									//	{
									//		await UpdateExistingPrincipleContent(lLSLegalPrinciple.lLSLegalPrinciplesContentList, _dbContext);
									//	}
									//	if (lLSLegalPrinciple.lLSLegalPrincipleCategoryList.Count() != 0)
									//	{
									//		await SaveManyToManyPrincipleContentCategory(lLSLegalPrinciple.lLSLegalPrincipleCategoryList, lLSLegalPrinciple.lLSLegalPrinciplesContentList, _dbContext);
									//	}
									//	if (lLSLegalPrinciple.linkContents.Count() != 0)
									//	{
									//		await UpdatePrincipleContentSourceDocumentReference(lLSLegalPrinciple.SourceDocumentDeletedReferenceId, lLSLegalPrinciple.lLSLegalPrincipleCategoryList.Select(x => x.PrincipleContentId).FirstOrDefault(), lLSLegalPrinciple.linkContents, false, _dbContext);
									//	}
									//}

									// If the principle content are linked to multiple
									// source document and the user need to delete
									// the selected content, keep the content and
									// just remove the linkage with the source
									// document.

									//else if (lLSLegalPrinciple.linkContents.Any() && lLSLegalPrinciple.linkContents.Count() > 1)
                                    // {
									//	if (lLSLegalPrinciple.lLSLegalPrinciplesContentList.Count() != 0)
									//	{
									//		await UpdateExistingPrincipleContent(lLSLegalPrinciple.lLSLegalPrinciplesContentList, _dbContext);
									//	}
									//	if (lLSLegalPrinciple.lLSLegalPrincipleCategoryList.Count() != 0)
                                    //  {
                                    //     await SaveManyToManyPrincipleContentCategory(lLSLegalPrinciple.lLSLegalPrincipleCategoryList, lLSLegalPrinciple.lLSLegalPrinciplesContentList, _dbContext);
                                    //  }
                                    //  if (lLSLegalPrinciple.linkContents.Count() != 0)
                                    //  {
                                    //      await UpdatePrincipleContentSourceDocumentReference(lLSLegalPrinciple.SourceDocumentDeletedReferenceId, lLSLegalPrinciple.lLSLegalPrincipleCategoryList.Select(x => x.PrincipleContentId).FirstOrDefault(), lLSLegalPrinciple.linkContents, false, _dbContext);
                                    //  }
                                    // }
                                }

                                // If the main principle has other principle
                                // content and the user needs to delete the
                                // selected content, only that content will be
                                // removed from the main principle and remove
                                // the linkage with source document.

                                else if (resultPrincipleContentList.Count() > 1)
                                {
                                    if (lLSLegalPrinciple.linkContents.Any())
                                    {
										await UpdateExistingMainPrinciple(lLSLegalPrinciple, _dbContext);
										if (lLSLegalPrinciple.SourceDocumentDeletedReferenceId.Count() == lLSLegalPrinciple.linkContents.Count())
                                        {
											if (lLSLegalPrinciple.lLSLegalPrinciplesContentList.Count() != 0)
											{
												await SoftDeleteExistingPrincipleContent(lLSLegalPrinciple.lLSLegalPrinciplesContentList, lLSLegalPrinciple, _dbContext);
											}
											if (lLSLegalPrinciple.linkContents.Count() != 0)
											{
												await UpdatePrincipleContentSourceDocumentReference(lLSLegalPrinciple.SourceDocumentDeletedReferenceId, lLSLegalPrinciple.lLSLegalPrincipleCategoryList.Select(x => x.PrincipleContentId).FirstOrDefault(), lLSLegalPrinciple.linkContents, true, _dbContext);
											}
										}
                                        // if Source Document delete list is not equal with linked source document
                                        else
                                        {
											if (lLSLegalPrinciple.lLSLegalPrinciplesContentList.Count() != 0)
											{
												await UpdateExistingPrincipleContent(lLSLegalPrinciple.lLSLegalPrinciplesContentList, _dbContext);
											}
											if (lLSLegalPrinciple.linkContents.Count() != 0)
											{
												await UpdatePrincipleContentSourceDocumentReference(lLSLegalPrinciple.SourceDocumentDeletedReferenceId, lLSLegalPrinciple.lLSLegalPrincipleCategoryList.Select(x => x.PrincipleContentId).FirstOrDefault(), lLSLegalPrinciple.linkContents, false, _dbContext);
											}
										}
                                        if (lLSLegalPrinciple.lLSLegalPrincipleCategoryList.Count() != 0)
                                        {
                                            await SaveManyToManyPrincipleContentCategory(lLSLegalPrinciple.lLSLegalPrincipleCategoryList, lLSLegalPrinciple.lLSLegalPrinciplesContentList, _dbContext);
                                        }
                                        
                                    }
                                    // If the principle content are linked to multiple
                                    // source document and the user need to delete
                                    // the selected content, keep the content and
                                    // just remove the linkage with the source
                                    // document.

                                    //else if (lLSLegalPrinciple.linkContents.Any() && lLSLegalPrinciple.linkContents.Count() > 1)
                                    //{
                                    //    if (lLSLegalPrinciple.lLSLegalPrincipleCategoryList.Count() != 0)
                                    //    {
                                    //        await SaveManyToManyPrincipleContentCategory(lLSLegalPrinciple.lLSLegalPrincipleCategoryList, lLSLegalPrinciple.lLSLegalPrinciplesContentList, _dbContext);
                                    //    }
                                    //    if (lLSLegalPrinciple.linkContents.Count() != 0)
                                    //    {
                                    //        await UpdatePrincipleContentSourceDocumentReference(lLSLegalPrinciple.SourceDocumentDeletedReferenceId, lLSLegalPrinciple.lLSLegalPrincipleCategoryList.Select(x => x.PrincipleContentId).FirstOrDefault(), lLSLegalPrinciple.linkContents, false, _dbContext);
                                    //    }
                                    //}
                                }
                            }
                        }
                        await UpdateLinkedWorkflowInstance(lLSLegalPrinciple, _dbContext);
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

		private async Task UpdateExistingMainPrinciple(LLSLegalPrincipleSystem lLSLegalPrinciple, DatabaseContext dbContext)
		{
            try
            {
				var resultExisting = await dbContext.LLSLegalPrinciples.Where(x => x.PrincipleId == lLSLegalPrinciple.PrincipleId).FirstOrDefaultAsync();
				if (resultExisting != null)
				{
					resultExisting.FlowStatus = lLSLegalPrinciple.FlowStatus;
					resultExisting.ModifiedBy = lLSLegalPrinciple.ModifiedBy;
					resultExisting.ModifiedDate = lLSLegalPrinciple.ModifiedDate;
					dbContext.Entry(resultExisting).State = EntityState.Modified;
					await dbContext.SaveChangesAsync();
				}
			}
            catch (Exception ex)
            {
				throw ex;
			}
		}

		private async Task UpdateExistingPrincipleContent(List<LLSLegalPrincipleContent> lLSLegalPrinciplesContentList, DatabaseContext dbContext)
		{
            try
            {
                if (lLSLegalPrinciplesContentList.Any())
                {
                    foreach (var item in lLSLegalPrinciplesContentList)
                    {
                        var existingContent = await dbContext.LLSLegalPrincipleContents.Where(x => x.PrincipleContentId == item.PrincipleContentId).FirstOrDefaultAsync();
                        if (existingContent != null)
                        {
                            existingContent.PrincipleContent = item.PrincipleContent;
                            existingContent.StartDate = item.StartDate;
                            existingContent.EndDate = item.EndDate;
							existingContent.ModifiedBy = item.ModifiedBy;
							existingContent.ModifiedDate = item.ModifiedDate;
							dbContext.Entry(existingContent).State = EntityState.Modified;
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

		private async Task UpdateLinkedWorkflowInstance(LLSLegalPrincipleSystem lLSLegalPrinciple, DatabaseContext dbContext)
        {
            try
            {
                string StoredProc = $"exec pWorkflowSelAdvanceSearch @statusId = '{(int)WorkflowStatusEnum.Active}', @submoduleId='{(int)WorkflowSubModuleEnum.LegalPrinciples}'";
                var activeWorkflow = await dbContext.WorkflowVM.FromSqlRaw(StoredProc).ToListAsync();
                if (activeWorkflow?.Count() > 0)
                {
                    WorkflowActivity firstActivity = await dbContext.WorkflowActivity.Where(a => a.WorkflowId == (int)activeWorkflow.FirstOrDefault().WorkflowId).OrderBy(a => a.SequenceNumber).FirstOrDefaultAsync();
                    WorkflowInstance workflowInstance = await dbContext.WorkflowInstance.Where(w => w.ReferenceId == lLSLegalPrinciple.PrincipleId).FirstOrDefaultAsync();
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
                        workflowInstance = new WorkflowInstance { ReferenceId = lLSLegalPrinciple.PrincipleId, StatusId = (int)WorkflowInstanceStatusEnum.InProgress, WorkflowId = (int)activeWorkflow.FirstOrDefault().WorkflowId, WorkflowActivityId = firstActivity.WorkflowActivityId, IsSlaExecuted = false };
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
        private async Task UpdatePrincipleContentSourceDocumentReference(List<int> sourceDocumentDeletedReferenceId, Guid principleContentId, List<LLSLegalPrincipleContentSourceDocumentReference> linkContents, bool IsDeleteAll, DatabaseContext dbContext)
        {
            try
            {
                // first delete source document by using list
                if (sourceDocumentDeletedReferenceId.Any())
                {
					foreach (var item in sourceDocumentDeletedReferenceId)
					{
						var resultReference = await dbContext.LLSLegalPrincipleContentSourceDocumentReferences.Where(x => x.ReferenceId == item).FirstOrDefaultAsync();
						if (resultReference != null)
						{
							resultReference.IsDeleted = true;
							dbContext.Entry(resultReference).State = EntityState.Modified;
							//dbContext.LLSLegalPrincipleContentSourceDocumentReferences.Remove(resultReference);
						}
					}
					await dbContext.SaveChangesAsync();
				}
				// delete all existing source documents records against principle content id.

				//var resultExistingAll = await dbContext.LLSLegalPrincipleContentSourceDocumentReferences.Where(x => x.PrincipleContentId == principleContentId).ToListAsync();
    //            if (resultExistingAll.Any())
    //            {
    //                dbContext.LLSLegalPrincipleContentSourceDocumentReferences.RemoveRange(resultExistingAll);
    //                await dbContext.SaveChangesAsync();
    //            }

                // now add new records
                if (linkContents.Any())
                {
					foreach (var item in linkContents)
					{
						if (IsDeleteAll) // if content linked with one source documentS
						{
							item.IsDeleted = true;
						}
						else
						{
							item.IsDeleted = false;
						}
						if (item.ReferenceId != 0)
						{
							var resultReferences = await dbContext.LLSLegalPrincipleContentSourceDocumentReferences.Where(x => x.ReferenceId == item.ReferenceId).FirstOrDefaultAsync();
							if (resultReferences != null)
							{
								resultReferences.PrincipleContentId = item.PrincipleContentId;
								resultReferences.PageNumber = item.PageNumber;
								resultReferences.OriginalSourceDocId = item.OriginalSourceDocId;
								resultReferences.CopySourceDocId = item.CopySourceDocId;
								resultReferences.IsMaskedJudgment = item.IsMaskedJudgment;
								resultReferences.IsDeleted = item.IsDeleted;
								dbContext.Entry(resultReferences).State = EntityState.Modified;
							}
							else
							{
								item.ReferenceId = 0;
								await dbContext.LLSLegalPrincipleContentSourceDocumentReferences.AddAsync(item);
							}
						}
					}
					await dbContext.SaveChangesAsync();
				}
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        private async Task SoftDeleteExistingPrincipleContent(List<LLSLegalPrincipleContent> lLSLegalPrinciplesContentList, LLSLegalPrincipleSystem lLSLegalPrinciple, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in lLSLegalPrinciplesContentList)
                {
                    var resultExistingContent = await dbContext.LLSLegalPrincipleContents.Where(x => x.PrincipleId == item.PrincipleId && x.PrincipleContentId == item.PrincipleContentId).FirstOrDefaultAsync();
                    if (resultExistingContent != null)
                    {
                        resultExistingContent.PrincipleContent = item.PrincipleContent;
                        resultExistingContent.StartDate = item.StartDate;
                        resultExistingContent.EndDate = item.EndDate;
                        resultExistingContent.DeletedBy = lLSLegalPrinciple.DeletedBy;
                        resultExistingContent.DeletedDate = lLSLegalPrinciple.DeletedDate;
                        resultExistingContent.IsDeleted = lLSLegalPrinciple.IsDeleted;
                        dbContext.Entry(resultExistingContent).State = EntityState.Modified;
                        await dbContext.SaveChangesAsync();
                    }
                }

            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        private async Task SoftDeleteMainPrinciple(LLSLegalPrincipleSystem lLSLegalPrinciple, DatabaseContext dbContext)
        {
            try
            {
                var resultExisting = await dbContext.LLSLegalPrinciples.Where(x => x.PrincipleId == lLSLegalPrinciple.PrincipleId).FirstOrDefaultAsync();
                if (resultExisting != null)
                {
                    resultExisting.FlowStatus = lLSLegalPrinciple.FlowStatus;
                    resultExisting.DeletedBy = lLSLegalPrinciple.DeletedBy;
                    resultExisting.DeletedDate = lLSLegalPrinciple.DeletedDate;
                    resultExisting.IsDeleted = lLSLegalPrinciple.IsDeleted;
                    dbContext.Entry(resultExisting).State = EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region Legal Principle
        public async Task<List<LLSLegalPrinciplesReviewVM>> GetLegalPrinciples(LLSLegalPrincipleAdvanceSearchVM search)
        {
            try
            {
                List<LLSLegalPrinciplesReviewVM> lLSLegalPrinciplesReviewVMs = new List<LLSLegalPrinciplesReviewVM>();
                string FromDate = search.FromDate != null ? Convert.ToDateTime(search.FromDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string ToDate = search.ToDate != null ? Convert.ToDateTime(search.ToDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string StoredProc = $"exec pllsLegalPrincipleList @IsPublishUnPublish='{search.IsPublishUnPublish}', @flowStatusId='{search.FlowStatusId}', @userId='{search.UserId}', @FromDate='{FromDate}', @ToDate='{FromDate}'" +
                                    $",@PageNumber ='{search.PageNumber}',@PageSize ='{search.PageSize}'";
                lLSLegalPrinciplesReviewVMs = await _dbContext.LLSLegalPrinciplesReviewVMs.FromSqlRaw(StoredProc).ToListAsync();
                return lLSLegalPrinciplesReviewVMs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Legal Principle detail by Priciple Id
        public async Task<LLSLegalPrinciplesReviewVM> GetLegalPrincipleDetailById(Guid principleId)
        {
            try
            {
                string StoredProc = $"exec pllsLegalPrincipleList @PrincipleId = '{principleId}' ";
                var res = await _dbContext.LLSLegalPrinciplesReviewVMs.FromSqlRaw(StoredProc).ToListAsync();
                return res.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update Legal Principle Decision 
        public async Task UpdateLegalPrincipleDecision(LLSLegalPrincipleDecisionVM legalPrincipleDecisionVM)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            if (legalPrincipleDecisionVM.FlowStatusId > (int)PrincipleFlowStatusEnum.Reject && legalPrincipleDecisionVM.FlowStatusId < (int)PrincipleFlowStatusEnum.Publish)
                            {
                                legalPrincipleDecisionVM.FlowStatusId = (int)PrincipleFlowStatusEnum.NeedModification;
                            }
                            if (legalPrincipleDecisionVM.FlowStatusId == (int)PrincipleFlowStatusEnum.Unpublished)
                            {
                                legalPrincipleDecisionVM.FlowStatusId = (int)PrincipleFlowStatusEnum.Unpublished;
                            }
                            LLSLegalPrincipleSystem? legalPrincipleResult = await _dbContext.LLSLegalPrinciples.Where(x => x.PrincipleId == legalPrincipleDecisionVM.PrincipleId).FirstOrDefaultAsync();
                            if (legalPrincipleResult != null)
                            {
                                legalPrincipleResult.FlowStatus = (int)legalPrincipleDecisionVM.FlowStatusId;
                                legalPrincipleResult.Principle_Comment = legalPrincipleDecisionVM.Principle_Comment;
                                //legalPrincipleResult.ModifiedDate = DateTime.Now;
                                //legalPrincipleResult.ModifiedBy = legalPrincipleDecisionVM.Createdby;
                                //_dbContext.Update(legalPrinciple);
                                _dbContext.Entry(legalPrincipleResult).State = EntityState.Modified;
                                await _dbContext.SaveChangesAsync();
                                await InsertCommentsBylegalPrinciple(legalPrincipleDecisionVM, _dbContext);
                                   
                            }
                            legalPrincipleDecisionVM.NotificationParameter.PrincipleNumber = legalPrincipleDecisionVM.PrincipleNumber.ToString();
                            legalPrincipleDecisionVM.NotificationParameter.Type = _dbContext.LegalPrincipleFlowStatuses.Where(x => x.Id == legalPrincipleResult.FlowStatus).Select(x => x.Name_En + "/" + x.Name_Ar).FirstOrDefault();
                            legalPrincipleDecisionVM.NotificationParameter.Status = _dbContext.LegalPrincipleFlowStatuses.Where(x => x.Id == legalPrincipleResult.FlowStatus).Select(x => x.Name_En + "/" + x.Name_Ar).FirstOrDefault();
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
        private async Task InsertCommentsBylegalPrinciple(LLSLegalPrincipleDecisionVM legalPrinciple, DatabaseContext dbContext)
        {
            try
            {
                LLSLegalPrincipleComment commentobj = new LLSLegalPrincipleComment();

                if (legalPrinciple.FlowStatusId != (int)PrincipleFlowStatusEnum.Publish)
                {
                    commentobj.CommentId = Guid.NewGuid();
                    commentobj.PrincipleId = legalPrinciple.PrincipleId;
                    commentobj.Comment = legalPrinciple.Principle_Comment;
                    commentobj.Status = legalPrinciple.FlowStatusId.ToString();
                    commentobj.CreatedDate = DateTime.Now;
                    commentobj.Createdby = legalPrinciple.CreatedBy;
                    await dbContext.LLSLegalPrincipleComments.AddAsync(commentobj);
                    await dbContext.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Legal Principle Details By Using principleId For Edit Form
        //<!-- <History Author = 'Muhammad Zaeem' Date='2022-05-26' Version="1.0" Branch="master">get legal principle by Id for edit</History>

        public async Task<LLSLegalPrincipleSystem> GetLegalPrincipleDetailsByUsingPrincipleId(Guid principleId)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        LLSLegalPrincipleSystem lLSLegalPrincipleSystem = new LLSLegalPrincipleSystem();
                        var resultPrinciple = await _dbContext.LLSLegalPrinciples.Where(x => x.PrincipleId == principleId).FirstOrDefaultAsync();
                        if (resultPrinciple != null)
                        {
                            //lLSLegalPrincipleSystem.PrincipleId = resultPrinciple.PrincipleId;
                            //lLSLegalPrincipleSystem.PrincipleNumber = resultPrinciple.PrincipleNumber;
                            //lLSLegalPrincipleSystem.OriginalSourceDocumentId = resultPrinciple.OriginalSourceDocumentId;
                            //lLSLegalPrincipleSystem.UserId = resultPrinciple.UserId;
                            //lLSLegalPrincipleSystem.RoleId = resultPrinciple.RoleId;
                            //lLSLegalPrincipleSystem.Principle_Comment = resultPrinciple.Principle_Comment;
                            lLSLegalPrincipleSystem = resultPrinciple;

                            var resultPrincipleContent = await _dbContext.LLSLegalPrincipleContents.Where(x => x.PrincipleId == principleId).ToListAsync();
                            if (resultPrincipleContent.Count() != 0)
                            {
                                lLSLegalPrincipleSystem.lLSLegalPrinciplesContentList = resultPrincipleContent;

                                var resultContentCategories = await GetAllLegalPrincipelContentCategoryDetailsByUsingPrincipleContentId(resultPrincipleContent, _dbContext);
                                if (resultContentCategories.Count() != 0)
                                {
                                    lLSLegalPrincipleSystem.lLSLegalPrincipleCategoryList = resultContentCategories;
                                }

                                var resultSourceList = await GetAllLegalPrincipleContentSourceDocumentDetailByUsingPrincipleContentId(resultPrincipleContent, _dbContext);
                                lLSLegalPrincipleSystem.linkContents = resultSourceList;
                            }
                            transaction.Commit();
                        }
                        return lLSLegalPrincipleSystem;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return new LLSLegalPrincipleSystem();
                    }
                }
            }
        }

        private async Task<List<LLSLegalPrincipleContentSourceDocumentReference>> GetAllLegalPrincipleContentSourceDocumentDetailByUsingPrincipleContentId(List<LLSLegalPrincipleContent> resultPrincipleContent, DatabaseContext dbContext)
        {
            List<LLSLegalPrincipleContentSourceDocumentReference> lLsLegalPrincipleContentSourceDocumentReference = new List<LLSLegalPrincipleContentSourceDocumentReference>();
            foreach (var item in lLsLegalPrincipleContentSourceDocumentReference)
            {
                var resultContent = await dbContext.LLSLegalPrincipleContentSourceDocumentReferences.Where(x => x.PrincipleContentId == item.PrincipleContentId).ToListAsync();
                if (resultContent.Count() != 0)
                {
                    foreach (var itemContent in resultContent)
                    {
                        lLsLegalPrincipleContentSourceDocumentReference.Add(itemContent);
                    }
                }
            }
            return lLsLegalPrincipleContentSourceDocumentReference;
        }

        private async Task<List<LLSLegalPrincipleContentCategory>> GetAllLegalPrincipelContentCategoryDetailsByUsingPrincipleContentId(List<LLSLegalPrincipleContent> resultPrincipleContent, DatabaseContext dbContext)
        {
            try
            {
                List<LLSLegalPrincipleContentCategory> lLSLegalPrincipleContentCategories = new List<LLSLegalPrincipleContentCategory>();
                foreach (var item in resultPrincipleContent)
                {
                    var resultCategory = await dbContext.LLSLegalPrincipleLLSLegalPrincipleCategorys.Where(x => x.PrincipleContentId == item.PrincipleContentId).ToListAsync();
                    if (resultCategory.Count() != 0)
                    {
                        foreach (var itemCategory in resultCategory)
                        {
                            lLSLegalPrincipleContentCategories.Add(itemCategory);
                        }
                    }
                }
                return lLSLegalPrincipleContentCategories;
            }
            catch (Exception ex)
            {
                return new List<LLSLegalPrincipleContentCategory>();
            }
        }
        #endregion

        #region GetLLSLegalPrincipleContentDetailsByUsingPrincipleId
        public async Task<List<LLSLegalPrinciplesContentVM>> GetLLSLegalPrincipleContentDetailsByUsingPrincipleId(Guid principleId)
        {
            try
            {
                string StoredProc = $"exec pLLSLegalPrincipleContentListByPrincipleId @PrincipleId = '{principleId}' ";
                var res = await _dbContext.LLSLegalPrinciplesContentVMs.FromSqlRaw(StoredProc).ToListAsync();
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get principle flow status details
        //<!-- <History Author = 'Muhammad Zaeem' Date='2022-05-26' Version="1.0" Branch="master">get legal principle status detail</History>
        public async Task<List<LegalPrincipleFlowStatus>> GetPrincipleFlowStatusDetails()
        {
            try
            {
                var task = await _dbContext.LegalPrincipleFlowStatuses.OrderBy(x => x.Id).ToListAsync();
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
        #region Mobile Application
        public async Task<MobileAppLegalPrincipleContentDetailVM> GetLLSLegalPrincipleContentByIdForMobileApp(Guid? principleContentId)
        {
            try
            {
                string StoredProc = $"exec pMobileAppLegalPrincipleContentDetailById @principleContentId='{principleContentId}'";
                var response = (await _dbContext.MobileAppLegalPrincipleContentDetailVM.FromSqlRaw(StoredProc).ToListAsync()).FirstOrDefault();
                if (response != null)
                {
                    string StoredProcDms = $"exec pMobileAppLegalPrincipleUploadedDocuments @PrincipleContentId = N'{response.PrincipleContentId}'";
                    response.Attachments = await _dbDmsContext.MobileAppUploadDocumentsVM.FromSqlRaw(StoredProcDms).ToListAsync();
                    if (response.Attachments.Any())
                    {
                        foreach (var item in response.Attachments)
                        {
                            var physicalPath = Path.Combine(_Config.GetValue<string>("dms_file_path") + item.FileUrl).Replace(@"\\", @"\");
#if !DEBUG
						physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
#endif
                            item.FileUrl = physicalPath;
                        }
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Legal Principle Dms
        //public async Task<List<LegalPrinciplesDmsVM>> GetLegalPrincipleDms()
        //{
        //    try
        //    {
        //        List<LegalPrinciplesDmsVM> _legalPrinciplesDmsVMs = new List<LegalPrinciplesDmsVM>();
        //        string StoredProc = $"exec pPrincipleListFilteredDms @LookupsTableId='{(int)LookupsTablesEnum.LEGAL_PRINCIPLE_TYPE}'";
        //        _legalPrinciplesDmsVMs = await _dbContext.LegalPrinciplesDmsVMs.FromSqlRaw(StoredProc).ToListAsync();
        //        if (_legalPrinciplesDmsVMs.Count() != 0)
        //        {
        //            return _legalPrinciplesDmsVMs;
        //        }
        //        return new List<LegalPrinciplesDmsVM>();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        #endregion
    }

}
