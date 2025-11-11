using AutoMapper;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Enums;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.Lms;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.LiteratureEnum;
using static FATWA_DOMAIN.Enums.UserEnum;

namespace FATWA_INFRASTRUCTURE.Repository
{
    //<History Author = 'Aqeel Altaf' Date='2022-03-23' Version="1.0" Branch="master"> Create structure and repo for literature</History>
    //<History Author = 'Hassan Abbas' Date='2022-03-30' Version="2.0" Branch="master"> Changes made according to model, added properties of purchase, author, barcode in the model</History>
    //<History Author = 'Hassan Abbas' Date='2022-04-04' Version="2.1" Branch="master"> Added a new function for geting author details based on ID, and count of books author is associated with, soft delete a book</History>
    public class LmsLiteraturesRepository : ILmsLiterature
    {
        #region variable
        private readonly DatabaseContext _DbContext;
        private readonly DmsDbContext _dbDmsContext;
        private readonly IConfiguration _Config;
        private List<LmsLiterature> _LmsLiteratures;
        private List<LiteratureDetailVM> _LiteratureDetailsVM;
        private List<LmsViewableLiteratureVM> _ViewableLiteratureVM;
        private List<LiteratureAllDetailsVM> _LiteratureAllDetailsVM;
        private List<LiteratureAllAuthorsVM> _LiteratureAllAuthorsVM;
        private List<BorrowDetailVM> _LiteratureBorrowDetail;
        private List<LmsLiteratureAuthor> _LmsLiteraturesAuthor = new List<LmsLiteratureAuthor>();
        private LmsLiteratureBarcode _lmsLiteratureBarcode;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor
        public LmsLiteraturesRepository(DatabaseContext dbContext, IServiceScopeFactory serviceScopeFactory, DmsDbContext dmsDbContext, IConfiguration config, IMapper mapper)
        {
            _DbContext = dbContext;
            _serviceScopeFactory = serviceScopeFactory;
            _dbDmsContext = dmsDbContext;
            _Config = config;
            _mapper = mapper;
        }

        #endregion

