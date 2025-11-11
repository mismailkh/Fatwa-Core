using FATWA_DOMAIN.Models.ViewModel;
using FATWA_WEB.Services;
using FATWA_WEB.Services.Lms;
using Microsoft.AspNetCore.Mvc;

namespace FATWA_WEB
{
    public partial class ExportFatwaDbController : ExportController
    {
        //private readonly FatwaDbContext context;
        private readonly LmsLiteratureService _service;
        private readonly LmsLiteratureTypeService _literaturetypeservice;
        private readonly LmsLiteratureClassificationService _literatureclasificationservice;
        private readonly ProcessLogService _processlogservice;
        private readonly ErrorLogService _errorlogservice;
        private readonly LmsLiteratureBorrowDetailService _literatureborrowservice;
        private readonly LmsLiteratureIndexDivisionService _literatureindexdivisionservice;
        private readonly LmsLiteratureIndexService _literatureindexservice;
        private readonly LmsLiteratureParentIndexService _literatureparentindexservice;

        public ExportFatwaDbController(
            LmsLiteratureService service,
            LmsLiteratureTypeService lmsservice,
            LmsLiteratureClassificationService lmsclasification,
            ProcessLogService processlogservice,
            ErrorLogService errorlogservice,
            LmsLiteratureBorrowDetailService literatureborrowservice,
            LmsLiteratureIndexDivisionService literatureindexdivisionservice,
            LmsLiteratureIndexService literatureindexservice
            )
        {
            this._service = service;
            this._literaturetypeservice = lmsservice;
            this._literatureclasificationservice = lmsclasification;
            this._processlogservice = processlogservice;
            this._errorlogservice = errorlogservice;    
            this._literatureborrowservice = literatureborrowservice;
            this._literatureindexdivisionservice = literatureindexdivisionservice;
            this._literatureindexservice = literatureindexservice;
        }
        #region Literature Types