        #region Get LMS Letrature  by Id 
        public async Task<LiteratureAllDetailsVM> GetLMSLiteratureDetailById(int LiteratureId)
        {

            try
            {

                LiteratureAllDetailsVM literatureFirstResult = new LiteratureAllDetailsVM();
                if (_LiteratureAllDetailsVM == null)
                {
                    string StoredProc = $"exec pLiteratureDetailById @LiteratureId = N'{LiteratureId}'";
                    _LiteratureAllDetailsVM = await _DbContext.LiteratureAllDetailsVMs.FromSqlRaw(StoredProc).ToListAsync();
                    if (_LiteratureAllDetailsVM.Count() != 0)
                    {
                        literatureFirstResult = _LiteratureAllDetailsVM.FirstOrDefault();
                        var result = await _DbContext.LmsLiteratureBarcodes.Where(x => x.LiteratureId == literatureFirstResult.LiteratureId && x.IsDeleted != true && x.Active != false).ToListAsync();
                        if (result.Count() != 0)
                        {
                            literatureFirstResult.literatureBarCodeList = result;
                        }
                        string StoredProcDms = $"exec pMobileAppUploadDocumentsList @LiteratureId = N'{LiteratureId}'";
                        literatureFirstResult.Attachments = await _dbDmsContext.MobileAppUploadDocumentsVM.FromSqlRaw(StoredProcDms).ToListAsync();
                        if (literatureFirstResult.Attachments.Any(x => x.AttachmentTypeId == (int)AttachmentTypeEnum.BookDigitalCopy))
                        {
                            foreach (var item in literatureFirstResult.Attachments)
                            {
                                var physicalPath = Path.Combine(_Config.GetValue<string>("dms_file_path") + item.FileUrl).Replace(@"\\", @"\");
#if !DEBUG
						physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
#endif
                                item.FileUrl = physicalPath;
                            }
                        }
                        else
                        {
                            literatureFirstResult.Attachments = new List<MobileAppUploadDocumentsVM>();
                        }
                    }

                    string StoredProcTags = $"exec pLiteratureTagsListById @literatureId = '{LiteratureId}'";
                    literatureFirstResult.LiteratureTags = await _DbContext.LmsLiteratureDetailLiteratureTagVM.FromSqlRaw(StoredProcTags).ToListAsync();
                }
                return literatureFirstResult;



            }
            catch (Exception ex)
            {



                throw new Exception(ex.Message);
            }
        }


        #endregion

        #region Get LMS Letrature Authors  by Id 
        public async Task<List<LiteratureAllAuthorsVM>> GetLMSLiteratureAuthorsById(int LiteratureId)
        {
            try
            {
                if (_LiteratureAllAuthorsVM == null)
                {
                    string StoredProc = $"exec pLiteratureAuthorlById @LiteratureId = N'{LiteratureId}'";
                    _LiteratureAllAuthorsVM = await _DbContext.LiteratureAllAuthorsVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _LiteratureAllAuthorsVM;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get LMS Letrature Borrow Detail  by Id 
        public async Task<List<BorrowDetailVM>> GetBorrowDetailById(int LiteratureId, string? UserId, string RoleName)
        {
            try
            {

                string StoredProc = "";


                if (_LiteratureBorrowDetail == null)
                {
                    //Conditions to fetch all records for Super Admin else for loggedIn user 
                    //string storedProc = "exec pLiteratureBorrowDetail ";

                    if (UserId != null && (RoleName == Roles.FatwaAdmin.GetDisplayName() || RoleName == Roles.SuperAdmin.GetDisplayName() || RoleName == Roles.LMSAdmin.GetDisplayName()))
                    {
                        StoredProc = $"exec pLiteratureBorrowDetail @UserId = '', @LiteratureId = '{LiteratureId}'";

                    }
                    else
                    {
                        StoredProc = $"exec pLiteratureBorrowDetail @UserId = N'{UserId}', @LiteratureId = '{LiteratureId}'";
                    }
                    _LiteratureBorrowDetail = await _DbContext.LmsLiteratureBorrowDetailsVM.FromSqlRaw(StoredProc).ToListAsync();
                }
                if (_LiteratureBorrowDetail.Count() != 0)
                {
                    return _LiteratureBorrowDetail;
                }
                else
                {
                    return new List<BorrowDetailVM>();
                }

            }






            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Getliterature and other dependent tables
        public async Task<List<LiteratureDetailVM>> GetLmsLiteratures()
        {
            try
            {
                if (_LiteratureDetailsVM == null)
                {
                    string StoredProc = "exec pLiteratureSelAll ";
                    _LiteratureDetailsVM = await _DbContext.LmsLiteratureDetailsVM.FromSqlRaw(StoredProc).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _LiteratureDetailsVM;
        }

        public async Task<List<LiteratureDetailVM>> GetLmsLiteraturesAdvanceSearch(LiteratureAdvancedSearchVM advancedSearch)
        {
            try
            {
                string purchaseDateKeyword = advancedSearch.PurchaseDateKeyword != null ? Convert.ToDateTime(advancedSearch.PurchaseDateKeyword).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string fromDate = advancedSearch.FromDate != null ? Convert.ToDateTime(advancedSearch.FromDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string toDate = advancedSearch.ToDate != null ? Convert.ToDateTime(advancedSearch.ToDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string StoredProc = "";
                switch (advancedSearch.EnumSearchValue)
                {
                    case LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum.Book_Name:
                        {
                            StoredProc = $"exec pLiteratureListFiltered @bookName = N'{advancedSearch.KeywordsType}', @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}" +
                                         $",@PageNumber ='{advancedSearch.PageNumber}',@PageSize ='{advancedSearch.PageSize}'";
                            break;
                        }
                    case LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum.Book_Index:
                        {
                            StoredProc = $"exec pLiteratureListFiltered @indexId = '{advancedSearch.GenericsIntergerKeyword}', @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}" +
                                         $",@PageNumber ='{advancedSearch.PageNumber}',@PageSize ='{advancedSearch.PageSize}'";
                            break;
                        }

                    case LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum.Barcode:
                        {
                            StoredProc = $"exec pLiteratureListFiltered @barcode = '{advancedSearch.KeywordsType}', @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}" +
                                         $",@PageNumber ='{advancedSearch.PageNumber}',@PageSize ='{advancedSearch.PageSize}'";
                            break;
                        }

                    case LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum.Sticker:
                        {
                            StoredProc = $"exec pLiteratureListFiltered @character82 = '{advancedSearch.KeywordsType}', @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}" +
                                         $",@PageNumber ='{advancedSearch.PageNumber}',@PageSize ='{advancedSearch.PageSize}'";
                            break;
                        }

                    case LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum.Author_Name:
                        {

                            StoredProc = $"exec pLiteratureListFiltered @authorId = '{advancedSearch.GenericsIntergerKeyword}', @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}" +
                                         $",@PageNumber ='{advancedSearch.PageNumber}',@PageSize ='{advancedSearch.PageSize}'";
                            break;
                        }

                    default:
                        if (fromDate == null && toDate == null && advancedSearch.ClassificationId != 0)
                        {
                            StoredProc = $"exec pLiteratureListFiltered @classificationId = {advancedSearch.ClassificationId} ,@PageNumber ='{advancedSearch.PageNumber}',@PageSize ='{advancedSearch.PageSize}'";
                            break;
                        }
                        else if (fromDate != null && toDate != null)
                        {
                            StoredProc = $"exec pLiteratureListFiltered @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}" +
                                         $",@PageNumber ='{advancedSearch.PageNumber}',@PageSize ='{advancedSearch.PageSize}'";
                            break;
                        }
                        StoredProc = $"exec pLiteratureListFiltered @PageNumber ='{advancedSearch.PageNumber}',@PageSize ='{advancedSearch.PageSize}'";
                        break;
                }
                if (_LiteratureDetailsVM == null)
                {
                    _LiteratureDetailsVM = await _DbContext.LmsLiteratureDetailsVM.FromSqlRaw(StoredProc).ToListAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _LiteratureDetailsVM;
        }

        public async Task<List<LiteratureListMobileAppVM>> GetLmsLiteraturesForMobileApp(LiteratureAdvancedSearchVM advancedSearch)
        {
            try
            {
                string purchaseDateKeyword = advancedSearch.PurchaseDateKeyword != null ? Convert.ToDateTime(advancedSearch.PurchaseDateKeyword).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string fromDate = advancedSearch.FromDate != null ? Convert.ToDateTime(advancedSearch.FromDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string toDate = advancedSearch.ToDate != null ? Convert.ToDateTime(advancedSearch.ToDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string StoredProc = "";
                switch (advancedSearch.EnumSearchValue)
                {
                    case LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum.Book_Name:
                        {
                            StoredProc = $"exec pLiteratureListFiltered @bookName = N'{advancedSearch.KeywordsType}', @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}";
                            break;
                        }
                    case LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum.Book_Index:
                        {
                            StoredProc = $"exec pLiteratureListFiltered @indexId = '{advancedSearch.GenericsIntergerKeyword}', @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}";
                            break;
                        }

                    case LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum.Barcode:
                        {
                            StoredProc = $"exec pLiteratureListFiltered @barcode = '{advancedSearch.KeywordsType}', @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}";
                            break;
                        }

                    case LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum.Sticker:
                        {
                            StoredProc = $"exec pLiteratureListFiltered @character82 = '{advancedSearch.KeywordsType}', @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}";
                            break;
                        }

                    case LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum.Author_Name:
                        {

                            StoredProc = $"exec pLiteratureListFiltered @authorId = '{advancedSearch.GenericsIntergerKeyword}', @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}";
                            break;
                        }

                    default:
                        if (fromDate == null && toDate == null && advancedSearch.ClassificationId != 0)
                        {
                            StoredProc = $"exec pLiteratureListFiltered @classificationId = {advancedSearch.ClassificationId}";
                            break;
                        }
                        else if (fromDate != null && toDate != null)
                        {
                            StoredProc = $"exec pLiteratureListFiltered @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}";
                            break;
                        }
                        StoredProc = $"exec pLiteratureListFiltered";
                        break;
                }
                return await _DbContext.LiteratureListMobileAppVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<LmsLiterature> GetLmsLiteraturesSync()
        {
            if (_LmsLiteratures == null)
            {
                _LmsLiteratures = _DbContext.LmsLiteratures.ToList();
            }

            return _LmsLiteratures;
        }
        public async Task<List<LmsLiteratureAuthor>> GetLmsLiteraturesAuthorBySearchTerm(string searchTerm)
        {
            try
            {
                if (_LmsLiteraturesAuthor == null || _LmsLiteraturesAuthor.Count() == 0)
                {
                    string StoredProc = "";
                    if (searchTerm == String.Empty)
                    {
                        StoredProc = "exec pLiteratureAuthorSelBySearchTerm";
                    }
                    else
                    {
                        StoredProc = "exec pLiteratureAuthorSelBySearchTerm " + "@searchTerm = " + searchTerm;
                    }
                    _LmsLiteraturesAuthor = await _DbContext.LmsLiteratureAuthors.FromSqlRaw(StoredProc).ToListAsync();
                }
                if (_LmsLiteraturesAuthor != null)
                {
                    return _LmsLiteraturesAuthor;
                }
                else
                {
                    return new List<LmsLiteratureAuthor>();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<LmsLiteratureAuthor> GetLmsLiteratureAuthorById(int authorId)
        {
            try
            {
                return await _DbContext.LmsLiteratureAuthors.FindAsync(authorId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<int> GetLmsLiteratureCountByAuthorId(int authorId)
        {
            try
            {
                return _DbContext.LmsLiteratureAuthors.Where(x => x.AuthorId == authorId).Count();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<LiteratureDetailVM>> GetLmsLiteraturesBySearchTerm(string? searchTerm, string appCulture)
        {
            try
            {
                if (_LiteratureDetailsVM == null)
                {
                    string StoredProc = "exec pLiteratureSelBySearchTerm " +
                    "@searchTerm = '" + searchTerm + "'";
                    _LiteratureDetailsVM = await _DbContext.LmsLiteratureDetailsVM.FromSqlRaw(StoredProc).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _LiteratureDetailsVM;
        }


        //<History Author = 'Hassan Abbas' Date='2022-08-30' Version="1.0" Branch="master"> Get new literature number</History>
        public async Task<int> GetNewLmsLiteratureNumber()
        {
            try
            {
                if (_DbContext.LmsLiteratures.Any())
                    return await _DbContext.LmsLiteratures.Select(x => x.Number).MaxAsync() + 1;
                else
                    return 1;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region CalculateNumberOfCopyWithSeriesNumber
        public async Task<decimal> DivideNumberOfCopyBySeriesNumber(int seriesNumber, int noOfCopies)
        {
            return noOfCopies / seriesNumber;
        }
        #endregion

        #region Create Literature with transaction

        public async Task<LmsLiterature> CreateLmsLiterature(LmsLiterature LmsLiterature)
        {
            _DbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            using (_DbContext)
            {
                using (var transaction = _DbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _DbContext.LmsLiteratures.AsNoTracking();
                        string IndexNumber = string.Empty;
                        int seriesNumber = LmsLiterature.SeriesNumber;
                        LmsLiterature.LiteratureUniqueId = Guid.NewGuid();
                        for (int i = 0; i < seriesNumber; i++)
                        {
                            if (LmsLiterature.LiteratureId != 0)
                            {
                                LmsLiterature.LiteratureId = 0;
                            }
                            if (LmsLiterature.LmsLiteratureIndex != null)
                            {
                                IndexNumber = LmsLiterature.LmsLiteratureIndex.IndexNumber;
                            }
                            (LmsLiterature.DeweyBookNumber, LmsLiterature.SeriesSequenceNumber, string digitalSequenceNumberOutput) = await GenerateUniqueDeweyBookNumber(IndexNumber, i);
                            LmsLiterature.Characters = GetSubString(LmsLiterature.Name) + "." + GetSubString(LmsLiterature.LmsLiteratureAuthors.Select(x => x.FullName_En).FirstOrDefault()) + "/" + IndexNumber.ToString() + "." + digitalSequenceNumberOutput;
                            LmsLiterature.Characters82 = (IndexNumber.ToString()) + "." + digitalSequenceNumberOutput;
                            LmsLiterature.LmsLiteratureIndex = null;
                            await _DbContext.LmsLiteratures.AddAsync(LmsLiterature);
                            await _DbContext.SaveChangesAsync();
                            var LiteratureId = LmsLiterature.LiteratureId;
                            int AuthorId = 0;
                            if (LiteratureId != 0)
                            {
                                await InsertPurchaseByLiterature(LmsLiterature);
                                if (LmsLiterature.LmsLiteratureAuthors.Count() > 0)
                                {
                                    await InsertLiteratureAuthor(LmsLiterature, LmsLiterature.LiteratureId, LmsLiterature.LmsLiteratureAuthors, _DbContext);
                                }
                                if (LmsLiterature.LiteratureBarcodes.Any())
                                {
                                    List<LmsLiteratureBarcode> lmsLiteratureBarcodes = LmsLiterature.LiteratureBarcodes;
                                    LmsLiterature.LiteratureBarcodes = LmsLiterature.LiteratureBarcodes.GetRange(0, LmsLiterature.CopyCount);
                                    await InsertBarcodeByLiterature(LmsLiterature, _DbContext);
                                    LmsLiterature.LiteratureBarcodes = lmsLiteratureBarcodes;
                                    LmsLiterature.LiteratureBarcodes.RemoveRange(0, LmsLiterature.CopyCount);
                                }
                                await InsertLiteratureTags(LmsLiterature, _DbContext);
                            }
                            var literature = await _DbContext.LmsLiteratures.FindAsync(LmsLiterature.LiteratureId);
                            LmsLiterature.NotificationParameter.Name = literature.Name;

                            //  return LiteratureId;

                            // adding literature id to list for attachment use
                            LmsLiterature.LiteratureIdList.Add(LmsLiterature.LiteratureId);
                        }
                        await transaction.CommitAsync();
                        return LmsLiterature;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        private static string GetSubString(string name)
        {
            string bookName = string.Empty;
            if (name != null)
            {
                name = name.TrimStart();
                name = name.TrimEnd();
                var splitResult = name.Split(' ');

                if (splitResult.Length > 1)
                {
                    var resultfirst = splitResult[0];
                    if (resultfirst != null)
                    {
                        bookName += resultfirst.Substring(0, 1);
                    }
                    var result = splitResult[splitResult.Length - 1];
                    if (result != null)
                    {
                        bookName += result.Substring(0, 1);
                    }
                }
                else
                {
                    bookName += splitResult[0].Substring(0, 1).ToUpper();
                }
                return bookName.ToUpper();
            }
            return bookName.ToUpper();
        }
        private async Task InsertLiteratureAuthor(LmsLiterature literature, int literatureId, List<LmsLiteratureAuthor>? lmsLiteratureAuthors, DatabaseContext dbContext)
        {
            try
            {
                foreach (var item in lmsLiteratureAuthors)
                {
                    if (item.AuthorId == 0)
                    {
                        item.FullName_En = item.FullName_En;
                        item.FullName_Ar = item.FullName_Ar != null ? item.FullName_Ar : item.FullName_En;
                        item.CreatedBy = literature.ModifiedBy != null ? literature.ModifiedBy : literature.CreatedBy;
                        item.CreatedDate = DateTime.Now;
                        item.FirstName_Ar = literature.Author_FirstName_Ar;
                        item.FirstName_En = literature.Author_FirstName_En;
                        item.SecondName_Ar = literature.Author_SecondName_Ar;
                        item.SecondName_En = literature.Author_SecondName_En;
                        item.ThirdName_Ar = literature.Author_ThirdName_Ar;
                        item.ThirdName_En = literature.Author_ThirdName_En;
                        item.Address_En = item.Address_En;
                        item.Address_Ar = item.Address_Ar != null ? item.Address_Ar : item.Address_En;
                        await dbContext.LmsLiteratureAuthors.AddAsync(item);
                        await dbContext.SaveChangesAsync();
                    }
                    var result = await dbContext.LmsLiteratureDetailsLmsLiteratureAuthors.Where(x => x.LiteratureId == literatureId && x.AuthorId == item.AuthorId).FirstOrDefaultAsync();
                    if (result == null)
                    {
                        var ForInsertObj = new LmsLiteratureDetailsLmsLiteratureAuthor();
                        ForInsertObj.LiteratureId = literatureId;
                        ForInsertObj.AuthorId = item.AuthorId;
                        await dbContext.LmsLiteratureDetailsLmsLiteratureAuthors.AddAsync(ForInsertObj);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task UpdateLiteratureAuthor(LmsLiterature literature, int LiteratureId, List<LmsLiteratureAuthor>? lmsLiteratureAuthors, DatabaseContext dbContext)
        {
            try
            {
                await ExistingLiteratureAuthorRecordDelete(LiteratureId, dbContext);
                await InsertLiteratureAuthor(literature, LiteratureId, lmsLiteratureAuthors, dbContext);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task ExistingLiteratureAuthorRecordDelete(int LiteratureId, DatabaseContext dbContext)
        {
            List<LmsLiteratureDetailsLmsLiteratureAuthor> LiteratureAuthor = await dbContext.LmsLiteratureDetailsLmsLiteratureAuthors.Where(x => x.LiteratureId == LiteratureId).ToListAsync();
            if (LiteratureAuthor.Count() > 0)
            {
                dbContext.LmsLiteratureDetailsLmsLiteratureAuthors.RemoveRange(LiteratureAuthor);
                await dbContext.SaveChangesAsync();
            }
        }
        private async Task InsertLiteratureTags(LmsLiterature lmsLiterature, DatabaseContext dbContext)
        {
            try
            {
                foreach (var tag in lmsLiterature.LiteratureTags)
                {
                    LiteratureDetailLiteratureTag literatureTag = new LiteratureDetailLiteratureTag();
                    literatureTag.TagId = tag.TagId;
                    literatureTag.LiteratureId = lmsLiterature.LiteratureId;
                    literatureTag.Value = tag.Value;
                    await dbContext.LiteratureDetailLiteratureTags.AddAsync(literatureTag);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task InsertBarcodeByLiterature(LmsLiterature lmsLiterature, DatabaseContext dbContext)
        {
            try
            {
                if (lmsLiterature.LiteratureBarcodes.Count() != 0 || lmsLiterature.LiteratureBarcodes != null)
                {
                    foreach (var barcode in lmsLiterature.LiteratureBarcodes)
                    {
                        LmsLiteratureBarcode pBarcodeObj = new LmsLiteratureBarcode();
                        pBarcodeObj.LiteratureId = lmsLiterature.LiteratureId;
                        pBarcodeObj.BarCodeNumber = barcode.BarCodeNumber;
                        pBarcodeObj.CreatedBy = lmsLiterature.CreatedBy;
                        pBarcodeObj.CreatedDate = lmsLiterature.CreatedDate;
                        pBarcodeObj.IsDeleted = false;
                        pBarcodeObj.Active = barcode.Active;
                        pBarcodeObj.IsBorrowed = false;
                        pBarcodeObj.RFIDValue = Convert.ToString(barcode.RFID);
                        await dbContext.LmsLiteratureBarcodes.AddAsync(pBarcodeObj);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task InsertManyToManyLmsLiteratureAuthor(int LiteratureId, List<LmsLiteratureAuthor>? lmsLiteratureAuthors, DatabaseContext dbContext)
        {
            try
            {
                LmsLiteratureDetailsLmsLiteratureAuthor pAuthorObj = new LmsLiteratureDetailsLmsLiteratureAuthor();
                foreach (var pAuthor in lmsLiteratureAuthors)
                {
                    pAuthorObj.LiteratureId = LiteratureId;
                    pAuthorObj.AuthorId = pAuthor.AuthorId;
                    await dbContext.LmsLiteratureDetailsLmsLiteratureAuthors.AddAsync(pAuthorObj);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<int> InsertAuthorByLiterature(LmsLiterature lmsLiterature, DatabaseContext dbContext)
        {
            try
            {
                LmsLiteratureAuthor pAuthorObj = new LmsLiteratureAuthor();
                pAuthorObj.FullName_Ar = lmsLiterature.Author_FullName_Ar != null ? lmsLiterature.Author_FullName_Ar : lmsLiterature.Author_FullName_En;
                pAuthorObj.FullName_En = lmsLiterature.Author_FullName_En;
                pAuthorObj.FirstName_Ar = lmsLiterature.Author_FirstName_Ar;
                pAuthorObj.FirstName_En = lmsLiterature.Author_FirstName_En;
                pAuthorObj.SecondName_Ar = lmsLiterature.Author_SecondName_Ar;
                pAuthorObj.SecondName_En = lmsLiterature.Author_SecondName_En;
                pAuthorObj.ThirdName_Ar = lmsLiterature.Author_ThirdName_Ar;
                pAuthorObj.ThirdName_En = lmsLiterature.Author_ThirdName_En;
                pAuthorObj.Address_Ar = lmsLiterature.Author_Address_En;
                pAuthorObj.Address_En = lmsLiterature.Author_Address_En;
                pAuthorObj.CreatedBy = lmsLiterature.CreatedBy;
                pAuthorObj.CreatedDate = DateTime.Now;

                await dbContext.LmsLiteratureAuthors.AddAsync(pAuthorObj);
                await dbContext.SaveChangesAsync();
                return pAuthorObj.AuthorId;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async Task InsertPurchaseByLiterature(LmsLiterature lmsLiterature, int a = 0)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                LmsLiteraturePurchase pObj = new LmsLiteraturePurchase();
                pObj.LiteratureId = lmsLiterature.LiteratureId;
                pObj.CreatedDate = DateTime.Now;
                pObj.CreatedBy = lmsLiterature.CreatedBy;
                pObj.IsDeleted = false;
                pObj.Price = (decimal)lmsLiterature.Purchase_Price;
                pObj.Location = lmsLiterature.Purchase_Location;
                pObj.Date = lmsLiterature.Purchase_Date;
                await _dbContext.LmsLiteraturePurchases.AddAsync(pObj);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Function Generate Unique Dewey Book Number

        public async Task<(string DeweyBookNumber, string SequnecNumber, string digitalSequenceNumberOutput)> GenerateUniqueDeweyBookNumber(string indexNumber, int iterationNumber)
        {
            try
            {
                string storedProc = $"exec pGetliteratureDeweyNumberPatternslist";
                var result = await _DbContext.LiteratureDeweyNumberPatternVMs.FromSqlRaw(storedProc).ToListAsync();
                if (result.Any())
                {
                    var letestDeweyPatternNumberRecord = result.OrderByDescending(e => e.CreatedDate).FirstOrDefault();
                    if (letestDeweyPatternNumberRecord != null)
                    {
                        List<string> deweyBookNumbers = new List<string>();
                        if (letestDeweyPatternNumberRecord.CheracterSeriesOrder < letestDeweyPatternNumberRecord.DigitSequnceOrder)
                        {
                            deweyBookNumbers = await _DbContext.LmsLiteratures.Where(x => x.DeweyBookNumber
                           .StartsWith($"{indexNumber}{letestDeweyPatternNumberRecord.SeperatorPattern}{letestDeweyPatternNumberRecord.SeriesNumber}"))
                         .Select(x => x.DeweyBookNumber).ToListAsync() ?? new List<string>();
                        }
                        else
                        {
                            string DigitSeq = letestDeweyPatternNumberRecord.DigitSequnceOrder.ToString("D4");
                            string pattern = $"{indexNumber}{letestDeweyPatternNumberRecord.SeperatorPattern}{DigitSeq.Substring(0, DigitSeq.Length - 1)}%";
                            deweyBookNumbers = await _DbContext.LmsLiteratures.Where(x => EF.Functions.Like(x.DeweyBookNumber, pattern))
                               .Select(x => x.DeweyBookNumber).ToListAsync() ?? new List<string>();
                        }
                        if (deweyBookNumbers != null && deweyBookNumbers.Count > 0)
                        {
                            List<int> ListSequenceNumbers = new();
                            if (letestDeweyPatternNumberRecord.CheracterSeriesOrder > letestDeweyPatternNumberRecord.DigitSequnceOrder)
                            {
                                foreach (var maxNumber in deweyBookNumbers)
                                {
                                    int istSeperatorIndex = maxNumber.IndexOf($"{letestDeweyPatternNumberRecord.SeperatorPattern}");
                                    int SecondSeperatorIndex = maxNumber.IndexOf($"{letestDeweyPatternNumberRecord.SeperatorPattern}", istSeperatorIndex + 1);
                                    if (istSeperatorIndex >= 0 && SecondSeperatorIndex >= 0)
                                    {
                                        string trimmed = maxNumber.Substring(istSeperatorIndex + 1, SecondSeperatorIndex - istSeperatorIndex - 1);
                                        ListSequenceNumbers.Add(Convert.ToInt32(trimmed));
                                    }
                                }
                            }
                            else
                            {
                                foreach (var maxNumber in deweyBookNumbers)
                                {
                                    int firstSeparatorIndex = maxNumber.IndexOf($"{letestDeweyPatternNumberRecord.SeperatorPattern}");
                                    int secondSeparatorIndex = maxNumber.IndexOf($"{letestDeweyPatternNumberRecord.SeperatorPattern}", firstSeparatorIndex + 1);

                                    if (firstSeparatorIndex >= 0 && secondSeparatorIndex >= 0)
                                    {
                                        string lastPart = maxNumber.Substring(secondSeparatorIndex + 1);
                                        ListSequenceNumbers.Add(Convert.ToInt32(lastPart));
                                    }
                                }
                            }
                            int sequenceNumbersMaxValue = ListSequenceNumbers.Max();
                            int DigitalSequenceNumber = int.Parse(letestDeweyPatternNumberRecord.DigitSequenceNumber);
                            if (iterationNumber == 0)
                            {
                                DigitalSequenceNumber = sequenceNumbersMaxValue;
                                DigitalSequenceNumber++;
                            }
                            else
                                DigitalSequenceNumber = sequenceNumbersMaxValue;

                            iterationNumber++;
                            string digitalSequenceNumberOutput = DigitalSequenceNumber.ToString($"D{letestDeweyPatternNumberRecord.DigitSequenceNumber.Length}");

                            int SeriesSequenceNumber = int.Parse(letestDeweyPatternNumberRecord.SeriesSequenceNumber);
                            SeriesSequenceNumber = iterationNumber;
                            string SeriesSequence = SeriesSequenceNumber.ToString($"D{letestDeweyPatternNumberRecord.SeriesSequenceNumber.Length}");

                            if (letestDeweyPatternNumberRecord.CheracterSeriesOrder > letestDeweyPatternNumberRecord.DigitSequnceOrder)
                                return ($"{indexNumber}{letestDeweyPatternNumberRecord.SeperatorPattern}{digitalSequenceNumberOutput}{letestDeweyPatternNumberRecord.SeperatorPattern}{letestDeweyPatternNumberRecord.SeriesNumber}{SeriesSequence}", SeriesSequence.ToString(), digitalSequenceNumberOutput);
                            else
                                return ($"{indexNumber}{letestDeweyPatternNumberRecord.SeperatorPattern}{letestDeweyPatternNumberRecord.SeriesNumber}{SeriesSequence}{letestDeweyPatternNumberRecord.SeperatorPattern}{digitalSequenceNumberOutput}", SeriesSequence.ToString(), digitalSequenceNumberOutput);
                        }
                        else
                        {
                            int maxValue = 0;
                            maxValue = maxValue + 1;
                            iterationNumber++;
                            int DigitalSequenceNumber = int.Parse(letestDeweyPatternNumberRecord.DigitSequenceNumber);
                            DigitalSequenceNumber += maxValue;
                            string digitalSequenceNumberOutput = DigitalSequenceNumber.ToString($"D{letestDeweyPatternNumberRecord.DigitSequenceNumber.Length}");

                            int SeriesSequenceNumber = int.Parse(letestDeweyPatternNumberRecord.SeriesSequenceNumber);
                            SeriesSequenceNumber = iterationNumber;
                            string SeriesSequence = SeriesSequenceNumber.ToString($"D{letestDeweyPatternNumberRecord.SeriesSequenceNumber.Length}");

                            if (letestDeweyPatternNumberRecord.CheracterSeriesOrder > letestDeweyPatternNumberRecord.DigitSequnceOrder)
                                return ($"{indexNumber}{letestDeweyPatternNumberRecord.SeperatorPattern}{digitalSequenceNumberOutput}{letestDeweyPatternNumberRecord.SeperatorPattern}{letestDeweyPatternNumberRecord.SeriesNumber}{SeriesSequence}", SeriesSequence.ToString(), digitalSequenceNumberOutput);
                            else
                                return ($"{indexNumber}{letestDeweyPatternNumberRecord.SeperatorPattern}{letestDeweyPatternNumberRecord.SeriesNumber}{SeriesSequence}{letestDeweyPatternNumberRecord.SeperatorPattern}{digitalSequenceNumberOutput}", SeriesSequence.ToString(), digitalSequenceNumberOutput);
                        }
                    }
                    else
                        return ("Please_Add_Number_Pattern_Lookups_First", null, null);
                }
                else
                    return ("Please_Add_Number_Pattern_Lookups_First", null, null);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<(string DeweyBookNumber, string SequnecNumber)> IncreaseDeweySeriesNumber(string indexNumber, int iterationNumber, Guid LiteratureUniqueId)
        {
            try
            {
                string storedProc = $"exec pGetliteratureDeweyNumberPatternslist";
                var result = await _DbContext.LiteratureDeweyNumberPatternVMs.FromSqlRaw(storedProc).ToListAsync();
                var letestDeweyPatternNumberRecord = result.OrderByDescending(e => e.CreatedDate).FirstOrDefault();
                if (letestDeweyPatternNumberRecord != null)
                {
                    // List<string> deweyBookNumbers = new List<string>();
                    dynamic deweyBookNumbers;
                    List<int> ListSequenceNumbers = new();
                    if (letestDeweyPatternNumberRecord.CheracterSeriesOrder < letestDeweyPatternNumberRecord.DigitSequnceOrder)
                    {
                        deweyBookNumbers = await _DbContext.LmsLiteratures.Where(x => x.DeweyBookNumber
                      .StartsWith($"{indexNumber}{letestDeweyPatternNumberRecord.SeperatorPattern}{letestDeweyPatternNumberRecord.SeriesNumber}") && x.LiteratureUniqueId == LiteratureUniqueId)
                    .Select(x => new { x.DeweyBookNumber, x.SeriesSequenceNumber }).ToListAsync();
                    }
                    else
                    {
                        string DigitSeq = letestDeweyPatternNumberRecord.DigitSequnceOrder.ToString("D4");
                        string pattern = $"{indexNumber}{letestDeweyPatternNumberRecord.SeperatorPattern}{DigitSeq.Substring(0, DigitSeq.Length - 1)}%";
                        deweyBookNumbers = await _DbContext.LmsLiteratures.Where(x => EF.Functions.Like(x.DeweyBookNumber, pattern) && x.LiteratureUniqueId == LiteratureUniqueId)
                           .Select(x => new { x.DeweyBookNumber, x.SeriesSequenceNumber }).ToListAsync();
                    }
                    if (deweyBookNumbers != null && deweyBookNumbers.Count > 0)
                    {
                        List<int> ListDigitalSequenceNumbers = new();
                        if (letestDeweyPatternNumberRecord.CheracterSeriesOrder < letestDeweyPatternNumberRecord.DigitSequnceOrder)
                        {
                            foreach (var maxNumber in deweyBookNumbers)
                            {
                                int firstSeparatorIndex = maxNumber.DeweyBookNumber.IndexOf($"{letestDeweyPatternNumberRecord.SeperatorPattern}");
                                int secondSeparatorIndex = maxNumber.DeweyBookNumber.IndexOf($"{letestDeweyPatternNumberRecord.SeperatorPattern}", firstSeparatorIndex + 1);

                                if (firstSeparatorIndex >= 0 && secondSeparatorIndex >= 0)
                                {
                                    string lastPart = maxNumber.DeweyBookNumber.Substring(secondSeparatorIndex + 1);
                                    ListDigitalSequenceNumbers.Add(Convert.ToInt32(lastPart));
                                }
                                ListSequenceNumbers.Add(Convert.ToInt32(maxNumber.SeriesSequenceNumber));
                            }
                        }
                        else
                        {
                            foreach (var maxNumber in deweyBookNumbers)
                            {
                                int istSeperatorIndex = maxNumber.DeweyBookNumber.IndexOf($"{letestDeweyPatternNumberRecord.SeperatorPattern}");
                                int SecondSeperatorIndex = maxNumber.DeweyBookNumber.IndexOf($"{letestDeweyPatternNumberRecord.SeperatorPattern}", istSeperatorIndex + 1);
                                if (istSeperatorIndex >= 0 && SecondSeperatorIndex >= 0)
                                {
                                    string trimmed = maxNumber.DeweyBookNumber.Substring(istSeperatorIndex + 1, SecondSeperatorIndex - istSeperatorIndex - 1);
                                    ListDigitalSequenceNumbers.Add(Convert.ToInt32(trimmed));
                                }

                                ListSequenceNumbers.Add(Convert.ToInt32(maxNumber.SeriesSequenceNumber));
                            }
                        }
                        int sequenceNumbersMaxValue = ListSequenceNumbers.Max();
                        int DigitSequnecNumber = ListDigitalSequenceNumbers.Max();

                        int DigitalSequenceNumber = int.Parse(letestDeweyPatternNumberRecord.DigitSequenceNumber);

                        DigitalSequenceNumber = DigitSequnecNumber;
                        string digitalSequenceNumberOutput = DigitalSequenceNumber.ToString($"D{letestDeweyPatternNumberRecord.DigitSequenceNumber.Length}");

                        int SeriesSequenceNumber = int.Parse(letestDeweyPatternNumberRecord.SeriesSequenceNumber);
                        SeriesSequenceNumber = sequenceNumbersMaxValue + 1;
                        string SeriesSequence = SeriesSequenceNumber.ToString($"D{letestDeweyPatternNumberRecord.SeriesSequenceNumber.Length}");

                        if (letestDeweyPatternNumberRecord.CheracterSeriesOrder > letestDeweyPatternNumberRecord.DigitSequnceOrder)
                            return ($"{indexNumber}{letestDeweyPatternNumberRecord.SeperatorPattern}{digitalSequenceNumberOutput}{letestDeweyPatternNumberRecord.SeperatorPattern}{letestDeweyPatternNumberRecord.SeriesNumber}{SeriesSequence}", SeriesSequence.ToString());
                        else
                            return ($"{indexNumber}{letestDeweyPatternNumberRecord.SeperatorPattern}{letestDeweyPatternNumberRecord.SeriesNumber}{SeriesSequence}{letestDeweyPatternNumberRecord.SeperatorPattern}{digitalSequenceNumberOutput}", SeriesSequence.ToString());
                    }
                    else
                    {
                        int maxValue = 0;
                        maxValue = maxValue + 1;
                        iterationNumber++;
                        int DigitalSequenceNumber = int.Parse(letestDeweyPatternNumberRecord.DigitSequenceNumber);
                        DigitalSequenceNumber += maxValue;
                        string digitalSequenceNumberOutput = DigitalSequenceNumber.ToString($"D{letestDeweyPatternNumberRecord.DigitSequenceNumber.Length}");

                        int SeriesSequenceNumber = int.Parse(letestDeweyPatternNumberRecord.SeriesSequenceNumber);
                        SeriesSequenceNumber = iterationNumber;
                        string SeriesSequence = SeriesSequenceNumber.ToString($"D{letestDeweyPatternNumberRecord.SeriesSequenceNumber.Length}");

                        if (letestDeweyPatternNumberRecord.CheracterSeriesOrder > letestDeweyPatternNumberRecord.DigitSequnceOrder)
                            return ($"{indexNumber}{letestDeweyPatternNumberRecord.SeperatorPattern}{digitalSequenceNumberOutput}{letestDeweyPatternNumberRecord.SeperatorPattern}{letestDeweyPatternNumberRecord.SeriesNumber}{SeriesSequence}", SeriesSequence.ToString());
                        else
                            return ($"{indexNumber}{letestDeweyPatternNumberRecord.SeperatorPattern}{letestDeweyPatternNumberRecord.SeriesNumber}{SeriesSequence}{letestDeweyPatternNumberRecord.SeperatorPattern}{digitalSequenceNumberOutput}", SeriesSequence.ToString());
                    }
                }
                else
                    return ("Please_Add_Number_Pattern_Lookups_First", "Null");
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

        #region GetUniqueInfo with all dependent

        private async Task GetAndFillAuthorInfoByPurchase(LmsLiterature literatuure)
        {
            try
            {
                List<LmsLiteratureDetailsLmsLiteratureAuthor>? LiteratureAuthor = await _DbContext.LmsLiteratureDetailsLmsLiteratureAuthors.Where(x => x.LiteratureId == literatuure.LiteratureId).ToListAsync();
                if (LiteratureAuthor != null)
                {
                    foreach (var Authors in LiteratureAuthor)
                    {
                        List<LmsLiteratureAuthor>? Author = await _DbContext.LmsLiteratureAuthors.Where(x => x.AuthorId == Authors.AuthorId).ToListAsync();
                        foreach (var Auth in Author)
                        {
                            _LmsLiteraturesAuthor.Add(new LmsLiteratureAuthor
                            {
                                Address_Ar = Auth.Address_Ar,
                                Address_En = Auth.Address_En,
                                FirstName_Ar = Auth.FirstName_Ar,
                                FirstName_En = Auth.FirstName_En,
                                ThirdName_En = Auth.ThirdName_En,
                                ThirdName_Ar = Auth.ThirdName_Ar,
                                SecondName_En = Auth.SecondName_En,
                                SecondName_Ar = Auth.SecondName_Ar,
                                FullName_Ar = Auth.FullName_Ar,
                                FullName_En = Auth.FullName_En,
                                AuthorId = Auth.AuthorId
                            });
                        }
                    }


                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GetAndFillPurchaseInfoByLiterature(LmsLiterature literatuure)
        {

            try
            {
                LmsLiteraturePurchase? PurchaseObj = _DbContext.LmsLiteraturePurchases.Where(x => x.LiteratureId == literatuure.LiteratureId).FirstOrDefault();
                if (PurchaseObj != null)
                {
                    literatuure.Purchase_Date = PurchaseObj.Date;
                    literatuure.Purchase_Location = PurchaseObj.Location;
                    literatuure.Purchase_Price = PurchaseObj.Price;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GetAndFillBarcodeInfoByLiterature(LmsLiterature lmsLiterature)
        {
            try
            {
                LmsLiteratureBarcode? BarcodeObj = _DbContext.LmsLiteratureBarcodes.Where(x => x.LiteratureId == lmsLiterature.LiteratureId && x.IsDeleted != true && x.Active != false).FirstOrDefault();
                if (BarcodeObj != null)
                    lmsLiterature.BarCodeNumber = BarcodeObj.BarCodeNumber;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task GetAndFillLiteratureTags(LmsLiterature lmsLiterature)
        {
            try
            {
                string StoredProc = $"exec pLiteratureTagsListById @literatureId = '{lmsLiterature.LiteratureId}'";
                lmsLiterature.LiteratureTags = await _DbContext.LmsLiteratureDetailLiteratureTagVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task GetAndFillLiteratureTagsbyLiteratureId(LmsLiterature lmsLiterature)
        {
            try
            {
                string StoredProc = $"exec pLiteratureTagsListById @literatureId = '{lmsLiterature.LiteratureId}'";
                lmsLiterature.LiteratureTags = await _DbContext.LmsLiteratureDetailLiteratureTagVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<LmsLiterature> GetLiteratureDetailTagById(int Id)
        {
            try
            {
                LmsLiterature? literatuure = await _DbContext.LmsLiteratures.FindAsync(Id);
                if (literatuure != null)
                {
                    await GetAndFillLiteratureTagsbyLiteratureId(literatuure);
                    return literatuure;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task GetAndFillLiteratureBarcodes(LmsLiterature lmsLiterature)
        {
            try
            {
                lmsLiterature.LiteratureBarcodes = await _DbContext.LmsLiteratureBarcodes.Where(b => b.LiteratureId == lmsLiterature.LiteratureId && b.IsDeleted != true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<LmsLiterature> GetLiteratureDetail(int Id)
        {
            try
            {
                LmsLiterature? literatuure = await _DbContext.LmsLiteratures.FindAsync(Id);
                if (literatuure != null)
                {
                    GetAndFillBarcodeInfoByLiterature(literatuure);
                    GetAndFillPurchaseInfoByLiterature(literatuure);
                    await GetAndFillAuthorInfoByPurchase(literatuure);
                    literatuure.LmsLiteratureAuthors = _LmsLiteraturesAuthor;
                    await GetAndFillLiteratureTags(literatuure);
                    await GetAndFillLiteratureBarcodes(literatuure);
                    return literatuure;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Update literature with dependent tables
        public async Task<LmsLiterature> UpdateLmsLiterature(LmsLiterature LmsLiteratures)
        {
            using (_DbContext)
            {
                using (var transaction = _DbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _DbContext.LmsLiteratures.AsNoTracking();
                        // if only index number will be changed
                        if (LmsLiteratures.LmsLiteratureIndex.IndexNumber != LmsLiteratures.PreviousLmsLiteratureIndex.IndexNumber && LmsLiteratures.SeriesNumber == LmsLiteratures.PreviousSeriesNumber
                            && LmsLiteratures.CopyCount == LmsLiteratures.PreviousCopyCount)
                        {
                            string IndexNumber = string.Empty;
                            if (LmsLiteratures.LmsLiteratureIndex != null)
                            {
                                IndexNumber = LmsLiteratures.LmsLiteratureIndex.IndexNumber;
                            }
                            (LmsLiteratures.DeweyBookNumber, LmsLiteratures.SeriesSequenceNumber, string digitalSequenceNumberOutput) = await GenerateUniqueDeweyBookNumber(LmsLiteratures.LmsLiteratureIndex.IndexNumber, 0);
                            var resultLiterature = await _DbContext.LmsLiteratures.AsNoTracking().Where(x => x.LiteratureId == LmsLiteratures.LiteratureId).FirstOrDefaultAsync();
                            if (resultLiterature != null)
                            {
                                resultLiterature = LmsLiteratures;
                                resultLiterature.Characters = GetSubString(LmsLiteratures.Name) + "." + GetSubString(LmsLiteratures.LmsLiteratureAuthors.Select(x => x.FullName_En).FirstOrDefault()) + "/" + IndexNumber.ToString() + "." + digitalSequenceNumberOutput;
                                resultLiterature.IndexId = LmsLiteratures.IndexId;
                                resultLiterature.Characters82 = (IndexNumber.ToString()) + "." + digitalSequenceNumberOutput;
                                _DbContext.Entry(resultLiterature).State = EntityState.Modified;
                                await _DbContext.SaveChangesAsync();
                            }
                            await UpdatePurchaseByLiterature(LmsLiteratures, _DbContext);
                            await UpdateBarCodeByLiterature(LmsLiteratures, _DbContext);
                            await UpdateAuthorByLiterature(LmsLiteratures, _DbContext);
                            await UpdateLiteratureTags(LmsLiteratures, _DbContext);
                            transaction.Commit();
                            return LmsLiteratures;
                        }

                        // if only copy count will be changed
                        else if (LmsLiteratures.CopyCount != LmsLiteratures.PreviousCopyCount && LmsLiteratures.LmsLiteratureIndex.IndexNumber == LmsLiteratures.PreviousLmsLiteratureIndex.IndexNumber
                             && LmsLiteratures.SeriesNumber == LmsLiteratures.PreviousSeriesNumber)
                        {
                            var resultLiterature = await _DbContext.LmsLiteratures.AsNoTracking().Where(x => x.LiteratureId == LmsLiteratures.LiteratureId).FirstOrDefaultAsync();
                            if (resultLiterature != null)
                            {
                                resultLiterature = LmsLiteratures;
                                _DbContext.Entry(resultLiterature).State = EntityState.Modified;
                                await _DbContext.SaveChangesAsync();
                            }
                            await UpdatePurchaseByLiterature(LmsLiteratures, _DbContext);
                            await UpdateBarCodeByLiterature(LmsLiteratures, _DbContext);
                            await UpdateAuthorByLiterature(LmsLiteratures, _DbContext);
                            await UpdateLiteratureTags(LmsLiteratures, _DbContext);
                            transaction.Commit();
                            return LmsLiteratures;
                        }

                        // if only index number and copy count number will be changed
                        else if (LmsLiteratures.LmsLiteratureIndex.IndexNumber != LmsLiteratures.PreviousLmsLiteratureIndex.IndexNumber && LmsLiteratures.SeriesNumber == LmsLiteratures.PreviousSeriesNumber
                             && LmsLiteratures.CopyCount != LmsLiteratures.PreviousCopyCount)
                        {
                            string IndexNumber = string.Empty;
                            if (LmsLiteratures.LmsLiteratureIndex != null)
                            {
                                IndexNumber = LmsLiteratures.LmsLiteratureIndex.IndexNumber;
                            }
                            (LmsLiteratures.DeweyBookNumber, LmsLiteratures.SeriesSequenceNumber, string digitalSequenceNumberOutput) = await GenerateUniqueDeweyBookNumber(LmsLiteratures.LmsLiteratureIndex.IndexNumber, 0);
                            var resultLiterature = await _DbContext.LmsLiteratures.AsNoTracking().Where(x => x.LiteratureId == LmsLiteratures.LiteratureId).FirstOrDefaultAsync();
                            if (resultLiterature != null)
                            {
                                resultLiterature = LmsLiteratures;
                                resultLiterature.Characters = GetSubString(LmsLiteratures.Name) + "." + GetSubString(LmsLiteratures.LmsLiteratureAuthors.Select(x => x.FullName_En).FirstOrDefault()) + "/" + IndexNumber.ToString() + "." + digitalSequenceNumberOutput;
                                resultLiterature.IndexId = LmsLiteratures.IndexId;
                                resultLiterature.Characters82 = (IndexNumber.ToString()) + "." + digitalSequenceNumberOutput;
                                _DbContext.Entry(resultLiterature).State = EntityState.Modified;
                                await _DbContext.SaveChangesAsync();
                            }
                            await UpdatePurchaseByLiterature(LmsLiteratures, _DbContext);
                            await UpdateBarCodeByLiterature(LmsLiteratures, _DbContext);
                            await UpdateAuthorByLiterature(LmsLiteratures, _DbContext);
                            await UpdateLiteratureTags(LmsLiteratures, _DbContext);
                            transaction.Commit();
                            return LmsLiteratures;
                        }

                        // if series number and copy count will be changed
                        else if (LmsLiteratures.SeriesNumber != LmsLiteratures.PreviousSeriesNumber && LmsLiteratures.CopyCount != LmsLiteratures.PreviousCopyCount &&
                                 LmsLiteratures.LmsLiteratureIndex.IndexNumber == LmsLiteratures.PreviousLmsLiteratureIndex.IndexNumber)
                        {
                            if (LmsLiteratures.SeriesNumber > LmsLiteratures.PreviousSeriesNumber)
                            {
                                int oldCopyCount = LmsLiteratures.CopyCount;
                                int copiesCount = LmsLiteratures.CopyCount - (int)LmsLiteratures.PreviousCopyCount;
                                LmsLiteratures.CopyCount = copiesCount;
                                var resultLiterature = await _DbContext.LmsLiteratures.AsNoTracking().Where(x => x.LiteratureId == LmsLiteratures.LiteratureId).FirstOrDefaultAsync();
                                if (resultLiterature != null)
                                {
                                    resultLiterature = LmsLiteratures;
                                    _DbContext.Entry(resultLiterature).State = EntityState.Modified;
                                    await _DbContext.SaveChangesAsync();
                                }
                                await UpdatePurchaseByLiterature(LmsLiteratures, _DbContext);

                                // Geting all Old Copy count
                                List<LmsLiteratureBarcode> TotalBarcodeList = LmsLiteratures.LiteratureBarcodes;
                                List<LmsLiteratureBarcode> OldBarcodes = new List<LmsLiteratureBarcode>();
                                List<LmsLiteratureBarcode> NewBarcodes = new List<LmsLiteratureBarcode>();
                                foreach (var oldBarcode in LmsLiteratures.PreviousLmsLiteratureBarcode)
                                {
                                    var res = LmsLiteratures.LiteratureBarcodes.Where(x => x.LiteratureId == oldBarcode.LiteratureId && x.BarcodeId == oldBarcode.BarcodeId).FirstOrDefault();
                                    if (res != null)
                                        OldBarcodes.Add(res);
                                }
                                LmsLiteratures.LiteratureBarcodes = new List<LmsLiteratureBarcode>();
                                LmsLiteratures.LiteratureBarcodes = OldBarcodes;
                                await UpdateBarCodeByLiterature(LmsLiteratures, _DbContext);
                                await UpdateAuthorByLiterature(LmsLiteratures, _DbContext);
                                await UpdateLiteratureTags(LmsLiteratures, _DbContext);
                                await UpdateLiteratureSeriesNumber(LmsLiteratures, _DbContext);
                                await _DbContext.SaveChangesAsync();

                                // adding new Barcodes
                                LmsLiteratures.LiteratureBarcodes = new List<LmsLiteratureBarcode>();
                                NewBarcodes.AddRange(TotalBarcodeList.Where(x => x.BarcodeId == 0));
                                LmsLiteratures.LiteratureBarcodes = NewBarcodes;
                                bool indexChanged = false;
                                var LiteratureId = await CreateNewLiterature(LmsLiteratures, LmsLiteratures.SeriesNumber, LmsLiteratures.CopyCount, _DbContext, indexChanged);
                                if (LiteratureId.Id != 0)
                                {
                                    LmsLiteratures.LiteratureIdList = LiteratureId.LiteratureIds;
                                    transaction.Commit();
                                    return LmsLiteratures;
                                }
                                else
                                {
                                    transaction.Rollback();
                                    return null;
                                }
                            }
                            else // in case of series number decreasing
                            {
                                int NumberOfDeletedRecord = (int)LmsLiteratures.PreviousSeriesNumber - LmsLiteratures.SeriesNumber;
                                int NewSeriesNumber = 0;
                                //var result = await _DbContext.LmsLiteratures.Where(x => x.LiteratureUniqueId == LmsLiteratures.LiteratureUniqueId && x.IsBorrowable == false)
                                //    .ToListAsync() ?? new List<LmsLiterature>();

                                var resultLiterature = await _DbContext.LmsLiteratures
                                     .Where(x => x.LiteratureUniqueId == LmsLiteratures.LiteratureUniqueId && x.IsBorrowable == false && x.IsDeleted == false)
                                    .Take(NumberOfDeletedRecord).ToListAsync() ?? new List<LmsLiterature>();

                                if (resultLiterature != null && resultLiterature.Count > 0)
                                {
                                    List<LmsLiterature> UpdateLiteratures = new();
                                    for (int i = 0; i < NumberOfDeletedRecord; i++)
                                    {
                                        if (resultLiterature != null)
                                        {
                                            LmsLiterature literature = resultLiterature.First();
                                            literature.IsDeleted = true;
                                            UpdateLiteratures.Add(literature);
                                            resultLiterature.Remove(literature);
                                            NewSeriesNumber = i + 1;
                                        }

                                    }

                                    _DbContext.LmsLiteratures.UpdateRange(UpdateLiteratures);
                                    await _DbContext.SaveChangesAsync();
                                    LmsLiteratures.SeriesNumber = (int)LmsLiteratures.PreviousSeriesNumber - NewSeriesNumber;
                                    await UpdateLiteratureSeriesNumber(LmsLiteratures, _DbContext);
                                    transaction.Commit();
                                    return LmsLiteratures;
                                }
                                else
                                {
                                    return null;
                                }

                            }

                        }
                        
                        // if series number, copy count and Index number will be changed
                        else if (LmsLiteratures.SeriesNumber != LmsLiteratures.PreviousSeriesNumber && LmsLiteratures.CopyCount != LmsLiteratures.PreviousCopyCount &&
                                LmsLiteratures.LmsLiteratureIndex.IndexNumber != LmsLiteratures.PreviousLmsLiteratureIndex.IndexNumber)
                        {
                            // int OldSeriesNumber = LmsLiteratures.SeriesNumber;
                            // LmsLiteratures.SeriesNumber = 1;
                            if (LmsLiteratures.SeriesNumber > LmsLiteratures.PreviousSeriesNumber)
                            {
                                string IndexNumber = string.Empty;
                                if (LmsLiteratures.LmsLiteratureIndex != null)
                                {
                                    IndexNumber = LmsLiteratures.LmsLiteratureIndex.IndexNumber;
                                }
                                (LmsLiteratures.DeweyBookNumber, LmsLiteratures.SeriesSequenceNumber, string digitalSequenceNumberOutput) = await GenerateUniqueDeweyBookNumber(LmsLiteratures.LmsLiteratureIndex.IndexNumber, 0);
                                int oldCopyCount = LmsLiteratures.CopyCount;
                                int copiesCount = LmsLiteratures.CopyCount - (int)LmsLiteratures.PreviousCopyCount;
                                LmsLiteratures.CopyCount = copiesCount;
                                //LmsLiteratures.LiteratureUniqueId = Guid.NewGuid();
                                var resultLiterature = await _DbContext.LmsLiteratures.AsNoTracking().Where(x => x.LiteratureId == LmsLiteratures.LiteratureId).FirstOrDefaultAsync();
                                if (resultLiterature != null)
                                {
                                    resultLiterature = LmsLiteratures;
                                    resultLiterature.Characters = GetSubString(LmsLiteratures.Name) + "." + GetSubString(LmsLiteratures.LmsLiteratureAuthors.Select(x => x.FullName_En).FirstOrDefault()) + "/" + IndexNumber.ToString() + "." + digitalSequenceNumberOutput;
                                    resultLiterature.IndexId = LmsLiteratures.IndexId;
                                    resultLiterature.Characters82 = (IndexNumber.ToString()) + "." + digitalSequenceNumberOutput;
                                    _DbContext.Entry(resultLiterature).State = EntityState.Modified;
                                    await _DbContext.SaveChangesAsync();
                                }
                                await UpdatePurchaseByLiterature(LmsLiteratures, _DbContext);

                                List<LmsLiteratureBarcode> TotalBarcodeList = LmsLiteratures.LiteratureBarcodes;
                                List<LmsLiteratureBarcode> OldBarcodes = new List<LmsLiteratureBarcode>();
                                List<LmsLiteratureBarcode> NewBarcodes = new List<LmsLiteratureBarcode>();
                                foreach (var oldBarcode in LmsLiteratures.PreviousLmsLiteratureBarcode)
                                {
                                    var res = LmsLiteratures.LiteratureBarcodes.Where(x => x.LiteratureId == oldBarcode.LiteratureId && x.BarcodeId == oldBarcode.BarcodeId).FirstOrDefault();
                                    if (res != null)
                                        OldBarcodes.Add(res);
                                }
                                LmsLiteratures.LiteratureBarcodes = new List<LmsLiteratureBarcode>();
                                LmsLiteratures.LiteratureBarcodes = OldBarcodes;
                                await UpdateBarCodeByLiterature(LmsLiteratures, _DbContext);
                                await UpdateAuthorByLiterature(LmsLiteratures, _DbContext);
                                await UpdateLiteratureTags(LmsLiteratures, _DbContext);
                                await UpdateLiteratureSeriesNumber(LmsLiteratures, _DbContext);
                                await _DbContext.SaveChangesAsync();

                                // adding new Barcodes
                                LmsLiteratures.LiteratureBarcodes = new List<LmsLiteratureBarcode>();
                                NewBarcodes.AddRange(TotalBarcodeList.Where(x => x.BarcodeId == 0));
                                LmsLiteratures.LiteratureBarcodes = NewBarcodes;
                                bool indexChanged = true;
                                int OldSeriesNumber = LmsLiteratures.SeriesNumber - (int)LmsLiteratures.PreviousSeriesNumber;
                                var LiteratureId = await CreateNewLiterature(LmsLiteratures, OldSeriesNumber, LmsLiteratures.CopyCount, _DbContext, indexChanged);
                                //var LiteratureId = await CreateNewLiterature(LmsLiteratures, OldSeriesNumber - 1, LmsLiteratures.CopyCount, _DbContext);
                                if (LiteratureId.Id != 0)
                                {
                                    LmsLiteratures.LiteratureIdList = LiteratureId.LiteratureIds;
                                    transaction.Commit();
                                    return LmsLiteratures;
                                }
                                else
                                {
                                    transaction.Rollback();
                                    return null;
                                }
                            }
                            else  // in case of series decreasing
                            {
                                int NumberOfDeletedRecord = (int)LmsLiteratures.PreviousSeriesNumber - LmsLiteratures.SeriesNumber;
                                int NewSeriesNumber = 0;
                                //var result = await _DbContext.LmsLiteratures.Where(x => x.LiteratureUniqueId == LmsLiteratures.LiteratureUniqueId && x.IsBorrowable == false)
                                //    .ToListAsync() ?? new List<LmsLiterature>();

                                var resultLiterature = await _DbContext.LmsLiteratures
                                     .Where(x => x.LiteratureUniqueId == LmsLiteratures.LiteratureUniqueId && x.IsBorrowable == false && x.IsDeleted == false)
                                    .Take(NumberOfDeletedRecord).ToListAsync() ?? new List<LmsLiterature>();

                                if (resultLiterature != null && resultLiterature.Count > 0)
                                {
                                    List<LmsLiterature> UpdateLiteratures = new();
                                    for (int i = 0; i < NumberOfDeletedRecord; i++)
                                    {
                                        if (resultLiterature != null)
                                        {
                                            LmsLiterature literature = resultLiterature.First();
                                            literature.IsDeleted = true;
                                            UpdateLiteratures.Add(literature);
                                            resultLiterature.Remove(literature);
                                            NewSeriesNumber = i + 1;
                                        }
                                    }
                                    _DbContext.LmsLiteratures.UpdateRange(UpdateLiteratures);
                                    await _DbContext.SaveChangesAsync();
                                    LmsLiteratures.SeriesNumber = (int)LmsLiteratures.PreviousSeriesNumber - NewSeriesNumber;
                                    await UpdateLiteratureSeriesNumber(LmsLiteratures, _DbContext);
                                    transaction.Commit();
                                    return LmsLiteratures;
                                }
                                else
                                {
                                    return null;
                                }
                            }
                        }

                        // if index number and series number will be changed
                        else if (LmsLiteratures.LmsLiteratureIndex.IndexNumber != LmsLiteratures.PreviousLmsLiteratureIndex.IndexNumber && LmsLiteratures.SeriesNumber != LmsLiteratures.PreviousSeriesNumber && 
                                LmsLiteratures.CopyCount == LmsLiteratures.PreviousCopyCount)
                        {
                            string IndexNumber = string.Empty;
                            if (LmsLiteratures.LmsLiteratureIndex != null)
                            {
                                IndexNumber = LmsLiteratures.LmsLiteratureIndex.IndexNumber;
                            }
                            (LmsLiteratures.DeweyBookNumber, LmsLiteratures.SeriesSequenceNumber, string digitalSequenceNumberOutput) = await GenerateUniqueDeweyBookNumber(LmsLiteratures.LmsLiteratureIndex.IndexNumber, 0);

                            int NumberOfDeletedRecord = (int)LmsLiteratures.PreviousSeriesNumber - LmsLiteratures.SeriesNumber;
                            int NewSeriesNumber = 0;

                            var resultLiterature = await _DbContext.LmsLiteratures
                                .Where(x => x.LiteratureUniqueId == LmsLiteratures.LiteratureUniqueId && x.IsBorrowable == false && x.IsDeleted == false)
                                .Take(NumberOfDeletedRecord).ToListAsync() ?? new List<LmsLiterature>();

                            if (resultLiterature != null && resultLiterature.Count > 0)
                            {
                                List<LmsLiterature> UpdateLiteratures = new();
                                for (int i = 0; i < NumberOfDeletedRecord; i++)
                                {
                                    if (resultLiterature != null)
                                    {
                                        LmsLiterature literature = resultLiterature.First();
                                        literature.DeletedBy = LmsLiteratures.DeletedBy != null ? LmsLiteratures.DeletedBy : LmsLiteratures.ModifiedBy;
                                        literature.DeletedDate = DateTime.Now;
                                        literature.IsDeleted = true;
                                        literature.DeweyBookNumber = LmsLiteratures.DeweyBookNumber;
                                        literature.SeriesSequenceNumber = LmsLiteratures.SeriesSequenceNumber;
                                        literature.Characters = GetSubString(LmsLiteratures.Name) + "." + GetSubString(LmsLiteratures.LmsLiteratureAuthors.Select(x => x.FullName_En).FirstOrDefault()) + "/" + IndexNumber.ToString() + "." + digitalSequenceNumberOutput;
                                        literature.IndexId = LmsLiteratures.IndexId;
                                        literature.Characters82 = (IndexNumber.ToString()) + "." + digitalSequenceNumberOutput;
                                        UpdateLiteratures.Add(literature);
                                        resultLiterature.Remove(literature);
                                        NewSeriesNumber = i + 1;
                                    }
                                }
                                _DbContext.LmsLiteratures.UpdateRange(UpdateLiteratures);
                                await _DbContext.SaveChangesAsync();
                                LmsLiteratures.SeriesNumber = (int)LmsLiteratures.PreviousSeriesNumber - NewSeriesNumber;
                                LmsLiteratures.DeweyBookNumber = LmsLiteratures.DeweyBookNumber;
                                LmsLiteratures.SeriesSequenceNumber = LmsLiteratures.SeriesSequenceNumber;
                                await UpdateLiteratureSeriesNumber(LmsLiteratures, _DbContext);
                            }
                            await UpdatePurchaseByLiterature(LmsLiteratures, _DbContext);
                            await UpdateBarCodeByLiterature(LmsLiteratures, _DbContext);
                            await UpdateAuthorByLiterature(LmsLiteratures, _DbContext);
                            await UpdateLiteratureTags(LmsLiteratures, _DbContext);
                            transaction.Commit();
                            return LmsLiteratures;
                        }

                        // if no index number, copy count and series number will be changed
                        var result = await _DbContext.LmsLiteratures.AsNoTracking().Where(x => x.LiteratureId == LmsLiteratures.LiteratureId).FirstOrDefaultAsync();
                        if (result != null)
                        {
                            result = LmsLiteratures;
                            _DbContext.Entry(result).State = EntityState.Modified;
                            await _DbContext.SaveChangesAsync();
                        }
                        await UpdatePurchaseByLiterature(LmsLiteratures, _DbContext);
                        await UpdateBarCodeByLiterature(LmsLiteratures, _DbContext);
                        await UpdateAuthorByLiterature(LmsLiteratures, _DbContext);
                        await UpdateLiteratureTags(LmsLiteratures, _DbContext);
                        transaction.Commit();
                        return LmsLiteratures;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return null;
                    }
                }
            }
        }

        private async Task UpdateAuthorByLiterature(LmsLiterature lmsLiterature, DatabaseContext dbContext)
        {
            try
            {
                if (lmsLiterature.LmsLiteratureAuthors.Count() > 0)
                {
                    await UpdateLiteratureAuthor(lmsLiterature, lmsLiterature.LiteratureId, lmsLiterature.LmsLiteratureAuthors, dbContext);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task UpdateBarCodeByLiterature(LmsLiterature lmsLiterature, DatabaseContext dbContext)
        {
            try
            {
                if (lmsLiterature.DeletedLiteratureBarcodes.Count() != 0)
                {
                    foreach (var item in lmsLiterature.DeletedLiteratureBarcodes)
                    {
                        if (item.BarcodeId != 0)
                        {
                            item.DeletedBy = lmsLiterature.DeletedBy != null ? lmsLiterature.DeletedBy : lmsLiterature.ModifiedBy;
                            item.DeletedDate = DateTime.Now;
                            item.IsDeleted = true;
                            dbContext.Entry(item).State = EntityState.Modified;
                        }
                    }
                    await dbContext.SaveChangesAsync();
                }
                if (lmsLiterature.LiteratureBarcodes.Count() != 0)
                {
                    foreach (var itemResult in lmsLiterature.LiteratureBarcodes)
                    {
                        if (itemResult.BarcodeId == 0)
                        {
                            LmsLiteratureBarcode pBarcodeObj = new LmsLiteratureBarcode();
                            pBarcodeObj.LiteratureId = lmsLiterature.LiteratureId;
                            pBarcodeObj.BarCodeNumber = itemResult.BarCodeNumber;
                            pBarcodeObj.CreatedBy = lmsLiterature.ModifiedBy != null ? lmsLiterature.ModifiedBy : lmsLiterature.CreatedBy;
                            pBarcodeObj.CreatedDate = DateTime.Now;
                            pBarcodeObj.IsDeleted = false;
                            pBarcodeObj.Active = itemResult.Active;
                            pBarcodeObj.IsBorrowed = false;
                            pBarcodeObj.RFIDValue = Convert.ToString(itemResult.RFID);
                            await dbContext.LmsLiteratureBarcodes.AddAsync(pBarcodeObj);
                        }
                        else
                        {
                            var result = await dbContext.LmsLiteratureBarcodes.Where(x => x.BarcodeId == itemResult.BarcodeId).FirstOrDefaultAsync();
                            if (result != null)
                            {
                                result.Active = itemResult.Active;
                                result.RFIDValue = itemResult.RFIDValue;
                                result.ModifiedBy = lmsLiterature.ModifiedBy != null ? lmsLiterature.ModifiedBy : lmsLiterature.CreatedBy;
                                result.ModifiedDate = DateTime.Now;
                                dbContext.Entry(result).State = EntityState.Modified;
                            }
                        }
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task UpdatePurchaseByLiterature(LmsLiterature lmsLiterature, DatabaseContext dbContext)
        {
            try
            {
                var purchase = await dbContext.LmsLiteraturePurchases.Where(x => x.LiteratureId == lmsLiterature.LiteratureId).ToListAsync();
                if (purchase.Count() > 0)
                {
                    foreach (var item in purchase)
                    {
                        item.Date = lmsLiterature.Purchase_Date;
                        item.Price = (decimal)lmsLiterature.Purchase_Price;
                        item.Location = lmsLiterature.Purchase_Location;
                        item.ModifiedBy = lmsLiterature.ModifiedBy;
                        item.ModifiedDate = lmsLiterature.ModifiedDate;
                        dbContext.Entry(item).State = EntityState.Modified;
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async Task UpdateLiteratureTags(LmsLiterature lmsLiterature, DatabaseContext dbContext)
        {
            try
            {
                dbContext.LiteratureDetailLiteratureTags.RemoveRange(await dbContext.LiteratureDetailLiteratureTags.Where(c => c.LiteratureId == lmsLiterature.LiteratureId).ToListAsync());
                await dbContext.SaveChangesAsync();
                if (lmsLiterature.LiteratureTags.Count() > 0)
                {
                    foreach (var tag in lmsLiterature.LiteratureTags)
                    {
                        LiteratureDetailLiteratureTag literatureTag = new LiteratureDetailLiteratureTag();
                        literatureTag.TagId = tag.TagId;
                        literatureTag.LiteratureId = lmsLiterature.LiteratureId;
                        literatureTag.Value = tag.Value;
                        await dbContext.LiteratureDetailLiteratureTags.AddAsync(literatureTag);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task UpdateLiteratureSeriesNumber(LmsLiterature lmsLiterature, DatabaseContext dbContext)
        {
            try
            {
                var result = await dbContext.LmsLiteratures.Where(x => x.LiteratureUniqueId == lmsLiterature.LiteratureUniqueId && x.LiteratureId != lmsLiterature.LiteratureId).ToListAsync();
                if (result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        item.SeriesNumber = lmsLiterature.SeriesNumber;
                        dbContext.LmsLiteratures.Update(item);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region delete
        public async Task DeleteLmsLiterature(int Id)
        {

            try
            {
                LmsLiterature? _LmsLiteratures = await _DbContext.LmsLiteratures.FindAsync(Id);
                if (_LmsLiteratures != null)
                {
                    _DbContext.LmsLiteratures.Remove(_LmsLiteratures);
                    await _DbContext.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                _DbContext.Entry(_LmsLiteratures).State = EntityState.Unchanged;
                throw;
            }

        }
        //<History Author = 'Hassan Abbas' Date='2022-04-05' Version="1.0" Branch="master">function for soft deleting a book</History>
        public async Task SoftDeleteLmsLiterature(LmsLiterature LmsLiterature)
        {
            using (_DbContext)
            {
                using (var transaction = _DbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _DbContext.Entry(LmsLiterature).State = EntityState.Modified;
                        await _DbContext.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public async Task SoftDeleteLiterature(List<LiteratureDetailVM> literatures)
        {
            using (_DbContext)
            {
                using (var transaction = _DbContext.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (LiteratureDetailVM item in literatures)
                        {
                            var literature = _DbContext.LmsLiteratures.Where(x => x.LiteratureId == item.LiteratureId).FirstOrDefault();
                            if (literature != null)
                            {
                                literature.DeletedBy = item.DeletedBy;
                                literature.DeletedDate = DateTime.Now;
                                literature.IsDeleted = true;

                                _DbContext.Entry(literature).State = EntityState.Modified;
                                await _DbContext.SaveChangesAsync();
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }
        #endregion

        #region Literature Tag

        public async Task<List<LiteratureTag>> GetAllActiveLiteratureTags()
        {
            try
            {
                return await _DbContext.LiteratureTags.Where(x => x.IsActive != false && x.IsDeleted != true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region get isactive & isborrowable barcode number detail's by using literature id
        public async Task<LiteratureDetailsForBorrowRequestVM> GetBarcodeNumberDetailByusingLiteratureId(int literatureId)
        {
            try
            {
                LiteratureDetailsForBorrowRequestVM ObjResult = new LiteratureDetailsForBorrowRequestVM();
                string StoredProc = $"exec pLiteratureDetailsForBorrowRequest @literatureId = '{literatureId}'";
                var resultBorrow = await _DbContext.LiteratureDetailsForBorrowRequestVMs.FromSqlRaw(StoredProc).ToListAsync();
                if (resultBorrow.Count() != 0)
                {
                    ObjResult = resultBorrow.FirstOrDefault();
                }
                return ObjResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Copy Create Function

        public async Task<(int Id, List<int> LiteratureIds)> CreateNewLiterature(LmsLiterature lmsLiterature1, int newSeriesNumber, int copyCount, DatabaseContext dbContext, bool indexChanged)
        {
            try
            {
                int seriesNumber = lmsLiterature1.SeriesNumber - (int)lmsLiterature1.PreviousSeriesNumber;
                List<int> LiteratureIds = new List<int>();
                int iterationNumber = 1;
                for (int i = 0; i < seriesNumber; i++)
                {
                    LmsLiterature lmsLiterature = new MapperConfiguration(cfg => cfg.CreateMap<LmsLiterature, LmsLiterature>()).CreateMapper().Map<LmsLiterature>(lmsLiterature1);
                    lmsLiterature.ModifiedDate = null;
                    lmsLiterature.LiteratureId = 0;
                    lmsLiterature.SeriesNumber = newSeriesNumber;
                    lmsLiterature.CopyCount = copyCount;

                    lmsLiterature.CopyCount = (int)await DivideNumberOfCopyBySeriesNumber(lmsLiterature.SeriesNumber, lmsLiterature.CopyCount);
                    string IndexNumber = string.Empty;

                    if (lmsLiterature.LmsLiteratureIndex != null)
                    {
                        IndexNumber = lmsLiterature.LmsLiteratureIndex.IndexNumber;
                    }

                    if (indexChanged == true)
                    {
                        (lmsLiterature.DeweyBookNumber, lmsLiterature.SeriesSequenceNumber, string digitalSequenceNumberOutput) = await GenerateUniqueDeweyBookNumber(IndexNumber, i);
                        lmsLiterature.Characters = GetSubString(lmsLiterature.Name) + "." + GetSubString(lmsLiterature.LmsLiteratureAuthors.Select(x => x.FullName_En).FirstOrDefault()) + "/" + IndexNumber.ToString() + "." + digitalSequenceNumberOutput;
                        lmsLiterature.Characters82 = (IndexNumber.ToString()) + "." + digitalSequenceNumberOutput;
                    }
                    else
                        (lmsLiterature.DeweyBookNumber, lmsLiterature.SeriesSequenceNumber) = await IncreaseDeweySeriesNumber(IndexNumber, i, lmsLiterature.LiteratureUniqueId);

                    lmsLiterature.LmsLiteratureIndex = null;
                    await dbContext.LmsLiteratures.AddAsync(lmsLiterature);
                    await dbContext.SaveChangesAsync();
                    var LiteratureId = lmsLiterature.LiteratureId;
                    int AuthorId = 0;
                    if (LiteratureId != 0)
                    {
                        await InsertNewPurchaseByLiterature(lmsLiterature);
                        if (lmsLiterature.LmsLiteratureAuthors.Count() > 0)
                        {
                            await InsertLiteratureAuthor(lmsLiterature, lmsLiterature.LiteratureId, lmsLiterature.LmsLiteratureAuthors, dbContext);
                        }
                        if (iterationNumber == 1)
                        {
                            List<LmsLiteratureBarcode> lmsLiteratureBarcodes = lmsLiterature.LiteratureBarcodes;
                            lmsLiterature.LiteratureBarcodes = lmsLiterature.LiteratureBarcodes.GetRange(0, lmsLiterature.CopyCount);
                            await InsertBarcodeByLiterature(lmsLiterature, dbContext);
                            lmsLiterature.LiteratureBarcodes = lmsLiteratureBarcodes;
                            lmsLiterature.LiteratureBarcodes.RemoveRange(0, lmsLiterature.CopyCount);
                            iterationNumber++;
                        }
                        else
                        {
                            await InsertBarcodeByLiterature(lmsLiterature, dbContext);
                            if (lmsLiterature.LiteratureBarcodes != null && lmsLiterature.LiteratureBarcodes.Count > 0)
                            {
                                lmsLiterature.LiteratureBarcodes.RemoveRange(0, lmsLiterature.CopyCount);
                                iterationNumber++;
                            }
                        }
                        await InsertLiteratureTags(lmsLiterature, dbContext);
                    }
                    var literature = await dbContext.LmsLiteratures.FindAsync(lmsLiterature.LiteratureId);
                    if (literature != null)
                    {
                        lmsLiterature.NotificationParameter.Name = literature.Name;
                    }

                    // adding literature id to list for attachment use
                    //lmsLiterature.LiteratureIdList.Add(lmsLiterature.LiteratureId);
                    LiteratureIds.Add(lmsLiterature.LiteratureId);
                }
                //return lmsLiterature.LiteratureId;
                // returnObject.LiteratureId = lmsLiterature.LiteratureId;
                return (lmsLiterature1.LiteratureId, LiteratureIds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get User Id By Email
        public async Task<string> GetUserIdByEmail(string createdBy)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            return _DbContext.Users.Where(x => x.Email == createdBy).Select(y => y.Id).FirstOrDefault();
        }
        #endregion

        #region Get Lms Admin User
        public async Task<List<UserRole>> GetLmsAdminUser()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            List<UserRole> users = await _DbContext.UserRoles.Where(x => x.RoleId == SystemRoles.LLSAdmin).ToListAsync();
            return users;
        }
        #endregion

        #region Get Fatwa Amdin user
        public async Task<List<UserRole>> GetFatwaAdminUser()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            List<UserRole> users = await _DbContext.UserRoles.Where(x => x.RoleId == SystemRoles.FatwaAdmin).ToListAsync();
            return users;
        }
        #endregion

        #region Insert New Purchase By Literature
        // this function is only use in CreateNewLiterature Function 
        private async Task InsertNewPurchaseByLiterature(LmsLiterature lmsLiterature2)
        {
            try
            {
                LmsLiterature lmsLiterature = new MapperConfiguration(cfg => cfg.CreateMap<LmsLiterature, LmsLiterature>()).CreateMapper().Map<LmsLiterature>(lmsLiterature2);
                using var scope = _serviceScopeFactory.CreateScope();
                var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                LmsLiteraturePurchase pObj = new LmsLiteraturePurchase();
                pObj.LiteratureId = lmsLiterature.LiteratureId;
                pObj.CreatedDate = DateTime.Now;
                pObj.CreatedBy = lmsLiterature.ModifiedBy != null ? lmsLiterature.ModifiedBy : lmsLiterature.CreatedBy;
                pObj.IsDeleted = false;
                pObj.Price = (decimal)lmsLiterature.Purchase_Price;
                pObj.Location = lmsLiterature.Purchase_Location;
                pObj.Date = lmsLiterature.Purchase_Date;
                await _dbContext.LmsLiteraturePurchases.AddAsync(pObj);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Check RFID value Exists
        public async Task<bool> CheckRFIDValueExists(string barCodeNumber, string rfIdValue)
        {
            try
            {
                var result = await _DbContext.LmsLiteratureBarcodes.Where(x => x.RFIDValue == rfIdValue).FirstOrDefaultAsync();
                if (result != null)
                {
                    var barresult = await _DbContext.LmsLiteratureBarcodes.Where(x => x.RFIDValue == rfIdValue && x.BarCodeNumber == barCodeNumber).FirstOrDefaultAsync();
                    if (barresult != null)
                    {
                        return false;
                    }
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Stock Taking List
        public async Task<List<LmsStockTakingListVM>> GetStockTakingList(StockTakingAdvancedSearchVM advancedSearchVM)
        {
            try
            {
                string fromDate = advancedSearchVM.FromDate != null ? Convert.ToDateTime(advancedSearchVM.FromDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string toDate = advancedSearchVM.ToDate != null ? Convert.ToDateTime(advancedSearchVM.ToDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string StockTakingFromDate = advancedSearchVM.StockTakingFromDate != null ? Convert.ToDateTime(advancedSearchVM.StockTakingFromDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string StockTakingToDate = advancedSearchVM.StockTakingToDate != null ? Convert.ToDateTime(advancedSearchVM.StockTakingToDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string StoredProc = $"exec PGetStockTakingList  @StatusId='{advancedSearchVM.StatusId}', @From='{fromDate}', @To='{toDate}', @StockTakingDateFrom='{StockTakingFromDate}', @StockTakingDateTo='{StockTakingToDate}',@PageNumber ='{advancedSearchVM.PageNumber}',@PageSize ='{advancedSearchVM.PageSize}'";
                var result = await _DbContext.StockTakingVM.FromSqlRaw(StoredProc).ToListAsync();
                return result;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Stock Taking Details By Id
        public async Task<LmsStockTakingDetailVM> GetStockTakingDetailById(Guid Id)
        {
            try
            {
                string StoredProc = $"exec pGetStockTakingDetails @StockTakingId='{Id}'";
                var result = await _DbContext.LmsStockTakingDetailVM.FromSqlRaw(StoredProc).ToListAsync();
                return result.FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Stock Taking Statuses
        public async Task<List<LmsStockTakingStatus>> GetStockTakingStatus()
        {
            try
            {
                var result = await _DbContext.LmsStockTakingStatuses.OrderBy(x => x.Id).ToListAsync();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new List<LmsStockTakingStatus>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Total Number of Books
        public async Task<int> GetTotalNoOfBooks()
        {
            try
            {
                var result = await _DbContext.LmsLiteratureBarcodes.OrderBy(x => x.BarcodeId).ToListAsync();
                if (result != null)
                {
                    return result.Count;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Literature Books Report List Group By Barcode and StockTaking Id
        public async Task<List<LmsStockTakingBooksReportListVm>> GetLmsBookStockTakingReportList(Guid? StockTakingId)
        {
            try
            {
                string StoredProc = "";
                if (StockTakingId != null)
                {
                    StoredProc = $"exec pGetLmsLiteratureListOrderByBarcodeAndStockTakingId @StockTakingId = '{StockTakingId}'";
                }
                else
                {
                    StoredProc = $"exec pGetLmsLiteratureListOrderByBarcodeAndStockTakingId";
                }
                var result = await _DbContext.StockTakingBooksReportListVms.FromSqlRaw(StoredProc).ToListAsync();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new List<LmsStockTakingBooksReportListVm>();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Save Stock Taking Report 
        public async Task<bool> SubmitStockTakingReport(SaveStockTakingVm saveStockTakingVm)
        {
            using (_DbContext)
            {
                using (var transaction = _DbContext.Database.BeginTransaction())
                {
                    bool isSaved = false;
                    try
                    {
                        if (saveStockTakingVm.IsEdit == false)
                        {
                            var reportNumber = await GetAutoGeneratedReportNumber();
                            saveStockTakingVm.stockTaking.ReportNumber = reportNumber.ReportNumber;
                            saveStockTakingVm.stockTaking.ShortNumber = reportNumber.ShortNumber;
                            _DbContext.LmsStockTakings.Add(saveStockTakingVm.stockTaking);
                        }
                        else
                        {
                            _DbContext.LmsStockTakings.Update(saveStockTakingVm.stockTaking);
                        }
                        await _DbContext.SaveChangesAsync();
                        isSaved = await SaveStockTakingReportList(saveStockTakingVm, _DbContext);
                        isSaved = await SaveStockTakingPerformers(saveStockTakingVm, _DbContext);
                        isSaved = await SaveStockTakingReportReamrks(saveStockTakingVm, _DbContext);
                        await SaveLmsStockTakingHistory(saveStockTakingVm.stockTaking.Id, (int)LmsStockTakingEventEnum.InProgress, saveStockTakingVm.IsEdit == false ? saveStockTakingVm.stockTaking.CreatedBy : saveStockTakingVm.stockTaking.ModifiedBy);
                        if (isSaved)
                        {
                            transaction.Commit();
                        }

                    }
                    catch (Exception ex)
                    {
                        isSaved = false;
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                    return isSaved;
                }
            }
        }
        #endregion

        #region Save Stock Taking Report List
        protected async Task<bool> SaveStockTakingReportList(SaveStockTakingVm args, DatabaseContext dbContext)
        {
            bool isSaved;
            try
            {
                LmsStockTakingReport obj = new LmsStockTakingReport();
                if (args.IsEdit == true)
                {
                    var stockTaking = await dbContext.LmsStockTakingReports.Where(x => x.StockTakingId == args.stockTaking.Id).ToListAsync();
                    if (stockTaking != null)
                    {
                        dbContext.LmsStockTakingReports.RemoveRange(stockTaking);
                        await dbContext.SaveChangesAsync();
                    }
                }
                foreach (var items in args.lmsStockTakingBooksReportListVms)
                {
                    obj.Id = Guid.NewGuid();
                    obj.StockTakingId = args.stockTaking.Id;
                    obj.BarcodeId = items.BarcodeId;
                    obj.IsBorrowed = items.CopiesBorrowed == 1 ? true : items.CopiesNotBorrowed == 1 ? false : false;
                    obj.Excess = items.Excess;
                    obj.Shortage = items.Shortage;
                    await _DbContext.LmsStockTakingReports.AddAsync(obj);
                    await _DbContext.SaveChangesAsync();
                }
                isSaved = true;
            }
            catch (Exception ex)
            {
                isSaved = false;
                throw;
            }
            return isSaved;

        }
        #endregion

        #region Save Stock Taking Performer
        protected async Task<bool> SaveStockTakingPerformers(SaveStockTakingVm args, DatabaseContext dbContext)
        {
            bool isSaved;
            try
            {
                LmsStockTakingPerformer obj = new LmsStockTakingPerformer();
                if (args.IsEdit == true)
                {
                    var stockTaking = await dbContext.LmsStockTakingPerformers.Where(x => x.StockTakingId == args.stockTaking.Id).ToListAsync();
                    if (stockTaking != null)
                    {
                        dbContext.LmsStockTakingPerformers.RemoveRange(stockTaking);
                        await dbContext.SaveChangesAsync();
                    }
                }
                foreach (var items in args.StockTakingPerformerIds)
                {
                    obj.Id = Guid.NewGuid();
                    obj.StockTakingId = args.stockTaking.Id;
                    obj.UserId = items;
                    obj.CreatedBy = args.stockTaking.CreatedBy;
                    obj.CreatedDate = DateTime.Now;
                    await _DbContext.LmsStockTakingPerformers.AddAsync(obj);
                    await _DbContext.SaveChangesAsync();
                }
                isSaved = true;
            }
            catch (Exception ex)
            {
                isSaved = false;
                throw;
            }
            return isSaved;

        }
        #endregion

        #region Save Stock Taking Report Reamrks 
        protected async Task<bool> SaveStockTakingReportReamrks(SaveStockTakingVm args, DatabaseContext dbContext)
        {
            bool isSaved;
            try
            {
                LmsBarcodeStockTakingRemarks obj = new LmsBarcodeStockTakingRemarks();
                if (args.IsEdit == true)
                {
                    var stockTakingRemarks = await dbContext.LmsBarcodeStockTakingRemarks.Where(x => x.StockTakingId == args.stockTaking.Id).ToListAsync();
                    if (stockTakingRemarks != null)
                    {
                        dbContext.LmsBarcodeStockTakingRemarks.RemoveRange(stockTakingRemarks);
                        await dbContext.SaveChangesAsync();
                    }
                }
                foreach (var items in args.lmsStockTakingBooksReportListVms)
                {
                    obj.Id = Guid.NewGuid();
                    obj.StockTakingId = args.stockTaking.Id;
                    obj.BarcodeId = items.BarcodeId;
                    obj.Remarks = items.Remarks;
                    obj.CreatedBy = args.stockTaking.CreatedBy;
                    obj.CreatedDate = args.stockTaking.CreatedDate;
                    await dbContext.LmsBarcodeStockTakingRemarks.AddAsync(obj);
                    await dbContext.SaveChangesAsync();
                }
                isSaved = true;
            }
            catch (Exception ex)
            {
                isSaved = false;
                throw;
            }
            return isSaved;

        }

        #endregion

        #region Approve StockTaking Report
        public async Task<bool> ApproveStockTakingReport(Guid StockTakingId, string ApprovedBy)
        {
            try
            {
                var result = await _DbContext.LmsStockTakings.Where(x => x.Id == StockTakingId).FirstOrDefaultAsync();
                if (result != null)
                {
                    result.StatusId = (int)StockTakingStatusEnum.Completed;
                    _DbContext.LmsStockTakings.Update(result);
                    _DbContext.SaveChanges();
                    await SaveLmsStockTakingHistory(result.Id, (int)LmsStockTakingEventEnum.Completed, ApprovedBy);

                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Get Auto Generated Report Number
        public async Task<LmsStockTaking> GetAutoGeneratedReportNumber()
        {
            try
            {
                LmsStockTaking lmsStockTaking = new LmsStockTaking();
                if (_DbContext.LmsStockTakings.Any())
                {
                    var shortNumber = await _DbContext.LmsStockTakings.Select(x => x.ShortNumber).MaxAsync() + 1;
                    lmsStockTaking.ReportNumber = "R-" + (shortNumber).ToString().PadLeft(6, '0');
                    lmsStockTaking.ShortNumber = shortNumber;
                }
                else
                {
                    lmsStockTaking.ReportNumber = "R-000001";
                    lmsStockTaking.ShortNumber = 1;
                }
                return lmsStockTaking;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion

        #region Get Lms StockTaking By Id
        public async Task<LmsStockTaking> GetLmsStockTakingById(Guid StockTakingId)
        {
            try
            {

                var result = await _DbContext.LmsStockTakings.Where(x => x.Id == StockTakingId).FirstOrDefaultAsync();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new LmsStockTaking();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Performers By StockTaking Id
        public async Task<List<StockTakingPerformerVm>> GetPerformersByStockTakingId(Guid StockTakingId)
        {
            try
            {
                string storProc = $"exec pGetPerformersByStockTakingId @StockTakingId='{StockTakingId}'";
                var result = await _DbContext.StockTakingPerformerVms.FromSqlRaw(storProc).ToListAsync();
                if (result != null && result.Count > 0)
                {
                    return result;
                }
                else
                {
                    return new List<StockTakingPerformerVm>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Check If Any InProgress StockTaking
        public async Task<bool> CheckIfAnyInProgressStockTaking()
        {
            try
            {

                var result = await _DbContext.LmsStockTakings.Where(x => x.StatusId == (int)StockTakingStatusEnum.InProgress && x.IsDeleted == false).FirstOrDefaultAsync();
                if (result != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region View Able Literature
        public async Task<List<LmsViewableLiteratureVM>> GetLmsViewableLiteratures(LiteratureAdvancedSearchVM advancedSearch)
        {
            try
            {
                string purchaseDateKeyword = advancedSearch.PurchaseDateKeyword != null ? Convert.ToDateTime(advancedSearch.PurchaseDateKeyword).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string fromDate = advancedSearch.FromDate != null ? Convert.ToDateTime(advancedSearch.FromDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string toDate = advancedSearch.ToDate != null ? Convert.ToDateTime(advancedSearch.ToDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string StoredProc = "";
                // Following Sp will get all the literature which are viewableand only pick those book copies which has attachment type of book digital copy
                switch (advancedSearch.EnumSearchValue)
                {
                    case LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum.Book_Name:
                        {
                            StoredProc = $"exec pLiteratureViewableListFiltered @bookName = N'{advancedSearch.KeywordsType}', @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}" +
                                         $",@PageNumber ='{advancedSearch.PageNumber}',@PageSize ='{advancedSearch.PageSize}'";
                            break;
                        }
                    case LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum.Book_Index:
                        {
                            StoredProc = $"exec pLiteratureViewableListFiltered @indexId = '{advancedSearch.GenericsIntergerKeyword}', @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}" +
                                         $",@PageNumber ='{advancedSearch.PageNumber}',@PageSize ='{advancedSearch.PageSize}'";
                            break;
                        }

                    case LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum.Barcode:
                        {
                            StoredProc = $"exec pLiteratureViewableListFiltered @barcode = '{advancedSearch.KeywordsType}', @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}" +
                                         $",@PageNumber ='{advancedSearch.PageNumber}',@PageSize ='{advancedSearch.PageSize}'";
                            break;
                        }

                    case LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum.Sticker:
                        {
                            StoredProc = $"exec pLiteratureViewableListFiltered @character82 = '{advancedSearch.KeywordsType}', @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}" +
                                         $",@PageNumber ='{advancedSearch.PageNumber}',@PageSize ='{advancedSearch.PageSize}'";
                            break;
                        }

                    case LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum.Author_Name:
                        {

                            StoredProc = $"exec pLiteratureViewableListFiltered @authorId = '{advancedSearch.GenericsIntergerKeyword}', @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}" +
                                         $",@PageNumber ='{advancedSearch.PageNumber}',@PageSize ='{advancedSearch.PageSize}'";
                            break;
                        }

                    default:
                        if (fromDate == null && toDate == null && advancedSearch.ClassificationId != 0)
                        {
                            StoredProc = $"exec pLiteratureViewableListFiltered @classificationId = {advancedSearch.ClassificationId} ,@PageNumber ='{advancedSearch.PageNumber}',@PageSize ='{advancedSearch.PageSize}";
                            break;
                        }
                        else if (fromDate != null && toDate != null)
                        {
                            StoredProc = $"exec pLiteratureViewableListFiltered @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId} , ,@PageNumber ='{advancedSearch.PageNumber}',@PageSize ='{advancedSearch.PageSize}";
                            break;
                        }
                        StoredProc = $"exec pLiteratureViewableListFiltered @PageNumber ='{advancedSearch.PageNumber}',@PageSize ='{advancedSearch.PageSize}'";
                        break;
                }
                if (_ViewableLiteratureVM == null)
                {
                    _ViewableLiteratureVM = await _DbContext.ViewableLiteratureVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _ViewableLiteratureVM;
        }

        #endregion

        #region Delete Lms StockTaking
        public async Task DeleteLmsStockTaking(LmsStockTakingListVM item)
        {
            using (_DbContext)
            {
                using (var transaction = _DbContext.Database.BeginTransaction())
                {

                    try
                    {
                        var existingStockTaking = await _DbContext.LmsStockTakings.FindAsync(item.Id);
                        if (existingStockTaking != null)
                        {
                            existingStockTaking.DeletedBy = item.DeletedBy;
                            existingStockTaking.DeletedDate = item.DeletedDate;
                            existingStockTaking.IsDeleted = item.IsDeleted;
                            _DbContext.Entry(existingStockTaking).State = EntityState.Modified;
                            await _DbContext.SaveChangesAsync();
                            await SaveLmsStockTakingHistory(item.Id, (int)LmsStockTakingEventEnum.Deleted, item.DeletedBy);

                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }


        }
        #endregion

        #region Save Lms StockTaking History
        public async Task SaveLmsStockTakingHistory(Guid StockTakingId, int EventId, string CreatedBy)
        {
            try
            {
                LmsStockTakingHistory obj = new LmsStockTakingHistory();
                obj.Id = Guid.NewGuid();
                obj.StockTakingId = StockTakingId;
                obj.EventId = EventId;
                obj.CreatedBy = CreatedBy;
                obj.CreatedDate = DateTime.Now;
                await _DbContext.LmsStockTakingHistories.AddAsync(obj);
                await _DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Lms StockTaking History By Id
        public async Task<List<LmsStockTakingHistoryVm>> GetLmsStockTakingHistoryById(Guid StockTakingId)
        {
            try
            {
                string StorProc = $"exec pGetLmsStockTakingHistoryById @StockTakingId = '{StockTakingId}'";
                var result = await _DbContext.GetLmsStockTakingHistoryVms.FromSqlRaw(StorProc).ToListAsync();
                if(result != null)
                {
                    return result;
                }
                else
                {
                    return new List<LmsStockTakingHistoryVm>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