        [HttpGet("/export/FatwaDb/lmsliteraturetypes/csv")]
        [HttpGet("/export/FatwaDb/lmsliteraturetypes/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLmsLiteratureTypesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await _literaturetypeservice.GetLmsLiteratureTypes(), Request.Query), fileName);
        }

        [HttpGet("/export/FatwaDb/lmsliteraturetypes/excel")]
        [HttpGet("/export/FatwaDb/lmsliteraturetypes/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLmsLiteratureTypesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await _literaturetypeservice.GetLmsLiteratureTypes(), Request.Query), fileName);
        }

        #endregion
        #region Literature Borrow Extension Approval

        [HttpGet("/export/FatwaDb/lmsliteratureborrowapprovals/csv")]
        [HttpGet("/export/FatwaDb/lmsliteratureborrowapprovals/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLmsLiteratureBorrowApprovalsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await _literatureborrowservice.GetLiteratureBorrowExtensionApprovals(), Request.Query), fileName);
        }

        [HttpGet("/export/FatwaDb/lmsliteratureborrowapprovals/excel")]
        [HttpGet("/export/FatwaDb/lmsliteratureborrowapprovals/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLmsLiteratureBorrowApprovalsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await _literatureborrowservice.GetLiteratureBorrowExtensionApprovals(), Request.Query), fileName);
        }

        #endregion

        #region Literature Borrow Detail

        [HttpGet("/export/FatwaDb/lmsliteratureborrowdetails/csv")]
        [HttpGet("/export/FatwaDb/lmsLiteratureborrowdetails/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLmsLiteratureBorrowDetailsToCSV(string fileName = null)
        {
            var response = await _literatureborrowservice.GetLmsLiteratureBorrowDetails();
            if (response.IsSuccessStatusCode)
            {
                var result = (IEnumerable<BorrowDetailVM>)response.ResultData;
                return ToCSV(ApplyQuery(result.AsQueryable(), Request.Query), fileName);
            }
            return null;
        }

        [HttpGet("/export/FatwaDb/lmsliteratureborrowdetails/excel")]
        [HttpGet("/export/FatwaDb/lmsliteratureborrowdetails/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLmsLiteratureBorrowDetailsToExcel(string fileName = null)
        {
            var response = await _literatureborrowservice.GetLmsLiteratureBorrowDetails();
            if (response.IsSuccessStatusCode)
            {
                var result = (IEnumerable<BorrowDetailVM>)response.ResultData;
                return ToExcel(ApplyQuery(result.AsQueryable(), Request.Query), fileName);
            }
            return null;
        }

        #endregion

        #region Literature Index Division

        //[HttpGet("/export/FatwaDb/lmsliteratureindexdivisions/csv")]
        //[HttpGet("/export/FatwaDb/lmsLiteratureindexdivisions/csv(fileName='{fileName}'&indexId='{indexId}')")]
        //public async Task<FileStreamResult> ExportLmsLiteratureIndexDivisionsToCSV(int indexId, string fileName = null)
        //{
        //    return ToCSV(ApplyQuery(await _literatureindexdivisionservice.GetLmsLiteratureIndexDivisions(indexId), Request.Query), fileName);
        //}

        //[HttpGet("/export/FatwaDb/lmsliteratureindexdivisions/excel")]
        //[HttpGet("/export/FatwaDb/lmsliteratureindexdivisions/excel(fileName='{fileName}')")]
        //public async Task<FileStreamResult> ExportLmsLiteratureIndexDivisionsToExcel(int indexId, string fileName = null)
        //{
        //    return ToExcel(ApplyQuery(await _literatureindexdivisionservice.GetLmsLiteratureIndexDivisions(indexId), Request.Query), fileName);
        //}

        #endregion

        #region Literature Index

        [HttpGet("/export/FatwaDb/lmsliteratureindexs/csv")]
        [HttpGet("/export/FatwaDb/lmsLiteratureindexs/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLmsLiteratureIndexsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await _literatureindexservice.GetLmsLiteratureIndexs(), Request.Query), fileName);
        }

        [HttpGet("/export/FatwaDb/lmsliteratureindexs/excel")]
        [HttpGet("/export/FatwaDb/lmsliteratureindexs/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLmsLiteratureIndexsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await _literatureindexservice.GetLmsLiteratureIndexs(), Request.Query), fileName);
        }

        #endregion

        #region Literature Parent Index

        [HttpGet("/export/FatwaDb/lmsliteratureparentindexs/csv")]
        [HttpGet("/export/FatwaDb/lmsLiteratureparentindexs/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLmsLiteratureParentIndexsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await _literatureparentindexservice.GetLmsLiteratureParentIndexs(), Request.Query), fileName);
        }

        [HttpGet("/export/FatwaDb/lmsliteratureparentindexs/excel")]
        [HttpGet("/export/FatwaDb/lmsliteratureparentindexs/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLmsLiteratureParentIndexsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await _literatureparentindexservice.GetLmsLiteratureParentIndexs(), Request.Query), fileName);
        }

        #endregion

        #region Error Logs

        [HttpGet("/export/FatwaDb/errorlogs/csv")]
        [HttpGet("/export/FatwaDb/errorlogs/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportErrorlogsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await _errorlogservice.GetErrorLog(), Request.Query), fileName);
        }

        [HttpGet("/export/FatwaDb/errorlogs/excel")]
        [HttpGet("/export/FatwaDb/errorlogs/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportErrorlogsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await _errorlogservice.GetErrorLog(), Request.Query), fileName);
        }

        #endregion

        #region Process Logs

        [HttpGet("/export/FatwaDb/processlogs/csv")]
        [HttpGet("/export/FatwaDb/processlogs/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportProcesslogsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await _processlogservice.GetProcessLog(), Request.Query), fileName);
        }

        [HttpGet("/export/FatwaDb/processlogs/excel")]
        [HttpGet("/export/FatwaDb/processlogs/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportProcesslogsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await _processlogservice.GetProcessLog(), Request.Query), fileName);
        }

        #endregion
    }
}
